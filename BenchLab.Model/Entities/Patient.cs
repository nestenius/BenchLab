using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BenchLab.Resources;

namespace BenchLab.Model
{
    [Table("TBL_PATIENT")]
    public class Patient : BenchLabEntity<Patient>
    {
        #region Fields
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private int _age;
        private Gender _gender;
        private string _address;
        private string _contactnr;
        private string _email;
        private bool _isSelected;
        private bool _isDeleted;
        private ICollection<PatientTestReport> _patientTestReportCollection;
        #endregion

        #region Properties
        [Column("FIRST_NAME")]
        public string FirstName
        {
            get { return _firstName; }
            set { this.SetProperty("FirstName", ref _firstName, value); }
        }
        [Column("MIDDLE_NAME")]
        public string MiddleName
        {
            get { return _middleName; }
            set { this.SetProperty("MiddleName", ref _middleName, value); }
        }
        [Column("LAST_NAME")]
        public string LastName
        {
            get { return _lastName; }
            set { this.SetProperty("LastName", ref _lastName, value); }
        }
        [Column("AGE")]
        public int Age
        {
            get { return _age; }
            set { this.SetProperty("Age", ref _age, value); }
        }
        [Column("GENDER")]
        public Gender Gender
        {
            get { return _gender; }
            set { this.SetProperty("Gender", ref _gender, value); }
        }
        [Column("ADDRESS")]
        public string Address
        {
            get { return _address; }
            set { this.SetProperty("Address", ref _address, value); }
        }
        [Column("CONTACTNR")]
        public string ContactNr
        {
            get { return _contactnr; }
            set { this.SetProperty("ContactNr", ref _contactnr, value); }
        }
        [Column("EMAIL")]
        public string Email
        {
            get { return _email; }
            set { this.SetProperty("Email", ref _email, value); }
        }
        [NotMapped]
        public string FormattedName
        {
            get { return !string.IsNullOrEmpty(LastName) ? this.LastName + ", " + this.FirstName : this.FirstName; }
        }
        [NotMapped]
        public string FormattedAddress
        {
            get
            {
                return this.Address.Length > 23 ? this.Address.Substring(0, 23) + Environment.NewLine + this.Address.Substring(23, this.Address.Length - 23) : this.Address;
            }
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

        public virtual ICollection<PatientTestReport> PatientTestReportCollection
        {
            get { return _patientTestReportCollection; }
            set { this.SetProperty("PatientTestReportCollection", ref _patientTestReportCollection, value); }
        }
        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        #endregion

        #region Override Methods
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName")
                {
                    return string.IsNullOrEmpty(this._firstName) ? ErrorResources.RequiredField : null;
                }
                if (columnName == "LastName")
                {
                    return string.IsNullOrEmpty(this._lastName) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "Email")
                {
                    if (!String.IsNullOrEmpty(_email))
                    {
                        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        var match = regex.Match(_email);
                        return match.Success ? "" : ErrorResources.InvalidEmailFormat;

                    }
                }
                return null;
            }
        }
        #endregion
    }
}
