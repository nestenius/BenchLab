using System.Collections.Generic;

namespace BenchLab.Model
{
    public class LabTestReportCollection : BenchLabEntityList<LabTestReport>
    {
        public LabTestReportCollection()
        {

        }

        public LabTestReportCollection(List<LabTestReport> list)
            : base(list)
        {

        }

    }
}
