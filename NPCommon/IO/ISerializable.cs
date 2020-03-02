using System.IO;

namespace NPCommon.IO
{
    public interface ISerializable
    {
        ushort Size { get; }

        void Serialize(BinaryWriter writer);
        void Deserialize(BinaryReader reader);
    }
}
