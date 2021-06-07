using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_FloatingObject), true)]
	public class MyFloatingObject : MyEntity, IMyUseObject, IMyUsableEntity, IMyDestroyableObject, IMyFloatingObject, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEventProxy, IMyEventOwner, IMySyncedEntity
	{
		protected sealed class OnClosedRequest_003C_003E : ICallSite<MyFloatingObject, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFloatingObject @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnClosedRequest();
			}
		}

		protected class Amount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType amount;
				ISyncType result = amount = new Sync<MyFixedPoint, SyncDirection.FromServer>(P_1, P_2);
				((MyFloatingObject)P_0).Amount = (Sync<MyFixedPoint, SyncDirection.FromServer>)amount;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyFloatingObject_003C_003EActor : IActivator, IActivator<MyFloatingObject>
		{
			private sealed override object CreateInstance()
			{
				return new MyFloatingObject();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFloatingObject CreateInstance()
			{
				return new MyFloatingObject();
			}

			MyFloatingObject IActivator<MyFloatingObject>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static MyStringHash m_explosives = MyStringHash.GetOrCompute("Explosives");

		public static MyObjectBuilder_Ore ScrapBuilder = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Scrap");

		private StringBuilder m_displayedText = new StringBuilder();

		public MyPhysicalInventoryItem Item;

		private int m_modelVariant;

		public MyVoxelMaterialDefinition VoxelMaterial;

		public long CreationTime;

		private float m_health = 100f;

		private MyEntity3DSoundEmitter m_soundEmitter;

		public int m_lastTimePlayedSound;

		public float ClosestDistanceToAnyPlayerSquared = -1f;

		public int NumberOfFramesInsideVoxel;

		public const int NUMBER_OF_FRAMES_INSIDE_VOXEL_TO_REMOVE = 5;

		public long SyncWaitCounter;

		private DateTime lastTimeSound = DateTime.MinValue;

		private Vector3 m_smoothGravity;

		private Vector3 m_smoothGravityDir;

		private List<Vector3> m_supportNormals;

		public Sync<MyFixedPoint, SyncDirection.FromServer> Amount;

		private HkEasePenetrationAction m_easeCollisionForce;

		private TimeSpan m_timeFromSpawn;

		public bool WasRemovedFromWorld
		{
			get;
			set;
		}

		public MyPhysicalItemDefinition ItemDefinition
		{
			get;
			private set;
		}

		public new MyPhysicsBody Physics
		{
			get
			{
				return base.Physics as MyPhysicsBody;
			}
			set
			{
				base.Physics = value;
			}
		}

		public SyncType SyncType
		{
			get;
			set;
		}

		VRage.ModAPI.IMyEntity IMyUseObject.Owner => this;

		MyModelDummy IMyUseObject.Dummy => null;

		float IMyUseObject.InteractiveDistance => MyConstants.FLOATING_OBJ_INTERACTIVE_DISTANCE;

		MatrixD IMyUseObject.ActivationMatrix
		{
			get
			{
				if (base.PositionComp == null)
				{
					return MatrixD.Zero;
				}
				return Matrix.CreateScale(base.PositionComp.LocalAABB.Size) * base.WorldMatrix;
			}
		}

		MatrixD IMyUseObject.WorldMatrix => base.WorldMatrix;

		uint IMyUseObject.RenderObjectID
		{
			get
			{
				if (base.Render.RenderObjectIDs.Length != 0)
				{
					return base.Render.RenderObjectIDs[0];
				}
				return uint.MaxValue;
			}
		}

		int IMyUseObject.InstanceID => -1;

		bool IMyUseObject.ShowOverlay => false;

		UseActionEnum IMyUseObject.SupportedActions
		{
			get
			{
				if (!MyFakes.ENABLE_SEPARATE_USE_AND_PICK_UP_KEY)
				{
					return UseActionEnum.Manipulate;
				}
				return UseActionEnum.PickUp;
			}
		}

		UseActionEnum IMyUseObject.PrimaryAction
		{
			get
			{
				if (!MyFakes.ENABLE_SEPARATE_USE_AND_PICK_UP_KEY)
				{
					return UseActionEnum.Manipulate;
				}
				return UseActionEnum.PickUp;
			}
		}

		UseActionEnum IMyUseObject.SecondaryAction => UseActionEnum.None;

		bool IMyUseObject.ContinuousUsage => true;

		bool IMyUseObject.PlayIndicatorSound => false;

		public float Integrity => m_health;

		public bool UseDamageSystem
		{
			get;
			private set;
		}

		float IMyDestroyableObject.Integrity => Integrity;

		bool IMyDestroyableObject.UseDamageSystem => UseDamageSystem;

		public MyFloatingObject()
		{
			WasRemovedFromWorld = false;
			m_soundEmitter = new MyEntity3DSoundEmitter(this);
			m_lastTimePlayedSound = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			base.Render = new MyRenderComponentFloatingObject();
			SyncType = SyncHelpers.Compose(this);
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = objectBuilder as MyObjectBuilder_FloatingObject;
			if (myObjectBuilder_FloatingObject.Item.Amount <= 0)
			{
				throw new ArgumentOutOfRangeException("MyPhysicalInventoryItem.Amount", $"Creating floating object with invalid amount: {myObjectBuilder_FloatingObject.Item.Amount}x '{myObjectBuilder_FloatingObject.Item.PhysicalContent.GetId()}'");
			}
			base.Init(objectBuilder);
			Item = new MyPhysicalInventoryItem(myObjectBuilder_FloatingObject.Item);
			m_modelVariant = myObjectBuilder_FloatingObject.ModelVariant;
			Amount.SetLocalValue(Item.Amount);
			Amount.ValueChanged += delegate
			{
				Item.Amount = Amount.Value;
				UpdateInternalState();
			};
			InitInternal();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			UseDamageSystem = true;
			MyPhysicalItemDefinition definition = null;
			if (!MyDefinitionManager.Static.TryGetPhysicalItemDefinition(Item.GetDefinitionId(), out definition))
			{
				ItemDefinition = null;
			}
			else
			{
				ItemDefinition = definition;
			}
			m_timeFromSpawn = MySession.Static.ElapsedPlayTime;
			m_smoothGravity = Physics.RigidBody.Gravity;
			m_smoothGravityDir = m_smoothGravity;
			m_smoothGravityDir.Normalize();
			m_supportNormals = new List<Vector3>();
			m_supportNormals.Capacity = 3;
			if (!Sync.IsServer)
			{
				Physics.RigidBody.UpdateMotionType(HkMotionType.Fixed);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			Vector3 vector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.PositionComp.GetPosition());
			if (Physics.RigidBody.GetShape().ShapeType == HkShapeType.Sphere)
			{
				m_smoothGravity = m_smoothGravity * 0.5f + vector * 0.5f;
				m_smoothGravityDir = m_smoothGravity;
				m_smoothGravityDir.Normalize();
				bool flag = false;
				foreach (Vector3 supportNormal in m_supportNormals)
				{
					if (supportNormal.Dot(m_smoothGravityDir) > 0.8f)
					{
						flag = true;
						break;
					}
				}
				m_supportNormals.Clear();
				if (flag)
				{
					if (Physics.RigidBody.Gravity.Length() > 0.01f)
					{
						Physics.RigidBody.Gravity *= 0.99f;
					}
				}
				else
				{
					Physics.RigidBody.Gravity = m_smoothGravity;
				}
			}
			else
			{
				Physics.RigidBody.Gravity = vector;
			}
		}

		private void RigidBody_ContactPointCallback(ref HkContactPointEvent e)
		{
			if (e.EventType == HkContactPointEvent.Type.Manifold && Physics.RigidBody.GetShape().ShapeType == HkShapeType.Sphere)
			{
				Vector3 item = e.ContactPoint.Position - Physics.RigidBody.Position;
				if (item.Normalize() > 0.001f)
				{
					m_supportNormals.Add(item);
				}
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			MyFloatingObjects.RegisterFloatingObject(this);
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_FloatingObject obj = (MyObjectBuilder_FloatingObject)base.GetObjectBuilder(copy);
			obj.Item = Item.GetObjectBuilder();
			obj.ModelVariant = m_modelVariant;
			return obj;
		}

		public bool HasConstraints()
		{
			return Physics.RigidBody.HasConstraints();
		}

		private void InitInternal()
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(Item.Content);
			m_health = physicalItemDefinition.Health;
			VoxelMaterial = null;
			if (physicalItemDefinition.VoxelMaterial != MyStringHash.NullOrEmpty)
			{
				VoxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(physicalItemDefinition.VoxelMaterial.String);
			}
			else if (Item.Content is MyObjectBuilder_Ore)
			{
				string subtypeName = physicalItemDefinition.Id.SubtypeName;
				string materialName = (Item.Content as MyObjectBuilder_Ore).GetMaterialName();
				bool flag = (Item.Content as MyObjectBuilder_Ore).HasMaterialName();
				foreach (MyVoxelMaterialDefinition voxelMaterialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
				{
					if ((flag && materialName == voxelMaterialDefinition.Id.SubtypeName) || (!flag && subtypeName == voxelMaterialDefinition.MinedOre))
					{
						VoxelMaterial = voxelMaterialDefinition;
						break;
					}
				}
			}
			if (VoxelMaterial != null && VoxelMaterial.DamagedMaterial != MyStringHash.NullOrEmpty)
			{
				VoxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(VoxelMaterial.DamagedMaterial.ToString());
			}
			string model = physicalItemDefinition.Model;
			if (physicalItemDefinition.HasModelVariants)
			{
				int num = physicalItemDefinition.Models.Length;
				m_modelVariant %= num;
				model = physicalItemDefinition.Models[m_modelVariant];
			}
			else if (Item.Content is MyObjectBuilder_Ore && VoxelMaterial != null)
			{
				float val = 50f;
				model = MyDebris.GetAmountBasedDebrisVoxel(Math.Max((float)Item.Amount, val));
			}
			float scale = 0.7f;
			FormatDisplayName(m_displayedText, Item);
			Init(m_displayedText, model, null, null);
			HkMassProperties massProperties = default(HkMassProperties);
			float num2 = MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(physicalItemDefinition.Mass) : physicalItemDefinition.Mass;
			num2 = MathHelper.Clamp(num2 * (float)Item.Amount, 3f, 100000f);
			HkShape physicsShape = GetPhysicsShape(num2, scale, out massProperties);
			massProperties.Mass = num2;
			_ = Matrix.Identity;
			if (Physics != null)
			{
				Physics.Close();
			}
			Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_DEBRIS);
			int collisionFilter = ((double)num2 > MyPerGameSettings.MinimumLargeShipCollidableMass) ? 23 : 10;
			Physics.LinearDamping = 0.1f;
			Physics.AngularDamping = 2f;
			Physics.CreateFromCollisionObject(physicsShape, Vector3.Zero, MatrixD.Identity, massProperties, collisionFilter);
			Physics.Enabled = true;
			Physics.Friction = 2f;
			Physics.MaterialType = EvaluatePhysicsMaterial(physicalItemDefinition.PhysicalMaterial);
			Physics.PlayCollisionCueEnabled = true;
			Physics.RigidBody.ContactSoundCallbackEnabled = true;
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
			Physics.RigidBody.SetProperty(255, 0f);
			HkMassChangerUtil.Create(Physics.RigidBody, 66048, 1f, 0f);
		}

		private MyStringHash EvaluatePhysicsMaterial(MyStringHash originalMaterial)
		{
			if (VoxelMaterial == null)
			{
				return originalMaterial;
			}
			return MyMaterialType.ROCK;
		}

		public void RefreshDisplayName()
		{
			FormatDisplayName(m_displayedText, Item);
		}

		private void FormatDisplayName(StringBuilder outputBuffer, MyPhysicalInventoryItem item)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content);
			outputBuffer.Clear().Append(physicalItemDefinition.DisplayNameText);
			if (Item.Amount != 1)
			{
				outputBuffer.Append(" (");
				MyGuiControlInventoryOwner.FormatItemAmount(item, outputBuffer);
				outputBuffer.Append(")");
			}
		}

		protected override void Closing()
		{
			MyFloatingObjects.UnregisterFloatingObject(this);
			base.Closing();
		}

		protected virtual HkShape GetPhysicsShape(float mass, float scale, out HkMassProperties massProperties)
		{
			if (base.Model == null)
			{
				MyLog.Default.WriteLine("Invalid floating object model: " + Item.GetDefinitionId());
			}
			HkShapeType shapeType;
			if (VoxelMaterial != null)
			{
				shapeType = HkShapeType.Sphere;
				massProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(base.Model.BoundingSphere.Radius * scale, mass);
			}
			else
			{
				shapeType = HkShapeType.Box;
				Vector3 value = 2f * (base.Model.BoundingBox.Max - base.Model.BoundingBox.Min) / 2f;
				massProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(value * scale, mass);
				massProperties.Mass = mass;
				massProperties.CenterOfMass = base.Model.BoundingBox.Center;
			}
			return MyDebris.Static.GetDebrisShape(base.Model, shapeType, scale);
		}

		void IMyUseObject.SetRenderID(uint id)
		{
		}

		void IMyUseObject.SetInstanceID(int id)
		{
		}

		void IMyUseObject.Use(UseActionEnum actionEnum, VRage.ModAPI.IMyEntity entity)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (base.MarkedForClose)
			{
				return;
			}
			MyFixedPoint myFixedPoint = MyFixedPoint.Min(Item.Amount, myCharacter.GetInventory().ComputeAmountThatFits(Item.Content.GetId()));
			if (myFixedPoint == 0)
			{
				if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimePlayedSound > 2500)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudVocInventoryFull);
					m_lastTimePlayedSound = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
				MyHud.Stats.GetStat<MyStatPlayerInventoryFull>().InventoryFull = true;
				return;
			}
			if (myFixedPoint > 0)
			{
				if (MySession.Static.ControlledEntity == myCharacter && (lastTimeSound == DateTime.MinValue || (DateTime.UtcNow - lastTimeSound).TotalMilliseconds > 500.0))
				{
					MyGuiAudio.PlaySound(MyGuiSounds.PlayTakeItem);
					lastTimeSound = DateTime.UtcNow;
				}
				myCharacter.GetInventory().PickupItem(this, myFixedPoint);
			}
			MyHud.Notifications.ReloadTexts();
		}

		public void UpdateInternalState()
		{
			if (Item.Amount <= 0)
			{
				Close();
				return;
			}
			base.Render.UpdateRenderObject(visible: false);
			InitInternal();
			Physics.Activate();
			base.InScene = true;
			base.Render.UpdateRenderObject(visible: true);
			MyHud.Notifications.ReloadTexts();
		}

		MyActionDescription IMyUseObject.GetActionInfo(UseActionEnum actionEnum)
		{
			MyActionDescription result;
			if (!MySandboxGame.Config.ControlsHints)
			{
				result = default(MyActionDescription);
				result.Text = MyCommonTexts.CustomText;
				result.IsTextControlHint = false;
				result.FormatParams = new object[1]
				{
					m_displayedText
				};
				result.JoystickText = MyCommonTexts.CustomText;
				result.JoystickFormatParams = new object[1]
				{
					m_displayedText
				};
				result.ShowForGamepad = true;
				return result;
			}
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			switch (actionEnum)
			{
			case UseActionEnum.PickUp:
				MyInput.Static.GetGameControl(MyControlsSpace.PICK_UP).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
				result = default(MyActionDescription);
				result.Text = MyCommonTexts.NotificationPickupObject;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.PICK_UP), "]"),
					m_displayedText
				};
				result.IsTextControlHint = true;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.PICK_UP),
					m_displayedText
				};
				result.ShowForGamepad = true;
				return result;
			case UseActionEnum.Manipulate:
				MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
				result = default(MyActionDescription);
				result.Text = MyCommonTexts.NotificationPickupObject;
				result.FormatParams = new object[2]
				{
					string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.USE), "]"),
					m_displayedText
				};
				result.IsTextControlHint = true;
				result.JoystickFormatParams = new object[2]
				{
					MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE),
					m_displayedText
				};
				result.ShowForGamepad = true;
				return result;
			default:
				result = default(MyActionDescription);
				return result;
			}
		}

		UseActionResult IMyUsableEntity.CanUse(UseActionEnum actionEnum, IMyControllableEntity user)
		{
			if (!base.MarkedForClose)
			{
				return UseActionResult.OK;
			}
			return UseActionResult.Closed;
		}

		public bool DoDamage(float damage, MyStringHash damageType, bool sync, long attackerId)
		{
			if (base.MarkedForClose)
			{
				return false;
			}
			if (sync)
			{
				if (Sync.IsServer)
				{
					MySyncDamage.DoDamageSynced(this, damage, damageType, attackerId);
					return true;
				}
				return false;
			}
			MyDamageInformation info = new MyDamageInformation(isDeformation: false, damage, damageType, attackerId);
			if (UseDamageSystem)
			{
				MyDamageSystem.Static.RaiseBeforeDamageApplied(this, ref info);
			}
			MyObjectBuilderType typeId = Item.Content.TypeId;
			if (typeId == typeof(MyObjectBuilder_Ore) || typeId == typeof(MyObjectBuilder_Ingot))
			{
				if (Item.Amount < 1)
				{
					if (MyParticlesManager.TryCreateParticleEffect("Smoke_Construction", base.WorldMatrix, out MyParticleEffect effect))
					{
						effect.UserScale = 0.4f;
					}
					if (Sync.IsServer)
					{
						MyFloatingObjects.RemoveFloatingObject(this);
					}
				}
				else if (Sync.IsServer)
				{
					MyFloatingObjects.RemoveFloatingObject(this, (MyFixedPoint)info.Amount);
				}
			}
			else
			{
				m_health -= 10f * info.Amount;
				if (UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseAfterDamageApplied(this, info);
				}
				if (m_health < 0f)
				{
					if (MyParticlesManager.TryCreateParticleEffect("Smoke_Construction", base.WorldMatrix, out MyParticleEffect effect2))
					{
						effect2.UserScale = 0.4f;
					}
					if (Sync.IsServer)
					{
						MyFloatingObjects.RemoveFloatingObject(this);
					}
					if (Item.Content.SubtypeId == m_explosives && Sync.IsServer)
					{
						BoundingSphere b = new BoundingSphere(base.WorldMatrix.Translation, (float)Item.Amount * 0.01f + 0.5f);
						MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
						myExplosionInfo.PlayerDamage = 0f;
						myExplosionInfo.Damage = 800f;
						myExplosionInfo.ExplosionType = MyExplosionTypeEnum.WARHEAD_EXPLOSION_15;
						myExplosionInfo.ExplosionSphere = b;
						myExplosionInfo.LifespanMiliseconds = 700;
						myExplosionInfo.HitEntity = this;
						myExplosionInfo.ParticleScale = 1f;
						myExplosionInfo.OwnerEntity = this;
						myExplosionInfo.Direction = base.WorldMatrix.Forward;
						myExplosionInfo.VoxelExplosionCenter = b.Center;
						myExplosionInfo.ExplosionFlags = (MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION);
						myExplosionInfo.VoxelCutoutScale = 0.5f;
						myExplosionInfo.PlaySound = true;
						myExplosionInfo.ApplyForceAndDamage = true;
						myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
						MyExplosionInfo explosionInfo = myExplosionInfo;
						if (Physics != null)
						{
							explosionInfo.Velocity = Physics.LinearVelocity;
						}
						MyExplosions.AddExplosion(ref explosionInfo);
					}
					if (MyFakes.ENABLE_SCRAP && Sync.IsServer)
					{
						if (Item.Content.SubtypeId == ScrapBuilder.SubtypeId)
						{
							return true;
						}
						if (Item.Content.GetId().TypeId == typeof(MyObjectBuilder_Component))
						{
							MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition((Item.Content as MyObjectBuilder_Component).GetId());
							if (MyRandom.Instance.NextFloat() < componentDefinition.DropProbability)
							{
								MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(Item.Amount * 0.8f, ScrapBuilder), base.PositionComp.GetPosition(), base.WorldMatrix.Forward, base.WorldMatrix.Up);
							}
						}
					}
					if (ItemDefinition != null && ItemDefinition.DestroyedPieceId.HasValue && Sync.IsServer && MyDefinitionManager.Static.TryGetPhysicalItemDefinition(ItemDefinition.DestroyedPieceId.Value, out MyPhysicalItemDefinition definition))
					{
						MyFloatingObjects.Spawn(definition, base.WorldMatrix.Translation, base.WorldMatrix.Forward, base.WorldMatrix.Up, ItemDefinition.DestroyedPieces);
					}
					if (UseDamageSystem)
					{
						MyDamageSystem.Static.RaiseDestroyed(this, info);
					}
				}
			}
			return true;
		}

		public void RemoveUsers(bool local)
		{
		}

		public void OnDestroy()
		{
		}

		void IMyDestroyableObject.OnDestroy()
		{
			OnDestroy();
		}

		bool IMyDestroyableObject.DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId)
		{
			return DoDamage(damage, damageType, sync, attackerId);
		}

		bool IMyUseObject.HandleInput()
		{
			return false;
		}

		void IMyUseObject.OnSelectionLost()
		{
		}

		public void SendCloseRequest()
		{
			MyMultiplayer.RaiseEvent(this, (MyFloatingObject x) => x.OnClosedRequest);
		}

		[Event(null, 798)]
		[Reliable]
		[Server]
		private void OnClosedRequest()
		{
			if (!MySession.Static.CreativeMode && !MyEventContext.Current.IsLocallyInvoked && !MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				Close();
			}
		}
	}
}
