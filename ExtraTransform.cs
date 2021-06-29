using FoxKit.Utils;
using FoxLib;
using System.IO;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

namespace GrxArrayTool
{
    public class ExtraTransform
    {
        public Vector3 Scale { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Translation { get; set; }

        public virtual void Read(BinaryReader reader)
        {
            Scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Rotation = FoxUtils.FoxToUnity(new Core.Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            Translation = new Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(Scale.x); writer.Write(Scale.y); writer.Write(Scale.z);
            Core.Quaternion newQuat = FoxUtils.UnityToFox(Rotation);
            writer.Write(newQuat.X); writer.Write(newQuat.Y); writer.Write(newQuat.Z); writer.Write(newQuat.W);
            writer.Write(-Translation.x); writer.Write(Translation.y); writer.Write(Translation.z);
        }
        public virtual void Log()
        {
        }
    }
}
