using System.Collections.Generic;

namespace BenchLab.Model
{
    public class LabTestCollection : BenchLabEntityList<LabTest>
    {
        public LabTestCollection()
        {

        }

        public LabTestCollection(List<LabTest> list)
            : base(list)
        {

        }

    }
}
