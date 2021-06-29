using System;
using System.IO;
using Vector4 = UnityEngine.Vector4;

namespace GrxArrayTool
{
    public class LightTypeOccluder
    {
        public uint valsOcc_1 { get; set; } // Different in GZ
        public Vector4[] Node { get; set; }
        public struct Face
        {
            public short value1 { get; set; }
            public short value2 { get; set; }
            public short VertexIndex { get; set; }
            public short Size { get; set; }
        }
        public Face[] Faces { get; set; }
        public void Read(BinaryReader reader)
        {
            valsOcc_1 = reader.ReadUInt32();
            reader.BaseStream.Position += 4;
            uint facesCount = reader.ReadUInt32();
            reader.BaseStream.Position += 4;
            uint nodesCount = reader.ReadUInt32();
            Console.WriteLine($"Occluder entry");
            Console.WriteLine($"    valsOcc_1={valsOcc_1}");
            Console.WriteLine($"    edgesCount={facesCount}");
            Console.WriteLine($"    nodesCount={nodesCount}");
            Node = new Vector4[nodesCount];
            for (int i = 0; i < nodesCount; i++)
            {
                Node[i] = new Vector4(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                Console.WriteLine($"    Node#{i} X={Node[i].x}, Y={Node[i].y}, Z={Node[i].z}, W={Node[i].w}");
            }
            Faces = new Face[facesCount];
            for (int i = 0; i < facesCount; i++)
            {
                Faces[i].value1 = reader.ReadInt16();
                Faces[i].value2 = reader.ReadInt16();
                Faces[i].VertexIndex = reader.ReadInt16();
                Faces[i].Size = reader.ReadInt16();
                Console.WriteLine($"    Face#{i} value1={Faces[i].value1}, value2={Faces[i].value2}, VertexIndex={Faces[i].VertexIndex}, Size={Faces[i].Size}");
            }
        }
        public void Write(BinaryWriter writer)
        {
            writer.Write(valsOcc_1);
            int nodeCount = Node.Length;
            writer.Write(0x10 * (nodeCount + 1));
            writer.Write(Faces.Length);
            writer.Write(8);
            writer.Write(nodeCount);
            for (int i=0; i<nodeCount;i++)
            {
                writer.Write(-Node[i].x); writer.Write(Node[i].y); writer.Write(Node[i].z);
            }
            for (int i = 0; i < Faces.Length; i++)
            {
                writer.Write(Faces[i].value1);
                writer.Write(Faces[i].value2);
                writer.Write(Faces[i].VertexIndex);
                writer.Write(Faces[i].Size);
            }
        }
    }
}
