using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace WpfTemplateStudio.Core.Services
{
    public static class ObjectSerializer
    {
        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                string jsonString = JsonConvert.SerializeObject(obj);
                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (JsonException ex)
            {
                // Handle JSON serialization exceptions
                // Log the exception or handle it as needed
                throw new InvalidOperationException("Serialization failed.", ex);
            }
        }

        // Convert a byte array to an object
        public static T? ByteArrayToObject<T>(byte[] arrBytes)
        {
            if (arrBytes == null || arrBytes.Length == 0)
                return default;

            try
            {
                string jsonString = Encoding.UTF8.GetString(arrBytes);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization exceptions
                // Log the exception or handle it as needed
                throw new InvalidOperationException("Deserialization failed.", ex);
            }
        }
    }
}
