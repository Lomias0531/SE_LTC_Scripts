using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Gui
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Questlog : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003ELineData_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, List<MultilineData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Questlog owner, in List<MultilineData> value)
			{
				owner.LineData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Questlog owner, out List<MultilineData> value)
			{
				value = owner.LineData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003ETitle_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Questlog owner, in string value)
			{
				owner.Title = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Questlog owner, out string value)
			{
				value = owner.Title;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003EVisible_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Questlog owner, in bool value)
			{
				owner.Visible = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Questlog owner, out bool value)
			{
				value = owner.Visible;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003EIsUsedByVisualScripting_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Questlog, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Questlog owner, in bool value)
			{
				owner.IsUsedByVisualScripting = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Questlog owner, out bool value)
			{
				value = owner.IsUsedByVisualScripting;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Questlog owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Questlog owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Questlog owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Questlog owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Questlog owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Questlog owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Questlog, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Questlog owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Questlog owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_Questlog_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Questlog>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Questlog();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Questlog CreateInstance()
			{
				return new MyObjectBuilder_Questlog();
			}

			MyObjectBuilder_Questlog IActivator<MyObjectBuilder_Questlog>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MultilineData> LineData = new List<MultilineData>();

		public string Title = string.Empty;

		public bool Visible = true;

		public bool IsUsedByVisualScripting;
	}
}
