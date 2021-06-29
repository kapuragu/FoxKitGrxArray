using System;
using System.Collections.Generic;
using System.IO;
using Vector3 = UnityEngine.Vector3;
using Color = UnityEngine.Color;
using UnityEngine;
using UnityEditor;

namespace GrxArrayTool
{
    public class ComponentLightArea : MonoBehaviour
    {
        void OnDrawGizmosSelected()
        {
            DrawShape(Color.yellow + new Color(0.25f, 0.25f, 0.25f, 0.5f), new Color(1, 1, 0.5F, 0.25f));
        }
        void OnDrawGizmos()
        {
            DrawShape(Color.yellow, new Color(1, 1, 0, 0.25f));
        }
        void DrawShape(Color colorHard, Color colorSoft)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = colorHard;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.color = colorSoft;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
    public class ComponentIrradiationPoint : MonoBehaviour
    {
        void OnDrawGizmosSelected()
        {
            DrawShape(Color.red + new Color(0.25f, 0.25f, 0.25f, 1));
        }
        void OnDrawGizmos()
        {
            DrawShape(Color.red);
        }
        void DrawShape(Color colorHard)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = colorHard;
            Gizmos.DrawLine(Vector3.down, Vector3.up);
            Gizmos.DrawLine(Vector3.right, Vector3.left);
            Gizmos.DrawLine(Vector3.back, Vector3.forward);
            Handles.Label(transform.position, " IRRADIATION");
        }
    }
    public class ComponentReachPoint : MonoBehaviour
    {
        void OnDrawGizmosSelected()
        {
            DrawShape(Color.blue + new Color(0.25f, 0.25f, 0.25f, 1));
        }
        void OnDrawGizmos()
        {
            DrawShape(Color.blue);
        }
        void DrawShape(Color colorHard)
        {
            Gizmos.color = colorHard;
            Gizmos.DrawLine(transform.position + Vector3.down, transform.position + Vector3.up);
            Gizmos.DrawLine(transform.position + Vector3.right, transform.position + Vector3.left);
            Gizmos.DrawLine(transform.position + Vector3.back, transform.position + Vector3.forward);
            Handles.Label(transform.position, " REACH");
        }
    }
    public class ComponentPointLight : MonoBehaviour
    {
        public uint vals4_2;
        public uint LightFlags;
        public uint vals4_4;
        public Color Color;
        public float Temperature;
        public float ColorDeflection;
        public float Lumen;
        public float vals5_3;
        public float vals5_4;
        public float vals3_1;
        public float vals3_2;
        public float vals6;
        public float vals13;
        public uint vals7_1;
        public uint vals7_2;
        void OnDrawGizmos()
        {}
    }

    public class LightTypePointLight
    {
        public ulong HashName { get; set; }
        public string StringName { get; set; }
        public uint vals4_2 { get; set; } // Different in GZ
        public uint LightFlags { get; set; }
        public uint vals4_4 { get; set; } // Sometimes different in GZ?
        public Vector3 Translation { get; set; }
        public Vector3 ReachPoint { get; set; }
        public Color Color { get; set; }
        public float Temperature { get; set; }
        public float ColorDeflection { get; set; }
        public float Lumen { get; set; }
        public float vals5_3 { get; set; }
        public float vals5_4 { get; set; }
        public float vals3_1 { get; set; }
        public float vals3_2 { get; set; }
        public float vals6 { get; set; }
        public float vals13 { get; set; }
        public uint vals7_1 { get; set; }
        public uint vals7_2 { get; set; }
        public List<ExtraTransform> LightArea = new List<ExtraTransform>();
        public List<ExtraTransform> IrradiationPoint = new List<ExtraTransform>();
        public void Read(BinaryReader reader)
        {
            HashName = reader.ReadUInt64(); //Doesn't look like the PathCode64 of the .fox2?
            uint offsetToString = reader.ReadUInt32();
            vals4_2 = reader.ReadUInt32();
            LightFlags = reader.ReadUInt32();
            vals4_4 = reader.ReadUInt32();
            uint offsetToLightArea = reader.ReadUInt32();

            Translation = new Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            ReachPoint = new Vector3(
                -(Half.ToHalf(reader.ReadUInt16())),
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16())
            );

            Color = new Color(
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16())
            );

            Temperature = Half.ToHalf(reader.ReadUInt16());
            ColorDeflection = reader.ReadSingle();
            Lumen = reader.ReadSingle();
            vals5_3 = Half.ToHalf(reader.ReadUInt16());
            vals5_4 = Half.ToHalf(reader.ReadUInt16());
            vals3_1 = Half.ToHalf(reader.ReadUInt16());
            vals3_2 = Half.ToHalf(reader.ReadUInt16());
            vals6 = Half.ToHalf(reader.ReadUInt16());
            vals13 = Half.ToHalf(reader.ReadUInt16());
            vals7_1 = reader.ReadUInt32();
            vals7_2 = reader.ReadUInt32();
            uint offsetToIrraditationTransform = reader.ReadUInt32();

            StringName = string.Empty;
            //PS3 files don't use strings for light objects (however there's no way to tell byte sex apart so tool won't parse them anyway)
            if (offsetToString > 0)
            {
                StringName = reader.ReadCString();
                if (reader.BaseStream.Position % 0x4 != 0)
                    reader.BaseStream.Position += 0x4 - reader.BaseStream.Position % 0x4;
            }

