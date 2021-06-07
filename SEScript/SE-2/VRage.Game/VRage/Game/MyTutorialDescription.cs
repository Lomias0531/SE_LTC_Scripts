using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyTutorialDescription
	{
		protected class VRage_Game_MyTutorialDescription_003C_003EName_003C_003EAccessor : IMemberAccessor<MyTutorialDescription, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTutorialDescription owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTutorialDescription owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyTutorialDescription_003C_003EUnlockedBy_003C_003EAccessor : IMemberAccessor<MyTutorialDescription, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyTutorialDescription owner, in string[] value)
			{
				owner.UnlockedBy = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyTutorialDescription owner, out string[] value)
			{
				value = owner.UnlockedBy;
			}
		}

		private class VRage_Game_MyTutorialDescription_003C_003EActor : IActivator, IActivator<MyTutorialDescription>
		{
			private sealed override object CreateInstance()
			{
				return default(MyTutorialDescription);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTutorialDescription CreateInstance()
			{
				return (MyTutorialDescription)(object)default(MyTutorialDescription);
			}

			MyTutorialDescription IActivator<MyTutorialDescription>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string Name;

		[ProtoMember(4)]
		[XmlArrayItem("Tutorial")]
		public string[] UnlockedBy;
	}
}
