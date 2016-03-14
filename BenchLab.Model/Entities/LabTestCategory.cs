using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchLab.Resources;

namespace BenchLab.Model
{
    [Table("TBL_LABTEST_CATEGORY")]
    public class LabTestCategory : BenchLabEntity<LabTestCategory>
    {
        #region Fields
        private string _labTestCategoryName;
        private int? _parentCategoryID;
        private ICollection<LabTest> _labTestCollection;
        private bool _isSelected;
        #endregion

        #region Properties
        [Column("LABTESTCATEGORYNAME")]
        public string LabTestCategoryName
        {
            get { return _labTestCategoryName; }
            set { this.SetProperty("LabTestCategoryName", ref _labTestCategoryName, value); }
        }
        [Column("PARENTCATEGORYID")]
        public int? ParentCategoryID
        {
            get { return _parentCategoryID; }
            set { this.SetProperty("ParentCategoryID", ref  _parentCategoryID, value); }
        }

        public virtual ICollection<LabTest> LabTestCollection
        {
            get { return _labTestCollection; }
            set { this.SetProperty("LabTestCollection", ref _labTestCollection, value); }
        }

        [ForeignKey("ParentCategoryID")]
        public LabTestCategory ParentLabTestCategory { get; set; }

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
                return (!string.IsNullOrEmpty(LabTestCategoryName));
            }
        }
        #endregion

        #region Constructors
        public LabTestCategory()
        {
            this.ParentCategoryID = 0;
        }
        #endregion

        #region Public Methods
        
        #endregion

        #region Override
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "LabTestCategoryName")
                {
                    return string.IsNullOrEmpty(this.LabTestCategoryName) ? ErrorResources.RequiredField : null;
                }

                return "";
            }
        }
        #endregion
    }
}
