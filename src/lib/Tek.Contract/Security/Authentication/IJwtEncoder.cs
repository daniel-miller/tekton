﻿namespace Tek.Contract
{
    public interface IJwtEncoder
    {
        string Encode(IJwt token, string secret);

        IJwt Decode(string token);
    }
}