using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMate.Common
{
    public class SerializerUtil
    {
        public static byte[] Serialize<T>(T objectSerialize)
        {
            MemoryStream memorystream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(memorystream, objectSerialize);
            memorystream.Flush();
            byte[] bytesData = memorystream.ToArray();
            return bytesData;
        }

        public static object Deserialize(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream memorystream = new MemoryStream(data);
            object objDeserialized = bf.Deserialize(memorystream);
            return objDeserialized;
        }
    }
}
