using System.Collections.Generic;

namespace BenchLab.Model
{
    public class PatientCollection : BenchLabEntityList<Patient>
    {
        public PatientCollection()
        {

        }

        public PatientCollection(List<Patient> list)
            : base(list)
        {

        }

    }
}
