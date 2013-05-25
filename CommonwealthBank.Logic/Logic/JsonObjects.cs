using System;
using System.Linq;

namespace CMcG.CommonwealthBank.Logic
{
    public class UpcomingTransResult
    {
        public Data.UpcomingTransaction[] Transactions { get; set; }
    }

    public class LoginResult
    {
        public string         SID           { get; set; }
        public AccountGroup[] AccountGroups { get; set; }
    }

    public class AccountGroup
    {
        public Data.Account[] ListAccount { get; set; }
    }

    public class JsonParameters
    {
        public NameValue[] Params { get; set; }
    }

    public class NameValue
    {
        public NameValue() { }
        public NameValue(string name, object value)
        {
            Name  = name;
            Value = value;
        }

        public string Name  { get; set; }
        public object Value { get; set; }
    }

    public class TransferAccountResult
    {
        public Data.TransferToAccount[] AccountsFrom        { get; set; }
        public Data.TransferToAccount[] AccountsToLinked    { get; set; }
        public Data.TransferToAccount[] AccountsToNotLinked { get; set; }
    }
}