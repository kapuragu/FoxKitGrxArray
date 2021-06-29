﻿using System;
using System.Collections.Generic;
using System.IO;
using Vector3 = UnityEngine.Vector3;
using Color = UnityEngine.Color;
using Quaternion = UnityEngine.Quaternion;
using FoxKit.Utils;
using FoxLib;
using UnityEngine;

namespace GrxArrayTool
{
    public class ComponentSpotLight : MonoBehaviour
    {
        public uint vals4_2;
        public uint LightFlags;
        public uint vals4_4;
        public float OuterRange;
        public float InnerRange;
        public float UmbraAngle;
        public float PenumbraAngle;
        public float AttenuationExponent;
        public float vals14_6;
        public Color Color;
        public float Temperature;
        public float ColorDeflection; // inconsistency with pointlight having it as a float makes me doubt this is colordeflection too
        public float Lumen;
        public float vals10;
        public float ShadowUmbraAngle;
        public float ShadowPenumbraAngle;
        public float ShadowAttenuationExponent;
        public float Dimmer;
        public float ShadowBias;
        public float ViewBias;
        public float vals11_1;
        public float vals11_2;
        public float vals11_3;
        public uint LodRadiusLevel;
        public uint vals12_2;
        void OnDrawGizmos()
        {}
    }

    public class LightTypeSpotLight
    {
        public ulong HashName { get; set; }
        public string StringName { get; set; }
        public uint vals4_2 { get; set; } // Different in GZ
        public uint LightFlags { get; set; }
        public uint vals4_4 { get; set; } // Sometimes different in GZ?
        public Vector3 Translation { get; set; }
        public Vector3 ReachPoint { get; set; }
        public Quaternion Rotation { get; set; }
        public float OuterRange { get; set; }
        public float InnerRange { get; set; }
        public float UmbraAngle { get; set; }
        public float PenumbraAngle { get; set; }
        public float AttenuationExponent { get; set; }
        public float vals14_6 { get; set; }
        public Color Color { get; set; }
        public float Temperature { get; set; }
        public float ColorDeflection { get; set; } // inconsistency with pointlight having it as a float makes me doubt this is colordeflection too
        public float Lumen { get; set; }
        public float vals10 { get; set; }
        public float ShadowUmbraAngle { get; set; }
        public float ShadowPenumbraAngle { get; set; }
        public float ShadowAttenuationExponent { get; set; }
        public float Dimmer { get; set; }
        public float ShadowBias { get; set; }
        public float ViewBias { get; set; }
        public float vals11_1 { get; set; }
        public float vals11_2 { get; set; }
        public float vals11_3 { get; set; }
        public uint LodRadiusLevel { get; set; }
        public uint vals12_2 { get; set; }
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

