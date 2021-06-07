using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyMusicCategory
	{
		protected class VRage_Game_MyMusicCategory_003C_003ECategory_003C_003EAccessor : IMemberAccessor<MyMusicCategory, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyMusicCategory owner, in string value)
			{
				owner.Category = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyMusicCategory owner, out string value)
			{
				value = owner.Category;
			}
		}

		protected class VRage_Game_MyMusicCategory_003C_003EFrequency_003C_003EAccessor : IMemberAccessor<MyMusicCategory, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyMusicCategory owner, in float value)
			{
				owner.Frequency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyMusicCategory owner, out float value)
			{
				value = owner.Frequency;
			}
		}

		private class VRage_Game_MyMusicCategory_003C_003EActor : IActivator, IActivator<MyMusicCategory>
		{
			private sealed override object CreateInstance()
			{
				return new MyMusicCategory();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMusicCategory CreateInstance()
			{
				return new MyMusicCategory();
			}

			MyMusicCategory IActivator<MyMusicCategory>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(51)]
		[XmlAttribute(AttributeName = "Category")]
		public string Category;

		[ProtoMember(52)]
		[XmlAttribute(AttributeName = "Frequency")]
		public float Frequency = 1f;
	}
}
