using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions.SessionComponents
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ContainerDropSystemDefinition : MyObjectBuilder_SessionComponentDefinition
	{
		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EPersonalContainerRatio_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.PersonalContainerRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.PersonalContainerRatio;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EContainerDropTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.ContainerDropTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.ContainerDropTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EPersonalContainerDistMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.PersonalContainerDistMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.PersonalContainerDistMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EPersonalContainerDistMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.PersonalContainerDistMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.PersonalContainerDistMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003ECompetetiveContainerDistMin_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.CompetetiveContainerDistMin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.CompetetiveContainerDistMin;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003ECompetetiveContainerDistMax_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.CompetetiveContainerDistMax = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.CompetetiveContainerDistMax;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003ECompetetiveContainerGPSTimeOut_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.CompetetiveContainerGPSTimeOut = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.CompetetiveContainerGPSTimeOut;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003ECompetetiveContainerGridTimeOut_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.CompetetiveContainerGridTimeOut = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.CompetetiveContainerGridTimeOut;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EPersonalContainerGridTimeOut_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in float value)
			{
				owner.PersonalContainerGridTimeOut = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out float value)
			{
				value = owner.PersonalContainerGridTimeOut;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003ECompetetiveContainerGPSColorFree_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, RGBColor>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in RGBColor value)
			{
				owner.CompetetiveContainerGPSColorFree = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out RGBColor value)
			{
				value = owner.CompetetiveContainerGPSColorFree;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003ECompetetiveContainerGPSColorClaimed_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, RGBColor>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in RGBColor value)
			{
				owner.CompetetiveContainerGPSColorClaimed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out RGBColor value)
			{
				value = owner.CompetetiveContainerGPSColorClaimed;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EPersonalContainerGPSColor_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, RGBColor>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in RGBColor value)
			{
				owner.PersonalContainerGPSColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out RGBColor value)
			{
				value = owner.PersonalContainerGPSColor;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EContainerAudioCue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string value)
			{
				owner.ContainerAudioCue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string value)
			{
				value = owner.ContainerAudioCue;
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in bool value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out bool value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string[] value)
			{
				Set(ref *(MyObjectBuilder_DefinitionBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string[] value)
			{
				Get(ref *(MyObjectBuilder_DefinitionBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ContainerDropSystemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ContainerDropSystemDefinition owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ContainerDropSystemDefinition owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Definitions_SessionComponents_MyObjectBuilder_ContainerDropSystemDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ContainerDropSystemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ContainerDropSystemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ContainerDropSystemDefinition CreateInstance()
			{
				return new MyObjectBuilder_ContainerDropSystemDefinition();
			}

			MyObjectBuilder_ContainerDropSystemDefinition IActivator<MyObjectBuilder_ContainerDropSystemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Chance that the next cache will be personal
		/// </summary>
		public float PersonalContainerRatio = 0.95f;

		/// <summary>
		/// Cooldown after cache is retrieved in minutes
		/// </summary>
		public float ContainerDropTime = 30f;

		/// <summary>
		/// Personal cache minimum distance in kilometers
		/// </summary>
		public float PersonalContainerDistMin = 1f;

		/// <summary>
		/// Personal cache maximum distance in kilometers
		/// </summary>
		public float PersonalContainerDistMax = 15f;

		/// <summary>
		/// Competetive cache minimum distance in kilometers
		/// </summary>
		public float CompetetiveContainerDistMin = 15f;

		/// <summary>
		/// Competetive cache maximum distance in kilometers
		/// </summary>
		public float CompetetiveContainerDistMax = 30f;

		/// <summary>
		/// Time in minutes how long will GPS stay after discovery for other players in competetive caches
		/// </summary>
		public float CompetetiveContainerGPSTimeOut = 5f;

		/// <summary>
		/// Time in minutes how long will competetive cache exist in the world
		/// </summary>
		public float CompetetiveContainerGridTimeOut = 60f;

		/// <summary>
		/// Time in minutes how long will Personal cache exist in the world
		/// </summary>
		public float PersonalContainerGridTimeOut = 45f;

		/// <summary>
		/// GPS color for competive containers before they are claimed
		/// </summary>
		public RGBColor CompetetiveContainerGPSColorFree;

		/// <summary>
		/// GPS color for competive containers after they are claimed
		/// </summary>
		public RGBColor CompetetiveContainerGPSColorClaimed;

		/// <summary>
		/// GPS color for personal containers
		/// </summary>
		public RGBColor PersonalContainerGPSColor;

		/// <summary>
		/// Audio cue that will be looped until claimed
		/// </summary>
		public string ContainerAudioCue = "BlockContainer";
	}
}
