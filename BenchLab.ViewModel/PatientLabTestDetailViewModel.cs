using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using BenchLab.DataAccess;
using BenchLab.Model;
using BenchLab.SimpleUI.Entities;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace BenchLab.ViewModel
{
    public class PatientLabTestDetailViewModel : BaseViewModel<LabTestReportCollection>
    {
        #region Fields
        private PatientTestReport _selectedPatientTestReport;
        #endregion

        #region Properties
        public PatientTestReport SelectedPatientTestReport
        {
            get { return _selectedPatientTestReport; }
            set
            {
                _selectedPatientTestReport = value;
                this.RaisePropertyChanged(() => this.SelectedPatientTestReport);
            }
        }

        public CollectionViewSource PatientTestCollectionView { get; private set; }

        private IEnumerable<LabTestReport> _labTestReports;

        public IEnumerable<LabTestReport> LabTestReports
        {
            get { return _labTestReports; }
            set
            {
                _labTestReports = value;
                RaisePropertyChanged(() => LabTestReports);
            }
        }
        #endregion

        #region Window Properties
        public override string Title
        {
            get
            {
                return Resources.TitleResources.PatientLabTests;
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

        #region Constructors
        public PatientLabTestDetailViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public PatientLabTestDetailViewModel(IMessenger messenger, UserLogin userLogin, PatientTestReport patientTestReport)
            : base(messenger, userLogin)
        {
            SelectedPatientTestReport = patientTestReport;

            if (SelectedPatientTestReport != null)
            {
                this.IsInEditMode = true;
                this.Entity = LabTestAction.GetLabTestReportListByPatientTestReport(this.DBConnectionString, SelectedPatientTestReport);//new LabTestReportCollection(SelectedPatientTestReport.LabTestReportCollection.ToList());
                this.LabTestReports = this.Entity.ObservableList.AsEnumerable<LabTestReport>();
            }
            else
            {
                this.GetRefreshLabTestReportCollection();
            }
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Unload()
        {
            base.Unload();
        }
        #endregion

        #region Command Methods
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSavePatientLabTestDetail, CanSavePatientLabTestDetail)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelPatientLabTestDetail)); }
        }
        #endregion

        #region Override Methods
        private void OnSavePatientLabTestDetail()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? LabTestAction.AddLabTestReports(this.DBConnectionString, this.Entity) : LabTestAction.UpdateLabTestReports(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {

                GetRefreshLabTestReportCollection();

                var messageDailog = new MessageDailog()
                {
                    Caption = Resources.MessageResources.DataSavedSuccessfully,
                    DialogButton = DialogButton.Ok,
                    Title = Resources.TitleResources.Information
                };

                MessengerInstance.Send(messageDailog);

                if (this.CloseWindow != null)
                    this.CloseWindow();


            }
            else
            {
                var messageDailog = new MessageDailog() { Caption = Resources.MessageResources.DataSavedFailed, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Error };
                MessengerInstance.Send(messageDailog);
            }
        }
        private bool CanSavePatientLabTestDetail()
        {
            return this.Entity != null; //&& this.Entity.IsSaveEnabled;
            //return this.Entity != null && this.Entity.HasValueInAllRequiredField;
            //return this.Entity != null && ( this.IsInEditMode ? this.Entity.VaidateChangePassword() && this.Entity.VaidateCurrentPassword() : ( this.Entity.VaidateNewPassword() && !string.IsNullOrEmpty(this.Entity.LoginName)));
        }

        private void OnCancelPatientLabTestDetail()
        {
            var messageDailog = new MessageDailog((result) =>
            {
                if (result == DialogResult.Ok)
                {
                    //this.Entity.ClearValues();

                    if (this.ParentViewModel != null)
                        this.ParentViewModel.ChildViewModel = null;
                    this.Unload();

                    if (this.CloseWindow != null)
                        this.CloseWindow();


                }
            }) { Caption = Resources.MessageResources.CancelWindowMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);


        }
        #endregion
        #region Public Methods

        #endregion

        #region Private Methods
        private void GetRefreshLabTestReportCollection()
        {

            Task.Factory.StartNew(() =>
            {
                //this.Entity = LabTestAction.GetLabTestReportList(this.DBConnectionString, this.SelectedPatientTestReport);
                this.Entity = LabTestAction.GetLabTestReportListByPatientTestReport(this.DBConnectionString, this.SelectedPatientTestReport);
                //this.PatientTestCollectionView.Source = this.Entity.ObservableList.AsEnumerable<LabTestReport>();
                this.LabTestReports = this.Entity.ObservableList.AsEnumerable<LabTestReport>();
            });
        }
        #endregion


    }
}
