using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Gui;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ToolItemDefinition : MyObjectBuilder_PhysicalItemDefinition
	{
		[ProtoContract]
		public class MyVoxelMiningDefinition
		{
			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyVoxelMiningDefinition_003C_003EMinedOre_003C_003EAccessor : IMemberAccessor<MyVoxelMiningDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyVoxelMiningDefinition owner, in string value)
				{
					owner.MinedOre = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyVoxelMiningDefinition owner, out string value)
				{
					value = owner.MinedOre;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyVoxelMiningDefinition_003C_003EHitCount_003C_003EAccessor : IMemberAccessor<MyVoxelMiningDefinition, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyVoxelMiningDefinition owner, in int value)
				{
					owner.HitCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyVoxelMiningDefinition owner, out int value)
				{
					value = owner.HitCount;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyVoxelMiningDefinition_003C_003EPhysicalItemId_003C_003EAccessor : IMemberAccessor<MyVoxelMiningDefinition, SerializableDefinitionId>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyVoxelMiningDefinition owner, in SerializableDefinitionId value)
				{
					owner.PhysicalItemId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyVoxelMiningDefinition owner, out SerializableDefinitionId value)
				{
					value = owner.PhysicalItemId;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyVoxelMiningDefinition_003C_003ERemovedRadius_003C_003EAccessor : IMemberAccessor<MyVoxelMiningDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyVoxelMiningDefinition owner, in float value)
				{
					owner.RemovedRadius = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyVoxelMiningDefinition owner, out float value)
				{
					value = owner.RemovedRadius;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyVoxelMiningDefinition_003C_003EOnlyApplyMaterial_003C_003EAccessor : IMemberAccessor<MyVoxelMiningDefinition, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyVoxelMiningDefinition owner, in bool value)
				{
					owner.OnlyApplyMaterial = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyVoxelMiningDefinition owner, out bool value)
				{
					value = owner.OnlyApplyMaterial;
				}
			}

			private class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyVoxelMiningDefinition_003C_003EActor : IActivator, IActivator<MyVoxelMiningDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new MyVoxelMiningDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyVoxelMiningDefinition CreateInstance()
				{
					return new MyVoxelMiningDefinition();
				}

				MyVoxelMiningDefinition IActivator<MyVoxelMiningDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[DefaultValue(null)]
			public string MinedOre;

			[ProtoMember(4)]
			[DefaultValue(0)]
			public int HitCount;

			[ProtoMember(7)]
			[DefaultValue(null)]
			public SerializableDefinitionId PhysicalItemId;

			[ProtoMember(10)]
			[DefaultValue(0f)]
			public float RemovedRadius;

			[ProtoMember(13)]
			[DefaultValue(false)]
			public bool OnlyApplyMaterial;
		}

		[ProtoContract]
		public class MyToolActionHitCondition
		{
			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EEntityType_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, string[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in string[] value)
				{
					owner.EntityType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out string[] value)
				{
					value = owner.EntityType;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EAnimation_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in string value)
				{
					owner.Animation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out string value)
				{
					value = owner.Animation;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EAnimationTimeScale_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in float value)
				{
					owner.AnimationTimeScale = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out float value)
				{
					value = owner.AnimationTimeScale;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EStatsAction_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in string value)
				{
					owner.StatsAction = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out string value)
				{
					value = owner.StatsAction;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EStatsActionIfHit_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in string value)
				{
					owner.StatsActionIfHit = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out string value)
				{
					value = owner.StatsActionIfHit;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EStatsModifier_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in string value)
				{
					owner.StatsModifier = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out string value)
				{
					value = owner.StatsModifier;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EStatsModifierIfHit_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in string value)
				{
					owner.StatsModifierIfHit = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out string value)
				{
					value = owner.StatsModifierIfHit;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EComponent_003C_003EAccessor : IMemberAccessor<MyToolActionHitCondition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionHitCondition owner, in string value)
				{
					owner.Component = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionHitCondition owner, out string value)
				{
					value = owner.Component;
				}
			}

			private class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionHitCondition_003C_003EActor : IActivator, IActivator<MyToolActionHitCondition>
			{
				private sealed override object CreateInstance()
				{
					return new MyToolActionHitCondition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyToolActionHitCondition CreateInstance()
				{
					return new MyToolActionHitCondition();
				}

				MyToolActionHitCondition IActivator<MyToolActionHitCondition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(16)]
			[DefaultValue(null)]
			public string[] EntityType;

			[ProtoMember(19)]
			public string Animation;

			[ProtoMember(22)]
			public float AnimationTimeScale = 1f;

			[ProtoMember(25)]
			public string StatsAction;

			[ProtoMember(28)]
			public string StatsActionIfHit;

			[ProtoMember(31)]
			public string StatsModifier;

			[ProtoMember(34)]
			public string StatsModifierIfHit;

			[ProtoMember(37)]
			public string Component;
		}

		[ProtoContract]
		public class MyToolActionDefinition
		{
			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EName_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EStartTime_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in float value)
				{
					owner.StartTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out float value)
				{
					value = owner.StartTime;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EEndTime_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in float value)
				{
					owner.EndTime = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out float value)
				{
					value = owner.EndTime;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EEfficiency_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in float value)
				{
					owner.Efficiency = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out float value)
				{
					value = owner.Efficiency;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EStatsEfficiency_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in string value)
				{
					owner.StatsEfficiency = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out string value)
				{
					value = owner.StatsEfficiency;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003ESwingSound_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in string value)
				{
					owner.SwingSound = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out string value)
				{
					value = owner.SwingSound;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003ESwingSoundStart_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in float value)
				{
					owner.SwingSoundStart = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out float value)
				{
					value = owner.SwingSoundStart;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EHitStart_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in float value)
				{
					owner.HitStart = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out float value)
				{
					value = owner.HitStart;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EHitDuration_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in float value)
				{
					owner.HitDuration = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out float value)
				{
					value = owner.HitDuration;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EHitSound_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in string value)
				{
					owner.HitSound = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out string value)
				{
					value = owner.HitSound;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003ECustomShapeRadius_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in float value)
				{
					owner.CustomShapeRadius = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out float value)
				{
					value = owner.CustomShapeRadius;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003ECrosshair_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, MyHudTexturesEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in MyHudTexturesEnum value)
				{
					owner.Crosshair = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out MyHudTexturesEnum value)
				{
					value = owner.Crosshair;
				}
			}

			protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EHitConditions_003C_003EAccessor : IMemberAccessor<MyToolActionDefinition, MyToolActionHitCondition[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyToolActionDefinition owner, in MyToolActionHitCondition[] value)
				{
					owner.HitConditions = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyToolActionDefinition owner, out MyToolActionHitCondition[] value)
				{
					value = owner.HitConditions;
				}
			}

			private class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMyToolActionDefinition_003C_003EActor : IActivator, IActivator<MyToolActionDefinition>
			{
				private sealed override object CreateInstance()
				{
					return new MyToolActionDefinition();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyToolActionDefinition CreateInstance()
				{
					return new MyToolActionDefinition();
				}

				MyToolActionDefinition IActivator<MyToolActionDefinition>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(40)]
			public string Name;

			[ProtoMember(43)]
			[DefaultValue(0)]
			public float StartTime;

			[ProtoMember(46)]
			[DefaultValue(1)]
			public float EndTime = 1f;

			[ProtoMember(49)]
			[DefaultValue(1f)]
			public float Efficiency = 1f;

			[ProtoMember(52)]
			[DefaultValue(null)]
			public string StatsEfficiency;

			[ProtoMember(55)]
			[DefaultValue(null)]
			public string SwingSound;

			[ProtoMember(58)]
			[DefaultValue(0f)]
			public float SwingSoundStart;

			[ProtoMember(61)]
			[DefaultValue(0f)]
			public float HitStart;

			[ProtoMember(64)]
			[DefaultValue(1f)]
			public float HitDuration = 1f;

			[ProtoMember(67)]
			[DefaultValue(null)]
			public string HitSound;

			[ProtoMember(70)]
			[DefaultValue(0f)]
			public float CustomShapeRadius;

			[ProtoMember(73)]
			public MyHudTexturesEnum Crosshair = MyHudTexturesEnum.HudOre;

			[XmlArrayItem("HitCondition")]
			[ProtoMember(76)]
			[DefaultValue(null)]
			public MyToolActionHitCondition[] HitConditions;
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EVoxelMinings_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyVoxelMiningDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyVoxelMiningDefinition[] value)
			{
				owner.VoxelMinings = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyVoxelMiningDefinition[] value)
			{
				value = owner.VoxelMinings;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EPrimaryActions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyToolActionDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyToolActionDefinition[] value)
			{
				owner.PrimaryActions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyToolActionDefinition[] value)
			{
				value = owner.PrimaryActions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ESecondaryActions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyToolActionDefinition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyToolActionDefinition[] value)
			{
				owner.SecondaryActions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyToolActionDefinition[] value)
			{
				value = owner.SecondaryActions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EHitDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float value)
			{
				owner.HitDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float value)
			{
				value = owner.HitDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ESize_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ESize_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in Vector3 value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out Vector3 value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMass_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMass_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EModel_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModel_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EModels_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModels_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EIconSymbol_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EIconSymbol_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EModelVolume_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EModelVolume_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EPhysicalMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EVoxelMaterial_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanSpawnFromScreen_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnX_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnY_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ERotateOnSpawnZ_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EHealth_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EHealth_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieceId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EDestroyedPieces_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EExtraInventoryTooltipLine_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaxStackAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyFixedPoint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyFixedPoint value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyFixedPoint value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimalPricePerUnit_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOfferAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumOrderAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003ECanPlayerOrder_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMinimumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalItemDefinition_003C_003EMaximumAcquisitionAmount_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in int value)
			{
				Set(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out int value)
			{
				Get(ref *(MyObjectBuilder_PhysicalItemDefinition*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ToolItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ToolItemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ToolItemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ToolItemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ToolItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ToolItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ToolItemDefinition CreateInstance()
			{
				return new MyObjectBuilder_ToolItemDefinition();
			}

			MyObjectBuilder_ToolItemDefinition IActivator<MyObjectBuilder_ToolItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("Mining")]
		[ProtoMember(79)]
		[DefaultValue(null)]
		public MyVoxelMiningDefinition[] VoxelMinings;

		[XmlArrayItem("Action")]
		[ProtoMember(82)]
		[DefaultValue(null)]
		public MyToolActionDefinition[] PrimaryActions;

		[XmlArrayItem("Action")]
		[ProtoMember(85)]
		[DefaultValue(null)]
		public MyToolActionDefinition[] SecondaryActions;

		[ProtoMember(88)]
		[DefaultValue(1)]
		public float HitDistance = 1f;
	}
}
