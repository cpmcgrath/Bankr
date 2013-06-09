using System;

namespace CMcG.Bankr.Data
{
    [Flags]
    public enum AccessLevel
    {
        None           = 0,
        Pin            = 1,
        Password       = 2,
        PinAndPassword = Pin | Password,
        CreateLogin    = 4
    }
}
