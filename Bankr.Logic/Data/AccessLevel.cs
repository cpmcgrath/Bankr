using System;

namespace CMcG.CommonwealthBank.Data
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
