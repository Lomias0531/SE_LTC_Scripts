using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetAnimal
	{
		protected class VRage_Game_MyPlanetAnimal_003C_003EAnimalType_003C_003EAccessor : IMemberAccessor<MyPlanetAnimal, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetAnimal owner, in string value)
			{
				owner.AnimalType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetAnimal owner, out string value)
			{
				value = owner.AnimalType;
			}
		}

		private class VRage_Game_MyPlanetAnimal_003C_003EActor : IActivator, IActivator<MyPlanetAnimal>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetAnimal();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetAnimal CreateInstance()
			{
				return new MyPlanetAnimal();
			}

			MyPlanetAnimal IActivator<MyPlanetAnimal>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(25)]
		[XmlAttribute(AttributeName = "Type")]
		public string AnimalType;
	}
}
