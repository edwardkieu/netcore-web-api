using Application.Constants;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.DateTime;

namespace Application.Commons.Helpers
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TryParseExact(reader.GetString(),
                                   GlobalConstants.DateTimeFormat,
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out var dt);

            if (dt == MinValue)
            {
                TryParse(reader.GetString(), out dt);
            }

            return dt;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(GlobalConstants.DateTimeFormat));
        }
    }
}