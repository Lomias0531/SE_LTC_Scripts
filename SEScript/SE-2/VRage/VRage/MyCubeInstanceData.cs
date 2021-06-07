using System;
using VRageMath;

namespace VRage
{
	public struct MyCubeInstanceData
	{
		private unsafe fixed byte m_bones[32];

		public Vector4 m_translationAndRot;

		public Vector4 ColorMaskHSV;

		public Matrix LocalMatrix
		{
			get
			{
				Vector4.UnpackOrthoMatrix(ref m_translationAndRot, out Matrix matrix);
				return matrix;
			}
			set
			{
				m_translationAndRot = Vector4.PackOrthoMatrix(ref value);
			}
		}

		/// <summary>
		/// Gets translation, faster than getting local matrix
		/// </summary>
		public Vector3 Translation => new Vector3(m_translationAndRot);

		public Vector4 PackedOrthoMatrix
		{
			get
			{
				return m_translationAndRot;
			}
			set
			{
				m_translationAndRot = value;
			}
		}

		public unsafe float BoneRange
		{
			get
			{
				fixed (byte* ptr = m_bones)
				{
					return (float)(int)((Vector4UByte*)(ptr + 4L * (long)sizeof(Vector4UByte)))->W / 10f;
				}
			}
			set
			{
				fixed (byte* ptr = m_bones)
				{
					((Vector4UByte*)(ptr + 4L * (long)sizeof(Vector4UByte)))->W = (byte)(value * 10f);
				}
			}
		}

		public unsafe bool EnableSkinning
		{
			get
			{
				fixed (byte* ptr = m_bones)
				{
					return (((Vector4UByte*)(ptr + 3L * (long)sizeof(Vector4UByte)))->W & 1) > 0;
				}
			}
			set
			{
				fixed (byte* ptr = m_bones)
				{
					if (value)
					{
						byte* w = &((Vector4UByte*)(ptr + 3L * (long)sizeof(Vector4UByte)))->W;
						*w = (byte)(*w | 1);
					}
					else
					{
						byte* w2 = &((Vector4UByte*)(ptr + 3L * (long)sizeof(Vector4UByte)))->W;
						*w2 = (byte)(*w2 & 0xFE);
					}
				}
			}
		}

		public unsafe Vector3UByte this[int index]
		{
			get
			{
				fixed (byte* ptr = m_bones)
				{
					if (index == 8)
					{
						return new Vector3UByte(((Vector4UByte*)ptr)->W, ((Vector4UByte*)(ptr + sizeof(Vector4UByte)))->W, ((Vector4UByte*)(ptr + 2L * (long)sizeof(Vector4UByte)))->W);
					}
					return *(Vector3UByte*)(ptr + (long)index * (long)sizeof(Vector4UByte));
				}
			}
			set
			{
				fixed (byte* ptr = m_bones)
				{
					if (index == 8)
					{
						((Vector4UByte*)ptr)->W = value.X;
						((Vector4UByte*)(ptr + sizeof(Vector4UByte)))->W = value.Y;
						((Vector4UByte*)(ptr + 2L * (long)sizeof(Vector4UByte)))->W = value.Z;
					}
					else
					{
						*(Vector3UByte*)(ptr + (long)index * (long)sizeof(Vector4UByte)) = value;
					}
				}
			}
		}

		public Matrix ConstructDeformedCubeInstanceMatrix(ref Vector4UByte boneIndices, ref Vector4 boneWeights, out Matrix localMatrix)
		{
			localMatrix = LocalMatrix;
			Matrix result = localMatrix;
			if (EnableSkinning)
			{
				Vector3 value = ComputeBoneOffset(ref boneIndices, ref boneWeights);
				Vector3 translation = result.Translation;
				translation = (result.Translation = translation + value);
			}
			return result;
		}

		public Vector3 ComputeBoneOffset(ref Vector4UByte boneIndices, ref Vector4 boneWeights)
		{
			Matrix matrix = default(Matrix);
			Vector4 normalizedBone = GetNormalizedBone(boneIndices[0]);
			Vector4 normalizedBone2 = GetNormalizedBone(boneIndices[1]);
			Vector4 normalizedBone3 = GetNormalizedBone(boneIndices[2]);
			Vector4 normalizedBone4 = GetNormalizedBone(boneIndices[3]);
			matrix.SetRow(0, normalizedBone);
			matrix.SetRow(1, normalizedBone2);
			matrix.SetRow(2, normalizedBone3);
			matrix.SetRow(3, normalizedBone4);
			return Denormalize(Vector4.Transform(boneWeights, matrix), BoneRange);
		}

