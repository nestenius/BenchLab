using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BenchLab.DataAccess;
using BenchLab.ErrorLog;
using BenchLab.Model;
using BenchLab.Resources;
using BenchLab.SimpleUI.Entities;
using BenchLab.ViewModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;


namespace BenchLab.ViewModel
{
    public class ReportDetailViewModel : BaseViewModel<PatientTestReport>
    {
        #region Fields
        
        #endregion

        #region Properties

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

        #region Command Properties

        private ICommand _printReportStatusCommand;
        public ICommand PrintReportCommand
        {
            get { return this._printReportStatusCommand ?? (this._printReportStatusCommand = new RelayCommand(OnPrintReport, CanPrintReport)); }
        }

        #endregion

        #region Constructors
        public ReportDetailViewModel(IMessenger messenger, UserLogin userLogin, PatientTestReport patientTestReport)
            : base(messenger, userLogin)
        {
            this.Entity = patientTestReport;
            this.GetRefreshLabTestTReport();
        }
        #endregion

        #region Override Methods
        
        #endregion

        #region Command Methods

        private bool CanPrintReport()
        {
            return (this.Entity != null);
        }

        private void OnPrintReport()
        {
            try
            {
                var childVM = new PrintViewModel(this.Messenger, this.UserLogin, this.Entity, this.LabTestReports) { ParentViewModel = this };
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        #endregion

        #region Private Methods
        
        #endregion

        #region Public Methods
        private void GetRefreshLabTestTReport()
        {

            Task.Factory.StartNew(() =>
            {
                var items = LabTestAction.GetLabTestReportListByPatientTestReport(this.DBConnectionString, this.Entity);
                if (items != null && items.InternalList.Any())
                {
                    this.Entity.LabTestReportCollection = null;
                    this.Entity.LabTestReportCollection = items.InternalList.ToList().FindAll(x => x.ReportDescription != null && x.ReportDescription != string.Empty);                   
                }
                if (items != null && items.ObservableList.Any())
                {
                    this.LabTestReports = items.ObservableList.ToList().FindAll(x => x.ReportDescription != null && x.ReportDescription != string.Empty).AsEnumerable<LabTestReport>();
                }
            });
        }

        #endregion



    }
}