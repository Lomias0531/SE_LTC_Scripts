using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Entities.Character
{
	[Obsolete]
	public static class MyInverseKinematics
	{
		public struct CastHit
		{
			public Vector3 Position;

			public Vector3 Normal;
		}

		public static bool SolveCCDIk(ref Vector3 desiredEnd, List<MyCharacterBone> bones, float stopDistance, int maxTries, float gain, ref Matrix finalTransform, MyCharacterBone finalBone = null, bool allowFinalBoneTranslation = true)
		{
			MyCharacterBone myCharacterBone = bones.Last();
			int num = 0;
			Vector3D value = Vector3.Zero;
			do
			{
				foreach (MyCharacterBone item in Enumerable.Reverse(bones))
				{
					myCharacterBone.ComputeAbsoluteTransform();
					Matrix absoluteTransform = item.AbsoluteTransform;
					Vector3D value2 = absoluteTransform.Translation;
					value = myCharacterBone.AbsoluteTransform.Translation;
					if (Vector3D.DistanceSquared(value, desiredEnd) > (double)stopDistance)
					{
						Vector3D vector3D = value - value2;
						Vector3D v = desiredEnd - value2;
						vector3D.Normalize();
						v.Normalize();
						double num2 = vector3D.Dot(v);
						if (num2 < 1.0)
						{
							Vector3D v2 = vector3D.Cross(v);
							v2.Normalize();
							double num3 = Math.Acos(num2);
							Matrix matrix = Matrix.CreateFromAxisAngle(v2, (float)num3 * gain);
							Matrix matrix2 = Matrix.Normalize(absoluteTransform).GetOrientation() * matrix;
							Matrix matrix3 = Matrix.Identity;
							if (item.Parent != null)
							{
								matrix3 = item.Parent.AbsoluteTransform;
							}
							matrix3 = Matrix.Normalize(matrix3);
							Matrix matrix4 = Matrix.Multiply(matrix2, Matrix.Invert(item.BindTransform * matrix3));
							item.Rotation = Quaternion.CreateFromRotationMatrix(matrix4);
							item.ComputeAbsoluteTransform();
						}
					}
				}
			}
			while (num++ < maxTries && Vector3D.DistanceSquared(value, desiredEnd) > (double)stopDistance);
			if (finalBone != null && finalTransform.IsValid())
			{
				MatrixD matrixD = (!allowFinalBoneTranslation) ? (finalTransform.GetOrientation() * MatrixD.Invert((MatrixD)finalBone.BindTransform * finalBone.Parent.AbsoluteTransform)) : (finalTransform * MatrixD.Invert((MatrixD)finalBone.BindTransform * finalBone.Parent.AbsoluteTransform));
				finalBone.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.Normalize(matrixD.GetOrientation()));
				if (allowFinalBoneTranslation)
				{
					finalBone.Translation = matrixD.Translation;
				}
				finalBone.ComputeAbsoluteTransform();
			}
			return Vector3D.DistanceSquared(value, desiredEnd) <= (double)stopDistance;
		}

		public static bool SolveTwoJointsIk(ref Vector3 desiredEnd, MyCharacterBone firstBone, MyCharacterBone secondBone, MyCharacterBone endBone, ref Matrix finalTransform, Matrix WorldMatrix, Vector3 normal, bool preferPositiveAngle = true, MyCharacterBone finalBone = null, bool allowFinalBoneTranslation = true, bool minimizeRotation = true)
		{
			throw new NotImplementedException();
		}

		public static bool SolveTwoJointsIkCCD(MyCharacterBone[] characterBones, int firstBoneIndex, int secondBoneIndex, int endBoneIndex, ref Matrix finalTransform, ref MatrixD worldMatrix, MyCharacterBone finalBone = null, bool allowFinalBoneTranslation = true)
		{
			if (finalBone == null)
			{
				return false;
			}
			Vector3 translation = finalTransform.Translation;
			int num = 0;
			int num2 = 50;
			float num3 = 2.5E-05f;
			_ = characterBones[firstBoneIndex];
			_ = characterBones[secondBoneIndex];
			MyCharacterBone myCharacterBone = characterBones[endBoneIndex];
			int[] array = new int[3];
			array[2] = firstBoneIndex;
			array[1] = secondBoneIndex;
			array[0] = endBoneIndex;
			Vector3 zero = Vector3.Zero;
			for (int i = 0; i < 3; i++)
			{
				MyCharacterBone obj = characterBones[array[i]];
				Vector3 translation2 = obj.BindTransform.Translation;
				Quaternion rotation = Quaternion.CreateFromRotationMatrix(obj.BindTransform);
				obj.SetCompleteTransform(ref translation2, ref rotation);
				obj.ComputeAbsoluteTransform();
			}
			myCharacterBone.ComputeAbsoluteTransform();
			zero = myCharacterBone.AbsoluteTransform.Translation;
			float num4 = 1f / (float)Vector3D.DistanceSquared(zero, translation);
			do
			{
				for (int j = 0; j < 3; j++)
				{
					MyCharacterBone myCharacterBone2 = characterBones[array[j]];
					myCharacterBone.ComputeAbsoluteTransform();
					Matrix absoluteTransform = myCharacterBone2.AbsoluteTransform;
					Vector3 translation3 = absoluteTransform.Translation;
					zero = myCharacterBone.AbsoluteTransform.Translation;
					double num5 = Vector3D.DistanceSquared(zero, translation);
					if (!(num5 > (double)num3))
					{
						continue;
					}
					Vector3 fromVector = zero - translation3;
					Vector3 vector = translation - translation3;
					double num6 = fromVector.LengthSquared();
					double num7 = vector.LengthSquared();
					double num8 = fromVector.Dot(vector);
					if (num8 < 0.0 || num8 * num8 < num6 * num7 * 0.99998998641967773)
					{
						float amount = 1f / (num4 * (float)num5 + 1f);
						Vector3 toVector = Vector3.Lerp(fromVector, vector, amount);
						Matrix.CreateRotationFromTwoVectors(ref fromVector, ref toVector, out Matrix resultMatrix);
						Matrix matrix = Matrix.Normalize(absoluteTransform).GetOrientation() * resultMatrix;
						Matrix matrix2 = Matrix.Identity;
						if (myCharacterBone2.Parent != null)
						{
							matrix2 = myCharacterBone2.Parent.AbsoluteTransform;
						}
						matrix2 = Matrix.Normalize(matrix2);
						Matrix matrix3 = Matrix.Multiply(matrix, Matrix.Invert(myCharacterBone2.BindTransform * matrix2));
						myCharacterBone2.Rotation = Quaternion.CreateFromRotationMatrix(matrix3);
						myCharacterBone2.ComputeAbsoluteTransform();
					}
				}
			}
			while (num++ < num2 && Vector3D.DistanceSquared(zero, translation) > (double)num3);
			if (finalTransform.IsValid())
			{
				MatrixD matrixD = (!allowFinalBoneTranslation) ? (finalTransform.GetOrientation() * MatrixD.Invert((MatrixD)finalBone.BindTransform * finalBone.Parent.AbsoluteTransform)) : (finalTransform * MatrixD.Invert((MatrixD)finalBone.BindTransform * finalBone.Parent.AbsoluteTransform));
				finalBone.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.Normalize(matrixD.GetOrientation()));
				if (allowFinalBoneTranslation)
				{
					finalBone.Translation = matrixD.Translation;
				}
				finalBone.ComputeAbsoluteTransform();
			}
			return true;
		}

		public static void RotateBone(MyCharacterBone bone, Vector3 planeNormal, double angle)
		{
			Matrix matrix = Matrix.CreateFromAxisAngle(planeNormal, (float)angle);
			Matrix matrix2 = bone.AbsoluteTransform * matrix;
			Matrix matrix3 = (bone.Parent != null) ? bone.Parent.AbsoluteTransform : Matrix.Identity;
			Matrix matrix4 = Matrix.Multiply(matrix2, Matrix.Invert(bone.BindTransform * matrix3));
			bone.Rotation = Quaternion.CreateFromRotationMatrix(matrix4);
			bone.ComputeAbsoluteTransform();
		}

		public static double GetAngle(Vector3 a, Vector3 b)
		{
			return Math.Acos(MathHelper.Clamp(Vector3.Dot(Vector3.Normalize(a), Vector3.Normalize(b)), -1f, 1f));
		}

		public static double GetAngleSigned(Vector3 a, Vector3 b, Vector3 normal)
		{
			double num = Math.Acos(MathHelper.Clamp(Vector3.Dot(Vector3.Normalize(a), Vector3.Normalize(b)), -1f, 1f));
			if (Vector3.Dot(normal, Vector3.Cross(a, b)) < 0f)
			{
				num = 0.0 - num;
			}
			return num;
		}

		public static void CosineLaw(float A, float B, float C, out double alpha, out double beta)
		{
			double value = (0f - (B * B - A * A - C * C)) / (2f * A * C);
			value = MathHelper.Clamp(value, -1.0, 1.0);
			alpha = Math.Acos(value);
			double value2 = (0f - (C * C - A * A - B * B)) / (2f * A * B);
			value2 = MathHelper.Clamp(value2, -1.0, 1.0);
			beta = Math.Acos(value2);
		}

		public static bool SolveTwoJointsIk(ref Vector3 desiredEnd, MyCharacterBone firstBone, MyCharacterBone secondBone, MyCharacterBone endBone, ref Matrix finalTransform, Matrix WorldMatrix, MyCharacterBone finalBone = null, bool allowFinalBoneTranslation = true)
		{
			Matrix absoluteTransform = firstBone.AbsoluteTransform;
			Matrix absoluteTransform2 = secondBone.AbsoluteTransform;
			Matrix absoluteTransform3 = endBone.AbsoluteTransform;
			Vector3 translation = absoluteTransform.Translation;
			Vector3 vector = absoluteTransform3.Translation - translation;
			Vector3 vector2 = desiredEnd - translation;
			Vector3 vector3 = absoluteTransform2.Translation - translation;
			Vector3 vector4 = vector - vector3;
			float num = vector3.Length();
			float num2 = vector4.Length();
			float num3 = vector2.Length();
			float num4 = vector.Length();
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
			{
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(desiredEnd, WorldMatrix), 0.01f, Color.Red, 1f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3.Transform(translation, WorldMatrix), Vector3.Transform(translation + vector, WorldMatrix), Color.Yellow, Color.Yellow, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3.Transform(translation, WorldMatrix), Vector3.Transform(translation + vector2, WorldMatrix), Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3.Transform(translation, WorldMatrix), Vector3.Transform(translation + vector3, WorldMatrix), Color.Green, Color.Green, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3.Transform(translation + vector3, WorldMatrix), Vector3.Transform(translation + vector3 + vector4, WorldMatrix), Color.Blue, Color.Blue, depthRead: false);
			}
			bool flag = num + num2 > num3;
			double num5 = 0.0;
			double num6 = 0.0;
			if (flag)
			{
				num5 = Math.Acos(MathHelper.Clamp((0f - (num2 * num2 - num * num - num3 * num3)) / (2f * num * num3), -1.0, 1.0));
				num6 = Math.Acos(MathHelper.Clamp((0f - (num3 * num3 - num * num - num2 * num2)) / (2f * num * num2), -1.0, 1.0));
				num6 = Math.PI - num6;
			}
			double num7 = Math.Acos(MathHelper.Clamp((0f - (num2 * num2 - num * num - num4 * num4)) / (2f * num * num4), -1.0, 1.0));
			double num8 = Math.Acos(MathHelper.Clamp((0f - (num4 * num4 - num * num - num2 * num2)) / (2f * num * num2), -1.0, 1.0));
			num8 = Math.PI - num8;
			Vector3 vector5 = Vector3.Cross(vector3, vector);
			vector5.Normalize();
			float angle = (float)(num5 - num7);
			float angle2 = (float)(num6 - num8);
			Matrix matrix = Matrix.CreateFromAxisAngle(-vector5, angle);
			Matrix matrix2 = Matrix.CreateFromAxisAngle(vector5, angle2);
			vector.Normalize();
			vector2.Normalize();
			double num9 = Math.Acos(MathHelper.Clamp(vector.Dot(vector2), -1.0, 1.0));
			Vector3 axis = Vector3.Cross(vector, vector2);
			axis.Normalize();
			matrix = Matrix.CreateFromAxisAngle(axis, (float)num9) * matrix;
			matrix2 *= matrix;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
			{
				Vector3 value = Vector3.Transform(vector3, matrix);
				Vector3 value2 = Vector3.Transform(vector4, matrix2);
				MyRenderProxy.DebugDrawLine3D(Vector3.Transform(translation, WorldMatrix), Vector3.Transform(translation + value, WorldMatrix), Color.Purple, Color.Purple, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(Vector3.Transform(translation + value, WorldMatrix), Vector3.Transform(translation + value + value2, WorldMatrix), Color.White, Color.White, depthRead: false);
			}
			Matrix matrix3 = absoluteTransform * matrix;
			Matrix absoluteTransform4 = firstBone.Parent.AbsoluteTransform;
			Matrix matrix4 = Matrix.Multiply(matrix3, Matrix.Invert(firstBone.BindTransform * absoluteTransform4));
			firstBone.Rotation = Quaternion.CreateFromRotationMatrix(matrix4);
			firstBone.ComputeAbsoluteTransform();
			Matrix matrix5 = absoluteTransform2 * matrix2;
			Matrix absoluteTransform5 = secondBone.Parent.AbsoluteTransform;
			Matrix matrix6 = Matrix.Multiply(matrix5, Matrix.Invert(secondBone.BindTransform * absoluteTransform5));
			secondBone.Rotation = Quaternion.CreateFromRotationMatrix(matrix6);
			secondBone.ComputeAbsoluteTransform();
			if (finalBone != null && finalTransform.IsValid() && flag)
			{
				MatrixD matrixD = (!allowFinalBoneTranslation) ? (finalTransform.GetOrientation() * MatrixD.Invert((MatrixD)finalBone.BindTransform * finalBone.Parent.AbsoluteTransform)) : (finalTransform * MatrixD.Invert((MatrixD)finalBone.BindTransform * finalBone.Parent.AbsoluteTransform));
				finalBone.Rotation = Quaternion.CreateFromRotationMatrix(Matrix.Normalize(matrixD.GetOrientation()));
				if (allowFinalBoneTranslation)
				{
					finalBone.Translation = matrixD.Translation;
				}
				finalBone.ComputeAbsoluteTransform();
			}
			return flag;
		}

		public static CastHit? GetClosestFootSupportPosition(MyEntity characterEntity, MyEntity characterTool, Vector3 from, Vector3 up, Vector3 footDimension, Matrix WorldMatrix, float castDownLimit, float castUpLimit, uint raycastFilterLayer = 0u)
		{
			bool flag = false;
			CastHit value = default(CastHit);
			MatrixD matrix = WorldMatrix;
			Vector3 zero = Vector3.Zero;
			matrix.Translation = Vector3.Zero;
			zero = Vector3.Transform(zero, matrix);
			matrix.Translation = from + up * castUpLimit + zero;
			new Vector3(0f, footDimension.Y / 2f, 0f);
			new Vector3(0f, footDimension.Y / 2f, 0f - footDimension.Z);
			Vector3 vector = from + up * castUpLimit;
			Vector3 vector2 = from - up * castDownLimit;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTLINE)
			{
				MyRenderProxy.DebugDrawText3D(vector + zero, "Cast line", Color.White, 1f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector + zero, vector2 + zero, Color.White, Color.White, depthRead: false);
			}
			if (MyFakes.ENABLE_FOOT_IK_USE_HAVOK_RAYCAST)
			{
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTLINE)
				{
					MyRenderProxy.DebugDrawText3D(vector, "Raycast line", Color.Green, 1f, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector, vector2, Color.Green, Color.Green, depthRead: false);
				}
				if (MyPhysics.CastRay(vector, vector2, out MyPhysics.HitInfo hitInfo, raycastFilterLayer, ignoreConvexShape: true))
				{
					flag = true;
					if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_RAYCASTHITS)
					{
						MyRenderProxy.DebugDrawSphere(hitInfo.Position, 0.02f, Color.Green, 1f, depthRead: false);
						MyRenderProxy.DebugDrawText3D(hitInfo.Position, "RayCast hit", Color.Green, 1f, depthRead: false);
					}
					if (Vector3.Dot(hitInfo.Position, up) > Vector3.Dot(value.Position, up))
					{
						value.Position = hitInfo.Position;
						value.Normal = hitInfo.HkHitInfo.Normal;
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			return value;
		}
	}
}
