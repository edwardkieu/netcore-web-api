using Application.Constants;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Commons.Helpers
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DateTime dt;
            DateTime.TryParseExact(reader.GetString(),
                                   GlobalConstants.DATE_TIME_FORMAT,
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out dt);

            if (dt == DateTime.MinValue)
            {
                DateTime.TryParse(reader.GetString(), out dt);
            }

            return dt;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(GlobalConstants.DATE_TIME_FORMAT));
        }
    }
}