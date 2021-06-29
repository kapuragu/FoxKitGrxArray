using UnityEngine;
using System.IO;
using UnityEditor;

namespace GrxArrayTool
{
	public class GrxArrayFileImport : MonoBehaviour
	{
		[MenuItem("Fox/Import GrxArray")]
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
				name = file.DataSetPath
			};

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
				component.vals4_2 = pointLight.vals4_2;
				component.LightFlags = pointLight.LightFlags;
				component.vals4_4 = pointLight.vals4_4;

				var reachPoint = new GameObject
				{
					name = entry.name + "_RP"
				};
				reachPoint.transform.parent = entry.transform;
				reachPoint.transform.position = entry.transform.position+pointLight.ReachPoint;
				var reachPointComponent = reachPoint.AddComponent<ComponentReachPoint>();

				component.Color = pointLight.Color;
				component.Temperature = pointLight.Temperature;
				component.ColorDeflection = pointLight.ColorDeflection;
				component.Lumen = pointLight.Lumen;
				component.vals5_3=pointLight.vals5_3;
				component.vals5_4 = pointLight.vals5_4;
				component.vals3_1 = pointLight.vals3_1;
				component.vals3_2 = pointLight.vals3_2;
				component.vals6 = pointLight.vals6;
				component.vals13 = pointLight.vals13;
				component.vals7_1 = pointLight.vals7_1;
				component.vals7_2 = pointLight.vals7_2;

				foreach (ExtraTransform area in pointLight.LightArea)
				{
					var lightArea = new GameObject
					{
						name = entry.name+"_LA"
					};
					var lightAreaComponent = lightArea.AddComponent<ComponentLightArea>();
					lightArea.transform.position = area.Translation;
					lightArea.transform.rotation = area.Rotation;
					lightArea.transform.localScale = area.Scale;
					lightArea.transform.parent = entry.transform;
				}

				foreach (ExtraTransform point in pointLight.IrradiationPoint)
				{
					var lightArea = new GameObject
					{
						name = entry.name + "_IP"
					};
					var irradiationPointComponent = lightArea.AddComponent<ComponentIrradiationPoint>();
					lightArea.transform.position = point.Translation;
					lightArea.transform.rotation = point.Rotation;
					lightArea.transform.localScale = point.Scale;
					lightArea.transform.parent = entry.transform;
				}
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

				var component = entry.AddComponent<ComponentSpotLight>();
				component.vals4_2 = spotLight.vals4_2;
				component.LightFlags = spotLight.LightFlags;
				component.vals4_4 = spotLight.vals4_4;

				var reachPoint = new GameObject
				{
					name = entry.name + "_ReachPoint"
				};
				reachPoint.transform.position = entry.transform.position + spotLight.ReachPoint;
				reachPoint.transform.parent = entry.transform;
				var reachPointComponent = reachPoint.AddComponent<ComponentReachPoint>();

				component.OuterRange = spotLight.OuterRange;
				component.InnerRange = spotLight.InnerRange;
				component.UmbraAngle = spotLight.UmbraAngle;
				component.PenumbraAngle = spotLight.PenumbraAngle;
				component.AttenuationExponent = spotLight.AttenuationExponent;
				component.Color = spotLight.Color;
				component.Temperature = spotLight.Temperature;
				component.ColorDeflection = spotLight.ColorDeflection;
				component.Lumen = spotLight.Lumen;
				component.vals10 = spotLight.vals10;
				component.ShadowUmbraAngle = spotLight.ShadowUmbraAngle;
				component.ShadowPenumbraAngle = spotLight.ShadowPenumbraAngle;
				component.ShadowAttenuationExponent = spotLight.ShadowAttenuationExponent;
				component.Dimmer = spotLight.Dimmer;
				component.ShadowBias = spotLight.ShadowBias;
				component.ViewBias = spotLight.ViewBias;
				component.vals11_1 = spotLight.vals11_1;
				component.vals11_2 = spotLight.vals11_2;
				component.vals11_3 = spotLight.vals11_3;
				component.LodRadiusLevel = spotLight.LodRadiusLevel;
				component.vals12_2 = spotLight.vals12_2;
				foreach (ExtraTransform area in spotLight.LightArea)
				{
					var lightArea = new GameObject
					{
						name = entry.name + "_LA"
					};
					var lightAreaComponent = lightArea.AddComponent<ComponentLightArea>();
					lightArea.transform.position = area.Translation;
					lightArea.transform.rotation = area.Rotation;
					lightArea.transform.localScale = area.Scale;
					lightArea.transform.parent = entry.transform;
				}

				foreach (ExtraTransform point in spotLight.IrradiationPoint)
				{
					var lightArea = new GameObject
					{
						name = entry.name + "_IP"
					};
					var irradiationPointComponent = lightArea.AddComponent<ComponentIrradiationPoint>();
					lightArea.transform.position = point.Translation;
					lightArea.transform.rotation = point.Rotation;
					lightArea.transform.localScale = point.Scale;
					lightArea.transform.parent = entry.transform;
				}
			}

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
				component.InnerScalePositive = new Vector3(lightProbe.InnerScaleXNegative,lightProbe.InnerScaleYPositive,lightProbe.InnerScaleZPositive);
				component.InnerScaleNegative = new Vector3(lightProbe.InnerScaleXPositive,lightProbe.InnerScaleYNegative,lightProbe.InnerScaleZNegative);
				component.vals16 = lightProbe.vals16;
				component.Priority = lightProbe.Priority;
				component.ShapeType = lightProbe.ShapeType;
				component.RelatedLightIndex = lightProbe.RelatedLightIndex;
				component.SHDataIndex = lightProbe.SphericalHaromincsDataIndex;
				component.LightSize = lightProbe.LightSize;
				component.u5 = lightProbe.u5;
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
