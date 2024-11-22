using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using EvoComms.Core.Exceptions;

namespace EvoComms.Core.Util
{
    public class RecordDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString() ?? throw new InvalidDateValueException("Time", null);
            return DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}