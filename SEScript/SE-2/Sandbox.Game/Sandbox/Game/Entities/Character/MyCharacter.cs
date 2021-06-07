#define VRAGE
using Havok;
using ParallelTasks;
using Sandbox.Common;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.Utils;
using VRage.GameServices;
using VRage.Generics;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Character
{
	[MyEntityType(typeof(MyObjectBuilder_Character), true)]
	[StaticEventOwner]
	public class MyCharacter : MySkinnedEntity, IMyCameraController, IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyInventoryOwner, IMyUseObject, IMyDestroyableObject, IMyDecalProxy, IMyCharacter, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEventProxy, IMyEventOwner, IMyComponentOwner<MyIDModule>, IMySyncedEntity
	{
		private class MyCharacterPosition : MyPositionComponent
		{
			private class Sandbox_Game_Entities_Character_MyCharacter_003C_003EMyCharacterPosition_003C_003EActor : IActivator, IActivator<MyCharacterPosition>
			{
				private sealed override object CreateInstance()
				{
					return new MyCharacterPosition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyCharacterPosition CreateInstance()
				{
					return new MyCharacterPosition();
				}

				MyCharacterPosition IActivator<MyCharacterPosition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private const int CHECK_FREQUENCY = 20;

			private int m_checkOutOfWorldCounter;

			protected override void OnWorldPositionChanged(object source, bool updateChildren, bool forceUpdateAllChildren)
			{
				ClampToWorld();
				base.OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
			}

			private void ClampToWorld()
			{
				if (!MySession.Static.WorldBoundaries.HasValue)
				{
					return;
				}
				m_checkOutOfWorldCounter++;
				if (m_checkOutOfWorldCounter <= 20)
				{
					return;
				}
				Vector3D position = GetPosition();
				Vector3D min = MySession.Static.WorldBoundaries.Value.Min;
				Vector3D max = MySession.Static.WorldBoundaries.Value.Max;
				Vector3D vector3D = position - Vector3.One * 10f;
				Vector3D vector3D2 = position + Vector3.One * 10f;
				if (!(vector3D.X < min.X) && !(vector3D.Y < min.Y) && !(vector3D.Z < min.Z) && !(vector3D2.X > max.X) && !(vector3D2.Y > max.Y) && !(vector3D2.Z > max.Z))
				{
					m_checkOutOfWorldCounter = 0;
					return;
				}
				Vector3 linearVelocity = base.Container.Entity.Physics.LinearVelocity;
				bool flag = false;
				if (position.X < min.X || position.X > max.X)
				{
					flag = true;
					linearVelocity.X = 0f;
				}
				if (position.Y < min.Y || position.Y > max.Y)
				{
					flag = true;
					linearVelocity.Y = 0f;
				}
				if (position.Z < min.Z || position.Z > max.Z)
				{
					flag = true;
					linearVelocity.Z = 0f;
				}
				if (flag)
				{
					m_checkOutOfWorldCounter = 0;
					SetPosition(Vector3.Clamp(position, min, max));
					base.Container.Entity.Physics.LinearVelocity = linearVelocity;
				}
				m_checkOutOfWorldCounter = 20;
			}
		}

		protected sealed class SynchronizeBuildPlanner_Implementation_003C_003EVRage_Game_MyObjectBuilder_Character_003C_003EBuildPlanItem_003C_0023_003E : ICallSite<MyCharacter, MyObjectBuilder_Character.BuildPlanItem[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in MyObjectBuilder_Character.BuildPlanItem[] buildPlanner, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SynchronizeBuildPlanner_Implementation(buildPlanner);
			}
		}

		protected sealed class EnableIronsightCallback_003C_003ESystem_Boolean_0023System_Boolean_0023System_Boolean_0023System_Boolean_0023System_Boolean : ICallSite<MyCharacter, bool, bool, bool, bool, bool, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in bool enable, in bool newKeyPress, in bool changeCamera, in bool hideCrosshairWhenAiming, in bool forceChangeCamera, in DBNull arg6)
			{
				@this.EnableIronsightCallback(enable, newKeyPress, changeCamera, hideCrosshairWhenAiming, forceChangeCamera);
			}
		}

		protected sealed class Jump_003C_003EVRageMath_Vector3 : ICallSite<MyCharacter, Vector3, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in Vector3 moveIndicator, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.Jump(moveIndicator);
			}
		}

		protected sealed class UnequipWeapon_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UnequipWeapon();
			}
		}

		protected sealed class EnableLightsCallback_003C_003ESystem_Boolean : ICallSite<MyCharacter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in bool enable, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.EnableLightsCallback(enable);
			}
		}

		protected sealed class EnableBroadcastingCallback_003C_003ESystem_Boolean : ICallSite<MyCharacter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in bool enable, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.EnableBroadcastingCallback(enable);
			}
		}

		protected sealed class OnSuicideRequest_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSuicideRequest();
			}
		}

		protected sealed class RefreshAssetModifiers_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in long entityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RefreshAssetModifiers(playerId, entityId);
			}
		}

		protected sealed class SendSkinData_003C_003ESystem_Int64_0023System_Byte_003C_0023_003E : ICallSite<IMyEventOwner, long, byte[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in byte[] checkDataResult, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendSkinData(entityId, checkDataResult);
			}
		}

		protected sealed class ChangeModel_Implementation_003C_003ESystem_String_0023VRageMath_Vector3_0023System_Boolean_0023System_Int64 : ICallSite<MyCharacter, string, Vector3, bool, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in string model, in Vector3 colorMaskHSV, in bool resetToDefault, in long caller, in DBNull arg5, in DBNull arg6)
			{
				@this.ChangeModel_Implementation(model, colorMaskHSV, resetToDefault, caller);
			}
		}

		protected sealed class UpdateStoredGas_Implementation_003C_003EVRage_ObjectBuilders_SerializableDefinitionId_0023System_Single : ICallSite<MyCharacter, SerializableDefinitionId, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in SerializableDefinitionId gasId, in float fillLevel, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UpdateStoredGas_Implementation(gasId, fillLevel);
			}
		}

		protected sealed class OnUpdateOxygen_003C_003ESystem_Single : ICallSite<MyCharacter, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in float oxygenAmount, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateOxygen(oxygenAmount);
			}
		}

		protected sealed class OnRefillFromBottle_003C_003EVRage_ObjectBuilders_SerializableDefinitionId : ICallSite<MyCharacter, SerializableDefinitionId, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in SerializableDefinitionId gasId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRefillFromBottle(gasId);
			}
		}

		protected sealed class OnSecondarySoundPlay_003C_003EVRage_Audio_MyCueId : ICallSite<MyCharacter, MyCueId, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in MyCueId soundId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSecondarySoundPlay(soundId);
			}
		}

		protected sealed class EnablePhysics_003C_003ESystem_Boolean : ICallSite<MyCharacter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in bool enabled, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.EnablePhysics(enabled);
			}
		}

		protected sealed class OnKillCharacter_003C_003EVRage_Game_ModAPI_MyDamageInformation_0023VRageMath_Vector3 : ICallSite<MyCharacter, MyDamageInformation, Vector3, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in MyDamageInformation damageInfo, in Vector3 lastLinearVelocity, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnKillCharacter(damageInfo, lastLinearVelocity);
			}
		}

		protected sealed class SpawnCharacterRelative_003C_003ESystem_Int64_0023VRageMath_Vector3 : ICallSite<MyCharacter, long, Vector3, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in long RelatedEntity, in Vector3 DeltaPosition, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SpawnCharacterRelative(RelatedEntity, DeltaPosition);
			}
		}

		protected sealed class ShootBeginCallback_003C_003EVRageMath_Vector3_0023Sandbox_Game_Entities_MyShootActionEnum_0023System_Boolean : ICallSite<MyCharacter, Vector3, MyShootActionEnum, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in Vector3 direction, in MyShootActionEnum action, in bool doubleClick, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShootBeginCallback(direction, action, doubleClick);
			}
		}

		protected sealed class ShootEndCallback_003C_003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyCharacter, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in MyShootActionEnum action, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShootEndCallback(action);
			}
		}

		protected sealed class GunDoubleClickedCallback_003C_003ESandbox_Game_Entities_MyShootActionEnum : ICallSite<MyCharacter, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in MyShootActionEnum action, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GunDoubleClickedCallback(action);
			}
		}

		protected sealed class ShootDirectionChangeCallback_003C_003EVRageMath_Vector3 : ICallSite<MyCharacter, Vector3, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in Vector3 direction, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShootDirectionChangeCallback(direction);
			}
		}

		protected sealed class OnSwitchAmmoMagazineRequest_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchAmmoMagazineRequest();
			}
		}

		protected sealed class OnSwitchAmmoMagazineSuccess_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchAmmoMagazineSuccess();
			}
		}

		protected sealed class SwitchToWeaponMessage_003C_003ESystem_Nullable_00601_003CVRage_ObjectBuilders_SerializableDefinitionId_003E_0023System_Nullable_00601_003CSystem_UInt32_003E : ICallSite<MyCharacter, SerializableDefinitionId?, uint?, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in SerializableDefinitionId? weapon, in uint? inventoryItemId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SwitchToWeaponMessage(weapon, inventoryItemId);
			}
		}

		protected sealed class OnSwitchToWeaponSuccess_003C_003ESystem_Nullable_00601_003CVRage_ObjectBuilders_SerializableDefinitionId_003E_0023System_Nullable_00601_003CSystem_UInt32_003E_0023System_Int64 : ICallSite<MyCharacter, SerializableDefinitionId?, uint?, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in SerializableDefinitionId? weapon, in uint? inventoryItemId, in long weaponEntityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchToWeaponSuccess(weapon, inventoryItemId, weaponEntityId);
			}
		}

		protected sealed class OnAnimationCommand_003C_003ESandbox_Game_Entities_MyAnimationCommand : ICallSite<MyCharacter, MyAnimationCommand, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in MyAnimationCommand command, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAnimationCommand(command);
			}
		}

		protected sealed class OnAnimationEvent_003C_003ESystem_String_0023System_String_003C_0023_003E : ICallSite<MyCharacter, string, string[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in string eventName, in string[] layers, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAnimationEvent(eventName, layers);
			}
		}

		protected sealed class OnRagdollTransformsUpdate_003C_003ESystem_Int32_0023VRageMath_Vector3_003C_0023_003E_0023VRageMath_Quaternion_003C_0023_003E_0023VRageMath_Quaternion_0023VRageMath_Vector3 : ICallSite<MyCharacter, int, Vector3[], Quaternion[], Quaternion, Vector3, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in int transformsCount, in Vector3[] transformsPositions, in Quaternion[] transformsOrientations, in Quaternion worldOrientation, in Vector3 worldPosition, in DBNull arg6)
			{
				@this.OnRagdollTransformsUpdate(transformsCount, transformsPositions, transformsOrientations, worldOrientation, worldPosition);
			}
		}

		protected sealed class OnSwitchHelmet_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchHelmet();
			}
		}

		protected sealed class SwitchJetpack_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SwitchJetpack();
			}
		}

		protected sealed class GetOnLadder_Request_003C_003ESystem_Int64 : ICallSite<MyCharacter, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in long ladderId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetOnLadder_Request(ladderId);
			}
		}

		protected sealed class GetOnLadder_Failed_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetOnLadder_Failed();
			}
		}

		protected sealed class GetOnLadder_Implementation_003C_003ESystem_Int64_0023System_Boolean_0023System_Nullable_00601_003CVRage_Game_MyObjectBuilder_Character_003C_003ELadderInfo_003E : ICallSite<MyCharacter, long, bool, MyObjectBuilder_Character.LadderInfo?, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in long ladderId, in bool resetPosition, in MyObjectBuilder_Character.LadderInfo? newLadderInfo, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetOnLadder_Implementation(ladderId, resetPosition, newLadderInfo);
			}
		}

		protected sealed class GetOffLadder_Implementation_003C_003E : ICallSite<MyCharacter, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetOffLadder_Implementation();
			}
		}

		protected sealed class CreateBurrowingParticleFX_Client_003C_003EVRageMath_Vector3D : ICallSite<MyCharacter, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in Vector3D position, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateBurrowingParticleFX_Client(position);
			}
		}

		protected sealed class DeleteBurrowingParticleFX_Client_003C_003EVRageMath_Vector3D : ICallSite<MyCharacter, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in Vector3D position, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DeleteBurrowingParticleFX_Client(position);
			}
		}

		protected sealed class TriggerAnimationEvent_003C_003ESystem_String : ICallSite<MyCharacter, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyCharacter @this, in string eventName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.TriggerAnimationEvent(eventName);
			}
		}

		protected class m_bootsState_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType bootsState;
				ISyncType result = bootsState = new Sync<MyBootsState, SyncDirection.FromServer>(P_1, P_2);
				((MyCharacter)P_0).m_bootsState = (Sync<MyBootsState, SyncDirection.FromServer>)bootsState;
				return result;
			}
		}

		protected class m_currentAmmoCount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType currentAmmoCount;
				ISyncType result = currentAmmoCount = new Sync<int, SyncDirection.FromServer>(P_1, P_2);
				((MyCharacter)P_0).m_currentAmmoCount = (Sync<int, SyncDirection.FromServer>)currentAmmoCount;
				return result;
			}
		}

		protected class m_currentMagazineAmmoCount_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType currentMagazineAmmoCount;
				ISyncType result = currentMagazineAmmoCount = new Sync<int, SyncDirection.FromServer>(P_1, P_2);
				((MyCharacter)P_0).m_currentMagazineAmmoCount = (Sync<int, SyncDirection.FromServer>)currentMagazineAmmoCount;
				return result;
			}
		}

		protected class m_aimedGrid_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType aimedGrid;
				ISyncType result = aimedGrid = new Sync<long, SyncDirection.BothWays>(P_1, P_2);
				((MyCharacter)P_0).m_aimedGrid = (Sync<long, SyncDirection.BothWays>)aimedGrid;
				return result;
			}
		}

		protected class m_aimedBlock_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType aimedBlock;
				ISyncType result = aimedBlock = new Sync<Vector3I, SyncDirection.BothWays>(P_1, P_2);
				((MyCharacter)P_0).m_aimedBlock = (Sync<Vector3I, SyncDirection.BothWays>)aimedBlock;
				return result;
			}
		}

		protected class m_controlInfo_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType controlInfo;
				ISyncType result = controlInfo = new Sync<MyPlayer.PlayerId, SyncDirection.FromServer>(P_1, P_2);
				((MyCharacter)P_0).m_controlInfo = (Sync<MyPlayer.PlayerId, SyncDirection.FromServer>)controlInfo;
				return result;
			}
		}

		protected class m_localHeadPosition_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType localHeadPosition;
				ISyncType result = localHeadPosition = new Sync<Vector3, SyncDirection.BothWays>(P_1, P_2);
				((MyCharacter)P_0).m_localHeadPosition = (Sync<Vector3, SyncDirection.BothWays>)localHeadPosition;
				return result;
			}
		}

		protected class m_animLeaning_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType animLeaning;
				ISyncType result = animLeaning = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyCharacter)P_0).m_animLeaning = (Sync<float, SyncDirection.BothWays>)animLeaning;
				return result;
			}
		}

		protected class EnvironmentOxygenLevelSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType environmentOxygenLevelSync;
				ISyncType result = environmentOxygenLevelSync = new Sync<float, SyncDirection.FromServer>(P_1, P_2);
				((MyCharacter)P_0).EnvironmentOxygenLevelSync = (Sync<float, SyncDirection.FromServer>)environmentOxygenLevelSync;
				return result;
			}
		}

		protected class OxygenLevelAtCharacterLocation_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType oxygenLevelAtCharacterLocation;
				ISyncType result = oxygenLevelAtCharacterLocation = new Sync<float, SyncDirection.FromServer>(P_1, P_2);
				((MyCharacter)P_0).OxygenLevelAtCharacterLocation = (Sync<float, SyncDirection.FromServer>)oxygenLevelAtCharacterLocation;
				return result;
			}
		}

		protected class OxygenSourceGridEntityId_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType oxygenSourceGridEntityId;
				ISyncType result = oxygenSourceGridEntityId = new Sync<long, SyncDirection.FromServer>(P_1, P_2);
				((MyCharacter)P_0).OxygenSourceGridEntityId = (Sync<long, SyncDirection.FromServer>)oxygenSourceGridEntityId;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Character_MyCharacter_003C_003EActor : IActivator, IActivator<MyCharacter>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacter();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacter CreateInstance()
			{
				return new MyCharacter();
			}

			MyCharacter IActivator<MyCharacter>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyBlueprintClassDefinition m_buildPlannerBlueprintClass;

		private static List<VertexArealBoneIndexWeight> m_boneIndexWeightTmp;

		[ThreadStatic]
		private static MyCharacterHitInfo m_hitInfoTmp;

		public const float MAGIC_COS = 0.996795f;

		public const float CAMERA_NEAR_DISTANCE = 60f;

		internal const float CHARACTER_X_ROTATION_SPEED = 0.13f;

		private const float CHARACTER_Y_ROTATION_FACTOR = 0.02f;

		private const float WALK_THRESHOLD = 0.4f;

		private const float SPRINT_THRESHOLD = 1.6f;

		private const float SPRINT_X_TOLERANCE = 0.1f;

		public const float MINIMAL_SPEED = 0.001f;

		private const float JUMP_DURATION = 0.55f;

		private const float JUMP_TIME = 1f;

		private const float SHOT_TIME = 0.1f;

		private const float FALL_TIME = 0.3f;

		private const float RESPAWN_TIME = 5f;

		internal const float MIN_HEAD_LOCAL_X_ANGLE = -89.9f;

		internal const float MAX_HEAD_LOCAL_X_ANGLE = 89f;

		internal const float MIN_HEAD_LOCAL_Y_ANGLE_ON_LADDER = -89.9f;

		internal const float MAX_HEAD_LOCAL_Y_ANGLE_ON_LADDER = 89f;

		public const int HK_CHARACTER_FLYING = 5;

		private const float AERIAL_CONTROL_FORCE_MULTIPLIER = 0.062f;

		public static float MAX_SHAKE_DAMAGE = 90f;

		private float m_currentShotTime;

		private float m_currentShootPositionTime;

		private float m_cameraDistance;

		private float m_currentSpeed;

		private Vector3 m_currentMovementDirection = Vector3.Zero;

		private float m_currentDecceleration;

		private float m_currentJumpTime;

		private float m_frictionBeforeJump = 1.3f;

		private bool m_assetModifiersLoaded;

		private bool m_canJump = true;

		public bool UpdateRotationsOverride;

		private float m_currentWalkDelay;

		private float m_canPlayImpact;

		private static MyStringId m_stringIdHit = MyStringId.GetOrCompute("Hit");

		private MyStringHash m_physicalMaterialHash;

		private long m_deadPlayerIdentityId = -1L;

		private Vector3 m_gravity = Vector3.Zero;

		private bool m_resolveHighlightOverlap;

		public static MyHudNotification OutOfAmmoNotification;

		private int m_weaponBone = -1;

		public float CharacterGeneralDamageModifier = 1f;

		private bool m_usingByPrimary;

		private float m_headLocalXAngle;

		private float m_headLocalYAngle;

		private bool m_headRenderingEnabled = true;

		private readonly Sync<MyBootsState, SyncDirection.FromServer> m_bootsState;

		public float RotationSpeed = 0.13f;

		private const double MIN_FORCE_PREDICTION_DURATION = 10.0;

		private bool m_forceDisablePrediction;

		private double m_forceDisablePredictionTime;

		private int m_headBoneIndex = -1;

		private int m_camera3rdBoneIndex = -1;

		private int m_leftHandIKStartBone = -1;

		private int m_leftHandIKEndBone = -1;

		private int m_rightHandIKStartBone = -1;

		private int m_rightHandIKEndBone = -1;

		private int m_leftUpperarmBone = -1;

		private int m_leftForearmBone = -1;

		private int m_rightUpperarmBone = -1;

		private int m_rightForearmBone = -1;

		private int m_leftHandItemBone = -1;

		private int m_rightHandItemBone = -1;

		private int m_spineBone = -1;

		protected bool m_characterBoneCapsulesReady;

		private bool m_animationCommandsEnabled = true;

		private float m_currentAnimationChangeDelay;

		private float SAFE_DELAY_FOR_ANIMATION_BLEND = 0.1f;

		private MyCharacterMovementEnum m_currentMovementState;

		private MyCharacterMovementEnum m_previousMovementState;

		private MyCharacterMovementEnum m_previousNetworkMovementState;

		private MyEntity m_leftHandItem;

		private MyHandItemDefinition m_handItemDefinition;

		private MyZoomModeEnum m_zoomMode;

		private float m_currentHandItemWalkingBlend;

		private float m_currentHandItemShootBlend;

		private CapsuleD[] m_bodyCapsules = new CapsuleD[1];

		private MatrixD m_headMatrix = MatrixD.CreateTranslation(0.0, 1.65, 0.0);

		private MyHudNotification m_pickupObjectNotification;

		private HkCharacterStateType m_currentCharacterState;

		private bool m_isFalling;

		private bool m_isFallingAnimationPlayed;

		private float m_currentFallingTime;

		private bool m_crouchAfterFall;

		private MyCharacterMovementFlags m_movementFlags;

		private MyCharacterMovementFlags m_netMovementFlags;

		private MyCharacterMovementFlags m_previousMovementFlags;

		private bool m_movementsFlagsChanged;

		private string m_characterModel;

		private MyBattery m_suitBattery;

		private MyResourceDistributorComponent m_suitResourceDistributor;

		private float m_outsideTemperature;

		private MyResourceSinkComponent m_sinkComp;

		private MyEntity m_topGrid;

		private MyEntity m_usingEntity;

		private bool m_enableBag = true;

		private static readonly float ROTATION_SPEED_CLASSIC = 1f;

		private static readonly float ROTATION_SPEED_IRONSIGHTS = 0.6f;

		public const float REFLECTOR_RANGE = 35f;

		public const float REFLECTOR_CONE_ANGLE = 0.373f;

		public const float REFLECTOR_BILLBOARD_LENGTH = 40f;

		public const float REFLECTOR_BILLBOARD_THICKNESS = 6f;

		public static Vector4 REFLECTOR_COLOR = Vector4.One;

		public static float REFLECTOR_FALLOFF = 1f;

		public static float REFLECTOR_GLOSS_FACTOR = 1f;

		public static float REFLECTOR_DIFFUSE_FACTOR = 3.14f;

		public static float REFLECTOR_INTENSITY = 25f;

		public static Vector4 POINT_COLOR = Vector4.One;

		public static float POINT_FALLOFF = 0.3f;

		public static float POINT_GLOSS_FACTOR = 1f;

		public static float POINT_DIFFUSE_FACTOR = 3.14f;

		public static float POINT_LIGHT_INTENSITY = 0.5f;

		public static float POINT_LIGHT_RANGE = 1.08f;

		public static bool LIGHT_PARAMETERS_CHANGED = false;

		public const float LIGHT_GLARE_MAX_DISTANCE_SQR = 1600f;

		private float m_currentLightPower;

		private float m_lightPowerFromProducer;

		private float m_lightTurningOnSpeed = 0.05f;

		private float m_lightTurningOffSpeed = 0.05f;

		private bool m_lightEnabled = true;

		private float m_currentHeadAnimationCounter;

		private float m_currentLocalHeadAnimation = -1f;

		private float m_localHeadAnimationLength = -1f;

		private Vector2? m_localHeadAnimationX;

		private Vector2? m_localHeadAnimationY;

		private List<MyBoneCapsuleInfo> m_bodyCapsuleInfo = new List<MyBoneCapsuleInfo>();

		private HashSet<uint> m_shapeContactPoints = new HashSet<uint>();

		private float m_currentRespawnCounter;

		private MyHudNotification m_respawnNotification;

		private MyHudNotification m_notEnoughStatNotification;

		private MyStringHash manipulationToolId = MyStringHash.GetOrCompute("ManipulationTool");

		private Queue<Vector3> m_bobQueue = new Queue<Vector3>();

		private bool m_dieAfterSimulation;

		private Vector3? m_deathLinearVelocityFromSever;

		private float m_currentLootingCounter;

		private MyEntityCameraSettings m_cameraSettingsWhenAlive;

		private bool m_useAnimationForWeapon = true;

		private long m_relativeDampeningEntityInit;

		private MyCharacterDefinition m_characterDefinition;

		private bool m_isInFirstPersonView = true;

		private bool m_targetFromCamera;

		private bool m_forceFirstPersonCamera;

		private bool m_moveAndRotateStopped;

		private bool m_moveAndRotateCalled;

		private readonly Sync<int, SyncDirection.FromServer> m_currentAmmoCount;

		private readonly Sync<int, SyncDirection.FromServer> m_currentMagazineAmmoCount;

		private readonly Sync<long, SyncDirection.BothWays> m_aimedGrid;

		private readonly Sync<Vector3I, SyncDirection.BothWays> m_aimedBlock;

		private readonly Sync<MyPlayer.PlayerId, SyncDirection.FromServer> m_controlInfo;

		private MyPlayer.PlayerId? m_savedPlayer;

		private readonly Sync<Vector3, SyncDirection.BothWays> m_localHeadPosition;

		private Sync<float, SyncDirection.BothWays> m_animLeaning;

		private List<IMyNetworkCommand> m_cachedCommands;

		private Vector3 m_previousLinearVelocity;

		private Vector3D m_previousPosition;

		private bool[] m_isShooting;

		public Vector3 ShootDirection = Vector3.One;

		private bool m_shootDoubleClick;

		private long m_lastShootDirectionUpdate;

		private long m_closestParentId;

		private MyIDModule m_idModule = new MyIDModule(0L, MyOwnershipShareModeEnum.Faction);

		internal readonly Sync<float, SyncDirection.FromServer> EnvironmentOxygenLevelSync;

		internal readonly Sync<float, SyncDirection.FromServer> OxygenLevelAtCharacterLocation;

		internal readonly Sync<long, SyncDirection.FromServer> OxygenSourceGridEntityId;

		private static readonly Vector3[] m_defaultColors = new Vector3[7]
		{
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, -0.96f, -0.5f),
			new Vector3(0.575f, 0.15f, 0.2f),
			new Vector3(0.333f, -0.33f, -0.05f),
			new Vector3(0f, 0f, 0.05f),
			new Vector3(0f, -0.8f, 0.6f),
			new Vector3(0.122f, 0.05f, 0.46f)
		};

		public static readonly string DefaultModel = "Default_Astronaut";

		private float? m_savedHealth;

		private bool m_wasInFirstPerson;

		private bool m_isInFirstPerson;

		private bool m_wasInThirdPersonBeforeIronSight;

		private List<HkBodyCollision> m_physicsCollisionResults;

		private List<MyEntity> m_supportedEntitiesTmp = new List<MyEntity>();

		private Vector3D m_crosshairPoint;

		private Vector3D m_aimedPoint;

		private List<HkBodyCollision> m_penetrationList = new List<HkBodyCollision>();

		private List<MyPhysics.HitInfo> m_raycastList;

		private float m_headMovementXOffset;

		private float m_headMovementYOffset;

		private float m_maxHeadMovementOffset = 3f;

		private float m_headMovementStep = 0.1f;

		private bool m_lastGetViewWasDead;

		private Matrix m_getViewAliveWorldMatrix = Matrix.Identity;

		private Vector3D m_lastProceduralGeneratorPosition = Vector3D.PositiveInfinity;

		private static readonly List<uint> m_tmpIds = new List<uint>();

		private MyControllerInfo m_info = new MyControllerInfo();

		private MyDefinitionId? m_endShootAutoswitch;

		private MyDefinitionId? m_autoswitch;

		private MatrixD m_lastCorrectSpectatorCamera;

		private float m_squeezeDamageTimer;

		private const float m_weaponMinAmp = 1.12377834f;

		private const float m_weaponMaxAmp = 1.217867f;

		private const float m_weaponMedAmp = 1.17082262f;

		private const float m_weaponRunMedAmp = 1.12876678f;

		private Quaternion m_weaponMatrixOrientationBackup;

		private MyCharacterBreath m_breath;

		public MyEntity ManipulatedEntity;

		private MyGuiScreenBase m_InventoryScreen;

		private MyCharacterClientState m_lastClientState;

		private MyEntity m_relativeDampeningEntity;

		private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();

		private List<MyPhysics.HitInfo> m_hits2 = new List<MyPhysics.HitInfo>(4);

		private MyCubeGrid m_standingOnGrid;

		private MyVoxelBase m_standingOnVoxel;

		private const float LadderSpeed = 2f;

		private const float MinHeadLadderLocalYAngle = -90f;

		private const float MaxHeadLadderLocalYAngle = 90f;

		private float m_stepIncrement;

		private int m_stepsPerAnimation;

		private Vector3 m_ladderIncrementToBase;

		private Vector3 m_ladderIncrementToBaseServer;

		private MatrixD m_baseMatrix;

		private int m_currentLadderStep;

		private MyLadder m_ladder;

		private MyHudNotification m_ladderOffNotification;

		private MyHudNotification m_ladderUpDownNotification;

		private MyHudNotification m_ladderJumpOffNotification;

		private MyHudNotification m_ladderBlockedNotification;

		private long? m_ladderIdInit;

		private MyObjectBuilder_Character.LadderInfo? m_ladderInfoInit;

		private HkConstraint m_constraintInstance;

		private HkFixedConstraintData m_constraintData;

		private HkBreakableConstraintData m_constraintBreakableData;

		private bool m_needReconnectLadder;

		private MyCubeGrid m_oldLadderGrid;

		private float m_verticalFootError;

		private float m_cummulativeVerticalFootError;

		private static string TopBody = "LeftHand RightHand LeftFingers RightFingers Head Spine";

		private bool m_resetWeaponAnimationState;

		private Quaternion m_lastRotation;

		private static Dictionary<Vector3D, MyParticleEffect> m_burrowEffectTable = new Dictionary<Vector3D, MyParticleEffect>();

		private readonly Vector3[] m_animationSpeedFilter = new Vector3[4];

		private int m_animationSpeedFilterCursor;

		private int m_wasOnLadder;

		private static List<MyEntity> m_supportingEntities;

		public IReadOnlyList<MyIdentity.BuildPlanItem> BuildPlanner
		{
			get
			{
				if (GetIdentity() == null)
				{
					return null;
				}
				return GetIdentity().BuildPlanner;
			}
		}

		internal bool CanJump
		{
			get
			{
				return m_canJump;
			}
			set
			{
				m_canJump = value;
			}
		}

		internal float CurrentWalkDelay
		{
			get
			{
				return m_currentWalkDelay;
			}
			set
			{
				m_currentWalkDelay = value;
			}
		}

		public Vector3 Gravity => m_gravity;

		public int WeaponBone => m_weaponBone;

		private IMyHandheldGunObject<MyDeviceBase> m_currentWeapon
		{
			get;
			set;
		}

		public bool IsClientPredicted
		{
			get;
			private set;
		}

		public bool ForceDisablePrediction
		{
			get
			{
				return m_forceDisablePrediction;
			}
			set
			{
				m_forceDisablePrediction = value;
				m_forceDisablePredictionTime = MySandboxGame.Static.SimulationTime.Seconds;
			}
		}

		public bool AlwaysDisablePrediction
		{
			get;
			set;
		}

		public bool HeadRenderingEnabled => m_headRenderingEnabled;

		public float HeadLocalXAngle
		{
			get
			{
				if (!m_headLocalXAngle.IsValid())
				{
					return 0f;
				}
				return m_headLocalXAngle;
			}
			set
			{
				m_headLocalXAngle = (value.IsValid() ? MathHelper.Clamp(value, -89.9f, 89f) : 0f);
			}
		}

		public float HeadLocalYAngle
		{
			get
			{
				return m_headLocalYAngle;
			}
			set
			{
				if (IsOnLadder && IsInFirstPersonView)
				{
					m_headLocalYAngle = MathHelper.Clamp(value, -89.9f, 89f);
				}
				else
				{
					m_headLocalYAngle = value;
				}
			}
		}

		public MyCharacterMovementEnum CurrentMovementState
		{
			get
			{
				return m_currentMovementState;
			}
			set
			{
				SetCurrentMovementState(value);
			}
		}

		public MyCharacterMovementEnum PreviousMovementState => m_previousMovementState;

		public MyHandItemDefinition HandItemDefinition => m_handItemDefinition;

		public MyZoomModeEnum ZoomMode => m_zoomMode;

		public bool ShouldSupressShootAnimation
		{
			get
			{
				if (m_currentWeapon == null)
				{
					return false;
				}
				return m_currentWeapon.SupressShootAnimation();
			}
		}

		public HkCharacterStateType CharacterGroundState => m_currentCharacterState;

		public bool JetpackRunning
		{
			get
			{
				if (JetpackComp != null)
				{
					return JetpackComp.Running;
				}
				return false;
			}
		}

		internal MyResourceDistributorComponent SuitRechargeDistributor
		{
			get
			{
				return m_suitResourceDistributor;
			}
			set
			{
				if (base.Components.Contains(typeof(MyResourceDistributorComponent)))
				{
					base.Components.Remove<MyResourceDistributorComponent>();
				}
				base.Components.Add(value);
				m_suitResourceDistributor = value;
			}
		}

		public MyResourceSinkComponent SinkComp
		{
			get
			{
				return m_sinkComp;
			}
			set
			{
				m_sinkComp = value;
			}
		}

		public bool EnabledBag => m_enableBag;

		public SyncType SyncType
		{
			get;
			set;
		}

		public float CurrentLightPower => m_currentLightPower;

		public float CurrentRespawnCounter => m_currentRespawnCounter;

		internal MyRadioReceiver RadioReceiver
		{
			get
			{
				return (MyRadioReceiver)base.Components.Get<MyDataReceiver>();
			}
			private set
			{
				base.Components.Add((MyDataReceiver)value);
			}
		}

		internal MyRadioBroadcaster RadioBroadcaster
		{
			get
			{
				return (MyRadioBroadcaster)base.Components.Get<MyDataBroadcaster>();
			}
			private set
			{
				base.Components.Add((MyDataBroadcaster)value);
			}
		}

		public StringBuilder CustomNameWithFaction
		{
			get;
			private set;
		}

		internal new MyRenderComponentCharacter Render
		{
			get
			{
				return base.Render as MyRenderComponentCharacter;
			}
			set
			{
				base.Render = value;
			}
		}

		public MyCharacterSoundComponent SoundComp
		{
			get
			{
				return base.Components.Get<MyCharacterSoundComponent>();
			}
			set
			{
				if (base.Components.Has<MyCharacterSoundComponent>())
				{
					base.Components.Remove<MyCharacterSoundComponent>();
				}
				base.Components.Add(value);
			}
		}

		public MyAtmosphereDetectorComponent AtmosphereDetectorComp
		{
			get
			{
				return base.Components.Get<MyAtmosphereDetectorComponent>();
			}
			set
			{
				if (base.Components.Has<MyAtmosphereDetectorComponent>())
				{
					base.Components.Remove<MyAtmosphereDetectorComponent>();
				}
				base.Components.Add(value);
			}
		}

		public MyEntityReverbDetectorComponent ReverbDetectorComp
		{
			get
			{
				return base.Components.Get<MyEntityReverbDetectorComponent>();
			}
			set
			{
				if (base.Components.Has<MyEntityReverbDetectorComponent>())
				{
					base.Components.Remove<MyEntityReverbDetectorComponent>();
				}
				base.Components.Add(value);
			}
		}

		public MyCharacterStatComponent StatComp
		{
			get
			{
				return base.Components.Get<MyEntityStatComponent>() as MyCharacterStatComponent;
			}
			set
			{
				if (base.Components.Has<MyEntityStatComponent>())
				{
					base.Components.Remove<MyEntityStatComponent>();
				}
				base.Components.Add((MyEntityStatComponent)value);
			}
		}

		public MyCharacterJetpackComponent JetpackComp
		{
			get
			{
				return base.Components.Get<MyCharacterJetpackComponent>();
			}
			set
			{
				if (base.Components.Has<MyCharacterJetpackComponent>())
				{
					base.Components.Remove<MyCharacterJetpackComponent>();
				}
				base.Components.Add(value);
			}
		}

		float IMyCharacter.BaseMass => BaseMass;

		float IMyCharacter.CurrentMass => CurrentMass;

		public float BaseMass => Definition.Mass;

		public float CurrentMass
		{
			get
			{
				float num = 0f;
				if (ManipulatedEntity != null && ManipulatedEntity.Physics != null)
				{
					num = ManipulatedEntity.Physics.Mass;
				}
				if (this.GetInventory() != null)
				{
					return BaseMass + (float)this.GetInventory().CurrentMass + num;
				}
				return BaseMass + num;
			}
		}

		public MyCharacterDefinition Definition => m_characterDefinition;

		MyDefinitionBase IMyCharacter.Definition => m_characterDefinition;

		public bool IsInFirstPersonView
		{
			get
			{
				return m_isInFirstPersonView;
			}
			set
			{
				if (!value && !MySession.Static.Settings.Enable3rdPersonView)
				{
					m_isInFirstPersonView = true;
				}
				else if (Definition.EnableFirstPersonView)
				{
					m_isInFirstPersonView = value;
					ResetHeadRotation();
					if (!m_isInFirstPersonView && m_zoomMode == MyZoomModeEnum.IronSight)
					{
						EnableIronsight(enable: false, newKeyPress: false, changeCamera: true);
					}
					SwitchCameraIronSightChanges();
				}
				else
				{
					m_isInFirstPersonView = false;
				}
			}
		}

		public bool EnableFirstPersonView
		{
			get
			{
				return Definition.EnableFirstPersonView;
			}
			set
			{
			}
		}

		public bool TargetFromCamera
		{
			get
			{
				if (MySession.Static.ControlledEntity == this)
				{
					return MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator;
				}
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					return false;
				}
				return m_targetFromCamera;
			}
			set
			{
				m_targetFromCamera = value;
			}
		}

		public MyToolbar Toolbar => MyToolbarComponent.CharacterToolbar;

		public bool ForceFirstPersonCamera
		{
			get
			{
				if (!IsDead)
				{
					if (!m_forceFirstPersonCamera)
					{
						return MyThirdPersonSpectator.Static.IsCameraForced();
					}
					return true;
				}
				return false;
			}
			set
			{
				m_forceFirstPersonCamera = (!IsDead && value);
			}
		}

		public bool IsCameraNear
		{
			get
			{
				if (MyFakes.ENABLE_PERMANENT_SIMULATIONS_COMPUTATION)
				{
					return true;
				}
				if (Render.IsVisible())
				{
					return m_cameraDistance <= 60f;
				}
				return false;
			}
		}

		public MyInventoryAggregate InventoryAggregate
		{
			get
			{
				return base.Components.Get<MyInventoryBase>() as MyInventoryAggregate;
			}
			set
			{
				if (base.Components.Has<MyInventoryBase>())
				{
					base.Components.Remove<MyInventoryBase>();
				}
				base.Components.Add((MyInventoryBase)value);
			}
		}

		public MyCharacterOxygenComponent OxygenComponent
		{
			get;
			private set;
		}

		public MyCharacterWeaponPositionComponent WeaponPosition
		{
			get;
			private set;
		}

		public Vector3 MoveIndicator
		{
			get;
			set;
		}

		public Vector2 RotationIndicator
		{
			get;
			set;
		}

		public bool IsRotating
		{
			get;
			set;
		}

		public float RollIndicator
		{
			get;
			set;
		}

		public Vector3 RotationCenterIndicator
		{
			get;
			set;
		}

		public long AimedGrid
		{
			get
			{
				return m_aimedGrid.Value;
			}
			set
			{
				m_aimedGrid.Value = value;
			}
		}

		public Vector3I AimedBlock
		{
			get
			{
				return m_aimedBlock.Value;
			}
			set
			{
				m_aimedBlock.Value = value;
			}
		}

		public ulong ControlSteamId
		{
			get
			{
				if (m_controlInfo == null)
				{
					return 0uL;
				}
				return m_controlInfo.Value.SteamId;
			}
		}

		public MyPromoteLevel PromoteLevel
		{
			get
			{
				MyPlayer.PlayerId value = m_controlInfo.Value;
				return MySession.Static.GetUserPromoteLevel(value.SteamId);
			}
		}

		public long ClosestParentId
		{
			get
			{
				return m_closestParentId;
			}
			set
			{
				if (m_closestParentId != value || !MyGridPhysicalHierarchy.Static.NonGridLinkExists(value, this))
				{
					if (MyEntities.TryGetEntityById(m_closestParentId, out MyCubeGrid entity, allowClosed: true))
					{
						MyGridPhysicalHierarchy.Static.RemoveNonGridNode(entity, this);
					}
					if (MyEntities.TryGetEntityById(value, out entity))
					{
						m_closestParentId = value;
						MyGridPhysicalHierarchy.Static.AddNonGridNode(entity, this);
					}
					else
					{
						m_closestParentId = 0L;
					}
				}
			}
		}

		public bool IsPersistenceCharacter
		{
			get;
			set;
		}

		public MyPlayer.PlayerId? SavedPlayer => m_savedPlayer;

		public bool InheritRotation
		{
			get
			{
				if (!JetpackRunning && !IsFalling)
				{
					return !IsJumping;
				}
				return false;
			}
		}

		public bool NeedsPerFrameUpdate
		{
			get
			{
				if (GetCurrentMovementState() != MyCharacterMovementEnum.Sitting)
				{
					return true;
				}
				if (!(IsUsing is MyCryoChamber))
				{
					return true;
				}
				if (Sync.Players.IsPlayerOnline(ControllerInfo.ControllingIdentityId))
				{
					return true;
				}
				if (MySession.Static.LocalCharacter == this)
				{
					return true;
				}
				if (StatComp != null && StatComp.Health != null && StatComp.Health.Value < MySpaceStatEffect.MAX_REGEN_HEALTH_RATIO)
				{
					return true;
				}
				return false;
			}
		}

		public Vector3D AimedPoint
		{
			get
			{
				return m_aimedPoint;
			}
			set
			{
				m_aimedPoint = value;
			}
		}

		public bool IsIdle
		{
			get
			{
				if (m_currentMovementState != 0)
				{
					return m_currentMovementState == MyCharacterMovementEnum.Crouching;
				}
				return true;
			}
		}

		internal float HeadMovementXOffset => m_headMovementXOffset;

		internal float HeadMovementYOffset => m_headMovementYOffset;

		public MyEntity IsUsing
		{
			get
			{
				return m_usingEntity;
			}
			set
			{
				m_usingEntity = value;
			}
		}

		public float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public bool LightEnabled => m_lightEnabled;

		public bool IsCrouching => m_currentMovementState.GetMode() == 2;

		public bool IsSprinting => m_currentMovementState == MyCharacterMovementEnum.Sprinting;

		public bool IsFalling
		{
			get
			{
				MyCharacterMovementEnum currentMovementState = GetCurrentMovementState();
				if (m_isFalling)
				{
					return currentMovementState != MyCharacterMovementEnum.Flying;
				}
				return false;
			}
		}

		public bool IsJumping => m_currentMovementState == MyCharacterMovementEnum.Jump;

		public bool IsMagneticBootsEnabled
		{
			get
			{
				if (!IsJumping && !IsOnLadder && !IsFalling && Physics != null && Physics.CharacterProxy != null && Physics.CharacterProxy.Gravity.LengthSquared() < 0.001f)
				{
					return !JetpackRunning;
				}
				return false;
			}
		}

		public bool IsMagneticBootsActive
		{
			get
			{
				if (IsMagneticBootsEnabled)
				{
					return (MyBootsState)m_bootsState == MyBootsState.Enabled;
				}
				return false;
			}
		}

		bool IMyCharacter.IsDead => IsDead;

		public long DeadPlayerIdentityId => m_deadPlayerIdentityId;

		public Vector3 ColorMask
		{
			get
			{
				return base.Render.ColorMaskHsv;
			}
			set
			{
				ChangeModelAndColor(ModelName, value, resetToDefault: false, 0L);
			}
		}

		public string ModelName
		{
			get
			{
				return m_characterModel;
			}
			set
			{
				ChangeModelAndColor(value, ColorMask, resetToDefault: false, 0L);
			}
		}

		public IMyGunObject<MyDeviceBase> CurrentWeapon => m_currentWeapon;

		public IMyHandheldGunObject<MyDeviceBase> LeftHandItem => m_leftHandItem as IMyHandheldGunObject<MyDeviceBase>;

		internal IMyControllableEntity CurrentRemoteControl
		{
			get;
			set;
		}

		public MyBattery SuitBattery => m_suitBattery;

		public override string DisplayNameText => base.DisplayName;

		public static bool CharactersCanDie
		{
			get
			{
				if (MySession.Static.CreativeMode)
				{
					return MyFakes.CHARACTER_CAN_DIE_EVEN_IN_CREATIVE_MODE;
				}
				return true;
			}
		}

		public bool CharacterCanDie
		{
			get
			{
				if (!CharactersCanDie)
				{
					if (ControllerInfo.Controller != null)
					{
						return ControllerInfo.Controller.Player.Id.SerialId != 0;
					}
					return false;
				}
				return true;
			}
		}

		public override Vector3D LocationForHudMarker => base.LocationForHudMarker + base.WorldMatrix.Up * 2.1;

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

		public MyEntity Entity => this;

		public MyControllerInfo ControllerInfo => m_info;

		public bool IsDead => m_currentMovementState == MyCharacterMovementEnum.Died;

		public bool IsSitting => m_currentMovementState == MyCharacterMovementEnum.Sitting;

		public float CurrentJump => m_currentJumpTime;

		public MyToolbarType ToolbarType => MyToolbarType.Character;

		VRage.ModAPI.IMyEntity IMyUseObject.Owner => this;

		MyModelDummy IMyUseObject.Dummy => null;

		float IMyUseObject.InteractiveDistance => 5f;

		MatrixD IMyUseObject.ActivationMatrix
		{
			get
			{
				if (base.PositionComp == null)
				{
					return MatrixD.Zero;
				}
				if (IsDead && Physics != null && Definition.DeadBodyShape != null)
				{
					float num = 0.8f;
					Matrix m = base.WorldMatrix;
					m.Forward *= num;
					m.Up *= Definition.CharacterCollisionHeight * num;
					m.Right *= num;
					m.Translation = base.PositionComp.WorldAABB.Center;
					m.Translation += 0.5f * m.Right * Definition.DeadBodyShape.RelativeShapeTranslation.X;
					m.Translation += 0.5f * m.Up * Definition.DeadBodyShape.RelativeShapeTranslation.Y;
					m.Translation += 0.5f * m.Forward * Definition.DeadBodyShape.RelativeShapeTranslation.Z;
					return m;
				}
				float num2 = 0.75f;
				Matrix m2 = base.WorldMatrix;
				m2.Forward *= num2;
				m2.Up *= Definition.CharacterCollisionHeight * num2;
				m2.Right *= num2;
				m2.Translation = base.PositionComp.WorldAABB.Center;
				return m2;
			}
		}

		MatrixD IMyUseObject.WorldMatrix => base.WorldMatrix;

		uint IMyUseObject.RenderObjectID => base.Render.GetRenderObjectID();

		int IMyUseObject.InstanceID => -1;

		bool IMyUseObject.ShowOverlay => false;

		UseActionEnum IMyUseObject.SupportedActions
		{
			get
			{
				if (IsDead && !Definition.EnableSpawnInventoryAsContainer)
				{
					return UseActionEnum.OpenTerminal | UseActionEnum.OpenInventory;
				}
				return UseActionEnum.None;
			}
		}

		UseActionEnum IMyUseObject.PrimaryAction
		{
			get
			{
				if (IsDead && !Definition.EnableSpawnInventoryAsContainer)
				{
					return UseActionEnum.OpenInventory;
				}
				return UseActionEnum.None;
			}
		}

		UseActionEnum IMyUseObject.SecondaryAction
		{
			get
			{
				if (IsDead && !Definition.EnableSpawnInventoryAsContainer)
				{
					return UseActionEnum.OpenTerminal;
				}
				return UseActionEnum.None;
			}
		}

		bool IMyUseObject.ContinuousUsage => false;

		bool IMyUseObject.PlayIndicatorSound => true;

		public bool UseDamageSystem
		{
			get;
			private set;
		}

		public float Integrity
		{
			get
			{
				float result = 100f;
				if (StatComp != null && StatComp.Health != null)
				{
					result = StatComp.Health.Value;
				}
				return result;
			}
		}

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return IsInFirstPersonView;
			}
			set
			{
				IsInFirstPersonView = value;
			}
		}

		bool IMyCameraController.ForceFirstPersonCamera
		{
			get
			{
				return ForceFirstPersonCamera;
			}
			set
			{
				ForceFirstPersonCamera = value;
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ForceFirstPersonCamera
		{
			get
			{
				return ForceFirstPersonCamera;
			}
			set
			{
				ForceFirstPersonCamera = value;
			}
		}

		bool IMyCameraController.AllowCubeBuilding => true;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledThrusts
		{
			get
			{
				if (JetpackComp != null)
				{
					return JetpackComp.TurnedOn;
				}
				return false;
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledDamping
		{
			get
			{
				if (JetpackComp != null)
				{
					return JetpackComp.DampenersTurnedOn;
				}
				return false;
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLights => LightEnabled;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLeadingGears => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledReactors => false;

		bool IMyControllableEntity.EnabledBroadcasting => RadioBroadcaster.Enabled;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledHelmet => OxygenComponent.HelmetEnabled;

		float IMyDestroyableObject.Integrity => Integrity;

		public bool PrimaryLookaround
		{
			get
			{
				if (IsOnLadder)
				{
					return !IsInFirstPersonView;
				}
				return false;
			}
		}

		public MyCharacterMovementFlags MovementFlags
		{
			get
			{
				return m_movementFlags;
			}
			internal set
			{
				m_movementFlags = value;
			}
		}

		public MyCharacterMovementFlags PreviousMovementFlags => m_previousMovementFlags;

		public bool WantsJump
		{
			get
			{
				return (m_movementFlags & MyCharacterMovementFlags.Jump) == MyCharacterMovementFlags.Jump;
			}
			private set
			{
				if (value)
				{
					m_movementFlags |= MyCharacterMovementFlags.Jump;
				}
				else
				{
					m_movementFlags &= ~MyCharacterMovementFlags.Jump;
				}
			}
		}

		private bool WantsSprint
		{
			get
			{
				return (m_movementFlags & MyCharacterMovementFlags.Sprint) == MyCharacterMovementFlags.Sprint;
			}
			set
			{
				if (value)
				{
					m_movementFlags |= MyCharacterMovementFlags.Sprint;
				}
				else
				{
					m_movementFlags &= ~MyCharacterMovementFlags.Sprint;
				}
			}
		}

		public bool WantsWalk
		{
			get
			{
				return (m_movementFlags & MyCharacterMovementFlags.Walk) == MyCharacterMovementFlags.Walk;
			}
			private set
			{
				if (value)
				{
					m_movementFlags |= MyCharacterMovementFlags.Walk;
				}
				else
				{
					m_movementFlags &= ~MyCharacterMovementFlags.Walk;
				}
			}
		}

		private bool WantsFlyUp
		{
			get
			{
				return (m_movementFlags & MyCharacterMovementFlags.FlyUp) == MyCharacterMovementFlags.FlyUp;
			}
			set
			{
				if (value)
				{
					m_movementFlags |= MyCharacterMovementFlags.FlyUp;
				}
				else
				{
					m_movementFlags &= ~MyCharacterMovementFlags.FlyUp;
				}
			}
		}

		private bool WantsFlyDown
		{
			get
			{
				return (m_movementFlags & MyCharacterMovementFlags.FlyDown) == MyCharacterMovementFlags.FlyDown;
			}
			set
			{
				if (value)
				{
					m_movementFlags |= MyCharacterMovementFlags.FlyDown;
				}
				else
				{
					m_movementFlags &= ~MyCharacterMovementFlags.FlyDown;
				}
			}
		}

		private bool WantsCrouch
		{
			get
			{
				return (m_movementFlags & MyCharacterMovementFlags.Crouch) == MyCharacterMovementFlags.Crouch;
			}
			set
			{
				if (value)
				{
					m_movementFlags |= MyCharacterMovementFlags.Crouch;
				}
				else
				{
					m_movementFlags &= ~MyCharacterMovementFlags.Crouch;
				}
			}
		}

		public MyCharacterBreath Breath => m_breath;

		public float CharacterAccumulatedDamage
		{
			get;
			set;
		}

		public MyStringId ControlContext
		{
			get
			{
				if (JetpackRunning)
				{
					return MySpaceBindingCreator.CX_JETPACK;
				}
				return MySpaceBindingCreator.CX_CHARACTER;
			}
		}

		public MyStringId AuxiliaryContext
		{
			get
			{
				if (MyCubeBuilder.Static.IsActivated)
				{
					if (MyCubeBuilder.Static.IsSymmetrySetupMode())
					{
						return MySpaceBindingCreator.AX_SYMMETRY;
					}
					if (MyCubeBuilder.Static.IsBuildToolActive())
					{
						return MySpaceBindingCreator.AX_BUILD;
					}
					if (MyCubeBuilder.Static.IsOnlyColorToolActive())
					{
						return MySpaceBindingCreator.AX_COLOR_PICKER;
					}
				}
				if (MySessionComponentVoxelHand.Static.Enabled)
				{
					return MySpaceBindingCreator.AX_VOXEL;
				}
				if (MyClipboardComponent.Static.IsActive)
				{
					return MySpaceBindingCreator.AX_CLIPBOARD;
				}
				return MySpaceBindingCreator.AX_TOOLS;
			}
		}

		public float EnvironmentOxygenLevel => EnvironmentOxygenLevelSync;

		public float OxygenLevel => OxygenLevelAtCharacterLocation;

		public float SuitEnergyLevel => SuitBattery.ResourceSource.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId) / 1E-05f;

		public bool IsPlayer => !MySession.Static.Players.IdentityIsNpc(GetPlayerIdentityId());

		public bool IsBot => !IsPlayer;

		public int SpineBoneIndex => m_spineBone;

		public int HeadBoneIndex => m_headBoneIndex;

		public VRage.ModAPI.IMyEntity EquippedTool => m_currentWeapon as VRage.ModAPI.IMyEntity;

		public MyEntity RelativeDampeningEntity
		{
			get
			{
				return m_relativeDampeningEntity;
			}
			set
			{
				if (m_relativeDampeningEntity != value)
				{
					if (m_relativeDampeningEntity != null)
					{
						m_relativeDampeningEntity.OnClose -= relativeDampeningEntityClosed;
					}
					m_relativeDampeningEntity = value;
					if (m_relativeDampeningEntity != null)
					{
						m_relativeDampeningEntity.OnClose += relativeDampeningEntityClosed;
					}
				}
			}
		}

		public bool IsOnLadder => m_ladder != null;

		public MyLadder Ladder => m_ladder;

		VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => Entity;

		public Vector3 LastMotionIndicator
		{
			get;
			set;
		}

		public Vector3 LastRotationIndicator
		{
			get;
			set;
		}

		IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => ControllerInfo;

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public static event Action<MyCharacter> OnCharacterDied;

		public event Action<IMyHandheldGunObject<MyDeviceBase>> WeaponEquiped;

		[Obsolete("OnMovementStateChanged is deprecated, use MovementStateChanged")]
		public event CharacterMovementStateDelegate OnMovementStateChanged;

		public event CharacterMovementStateChangedDelegate MovementStateChanged;

		public event EventHandler OnWeaponChanged;

		public event Action<MyCharacter> CharacterDied;

		event Action<IMyCharacter> IMyCharacter.CharacterDied
		{
			add
			{
				CharacterDied += GetDelegate(value);
			}
			remove
			{
				CharacterDied -= GetDelegate(value);
			}
		}

		private static MyBlueprintDefinitionBase MakeBlueprintFromBuildPlanItem(MyIdentity.BuildPlanItem buildPlanItem)
		{
			MyObjectBuilder_CompositeBlueprintDefinition myObjectBuilder_CompositeBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompositeBlueprintDefinition>();
			myObjectBuilder_CompositeBlueprintDefinition.Id = new SerializableDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), buildPlanItem.BlockDefinition.Id.ToString().Replace("MyObjectBuilder_", "BuildPlanItem_"));
			Dictionary<MyDefinitionId, MyFixedPoint> dictionary = new Dictionary<MyDefinitionId, MyFixedPoint>();
			foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
			{
				MyDefinitionId id = component.ComponentDefinition.Id;
				if (!dictionary.ContainsKey(id))
				{
					dictionary[id] = 0;
				}
				dictionary[id] += (MyFixedPoint)component.Count;
			}
			myObjectBuilder_CompositeBlueprintDefinition.Blueprints = new BlueprintItem[dictionary.Count];
			int num = 0;
			foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> item in dictionary)
			{
				MyBlueprintDefinitionBase myBlueprintDefinitionBase = null;
				if ((myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(item.Key)) == null)
				{
					return null;
				}
				myObjectBuilder_CompositeBlueprintDefinition.Blueprints[num] = new BlueprintItem
				{
					Id = new SerializableDefinitionId(myBlueprintDefinitionBase.Id.TypeId, myBlueprintDefinitionBase.Id.SubtypeName),
					Amount = item.Value.ToString()
				};
				num++;
			}
			myObjectBuilder_CompositeBlueprintDefinition.Icons = buildPlanItem.BlockDefinition.Icons;
			myObjectBuilder_CompositeBlueprintDefinition.DisplayName = (buildPlanItem.BlockDefinition.DisplayNameEnum.HasValue ? buildPlanItem.BlockDefinition.DisplayNameEnum.Value.ToString() : buildPlanItem.BlockDefinition.DisplayNameText);
			myObjectBuilder_CompositeBlueprintDefinition.Public = buildPlanItem.BlockDefinition.Public;
			MyCompositeBlueprintDefinition myCompositeBlueprintDefinition = new MyCompositeBlueprintDefinition();
			myCompositeBlueprintDefinition.Init(myObjectBuilder_CompositeBlueprintDefinition, MyModContext.BaseGame);
			myCompositeBlueprintDefinition.Postprocess();
			return myCompositeBlueprintDefinition;
		}

		public bool AddToBuildPlanner(MyCubeBlockDefinition block, int index = -1, List<MyIdentity.BuildPlanItem.Component> components = null)
		{
			if (GetIdentity().AddToBuildPlanner(block, index, components))
			{
				UpdateBuildPlanner();
				return true;
			}
			return false;
		}

		public void RemoveAtBuildPlanner(int index)
		{
			GetIdentity().RemoveAtBuildPlanner(index);
			UpdateBuildPlanner();
		}

		public void CleanFinishedBuildPlanner()
		{
			GetIdentity().CleanFinishedBuildPlanner();
			UpdateBuildPlanner();
		}

		private void LoadBuildPlanner(MyObjectBuilder_Character.BuildPlanItem[] buildPlanner)
		{
			MyIdentity identity = GetIdentity();
			if (identity != null)
			{
				identity.LoadBuildPlanner(buildPlanner);
				UpdateBuildPlanner(synchronize: false);
			}
		}

		private void UpdateBuildPlanner(bool synchronize = true)
		{
			if (m_buildPlannerBlueprintClass == null)
			{
				m_buildPlannerBlueprintClass = MyDefinitionManager.Static.GetBlueprintClass("BuildPlanner");
			}
			if (m_buildPlannerBlueprintClass != null)
			{
				m_buildPlannerBlueprintClass.ClearBlueprints();
				foreach (MyIdentity.BuildPlanItem item in GetIdentity().BuildPlanner)
				{
					MyBlueprintDefinitionBase blueprint = MakeBlueprintFromBuildPlanItem(item);
					if (!m_buildPlannerBlueprintClass.ContainsBlueprint(blueprint))
					{
						m_buildPlannerBlueprintClass.AddBlueprint(blueprint);
					}
				}
				if (synchronize)
				{
					SynchronizeBuildPlanner();
				}
			}
		}

		private MyObjectBuilder_Character.BuildPlanItem[] SaveBuildPlanner()
		{
			List<MyObjectBuilder_Character.BuildPlanItem> list = new List<MyObjectBuilder_Character.BuildPlanItem>();
			foreach (MyIdentity.BuildPlanItem item3 in GetIdentity().BuildPlanner)
			{
				MyObjectBuilder_Character.BuildPlanItem item = default(MyObjectBuilder_Character.BuildPlanItem);
				item.BlockId = item3.BlockDefinition.Id;
				item.IsInProgress = item3.IsInProgress;
				item.Components = new List<MyObjectBuilder_Character.ComponentItem>();
				foreach (MyIdentity.BuildPlanItem.Component component in item3.Components)
				{
					MyObjectBuilder_Character.ComponentItem item2 = default(MyObjectBuilder_Character.ComponentItem);
					item2.ComponentId = component.ComponentDefinition.Id;
					item2.Count = component.Count;
					item.Components.Add(item2);
				}
				list.Add(item);
			}
			return list.ToArray();
		}

		internal void SynchronizeBuildPlanner()
		{
			if (!Sync.IsServer)
			{
				MyObjectBuilder_Character.BuildPlanItem[] arg = SaveBuildPlanner();
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.SynchronizeBuildPlanner_Implementation, arg);
			}
		}

		[Event(null, 184)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void SynchronizeBuildPlanner_Implementation(MyObjectBuilder_Character.BuildPlanItem[] buildPlanner)
		{
			GetIdentity().LoadBuildPlanner(buildPlanner);
			UpdateBuildPlanner(synchronize: false);
		}

		public bool UpdateCalled()
		{
			bool result = m_actualUpdateFrame != m_actualDrawFrame;
			m_actualDrawFrame = m_actualUpdateFrame;
			return result;
		}

		public bool IsShooting(MyShootActionEnum action)
		{
			return m_isShooting[(uint)action];
		}

		public MyShootActionEnum? GetShootingAction()
		{
			MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
			foreach (MyShootActionEnum myShootActionEnum in values)
			{
				if (m_isShooting[(uint)myShootActionEnum])
				{
					return myShootActionEnum;
				}
			}
			return null;
		}

		bool IMyComponentOwner<MyIDModule>.GetComponent(out MyIDModule module)
		{
			module = m_idModule;
			return true;
		}

		public static MyObjectBuilder_Character Random()
		{
			return new MyObjectBuilder_Character
			{
				CharacterModel = DefaultModel,
				SubtypeName = DefaultModel,
				ColorMaskHSV = m_defaultColors[MyUtils.GetRandomInt(0, m_defaultColors.Length)]
			};
		}

		public MyCharacter()
		{
			ControllerInfo.ControlAcquired += OnControlAcquired;
			ControllerInfo.ControlReleased += OnControlReleased;
			CustomNameWithFaction = new StringBuilder();
			base.PositionComp = new MyCharacterPosition();
			(base.PositionComp as MyPositionComponent).WorldPositionChanged = WorldPositionChanged;
			Render = new MyRenderComponentCharacter();
			Render.EnableColorMaskHsv = true;
			Render.NeedsDraw = true;
			Render.CastShadows = true;
			Render.NeedsResolveCastShadow = false;
			Render.SkipIfTooSmall = false;
			Render.DrawInAllCascades = true;
			Render.MetalnessColorable = true;
			SinkComp = new MyResourceSinkComponent();
			SyncType = SyncHelpers.Compose(this);
			AddDebugRenderComponent(new MyDebugRenderComponentCharacter(this));
			if (MyPerGameSettings.CharacterDetectionComponent != null)
			{
				base.Components.Add((MyCharacterDetectorComponent)Activator.CreateInstance(MyPerGameSettings.CharacterDetectionComponent));
			}
			else
			{
				base.Components.Add((MyCharacterDetectorComponent)new MyCharacterRaycastDetectorComponent());
			}
			m_currentAmmoCount.AlwaysReject();
			m_currentMagazineAmmoCount.AlwaysReject();
			m_controlInfo.ValueChanged += delegate
			{
				ControlChanged();
			};
			m_controlInfo.AlwaysReject();
			m_isShooting = new bool[(uint)(MyEnum<MyShootActionEnum>.Range.Max + 1)];
			base.OnClose += MyCharacter_OnClose;
			base.OnClosing += MyEntity_OnClosing;
		}

		private void MyCharacter_OnClose(MyEntity obj)
		{
			if (Render != null)
			{
				Render.CleanLights();
			}
		}

		private void MyEntity_OnClosing(MyEntity entity)
		{
			if ((entity as MyCharacter).DeadPlayerIdentityId == MySession.Static.LocalPlayerId)
			{
				RadioReceiver.Clear();
			}
		}

		private static string GetRealModel(string asset, ref Vector3 colorMask)
		{
			if (!string.IsNullOrEmpty(asset) && MyObjectBuilder_Character.CharacterModels.ContainsKey(asset))
			{
				SerializableVector3 v = MyObjectBuilder_Character.CharacterModels[asset];
				if (v.X > -1f || v.Y > -1f || v.Z > -1f)
				{
					colorMask = v;
				}
				asset = DefaultModel;
			}
			return asset;
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			RadioReceiver = new MyRadioReceiver();
			base.Components.Add((MyDataBroadcaster)new MyRadioBroadcaster());
			RadioBroadcaster.BroadcastRadius = 200f;
			base.SyncFlag = true;
			MyObjectBuilder_Character myObjectBuilder_Character = (MyObjectBuilder_Character)objectBuilder;
			if (myObjectBuilder_Character.OwningPlayerIdentityId.HasValue)
			{
				m_idModule.Owner = myObjectBuilder_Character.OwningPlayerIdentityId.Value;
			}
			else
			{
				m_idModule.Owner = MySession.Static.Players.TryGetIdentityId(myObjectBuilder_Character.PlayerSteamId, myObjectBuilder_Character.PlayerSerialId);
			}
			Render.ColorMaskHsv = myObjectBuilder_Character.ColorMaskHSV;
			Vector3 colorMask = Render.ColorMaskHsv;
			GetModelAndDefinition(myObjectBuilder_Character, out m_characterModel, out m_characterDefinition, ref colorMask);
			m_physicalMaterialHash = MyStringHash.GetOrCompute(m_characterDefinition.PhysicalMaterial);
			UseNewAnimationSystem = m_characterDefinition.UseNewAnimationSystem;
			if (UseNewAnimationSystem && (!Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.DisableAnimationsOnDS))
			{
				base.AnimationController.Clear();
				MyStringHash orCompute = MyStringHash.GetOrCompute(m_characterDefinition.AnimationController);
				MyAnimationControllerDefinition definition = MyDefinitionManager.Static.GetDefinition<MyAnimationControllerDefinition>(orCompute);
				if (definition != null)
				{
					base.AnimationController.InitFromDefinition(definition);
				}
			}
			if (Render.ColorMaskHsv != colorMask)
			{
				Render.ColorMaskHsv = colorMask;
			}
			myObjectBuilder_Character.SubtypeName = m_characterDefinition.Id.SubtypeName;
			base.Init(objectBuilder);
			m_currentAnimationChangeDelay = 0f;
			SoundComp = new MyCharacterSoundComponent();
			RadioBroadcaster.WantsToBeEnabled = (myObjectBuilder_Character.EnableBroadcasting && Definition.VisibleOnHud);
			Init(new StringBuilder(myObjectBuilder_Character.DisplayName), m_characterDefinition.Model, null, null);
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.SIMULATE);
			SetStandingLocalAABB();
			m_currentLootingCounter = myObjectBuilder_Character.LootingCounter;
			if (m_currentLootingCounter <= 0f)
			{
				UpdateCharacterPhysics();
			}
			m_currentMovementState = myObjectBuilder_Character.MovementState;
			if (Physics != null && Physics.CharacterProxy != null)
			{
				switch (m_currentMovementState)
				{
				case MyCharacterMovementEnum.Flying:
				case MyCharacterMovementEnum.Falling:
					Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_IN_AIR);
					break;
				case MyCharacterMovementEnum.Jump:
					Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_JUMPING);
					break;
				case MyCharacterMovementEnum.Ladder:
				case MyCharacterMovementEnum.LadderUp:
				case MyCharacterMovementEnum.LadderDown:
				case MyCharacterMovementEnum.LadderOut:
					Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_CLIMBING);
					break;
				default:
					Physics.CharacterProxy.SetState(HkCharacterStateType.HK_CHARACTER_ON_GROUND);
					break;
				}
			}
			InitAnimations();
			ValidateBonesProperties();
			CalculateTransforms(0f);
			InitAnimationCorrection();
			if (m_currentLootingCounter > 0f)
			{
				InitDeadBodyPhysics();
				if (m_currentMovementState != MyCharacterMovementEnum.Died)
				{
					SetCurrentMovementState(MyCharacterMovementEnum.Died);
				}
				SwitchAnimation(MyCharacterMovementEnum.Died, checkState: false);
			}
			InitInventory(myObjectBuilder_Character);
			if (myObjectBuilder_Character.BuildPlanner != null)
			{
				LoadBuildPlanner(myObjectBuilder_Character.BuildPlanner.ToArray());
			}
			Physics.Enabled = true;
			SetHeadLocalXAngle(myObjectBuilder_Character.HeadAngle.X);
			SetHeadLocalYAngle(myObjectBuilder_Character.HeadAngle.Y);
			Render.InitLight(m_characterDefinition);
			Render.InitJetpackThrusts(m_characterDefinition);
			m_lightEnabled = myObjectBuilder_Character.LightEnabled;
			Physics.LinearVelocity = myObjectBuilder_Character.LinearVelocity;
			if (Physics.CharacterProxy != null)
			{
				Physics.CharacterProxy.ContactPointCallbackEnabled = true;
				Physics.CharacterProxy.ContactPointCallback += RigidBody_ContactPointCallback;
			}
			Render.UpdateLightProperties(m_currentLightPower);
			IsInFirstPersonView = (!MySession.Static.Settings.Enable3rdPersonView || myObjectBuilder_Character.IsInFirstPersonView);
			m_breath = new MyCharacterBreath(this);
			m_notEnoughStatNotification = new MyHudNotification(MyCommonTexts.NotificationStatNotEnough, 1000, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			if (InventoryAggregate != null)
			{
				InventoryAggregate.Init();
			}
			UseDamageSystem = true;
			if (myObjectBuilder_Character.EnabledComponents == null)
			{
				myObjectBuilder_Character.EnabledComponents = new List<string>();
			}
			foreach (string componentName in m_characterDefinition.EnabledComponents)
			{
				if (myObjectBuilder_Character.EnabledComponents.All((string x) => x != componentName))
				{
					myObjectBuilder_Character.EnabledComponents.Add(componentName);
				}
			}
			foreach (string enabledComponent in myObjectBuilder_Character.EnabledComponents)
			{
				if (MyCharacterComponentTypes.CharacterComponents.TryGetValue(MyStringId.GetOrCompute(enabledComponent), out Tuple<Type, Type> value))
				{
					MyEntityComponentBase component = Activator.CreateInstance(value.Item1) as MyEntityComponentBase;
					base.Components.Add(value.Item2, component);
				}
			}
			if (m_characterDefinition.UsesAtmosphereDetector)
			{
				AtmosphereDetectorComp = new MyAtmosphereDetectorComponent();
				AtmosphereDetectorComp.InitComponent(onlyLocalPlayer: true, this);
			}
			if (m_characterDefinition.UsesReverbDetector)
			{
				ReverbDetectorComp = new MyEntityReverbDetectorComponent();
				ReverbDetectorComp.InitComponent(this, sendInformationToAudio: true);
			}
			bool num = Definition.SuitResourceStorage.Count > 0;
			List<MyResourceSinkInfo> list = new List<MyResourceSinkInfo>();
			List<MyResourceSourceInfo> list2 = new List<MyResourceSourceInfo>();
			if (num)
			{
				OxygenComponent = new MyCharacterOxygenComponent();
				base.Components.Add(OxygenComponent);
				OxygenComponent.Init(myObjectBuilder_Character);
				OxygenComponent.AppendSinkData(list);
				OxygenComponent.AppendSourceData(list2);
			}
			m_suitBattery = new MyBattery(this);
			m_suitBattery.Init(myObjectBuilder_Character.Battery, list, list2);
			if (num)
			{
				OxygenComponent.CharacterGasSink = m_suitBattery.ResourceSink;
				OxygenComponent.CharacterGasSource = m_suitBattery.ResourceSource;
			}
			list.Clear();
			MyResourceSinkInfo item = new MyResourceSinkInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				MaxRequiredInput = 1.19999995E-05f,
				RequiredInputFunc = ComputeRequiredPower
			};
			list.Add(item);
			if (num)
			{
				item = new MyResourceSinkInfo
				{
					ResourceTypeId = MyCharacterOxygenComponent.OxygenId,
					MaxRequiredInput = (OxygenComponent.OxygenCapacity + ((!OxygenComponent.NeedsOxygenFromSuit) ? Definition.OxygenConsumption : 0f)) * Definition.OxygenConsumptionMultiplier * 60f / 100f,
					RequiredInputFunc = (() => (OxygenComponent.HelmetEnabled ? Definition.OxygenConsumption : 0f) * Definition.OxygenConsumptionMultiplier * 60f / 100f)
				};
				list.Add(item);
			}
			SinkComp.Init(MyStringHash.GetOrCompute("Utility"), list);
			SinkComp.CurrentInputChanged += delegate
			{
				SetPowerInput(SinkComp.CurrentInputByType(MyResourceDistributorComponent.ElectricityId));
			};
			SinkComp.TemporaryConnectedEntity = this;
			SuitRechargeDistributor = new MyResourceDistributorComponent(ToString());
			SuitRechargeDistributor.AddSource(m_suitBattery.ResourceSource);
			SuitRechargeDistributor.AddSink(SinkComp);
			SinkComp.Update();
			if (m_characterDefinition.Jetpack != null)
			{
				JetpackComp = new MyCharacterJetpackComponent();
				JetpackComp.Init(myObjectBuilder_Character);
			}
			WeaponPosition = new MyCharacterWeaponPositionComponent();
			base.Components.Add(WeaponPosition);
			WeaponPosition.Init(myObjectBuilder_Character);
			InitWeapon(myObjectBuilder_Character.HandWeapon);
			if (Definition.RagdollBonesMappings.Count > 0)
			{
				CreateBodyCapsulesForHits(Definition.RagdollBonesMappings);
			}
			else
			{
				m_bodyCapsuleInfo.Clear();
			}
			PlayCharacterAnimation(Definition.InitialAnimation, MyBlendOption.Immediate, MyFrameOption.JustFirstFrame, 0f);
			m_savedHealth = myObjectBuilder_Character.Health;
			m_savedPlayer = new MyPlayer.PlayerId(myObjectBuilder_Character.PlayerSteamId, myObjectBuilder_Character.PlayerSerialId);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			m_previousLinearVelocity = myObjectBuilder_Character.LinearVelocity;
			ControllerInfo.IsLocallyControlled();
			CheckExistingStatComponent();
			CharacterGeneralDamageModifier = myObjectBuilder_Character.CharacterGeneralDamageModifier;
			m_resolveHighlightOverlap = true;
			IsPersistenceCharacter = myObjectBuilder_Character.IsPersistenceCharacter;
			m_bootsState.ValueChanged += OnBootsStateChanged;
			if (Sync.IsServer)
			{
				m_bootsState.Value = MyBootsState.Init;
			}
			m_relativeDampeningEntityInit = myObjectBuilder_Character.RelativeDampeningEntity;
			m_ladderIdInit = myObjectBuilder_Character.UsingLadder;
			m_ladderInfoInit = myObjectBuilder_Character.UsingLadderInfo;
		}

		private void CheckExistingStatComponent()
		{
			if (StatComp == null)
			{
				bool flag = false;
				MyContainerDefinition definition = null;
				MyComponentContainerExtension.TryGetContainerDefinition(m_characterDefinition.Id.TypeId, m_characterDefinition.Id.SubtypeId, out definition);
				if (definition != null)
				{
					foreach (MyContainerDefinition.DefaultComponent defaultComponent in definition.DefaultComponents)
					{
						if (defaultComponent.BuilderType == typeof(MyObjectBuilder_CharacterStatComponent))
						{
							flag = true;
							break;
						}
					}
				}
				string msg = string.Concat("Stat component has not been created for character: ", m_characterDefinition.Id, ", container defined: ", (definition != null).ToString(), ", stat component defined: ", flag.ToString());
				MyLog.Default.WriteLine(msg);
			}
		}

		private void InitAnimationCorrection()
		{
			if (IsDead && UseNewAnimationSystem)
			{
				base.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdDead, 1f);
			}
		}

		public static void GetModelAndDefinition(MyObjectBuilder_Character characterOb, out string characterModel, out MyCharacterDefinition characterDefinition, ref Vector3 colorMask)
		{
			characterModel = GetRealModel(characterOb.CharacterModel, ref colorMask);
			characterDefinition = null;
			if (string.IsNullOrEmpty(characterModel) || !MyDefinitionManager.Static.Characters.TryGetValue(characterModel, out characterDefinition))
			{
				characterDefinition = MyDefinitionManager.Static.Characters.First();
				characterModel = characterDefinition.Model;
			}
		}

		private void InitInventory(MyObjectBuilder_Character characterOb)
		{
			if (this.GetInventory() == null)
			{
				if (m_characterDefinition.InventoryDefinition == null)
				{
					m_characterDefinition.InventoryDefinition = new MyObjectBuilder_InventoryDefinition();
				}
				MyInventory myInventory = new MyInventory(m_characterDefinition.InventoryDefinition, (MyInventoryFlags)0);
				myInventory.Init(null);
				if (InventoryAggregate != null)
				{
					InventoryAggregate.AddComponent(myInventory);
				}
				else
				{
					base.Components.Add((MyInventoryBase)myInventory);
				}
				myInventory.Init(characterOb.Inventory);
				MyCubeBuilder.BuildComponent.AfterCharacterCreate(this);
				if (MyFakes.ENABLE_MEDIEVAL_INVENTORY && InventoryAggregate != null)
				{
					MyInventoryAggregate myInventoryAggregate = InventoryAggregate.GetInventory(MyStringHash.GetOrCompute("Internal")) as MyInventoryAggregate;
					if (myInventoryAggregate != null)
					{
						myInventoryAggregate.AddComponent(myInventory);
					}
					else
					{
						InventoryAggregate.AddComponent(myInventory);
					}
				}
			}
			else if (MyPerGameSettings.ConstrainInventory())
			{
				MyInventory inventory = this.GetInventory();
				if (inventory.IsConstrained)
				{
					inventory.FixInventoryVolume(m_characterDefinition.InventoryDefinition.InventoryVolume);
				}
			}
			this.GetInventory().ContentsChanged -= inventory_OnContentsChanged;
			this.GetInventory().BeforeContentsChanged -= inventory_OnBeforeContentsChanged;
			this.GetInventory().BeforeRemovedFromContainer -= inventory_OnRemovedFromContainer;
			this.GetInventory().ContentsChanged += inventory_OnContentsChanged;
			this.GetInventory().BeforeContentsChanged += inventory_OnBeforeContentsChanged;
			this.GetInventory().BeforeRemovedFromContainer += inventory_OnRemovedFromContainer;
		}

		private void CreateBodyCapsulesForHits(Dictionary<string, MyCharacterDefinition.RagdollBoneSet> bonesMappings)
		{
			m_bodyCapsuleInfo.Clear();
			m_bodyCapsules = new CapsuleD[bonesMappings.Count];
			foreach (KeyValuePair<string, MyCharacterDefinition.RagdollBoneSet> bonesMapping in bonesMappings)
			{
				try
				{
					string[] bones = bonesMapping.Value.Bones;
					int index;
					MyCharacterBone myCharacterBone = base.AnimationController.FindBone(bones.First(), out index);
					int index2;
					MyCharacterBone myCharacterBone2 = base.AnimationController.FindBone(bones.Last(), out index2);
					if (myCharacterBone != null && myCharacterBone2 != null)
					{
						if (myCharacterBone.Depth > myCharacterBone2.Depth)
						{
							int num = index;
							index = index2;
							index2 = num;
						}
						m_bodyCapsuleInfo.Add(new MyBoneCapsuleInfo
						{
							Bone1 = myCharacterBone.Index,
							Bone2 = myCharacterBone2.Index,
							AscendantBone = index,
							DescendantBone = index2,
							Radius = bonesMapping.Value.CollisionRadius
						});
					}
				}
				catch (Exception)
				{
				}
			}
			int num2 = 0;
			while (true)
			{
				if (num2 < m_bodyCapsuleInfo.Count)
				{
					if (m_bodyCapsuleInfo[num2].Bone1 == m_headBoneIndex)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			m_bodyCapsuleInfo.Move(num2, 0);
		}

		private void Toolbar_ItemChanged(MyToolbar toolbar, MyToolbar.IndexArgs index)
		{
			MyToolbarItem itemAtIndex = toolbar.GetItemAtIndex(index.ItemIndex);
			if (itemAtIndex != null)
			{
				MyToolbarItemDefinition myToolbarItemDefinition = itemAtIndex as MyToolbarItemDefinition;
				if (myToolbarItemDefinition != null)
				{
					MyDefinitionId id = myToolbarItemDefinition.Definition.Id;
					if (id.TypeId != typeof(MyObjectBuilder_PhysicalGunObject))
					{
						MyToolBarCollection.RequestChangeSlotItem(MySession.Static.LocalHumanPlayer.Id, index.ItemIndex, id);
					}
					else
					{
						MyToolBarCollection.RequestChangeSlotItem(MySession.Static.LocalHumanPlayer.Id, index.ItemIndex, itemAtIndex.GetObjectBuilder());
					}
				}
			}
			else if (MySandboxGame.IsGameReady)
			{
				MyToolBarCollection.RequestClearSlot(MySession.Static.LocalHumanPlayer.Id, index.ItemIndex);
			}
		}

		private void inventory_OnRemovedFromContainer(MyEntityComponentBase component)
		{
			this.GetInventory().BeforeRemovedFromContainer -= inventory_OnRemovedFromContainer;
			this.GetInventory().ContentsChanged -= inventory_OnContentsChanged;
			this.GetInventory().BeforeContentsChanged -= inventory_OnBeforeContentsChanged;
		}

		private void inventory_OnContentsChanged(MyInventoryBase inventory)
		{
			if (this == MySession.Static.LocalCharacter)
			{
				if (m_currentWeapon != null && WeaponTakesBuilderFromInventory(m_currentWeapon.DefinitionId) && inventory != null && inventory is MyInventory && !(inventory as MyInventory).ContainItems(1, m_currentWeapon.PhysicalObject))
				{
					SwitchToWeapon((MyToolbarItemWeapon)null);
				}
				if (LeftHandItem != null && !CanSwitchToWeapon(LeftHandItem.DefinitionId))
				{
					LeftHandItem.OnControlReleased();
					m_leftHandItem.Close();
					m_leftHandItem = null;
				}
			}
		}

		private void inventory_OnBeforeContentsChanged(MyInventoryBase inventory)
		{
			if (this == MySession.Static.LocalCharacter && m_currentWeapon != null && WeaponTakesBuilderFromInventory(m_currentWeapon.DefinitionId) && inventory != null && inventory is MyInventory && (inventory as MyInventory).ContainItems(1, m_currentWeapon.PhysicalObject))
			{
				SaveAmmoToWeapon();
			}
		}

		private void RigidBody_ContactPointCallback(ref HkContactPointEvent value)
		{
			if (IsDead || Physics == null || Physics.CharacterProxy == null || MySession.Static == null || value.Base.BodyA == null || value.Base.BodyB == null || value.Base.BodyA.UserObject == null || value.Base.BodyB.UserObject == null || value.Base.BodyA.HasProperty(254) || value.Base.BodyB.HasProperty(254))
			{
				return;
			}
			if (Render != null)
			{
				Render.TrySpawnWalkingParticles(ref value);
			}
			int num = 0;
			Vector3 vector = value.ContactPoint.Normal;
			MyEntity other = value.GetPhysicsBody(num).Entity as MyEntity;
			HkRigidBody hkRigidBody = value.Base.BodyA;
			if (other == this)
			{
				num = 1;
				other = (value.GetPhysicsBody(num).Entity as MyEntity);
				hkRigidBody = value.Base.BodyB;
				vector = -vector;
			}
			MyCharacter myCharacter = other as MyCharacter;
			if (myCharacter != null && myCharacter.Physics != null)
			{
				if (myCharacter.IsDead)
				{
					if (myCharacter.Physics.Ragdoll != null && myCharacter.Physics.Ragdoll.GetRootRigidBody().HasProperty(254))
					{
						return;
					}
				}
				else if (myCharacter.Physics.CharacterProxy == null || (Physics.CharacterProxy.Supported && myCharacter.Physics.CharacterProxy.Supported))
				{
					return;
				}
			}
			MyCubeGrid myCubeGrid = other as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (IsOnLadder)
				{
					uint shapeKey = value.GetShapeKey(num);
					bool flag = shapeKey == uint.MaxValue;
					if (!flag)
					{
						MySlimBlock blockFromShapeKey = myCubeGrid.Physics.Shape.GetBlockFromShapeKey(shapeKey);
						if (blockFromShapeKey != null)
						{
							MyLadder myLadder = blockFromShapeKey.FatBlock as MyLadder;
							flag = (myLadder != null && !ShouldCollideWith(myLadder));
						}
					}
					if (flag)
					{
						HkContactPointProperties contactProperties = value.ContactProperties;
						contactProperties.IsDisabled = true;
					}
				}
				if (MyFakes.ENABLE_REALISTIC_ON_TOUCH && SoundComp != null)
				{
					SoundComp.UpdateEntityEmitters(myCubeGrid);
				}
			}
			if (Math.Abs(value.SeparatingVelocity) < 3f)
			{
				return;
			}
			Vector3 linearVelocity = Physics.LinearVelocity;
			if ((linearVelocity - m_previousLinearVelocity).Length() > 10f)
			{
				return;
			}
			Vector3 velocityAtPoint = hkRigidBody.GetVelocityAtPoint(value.ContactPoint.Position);
			float num2 = linearVelocity.Length();
			float num3 = velocityAtPoint.Length();
			Vector3 vector2 = (num2 > 0f) ? Vector3.Normalize(linearVelocity) : Vector3.Zero;
			Vector3 vector3 = (num3 > 0f) ? Vector3.Normalize(velocityAtPoint) : Vector3.Zero;
			float num4 = (num2 > 0f) ? Vector3.Dot(vector2, vector) : 0f;
			float num5 = (num3 > 0f) ? (0f - Vector3.Dot(vector3, vector)) : 0f;
			num2 *= num4;
			num3 *= num5;
			float num6 = Math.Min(num2 + num3, Math.Abs(value.SeparatingVelocity) - 17f);
			if (num6 >= -8f && m_canPlayImpact <= 0f)
			{
				m_canPlayImpact = 0.3f;
				HkContactPointEvent hkContactPointEvent = value;
				Func<bool> canHear = delegate
				{
					if (MySession.Static.ControlledEntity != null)
					{
						MyEntity topMostParent = MySession.Static.ControlledEntity.Entity.GetTopMostParent();
						if (topMostParent != hkContactPointEvent.GetPhysicsBody(0).Entity)
						{
							return topMostParent == hkContactPointEvent.GetPhysicsBody(1).Entity;
						}
						return true;
					}
					return false;
				};
				MyPhysicsBody body = value.Base.BodyB.GetBody();
				Vector3D vector3D = Physics.ClusterToWorld(value.ContactPoint.Position);
				MyStringHash materialAt = body.GetMaterialAt(vector3D - value.ContactPoint.Normal * 0.1f);
				float volume = (Math.Abs(value.SeparatingVelocity) < 15f) ? (0.5f + Math.Abs(value.SeparatingVelocity) / 30f) : 1f;
				MyAudioComponent.PlayContactSound(Entity.EntityId, m_stringIdHit, vector3D, m_physicalMaterialHash, materialAt, volume, canHear);
			}
			if (!Sync.IsServer || num6 < 0f)
			{
				return;
			}
			float num7 = MyDestructionHelper.MassFromHavok(Physics.Mass);
			float num8 = MyDestructionHelper.MassFromHavok(hkRigidBody.Mass);
			float num9;
			if (num7 > num8 && !hkRigidBody.IsFixedOrKeyframed)
			{
				num9 = num8;
			}
			else
			{
				num9 = MyDestructionHelper.MassToHavok(70f);
				if (Physics.CharacterProxy.Supported && !hkRigidBody.IsFixedOrKeyframed)
				{
					num9 += Math.Abs(Vector3.Dot(Vector3.Normalize(velocityAtPoint), Physics.CharacterProxy.SupportNormal)) * num8 / 10f;
				}
			}
			num9 = MyDestructionHelper.MassFromHavok(num9);
			float impact = num9 * num6 * num6 / 2f;
			if (num3 > 2f)
			{
				impact -= 400f;
			}
			else if (num3 == 0f && impact > 100f)
			{
				impact /= 80f;
			}
			impact /= 10f;
			if (impact >= 1f && Sync.IsServer)
			{
				if (value.GetPhysicsBody(0).Entity == this)
				{
					_ = value.GetPhysicsBody(1).Entity;
				}
				MySandboxGame.Static.Invoke(delegate
				{
					DoDamage(impact, MyDamageType.Environment, updateSync: true, (other != null) ? other.EntityId : 0);
				}, "MyCharacter.DoDamage");
			}
		}

		private DamageImpactEnum GetDamageFromSqueeze(HkRigidBody collidingBody, MyEntity collidingEntity, ref HkContactPointEvent value)
		{
			if (collidingBody.IsFixed || collidingBody.Mass < MyPerGameSettings.CharacterSqueezeMinMass)
			{
				return DamageImpactEnum.NoDamage;
			}
			if (value.ContactProperties.IsNew)
			{
				return DamageImpactEnum.NoDamage;
			}
			Vector3 vector = Physics.CharacterProxy.Position - collidingBody.Position;
			vector.Normalize();
			Vector3 gravity = m_gravity;
			gravity.Normalize();
			if (Vector3.Dot(vector, gravity) < 0.5f)
			{
				return DamageImpactEnum.NoDamage;
			}
			if (m_squeezeDamageTimer > 0f)
			{
				m_squeezeDamageTimer -= 0.0166666675f;
				return DamageImpactEnum.NoDamage;
			}
			m_squeezeDamageTimer = MyPerGameSettings.CharacterSqueezeDamageDelay;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
			{
				MatrixD worldMatrix = collidingEntity.Physics.GetWorldMatrix();
				int shapeIndex = 2;
				MyPhysicsDebugDraw.DrawCollisionShape(collidingBody.GetShape(), worldMatrix, 1f, ref shapeIndex);
				MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "SQUEEZE, MASS:" + collidingBody.Mass, Color.Yellow, 2f, depthRead: false);
			}
			if (collidingBody.Mass > MyPerGameSettings.CharacterSqueezeDeadlyDamageMass)
			{
				return DamageImpactEnum.DeadlyDamage;
			}
			if (collidingBody.Mass > MyPerGameSettings.CharacterSqueezeCriticalDamageMass)
			{
				return DamageImpactEnum.CriticalDamage;
			}
			if (collidingBody.Mass > MyPerGameSettings.CharacterSqueezeMediumDamageMass)
			{
				return DamageImpactEnum.MediumDamage;
			}
			return DamageImpactEnum.SmallDamage;
		}

		private DamageImpactEnum GetDamageFromHit(HkRigidBody collidingBody, MyEntity collidingEntity, ref HkContactPointEvent value)
		{
			if (collidingBody.LinearVelocity.Length() < MyPerGameSettings.CharacterDamageHitObjectMinVelocity)
			{
				return DamageImpactEnum.NoDamage;
			}
			if (collidingEntity == ManipulatedEntity)
			{
				return DamageImpactEnum.NoDamage;
			}
			if (collidingBody.HasProperty(254))
			{
				return DamageImpactEnum.NoDamage;
			}
			float num = MyPerGameSettings.Destruction ? MyDestructionHelper.MassFromHavok(collidingBody.Mass) : collidingBody.Mass;
			if (num < MyPerGameSettings.CharacterDamageHitObjectMinMass)
			{
				return DamageImpactEnum.NoDamage;
			}
			float num2 = Math.Abs(value.SeparatingVelocity) * num;
			if (num2 > MyPerGameSettings.CharacterDamageHitObjectDeadlyEnergy)
			{
				return DamageImpactEnum.DeadlyDamage;
			}
			if (num2 > MyPerGameSettings.CharacterDamageHitObjectCriticalEnergy)
			{
				return DamageImpactEnum.CriticalDamage;
			}
			if (num2 > MyPerGameSettings.CharacterDamageHitObjectMediumEnergy)
			{
				return DamageImpactEnum.MediumDamage;
			}
			if (num2 > MyPerGameSettings.CharacterDamageHitObjectSmallEnergy)
			{
				return DamageImpactEnum.SmallDamage;
			}
			return DamageImpactEnum.NoDamage;
		}

		private void ApplyDamage(DamageImpactEnum damageImpact, MyStringHash myDamageType)
		{
			if (Sync.IsServer)
			{
				if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE && damageImpact != 0)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(100f, 100f), "DAMAGE! TYPE: " + myDamageType.ToString() + " IMPACT: " + damageImpact, Color.Red, 1f);
				}
				switch (damageImpact)
				{
				case DamageImpactEnum.NoDamage:
					break;
				case DamageImpactEnum.SmallDamage:
					DoDamage(MyPerGameSettings.CharacterSmallDamage, myDamageType, updateSync: true, 0L);
					break;
				case DamageImpactEnum.MediumDamage:
					DoDamage(MyPerGameSettings.CharacterMediumDamage, myDamageType, updateSync: true, 0L);
					break;
				case DamageImpactEnum.CriticalDamage:
					DoDamage(MyPerGameSettings.CharacterCriticalDamage, myDamageType, updateSync: true, 0L);
					break;
				case DamageImpactEnum.DeadlyDamage:
					DoDamage(MyPerGameSettings.CharacterDeadlyDamage, myDamageType, updateSync: true, 0L);
					break;
				}
			}
		}

		private DamageImpactEnum GetDamageFromFall(HkRigidBody collidingBody, MyEntity collidingEntity, ref HkContactPointEvent value)
		{
			float num = Vector3.Dot(value.ContactPoint.Normal, Vector3.Normalize(Physics.HavokWorld.Gravity));
			if (!(num <= 0f))
			{
				return DamageImpactEnum.NoDamage;
			}
			if (Math.Abs(value.SeparatingVelocity * num) < MyPerGameSettings.CharacterDamageMinVelocity)
			{
				return DamageImpactEnum.NoDamage;
			}
			if (Math.Abs(value.SeparatingVelocity * num) > MyPerGameSettings.CharacterDamageDeadlyDamageVelocity)
			{
				return DamageImpactEnum.DeadlyDamage;
			}
			if (Math.Abs(value.SeparatingVelocity * num) > MyPerGameSettings.CharacterDamageMediumDamageVelocity)
			{
				return DamageImpactEnum.MediumDamage;
			}
			return DamageImpactEnum.SmallDamage;
		}

		private void InitWeapon(MyObjectBuilder_EntityBase weapon)
		{
			if (weapon != null)
			{
				if ((m_rightHandItemBone == -1 || weapon != null) && m_currentWeapon != null)
				{
					DisposeWeapon();
				}
				MyPhysicalItemDefinition physicalItemForHandItem = MyDefinitionManager.Static.GetPhysicalItemForHandItem(weapon.GetId());
				bool flag = physicalItemForHandItem != null && (!MySession.Static.SurvivalMode || this.GetInventory().GetItemAmount(physicalItemForHandItem.Id) > 0);
				if (m_rightHandItemBone != -1 && flag)
				{
					m_currentWeapon = CreateGun(weapon);
					((MyEntity)m_currentWeapon).Render.DrawInAllCascades = true;
				}
			}
		}

		private void ValidateBonesProperties()
		{
			if (m_rightHandItemBone == -1 && m_currentWeapon != null)
			{
				DisposeWeapon();
			}
		}

		private void DisposeWeapon()
		{
			MyEntity myEntity = m_currentWeapon as MyEntity;
			if (myEntity != null)
			{
				myEntity.EntityId = 0L;
				myEntity.Close();
				m_currentWeapon = null;
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_Character myObjectBuilder_Character = (MyObjectBuilder_Character)base.GetObjectBuilder(copy);
			myObjectBuilder_Character.CharacterModel = m_characterModel;
			myObjectBuilder_Character.ColorMaskHSV = ColorMask;
			if (this.GetInventory() != null && !MyFakes.ENABLE_MEDIEVAL_INVENTORY)
			{
				myObjectBuilder_Character.Inventory = this.GetInventory().GetObjectBuilder();
			}
			else
			{
				myObjectBuilder_Character.Inventory = null;
			}
			if (m_currentWeapon != null)
			{
				myObjectBuilder_Character.HandWeapon = ((MyEntity)m_currentWeapon).GetObjectBuilder();
			}
			myObjectBuilder_Character.Battery = m_suitBattery.GetObjectBuilder();
			myObjectBuilder_Character.LightEnabled = m_lightEnabled;
			if (IsOnLadder)
			{
				myObjectBuilder_Character.UsingLadder = m_ladder.EntityId;
				myObjectBuilder_Character.UsingLadderInfo = new MyObjectBuilder_Character.LadderInfo
				{
					BaseMatrix = new MyPositionAndOrientation(ref m_baseMatrix),
					IncrementToBase = m_ladderIncrementToBase
				};
			}
			else
			{
				myObjectBuilder_Character.UsingLadder = null;
				myObjectBuilder_Character.UsingLadderInfo = null;
			}
			myObjectBuilder_Character.HeadAngle = new Vector2(m_headLocalXAngle, m_headLocalYAngle);
			myObjectBuilder_Character.LinearVelocity = ((Physics != null) ? Physics.LinearVelocity : Vector3.Zero);
			myObjectBuilder_Character.Health = null;
			myObjectBuilder_Character.LootingCounter = m_currentLootingCounter;
			myObjectBuilder_Character.DisplayName = base.DisplayName;
			myObjectBuilder_Character.CharacterGeneralDamageModifier = CharacterGeneralDamageModifier;
			myObjectBuilder_Character.IsInFirstPersonView = (Sandbox.Engine.Platform.Game.IsDedicated || m_isInFirstPersonView);
			myObjectBuilder_Character.EnableBroadcasting = RadioBroadcaster.WantsToBeEnabled;
			myObjectBuilder_Character.MovementState = m_currentMovementState;
			if (base.Components != null)
			{
				if (myObjectBuilder_Character.EnabledComponents == null)
				{
					myObjectBuilder_Character.EnabledComponents = new List<string>();
				}
				foreach (MyComponentBase component in base.Components)
				{
					foreach (KeyValuePair<MyStringId, Tuple<Type, Type>> characterComponent in MyCharacterComponentTypes.CharacterComponents)
					{
						if (characterComponent.Value.Item2 == component.GetType() && !myObjectBuilder_Character.EnabledComponents.Contains(characterComponent.Key.ToString()))
						{
							myObjectBuilder_Character.EnabledComponents.Add(characterComponent.Key.ToString());
						}
					}
				}
				if (JetpackComp != null)
				{
					JetpackComp.GetObjectBuilder(myObjectBuilder_Character);
				}
				if (OxygenComponent != null)
				{
					OxygenComponent.GetObjectBuilder(myObjectBuilder_Character);
				}
			}
			myObjectBuilder_Character.PlayerSerialId = m_controlInfo.Value.SerialId;
			myObjectBuilder_Character.PlayerSteamId = m_controlInfo.Value.SteamId;
			myObjectBuilder_Character.OwningPlayerIdentityId = m_idModule.Owner;
			myObjectBuilder_Character.IsPersistenceCharacter = IsPersistenceCharacter;
			myObjectBuilder_Character.RelativeDampeningEntity = ((RelativeDampeningEntity != null) ? RelativeDampeningEntity.EntityId : 0);
			if (GetIdentity() != null && GetIdentity().BuildPlanner.Count > 0)
			{
				myObjectBuilder_Character.BuildPlanner = SaveBuildPlanner().ToList();
			}
			return myObjectBuilder_Character;
		}

		protected override void Closing()
		{
			CloseInternal();
			if (m_breath != null)
			{
				m_breath.Close();
			}
			base.Closing();
		}

		private void CloseInternal()
		{
			if (m_currentWeapon != null)
			{
				((MyEntity)m_currentWeapon).Close();
				m_currentWeapon = null;
			}
			if (m_leftHandItem != null)
			{
				m_leftHandItem.Close();
				m_leftHandItem = null;
			}
			RemoveNotifications();
			if (IsOnLadder)
			{
				CloseLadderConstraint(m_ladder.CubeGrid);
				m_ladder.IsWorkingChanged -= MyLadder_IsWorkingChanged;
			}
			RadioBroadcaster.Enabled = false;
			if (MyToolbarComponent.CharacterToolbar != null)
			{
				MyToolbarComponent.CharacterToolbar.ItemChanged -= Toolbar_ItemChanged;
			}
		}

		public void UpdatePredictionFlag()
		{
			if (Sync.IsServer || IsDead)
			{
				IsClientPredicted = true;
				return;
			}
			if (ForceDisablePrediction && MySandboxGame.Static.SimulationTime.Seconds > m_forceDisablePredictionTime + 10.0)
			{
				ForceDisablePrediction = false;
			}
			bool flag = MySession.Static.TopMostControlledEntity == this;
			bool flag2 = MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER && flag && !IsDead && (!JetpackRunning || MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER_IN_JETPACK) && !ForceDisablePrediction && !AlwaysDisablePrediction;
			if (ControllerInfo.IsLocallyControlled())
			{
				HkShape shape = HkShape.Empty;
				MyCharacterProxy characterProxy = Physics.CharacterProxy;
				if (characterProxy != null)
				{
					shape = characterProxy.GetCollisionShape();
				}
				if (!shape.IsZero)
				{
					using (MyUtils.ReuseCollection(ref m_physicsCollisionResults))
					{
						MatrixD matrix = Physics.GetWorldMatrix();
						matrix.Translation += Vector3D.TransformNormal(Physics.Center, ref matrix);
						Vector3D translation = matrix.Translation;
						Quaternion rotation = Quaternion.CreateFromRotationMatrix(matrix);
						MyPhysics.GetPenetrationsShape(shape, ref translation, ref rotation, m_physicsCollisionResults, 30);
						foreach (HkBodyCollision physicsCollisionResult in m_physicsCollisionResults)
						{
							MyGridPhysics myGridPhysics = physicsCollisionResult.Body.UserObject as MyGridPhysics;
							if (myGridPhysics != null)
							{
								if (IsOnLadder)
								{
									MySlimBlock blockFromShapeKey = myGridPhysics.Shape.GetBlockFromShapeKey(physicsCollisionResult.ShapeKey);
									if (blockFromShapeKey != null && blockFromShapeKey.FatBlock is MyLadder && !ShouldCollideWith(blockFromShapeKey.FatBlock as MyLadder))
									{
										ForceDisablePrediction = false;
										flag2 = true;
										continue;
									}
								}
								ForceDisablePrediction = true;
								flag2 = false;
								break;
							}
						}
					}
				}
			}
			if (IsClientPredicted != flag2)
			{
				IsClientPredicted = flag2;
				UpdateCharacterPhysics();
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (MySession.Static == null)
			{
				return;
			}
			base.AnimationController.UpdateTransformations();
			UpdatePredictionFlag();
			m_previousMovementFlags = m_movementFlags;
			m_previousNetworkMovementState = GetCurrentMovementState();
			UpdateZeroMovement();
			m_moveAndRotateCalled = false;
			m_actualUpdateFrame++;
			m_isInFirstPerson = (MySession.Static.CameraController == this && IsInFirstPersonView);
			bool flag = ControllerInfo.IsLocallyControlled() && MySession.Static.CameraController == this;
			bool flag2 = (m_isInFirstPerson || ForceFirstPersonCamera) && flag;
			if (m_wasInFirstPerson != flag2 && m_currentMovementState != MyCharacterMovementEnum.Sitting)
			{
				MySector.MainCamera.Zoom.ApplyToFov = flag2;
				UpdateNearFlag();
			}
			m_wasInFirstPerson = flag2;
			UpdateLightPower();
			m_currentAnimationChangeDelay += 0.0166666675f;
			if (Sync.IsServer && !IsDead && !MyEntities.IsInsideWorld(base.PositionComp.GetPosition()) && MySession.Static.SurvivalMode)
			{
				DoDamage(1000f, MyDamageType.Suicide, updateSync: true, base.EntityId);
			}
			foreach (MyComponentBase component in base.Components)
			{
				MyCharacterComponent myCharacterComponent = component as MyCharacterComponent;
				if (myCharacterComponent != null && myCharacterComponent.NeedsUpdateBeforeSimulation)
				{
					myCharacterComponent.UpdateBeforeSimulation();
				}
			}
			if (m_canPlayImpact > 0f)
			{
				m_canPlayImpact -= 0.0166666675f;
			}
			if (ReverbDetectorComp != null && this == MySession.Static.LocalCharacter)
			{
				ReverbDetectorComp.Update();
			}
			if (m_resolveHighlightOverlap)
			{
				if (ControllerInfo.IsLocallyControlled() && !(base.Parent is MyCockpit))
				{
					MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
					if (highlightSystem != null)
					{
						Render.RenderObjectIDs.ForEach(delegate(uint id)
						{
							highlightSystem.AddHighlightOverlappingModel(id);
						});
					}
				}
				m_resolveHighlightOverlap = false;
			}
			if (!MySessionComponentReplay.Static.IsEntityBeingReplayed(base.EntityId, out PerFrameData perFrameData))
			{
				return;
			}
			if (perFrameData.SwitchWeaponData.HasValue)
			{
				SwitchToWeaponInternal(perFrameData.SwitchWeaponData.Value.WeaponDefinition, updateSync: false, perFrameData.SwitchWeaponData.Value.InventoryItemId, perFrameData.SwitchWeaponData.Value.WeaponEntityId);
			}
			if (perFrameData.ShootData.HasValue)
			{
				if (perFrameData.ShootData.Value.Begin)
				{
					BeginShoot((MyShootActionEnum)perFrameData.ShootData.Value.ShootAction);
				}
				else
				{
					EndShoot((MyShootActionEnum)perFrameData.ShootData.Value.ShootAction);
				}
			}
			if (perFrameData.AnimationData.HasValue)
			{
				TriggerCharacterAnimationEvent(perFrameData.AnimationData.Value.Animation, sync: false);
				if (!string.IsNullOrEmpty(perFrameData.AnimationData.Value.Animation2))
				{
					TriggerCharacterAnimationEvent(perFrameData.AnimationData.Value.Animation2, sync: false);
				}
			}
			if (perFrameData.ControlSwitchesData.HasValue)
			{
				if (perFrameData.ControlSwitchesData.Value.SwitchDamping)
				{
					((VRage.Game.ModAPI.Interfaces.IMyControllableEntity)this).SwitchDamping();
				}
				if (perFrameData.ControlSwitchesData.Value.SwitchHelmet)
				{
					((VRage.Game.ModAPI.Interfaces.IMyControllableEntity)this).SwitchHelmet();
				}
				if (perFrameData.ControlSwitchesData.Value.SwitchLandingGears)
				{
					((VRage.Game.ModAPI.Interfaces.IMyControllableEntity)this).SwitchLandingGears();
				}
				if (perFrameData.ControlSwitchesData.Value.SwitchLights)
				{
					((VRage.Game.ModAPI.Interfaces.IMyControllableEntity)this).SwitchLights();
				}
				if (perFrameData.ControlSwitchesData.Value.SwitchReactors)
				{
					((VRage.Game.ModAPI.Interfaces.IMyControllableEntity)this).SwitchReactors();
				}
				if (perFrameData.ControlSwitchesData.Value.SwitchThrusts)
				{
					((VRage.Game.ModAPI.Interfaces.IMyControllableEntity)this).SwitchThrusts();
				}
			}
			if (perFrameData.UseData.HasValue)
			{
				if (perFrameData.UseData.Value.Use)
				{
					Use();
				}
				else if (perFrameData.UseData.Value.UseContinues)
				{
					UseContinues();
				}
				else if (perFrameData.UseData.Value.UseFinished)
				{
					UseFinished();
				}
			}
		}

		public override void Simulate()
		{
			base.Simulate();
			if (m_cachedCommands != null)
			{
				if (IsUsing != null && !IsOnLadder)
				{
					m_cachedCommands.Clear();
				}
				foreach (IMyNetworkCommand cachedCommand in m_cachedCommands)
				{
					if (cachedCommand.ExecuteBeforeMoveAndRotate)
					{
						cachedCommand.Apply();
					}
				}
			}
			if (ControllerInfo.IsLocallyControlled() || ((IsUsing == null || IsOnLadder) && m_cachedCommands != null && m_cachedCommands.Count == 0) || (base.Parent == null && MySessionComponentReplay.Static.IsEntityBeingReplayed(base.EntityId)))
			{
				MoveAndRotateInternal(MoveIndicator, RotationIndicator, RollIndicator, RotationCenterIndicator);
			}
			if (m_cachedCommands != null)
			{
				if ((IsUsing != null && !IsOnLadder) || IsDead)
				{
					m_cachedCommands.Clear();
				}
				foreach (IMyNetworkCommand cachedCommand2 in m_cachedCommands)
				{
					if (!cachedCommand2.ExecuteBeforeMoveAndRotate)
					{
						cachedCommand2.Apply();
					}
				}
				m_cachedCommands.Clear();
			}
			foreach (MyComponentBase component in base.Components)
			{
				MyCharacterComponent myCharacterComponent = component as MyCharacterComponent;
				if (myCharacterComponent != null && myCharacterComponent.NeedsUpdateSimulation)
				{
					myCharacterComponent.Simulate();
				}
			}
			if (IsDead || m_currentMovementState == MyCharacterMovementEnum.Sitting || MySandboxGame.IsPaused || Physics.CharacterProxy == null)
			{
				return;
			}
			Vector3 linearVelocity = Physics.LinearVelocity;
			Vector3 angularVelocity = Physics.AngularVelocity;
			if (!JetpackRunning)
			{
				bool supported = Physics.CharacterProxy.Supported;
				Physics.CharacterProxy.GetSupportingEntities(m_supportedEntitiesTmp);
				Physics.CharacterProxy.StepSimulation(0.0166666675f);
				bool supported2 = Physics.CharacterProxy.Supported;
				if (!Sync.IsServer && !supported2 && supported && m_supportedEntitiesTmp.Count > 0 && m_supportedEntitiesTmp[0].Physics.RigidBody != null)
				{
					Vector3D worldPos = base.WorldMatrix.Translation;
					m_supportedEntitiesTmp[0].Physics.GetVelocityAtPointLocal(ref worldPos, out Vector3 linearVelocity2);
					Vector3 value = Physics.LinearVelocity - Physics.LinearVelocityLocal;
					Physics.LinearVelocity = Physics.LinearVelocityLocal + linearVelocity2 - value;
				}
				m_supportedEntitiesTmp.Clear();
			}
			else
			{
				Physics.CharacterProxy.UpdateSupport(0.0166666675f);
				Physics.CharacterProxy.ApplyGravity(Physics.Gravity);
				Physics.CharacterProxy.AngularVelocity = Vector3.Zero;
			}
			if (!Sync.IsServer && !IsClientPredicted)
			{
				Physics.LinearVelocity = linearVelocity;
				Physics.AngularVelocity = angularVelocity;
			}
		}

		public void UpdateLightPower(bool chargeImmediately = false)
		{
			float currentLightPower = m_currentLightPower;
			if (m_lightPowerFromProducer > 0f && m_lightEnabled)
			{
				if (chargeImmediately)
				{
					m_currentLightPower = 1f;
				}
				else
				{
					m_currentLightPower = MathHelper.Clamp(m_currentLightPower + m_lightTurningOnSpeed, 0f, 1f);
				}
			}
			else
			{
				m_currentLightPower = (chargeImmediately ? 0f : MathHelper.Clamp(m_currentLightPower - m_lightTurningOffSpeed, 0f, 1f));
			}
			if (Render != null)
			{
				Render.UpdateLight(m_currentLightPower, currentLightPower != m_currentLightPower, LIGHT_PARAMETERS_CHANGED);
			}
			if (RadioBroadcaster != null)
			{
				if (RadioBroadcaster.WantsToBeEnabled && m_suitBattery != null)
				{
					RadioBroadcaster.Enabled = (m_suitBattery.ResourceSource.CurrentOutput > 0f);
				}
				else
				{
					RadioBroadcaster.Enabled = false;
				}
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			SuitRechargeDistributor.UpdateBeforeSimulation();
			RadioReceiver.UpdateBroadcastersInRange();
			if (this == MySession.Static.LocalCharacter)
			{
				RadioReceiver.UpdateHud();
			}
		}

		public bool HasAccessToLogicalGroup(MyGridLogicalGroupData group)
		{
			return RadioReceiver.HasAccessToLogicalGroup(group);
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			m_suitBattery.UpdateOnServer100();
			if (Sync.IsServer && !m_suitBattery.ResourceSource.HasCapacityRemaining)
			{
				MyTemperatureLevel myTemperatureLevel = MySectorWeatherComponent.TemperatureToLevel(GetOutsideTemperature());
				float num = 0f;
				switch (myTemperatureLevel)
				{
				case MyTemperatureLevel.ExtremeFreeze:
				case MyTemperatureLevel.ExtremeHot:
					num = 5f;
					break;
				case MyTemperatureLevel.Freeze:
				case MyTemperatureLevel.Hot:
					num = 2f;
					break;
				}
				if (num > 0f)
				{
					DoDamage(num, MyDamageType.Temperature, updateSync: true, 0L);
				}
			}
			foreach (MyComponentBase component in base.Components)
			{
				MyCharacterComponent myCharacterComponent = component as MyCharacterComponent;
				if (myCharacterComponent != null && myCharacterComponent.NeedsUpdateBeforeSimulation100)
				{
					myCharacterComponent.UpdateBeforeSimulation100();
				}
			}
			if (AtmosphereDetectorComp != null)
			{
				AtmosphereDetectorComp.UpdateAtmosphereStatus();
			}
			if (m_relativeDampeningEntityInit != 0L && JetpackComp != null && !JetpackComp.DampenersTurnedOn)
			{
				m_relativeDampeningEntityInit = 0L;
			}
			if (RelativeDampeningEntity == null && m_relativeDampeningEntityInit != 0L)
			{
				RelativeDampeningEntity = MyEntities.GetEntityByIdOrDefault(m_relativeDampeningEntityInit);
				if (RelativeDampeningEntity != null)
				{
					m_relativeDampeningEntityInit = 0L;
				}
			}
			if (RelativeDampeningEntity != null)
			{
				MyEntityThrustComponent.UpdateRelativeDampeningEntity(this, RelativeDampeningEntity);
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			foreach (MyComponentBase component in base.Components)
			{
				MyCharacterComponent myCharacterComponent = component as MyCharacterComponent;
				if (myCharacterComponent != null && myCharacterComponent.NeedsUpdateAfterSimulation10)
				{
					myCharacterComponent.UpdateAfterSimulation10();
				}
			}
			UpdateCameraDistance();
			if (Sync.IsServer)
			{
				UpdateBootsStateAndEmmisivity();
			}
		}

		private void UpdateBootsStateAndEmmisivity()
		{
			if (IsMagneticBootsEnabled && !IsDead && !IsSitting && Physics.CharacterProxy.Supported)
			{
				m_bootsState.Value = MyBootsState.Enabled;
			}
			else if ((JetpackRunning || IsFalling || IsJumping) && Physics.CharacterProxy != null && Physics.CharacterProxy.Supported && m_gravity.LengthSquared() < 0.001f)
			{
				m_bootsState.Value = MyBootsState.Proximity;
			}
			else
			{
				m_bootsState.Value = MyBootsState.Disabled;
			}
		}

		private void OnBootsStateChanged(SyncBase obj)
		{
			if (!Sync.IsDedicated && SoundComp != null && Render != null)
			{
				switch (m_bootsState.Value)
				{
				case MyBootsState.Enabled:
					MyRenderProxy.UpdateColorEmissivity(Render.RenderObjectIDs[0], 0, "Emissive", Color.ForestGreen, 1f);
					SoundComp.PlayMagneticBootsStart();
					break;
				case MyBootsState.Proximity:
					MyRenderProxy.UpdateColorEmissivity(Render.RenderObjectIDs[0], 0, "Emissive", Color.Yellow, 1f);
					SoundComp.PlayMagneticBootsProximity();
					break;
				case MyBootsState.Disabled:
					MyRenderProxy.UpdateColorEmissivity(Render.RenderObjectIDs[0], 0, "Emissive", Color.White, 0f);
					SoundComp.PlayMagneticBootsEnd();
					break;
				}
			}
			m_movementsFlagsChanged = true;
		}

		private void UpdateCameraDistance()
		{
			m_cameraDistance = (float)Vector3D.Distance(MySector.MainCamera.Position, base.WorldMatrix.Translation);
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			MyHud.Crosshair.Recenter();
			if (m_currentWeapon != null)
			{
				m_currentWeapon.DrawHud(camera, playerId);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			LIGHT_PARAMETERS_CHANGED = false;
			UpdateLadder();
			if (!IsDead && StatComp != null)
			{
				StatComp.Update();
			}
			UpdateDying();
			if ((!Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.DisableAnimationsOnDS) && !IsDead)
			{
				UpdateShake();
			}
			if (IsDead || (!Sync.IsServer && !IsClientPredicted && MySession.Static.TopMostControlledEntity == this))
			{
				UpdatePhysicalMovement();
			}
			if (!IsDead)
			{
				UpdateFallAndSpine();
			}
			if (JetpackRunning)
			{
				JetpackComp.ClearMovement();
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.DisableAnimationsOnDS)
			{
				MyCharacterRagdollComponent myCharacterRagdollComponent = base.Components.Get<MyCharacterRagdollComponent>();
				if (myCharacterRagdollComponent != null)
				{
					myCharacterRagdollComponent.Distance = m_cameraDistance;
				}
				Render.UpdateLightPosition();
				UpdateBobQueue();
			}
			else if (m_currentWeapon != null && WeaponPosition != null)
			{
				WeaponPosition.Update();
			}
			UpdateCharacterStateChange();
			UpdateRespawnAndLooting();
			UpdateShooting();
			foreach (MyComponentBase component in base.Components)
			{
				MyCharacterComponent myCharacterComponent = component as MyCharacterComponent;
				if (myCharacterComponent != null && myCharacterComponent.NeedsUpdateAfterSimulation)
				{
					myCharacterComponent.UpdateAfterSimulation();
				}
			}
			m_characterBoneCapsulesReady = false;
			if (Physics != null)
			{
				m_previousLinearVelocity = Physics.LinearVelocity;
			}
			m_previousPosition = base.WorldMatrix.Translation;
			if (Physics != null && Physics.CharacterProxy == null)
			{
				Render.UpdateWalkParticles();
			}
			SoundComp.FindAndPlayStateSound();
			SoundComp.UpdateWindSounds();
		}

		private void UpdateCharacterStateChange()
		{
			if (!IsDead && Physics.CharacterProxy != null)
			{
				OnCharacterStateChanged(Physics.CharacterProxy.GetState());
			}
		}

		private void UpdateRespawnAndLooting()
		{
			if (m_currentRespawnCounter > 0f)
			{
				MyPlayer myPlayer = TryGetPlayer();
				if (myPlayer != null && !MySessionComponentMissionTriggers.CanRespawn(myPlayer.Id))
				{
					if (m_respawnNotification != null)
					{
						m_respawnNotification.m_lifespanMs = 0;
					}
					m_currentRespawnCounter = -1f;
				}
				m_currentRespawnCounter -= 0.0166666675f;
				if (m_respawnNotification != null)
				{
					m_respawnNotification.SetTextFormatArguments((int)m_currentRespawnCounter);
				}
				if (m_currentRespawnCounter <= 0f && Sync.IsServer && myPlayer != null)
				{
					Sync.Players.KillPlayer(myPlayer);
				}
			}
			UpdateLooting(0.0166666675f);
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			UpdateAssetModifiers();
			SoundComp.UpdateAfterSimulation100();
			UpdateOutsideTemperature();
		}

		private bool UpdateLooting(float amount)
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
			{
				MyRenderProxy.DebugDrawText3D(base.WorldMatrix.Translation, m_currentLootingCounter.ToString("n1"), Color.Green, 1f, depthRead: false);
			}
			if (m_currentLootingCounter > 0f)
			{
				m_currentLootingCounter -= amount;
				if (m_currentLootingCounter <= 0f && Sync.IsServer)
				{
					Close();
					base.Save = false;
					return true;
				}
			}
			return false;
		}

		private void UpdateBobQueue()
		{
			int num = IsInFirstPersonView ? m_headBoneIndex : m_camera3rdBoneIndex;
			if (num != -1)
			{
				m_bobQueue.Enqueue(base.BoneAbsoluteTransforms[num].Translation);
				int num2 = (m_currentMovementState == MyCharacterMovementEnum.Standing || m_currentMovementState == MyCharacterMovementEnum.Sitting || m_currentMovementState == MyCharacterMovementEnum.Crouching || m_currentMovementState == MyCharacterMovementEnum.RotatingLeft || m_currentMovementState == MyCharacterMovementEnum.RotatingRight || m_currentMovementState == MyCharacterMovementEnum.Died) ? 5 : 20;
				if (WantsCrouch)
				{
					num2 = 3;
				}
				while (m_bobQueue.Count > num2)
				{
					m_bobQueue.Dequeue();
				}
			}
		}

		private void UpdateFallAndSpine()
		{
			MyCharacterJetpackComponent jetpackComp = JetpackComp;
			jetpackComp?.UpdateFall();
			if (m_isFalling && !JetpackRunning)
			{
				m_currentFallingTime += 0.0166666675f;
				if (m_currentFallingTime > 0.3f && !m_isFallingAnimationPlayed)
				{
					SwitchAnimation(MyCharacterMovementEnum.Falling, checkState: false);
					m_isFallingAnimationPlayed = true;
				}
			}
			if ((!JetpackRunning || (jetpackComp.Running && (IsLocalHeadAnimationInProgress() || Definition.VerticalPositionFlyingOnly))) && !IsDead && !IsSitting && !IsOnLadder)
			{
				float num = IsInFirstPersonView ? m_characterDefinition.BendMultiplier1st : m_characterDefinition.BendMultiplier3rd;
				if (UseNewAnimationSystem)
				{
					float num2 = MathHelper.Clamp(0f - m_headLocalXAngle, -89.9f, 89f);
					float value = m_characterDefinition.BendMultiplier3rd * num2;
					if (MySession.Static.LocalCharacter == this && (!MyControllerHelper.IsControl(MyControllerHelper.CX_CHARACTER, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED) || IsInFirstPersonView || ForceFirstPersonCamera || CurrentWeapon != null))
					{
						m_animLeaning.Value = value;
					}
				}
				else
				{
					float num3 = MathHelper.Clamp(0f - m_headLocalXAngle, -45f, 89f);
					Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.ToRadians(num * num3));
					Quaternion rotationForClients = Quaternion.CreateFromAxisAngle(Vector3.Backward, MathHelper.ToRadians(m_characterDefinition.BendMultiplier3rd * num3));
					SetSpineAdditionalRotation(rotation, rotationForClients);
				}
			}
			else if (UseNewAnimationSystem)
			{
				m_animLeaning.Value = 0f;
			}
			else
			{
				SetSpineAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Backward, 0f), Quaternion.CreateFromAxisAngle(Vector3.Backward, 0f));
			}
			if (m_currentWeapon == null && !IsDead && !JetpackRunning && !IsSitting)
			{
				_ = m_headLocalXAngle;
				_ = -11f;
				_ = m_headLocalXAngle;
				_ = -11f;
			}
			else
			{
				SetHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0f)));
				SetUpperHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0f)));
			}
		}

		private void UpdateShooting()
		{
			if (m_currentWeapon != null)
			{
				if (MySession.Static.LocalCharacter == this && !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay) && MyScreenManager.IsAnyScreenOpening() && (MyInput.Static.IsGameControlPressed(MyControlsSpace.PRIMARY_TOOL_ACTION) || MyInput.Static.IsGameControlPressed(MyControlsSpace.SECONDARY_TOOL_ACTION)))
				{
					EndShootAll();
				}
				if (m_currentWeapon.IsShooting)
				{
					m_currentShootPositionTime = 0.1f;
				}
				ShootInternal();
			}
			else if (m_usingByPrimary)
			{
				if (!MyControllerHelper.IsControl(MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED))
				{
					m_usingByPrimary = false;
				}
				UseContinues();
			}
			if (m_currentShotTime > 0f)
			{
				m_currentShotTime -= 0.0166666675f;
				if (m_currentShotTime <= 0f)
				{
					m_currentShotTime = 0f;
				}
			}
			if (m_currentShootPositionTime > 0f)
			{
				m_currentShootPositionTime -= 0.0166666675f;
				if (m_currentShootPositionTime <= 0f)
				{
					m_currentShootPositionTime = 0f;
				}
			}
		}

		internal void UpdatePhysicalMovement()
		{
			if (!MySandboxGame.IsGameReady || Physics == null || !Physics.Enabled || !MySession.Static.Ready || Physics.HavokWorld == null)
			{
				return;
			}
			MyCharacterJetpackComponent jetpackComp = JetpackComp;
			bool flag = jetpackComp?.UpdatePhysicalMovement() ?? false;
			bool flag2 = MyGravityProviderSystem.IsGravityReady();
			m_gravity = MyGravityProviderSystem.CalculateTotalGravityInPoint(base.PositionComp.WorldAABB.Center) + Physics.HavokWorld.Gravity;
			if (m_gravity.Length() > 100f)
			{
				m_gravity.Normalize();
				m_gravity *= 100f;
			}
			MatrixD worldMatrix = base.WorldMatrix;
			bool flag3 = false;
			bool flag4 = true;
			if ((!flag || Definition.VerticalPositionFlyingOnly || IsMagneticBootsEnabled) && !IsDead && !IsOnLadder)
			{
				Vector3 chUp = worldMatrix.Up;
				Vector3 chForward = worldMatrix.Forward;
				if (Physics.CharacterProxy != null)
				{
					if (!Physics.CharacterProxy.Up.IsValid() || !Physics.CharacterProxy.Forward.IsValid())
					{
						Physics.CharacterProxy.SetForwardAndUp(worldMatrix.Forward, worldMatrix.Up);
					}
					chUp = Physics.CharacterProxy.Up;
					chForward = Physics.CharacterProxy.Forward;
					if (!flag)
					{
						Physics.CharacterProxy.Gravity = m_gravity * MyPerGameSettings.CharacterGravityMultiplier;
					}
					else
					{
						Physics.CharacterProxy.Gravity = Vector3.Zero;
					}
				}
				if (m_gravity.LengthSquared() > 0.1f && chUp != Vector3.Zero && m_gravity.IsValid())
				{
					UpdateStandup(ref m_gravity, ref chUp, ref chForward);
					if (jetpackComp != null)
					{
						jetpackComp.CurrentAutoEnableDelay = 0f;
					}
				}
				else
				{
					if (IsMagneticBootsEnabled)
					{
						Vector3 gravity = -Physics.CharacterProxy.SupportNormal;
						UpdateStandup(ref gravity, ref chUp, ref chForward);
						if (!IsMagneticBootsActive && Sync.IsServer)
						{
							UpdateBootsStateAndEmmisivity();
						}
					}
					else if (!IsJumping && !IsFalling && !JetpackRunning && Physics.CharacterProxy == null)
					{
						MatrixD worldMatrix2 = Physics.GetWorldMatrix();
						MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(worldMatrix2.Translation + worldMatrix2.Up, worldMatrix2.Translation + worldMatrix2.Down * 0.5, 30);
						if (hitInfo.HasValue)
						{
							Vector3 gravity2 = -hitInfo.Value.HkHitInfo.Normal;
							UpdateStandup(ref gravity2, ref chUp, ref chForward);
						}
					}
					if (jetpackComp != null && jetpackComp.CurrentAutoEnableDelay != -1f && !IsMagneticBootsActive && flag2)
					{
						jetpackComp.CurrentAutoEnableDelay += 0.0166666675f;
					}
				}
				if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.SetForwardAndUp(chForward, chUp);
				}
				else
				{
					flag4 = false;
					worldMatrix = MatrixD.CreateWorld(worldMatrix.Translation, chForward, chUp);
				}
			}
			else if (IsDead)
			{
				if (Physics.HasRigidBody && Physics.RigidBody.IsActive)
				{
					Vector3 gravity3 = m_gravity;
					if (Sync.IsDedicated && MyFakes.ENABLE_RAGDOLL && !MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC)
					{
						gravity3 = Vector3.Zero;
					}
					Physics.RigidBody.Gravity = gravity3;
				}
			}
			else if (IsOnLadder && Physics.CharacterProxy != null)
			{
				Physics.CharacterProxy.Gravity = Vector3.Zero;
				MatrixD matrixD = m_baseMatrix * m_ladder.WorldMatrix;
				Physics.CharacterProxy.SetForwardAndUp(matrixD.Forward, matrixD.Up);
			}
			if (flag4)
			{
				worldMatrix = Physics.GetWorldMatrix();
			}
			if (m_currentMovementState == MyCharacterMovementEnum.Standing)
			{
				m_cummulativeVerticalFootError += m_verticalFootError * 0.2f;
				m_cummulativeVerticalFootError = MathHelper.Clamp(m_cummulativeVerticalFootError, -0.75f, 0.75f);
			}
			else
			{
				m_cummulativeVerticalFootError = 0f;
			}
			worldMatrix.Translation += worldMatrix.Up * m_cummulativeVerticalFootError;
			Vector3D value = worldMatrix.Translation - base.WorldMatrix.Translation;
			if (value.LengthSquared() > 9.9999997473787516E-06 || Vector3D.DistanceSquared(base.WorldMatrix.Forward, worldMatrix.Forward) > 9.9999997473787516E-06 || Vector3D.DistanceSquared(base.WorldMatrix.Up, worldMatrix.Up) > 9.9999997473787516E-06)
			{
				base.PositionComp.SetWorldMatrix(worldMatrix, (flag3 || !flag4) ? null : Physics);
			}
			else
			{
				value = Vector3D.Zero;
			}
			MyCharacterProxy characterProxy = Physics.CharacterProxy;
			if (characterProxy != null)
			{
				HkCharacterRigidBody characterRigidBody = characterProxy.CharacterRigidBody;
				if (characterRigidBody != null)
				{
					characterRigidBody.InterpolatedVelocity = value / 0.01666666753590107;
				}
			}
			if (IsClientPredicted || Sync.IsServer)
			{
				Physics.UpdateAccelerations();
			}
		}

		private void UpdateStandup(ref Vector3 gravity, ref Vector3 chUp, ref Vector3 chForward)
		{
			Vector3 vector = -Vector3.Normalize(gravity);
			Vector3 vector2 = vector;
			if (Physics != null)
			{
				Vector3 v = Physics.SupportNormal;
				if (Definition.RotationToSupport == MyEnumCharacterRotationToSupport.OneAxis)
				{
					float num = vector.Dot(ref v);
					if (!MyUtils.IsZero(num - 1f) && !MyUtils.IsZero(num + 1f))
					{
						Vector3 vector3 = vector.Cross(v);
						vector3.Normalize();
						vector2 = Vector3.Lerp(v, vector, Math.Abs(vector3.Dot(base.WorldMatrix.Forward)));
					}
				}
				else if (Definition.RotationToSupport == MyEnumCharacterRotationToSupport.Full)
				{
					vector2 = v;
				}
			}
			float num2 = Vector3.Dot(chUp, vector2);
			float num3 = chUp.Length() * vector2.Length();
			float num4 = num2 / num3;
			if (float.IsNaN(num4) || float.IsNegativeInfinity(num4) || float.IsPositiveInfinity(num4))
			{
				num4 = 1f;
			}
			num4 = MathHelper.Clamp(num4, -1f, 1f);
			if (!MyUtils.IsZero(num4 - 1f, 1E-08f))
			{
				float num5 = 0f;
				num5 = ((!MyUtils.IsZero(num4 + 1f, 1E-08f)) ? ((float)Math.Acos(num4)) : 0.1f);
				num5 = Math.Min(Math.Abs(num5), 0.04f) * (float)Math.Sign(num5);
				Vector3 value = Vector3.Cross(chUp, vector2);
				if (value.LengthSquared() > 0f)
				{
					value = Vector3.Normalize(value);
					chUp = Vector3.TransformNormal(chUp, Matrix.CreateFromAxisAngle(value, num5));
					chForward = Vector3.TransformNormal(chForward, Matrix.CreateFromAxisAngle(value, num5));
				}
			}
		}

		private void UpdateShake()
		{
			if (MySession.Static.LocalHumanPlayer == null || this != MySession.Static.LocalHumanPlayer.Identity.Character)
			{
				return;
			}
			if (m_currentMovementState == MyCharacterMovementEnum.Standing || m_currentMovementState == MyCharacterMovementEnum.Crouching || m_currentMovementState == MyCharacterMovementEnum.Flying)
			{
				m_currentHeadAnimationCounter += 0.0166666675f;
			}
			else
			{
				m_currentHeadAnimationCounter = 0f;
			}
			if (m_currentLocalHeadAnimation >= 0f)
			{
				m_currentLocalHeadAnimation += 0.0166666675f;
				float amount = m_currentLocalHeadAnimation / m_localHeadAnimationLength;
				if (m_currentLocalHeadAnimation > m_localHeadAnimationLength)
				{
					m_currentLocalHeadAnimation = -1f;
					amount = 1f;
				}
				if (m_localHeadAnimationX.HasValue)
				{
					SetHeadLocalXAngle(MathHelper.Lerp(m_localHeadAnimationX.Value.X, m_localHeadAnimationX.Value.Y, amount));
				}
				if (m_localHeadAnimationY.HasValue)
				{
					SetHeadLocalYAngle(MathHelper.Lerp(m_localHeadAnimationY.Value.X, m_localHeadAnimationY.Value.Y, amount));
				}
			}
		}

		public void UpdateZeroMovement()
		{
			if (ControllerInfo.IsLocallyControlled() && !m_moveAndRotateCalled)
			{
				MoveAndRotate(Vector3.Zero, Vector2.Zero, 0f);
			}
		}

		private void UpdateDying()
		{
			if (m_dieAfterSimulation)
			{
				m_bootsState.ValueChanged -= OnBootsStateChanged;
				DieInternal();
				m_dieAfterSimulation = false;
			}
		}

		internal void SetHeadLocalXAngle(float angle)
		{
			HeadLocalXAngle = angle;
		}

		private void SetHeadLocalYAngle(float angle)
		{
			HeadLocalYAngle = angle;
		}

		private bool ShouldUseAnimatedHeadRotation()
		{
			return false;
		}

		private Vector3D GetAimedPointFromHead()
		{
			MatrixD headMatrix = GetHeadMatrix(includeY: false);
			return headMatrix.Translation + headMatrix.Forward * 10.0;
		}

		private Vector3D GetAimedPointFromCamera()
		{
			if (!TargetFromCamera)
			{
				return GetAimedPointFromHead();
			}
			MatrixD matrix = GetViewMatrix();
			MatrixD.Invert(ref matrix, out MatrixD result);
			Vector3D forward = result.Forward;
			forward.Normalize();
			Vector3D translation = result.Translation;
			Vector3D translation2 = GetHeadMatrix(includeY: false, includeX: false).Translation;
			translation += forward * (translation2 - translation).Dot(forward);
			Vector3D result2 = (WeaponPosition != null) ? (WeaponPosition.LogicalPositionWorld + forward * 25000.0) : (translation + forward * 25000.0);
			if (MySession.Static.ControlledEntity == this)
			{
				if (m_raycastList == null)
				{
					m_raycastList = new List<MyPhysics.HitInfo>();
				}
				m_raycastList.Clear();
				MyPhysics.CastRay(translation, translation + forward * 100.0, m_raycastList);
				{
					foreach (MyPhysics.HitInfo raycast in m_raycastList)
					{
						VRage.ModAPI.IMyEntity hitEntity = raycast.HkHitInfo.GetHitEntity();
						if (hitEntity != this && hitEntity != CurrentWeapon)
						{
							return raycast.Position;
						}
					}
					return result2;
				}
			}
			return result2;
		}

		public void Rotate(Vector2 rotationIndicator, float roll)
		{
			if (!IsInFirstPersonView)
			{
				RotateHead(rotationIndicator, 0.5f);
				MyThirdPersonSpectator.Static.Rotate(rotationIndicator, roll);
			}
			else
			{
				rotationIndicator.Y = 0f;
				RotateHead(rotationIndicator, 0.2f);
			}
		}

		public void RotateStopped()
		{
		}

		public void MoveAndRotateStopped()
		{
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			float num = 1f;
			switch (m_zoomMode)
			{
			case MyZoomModeEnum.Classic:
				num = ROTATION_SPEED_CLASSIC;
				break;
			case MyZoomModeEnum.IronSight:
				num = ROTATION_SPEED_IRONSIGHTS;
				break;
			}
			LastMotionIndicator = moveIndicator;
			LastRotationIndicator = new Vector3(rotationIndicator, rollIndicator);
			if (moveIndicator == Vector3.Zero && rotationIndicator == Vector2.Zero && rollIndicator == 0f)
			{
				if (!(MoveIndicator == moveIndicator) || !(rotationIndicator == RotationIndicator) || RollIndicator != rollIndicator)
				{
					MoveIndicator = Vector3.Zero;
					RotationIndicator = Vector2.Zero;
					RollIndicator = 0f;
					m_moveAndRotateStopped = true;
				}
				return;
			}
			MoveIndicator = moveIndicator;
			RotationIndicator = rotationIndicator * num;
			RollIndicator = rollIndicator * num;
			m_moveAndRotateCalled = true;
			if (this == MySession.Static.LocalCharacter && MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsAnyAltKeyPressed())
			{
				if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
				{
					RotationSpeed = Math.Min(RotationSpeed * 1.5f, 0.13f);
				}
				else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
				{
					RotationSpeed = Math.Max(RotationSpeed / 1.5f, 0.01f);
				}
			}
		}

		public void CacheMove(ref Vector3 moveIndicator, ref Quaternion rotate)
		{
			if (m_cachedCommands == null)
			{
				m_cachedCommands = new List<IMyNetworkCommand>();
			}
			m_cachedCommands.Add(new MyMoveNetCommand(this, ref moveIndicator, ref rotate));
		}

		public void CacheMoveDelta(ref Vector3D moveDeltaIndicator)
		{
			if (m_cachedCommands == null)
			{
				m_cachedCommands = new List<IMyNetworkCommand>();
			}
			m_cachedCommands.Add(new MyDeltaNetCommand(this, ref moveDeltaIndicator));
		}

		internal void MoveAndRotateInternal(Vector3 moveIndicator, Vector2 rotationIndicator, float roll, Vector3 rotationCenter)
		{
			if (Physics == null)
			{
				return;
			}
			if (Physics.CharacterProxy == null && IsDead && !JetpackRunning)
			{
				moveIndicator = Vector3.Zero;
				rotationIndicator = Vector2.Zero;
				roll = 0f;
			}
			if (MySessionComponentReplay.Static.IsEntityBeingReplayed(base.EntityId, out PerFrameData perFrameData))
			{
				if (perFrameData.MovementData.HasValue)
				{
					moveIndicator = perFrameData.MovementData.Value.MoveVector;
					rotationIndicator = new Vector2(perFrameData.MovementData.Value.RotateVector.X, perFrameData.MovementData.Value.RotateVector.Y);
					roll = perFrameData.MovementData.Value.RotateVector.Z;
					MovementFlags = (MyCharacterMovementFlags)perFrameData.MovementData.Value.MovementFlags;
				}
			}
			else if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.EntityId))
			{
				PerFrameData perFrameData2 = default(PerFrameData);
				perFrameData2.MovementData = new MovementData
				{
					MoveVector = moveIndicator,
					RotateVector = new SerializableVector3(rotationIndicator.X, rotationIndicator.Y, roll),
					MovementFlags = (byte)MovementFlags
				};
				perFrameData = perFrameData2;
				MySessionComponentReplay.Static.ProvideEntityRecordData(base.EntityId, perFrameData);
			}
			bool flag = moveIndicator.Z != 0f && WantsSprint;
			bool walk = WantsWalk || Math.Abs(moveIndicator.X) + Math.Abs(moveIndicator.Z) < 0.4f;
			bool wantsJump = WantsJump;
			bool flag2 = !JetpackRunning && ((m_currentCharacterState != HkCharacterStateType.HK_CHARACTER_IN_AIR && m_currentCharacterState != (HkCharacterStateType)5) || !(m_currentJumpTime <= 0f)) && m_currentMovementState != MyCharacterMovementEnum.Died && !IsFalling;
			bool flag3 = (JetpackRunning || (m_currentCharacterState != HkCharacterStateType.HK_CHARACTER_IN_AIR && m_currentCharacterState != (HkCharacterStateType)5) || !(m_currentJumpTime <= 0f)) && m_currentMovementState != MyCharacterMovementEnum.Died;
			bool flag4 = !m_isFalling && m_currentJumpTime <= 0f && Physics.CharacterProxy != null && Physics.CharacterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_IN_AIR;
			if (IsOnLadder)
			{
				moveIndicator = ProceedLadderMovement(moveIndicator);
			}
			float acceleration = 0f;
			_ = m_currentSpeed;
			if (JetpackRunning)
			{
				JetpackComp.MoveAndRotate(ref moveIndicator, ref rotationIndicator, roll, flag3);
			}
			else if (flag2 || m_movementsFlagsChanged || flag4)
			{
				if (moveIndicator.LengthSquared() > 0f)
				{
					moveIndicator = Vector3.Normalize(moveIndicator);
				}
				MyCharacterMovementEnum newMovementState = GetNewMovementState(ref moveIndicator, ref rotationIndicator, ref acceleration, flag, walk, flag2, m_movementsFlagsChanged);
				SwitchAnimation(newMovementState);
				m_movementsFlagsChanged = false;
				SetCurrentMovementState(newMovementState);
				if (newMovementState == MyCharacterMovementEnum.Sprinting && StatComp != null)
				{
					StatComp.ApplyModifier("Sprint");
				}
				if (!IsIdle)
				{
					m_currentWalkDelay = MathHelper.Clamp(m_currentWalkDelay - 0.0166666675f, 0f, m_currentWalkDelay);
				}
				if (flag2)
				{
					float serverRatio = 1f;
					m_currentSpeed = LimitMaxSpeed(m_currentSpeed + ((m_currentWalkDelay <= 0f) ? (acceleration * 0.0166666675f) : 0f), m_currentMovementState, serverRatio);
				}
				if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.PosX = ((m_currentMovementState != MyCharacterMovementEnum.Sprinting) ? (0f - moveIndicator.X) : 0f);
					Physics.CharacterProxy.PosY = moveIndicator.Z;
					Physics.CharacterProxy.Elevate = 0f;
				}
				if (flag2 && m_currentMovementState != MyCharacterMovementEnum.Jump)
				{
					int num = Math.Sign(m_currentSpeed);
					m_currentSpeed += (float)(-num) * m_currentDecceleration * 0.0166666675f;
					if (Math.Sign(num) != Math.Sign(m_currentSpeed))
					{
						m_currentSpeed = 0f;
					}
				}
				if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.Speed = ((m_currentMovementState != MyCharacterMovementEnum.Died) ? m_currentSpeed : 0f);
				}
				m_currentMovementDirection = moveIndicator;
				if (Physics.CharacterProxy != null && Physics.CharacterProxy.GetHitRigidBody() != null)
				{
					if (wantsJump && m_currentMovementState != MyCharacterMovementEnum.Jump)
					{
						PlayCharacterAnimation("Jump", MyBlendOption.Immediate, MyFrameOption.StayOnLastFrame, 0f, 1.3f);
						if (UseNewAnimationSystem)
						{
							TriggerCharacterAnimationEvent("jump", sync: true);
						}
						if (StatComp != null)
						{
							StatComp.DoAction("Jump");
							StatComp.ApplyModifier("Jump");
						}
						m_currentJumpTime = 0.55f;
						SetCurrentMovementState(MyCharacterMovementEnum.Jump);
						m_canJump = false;
						m_frictionBeforeJump = Physics.CharacterProxy.GetHitRigidBody().Friction;
						Physics.CharacterProxy.Jump = true;
					}
					if (m_currentJumpTime > 0f)
					{
						m_currentJumpTime -= 0.0166666675f;
						Physics.CharacterProxy.GetHitRigidBody().Friction = 0f;
					}
					if (m_currentJumpTime <= 0f && m_currentMovementState == MyCharacterMovementEnum.Jump)
					{
						Physics.CharacterProxy.GetHitRigidBody().Friction = m_frictionBeforeJump;
						if (m_currentCharacterState != 0)
						{
							StartFalling();
						}
						else
						{
							MyCharacterMovementEnum myCharacterMovementEnum = MyCharacterMovementEnum.Standing;
							if (Physics.CharacterProxy != null && (Physics.CharacterProxy.GetState() == HkCharacterStateType.HK_CHARACTER_IN_AIR || Physics.CharacterProxy.GetState() == (HkCharacterStateType)5))
							{
								StartFalling();
							}
							else if (!IsFalling)
							{
								if (moveIndicator.X != 0f || moveIndicator.Z != 0f)
								{
									if (!WantsCrouch)
									{
										if (moveIndicator.Z < 0f)
										{
											if (flag)
											{
												myCharacterMovementEnum = MyCharacterMovementEnum.Sprinting;
												PlayCharacterAnimation("Sprint", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
											}
											else
											{
												myCharacterMovementEnum = MyCharacterMovementEnum.Walking;
												PlayCharacterAnimation("Walk", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.5f);
											}
										}
										else
										{
											myCharacterMovementEnum = MyCharacterMovementEnum.BackWalking;
											PlayCharacterAnimation("WalkBack", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.5f);
										}
									}
									else if (moveIndicator.Z < 0f)
									{
										myCharacterMovementEnum = MyCharacterMovementEnum.CrouchWalking;
										PlayCharacterAnimation("CrouchWalk", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
									}
									else
									{
										myCharacterMovementEnum = MyCharacterMovementEnum.CrouchBackWalking;
										PlayCharacterAnimation("CrouchWalkBack", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
									}
								}
								else
								{
									myCharacterMovementEnum = MyCharacterMovementEnum.Standing;
									PlayCharacterAnimation("Idle", MyBlendOption.WaitForPreviousEnd, MyFrameOption.Loop, 0.2f);
								}
								if (!m_canJump)
								{
									SoundComp.PlayFallSound();
								}
								m_canJump = true;
								SetCurrentMovementState(myCharacterMovementEnum);
							}
						}
						m_currentJumpTime = 0f;
					}
				}
			}
			else if (Physics.CharacterProxy != null)
			{
				Physics.CharacterProxy.Elevate = 0f;
			}
			UpdateHeadOffset();
			if (!JetpackRunning && !IsOnLadder)
			{
				if (rotationIndicator.Y != 0f && (flag3 || m_isFalling || m_currentJumpTime > 0f))
				{
					if (Physics.CharacterProxy != null)
					{
						MatrixD matrix = MatrixD.CreateRotationY((0f - rotationIndicator.Y) * RotationSpeed * 0.02f);
						MatrixD matrix2 = MatrixD.CreateWorld(Physics.CharacterProxy.Position, Physics.CharacterProxy.Forward, Physics.CharacterProxy.Up);
						matrix2 = matrix * matrix2;
						Physics.CharacterProxy.SetForwardAndUp(matrix2.Forward, matrix2.Up);
					}
					else
					{
						MatrixD matrix3 = MatrixD.CreateRotationY((0f - rotationIndicator.Y) * RotationSpeed * 0.02f);
						MatrixD worldMatrix = base.WorldMatrix;
						worldMatrix = matrix3 * worldMatrix;
						worldMatrix.Translation = base.WorldMatrix.Translation;
						base.PositionComp.SetWorldMatrix(worldMatrix);
					}
				}
				if (rotationIndicator.X != 0f && ((m_currentMovementState == MyCharacterMovementEnum.Died && !m_isInFirstPerson) || m_currentMovementState != MyCharacterMovementEnum.Died))
				{
					SetHeadLocalXAngle(m_headLocalXAngle - rotationIndicator.X * RotationSpeed);
					int num2 = IsInFirstPersonView ? m_headBoneIndex : m_camera3rdBoneIndex;
					if (num2 != -1)
					{
						m_bobQueue.Clear();
						m_bobQueue.Enqueue(base.BoneAbsoluteTransforms[num2].Translation);
					}
				}
			}
			if (Physics.CharacterProxy != null && Physics.CharacterProxy.LinearVelocity.LengthSquared() > 0.1f)
			{
				m_shapeContactPoints.Clear();
			}
			WantsJump = false;
			WantsFlyUp = false;
			WantsFlyDown = false;
		}

		private void RotateHead(Vector2 rotationIndicator, float sensitivity)
		{
			if (rotationIndicator.X != 0f)
			{
				SetHeadLocalXAngle(m_headLocalXAngle - rotationIndicator.X * sensitivity);
			}
			if (rotationIndicator.Y != 0f)
			{
				float num = (0f - rotationIndicator.Y) * sensitivity;
				SetHeadLocalYAngle(m_headLocalYAngle + num);
			}
		}

		public bool CanPlaceCharacter(ref MatrixD worldMatrix, bool useCharacterCenter = false, bool checkCharacters = false, MyEntity ignoreEntity = null)
		{
			Vector3D translation = worldMatrix.Translation;
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(worldMatrix);
			if (Physics == null || (Physics.CharacterProxy == null && Physics.RigidBody == null))
			{
				return true;
			}
			m_penetrationList.Clear();
			if (!useCharacterCenter)
			{
				Vector3D vector3D = Vector3D.TransformNormal(Physics.Center, worldMatrix);
				translation += vector3D;
			}
			m_penetrationList.Clear();
			MyPhysics.GetPenetrationsShape((Physics.CharacterProxy != null) ? Physics.CharacterProxy.GetCollisionShape() : Physics.RigidBody.GetShape(), ref translation, ref rotation, m_penetrationList, 18);
			bool flag = false;
			foreach (HkBodyCollision penetration in m_penetrationList)
			{
				VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
				if (ignoreEntity != collisionEntity)
				{
					if (collisionEntity != null)
					{
						if (collisionEntity.Physics == null)
						{
							MyLog.Default.WriteLine("CanPlaceCharacter found Entity with no physics: " + collisionEntity);
						}
						else if (!collisionEntity.Physics.IsPhantom)
						{
							flag = true;
							break;
						}
					}
					else if (checkCharacters)
					{
						flag = true;
						break;
					}
				}
			}
			if (MySession.Static.VoxelMaps == null)
			{
				return true;
			}
			if (!flag)
			{
				BoundingSphereD sphere = new BoundingSphereD(worldMatrix.Translation, 0.75);
				flag = (MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere) != null);
			}
			return !flag;
		}

		public MyCharacterMovementEnum GetCurrentMovementState()
		{
			return m_currentMovementState;
		}

		public MyCharacterMovementEnum GetPreviousMovementState()
		{
			return m_previousMovementState;
		}

		public MyCharacterMovementEnum GetNetworkMovementState()
		{
			return m_previousNetworkMovementState;
		}

		public void SetPreviousMovementState(MyCharacterMovementEnum previousMovementState)
		{
			m_previousMovementState = previousMovementState;
		}

		private void SetStandingLocalAABB()
		{
			float num = Definition.CharacterCollisionWidth / 2f;
			base.PositionComp.LocalAABB = new BoundingBox(-new Vector3(num, 0f, num), new Vector3(num, Definition.CharacterCollisionHeight, num));
		}

		private void SetCrouchingLocalAABB()
		{
			float num = Definition.CharacterCollisionWidth / 2f;
			base.PositionComp.LocalAABB = new BoundingBox(-new Vector3(num, 0f, num), new Vector3(num, Definition.CharacterCollisionHeight / 2f, num));
		}

		internal void SetCurrentMovementState(MyCharacterMovementEnum state)
		{
			if (m_currentMovementState != state)
			{
				m_previousMovementState = m_currentMovementState;
				m_currentMovementState = state;
				UpdateCrouchState();
				if (this.OnMovementStateChanged != null)
				{
					this.OnMovementStateChanged(m_previousMovementState, m_currentMovementState);
				}
				if (this.MovementStateChanged != null)
				{
					this.MovementStateChanged(this, m_previousMovementState, m_currentMovementState);
				}
			}
		}

		private void UpdateCrouchState()
		{
			bool isCrouching = IsCrouching;
			bool flag = m_previousMovementState.GetMode() == 2;
			MyCharacterProxy characterProxy = Physics.CharacterProxy;
			if (characterProxy != null && characterProxy.IsCrouching != isCrouching)
			{
				characterProxy.SetShapeForCrouch(Physics.HavokWorld, isCrouching);
			}
			if (isCrouching != flag)
			{
				if (isCrouching)
				{
					SetCrouchingLocalAABB();
				}
				else
				{
					SetStandingLocalAABB();
				}
				if (characterProxy == null)
				{
					UpdateCharacterPhysics(forceUpdate: true);
				}
			}
		}

		private float GetMovementAcceleration(MyCharacterMovementEnum movement)
		{
			switch (movement)
			{
			case MyCharacterMovementEnum.Standing:
			case MyCharacterMovementEnum.Crouching:
				return MyPerGameSettings.CharacterMovement.WalkAcceleration;
			case MyCharacterMovementEnum.Walking:
			case MyCharacterMovementEnum.CrouchWalking:
			case MyCharacterMovementEnum.BackWalking:
			case MyCharacterMovementEnum.CrouchBackWalking:
			case MyCharacterMovementEnum.WalkingLeftFront:
			case MyCharacterMovementEnum.CrouchWalkingLeftFront:
			case MyCharacterMovementEnum.WalkingLeftBack:
			case MyCharacterMovementEnum.CrouchWalkingLeftBack:
			case MyCharacterMovementEnum.WalkingRightFront:
			case MyCharacterMovementEnum.CrouchWalkingRightFront:
			case MyCharacterMovementEnum.WalkingRightBack:
			case MyCharacterMovementEnum.CrouchWalkingRightBack:
			case MyCharacterMovementEnum.Running:
			case MyCharacterMovementEnum.Backrunning:
			case MyCharacterMovementEnum.RunningLeftFront:
			case MyCharacterMovementEnum.RunningLeftBack:
			case MyCharacterMovementEnum.RunningRightFront:
			case MyCharacterMovementEnum.RunningRightBack:
				return MyPerGameSettings.CharacterMovement.WalkAcceleration;
			case MyCharacterMovementEnum.Sprinting:
				return MyPerGameSettings.CharacterMovement.SprintAcceleration;
			case MyCharacterMovementEnum.Jump:
				return 0f;
			case MyCharacterMovementEnum.WalkStrafingLeft:
			case MyCharacterMovementEnum.CrouchStrafingLeft:
			case MyCharacterMovementEnum.RunStrafingLeft:
				return MyPerGameSettings.CharacterMovement.WalkAcceleration;
			case MyCharacterMovementEnum.WalkStrafingRight:
			case MyCharacterMovementEnum.CrouchStrafingRight:
			case MyCharacterMovementEnum.RunStrafingRight:
				return MyPerGameSettings.CharacterMovement.WalkAcceleration;
			default:
				return 0f;
			}
		}

		private void SlowDownX()
		{
			if (Math.Abs(m_headMovementXOffset) > 0f)
			{
				m_headMovementXOffset += (float)Math.Sign(0f - m_headMovementXOffset) * m_headMovementStep;
				if (Math.Abs(m_headMovementXOffset) < m_headMovementStep)
				{
					m_headMovementXOffset = 0f;
				}
			}
		}

		private void SlowDownY()
		{
			if (Math.Abs(m_headMovementYOffset) > 0f)
			{
				m_headMovementYOffset += (float)Math.Sign(0f - m_headMovementYOffset) * m_headMovementStep;
				if (Math.Abs(m_headMovementYOffset) < m_headMovementStep)
				{
					m_headMovementYOffset = 0f;
				}
			}
		}

		private void AccelerateX(float sign)
		{
			m_headMovementXOffset += sign * m_headMovementStep;
			if (sign > 0f)
			{
				if (m_headMovementXOffset > m_maxHeadMovementOffset)
				{
					m_headMovementXOffset = m_maxHeadMovementOffset;
				}
			}
			else if (m_headMovementXOffset < 0f - m_maxHeadMovementOffset)
			{
				m_headMovementXOffset = 0f - m_maxHeadMovementOffset;
			}
		}

		private void AccelerateY(float sign)
		{
			m_headMovementYOffset += sign * m_headMovementStep;
			if (sign > 0f)
			{
				if (m_headMovementYOffset > m_maxHeadMovementOffset)
				{
					m_headMovementYOffset = m_maxHeadMovementOffset;
				}
			}
			else if (m_headMovementYOffset < 0f - m_maxHeadMovementOffset)
			{
				m_headMovementYOffset = 0f - m_maxHeadMovementOffset;
			}
		}

		private void UpdateHeadOffset()
		{
			switch (m_currentMovementState)
			{
			case (MyCharacterMovementEnum)67:
			case (MyCharacterMovementEnum)68:
			case (MyCharacterMovementEnum)69:
			case (MyCharacterMovementEnum)70:
			case (MyCharacterMovementEnum)71:
			case (MyCharacterMovementEnum)72:
			case (MyCharacterMovementEnum)73:
			case (MyCharacterMovementEnum)74:
			case (MyCharacterMovementEnum)75:
			case (MyCharacterMovementEnum)76:
			case (MyCharacterMovementEnum)77:
			case (MyCharacterMovementEnum)78:
			case (MyCharacterMovementEnum)79:
			case (MyCharacterMovementEnum)81:
			case MyCharacterMovementEnum.CrouchWalkingLeftFront:
				_ = 82;
				break;
			case (MyCharacterMovementEnum)83:
			case (MyCharacterMovementEnum)84:
			case (MyCharacterMovementEnum)85:
			case (MyCharacterMovementEnum)86:
			case (MyCharacterMovementEnum)87:
			case (MyCharacterMovementEnum)88:
			case (MyCharacterMovementEnum)89:
			case (MyCharacterMovementEnum)90:
			case (MyCharacterMovementEnum)91:
			case (MyCharacterMovementEnum)92:
			case (MyCharacterMovementEnum)93:
			case (MyCharacterMovementEnum)94:
			case (MyCharacterMovementEnum)95:
			case (MyCharacterMovementEnum)97:
			case MyCharacterMovementEnum.CrouchWalkingLeftBack:
				_ = 98;
				break;
			case (MyCharacterMovementEnum)131:
			case (MyCharacterMovementEnum)132:
			case (MyCharacterMovementEnum)133:
			case (MyCharacterMovementEnum)134:
			case (MyCharacterMovementEnum)135:
			case (MyCharacterMovementEnum)136:
			case (MyCharacterMovementEnum)137:
			case (MyCharacterMovementEnum)138:
			case (MyCharacterMovementEnum)139:
			case (MyCharacterMovementEnum)140:
			case (MyCharacterMovementEnum)141:
			case (MyCharacterMovementEnum)142:
			case (MyCharacterMovementEnum)143:
			case (MyCharacterMovementEnum)145:
			case MyCharacterMovementEnum.CrouchWalkingRightFront:
				_ = 146;
				break;
			case (MyCharacterMovementEnum)1089:
			case (MyCharacterMovementEnum)1090:
			case (MyCharacterMovementEnum)1091:
			case (MyCharacterMovementEnum)1092:
			case (MyCharacterMovementEnum)1093:
			case (MyCharacterMovementEnum)1094:
			case (MyCharacterMovementEnum)1095:
			case (MyCharacterMovementEnum)1096:
			case (MyCharacterMovementEnum)1097:
			case (MyCharacterMovementEnum)1098:
			case (MyCharacterMovementEnum)1099:
			case (MyCharacterMovementEnum)1100:
			case (MyCharacterMovementEnum)1101:
			case (MyCharacterMovementEnum)1102:
			case (MyCharacterMovementEnum)1103:
			case (MyCharacterMovementEnum)1105:
			case (MyCharacterMovementEnum)1106:
			case (MyCharacterMovementEnum)1107:
			case (MyCharacterMovementEnum)1108:
			case (MyCharacterMovementEnum)1109:
			case (MyCharacterMovementEnum)1110:
			case (MyCharacterMovementEnum)1111:
			case (MyCharacterMovementEnum)1112:
			case (MyCharacterMovementEnum)1113:
			case (MyCharacterMovementEnum)1114:
			case (MyCharacterMovementEnum)1115:
			case (MyCharacterMovementEnum)1116:
			case (MyCharacterMovementEnum)1117:
			case (MyCharacterMovementEnum)1118:
			case (MyCharacterMovementEnum)1119:
			case MyCharacterMovementEnum.RunningLeftBack:
				_ = 1120;
				break;
			case (MyCharacterMovementEnum)1121:
			case (MyCharacterMovementEnum)1122:
			case (MyCharacterMovementEnum)1123:
			case (MyCharacterMovementEnum)1124:
			case (MyCharacterMovementEnum)1125:
			case (MyCharacterMovementEnum)1126:
			case (MyCharacterMovementEnum)1127:
			case (MyCharacterMovementEnum)1128:
			case (MyCharacterMovementEnum)1129:
			case (MyCharacterMovementEnum)1130:
			case (MyCharacterMovementEnum)1131:
			case (MyCharacterMovementEnum)1132:
			case (MyCharacterMovementEnum)1133:
			case (MyCharacterMovementEnum)1134:
			case (MyCharacterMovementEnum)1135:
			case (MyCharacterMovementEnum)1136:
			case (MyCharacterMovementEnum)1137:
			case (MyCharacterMovementEnum)1138:
			case (MyCharacterMovementEnum)1139:
			case (MyCharacterMovementEnum)1140:
			case (MyCharacterMovementEnum)1141:
			case (MyCharacterMovementEnum)1142:
			case (MyCharacterMovementEnum)1143:
			case (MyCharacterMovementEnum)1144:
			case (MyCharacterMovementEnum)1145:
			case (MyCharacterMovementEnum)1146:
			case (MyCharacterMovementEnum)1147:
			case (MyCharacterMovementEnum)1148:
			case (MyCharacterMovementEnum)1149:
			case (MyCharacterMovementEnum)1150:
			case (MyCharacterMovementEnum)1151:
			case (MyCharacterMovementEnum)1153:
			case (MyCharacterMovementEnum)1154:
			case (MyCharacterMovementEnum)1155:
			case (MyCharacterMovementEnum)1156:
			case (MyCharacterMovementEnum)1157:
			case (MyCharacterMovementEnum)1158:
			case (MyCharacterMovementEnum)1159:
			case (MyCharacterMovementEnum)1160:
			case (MyCharacterMovementEnum)1161:
			case (MyCharacterMovementEnum)1162:
			case (MyCharacterMovementEnum)1163:
			case (MyCharacterMovementEnum)1164:
			case (MyCharacterMovementEnum)1165:
			case (MyCharacterMovementEnum)1166:
			case (MyCharacterMovementEnum)1167:
			case MyCharacterMovementEnum.RunningRightFront:
				_ = 1168;
				break;
			case MyCharacterMovementEnum.Standing:
			case MyCharacterMovementEnum.Crouching:
			case MyCharacterMovementEnum.Falling:
			case MyCharacterMovementEnum.Jump:
			case MyCharacterMovementEnum.RotatingLeft:
			case MyCharacterMovementEnum.CrouchRotatingLeft:
			case MyCharacterMovementEnum.RotatingRight:
			case MyCharacterMovementEnum.CrouchRotatingRight:
				SlowDownX();
				SlowDownY();
				break;
			case MyCharacterMovementEnum.Walking:
			case MyCharacterMovementEnum.CrouchWalking:
			case MyCharacterMovementEnum.Running:
			case MyCharacterMovementEnum.Sprinting:
				AccelerateX(-1f);
				SlowDownY();
				break;
			case MyCharacterMovementEnum.BackWalking:
			case MyCharacterMovementEnum.CrouchBackWalking:
			case MyCharacterMovementEnum.Backrunning:
				AccelerateX(1f);
				SlowDownY();
				break;
			case MyCharacterMovementEnum.WalkStrafingLeft:
			case MyCharacterMovementEnum.CrouchStrafingLeft:
			case MyCharacterMovementEnum.RunStrafingLeft:
				SlowDownX();
				AccelerateY(1f);
				break;
			case MyCharacterMovementEnum.WalkStrafingRight:
			case MyCharacterMovementEnum.CrouchStrafingRight:
			case MyCharacterMovementEnum.RunStrafingRight:
				SlowDownX();
				AccelerateY(-1f);
				break;
			}
		}

		public static bool IsWalkingState(MyCharacterMovementEnum state)
		{
			switch (state)
			{
			case MyCharacterMovementEnum.Walking:
			case MyCharacterMovementEnum.CrouchWalking:
			case MyCharacterMovementEnum.BackWalking:
			case MyCharacterMovementEnum.CrouchBackWalking:
			case MyCharacterMovementEnum.WalkStrafingLeft:
			case MyCharacterMovementEnum.CrouchStrafingLeft:
			case MyCharacterMovementEnum.WalkingLeftFront:
			case MyCharacterMovementEnum.CrouchWalkingLeftFront:
			case MyCharacterMovementEnum.WalkingLeftBack:
			case MyCharacterMovementEnum.CrouchWalkingLeftBack:
			case MyCharacterMovementEnum.WalkStrafingRight:
			case MyCharacterMovementEnum.CrouchStrafingRight:
			case MyCharacterMovementEnum.WalkingRightFront:
			case MyCharacterMovementEnum.CrouchWalkingRightFront:
			case MyCharacterMovementEnum.WalkingRightBack:
			case MyCharacterMovementEnum.CrouchWalkingRightBack:
			case MyCharacterMovementEnum.Running:
			case MyCharacterMovementEnum.Backrunning:
			case MyCharacterMovementEnum.RunStrafingLeft:
			case MyCharacterMovementEnum.RunningLeftFront:
			case MyCharacterMovementEnum.RunningLeftBack:
			case MyCharacterMovementEnum.RunStrafingRight:
			case MyCharacterMovementEnum.RunningRightFront:
			case MyCharacterMovementEnum.RunningRightBack:
			case MyCharacterMovementEnum.Sprinting:
				return true;
			default:
				return false;
			}
		}

		public static bool IsRunningState(MyCharacterMovementEnum state)
		{
			switch (state)
			{
			case MyCharacterMovementEnum.Running:
			case MyCharacterMovementEnum.Backrunning:
			case MyCharacterMovementEnum.RunStrafingLeft:
			case MyCharacterMovementEnum.RunningLeftFront:
			case MyCharacterMovementEnum.RunningLeftBack:
			case MyCharacterMovementEnum.RunStrafingRight:
			case MyCharacterMovementEnum.RunningRightFront:
			case MyCharacterMovementEnum.RunningRightBack:
			case MyCharacterMovementEnum.Sprinting:
				return true;
			default:
				return false;
			}
		}

		internal void SwitchAnimation(MyCharacterMovementEnum movementState, bool checkState = true)
		{
			if ((!Sandbox.Engine.Platform.Game.IsDedicated || !MyPerGameSettings.DisableAnimationsOnDS) && (!checkState || m_currentMovementState != movementState))
			{
				bool num = IsWalkingState(m_currentMovementState);
				bool flag = IsWalkingState(movementState);
				if (num != flag)
				{
					m_currentHandItemWalkingBlend = 0f;
				}
				switch (movementState)
				{
				case MyCharacterMovementEnum.Walking:
					PlayCharacterAnimation("Walk", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.1f));
					break;
				case MyCharacterMovementEnum.BackWalking:
					PlayCharacterAnimation("WalkBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.WalkingLeftBack:
					PlayCharacterAnimation("WalkLeftBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.WalkingRightBack:
					PlayCharacterAnimation("WalkRightBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.WalkStrafingLeft:
					PlayCharacterAnimation("StrafeLeft", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.WalkStrafingRight:
					PlayCharacterAnimation("StrafeRight", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.WalkingLeftFront:
					PlayCharacterAnimation("WalkLeftFront", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.WalkingRightFront:
					PlayCharacterAnimation("WalkRightFront", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.Running:
					PlayCharacterAnimation("Run", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.Backrunning:
					PlayCharacterAnimation("RunBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RunningLeftBack:
					PlayCharacterAnimation("RunLeftBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RunningRightBack:
					PlayCharacterAnimation("RunRightBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RunStrafingLeft:
					PlayCharacterAnimation("RunLeft", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RunStrafingRight:
					PlayCharacterAnimation("RunRight", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RunningLeftFront:
					PlayCharacterAnimation("RunLeftFront", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RunningRightFront:
					PlayCharacterAnimation("RunRightFront", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchWalking:
					PlayCharacterAnimation("CrouchWalk", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchWalkingLeftFront:
					PlayCharacterAnimation("CrouchWalkLeftFront", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchWalkingRightFront:
					PlayCharacterAnimation("CrouchWalkRightFront", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchBackWalking:
					PlayCharacterAnimation("CrouchWalkBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchWalkingLeftBack:
					PlayCharacterAnimation("CrouchWalkLeftBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchWalkingRightBack:
					PlayCharacterAnimation("CrouchWalkRightBack", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchStrafingLeft:
					PlayCharacterAnimation("CrouchStrafeLeft", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchStrafingRight:
					PlayCharacterAnimation("CrouchStrafeRight", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.Sprinting:
					PlayCharacterAnimation("Sprint", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.1f));
					break;
				case MyCharacterMovementEnum.Standing:
					PlayCharacterAnimation("Idle", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.Crouching:
					PlayCharacterAnimation("CrouchIdle", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.1f));
					break;
				case MyCharacterMovementEnum.Flying:
					PlayCharacterAnimation("Jetpack", AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Loop, AdjustSafeAnimationBlend(0f));
					break;
				case MyCharacterMovementEnum.Jump:
					PlayCharacterAnimation("Jump", AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Default, AdjustSafeAnimationBlend(0f), 1.3f);
					break;
				case MyCharacterMovementEnum.Falling:
					PlayCharacterAnimation("FreeFall", AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchRotatingLeft:
					PlayCharacterAnimation("CrouchLeftTurn", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.CrouchRotatingRight:
					PlayCharacterAnimation("CrouchRightTurn", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RotatingLeft:
					PlayCharacterAnimation("StandLeftTurn", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.RotatingRight:
					PlayCharacterAnimation("StandRightTurn", AdjustSafeAnimationEnd(MyBlendOption.WaitForPreviousEnd), MyFrameOption.Loop, AdjustSafeAnimationBlend(0.2f));
					break;
				case MyCharacterMovementEnum.Died:
					PlayCharacterAnimation("Died", AdjustSafeAnimationEnd(MyBlendOption.Immediate), MyFrameOption.Default, AdjustSafeAnimationBlend(0.5f));
					break;
				}
			}
		}

		private MyCharacterMovementEnum GetNewMovementState(ref Vector3 moveIndicator, ref Vector2 rotationIndicator, ref float acceleration, bool sprint, bool walk, bool canMove, bool movementFlagsChanged)
		{
			if (m_currentMovementState == MyCharacterMovementEnum.Died)
			{
				return MyCharacterMovementEnum.Died;
			}
			MyCharacterMovementEnum myCharacterMovementEnum = m_currentMovementState;
			if (Definition.UseOnlyWalking)
			{
				walk = true;
			}
			if (m_currentJumpTime > 0f)
			{
				return MyCharacterMovementEnum.Jump;
			}
			if (JetpackRunning)
			{
				return MyCharacterMovementEnum.Flying;
			}
			bool flag = true;
			bool flag2 = true;
			bool flag3 = true;
			bool flag4 = true;
			bool continuous = false;
			bool continuous2 = false;
			bool continuous3 = false;
			switch (m_currentMovementState)
			{
			case MyCharacterMovementEnum.Walking:
			case MyCharacterMovementEnum.WalkStrafingLeft:
			case MyCharacterMovementEnum.WalkingLeftFront:
			case MyCharacterMovementEnum.WalkingLeftBack:
			case MyCharacterMovementEnum.WalkStrafingRight:
			case MyCharacterMovementEnum.WalkingRightFront:
			case MyCharacterMovementEnum.WalkingRightBack:
				continuous = true;
				break;
			case MyCharacterMovementEnum.Running:
			case MyCharacterMovementEnum.RunStrafingLeft:
			case MyCharacterMovementEnum.RunningLeftFront:
			case MyCharacterMovementEnum.RunningLeftBack:
			case MyCharacterMovementEnum.RunStrafingRight:
			case MyCharacterMovementEnum.RunningRightFront:
			case MyCharacterMovementEnum.RunningRightBack:
				continuous2 = true;
				break;
			case MyCharacterMovementEnum.Sprinting:
				continuous3 = true;
				break;
			case MyCharacterMovementEnum.Ladder:
			case MyCharacterMovementEnum.LadderUp:
			case MyCharacterMovementEnum.LadderDown:
			case MyCharacterMovementEnum.LadderOut:
				return myCharacterMovementEnum;
			}
			if (StatComp != null)
			{
				flag = StatComp.CanDoAction("Walk", out MyTuple<ushort, MyStringHash> message, continuous);
				flag2 = StatComp.CanDoAction("Run", out message, continuous2);
				flag3 = StatComp.CanDoAction("Sprint", out message, continuous3);
				if (MySession.Static != null && MySession.Static.LocalCharacter == this && message.Item1 == 4 && message.Item2.String.CompareTo("Stamina") == 0)
				{
					m_notEnoughStatNotification.SetTextFormatArguments(message.Item2);
					MyHud.Notifications.Add(m_notEnoughStatNotification);
				}
				flag4 = (flag || flag2 || flag3);
			}
			bool flag5 = (moveIndicator.X != 0f || moveIndicator.Z != 0f) && canMove && flag4;
			bool flag6 = rotationIndicator.X != 0f || rotationIndicator.Y != 0f;
			if (flag5 || movementFlagsChanged)
			{
				myCharacterMovementEnum = ((sprint && flag3) ? GetSprintState(ref moveIndicator) : ((!flag5) ? GetIdleState() : ((walk && flag) ? GetWalkingState(ref moveIndicator) : ((!flag2) ? GetWalkingState(ref moveIndicator) : GetRunningState(ref moveIndicator)))));
				acceleration = GetMovementAcceleration(myCharacterMovementEnum);
				m_currentDecceleration = 0f;
			}
			else if (flag6)
			{
				if (Math.Abs(rotationIndicator.Y) > 20f && (m_currentMovementState == MyCharacterMovementEnum.Standing || m_currentMovementState == MyCharacterMovementEnum.Crouching))
				{
					myCharacterMovementEnum = (WantsCrouch ? ((!(rotationIndicator.Y > 0f)) ? MyCharacterMovementEnum.CrouchRotatingLeft : MyCharacterMovementEnum.CrouchRotatingRight) : ((!(rotationIndicator.Y > 0f)) ? MyCharacterMovementEnum.RotatingLeft : MyCharacterMovementEnum.RotatingRight));
				}
			}
			else
			{
				switch (m_currentMovementState)
				{
				case MyCharacterMovementEnum.Walking:
				case MyCharacterMovementEnum.CrouchWalking:
				case MyCharacterMovementEnum.BackWalking:
				case MyCharacterMovementEnum.CrouchBackWalking:
				case MyCharacterMovementEnum.WalkStrafingLeft:
				case MyCharacterMovementEnum.CrouchStrafingLeft:
				case MyCharacterMovementEnum.WalkingLeftFront:
				case MyCharacterMovementEnum.CrouchWalkingLeftFront:
				case MyCharacterMovementEnum.WalkingLeftBack:
				case MyCharacterMovementEnum.CrouchWalkingLeftBack:
				case MyCharacterMovementEnum.WalkStrafingRight:
				case MyCharacterMovementEnum.CrouchStrafingRight:
				case MyCharacterMovementEnum.WalkingRightFront:
				case MyCharacterMovementEnum.CrouchWalkingRightFront:
				case MyCharacterMovementEnum.WalkingRightBack:
				case MyCharacterMovementEnum.CrouchWalkingRightBack:
				case MyCharacterMovementEnum.Running:
				case MyCharacterMovementEnum.Backrunning:
				case MyCharacterMovementEnum.RunStrafingLeft:
				case MyCharacterMovementEnum.RunningLeftFront:
				case MyCharacterMovementEnum.RunningLeftBack:
				case MyCharacterMovementEnum.RunStrafingRight:
				case MyCharacterMovementEnum.RunningRightFront:
				case MyCharacterMovementEnum.RunningRightBack:
					myCharacterMovementEnum = GetIdleState();
					m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
					break;
				case MyCharacterMovementEnum.Sprinting:
					myCharacterMovementEnum = GetIdleState();
					m_currentDecceleration = MyPerGameSettings.CharacterMovement.SprintDecceleration;
					break;
				case MyCharacterMovementEnum.Standing:
					if (WantsCrouch)
					{
						myCharacterMovementEnum = GetIdleState();
					}
					m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
					break;
				case MyCharacterMovementEnum.Crouching:
					if (!WantsCrouch)
					{
						myCharacterMovementEnum = GetIdleState();
					}
					m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
					break;
				case MyCharacterMovementEnum.RotatingLeft:
				case MyCharacterMovementEnum.CrouchRotatingLeft:
				case MyCharacterMovementEnum.RotatingRight:
				case MyCharacterMovementEnum.CrouchRotatingRight:
					myCharacterMovementEnum = GetIdleState();
					m_currentDecceleration = MyPerGameSettings.CharacterMovement.WalkDecceleration;
					break;
				}
			}
			return myCharacterMovementEnum;
		}

		internal float LimitMaxSpeed(float currentSpeed, MyCharacterMovementEnum movementState, float serverRatio)
		{
			float result = currentSpeed;
			switch (movementState)
			{
			case MyCharacterMovementEnum.Flying:
			case MyCharacterMovementEnum.Running:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxRunSpeed) * serverRatio, Definition.MaxRunSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.Walking:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxWalkSpeed) * serverRatio, Definition.MaxWalkSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.BackWalking:
			case MyCharacterMovementEnum.WalkingLeftBack:
			case MyCharacterMovementEnum.WalkingRightBack:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxBackwalkSpeed) * serverRatio, Definition.MaxBackwalkSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.WalkStrafingLeft:
			case MyCharacterMovementEnum.WalkingLeftFront:
			case MyCharacterMovementEnum.WalkStrafingRight:
			case MyCharacterMovementEnum.WalkingRightFront:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxWalkStrafingSpeed) * serverRatio, Definition.MaxWalkStrafingSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.Backrunning:
			case MyCharacterMovementEnum.RunningLeftBack:
			case MyCharacterMovementEnum.RunningRightBack:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxBackrunSpeed) * serverRatio, Definition.MaxBackrunSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.RunStrafingLeft:
			case MyCharacterMovementEnum.RunningLeftFront:
			case MyCharacterMovementEnum.RunStrafingRight:
			case MyCharacterMovementEnum.RunningRightFront:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxRunStrafingSpeed) * serverRatio, Definition.MaxRunStrafingSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.CrouchWalking:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxCrouchWalkSpeed) * serverRatio, Definition.MaxCrouchWalkSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.CrouchStrafingLeft:
			case MyCharacterMovementEnum.CrouchWalkingLeftFront:
			case MyCharacterMovementEnum.CrouchStrafingRight:
			case MyCharacterMovementEnum.CrouchWalkingRightFront:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxCrouchStrafingSpeed) * serverRatio, Definition.MaxCrouchStrafingSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.CrouchBackWalking:
			case MyCharacterMovementEnum.CrouchWalkingLeftBack:
			case MyCharacterMovementEnum.CrouchWalkingRightBack:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxCrouchBackwalkSpeed) * serverRatio, Definition.MaxCrouchBackwalkSpeed * serverRatio);
				break;
			case MyCharacterMovementEnum.Sprinting:
				result = MathHelper.Clamp(currentSpeed, (0f - Definition.MaxSprintSpeed) * serverRatio, Definition.MaxSprintSpeed * serverRatio);
				break;
			}
			return result;
		}

		private float AdjustSafeAnimationBlend(float idealBlend)
		{
			float result = 0f;
			if (m_currentAnimationChangeDelay > SAFE_DELAY_FOR_ANIMATION_BLEND)
			{
				result = idealBlend;
			}
			m_currentAnimationChangeDelay = 0f;
			return result;
		}

		private MyBlendOption AdjustSafeAnimationEnd(MyBlendOption idealEnd)
		{
			MyBlendOption result = MyBlendOption.Immediate;
			if (m_currentAnimationChangeDelay > SAFE_DELAY_FOR_ANIMATION_BLEND)
			{
				result = idealEnd;
			}
			return result;
		}

		private MyCharacterMovementEnum GetWalkingState(ref Vector3 moveIndicator)
		{
			double num = Math.Tan(MathHelper.ToRadians(23f));
			if ((double)Math.Abs(moveIndicator.X) < num * (double)Math.Abs(moveIndicator.Z))
			{
				if (moveIndicator.Z < 0f)
				{
					if (!WantsCrouch)
					{
						return MyCharacterMovementEnum.Walking;
					}
					return MyCharacterMovementEnum.CrouchWalking;
				}
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.BackWalking;
				}
				return MyCharacterMovementEnum.CrouchBackWalking;
			}
			if ((double)Math.Abs(moveIndicator.X) * num > (double)Math.Abs(moveIndicator.Z))
			{
				if (moveIndicator.X > 0f)
				{
					if (!WantsCrouch)
					{
						return MyCharacterMovementEnum.WalkStrafingRight;
					}
					return MyCharacterMovementEnum.CrouchStrafingRight;
				}
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.WalkStrafingLeft;
				}
				return MyCharacterMovementEnum.CrouchStrafingLeft;
			}
			if (moveIndicator.X > 0f)
			{
				if (moveIndicator.Z < 0f)
				{
					if (!WantsCrouch)
					{
						return MyCharacterMovementEnum.WalkingRightFront;
					}
					return MyCharacterMovementEnum.CrouchWalkingRightFront;
				}
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.WalkingRightBack;
				}
				return MyCharacterMovementEnum.CrouchWalkingRightBack;
			}
			if (moveIndicator.Z < 0f)
			{
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.WalkingLeftFront;
				}
				return MyCharacterMovementEnum.CrouchWalkingLeftFront;
			}
			if (!WantsCrouch)
			{
				return MyCharacterMovementEnum.WalkingLeftBack;
			}
			return MyCharacterMovementEnum.CrouchWalkingLeftBack;
		}

		private MyCharacterMovementEnum GetRunningState(ref Vector3 moveIndicator)
		{
			double num = Math.Tan(MathHelper.ToRadians(23f));
			if ((double)Math.Abs(moveIndicator.X) < num * (double)Math.Abs(moveIndicator.Z))
			{
				if (moveIndicator.Z < 0f)
				{
					if (!WantsCrouch)
					{
						return MyCharacterMovementEnum.Running;
					}
					return MyCharacterMovementEnum.CrouchWalking;
				}
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.Backrunning;
				}
				return MyCharacterMovementEnum.CrouchBackWalking;
			}
			if ((double)Math.Abs(moveIndicator.X) * num > (double)Math.Abs(moveIndicator.Z))
			{
				if (moveIndicator.X > 0f)
				{
					if (!WantsCrouch)
					{
						return MyCharacterMovementEnum.RunStrafingRight;
					}
					return MyCharacterMovementEnum.CrouchStrafingRight;
				}
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.RunStrafingLeft;
				}
				return MyCharacterMovementEnum.CrouchStrafingLeft;
			}
			if (moveIndicator.X > 0f)
			{
				if (moveIndicator.Z < 0f)
				{
					if (!WantsCrouch)
					{
						return MyCharacterMovementEnum.RunningRightFront;
					}
					return MyCharacterMovementEnum.CrouchWalkingRightFront;
				}
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.RunningRightBack;
				}
				return MyCharacterMovementEnum.CrouchWalkingRightBack;
			}
			if (moveIndicator.Z < 0f)
			{
				if (!WantsCrouch)
				{
					return MyCharacterMovementEnum.RunningLeftFront;
				}
				return MyCharacterMovementEnum.CrouchWalkingLeftFront;
			}
			if (!WantsCrouch)
			{
				return MyCharacterMovementEnum.RunningLeftBack;
			}
			return MyCharacterMovementEnum.CrouchWalkingLeftBack;
		}

		private MyCharacterMovementEnum GetSprintState(ref Vector3 moveIndicator)
		{
			if (Math.Abs(moveIndicator.X) < 0.1f && moveIndicator.Z < 0f)
			{
				return MyCharacterMovementEnum.Sprinting;
			}
			return GetRunningState(ref moveIndicator);
		}

		private MyCharacterMovementEnum GetIdleState()
		{
			if (!WantsCrouch)
			{
				return MyCharacterMovementEnum.Standing;
			}
			return MyCharacterMovementEnum.Crouching;
		}

		private bool UpdateCapsuleBones()
		{
			if (m_characterBoneCapsulesReady)
			{
				return true;
			}
			if (m_bodyCapsuleInfo == null || m_bodyCapsuleInfo.Count == 0)
			{
				return false;
			}
			MyRenderDebugInputComponent.Clear();
			MyCharacterBone[] characterBones = base.AnimationController.CharacterBones;
			if (Physics.Ragdoll != null && base.Components.Has<MyCharacterRagdollComponent>())
			{
				MyCharacterRagdollComponent myCharacterRagdollComponent = base.Components.Get<MyCharacterRagdollComponent>();
				for (int i = 0; i < m_bodyCapsuleInfo.Count; i++)
				{
					MyBoneCapsuleInfo myBoneCapsuleInfo = m_bodyCapsuleInfo[i];
					if (characterBones == null || myBoneCapsuleInfo.Bone1 >= characterBones.Length || myBoneCapsuleInfo.Bone2 >= characterBones.Length)
					{
						continue;
					}
					HkRigidBody bodyBindedToBone = myCharacterRagdollComponent.RagdollMapper.GetBodyBindedToBone(characterBones[myBoneCapsuleInfo.Bone1]);
					MatrixD matrix = characterBones[myBoneCapsuleInfo.Bone1].AbsoluteTransform * base.WorldMatrix;
					HkShape shape = bodyBindedToBone.GetShape();
					m_bodyCapsules[i].P0 = matrix.Translation;
					m_bodyCapsules[i].P1 = (characterBones[myBoneCapsuleInfo.Bone2].AbsoluteTransform * base.WorldMatrix).Translation;
					Vector3 vector = m_bodyCapsules[i].P0 - m_bodyCapsules[i].P1;
					if (vector.LengthSquared() < 0.05f)
					{
						if (shape.ShapeType == HkShapeType.Capsule)
						{
							HkCapsuleShape hkCapsuleShape = (HkCapsuleShape)shape;
							m_bodyCapsules[i].P0 = Vector3.Transform(hkCapsuleShape.VertexA, matrix);
							m_bodyCapsules[i].P1 = Vector3.Transform(hkCapsuleShape.VertexB, matrix);
							m_bodyCapsules[i].Radius = hkCapsuleShape.Radius * 0.8f;
							if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
							{
								MyRenderDebugInputComponent.AddCapsule(m_bodyCapsules[i], Color.Green);
							}
							continue;
						}
						shape.GetLocalAABB(0.0001f, out Vector4 min, out Vector4 max);
						float num = Math.Max(Math.Max(max.X - min.X, max.Y - min.Y), max.Z - min.Z) * 0.5f;
						m_bodyCapsules[i].P0 = matrix.Translation + matrix.Left * num * 0.25;
						m_bodyCapsules[i].P1 = matrix.Translation + matrix.Left * num * 0.5;
						m_bodyCapsules[i].Radius = num * 0.25f;
						if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
						{
							MyRenderDebugInputComponent.AddCapsule(m_bodyCapsules[i], Color.Blue);
						}
					}
					else
					{
						if (myBoneCapsuleInfo.Radius != 0f)
						{
							m_bodyCapsules[i].Radius = myBoneCapsuleInfo.Radius;
						}
						else if (shape.ShapeType == HkShapeType.Capsule)
						{
							HkCapsuleShape hkCapsuleShape2 = (HkCapsuleShape)shape;
							m_bodyCapsules[i].Radius = hkCapsuleShape2.Radius;
						}
						else
						{
							m_bodyCapsules[i].Radius = vector.Length() * 0.28f;
						}
						if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
						{
							MyRenderDebugInputComponent.AddCapsule(m_bodyCapsules[i], Color.Blue);
							MyRenderProxy.DebugDrawCapsule(m_bodyCapsules[i].P0, m_bodyCapsules[i].P1, m_bodyCapsules[i].Radius, Color.Yellow, depthRead: false);
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < m_bodyCapsuleInfo.Count; j++)
				{
					MyBoneCapsuleInfo myBoneCapsuleInfo2 = m_bodyCapsuleInfo[j];
					if (characterBones != null && myBoneCapsuleInfo2.Bone1 < characterBones.Length && myBoneCapsuleInfo2.Bone2 < characterBones.Length)
					{
						m_bodyCapsules[j].P0 = (characterBones[myBoneCapsuleInfo2.Bone1].AbsoluteTransform * base.WorldMatrix).Translation;
						m_bodyCapsules[j].P1 = (characterBones[myBoneCapsuleInfo2.Bone2].AbsoluteTransform * base.WorldMatrix).Translation;
						Vector3 vector2 = m_bodyCapsules[j].P0 - m_bodyCapsules[j].P1;
						if (myBoneCapsuleInfo2.Radius != 0f)
						{
							m_bodyCapsules[j].Radius = myBoneCapsuleInfo2.Radius;
						}
						else if (vector2.LengthSquared() < 0.05f)
						{
							m_bodyCapsules[j].P1 = m_bodyCapsules[j].P0 + (characterBones[myBoneCapsuleInfo2.Bone1].AbsoluteTransform * base.WorldMatrix).Left * 0.10000000149011612;
							m_bodyCapsules[j].Radius = 0.1f;
						}
						else
						{
							m_bodyCapsules[j].Radius = vector2.Length() * 0.3f;
						}
						if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
						{
							MyRenderDebugInputComponent.AddCapsule(m_bodyCapsules[j], Color.Green);
						}
					}
				}
			}
			m_characterBoneCapsulesReady = true;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_SHOW_DAMAGE)
			{
				foreach (Tuple<CapsuleD, Color> item in MyRenderDebugInputComponent.CapsulesToDraw)
				{
					MyRenderProxy.DebugDrawCapsule(item.Item1.P0, item.Item1.P1, item.Item1.Radius, item.Item2, depthRead: false);
				}
			}
			return true;
		}

		private MatrixD GetHeadMatrixInternal(int headBone, bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false)
		{
			if (base.PositionComp == null)
			{
				return MatrixD.Identity;
			}
			MatrixD matrixD = MatrixD.Identity;
			bool flag = (ShouldUseAnimatedHeadRotation() && (!JetpackRunning || IsLocalHeadAnimationInProgress())) || forceHeadAnim;
			if (includeX && !flag)
			{
				matrixD = MatrixD.CreateFromAxisAngle(Vector3D.Right, MathHelper.ToRadians(m_headLocalXAngle));
			}
			if (includeY)
			{
				matrixD *= Matrix.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(m_headLocalYAngle));
			}
			Vector3 translation = Vector3.Zero;
			if (headBone != -1)
			{
				translation = base.BoneAbsoluteTransforms[headBone].Translation;
				float num = 1f - (float)Math.Cos(MathHelper.ToRadians(m_headLocalXAngle));
				translation.Y += num * base.AnimationController.InverseKinematics.RootBoneVerticalOffset;
			}
			if (flag && headBone != -1 && base.BoneAbsoluteTransforms[headBone].Right.LengthSquared() > float.Epsilon && base.BoneAbsoluteTransforms[headBone].Up.LengthSquared() > float.Epsilon && base.BoneAbsoluteTransforms[headBone].Forward.LengthSquared() > float.Epsilon)
			{
				Matrix identity = Matrix.Identity;
				identity.Translation = translation;
				m_headMatrix = MatrixD.CreateRotationX(-Math.PI / 2.0) * identity;
			}
			else
			{
				m_headMatrix = MatrixD.CreateTranslation(0.0, translation.Y, translation.Z);
			}
			if (IsInFirstPersonView && !MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER)
			{
				float num2 = Math.Abs(m_headMovementXOffset);
				float num3 = 0.03f;
				if (num2 > 0f && num2 < m_maxHeadMovementOffset / 2f)
				{
					m_headMatrix.Translation += num3 * m_headMatrix.Up * num2 * (Math.Sin(10.0 * MySandboxGame.Static.SimulationTime.Seconds) + 3.0);
				}
				else if (num2 > 0f)
				{
					m_headMatrix.Translation += num3 * m_headMatrix.Up * (m_maxHeadMovementOffset - num2) * (Math.Sin(10.0 * MySandboxGame.Static.SimulationTime.Seconds) + 3.0);
				}
				float num4 = Math.Abs(m_headMovementYOffset);
				if (num4 > 0f && num4 < m_maxHeadMovementOffset / 2f)
				{
					m_headMatrix.Translation += num3 * m_headMatrix.Up * num4 * (Math.Sin(10.0 * MySandboxGame.Static.SimulationTime.Seconds) + 3.0);
				}
				else if (num4 > 0f)
				{
					m_headMatrix.Translation += num3 * m_headMatrix.Up * (m_maxHeadMovementOffset - num4) * (Math.Sin(10.0 * MySandboxGame.Static.SimulationTime.Seconds) + 3.0);
				}
			}
			MatrixD matrixD2 = matrixD * m_headMatrix * base.WorldMatrix;
			MatrixD matrix = MatrixD.CreateFromDir(base.WorldMatrix.Forward, base.WorldMatrix.Up);
			MatrixD result = m_headMatrix * matrixD * matrix;
			result.Translation = matrixD2.Translation;
			return result;
		}

		public MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false, bool preferLocalOverSync = false)
		{
			int headBone = (IsInFirstPersonView || forceHeadBone) ? m_headBoneIndex : m_camera3rdBoneIndex;
			return GetHeadMatrixInternal(headBone, includeY, includeX, forceHeadAnim, forceHeadBone);
		}

		public MatrixD Get3rdCameraMatrix(bool includeY, bool includeX = true)
		{
			return Matrix.Invert(Get3rdBoneMatrix(includeY, includeX));
		}

		public MatrixD Get3rdBoneMatrix(bool includeY, bool includeX = true)
		{
			return GetHeadMatrixInternal(m_camera3rdBoneIndex, includeY, includeX);
		}

		public override MatrixD GetViewMatrix()
		{
			if (IsDead && MyPerGameSettings.SwitchToSpectatorCameraAfterDeath)
			{
				m_isInFirstPersonView = false;
				if (m_lastCorrectSpectatorCamera == MatrixD.Zero)
				{
					m_lastCorrectSpectatorCamera = MatrixD.CreateLookAt(base.WorldMatrix.Translation + 2f * Vector3.Up - 2f * Vector3.Forward, base.WorldMatrix.Translation, Vector3.Up);
				}
				Vector3 v = MatrixD.Invert(m_lastCorrectSpectatorCamera).Translation;
				Vector3 v2 = base.WorldMatrix.Translation;
				if (m_headBoneIndex != -1)
				{
					v2 = Vector3.Transform(base.AnimationController.CharacterBones[m_headBoneIndex].AbsoluteTransform.Translation, base.WorldMatrix);
				}
				MatrixD matrixD = MatrixD.CreateLookAt(v, v2, Vector3.Up);
				if (!matrixD.IsValid() || !(matrixD != MatrixD.Zero))
				{
					return m_lastCorrectSpectatorCamera;
				}
				return matrixD;
			}
			if (IsDead)
			{
				MySpectator.Static.SetTarget(base.PositionComp.GetPosition() + base.WorldMatrix.Up, base.WorldMatrix.Up);
			}
			if ((!ForceFirstPersonCamera || !IsDead) && !m_isInFirstPersonView)
			{
				_ = ForceFirstPersonCamera;
				bool flag = !MyThirdPersonSpectator.Static.IsCameraForced();
				if (!ForceFirstPersonCamera && flag)
				{
					return MyThirdPersonSpectator.Static.GetViewMatrix();
				}
			}
			MatrixD headMatrix = GetHeadMatrix(IsOnLadder, includeX: true, forceHeadAnim: false, ForceFirstPersonCamera, preferLocalOverSync: true);
			if (IsDead)
			{
				Vector3D translation = headMatrix.Translation;
				Vector3D vector3D = -MyGravityProviderSystem.CalculateTotalGravityInPoint(translation);
				if (!Vector3D.IsZero(vector3D))
				{
					Vector3 halfExtents = new Vector3(Definition.CharacterHeadSize * 0.5f);
					m_penetrationList.Clear();
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref Quaternion.Identity, m_penetrationList, 0);
					foreach (HkBodyCollision penetration in m_penetrationList)
					{
						VRage.ModAPI.IMyEntity collisionEntity = penetration.GetCollisionEntity();
						if (collisionEntity is MyVoxelBase || collisionEntity is MyCubeGrid)
						{
							vector3D.Normalize();
							headMatrix.Translation += vector3D;
							m_forceFirstPersonCamera = false;
							m_isInFirstPersonView = false;
							m_isInFirstPerson = false;
							break;
						}
					}
				}
			}
			m_lastCorrectSpectatorCamera = MatrixD.Zero;
			if (IsDead && m_lastGetViewWasDead)
			{
				MatrixD matrix = m_getViewAliveWorldMatrix;
				matrix.Translation = headMatrix.Translation;
				return MatrixD.Invert(matrix);
			}
			m_getViewAliveWorldMatrix = headMatrix;
			m_getViewAliveWorldMatrix.Translation = Vector3.Zero;
			m_lastGetViewWasDead = IsDead;
			return MatrixD.Invert(headMatrix);
		}

		public override bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? tri, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			bool intersectionWithLine = GetIntersectionWithLine(ref line, ref m_hitInfoTmp, flags);
			tri = m_hitInfoTmp.Triangle;
			return intersectionWithLine;
		}

		public bool GetIntersectionWithLine(ref LineD line, ref MyCharacterHitInfo info, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			if (info == null)
			{
				info = new MyCharacterHitInfo();
			}
			info.Reset();
			if (!UpdateCapsuleBones())
			{
				return false;
			}
			double num = double.MaxValue;
			Vector3D hitPosition = Vector3D.Zero;
			Vector3D p = Vector3D.Zero;
			Vector3 n = Vector3.Zero;
			Vector3 n2 = Vector3.Zero;
			int num2 = -1;
			for (int i = 0; i < m_bodyCapsules.Length; i++)
			{
				CapsuleD capsuleD = m_bodyCapsules[i];
				if (capsuleD.Intersect(line, ref hitPosition, ref p, ref n, ref n2))
				{
					double num3 = Vector3.Distance(hitPosition, line.From);
					if (!(num3 >= num))
					{
						num = num3;
						num2 = i;
					}
				}
			}
			if (num2 != -1)
			{
				MatrixD worldMatrix = base.PositionComp.WorldMatrix;
				int num4 = FindBestBone(num2, ref hitPosition, ref worldMatrix);
				MatrixD matrix = base.PositionComp.WorldMatrixNormalizedInv;
				Vector3D v = Vector3D.Transform(line.From, ref matrix);
				Vector3D v2 = Vector3D.Transform(line.To, ref matrix);
				Line inputLineInObjectSpace = new Line(v, v2);
				MyCharacterBone obj = base.AnimationController.CharacterBones[num4];
				obj.ComputeAbsoluteTransform();
				Matrix absoluteTransform = obj.AbsoluteTransform;
				Matrix matrix2 = obj.SkinTransform * absoluteTransform;
				Matrix matrix3 = Matrix.Invert(matrix2);
				v = Vector3.Transform(v, ref matrix3);
				v2 = Vector3.Transform(v2, ref matrix3);
				LineD line2 = new LineD(Vector3D.Transform(v, ref worldMatrix), Vector3D.Transform(v2, ref worldMatrix));
				if (base.GetIntersectionWithLine(ref line2, out MyIntersectionResultLineTriangleEx? t, flags))
				{
					MyIntersectionResultLineTriangleEx triangle = t.Value;
					info.CapsuleIndex = num2;
					info.BoneIndex = num4;
					info.Capsule = m_bodyCapsules[info.CapsuleIndex];
					info.HitHead = (info.CapsuleIndex == 0 && m_bodyCapsules.Length > 1);
					info.HitPositionBindingPose = triangle.IntersectionPointInObjectSpace;
					info.HitNormalBindingPose = triangle.NormalInObjectSpace;
					info.BindingTransformation = matrix2;
					MyTriangle_Vertices triangle2 = default(MyTriangle_Vertices);
					triangle2.Vertex0 = Vector3.Transform(triangle.Triangle.InputTriangle.Vertex0, ref matrix2);
					triangle2.Vertex1 = Vector3.Transform(triangle.Triangle.InputTriangle.Vertex1, ref matrix2);
					triangle2.Vertex2 = Vector3.Transform(triangle.Triangle.InputTriangle.Vertex2, ref matrix2);
					Vector3 triangleNormal = Vector3.TransformNormal(triangle.Triangle.InputTriangleNormal, matrix2);
					MyIntersectionResultLineTriangle triangle3 = new MyIntersectionResultLineTriangle(triangle.Triangle.TriangleIndex, ref triangle2, ref triangle.Triangle.BoneWeights, ref triangleNormal, triangle.Triangle.Distance);
					Vector3 vector = Vector3.Transform(triangle.IntersectionPointInObjectSpace, ref matrix2);
					Vector3 vector2 = Vector3.TransformNormal(triangle.NormalInObjectSpace, matrix2);
					triangle = default(MyIntersectionResultLineTriangleEx);
					triangle.Triangle = triangle3;
					triangle.IntersectionPointInObjectSpace = vector;
					triangle.NormalInObjectSpace = vector2;
					triangle.IntersectionPointInWorldSpace = Vector3D.Transform(vector, ref worldMatrix);
					triangle.NormalInWorldSpace = Vector3.TransformNormal(vector2, worldMatrix);
					triangle.InputLineInObjectSpace = inputLineInObjectSpace;
					triangle.Entity = t.Value.Entity;
					info.Triangle = triangle;
					if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
					{
						MyRenderProxy.DebugClearPersistentMessages();
						MyRenderProxy.DebugDrawCapsule(info.Capsule.P0, info.Capsule.P1, info.Capsule.Radius, Color.Aqua, depthRead: false, shaded: false, persistent: true);
						Vector3 position = Vector3D.Transform(info.Capsule.P0, ref matrix);
						Vector3 position2 = Vector3D.Transform(info.Capsule.P1, ref matrix);
						Vector3 v3 = Vector3.Transform(position, ref matrix3);
						Vector3 v4 = Vector3.Transform(position2, ref matrix3);
						MyRenderProxy.DebugDrawCapsule(Vector3D.Transform(v3, ref worldMatrix), Vector3D.Transform(v4, ref worldMatrix), info.Capsule.Radius, Color.Brown, depthRead: false, shaded: false, persistent: true);
						MyRenderProxy.DebugDrawLine3D(line.From, line.To, Color.Blue, Color.Red, depthRead: false, persistent: true);
						MyRenderProxy.DebugDrawLine3D(line2.From, line2.To, Color.Green, Color.Yellow, depthRead: false, persistent: true);
						MyRenderProxy.DebugDrawSphere(triangle.IntersectionPointInWorldSpace, 0.02f, Color.Red, 1f, depthRead: false, smooth: false, cull: true, persistent: true);
						MyRenderProxy.DebugDrawAxis((MatrixD)matrix2 * base.WorldMatrix, 0.1f, depthRead: false, skipScale: true, persistent: true);
					}
					return true;
				}
			}
			return false;
		}

		private int FindBestBone(int capsuleIndex, ref Vector3D hitPosition, ref MatrixD worldMatrix)
		{
			MyBoneCapsuleInfo myBoneCapsuleInfo = m_bodyCapsuleInfo[capsuleIndex];
			CapsuleD capsuleD = m_bodyCapsules[capsuleIndex];
			MyCharacterBone myCharacterBone = base.AnimationController.CharacterBones[myBoneCapsuleInfo.AscendantBone];
			MyCharacterBone obj = base.AnimationController.CharacterBones[myBoneCapsuleInfo.DescendantBone];
			Vector3D vector = Vector3.Normalize(capsuleD.P0 - capsuleD.P1);
			double num = vector.Length();
			double num2 = Vector3D.Dot(hitPosition - capsuleD.P1, vector) / num;
			int index = obj.Index;
			double num3 = 0.0;
			MyCharacterBone parent = obj.Parent;
			while (!(num2 < num3) && index != myCharacterBone.Index)
			{
				num3 = Vector3D.Dot(Vector3D.Transform(parent.AbsoluteTransform.Translation, ref worldMatrix) - capsuleD.P1, vector) / num;
				index = parent.Index;
				parent = parent.Parent;
				if (parent == null)
				{
					break;
				}
			}
			return index;
		}

		public void BeginShoot(MyShootActionEnum action)
		{
			if (m_currentMovementState == MyCharacterMovementEnum.Died)
			{
				return;
			}
			PerFrameData perFrameData;
			if (m_currentWeapon == null)
			{
				if (action == MyShootActionEnum.SecondaryAction)
				{
					if (!MyControllerHelper.IsControl(ControlContext, MyControlsSpace.BUILD_PLANNER, MyControlStateType.PRESSED))
					{
						UseTerminal();
					}
					return;
				}
				Use();
				m_usingByPrimary = true;
				if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.EntityId))
				{
					perFrameData = default(PerFrameData);
					perFrameData.UseData = new UseData
					{
						Use = true
					};
					PerFrameData data = perFrameData;
					MySessionComponentReplay.Static.ProvideEntityRecordData(base.EntityId, data);
				}
				return;
			}
			MyShootActionEnum? shootingAction = GetShootingAction();
			if (shootingAction.HasValue && action != shootingAction.Value)
			{
				EndShoot(shootingAction.Value);
			}
			if (!m_currentWeapon.EnabledInWorldRules)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.WeaponDisabledInWorldSettings);
				return;
			}
			if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.EntityId))
			{
				perFrameData = default(PerFrameData);
				perFrameData.ShootData = new ShootData
				{
					Begin = true,
					ShootAction = (byte)action
				};
				PerFrameData data2 = perFrameData;
				MySessionComponentReplay.Static.ProvideEntityRecordData(base.EntityId, data2);
			}
			UpdateShootDirection(m_currentWeapon.DirectionToTarget(m_aimedPoint), m_currentWeapon.ShootDirectionUpdateTime);
			BeginShootSync(ShootDirection, action, MyGuiScreenGamePlay.DoubleClickDetected != null && MyGuiScreenGamePlay.DoubleClickDetected[(uint)action]);
		}

		public void OnBeginShoot(MyShootActionEnum action)
		{
			if (ControllerInfo != null && m_currentWeapon != null && !m_isShooting[(uint)action])
			{
				MyGunStatusEnum status = MyGunStatusEnum.OK;
				m_currentWeapon.CanShoot(action, ControllerInfo.ControllingIdentityId, out status);
				_ = 4;
				if (m_shootDoubleClick && status == MyGunStatusEnum.Cooldown)
				{
					status = MyGunStatusEnum.OK;
					m_shootDoubleClick = false;
				}
				m_isShooting[(uint)action] = (status == MyGunStatusEnum.OK);
				if (status != 0 && status != MyGunStatusEnum.Cooldown)
				{
					ShootBeginFailed(action, status);
				}
			}
		}

		private void ShootInternal()
		{
			MyGunStatusEnum status = MyGunStatusEnum.OK;
			MyShootActionEnum? shootingAction = GetShootingAction();
			if (ControllerInfo == null)
			{
				return;
			}
			if (Sync.IsServer)
			{
				m_currentAmmoCount.Value = m_currentWeapon.CurrentAmmunition;
				m_currentMagazineAmmoCount.Value = m_currentWeapon.CurrentMagazineAmmunition;
			}
			else
			{
				m_currentWeapon.CurrentMagazineAmmunition = m_currentMagazineAmmoCount;
				m_currentWeapon.CurrentAmmunition = m_currentAmmoCount;
				MyEngineerToolBase myEngineerToolBase;
				if (MySession.Static.LocalCharacter == this && (myEngineerToolBase = (m_currentWeapon as MyEngineerToolBase)) != null)
				{
					MySlimBlock targetBlock = myEngineerToolBase.GetTargetBlock();
					if (targetBlock != null)
					{
						AimedGrid = targetBlock.CubeGrid.EntityId;
						AimedBlock = targetBlock.Position;
					}
					else
					{
						AimedGrid = 0L;
					}
				}
			}
			m_aimedPoint = GetAimedPointFromCamera();
			UpdateShootDirection(m_currentWeapon.DirectionToTarget(m_aimedPoint), m_currentWeapon.ShootDirectionUpdateTime);
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS)
			{
				float num = 20f;
				MatrixD matrix = GetViewMatrix();
				matrix = MatrixD.Invert(ref matrix);
				MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Forward * num, Color.LightGreen, Color.LightGreen, depthRead: false);
				MyDebugDrawHelper.DrawNamedPoint(matrix.Translation + matrix.Forward * 5.0, "crosshair", Color.LightGreen);
				MyRenderProxy.DebugDrawLine3D(WeaponPosition.LogicalPositionWorld, WeaponPosition.LogicalPositionWorld + ShootDirection * num, Color.Red, Color.Red, depthRead: false);
				MyDebugDrawHelper.DrawNamedPoint(WeaponPosition.LogicalPositionWorld + ShootDirection * 5f, "shootdir", Color.Red);
				MyDebugDrawHelper.DrawNamedPoint(m_aimedPoint, "aimed", Color.White);
			}
			bool flag = shootingAction.HasValue && m_currentWeapon.CanShoot(shootingAction.Value, ControllerInfo.ControllingIdentityId, out status);
			if (m_shootDoubleClick && status == MyGunStatusEnum.Cooldown)
			{
				flag = true;
				status = MyGunStatusEnum.OK;
				m_shootDoubleClick = false;
			}
			if (flag)
			{
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					m_currentWeapon.Shoot(shootingAction.Value, ShootDirection, WeaponPosition.LogicalPositionWorld);
				}
				else
				{
					m_currentWeapon.Shoot(shootingAction.Value, ShootDirection, null);
				}
			}
			if (m_currentWeapon != null)
			{
				if (MySession.Static.ControlledEntity == this)
				{
					if (status != 0 && status != MyGunStatusEnum.Cooldown)
					{
						ShootFailedLocal(shootingAction.Value, status);
					}
					else if (shootingAction.HasValue && m_currentWeapon.IsShooting && status == MyGunStatusEnum.OK)
					{
						ShootSuccessfulLocal(shootingAction.Value);
					}
				}
				if (status != 0 && status != MyGunStatusEnum.Cooldown)
				{
					m_isShooting[(uint)shootingAction.Value] = false;
				}
			}
			if (m_autoswitch.HasValue)
			{
				SwitchToWeapon(m_autoswitch);
				m_autoswitch = null;
			}
		}

		private void ShootFailedLocal(MyShootActionEnum action, MyGunStatusEnum status)
		{
			if (status == MyGunStatusEnum.OutOfAmmo)
			{
				ShowOutOfAmmoNotification();
			}
			m_currentWeapon.ShootFailReactionLocal(action, status);
		}

		private void ShootBeginFailed(MyShootActionEnum action, MyGunStatusEnum status)
		{
			m_currentWeapon.BeginFailReaction(action, status);
			m_isShooting[(uint)action] = false;
			if (MySession.Static.ControlledEntity == this)
			{
				m_currentWeapon.BeginFailReactionLocal(action, status);
			}
		}

		private void ShootSuccessfulLocal(MyShootActionEnum action)
		{
			m_currentShotTime = 0.1f;
			WeaponPosition.AddBackkick(m_currentWeapon.BackkickForcePerSecond * 0.0166666675f);
			_ = JetpackComp;
			if (m_currentWeapon.BackkickForcePerSecond > 0f && (JetpackRunning || m_isFalling))
			{
				Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, (0f - m_currentWeapon.BackkickForcePerSecond) * (Vector3)(m_currentWeapon as MyEntity).WorldMatrix.Forward, (Vector3)base.PositionComp.GetPosition(), null);
			}
		}

		public void SetupAutoswitch(MyDefinitionId? switchToNow, MyDefinitionId? switchOnEndShoot)
		{
			m_autoswitch = switchToNow;
			m_endShootAutoswitch = switchOnEndShoot;
		}

		private void EndShootAll()
		{
			MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
			foreach (MyShootActionEnum action in values)
			{
				if (IsShooting(action))
				{
					EndShoot(action);
				}
			}
		}

		public void EndShoot(MyShootActionEnum action)
		{
			if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.EntityId))
			{
				PerFrameData perFrameData = default(PerFrameData);
				perFrameData.ShootData = new ShootData
				{
					Begin = false,
					ShootAction = (byte)action
				};
				PerFrameData data = perFrameData;
				MySessionComponentReplay.Static.ProvideEntityRecordData(base.EntityId, data);
			}
			if (MySession.Static.LocalCharacter == this && m_currentMovementState != MyCharacterMovementEnum.Died && m_currentWeapon != null)
			{
				if (MyGuiScreenGamePlay.DoubleClickDetected != null && MyGuiScreenGamePlay.DoubleClickDetected[(uint)action] && m_currentWeapon.CanDoubleClickToStick(action))
				{
					GunDoubleClickedSync(action);
				}
				else
				{
					EndShootSync(action);
				}
			}
			if (m_usingByPrimary)
			{
				m_usingByPrimary = false;
				UseFinished();
			}
		}

		public void OnEndShoot(MyShootActionEnum action)
		{
			if (m_currentMovementState != MyCharacterMovementEnum.Died && m_currentWeapon != null)
			{
				m_currentWeapon.EndShoot(action);
				if (m_endShootAutoswitch.HasValue)
				{
					SwitchToWeapon(m_endShootAutoswitch);
					m_endShootAutoswitch = null;
				}
			}
		}

		public void OnGunDoubleClicked(MyShootActionEnum action)
		{
			if (m_currentMovementState != MyCharacterMovementEnum.Died && m_currentWeapon != null)
			{
				m_currentWeapon.DoubleClicked(action);
			}
		}

		public bool ShouldEndShootingOnPause(MyShootActionEnum action)
		{
			if (m_currentMovementState != MyCharacterMovementEnum.Died && m_currentWeapon != null)
			{
				return m_currentWeapon.ShouldEndShootOnPause(action);
			}
			return true;
		}

		public void Zoom(bool newKeyPress, bool hideCrosshairWhenAiming = true)
		{
			switch (m_zoomMode)
			{
			case MyZoomModeEnum.Classic:
				if (Definition.CanIronsight && m_currentWeapon != null && (MySession.Static.CameraController == this || !ControllerInfo.IsLocallyControlled()))
				{
					if (!IsInFirstPersonView)
					{
						MyGuiScreenGamePlay.Static.SwitchCamera();
						m_wasInThirdPersonBeforeIronSight = true;
					}
					SoundComp.PlaySecondarySound(CharacterSoundsEnum.IRONSIGHT_ACT_SOUND, stopPrevious: true);
					EnableIronsight(enable: true, newKeyPress, changeCamera: true, hideCrosshairWhenAiming);
				}
				break;
			case MyZoomModeEnum.IronSight:
				if (MySession.Static.CameraController == this || !ControllerInfo.IsLocallyControlled())
				{
					SoundComp.PlaySecondarySound(CharacterSoundsEnum.IRONSIGHT_DEACT_SOUND, stopPrevious: true);
					EnableIronsight(enable: false, newKeyPress, changeCamera: true);
					if (m_wasInThirdPersonBeforeIronSight)
					{
						MyGuiScreenGamePlay.Static.SwitchCamera();
					}
				}
				break;
			}
		}

		public void EnableIronsight(bool enable, bool newKeyPress, bool changeCamera, bool hideCrosshairWhenAiming = true)
		{
			if (Sync.IsServer || MySession.Static.LocalCharacter == this)
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.EnableIronsightCallback, enable, newKeyPress, changeCamera, hideCrosshairWhenAiming, arg6: false);
			}
			if (!Sync.IsServer)
			{
				EnableIronsightCallback(enable, newKeyPress, changeCamera, hideCrosshairWhenAiming, forceChangeCamera: true);
			}
		}

		[Event(null, 5793)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		public void EnableIronsightCallback(bool enable, bool newKeyPress, bool changeCamera, bool hideCrosshairWhenAiming = true, bool forceChangeCamera = false)
		{
			if (enable)
			{
				if (m_currentWeapon == null || m_zoomMode == MyZoomModeEnum.IronSight)
				{
					return;
				}
				m_zoomMode = MyZoomModeEnum.IronSight;
				if (changeCamera && (MyEventContext.Current.IsLocallyInvoked || forceChangeCamera))
				{
					float headLocalXAngle = m_headLocalXAngle;
					float headLocalYAngle = m_headLocalYAngle;
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
					m_headLocalXAngle = headLocalXAngle;
					m_headLocalYAngle = headLocalYAngle;
					if (hideCrosshairWhenAiming)
					{
						MyHud.Crosshair.HideDefaultSprite();
					}
					MySector.MainCamera.Zoom.SetZoom(MyCameraZoomOperationType.ZoomingIn);
				}
			}
			else
			{
				m_zoomMode = MyZoomModeEnum.Classic;
				ForceFirstPersonCamera = false;
				if (changeCamera && (MyEventContext.Current.IsLocallyInvoked || forceChangeCamera))
				{
					MyHud.Crosshair.ResetToDefault();
					MySector.MainCamera.Zoom.SetZoom(MyCameraZoomOperationType.ZoomingOut);
					float headLocalXAngle2 = m_headLocalXAngle;
					float headLocalYAngle2 = m_headLocalYAngle;
					m_headLocalXAngle = headLocalXAngle2;
					m_headLocalYAngle = headLocalYAngle2;
				}
			}
		}

		private void SwitchCameraIronSightChanges()
		{
			m_wasInThirdPersonBeforeIronSight = false;
			if (m_zoomMode == MyZoomModeEnum.IronSight)
			{
				if (m_isInFirstPersonView)
				{
					MyHud.Crosshair.HideDefaultSprite();
				}
				else
				{
					MyHud.Crosshair.ResetToDefault();
				}
			}
		}

		public static IMyHandheldGunObject<MyDeviceBase> CreateGun(MyObjectBuilder_EntityBase gunEntity, uint? inventoryItemId = null)
		{
			if (gunEntity != null)
			{
				MyEntity myEntity = MyEntityFactory.CreateEntity(gunEntity);
				try
				{
					myEntity.Init(gunEntity);
				}
				catch (Exception)
				{
					return null;
				}
				IMyHandheldGunObject<MyDeviceBase> myHandheldGunObject = (IMyHandheldGunObject<MyDeviceBase>)myEntity;
				if (myHandheldGunObject != null && myHandheldGunObject.GunBase != null && !myHandheldGunObject.GunBase.InventoryItemId.HasValue && inventoryItemId.HasValue)
				{
					myHandheldGunObject.GunBase.InventoryItemId = inventoryItemId.Value;
				}
				return myHandheldGunObject;
			}
			return null;
		}

		public MyPhysicalInventoryItem? FindWeaponItemByDefinition(MyDefinitionId weaponDefinition)
		{
			MyPhysicalInventoryItem? result = null;
			MyDefinitionId? myDefinitionId = MyDefinitionManager.Static.ItemIdFromWeaponId(weaponDefinition);
			if (myDefinitionId.HasValue && this.GetInventory() != null)
			{
				return this.GetInventory().FindUsableItem(myDefinitionId.Value);
			}
			return result;
		}

		private void SaveAmmoToWeapon()
		{
		}

		public bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition)
		{
			if (weaponDefinition.HasValue && weaponDefinition.Value.TypeId == typeof(MyObjectBuilder_CubePlacer) && !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Building, 0L, 0uL))
			{
				return false;
			}
			if (IsOnLadder)
			{
				return false;
			}
			if (!WeaponTakesBuilderFromInventory(weaponDefinition))
			{
				return true;
			}
			if (FindWeaponItemByDefinition(weaponDefinition.Value).HasValue)
			{
				return true;
			}
			return false;
		}

		public bool WeaponTakesBuilderFromInventory(MyDefinitionId? weaponDefinition)
		{
			if (!weaponDefinition.HasValue)
			{
				return false;
			}
			if (weaponDefinition.Value.TypeId == typeof(MyObjectBuilder_CubePlacer) || (weaponDefinition.Value.TypeId == typeof(MyObjectBuilder_PhysicalGunObject) && weaponDefinition.Value.SubtypeId == manipulationToolId))
			{
				return false;
			}
			if (!MySession.Static.CreativeMode)
			{
				return !MyFakes.ENABLE_SURVIVAL_SWITCHING;
			}
			return false;
		}

		public void SwitchToWeapon(MyDefinitionId weaponDefinition)
		{
			SwitchToWeapon(weaponDefinition, sync: true);
		}

		public void SwitchAmmoMagazine()
		{
			SwitchAmmoMagazineInternal(sync: true);
		}

		public bool CanSwitchAmmoMagazine()
		{
			if (m_currentWeapon != null && m_currentWeapon.GunBase != null)
			{
				return m_currentWeapon.GunBase.CanSwitchAmmoMagazine();
			}
			return false;
		}

		private void SwitchAmmoMagazineInternal(bool sync)
		{
			if (sync)
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnSwitchAmmoMagazineRequest);
			}
			else if (!IsDead && CurrentWeapon != null)
			{
				CurrentWeapon.GunBase.SwitchAmmoMagazineToNextAvailable();
			}
		}

		private void SwitchAmmoMagazineSuccess()
		{
			SwitchAmmoMagazineInternal(sync: false);
		}

		public void SwitchToWeapon(MyDefinitionId? weaponDefinition, bool sync = true)
		{
			if (weaponDefinition.HasValue && m_rightHandItemBone == -1)
			{
				return;
			}
			if (WeaponTakesBuilderFromInventory(weaponDefinition))
			{
				MyPhysicalInventoryItem? myPhysicalInventoryItem = FindWeaponItemByDefinition(weaponDefinition.Value);
				if (myPhysicalInventoryItem.HasValue)
				{
					if (myPhysicalInventoryItem.Value.Content == null)
					{
						MySandboxGame.Log.WriteLine("item.Value.Content was null in MyCharacter.SwitchToWeapon");
						MySandboxGame.Log.WriteLine("item.Value = " + myPhysicalInventoryItem.Value);
						MySandboxGame.Log.WriteLine("weaponDefinition.Value = " + weaponDefinition);
					}
					else
					{
						SwitchToWeaponInternal(weaponDefinition, sync, myPhysicalInventoryItem.Value.ItemId, 0L);
					}
				}
			}
			else
			{
				SwitchToWeaponInternal(weaponDefinition, sync, null, 0L);
			}
		}

		public void SwitchToWeapon(MyToolbarItemWeapon weapon)
		{
			SwitchToWeapon(weapon, null);
		}

		public void SwitchToWeapon(MyToolbarItemWeapon weapon, uint? inventoryItemId, bool sync = true)
		{
			MyDefinitionId? myDefinitionId = null;
			if (weapon != null)
			{
				myDefinitionId = weapon.Definition.Id;
			}
			if (myDefinitionId.HasValue && m_rightHandItemBone == -1)
			{
				return;
			}
			if (WeaponTakesBuilderFromInventory(myDefinitionId))
			{
				MyPhysicalInventoryItem? myPhysicalInventoryItem = null;
				myPhysicalInventoryItem = ((!inventoryItemId.HasValue) ? FindWeaponItemByDefinition(myDefinitionId.Value) : this.GetInventory().GetItemByID(inventoryItemId.Value));
				if (myPhysicalInventoryItem.HasValue)
				{
					if (myPhysicalInventoryItem.Value.Content == null)
					{
						MySandboxGame.Log.WriteLine("item.Value.Content was null in MyCharacter.SwitchToWeapon");
						MySandboxGame.Log.WriteLine("item.Value = " + myPhysicalInventoryItem.Value);
						MySandboxGame.Log.WriteLine("weaponDefinition.Value = " + myDefinitionId);
					}
					else
					{
						SwitchToWeaponInternal(myDefinitionId, sync, myPhysicalInventoryItem.Value.ItemId, 0L);
					}
				}
			}
			else
			{
				SwitchToWeaponInternal(myDefinitionId, sync, null, 0L);
			}
		}

		private void SwitchToWeaponInternal(MyDefinitionId? weaponDefinition, bool updateSync, uint? inventoryItemId, long weaponEntityId)
		{
			if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.EntityId))
			{
				PerFrameData perFrameData = default(PerFrameData);
				perFrameData.SwitchWeaponData = new SwitchWeaponData
				{
					WeaponDefinition = weaponDefinition,
					InventoryItemId = inventoryItemId,
					WeaponEntityId = weaponEntityId
				};
				PerFrameData data = perFrameData;
				MySessionComponentReplay.Static.ProvideEntityRecordData(base.EntityId, data);
			}
			if (updateSync)
			{
				UnequipWeapon();
				RequestSwitchToWeapon(weaponDefinition, inventoryItemId);
				return;
			}
			UnequipWeapon();
			StopCurrentWeaponShooting();
			if (weaponDefinition.HasValue && weaponDefinition.Value.TypeId != MyObjectBuilderType.Invalid)
			{
				IMyHandheldGunObject<MyDeviceBase> newWeapon = CreateGun(GetObjectBuilderForWeapon(weaponDefinition, ref inventoryItemId, weaponEntityId), inventoryItemId);
				EquipWeapon(newWeapon);
				UpdateShadowIgnoredObjects();
			}
		}

		private MyObjectBuilder_EntityBase GetObjectBuilderForWeapon(MyDefinitionId? weaponDefinition, ref uint? inventoryItemId, long weaponEntityId)
		{
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = null;
			if (inventoryItemId.HasValue && (Sync.IsServer || ControllerInfo.IsLocallyControlled()))
			{
				MyPhysicalInventoryItem? itemByID = this.GetInventory().GetItemByID(inventoryItemId.Value);
				if (itemByID.HasValue)
				{
					MyObjectBuilder_PhysicalGunObject myObjectBuilder_PhysicalGunObject = itemByID.Value.Content as MyObjectBuilder_PhysicalGunObject;
					if (myObjectBuilder_PhysicalGunObject != null)
					{
						myObjectBuilder_EntityBase = myObjectBuilder_PhysicalGunObject.GunEntity;
					}
					if (myObjectBuilder_EntityBase == null)
					{
						MyHandItemDefinition myHandItemDefinition = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(myObjectBuilder_PhysicalGunObject.GetId());
						if (myHandItemDefinition != null)
						{
							myObjectBuilder_EntityBase = (MyObjectBuilder_EntityBase)MyObjectBuilderSerializer.CreateNewObject(myHandItemDefinition.Id);
							myObjectBuilder_EntityBase.EntityId = weaponEntityId;
						}
					}
					else
					{
						myObjectBuilder_EntityBase.EntityId = weaponEntityId;
					}
					if (myObjectBuilder_PhysicalGunObject != null)
					{
						myObjectBuilder_PhysicalGunObject.GunEntity = myObjectBuilder_EntityBase;
					}
				}
			}
			else
			{
				bool flag = (!Sync.IsServer && ControllerInfo.IsRemotelyControlled()) || !WeaponTakesBuilderFromInventory(weaponDefinition);
				if (!weaponDefinition.HasValue)
				{
					EquipWeapon(null);
				}
				else if (flag && weaponDefinition.Value.TypeId == typeof(MyObjectBuilder_PhysicalGunObject))
				{
					MyHandItemDefinition myHandItemDefinition2 = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(weaponDefinition.Value);
					if (myHandItemDefinition2 != null)
					{
						myObjectBuilder_EntityBase = (MyObjectBuilder_EntityBase)MyObjectBuilderSerializer.CreateNewObject(myHandItemDefinition2.Id);
						myObjectBuilder_EntityBase.EntityId = weaponEntityId;
					}
				}
				else
				{
					myObjectBuilder_EntityBase = (MyObjectBuilderSerializer.CreateNewObject(weaponDefinition.Value.TypeId, weaponDefinition.Value.SubtypeName) as MyObjectBuilder_EntityBase);
					if (myObjectBuilder_EntityBase != null)
					{
						myObjectBuilder_EntityBase.EntityId = weaponEntityId;
						if (WeaponTakesBuilderFromInventory(weaponDefinition))
						{
							MyPhysicalInventoryItem? myPhysicalInventoryItem = FindWeaponItemByDefinition(weaponDefinition.Value);
							if (myPhysicalInventoryItem.HasValue)
							{
								MyObjectBuilder_PhysicalGunObject myObjectBuilder_PhysicalGunObject2 = myPhysicalInventoryItem.Value.Content as MyObjectBuilder_PhysicalGunObject;
								if (myObjectBuilder_PhysicalGunObject2 != null)
								{
									myObjectBuilder_PhysicalGunObject2.GunEntity = myObjectBuilder_EntityBase;
								}
								inventoryItemId = myPhysicalInventoryItem.Value.ItemId;
							}
						}
					}
				}
			}
			if (myObjectBuilder_EntityBase != null)
			{
				IMyObjectBuilder_GunObject<MyObjectBuilder_DeviceBase> myObjectBuilder_GunObject = myObjectBuilder_EntityBase as IMyObjectBuilder_GunObject<MyObjectBuilder_DeviceBase>;
				if (myObjectBuilder_GunObject != null && myObjectBuilder_GunObject.DeviceBase != null)
				{
					myObjectBuilder_GunObject.DeviceBase.InventoryItemId = inventoryItemId;
				}
			}
			return myObjectBuilder_EntityBase;
		}

		private void StopCurrentWeaponShooting()
		{
			if (m_currentWeapon == null)
			{
				return;
			}
			MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
			foreach (MyShootActionEnum action in values)
			{
				if (IsShooting(action))
				{
					m_currentWeapon.EndShoot(action);
				}
			}
		}

		private void UpdateShadowIgnoredObjects()
		{
			if (Render != null)
			{
				Render.UpdateShadowIgnoredObjects();
				if (m_currentWeapon != null)
				{
					UpdateShadowIgnoredObjects((MyEntity)m_currentWeapon);
				}
				if (m_leftHandItem != null)
				{
					UpdateShadowIgnoredObjects(m_leftHandItem);
				}
			}
		}

		private void UpdateShadowIgnoredObjects(VRage.ModAPI.IMyEntity parent)
		{
			Render.UpdateShadowIgnoredObjects(parent);
			foreach (MyHierarchyComponentBase child in parent.Hierarchy.Children)
			{
				UpdateShadowIgnoredObjects(child.Container.Entity);
			}
		}

		public void Use()
		{
			if (IsOnLadder)
			{
				if (GetCurrentMovementState() != MyCharacterMovementEnum.LadderOut)
				{
					Vector3D pos = base.PositionComp.GetPosition() + m_ladder.WorldMatrix.Forward * 1.2000000476837158;
					GetOffLadder();
					base.PositionComp.SetPosition(pos);
				}
			}
			else
			{
				if (IsDead)
				{
					return;
				}
				MyCharacterDetectorComponent myCharacterDetectorComponent = base.Components.Get<MyCharacterDetectorComponent>();
				if (myCharacterDetectorComponent != null && myCharacterDetectorComponent.UseObject != null)
				{
					if (myCharacterDetectorComponent.UseObject.PrimaryAction != 0)
					{
						if (myCharacterDetectorComponent.UseObject.PlayIndicatorSound)
						{
							MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
							SoundComp.StopStateSound();
						}
						myCharacterDetectorComponent.RaiseObjectUsed();
						myCharacterDetectorComponent.UseObject.Use(myCharacterDetectorComponent.UseObject.PrimaryAction, this);
					}
					else if (myCharacterDetectorComponent.UseObject.SecondaryAction != 0)
					{
						if (myCharacterDetectorComponent.UseObject.PlayIndicatorSound)
						{
							MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
							SoundComp.StopStateSound();
						}
						myCharacterDetectorComponent.RaiseObjectUsed();
						myCharacterDetectorComponent.UseObject.Use(myCharacterDetectorComponent.UseObject.SecondaryAction, this);
					}
					return;
				}
				MyEntity myEntity = myCharacterDetectorComponent.DetectedEntity as MyEntity;
				if (myEntity != null && (!(myEntity is MyCharacter) || (myEntity as MyCharacter).IsDead))
				{
					MyInventoryBase inventoryBase = null;
					if (myEntity.TryGetInventory(out inventoryBase))
					{
						ShowAggregateInventoryScreen(inventoryBase);
					}
				}
			}
		}

		public void UseContinues()
		{
			if (!IsDead)
			{
				MyCharacterDetectorComponent myCharacterDetectorComponent = base.Components.Get<MyCharacterDetectorComponent>();
				if (myCharacterDetectorComponent != null && myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.Manipulate) && myCharacterDetectorComponent.UseObject.ContinuousUsage)
				{
					myCharacterDetectorComponent.UseObject.Use(UseActionEnum.Manipulate, this);
				}
			}
		}

		public void UseTerminal()
		{
			if (IsDead)
			{
				return;
			}
			MyCharacterDetectorComponent myCharacterDetectorComponent = base.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.OpenTerminal))
			{
				if (myCharacterDetectorComponent.UseObject.PlayIndicatorSound)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
					SoundComp.StopStateSound();
				}
				myCharacterDetectorComponent.UseObject.Use(UseActionEnum.OpenTerminal, this);
				myCharacterDetectorComponent.UseContinues();
			}
		}

		public void UseFinished()
		{
			if (!IsDead)
			{
				MyCharacterDetectorComponent myCharacterDetectorComponent = base.Components.Get<MyCharacterDetectorComponent>();
				if (myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.UseFinished))
				{
					myCharacterDetectorComponent.UseObject.Use(UseActionEnum.UseFinished, this);
				}
			}
		}

		public void PickUp()
		{
			if (!IsDead)
			{
				base.Components.Get<MyCharacterPickupComponent>()?.PickUp();
			}
		}

		public void PickUpContinues()
		{
			if (!IsDead)
			{
				base.Components.Get<MyCharacterPickupComponent>()?.PickUpContinues();
			}
		}

		public void PickUpFinished()
		{
			if (!IsDead)
			{
				base.Components.Get<MyCharacterPickupComponent>()?.PickUpFinished();
			}
		}

		private bool HasEnoughSpaceToStandUp()
		{
			if (!IsCrouching)
			{
				return true;
			}
			Vector3D vector3D = base.WorldMatrix.Translation + Definition.CharacterCollisionCrouchHeight * base.WorldMatrix.Up;
			float num = Definition.CharacterCollisionHeight - Definition.CharacterCollisionCrouchHeight;
			if (MyPhysics.CastRay(vector3D, vector3D + num * base.WorldMatrix.Up, 18).HasValue)
			{
				return false;
			}
			return true;
		}

		public void Crouch()
		{
			if (!IsDead && Definition.CanCrouch && !JetpackRunning && !m_isFalling && HasEnoughSpaceToStandUp())
			{
				WantsCrouch = !WantsCrouch;
			}
		}

		public void Down()
		{
			if (WantsFlyUp)
			{
				WantsFlyDown = false;
				WantsFlyUp = false;
			}
			else
			{
				WantsFlyDown = true;
			}
		}

		public void Up()
		{
			if (WantsFlyDown)
			{
				WantsFlyUp = false;
				WantsFlyDown = false;
			}
			else
			{
				WantsFlyUp = true;
			}
		}

		public void Sprint(bool enabled)
		{
			if (WantsSprint != enabled)
			{
				WantsSprint = enabled;
			}
			if (WantsSprint && m_zoomMode == MyZoomModeEnum.IronSight)
			{
				EnableIronsight(enable: false, newKeyPress: false, changeCamera: true);
				if (m_wasInThirdPersonBeforeIronSight)
				{
					MyGuiScreenGamePlay.Static.SwitchCamera();
				}
			}
		}

		public void SwitchWalk()
		{
			WantsWalk = !WantsWalk;
		}

		[Event(null, 6439)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		public void Jump(Vector3 moveIndicator)
		{
			if (m_currentMovementState == MyCharacterMovementEnum.Died || !HasEnoughSpaceToStandUp())
			{
				return;
			}
			if (StatComp != null && !StatComp.CanDoAction("Jump", out MyTuple<ushort, MyStringHash> message, m_currentMovementState == MyCharacterMovementEnum.Jump))
			{
				if (MySession.Static != null && MySession.Static.LocalCharacter == this && message.Item1 == 4 && message.Item2.String.CompareTo("Stamina") == 0 && m_notEnoughStatNotification != null)
				{
					m_notEnoughStatNotification.SetTextFormatArguments(message.Item2);
					MyHud.Notifications.Add(m_notEnoughStatNotification);
				}
			}
			else if (IsMagneticBootsActive)
			{
				if (Sync.IsServer || IsClientPredicted)
				{
					Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, 1000f * Physics.SupportNormal, null, null);
				}
				StartFalling();
			}
			else if (IsOnLadder)
			{
				if (Sync.IsServer)
				{
					GetOffLadder();
				}
				else
				{
					GetOffLadder_Implementation();
				}
				if (Sync.IsServer || IsClientPredicted)
				{
					Vector3 jumpDirection = base.WorldMatrix.Backward;
					if (MoveIndicator.X > 0f)
					{
						jumpDirection = base.WorldMatrix.Right;
					}
					else if (MoveIndicator.X < 0f)
					{
						jumpDirection = base.WorldMatrix.Left;
					}
					MySandboxGame.Static.Invoke(delegate
					{
						Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, 1000f * jumpDirection, null, null);
					}, "Ladder jump");
				}
			}
			else
			{
				WantsJump = true;
			}
		}

		public void ShowInventory()
		{
			if (m_currentMovementState == MyCharacterMovementEnum.Died)
			{
				return;
			}
			MyCharacterDetectorComponent myCharacterDetectorComponent = base.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.OpenInventory))
			{
				myCharacterDetectorComponent.UseObject.Use(UseActionEnum.OpenInventory, this);
			}
			else if (MyPerGameSettings.TerminalEnabled)
			{
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, this, null);
			}
			else if (base.HasInventory)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory != null)
				{
					ShowAggregateInventoryScreen(inventory);
				}
			}
		}

		public MyGuiScreenBase ShowAggregateInventoryScreen(MyInventoryBase rightSelectedInventory = null)
		{
			if (MyPerGameSettings.GUI.InventoryScreen != null && InventoryAggregate != null)
			{
				InventoryAggregate.Init();
				m_InventoryScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.InventoryScreen, InventoryAggregate, rightSelectedInventory);
				MyGuiSandbox.AddScreen(m_InventoryScreen);
				m_InventoryScreen.Closed += delegate
				{
					if (InventoryAggregate != null)
					{
						InventoryAggregate.DetachCallbacks();
					}
					m_InventoryScreen = null;
				};
			}
			return m_InventoryScreen;
		}

		public void ShowTerminal()
		{
			if (m_currentMovementState == MyCharacterMovementEnum.Died)
			{
				return;
			}
			MyCharacterDetectorComponent myCharacterDetectorComponent = base.Components.Get<MyCharacterDetectorComponent>();
			if (MyToolbarComponent.CharacterToolbar == null || !(MyToolbarComponent.CharacterToolbar.SelectedItem is MyToolbarItemVoxelHand))
			{
				if (myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.OpenTerminal))
				{
					myCharacterDetectorComponent.UseObject.Use(UseActionEnum.OpenTerminal, this);
				}
				else if (MyPerGameSettings.TerminalEnabled)
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, this, null);
				}
				else if (MyFakes.ENABLE_QUICK_WARDROBE)
				{
					MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenWardrobe(this));
				}
				else if (MyPerGameSettings.GUI.GameplayOptionsScreen != null && !MySession.Static.SurvivalMode)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.GameplayOptionsScreen));
				}
			}
		}

		public void SwitchLights()
		{
			if (m_currentMovementState != MyCharacterMovementEnum.Died)
			{
				EnableLights(!LightEnabled);
				RecalculatePowerRequirement();
			}
		}

		public void SwitchReactors()
		{
		}

		public void SwitchBroadcasting()
		{
			if (m_currentMovementState != MyCharacterMovementEnum.Died)
			{
				EnableBroadcasting(!RadioBroadcaster.WantsToBeEnabled);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
			if (highlightSystem != null)
			{
				Render.RenderObjectIDs.ForEach(delegate(uint id)
				{
					highlightSystem.RemoveHighlightOverlappingModel(id);
				});
			}
			base.OnRemovedFromScene(source);
			if (m_currentWeapon != null)
			{
				if (highlightSystem != null)
				{
					((MyEntity)m_currentWeapon).Render.RenderObjectIDs.ForEach(delegate(uint id)
					{
						highlightSystem.RemoveHighlightOverlappingModel(id);
					});
				}
				((MyEntity)m_currentWeapon).OnRemovedFromScene(source);
			}
			if (m_leftHandItem != null)
			{
				m_leftHandItem.OnRemovedFromScene(source);
			}
			m_resolveHighlightOverlap = true;
		}

		public void RemoveNotification(ref MyHudNotification notification)
		{
			if (notification != null)
			{
				MyHud.Notifications.Remove(notification);
				notification = null;
			}
		}

		private void RemoveNotifications()
		{
			RemoveNotification(ref m_pickupObjectNotification);
			RemoveNotification(ref m_respawnNotification);
		}

		private void OnControlAcquired(MyEntityController controller)
		{
			MyPlayer player = controller.Player;
			m_idModule.Owner = player.Identity.IdentityId;
			SetPlayer(controller.Player);
			if (MyMultiplayer.Static != null && Sync.IsServer)
			{
				IsPersistenceCharacter = true;
			}
			if (player.IsLocalPlayer)
			{
				if (player == MySession.Static.LocalHumanPlayer)
				{
					MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
					if (highlightSystem != null)
					{
						Render.RenderObjectIDs.ForEach(delegate(uint id)
						{
							highlightSystem.AddHighlightOverlappingModel(id);
						});
						m_resolveHighlightOverlap = false;
					}
					MyHud.SetHudDefinition(Definition.HUD);
					MyHud.HideAll();
					MyHud.Crosshair.ResetToDefault();
					MyHud.Crosshair.Recenter();
					if (MyGuiScreenGamePlay.Static != null)
					{
						MySession.Static.CameraAttachedToChanged += Static_CameraAttachedToChanged;
					}
					if (MySession.Static.CameraController is MyEntity && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
					{
						MySession.Static.SetCameraController(IsInFirstPersonView ? MyCameraControllerEnum.Entity : MyCameraControllerEnum.ThirdPersonSpectator, this);
					}
					MyHud.GravityIndicator.Entity = this;
					MyHud.GravityIndicator.Show(null);
					MyHud.OreMarkers.Visible = true;
					MyHud.LargeTurretTargets.Visible = true;
					if (MySession.Static.IsScenario)
					{
						MyHud.ScenarioInfo.Show(null);
					}
				}
				MyCharacterJetpackComponent jetpackComp = JetpackComp;
				jetpackComp?.TurnOnJetpack(jetpackComp.TurnedOn);
				m_suitBattery.OwnedByLocalPlayer = true;
				base.DisplayName = player.Identity.DisplayName;
			}
			else
			{
				base.DisplayName = player.Identity.DisplayName;
				UpdateHudMarker();
			}
			if (StatComp != null && StatComp.Health != null && StatComp.Health.Value <= 0f)
			{
				m_dieAfterSimulation = true;
				return;
			}
			if (m_currentWeapon != null)
			{
				m_currentWeapon.OnControlAcquired(this);
			}
			UpdateCharacterPhysics();
			if (this == MySession.Static.ControlledEntity && MyToolbarComponent.CharacterToolbar != null)
			{
				MyToolbarComponent.CharacterToolbar.ItemChanged -= Toolbar_ItemChanged;
				MyToolbarComponent.CharacterToolbar.ItemChanged += Toolbar_ItemChanged;
			}
		}

		private void UpdateHudMarker()
		{
			if (!MyFakes.ENABLE_RADIO_HUD)
			{
				MyHud.LocationMarkers.RegisterMarker(this, new MyHudEntityParams
				{
					FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_TEXT,
					Text = new StringBuilder(GetIdentity().DisplayName),
					ShouldDraw = MyHud.CheckShowPlayerNamesOnHud
				});
			}
		}

		public StringBuilder UpdateCustomNameWithFaction()
		{
			CustomNameWithFaction.Clear();
			MyIdentity identity = GetIdentity();
			if (identity == null)
			{
				CustomNameWithFaction.Append(base.DisplayName);
			}
			else
			{
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(identity.IdentityId);
				if (myFaction != null)
				{
					CustomNameWithFaction.Append(myFaction.Tag);
					CustomNameWithFaction.Append('.');
				}
				CustomNameWithFaction.Append(identity.DisplayName);
			}
			return CustomNameWithFaction;
		}

		internal void ClearShapeContactPoints()
		{
			m_shapeContactPoints.Clear();
		}

		public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			UpdateCustomNameWithFaction();
			m_hudParams.Clear();
			if (MySession.Static.LocalHumanPlayer == null)
			{
				return m_hudParams;
			}
			m_hudParams.Add(new MyHudEntityParams
			{
				FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_TEXT,
				Text = CustomNameWithFaction,
				ShouldDraw = MyHud.CheckShowPlayerNamesOnHud,
				Owner = GetPlayerIdentityId(),
				Share = MyOwnershipShareModeEnum.Faction,
				Entity = this
			});
			return m_hudParams;
		}

		private void OnControlReleased(MyEntityController controller)
		{
			Static_CameraAttachedToChanged(null, null);
			if (MySession.Static.LocalHumanPlayer == controller.Player)
			{
				MyHud.SelectedObjectHighlight.RemoveHighlight();
				RemoveNotifications();
				if (MyGuiScreenGamePlay.Static != null)
				{
					MySession.Static.CameraAttachedToChanged -= Static_CameraAttachedToChanged;
				}
				MyHud.GravityIndicator.Hide();
				m_suitBattery.OwnedByLocalPlayer = false;
				MyHud.LargeTurretTargets.Visible = false;
				MyHud.OreMarkers.Visible = false;
				RadioReceiver.Clear();
				if (MyGuiScreenGamePlay.ActiveGameplayScreen != null)
				{
					MyGuiScreenGamePlay.ActiveGameplayScreen.CloseScreen();
				}
				ResetMovement();
				MyCubeBuilder.Static.Deactivate();
			}
			else if (!MyFakes.ENABLE_RADIO_HUD)
			{
				MyHud.LocationMarkers.UnregisterMarker(this);
			}
			SoundComp.StopStateSound();
			MyToolbarComponent.CharacterToolbar.ItemChanged -= Toolbar_ItemChanged;
		}

		private void Static_CameraAttachedToChanged(IMyCameraController oldController, IMyCameraController newController)
		{
			if (!base.Closed)
			{
				if (oldController != newController && MySession.Static.ControlledEntity == this && newController != this)
				{
					ResetMovement();
					EndShootAll();
				}
				UpdateNearFlag();
				if ((Render.NearFlag || MySector.MainCamera == null) && oldController != newController)
				{
					ResetHeadRotation();
				}
			}
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
		}

		public void ResetHeadRotation()
		{
			if (m_actualUpdateFrame != 0)
			{
				m_headLocalYAngle = 0f;
				m_headLocalXAngle = 0f;
			}
		}

		private void UpdateNearFlag()
		{
			bool flag = ControllerInfo.IsLocallyControlled() && MySession.Static.CameraController == this && (m_isInFirstPerson || ForceFirstPersonCamera) && !IsOnLadder;
			flag &= (CurrentMovementState != MyCharacterMovementEnum.Sitting);
			if (m_currentWeapon != null)
			{
				((MyEntity)m_currentWeapon).Render.NearFlag = flag;
			}
			if (m_leftHandItem != null)
			{
				m_leftHandItem.Render.NearFlag = flag;
			}
			Render.NearFlag = flag;
			m_bobQueue.Clear();
		}

		private void WorldPositionChanged(object source)
		{
			if (RadioBroadcaster != null)
			{
				RadioBroadcaster.MoveBroadcaster();
			}
			Render.UpdateLightPosition();
		}

		private void OnCharacterStateChanged(HkCharacterStateType newState)
		{
			if (m_currentCharacterState == newState)
			{
				return;
			}
			if (m_currentMovementState != MyCharacterMovementEnum.Died)
			{
				if (!JetpackRunning && !IsOnLadder)
				{
					if ((m_currentJumpTime <= 0f && newState == HkCharacterStateType.HK_CHARACTER_IN_AIR) || newState == (HkCharacterStateType)5)
					{
						StartFalling();
					}
					else if (m_isFalling)
					{
						StopFalling();
					}
				}
				if (JetpackRunning)
				{
					m_currentJumpTime = 0f;
				}
			}
			m_currentCharacterState = newState;
		}

		internal void StartFalling()
		{
			if (!JetpackRunning && m_currentMovementState != MyCharacterMovementEnum.Died && m_currentMovementState != MyCharacterMovementEnum.Sitting)
			{
				if (m_currentCharacterState == HkCharacterStateType.HK_CHARACTER_JUMPING)
				{
					m_currentFallingTime = -1f;
				}
				else
				{
					m_currentFallingTime = 0f;
				}
				m_isFalling = true;
				m_crouchAfterFall = WantsCrouch;
				WantsCrouch = false;
				SetCurrentMovementState(MyCharacterMovementEnum.Falling);
			}
		}

		internal void StopFalling()
		{
			if (m_currentMovementState != MyCharacterMovementEnum.Died)
			{
				MyCharacterJetpackComponent jetpackComp = JetpackComp;
				if (m_isFalling && (jetpackComp == null || !jetpackComp.TurnedOn || !jetpackComp.IsPowered))
				{
					SoundComp.PlayFallSound();
				}
				if (m_isFalling)
				{
					m_movementsFlagsChanged = true;
				}
				m_isFalling = false;
				m_isFallingAnimationPlayed = false;
				m_currentFallingTime = 0f;
				m_currentJumpTime = 0f;
				m_canJump = true;
				WantsCrouch = m_crouchAfterFall;
				m_crouchAfterFall = false;
			}
		}

		public bool CanStartConstruction(MyCubeBlockDefinition blockDefinition)
		{
			if (blockDefinition == null)
			{
				return false;
			}
			MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(this);
			if (builderInventory == null)
			{
				return false;
			}
			return builderInventory.GetItemAmount(blockDefinition.Components[0].Definition.Id) >= 1;
		}

		public bool CanStartConstruction(Dictionary<MyDefinitionId, int> constructionCost)
		{
			MyInventoryBase builderInventory = MyCubeBuilder.BuildComponent.GetBuilderInventory(this);
			foreach (KeyValuePair<MyDefinitionId, int> item in constructionCost)
			{
				if (builderInventory.GetItemAmount(item.Key) < item.Value)
				{
					return false;
				}
			}
			return true;
		}

		[Event(null, 7038)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		public void UnequipWeapon()
		{
			if (m_leftHandItem != null && m_leftHandItem is IMyHandheldGunObject<MyDeviceBase>)
			{
				(m_leftHandItem as IMyHandheldGunObject<MyDeviceBase>).OnControlReleased();
				m_leftHandItem.Close();
				m_leftHandItem = null;
				bool sync = this == MySession.Static.LocalCharacter;
				TriggerCharacterAnimationEvent("unequip_left_tool", sync);
			}
			if (m_currentWeapon != null)
			{
				if (ControllerInfo.IsLocallyControlled())
				{
					MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
					if (highlightSystem != null)
					{
						MyEntity myEntity = (MyEntity)m_currentWeapon;
						myEntity.Render.RenderObjectIDs.ForEach(delegate(uint id)
						{
							if (id != uint.MaxValue)
							{
								highlightSystem.RemoveHighlightOverlappingModel(id);
							}
						});
						if (myEntity.Subparts != null)
						{
							foreach (KeyValuePair<string, MyEntitySubpart> subpart in myEntity.Subparts)
							{
								subpart.Value.Render.RenderObjectIDs.ForEach(delegate(uint id)
								{
									if (id != uint.MaxValue)
									{
										highlightSystem.RemoveHighlightOverlappingModel(id);
									}
								});
							}
						}
					}
				}
				if (MySession.Static.LocalCharacter == this && !MyInput.Static.IsGameControlPressed(MyControlsSpace.PRIMARY_TOOL_ACTION) && !MyInput.Static.IsGameControlPressed(MyControlsSpace.SECONDARY_TOOL_ACTION))
				{
					EndShootAll();
				}
				else if (Sync.IsServer)
				{
					MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
					foreach (MyShootActionEnum action in values)
					{
						if (IsShooting(action))
						{
							m_currentWeapon.EndShoot(action);
						}
					}
				}
				MyEntity obj = m_currentWeapon as MyEntity;
				if (UseNewAnimationSystem && m_handItemDefinition != null && !string.IsNullOrEmpty(m_handItemDefinition.Id.SubtypeName))
				{
					IMyVariableStorage<float> variables = base.AnimationController.Variables;
					MyObjectBuilderType typeId = m_handItemDefinition.Id.TypeId;
					variables.SetValue(MyStringId.GetOrCompute(typeId.ToString().ToLower()), 0f);
				}
				SaveAmmoToWeapon();
				m_currentWeapon.OnControlReleased();
				if (m_zoomMode == MyZoomModeEnum.IronSight)
				{
					bool isInFirstPersonView = IsInFirstPersonView;
					EnableIronsight(enable: false, newKeyPress: true, MySession.Static.CameraController == this);
					IsInFirstPersonView = isInFirstPersonView;
				}
				MyResourceSinkComponent myResourceSinkComponent = obj.Components.Get<MyResourceSinkComponent>();
				if (myResourceSinkComponent != null)
				{
					SuitRechargeDistributor.RemoveSink(myResourceSinkComponent);
				}
				obj.SetFadeOut(state: false);
				obj.OnClose -= gunEntity_OnClose;
				MyEntities.Remove(obj);
				obj.Close();
				m_currentWeapon = null;
				if (ControllerInfo.IsLocallyHumanControlled() && MySector.MainCamera != null)
				{
					MySector.MainCamera.Zoom.ResetZoom();
				}
				if (UseNewAnimationSystem)
				{
					bool sync2 = this == MySession.Static.LocalCharacter;
					TriggerCharacterAnimationEvent("unequip_left_tool", sync2);
					TriggerCharacterAnimationEvent("unequip_right_tool", sync2);
				}
				else
				{
					StopUpperAnimation(0.2f);
					SwitchAnimation(m_currentMovementState, checkState: false);
				}
				base.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdShooting, 0f);
				if (!Sync.IsDedicated)
				{
					IMySkinnedEntity mySkinnedEntity;
					if ((mySkinnedEntity = (base.AnimationController.Entity as IMySkinnedEntity)) != null)
					{
						mySkinnedEntity.UpdateControl(0f);
					}
					base.AnimationController.ApplyVariables();
					base.AnimationController.Update();
				}
				MyAnalyticsHelper.ReportActivityEnd(this, "item_equip");
			}
			if (m_currentShotTime <= 0f)
			{
				StopUpperAnimation(0f);
				StopFingersAnimation(0f);
			}
			m_currentWeapon = null;
			StopFingersAnimation(0f);
		}

		private void EquipWeapon(IMyHandheldGunObject<MyDeviceBase> newWeapon, bool showNotification = false)
		{
			if (newWeapon == null)
			{
				return;
			}
			MyEntity myEntity = (MyEntity)newWeapon;
			myEntity.Render.CastShadows = true;
			myEntity.Render.NeedsResolveCastShadow = false;
			myEntity.Save = false;
			myEntity.OnClose += gunEntity_OnClose;
			MyAssetModifierComponent myAssetModifierComponent = new MyAssetModifierComponent();
			myEntity.Components.Add(myAssetModifierComponent);
			MyEntities.Add(myEntity);
			if (MySession.Static.LocalCharacter == this)
			{
				MyLocalCache.LoadInventoryConfig(myEntity, myAssetModifierComponent);
			}
			m_handItemDefinition = null;
			m_currentWeapon = newWeapon;
			m_currentWeapon.OnControlAcquired(this);
			((MyEntity)m_currentWeapon).Render.DrawInAllCascades = true;
			if (ControllerInfo.IsLocallyControlled())
			{
				MyHighlightSystem highlightSystem = MySession.Static.GetComponent<MyHighlightSystem>();
				if (m_currentWeapon != null && highlightSystem != null)
				{
					MyEntity myEntity2 = (MyEntity)m_currentWeapon;
					myEntity2.Render.RenderObjectIDs.ForEach(delegate(uint id)
					{
						if (id != uint.MaxValue)
						{
							highlightSystem.AddHighlightOverlappingModel(id);
						}
					});
					if (myEntity2.Subparts != null)
					{
						foreach (KeyValuePair<string, MyEntitySubpart> subpart in myEntity2.Subparts)
						{
							subpart.Value.Render.RenderObjectIDs.ForEach(delegate(uint id)
							{
								if (id != uint.MaxValue)
								{
									highlightSystem.AddHighlightOverlappingModel(id);
								}
							});
						}
					}
				}
			}
			if (this.WeaponEquiped != null)
			{
				this.WeaponEquiped(m_currentWeapon);
			}
			if (MyVisualScriptLogicProvider.ToolEquipped != null && ControllerInfo != null)
			{
				long controllingIdentityId = ControllerInfo.ControllingIdentityId;
				ToolEquipedEvent toolEquipped = MyVisualScriptLogicProvider.ToolEquipped;
				MyObjectBuilderType typeId = newWeapon.DefinitionId.TypeId;
				toolEquipped(controllingIdentityId, typeId.ToString(), newWeapon.DefinitionId.SubtypeName);
			}
			MyAnalyticsHelper.ReportActivityStart(this, "item_equip", "character", "toolbar_item_usage", m_currentWeapon.GetType().Name);
			if (m_currentWeapon.PhysicalObject != null)
			{
				MyDefinitionId id2 = m_currentWeapon.PhysicalObject.GetId();
				m_handItemDefinition = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(id2);
			}
			else if (m_currentWeapon.DefinitionId.TypeId == typeof(MyObjectBuilder_CubePlacer))
			{
				MyDefinitionId id3 = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));
				m_handItemDefinition = MyDefinitionManager.Static.TryGetHandItemDefinition(ref id3);
			}
			if (m_handItemDefinition != null && !string.IsNullOrEmpty(m_handItemDefinition.FingersAnimation))
			{
				if (!m_characterDefinition.AnimationNameToSubtypeName.TryGetValue(m_handItemDefinition.FingersAnimation, out string value))
				{
					value = m_handItemDefinition.FingersAnimation;
				}
				MyAnimationDefinition myAnimationDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(value);
				if (!myAnimationDefinition.LeftHandItem.TypeId.IsNull)
				{
					m_currentWeapon.OnControlReleased();
					(m_currentWeapon as MyEntity).Close();
					m_currentWeapon = null;
				}
				PlayCharacterAnimation(m_handItemDefinition.FingersAnimation, MyBlendOption.Immediate, (!myAnimationDefinition.Loop) ? MyFrameOption.PlayOnce : MyFrameOption.Loop, 1f);
				if (UseNewAnimationSystem)
				{
					bool sync = this == MySession.Static.LocalCharacter;
					TriggerCharacterAnimationEvent("equip_left_tool", sync);
					TriggerCharacterAnimationEvent("equip_right_tool", sync);
					TriggerCharacterAnimationEvent(m_handItemDefinition.Id.SubtypeName.ToLower(), sync);
					TriggerCharacterAnimationEvent(m_handItemDefinition.FingersAnimation.ToLower(), sync);
					if (!string.IsNullOrEmpty(m_handItemDefinition.Id.SubtypeName))
					{
						IMyVariableStorage<float> variables = base.AnimationController.Variables;
						MyObjectBuilderType typeId = m_handItemDefinition.Id.TypeId;
						variables.SetValue(MyStringId.GetOrCompute(typeId.ToString().ToLower()), 1f);
					}
				}
				if (!myAnimationDefinition.LeftHandItem.TypeId.IsNull)
				{
					if (m_leftHandItem != null)
					{
						(m_leftHandItem as IMyHandheldGunObject<MyDeviceBase>).OnControlReleased();
						m_leftHandItem.Close();
					}
					long weaponEntityId = MyEntityIdentifier.AllocateId();
					uint? inventoryItemId = null;
					IMyHandheldGunObject<MyDeviceBase> myHandheldGunObject = CreateGun(GetObjectBuilderForWeapon(myAnimationDefinition.LeftHandItem, ref inventoryItemId, weaponEntityId), inventoryItemId);
					if (myHandheldGunObject != null)
					{
						m_leftHandItem = (myHandheldGunObject as MyEntity);
						m_leftHandItem.Render.DrawInAllCascades = true;
						myHandheldGunObject.OnControlAcquired(this);
						UpdateLeftHandItemPosition();
						MyEntities.Add(m_leftHandItem);
					}
				}
			}
			else if (m_handItemDefinition != null)
			{
				if (UseNewAnimationSystem)
				{
					bool sync2 = this == MySession.Static.LocalCharacter;
					TriggerCharacterAnimationEvent("equip_left_tool", sync2);
					TriggerCharacterAnimationEvent("equip_right_tool", sync2);
					TriggerCharacterAnimationEvent(m_handItemDefinition.Id.SubtypeName.ToLower(), sync2);
					if (!string.IsNullOrEmpty(m_handItemDefinition.Id.SubtypeName))
					{
						IMyVariableStorage<float> variables2 = base.AnimationController.Variables;
						MyObjectBuilderType typeId = m_handItemDefinition.Id.TypeId;
						variables2.SetValue(MyStringId.GetOrCompute(typeId.ToString().ToLower()), 1f);
					}
				}
			}
			else
			{
				StopFingersAnimation(0f);
			}
			MyResourceSinkComponent myResourceSinkComponent = myEntity.Components.Get<MyResourceSinkComponent>();
			if (myResourceSinkComponent != null && SuitRechargeDistributor != null)
			{
				SuitRechargeDistributor.AddSink(myResourceSinkComponent);
			}
			if (showNotification)
			{
				MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationUsingWeaponType, 2000);
				myHudNotification.SetTextFormatArguments(MyDeviceBase.GetGunNotificationName(newWeapon.DefinitionId));
				MyHud.Notifications.Add(myHudNotification);
			}
			Static_CameraAttachedToChanged(null, null);
			if (!(IsUsing is MyCockpit))
			{
				MyHud.Crosshair.ResetToDefault(clear: false);
			}
		}

		private void gunEntity_OnClose(MyEntity obj)
		{
			if (m_currentWeapon == obj)
			{
				m_currentWeapon = null;
			}
		}

		private void SetPowerInput(float input)
		{
			if (LightEnabled && input >= 2E-06f)
			{
				m_lightPowerFromProducer = 2E-06f;
				input -= 2E-06f;
			}
			else
			{
				m_lightPowerFromProducer = 0f;
			}
		}

		private float ComputeRequiredPower()
		{
			float num = 1E-05f;
			if (OxygenComponent != null && OxygenComponent.NeedsOxygenFromSuit)
			{
				num = 1E-06f;
			}
			if (m_lightEnabled)
			{
				num += 2E-06f;
			}
			float num2 = Math.Abs((GetOutsideTemperature() * 2f - 1f) * (Definition.SuitConsumptionInTemperatureExtreme / 100000f));
			return num + num2;
		}

		internal void RecalculatePowerRequirement(bool chargeImmediatelly = false)
		{
			SinkComp.Update();
			UpdateLightPower(chargeImmediatelly);
		}

		public void EnableLights(bool enable)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.EnableLightsCallback, enable);
			if (!Sync.IsServer)
			{
				EnableLightsCallback(enable);
			}
		}

		[Event(null, 7418)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		private void EnableLightsCallback(bool enable)
		{
			if (m_lightEnabled != enable)
			{
				m_lightEnabled = enable;
				RecalculatePowerRequirement();
				if (Render != null)
				{
					Render.UpdateLightPosition();
				}
			}
		}

		public void EnableBroadcasting(bool enable)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.EnableBroadcastingCallback, enable);
			if (!Sync.IsServer)
			{
				EnableBroadcastingCallback(enable);
			}
		}

		[Event(null, 7440)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		public void EnableBroadcastingCallback(bool enable)
		{
			if (RadioBroadcaster != null && RadioBroadcaster.WantsToBeEnabled != enable)
			{
				RadioBroadcaster.WantsToBeEnabled = enable;
				RadioBroadcaster.Enabled = enable;
			}
		}

		public void Sit(bool enableFirstPerson, bool playerIsPilot, bool enableBag, string animation)
		{
			EndShootAll();
			SwitchToWeaponInternal(null, updateSync: false, null, 0L);
			Render.NearFlag = false;
			m_isFalling = false;
			PlayCharacterAnimation(animation, MyBlendOption.Immediate, MyFrameOption.Loop, 0f);
			StopUpperCharacterAnimation(0f);
			StopFingersAnimation(0f);
			SetHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0f)));
			SetUpperHandAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(0f)));
			if (UseNewAnimationSystem)
			{
				base.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdLean, 0f);
			}
			SetSpineAdditionalRotation(Quaternion.CreateFromAxisAngle(Vector3.Forward, 0f), Quaternion.CreateFromAxisAngle(Vector3.Forward, 0f));
			SetHeadAdditionalRotation(Quaternion.Identity, updateSync: false);
			FlushAnimationQueue();
			SinkComp.Update();
			UpdateLightPower(chargeImmediately: true);
			EnableBag(enableBag);
			if (Sync.IsServer)
			{
				m_bootsState.Value = MyBootsState.Init;
			}
			SetCurrentMovementState(MyCharacterMovementEnum.Sitting);
			if (!Sandbox.Engine.Platform.Game.IsDedicated && UseNewAnimationSystem)
			{
				TriggerCharacterAnimationEvent("sit", sync: false);
				if (!string.IsNullOrEmpty(animation))
				{
					string value = string.Empty;
					if (!Definition.AnimationNameToSubtypeName.TryGetValue(animation, out value))
					{
						value = animation;
					}
					TriggerCharacterAnimationEvent(value, sync: false);
				}
			}
			UpdateAnimation(0f);
		}

		public void EnableBag(bool enabled)
		{
			m_enableBag = enabled;
			if (base.InScene && Render.RenderObjectIDs[0] != uint.MaxValue)
			{
				MyRenderProxy.UpdateModelProperties(Render.RenderObjectIDs[0], "Backpack", enabled ? RenderFlags.Visible : RenderFlags.SkipInDepth, enabled ? RenderFlags.SkipInDepth : RenderFlags.Visible, null, null);
			}
		}

		public void EnableHead(bool enabled)
		{
			if (base.InScene && m_headRenderingEnabled != enabled)
			{
				UpdateHeadModelProperties(enabled);
			}
		}

		private void UpdateHeadModelProperties(bool enabled)
		{
			if (m_characterDefinition.MaterialsDisabledIn1st == null)
			{
				return;
			}
			m_headRenderingEnabled = enabled;
			if (Render.RenderObjectIDs[0] != uint.MaxValue)
			{
				string[] materialsDisabledIn1st = m_characterDefinition.MaterialsDisabledIn1st;
				foreach (string materialName in materialsDisabledIn1st)
				{
					MyRenderProxy.UpdateModelProperties(Render.RenderObjectIDs[0], materialName, enabled ? RenderFlags.Visible : ((RenderFlags)0), (!enabled) ? RenderFlags.Visible : ((RenderFlags)0), null, null);
				}
			}
		}

		public void Stand()
		{
			PlayCharacterAnimation("Idle", MyBlendOption.Immediate, MyFrameOption.Loop, 0f);
			Render.NearFlag = false;
			StopUpperCharacterAnimation(0f);
			RecalculatePowerRequirement();
			EnableBag(enabled: true);
			UpdateHeadModelProperties(m_headRenderingEnabled);
			SetCurrentMovementState(MyCharacterMovementEnum.Standing);
			m_wasInFirstPerson = false;
			IsUsing = null;
			if (Sync.IsServer)
			{
				m_bootsState.Value = MyBootsState.Init;
			}
			if (Physics.CharacterProxy != null)
			{
				Physics.CharacterProxy.Stand();
			}
		}

		public void ForceUpdateBreath()
		{
			if (m_breath != null)
			{
				m_breath.ForceUpdate();
			}
		}

		public long GetPlayerIdentityId()
		{
			return m_idModule.Owner;
		}

		private MyPlayer TryGetPlayer()
		{
			return ControllerInfo.Controller?.Player;
		}

		public MyIdentity GetIdentity()
		{
			MyPlayer myPlayer = TryGetPlayer();
			if (myPlayer != null)
			{
				return myPlayer.Identity;
			}
			return MySession.Static.Players.TryGetIdentity(GetPlayerIdentityId());
		}

		public MyPlayer.PlayerId GetClientIdentity()
		{
			MyPlayer myPlayer = TryGetPlayer();
			if (myPlayer != null)
			{
				return myPlayer.Id;
			}
			MySession.Static.Players.TryGetPlayerId(GetPlayerIdentityId(), out MyPlayer.PlayerId result);
			return result;
		}

		public bool DoDamage(float damage, MyStringHash damageType, bool updateSync, long attackerId = 0L)
		{
			damage *= CharacterGeneralDamageModifier;
			if (damage < 0f)
			{
				return false;
			}
			if (damageType != MyDamageType.Suicide && damageType != MyDamageType.Asphyxia && damageType != MyDamageType.LowPressure && damageType != MyDamageType.Temperature && !MySessionComponentSafeZones.IsActionAllowed(this, MySafeZoneAction.Damage, 0L, 0uL))
			{
				return false;
			}
			MyPlayer.PlayerId clientIdentity = GetClientIdentity();
			if (clientIdentity.SerialId == 0 && MySession.Static.RemoteAdminSettings.TryGetValue(clientIdentity.SteamId, out AdminSettingsEnum value) && value.HasFlag(AdminSettingsEnum.Invulnerable) && damageType != MyDamageType.Suicide)
			{
				return false;
			}
			if (damageType != MyDamageType.Suicide && ControllerInfo.IsLocallyControlled() && MySession.Static.CameraController == this && MAX_SHAKE_DAMAGE > 0f)
			{
				float shakePower = MySector.MainCamera.CameraShake.MaxShake * MathHelper.Clamp(damage, 0f, MAX_SHAKE_DAMAGE) / MAX_SHAKE_DAMAGE;
				MySector.MainCamera.CameraShake.AddShake(shakePower);
			}
			if (updateSync)
			{
				TriggerCharacterAnimationEvent("hurt", sync: true);
			}
			else
			{
				base.AnimationController.TriggerAction(MyAnimationVariableStorageHints.StrIdActionHurt);
			}
			if ((!CharacterCanDie && (!(damageType == MyDamageType.Suicide) || !MyPerGameSettings.CharacterSuicideEnabled)) || StatComp == null)
			{
				return false;
			}
			MyPlayer playerFromCharacter = MyPlayer.GetPlayerFromCharacter(this);
			MyPlayer myPlayer = null;
			if (damageType != MyDamageType.Suicide && MyEntities.TryGetEntityById(attackerId, out MyEntity entity))
			{
				if (entity == this)
				{
					return false;
				}
				if (entity is MyCharacter)
				{
					myPlayer = MyPlayer.GetPlayerFromCharacter(entity as MyCharacter);
				}
				else if (entity is IMyGunBaseUser)
				{
					myPlayer = MyPlayer.GetPlayerFromWeapon(entity as IMyGunBaseUser);
				}
				else if (entity is MyHandDrill)
				{
					myPlayer = MyPlayer.GetPlayerFromCharacter((entity as MyHandDrill).Owner);
				}
				if (playerFromCharacter != null && myPlayer != null)
				{
					MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerFromCharacter.Identity.IdentityId) as MyFaction;
					if (myFaction != null && !myFaction.EnableFriendlyFire && myFaction.IsMember(myPlayer.Identity.IdentityId))
					{
						return false;
					}
				}
				if (damage >= 0f && MySession.Static != null && MyMusicController.Static != null)
				{
					if (this == MySession.Static.LocalCharacter && !(entity is MyVoxelPhysics) && !(entity is MyCubeGrid))
					{
						MyMusicController.Static.Fighting(heavy: false, (int)damage * 3);
					}
					else if (entity == MySession.Static.LocalCharacter)
					{
						MyMusicController.Static.Fighting(heavy: false, (int)damage * 2);
					}
					else if (entity is IMyGunBaseUser && (entity as IMyGunBaseUser).Owner as MyCharacter == MySession.Static.LocalCharacter)
					{
						MyMusicController.Static.Fighting(heavy: false, (int)damage * 2);
					}
					else if (MySession.Static.ControlledEntity == entity)
					{
						MyMusicController.Static.Fighting(heavy: false, (int)damage);
					}
				}
			}
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.SIMULATE);
			MyDamageInformation info = new MyDamageInformation(isDeformation: false, damage, damageType, attackerId);
			if (UseDamageSystem && !m_dieAfterSimulation && !IsDead)
			{
				MyDamageSystem.Static.RaiseBeforeDamageApplied(this, ref info);
			}
			if (info.Amount <= 0f)
			{
				return false;
			}
			StatComp.DoDamage(damage, info);
			bool flag = false;
			if (playerFromCharacter != null && StatComp.Health.Value - StatComp.Health.MinValue < 0.001f)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetPlayerIdentity(playerFromCharacter.Id);
				if (myPlayer != null)
				{
					MyIdentity myIdentity2 = MySession.Static.Players.TryGetPlayerIdentity(myPlayer.Id);
					if (myIdentity != null && myIdentity2 != null)
					{
						if (Sync.IsServer && myIdentity2.IdentityId != myIdentity.IdentityId)
						{
							MySession.Static.Factions.DamageFactionPlayerReputation(myIdentity2.IdentityId, myIdentity.IdentityId, MyReputationDamageType.Killing);
						}
						if (MySession.Static.Factions.PlayerKilledByPlayer != null)
						{
							MySession.Static.Factions.PlayerKilledByPlayer(myIdentity.IdentityId, myIdentity2.IdentityId);
						}
						flag = true;
					}
				}
				if (!flag && myIdentity != null && MySession.Static.Factions.PlayerKilledByUnknown != null)
				{
					MySession.Static.Factions.PlayerKilledByUnknown.InvokeIfNotNull(myIdentity.IdentityId);
				}
			}
			MyAnalyticsHelper.SetLastDamageInformation(info);
			if (UseDamageSystem)
			{
				MyDamageSystem.Static.RaiseAfterDamageApplied(this, info);
			}
			return true;
		}

		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, object customdata, IMyDecalHandler decalHandler, MyStringHash material)
		{
			MyCharacterHitInfo myCharacterHitInfo = customdata as MyCharacterHitInfo;
			if (myCharacterHitInfo != null && myCharacterHitInfo.BoneIndex != -1)
			{
				MyDecalRenderInfo renderInfo = default(MyDecalRenderInfo);
				renderInfo.Position = myCharacterHitInfo.Triangle.IntersectionPointInObjectSpace;
				renderInfo.Normal = myCharacterHitInfo.Triangle.NormalInObjectSpace;
				renderInfo.RenderObjectIds = Render.RenderObjectIDs;
				renderInfo.Source = source;
				if (material.GetHashCode() == 0)
				{
					renderInfo.Material = MyStringHash.GetOrCompute(m_characterDefinition.PhysicalMaterial);
				}
				else
				{
					renderInfo.Material = material;
				}
				VertexBoneIndicesWeights? affectingBoneIndicesWeights = myCharacterHitInfo.Triangle.GetAffectingBoneIndicesWeights(ref m_boneIndexWeightTmp);
				renderInfo.BoneIndices = affectingBoneIndicesWeights.Value.Indices;
				renderInfo.BoneWeights = affectingBoneIndicesWeights.Value.Weights;
				renderInfo.Binding = new MyDecalBindingInfo
				{
					Position = myCharacterHitInfo.HitPositionBindingPose,
					Normal = myCharacterHitInfo.HitNormalBindingPose,
					Transformation = myCharacterHitInfo.BindingTransformation
				};
				m_tmpIds.Clear();
				decalHandler.AddDecal(ref renderInfo, m_tmpIds);
				foreach (uint tmpId in m_tmpIds)
				{
					AddBoneDecal(tmpId, myCharacterHitInfo.BoneIndex);
				}
			}
		}

		void IMyCharacter.Kill(object statChangeData)
		{
			MyDamageInformation damageInfo = default(MyDamageInformation);
			if (statChangeData != null)
			{
				damageInfo = (MyDamageInformation)statChangeData;
			}
			Kill(sync: true, damageInfo);
		}

		void IMyCharacter.TriggerCharacterAnimationEvent(string eventName, bool sync)
		{
			TriggerCharacterAnimationEvent(eventName, sync);
		}

		private Action<MyCharacter> GetDelegate(Action<IMyCharacter> value)
		{
			return (Action<MyCharacter>)Delegate.CreateDelegate(typeof(Action<MyCharacter>), value.Target, value.Method);
		}

		public void Kill(bool sync, MyDamageInformation damageInfo)
		{
			if (m_dieAfterSimulation || IsDead || (MyFakes.DEVELOPMENT_PRESET && damageInfo.Type != MyDamageType.Suicide))
			{
				return;
			}
			if (sync)
			{
				KillCharacter(damageInfo);
				return;
			}
			if (UseDamageSystem)
			{
				MyDamageSystem.Static.RaiseDestroyed(this, damageInfo);
			}
			MyAnalyticsHelper.SetLastDamageInformation(damageInfo);
			StatComp.LastDamage = damageInfo;
			m_dieAfterSimulation = true;
		}

		public void Die()
		{
			if ((CharacterCanDie || MyPerGameSettings.CharacterSuicideEnabled) && !IsDead)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextSuicide), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
				{
					if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnSuicideRequest);
					}
				}, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
			}
		}

		[Event(null, 7910)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		private void OnSuicideRequest()
		{
			DoDamage(1000f, MyDamageType.Suicide, updateSync: true, base.EntityId);
		}

		private void DieInternal()
		{
			if (!CharacterCanDie && !MyPerGameSettings.CharacterSuicideEnabled)
			{
				return;
			}
			MyPlayer myPlayer = TryGetPlayer();
			if (myPlayer != null && myPlayer.IsImmortal)
			{
				return;
			}
			bool flag = MySession.Static.LocalCharacter == this;
			SoundComp.PlayDeathSound((StatComp != null) ? StatComp.LastDamage.Type : MyStringHash.NullOrEmpty);
			if (UseNewAnimationSystem)
			{
				base.AnimationController.Variables.SetValue(MyAnimationVariableStorageHints.StrIdDead, 1f);
			}
			if (m_InventoryScreen != null)
			{
				m_InventoryScreen.CloseScreen();
			}
			if (StatComp != null && StatComp.Health != null)
			{
				StatComp.Health.OnStatChanged -= StatComp.OnHealthChanged;
			}
			if (m_breath != null)
			{
				m_breath.CurrentState = MyCharacterBreath.State.NoBreath;
			}
			if (IsOnLadder)
			{
				GetOffLadder();
			}
			if (CurrentRemoteControl != null)
			{
				MyRemoteControl myRemoteControl = CurrentRemoteControl as MyRemoteControl;
				if (myRemoteControl != null)
				{
					myRemoteControl.ForceReleaseControl();
				}
				else
				{
					(CurrentRemoteControl as MyLargeTurretBase)?.ForceReleaseControl();
				}
			}
			if (ControllerInfo != null && ControllerInfo.IsLocallyHumanControlled())
			{
				if (MyGuiScreenTerminal.IsOpen)
				{
					MyGuiScreenTerminal.Hide();
				}
				if (MyGuiScreenGamePlay.ActiveGameplayScreen != null)
				{
					MyGuiScreenGamePlay.ActiveGameplayScreen.CloseScreen();
					MyGuiScreenGamePlay.ActiveGameplayScreen = null;
				}
				if (MyGuiScreenGamePlay.TmpGameplayScreenHolder != null)
				{
					MyGuiScreenGamePlay.TmpGameplayScreenHolder.CloseScreen();
					MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
				}
				if (ControllerInfo.Controller != null)
				{
					ControllerInfo.Controller.SaveCamera();
				}
			}
			if (base.Parent is MyCockpit)
			{
				MyCockpit myCockpit = base.Parent as MyCockpit;
				if (myCockpit.Pilot == this)
				{
					myCockpit.RemovePilot();
				}
			}
			if (MySession.Static.ControlledEntity is MyRemoteControl)
			{
				MyRemoteControl myRemoteControl2 = MySession.Static.ControlledEntity as MyRemoteControl;
				if (myRemoteControl2.PreviousControlledEntity == this)
				{
					myRemoteControl2.ForceReleaseControl();
				}
			}
			if (MySession.Static.ControlledEntity is MyLargeTurretBase && MySession.Static.LocalCharacter == this)
			{
				(MySession.Static.ControlledEntity as MyLargeTurretBase).ForceReleaseControl();
			}
			if (m_currentMovementState == MyCharacterMovementEnum.Died)
			{
				StartRespawn(0.1f);
				return;
			}
			ulong num = 0uL;
			if (ControllerInfo.Controller != null && ControllerInfo.Controller.Player != null)
			{
				num = ControllerInfo.Controller.Player.Id.SteamId;
				if (!MySession.Static.Cameras.TryGetCameraSettings(ControllerInfo.Controller.Player.Id, base.EntityId, ControllerInfo.Controller.ControlledEntity is MyCharacter && MySession.Static.LocalCharacter == ControllerInfo.Controller.ControlledEntity, out m_cameraSettingsWhenAlive) && ControllerInfo.IsLocallyHumanControlled())
				{
					m_cameraSettingsWhenAlive = new MyEntityCameraSettings
					{
						Distance = MyThirdPersonSpectator.Static.GetViewerDistance(),
						IsFirstPerson = IsInFirstPersonView,
						HeadAngle = new Vector2(HeadLocalXAngle, HeadLocalYAngle)
					};
				}
			}
			MyAnalyticsHelper.ReportPlayerDeath(ControllerInfo.IsLocallyHumanControlled(), num);
			MySandboxGame.Log.WriteLine("Player character died. Id : " + num);
			m_deadPlayerIdentityId = GetPlayerIdentityId();
			IsUsing = null;
			base.Save = false;
			m_isFalling = false;
			SetCurrentMovementState(MyCharacterMovementEnum.Died);
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.UnequipWeapon);
			}
			StopUpperAnimation(0.5f);
			m_animationCommandsEnabled = true;
			if (m_isInFirstPerson)
			{
				PlayCharacterAnimation("DiedFps", MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0.5f);
			}
			else
			{
				PlayCharacterAnimation("Died", MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0.5f);
			}
			InitDeadBodyPhysics();
			StartRespawn(5f);
			m_currentLootingCounter = m_characterDefinition.LootingTime;
			if (flag)
			{
				EnableLights(enable: false);
			}
			if (this.CharacterDied != null)
			{
				this.CharacterDied(this);
			}
			foreach (MyComponentBase component in base.Components)
			{
				(component as MyCharacterComponent)?.OnCharacterDead();
			}
			SoundComp.CharacterDied();
			JetpackComp = null;
			if (!base.Components.Has<MyCharacterRagdollComponent>())
			{
				base.SyncFlag = true;
			}
			MyCharacter.OnCharacterDied?.Invoke(this);
		}

		private void StartRespawn(float respawnTime)
		{
			MyPlayer myPlayer = TryGetPlayer();
			if (myPlayer != null)
			{
				MySessionComponentMissionTriggers.PlayerDied(myPlayer);
				if (MyVisualScriptLogicProvider.PlayerDied != null && !IsBot)
				{
					MyVisualScriptLogicProvider.PlayerDied(myPlayer.Identity.IdentityId);
				}
				if (MyVisualScriptLogicProvider.NPCDied != null && IsBot)
				{
					MyVisualScriptLogicProvider.NPCDied(DefinitionId.HasValue ? DefinitionId.Value.SubtypeName : "");
				}
				if (!MySessionComponentMissionTriggers.CanRespawn(myPlayer.Id))
				{
					m_currentRespawnCounter = -1f;
				}
			}
			if (m_currentRespawnCounter != -1f)
			{
				if (MySession.Static != null && this == MySession.Static.ControlledEntity)
				{
					MyGuiScreenTerminal.Hide();
					m_respawnNotification = new MyHudNotification(MyCommonTexts.NotificationRespawn, 5000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 5);
					m_respawnNotification.Level = MyNotificationLevel.Important;
					m_respawnNotification.SetTextFormatArguments((int)m_currentRespawnCounter);
					MyHud.Notifications.Add(m_respawnNotification);
				}
				m_currentRespawnCounter = respawnTime;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		internal void DeactivateRespawn()
		{
			m_currentRespawnCounter = -1f;
		}

		private void InitDeadBodyPhysics()
		{
			Vector3 linearVelocity = Vector3.Zero;
			EnableBag(enabled: false);
			RadioBroadcaster.BroadcastRadius = 5f;
			if (Physics != null)
			{
				linearVelocity = Physics.LinearVelocity;
				Physics.Enabled = false;
				Physics.Close();
				Physics = null;
			}
			if (m_deathLinearVelocityFromSever.HasValue)
			{
				linearVelocity = m_deathLinearVelocityFromSever.Value;
			}
			HkMassProperties value = default(HkMassProperties);
			value.Mass = 500f;
			int collisionFilter = (!Sync.IsDedicated || !MyFakes.ENABLE_RAGDOLL || MyFakes.ENABLE_RAGDOLL_CLIENT_SYNC) ? 23 : 19;
			if (Definition.DeadBodyShape != null)
			{
				HkBoxShape shape = new HkBoxShape(base.PositionComp.LocalAABB.HalfExtents * Definition.DeadBodyShape.BoxShapeScale);
				value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(shape.HalfExtents, value.Mass);
				value.CenterOfMass = shape.HalfExtents * Definition.DeadBodyShape.RelativeCenterOfMass;
				HkShape shape2 = shape;
				Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_DEFAULT);
				Vector3D vector3D = base.PositionComp.LocalAABB.HalfExtents * Definition.DeadBodyShape.RelativeShapeTranslation;
				MatrixD worldTransform = MatrixD.CreateTranslation(vector3D);
				Physics.CreateFromCollisionObject(shape2, base.PositionComp.LocalVolume.Center + vector3D, worldTransform, value, collisionFilter);
				Physics.Friction = Definition.DeadBodyShape.Friction;
				Physics.RigidBody.MaxAngularVelocity = MathF.E * 449f / 777f;
				Physics.LinearVelocity = linearVelocity;
				shape2.RemoveReference();
				Physics.Enabled = true;
			}
			else
			{
				Vector3 halfExtents = base.PositionComp.LocalAABB.HalfExtents;
				halfExtents.X *= 0.7f;
				halfExtents.Z *= 0.7f;
				HkBoxShape shape3 = new HkBoxShape(halfExtents);
				value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(shape3.HalfExtents, value.Mass);
				value.CenterOfMass = new Vector3(halfExtents.X, 0f, 0f);
				HkShape shape2 = shape3;
				Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_DEFAULT);
				Physics.CreateFromCollisionObject(shape2, base.PositionComp.LocalAABB.Center, MatrixD.Identity, value, collisionFilter);
				Physics.Friction = 0.5f;
				Physics.RigidBody.MaxAngularVelocity = MathF.E * 449f / 777f;
				Physics.LinearVelocity = linearVelocity;
				shape2.RemoveReference();
				Physics.Enabled = true;
			}
			HkMassChangerUtil.Create(Physics.RigidBody, 65536, 1f, 0f);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			if (m_needReconnectLadder)
			{
				if (m_ladder != null)
				{
					ReconnectConstraint(m_oldLadderGrid, m_ladder.CubeGrid);
					if (m_constraintInstance != null)
					{
						SetCharacterLadderConstraint(base.WorldMatrix);
					}
				}
				m_needReconnectLadder = false;
				m_oldLadderGrid = null;
			}
			RecalculatePowerRequirement(chargeImmediatelly: true);
			MyEntityStat myEntityStat = (StatComp != null) ? StatComp.Health : null;
			if (myEntityStat != null)
			{
				if (m_savedHealth.HasValue)
				{
					myEntityStat.Value = m_savedHealth.Value;
				}
				myEntityStat.OnStatChanged += StatComp.OnHealthChanged;
			}
			if (m_breath != null)
			{
				m_breath.ForceUpdate();
			}
			if (m_currentMovementState == MyCharacterMovementEnum.Died)
			{
				Physics.ForceActivate();
			}
			base.UpdateOnceBeforeFrame();
			if (m_currentWeapon != null)
			{
				MyEntities.Remove((MyEntity)m_currentWeapon);
				EquipWeapon(m_currentWeapon);
			}
			if (ControllerInfo.Controller == null && m_savedPlayer.HasValue && m_savedPlayer.Value.SteamId != 0L && Sync.IsServer)
			{
				m_controlInfo.Value = m_savedPlayer.Value;
			}
			if (m_relativeDampeningEntityInit != 0L)
			{
				RelativeDampeningEntity = MyEntities.GetEntityByIdOrDefault(m_relativeDampeningEntityInit);
			}
			if (m_ladderIdInit.HasValue)
			{
				if (MyEntities.GetEntityById(m_ladderIdInit.Value) is MyLadder)
				{
					GetOnLadder_Implementation(m_ladderIdInit.Value, resetPosition: false, m_ladderInfoInit);
					m_ladderIdInit = null;
					m_ladderInfoInit = null;
				}
				else
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
			}
			UpdateAssetModifiers();
		}

		private void UpdateAssetModifiers()
		{
			if (m_assetModifiersLoaded || Sandbox.Engine.Platform.Game.IsDedicated || MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			long identityId = MySession.Static.LocalHumanPlayer.Identity.IdentityId;
			long playerIdentityId = GetPlayerIdentityId();
			if (playerIdentityId == identityId)
			{
				if (!IsDead && !IsBot)
				{
					MyLocalCache.LoadInventoryConfig(this, setModel: false);
					m_assetModifiersLoaded = true;
				}
			}
			else if (playerIdentityId != -1)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RefreshAssetModifiers, playerIdentityId, base.EntityId);
				m_assetModifiersLoaded = true;
			}
		}

		[Event(null, 8356)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void RefreshAssetModifiers(long playerId, long entityId)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Identity.IdentityId == playerId && MyGameService.InventoryItems != null)
			{
				List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
				foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
				{
					if (inventoryItem.IsInUse)
					{
						list.Add(inventoryItem);
					}
				}
				MyGameService.GetItemsCheckData(list, delegate(byte[] checkDataResult)
				{
					if (checkDataResult != null)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SendSkinData, entityId, checkDataResult);
					}
				});
			}
		}

		[Event(null, 8394)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void SendSkinData(long entityId, byte[] checkDataResult)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			MyCharacter myCharacter = MyEntities.GetEntityById(entityId) as MyCharacter;
			if (myCharacter != null)
			{
				if (myCharacter.Components.TryGet(out MyAssetModifierComponent _))
				{
					MyAssetModifierComponent.ApplyAssetModifierSync(entityId, checkDataResult, addToList: true);
				}
				MyEntity myEntity = myCharacter.CurrentWeapon as MyEntity;
				if (myEntity != null && myCharacter.CurrentWeapon != null && myCharacter.CurrentWeapon.IsSkinnable)
				{
					MyAssetModifierComponent.ApplyAssetModifierSync(myEntity.EntityId, checkDataResult, addToList: true);
				}
			}
		}

		public void SetLocalHeadAnimation(float? targetX, float? targetY, float length)
		{
			if (length > 0f)
			{
				if (m_headLocalYAngle < 0f)
				{
					m_headLocalYAngle = 0f - m_headLocalYAngle;
					m_headLocalYAngle = (m_headLocalYAngle + 180f) % 360f - 180f;
					m_headLocalYAngle = 0f - m_headLocalYAngle;
				}
				else
				{
					m_headLocalYAngle = (m_headLocalYAngle + 180f) % 360f - 180f;
				}
			}
			m_currentLocalHeadAnimation = 0f;
			m_localHeadAnimationLength = length;
			if (targetX.HasValue)
			{
				m_localHeadAnimationX = new Vector2(m_headLocalXAngle, targetX.Value);
			}
			else
			{
				m_localHeadAnimationX = null;
			}
			if (targetY.HasValue)
			{
				m_localHeadAnimationY = new Vector2(m_headLocalYAngle, targetY.Value);
			}
			else
			{
				m_localHeadAnimationY = null;
			}
		}

		public bool IsLocalHeadAnimationInProgress()
		{
			return m_currentLocalHeadAnimation >= 0f;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (IsUsing is MyCockpit)
			{
				_ = !(IsUsing as MyCockpit).BlockDefinition.EnableFirstPerson;
			}
			else
				_ = 0;
			if (m_currentMovementState == MyCharacterMovementEnum.Sitting)
			{
				EnableBag(m_enableBag);
			}
			if (m_currentWeapon != null)
			{
				MyEntities.Remove((MyEntity)m_currentWeapon);
				MyEntities.Add((MyEntity)m_currentWeapon);
			}
			UpdateShadowIgnoredObjects();
			MyPlayerCollection.UpdateControl(this);
		}

		public static MyCharacter CreateCharacter(MatrixD worldMatrix, Vector3 velocity, string characterName, string model, Vector3? colorMask, MyBotDefinition botDefinition, bool findNearPos = true, bool AIMode = false, MyCockpit cockpit = null, bool useInventory = true, long identityId = 0L, bool addDefaultItems = true)
		{
			Vector3D? vector3D = null;
			if (findNearPos)
			{
				vector3D = MyEntities.FindFreePlace(worldMatrix.Translation, 2f, 200, 5, 0.5f);
				if (!vector3D.HasValue)
				{
					vector3D = MyEntities.FindFreePlace(worldMatrix.Translation, 2f, 200, 5, 5f);
				}
			}
			if (vector3D.HasValue)
			{
				worldMatrix.Translation = vector3D.Value;
			}
			return CreateCharacterBase(worldMatrix, ref velocity, characterName, model, colorMask, AIMode, useInventory, botDefinition, identityId, addDefaultItems);
		}

		private static MyCharacter CreateCharacterBase(MatrixD worldMatrix, ref Vector3 velocity, string characterName, string model, Vector3? colorMask, bool AIMode, bool useInventory = true, MyBotDefinition botDefinition = null, long identityId = 0L, bool addDefaultItems = true)
		{
			MyCharacter myCharacter = new MyCharacter();
			MyObjectBuilder_Character myObjectBuilder_Character = Random();
			myObjectBuilder_Character.CharacterModel = (model ?? myObjectBuilder_Character.CharacterModel);
			if (colorMask.HasValue)
			{
				myObjectBuilder_Character.ColorMaskHSV = colorMask.Value;
			}
			myObjectBuilder_Character.JetpackEnabled = MySession.Static.CreativeMode;
			myObjectBuilder_Character.Battery = new MyObjectBuilder_Battery
			{
				CurrentCapacity = 1f
			};
			myObjectBuilder_Character.AIMode = AIMode;
			myObjectBuilder_Character.DisplayName = characterName;
			myObjectBuilder_Character.LinearVelocity = velocity;
			myObjectBuilder_Character.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix);
			myObjectBuilder_Character.CharacterGeneralDamageModifier = 1f;
			myObjectBuilder_Character.OwningPlayerIdentityId = identityId;
			myCharacter.Init(myObjectBuilder_Character);
			MyEntities.RaiseEntityCreated(myCharacter);
			MyEntities.Add(myCharacter);
			MyInventory inventory = myCharacter.GetInventory();
			if (useInventory)
			{
				if (inventory != null && addDefaultItems)
				{
					MyWorldGenerator.InitInventoryWithDefaults(inventory);
				}
			}
			else
			{
				botDefinition?.AddItems(myCharacter);
			}
			if (velocity.Length() > 0f)
			{
				myCharacter.JetpackComp?.EnableDampeners(enable: false);
			}
			return myCharacter;
		}

		public override string ToString()
		{
			return m_characterModel;
		}

		public void ShowOutOfAmmoNotification()
		{
			if (OutOfAmmoNotification == null)
			{
				OutOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 2000, "Red");
			}
			if (m_currentWeapon is MyEntity)
			{
				OutOfAmmoNotification.SetTextFormatArguments((m_currentWeapon as MyEntity).DisplayName);
			}
			MyHud.Notifications.Add(OutOfAmmoNotification);
		}

		public void UpdateCharacterPhysics(bool forceUpdate = false)
		{
			if (Physics != null && !Physics.Enabled)
			{
				return;
			}
			float num = 2f * MyPerGameSettings.PhysicsConvexRadius + 0.03f;
			float maxSpeedRelativeToShip = Math.Max(Definition.MaxSprintSpeed, Math.Max(Definition.MaxRunSpeed, Definition.MaxBackrunSpeed));
			if (Sync.IsServer || (IsClientPredicted && !ForceDisablePrediction))
			{
				if (Physics == null || Physics.IsStatic || forceUpdate)
				{
					Vector3 linearVelocity = Vector3.Zero;
					if (Physics != null)
					{
						linearVelocity = Physics.LinearVelocityLocal;
						Physics.Close();
					}
					MyPhysicsHelper.InitCharacterPhysics(center: new Vector3(0f, Definition.CharacterCollisionHeight / 2f, 0f), entity: this, materialType: MyMaterialType.CHARACTER, characterWidth: Definition.CharacterCollisionWidth * Definition.CharacterCollisionScale, characterHeight: Definition.CharacterCollisionHeight - Definition.CharacterCollisionWidth * Definition.CharacterCollisionScale - num, crouchHeight: Definition.CharacterCollisionCrouchHeight - Definition.CharacterCollisionWidth, ladderHeight: Definition.CharacterCollisionWidth - num, headSize: Definition.CharacterHeadSize * Definition.CharacterCollisionScale, headHeight: Definition.CharacterHeadHeight, linearDamping: 0.7f, angularDamping: 0.7f, collisionLayer: 18, rbFlag: RigidBodyFlag.RBF_DEFAULT, mass: MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(Definition.Mass) : Definition.Mass, isOnlyVertical: Definition.VerticalPositionFlyingOnly, maxSlope: Definition.MaxSlope, maxImpulse: Definition.ImpulseLimit, maxSpeedRelativeToShip: maxSpeedRelativeToShip, networkProxy: false, maxForce: Definition.MaxForce);
					if (Physics.CharacterProxy != null)
					{
						Physics.CharacterProxy.ContactPointCallback -= RigidBody_ContactPointCallback;
						Physics.CharacterProxy.ContactPointCallbackEnabled = true;
						Physics.CharacterProxy.ContactPointCallback += RigidBody_ContactPointCallback;
					}
					Physics.Enabled = true;
					Physics.LinearVelocity = linearVelocity;
					UpdateCrouchState();
				}
			}
			else if (Physics == null || !Physics.IsStatic || forceUpdate)
			{
				if (Physics != null)
				{
					Physics.Close();
				}
				float num2 = 1f;
				int num3 = 22;
				MyPhysicsHelper.InitCharacterPhysics(center: new Vector3(0f, Definition.CharacterCollisionHeight / 2f, 0f), entity: this, materialType: MyMaterialType.CHARACTER, characterWidth: Definition.CharacterCollisionWidth * Definition.CharacterCollisionScale * num2, characterHeight: Definition.CharacterCollisionHeight - Definition.CharacterCollisionWidth * Definition.CharacterCollisionScale * num2 - num, crouchHeight: Definition.CharacterCollisionCrouchHeight - Definition.CharacterCollisionWidth, ladderHeight: Definition.CharacterCollisionWidth - num, headSize: Definition.CharacterHeadSize * Definition.CharacterCollisionScale * num2, headHeight: Definition.CharacterHeadHeight, linearDamping: 0.7f, angularDamping: 0.7f, collisionLayer: (ushort)num3, rbFlag: RigidBodyFlag.RBF_STATIC, mass: 0f, isOnlyVertical: Definition.VerticalPositionFlyingOnly, maxSlope: Definition.MaxSlope, maxImpulse: Definition.ImpulseLimit, maxSpeedRelativeToShip: maxSpeedRelativeToShip, networkProxy: true, maxForce: Definition.MaxForce);
				Physics.Enabled = true;
			}
		}

		public void GetNetState(out MyCharacterClientState state)
		{
			state.HeadX = HeadLocalXAngle;
			state.HeadY = HeadLocalYAngle;
			state.MovementState = GetCurrentMovementState();
			state.MovementFlags = MovementFlags;
			bool flag = JetpackComp != null;
			state.Jetpack = (flag && JetpackComp.TurnedOn);
			state.Dampeners = (flag && JetpackComp.DampenersTurnedOn);
			state.TargetFromCamera = TargetFromCamera;
			state.MoveIndicator = MoveIndicator;
			Quaternion quaternion = state.Rotation = Quaternion.CreateFromRotationMatrix(Entity.WorldMatrix);
			state.CharacterState = m_currentCharacterState;
			state.SupportNormal = (IsOnLadder ? m_ladderIncrementToBase : ((Physics != null && Physics.CharacterProxy != null) ? Physics.CharacterProxy.SupportNormal : Vector3.Zero));
			state.MovementSpeed = m_currentSpeed;
			state.MovementDirection = m_currentMovementDirection;
			state.IsOnLadder = IsOnLadder;
			state.Valid = true;
		}

		public void SetNetState(ref MyCharacterClientState state)
		{
			if (IsDead || (IsUsing != null && !IsOnLadder) || base.Closed)
			{
				return;
			}
			bool flag = ControllerInfo.IsLocallyControlled();
			if (Sync.IsServer || !flag)
			{
				if ((state.MovementState == MyCharacterMovementEnum.LadderUp || state.MovementState == MyCharacterMovementEnum.LadderOut) && GetCurrentMovementState() == MyCharacterMovementEnum.Ladder && state.IsOnLadder)
				{
					state.MoveIndicator.Z = -1f;
				}
				if (state.MovementState == MyCharacterMovementEnum.LadderDown && GetCurrentMovementState() == MyCharacterMovementEnum.Ladder && state.IsOnLadder)
				{
					state.MoveIndicator.Z = 1f;
				}
				SetHeadLocalXAngle(state.HeadX);
				SetHeadLocalYAngle(state.HeadY);
				MyCharacterJetpackComponent jetpackComp = JetpackComp;
				if (jetpackComp != null && !IsOnLadder)
				{
					if (state.Jetpack != JetpackComp.TurnedOn)
					{
						jetpackComp.TurnOnJetpack(state.Jetpack, fromInit: true);
					}
					if (state.Dampeners != JetpackComp.DampenersTurnedOn)
					{
						jetpackComp.EnableDampeners(state.Dampeners);
					}
				}
				if (GetCurrentMovementState() != state.MovementState && state.MovementState == MyCharacterMovementEnum.LadderOut)
				{
					TriggerCharacterAnimationEvent("LadderOut", sync: false);
				}
				if ((IsOnLadder && state.IsOnLadder) || (!IsOnLadder && !state.IsOnLadder))
				{
					CacheMove(ref state.MoveIndicator, ref state.Rotation);
					if (IsOnLadder)
					{
						m_ladderIncrementToBaseServer = state.SupportNormal;
					}
				}
				MovementFlags = (state.MovementFlags | (MovementFlags & MyCharacterMovementFlags.Jump));
				if (!IsOnLadder)
				{
					SetCurrentMovementState(state.MovementState);
				}
			}
			if (Sync.IsServer)
			{
				TargetFromCamera = state.TargetFromCamera;
			}
			if (!Sync.IsServer && (!IsClientPredicted || !flag))
			{
				if (m_previousMovementState == MyCharacterMovementEnum.Jump && state.CharacterState == HkCharacterStateType.HK_CHARACTER_ON_GROUND)
				{
					StopFalling();
				}
				m_currentSpeed = state.MovementSpeed;
				m_currentMovementDirection = state.MovementDirection;
				OnCharacterStateChanged(state.CharacterState);
				if (!IsOnLadder)
				{
					Physics.SupportNormal = state.SupportNormal;
				}
			}
		}

		public void UpdateMovementAndFlags(MyCharacterMovementEnum movementState, MyCharacterMovementFlags flags)
		{
			if (m_currentMovementState != movementState && Physics != null)
			{
				m_movementFlags = flags;
				SwitchAnimation(movementState);
				SetCurrentMovementState(movementState);
			}
		}

		private void SwitchToWeaponSuccess(MyDefinitionId? weapon, uint? inventoryItemId, long weaponEntityId)
		{
			if (!base.Closed)
			{
				if (!IsDead)
				{
					SwitchToWeaponInternal(weapon, updateSync: false, inventoryItemId, weaponEntityId);
				}
				if (this.OnWeaponChanged != null)
				{
					this.OnWeaponChanged(this, null);
				}
			}
		}

		private void UpdateLeftHandItemPosition()
		{
			MatrixD worldMatrix = base.AnimationController.CharacterBones[m_leftHandItemBone].AbsoluteTransform * base.WorldMatrix;
			Vector3D up = worldMatrix.Up;
			worldMatrix.Up = worldMatrix.Forward;
			worldMatrix.Forward = up;
			worldMatrix.Right = Vector3D.Cross(worldMatrix.Forward, worldMatrix.Up);
			m_leftHandItem.WorldMatrix = worldMatrix;
		}

		public void ChangeModelAndColor(string model, Vector3 colorMaskHSV, bool resetToDefault = false, long caller = 0L)
		{
			if (ResponsibleForUpdate(Sync.Clients.LocalClient))
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.ChangeModel_Implementation, model, colorMaskHSV, resetToDefault, caller);
			}
		}

		[Event(null, 8985)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[Broadcast]
		private void ChangeModel_Implementation(string model, Vector3 colorMaskHSV, bool resetToDefault, long caller)
		{
			bool flag = m_characterModel != model;
			ChangeModelAndColorInternal(model, colorMaskHSV);
			if (MySession.Static.LocalPlayerId != caller)
			{
				return;
			}
			MyGuiScreenLoadInventory.ResetOnFinish(model, resetToDefault);
			if (!flag || m_characterDefinition == null || !(m_characterDefinition.Skeleton == "Humanoid"))
			{
				return;
			}
			MyLocalCache.LoadInventoryConfig(this, setModel: false, setColor: false);
			MyEntity myEntity;
			if ((myEntity = (CurrentWeapon as MyEntity)) != null)
			{
				MyAssetModifierComponent myAssetModifierComponent = myEntity.Components.Get<MyAssetModifierComponent>();
				if (myAssetModifierComponent != null)
				{
					MyLocalCache.LoadInventoryConfig(myEntity, myAssetModifierComponent);
				}
			}
		}

		public void UpdateStoredGas(MyDefinitionId gasId, float fillLevel)
		{
			MyMultiplayer.RaiseEvent(this, (Func<MyCharacter, Action<SerializableDefinitionId, float>>)((MyCharacter x) => x.UpdateStoredGas_Implementation), (SerializableDefinitionId)gasId, fillLevel, default(EndpointId));
		}

		[Event(null, 9013)]
		[Reliable]
		[Broadcast]
		private void UpdateStoredGas_Implementation(SerializableDefinitionId gasId, float fillLevel)
		{
			if (OxygenComponent != null)
			{
				MyDefinitionId gasId2 = gasId;
				OxygenComponent.UpdateStoredGasLevel(ref gasId2, fillLevel);
			}
		}

		public void UpdateOxygen(float oxygenAmount)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnUpdateOxygen, oxygenAmount);
		}

		[Event(null, 9028)]
		[Reliable]
		[Broadcast]
		private void OnUpdateOxygen(float oxygenAmount)
		{
			if (OxygenComponent != null)
			{
				OxygenComponent.SuitOxygenAmount = oxygenAmount;
			}
		}

		public void SendRefillFromBottle(MyDefinitionId gasId)
		{
			MyMultiplayer.RaiseEvent(this, (Func<MyCharacter, Action<SerializableDefinitionId>>)((MyCharacter x) => x.OnRefillFromBottle), (SerializableDefinitionId)gasId, default(EndpointId));
		}

		[Event(null, 9042)]
		[Reliable]
		[Broadcast]
		private void OnRefillFromBottle(SerializableDefinitionId gasId)
		{
			if (this == MySession.Static.LocalCharacter)
			{
				_ = OxygenComponent;
			}
		}

		public void PlaySecondarySound(MyCueId soundId)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnSecondarySoundPlay, soundId);
		}

		[Event(null, 9056)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		private void OnSecondarySoundPlay(MyCueId soundId)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				SoundComp.StartSecondarySound(soundId);
			}
		}

		internal void ChangeModelAndColorInternal(string model, Vector3 colorMaskHSV)
		{
			if (base.Closed)
			{
				return;
			}
			if (model != m_characterModel && MyDefinitionManager.Static.Characters.TryGetValue(model, out MyCharacterDefinition result) && !string.IsNullOrEmpty(result.Model))
			{
				MyObjectBuilder_Character myObjectBuilder_Character = (MyObjectBuilder_Character)GetObjectBuilder();
				base.Components.Remove<MyInventoryBase>();
				base.Components.Remove<MyCharacterJetpackComponent>();
				base.Components.Remove<MyCharacterRagdollComponent>();
				base.AnimationController.Clear();
				MyModel modelOnlyData = MyModels.GetModelOnlyData(result.Model);
				if (modelOnlyData == null)
				{
					return;
				}
				if (MySandboxGame.Static.UpdateThread == Thread.CurrentThread)
				{
					Render.CleanLights();
				}
				CloseInternal();
				if (!MyEntities.Remove(this))
				{
					MyEntities.UnregisterForUpdate(this);
					Render.RemoveRenderObjects();
				}
				if (Physics != null)
				{
					Physics.Close();
					Physics = null;
				}
				m_characterModel = model;
				Render.ModelStorage = modelOnlyData;
				myObjectBuilder_Character.CharacterModel = model;
				myObjectBuilder_Character.EntityId = 0L;
				if (myObjectBuilder_Character.HandWeapon != null)
				{
					myObjectBuilder_Character.HandWeapon.EntityId = 0L;
				}
				if (m_breath != null)
				{
					m_breath.Close();
					m_breath = null;
				}
				float num = (StatComp != null) ? StatComp.HealthRatio : 1f;
				float headLocalXAngle = m_headLocalXAngle;
				float headLocalYAngle = m_headLocalYAngle;
				MatrixD worldMatrix = base.PositionComp.WorldMatrix;
				myObjectBuilder_Character.PositionAndOrientation = null;
				Init(myObjectBuilder_Character);
				base.PositionComp.SetWorldMatrix(worldMatrix);
				this.GetInventory().ResetVolume();
				InitInventory(myObjectBuilder_Character);
				m_headLocalXAngle = headLocalXAngle;
				m_headLocalYAngle = headLocalYAngle;
				if (StatComp != null && StatComp.Health != null)
				{
					StatComp.Health.Value = StatComp.Health.MaxValue - StatComp.Health.MaxValue * (1f - num);
				}
				SwitchAnimation(myObjectBuilder_Character.MovementState, checkState: false);
				if (m_currentWeapon != null)
				{
					m_currentWeapon.OnControlAcquired(this);
				}
				if (base.Parent == null)
				{
					MyEntities.Add(this);
				}
				else if (!base.InScene)
				{
					OnAddedToScene(this);
				}
				MyPlayer myPlayer = TryGetPlayer();
				if (myPlayer != null && myPlayer.Identity != null)
				{
					myPlayer.Identity.ChangeCharacter(this);
				}
				SuitRechargeDistributor.UpdateBeforeSimulation();
			}
			Render.ColorMaskHsv = colorMaskHSV;
			if (MySession.Static.LocalHumanPlayer != null)
			{
				MySession.Static.LocalHumanPlayer.Identity.SetColorMask(colorMaskHSV);
			}
		}

		public void SetPhysicsEnabled(bool enabled)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.EnablePhysics, enabled);
		}

		[Event(null, 9180)]
		[Reliable]
		[Broadcast]
		private void EnablePhysics(bool enabled)
		{
			Physics.Enabled = enabled;
		}

		public MyRelationsBetweenPlayerAndBlock GetRelationTo(long playerId)
		{
			return MyPlayer.GetRelationBetweenPlayers(GetPlayerIdentityId(), playerId);
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
			if (MyPerGameSettings.TerminalEnabled)
			{
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, myCharacter, this);
			}
			if (MyPerGameSettings.GUI.InventoryScreen != null && IsDead)
			{
				MyInventoryAggregate myInventoryAggregate = base.Components.Get<MyInventoryAggregate>();
				if (myInventoryAggregate != null)
				{
					myCharacter.ShowAggregateInventoryScreen(myInventoryAggregate);
				}
			}
		}

		MyActionDescription IMyUseObject.GetActionInfo(UseActionEnum actionEnum)
		{
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			MyActionDescription result = default(MyActionDescription);
			result.Text = MySpaceTexts.NotificationHintPressToOpenInventory;
			result.FormatParams = new object[2]
			{
				string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.INVENTORY), "]"),
				base.DisplayName
			};
			result.IsTextControlHint = true;
			result.JoystickText = MyCommonTexts.NotificationHintJoystickPressToOpenInventory;
			result.JoystickFormatParams = new object[2]
			{
				MyControllerHelper.GetCodeForControl(context, MyControlsSpace.INVENTORY),
				base.DisplayName
			};
			result.ShowForGamepad = true;
			return result;
		}

		void IMyUseObject.OnSelectionLost()
		{
		}

		public void SwitchLandingGears()
		{
		}

		public void OnInventoryBreak()
		{
		}

		public void OnDestroy()
		{
			Die();
		}

		void IMyCameraController.ControlCamera(MyCamera currentCamera)
		{
			MatrixD viewMatrix = GetViewMatrix();
			currentCamera.SetViewMatrix(viewMatrix);
			currentCamera.CameraSpring.Enabled = (!IsInFirstPersonView && !ForceFirstPersonCamera);
			EnableHead(!ControllerInfo.IsLocallyControlled() || (!IsInFirstPersonView && !ForceFirstPersonCamera));
		}

		void IMyCameraController.Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
			Rotate(rotationIndicator, rollIndicator);
		}

		void IMyCameraController.RotateStopped()
		{
			RotateStopped();
		}

		void IMyCameraController.OnAssumeControl(IMyCameraController previousCameraController)
		{
			OnAssumeControl(previousCameraController);
		}

		void IMyCameraController.OnReleaseControl(IMyCameraController newCameraController)
		{
			OnReleaseControl(newCameraController);
			if (base.InScene)
			{
				EnableHead(enabled: true);
			}
		}

		bool IMyCameraController.HandleUse()
		{
			return false;
		}

		bool IMyCameraController.HandlePickUp()
		{
			return false;
		}

		MatrixD VRage.Game.ModAPI.Interfaces.IMyControllableEntity.GetHeadMatrix(bool includeY, bool includeX, bool forceHeadAnim, bool forceHeadBone)
		{
			return GetHeadMatrix(includeY, includeX, forceHeadAnim);
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotateStopped()
		{
			MoveAndRotateStopped();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Use()
		{
			Use();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.UseContinues()
		{
			UseContinues();
		}

		void IMyControllableEntity.UseFinished()
		{
			UseFinished();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUp()
		{
			PickUp();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUpContinues()
		{
			PickUpContinues();
		}

		void IMyControllableEntity.PickUpFinished()
		{
			PickUpFinished();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Jump(Vector3 moveIndicator)
		{
			Jump(moveIndicator);
			if (!Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.Jump, moveIndicator);
			}
		}

		void IMyControllableEntity.Sprint(bool enabled)
		{
			Sprint(enabled);
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Up()
		{
			Up();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Crouch()
		{
			Crouch();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Down()
		{
			Down();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowInventory()
		{
			ShowInventory();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowTerminal()
		{
			ShowTerminal();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchThrusts()
		{
			MyCharacterJetpackComponent jetpackComp = JetpackComp;
			if (jetpackComp != null && HasEnoughSpaceToStandUp())
			{
				jetpackComp.SwitchThrusts();
			}
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchDamping()
		{
			MyCharacterJetpackComponent jetpackComp = JetpackComp;
			if (jetpackComp != null)
			{
				jetpackComp.SwitchDamping();
				if (!jetpackComp.DampenersEnabled)
				{
					RelativeDampeningEntity = null;
				}
			}
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLights()
		{
			SwitchLights();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLandingGears()
		{
			SwitchLandingGears();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchReactors()
		{
			SwitchReactors();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHelmet()
		{
			if (Sync.IsServer || MySession.Static.LocalCharacter == this)
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnSwitchHelmet);
			}
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Die()
		{
			Die();
		}

		void IMyDestroyableObject.OnDestroy()
		{
			OnDestroy();
		}

		bool IMyDestroyableObject.DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId)
		{
			return DoDamage(damage, damageType, sync, attackerId);
		}

		public static void Preload()
		{
			Parallel.ForEach(MyDefinitionManager.Static.GetAnimationDefinitions(), delegate(MyAnimationDefinition animation)
			{
				string animationModel = animation.AnimationModel;
				if (!string.IsNullOrEmpty(animationModel))
				{
					MyModels.GetModelOnlyAnimationData(animationModel);
				}
			});
			if (MyModelImporter.LINEAR_KEYFRAME_REDUCTION_STATS)
			{
				Dictionary<string, List<MyModelImporter.ReductionInfo>> reductionStats = MyModelImporter.ReductionStats;
				List<float> list = new List<float>();
				foreach (KeyValuePair<string, List<MyModelImporter.ReductionInfo>> item in reductionStats)
				{
					foreach (MyModelImporter.ReductionInfo item2 in item.Value)
					{
						list.Add((float)item2.OptimizedKeys / (float)item2.OriginalKeys);
					}
				}
				list.Average();
			}
		}

		bool IMyUseObject.HandleInput()
		{
			MyCharacterDetectorComponent myCharacterDetectorComponent = base.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent != null && myCharacterDetectorComponent.UseObject != null)
			{
				return myCharacterDetectorComponent.UseObject.HandleInput();
			}
			return false;
		}

		public MyEntityCameraSettings GetCameraEntitySettings()
		{
			return m_cameraSettingsWhenAlive;
		}

		private void ResetMovement()
		{
			MoveIndicator = Vector3.Zero;
			RotationIndicator = Vector2.Zero;
			RollIndicator = 0f;
		}

		public float GetSuitGasFillLevel(MyDefinitionId gasDefinitionId)
		{
			return OxygenComponent.GetGasFillLevel(gasDefinitionId);
		}

		private void KillCharacter(MyDamageInformation damageInfo)
		{
			Kill(sync: false, damageInfo);
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnKillCharacter, damageInfo, Physics.LinearVelocity);
		}

		[Event(null, 10019)]
		[Reliable]
		[Broadcast]
		private void OnKillCharacter(MyDamageInformation damageInfo, Vector3 lastLinearVelocity)
		{
			m_deathLinearVelocityFromSever = lastLinearVelocity;
			Kill(sync: false, damageInfo);
		}

		[Event(null, 10026)]
		[Reliable]
		[Broadcast]
		public void SpawnCharacterRelative(long RelatedEntity, Vector3 DeltaPosition)
		{
			if (RelatedEntity != 0L && MyEntities.TryGetEntityById(RelatedEntity, out MyEntity entity))
			{
				Physics.LinearVelocity = entity.Physics.LinearVelocity;
				Physics.AngularVelocity = entity.Physics.AngularVelocity;
				MatrixD matrixD = Matrix.CreateTranslation(DeltaPosition) * entity.WorldMatrix;
				base.PositionComp.SetPosition(matrixD.Translation);
			}
		}

		public void SetPlayer(MyPlayer player, bool update = true)
		{
			if (Sync.IsServer)
			{
				m_controlInfo.Value = player.Id;
				if (update)
				{
					MyPlayerCollection.ChangePlayerCharacter(player, this, this);
				}
				m_savedPlayer = player.Id;
			}
		}

		private void ControlChanged()
		{
			if (Sync.IsServer || IsDead)
			{
				return;
			}
			if (m_controlInfo.Value.SteamId != 0L && (ControllerInfo.Controller == null || ControllerInfo.Controller.Player.Id != m_controlInfo.Value))
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(m_controlInfo.Value);
				if (playerById != null)
				{
					MyPlayerCollection.ChangePlayerCharacter(playerById, this, this);
					if (playerById.Controller != null && playerById.Controller.ControlledEntity != null)
					{
						IsUsing = (playerById.Controller.ControlledEntity as MyEntity);
					}
					if (m_usingEntity != null && playerById != null && Sync.Players.GetControllingPlayer(m_usingEntity) != playerById)
					{
						Sync.Players.SetControlledEntityLocally(playerById.Id, m_usingEntity);
					}
				}
			}
			if (!IsDead && this == MySession.Static.LocalCharacter)
			{
				MySpectatorCameraController.Static.Position = base.PositionComp.GetPosition();
			}
		}

		private void PromotedChanged()
		{
		}

		public bool ResponsibleForUpdate(MyNetworkClient player)
		{
			if (Sync.Players == null)
			{
				return false;
			}
			MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(this);
			if (controllingPlayer == null && CurrentRemoteControl != null)
			{
				controllingPlayer = Sync.Players.GetControllingPlayer(CurrentRemoteControl as MyEntity);
			}
			if (controllingPlayer == null)
			{
				return player.IsGameServer();
			}
			return controllingPlayer.Client == player;
		}

		private void StartShooting(Vector3 direction, MyShootActionEnum action, bool doubleClick)
		{
			ShootDirection = direction;
			m_shootDoubleClick = doubleClick;
			OnBeginShoot(action);
		}

		public void BeginShootSync(Vector3 direction, MyShootActionEnum action = MyShootActionEnum.PrimaryAction, bool doubleClick = false)
		{
			StartShooting(direction, action, doubleClick);
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.ShootBeginCallback, direction, action, doubleClick);
			if (MyFakes.SIMULATE_QUICK_TRIGGER)
			{
				EndShootInternal(action);
			}
		}

		[Event(null, 10146)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		private void ShootBeginCallback(Vector3 direction, MyShootActionEnum action, bool doubleClick)
		{
			if (!Sync.IsServer || !MyEventContext.Current.IsLocallyInvoked)
			{
				StartShooting(direction, action, doubleClick);
			}
		}

		private void StopShooting(MyShootActionEnum action)
		{
			m_isShooting[(uint)action] = false;
			OnEndShoot(action);
		}

		private void GunDoubleClicked(MyShootActionEnum action)
		{
			OnGunDoubleClicked(action);
		}

		public void EndShootSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
		{
			if (!MyFakes.SIMULATE_QUICK_TRIGGER)
			{
				EndShootInternal(action);
			}
		}

		public void GunDoubleClickedSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
		{
			GunDoubleClickedInternal(action);
		}

		private void EndShootInternal(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.ShootEndCallback, action);
			StopShooting(action);
		}

		private void GunDoubleClickedInternal(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.GunDoubleClickedCallback, action);
			GunDoubleClicked(action);
		}

		[Event(null, 10191)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		private void ShootEndCallback(MyShootActionEnum action)
		{
			if (!Sync.IsServer || !MyEventContext.Current.IsLocallyInvoked)
			{
				StopShooting(action);
			}
		}

		[Event(null, 10201)]
		[Reliable]
		[Server]
		[BroadcastExcept]
		private void GunDoubleClickedCallback(MyShootActionEnum action)
		{
			if (!Sync.IsServer || !MyEventContext.Current.IsLocallyInvoked)
			{
				GunDoubleClicked(action);
			}
		}

		public void UpdateShootDirection(Vector3 direction, int multiplayerUpdateInterval)
		{
			MatrixD headMatrix = GetHeadMatrix(includeY: false, !JetpackRunning);
			if (direction.Dot(headMatrix.Forward) < 0.996795f)
			{
				direction = headMatrix.Forward;
			}
			if (multiplayerUpdateInterval != 0 && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastShootDirectionUpdate > multiplayerUpdateInterval)
			{
				if (ControllerInfo != null && ControllerInfo.IsLocallyControlled())
				{
					MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.ShootDirectionChangeCallback, direction);
				}
				m_lastShootDirectionUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			ShootDirection = direction;
		}

		[Event(null, 10233)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[BroadcastExcept]
		private void ShootDirectionChangeCallback(Vector3 direction)
		{
			if (ControllerInfo == null || !ControllerInfo.IsLocallyControlled())
			{
				ShootDirection = direction;
			}
		}

		[Event(null, 10242)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		private void OnSwitchAmmoMagazineRequest()
		{
			if (((IMyControllableEntity)this).CanSwitchAmmoMagazine())
			{
				SwitchAmmoMagazineSuccess();
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnSwitchAmmoMagazineSuccess);
			}
		}

		[Event(null, 10254)]
		[Reliable]
		[Broadcast]
		private void OnSwitchAmmoMagazineSuccess()
		{
			SwitchAmmoMagazineSuccess();
		}

		private void RequestSwitchToWeapon(MyDefinitionId? weapon, uint? inventoryItemId)
		{
			SerializableDefinitionId? arg = weapon;
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.SwitchToWeaponMessage, arg, inventoryItemId);
		}

		[Event(null, 10266)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		private void SwitchToWeaponMessage(SerializableDefinitionId? weapon, uint? inventoryItemId)
		{
			if (!CanSwitchToWeapon(weapon))
			{
				return;
			}
			if (inventoryItemId.HasValue)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory == null)
				{
					return;
				}
				MyPhysicalInventoryItem? itemByID = inventory.GetItemByID(inventoryItemId.Value);
				if (itemByID.HasValue)
				{
					MyDefinitionId? myDefinitionId = MyDefinitionManager.Static.ItemIdFromWeaponId(weapon.Value);
					if (myDefinitionId.HasValue && itemByID.Value.Content.GetObjectId() == myDefinitionId.Value)
					{
						long num = MyEntityIdentifier.AllocateId();
						SwitchToWeaponSuccess(weapon, inventoryItemId, num);
						MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnSwitchToWeaponSuccess, weapon, inventoryItemId, num);
					}
				}
			}
			else if (weapon.HasValue)
			{
				long num2 = MyEntityIdentifier.AllocateId();
				SwitchToWeaponSuccess(weapon, inventoryItemId, num2);
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnSwitchToWeaponSuccess, weapon, inventoryItemId, num2);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.UnequipWeapon);
			}
		}

		[Event(null, 10304)]
		[Reliable]
		[Broadcast]
		private void OnSwitchToWeaponSuccess(SerializableDefinitionId? weapon, uint? inventoryItemId, long weaponEntityId)
		{
			SwitchToWeaponSuccess(weapon, inventoryItemId, weaponEntityId);
		}

		public void SendAnimationCommand(ref MyAnimationCommand command)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnAnimationCommand, command);
		}

		[Event(null, 10317)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[Broadcast]
		private void OnAnimationCommand(MyAnimationCommand command)
		{
			AddCommand(command);
		}

		public void SendAnimationEvent(string eventName, string[] layers = null)
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnAnimationEvent, eventName, layers);
		}

		[Event(null, 10328)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[Broadcast]
		private void OnAnimationEvent(string eventName, [Nullable] string[] layers)
		{
			if (UseNewAnimationSystem)
			{
				base.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName), layers);
			}
		}

		public void SendRagdollTransforms(Matrix world, Matrix[] localBodiesTransforms)
		{
			if (ResponsibleForUpdate(Sync.Clients.LocalClient))
			{
				Vector3 translation = world.Translation;
				int num = localBodiesTransforms.Length;
				Quaternion arg = Quaternion.CreateFromRotationMatrix(world.GetOrientation());
				Vector3[] array = new Vector3[num];
				Quaternion[] array2 = new Quaternion[num];
				for (int i = 0; i < localBodiesTransforms.Length; i++)
				{
					array[i] = localBodiesTransforms[i].Translation;
					array2[i] = Quaternion.CreateFromRotationMatrix(localBodiesTransforms[i].GetOrientation());
				}
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.OnRagdollTransformsUpdate, num, array, array2, arg, translation);
			}
		}

		[Event(null, 10355)]
		[Reliable]
		[Broadcast]
		private void OnRagdollTransformsUpdate(int transformsCount, Vector3[] transformsPositions, Quaternion[] transformsOrientations, Quaternion worldOrientation, Vector3 worldPosition)
		{
			MyCharacterRagdollComponent myCharacterRagdollComponent = base.Components.Get<MyCharacterRagdollComponent>();
			if (myCharacterRagdollComponent != null && Physics != null && Physics.Ragdoll != null && myCharacterRagdollComponent.RagdollMapper != null && Physics.Ragdoll.InWorld && myCharacterRagdollComponent.RagdollMapper.IsActive)
			{
				Matrix worldMatrix = Matrix.CreateFromQuaternion(worldOrientation);
				worldMatrix.Translation = worldPosition;
				Matrix[] array = new Matrix[transformsCount];
				for (int i = 0; i < transformsCount; i++)
				{
					array[i] = Matrix.CreateFromQuaternion(transformsOrientations[i]);
					array[i].Translation = transformsPositions[i];
				}
				myCharacterRagdollComponent.RagdollMapper.UpdateRigidBodiesTransformsSynced(transformsCount, worldMatrix, array);
			}
		}

		[Event(null, 10384)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[Broadcast]
		private void OnSwitchHelmet()
		{
			if (OxygenComponent != null)
			{
				OxygenComponent.SwitchHelmet();
				if (m_currentWeapon != null)
				{
					m_currentWeapon.UpdateSoundEmitter();
				}
			}
		}

		public Vector3 GetLocalWeaponPosition()
		{
			return WeaponPosition.LogicalPositionLocalSpace;
		}

		private void ToolHeadTransformChanged()
		{
			MyEngineerToolBase myEngineerToolBase = m_currentWeapon as MyEngineerToolBase;
			if (myEngineerToolBase != null && !ControllerInfo.IsLocallyControlled())
			{
				myEngineerToolBase.UpdateSensorPosition();
			}
		}

		public void SyncHeadToolTransform(ref MatrixD headMatrix)
		{
			if (ControllerInfo.IsLocallyControlled())
			{
				MatrixD m = headMatrix * base.PositionComp.WorldMatrixInvScaled;
				MyTransform myTransform = new MyTransform(m);
				myTransform.Rotation = Quaternion.Normalize(myTransform.Rotation);
			}
		}

		[Event(null, 10422)]
		[Reliable]
		[Client]
		public void SwitchJetpack()
		{
			if (JetpackComp != null)
			{
				JetpackComp.SwitchThrusts();
			}
		}

		public Quaternion GetRotation()
		{
			if (JetpackRunning)
			{
				MatrixD matrix = base.WorldMatrix;
				Quaternion.CreateFromRotationMatrix(ref matrix, out Quaternion result);
				return result;
			}
			if (Physics.CharacterProxy != null)
			{
				return Quaternion.CreateFromForwardUp(Physics.CharacterProxy.Forward, Physics.CharacterProxy.Up);
			}
			return Quaternion.CreateFromForwardUp(base.WorldMatrix.Forward, base.WorldMatrix.Up);
		}

		public void ApplyRotation(Quaternion rot)
		{
			if (!IsOnLadder)
			{
				MatrixD other = MatrixD.CreateFromQuaternion(rot);
				if (JetpackRunning && Physics.CharacterProxy != null)
				{
					float y = base.ModelCollision.BoundingBoxSizeHalf.Y;
					Vector3D value = Physics.GetWorldMatrix().Translation + base.WorldMatrix.Up * y;
					other.Translation = value - other.Up * y;
					IsRotating = !base.WorldMatrix.EqualsFast(ref other);
					base.WorldMatrix = other;
					ClearShapeContactPoints();
				}
				else if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.SetForwardAndUp(other.Forward, other.Up);
				}
			}
		}

		public override void SerializeControls(BitStream stream)
		{
			if (!IsDead)
			{
				stream.WriteBool(value: true);
				GetNetState(out MyCharacterClientState state);
				state.Serialize(stream);
				if (MyCompilationSymbols.EnableNetworkPositionTracking)
				{
					MoveIndicator.Equals(Vector3.Zero, 0.001f);
				}
			}
			else
			{
				stream.WriteBool(value: false);
			}
		}

		public override void DeserializeControls(BitStream stream, bool outOfOrder)
		{
			if (stream.ReadBool())
			{
				MyCharacterClientState lastClientState = new MyCharacterClientState(stream);
				if (!outOfOrder)
				{
					m_lastClientState = lastClientState;
					SetNetState(ref m_lastClientState);
				}
			}
			else
			{
				m_lastClientState.Valid = false;
			}
		}

		public override void ResetControls()
		{
			ResetMovement();
			m_lastClientState.Valid = false;
		}

		public override void ApplyLastControls()
		{
			if (m_lastClientState.Valid && (Sync.IsServer || !ControllerInfo.IsLocallyControlled()))
			{
				CacheMove(ref m_lastClientState.MoveIndicator, ref m_lastClientState.Rotation);
			}
		}

		private void relativeDampeningEntityClosed(MyEntity entity)
		{
			m_relativeDampeningEntity = null;
		}

		private void SetRelativeDampening(MyEntity entity)
		{
			RelativeDampeningEntity = entity;
			JetpackComp.EnableDampeners(enable: true);
			JetpackComp.TurnOnJetpack(newState: true);
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyPlayerCollection.SetDampeningEntityClient, base.EntityId, RelativeDampeningEntity.EntityId);
			}
		}

		private void UpdateOutsideTemperature()
		{
			MyCockpit myCockpit = base.Parent as MyCockpit;
			if (myCockpit != null && myCockpit.BlockDefinition.IsPressurized && myCockpit.IsWorking)
			{
				m_outsideTemperature = 0.5f;
				return;
			}
			float temperatureInPoint = MySectorWeatherComponent.GetTemperatureInPoint(base.PositionComp.GetPosition());
			float outsideTemperature = temperatureInPoint;
			if ((long)OxygenSourceGridEntityId != 0L)
			{
				outsideTemperature = MathHelper.Lerp(temperatureInPoint, 0.5f, OxygenLevelAtCharacterLocation);
			}
			float outsideTemperature2 = m_outsideTemperature;
			m_outsideTemperature = outsideTemperature;
			if (MySectorWeatherComponent.TemperatureToLevel(m_outsideTemperature) != MySectorWeatherComponent.TemperatureToLevel(outsideTemperature2))
			{
				RecalculatePowerRequirement();
			}
		}

		public float GetOutsideTemperature()
		{
			return m_outsideTemperature;
		}

		private bool RayCastGround()
		{
			float dEFAULT_GROUND_SEARCH_DISTANCE = MyConstants.DEFAULT_GROUND_SEARCH_DISTANCE;
			Vector3D vector3D = base.PositionComp.GetPosition() + base.PositionComp.WorldMatrix.Up * 0.5;
			Vector3D vector3D2 = vector3D + base.PositionComp.WorldMatrix.Down * dEFAULT_GROUND_SEARCH_DISTANCE;
			Vector3D value = base.PositionComp.WorldMatrix.Forward * 0.20000000298023224;
			Vector3D value2 = -base.PositionComp.WorldMatrix.Forward * 0.20000000298023224;
			m_hits.Clear();
			m_hits2.Clear();
			MyPhysics.CastRay(vector3D, vector3D2, m_hits2, 18);
			m_hits.AddRange(m_hits2);
			MyPhysics.CastRay(vector3D + value, vector3D2 + value, m_hits2, 18);
			m_hits.AddRange(m_hits2);
			MyPhysics.CastRay(vector3D + value2, vector3D2 + value2, m_hits2, 18);
			m_hits.AddRange(m_hits2);
			int i;
			for (i = 0; i < m_hits.Count && (m_hits[i].HkHitInfo.Body == null || m_hits[i].HkHitInfo.GetHitEntity() == Entity.Components); i++)
			{
			}
			if (m_hits.Count == 0)
			{
				m_standingOnGrid = null;
				m_standingOnVoxel = null;
			}
			if (i < m_hits.Count)
			{
				MyPhysics.HitInfo hitInfo = m_hits[i];
				VRage.ModAPI.IMyEntity myEntity = hitInfo.HkHitInfo.GetHitEntity();
				if (myEntity != null)
				{
					myEntity = myEntity.GetTopMostParent();
				}
				if (Vector3D.DistanceSquared(hitInfo.Position, vector3D) < (double)(dEFAULT_GROUND_SEARCH_DISTANCE * dEFAULT_GROUND_SEARCH_DISTANCE))
				{
					MyCubeGrid standingOnGrid = myEntity as MyCubeGrid;
					MyVoxelBase standingOnVoxel = myEntity as MyVoxelBase;
					m_standingOnGrid = standingOnGrid;
					m_standingOnVoxel = standingOnVoxel;
				}
			}
			m_hits.Clear();
			if (m_standingOnGrid == null)
			{
				return m_standingOnVoxel != null;
			}
			return true;
		}

		public void GetOnLadder(MyLadder ladder)
		{
			if (ResponsibleForUpdate(Sync.Clients.LocalClient))
			{
				MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.GetOnLadder_Request, ladder.EntityId);
			}
		}

		[Event(null, 80)]
		[Reliable]
		[Server]
		private void GetOnLadder_Request(long ladderId)
		{
			if (Sync.IsServer && MyEntities.TryGetEntityById(ladderId, out MyLadder entity))
			{
				MatrixD worldMatrix = entity.PositionComp.WorldMatrix;
				if (!CanPlaceCharacter(ref worldMatrix, useCharacterCenter: true, checkCharacters: true, this))
				{
					ulong value = MyEventContext.Current.Sender.Value;
					MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.GetOnLadder_Failed, new EndpointId(value));
				}
				else
				{
					MyMultiplayer.RaiseEvent<MyCharacter, long, bool, MyObjectBuilder_Character.LadderInfo?>(this, (MyCharacter x) => x.GetOnLadder_Implementation, entity.EntityId, arg3: true, null);
				}
			}
		}

		[Event(null, 102)]
		[Reliable]
		[Client]
		private void GetOnLadder_Failed()
		{
			if (m_ladderBlockedNotification == null && this == MySession.Static.LocalCharacter)
			{
				m_ladderBlockedNotification = new MyHudNotification(MySpaceTexts.NotificationHintLadderBlocked, 2500, "Red");
			}
			MyHud.Notifications.Add(m_ladderBlockedNotification);
		}

		[Event(null, 112)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[Broadcast]
		private void GetOnLadder_Implementation(long ladderId, bool resetPosition = true, MyObjectBuilder_Character.LadderInfo? newLadderInfo = null)
		{
			if (MyEntities.TryGetEntityById(ladderId, out MyLadder entity) && !IsOnLadder)
			{
				if (!IsClientPredicted)
				{
					ForceDisablePrediction = false;
					UpdatePredictionFlag();
				}
				MoveIndicator = Vector3.Zero;
				ChangeLadder(entity, resetPosition, newLadderInfo);
				StopFalling();
				SwitchToWeapon(null, null, sync: false);
				if (JetpackComp != null)
				{
					JetpackComp.TurnOnJetpack(newState: false);
				}
				SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
				if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.EnableLadderState(enable: true);
				}
				UpdateNearFlag();
				Physics.ClearSpeed();
				Physics.LinearVelocity = entity.CubeGrid.Physics.GetVelocityAtPoint(base.WorldMatrix.Translation);
				m_currentLadderStep = 0;
				m_stepsPerAnimation = 59;
				m_stepIncrement = 2f * entity.DistanceBetweenPoles / (float)m_stepsPerAnimation;
				StopUpperAnimation(0f);
				TriggerCharacterAnimationEvent("GetOnLadder", sync: false);
				if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.AtLadder = true;
				}
				UpdateLadderNotifications();
			}
		}

		public void GetOffLadder()
		{
			MyMultiplayer.RaiseEvent(this, (MyCharacter x) => x.GetOffLadder_Implementation);
		}

		[Event(null, 180)]
		[Reliable]
		[Server(ValidationType.Controlled)]
		[Broadcast]
		private void GetOffLadder_Implementation()
		{
			if (IsOnLadder && Physics != null && !IsDead)
			{
				MyLadder ladder = m_ladder;
				ChangeLadder(null);
				UpdateLadderNotifications();
				if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.AtLadder = false;
					Physics.CharacterProxy.EnableLadderState(enable: false);
				}
				m_currentLadderStep = 0;
				TriggerCharacterAnimationEvent("GetOffLadder", sync: false);
				if (m_currentMovementState == MyCharacterMovementEnum.LadderOut)
				{
					m_currentJumpTime = 0.2f;
					Stand();
				}
				else
				{
					StartFalling();
				}
				Vector3 linearVelocity = ladder.Parent.Physics.LinearVelocity;
				Physics.LinearVelocity = linearVelocity;
				if (!Vector3.IsZero(linearVelocity))
				{
					SetRelativeDampening(ladder.Parent);
				}
			}
		}

		private void AddLadderConstraint(MyCubeGrid ladderGrid)
		{
			MyCharacterProxy characterProxy = Physics.CharacterProxy;
			if (characterProxy != null)
			{
				characterProxy.GetHitRigidBody().UpdateMotionType(HkMotionType.Dynamic);
				m_constraintData = new HkFixedConstraintData();
				if (Sync.IsServer)
				{
					m_constraintBreakableData = new HkBreakableConstraintData(m_constraintData);
					m_constraintBreakableData.ReapplyVelocityOnBreak = false;
					m_constraintBreakableData.RemoveFromWorldOnBrake = false;
					m_constraintBreakableData.Threshold = 200f;
				}
				else if (m_constraintBreakableData != null)
				{
					m_constraintBreakableData = null;
				}
				m_constraintInstance = new HkConstraint(ladderGrid.Physics.RigidBody, characterProxy.GetHitRigidBody(), (HkConstraintData)(((object)m_constraintBreakableData) ?? ((object)m_constraintData)));
				ladderGrid.Physics.AddConstraint(m_constraintInstance);
				m_constraintInstance.SetVirtualMassInverse(Vector4.Zero, Vector4.One);
			}
		}

		private void CloseLadderConstraint(MyCubeGrid ladderGrid)
		{
			if (Physics.CharacterProxy != null)
			{
				if (m_constraintInstance != null && ladderGrid != null)
				{
					ladderGrid.Physics?.RemoveConstraint(m_constraintInstance);
					m_constraintInstance.Dispose();
					m_constraintInstance = null;
				}
				if (m_constraintBreakableData != null)
				{
					m_constraintData.Dispose();
					m_constraintBreakableData = null;
				}
				m_constraintData = null;
			}
		}

		private void UpdateLadderNotifications()
		{
			if (this != MySession.Static.LocalCharacter)
			{
				return;
			}
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MySpaceBindingCreator.CX_BASE;
			if (IsOnLadder)
			{
				if (m_ladderOffNotification == null)
				{
					m_ladderOffNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToGetDownFromLadder, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control);
					if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
					{
						m_ladderOffNotification.SetTextFormatArguments("[" + MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]");
					}
					else
					{
						m_ladderOffNotification.SetTextFormatArguments(MyControllerHelper.GetCodeForControl(context, MyControlsSpace.USE));
					}
					MyHud.Notifications.Add(m_ladderOffNotification);
				}
				if (m_ladderUpDownNotification == null)
				{
					m_ladderUpDownNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToClimbUpDown, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control);
					if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
					{
						m_ladderUpDownNotification.SetTextFormatArguments("[" + MyInput.Static.GetGameControl(MyControlsSpace.FORWARD).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]", "[" + MyInput.Static.GetGameControl(MyControlsSpace.BACKWARD).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]");
					}
					else
					{
						m_ladderUpDownNotification.SetTextFormatArguments(MyControllerHelper.GetCodeForControl(context, MyControlsSpace.FORWARD), MyControllerHelper.GetCodeForControl(context, MyControlsSpace.BACKWARD));
					}
					MyHud.Notifications.Add(m_ladderUpDownNotification);
				}
				if (m_ladderJumpOffNotification == null)
				{
					m_ladderJumpOffNotification = new MyHudNotification(MySpaceTexts.NotificationHintPressToJumpOffLadder, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Control);
					if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
					{
						m_ladderJumpOffNotification.SetTextFormatArguments("[" + MyInput.Static.GetGameControl(MyControlsSpace.JUMP).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]");
					}
					else
					{
						m_ladderJumpOffNotification.SetTextFormatArguments(MyControllerHelper.GetCodeForControl(context, MyControlsSpace.JUMP));
					}
					MyHud.Notifications.Add(m_ladderJumpOffNotification);
				}
			}
			else
			{
				if (m_ladderOffNotification != null)
				{
					MyHud.Notifications.Remove(m_ladderOffNotification);
					m_ladderOffNotification = null;
				}
				if (m_ladderUpDownNotification != null)
				{
					MyHud.Notifications.Remove(m_ladderUpDownNotification);
					m_ladderUpDownNotification = null;
				}
				if (m_ladderJumpOffNotification != null)
				{
					MyHud.Notifications.Remove(m_ladderJumpOffNotification);
					m_ladderJumpOffNotification = null;
				}
			}
		}

		private void StartStep(bool forceStartAnimation)
		{
			if (m_currentLadderStep == 0 || forceStartAnimation)
			{
				TriggerCharacterAnimationEvent((m_currentMovementState == MyCharacterMovementEnum.LadderUp) ? "LadderUp" : "LadderDown", sync: false);
			}
			if (m_currentLadderStep == 0)
			{
				m_currentLadderStep = m_stepsPerAnimation;
			}
		}

		private void UpdateLadder()
		{
			if (!IsOnLadder || m_ladder.MarkedForClose || m_needReconnectLadder)
			{
				return;
			}
			if (Sync.IsServer && m_constraintInstance != null && m_constraintBreakableData != null && m_constraintBreakableData.getIsBroken(m_constraintInstance))
			{
				GetOffLadder();
			}
			if (m_currentLadderStep > 0)
			{
				Vector3D translation = base.PositionComp.WorldMatrix.Translation;
				float num = m_stepIncrement;
				if (GetCurrentMovementState() == MyCharacterMovementEnum.LadderDown)
				{
					num = 0f - num;
				}
				if (Physics.CharacterProxy == null && Vector3.Distance(m_ladderIncrementToBase, m_ladderIncrementToBaseServer) > 0.13f)
				{
					m_ladderIncrementToBase = m_ladderIncrementToBaseServer;
				}
				m_ladderIncrementToBase.Y += num;
				Vector3 movementDelta = base.WorldMatrix.Up * num;
				bool isHit;
				MyLadder myLadder = CheckBottomLadder(translation, ref movementDelta, out isHit);
				MyLadder myLadder2 = CheckTopLadder(translation, ref movementDelta, out isHit);
				if ((m_currentMovementState == MyCharacterMovementEnum.LadderUp && myLadder2 != null) || (m_currentMovementState == MyCharacterMovementEnum.LadderDown && myLadder != null) || Physics.CharacterProxy == null || m_currentMovementState == MyCharacterMovementEnum.LadderOut)
				{
					MyLadder myLadder3 = CheckMiddleLadder(translation + base.WorldMatrix.Up * 0.10000000149011612, ref movementDelta);
					MyLadder myLadder4 = CheckMiddleLadder(translation - base.WorldMatrix.Up * 0.10000000149011612, ref movementDelta);
					if (myLadder3 == myLadder4 && myLadder4 != m_ladder && myLadder4 != null)
					{
						ChangeLadder(myLadder4);
					}
					if (Physics.CharacterProxy != null && m_currentLadderStep < 20 && m_currentMovementState == MyCharacterMovementEnum.LadderOut)
					{
						Vector3 vector = new Vector3(0f, 0.001f, 0.025f);
						m_ladderIncrementToBase.Y += vector.Y - num;
						m_ladderIncrementToBase.Z += vector.Z;
					}
					MatrixD characterLadderConstraint = m_baseMatrix * m_ladder.WorldMatrix;
					characterLadderConstraint.Translation += base.WorldMatrix.Up * m_ladderIncrementToBase.Y;
					characterLadderConstraint.Translation += base.WorldMatrix.Forward * m_ladderIncrementToBase.Z;
					if (Physics.CharacterProxy != null && m_constraintInstance != null)
					{
						SetCharacterLadderConstraint(characterLadderConstraint);
					}
				}
				m_currentLadderStep--;
				if (m_currentLadderStep == 0)
				{
					if (GetCurrentMovementState() == MyCharacterMovementEnum.LadderUp || GetCurrentMovementState() == MyCharacterMovementEnum.LadderDown)
					{
						SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
					}
					else if (GetCurrentMovementState() == MyCharacterMovementEnum.LadderOut && Sync.IsServer)
					{
						Vector3 linearVelocity = m_ladder.Parent.Physics.LinearVelocity;
						Vector3 position = m_ladder.StopMatrix.Translation;
						if (Vector3.Dot(base.WorldMatrix.Up, m_ladder.PositionComp.WorldMatrix.Up) < 0f)
						{
							position = new Vector3(position.X, 0f - position.Y, position.Z);
						}
						Vector3D pos = Vector3D.Transform(position, m_ladder.WorldMatrix) - base.WorldMatrix.Up * 0.20000000298023224;
						GetOffLadder();
						base.PositionComp.SetPosition(pos);
						if (Vector3.IsZero(Gravity))
						{
							Physics.LinearVelocity = linearVelocity + base.WorldMatrix.Down * 0.5;
						}
					}
				}
			}
			if (Physics.CharacterProxy == null && m_constraintInstance == null)
			{
				MatrixD worldMatrix = m_baseMatrix * m_ladder.WorldMatrix;
				worldMatrix.Translation += base.WorldMatrix.Up * m_ladderIncrementToBase.Y;
				worldMatrix.Translation += base.WorldMatrix.Forward * m_ladderIncrementToBase.Z;
				base.PositionComp.SetWorldMatrix(worldMatrix);
			}
		}

		private void GetOffLadderFromMovement()
		{
			Vector3D pos = base.PositionComp.GetPosition();
			if (IsOnLadder)
			{
				pos = m_ladder.PositionComp.GetPosition() + base.WorldMatrix.Up * MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) * 0.60000002384185791 + base.WorldMatrix.Forward * 0.89999997615814209;
			}
			GetOffLadder();
			base.PositionComp.SetPosition(pos);
		}

		private MyLadder CheckTopLadder(Vector3D position, ref Vector3 movementDelta, out bool isHit)
		{
			Vector3D from = position + movementDelta + base.WorldMatrix.Up * 1.75 - base.WorldMatrix.Forward * 0.20000000298023224;
			Vector3D to = from + base.WorldMatrix.Up * 0.40000000596046448 + base.WorldMatrix.Forward * 1.5;
			return FindLadder(ref from, ref to, out isHit);
		}

		private MyLadder CheckBottomLadder(Vector3D position, ref Vector3 movementDelta, out bool isHit)
		{
			Vector3D from = position + base.WorldMatrix.Up * 0.20000000298023224 + movementDelta - base.WorldMatrix.Forward * 0.20000000298023224;
			Vector3D to = from + base.WorldMatrix.Down * 0.40000000596046448 + base.WorldMatrix.Forward * 1.5;
			return FindLadder(ref from, ref to, out isHit);
		}

		private MyLadder CheckMiddleLadder(Vector3D position, ref Vector3 movementDelta)
		{
			Vector3D from = position + movementDelta + base.WorldMatrix.Up * 0.800000011920929 - base.WorldMatrix.Forward * 0.20000000298023224;
			Vector3D to = from + base.WorldMatrix.Forward * 1.5;
			return FindLadder(ref from, ref to);
		}

		private MyLadder FindLadder(ref Vector3D from, ref Vector3D to)
		{
			bool isHit;
			return FindLadder(ref from, ref to, out isHit);
		}

		private MyLadder FindLadder(ref Vector3D from, ref Vector3D to, out bool isHit)
		{
			isHit = false;
			LineD line = new LineD(from, to);
			MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, this, null, ignoreChildren: false, ignoreFloatingObjects: false);
			MyLadder myLadder = null;
			if (intersectionWithLine.HasValue)
			{
				isHit = true;
				if (intersectionWithLine.Value.Entity is MyCubeGrid)
				{
					if (intersectionWithLine.Value.UserObject != null)
					{
						MySlimBlock cubeBlock = (intersectionWithLine.Value.UserObject as MyCube).CubeBlock;
						if (cubeBlock != null && cubeBlock.FatBlock != null)
						{
							MyLadder myLadder2 = cubeBlock.FatBlock as MyLadder;
							if (myLadder2 != null)
							{
								myLadder = myLadder2;
							}
						}
					}
				}
				else
				{
					MyLadder myLadder3 = intersectionWithLine.Value.Entity as MyLadder;
					if (myLadder3 != null)
					{
						myLadder = myLadder3;
					}
				}
			}
			if (myLadder == null)
			{
				return null;
			}
			if (Ladder != null)
			{
				if (myLadder == Ladder)
				{
					return myLadder;
				}
				if (myLadder.GetTopMostParent() != Ladder.GetTopMostParent())
				{
					return myLadder;
				}
				if (myLadder.Orientation.Forward != Ladder.Orientation.Forward)
				{
					return null;
				}
			}
			return myLadder;
		}

		private Vector3 ProceedLadderMovement(Vector3 moveIndicator)
		{
			Vector3D position = base.PositionComp.GetPosition();
			if (Physics.CharacterProxy == null)
			{
				MatrixD matrixD = m_baseMatrix * m_ladder.WorldMatrix;
				matrixD.Translation += base.WorldMatrix.Up * m_ladderIncrementToBase.Y;
				matrixD.Translation += base.WorldMatrix.Forward * m_ladderIncrementToBase.Z;
				position = matrixD.Translation;
			}
			Vector3 movementDelta = Vector3.Zero;
			if (moveIndicator.Z != 0f && m_currentLadderStep == 0)
			{
				if (moveIndicator.Z < 0f)
				{
					movementDelta = base.WorldMatrix.Up * m_stepIncrement * m_stepsPerAnimation;
				}
				if (moveIndicator.Z > 0f)
				{
					movementDelta = base.WorldMatrix.Down * m_stepIncrement * m_stepsPerAnimation;
				}
				bool isHit;
				MyLadder myLadder = CheckTopLadder(position, ref movementDelta, out isHit);
				bool isHit2;
				MyLadder myLadder2 = CheckBottomLadder(position, ref movementDelta, out isHit2);
				bool flag = false;
				bool forceStartAnimation = false;
				MyCharacterMovementEnum currentMovementState = GetCurrentMovementState();
				if (moveIndicator.Z < 0f)
				{
					flag = (myLadder?.IsFunctional ?? false);
					if (flag && GetCurrentMovementState() == MyCharacterMovementEnum.LadderDown)
					{
						m_currentLadderStep = m_stepsPerAnimation - m_currentLadderStep;
						forceStartAnimation = (m_currentLadderStep > m_stepsPerAnimation / 2);
					}
					currentMovementState = MyCharacterMovementEnum.LadderUp;
				}
				if (moveIndicator.Z > 0f)
				{
					flag = (myLadder2?.IsFunctional ?? false);
					if (flag && GetCurrentMovementState() == MyCharacterMovementEnum.LadderUp)
					{
						m_currentLadderStep = m_stepsPerAnimation - m_currentLadderStep;
						forceStartAnimation = (m_currentLadderStep > m_stepsPerAnimation / 2);
					}
					currentMovementState = MyCharacterMovementEnum.LadderDown;
				}
				if (flag)
				{
					SetCurrentMovementState(currentMovementState);
					StartStep(forceStartAnimation);
				}
				else if (Physics.CharacterProxy != null)
				{
					if (moveIndicator.Z < 0f && !isHit)
					{
						if (GetCurrentMovementState() != MyCharacterMovementEnum.LadderOut)
						{
							m_currentLadderStep = 2 * m_stepsPerAnimation + 50;
							SetCurrentMovementState(MyCharacterMovementEnum.LadderOut);
							TriggerCharacterAnimationEvent("LadderOut", sync: false);
						}
						else
						{
							SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
						}
					}
					else if (moveIndicator.Z > 0f && !isHit2)
					{
						if (Sync.IsServer)
						{
							GetOffLadder();
						}
					}
					else
					{
						SetCurrentMovementState(MyCharacterMovementEnum.Ladder);
					}
				}
				else if (m_currentLadderStep == 0 && !isHit)
				{
					m_currentLadderStep = 2 * m_stepsPerAnimation + 50;
				}
			}
			return moveIndicator;
		}

		private void SetCharacterLadderConstraint(MatrixD characterWM)
		{
			characterWM.Translation = Physics.WorldToCluster(characterWM.Translation) + Vector3D.TransformNormal(Physics.Center, characterWM);
			Matrix matrix = Matrix.Invert(m_ladder.Parent.Physics.RigidBody.GetRigidBodyMatrix());
			Matrix matrix2 = Matrix.Invert(characterWM);
			Matrix matrix3 = Matrix.CreateWorld(characterWM.Translation);
			matrix = matrix3 * matrix;
			matrix2 = matrix3 * matrix2;
			m_constraintData.SetInBodySpaceInternal(ref matrix, ref matrix2);
		}

		private void MyLadder_IsWorkingChanged(MyCubeBlock obj)
		{
			if (Sync.IsServer && !obj.IsWorking)
			{
				GetOffLadder();
			}
		}

		public void ChangeLadder(MyLadder newLadder, bool resetPosition = false, MyObjectBuilder_Character.LadderInfo? newLadderInfo = null)
		{
			if (newLadder == m_ladder)
			{
				return;
			}
			MyLadder ladder = m_ladder;
			bool flag = true;
			if (ladder != null)
			{
				if (newLadder != null)
				{
					flag = (ladder.CubeGrid != newLadder.CubeGrid);
				}
				ladder.IsWorkingChanged -= MyLadder_IsWorkingChanged;
				ladder.CubeGridChanged -= Ladder_OnCubeGridChanged;
				ladder.OnClose -= m_ladder_OnClose;
			}
			if (ladder != null && newLadder != null)
			{
				m_baseMatrix = m_baseMatrix * ladder.PositionComp.WorldMatrix * newLadder.PositionComp.WorldMatrixNormalizedInv;
			}
			m_ladder = newLadder;
			if (newLadder != null)
			{
				newLadder.IsWorkingChanged += MyLadder_IsWorkingChanged;
				newLadder.CubeGridChanged += Ladder_OnCubeGridChanged;
				newLadder.OnClose += m_ladder_OnClose;
			}
			if (flag && Physics != null)
			{
				ReconnectConstraint(ladder?.CubeGrid, newLadder?.CubeGrid);
				if (newLadder != null)
				{
					PutCharacterOnLadder(newLadder, resetPosition, newLadderInfo);
				}
			}
		}

		private void m_ladder_OnClose(MyEntity obj)
		{
			if (obj == m_ladder)
			{
				GetOffLadder_Implementation();
			}
		}

		private void Ladder_OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			m_needReconnectLadder = true;
			m_oldLadderGrid = oldGrid;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void PutCharacterOnLadder(MyLadder ladder, bool resetPosition, MyObjectBuilder_Character.LadderInfo? newLadderInfo = null)
		{
			if (!newLadderInfo.HasValue)
			{
				Vector3 vector = (base.WorldMatrix * ladder.PositionComp.WorldMatrixInvScaled).Translation;
				MatrixD matrixD = MatrixD.Normalize(ladder.StartMatrix) * MatrixD.CreateRotationY(3.1415929794311523);
				float num = Vector3.Dot(base.WorldMatrix.Up, ladder.PositionComp.WorldMatrix.Up);
				float num2 = ladder.StartMatrix.Translation.Y;
				if (num < 0f)
				{
					matrixD *= MatrixD.CreateRotationZ(3.1415929794311523);
					num2 = 0f - num2;
				}
				matrixD.Translation = new Vector3D(ladder.StartMatrix.Translation.X, resetPosition ? num2 : vector.Y, ladder.StartMatrix.Translation.Z);
				float num3 = (float)matrixD.Translation.Y;
				float num4 = m_stepIncrement * (float)m_currentLadderStep;
				if (num < 0f)
				{
					num4 *= -1f;
				}
				float num5 = num2 + ((GetCurrentMovementState() == MyCharacterMovementEnum.LadderUp) ? (0f - num4) : num4);
				float num6 = (float)(int)((num3 - num5) / ladder.DistanceBetweenPoles) * ladder.DistanceBetweenPoles + num5;
				matrixD.Translation = new Vector3(matrixD.Translation.X, num6, matrixD.Translation.Z);
				if (num < 0f)
				{
					num6 *= -1f;
				}
				m_ladderIncrementToBase = Vector3.Zero;
				m_ladderIncrementToBase.Y = num6;
				MatrixD worldMatrix = matrixD * ladder.WorldMatrix;
				if (Physics.CharacterProxy != null)
				{
					Physics.CharacterProxy.ImmediateSetWorldTransform = true;
				}
				matrixD.Translation = new Vector3D(matrixD.Translation.X, 0.0, matrixD.Translation.Z);
				m_baseMatrix = matrixD;
				base.PositionComp.SetWorldMatrix(worldMatrix);
			}
			else
			{
				m_baseMatrix = Matrix.CreateWorld((Vector3D)newLadderInfo.Value.BaseMatrix.Position, newLadderInfo.Value.BaseMatrix.Forward, newLadderInfo.Value.BaseMatrix.Up);
				m_ladderIncrementToBase = newLadderInfo.Value.IncrementToBase;
			}
			if (Physics.CharacterProxy != null)
			{
				SetCharacterLadderConstraint(base.PositionComp.WorldMatrix);
				Physics.CharacterProxy.ImmediateSetWorldTransform = false;
			}
		}

		private void ReconnectConstraint(MyCubeGrid oldLadderGrid, MyCubeGrid newLadderGrid)
		{
			CloseLadderConstraint(oldLadderGrid);
			if (newLadderGrid != null)
			{
				AddLadderConstraint(newLadderGrid);
			}
		}

		private bool ShouldCollideWith(MyLadder ladder)
		{
			return false;
		}

		private void CalculateHandIK(int startBoneIndex, int endBoneIndex, ref MatrixD targetTransform)
		{
			MyCharacterBone finalBone = base.AnimationController.CharacterBones[endBoneIndex];
			_ = base.AnimationController.CharacterBones[startBoneIndex];
			List<MyCharacterBone> list = new List<MyCharacterBone>();
			for (int i = startBoneIndex; i <= endBoneIndex; i++)
			{
				list.Add(base.AnimationController.CharacterBones[i]);
			}
			MatrixD worldMatrixNormalizedInv = base.PositionComp.WorldMatrixNormalizedInv;
			Matrix finalTransform = targetTransform * worldMatrixNormalizedInv;
			Vector3 desiredEnd = finalTransform.Translation;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
			{
				MyRenderProxy.DebugDrawText3D(targetTransform.Translation, "Hand target transform", Color.Purple, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(targetTransform.Translation, 0.03f, Color.Purple, 1f, depthRead: false);
				MyRenderProxy.DebugDrawAxis(targetTransform, 0.03f, depthRead: false);
			}
			_ = (Vector3)targetTransform.Translation;
			MyInverseKinematics.SolveCCDIk(ref desiredEnd, list, 0.0005f, 5, 0.5f, ref finalTransform, finalBone);
		}

		private void CalculateHandIK(int upperarmIndex, int forearmIndex, int palmIndex, ref MatrixD targetTransform)
		{
			MyCharacterBone[] characterBones = base.AnimationController.CharacterBones;
			MatrixD worldMatrixNormalizedInv = base.PositionComp.WorldMatrixNormalizedInv;
			Matrix finalTransform = targetTransform * worldMatrixNormalizedInv;
			_ = finalTransform.Translation;
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_IK_IKSOLVERS)
			{
				MyRenderProxy.DebugDrawText3D(targetTransform.Translation, "Hand target transform", Color.Purple, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(targetTransform.Translation, 0.03f, Color.Purple, 1f, depthRead: false);
				MyRenderProxy.DebugDrawAxis(targetTransform, 0.03f, depthRead: false);
			}
			if (characterBones.IsValidIndex(upperarmIndex) && characterBones.IsValidIndex(forearmIndex) && characterBones.IsValidIndex(palmIndex))
			{
				MatrixD worldMatrix = base.PositionComp.WorldMatrix;
				MyInverseKinematics.SolveTwoJointsIkCCD(characterBones, upperarmIndex, forearmIndex, palmIndex, ref finalTransform, ref worldMatrix, characterBones[palmIndex]);
			}
		}

		private void InitAnimations()
		{
			m_animationSpeedFilterCursor = 0;
			for (int i = 0; i < m_animationSpeedFilter.Length; i++)
			{
				m_animationSpeedFilter[i] = Vector3.Zero;
			}
			foreach (KeyValuePair<string, string[]> boneSet in m_characterDefinition.BoneSets)
			{
				AddAnimationPlayer(boneSet.Key, boneSet.Value);
			}
			SetBoneLODs(m_characterDefinition.BoneLODs);
			base.AnimationController.FindBone(m_characterDefinition.HeadBone, out m_headBoneIndex);
			base.AnimationController.FindBone(m_characterDefinition.Camera3rdBone, out m_camera3rdBoneIndex);
			if (m_camera3rdBoneIndex == -1)
			{
				m_camera3rdBoneIndex = m_headBoneIndex;
			}
			base.AnimationController.FindBone(m_characterDefinition.LeftHandIKStartBone, out m_leftHandIKStartBone);
			base.AnimationController.FindBone(m_characterDefinition.LeftHandIKEndBone, out m_leftHandIKEndBone);
			base.AnimationController.FindBone(m_characterDefinition.RightHandIKStartBone, out m_rightHandIKStartBone);
			base.AnimationController.FindBone(m_characterDefinition.RightHandIKEndBone, out m_rightHandIKEndBone);
			base.AnimationController.FindBone(m_characterDefinition.LeftUpperarmBone, out m_leftUpperarmBone);
			base.AnimationController.FindBone(m_characterDefinition.LeftForearmBone, out m_leftForearmBone);
			base.AnimationController.FindBone(m_characterDefinition.RightUpperarmBone, out m_rightUpperarmBone);
			base.AnimationController.FindBone(m_characterDefinition.RightForearmBone, out m_rightForearmBone);
			base.AnimationController.FindBone(m_characterDefinition.WeaponBone, out m_weaponBone);
			base.AnimationController.FindBone(m_characterDefinition.LeftHandItemBone, out m_leftHandItemBone);
			base.AnimationController.FindBone(m_characterDefinition.RighHandItemBone, out m_rightHandItemBone);
			base.AnimationController.FindBone(m_characterDefinition.SpineBone, out m_spineBone);
			UpdateAnimation(0f);
		}

		protected override void CalculateTransforms(float distance)
		{
			bool flag = IsInFirstPersonView && MySession.Static.CameraController == this;
			bool flag2 = flag || ForceFirstPersonCamera;
			base.CalculateTransforms(distance);
			Vector3 value = Vector3.Zero;
			if (m_headBoneIndex >= 0 && base.AnimationController.CharacterBones != null && flag && MySession.Static.CameraController == this && !IsBot && !IsOnLadder)
			{
				value = base.AnimationController.CharacterBones[m_headBoneIndex].AbsoluteTransform.Translation;
				value.Y = 0f;
				MyCharacterBone.TranslateAllBones(base.AnimationController.CharacterBones, -value);
			}
			if (IsOnLadder)
			{
				m_wasOnLadder = 100;
			}
			else if (m_wasOnLadder > 0)
			{
				m_wasOnLadder--;
			}
			if (Entity.InScene && m_wasOnLadder == 0)
			{
				base.AnimationController.UpdateInverseKinematics();
			}
			if (m_leftHandItem != null)
			{
				UpdateLeftHandItemPosition();
			}
			if (m_currentWeapon != null && WeaponPosition != null && m_handItemDefinition != null)
			{
				WeaponPosition.Update();
				if ((flag2 ? m_handItemDefinition.SimulateLeftHandFps : m_handItemDefinition.SimulateLeftHand) && m_leftHandIKStartBone != -1 && m_leftHandIKEndBone != -1)
				{
					MatrixD targetTransform = (MatrixD)m_handItemDefinition.LeftHand * ((MyEntity)m_currentWeapon).WorldMatrix;
					CalculateHandIK(m_leftHandIKStartBone, m_leftForearmBone, m_leftHandIKEndBone, ref targetTransform);
				}
				bool flag3 = flag2 ? m_handItemDefinition.SimulateRightHandFps : m_handItemDefinition.SimulateRightHand;
				if (m_rightHandIKStartBone != -1 && m_rightHandIKEndBone != -1 && !IsSitting)
				{
					if (flag3)
					{
						MatrixD targetTransform2 = (MatrixD)m_handItemDefinition.RightHand * ((MyEntity)m_currentWeapon).WorldMatrix;
						CalculateHandIK(m_rightHandIKStartBone, m_rightForearmBone, m_rightHandIKEndBone, ref targetTransform2);
					}
					else if (m_handItemDefinition.SimulateRightHand && !m_handItemDefinition.SimulateRightHandFps && flag2)
					{
						Matrix absoluteMatrix = base.AnimationController.CharacterBones[SpineBoneIndex].GetAbsoluteRigTransform();
						absoluteMatrix.Translation -= 2f * value;
						base.AnimationController.CharacterBones[m_rightHandIKEndBone].SetCompleteBindTransform();
						base.AnimationController.CharacterBones[m_rightForearmBone].SetCompleteBindTransform();
						base.AnimationController.CharacterBones[m_rightHandIKStartBone].SetCompleteTransformFromAbsoluteMatrix(ref absoluteMatrix, onlyRotation: false);
					}
				}
			}
			base.AnimationController.UpdateTransformations();
		}

		public override void UpdateControl(float distance)
		{
			base.UpdateControl(distance);
			if (distance < MyFakes.ANIMATION_UPDATE_DISTANCE && UseNewAnimationSystem)
			{
				UpdateAnimationNewSystem();
			}
		}

		public override void UpdateAnimation(float distance)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated && MyPerGameSettings.DisableAnimationsOnDS)
			{
				return;
			}
			if (distance < MyFakes.ANIMATION_UPDATE_DISTANCE)
			{
				base.UpdateAnimation(distance);
				if (TryGetAnimationPlayer("LeftHand", out MyAnimationPlayerBlendPair player) && player.GetState() == MyAnimationPlayerBlendPair.AnimationBlendState.Stopped && m_leftHandItem != null && !UseNewAnimationSystem)
				{
					m_leftHandItem.Close();
					m_leftHandItem = null;
				}
				Render.UpdateThrustMatrices(base.BoneAbsoluteTransforms);
				if (m_resetWeaponAnimationState)
				{
					m_resetWeaponAnimationState = false;
				}
			}
			else
			{
				WeaponPosition.Update();
			}
		}

		private void UpdateAnimationNewSystem()
		{
			IMyVariableStorage<float> variables = base.AnimationController.Variables;
			if (Physics != null)
			{
				Vector3 vector = Physics.LinearVelocity * Vector3.TransformNormal(m_currentMovementDirection, base.WorldMatrix);
				Vector3 vector2 = vector;
				Vector3 vector3 = vector2;
				MyCharacterProxy characterProxy = Physics.CharacterProxy;
				if (Sync.IsServer || MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER)
				{
					if (characterProxy != null)
					{
						vector = Physics.LinearVelocityLocal;
						Vector3 groundVelocity = characterProxy.GroundVelocity;
						Vector3 interpolatedVelocity = characterProxy.CharacterRigidBody.InterpolatedVelocity;
						vector2 = ((!((interpolatedVelocity - groundVelocity).LengthSquared() < (vector - groundVelocity).LengthSquared())) ? vector : interpolatedVelocity);
						vector3 = vector2 - groundVelocity;
						if (GetCurrentMovementState() == MyCharacterMovementEnum.Standing)
						{
							float num = characterProxy.Up.Dot(vector3);
							if (num < 0f)
							{
								vector3 -= characterProxy.Up * num;
							}
						}
					}
					else
					{
						vector3 = Physics.LinearVelocityLocal;
					}
				}
				Vector3 vector4 = FilterLocalSpeed(vector3);
				MatrixD worldMatrix = base.PositionComp.WorldMatrix;
				float num2 = vector4.Dot(worldMatrix.Right);
				float newValue = vector4.Dot(worldMatrix.Up);
				float num3 = vector4.Dot(worldMatrix.Forward);
				float newValue2 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeed, newValue2);
				variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedX, num2);
				variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedY, newValue);
				variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedZ, num3);
				float num4;
				for (num4 = ((vector4.LengthSquared() > 0.00250000018f) ? ((float)((0.0 - Math.Atan2(num3, num2)) * 180.0 / Math.PI) + 90f) : 0f); num4 < 0f; num4 += 360f)
				{
				}
				variables.SetValue(MyAnimationVariableStorageHints.StrIdSpeedAngle, num4);
				if (characterProxy != null)
				{
					Vector3 axis = Vector3.Zero;
					if (Sync.IsServer)
					{
						axis = characterProxy.GroundAngularVelocity;
					}
					else
					{
						using (MyUtils.ReuseCollection(ref m_supportingEntities))
						{
							int num5 = 0;
							characterProxy.GetSupportingEntities(m_supportingEntities);
							foreach (MyEntity supportingEntity in m_supportingEntities)
							{
								MyPhysicsComponentBase physics = supportingEntity.Physics;
								if (physics != null)
								{
									num5++;
									axis += physics.AngularVelocityLocal;
								}
							}
							if (num5 != 0)
							{
								axis /= (float)num5;
							}
						}
					}
					m_lastRotation *= Quaternion.CreateFromAxisAngle(axis, 0.0166666675f);
				}
				Quaternion rotation = GetRotation();
				float newValue3 = (Quaternion.Inverse(rotation) * m_lastRotation).Y / 0.00129999989f * 180f / MathF.PI;
				variables.SetValue(MyAnimationVariableStorageHints.StrIdTurningSpeed, newValue3);
				m_lastRotation = rotation;
				if (OxygenComponent != null)
				{
					variables.SetValue(MyAnimationVariableStorageHints.StrIdHelmetOpen, OxygenComponent.HelmetEnabled ? 0f : 1f);
				}
				if (base.Parent is MyCockpit || IsOnLadder)
				{
					variables.SetValue(MyAnimationVariableStorageHints.StrIdLean, 0f);
				}
				else
				{
					variables.SetValue(MyAnimationVariableStorageHints.StrIdLean, m_animLeaning);
				}
			}
			bool flag = MySession.Static.CameraController == this;
			bool flag2 = (m_isInFirstPerson || ForceFirstPersonCamera) && flag;
			if (JetpackComp != null)
			{
				variables.SetValue(MyAnimationVariableStorageHints.StrIdFlying, JetpackComp.Running ? 1f : 0f);
			}
			MyCharacterMovementEnum currentMovementState = GetCurrentMovementState();
			variables.SetValue(MyAnimationVariableStorageHints.StrIdFlying, (currentMovementState == MyCharacterMovementEnum.Flying) ? 1f : 0f);
			variables.SetValue(MyAnimationVariableStorageHints.StrIdFalling, (IsFalling || currentMovementState == MyCharacterMovementEnum.Falling) ? 1f : 0f);
			variables.SetValue(MyAnimationVariableStorageHints.StrIdCrouch, (WantsCrouch && !WantsSprint) ? 1f : 0f);
			variables.SetValue(MyAnimationVariableStorageHints.StrIdSitting, (currentMovementState == MyCharacterMovementEnum.Sitting) ? 1f : 0f);
			variables.SetValue(MyAnimationVariableStorageHints.StrIdJumping, (currentMovementState == MyCharacterMovementEnum.Jump) ? 1f : 0f);
			variables.SetValue(MyAnimationVariableStorageHints.StrIdFirstPerson, flag2 ? 1f : 0f);
			variables.SetValue(MyAnimationVariableStorageHints.StrIdForcedFirstPerson, ForceFirstPersonCamera ? 1f : 0f);
			variables.SetValue(MyAnimationVariableStorageHints.StrIdHoldingTool, (m_currentWeapon != null) ? 1f : 0f);
			if (WeaponPosition != null)
			{
				variables.SetValue(MyAnimationVariableStorageHints.StrIdShooting, (m_currentWeapon != null && WeaponPosition.IsShooting && !WeaponPosition.ShouldSupressShootAnimation) ? 1f : 0f);
				variables.SetValue(MyAnimationVariableStorageHints.StrIdIronsight, WeaponPosition.IsInIronSight ? 1f : 0f);
			}
			else
			{
				variables.SetValue(MyAnimationVariableStorageHints.StrIdShooting, 0f);
				variables.SetValue(MyAnimationVariableStorageHints.StrIdIronsight, 0f);
			}
			variables.SetValue(MyAnimationVariableStorageHints.StrIdLadder, IsOnLadder ? 1f : 0f);
		}

		private Vector3 FilterLocalSpeed(Vector3 localSpeedWorldRotUnfiltered)
		{
			return localSpeedWorldRotUnfiltered;
		}

		protected override void OnAnimationPlay(MyAnimationDefinition animDefinition, MyAnimationCommand command, ref string bonesArea, ref MyFrameOption frameOption, ref bool useFirstPersonVersion)
		{
			MyCharacterMovementEnum currentMovementState = GetCurrentMovementState();
			if (currentMovementState != 0 && currentMovementState != MyCharacterMovementEnum.RotatingLeft && currentMovementState != MyCharacterMovementEnum.RotatingRight && command.ExcludeLegsWhenMoving)
			{
				bonesArea = TopBody;
				frameOption = ((frameOption != MyFrameOption.JustFirstFrame) ? MyFrameOption.PlayOnce : frameOption);
			}
			useFirstPersonVersion = IsInFirstPersonView;
			if (animDefinition.AllowWithWeapon)
			{
				m_resetWeaponAnimationState = true;
			}
		}

		private void StopUpperAnimation(float blendTime)
		{
			PlayerStop("Head", blendTime);
			PlayerStop("Spine", blendTime);
			PlayerStop("LeftHand", blendTime);
			PlayerStop("RightHand", blendTime);
		}

		private void StopFingersAnimation(float blendTime)
		{
			PlayerStop("LeftFingers", blendTime);
			PlayerStop("RightFingers", blendTime);
		}

		public override void AddCommand(MyAnimationCommand command, bool sync = false)
		{
			if (!UseNewAnimationSystem)
			{
				base.AddCommand(command, sync);
				if (sync)
				{
					SendAnimationCommand(ref command);
				}
			}
		}

		public void SetSpineAdditionalRotation(Quaternion rotation, Quaternion rotationForClients, bool updateSync = true)
		{
			if (!string.IsNullOrEmpty(Definition.SpineBone) && GetAdditionalRotation(Definition.SpineBone) != rotation)
			{
				m_additionalRotations[Definition.SpineBone] = rotation;
			}
		}

		public void SetHeadAdditionalRotation(Quaternion rotation, bool updateSync = true)
		{
			if (!string.IsNullOrEmpty(Definition.HeadBone) && GetAdditionalRotation(Definition.HeadBone) != rotation)
			{
				m_additionalRotations[Definition.HeadBone] = rotation;
			}
		}

		public void SetHandAdditionalRotation(Quaternion rotation, bool updateSync = true)
		{
			if (!string.IsNullOrEmpty(Definition.LeftForearmBone) && GetAdditionalRotation(Definition.LeftForearmBone) != rotation)
			{
				m_additionalRotations[Definition.LeftForearmBone] = rotation;
				m_additionalRotations[Definition.RightForearmBone] = Quaternion.Inverse(rotation);
			}
		}

		public void SetUpperHandAdditionalRotation(Quaternion rotation, bool updateSync = true)
		{
			if (!string.IsNullOrEmpty(Definition.LeftUpperarmBone) && GetAdditionalRotation(Definition.LeftUpperarmBone) != rotation)
			{
				m_additionalRotations[Definition.LeftUpperarmBone] = rotation;
				m_additionalRotations[Definition.RightUpperarmBone] = Quaternion.Inverse(rotation);
			}
		}

		public bool HasAnimation(string animationName)
		{
			return Definition.AnimationNameToSubtypeName.ContainsKey(animationName);
		}

		public void DisableAnimationCommands()
		{
			m_animationCommandsEnabled = false;
		}

		public void EnableAnimationCommands()
		{
			m_animationCommandsEnabled = true;
		}

		public void TriggerCharacterAnimationEvent(string eventName, bool sync)
		{
			if (UseNewAnimationSystem && !string.IsNullOrEmpty(eventName))
			{
				if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.EntityId))
				{
					PerFrameData perFrameData = default(PerFrameData);
					perFrameData.AnimationData = new AnimationData
					{
						Animation = eventName
					};
					PerFrameData data = perFrameData;
					MySessionComponentReplay.Static.ProvideEntityRecordData(base.EntityId, data);
				}
				if (sync)
				{
					SendAnimationEvent(eventName);
				}
				else
				{
					base.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName));
				}
			}
		}

		public void TriggerCharacterAnimationEvent(string eventName, bool sync, string[] layers)
		{
			if (UseNewAnimationSystem && !string.IsNullOrEmpty(eventName) && !IsDead)
			{
				if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.EntityId))
				{
					PerFrameData perFrameData = default(PerFrameData);
					perFrameData.AnimationData = new AnimationData
					{
						Animation = eventName
					};
					PerFrameData data = perFrameData;
					MySessionComponentReplay.Static.ProvideEntityRecordData(base.EntityId, data);
				}
				if (sync)
				{
					SendAnimationEvent(eventName, layers);
				}
				else
				{
					base.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName), layers);
				}
			}
		}

		public void PlayCharacterAnimation(string animationName, MyBlendOption blendOption, MyFrameOption frameOption, float blendTime, float timeScale = 1f, bool sync = false, string influenceArea = null, bool excludeLegsWhenMoving = false)
		{
			if (UseNewAnimationSystem)
			{
				return;
			}
			bool flag = Sandbox.Engine.Platform.Game.IsDedicated && MyPerGameSettings.DisableAnimationsOnDS;
			if ((!flag || sync) && m_animationCommandsEnabled && animationName != null)
			{
				string value = null;
				if (!m_characterDefinition.AnimationNameToSubtypeName.TryGetValue(animationName, out value))
				{
					value = animationName;
				}
				MyAnimationCommand myAnimationCommand = default(MyAnimationCommand);
				myAnimationCommand.AnimationSubtypeName = value;
				myAnimationCommand.PlaybackCommand = MyPlaybackCommand.Play;
				myAnimationCommand.BlendOption = blendOption;
				myAnimationCommand.FrameOption = frameOption;
				myAnimationCommand.BlendTime = blendTime;
				myAnimationCommand.TimeScale = timeScale;
				myAnimationCommand.Area = influenceArea;
				myAnimationCommand.ExcludeLegsWhenMoving = excludeLegsWhenMoving;
				MyAnimationCommand command = myAnimationCommand;
				if (sync)
				{
					SendAnimationCommand(ref command);
				}
				else if (!flag)
				{
					AddCommand(command, sync);
				}
			}
		}

		public void StopUpperCharacterAnimation(float blendTime)
		{
			if (!UseNewAnimationSystem)
			{
				AddCommand(new MyAnimationCommand
				{
					AnimationSubtypeName = null,
					PlaybackCommand = MyPlaybackCommand.Stop,
					Area = TopBody,
					BlendTime = blendTime,
					TimeScale = 1f
				});
			}
		}

		public void StopLowerCharacterAnimation(float blendTime)
		{
			if (!UseNewAnimationSystem)
			{
				AddCommand(new MyAnimationCommand
				{
					AnimationSubtypeName = null,
					PlaybackCommand = MyPlaybackCommand.Stop,
					Area = "LowerBody",
					BlendTime = blendTime,
					TimeScale = 1f
				});
			}
		}

		[Event(null, 680)]
		[Reliable]
		[Broadcast]
		public void CreateBurrowingParticleFX_Client(Vector3D position)
		{
			if (MyParticlesManager.TryCreateParticleEffect("Burrowing", MatrixD.CreateTranslation(position), out MyParticleEffect effect))
			{
				effect.WorldMatrix = MatrixD.CreateTranslation(position);
				effect.UserAxisScale = new Vector3D(2.0, 2.0, 2.0);
				m_burrowEffectTable[position] = effect;
			}
		}

		[Event(null, 693)]
		[Reliable]
		[Broadcast]
		public void DeleteBurrowingParticleFX_Client(Vector3D position)
		{
			if (m_burrowEffectTable.TryGetValue(position, out MyParticleEffect value))
			{
				value.StopEmitting();
				m_burrowEffectTable.Remove(position);
			}
		}

		[Event(null, 704)]
		[Broadcast]
		[Reliable]
		public void TriggerAnimationEvent(string eventName)
		{
			base.AnimationController.TriggerAction(MyStringId.GetOrCompute(eventName));
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(IMyCameraController camera, long playerId)
		{
			if (camera != null)
			{
				DrawHud(camera, playerId);
			}
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}
	}
}
