using System.Collections.Generic;

namespace BenchLab.Model
{
    public class LabTestUnitCollection : BenchLabEntityList<LabTestUnit>
    {
        public LabTestUnitCollection()
        {

        }

        public LabTestUnitCollection(List<LabTestUnit> list)
            : base(list)
        {

        }

    }
}
