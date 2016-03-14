using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchLab.Resources;

namespace BenchLab.Model
{
    [Table("TBL_LABTEST_REPORT")]
    public class LabTestReport: BenchLabEntity<LabTestReport>
    {
        #region Fields
        private string _reportDescription;
        private int _patientTestReportRefID;
        private PatientTestReport _patientTestReport;
        private int _labTestRefID;
        private LabTest _labTest;
        private string _labTestRange;
        #endregion

        #region Properties
        [Column("REPORTDESCRIPTION")]
        public string ReportDescription
        {
            get { return _reportDescription; }
            set { this.SetProperty("ReportDescripton", ref _reportDescription, value); }
        }
        [Column("PATIENTTESTREPORTREF_ID")]
        public int PatientTestReportRefID
        {
            get { return _patientTestReportRefID; }
            set { this.SetProperty("PatientTestReportRefID", ref _patientTestReportRefID, value); }
        }
        [ForeignKey("PatientTestReportRefID")]
        public PatientTestReport PatientTestReport
        {
            get{ return _patientTestReport;}
            set{ this.SetProperty("PatientTestReport", ref _patientTestReport, value);}
        }
        [Column("LABTESTREF_ID")]
        public int LabTestRefID
        {
            get { return _labTestRefID; }
            set { this.SetProperty("LabTestRefID", ref _labTestRefID, value); }
        }
        [ForeignKey("LabTestRefID")]
        public LabTest LabTest
        {
            get { return _labTest; }
            set { this.SetProperty("LabTest", ref _labTest, value); }
        }
        [NotMapped]
        public string LabTestRange
        {
            get { return _labTestRange; }
            set { this.SetProperty("LabTestRange", ref _labTestRange, value); }
        }
        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        #endregion

        #region Override
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "ReportDescription")
                {
                    return string.IsNullOrEmpty(this.ReportDescription) ? ErrorResources.RequiredField : null;
                }

                return "";
            }
        }
        #endregion
    }
}
