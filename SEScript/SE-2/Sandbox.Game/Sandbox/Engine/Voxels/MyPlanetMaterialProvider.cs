using Sandbox.Definitions;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Noise;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyPlanetMaterialProvider
	{
		private struct MapBlendCache
		{
			public Vector2I Cell;

			public unsafe fixed ushort Data[4];

			public byte Face;

			public int HashCode;
		}

		public struct PlanetOre
		{
			public byte Value;

			public float Depth;

			public float Start;

			public float ColorInfluence;

			public MyVoxelMaterialDefinition Material;

			public Vector3? TargetColor;

			public override string ToString()
			{
				if (Material == null)
				{
					return "";
				}
				return $"{Material.Id.SubtypeName}({Start}:{Depth}; {Value})";
			}
		}

		public struct MaterialSampleParams
		{
			public Vector3 Gravity;

			public Vector3 Normal;

			public float DistanceToCenter;

			public float SampledHeight;

			public int Face;

			public Vector2 Texcoord;

			public float SurfaceDepth;

			public float LodSize;

			public float Latitude;

			public float Longitude;
		}

		public class VoxelMaterial
		{
			public MyVoxelMaterialDefinition Material;

			public float Depth;

			public byte Value;

			public virtual bool IsRule => false;

			public override string ToString()
			{
				if (Material != null)
				{
					return $"({Material.Id.SubtypeName}:{Depth})";
				}
				return "null";
			}
		}

		public class PlanetMaterial : VoxelMaterial
		{
			public VoxelMaterial[] Layers;

			public bool HasLayers
			{
				get
				{
					if (Layers != null)
					{
						return Layers.Length != 0;
					}
					return false;
				}
			}

			public MyVoxelMaterialDefinition FirstOrDefault
			{
				get
				{
					if (!HasLayers)
					{
						return Material;
					}
					return Layers[0].Material;
				}
			}

			public PlanetMaterial(MyPlanetMaterialDefinition def, float minimumSurfaceLayerDepth)
			{
				Depth = def.MaxDepth;
				if (def.Material != null)
				{
					Material = GetMaterial(def.Material);
				}
				Value = def.Value;
				if (!def.HasLayers)
				{
					return;
				}
				int num = def.Layers.Length;
				if (def.Layers[0].Depth < minimumSurfaceLayerDepth && GetMaterial(def.Layers[0].Material).RenderParams.Foliage.HasValue)
				{
					num++;
				}
				Layers = new VoxelMaterial[num];
				int num2 = 0;
				int num3 = 0;
				while (num2 < def.Layers.Length)
				{
					Layers[num3] = new VoxelMaterial
					{
						Material = GetMaterial(def.Layers[num2].Material),
						Depth = def.Layers[num2].Depth
					};
					if (num3 == 0 && def.Layers[num2].Depth < minimumSurfaceLayerDepth)
					{
						if (minimumSurfaceLayerDepth > 1f && Layers[num3].Material.RenderParams.Foliage.HasValue)
						{
							MyVoxelMaterialDefinition material = string.IsNullOrEmpty(Layers[num3].Material.BareVariant) ? Layers[num3].Material : MyDefinitionManager.Static.GetVoxelMaterialDefinition(Layers[num3].Material.BareVariant);
							Layers[num3].Depth = 1f;
							num3++;
							Layers[num3] = new VoxelMaterial
							{
								Material = material,
								Depth = minimumSurfaceLayerDepth - 1f
							};
						}
						else
						{
							Layers[num3].Depth = minimumSurfaceLayerDepth;
						}
					}
					num2++;
					num3++;
				}
			}

			private string FormatLayers(int padding)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string value = new string(' ', padding);
				stringBuilder.Append('[');
				if (Layers.Length != 0)
				{
					stringBuilder.Append('\n');
					for (int i = 0; i < Layers.Length; i++)
					{
						stringBuilder.Append(value);
						stringBuilder.Append("\t\t");
						stringBuilder.Append(Layers[i]);
						stringBuilder.Append('\n');
					}
				}
				stringBuilder.Append(value);
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}

			public override string ToString()
			{
				return ToString(0);
			}

			public string ToString(int padding)
			{
				if (HasLayers)
				{
					return $"LayeredMaterial({FormatLayers(padding)})";
				}
				return "SimpleMaterial" + base.ToString();
			}
		}

		public class PlanetMaterialRule : PlanetMaterial, IComparable<PlanetMaterialRule>
		{
			public SerializableRange Height;

			public SymmetricSerializableRange Latitude;

			public SerializableRange Longitude;

			public SerializableRange Slope;

			public int Index;

			public override bool IsRule => true;

			public bool Check(float height, float latitude, float longitude, float slope)
			{
				if (Height.ValueBetween(height) && Latitude.ValueBetween(latitude) && Longitude.ValueBetween(longitude))
				{
					return Slope.ValueBetween(slope);
				}
				return false;
			}

			public PlanetMaterialRule(MyPlanetMaterialPlacementRule def, int index, float minimumSurfaceLayerDepth)
				: base(def, minimumSurfaceLayerDepth)
			{
				Height = def.Height;
				Latitude = def.Latitude;
				Longitude = def.Longitude;
				Slope = def.Slope;
				Index = index;
			}

			public override string ToString()
			{
				return $"MaterialRule(\n\tHeight: {Height.ToString()};\n\tSlope: {Slope.ToStringAcos()};\n\tLatitude: {Latitude.ToStringAsin()};\n\tLongitude: {Longitude.ToStringLongitude()};\n\tMaterials: {ToString(4)})";
			}

			public int CompareTo(PlanetMaterialRule other)
			{
				if (this == other)
				{
					return 0;
				}
				if (other == null)
				{
					return 1;
				}
				return Index.CompareTo(other.Index);
			}
		}

		public class PlanetBiome
		{
			public MyDynamicAABBTree MateriaTree;

			public byte Value;

			public string Name;

			public List<PlanetMaterialRule> Rules;

			public bool IsValid => Rules.Count > 0;

			public PlanetBiome(MyPlanetMaterialGroup group, float minimumSurfaceLayerDepth)
			{
				Value = group.Value;
				Name = group.Name;
				Rules = new List<PlanetMaterialRule>(group.MaterialRules.Length);
				for (int i = 0; i < group.MaterialRules.Length; i++)
				{
					Rules.Add(new PlanetMaterialRule(group.MaterialRules[i], i, minimumSurfaceLayerDepth));
				}
				MateriaTree = new MyDynamicAABBTree(Vector3.Zero);
				foreach (PlanetMaterialRule rule in Rules)
				{
					BoundingBox aabb = new BoundingBox(new Vector3(rule.Height.Min, rule.Latitude.Min, rule.Longitude.Min), new Vector3(rule.Height.Max, rule.Latitude.Max, rule.Longitude.Max));
					MateriaTree.AddProxy(ref aabb, rule, 0u);
					if (rule.Latitude.Mirror)
					{
						float y = 0f - aabb.Max.Y;
						aabb.Max.Y = 0f - aabb.Min.Y;
						aabb.Min.Y = y;
						MateriaTree.AddProxy(ref aabb, rule, 0u);
					}
				}
			}
		}

		private int m_mapResolutionMinusOne;

		private MyPlanetShapeProvider m_planetShape;

		private Dictionary<byte, PlanetMaterial> m_materials;

		private Dictionary<byte, PlanetBiome> m_biomes;

		private Dictionary<byte, List<PlanetOre>> m_ores;

		private PlanetMaterial m_defaultMaterial;

		private PlanetMaterial m_subsurfaceMaterial;

		private MyDynamicAABBTree m_stationTree = new MyDynamicAABBTree(Vector3.Zero, 0f);

		private MyCubemap m_materialMap;

		private MyCubemap m_biomeMap;

		private MyCubemap m_oreMap;

		private MyTileTexture<byte> m_blendingTileset;

		private MyPlanetGeneratorDefinition m_generator;

		private float m_invHeightRange;

		private float m_heightmapScale;

		private float m_biomePixelSize;

		private bool m_hasRules;

		private int m_hashCode;

		[ThreadStatic]
		private static List<PlanetMaterialRule>[] m_rangeBiomes;

		[ThreadStatic]
		private static bool m_rangeClean;

		[ThreadStatic]
		private static WeakReference<MyPlanetMaterialProvider> m_chachedProviderRef;

		private MyPerlin m_perlin = new MyPerlin(MyNoiseQuality.Standard, 6, 123456, 5.0);

		private Vector3 m_targetShift = new Vector3(0f, -0.9f, -0.08f);

		[ThreadStatic]
		private static MapBlendCache m_materialBC;

		public MyCubemap[] Maps
		{
			get;
			private set;
		}

		private static MyPlanetMaterialProvider CachedProvider
		{
			get
			{
				if (m_chachedProviderRef != null && m_chachedProviderRef.TryGetTarget(out MyPlanetMaterialProvider target))
				{
					return target;
				}
				return null;
			}
			set
			{
				if (value != null || m_chachedProviderRef != null)
				{
					if (m_chachedProviderRef != null)
					{
						m_chachedProviderRef.SetTarget(value);
					}
					else
					{
						m_chachedProviderRef = new WeakReference<MyPlanetMaterialProvider>(value);
					}
				}
			}
		}

		public bool Closed
		{
			get;
			private set;
		}

		public MyPlanetMaterialProvider(MyPlanetGeneratorDefinition generatorDef, MyPlanetShapeProvider planetShape, MyCubemap[] maps)
		{
			m_materials = new Dictionary<byte, PlanetMaterial>(generatorDef.SurfaceMaterialTable.Length);
			for (int i = 0; i < generatorDef.SurfaceMaterialTable.Length; i++)
			{
				byte value = generatorDef.SurfaceMaterialTable[i].Value;
				m_materials[value] = new PlanetMaterial(generatorDef.SurfaceMaterialTable[i], generatorDef.MinimumSurfaceLayerDepth);
			}
			m_defaultMaterial = new PlanetMaterial(generatorDef.DefaultSurfaceMaterial, generatorDef.MinimumSurfaceLayerDepth);
			if (generatorDef.DefaultSubSurfaceMaterial != null)
			{
				m_subsurfaceMaterial = new PlanetMaterial(generatorDef.DefaultSubSurfaceMaterial, generatorDef.MinimumSurfaceLayerDepth);
			}
			else
			{
				m_subsurfaceMaterial = m_defaultMaterial;
			}
			m_planetShape = planetShape;
			Maps = maps;
			m_materialMap = maps[0];
			m_biomeMap = maps[1];
			m_oreMap = maps[2];
			if (m_materialMap != null)
			{
				m_mapResolutionMinusOne = m_materialMap.Resolution - 1;
			}
			m_generator = generatorDef;
			m_invHeightRange = 1f / (m_planetShape.MaxHillHeight - m_planetShape.MinHillHeight);
			m_biomePixelSize = (float)((double)(planetShape.MaxHillHeight + planetShape.Radius) * Math.PI) / ((float)(m_mapResolutionMinusOne + 1) * 2f);
			m_hashCode = generatorDef.FolderName.GetHashCode();
			if (m_generator.MaterialGroups != null && m_generator.MaterialGroups.Length != 0)
			{
				m_biomes = new Dictionary<byte, PlanetBiome>();
				MyPlanetMaterialGroup[] materialGroups = m_generator.MaterialGroups;
				foreach (MyPlanetMaterialGroup myPlanetMaterialGroup in materialGroups)
				{
					m_biomes[myPlanetMaterialGroup.Value] = new PlanetBiome(myPlanetMaterialGroup, m_generator.MinimumSurfaceLayerDepth);
				}
			}
			if (MyHeightMapLoadingSystem.Static != null)
			{
				m_blendingTileset = MyHeightMapLoadingSystem.Static.GetTerrainBlendTexture(m_generator.MaterialBlending);
			}
			m_ores = new Dictionary<byte, List<PlanetOre>>();
			MyPlanetOreMapping[] oreMappings = m_generator.OreMappings;
			foreach (MyPlanetOreMapping myPlanetOreMapping in oreMappings)
			{
				MyVoxelMaterialDefinition material = GetMaterial(myPlanetOreMapping.Type);
				if (material != null)
				{
					PlanetOre item = new PlanetOre
					{
						Depth = myPlanetOreMapping.Depth,
						Start = myPlanetOreMapping.Start,
						Value = myPlanetOreMapping.Value,
						Material = material,
						ColorInfluence = myPlanetOreMapping.ColorInfluence
					};
					if (myPlanetOreMapping.ColorShift.HasValue)
					{
						item.TargetColor = ColorExtensions.ColorToHSV(myPlanetOreMapping.ColorShift.Value);
					}
					if (!m_ores.ContainsKey(myPlanetOreMapping.Value))
					{
						List<PlanetOre> value2 = new List<PlanetOre>
						{
							item
						};
						m_ores.Add(myPlanetOreMapping.Value, value2);
					}
					m_ores[myPlanetOreMapping.Value].Add(item);
				}
			}
			Closed = false;
		}

		public bool IsMaterialBlacklistedForStation(MyDefinitionId materialId)
		{
			if (m_generator.StationBlockingMaterials != null)
			{
				return m_generator.StationBlockingMaterials.Contains(materialId);
			}
			return false;
		}

		public void Close()
		{
			m_blendingTileset = null;
			m_subsurfaceMaterial = null;
			m_generator = null;
			m_biomeMap = null;
			m_biomes = null;
			m_materials = null;
			m_planetShape = null;
			m_ores = null;
			m_materialMap = null;
			m_oreMap = null;
			m_biomeMap = null;
			Maps = null;
			Closed = true;
		}

		private unsafe void GetRuleBounds(ref BoundingBox request, out BoundingBox ruleBounds)
		{
			Vector3* ptr = stackalloc Vector3[8];
			ruleBounds.Min = new Vector3(float.PositiveInfinity);
			ruleBounds.Max = new Vector3(float.NegativeInfinity);
			request.GetCornersUnsafe(ptr);
			if (Vector3.Zero.IsInsideInclusive(ref request.Min, ref request.Max))
			{
				ruleBounds.Min.X = 0f;
			}
			else
			{
				Vector3 vector = Vector3.Clamp(Vector3.Zero, request.Min, request.Max);
				ruleBounds.Min.X = m_planetShape.DistanceToRatio(vector.Length());
			}
			Vector3 center = request.Center;
			Vector3 vector2 = default(Vector3);
			if (center.X < 0f)
			{
				vector2.X = request.Min.X;
			}
			else
			{
				vector2.X = request.Max.X;
			}
			if (center.Y < 0f)
			{
				vector2.Y = request.Min.Y;
			}
			else
			{
				vector2.Y = request.Max.Y;
			}
			if (center.Z < 0f)
			{
				vector2.Z = request.Min.Z;
			}
			else
			{
				vector2.Z = request.Max.Z;
			}
			ruleBounds.Max.X = m_planetShape.DistanceToRatio(vector2.Length());
			if (request.Min.X < 0f && request.Min.Z < 0f && request.Max.X > 0f && request.Max.Z > 0f)
			{
				ruleBounds.Min.Z = -1f;
				ruleBounds.Max.Z = 3f;
				for (int i = 0; i < 8; i++)
				{
					float num = ptr[i].Length();
					float num2 = ptr[i].Y / num;
					if (ruleBounds.Min.Y > num2)
					{
						ruleBounds.Min.Y = num2;
					}
					if (ruleBounds.Max.Y < num2)
					{
						ruleBounds.Max.Y = num2;
					}
				}
				return;
			}
			for (int j = 0; j < 8; j++)
			{
				float num3 = ptr[j].Length();
				ptr[j] /= num3;
				float num2 = ptr[j].Y;
				Vector2 vector3 = new Vector2(0f - ptr[j].X, 0f - ptr[j].Z);
				vector3.Normalize();
				float num4 = vector3.Y;
				if (vector3.X > 0f)
				{
					num4 = 2f - num4;
				}
				if (ruleBounds.Min.Y > num2)
				{
					ruleBounds.Min.Y = num2;
				}
				if (ruleBounds.Max.Y < num2)
				{
					ruleBounds.Max.Y = num2;
				}
				if (ruleBounds.Min.Z > num4)
				{
					ruleBounds.Min.Z = num4;
				}
				if (ruleBounds.Max.Z < num4)
				{
					ruleBounds.Max.Z = num4;
				}
			}
		}

		public void PrepareRulesForBox(ref BoundingBox request)
		{
			if (m_biomes != null)
			{
				if (request.Extents.Sum > 50f)
				{
					PrepareRulesForBoxInternal(ref request);
				}
				else
				{
					CleanRules();
				}
			}
		}

		private void PrepareRulesForBoxInternal(ref BoundingBox request)
		{
			if (m_rangeBiomes == null)
			{
				m_rangeBiomes = new List<PlanetMaterialRule>[256];
			}
			request.Translate(-m_planetShape.Center());
			request.Inflate(request.Extents.Length() * 0.1f);
			GetRuleBounds(ref request, out BoundingBox ruleBounds);
			bool flag = CachedProvider != this;
			foreach (PlanetBiome value in m_biomes.Values)
			{
				if (m_rangeBiomes[value.Value] == value.Rules || m_rangeBiomes[value.Value] == null || flag)
				{
					m_rangeBiomes[value.Value] = new List<PlanetMaterialRule>();
				}
				value.MateriaTree.OverlapAllBoundingBox(ref ruleBounds, m_rangeBiomes[value.Value]);
				m_rangeBiomes[value.Value].Sort();
			}
			m_rangeClean = false;
			CachedProvider = this;
		}

		private void CleanRules()
		{
			if (m_rangeBiomes == null)
			{
				m_rangeBiomes = new List<PlanetMaterialRule>[256];
			}
			foreach (PlanetBiome value in m_biomes.Values)
			{
				m_rangeBiomes[value.Value] = value.Rules;
			}
			m_rangeClean = true;
			CachedProvider = this;
		}

		public void ReadMaterialRange(ref MyVoxelDataRequest req, bool detectOnly = false)
		{
			req.Flags = (req.RequestFlags & MyVoxelRequestFlags.RequestFlags);
			Vector3I minInLod = req.MinInLod;
			Vector3I maxInLod = req.MaxInLod;
			float num = 1 << req.Lod;
			bool flag = req.RequestFlags.HasFlags(MyVoxelRequestFlags.SurfaceMaterial);
			bool flag2 = req.RequestFlags.HasFlags(MyVoxelRequestFlags.ConsiderContent);
			bool preciseOrePositions = req.RequestFlags.HasFlags(MyVoxelRequestFlags.PreciseOrePositions);
			m_planetShape.PrepareCache();
			if (m_biomes != null)
			{
				if (req.SizeLinear > 125)
				{
					BoundingBox request = new BoundingBox((Vector3)minInLod * num, (Vector3)maxInLod * num);
					PrepareRulesForBoxInternal(ref request);
				}
				else if (!m_rangeClean || CachedProvider != this)
				{
					CleanRules();
				}
			}
			Vector3 vector = (minInLod + 0.5f) * num;
			Vector3 pos = vector;
			Vector3I b = -minInLod + req.Offset;
			Vector3I a = default(Vector3I);
			if (detectOnly)
			{
				a.Z = minInLod.Z;
				while (a.Z <= maxInLod.Z)
				{
					a.Y = minInLod.Y;
					while (a.Y <= maxInLod.Y)
					{
						a.X = minInLod.X;
						while (a.X <= maxInLod.X)
						{
							byte biomeValue;
							MyVoxelMaterialDefinition materialForPosition = GetMaterialForPosition(ref pos, num, out biomeValue, preciseOrePositions);
							if (materialForPosition != null && materialForPosition.Index != byte.MaxValue)
							{
								return;
							}
							pos.X += num;
							a.X++;
						}
						pos.Y += num;
						pos.X = vector.X;
						a.Y++;
					}
					pos.Z += num;
					pos.Y = vector.Y;
					a.Z++;
				}
				req.Flags |= MyVoxelRequestFlags.EmptyData;
				return;
			}
			bool flag3 = true;
			MyStorageData target = req.Target;
			a.Z = minInLod.Z;
			while (a.Z <= maxInLod.Z)
			{
				a.Y = minInLod.Y;
				while (a.Y <= maxInLod.Y)
				{
					a.X = minInLod.X;
					Vector3I p = a + b;
					int num2 = target.ComputeLinear(ref p);
					while (a.X <= maxInLod.X)
					{
						byte biomeValue2;
						byte b2 = ((!flag || target.Material(num2) == 0) && (!flag2 || target.Content(num2) != 0)) ? (GetMaterialForPosition(ref pos, num, out biomeValue2, preciseOrePositions)?.Index ?? byte.MaxValue) : byte.MaxValue;
						target.Material(num2, b2);
						flag3 = (flag3 && b2 == byte.MaxValue);
						num2 += target.StepLinear;
						pos.X += num;
						a.X++;
					}
					pos.Y += num;
					pos.X = vector.X;
					a.Y++;
				}
				pos.Z += num;
				pos.Y = vector.Y;
				a.Z++;
			}
			if (flag3)
			{
				req.Flags |= MyVoxelRequestFlags.EmptyData;
			}
		}

		private static MyVoxelMaterialDefinition GetMaterial(string name)
		{
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(name);
			if (voxelMaterialDefinition == null)
			{
				MyLog.Default.WriteLine("Could not load voxel material " + name);
			}
			return voxelMaterialDefinition;
		}

		public MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 pos, float lodSize)
		{
			byte biomeValue;
			return GetMaterialForPosition(ref pos, lodSize, out biomeValue, preciseOrePositions: false);
		}

		public MyVoxelMaterialDefinition GetMaterialForPosition(ref Vector3 pos, float lodSize, out byte biomeValue, bool preciseOrePositions)
		{
			biomeValue = 0;
			GetPositionParams(ref pos, lodSize, out MaterialSampleParams ps);
			MyVoxelMaterialDefinition myVoxelMaterialDefinition = null;
			float num = (!preciseOrePositions) ? (ps.SurfaceDepth / Math.Max(lodSize * 0.5f, 1f) + 0.5f) : (ps.SurfaceDepth + 0.5f);
			if (m_oreMap != null)
			{
				byte value = m_oreMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
				if (m_ores.TryGetValue(value, out List<PlanetOre> value2))
				{
					foreach (PlanetOre item in value2)
					{
						if (item.Start <= 0f - num && item.Start + item.Depth >= 0f - num)
						{
							if (item.Material.IsRare)
							{
								BoundingBox bbox = new BoundingBox(pos - Vector3.One, pos + Vector3.One);
								if (m_stationTree.OverlapsAnyLeafBoundingBox(ref bbox))
								{
									continue;
								}
							}
							return item.Material;
						}
					}
				}
			}
			PlanetMaterial layeredMaterialForPosition = GetLayeredMaterialForPosition(ref ps, out biomeValue);
			float num2 = ps.SurfaceDepth / lodSize;
			if (layeredMaterialForPosition.HasLayers)
			{
				VoxelMaterial[] layers = layeredMaterialForPosition.Layers;
				for (int i = 0; i < layers.Length; i++)
				{
					if (num2 >= 0f - layers[i].Depth)
					{
						myVoxelMaterialDefinition = layeredMaterialForPosition.Layers[i].Material;
						break;
					}
				}
			}
			else if (num2 >= 0f - layeredMaterialForPosition.Depth)
			{
				myVoxelMaterialDefinition = layeredMaterialForPosition.Material;
			}
			if (myVoxelMaterialDefinition == null)
			{
				myVoxelMaterialDefinition = m_subsurfaceMaterial.FirstOrDefault;
			}
			return myVoxelMaterialDefinition;
		}

		public Vector3 GetColorShift(Vector3 position, byte material, float maxDepth = 1f)
		{
			if (maxDepth < 1f)
			{
				return Vector3.Zero;
			}
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(material);
			if (voxelMaterialDefinition == null || !voxelMaterialDefinition.ColorKey.HasValue)
			{
				return Vector3.Zero;
			}
			Vector3 localPos = position - m_planetShape.Center();
			MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out int face, out Vector2 texCoord);
			if (m_oreMap == null)
			{
				return Vector3.Zero;
			}
			byte value = m_oreMap.Faces[face].GetValue(texCoord.X, texCoord.Y);
			if (!m_ores.TryGetValue(value, out List<PlanetOre> value2))
			{
				return Vector3.Zero;
			}
			float scaleFactor = (float)MathHelper.Saturate(m_perlin.GetValue(texCoord.X * 1000f, texCoord.Y * 1000f, 0.0));
			foreach (PlanetOre item in value2)
			{
				if (item.Material.IsRare)
				{
					BoundingBox bbox = new BoundingBox(position - Vector3.One, position + Vector3.One);
					if (m_stationTree.OverlapsAnyLeafBoundingBox(ref bbox))
					{
						continue;
					}
				}
				float colorInfluence = item.ColorInfluence;
				colorInfluence = 256f;
				if (colorInfluence >= 1f && colorInfluence >= item.Start && item.Start <= maxDepth && item.TargetColor.HasValue)
				{
					Vector3 value3 = item.TargetColor.Value;
					_ = Color.Violet;
					value3 = ((!(value3 == Vector3.Backward)) ? m_targetShift : new Vector3(0f, 1f, -0.08f));
					return scaleFactor * value3 * (1f - item.Start / colorInfluence);
				}
			}
			return Vector3.Zero;
		}

		private unsafe byte ComputeMapBlend(Vector2 coords, int face, ref MapBlendCache cache, MyCubemapData<byte> map)
		{
			coords = coords * map.Resolution - 0.5f;
			Vector2I vector2I = new Vector2I(coords);
			if (cache.HashCode != m_hashCode || cache.Face != face || cache.Cell != vector2I)
			{
				cache.HashCode = m_hashCode;
				cache.Cell = vector2I;
				cache.Face = (byte)face;
				if (m_materialMap != null)
				{
					map.GetValue(vector2I.X, vector2I.Y, out byte value);
					map.GetValue(vector2I.X + 1, vector2I.Y, out byte value2);
					map.GetValue(vector2I.X, vector2I.Y + 1, out byte value3);
					map.GetValue(vector2I.X + 1, vector2I.Y + 1, out byte value4);
					byte* ptr = stackalloc byte[4];
					*ptr = value;
					ptr[1] = value2;
					ptr[2] = value3;
					ptr[3] = value4;
					if (value == value2 && value3 == value4 && value3 == value)
					{
						fixed (ushort* ptr2 = cache.Data)
						{
							*ptr2 = (ushort)((value << 8) | 0xF);
							ptr2[1] = 0;
							ptr2[2] = 0;
							ptr2[3] = 0;
						}
					}
					else
					{
						fixed (ushort* values = cache.Data)
						{
							Sort4(ptr);
							ComputeTilePattern(value, value2, value3, value4, ptr, values);
						}
					}
				}
			}
			byte computed;
			fixed (ushort* values2 = cache.Data)
			{
				coords -= Vector2.Floor(coords);
				if (coords.X == 1f)
				{
					coords.X = 0.99999f;
				}
				if (coords.Y == 1f)
				{
					coords.Y = 0.99999f;
				}
				SampleTile(values2, ref coords, out computed);
			}
			return computed;
		}

		private unsafe static void Sort4(byte* v)
		{
			if (*v > v[1])
			{
				byte b = v[1];
				v[1] = *v;
				*v = b;
			}
			if (v[2] > v[3])
			{
				byte b = v[2];
				v[2] = v[3];
				v[3] = b;
			}
			if (*v > v[3])
			{
				byte b = v[3];
				v[3] = *v;
				v[3] = b;
			}
			if (v[1] > v[2])
			{
				byte b = v[1];
				v[1] = v[2];
				v[2] = b;
			}
			if (*v > v[1])
			{
				byte b = v[1];
				v[1] = *v;
				*v = b;
			}
			if (v[2] > v[3])
			{
				byte b = v[2];
				v[2] = v[3];
				v[3] = b;
			}
		}

		private unsafe static void ComputeTilePattern(byte tl, byte tr, byte bl, byte br, byte* ss, ushort* values)
		{
			int i = 0;
			for (int j = 0; j < 4; j++)
			{
				if (j <= 0 || ss[j] != ss[j - 1])
				{
					values[i++] = (ushort)((ss[j] << 8) | ((ss[j] == tl) ? 8 : 0) | ((ss[j] == tr) ? 4 : 0) | ((ss[j] == bl) ? 2 : 0) | ((ss[j] == br) ? 1 : 0));
				}
			}
			for (; i < 4; i++)
			{
				values[i] = 0;
			}
		}

		private unsafe void SampleTile(ushort* values, ref Vector2 coords, out byte computed)
		{
			byte b = 0;
			for (int i = 0; i < 4; i++)
			{
				byte b2 = (byte)(values[i] >> 8);
				if (values[i] == 0)
				{
					break;
				}
				int corners = values[i] & 0xF;
				m_blendingTileset.GetValue(corners, coords, out byte value);
				b = b2;
				if (value == 0)
				{
					break;
				}
			}
			computed = b;
		}

		public void GetPositionParams(ref Vector3 pos, float lodSize, out MaterialSampleParams ps, bool skipCache = false)
		{
			Vector3 localPos = pos - m_planetShape.Center();
			ps.DistanceToCenter = localPos.Length();
			ps.LodSize = lodSize;
			if (ps.DistanceToCenter < 0.01f)
			{
				ps.SurfaceDepth = 0f;
				ps.Gravity = Vector3.Down;
				ps.Latitude = 0f;
				ps.Longitude = 0f;
				ps.Texcoord = Vector2.One / 2f;
				ps.Face = 0;
				ps.Normal = Vector3.Backward;
				ps.SampledHeight = 0f;
				return;
			}
			ps.Gravity = localPos / ps.DistanceToCenter;
			MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out ps.Face, out ps.Texcoord);
			if (skipCache)
			{
				ps.SampledHeight = m_planetShape.GetValueForPositionCacheless(ps.Face, ref ps.Texcoord, out ps.Normal);
			}
			else
			{
				ps.SampledHeight = m_planetShape.GetValueForPositionWithCache(ps.Face, ref ps.Texcoord, out ps.Normal);
			}
			ps.SurfaceDepth = m_planetShape.SignedDistanceWithSample(lodSize, ps.DistanceToCenter, ps.SampledHeight) * ps.Normal.Z;
			ps.Latitude = ps.Gravity.Y;
			Vector2 vector = new Vector2(0f - ps.Gravity.X, 0f - ps.Gravity.Z);
			vector.Normalize();
			ps.Longitude = vector.Y;
			if (0f - ps.Gravity.X > 0f)
			{
				ps.Longitude = 2f - ps.Longitude;
			}
		}

		public PlanetMaterial GetLayeredMaterialForPosition(ref MaterialSampleParams ps, out byte biomeValue)
		{
			if ((double)ps.DistanceToCenter < 0.01)
			{
				biomeValue = byte.MaxValue;
				return m_defaultMaterial;
			}
			byte b = 0;
			PlanetMaterial value = null;
			byte b2 = 0;
			if (m_biomeMap != null)
			{
				b2 = m_biomeMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
			}
			if (m_biomePixelSize < ps.LodSize)
			{
				if (m_materialMap != null)
				{
					b = m_materialMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
				}
			}
			else if (m_materialMap != null)
			{
				b = ComputeMapBlend(ps.Texcoord, ps.Face, ref m_materialBC, m_materialMap.Faces[ps.Face]);
			}
			m_materials.TryGetValue(b, out value);
			if (value == null && m_biomes != null)
			{
				List<PlanetMaterialRule> list = m_rangeBiomes[b];
				if (list != null && list.Count != 0)
				{
					float height = (ps.SampledHeight - m_planetShape.MinHillHeight) * m_invHeightRange;
					foreach (PlanetMaterialRule item in list)
					{
						if (item.Check(height, ps.Latitude, ps.Longitude, ps.Normal.Z))
						{
							value = item;
							break;
						}
					}
				}
			}
			if (value == null)
			{
				value = m_defaultMaterial;
			}
			biomeValue = b2;
			return value;
		}

		public void SetStationOreBlockTree(MyDynamicAABBTree tree)
		{
			m_stationTree = tree;
		}

		public void GetMaterialForPositionDebug(ref Vector3 pos, out MyPlanetStorageProvider.SurfacePropertiesExtended props)
		{
			GetPositionParams(ref pos, 1f, out MaterialSampleParams ps, skipCache: true);
			props.Position = pos;
			props.Gravity = -ps.Gravity;
			props.Material = m_defaultMaterial.FirstOrDefault;
			props.Slope = ps.Normal.Z;
			props.HeightRatio = m_planetShape.AltitudeToRatio(ps.SampledHeight);
			props.Depth = ps.SurfaceDepth;
			props.Latitude = ps.Latitude;
			props.Longitude = ps.Longitude;
			props.Altitude = ps.DistanceToCenter - m_planetShape.Radius;
			props.GroundHeight = ps.SampledHeight + m_planetShape.Radius;
			props.Face = ps.Face;
			props.Texcoord = ps.Texcoord;
			props.BiomeValue = 0;
			props.MaterialValue = 0;
			props.OreValue = 0;
			props.EffectiveRule = null;
			props.Biome = null;
			props.Ore = default(PlanetOre);
			props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Default;
			PlanetMaterial value = null;
			if (m_oreMap != null)
			{
				props.OreValue = m_oreMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
				if (m_ores.TryGetValue(props.OreValue, out List<PlanetOre> value2))
				{
					foreach (PlanetOre item in value2)
					{
						PlanetOre planetOre = props.Ore = item;
						if (planetOre.Start <= 0f - ps.SurfaceDepth && planetOre.Start + planetOre.Depth >= 0f - ps.SurfaceDepth)
						{
							props.Material = planetOre.Material;
							props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Ore;
							break;
						}
					}
				}
			}
			if ((double)ps.DistanceToCenter < 0.01)
			{
				return;
			}
			byte b = 0;
			if (m_biomePixelSize < ps.LodSize)
			{
				if (m_materialMap != null)
				{
					b = m_materialMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
				}
			}
			else if (m_materialMap != null)
			{
				b = ComputeMapBlend(ps.Texcoord, ps.Face, ref m_materialBC, m_materialMap.Faces[ps.Face]);
			}
			m_materials.TryGetValue(b, out value);
			props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Map;
			props.MaterialValue = b;
			if (value == null && m_biomes != null)
			{
				m_biomes.TryGetValue(b, out PlanetBiome value3);
				props.Biome = value3;
				if (value3 != null && value3.IsValid)
				{
					foreach (PlanetMaterialRule rule in value3.Rules)
					{
						if (rule.Check(props.HeightRatio, ps.Latitude, ps.Longitude, ps.Normal.Z))
						{
							value = rule;
							props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Rule;
							break;
						}
					}
				}
			}
			if (value == null)
			{
				value = m_defaultMaterial;
				props.Origin = MyPlanetStorageProvider.SurfacePropertiesExtended.MaterialOrigin.Default;
			}
			byte biomeValue = 0;
			if (m_biomeMap != null)
			{
				biomeValue = m_biomeMap.Faces[ps.Face].GetValue(ps.Texcoord.X, ps.Texcoord.Y);
			}
			props.BiomeValue = biomeValue;
			float num = ps.SurfaceDepth + 0.5f;
			if (value.HasLayers)
			{
				VoxelMaterial[] layers = value.Layers;
				for (int i = 0; i < layers.Length; i++)
				{
					if (num >= 0f - layers[i].Depth)
					{
						props.Material = value.Layers[i].Material;
						break;
					}
				}
			}
			else if (num >= 0f - value.Depth)
			{
				props.Material = value.Material;
			}
			props.EffectiveRule = value;
		}
	}
}
