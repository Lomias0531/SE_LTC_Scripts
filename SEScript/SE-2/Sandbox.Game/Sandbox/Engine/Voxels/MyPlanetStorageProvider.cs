using Sandbox.Definitions;
using Sandbox.Engine.Voxels.Planet;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	[MyStorageDataProvider(10042)]
	public class MyPlanetStorageProvider : IMyStorageDataProvider
	{
		private struct PlanetData
		{
			public long Version;

			public long Seed;

			public double Radius;
		}

		public struct SurfacePropertiesExtended
		{
			public enum MaterialOrigin
			{
				Rule,
				Ore,
				Map,
				Default
			}

			public Vector3 Position;

			public Vector3 Gravity;

			public MyVoxelMaterialDefinition Material;

			public float Slope;

			public float HeightRatio;

			public float Depth;

			public float GroundHeight;

			public float Latitude;

			public float Longitude;

			public float Altitude;

			public int Face;

			public Vector2 Texcoord;

			public byte BiomeValue;

			public byte MaterialValue;

			public byte OreValue;

			public MyPlanetMaterialProvider.PlanetMaterial EffectiveRule;

			public MyPlanetMaterialProvider.PlanetBiome Biome;

			public MyPlanetMaterialProvider.PlanetOre Ore;

			public MaterialOrigin Origin;
		}

		private static readonly int STORAGE_VERSION = 1;

		private PlanetData m_data;

		public MyPlanetGeneratorDefinition Generator
		{
			get;
			private set;
		}

		public unsafe int SerializedSize
		{
			get
			{
				int byteCount = Encoding.UTF8.GetByteCount(Generator.Id.SubtypeName);
				byteCount += (MathHelper.Log2Floor(byteCount) + 6) / 7;
				return sizeof(PlanetData) + byteCount;
			}
		}

		public bool Closed
		{
			get;
			private set;
		}

		public MyPlanetShapeProvider Shape
		{
			get;
			private set;
		}

		public MyPlanetMaterialProvider Material
		{
			get;
			private set;
		}

		public Vector3I StorageSize
		{
			get;
			private set;
		}

		public float Radius => (float)m_data.Radius;

		public void WriteTo(Stream stream)
		{
			stream.WriteNoAlloc(m_data.Version);
			stream.WriteNoAlloc(m_data.Seed);
			stream.WriteNoAlloc(m_data.Radius);
			stream.WriteNoAlloc(Generator.Id.SubtypeName);
		}

		public void ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat)
		{
			m_data.Version = stream.ReadInt64();
			m_data.Seed = stream.ReadInt64();
			m_data.Radius = stream.ReadDouble();
			string text = stream.ReadString();
			if (m_data.Version != STORAGE_VERSION)
			{
				isOldFormat = true;
			}
			MyPlanetGeneratorDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(text));
			if (definition == null)
			{
				throw new Exception($"Cannot load planet generator definition for subtype '{text}'.");
			}
			Generator = definition;
			Init(m_data.Seed);
		}

		public void Init(long seed, MyPlanetGeneratorDefinition generator, double radius)
		{
			radius = Math.Max(radius, 1.0);
			Generator = generator;
			m_data = new PlanetData
			{
				Radius = radius,
				Seed = seed,
				Version = STORAGE_VERSION
			};
			Init(seed);
			Closed = false;
		}

		private void Init(long seed)
		{
			float num = (float)m_data.Radius;
			float num2 = num * Generator.HillParams.Max;
			float num3 = num + num2;
			StorageSize = MyVoxelCoordSystems.FindBestOctreeSize(2f * num3);
			float value = (float)StorageSize.X * 0.5f;
			MyPlanetTextureMapProvider myPlanetTextureMapProvider = new MyPlanetTextureMapProvider();
			myPlanetTextureMapProvider.Init(seed, Generator, Generator.MapProvider);
			Shape = new MyPlanetShapeProvider(new Vector3(value), num, Generator, myPlanetTextureMapProvider.GetHeightmap(), myPlanetTextureMapProvider);
			Material = new MyPlanetMaterialProvider(Generator, Shape, myPlanetTextureMapProvider.GetMaps(Generator.PlanetMaps.ToSet()));
		}

		public void Close()
		{
			Material.Close();
			Shape.Close();
			Closed = true;
		}

		public unsafe void PostProcess(VrVoxelMesh mesh, MyStorageDataTypeFlags dataTypes)
		{
			if (!dataTypes.Requests(MyStorageDataTypeEnum.Material))
			{
				return;
			}
			VrVoxelVertex* vertices = mesh.Vertices;
			int vertexCount = mesh.VertexCount;
			Vector3 value = mesh.Start;
			float scale = mesh.Scale;
			for (int i = 0; i < vertexCount; i++)
			{
				Vector3 position = (value + vertices[i].Position) * scale;
				Vector3 colorShift = Material.GetColorShift(position, vertices[i].Material, 1024f);
				if (colorShift != Vector3.Zero)
				{
					vertices[i].Color.PackedValue = PackColorShift(colorShift * new Vector3(360f, 100f, 100f));
				}
			}
		}

		private static uint PackColorShift(Vector3 hsv)
		{
			int num = (int)hsv.X;
			int value = (int)hsv.Y;
			int value2 = (int)hsv.Z;
			num %= 360;
			value = MathHelper.Clamp(value, -100, 100);
			value2 = MathHelper.Clamp(value2, -100, 100);
			return (uint)(((0xFFFF & num) << 16) | ((0xFF & value) << 8) | (0xFF & value2));
		}

		public void ReadRange(MyStorageData target, MyStorageDataTypeFlags dataType, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod)
		{
			if (!Closed)
			{
				MyVoxelDataRequest myVoxelDataRequest = default(MyVoxelDataRequest);
				myVoxelDataRequest.Target = target;
				myVoxelDataRequest.Offset = writeOffset;
				myVoxelDataRequest.RequestedData = dataType;
				myVoxelDataRequest.Lod = lodIndex;
				myVoxelDataRequest.MinInLod = minInLod;
				myVoxelDataRequest.MaxInLod = maxInLod;
				MyVoxelDataRequest req = myVoxelDataRequest;
				ReadRange(ref req);
			}
		}

		public MyVoxelMaterialDefinition GetMaterialAtPosition(ref Vector3D localPosition)
		{
			if (Closed)
			{
				return null;
			}
			Vector3 pos = localPosition;
			return Material.GetMaterialForPosition(ref pos, 1f);
		}

		public void ReadRange(ref MyVoxelDataRequest req, bool detectOnly = false)
		{
			if (Closed)
			{
				return;
			}
			if (req.RequestedData.Requests(MyStorageDataTypeEnum.Content))
			{
				Shape.ReadContentRange(ref req, detectOnly);
				req.RequestFlags |= MyVoxelRequestFlags.ConsiderContent;
			}
			if (req.Flags.HasFlags(MyVoxelRequestFlags.EmptyData))
			{
				if (!detectOnly && req.RequestedData.Requests(MyStorageDataTypeEnum.Material))
				{
					req.Target.BlockFill(MyStorageDataTypeEnum.Material, req.MinInLod, req.MaxInLod, byte.MaxValue);
				}
			}
			else if (req.RequestedData.Requests(MyStorageDataTypeEnum.Material))
			{
				Material.ReadMaterialRange(ref req, detectOnly);
			}
		}

		public ContainmentType Intersect(BoundingBoxI box, int lod)
		{
			if (Closed)
			{
				return ContainmentType.Disjoint;
			}
			BoundingBox box2 = new BoundingBox(box);
			box2.Translate(-Shape.Center());
			return Shape.IntersectBoundingBox(ref box2, 1f);
		}

		public bool Intersect(ref LineD line, out double startOffset, out double endOffset)
		{
			LineD ll = line;
			Vector3 vector = Shape.Center();
			ll.To -= vector;
			ll.From -= vector;
			if (Shape.IntersectLine(ref ll, out startOffset, out endOffset))
			{
				ll.From += vector;
				ll.To += vector;
				line = ll;
				return true;
			}
			return false;
		}

		public void DebugDraw(ref MatrixD worldMatrix)
		{
		}

		public void ReindexMaterials(Dictionary<byte, byte> oldToNewIndexMap)
		{
		}

		public void ComputeCombinedMaterialAndSurface(Vector3 position, bool useCache, out MySurfaceParams props)
		{
			if (Closed)
			{
				props = default(MySurfaceParams);
				return;
			}
			position -= Shape.Center();
			float divider = position.Length();
			MyPlanetMaterialProvider.MaterialSampleParams ps = default(MyPlanetMaterialProvider.MaterialSampleParams);
			ps.Gravity = position / divider;
			props.Latitude = ps.Gravity.Y;
			Vector2 vector = new Vector2(0f - ps.Gravity.X, 0f - ps.Gravity.Z);
			vector.Normalize();
			props.Longitude = vector.Y;
			if (0f - ps.Gravity.X > 0f)
			{
				props.Longitude = 2f - props.Longitude;
			}
			MyCubemapHelpers.CalculateSampleTexcoord(ref position, out int face, out Vector2 texCoord);
			float num = ps.SampledHeight = (useCache ? Shape.GetValueForPositionWithCache(face, ref texCoord, out props.Normal) : Shape.GetValueForPositionCacheless(face, ref texCoord, out props.Normal));
			ps.SurfaceDepth = 0f;
			ps.Texcoord = texCoord;
			ps.LodSize = 1f;
			ps.Latitude = props.Latitude;
			ps.Longitude = props.Longitude;
			ps.Face = face;
			ps.Normal = props.Normal;
			props.Position = ps.Gravity * (Radius + num) + Shape.Center();
			props.Gravity = (ps.Gravity = -ps.Gravity);
			ps.DistanceToCenter = props.Position.Length();
			MyPlanetMaterialProvider.PlanetMaterial layeredMaterialForPosition = Material.GetLayeredMaterialForPosition(ref ps, out props.Biome);
			if (layeredMaterialForPosition.FirstOrDefault == null)
			{
				props.Material = 0;
			}
			else
			{
				props.Material = layeredMaterialForPosition.FirstOrDefault.Index;
			}
			props.Normal = ps.Normal;
			props.HeightRatio = Shape.AltitudeToRatio(num);
		}

		public void ComputeCombinedMaterialAndSurfaceExtended(Vector3 position, out SurfacePropertiesExtended props)
		{
			if (Closed)
			{
				props = default(SurfacePropertiesExtended);
			}
			else
			{
				Material.GetMaterialForPositionDebug(ref position, out props);
			}
		}
	}
}
