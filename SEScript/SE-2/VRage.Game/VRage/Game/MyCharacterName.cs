using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyCharacterName
	{
		protected class VRage_Game_MyCharacterName_003C_003EName_003C_003EAccessor : IMemberAccessor<MyCharacterName, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCharacterName owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCharacterName owner, out string value)
			{
				value = owner.Name;
			}
		}

		private class VRage_Game_MyCharacterName_003C_003EActor : IActivator, IActivator<MyCharacterName>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterName();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterName CreateInstance()
			{
				return new MyCharacterName();
			}

			MyCharacterName IActivator<MyCharacterName>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlAttribute]
		[ProtoMember(1)]
		public string Name;
	}
}
