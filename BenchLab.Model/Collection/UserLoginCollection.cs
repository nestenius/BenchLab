using System.Collections.Generic;

namespace BenchLab.Model
{
    public class UserLoginCollection : BenchLabEntityList<UserLogin>
    {
        public UserLoginCollection()
        {

        }

        public UserLoginCollection(List<UserLogin> list)
            : base(list)
        {

        }

    }
}
