using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchLab.Resources;

namespace BenchLab.Model
{
    [Table("TBL_LABTEST_UNIT")]
    public class LabTestUnit : BenchLabEntity<LabTestUnit>
    {
        #region Fields
        private string _unitName;
        private string _unitDescription;
        private ICollection<LabTest> _labTestCollection;
        private bool _isSelected;
        #endregion

        #region Properties

        [Column("UNITNAME")]
        public string UnitName
        {
            get { return _unitName; }
            set { this.SetProperty("UnitName", ref _unitName, value); }
        }

        [Column("UNITDESCRIPTION")]
        public string UnitDescription
        {
            get { return _unitDescription; }
            set { this.SetProperty("UnitDescription", ref _unitDescription, value); }
        }
        public virtual ICollection<LabTest> LabTestCollection
        {
            get { return _labTestCollection; }
            set { this.SetProperty("LabTestCollection", ref _labTestCollection, value); }
        }

        [NotMapped]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
            }
        }

        [NotMapped]
        public bool HasValueInAllRequiredField
        {
            get
            {
                return (!string.IsNullOrEmpty(UnitName));
            }
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
                if (columnName == "UnitName")
                {
                    return string.IsNullOrEmpty(this.UnitName) ? ErrorResources.RequiredField : null;
                }

                return "";
            }
        }
        #endregion
    }
}
