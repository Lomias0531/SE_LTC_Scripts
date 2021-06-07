using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ContractBlock))]
	public class MyContractBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, IMyTextSurfaceProvider
	{
		[Serializable]
		public struct MyTargetEntityInfoWrapper
		{
			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyTargetEntityInfoWrapper_003C_003EId_003C_003EAccessor : IMemberAccessor<MyTargetEntityInfoWrapper, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyTargetEntityInfoWrapper owner, in long value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyTargetEntityInfoWrapper owner, out long value)
				{
					value = owner.Id;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyTargetEntityInfoWrapper_003C_003EName_003C_003EAccessor : IMemberAccessor<MyTargetEntityInfoWrapper, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyTargetEntityInfoWrapper owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyTargetEntityInfoWrapper owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyTargetEntityInfoWrapper_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<MyTargetEntityInfoWrapper, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyTargetEntityInfoWrapper owner, in string value)
				{
					owner.DisplayName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyTargetEntityInfoWrapper owner, out string value)
				{
					value = owner.DisplayName;
				}
			}

			public long Id;

			public string Name;

			public string DisplayName;
		}

		[Serializable]
		public struct MyEntityInfoWrapper
		{
			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyEntityInfoWrapper_003C_003ENamePrefix_003C_003EAccessor : IMemberAccessor<MyEntityInfoWrapper, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityInfoWrapper owner, in string value)
				{
					owner.NamePrefix = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityInfoWrapper owner, out string value)
				{
					value = owner.NamePrefix;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyEntityInfoWrapper_003C_003ENameSuffix_003C_003EAccessor : IMemberAccessor<MyEntityInfoWrapper, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityInfoWrapper owner, in string value)
				{
					owner.NameSuffix = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityInfoWrapper owner, out string value)
				{
					value = owner.NameSuffix;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyEntityInfoWrapper_003C_003EId_003C_003EAccessor : IMemberAccessor<MyEntityInfoWrapper, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyEntityInfoWrapper owner, in long value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyEntityInfoWrapper owner, out long value)
				{
					value = owner.Id;
				}
			}

			public string NamePrefix;

			public string NameSuffix;

			public long Id;
		}

		[Serializable]
		private struct MyContractCreationDataWrapper_Deliver
		{
			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Deliver_003C_003ERewardMoney_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Deliver, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Deliver owner, in int value)
				{
					owner.RewardMoney = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Deliver owner, out int value)
				{
					value = owner.RewardMoney;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Deliver_003C_003EStartingDeposit_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Deliver, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Deliver owner, in int value)
				{
					owner.StartingDeposit = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Deliver owner, out int value)
				{
					value = owner.StartingDeposit;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Deliver_003C_003EDurationInMin_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Deliver, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Deliver owner, in int value)
				{
					owner.DurationInMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Deliver owner, out int value)
				{
					value = owner.DurationInMin;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Deliver_003C_003ETargetBlockId_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Deliver, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Deliver owner, in long value)
				{
					owner.TargetBlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Deliver owner, out long value)
				{
					value = owner.TargetBlockId;
				}
			}

			public int RewardMoney;

			public int StartingDeposit;

			public int DurationInMin;

			public long TargetBlockId;
		}

		[Serializable]
		private struct MyContractCreationDataWrapper_ObtainAndDeliver
		{
			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_ObtainAndDeliver_003C_003ERewardMoney_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_ObtainAndDeliver, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, in int value)
				{
					owner.RewardMoney = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, out int value)
				{
					value = owner.RewardMoney;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_ObtainAndDeliver_003C_003EStartingDeposit_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_ObtainAndDeliver, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, in int value)
				{
					owner.StartingDeposit = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, out int value)
				{
					value = owner.StartingDeposit;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_ObtainAndDeliver_003C_003EDurationInMin_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_ObtainAndDeliver, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, in int value)
				{
					owner.DurationInMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, out int value)
				{
					value = owner.DurationInMin;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_ObtainAndDeliver_003C_003ETargetBlockId_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_ObtainAndDeliver, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, in long value)
				{
					owner.TargetBlockId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, out long value)
				{
					value = owner.TargetBlockId;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_ObtainAndDeliver_003C_003EItemTypeId_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_ObtainAndDeliver, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, in SerializableDefinitionId value)
				{
					owner.ItemTypeId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, out SerializableDefinitionId value)
				{
					value = owner.ItemTypeId;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_ObtainAndDeliver_003C_003EItemAmount_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_ObtainAndDeliver, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, in int value)
				{
					owner.ItemAmount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_ObtainAndDeliver owner, out int value)
				{
					value = owner.ItemAmount;
				}
			}

			public int RewardMoney;

			public int StartingDeposit;

			public int DurationInMin;

			public long TargetBlockId;

			public SerializableDefinitionId ItemTypeId;

			public int ItemAmount;
		}

		[Serializable]
		private struct MyContractCreationDataWrapper_Find
		{
			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Find_003C_003ERewardMoney_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Find, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Find owner, in int value)
				{
					owner.RewardMoney = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Find owner, out int value)
				{
					value = owner.RewardMoney;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Find_003C_003EStartingDeposit_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Find, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Find owner, in int value)
				{
					owner.StartingDeposit = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Find owner, out int value)
				{
					value = owner.StartingDeposit;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Find_003C_003EDurationInMin_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Find, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Find owner, in int value)
				{
					owner.DurationInMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Find owner, out int value)
				{
					value = owner.DurationInMin;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Find_003C_003ETargetGridId_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Find, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Find owner, in long value)
				{
					owner.TargetGridId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Find owner, out long value)
				{
					value = owner.TargetGridId;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Find_003C_003ESearchRadius_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Find, double>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Find owner, in double value)
				{
					owner.SearchRadius = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Find owner, out double value)
				{
					value = owner.SearchRadius;
				}
			}

			public int RewardMoney;

			public int StartingDeposit;

			public int DurationInMin;

			public long TargetGridId;

			public double SearchRadius;
		}

		[Serializable]
		private struct MyContractCreationDataWrapper_Repair
		{
			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Repair_003C_003ERewardMoney_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Repair, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Repair owner, in int value)
				{
					owner.RewardMoney = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Repair owner, out int value)
				{
					value = owner.RewardMoney;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Repair_003C_003EStartingDeposit_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Repair, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Repair owner, in int value)
				{
					owner.StartingDeposit = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Repair owner, out int value)
				{
					value = owner.StartingDeposit;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Repair_003C_003EDurationInMin_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Repair, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Repair owner, in int value)
				{
					owner.DurationInMin = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Repair owner, out int value)
				{
					value = owner.DurationInMin;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Repair_003C_003ETargetGridId_003C_003EAccessor : IMemberAccessor<MyContractCreationDataWrapper_Repair, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyContractCreationDataWrapper_Repair owner, in long value)
				{
					owner.TargetGridId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyContractCreationDataWrapper_Repair owner, out long value)
				{
					value = owner.TargetGridId;
				}
			}

			public int RewardMoney;

			public int StartingDeposit;

			public int DurationInMin;

			public long TargetGridId;
		}

		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyContractBlock, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyContractBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyContractBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyContractBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyContractBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyContractBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyContractBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected sealed class GetContractBlockStatus_003C_003E : ICallSite<MyContractBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetContractBlockStatus();
			}
		}

		protected sealed class GetAvailibleContracts_003C_003E : ICallSite<MyContractBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetAvailibleContracts();
			}
		}

		protected sealed class GetAdministrableContracts_003C_003E : ICallSite<MyContractBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetAdministrableContracts();
			}
		}

		protected sealed class GetActiveContracts_003C_003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long identityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetActiveContracts(identityId);
			}
		}

		protected sealed class GetActiveContractsStatic_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				GetActiveContractsStatic();
			}
		}

		protected sealed class GetAllOwnedContractBlocks_003C_003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long identityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetAllOwnedContractBlocks(identityId);
			}
		}

		protected sealed class GetAllOwnedGrids_003C_003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long identityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetAllOwnedGrids(identityId);
			}
		}

		protected sealed class AcceptContract_003C_003ESystem_Int64_0023System_Int64 : ICallSite<MyContractBlock, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long identityId, in long contractId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AcceptContract(identityId, contractId);
			}
		}

		protected sealed class FinishContract_003C_003ESystem_Int64_0023System_Int64_0023System_Int64 : ICallSite<MyContractBlock, long, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long identityId, in long contractId, in long targetEntityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.FinishContract(identityId, contractId, targetEntityId);
			}
		}

		protected sealed class AbandonContract_003C_003ESystem_Int64_0023System_Int64 : ICallSite<MyContractBlock, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long identityId, in long contractId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AbandonContract(identityId, contractId);
			}
		}

		protected sealed class AbandonContractStatic_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long contractId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AbandonContractStatic(contractId);
			}
		}

		protected sealed class GetConnectedEntities_003C_003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long identityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.GetConnectedEntities(identityId);
			}
		}

		protected sealed class CreateCustomContractDeliver_003C_003ESandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Deliver : ICallSite<MyContractBlock, MyContractCreationDataWrapper_Deliver, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractCreationDataWrapper_Deliver data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateCustomContractDeliver(data);
			}
		}

		protected sealed class CreateCustomContractObtainAndDeliver_003C_003ESandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_ObtainAndDeliver : ICallSite<MyContractBlock, MyContractCreationDataWrapper_ObtainAndDeliver, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractCreationDataWrapper_ObtainAndDeliver data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateCustomContractObtainAndDeliver(data);
			}
		}

		protected sealed class CreateCustomContractRepair_003C_003ESandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Repair : ICallSite<MyContractBlock, MyContractCreationDataWrapper_Repair, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractCreationDataWrapper_Repair data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateCustomContractRepair(data);
			}
		}

		protected sealed class CreateCustomContractFind_003C_003ESandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyContractCreationDataWrapper_Find : ICallSite<MyContractBlock, MyContractCreationDataWrapper_Find, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractCreationDataWrapper_Find data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CreateCustomContractFind(data);
			}
		}

		protected sealed class DeleteCustomContract_003C_003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in long contractId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DeleteCustomContract(contractId);
			}
		}

		protected sealed class ReceiveContractBlockStatus_003C_003ESystem_Boolean : ICallSite<MyContractBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in bool isNpc, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveContractBlockStatus(isNpc);
			}
		}

		protected sealed class ReceiveAvailableContracts_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003E : ICallSite<MyContractBlock, List<MyObjectBuilder_Contract>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in List<MyObjectBuilder_Contract> availableContracts, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveAvailableContracts(availableContracts);
			}
		}

		protected sealed class ReceiveAdministrableContracts_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003E : ICallSite<MyContractBlock, List<MyObjectBuilder_Contract>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in List<MyObjectBuilder_Contract> administrableContracts, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveAdministrableContracts(administrableContracts);
			}
		}

		protected sealed class ReceiveActiveContracts_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003E_0023System_Int64_0023System_Int64 : ICallSite<MyContractBlock, List<MyObjectBuilder_Contract>, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in List<MyObjectBuilder_Contract> activeContracts, in long stationId, in long blockId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveActiveContracts(activeContracts, stationId, blockId);
			}
		}

		protected sealed class ReceiveActiveContractsStatic_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract_003E : ICallSite<IMyEventOwner, List<MyObjectBuilder_Contract>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MyObjectBuilder_Contract> activeContracts, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReceiveActiveContractsStatic(activeContracts);
			}
		}

		protected sealed class ReceiveAllOwnedContractBlocks_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyEntityInfoWrapper_003E : ICallSite<MyContractBlock, List<MyEntityInfoWrapper>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in List<MyEntityInfoWrapper> data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveAllOwnedContractBlocks(data);
			}
		}

		protected sealed class ReceiveAllOwnedGrids_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyEntityInfoWrapper_003E : ICallSite<MyContractBlock, List<MyEntityInfoWrapper>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in List<MyEntityInfoWrapper> data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveAllOwnedGrids(data);
			}
		}

		protected sealed class ReceiveActiveConditions_003C_003ESystem_Collections_Generic_List_00601_003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition_003E : ICallSite<MyContractBlock, List<MyObjectBuilder_ContractCondition>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in List<MyObjectBuilder_ContractCondition> activeConditions, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveActiveConditions(activeConditions);
			}
		}

		protected sealed class ReceiveAcceptContract_003C_003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<MyContractBlock, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractResults result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveAcceptContract(result);
			}
		}

		protected sealed class ReceiveFinishContract_003C_003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<MyContractBlock, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractResults result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveFinishContract(result);
			}
		}

		protected sealed class ReceiveAbandonContract_003C_003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<MyContractBlock, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractResults result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveAbandonContract(result);
			}
		}

		protected sealed class ReceiveAbandonContractStatic_003C_003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<IMyEventOwner, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyContractResults result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReceiveAbandonContractStatic(result);
			}
		}

		protected sealed class ReceiveGetConnectedGrids_003C_003ESystem_Boolean_0023System_Collections_Generic_List_00601_003CSandbox_Game_Entities_Blocks_MyContractBlock_003C_003EMyTargetEntityInfoWrapper_003E : ICallSite<MyContractBlock, bool, List<MyTargetEntityInfoWrapper>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in bool isSuccessful, in List<MyTargetEntityInfoWrapper> connectedGrids, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveGetConnectedGrids(isSuccessful, connectedGrids);
			}
		}

		protected sealed class ReceiveCreateContractResult_003C_003ESandbox_Game_World_Generator_MyContractCreationResults : ICallSite<MyContractBlock, MyContractCreationResults, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in MyContractCreationResults result, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveCreateContractResult(result);
			}
		}

		protected sealed class ReceiveDeleteCustomContractResult_003C_003ESystem_Boolean : ICallSite<MyContractBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyContractBlock @this, in bool success, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ReceiveDeleteCustomContractResult(success);
			}
		}

		protected class m_anyoneCanUse_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType anyoneCanUse;
				ISyncType result = anyoneCanUse = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyContractBlock)P_0).m_anyoneCanUse = (Sync<bool, SyncDirection.BothWays>)anyoneCanUse;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyContractBlock_003C_003EActor : IActivator, IActivator<MyContractBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractBlock CreateInstance()
			{
				return new MyContractBlock();
			}

			MyContractBlock IActivator<MyContractBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyMultiTextPanelComponent m_multiPanel;

		private MyGuiScreenTextPanel m_textBoxMultiPanel;

		private static Action<List<MyObjectBuilder_Contract>> m_localStaticRequestActiveContractsCallback;

		private static Action<MyContractResults> m_localStaticRequestAbandonCallback;

		private readonly Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private Action<bool> m_localRequestContractBlockStatucCallback;

		private Action<List<MyObjectBuilder_Contract>> m_localRequestAvailableContractsCallback;

		private Action<List<MyObjectBuilder_Contract>> m_localRequestAdministrableContractsCallback;

		private Action<List<MyObjectBuilder_Contract>, long, long> m_localRequestActiveContractsCallback;

		private Action<List<MyObjectBuilder_ContractCondition>> m_localRequestActiveConditionsCallback;

		private Action<MyContractResults> m_localRequestAcceptCallback;

		private Action<MyContractResults> m_localRequestFinishCallback;

		private Action<MyContractResults> m_localRequestAbandonCallback;

		private Action<bool, List<MyTargetEntityInfoWrapper>, long> m_localRequestConnectedEntitiesCallback;

		private long m_localRequestConnectedentitiesContractId;

		private Action<List<MyEntityInfoWrapper>> m_localRequestOwnedContractBlocksCallback;

		private Action<List<MyEntityInfoWrapper>> m_localRequestOwnedGridsCallback;

		private Action<MyContractCreationResults> m_localRequestCreateCustomContractCallback;

		private Action<bool> m_localRequestDeleteCustomContractCallback;

		private MyResourceStateEnum m_currentState = MyResourceStateEnum.NoPower;

		private bool m_isTextPanelOpen;

		public new MyContractBlockDefinition BlockDefinition => (MyContractBlockDefinition)base.BlockDefinition;

		public bool AnyoneCanUse
		{
			get
			{
				return m_anyoneCanUse;
			}
			set
			{
				m_anyoneCanUse.Value = value;
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public override MyCubeBlockHighlightModes HighlightMode
		{
			get
			{
				if (AnyoneCanUse)
				{
					return MyCubeBlockHighlightModes.AlwaysCanUse;
				}
				return MyCubeBlockHighlightModes.Default;
			}
		}

		int IMyTextSurfaceProvider.SurfaceCount
		{
			get
			{
				if (m_multiPanel == null)
				{
					return 0;
				}
				return m_multiPanel.SurfaceCount;
			}
		}

		MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => m_multiPanel;

		public MyTextPanelComponent PanelComponent
		{
			get
			{
				if (m_multiPanel == null)
				{
					return null;
				}
				return m_multiPanel.PanelComponent;
			}
		}

		public bool IsTextPanelOpen
		{
			get
			{
				return m_isTextPanelOpen;
			}
			set
			{
				if (m_isTextPanelOpen != value)
				{
					m_isTextPanelOpen = value;
					RaisePropertiesChanged();
				}
			}
		}

		public MyContractBlock()
		{
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_ContractBlock myObjectBuilder_ContractBlock = objectBuilder as MyObjectBuilder_ContractBlock;
			AnyoneCanUse = myObjectBuilder_ContractBlock.AnyoneCanUse;
			InitializeConveyorEndpoint();
			if (BlockDefinition.ScreenAreas != null && BlockDefinition.ScreenAreas.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				m_multiPanel = new MyMultiTextPanelComponent(this, BlockDefinition.ScreenAreas, myObjectBuilder_ContractBlock.TextPanels);
				m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			}
			base.OnClose += OnClose_Callback;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		private void OnClose_Callback(MyEntity obj)
		{
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component != null)
			{
				component.ContractBlockDestroyed(base.EntityId);
				base.OnClose -= OnClose_Callback;
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return m_currentState == MyResourceStateEnum.Ok;
			}
			return false;
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ContractBlock myObjectBuilder_ContractBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_ContractBlock;
			myObjectBuilder_ContractBlock.AnyoneCanUse = AnyoneCanUse;
			if (m_multiPanel != null)
			{
				myObjectBuilder_ContractBlock.TextPanels = m_multiPanel.Serialize();
			}
			return myObjectBuilder_ContractBlock;
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_multiPanel != null)
			{
				m_multiPanel.SetRender(null);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (m_multiPanel != null)
			{
				m_multiPanel.Reset();
			}
			UpdateScreen();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(CheckIsWorking());
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (base.CubeGrid.GridSystems.ResourceDistributor != null)
			{
				MyResourceStateEnum currentState = m_currentState;
				m_currentState = base.CubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
				if (currentState != m_currentState)
				{
					UpdateIsWorking();
				}
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_multiPanel != null && m_multiPanel.SurfaceCount > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.AddToScene();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateScreen();
		}

		public void UpdateScreen()
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateScreen(CheckIsWorking());
			}
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 267)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTextRequest(int panelIndex, [Nullable] string text)
		{
			m_multiPanel?.ChangeText(panelIndex, text);
		}

		private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 283)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}

		[Event(null, 299)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.RemoveItems(panelIndex, selection);
			}
		}

		[Event(null, 308)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectItems(panelIndex, selection);
			}
		}

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)panelItems[0].UserData);
			}
			RaisePropertiesChanged();
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			CreateTextBox(isEditable, new StringBuilder(PanelComponent.Text.ToString()), isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBoxMultiPanel);
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string displayNameText = DisplayNameText;
			string displayName = PanelComponent.DisplayName;
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBoxMultiPanel != null)
			{
				if (m_textBoxMultiPanel.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(isPublic: true);
				}
			}
		}

		public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBoxMultiPanel.Description.Text.Remove(100000, m_textBoxMultiPanel.Description.Text.Length - 100000);
				CloseWindow(isPublic: true);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBoxMultiPanel.Description.Text, isPublic: true);
				MyScreenManager.AddScreen(m_textBoxMultiPanel);
			}
		}

		[Event(null, 388)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 399)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			foreach (MySlimBlock cubeBlock in base.CubeGrid.CubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == base.EntityId)
				{
					SendChangeDescriptionMessage(m_textBoxMultiPanel.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text.Clear().Append((object)description);
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 467)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			PanelComponent.Text.Clear().Append((object)stringBuilder);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyContractBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyContractBlock> obj = new MyTerminalControlCheckbox<MyContractBlock>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse)
				{
					Getter = ((MyContractBlock x) => x.AnyoneCanUse),
					Setter = delegate(MyContractBlock x, bool v)
					{
						x.AnyoneCanUse = v;
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyMultiTextPanelComponent.CreateTerminalControls<MyContractBlock>();
			}
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		internal void GetContractBlockStatus(Action<bool> resultCallback)
		{
			m_localRequestContractBlockStatucCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.GetContractBlockStatus);
		}

		internal void GetAvailableContracts(Action<List<MyObjectBuilder_Contract>> resultCallback)
		{
			m_localRequestAvailableContractsCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.GetAvailibleContracts);
		}

		internal void GetAdministrableContracts(Action<List<MyObjectBuilder_Contract>> resultCallback)
		{
			m_localRequestAdministrableContractsCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.GetAdministrableContracts);
		}

		internal void GetActiveContracts(long localPlayerId, Action<List<MyObjectBuilder_Contract>, long, long> resultCallback)
		{
			m_localRequestActiveContractsCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.GetActiveContracts, localPlayerId);
		}

		internal static void GetActiveContractsStatic(Action<List<MyObjectBuilder_Contract>> resultCallback)
		{
			m_localStaticRequestActiveContractsCallback = resultCallback;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => GetActiveContractsStatic);
		}

		internal void GetAllOwnedContractBlocks(long localPlayerId, Action<List<MyEntityInfoWrapper>> resultCallback)
		{
			m_localRequestOwnedContractBlocksCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.GetAllOwnedContractBlocks, localPlayerId);
		}

		internal void GetAllOwnedGrids(long localPlayerId, Action<List<MyEntityInfoWrapper>> resultCallback)
		{
			m_localRequestOwnedGridsCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.GetAllOwnedGrids, localPlayerId);
		}

		internal void AcceptContract(long localPlayerId, long contractId, Action<MyContractResults> resultCallback)
		{
			m_localRequestAcceptCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.AcceptContract, localPlayerId, contractId);
		}

		internal void FinishContract(long localPlayerId, long contractId, long targetEntityId, Action<MyContractResults> resultCallback)
		{
			m_localRequestFinishCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.FinishContract, localPlayerId, contractId, targetEntityId);
		}

		internal void AbandonContract(long localPlayerId, long contractId, Action<MyContractResults> resultCallback)
		{
			m_localRequestAbandonCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.AbandonContract, localPlayerId, contractId);
		}

		internal static void AbandonContractStatic(long contractId, Action<MyContractResults> resultCallback)
		{
			m_localStaticRequestAbandonCallback = resultCallback;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AbandonContractStatic, contractId);
		}

		internal void GetConnectedEntities(long localPlayerId, long contractId, Action<bool, List<MyTargetEntityInfoWrapper>, long> resultCallback)
		{
			m_localRequestConnectedEntitiesCallback = resultCallback;
			m_localRequestConnectedentitiesContractId = contractId;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.GetConnectedEntities, localPlayerId);
		}

		internal void CreateCustomContractDeliver(int rewardMoney, int startingDeposit, int durationInMin, long targetBlockId, Action<MyContractCreationResults> resultCallback)
		{
			MyContractCreationDataWrapper_Deliver myContractCreationDataWrapper_Deliver = default(MyContractCreationDataWrapper_Deliver);
			myContractCreationDataWrapper_Deliver.RewardMoney = rewardMoney;
			myContractCreationDataWrapper_Deliver.StartingDeposit = startingDeposit;
			myContractCreationDataWrapper_Deliver.DurationInMin = durationInMin;
			myContractCreationDataWrapper_Deliver.TargetBlockId = targetBlockId;
			MyContractCreationDataWrapper_Deliver arg = myContractCreationDataWrapper_Deliver;
			m_localRequestCreateCustomContractCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.CreateCustomContractDeliver, arg);
		}

		internal void DeleteCustomContract(long contractId, Action<bool> resultCallback)
		{
			m_localRequestDeleteCustomContractCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.DeleteCustomContract, contractId);
		}

		internal void CreateCustomContractObtainAndDeliver(int rewardMoney, int startingDeposit, int durationInMin, long targetBlockId, MyDefinitionId itemTypeId, int itemAmount, Action<MyContractCreationResults> resultCallback)
		{
			MyContractCreationDataWrapper_ObtainAndDeliver myContractCreationDataWrapper_ObtainAndDeliver = default(MyContractCreationDataWrapper_ObtainAndDeliver);
			myContractCreationDataWrapper_ObtainAndDeliver.RewardMoney = rewardMoney;
			myContractCreationDataWrapper_ObtainAndDeliver.StartingDeposit = startingDeposit;
			myContractCreationDataWrapper_ObtainAndDeliver.DurationInMin = durationInMin;
			myContractCreationDataWrapper_ObtainAndDeliver.TargetBlockId = targetBlockId;
			myContractCreationDataWrapper_ObtainAndDeliver.ItemTypeId = itemTypeId;
			myContractCreationDataWrapper_ObtainAndDeliver.ItemAmount = itemAmount;
			MyContractCreationDataWrapper_ObtainAndDeliver arg = myContractCreationDataWrapper_ObtainAndDeliver;
			m_localRequestCreateCustomContractCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.CreateCustomContractObtainAndDeliver, arg);
		}

		internal void CreateCustomContractFind(int rewardMoney, int startingDeposit, int durationInMin, long targetGridId, double searchRadius, Action<MyContractCreationResults> resultCallback)
		{
			MyContractCreationDataWrapper_Find myContractCreationDataWrapper_Find = default(MyContractCreationDataWrapper_Find);
			myContractCreationDataWrapper_Find.RewardMoney = rewardMoney;
			myContractCreationDataWrapper_Find.StartingDeposit = startingDeposit;
			myContractCreationDataWrapper_Find.DurationInMin = durationInMin;
			myContractCreationDataWrapper_Find.TargetGridId = targetGridId;
			myContractCreationDataWrapper_Find.SearchRadius = searchRadius;
			MyContractCreationDataWrapper_Find arg = myContractCreationDataWrapper_Find;
			m_localRequestCreateCustomContractCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.CreateCustomContractFind, arg);
		}

		internal void CreateCustomContractRepair(int rewardMoney, int startingDeposit, int durationInMin, long targetGridId, Action<MyContractCreationResults> resultCallback)
		{
			MyContractCreationDataWrapper_Repair myContractCreationDataWrapper_Repair = default(MyContractCreationDataWrapper_Repair);
			myContractCreationDataWrapper_Repair.RewardMoney = rewardMoney;
			myContractCreationDataWrapper_Repair.StartingDeposit = startingDeposit;
			myContractCreationDataWrapper_Repair.DurationInMin = durationInMin;
			myContractCreationDataWrapper_Repair.TargetGridId = targetGridId;
			MyContractCreationDataWrapper_Repair arg = myContractCreationDataWrapper_Repair;
			m_localRequestCreateCustomContractCallback = resultCallback;
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.CreateCustomContractRepair, arg);
		}

		[Event(null, 647)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetContractBlockStatus()
		{
			if (HasAccess())
			{
				bool arg = false;
				if (MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId) != null)
				{
					arg = true;
				}
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveContractBlockStatus, arg, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 668)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetAvailibleContracts()
		{
			if (!HasAccess())
			{
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component != null)
			{
				MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId);
				if (stationByGridId != null)
				{
					List<MyObjectBuilder_Contract> availableContractsForStation_OB = component.GetAvailableContractsForStation_OB(stationByGridId.Id);
					MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAvailableContracts, availableContractsForStation_OB, MyEventContext.Current.Sender);
				}
				else
				{
					List<MyObjectBuilder_Contract> availableContractsForBlock_OB = component.GetAvailableContractsForBlock_OB(base.EntityId);
					MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAvailableContracts, availableContractsForBlock_OB, MyEventContext.Current.Sender);
				}
			}
		}

		[Event(null, 698)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetAdministrableContracts()
		{
			if (HasAccess())
			{
				MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
				if (component != null && MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId) == null)
				{
					List<MyObjectBuilder_Contract> availableContractsForBlock_OB = component.GetAvailableContractsForBlock_OB(base.EntityId);
					MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAdministrableContracts, availableContractsForBlock_OB, MyEventContext.Current.Sender);
				}
			}
		}

		[Event(null, 725)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetActiveContracts(long identityId)
		{
			if (!HasAccess())
			{
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component != null)
			{
				long arg = 0L;
				long num = 0L;
				MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId);
				if (stationByGridId != null)
				{
					arg = stationByGridId.Id;
				}
				num = base.EntityId;
				List<MyObjectBuilder_Contract> activeContractsForPlayer_OB = component.GetActiveContractsForPlayer_OB(identityId);
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveActiveContracts, activeContractsForPlayer_OB, arg, num, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 753)]
		[Reliable]
		[Server(ValidationType.Access)]
		private static void GetActiveContractsStatic()
		{
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component != null)
			{
				long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
				if (num != 0L)
				{
					List<MyObjectBuilder_Contract> activeContractsForPlayer_OB = component.GetActiveContractsForPlayer_OB(num);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReceiveActiveContractsStatic, activeContractsForPlayer_OB, MyEventContext.Current.Sender);
				}
			}
		}

		[Event(null, 770)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetAllOwnedContractBlocks(long identityId)
		{
			if (HasAccess())
			{
				List<MyEntityInfoWrapper> list = new List<MyEntityInfoWrapper>();
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
				if (myIdentity != null)
				{
					foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> item in myIdentity.BlockLimits.BlocksBuiltByGrid)
					{
						if (!MyEntities.TryGetEntityById(item.Key, out MyCubeGrid entity) || entity.BigOwners.Contains(identityId))
						{
							foreach (MySlimBlock block in entity.GetBlocks())
							{
								if (block.FatBlock != null && block.FatBlock is MyContractBlock)
								{
									list.Add(new MyEntityInfoWrapper
									{
										NamePrefix = (string.IsNullOrEmpty(entity.DisplayName) ? string.Empty : entity.DisplayName),
										NameSuffix = (string.IsNullOrEmpty(block.FatBlock.DisplayNameText) ? string.Empty : block.FatBlock.DisplayNameText),
										Id = block.FatBlock.EntityId
									});
								}
							}
						}
					}
				}
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAllOwnedContractBlocks, list, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 806)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetAllOwnedGrids(long identityId)
		{
			if (HasAccess())
			{
				List<MyEntityInfoWrapper> list = new List<MyEntityInfoWrapper>();
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
				if (myIdentity != null)
				{
					foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> item in myIdentity.BlockLimits.BlocksBuiltByGrid)
					{
						if (!MyEntities.TryGetEntityById(item.Key, out MyCubeGrid entity) || entity.BigOwners.Contains(identityId))
						{
							list.Add(new MyEntityInfoWrapper
							{
								NamePrefix = (string.IsNullOrEmpty(entity.DisplayName) ? string.Empty : entity.DisplayName),
								NameSuffix = string.Empty,
								Id = entity.EntityId
							});
						}
					}
				}
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAllOwnedGrids, list, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 838)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void AcceptContract(long identityId, long contractId)
		{
			if (!HasAccess())
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAcceptContract, MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAcceptContract, MyContractResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
				return;
			}
			MyContractResults arg = component.ActivateContract(identityId, contractId, MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId)?.Id ?? 0, base.EntityId);
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAcceptContract, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 865)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void FinishContract(long identityId, long contractId, long targetEntityId)
		{
			if (!HasAccess())
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, MyContractResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
				return;
			}
			if (!component.IsContractActive(contractId))
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, MyContractResults.Fail_ContractNotFound_Finish, MyEventContext.Current.Sender);
				return;
			}
			MyContract activeContractById = component.GetActiveContractById(contractId);
			if (activeContractById == null)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, MyContractResults.Fail_ContractNotFound_Finish, MyEventContext.Current.Sender);
				return;
			}
			if (!activeContractById.Owners.Contains(identityId))
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, MyContractResults.Error_InvalidData, MyEventContext.Current.Sender);
				return;
			}
			MyContractCondition contractCondition = activeContractById.ContractCondition;
			MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(base.CubeGrid.EntityId);
			if (contractCondition != null && !contractCondition.IsFinished && ((stationByGridId != null && stationByGridId.Id == contractCondition.StationEndId) || base.EntityId == contractCondition.BlockEndId))
			{
				if (!CheckConnectedEntityOwnership(MyEventContext.Current.Sender.Value, targetEntityId))
				{
					MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
					return;
				}
				MyContractResults myContractResults = component.FinishContractCondition(identityId, activeContractById, contractCondition, targetEntityId);
				if (myContractResults == MyContractResults.Success && activeContractById.CanBeFinished)
				{
					component.FinishContract(identityId, activeContractById);
				}
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, myContractResults, MyEventContext.Current.Sender);
			}
			else if (activeContractById.CanBeFinished)
			{
				MyContractResults arg = component.FinishContract(identityId, activeContractById);
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, arg, MyEventContext.Current.Sender);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveFinishContract, MyContractResults.Fail_FinishConditionsNotMet, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 935)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void AbandonContract(long identityId, long contractId)
		{
			if (HasAccess())
			{
				MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
				if (component != null)
				{
					MyContractResults arg = component.AbandonContract(identityId, contractId);
					MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveAbandonContract, arg, MyEventContext.Current.Sender);
				}
			}
		}

		[Event(null, 957)]
		[Reliable]
		[Server(ValidationType.Access)]
		private static void AbandonContractStatic(long contractId)
		{
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (num == 0L)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReceiveAbandonContractStatic, MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReceiveAbandonContractStatic, MyContractResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
				return;
			}
			MyContractResults arg = component.AbandonContract(num, contractId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReceiveAbandonContractStatic, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 979)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void GetConnectedEntities(long identityId)
		{
			List<MyTargetEntityInfoWrapper> list = new List<MyTargetEntityInfoWrapper>();
			if (!HasAccess())
			{
				return;
			}
			MyTargetEntityInfoWrapper item;
			if (MySession.Static.Players.TryGetPlayerId(identityId, out MyPlayer.PlayerId result))
			{
				MyCharacter myCharacter = MySession.Static.Players.GetPlayerById(result)?.Character;
				if (myCharacter != null)
				{
					item = new MyTargetEntityInfoWrapper
					{
						Id = myCharacter.EntityId,
						Name = (string.IsNullOrEmpty(myCharacter.Name) ? string.Empty : myCharacter.Name),
						DisplayName = MyTexts.GetString(MySpaceTexts.Economy_CharacterSelection)
					};
					list.Add(item);
				}
			}
			foreach (MyCubeGrid allConnectedTradingShip in GetAllConnectedTradingShips())
			{
				if (allConnectedTradingShip.BigOwners.Contains(identityId))
				{
					item = new MyTargetEntityInfoWrapper
					{
						Id = allConnectedTradingShip.EntityId,
						Name = (string.IsNullOrEmpty(allConnectedTradingShip.Name) ? string.Empty : allConnectedTradingShip.Name),
						DisplayName = (string.IsNullOrEmpty(allConnectedTradingShip.DisplayName) ? string.Empty : allConnectedTradingShip.DisplayName)
					};
					list.Add(item);
				}
			}
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveGetConnectedGrids, arg2: true, list, MyEventContext.Current.Sender);
		}

		private bool CheckConnectedEntityOwnership(ulong steamId, long entityId)
		{
			MyEntity entityById = MyEntities.GetEntityById(entityId);
			MyCharacter myCharacter;
			MyCubeGrid myCubeGrid;
			if ((myCharacter = (entityById as MyCharacter)) != null)
			{
				if (myCharacter.ControllerInfo != null && myCharacter.ControllerInfo.Controller != null && myCharacter.ControllerInfo.Controller.Player != null && myCharacter.ControllerInfo.Controller.Player.Id.SteamId == steamId)
				{
					return true;
				}
			}
			else if ((myCubeGrid = (entityById as MyCubeGrid)) != null)
			{
				long num = MySession.Static.Players.TryGetIdentityId(steamId);
				if (num != 0L && myCubeGrid.BigOwners.Contains(num))
				{
					return true;
				}
			}
			return false;
		}

		[Event(null, 1075)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void CreateCustomContractDeliver(MyContractCreationDataWrapper_Deliver data)
		{
			if (!HasAccess())
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_NoAccess, MyEventContext.Current.Sender);
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
				return;
			}
			int count = component.GetAvailableContractsForBlock_OB(base.EntityId).Count;
			if (component.GetContractCreationLimitPerPlayer() <= count)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
				return;
			}
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (num == 0L || base.OwnerId != num)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
				return;
			}
			long contractId;
			long contractConditionId;
			MyContractCreationResults arg = component.GenerateCustomContract_Deliver(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetBlockId, out contractId, out contractConditionId);
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 1112)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void CreateCustomContractObtainAndDeliver(MyContractCreationDataWrapper_ObtainAndDeliver data)
		{
			if (!HasAccess())
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_NoAccess, MyEventContext.Current.Sender);
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
				return;
			}
			int count = component.GetAvailableContractsForBlock_OB(base.EntityId).Count;
			if (component.GetContractCreationLimitPerPlayer() <= count)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
				return;
			}
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (num == 0L || base.OwnerId != num)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
				return;
			}
			long contractId;
			long contractConditionId;
			MyContractCreationResults arg = component.GenerateCustomContract_ObtainAndDeliver(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetBlockId, data.ItemTypeId, data.ItemAmount, out contractId, out contractConditionId);
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 1149)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void CreateCustomContractRepair(MyContractCreationDataWrapper_Repair data)
		{
			if (!HasAccess())
			{
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return;
			}
			int count = component.GetAvailableContractsForBlock_OB(base.EntityId).Count;
			if (component.GetContractCreationLimitPerPlayer() <= count)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
				return;
			}
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (num == 0L || base.OwnerId != num)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
				return;
			}
			long contractId;
			long contractConditionId;
			MyContractCreationResults arg = component.GenerateCustomContract_Repair(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetGridId, out contractId, out contractConditionId);
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 1187)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void CreateCustomContractFind(MyContractCreationDataWrapper_Find data)
		{
			if (!HasAccess())
			{
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component == null)
			{
				return;
			}
			int count = component.GetAvailableContractsForBlock_OB(base.EntityId).Count;
			if (component.GetContractCreationLimitPerPlayer() <= count)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
				return;
			}
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (num == 0L || base.OwnerId != num)
			{
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
				return;
			}
			long contractId;
			long contractConditionId;
			MyContractCreationResults arg = component.GenerateCustomContract_Find(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetGridId, data.SearchRadius, out contractId, out contractConditionId);
			MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, arg, MyEventContext.Current.Sender);
		}

		[Event(null, 1224)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void DeleteCustomContract(long contractId)
		{
			if (!HasAccess())
			{
				return;
			}
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			if (component != null)
			{
				long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
				if (num == 0L || base.OwnerId != num)
				{
					MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveCreateContractResult, MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
					return;
				}
				component.DeleteCustomContract(this, contractId);
				MyMultiplayer.RaiseEvent(this, (MyContractBlock x) => x.ReceiveDeleteCustomContractResult, arg2: true, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 1253)]
		[Reliable]
		[Client]
		private void ReceiveContractBlockStatus(bool isNpc)
		{
			m_localRequestContractBlockStatucCallback?.Invoke(isNpc);
			m_localRequestContractBlockStatucCallback = null;
		}

		[Event(null, 1260)]
		[Reliable]
		[Client]
		private void ReceiveAvailableContracts([DynamicItem(typeof(MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> availableContracts)
		{
			m_localRequestAvailableContractsCallback?.Invoke(availableContracts);
			m_localRequestAvailableContractsCallback = null;
		}

		[Event(null, 1267)]
		[Reliable]
		[Client]
		private void ReceiveAdministrableContracts([DynamicItem(typeof(MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> administrableContracts)
		{
			m_localRequestAdministrableContractsCallback?.Invoke(administrableContracts);
			m_localRequestAdministrableContractsCallback = null;
		}

		[Event(null, 1274)]
		[Reliable]
		[Client]
		private void ReceiveActiveContracts([DynamicItem(typeof(MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> activeContracts, long stationId, long blockId)
		{
			m_localRequestActiveContractsCallback?.Invoke(activeContracts, stationId, blockId);
			m_localRequestActiveContractsCallback = null;
		}

		[Event(null, 1281)]
		[Reliable]
		[Client]
		private static void ReceiveActiveContractsStatic([DynamicItem(typeof(MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> activeContracts)
		{
			m_localStaticRequestActiveContractsCallback?.Invoke(activeContracts);
			m_localStaticRequestActiveContractsCallback = null;
		}

		[Event(null, 1288)]
		[Reliable]
		[Client]
		private void ReceiveAllOwnedContractBlocks(List<MyEntityInfoWrapper> data)
		{
			m_localRequestOwnedContractBlocksCallback?.Invoke(data);
			m_localRequestOwnedContractBlocksCallback = null;
		}

		[Event(null, 1295)]
		[Reliable]
		[Client]
		private void ReceiveAllOwnedGrids(List<MyEntityInfoWrapper> data)
		{
			m_localRequestOwnedGridsCallback?.Invoke(data);
			m_localRequestOwnedGridsCallback = null;
		}

		[Event(null, 1302)]
		[Reliable]
		[Client]
		private void ReceiveActiveConditions([DynamicItem(typeof(MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_ContractCondition> activeConditions)
		{
			m_localRequestActiveConditionsCallback?.Invoke(activeConditions);
			m_localRequestActiveConditionsCallback = null;
		}

		[Event(null, 1309)]
		[Reliable]
		[Client]
		private void ReceiveAcceptContract(MyContractResults result)
		{
			m_localRequestAcceptCallback?.Invoke(result);
			m_localRequestAcceptCallback = null;
		}

		[Event(null, 1316)]
		[Reliable]
		[Client]
		private void ReceiveFinishContract(MyContractResults result)
		{
			m_localRequestFinishCallback?.Invoke(result);
			m_localRequestFinishCallback = null;
		}

		[Event(null, 1323)]
		[Reliable]
		[Client]
		private void ReceiveAbandonContract(MyContractResults result)
		{
			m_localRequestAbandonCallback?.Invoke(result);
			m_localRequestAbandonCallback = null;
		}

		[Event(null, 1330)]
		[Reliable]
		[Client]
		private static void ReceiveAbandonContractStatic(MyContractResults result)
		{
			m_localStaticRequestAbandonCallback?.Invoke(result);
			m_localStaticRequestAbandonCallback = null;
		}

		[Event(null, 1337)]
		[Reliable]
		[Client]
		private void ReceiveGetConnectedGrids(bool isSuccessful, List<MyTargetEntityInfoWrapper> connectedGrids)
		{
			m_localRequestConnectedEntitiesCallback?.Invoke(isSuccessful, connectedGrids, m_localRequestConnectedentitiesContractId);
			m_localRequestConnectedEntitiesCallback = null;
		}

		[Event(null, 1344)]
		[Reliable]
		[Client]
		private void ReceiveCreateContractResult(MyContractCreationResults result)
		{
			m_localRequestCreateCustomContractCallback?.Invoke(result);
			m_localRequestCreateCustomContractCallback = null;
		}

		[Event(null, 1351)]
		[Reliable]
		[Client]
		private void ReceiveDeleteCustomContractResult(bool success)
		{
			m_localRequestDeleteCustomContractCallback?.Invoke(success);
			m_localRequestDeleteCustomContractCallback = null;
		}

		private bool HasAccess()
		{
			long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			GetPlayer(identityId);
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(identityId);
			bool flag = false;
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(base.OwnerId);
			if (myFaction != null)
			{
				flag = MySession.Static.Factions.IsNpcFaction(myFaction.Tag);
			}
			if (!AnyoneCanUse || (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.Enemies && flag))
			{
				return HasPlayerAccess(identityId);
			}
			return true;
		}

		private MyPlayer GetPlayer(long identityId)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null || myIdentity.Character == null)
			{
				return null;
			}
			return MyPlayer.GetPlayerFromCharacter(myIdentity.Character);
		}

		public PullInformation GetPushInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty constraint")
			};
		}

		public List<MyCubeGrid> GetAllConnectedTradingShips()
		{
			List<MyCubeGrid> list = new List<MyCubeGrid>();
			foreach (MyShipConnector fatBlock in base.CubeGrid.GetFatBlocks<MyShipConnector>())
			{
				if (fatBlock != null && fatBlock.Connected && (bool)fatBlock.TradingEnabled)
				{
					MyShipConnector other = fatBlock.Other;
					if (other != null)
					{
						list.Add(other.CubeGrid);
					}
				}
			}
			return list;
		}
	}
}
