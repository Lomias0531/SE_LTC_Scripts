using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders.Definitions;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.EnvironmentItems
{
	[MyEntityType(typeof(MyObjectBuilder_TreesMedium), false)]
	[MyEntityType(typeof(MyObjectBuilder_Trees), true)]
	[StaticEventOwner]
	public class MyTrees : MyEnvironmentItems, IMyDecalProxy
	{
		private struct MyCutTreeInfo
		{
			public int ItemInstanceId;

			public int LastHit;

			public float HitPoints;

			public float MaxPoints;

			public float Progress => MathHelper.Clamp((MaxPoints - HitPoints) / MaxPoints, 0f, 1f);
		}

		protected sealed class PlaySound_003C_003EVRageMath_Vector3D_0023System_String : ICallSite<IMyEventOwner, Vector3D, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D position, in string cueName, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				PlaySound(position, cueName);
			}
		}

		private class Sandbox_Game_Entities_EnvironmentItems_MyTrees_003C_003EActor : IActivator, IActivator<MyTrees>
		{
			private sealed override object CreateInstance()
			{
				return new MyTrees();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTrees CreateInstance()
			{
				return new MyTrees();
			}

			MyTrees IActivator<MyTrees>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private List<MyCutTreeInfo> m_cutTreeInfos = new List<MyCutTreeInfo>();

		private const float MAX_TREE_CUT_DURATION = 60f;

		private const int BrokenTreeLifeSpan = 20000;

		public override void DoDamage(float damage, int itemInstanceId, Vector3D position, Vector3 normal, MyStringHash type)
		{
			MyEnvironmentItemData myEnvironmentItemData = m_itemsData[itemInstanceId];
			MyDefinitionId id = new MyDefinitionId(base.Definition.ItemDefinitionType, myEnvironmentItemData.SubtypeId);
			MyTreeDefinition myTreeDefinition = (MyTreeDefinition)MyDefinitionManager.Static.GetEnvironmentItemDefinition(id);
			MyParticlesManager.TryCreateParticleEffect(myTreeDefinition.CutEffect, MatrixD.CreateWorld(position, Vector3.CalculatePerpendicularVector(normal), normal), out MyParticleEffect _);
			if (!Sync.IsServer)
			{
				return;
			}
			MyCutTreeInfo myCutTreeInfo = default(MyCutTreeInfo);
			int num = -1;
			for (int i = 0; i < m_cutTreeInfos.Count; i++)
			{
				myCutTreeInfo = m_cutTreeInfos[i];
				if (itemInstanceId == myCutTreeInfo.ItemInstanceId)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				myCutTreeInfo = default(MyCutTreeInfo);
				myCutTreeInfo.ItemInstanceId = itemInstanceId;
				myCutTreeInfo.MaxPoints = (myCutTreeInfo.HitPoints = myTreeDefinition.HitPoints);
				num = m_cutTreeInfos.Count;
				m_cutTreeInfos.Add(myCutTreeInfo);
			}
			myCutTreeInfo.LastHit = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			myCutTreeInfo.HitPoints -= damage;
			if (myCutTreeInfo.Progress >= 1f)
			{
				CutTree(itemInstanceId, position, normal, (type == MyDamageType.Drill) ? 1f : 4f);
				m_cutTreeInfos.RemoveAtFast(num);
			}
			else
			{
				m_cutTreeInfos[num] = myCutTreeInfo;
			}
		}

		public static bool IsEntityFracturedTree(IMyEntity entity)
		{
			if (entity is MyFracturedPiece && ((MyFracturedPiece)entity).OriginalBlocks != null && ((MyFracturedPiece)entity).OriginalBlocks.Count > 0 && (((MyFracturedPiece)entity).OriginalBlocks[0].TypeId == typeof(MyObjectBuilder_Tree) || ((MyFracturedPiece)entity).OriginalBlocks[0].TypeId == typeof(MyObjectBuilder_DestroyableItem) || ((MyFracturedPiece)entity).OriginalBlocks[0].TypeId == typeof(MyObjectBuilder_TreeDefinition)))
			{
				return ((MyFracturedPiece)entity).Physics != null;
			}
			return false;
		}

		protected override void OnRemoveItem(int instanceId, ref Matrix matrix, MyStringHash myStringId, int userData)
		{
			base.OnRemoveItem(instanceId, ref matrix, myStringId, userData);
		}

		private void CutTree(int itemInstanceId, Vector3D hitWorldPosition, Vector3 hitNormal, float forceMultiplier = 1f)
		{
			_ = (HkStaticCompoundShape)base.Physics.RigidBody.GetShape();
			if (!m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out int value))
			{
				return;
			}
			MyEnvironmentItemData itemData = m_itemsData[itemInstanceId];
			MyDefinitionId id = new MyDefinitionId(base.Definition.ItemDefinitionType, itemData.SubtypeId);
			MyTreeDefinition myTreeDefinition = (MyTreeDefinition)MyDefinitionManager.Static.GetEnvironmentItemDefinition(id);
			if (RemoveItem(itemInstanceId, value, sync: true, immediateUpdate: true) && myTreeDefinition != null && myTreeDefinition.BreakSound != null && myTreeDefinition.BreakSound.Length > 0)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => PlaySound, hitWorldPosition, myTreeDefinition.BreakSound, default(EndpointId), hitWorldPosition);
			}
			if (MyPerGameSettings.Destruction && MyModels.GetModelOnlyData(myTreeDefinition.Model).HavokBreakableShapes != null)
			{
				if (myTreeDefinition.FallSound != null && myTreeDefinition.FallSound.Length > 0)
				{
					CreateBreakableShape(myTreeDefinition, ref itemData, ref hitWorldPosition, hitNormal, forceMultiplier, myTreeDefinition.FallSound);
				}
				else
				{
					CreateBreakableShape(myTreeDefinition, ref itemData, ref hitWorldPosition, hitNormal, forceMultiplier);
				}
			}
		}

		[Event(null, 157)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void PlaySound(Vector3D position, string cueName)
		{
			MySoundPair mySoundPair = new MySoundPair(cueName);
			if (mySoundPair != MySoundPair.Empty)
			{
				MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
				if (myEntity3DSoundEmitter != null)
				{
					myEntity3DSoundEmitter.SetPosition(position);
					myEntity3DSoundEmitter.PlaySound(mySoundPair);
				}
			}
		}

		protected override MyEntity DestroyItem(int itemInstanceId)
		{
			if (!m_localIdToPhysicsShapeInstanceId.TryGetValue(itemInstanceId, out int value))
			{
				value = -1;
			}
			MyEnvironmentItemData myEnvironmentItemData = m_itemsData[itemInstanceId];
			RemoveItem(itemInstanceId, value, sync: false, immediateUpdate: true);
			Vector3D position = myEnvironmentItemData.Transform.Position;
			string text = myEnvironmentItemData.Model.AssetName.Insert(myEnvironmentItemData.Model.AssetName.Length - 4, "_broken");
			bool flag = false;
			MyEntity myEntity;
			if (MyModels.GetModelOnlyData(text) != null)
			{
				flag = true;
				myEntity = MyDebris.Static.CreateDebris(text);
			}
			else
			{
				myEntity = MyDebris.Static.CreateDebris(myEnvironmentItemData.Model.AssetName);
			}
			MyDebrisBase.MyDebrisBaseLogic obj = myEntity.GameLogic as MyDebrisBase.MyDebrisBaseLogic;
			obj.LifespanInMiliseconds = 20000;
			MatrixD position2 = MatrixD.CreateFromQuaternion(myEnvironmentItemData.Transform.Rotation);
			position2.Translation = position + position2.Up * ((!flag) ? 5 : 0);
			obj.Start(position2, Vector3.Zero, randomRotation: false);
			return myEntity;
		}

		private void CreateBreakableShape(MyEnvironmentItemDefinition itemDefinition, ref MyEnvironmentItemData itemData, ref Vector3D hitWorldPosition, Vector3 hitNormal, float forceMultiplier, string fallSound = "")
		{
			HkdBreakableShape hkdBreakableShape = MyModels.GetModelOnlyData(itemDefinition.Model).HavokBreakableShapes[0].Clone();
			MatrixD transformMatrix = itemData.Transform.TransformMatrix;
			hkdBreakableShape.SetMassRecursively(500f);
			hkdBreakableShape.SetStrenghtRecursively(5000f, 0.7f);
			hkdBreakableShape.GetChildren(m_childrenTmp);
			_ = MyModels.GetModelOnlyData(itemDefinition.Model).HavokBreakableShapes;
			_ = (Vector3)Vector3D.Transform(hitWorldPosition, MatrixD.Normalize(MatrixD.Invert(transformMatrix)));
			float num = (float)(hitWorldPosition.Y - itemData.Transform.Position.Y);
			List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
			List<HkdShapeInstanceInfo> list2 = new List<HkdShapeInstanceInfo>();
			HkdShapeInstanceInfo? hkdShapeInstanceInfo = null;
			foreach (HkdShapeInstanceInfo item in m_childrenTmp)
			{
				if (!hkdShapeInstanceInfo.HasValue || item.CoM.Y < hkdShapeInstanceInfo.Value.CoM.Y)
				{
					hkdShapeInstanceInfo = item;
				}
				if (item.CoM.Y > num)
				{
					list2.Add(item);
				}
				else
				{
					list.Add(item);
				}
			}
			if (list.Count == 2)
			{
				if (list[0].CoM.Y < list[1].CoM.Y && num < list[1].CoM.Y + 1.25f)
				{
					list2.Insert(0, list[1]);
					list.RemoveAt(1);
				}
				else if (list[0].CoM.Y > list[1].CoM.Y && num < list[0].CoM.Y + 1.25f)
				{
					list2.Insert(0, list[0]);
					list.RemoveAt(0);
				}
			}
			else if (list.Count == 0 && list2.Remove(hkdShapeInstanceInfo.Value))
			{
				list.Add(hkdShapeInstanceInfo.Value);
			}
			if (list.Count > 0)
			{
				CreateFracturePiece(itemDefinition, hkdBreakableShape, transformMatrix, hitNormal, list, forceMultiplier, canContainFixedChildren: true);
			}
			if (list2.Count > 0)
			{
				CreateFracturePiece(itemDefinition, hkdBreakableShape, transformMatrix, hitNormal, list2, forceMultiplier, canContainFixedChildren: false, fallSound);
			}
			m_childrenTmp.Clear();
		}

		public static void CreateFracturePiece(MyEnvironmentItemDefinition itemDefinition, HkdBreakableShape oldBreakableShape, MatrixD worldMatrix, Vector3 hitNormal, List<HkdShapeInstanceInfo> shapeList, float forceMultiplier, bool canContainFixedChildren, string fallSound = "")
		{
			bool isStatic = false;
			if (canContainFixedChildren)
			{
				foreach (HkdShapeInstanceInfo shape in shapeList)
				{
					shape.Shape.SetMotionQualityRecursively(HkdBreakableShape.BodyQualityType.QUALITY_DEBRIS);
					Vector3D translation = worldMatrix.Translation + worldMatrix.Up * 1.5;
					Quaternion rotation = Quaternion.CreateFromRotationMatrix(worldMatrix.GetOrientation());
					MyPhysics.GetPenetrationsShape(shape.Shape.GetShape(), ref translation, ref rotation, MyEnvironmentItems.m_tmpResults, 15);
					bool flag = false;
					foreach (HkBodyCollision tmpResult in MyEnvironmentItems.m_tmpResults)
					{
						if (tmpResult.GetCollisionEntity() is MyVoxelMap)
						{
							shape.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
							isStatic = true;
							flag = true;
							break;
						}
						if (flag)
						{
							break;
						}
					}
					MyEnvironmentItems.m_tmpResults.Clear();
				}
			}
			HkdBreakableShape compound = new HkdCompoundBreakableShape(oldBreakableShape, shapeList);
			((HkdCompoundBreakableShape)compound).RecalcMassPropsFromChildren();
			MyFracturedPiece myFracturedPiece = MyDestructionHelper.CreateFracturePiece(compound, ref worldMatrix, isStatic, itemDefinition.Id, sync: true);
			if (myFracturedPiece != null && !canContainFixedChildren)
			{
				ApplyImpulseToTreeFracture(ref worldMatrix, ref hitNormal, shapeList, ref compound, myFracturedPiece, forceMultiplier);
				myFracturedPiece.Physics.ForceActivate();
				if (fallSound.Length > 0)
				{
					myFracturedPiece.StartFallSound(fallSound);
				}
			}
		}

		public static void ApplyImpulseToTreeFracture(ref MatrixD worldMatrix, ref Vector3 hitNormal, List<HkdShapeInstanceInfo> shapeList, ref HkdBreakableShape compound, MyFracturedPiece fp, float forceMultiplier = 1f)
		{
			float mass = compound.GetMass();
			Vector3 coMMaxY = Vector3.MinValue;
			shapeList.ForEach(delegate(HkdShapeInstanceInfo s)
			{
				coMMaxY = ((s.CoM.Y > coMMaxY.Y) ? s.CoM : coMMaxY);
			});
			Vector3 value = hitNormal;
			value.Y = 0f;
			value.Normalize();
			Vector3 impulse = 0.3f * forceMultiplier * mass * value;
			fp.Physics.Enabled = true;
			Vector3 point = fp.Physics.WorldToCluster(Vector3D.Transform(coMMaxY, worldMatrix));
			fp.Physics.RigidBody.AngularDamping = MyPerGameSettings.DefaultAngularDamping;
			fp.Physics.RigidBody.LinearDamping = MyPerGameSettings.DefaultLinearDamping;
			fp.Physics.RigidBody.ApplyPointImpulse(impulse, point);
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			UpdateTreeInfos();
		}

		private void UpdateTreeInfos()
		{
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			int num = 60000;
			for (int num2 = m_cutTreeInfos.Count - 1; num2 >= 0; num2--)
			{
				MyCutTreeInfo myCutTreeInfo = m_cutTreeInfos[num2];
				if (totalGamePlayTimeInMilliseconds - myCutTreeInfo.LastHit > num)
				{
					m_cutTreeInfos.RemoveAtFast(num2);
				}
			}
		}

		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, object customdata, IMyDecalHandler decalHandler, MyStringHash material)
		{
			MyDecalRenderInfo myDecalRenderInfo = default(MyDecalRenderInfo);
			myDecalRenderInfo.Position = hitInfo.Position;
			myDecalRenderInfo.Normal = hitInfo.Normal;
			myDecalRenderInfo.RenderObjectIds = null;
			myDecalRenderInfo.Flags = MyDecalFlags.World;
			myDecalRenderInfo.Source = source;
			MyDecalRenderInfo renderInfo = myDecalRenderInfo;
			if (material.GetHashCode() == 0)
			{
				renderInfo.Material = base.Physics.MaterialType;
			}
			else
			{
				renderInfo.Material = material;
			}
			decalHandler.AddDecal(ref renderInfo);
		}
	}
}
