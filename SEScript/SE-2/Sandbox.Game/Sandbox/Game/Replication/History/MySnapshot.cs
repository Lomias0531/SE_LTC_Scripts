using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using System;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.Replication.History
{
	public struct MySnapshot
	{
		public long ParentId;

		public bool SkippedParent;

		public bool Active;

		public bool InheritRotation;

		public Vector3D Position;

		public Quaternion Rotation;

		public Vector3 LinearVelocity;

		public Vector3 AngularVelocity;

		public Vector3 Pivot;

		public static bool ApplyReset = true;

		public MySnapshot(BitStream stream)
		{
			this = default(MySnapshot);
			Read(stream);
		}

		public MySnapshot(MyEntity entity, bool localPhysics = false, bool inheritRotation = true)
		{
			MyEntity parent = GetParent(entity, out SkippedParent);
			Active = (entity.Physics == null || entity.Physics.RigidBody == null || entity.Physics.RigidBody.IsActive);
			InheritRotation = inheritRotation;
			LinearVelocity = Vector3.Zero;
			AngularVelocity = Vector3.Zero;
			Pivot = Vector3.Zero;
			MatrixD matrix = entity.WorldMatrix;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (parent != null)
			{
				ParentId = parent.EntityId;
				MatrixD matrixD = default(MatrixD);
				if (InheritRotation)
				{
					matrixD = parent.PositionComp.WorldMatrixInvScaled;
					MatrixD.Multiply(ref matrix, ref matrixD, out matrix);
				}
				else
				{
					matrix.Translation -= parent.PositionComp.GetPosition();
				}
				Position = matrix.Translation;
				Quaternion.CreateFromRotationMatrix(ref matrix, out Rotation);
				Rotation.Normalize();
				if (myCubeGrid != null && MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS)
				{
					MyMechanicalConnectionBlockBase myMechanicalConnectionBlockBase = MyGridPhysicalHierarchy.Static.GetEntityConnectingToParent(myCubeGrid) as MyMechanicalConnectionBlockBase;
					if (myMechanicalConnectionBlockBase != null)
					{
						Vector3? constraintPosition = myMechanicalConnectionBlockBase.GetConstraintPosition(myCubeGrid);
						if (constraintPosition.HasValue)
						{
							Pivot = constraintPosition.Value;
							Vector3D.Transform(ref Pivot, ref matrix, out Position);
						}
					}
				}
				else
				{
					Pivot = ((entity.Physics != null && entity.Physics.RigidBody != null) ? entity.Physics.CenterOfMassLocal : entity.PositionComp.LocalAABB.Center);
					Vector3D.Transform(ref Pivot, ref matrix, out Position);
				}
				if (entity.Physics == null || parent.Physics == null)
				{
					return;
				}
				LinearVelocity = entity.Physics.LinearVelocityLocal;
				AngularVelocity = entity.Physics.AngularVelocity;
				if (!localPhysics)
				{
					Vector3 linearVelocity;
					if (InheritRotation)
					{
						Vector3D worldPos = entity.PositionComp.GetPosition();
						parent.Physics.GetVelocityAtPointLocal(ref worldPos, out linearVelocity);
					}
					else
					{
						linearVelocity = parent.Physics.LinearVelocity;
					}
					LinearVelocity -= linearVelocity;
				}
			}
			else
			{
				ParentId = 0L;
				Pivot = ((entity.Physics != null && entity.Physics.RigidBody != null) ? entity.Physics.CenterOfMassLocal : entity.PositionComp.LocalAABB.Center);
				Vector3D.Transform(ref Pivot, ref matrix, out Position);
				Quaternion.CreateFromRotationMatrix(ref matrix, out Rotation);
				Rotation.Normalize();
				if (entity.Physics != null)
				{
					LinearVelocity = entity.Physics.LinearVelocity;
					AngularVelocity = entity.Physics.AngularVelocity;
				}
			}
		}

		public static MyEntity GetParent(MyEntity entity, out bool skipped)
		{
			skipped = false;
			if (MyFakes.WORLD_SNAPSHOTS)
			{
				skipped = true;
				return null;
			}
			MyEntity myEntity = null;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (myCubeGrid.ClosestParentId != 0L)
				{
					myEntity = MyEntities.GetEntityById(myCubeGrid.ClosestParentId);
				}
				else if (MyGridPhysicalHierarchy.Static.GetNodeChainLength(myCubeGrid) < 4)
				{
					myEntity = MyGridPhysicalHierarchy.Static.GetParent(myCubeGrid);
				}
				else
				{
					skipped = true;
				}
			}
			else
			{
				MyCharacter myCharacter = entity as MyCharacter;
				if (myCharacter != null)
				{
					myEntity = MyEntities.GetEntityById(myCharacter.ClosestParentId);
				}
			}
			if (myEntity != null && (myEntity.MarkedForClose || myEntity.Closed))
			{
				return null;
			}
			return myEntity;
		}

		public void Diff(ref MySnapshot value, out MySnapshot ss)
		{
			if (ParentId == value.ParentId)
			{
				Quaternion quaternion = Quaternion.Inverse(Rotation);
				ss.Active = Active;
				ss.ParentId = ParentId;
				ss.SkippedParent = SkippedParent;
				ss.InheritRotation = InheritRotation;
				Vector3D.Subtract(ref Position, ref value.Position, out ss.Position);
				Quaternion.Multiply(ref quaternion, ref value.Rotation, out ss.Rotation);
				Vector3.Subtract(ref LinearVelocity, ref value.LinearVelocity, out ss.LinearVelocity);
				Vector3.Subtract(ref AngularVelocity, ref value.AngularVelocity, out ss.AngularVelocity);
				Vector3.Subtract(ref Pivot, ref value.Pivot, out ss.Pivot);
			}
			else
			{
				ss = default(MySnapshot);
			}
		}

		public void Scale(float factor)
		{
			ScaleTransform(factor);
			LinearVelocity *= factor;
			AngularVelocity *= factor;
		}

		private void ScaleTransform(float factor)
		{
			Rotation.GetAxisAngle(out Vector3 axis, out float angle);
			angle *= factor;
			Quaternion.CreateFromAxisAngle(ref axis, angle, out Rotation);
			Position *= (double)factor;
			Pivot *= factor;
		}

		public bool CheckThresholds(float posSq, float rotSq, float linearSq, float angularSq)
		{
			if (!(Position.LengthSquared() > (double)posSq) && !(Math.Abs(Rotation.W - 1f) > rotSq) && !(LinearVelocity.LengthSquared() > linearSq))
			{
				return AngularVelocity.LengthSquared() > angularSq;
			}
			return true;
		}

		public void Add(ref MySnapshot value)
		{
			if (ParentId == value.ParentId)
			{
				Active = value.Active;
				InheritRotation = value.InheritRotation;
				Position += value.Position;
				Pivot += value.Pivot;
				Rotation *= Quaternion.Inverse(value.Rotation);
				Rotation.Normalize();
				LinearVelocity += value.LinearVelocity;
				AngularVelocity += value.AngularVelocity;
			}
		}

		public void GetMatrix(MyEntity entity, out MatrixD mat, bool applyPosition = true, bool applyRotation = true)
		{
			MatrixD originalWorldMat = entity.WorldMatrix;
			GetMatrix(out mat, ref originalWorldMat, applyPosition, applyRotation);
		}

		public void GetMatrix(out MatrixD mat, ref MatrixD originalWorldMat, bool applyPosition = true, bool applyRotation = true)
		{
			MyEntity myEntity = null;
			if (ParentId != 0L)
			{
				myEntity = MyEntities.GetEntityById(ParentId);
			}
			if (myEntity == null)
			{
				if (applyRotation)
				{
					MatrixD.CreateFromQuaternion(ref Rotation, out mat);
				}
				else
				{
					mat = originalWorldMat;
				}
				if (applyPosition)
				{
					mat.Translation = Position;
					mat.Translation = Vector3D.Transform(-Pivot, ref mat);
				}
				else
				{
					mat.Translation = originalWorldMat.Translation;
				}
				return;
			}
			MatrixD matrix = myEntity.WorldMatrix;
			if (applyPosition && applyRotation)
			{
				MatrixD.CreateFromQuaternion(ref Rotation, out mat);
				mat.Translation = Position;
				if (InheritRotation)
				{
					MatrixD.Multiply(ref mat, ref matrix, out mat);
				}
				else
				{
					mat.Translation += matrix.Translation;
				}
			}
			else if (applyPosition)
			{
				mat = originalWorldMat;
				if (InheritRotation)
				{
					Vector3D.Transform(ref Position, ref matrix, out Vector3D result);
					mat.Translation = result;
				}
				else
				{
					mat.Translation = matrix.Translation + Position;
				}
			}
			else
			{
				MatrixD.CreateFromQuaternion(ref Rotation, out mat);
				if (InheritRotation)
				{
					MatrixD.Multiply(ref mat, ref matrix, out mat);
				}
				mat.Translation = originalWorldMat.Translation;
			}
			mat.Translation = Vector3D.Transform(-Pivot, ref mat);
		}

		public Vector3 GetLinearVelocity(bool local)
		{
			MyEntity myEntity = null;
			if (ParentId != 0L)
			{
				myEntity = MyEntities.GetEntityById(ParentId);
			}
			if (myEntity == null || local)
			{
				return LinearVelocity;
			}
			if (InheritRotation)
			{
				Vector3D worldPos = myEntity.PositionComp.GetPosition();
				myEntity.Physics.GetVelocityAtPointLocal(ref worldPos, out Vector3 linearVelocity);
				return linearVelocity + LinearVelocity;
			}
			return myEntity.Physics.LinearVelocity + LinearVelocity;
		}

		public Vector3 GetAngularVelocity(bool local)
		{
			return AngularVelocity;
		}

		public void ApplyPhysics(MyEntity entity, bool angular = true, bool linear = true, bool local = false)
		{
			if (entity.Physics == null)
			{
				return;
			}
			if (Active)
			{
				if (linear)
				{
					entity.Physics.LinearVelocity = GetLinearVelocity(local);
				}
				if (angular)
				{
					entity.Physics.AngularVelocity = GetAngularVelocity(local);
				}
			}
			else
			{
				entity.Physics.LinearVelocity = Vector3.Zero;
				entity.Physics.AngularVelocity = Vector3.Zero;
			}
			HkRigidBody rigidBody = entity.Physics.RigidBody;
			if (rigidBody != null && !rigidBody.IsFixed && rigidBody.IsActive != Active)
			{
				if (Active)
				{
					rigidBody.Activate();
				}
				else
				{
					rigidBody.Deactivate();
				}
			}
		}

		public void Lerp(ref MySnapshot value, float factor, out MySnapshot ss)
		{
			ss.Active = ((factor > 1f) ? value.Active : ((factor < 0f) ? Active : (Active || value.Active)));
			ss.ParentId = ((ParentId == value.ParentId) ? ParentId : (-1));
			ss.SkippedParent = SkippedParent;
			ss.InheritRotation = InheritRotation;
			Vector3D.Lerp(ref Position, ref value.Position, factor, out ss.Position);
			Vector3.Lerp(ref Pivot, ref value.Pivot, factor, out ss.Pivot);
			Quaternion.Slerp(ref Rotation, ref value.Rotation, factor, out ss.Rotation);
			Vector3.Lerp(ref LinearVelocity, ref value.LinearVelocity, factor, out ss.LinearVelocity);
			Vector3.Lerp(ref AngularVelocity, ref value.AngularVelocity, factor, out ss.AngularVelocity);
			ss.Rotation.Normalize();
		}

		public void Write(BitStream stream)
		{
			stream.WriteVariantSigned(ParentId);
			stream.WriteBool(Active);
			stream.WriteBool(InheritRotation);
			stream.Write(Position);
			stream.Write(Pivot);
			stream.WriteQuaternion(Rotation);
			if (Active)
			{
				stream.Write(LinearVelocity);
				stream.Write(AngularVelocity);
			}
		}

		private void Read(BitStream stream)
		{
			ParentId = stream.ReadInt64Variant();
			Active = stream.ReadBool();
			InheritRotation = stream.ReadBool();
			Position = stream.ReadVector3D();
			Pivot = stream.ReadVector3();
			Rotation = stream.ReadQuaternion();
			if (Active)
			{
				LinearVelocity = stream.ReadVector3();
				AngularVelocity = stream.ReadVector3();
			}
			else
			{
				LinearVelocity = Vector3.Zero;
				AngularVelocity = Vector3.Zero;
			}
		}

		public override string ToString()
		{
			return " pos " + Position.ToString("N3") + " linVel " + LinearVelocity.ToString("N3");
		}

		public bool SanityCheck()
		{
			if (Position.LengthSquared() < 250000.0 && AngularVelocity.LengthSquared() < 250000f)
			{
				return LinearVelocity.LengthSquared() < 160000f;
			}
			return false;
		}
	}
}