		public unsafe void RetrieveBones(byte* bones)
		{
			fixed (byte* ptr = m_bones)
			{
				for (int i = 0; i < 32; i++)
				{
					bones[i] = ptr[i];
				}
			}
		}

		/// <summary>
		/// Resets bones to zero and disables skinning
		/// </summary>
		public unsafe void ResetBones()
		{
			fixed (byte* ptr = m_bones)
			{
				ulong* ptr2 = (ulong*)ptr;
				*ptr2 = 9259542123273814144uL;
				ptr2[1] = 36170086419038336uL;
				ptr2[2] = 9259542123273814144uL;
				ptr2[3] = 36170086419038336uL;
			}
		}

		public unsafe void SetTextureOffset(Vector4UByte patternOffset)
		{
			fixed (byte* ptr = m_bones)
			{
				((Vector4UByte*)(ptr + 5L * (long)sizeof(Vector4UByte)))->W = patternOffset.X;
				((Vector4UByte*)(ptr + 6L * (long)sizeof(Vector4UByte)))->W = patternOffset.Y;
				((Vector4UByte*)(ptr + 7L * (long)sizeof(Vector4UByte)))->W = (byte)((patternOffset.W - 1) | (patternOffset.Z - 1 << 4));
			}
		}

		public unsafe float GetTextureOffset(int index)
		{
			fixed (byte* ptr = m_bones)
			{
				int num = ((Vector4UByte*)(ptr + (long)(5 + index) * (long)sizeof(Vector4UByte)))->W & 0xF;
				int num2 = (((Vector4UByte*)(ptr + (long)(5 + index) * (long)sizeof(Vector4UByte)))->W >> 4) & 0x10;
				return (num2 != 0) ? (num / num2) : 0;
			}
		}

		public unsafe void SetColorMaskHSV(Vector4 colorMaskHSV)
		{
			ColorMaskHSV = colorMaskHSV;
			fixed (byte* ptr = m_bones)
			{
				if (colorMaskHSV.W < 0f)
				{
					byte* w = &((Vector4UByte*)(ptr + 3L * (long)sizeof(Vector4UByte)))->W;
					*w = (byte)(*w | 2);
				}
				else
				{
					byte* w2 = &((Vector4UByte*)(ptr + 3L * (long)sizeof(Vector4UByte)))->W;
					*w2 = (byte)(*w2 & 0xFD);
				}
			}
			ColorMaskHSV.W = Math.Abs(ColorMaskHSV.W);
		}

		public Vector3 GetDenormalizedBone(int index)
		{
			return Denormalize(GetNormalizedBone(index), BoneRange);
		}

		public unsafe Vector4UByte GetPackedBone(int index)
		{
			fixed (byte* ptr = m_bones)
			{
				if (index == 8)
				{
					return new Vector4UByte(((Vector4UByte*)ptr)->W, ((Vector4UByte*)(ptr + sizeof(Vector4UByte)))->W, ((Vector4UByte*)(ptr + 2L * (long)sizeof(Vector4UByte)))->W, 0);
				}
				return *(Vector4UByte*)(ptr + (long)index * (long)sizeof(Vector4UByte));
			}
		}

		/// <returns>Vector in range [0,1]</returns>
		private Vector4 GetNormalizedBone(int index)
		{
			Vector4UByte packedBone = GetPackedBone(index);
			return new Vector4((int)packedBone.X, (int)packedBone.Y, (int)packedBone.Z, (int)packedBone.W) / 255f;
		}

		/// <param name="position">Scaled in range [0,1]</param>
		/// <param name="range">Unscaled</param>
		/// <returns>Unscaled position</returns>
		private Vector3 Denormalize(Vector4 position, float range)
		{
			return (new Vector3(position) + 0.00196078443f - 0.5f) * range * 2f;
		}
	}
}
