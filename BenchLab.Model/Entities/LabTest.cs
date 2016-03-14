using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchLab.Resources;

namespace BenchLab.Model
{
    [Table("TBL_LABTEST")]
    public class LabTest : BenchLabEntity<LabTest>
    {
        #region Fields
        private string _testName;
        private decimal? _minStdGeneral;
        private decimal? _maxStdGeneral;
        private decimal? _minStdMale;
        private decimal? _maxStdMale;
        private decimal? _minStdFemale;
        private decimal? _maxStdFemale;
        private int _labTestCategoryRefID;
        private LabTestCategory _labTestCategory;
        private int _labTestUnitRefID;
        private LabTestUnit _labTestUnit;
        private bool _isSelected;
        private bool _isDeleted;
        private ICollection<LabTestReport> _labTestReportCollection;
        #endregion

        #region Properties
        [Column("TESTNAME")]
        public string TestName
        {
            get { return _testName; }
            set { this.SetProperty("TestName", ref _testName, value); }
        }

        [Column("MINSTDGENERAL")]
        public decimal? MinStdGeneral
        {
            get { return _minStdGeneral; }
            set { this.SetProperty("MinStdGeneral", ref _minStdGeneral, value); }
        }

        [Column("MAXSTDGENERAL")]
        public decimal? MaxStdGeneral
        {
            get { return _maxStdGeneral; }
            set { this.SetProperty("MaxStdGeneral", ref _maxStdGeneral, value); }
        }

        [Column("MINSTDMALE")]
        public decimal? MinStdMale
        {
            get { return _minStdMale; }
            set { this.SetProperty("MinStdMale", ref _minStdMale, value); }
        }

        [Column("MAXSTDMALE")]
        public decimal? MaxStdMale
        {
            get { return _maxStdMale; }
            set { this.SetProperty("MaxStdMale", ref _maxStdMale, value); }
        }

        [Column("MINSTDFEMALE")]
        public decimal? MinStdFemale
        {
            get { return _minStdFemale; }
            set { this.SetProperty("MinStdFemale", ref _minStdFemale, value); }
        }

        [Column("MAXSTDFEMALE")]
        public decimal? MaxStdFemale
        {
            get { return _maxStdFemale; }
            set { this.SetProperty("MaxStdFemale", ref _maxStdFemale, value); }
        }

        [Column("LABTESTCATEGORYREF_ID")]
        public int LabTestCategoryRefID
        {
            get { return _labTestCategoryRefID; }
            set { this.SetProperty("LabTestCategoryRefID", ref _labTestCategoryRefID, value); }
        }
        [ForeignKey("LabTestCategoryRefID")]
        public virtual LabTestCategory LabTestCategory
        {
            get { return _labTestCategory; }
            set { this.SetProperty("LabTestCategory", ref _labTestCategory, value); }
        }
        [Column("LABTESTUNITREF_ID")]
        public int LabTestUnitRefID
        {
            get { return _labTestUnitRefID; }
            set { this.SetProperty("LabTestUnitRefID", ref _labTestUnitRefID, value); }
        }
        [ForeignKey("LabTestUnitRefID")]
        public virtual LabTestUnit LabTestUnit
        {
            get { return _labTestUnit; }
            set { this.SetProperty("LabTestUnit", ref _labTestUnit, value); }
        }
        [NotMapped]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.SetProperty("IsSelected", ref _isSelected, value); }
        }

        [NotMapped]
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { this.SetProperty("IsDeleted", ref _isDeleted, value); }
        }

        [NotMapped]
        public bool HasValueInAllRequiredField
        {
            get
            {
                return (!string.IsNullOrEmpty(TestName) && LabTestCategoryRefID > 0 && LabTestUnitRefID > 0);
            }
        }

        public virtual ICollection<LabTestReport> LabTestReportCollection
        {
            get { return _labTestReportCollection; }
            set { this.SetProperty("LabTestReportCollection", ref _labTestReportCollection, value); }
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
                if (columnName == "TestName")
                {
                    return string.IsNullOrEmpty(this.TestName) ? ErrorResources.RequiredField : null;
                }

                return "";
            }
        }
        #endregion
    }

    public class CustomLabTestElement
    { 
        public int? ID {get;set;}
        public int? LabTestRefID{get;set;}
        public string TestName {get;set;}
        public string UnitName {get;set;}
        public string ReportDescription { get; set; }
        public string LabTestCategoryName {get;set;}
        public DateTime LastChangeDateTimeUTC {get;set;}
        public string LastChangeUser{get;set;}
        public DateTime CreationDateTimeUTC{get;set;}
        public string CreationUser{get;set;}
        public Status Status {get;set;}
    }

}
