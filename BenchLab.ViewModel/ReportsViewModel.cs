using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using BenchLab.Bases;
using BenchLab.Model;
using BenchLab.Resources;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using BenchLab.DataAccess;
using BenchLab.ErrorLog;

namespace BenchLab.ViewModel
{

    public class ReportsViewModel : BaseViewModel<PatientTestReportCollection>
    {
        #region Fields

        private PatientTestReportCollection _reportList;
        private string _searchPatientFirstNameText;
        private string _searchPatientMiddleNameText;
        private string _searchPatientLastNameText;
        private string _searchSnText;
        private string _searchLabNrText;
        private string _searchTechnicianText;
        
        #endregion

        #region Properties

        public PatientTestReportCollection ReportList
        {
            get { return _reportList; }
            set
            {
                _reportList = value;
                this.RaisePropertyChanged(() => ReportList);
            }
        }

        public string SearchPatientFirstNameText
        {
            get { return _searchPatientFirstNameText; }
            set 
            {
                _searchPatientFirstNameText = value;
                this.RaisePropertyChanged(() => SearchPatientFirstNameText);
            }
        }

        public string SearchPatientMiddleNameText
        {
            get { return _searchPatientMiddleNameText; }
            set
            {
                _searchPatientMiddleNameText = value;
                this.RaisePropertyChanged(() => SearchPatientMiddleNameText);
            }
        }
        
        public string SearchPatientLastNameText
        {
            get { return _searchPatientLastNameText; }
            set
            {
                _searchPatientLastNameText = value;
                this.RaisePropertyChanged(() => SearchPatientLastNameText);
            }
        }

        public string SearchSnText
        {
            get { return _searchSnText; }
            set 
            {
                _searchSnText = value;
                this.RaisePropertyChanged(() => SearchSnText);
            }
        }

        public string SearchLabNrText
        {
            get { return _searchLabNrText; }
            set
            {
                _searchLabNrText = value;
                this.RaisePropertyChanged(() => SearchLabNrText);
            }
        }

        public string SearchTechnicianText
        {
            get { return _searchTechnicianText; }
            set
            {
                _searchTechnicianText = value;
                this.RaisePropertyChanged(() => SearchTechnicianText);
            }
        }

        #endregion

        #region Commands

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return this._searchCommand ?? (this._searchCommand = new RelayCommand(OnSearchReport, CanSearchReport)); }
        }

        private ICommand _reportDoubleClick;
        public ICommand ReportDoubleClick
        {
            get { return _reportDoubleClick; }
            set
            {
                _reportDoubleClick = value;
                this.RaisePropertyChanged(() => ReportDoubleClick);
            }
        }

        #endregion

        #region Constructors

        public ReportsViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            GetPatientLabTestReports();
        }

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
            ReportDoubleClick = new RelayCommand(OnReportDoubleClick, CanReportDoubleClick);
        }
        private void OnSearchReport()
        {
            GetPatientLabtestReportsWithSearch(this.SearchPatientFirstNameText, this.SearchPatientMiddleNameText, this.SearchPatientLastNameText, this.SearchSnText, this.SearchLabNrText, this.SearchTechnicianText);
        }

        private bool CanSearchReport()
        {
            return true;
        }

        private void OnReportDoubleClick()
        {
            var selectedReport = this.Entity.InternalList.FirstOrDefault(x => x.IsSelected);
            if (selectedReport != null)
            {
                this.ContentViewModel = null;
                this.ContentViewModel = new ReportDetailViewModel(this.Messenger, this.UserLogin, selectedReport) { ParentViewModel = this };
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
            }
        }

        private bool CanReportDoubleClick()
        {
            return true;
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
                    this.Entity = PatientAction.PatientTestReportList(this.DBConnectionString);
                }
                catch (Exception exception)
                {
                    NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                 ExceptionResources.ExceptionOccuredLogDetail);
                }
            });
        }
        private void GetPatientLabtestReportsWithSearch(string firstName, string middleName, string lastName, string sn, string labNr, string technician)
        {
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        this.Entity = PatientAction.PatientTestReportListWithSearch(this.DBConnectionString, firstName, middleName, lastName, sn, labNr, technician);
                    }
                    catch (Exception exception)
                    {
                        NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                 ExceptionResources.ExceptionOccuredLogDetail);
                    }
                });
        }
        
        #endregion

    }
}
