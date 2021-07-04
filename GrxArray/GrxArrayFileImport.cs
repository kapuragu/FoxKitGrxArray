using UnityEngine;
using System.IO;
using UnityEditor;

namespace FoxKit.Modules.GrxArrayTool
{
	public class GrxArrayFileImport : MonoBehaviour
	{
		[MenuItem("FoxKit/Import Lighting/Import GrxArray")]
		private static void OnImportAsset()
		{
			var assetPath = EditorUtility.OpenFilePanel("Import asset", "", "grxla");
			if (string.IsNullOrEmpty(assetPath))
			{
				return;
			}

			// You know the drill
			GrxArrayFile file = ReadFromBinary(assetPath);

			var array = new GameObject
			{
				name = Path.GetFileNameWithoutExtension(assetPath)
			};

			if (file.PointLights.Count > 0 || file.SpotLights.Count > 0)
			{
				var arrayComponent = array.AddComponent<ComponentLightArray>();
				arrayComponent.DataSetNameHash = file.DataSetNameHash;
				arrayComponent.DataSetPath = file.DataSetPath;

				foreach (var pointLight in file.PointLights)
				{
					Debug.Log(pointLight.StringName);
					var entry = new GameObject
					{
						name = pointLight.StringName
					};
					entry.transform.position = pointLight.Translation;
					entry.transform.parent = array.transform;

					var lightComponent = entry.AddComponent<Light>();
					lightComponent.type = LightType.Point;
					lightComponent.color = pointLight.Color;

					var component = entry.AddComponent<ComponentPointLight>();
					component.Flags1 = pointLight.Flags1;
					component.LightFlags = pointLight.LightFlags;
					component.Flags2 = pointLight.Flags2;
					component.ReachPoint = pointLight.ReachPoint;
					component.Color = pointLight.Color;
					component.Temperature = pointLight.Temperature;
					component.ColorDeflection = pointLight.ColorDeflection;
					component.Lumen = pointLight.Lumen;
					component.LightSize = pointLight.LightSize;
					component.Dimmer = pointLight.Dimmer;
					component.ShadowBias = pointLight.ShadowBias;
					component.LodFarSize = pointLight.LodFarSize;
					component.LodNearSize = pointLight.LodNearSize;
					component.LodShadowDrawRate = pointLight.LodShadowDrawRate;
					component.LodRadiusLevel = pointLight.LodRadiusLevel;
					component.LodFadeType = pointLight.LodFadeType;

					var la = pointLight.LightArea;
					if (la != null)
					{
						var lightArea = new GameObject
						{
							name = entry.name + "_LA"
						};
						var lightAreaComponent = lightArea.AddComponent<ComponentLightArea>();
						lightArea.transform.position = la.Translation;
						lightArea.transform.rotation = la.Rotation;
						lightArea.transform.localScale = la.Scale;
						lightArea.transform.parent = entry.transform;
						component.LightArea = lightAreaComponent;
					}
					else
					{
						component.LightArea = null;
					}

					var ip = pointLight.IrradiationPoint;
					if (ip != null)
					{
						var lightArea = new GameObject
						{
							name = entry.name + "_IP"
						};
						var irradiationPointComponent = lightArea.AddComponent<ComponentIrradiationPoint>();
						lightArea.transform.position = ip.Translation;
						lightArea.transform.rotation = ip.Rotation;
						lightArea.transform.localScale = ip.Scale;
						lightArea.transform.parent = entry.transform;
						component.IrradiationPoint = irradiationPointComponent;
					}
					else
					{
						component.IrradiationPoint = null;
					}

					arrayComponent.PointLights.Add(component);
				}

				foreach (var spotLight in file.SpotLights)
				{
					Debug.Log(spotLight.StringName);
					var entry = new GameObject
					{
						name = spotLight.StringName
					};
					entry.transform.position = spotLight.Translation;
					entry.transform.rotation = spotLight.Rotation;
					entry.transform.parent = array.transform;

					var lightComponent = entry.AddComponent<Light>();
					lightComponent.type = LightType.Spot;
					lightComponent.color = spotLight.Color;
					lightComponent.spotAngle = spotLight.UmbraAngle;
					lightComponent.shadowAngle = spotLight.ShadowUmbraAngle;
					lightComponent.range = spotLight.OuterRange;

					var component = entry.AddComponent<ComponentSpotLight>();
					component.Flags1 = spotLight.Flags1;
					component.LightFlags = spotLight.LightFlags;
					component.Flags2 = spotLight.Flags2;
					component.ReachPoint = spotLight.ReachPoint;
					component.OuterRange = spotLight.OuterRange;
					component.InnerRange = spotLight.InnerRange;
					component.UmbraAngle = spotLight.UmbraAngle;
					component.PenumbraAngle = spotLight.PenumbraAngle;
					component.AttenuationExponent = spotLight.AttenuationExponent;
					component.Color = spotLight.Color;
					component.Temperature = spotLight.Temperature;
					component.ColorDeflection = spotLight.ColorDeflection;
					component.Lumen = spotLight.Lumen;
					component.LightSize = spotLight.LightSize;
					component.ShadowUmbraAngle = spotLight.ShadowUmbraAngle;
					component.ShadowPenumbraAngle = spotLight.ShadowPenumbraAngle;
					component.ShadowAttenuationExponent = spotLight.ShadowAttenuationExponent;
					component.ShadowBias = spotLight.ShadowBias;
					component.ViewBias = spotLight.ViewBias;
					component.PowerScale = spotLight.PowerScale;
					component.LodFarSize = spotLight.LodFarSize;
					component.LodNearSize = spotLight.LodNearSize;
					component.LodShadowDrawRate = spotLight.LodShadowDrawRate;
					component.LodRadiusLevel = spotLight.LodRadiusLevel;
					component.LodFadeType = spotLight.LodFadeType;
					var la = spotLight.LightArea;
					if (la != null)
					{
						var lightArea = new GameObject
						{
							name = entry.name + "_LA"
						};
						var lightAreaComponent = lightArea.AddComponent<ComponentLightArea>();
						lightArea.transform.position = la.Translation;
						lightArea.transform.rotation = la.Rotation;
						lightArea.transform.localScale = la.Scale;
						lightArea.transform.parent = entry.transform;
						component.LightArea = lightAreaComponent;
					}
					else
					{
						component.LightArea = null;
					}

					var ip = spotLight.IrradiationPoint;
					if (ip != null)
					{
						var lightArea = new GameObject
						{
							name = entry.name + "_IP"
						};
						var irradiationPointComponent = lightArea.AddComponent<ComponentIrradiationPoint>();
						lightArea.transform.position = ip.Translation;
						lightArea.transform.rotation = ip.Rotation;
						lightArea.transform.localScale = ip.Scale;
						lightArea.transform.parent = entry.transform;
						component.IrradiationPoint = irradiationPointComponent;
					}
					else
					{
						component.IrradiationPoint = null;
					}

					arrayComponent.SpotLights.Add(component);
				}

			}
			else if (file.LightProbes.Count > 0)
			{
				var arrayComponent = array.AddComponent<ComponentLightProbeArray>();
				arrayComponent.DataSetNameHash = file.DataSetNameHash;
				arrayComponent.DataSetPath = file.DataSetPath;

				// Proof that it worked
				foreach (var lightProbe in file.LightProbes)
				{
					// Log the name to Unity's console
					Debug.Log(lightProbe.StringName);
					var entry = new GameObject
					{
						name = lightProbe.StringName
					};

					//var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					//cube.transform.parent = entry.transform;
					//cube.GetComponent<Renderer>().sharedMaterial.color = new Color(0, 1, 0, 0.5F);

					entry.transform.position = lightProbe.Translation;

					entry.transform.rotation = lightProbe.Rotation;

					entry.transform.localScale = lightProbe.Scale;
					entry.transform.parent = array.transform;

					var component = entry.AddComponent<ComponentLightProbe>();
					component.vals4_2 = lightProbe.vals4_2;
					component.LightFlags = lightProbe.LightFlags;
					component.vals4_4 = lightProbe.vals4_4;
					component.InnerScalePositive = new Vector3(lightProbe.InnerScaleXNegative, lightProbe.InnerScaleYPositive, lightProbe.InnerScaleZPositive);
					component.InnerScaleNegative = new Vector3(lightProbe.InnerScaleXPositive, lightProbe.InnerScaleYNegative, lightProbe.InnerScaleZNegative);
					component.vals16 = lightProbe.vals16;
					component.Priority = lightProbe.Priority;
					component.ShapeType = lightProbe.ShapeType;
					component.RelatedLightIndex = lightProbe.RelatedLightIndex;
					component.SHDataIndex = lightProbe.SHDataIndex;
					component.LightSize = lightProbe.LightSize;
					component.u5 = lightProbe.u5;

					arrayComponent.LightProbes.Add(component);
				}
			}
		}
		[MenuItem("FoxKit/Export Lighting/Export GrxArray")]
		private static void OnExportAsset()
		{
			GameObject obj = Selection.activeGameObject;
			if (obj == null)
				return;

			ComponentLightArray lightArrayComponent = obj.GetComponent(typeof(ComponentLightArray)) as ComponentLightArray;
			if (lightArrayComponent != null)
			{
				GrxArrayFile file = new GrxArrayFile();
				file.DataSetNameHash = lightArrayComponent.DataSetNameHash;
				file.DataSetPath = lightArrayComponent.DataSetPath;
				foreach (ComponentPointLight point in lightArrayComponent.PointLights)
				{
					var writePoint = new LightTypePointLight();
					writePoint.StringName = point.gameObject.name;
					writePoint.Flags1 = point.Flags1;
					writePoint.LightFlags = point.LightFlags;
					writePoint.Flags2 = point.Flags2;
					writePoint.Translation = point.transform.position;
					writePoint.ReachPoint = point.ReachPoint;
					writePoint.Color = point.Color;
					writePoint.Temperature = point.Temperature;
					writePoint.ColorDeflection = point.ColorDeflection;
					writePoint.Lumen = point.Lumen;
					writePoint.LightSize = point.LightSize;
					writePoint.Dimmer = point.Dimmer;
					writePoint.ShadowBias = point.ShadowBias;
					writePoint.LodFarSize = point.LodFarSize;
					writePoint.LodNearSize = point.LodNearSize;
					writePoint.LodShadowDrawRate = point.LodShadowDrawRate;
					writePoint.LodRadiusLevel = point.LodRadiusLevel;
					writePoint.LodFadeType = point.LodFadeType;

					if (point.LightArea == null)
						writePoint.LightArea = null;
					else
					{
						writePoint.LightArea = new ExtraTransform();
						writePoint.LightArea.Translation = point.LightArea.transform.position;
						writePoint.LightArea.Rotation = point.LightArea.transform.rotation;
						writePoint.LightArea.Scale = point.LightArea.transform.localScale;
					}
					if (point.IrradiationPoint == null)
						writePoint.IrradiationPoint = null;
					else
					{
						writePoint.IrradiationPoint = new ExtraTransform();
						writePoint.IrradiationPoint.Translation = point.IrradiationPoint.transform.position;
						writePoint.IrradiationPoint.Rotation = point.IrradiationPoint.transform.rotation;
						writePoint.IrradiationPoint.Scale = point.IrradiationPoint.transform.localScale;
					}

					file.PointLights.Add(writePoint);
				}
				foreach (ComponentSpotLight spot in lightArrayComponent.SpotLights)
				{
					var writeSpot = new LightTypeSpotLight();
					writeSpot.StringName = spot.gameObject.name;
					writeSpot.Flags1 = spot.Flags1;
					writeSpot.LightFlags = spot.LightFlags;
					writeSpot.Flags2 = spot.Flags2;
					writeSpot.Translation = spot.transform.position;
					writeSpot.ReachPoint = spot.ReachPoint;
					writeSpot.Rotation = spot.transform.rotation;
					writeSpot.OuterRange = spot.OuterRange;
					writeSpot.InnerRange = spot.InnerRange;
					writeSpot.UmbraAngle = spot.UmbraAngle;
					writeSpot.PenumbraAngle = spot.PenumbraAngle;
					writeSpot.AttenuationExponent = spot.AttenuationExponent;
					writeSpot.Dimmer = spot.Dimmer;
					writeSpot.Color = spot.Color;
					writeSpot.Temperature = spot.Temperature;
					writeSpot.ColorDeflection = spot.ColorDeflection;
					writeSpot.Lumen = spot.Lumen;
					writeSpot.LightSize = spot.LightSize;
					writeSpot.ShadowUmbraAngle = spot.ShadowUmbraAngle;
					writeSpot.ShadowPenumbraAngle = spot.ShadowPenumbraAngle;
					writeSpot.ShadowAttenuationExponent = spot.ShadowAttenuationExponent;
					writeSpot.ShadowBias = spot.ShadowBias;
					writeSpot.ViewBias = spot.ViewBias;
					writeSpot.PowerScale = spot.PowerScale;
					writeSpot.LodFarSize = spot.LodFarSize;
					writeSpot.LodNearSize = spot.LodNearSize;
					writeSpot.LodShadowDrawRate = spot.LodShadowDrawRate;
					writeSpot.LodRadiusLevel = spot.LodRadiusLevel;
					writeSpot.LodFadeType = spot.LodFadeType;

					if (spot.LightArea == null)
						writeSpot.LightArea = null;
					else
					{
						writeSpot.LightArea = new ExtraTransform();
						writeSpot.LightArea.Translation = spot.LightArea.transform.position;
						writeSpot.LightArea.Rotation = spot.LightArea.transform.rotation;
						writeSpot.LightArea.Scale = spot.LightArea.transform.localScale;
					}
					if (spot.IrradiationPoint == null)
						writeSpot.IrradiationPoint = null;
					else
					{
						writeSpot.IrradiationPoint = new ExtraTransform();
						writeSpot.IrradiationPoint.Translation = spot.IrradiationPoint.transform.position;
						writeSpot.IrradiationPoint.Rotation = spot.IrradiationPoint.transform.rotation;
						writeSpot.IrradiationPoint.Scale = spot.IrradiationPoint.transform.localScale;
					}

					file.SpotLights.Add(writeSpot);
				}

				var assetPath = EditorUtility.SaveFilePanel("Export asset", "", lightArrayComponent.gameObject.name, "grxla");
				if (string.IsNullOrEmpty(assetPath))
					return;

				using (BinaryWriter writer = new BinaryWriter(new FileStream(assetPath, FileMode.Create)))
				{
					file.Write(writer);
				}
			}

			ComponentLightProbeArray probeArrayComponent = obj.GetComponent(typeof(ComponentLightProbeArray)) as ComponentLightProbeArray;
			if (probeArrayComponent != null)
			{
				GrxArrayFile file = new GrxArrayFile();
				file.DataSetNameHash = probeArrayComponent.DataSetNameHash;
				file.DataSetPath = probeArrayComponent.DataSetPath;
				foreach (ComponentLightProbe probe in probeArrayComponent.LightProbes)
                {
					var writeProbe = new LightTypeLightProbe();
					writeProbe.StringName = probe.gameObject.name;
					writeProbe.vals4_2 = probe.vals4_2;
					writeProbe.LightFlags = probe.LightFlags;
					writeProbe.vals4_4 = probe.vals4_4;
					writeProbe.InnerScaleXPositive = probe.InnerScaleNegative.x;
					writeProbe.InnerScaleYPositive = probe.InnerScalePositive.y;
					writeProbe.InnerScaleZPositive = probe.InnerScalePositive.z;
					writeProbe.InnerScaleXNegative = probe.InnerScalePositive.x;
					writeProbe.InnerScaleYNegative = probe.InnerScaleNegative.y;
					writeProbe.InnerScaleZNegative = probe.InnerScaleNegative.z;
					writeProbe.Scale = probe.gameObject.transform.localScale;
					writeProbe.Rotation = probe.gameObject.transform.rotation;
					writeProbe.Translation = probe.gameObject.transform.position;
					writeProbe.vals16 = probe.vals16;
					writeProbe.Priority = probe.Priority;
					writeProbe.ShapeType = probe.ShapeType;
					writeProbe.RelatedLightIndex = probe.RelatedLightIndex;
					writeProbe.SHDataIndex = probe.SHDataIndex;
					writeProbe.LightSize = probe.LightSize;
					writeProbe.u5 = probe.u5;

					file.LightProbes.Add(writeProbe);
				}

				var assetPath = EditorUtility.SaveFilePanel("Export asset", "", probeArrayComponent.gameObject.name, "grxla");
				if (string.IsNullOrEmpty(assetPath))
					return;

				using (BinaryWriter writer = new BinaryWriter(new FileStream(assetPath, FileMode.Create)))
				{
					file.Write(writer);
				}
			}

		}
		public static GrxArrayFile ReadFromBinary(string path)
		{
			GrxArrayFile file = new GrxArrayFile();
			using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open)))
			{
				file.Read(reader);
			}
			return file;
		}
	}
}
