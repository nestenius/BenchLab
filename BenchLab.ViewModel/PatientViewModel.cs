using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.Bases;
using BenchLab.DataAccess;
using BenchLab.ErrorLog;
using BenchLab.Model;
using BenchLab.Resources;
using BenchLab.SimpleUI.Entities;

namespace BenchLab.ViewModel
{
    public class PatientViewModel : BaseViewModel<PatientCollection>
    {
        #region Fields
        private bool _showProgressBar;
        #endregion

        #region Command Properties
        //In Case an extra button is needed
        private ICommand _addLabTestCommand;
        //private ICommand _patientDoubleClick;

        public ICommand AddLabTestCommand
        {
            get { return this._addLabTestCommand ?? (this._addLabTestCommand = new RelayCommand(OnAddLabTest, CanAddLabTest)); }
        }

        //public ICommand PatientDoubleClick
        //{
        //    get { return _patientDoubleClick; }
        //    set
        //    {
        //        _patientDoubleClick = value;
        //        this.RaisePropertyChanged(() => PatientDoubleClick);
        //    }
        //}

        #endregion

      #region Properties

      #endregion

      #region Constructors
      public PatientViewModel(IMessenger messenger)
            : base(messenger)
        {
            GetPatientCollection();
        }

      public PatientViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            GetPatientCollection();
        }
        #endregion

        #region Override
        public override void Initialize()
        {
            base.Initialize();
            AddCommand = new RelayCommand(OnAddItem, CanAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
            //PatientDoubleClick = new RelayCommand(OnPatientDoubleClick, CanPatientDoubleClick);
        }

        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            if (this.ParentViewModel is MainWindowViewModel)
            {
                ((MainWindowViewModel)this.ParentViewModel).IsPatientTabSelected = true;
            }

        }

        public override void Unload()
        {
            base.Unload();
            this._addLabTestCommand = null;
        }
        #endregion

        #region Override Command Methods
        public override void OnAddItem()
        {
            this.OnAddPatient();
        }
        public override  bool CanAddItem()
        {
            return this.CanAddPatient();
        }
        public override  void OnDeleteItem()
        {
            this.OnDeletePatient();
        }
        public override  bool CanDeleteItem()
        {
            return this.CanDeletePatient();
        }
        public override  void OnEditItem()
        {
            this.OnEditPatient();
        }
        public override  bool CanEditItem()
        {
            return this.CanEditPatient();
        }
        public override  void OnRefreshItem()
        {
            this.OnRefreshPatient();
        }
        #endregion

        #region Command Methods
        private bool CanAddPatient()
        {
            return true;
        }

        private void OnAddPatient()
        {
            try
            {
                var childVM = new AddPatientViewModel(this.Messenger, this.UserLogin) { ParentViewModel = this };
                childVM.RefreshPatients += this.GetPatientCollection;
                var messageDialog = new VMMessageDailog() {ChildViewModel = childVM};
                MessengerInstance.Send(messageDialog);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        private void OnEditPatient()
        {
            try
            {
                var childVM = new AddPatientViewModel(this.Messenger, this.UserLogin,
                                                       this.Entity.InternalList.FirstOrDefault(x => x.IsSelected))
                    {
                        ParentViewModel = this
                    };
                childVM.RefreshPatients += this.GetPatientCollection;
                //childVM.SavePatient +=
                //    customer => vAction.UpdatePatient(this.DBConnectionString,
                //                                              this.Entity.InternalList.FirstOrDefault(x => x.IsSelected));
                var messageDialog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDialog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        private bool CanEditPatient()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        private bool CanDeletePatient()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }

        private void OnDeletePatient()
        {
            ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeletePatient) { Caption = Resources.MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }

        private void DeletePatient(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
               
                Task.Factory.StartNew(() =>
                    {
                        PatientAction.DeletePatients(this.DBConnectionString,
                                                        this.Entity.InternalList.Where(x => x.IsSelected));
                        GetPatientCollection();
                        ShowProgressBar = false;
                    });
            }
            else
            {
                ShowProgressBar = false;
            }
        }

        private void OnRefreshPatient()
        {
            GetPatientCollection();
        }

        private bool CanAddLabTest()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        private void OnAddLabTest()
        {
            try
            {
                var childVM = new PatientLabTestViewModel(this.MessengerInstance, this.UserLogin,
                                                       this.Entity.InternalList.FirstOrDefault(x => x.IsSelected))
                    {
                        ParentViewModel = this.ParentViewModel
                    };
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        private bool CanPatientDoubleClick()
        {
            return true;
        }

        private void OnPatientDoubleClick()
        {
            var selectedPatient = this.Entity.InternalList.FirstOrDefault(x => x.IsSelected);
            if (selectedPatient != null)
            {
                //this.ContentViewModel = null;
                //this.ContentViewModel = new PatientDetailViewModel(this.Messenger, this.UserLogin, selectedPatient) { ParentViewModel = this };
                //GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
            }
        }
        #endregion

        #region Private Methods
        private void GetPatientCollection()
        {
            try
            {
                ShowProgressBar = true;
                Task.Factory.StartNew(() =>
                    {
                        this.Entity = PatientAction.GetPatientList(this.DBConnectionString);
                        //this.Entity.MakeObservableListClean();
                        //this.Entity.MakeClean();
                        ShowProgressBar = false;
                    });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        #endregion

    }
}