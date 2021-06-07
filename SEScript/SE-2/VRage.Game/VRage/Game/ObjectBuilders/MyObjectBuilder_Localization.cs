using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Localization : MyObjectBuilder_Base
	{
		public struct KeyEntry
		{
			public string Key;

			public string Value;
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, uint>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in uint value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out uint value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003ELanguage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in string value)
			{
				owner.Language = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out string value)
			{
				value = owner.Language;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EContext_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in string value)
			{
				owner.Context = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out string value)
			{
				value = owner.Context;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EResourceName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in string value)
			{
				owner.ResourceName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out string value)
			{
				value = owner.ResourceName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EDefault_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in bool value)
			{
				owner.Default = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out bool value)
			{
				value = owner.Default;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EResXName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in string value)
			{
				owner.ResXName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out string value)
			{
				value = owner.ResXName;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EEntries_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, List<KeyEntry>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in List<KeyEntry> value)
			{
				owner.Entries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out List<KeyEntry> value)
			{
				value = owner.Entries;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EModified_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Localization, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Localization owner, in bool value)
			{
				owner.Modified = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Localization owner, out bool value)
			{
				value = owner.Modified;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Localization owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Localization owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Localization owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Localization owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Localization owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Localization owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Localization, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Localization owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Localization owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_Localization_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Localization>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Localization();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Localization CreateInstance()
			{
				return new MyObjectBuilder_Localization();
			}

			MyObjectBuilder_Localization IActivator<MyObjectBuilder_Localization>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public uint Id;

		public string Language = "English";

		public string Context = "VRage";

		public string ResourceName = "Default Name";

		public bool Default;

		public string ResXName;

		[XmlIgnore]
		public List<KeyEntry> Entries = new List<KeyEntry>();

		[XmlIgnore]
		public bool Modified;

		public override string ToString()
		{
			return ResourceName + " " + Id;
		}
	}
}
