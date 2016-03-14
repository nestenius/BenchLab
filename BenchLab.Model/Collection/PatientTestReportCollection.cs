using System.Collections.Generic;

namespace BenchLab.Model
{
    public class PatientTestReportCollection : BenchLabEntityList<PatientTestReport>
    {
        public PatientTestReportCollection()
        {

        }

        public PatientTestReportCollection(List<PatientTestReport> list)
            : base(list)
        {

        }

    }
}
