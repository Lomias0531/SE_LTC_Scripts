using Sandbox.Definitions;
using Sandbox.Engine.Voxels.Planet;
using System;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
	public class MyPlanetShapeProvider
	{
		private struct SurfaceDetailSampler : IDisposable
		{
			private MyHeightDetailTexture m_detail;

			public float Factor;

			public float Size;

			public float Scale;

			private float m_min;

			private float m_max;

			private float m_in;

			private float m_out;

			private float m_inRecip;

			private float m_outRecip;

			private float m_mid;

			public void Init(MyPlanetTextureMapProvider texProvider, MyPlanetSurfaceDetail def, float faceSize)
			{
				m_detail = texProvider.GetDetailMap(def.Texture);
				Size = def.Size;
				Factor = faceSize / Size;
				m_min = (float)Math.Cos(MathHelper.ToRadians(def.Slope.Max));
				m_max = (float)Math.Cos(MathHelper.ToRadians(def.Slope.Min));
				m_in = (float)Math.Cos(MathHelper.ToRadians(def.Slope.Max - def.Transition));
				m_out = (float)Math.Cos(MathHelper.ToRadians(def.Slope.Min + def.Transition));
				m_inRecip = 1f / (m_in - m_min);
				m_outRecip = 1f / (m_max - m_out);
				m_mid = (float)Math.Cos(MathHelper.ToRadians((def.Slope.Max + def.Slope.Min) / 2f));
				Scale = def.Scale;
			}

			public bool Matches(float angle)
			{
				if (angle <= m_max)
				{
					return angle >= m_min;
				}
				return false;
			}

			public float GetValue(float dtx, float dty, float angle)
			{
				if (m_detail == null)
				{
					return 0f;
				}
				float num = (!(angle > m_mid)) ? Math.Max(Math.Min((angle - m_in) * m_inRecip, 1f), 0f) : Math.Min(Math.Max(1f - (angle - m_out) * m_outRecip, 0f), 1f);
				return m_detail.GetValue(dtx, dty) * num * Scale;
			}

			public unsafe VrPlanetShape.DetailMapData GetDetailMapData()
			{
				VrPlanetShape.DetailMapData result = default(VrPlanetShape.DetailMapData);
				result.Data = m_detail.Data;
				result.Factor = Factor;
				result.Size = Size;
				result.Resolution = (int)m_detail.Resolution;
				result.Scale = Scale;
				result.m_min = m_min;
				result.m_max = m_max;
				result.m_in = m_in;
				result.m_out = m_out;
				result.m_inRecip = m_inRecip;
				result.m_outRecip = m_outRecip;
				result.m_mid = m_mid;
				return result;
			}

			public void Dispose()
			{
				if (m_detail != null)
				{
					m_detail.Dispose();
				}
				m_detail = null;
			}
		}

		private struct Cache
		{
			public struct Cell
			{
				public Matrix Gz;

				public float Min;

				public float Max;

				public Vector3I Coord;
			}

			private const int CacheBits = 4;

			private const int CacheMask = 15;

			private const int CacheSize = 256;

			public Cell[] Cells;

			public string Name;

			public int CellCoord(ref Vector3I coord)
			{
				return ((coord.Y & 0xF) << 4) | (coord.X & 0xF);
			}

			internal void Clean()
			{
				if (Cells == null)
				{
					Cells = new Cell[256];
				}
				for (int i = 0; i < 256; i++)
				{
					Cells[i].Coord = new Vector3I(-1);
				}
			}
		}

		private const int MAX_UNCULLED_HISTORY = 10;

		private static Matrix CR;

		private static Matrix CRT;

		private static Matrix BInv;

		private static Matrix BInvT;

		private static float Tau;

		public static MyDebugHitCounter PruningStats;

		public static MyDebugHitCounter CacheStats;

		public static MyDebugHitCounter CullStats;

		private readonly int m_mapResolutionMinusOne;

		private readonly float m_radius;

		private string m_dataFileName;

		private MyHeightCubemap m_heightmap;

		private VrPlanetShape m_nativeShape;

		private SurfaceDetailSampler m_detail;

		private float m_detailSlopeRecip;

		private float m_detailFade;

		private readonly Vector3 m_translation;

		private readonly float m_maxHillHeight;

		private readonly float m_minHillHeight;

		private readonly float m_heightRatio;

		private readonly float m_heightRatioRecip;

		private float m_detailScale;

		private readonly float m_pixelSize;

		private readonly float m_pixelSizeRecip2;

		private readonly float m_mapStepScale;

		private readonly float m_mapStepScaleSquare;

		private float m_mapHeightScale;

		[ThreadStatic]
		private static Cache m_cache;

		private const bool EnableBezierCull = true;

		[ThreadStatic]
		private static Matrix s_Cz;

		private const bool ForceBilinear = false;

		public float OuterRadius
		{
			get;
			private set;
		}

		public float InnerRadius
		{
			get;
			private set;
		}

		public bool Closed
		{
			get;
			private set;
		}

		public float Radius => m_radius;

		public float HeightRatio => m_heightRatio;

		public float MinHillHeight => m_minHillHeight;

		public float MaxHillHeight => m_maxHillHeight;

		public MyHeightCubemap Heightmap => m_heightmap;

		static MyPlanetShapeProvider()
		{
			PruningStats = new MyDebugHitCounter();
			CacheStats = new MyDebugHitCounter();
			CullStats = new MyDebugHitCounter();
			SetTau(0.5f);
			BInvT = new Matrix(1f, 1f, 1f, 1f, 0f, 0.333333343f, 2f / 3f, 1f, 0f, 0f, 0.333333343f, 1f, 0f, 0f, 0f, 1f);
			BInv = Matrix.Transpose(BInvT);
		}

		public static void SetTau(float tau)
		{
			Tau = tau;
			CRT = new Matrix(0f, 0f - Tau, 2f * Tau, 0f - Tau, 1f, 0f, Tau - 3f, 2f - Tau, 0f, Tau, 3f - 2f * Tau, Tau - 2f, 0f, 0f, 0f - Tau, Tau);
			CR = Matrix.Transpose(CRT);
		}

		public static float GetTau()
		{
			return Tau;
		}

		internal Vector3 Center()
		{
			return m_translation;
		}

		public MyPlanetShapeProvider(Vector3 translation, float radius, MyPlanetGeneratorDefinition definition, MyHeightCubemap cubemap, MyPlanetTextureMapProvider texProvider)
		{
			m_radius = radius;
			m_translation = translation;
			m_maxHillHeight = definition.HillParams.Max * m_radius;
			m_minHillHeight = definition.HillParams.Min * m_radius;
			InnerRadius = radius + m_minHillHeight;
			OuterRadius = radius + m_maxHillHeight;
			m_heightmap = cubemap;
			m_mapResolutionMinusOne = m_heightmap.Resolution - 1;
			m_heightRatio = m_maxHillHeight - m_minHillHeight;
			m_heightRatioRecip = 1f / m_heightRatio;
			float num = (float)((double)radius * Math.PI * 0.5);
			m_pixelSize = num / (float)m_heightmap.Resolution;
			m_pixelSizeRecip2 = 0.5f / m_pixelSize;
			m_mapStepScale = m_pixelSize / m_heightRatio;
			m_mapStepScaleSquare = m_mapStepScale * m_mapStepScale;
			if (definition.Detail != null)
			{
				m_detail.Init(texProvider, definition.Detail, num);
			}
			VrPlanetShape.DetailMapData detailMapData = default(VrPlanetShape.DetailMapData);
			if (definition.Detail != null)
			{
				detailMapData = m_detail.GetDetailMapData();
			}
			VrPlanetShape.Mapset mapset = m_heightmap.GetMapset();
			m_nativeShape = new VrPlanetShape(translation, radius, definition.HillParams.Min, definition.HillParams.Max, mapset, detailMapData, useLegacyAtan: true);
			Closed = false;
		}

		public void Close()
		{
			m_nativeShape.Dispose();
			m_nativeShape = null;
			m_heightmap = null;
			m_detail.Dispose();
			Closed = true;
		}

		public void PrepareCache()
		{
			if (m_heightmap.Name != m_cache.Name)
			{
				m_cache.Name = m_heightmap.Name;
				m_cache.Clean();
			}
		}

		private unsafe void CalculateCacheCell(MyHeightmapFace map, Cache.Cell* cell, bool compouteBounds = false)
		{
			int x = cell->Coord.X;
			int y = cell->Coord.Y;
			fixed (float* ptr = &s_Cz.M11)
			{
				int num = map.GetRowStart(y - 1) + x - 1;
				ushort* data = map.Data;
				map.Get4Row(num, ptr, data);
				num += map.RowStride;
				map.Get4Row(num, ptr + 4, data);
				num += map.RowStride;
				map.Get4Row(num, ptr + 8, data);
				num += map.RowStride;
				map.Get4Row(num, ptr + 12, data);
				Matrix.Multiply(ref CR, ref s_Cz, out cell->Gz);
				Matrix.Multiply(ref cell->Gz, ref CRT, out cell->Gz);
				if (compouteBounds)
				{
					float num2 = float.PositiveInfinity;
					float num3 = float.NegativeInfinity;
					Matrix.Multiply(ref BInv, ref cell->Gz, out Matrix result);
					Matrix.Multiply(ref result, ref BInvT, out result);
					float* ptr2 = &result.M11;
					for (int i = 0; i < 16; i++)
					{
						if (num3 < ptr2[i])
						{
							num3 = ptr2[i];
						}
						if (num2 > ptr2[i])
						{
							num2 = ptr2[i];
						}
					}
					cell->Max = num3;
					cell->Min = num2;
				}
				else
				{
					cell->Max = 1f;
					cell->Min = 0f;
				}
			}
		}

		private float SampleHeightBicubic(float s, float t, ref Matrix Gz, out Vector3 Normal)
		{
			float num = s * s;
			float num2 = num * s;
			float num3 = t * t;
			float num4 = num3 * t;
			float num5 = Gz.M12 + Gz.M22 * t + Gz.M32 * num3 + Gz.M42 * num4;
			float num6 = Gz.M13 + Gz.M23 * t + Gz.M33 * num3 + Gz.M43 * num4;
			float num7 = Gz.M14 + Gz.M24 * t + Gz.M34 * num3 + Gz.M44 * num4;
			float result = Gz.M11 + Gz.M21 * t + Gz.M31 * num3 + Gz.M41 * num4 + s * num5 + num * num6 + num2 * num7;
			float x = num5 + 2f * s * num6 + 3f * num * num7;
			float y = Gz.M21 + Gz.M22 * s + Gz.M23 * num + Gz.M24 * num2 + 2f * t * (Gz.M31 + Gz.M32 * s + Gz.M33 * num + Gz.M34 * num2) + 3f * num3 * (Gz.M41 + Gz.M42 * s + Gz.M43 * num + Gz.M44 * num2);
			Normal = new Vector3(x, y, m_mapStepScale);
			Normal.Normalize();
			return result;
		}

		private float SampleHeightBilinear(MyHeightmapFace map, float lodSize, float s, float t, int sx, int sy, out Vector3 Normal)
		{
			float num = lodSize * m_pixelSizeRecip2;
			float num2 = 1f - s;
			float num3 = 1f - t;
			int x = Math.Min(sx + (int)Math.Ceiling(num), m_heightmap.Resolution);
			int y = Math.Min(sy + (int)Math.Ceiling(num), m_heightmap.Resolution);
			float valuef = map.GetValuef(sx, sy);
			float valuef2 = map.GetValuef(x, sy);
			float valuef3 = map.GetValuef(sx, y);
			float valuef4 = map.GetValuef(x, y);
			float result = valuef * num2 * num3 + valuef2 * s * num3 + valuef3 * num2 * t + valuef4 * s * t;
			float num4 = (valuef2 - valuef) * num3 + (valuef4 - valuef3) * t;
			float num5 = (valuef3 - valuef) * num2 + (valuef4 - valuef2) * s;
			Normal = new Vector3(m_mapStepScale * num4, m_mapStepScale * num5, m_mapStepScaleSquare);
			Normal.Normalize();
			return result;
		}

		internal float SignedDistanceLocalCacheless(Vector3 position)
		{
			float num = position.Length();
			if ((double)num > 0.1 && num >= InnerRadius - 1f)
			{
				if (num > OuterRadius + 1f)
				{
					return float.PositiveInfinity;
				}
				float num2 = num - m_radius;
				MyCubemapHelpers.CalculateSampleTexcoord(ref position, out int face, out Vector2 texCoord);
				Vector3 localNormal;
				float num3 = GetValueForPositionCacheless(face, ref texCoord, out localNormal);
				if (m_detail.Matches(localNormal.Z))
				{
					float num4 = texCoord.X * m_detail.Factor;
					float num5 = texCoord.Y * m_detail.Factor;
					num4 -= (float)Math.Floor(num4);
					num5 -= (float)Math.Floor(num5);
					num3 += m_detail.GetValue(num4, num5, localNormal.Z);
				}
				return (num2 - num3) * localNormal.Z;
			}
			return -1f;
		}

		public unsafe float GetValueForPositionCacheless(int face, ref Vector2 texcoord, out Vector3 localNormal)
		{
			if (m_heightmap == null)
			{
				localNormal = Vector3.Zero;
				return 0f;
			}
			Vector2 vector = texcoord * m_mapResolutionMinusOne;
			if (vector.X >= (float)m_heightmap.Resolution || vector.Y >= (float)m_heightmap.Resolution || vector.X < 0f || vector.Y < 0f)
			{
				localNormal = Vector3.Zero;
				return 0f;
			}
			Cache.Cell cell = default(Cache.Cell);
			Cache.Cell* ptr = &cell;
			ptr->Coord = new Vector3I((int)vector.X, (int)vector.Y, face);
			CalculateCacheCell(m_heightmap.Faces[face], ptr);
			return SampleHeightBicubic(vector.X - (float)Math.Floor(vector.X), vector.Y - (float)Math.Floor(vector.Y), ref ptr->Gz, out localNormal) * m_heightRatio + m_minHillHeight;
		}

		public float SignedDistanceWithSample(float lodVoxelSize, float distance, float value)
		{
			return distance - m_radius - value;
		}

		public bool ProjectToSurface(Vector3 localPos, out Vector3 surface)
		{
			float num = localPos.Length();
			if (num.IsZero())
			{
				surface = localPos;
				return false;
			}
			Vector3 value = localPos / num;
			MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out int face, out Vector2 texCoord);
			float valueForPositionCacheless = GetValueForPositionCacheless(face, ref texCoord, out Vector3 _);
			valueForPositionCacheless += Radius;
			surface = valueForPositionCacheless * value;
			return true;
		}

		public double GetDistanceToSurfaceWithCache(Vector3 localPos)
		{
			float num = localPos.Length();
			if (num.IsZero())
			{
				return 0.0;
			}
			MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out int face, out Vector2 texCoord);
			float valueForPositionWithCache = GetValueForPositionWithCache(face, ref texCoord, out Vector3 _);
			valueForPositionWithCache += Radius;
			return num - valueForPositionWithCache;
		}

		public double GetDistanceToSurfaceCacheless(Vector3 localPos)
		{
			float num = localPos.Length();
			if (num.IsZero())
			{
				return 0.0;
			}
			if (!num.IsValid())
			{
				return 0.0;
			}
			MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out int face, out Vector2 texCoord);
			float valueForPositionCacheless = GetValueForPositionCacheless(face, ref texCoord, out Vector3 _);
			valueForPositionCacheless += Radius;
			return num - valueForPositionCacheless;
		}

		public unsafe float GetValueForPositionWithCache(int face, ref Vector2 texcoord, out Vector3 localNormal)
		{
			Vector2 vector = texcoord * m_mapResolutionMinusOne;
			int x = (int)vector.X;
			int y = (int)vector.Y;
			Vector3I coord = new Vector3I(x, y, face);
			fixed (Cache.Cell* ptr = &m_cache.Cells[m_cache.CellCoord(ref coord)])
			{
				if (ptr->Coord != coord)
				{
					ptr->Coord = coord;
					CalculateCacheCell(m_heightmap.Faces[face], ptr);
				}
				return SampleHeightBicubic(vector.X - (float)Math.Floor(vector.X), vector.Y - (float)Math.Floor(vector.Y), ref ptr->Gz, out localNormal) * m_heightRatio + m_minHillHeight;
			}
		}

		public float GetValue(int face, ref Vector2 texcoord, out Vector3 localNormal)
		{
			return m_nativeShape.GetValue(ref texcoord, face, out localNormal);
		}

		public float GetHeight(int face, ref Vector2 texcoord, out Vector3 localNormal)
		{
			return m_nativeShape.GetValue(ref texcoord, face, out localNormal) * m_heightRatio + InnerRadius;
		}

		internal unsafe float GetValueForPositionInternal(int face, ref Vector2 texcoord, float lodSize, float distance, out Vector3 Normal)
		{
			Vector2 vector = texcoord * m_mapResolutionMinusOne;
			float s = vector.X - (float)Math.Floor(vector.X);
			float t = vector.Y - (float)Math.Floor(vector.Y);
			int num = (int)vector.X;
			int num2 = (int)vector.Y;
			float num5;
			if (lodSize < m_pixelSize)
			{
				Vector3I coord = new Vector3I(num, num2, face);
				fixed (Cache.Cell* ptr = &m_cache.Cells[m_cache.CellCoord(ref coord)])
				{
					if (ptr->Coord != coord)
					{
						ptr->Coord = coord;
						CalculateCacheCell(m_heightmap.Faces[face], ptr, compouteBounds: true);
					}
					float num3 = (distance - InnerRadius) * m_heightRatioRecip;
					float num4 = lodSize * m_heightRatioRecip;
					if (num3 > ptr->Max + num4)
					{
						Normal = Vector3.Backward;
						return float.NegativeInfinity;
					}
					if (num3 < ptr->Min - num4)
					{
						Normal = Vector3.Backward;
						return float.PositiveInfinity;
					}
					num5 = SampleHeightBicubic(vector.X - (float)Math.Floor(vector.X), vector.Y - (float)Math.Floor(vector.Y), ref ptr->Gz, out Normal);
				}
			}
			else
			{
				num5 = SampleHeightBilinear(m_heightmap.Faces[face], lodSize, s, t, num, num2, out Normal);
			}
			return num5 * m_heightRatio + m_minHillHeight;
		}

		internal float SignedDistanceLocal(Vector3 position, float lodVoxelSize)
		{
			float num = position.Length();
			if ((double)num > 0.1 && num >= InnerRadius - lodVoxelSize)
			{
				if (num > OuterRadius + lodVoxelSize)
				{
					return float.PositiveInfinity;
				}
				float num2 = num - m_radius;
				MyCubemapHelpers.CalculateSampleTexcoord(ref position, out int face, out Vector2 texCoord);
				Vector3 Normal;
				float num3 = GetValueForPositionInternal(face, ref texCoord, lodVoxelSize, num, out Normal);
				if (m_detail.Matches(Normal.Z))
				{
					float num4 = texCoord.X * m_detail.Factor;
					float num5 = texCoord.Y * m_detail.Factor;
					num4 -= (float)Math.Floor(num4);
					num5 -= (float)Math.Floor(num5);
					num3 += m_detail.GetValue(num4, num5, Normal.Z);
				}
				return (num2 - num3) * Normal.Z;
			}
			return 0f - lodVoxelSize;
		}

		internal float SignedDistanceLocal(Vector3 position, float lodVoxelSize, int face)
		{
			float num = position.Length();
			if ((double)num > 0.1 && num >= InnerRadius - lodVoxelSize)
			{
				if (num > OuterRadius + lodVoxelSize)
				{
					return float.PositiveInfinity;
				}
				float num2 = num - m_radius;
				MyCubemapHelpers.CalculateTexcoordForFace(ref position, face, out Vector2 texCoord);
				Vector3 Normal;
				float num3 = GetValueForPositionInternal(face, ref texCoord, lodVoxelSize, num, out Normal);
				if (m_detail.Matches(Normal.Z))
				{
					float num4 = texCoord.X * m_detail.Factor;
					float num5 = texCoord.Y * m_detail.Factor;
					num4 -= (float)Math.Floor(num4);
					num5 -= (float)Math.Floor(num5);
					num3 += m_detail.GetValue(num4, num5, Normal.Z);
				}
				return (num2 - num3) * Normal.Z;
			}
			return 0f - lodVoxelSize;
		}

		public float AltitudeToRatio(float altitude)
		{
			return (altitude - m_minHillHeight) * m_heightRatioRecip;
		}

		internal float DistanceToRatio(float distance)
		{
			return (distance - InnerRadius) * m_heightRatioRecip;
		}

		public ContainmentType IntersectBoundingBox(ref BoundingBox box, float lodLevel)
		{
			box.Inflate(1f);
			BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, OuterRadius + lodLevel);
			boundingSphere.Intersects(ref box, out bool result);
			if (!result)
			{
				return ContainmentType.Disjoint;
			}
			boundingSphere.Radius = InnerRadius - lodLevel;
			boundingSphere.Contains(ref box, out ContainmentType result2);
			if (result2 == ContainmentType.Contains)
			{
				return ContainmentType.Contains;
			}
			int boxFace;
			return IntersectBoundingBoxInternal(ref box, lodLevel, out boxFace);
		}

		private unsafe ContainmentType IntersectBoundingBoxCornerCase(uint faces, Vector3* vertices, float minHeight, float maxHeight)
		{
			BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, minHeight), new Vector3(float.NegativeInfinity, float.NegativeInfinity, maxHeight));
			query.Min.Z = (query.Min.Z - m_radius - m_detailScale - m_minHillHeight) * m_heightRatioRecip;
			query.Max.Z = (query.Max.Z - m_radius - m_minHillHeight) * m_heightRatioRecip;
			ContainmentType containmentType = (ContainmentType)(-1);
			ContainmentType containmentType2 = containmentType;
			for (int i = 0; i <= 5; i++)
			{
				if ((faces & (1 << i)) == 0L)
				{
					continue;
				}
				query.Min.X = float.PositiveInfinity;
				query.Min.Y = float.PositiveInfinity;
				query.Max.X = float.NegativeInfinity;
				query.Max.Y = float.NegativeInfinity;
				for (int j = 0; j < 8; j++)
				{
					MyCubemapHelpers.TexcoordCalculators[i](ref vertices[j], out Vector2 texcoord);
					if (texcoord.X < query.Min.X)
					{
						query.Min.X = texcoord.X;
					}
					if (texcoord.X > query.Max.X)
					{
						query.Max.X = texcoord.X;
					}
					if (texcoord.Y < query.Min.Y)
					{
						query.Min.Y = texcoord.Y;
					}
					if (texcoord.Y > query.Max.Y)
					{
						query.Max.Y = texcoord.Y;
					}
				}
				ContainmentType containmentType3 = m_heightmap.Faces[i].QueryHeight(ref query);
				if (containmentType3 != containmentType2)
				{
					if (containmentType2 != containmentType)
					{
						return ContainmentType.Intersects;
					}
					containmentType2 = containmentType3;
				}
			}
			return containmentType2;
		}

		protected unsafe ContainmentType IntersectBoundingBoxInternal(ref BoundingBox box, float lodLevel, out int boxFace)
		{
			int num = -1;
			uint num2 = 0u;
			bool flag = false;
			Vector3* ptr = stackalloc Vector3[8];
			box.GetCornersUnsafe(ptr);
			for (int i = 0; i < 8; i++)
			{
				MyCubemapHelpers.GetCubeFace(ref ptr[i], out int face);
				if (num == -1)
				{
					num = face;
				}
				else if (num != face)
				{
					flag = true;
				}
				num2 = (uint)((int)num2 | (1 << face));
			}
			float num3 = (!Vector3.Zero.IsInsideInclusive(ref box.Min, ref box.Max)) ? Vector3.Clamp(Vector3.Zero, box.Min, box.Max).Length() : 0f;
			Vector3 center = box.Center;
			Vector3 vector = default(Vector3);
			if (center.X < 0f)
			{
				vector.X = box.Min.X;
			}
			else
			{
				vector.X = box.Max.X;
			}
			if (center.Y < 0f)
			{
				vector.Y = box.Min.Y;
			}
			else
			{
				vector.Y = box.Max.Y;
			}
			if (center.Z < 0f)
			{
				vector.Z = box.Min.Z;
			}
			else
			{
				vector.Z = box.Max.Z;
			}
			float num4 = vector.Length();
			if (flag)
			{
				boxFace = -1;
				return IntersectBoundingBoxCornerCase(num2, ptr, num3, num4);
			}
			BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, num3), new Vector3(float.NegativeInfinity, float.NegativeInfinity, num4));
			for (int j = 0; j < 8; j++)
			{
				MyCubemapHelpers.CalculateTexcoordForFace(ref ptr[j], num, out Vector2 texCoord);
				if (texCoord.X < query.Min.X)
				{
					query.Min.X = texCoord.X;
				}
				if (texCoord.X > query.Max.X)
				{
					query.Max.X = texCoord.X;
				}
				if (texCoord.Y < query.Min.Y)
				{
					query.Min.Y = texCoord.Y;
				}
				if (texCoord.Y > query.Max.Y)
				{
					query.Max.Y = texCoord.Y;
				}
			}
			query.Min.Z = (query.Min.Z - m_radius - m_detailScale - m_minHillHeight) * m_heightRatioRecip;
			query.Max.Z = (query.Max.Z - m_radius - m_minHillHeight) * m_heightRatioRecip;
			boxFace = num;
			return m_heightmap.Faces[num].QueryHeight(ref query);
		}

		private bool IntersectLineCornerCase(ref LineD line, uint faces, out double startOffset, out double endOffset)
		{
			startOffset = 1.0;
			endOffset = 0.0;
			return true;
		}

		public bool IntersectLineFace(ref LineD ll, int face, out double startOffset, out double endOffset)
		{
			Vector3 localPos = ll.From;
			Vector3 localPos2 = ll.To;
			MyCubemapHelpers.CalculateTexcoordForFace(ref localPos, face, out Vector2 texCoord);
			MyCubemapHelpers.CalculateTexcoordForFace(ref localPos2, face, out Vector2 texCoord2);
			int num = (int)Math.Ceiling((texCoord2 - texCoord).Length() * (float)m_heightmap.Resolution);
			double num2 = 1.0 / (double)num;
			for (int i = 0; i < num; i++)
			{
				localPos = ll.From + ll.Direction * ll.Length * i * num2;
				localPos2 = ll.From + ll.Direction * ll.Length * (i + 1) * num2;
				float num3 = localPos.Length();
				float num4 = localPos2.Length();
				MyCubemapHelpers.CalculateTexcoordForFace(ref localPos, face, out texCoord);
				MyCubemapHelpers.CalculateTexcoordForFace(ref localPos2, face, out texCoord2);
				localPos.X = texCoord.X;
				localPos.Y = texCoord.Y;
				localPos.Z = (num3 - m_radius - m_minHillHeight) * m_heightRatioRecip;
				localPos2.X = texCoord2.X;
				localPos2.Y = texCoord2.Y;
				localPos2.Z = (num4 - m_radius - m_minHillHeight) * m_heightRatioRecip;
				if (m_heightmap[face].QueryLine(ref localPos, ref localPos2, out float startOffset2, out float _))
				{
					startOffset = Math.Max((double)((float)i + startOffset2) * num2, 0.0);
					endOffset = 1.0;
					return true;
				}
			}
			startOffset = 0.0;
			endOffset = 1.0;
			return false;
		}

		public unsafe bool IntersectLine(ref LineD ll, out double startOffset, out double endOffset)
		{
			BoundingBox boundingBox = (BoundingBox)ll.GetBoundingBox();
			int num = -1;
			uint num2 = 0u;
			bool flag = false;
			Vector3* ptr = stackalloc Vector3[8];
			boundingBox.GetCornersUnsafe(ptr);
			for (int i = 0; i < 8; i++)
			{
				MyCubemapHelpers.GetCubeFace(ref ptr[i], out int face);
				if (num == -1)
				{
					num = face;
				}
				else if (num != face)
				{
					flag = true;
				}
				num2 = (uint)((int)num2 | (1 << face));
			}
			if (flag)
			{
				return IntersectLineCornerCase(ref ll, num2, out startOffset, out endOffset);
			}
			return IntersectLineFace(ref ll, num, out startOffset, out endOffset);
		}

		internal void ReadContentRange(ref MyVoxelDataRequest req, bool detectOnly = false)
		{
			if (Closed)
			{
				return;
			}
			float num = (float)(1 << req.Lod) * 1f;
			Vector3I minInLod = req.MinInLod;
			Vector3I maxInLod = req.MaxInLod;
			Vector3 vector = minInLod * num - m_translation;
			int boxFace = -1;
			MyVoxelRequestFlags requestFlags = req.RequestFlags;
			requestFlags &= ~(MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent | MyVoxelRequestFlags.ContentChecked | MyVoxelRequestFlags.ContentCheckedDeep);
			if ((req.MinInLod - req.MaxInLod).Size > 8)
			{
				BoundingBox box = new BoundingBox(vector, vector + (maxInLod - minInLod) * num);
				box.Inflate(num);
				BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, OuterRadius + num);
				boundingSphere.Intersects(ref box, out bool result);
				ContainmentType containmentType;
				if (!result)
				{
					containmentType = ContainmentType.Disjoint;
				}
				else
				{
					boundingSphere.Radius = InnerRadius - num;
					boundingSphere.Contains(ref box, out ContainmentType result2);
					if (result2 == ContainmentType.Contains)
					{
						containmentType = result2;
					}
					else
					{
						containmentType = IntersectBoundingBoxInternal(ref box, num, out boxFace);
						if (containmentType == ContainmentType.Intersects)
						{
							goto IL_00fd;
						}
					}
				}
				switch (containmentType)
				{
				case ContainmentType.Disjoint:
					if (req.RequestFlags.HasFlags(MyVoxelRequestFlags.ContentChecked))
					{
						requestFlags |= (MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.ContentChecked | MyVoxelRequestFlags.ContentCheckedDeep);
					}
					else
					{
						req.Target.BlockFillContent(req.Offset, req.Offset + maxInLod - minInLod, 0);
					}
					break;
				case ContainmentType.Contains:
					if (req.RequestFlags.HasFlags(MyVoxelRequestFlags.ContentChecked))
					{
						requestFlags |= (MyVoxelRequestFlags.FullContent | MyVoxelRequestFlags.ContentChecked | MyVoxelRequestFlags.ContentCheckedDeep);
					}
					else
					{
						req.Target.BlockFillContent(req.Offset, req.Offset + maxInLod - minInLod, byte.MaxValue);
					}
					break;
				}
				req.Flags = requestFlags;
				return;
			}
			goto IL_00fd;
			IL_00fd:
			if (detectOnly)
			{
				if (m_nativeShape != null)
				{
					switch (m_nativeShape.CheckContentRange(ref req.MinInLod, ref req.MaxInLod, num, boxFace))
					{
					case 0:
						requestFlags |= MyVoxelRequestFlags.EmptyData;
						break;
					case 255:
						requestFlags |= MyVoxelRequestFlags.FullContent;
						break;
					}
				}
			}
			else if (m_nativeShape != null)
			{
				int offset = req.Target.ComputeLinear(ref req.Offset);
				Vector3I strides = req.Target.Step;
				m_nativeShape.ReadContentRange(req.Target[MyStorageDataTypeEnum.Content], offset, ref strides, ref req.MinInLod, ref req.MaxInLod, num, boxFace);
			}
			else
			{
				MyStorageData target = req.Target;
				Vector3I writeOffsetLoc = req.Offset - minInLod;
				CalculateDistanceFieldInternal(vector, boxFace, minInLod, maxInLod, writeOffsetLoc, target, num);
			}
			req.Flags = requestFlags;
		}

		private void CalculateDistanceFieldInternal(Vector3 localPos, int faceHint, Vector3I min, Vector3I max, Vector3I writeOffsetLoc, MyStorageData target, float lodVoxelSize)
		{
			PrepareCache();
			Vector3 vector = localPos;
			Vector3I a = default(Vector3I);
			if (faceHint == -1)
			{
				a.Z = min.Z;
				while (a.Z <= max.Z)
				{
					a.Y = min.Y;
					while (a.Y <= max.Y)
					{
						a.X = min.X;
						Vector3I p = a + writeOffsetLoc;
						int num = target.ComputeLinear(ref p);
						while (a.X <= max.X)
						{
							byte content = (byte)((MathHelper.Clamp(0f - SignedDistanceLocal(localPos, lodVoxelSize) / lodVoxelSize, -1f, 1f) * 0.5f + 0.5f) * 255f);
							target.Content(num, content);
							num += target.StepLinear;
							localPos.X += lodVoxelSize;
							a.X++;
						}
						localPos.Y += lodVoxelSize;
						localPos.X = vector.X;
						a.Y++;
					}
					localPos.Z += lodVoxelSize;
					localPos.Y = vector.Y;
					a.Z++;
				}
				return;
			}
			a.Z = min.Z;
			while (a.Z <= max.Z)
			{
				a.Y = min.Y;
				while (a.Y <= max.Y)
				{
					a.X = min.X;
					Vector3I p2 = a + writeOffsetLoc;
					int num2 = target.ComputeLinear(ref p2);
					while (a.X <= max.X)
					{
						byte content2 = (byte)((MathHelper.Clamp(0f - SignedDistanceLocal(localPos, lodVoxelSize, faceHint) / lodVoxelSize, -1f, 1f) * 0.5f + 0.5f) * 255f);
						target.Content(num2, content2);
						num2 += target.StepLinear;
						localPos.X += lodVoxelSize;
						a.X++;
					}
					localPos.Y += lodVoxelSize;
					localPos.X = vector.X;
					a.Y++;
				}
				localPos.Z += lodVoxelSize;
				localPos.Y = vector.Y;
				a.Z++;
			}
		}

		public unsafe void GetBounds(Vector3D* localPoints, int pointCount, out float minHeight, out float maxHeight)
		{
			int num = -1;
			for (int i = 0; i < pointCount; i++)
			{
				Vector3 position = localPoints[i];
				MyCubemapHelpers.GetCubeFace(ref position, out int face);
				if (num == -1)
				{
					num = face;
				}
			}
			BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0f), new Vector3(float.NegativeInfinity, float.NegativeInfinity, 0f));
			for (int j = 0; j < pointCount; j++)
			{
				Vector3 localPos = localPoints[j];
				MyCubemapHelpers.CalculateTexcoordForFace(ref localPos, num, out Vector2 texCoord);
				if (texCoord.X < query.Min.X)
				{
					query.Min.X = texCoord.X;
				}
				if (texCoord.X > query.Max.X)
				{
					query.Max.X = texCoord.X;
				}
				if (texCoord.Y < query.Min.Y)
				{
					query.Min.Y = texCoord.Y;
				}
				if (texCoord.Y > query.Max.Y)
				{
					query.Max.Y = texCoord.Y;
				}
			}
			m_heightmap.Faces[num].GetBounds(ref query);
			minHeight = query.Min.Z * m_heightRatio + InnerRadius;
			maxHeight = query.Max.Z * m_heightRatio + InnerRadius;
		}

		public unsafe void GetBounds(Vector3* localPoints, int pointCount, out float minHeight, out float maxHeight)
		{
			int num = -1;
			for (int i = 0; i < pointCount; i++)
			{
				MyCubemapHelpers.GetCubeFace(ref localPoints[i], out int face);
				if (num == -1)
				{
					num = face;
				}
			}
			BoundingBox query = new BoundingBox(new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0f), new Vector3(float.NegativeInfinity, float.NegativeInfinity, 0f));
			for (int j = 0; j < pointCount; j++)
			{
				MyCubemapHelpers.CalculateTexcoordForFace(ref localPoints[j], num, out Vector2 texCoord);
				float f = texCoord.X * texCoord.Y;
				if (!float.IsNaN(f) && !float.IsInfinity(f))
				{
					if (texCoord.X < query.Min.X)
					{
						query.Min.X = texCoord.X;
					}
					if (texCoord.X > query.Max.X)
					{
						query.Max.X = texCoord.X;
					}
					if (texCoord.Y < query.Min.Y)
					{
						query.Min.Y = texCoord.Y;
					}
					if (texCoord.Y > query.Max.Y)
					{
						query.Max.Y = texCoord.Y;
					}
				}
			}
			m_heightmap.Faces[num].GetBounds(ref query);
			minHeight = query.Min.Z * m_heightRatio + InnerRadius;
			maxHeight = query.Max.Z * m_heightRatio + InnerRadius;
		}

		public unsafe void GetBounds(ref BoundingBox box)
		{
			Vector3* ptr = stackalloc Vector3[8];
			box.GetCornersUnsafe(ptr);
			GetBounds(ptr, 8, out box.Min.Z, out box.Max.Z);
		}

		public void GetBounds(ref BoundingBox2 texcoordRange, int face, out float min, out float max)
		{
			BoundingBox query = new BoundingBox(new Vector3(texcoordRange.Min, 0f), new Vector3(texcoordRange.Max, 0f));
			m_heightmap.Faces[face].GetBounds(ref query);
			min = query.Min.Z;
			max = query.Max.Z;
		}
	}
}
