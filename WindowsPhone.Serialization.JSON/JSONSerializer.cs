using Newtonsoft.Json;
using WindowsPhone.Contracts.Logging;
using WindowsPhone.Contracts.Serialization;

namespace WindowsPhone.Serialization.JSON
{
    public class JSONSerializer : ISerialize
    {
        private ILog _log;
        public JSONSerializer(ILog log)
        {
            _log = log;
        }
        public T Deserialize<T>(byte[] Data)
        {
            _log.Info(Data);
            return Deserialize<T>(System.Text.Encoding.UTF8.GetString(Data, 0, Data.Length));
        }     
        public T Deserialize<T>(string Data)
        {
            return JsonConvert.DeserializeObject<T>(Data);
        }

        public string Serialize<T>(T Object)
        {
            return JsonConvert.SerializeObject(Object);
        }
    }
}
