using System;

namespace Memoir
{
    public interface IEventSerializer
    {
        T Deserialize<T>(string serializedObject);
        object Deserialize(string serializedObject, Type type);
        string Serialize<T>(T @object);
        string Serialize(object @object, Type type);
    }
}