            Console.WriteLine($"Point light entry name: StrCode64={HashName} StringName='{StringName}'");
            Console.WriteLine($"    vals4_2={vals4_2} LightFlags={LightFlags} vals4_4={vals4_4}");
            Console.WriteLine($"    Translation X={Translation.x} Y={Translation.y} Z={Translation.z}");
            Console.WriteLine($"    ReachPoint X={ReachPoint.x} Y={ReachPoint.y} Z={ReachPoint.z}");
            Console.WriteLine($"    Color X={Color.r} Y={Color.g} Z={Color.b} W={Color.a}");
            Console.WriteLine($"    Temperature={Temperature} ColorDeflection={ColorDeflection} Lumen={Lumen}");
            Console.WriteLine($"    vals5_3={vals5_3} vals5_4={vals5_4} vals3_1={vals3_1}");
            Console.WriteLine($"    vals3_2={vals3_2} vals6={vals6} vals13={vals13}");
            Console.WriteLine($"    vals7_1={vals7_1} vals7_2={vals7_2}");

            ExtraTransform LightAreaTrasform = new ExtraTransform();
            if (offsetToLightArea > 0)
            {
                LightAreaTrasform.Read(reader);
                LightArea.Add(LightAreaTrasform);
            }
            ExtraTransform IrradiationPointTransform = new ExtraTransform();
            if (offsetToIrraditationTransform > 0)
            {
                IrradiationPointTransform.Read(reader);
                IrradiationPoint.Add(IrradiationPointTransform);
            }

            Log();
        }
        public void Write(BinaryWriter writer)
        {
            int offsetToTransforms = 0x50;
            if (StringName != string.Empty)
            {
                writer.Write(HashManager.StrCode64(StringName));
                writer.Write(offsetToTransforms);
                offsetToTransforms += StringName.Length + 1;
                if (offsetToTransforms % 0x4 != 0)
                    offsetToTransforms += (0x4 - offsetToTransforms % 0x4);
            }
            else
            {
                writer.Write(HashName);
                writer.Write(0);
            }
            writer.Write(vals4_2);
            writer.Write(LightFlags);
            writer.Write(vals4_4);
            if (LightArea.Count > 0)
                writer.Write(offsetToTransforms-0x10);
            else
                writer.Write(0);

            writer.Write(-Translation.x); writer.Write(Translation.y); writer.Write(Translation.z);
            writer.Write(Half.GetBytes(-(Half)ReachPoint.x)); 
            writer.Write(Half.GetBytes(-(Half)ReachPoint.y)); 
            writer.Write(Half.GetBytes(-(Half)ReachPoint.z));

            writer.Write(Half.GetBytes(-(Half)Color.r));
            writer.Write(Half.GetBytes(-(Half)Color.g));
            writer.Write(Half.GetBytes(-(Half)Color.b));
            writer.Write(Half.GetBytes(-(Half)Color.a));

            writer.Write(Half.GetBytes((Half)Temperature));
            writer.Write(ColorDeflection);
            writer.Write(Lumen);
            writer.Write(Half.GetBytes((Half)vals5_3));
            writer.Write(Half.GetBytes((Half)vals5_4));
            writer.Write(Half.GetBytes((Half)vals3_1));
            writer.Write(Half.GetBytes((Half)vals3_2));
            writer.Write(Half.GetBytes((Half)vals6));
            writer.Write(Half.GetBytes((Half)vals13));
            writer.Write(vals7_1);
            writer.Write(vals7_2);

            if (IrradiationPoint.Count > 0)
                writer.Write((offsetToTransforms + 0x28) - 0x4C);
            else
                writer.Write(0);

            if (StringName != string.Empty)
            {
                writer.WriteCString(StringName); writer.WriteZeroes(1);
                if (writer.BaseStream.Position % 0x4 != 0)
                    writer.WriteZeroes(0x4 - (int)writer.BaseStream.Position % 0x4);
            }

            foreach (var lightArea in LightArea)
            {
                lightArea.Write(writer);
            }

            foreach (var lightArea in IrradiationPoint)
            {
                lightArea.Write(writer);
            }

            Log();
        }
        public void Log()
        {
            Console.WriteLine($"Point light entry StrCode64={HashName} StringName='{StringName}'");
            Console.WriteLine($"    vals4_2={vals4_2} LightFlags={LightFlags} vals4_4={vals4_4}");
            Console.WriteLine($"    Translation X={Translation.x} Y={Translation.y} Z={Translation.z}");
            Console.WriteLine($"    ReachPoint X={ReachPoint.x} Y={ReachPoint.y} Z={ReachPoint.z}");
            Console.WriteLine($"    Color X={Color.r} Y={Color.g} Z={Color.b} W={Color.a}");
            Console.WriteLine($"    Temperature={Temperature} ColorDeflection={ColorDeflection} Lumen={Lumen}");
            Console.WriteLine($"    vals5_3={vals5_3} vals5_4={vals5_4} vals3_1={vals3_1}");
            Console.WriteLine($"    vals3_2={vals3_2} vals6={vals6} vals13={vals13}");
            Console.WriteLine($"    vals7_1={vals7_1} vals7_2={vals7_2}");
            foreach (var lightArea in LightArea)
            {
                Console.WriteLine("        LightArea");
                lightArea.Log();
            }
            foreach (var irradiationPoint in IrradiationPoint)
            {
                Console.WriteLine("        IrradiationPoint");
                irradiationPoint.Log();
            }
        }
    }
}
