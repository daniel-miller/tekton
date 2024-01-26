using System;

namespace Common.Contract
{
    [Flags]
    public enum AccessFunctions 
    { 
        Execute = 1,
        Read = 2,
        Write = 4,
        Create = 8,
        Delete = 16,
        Administrate = 32,
        Configure = 64
    }
}