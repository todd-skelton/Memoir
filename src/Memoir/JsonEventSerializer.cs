using System;
using System.Text.Json.Serialization;

namespace Memoir
{
    public class JsonEventSerializer : IEventSerializer
    {
        public T Deserialize<T>(string serializedObject) => JsonSerializer.Parse<T>(serializedObject);

        public object Deserialize(string serializedObject, Type type) => JsonSerializer.Parse(serializedObject, type);

        public string Serialize<T>(T @object) => JsonSerializer.ToString(@object);

        public string Serialize(object @object, Type type) => JsonSerializer.ToString(@object, type);
    }
}
