using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchLab.Resources;

namespace BenchLab.Model
{
    [Table("TBL_PATIENT_TESTREPORT")]
    public class PatientTestReport : BenchLabEntity<PatientTestReport>
    {
        #region Fields
        private int _sn;
        private DateTime _reportDateEn;
        private string _reportDateNp;
        private string _technicianName;
        private string _consultantName;
        private string _labNr;
        private int _patientRefID;
        private Patient _patient;
        private ICollection<LabTestReport> _labTestReportCollection;
        private bool _isSelected;
        private bool _isDeleted;
        private Patient _selectedPatient;

        #endregion

        #region Properties
        [Column("SN")]
        public int Sn
        {
            get { return _sn; }
            set { this.SetProperty("Sn", ref _sn, value); }
        }
        [Column("REPORTDATEEN")]
        public DateTime ReportDateEn
        {
            get { return _reportDateEn; }
            set { this.SetProperty("]", ref _reportDateEn, value); }
        }
        [Column("REPORTDATENP")]
        public string ReportDateNp
        {
            get { return _reportDateNp; }
            set { this.SetProperty("ReportDateNp", ref _reportDateNp, value); }
        }
        [Column("TECHNICIANNAME")]
        public string TechnicianName
        {
            get { return _technicianName; }
            set { this.SetProperty("TechnicianName", ref _technicianName, value); }
        }
        [Column("CONSULTANTNAME")]
        public string ConsultantName
        {
            get { return _consultantName; }
            set { this.SetProperty("ConsultantName", ref _consultantName, value); }
        }
        [Column("LABNR")]
        public string LabNr
        {
            get { return _labNr; }
            set { this.SetProperty("LabNr", ref _labNr, value); }
        }
        [Column("PATIENTREF_ID")]
        public int PatientRefID
        {
            get { return _patientRefID; }
            set { this.SetProperty("PatientRefID", ref _patientRefID, value); }
        }
        [ForeignKey("PatientRefID")]
        public virtual Patient Patient
        {
            get { return _patient; }
            set { this.SetProperty("Patient", ref _patient, value); }
        }

        public virtual ICollection<LabTestReport> LabTestReportCollection
        {
            get { return _labTestReportCollection; }
            set { this.SetProperty("LabTestReportCollection", ref _labTestReportCollection, value); }
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
        public bool IsSaveEnabled
        {
            get
            {
                return this.Sn > 0 &&
                       !string.IsNullOrEmpty(this.ReportDateNp);
            }
        }

        [NotMapped]
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                this.SetProperty("SelectedPatient", ref _selectedPatient, value);
                //Bikash: can have null too. Handle this later.
                this.PatientRefID = value.ID.Value;
                this.NotifyPropertyChanged("FormattedExistingPatientName");
            }
        }

        public string FormattedExistingPatientName
        {
            get
            {
                return this.SelectedPatient != null
                           ? string.Format("{0} - {1}", this.SelectedPatient.ID, this.SelectedPatient.FormattedName)
                           : "";
            }
        }
        #endregion

        #region Constructors
        public PatientTestReport() 
        {
            this.ReportDateEn = DateTime.Now;
        }
        #endregion

        #region Public Methods

        #endregion

        #region Override
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Sn")
                {
                    return this.Sn <=0 ? ErrorResources.RequiredField : null;
                }
                if (columnName == "ReportDateNp")
                {
                    return string.IsNullOrEmpty(this.ReportDateNp) ? ErrorResources.RequiredField : null;
                }

                return "";
            }
        }
        #endregion
    }

    //public class CustomPatientTestReport
    //{
    //    public int ReportId { get; set; }
    //    public int? SN { get; set; }
    //    public string LabNr { get; set; }
    //    public string FirstName { get; set; }
    //    public string MiddleName { get; set; }
    //    public string LastName { get; set; }
    //    public string ReportDate { get; set; }
    //    public string Technician { get; set; }
    //    public string Consultant { get; set; }
    //}
}
