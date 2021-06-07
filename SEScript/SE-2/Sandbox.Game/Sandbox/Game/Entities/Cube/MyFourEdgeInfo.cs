using Sandbox.Definitions;
using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyFourEdgeInfo
	{
		private struct Data
		{
			public Vector4 LocalOrthoMatrix;

			public MyCubeEdgeType EdgeType;

			private unsafe fixed uint m_data[4];

			private unsafe fixed byte m_data2[4];

			private unsafe fixed int m_edgeModels[4];

			private unsafe fixed int m_skinSubtypes[4];

			public unsafe bool Full
			{
				get
				{
					fixed (uint* ptr = m_data)
					{
						return (*ptr != 0) & (ptr[1] != 0) & (ptr[2] != 0) & (ptr[3] != 0);
					}
				}
			}

			public unsafe bool Empty
			{
				get
				{
					fixed (uint* ptr = m_data)
					{
						return (*(long*)ptr == 0) & (*(long*)(ptr + 2) == 0);
					}
				}
			}

			public unsafe int Count
			{
				get
				{
					fixed (uint* ptr = m_data)
					{
						return ((*ptr != 0) ? 1 : 0) + ((ptr[1] != 0) ? 1 : 0) + ((ptr[2] != 0) ? 1 : 0) + ((ptr[3] != 0) ? 1 : 0);
					}
				}
			}

			public unsafe int FirstAvailable
			{
				get
				{
					fixed (uint* ptr = m_data)
					{
						if (*ptr == 0)
						{
							if (ptr[1] == 0)
							{
								if (ptr[2] == 0)
								{
									if (ptr[3] == 0)
									{
										return -1;
									}
									return 3;
								}
								return 2;
							}
							return 1;
						}
						return 0;
					}
				}
			}

			public unsafe uint Get(int index)
			{
				fixed (uint* ptr = m_data)
				{
					return ptr[index];
				}
			}

			public unsafe void Get(int index, out Color color, out MyStringHash skinSubtypeId, out MyStringHash edgeModel, out Base27Directions.Direction normal0, out Base27Directions.Direction normal1)
			{
				fixed (uint* ptr = m_data)
				{
					fixed (byte* ptr2 = m_data2)
					{
						fixed (int* ptr3 = m_edgeModels)
						{
							fixed (int* ptr4 = m_skinSubtypes)
							{
								color = new Color(ptr[index]);
								normal0 = (Base27Directions.Direction)color.A;
								normal1 = (Base27Directions.Direction)ptr2[index];
								edgeModel = MyStringHash.TryGet(ptr3[index]);
								skinSubtypeId = MyStringHash.TryGet(ptr4[index]);
							}
						}
					}
				}
			}

			public unsafe bool Set(int index, Color value, MyStringHash skinSubtype, MyStringHash edgeModel, Base27Directions.Direction normal0, Base27Directions.Direction normal1)
			{
				fixed (uint* ptr = m_data)
				{
					fixed (byte* ptr2 = m_data2)
					{
						fixed (int* ptr3 = m_edgeModels)
						{
							fixed (int* ptr4 = m_skinSubtypes)
							{
								value.A = (byte)normal0;
								uint packedValue = value.PackedValue;
								bool result = false;
								if (ptr[index] != packedValue)
								{
									result = true;
									ptr[index] = packedValue;
								}
								ptr2[index] = (byte)normal1;
								ptr3[index] = (int)edgeModel;
								if (ptr4[index] != (int)skinSubtype)
								{
									result = true;
									ptr4[index] = (int)skinSubtype;
								}
								return result;
							}
						}
					}
				}
			}

			public unsafe bool Reset(int index)
			{
				fixed (uint* ptr = m_data)
				{
					fixed (int* ptr2 = m_edgeModels)
					{
						fixed (int* ptr3 = m_skinSubtypes)
						{
							bool result = ptr[index] != 0;
							ptr[index] = 0u;
							ptr2[index] = 0;
							ptr3[index] = 0;
							return result;
						}
					}
				}
			}
		}

		private static readonly int DirectionMax = MyUtils.GetMaxValueFromEnum<Base27Directions.Direction>() + 1;

		public const int MaxInfoCount = 4;

		private Data m_data;

		public Vector4 LocalOrthoMatrix => m_data.LocalOrthoMatrix;

		public MyCubeEdgeType EdgeType => m_data.EdgeType;

		public bool Empty => m_data.Empty;

		public bool Full => m_data.Full;

		public int DebugCount => m_data.Count;

		public int FirstAvailable => m_data.FirstAvailable;

		public MyFourEdgeInfo(Vector4 localOrthoMatrix, MyCubeEdgeType edgeType)
		{
			m_data.LocalOrthoMatrix = localOrthoMatrix;
			m_data.EdgeType = edgeType;
		}

		public bool AddInstance(Vector3 blockPos, Color color, MyStringHash skinSubtype, MyStringHash edgeModel, Base27Directions.Direction normal0, Base27Directions.Direction normal1)
		{
			return m_data.Set(GetIndex(ref blockPos), color, skinSubtype, edgeModel, normal0, normal1);
		}

		public bool RemoveInstance(Vector3 blockPos)
		{
			return m_data.Reset(GetIndex(ref blockPos));
		}

		private int GetIndex(ref Vector3 blockPos)
		{
			Vector3 vector = blockPos - new Vector3(LocalOrthoMatrix);
			if (Math.Abs(vector.X) < 1E-05f)
			{
				return ((vector.Y > 0f) ? 1 : 0) + ((vector.Z > 0f) ? 2 : 0);
			}
			if (Math.Abs(vector.Y) < 1E-05f)
			{
				return ((vector.X > 0f) ? 1 : 0) + ((vector.Z > 0f) ? 2 : 0);
			}
			return ((vector.X > 0f) ? 1 : 0) + ((vector.Y > 0f) ? 2 : 0);
		}

		public bool GetNormalInfo(int index, out Color color, out MyStringHash skinSubtypeId, out MyStringHash edgeModel, out Base27Directions.Direction normal0, out Base27Directions.Direction normal1)
		{
			m_data.Get(index, out color, out skinSubtypeId, out edgeModel, out normal0, out normal1);
			color.A = 0;
			return normal0 != (Base27Directions.Direction)0;
		}
	}
}
