using System.Collections.Generic;

namespace BenchLab.Model
{
    public class LabTestCategoryCollection : BenchLabEntityList<LabTestCategory>
    {
        public LabTestCategoryCollection()
        {

        }

        public LabTestCategoryCollection(List<LabTestCategory> list)
            : base(list)
        {

        }

    }
}
