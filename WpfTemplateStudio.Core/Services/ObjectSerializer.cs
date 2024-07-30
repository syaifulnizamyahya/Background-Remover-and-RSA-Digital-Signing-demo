using Newtonsoft.Json;
using System.Text;

namespace WpfTemplateStudio.Core.Services
{
    public static class ObjectSerializer
    {
        /// <summary>
        /// Convert an object to a byte array
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
                throw new InvalidOperationException(WpfTemplateStudio.Core.Properties.Resources.ObjectSerializer_ObjectToByteArray_SerializationFailed, ex);
            }
        }

        /// <summary>
        /// Convert a byte array to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T? ByteArrayToObject<T>(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return default;

            try
            {
                string jsonString = Encoding.UTF8.GetString(byteArray);
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(WpfTemplateStudio.Core.Properties.Resources.ObjectSerializer_ByteArrayToObject_DeserializationFailed, ex);
            }
        }
    }
}
