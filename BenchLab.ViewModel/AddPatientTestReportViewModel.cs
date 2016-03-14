using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.DataAccess;
using BenchLab.Model;
using BenchLab.SimpleUI.Entities;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BenchLab.ViewModel
{
    public class AddPatientTestReportViewModel : BaseViewModel<PatientTestReport>
    {
        #region Delegate
        public Action RefreshPatientTestReport { get; set; }
        #endregion

        #region Constants

        #endregion

        #region Fields
        
        #endregion

        #region Properties

        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? Resources.TitleResources.AddPatientLabTest : Resources.TitleResources.EditPatientLabTest;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 380;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 490;
            }
        }

        public override DialogType DialogType
        {
            get
            {
                return DialogType.BySizeInPixel;
            }
        }
        #endregion

        //public LabTestReportCollection AllLabTestReportCollection
        //{
        //    get { return _allLabTestReportCollection; }
        //    set
        //    {
        //        _allLabTestReportCollection = value;
        //        //foreach (var labTestReport in AllLabTestReportCollection)
        //        //{
        //        //    if (this.Entity.LabTestReportCollection != null)
        //        //    {
        //        //        this.Entity.LabTestReportCollection.Add(labTestReport);
        //        //    }
        //        //}
        //        this.RaisePropertyChanged(() => AllLabTestReportCollection);
        //    }
        //}

        #endregion

        #region Commands
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSavePatientTestReport, CanSavePatientTestReport)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelPatientTestReport)); }
        }

        //private LabTestReportCollection _allLabTestReportCollection;

        #endregion

       #region Constructors

        public AddPatientTestReportViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public AddPatientTestReportViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        public AddPatientTestReportViewModel(IMessenger messenger, UserLogin userLogin, Patient patient)
            : base(messenger, userLogin)
        {
            this.Entity.SelectedPatient = patient;
            //GetLabTestReportCollection();
        }

        public AddPatientTestReportViewModel(IMessenger messenger, UserLogin userLogin, PatientTestReport patientTestReport, Patient patient)
            :base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = patientTestReport;
            this.Entity.SelectedPatient = patient;
            //GetLabTestReportCollection();
        }
        #endregion

        #region Override Methods
        private void OnSavePatientTestReport()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? PatientAction.AddPatientTestReport(this.DBConnectionString, this.Entity) : PatientAction.UpdatePatientTestReport(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                //this.Entity.ClearValues();

                if (RefreshPatientTestReport != null)
                    this.RefreshPatientTestReport();

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
        private bool CanSavePatientTestReport()
        {
            return this.Entity != null && this.Entity.IsSaveEnabled;
            //return this.Entity != null && this.Entity.HasValueInAllRequiredField;
            //return this.Entity != null && ( this.IsInEditMode ? this.Entity.VaidateChangePassword() && this.Entity.VaidateCurrentPassword() : ( this.Entity.VaidateNewPassword() && !string.IsNullOrEmpty(this.Entity.LoginName)));
        }

        private void OnCancelPatientTestReport()
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

        #region Private Methods
        //private void GetLabTestReportCollection()
        //{
        //    try
        //    {
        //        ShowProgressBar = true;
        //        Task.Factory.StartNew(() =>
        //        {
        //            //List<LabTestReport> labTestReports = LabTestAction.GetLabTestReportListByPatientTestReport(this.DBConnectionString, this.Entity);
        //            //foreach (var labTestReport in labTestReports)
        //            //{
        //            //    if(this.Entity.LabTestReportCollection == null)
        //            //    {
        //            //        this.Entity.LabTestReportCollection = new List<LabTestReport>();
        //            //    }
        //            //    this.Entity.LabTestReportCollection.Add(labTestReport);
        //            //}
        //            this.AllLabTestReportCollection = LabTestAction.GetLabTestReportListByPatientTestReport(this.DBConnectionString, this.Entity);
        //            //this.Entity.MakeObservableListClean();
        //            //this.Entity.MakeClean();
        //            ShowProgressBar = false;
        //        });

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //        throw;
        //    }
        //}
        #endregion

    }
}