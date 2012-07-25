using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace CMcG.CommonwealthBank.Logic
{
    public class CbaDateConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var input = ((string)reader.Value).Trim();
            return DateTime.ParseExact(input, "dd/MM/yy", CultureInfo.CurrentCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = (DateTime)value;
            writer.WriteValue(date.ToString("dd/MM/yy"));
        }
    }
}
