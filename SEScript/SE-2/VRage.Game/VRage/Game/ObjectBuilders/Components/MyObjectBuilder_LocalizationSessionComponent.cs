using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_LocalizationSessionComponent : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003EAdditionalPaths_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in List<string> value)
			{
				owner.AdditionalPaths = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out List<string> value)
			{
				value = owner.AdditionalPaths;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003ECampaignPaths_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in List<string> value)
			{
				owner.CampaignPaths = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out List<string> value)
			{
				value = owner.CampaignPaths;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003ECampaignModFolderName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in string value)
			{
				owner.CampaignModFolderName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out string value)
			{
				value = owner.CampaignModFolderName;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003ELanguage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in string value)
			{
				owner.Language = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out string value)
			{
				value = owner.Language;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_LocalizationSessionComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_LocalizationSessionComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_LocalizationSessionComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_LocalizationSessionComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_LocalizationSessionComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_LocalizationSessionComponent CreateInstance()
			{
				return new MyObjectBuilder_LocalizationSessionComponent();
			}

			MyObjectBuilder_LocalizationSessionComponent IActivator<MyObjectBuilder_LocalizationSessionComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<string> AdditionalPaths = new List<string>();

		public List<string> CampaignPaths = new List<string>();

		public string CampaignModFolderName = string.Empty;

		public string Language = "English";
	}
}