            ReachPoint = new Vector3(-reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            Rotation = FoxUtils.FoxToUnity(new Core.Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            Rotation *= Quaternion.Euler(new Vector3(90, 0, 0));

            OuterRange = Half.ToHalf(reader.ReadUInt16());
            InnerRange = Half.ToHalf(reader.ReadUInt16());
            UmbraAngle = Half.ToHalf(reader.ReadUInt16());
            PenumbraAngle = Half.ToHalf(reader.ReadUInt16());
            AttenuationExponent = Half.ToHalf(reader.ReadUInt16());
            vals14_6 = Half.ToHalf(reader.ReadUInt16());

            Color = new Color(
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16()),
                Half.ToHalf(reader.ReadUInt16())
            );

            Temperature = Half.ToHalf(reader.ReadUInt16());
            ColorDeflection = Half.ToHalf(reader.ReadUInt16());
            Lumen = reader.ReadSingle();
            vals10 = Half.ToHalf(reader.ReadUInt16());
            ShadowUmbraAngle = Half.ToHalf(reader.ReadUInt16());
            ShadowPenumbraAngle = Half.ToHalf(reader.ReadUInt16());
            ShadowAttenuationExponent = Half.ToHalf(reader.ReadUInt16());
            Dimmer = Half.ToHalf(reader.ReadUInt16());
            ShadowBias = Half.ToHalf(reader.ReadUInt16());
            ViewBias = Half.ToHalf(reader.ReadUInt16());
            vals11_1 = Half.ToHalf(reader.ReadUInt16());
            vals11_2 = Half.ToHalf(reader.ReadUInt16());
            vals11_3 = Half.ToHalf(reader.ReadUInt16());
            LodRadiusLevel = reader.ReadUInt32();
            vals12_2 = reader.ReadUInt32();

            uint offsetToIrraditationTransform = reader.ReadUInt32();

            StringName = string.Empty;
            //PS3 files don't use strings for light objects (however there's no way to tell byte sex apart so tool won't parse them anyway)
            if (offsetToString > 0)
            {
                StringName = reader.ReadCString();
                if (reader.BaseStream.Position % 0x4 != 0)
                    reader.BaseStream.Position += 0x4 - reader.BaseStream.Position % 0x4;
            }

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
            int offsetToTransforms = 0x78;
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

            writer.Write(ReachPoint.x);
            writer.Write(ReachPoint.y);
            writer.Write(ReachPoint.z);

            Core.Quaternion newQuat = FoxUtils.UnityToFox(Rotation * Quaternion.Euler(new Vector3(-90, 0, 0)));
            writer.Write(newQuat.X); writer.Write(newQuat.Y); writer.Write(newQuat.Z); writer.Write(newQuat.W);

            writer.Write(Half.GetBytes((Half)OuterRange));
            writer.Write(Half.GetBytes((Half)InnerRange));
            writer.Write(Half.GetBytes((Half)UmbraAngle));
            writer.Write(Half.GetBytes((Half)PenumbraAngle));
            writer.Write(Half.GetBytes((Half)AttenuationExponent));
            writer.Write(Half.GetBytes((Half)vals14_6));

            writer.Write(Half.GetBytes(-(Half)Color.r));
            writer.Write(Half.GetBytes(-(Half)Color.g));
            writer.Write(Half.GetBytes(-(Half)Color.b));
            writer.Write(Half.GetBytes(-(Half)Color.a));

            writer.Write(Half.GetBytes((Half)Temperature));
            writer.Write(Half.GetBytes((Half)ColorDeflection));
            writer.Write(Lumen);
            writer.Write(Half.GetBytes((Half)vals10));
            writer.Write(Half.GetBytes((Half)ShadowUmbraAngle));
            writer.Write(Half.GetBytes((Half)ShadowPenumbraAngle));
            writer.Write(Half.GetBytes((Half)ShadowAttenuationExponent));
            writer.Write(Half.GetBytes((Half)Dimmer));
            writer.Write(Half.GetBytes((Half)ShadowBias));
            writer.Write(Half.GetBytes((Half)ViewBias));
            writer.Write(Half.GetBytes((Half)vals11_1));
            writer.Write(Half.GetBytes((Half)vals11_2));
            writer.Write(Half.GetBytes((Half)vals11_3));
            writer.Write(LodRadiusLevel);
            writer.Write(vals12_2);

            if (IrradiationPoint.Count > 0)
                writer.Write((offsetToTransforms + 0x28) - 0x74);
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

            foreach (var irradiationpoint in IrradiationPoint)
            {
                irradiationpoint.Write(writer);
            }

            Log();
        }
        public void Log()
        {
            Console.WriteLine($"Spotlight entry StrCode64={HashName} StringName='{StringName}'");
            Console.WriteLine($"    vals4_2={vals4_2} LightFlags={LightFlags} vals4_4={vals4_4}");
            Console.WriteLine($"    Translation X={Translation.x} Y={Translation.y} Z={Translation.z}");
            Console.WriteLine($"    ReachPoint X={ReachPoint.x} Y={ReachPoint.y} Z={ReachPoint.z}");
            Console.WriteLine($"    Rotation X={Rotation.x} Y={Rotation.y} Z={Rotation.z} W={Rotation.w}");
            Console.WriteLine($"    OuterRange={OuterRange} InnerRange={InnerRange}");
            Console.WriteLine($"    UmbraAngle={UmbraAngle} PenumbraAngle={PenumbraAngle}");
            Console.WriteLine($"    AttenuationExponent={AttenuationExponent} vals14_6={vals14_6}");
            Console.WriteLine($"    Color X={Color.r} Y={Color.g} Z={Color.b} W={Color.a}");
            Console.WriteLine($"    Temperature={Temperature} ColorDeflection={ColorDeflection} Lumen={Lumen} vals10={vals10}");
            Console.WriteLine($"    ShadowUmbraAngle={ShadowUmbraAngle} ShadowPenumbraAngle={ShadowPenumbraAngle} ");
            Console.WriteLine($"    Dimmer={Dimmer} ShadowBias={ShadowBias} ViewBias={ViewBias}");
            Console.WriteLine($"    vals11_1={vals11_1} vals11_2={vals11_2} vals11_3={vals11_3}");
            Console.WriteLine($"    LodRadiusLevel={LodRadiusLevel} vals12_2={vals12_2} vals11_3={vals11_3}");

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