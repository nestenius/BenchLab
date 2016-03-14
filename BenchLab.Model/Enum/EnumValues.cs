using System;
using System.Runtime.Serialization;

namespace BenchLab.Model
{
    [Serializable]
    [DataContract]
    public enum BLStatus
    {
        Ready = 0,
        Loading = 1
    }

    [Serializable]
    [DataContract]
    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public enum DirtyState : short
    {
        UnChanged = 0,
        PendingAddChange = 1,
        PendingDelete = 2,
    }

    public enum Status
    {
        InActive = 0,
        Active = 1,
    }

    public enum AccessType : short
    {
        Default = 0,
        ReadOnly = 1,
    }

    [Serializable]
    public enum ActionResultType
    {
        DataFetched,
        RequestedNew,
        DataNotFound
    }
}
