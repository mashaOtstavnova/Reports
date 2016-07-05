﻿using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;
using System.Globalization;

namespace IikoBizApi
{
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        private const string Format = "yyyy.MM.dd";
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(Format));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            var s = reader.Value.ToString();
            DateTime result;
            if (DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            return DateTime.Now;
        }
    }
}