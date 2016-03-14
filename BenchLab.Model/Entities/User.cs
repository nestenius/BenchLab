using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchLab.Model
{
    [Table("TBL_USER")]
    public class User : BenchLabEntity<User>
    {
        #region Fields
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private Gender _gender;
        private int _age;
        private string _email;
        #endregion

        #region Properties
        [Column("FIRSTNAME")]
        public string FirstName
        {
            get { return _firstName; }
            set { this.SetProperty("FirstName", ref _firstName, value); }
        }
        [Column("MIDDLENAME")]
        public string MiddleName
        {
            get { return _middleName; }
            set { this.SetProperty("MiddleName", ref _middleName, value); }
        }
        [Column("LASTNAME")]
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

        [Column("EMAIL")]
        public string Email
        {
            get { return _email; }
            set { this.SetProperty("Email", ref _email, value); }
        }
        #endregion


    }
}
