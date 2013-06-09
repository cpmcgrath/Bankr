using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace CMcG.Bankr.Logic
{
    public class CbaAmountConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return 0M;

            var     input    = ((string)reader.Value).Trim();
            bool    isCredit = input.EndsWith(" CR");
            decimal amount   = decimal.Parse(input.Substring(1, input.Length - 4));
            return isCredit ? amount : -amount;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}