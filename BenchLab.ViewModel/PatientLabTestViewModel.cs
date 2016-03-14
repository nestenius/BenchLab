using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.DataAccess;
using BenchLab.ErrorLog;
using BenchLab.Model;
using BenchLab.Resources;
using BenchLab.SimpleUI.Entities;
using System.Windows.Input;
using BenchLab.Bases;

namespace BenchLab.ViewModel
{
    public class PatientLabTestViewModel : BaseViewModel<PatientTestReportCollection>
    {
        #region Fields
        private Patient _selectedPatient;
        #endregion

        #region Properties
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                this.RaisePropertyChanged("SelectedPatient");
            }
        }

        #region Command Properties
        private ICommand _patientLabTestDoubleClick;
        public ICommand PatientLabTestDoubleClick
        {
            get { return _patientLabTestDoubleClick; }
            set
            {
                _patientLabTestDoubleClick = value;
                this.RaisePropertyChanged(() => PatientLabTestDoubleClick);
            }
        }
        #endregion
        #region Window Properties
        public override string Title
        {
            get
            {
                return Resources.TitleResources.PatientLabReports;
            }
        }

        public override double DialogStartupSizePercentage
        {
            get
            {
                return 95;
            }
        }

        public override double DialogStartupCustomHeight
        {
            get
            {
                return 600;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 650;
            }
        }

        public override DialogType DialogType
        {
            get
            {
                return DialogType.ByPercentage;
            }
        }
        #endregion
        #endregion

        #region Constructors

        public PatientLabTestViewModel(IMessenger messenger, UserLogin userLogin, Patient patient)
            : base(messenger, userLogin)
        {
            SelectedPatient = patient;
            GetPatientLabTestReports();
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
            PatientLabTestDoubleClick = new RelayCommand(OnPatientLabTestDoubleClick, CanPatientLabTestDoubleClick);
        }
        #endregion

        #region Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AddPatientTestReportViewModel(this.Messenger, this.UserLogin,this.SelectedPatient) { ParentViewModel = this };
                childVM.RefreshPatientTestReport += this.GetRefreshPatientLabTestCollection;
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        public override bool CanDeleteItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }

        public override void OnDeleteItem()
        {
            this.ParentViewModel.ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeletePatientLabTest) { Caption = MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }

        private void DeletePatientLabTest(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {

                Task.Factory.StartNew(() =>
                {
                    PatientAction.DeletePatientTestReports(this.DBConnectionString,
                                                    this.Entity.InternalList.Where(x => x.IsSelected));
                    GetRefreshPatientLabTestCollection();
                    ShowProgressBar = false;
                });
            }
            else
            {
                ShowProgressBar = false;
            }
        }
        public override void OnEditItem()
        {
            var childVM = new AddPatientTestReportViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected), this.SelectedPatient) { ParentViewModel = this };
            childVM.RefreshPatientTestReport += this.GetRefreshPatientLabTestCollection;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        public override void OnRefreshItem()
        {
            this.GetRefreshPatientLabTestCollection();
        }

        private bool CanPatientLabTestDoubleClick()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
            //return true;
        }

        private void OnPatientLabTestDoubleClick()
        {
            var selectedPatientLabTest = this.Entity.InternalList.FirstOrDefault(x => x.IsSelected);
            if (selectedPatientLabTest != null)
            {
                //this.ContentViewModel = null;
                //this.ContentViewModel = new PatientLabTestDetailViewModel(this.Messenger, this.UserLogin, selectedPatientLabTest) { ParentViewModel = this };
                //GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);

                var childVM = new PatientLabTestDetailViewModel(this.MessengerInstance, this.UserLogin, selectedPatientLabTest)
                {
                    ParentViewModel = this.ParentViewModel
                };
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);
            }
        }

        #endregion

        #region Public Methods


        #endregion

        #region Private Methods
        private void GetPatientLabTestReports()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Entity = PatientAction.GetPatientTestReportList(this.DBConnectionString, this.SelectedPatient);
                }
                catch (Exception exception)
                {
                    NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                 ExceptionResources.ExceptionOccuredLogDetail);
                }
            });
        }
        private void GetRefreshPatientLabTestCollection()
        {

            Task.Factory.StartNew(() =>
            {
                this.Entity = PatientAction.GetPatientTestReportList(this.DBConnectionString, this.SelectedPatient);
            });
        }

        private void DeleteLabTest(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    //LabTestAction.DeleteLabTests(this.DBConnectionString, this.Entity.InternalList.Where(x => x.IsSelected));
                    //GetRefreshPatientLabTestCollection();
                    this.ParentViewModel.ShowProgressBar = false;
                });
            }
            else
            {
                this.ParentViewModel.ShowProgressBar = false;
            }
        }
        #endregion

    }
}