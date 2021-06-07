using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	/// Rule group defines a material mappable set of surface rules.
	[XmlType("MaterialGroup")]
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyPlanetMaterialGroup : ICloneable
	{
		protected class VRage_Game_MyPlanetMaterialGroup_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialGroup, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialGroup owner, in byte value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialGroup owner, out byte value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_MyPlanetMaterialGroup_003C_003EName_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialGroup, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialGroup owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialGroup owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyPlanetMaterialGroup_003C_003EMaterialRules_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialGroup, MyPlanetMaterialPlacementRule[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialGroup owner, in MyPlanetMaterialPlacementRule[] value)
			{
				owner.MaterialRules = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialGroup owner, out MyPlanetMaterialPlacementRule[] value)
			{
				value = owner.MaterialRules;
			}
		}

		private class VRage_Game_MyPlanetMaterialGroup_003C_003EActor : IActivator, IActivator<MyPlanetMaterialGroup>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetMaterialGroup();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetMaterialGroup CreateInstance()
			{
				return new MyPlanetMaterialGroup();
			}

			MyPlanetMaterialGroup IActivator<MyPlanetMaterialGroup>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(15)]
		[XmlAttribute(AttributeName = "Value")]
		public byte Value;

		[ProtoMember(16)]
		[XmlAttribute(AttributeName = "Name")]
		public string Name = "Default";

		[ProtoMember(17)]
		[XmlElement("Rule")]
		public MyPlanetMaterialPlacementRule[] MaterialRules;

		public object Clone()
		{
			MyPlanetMaterialGroup myPlanetMaterialGroup = new MyPlanetMaterialGroup();
			myPlanetMaterialGroup.Value = Value;
			myPlanetMaterialGroup.Name = Name;
			myPlanetMaterialGroup.MaterialRules = new MyPlanetMaterialPlacementRule[MaterialRules.Length];
			for (int i = 0; i < MaterialRules.Length; i++)
			{
				myPlanetMaterialGroup.MaterialRules[i] = (MaterialRules[i].Clone() as MyPlanetMaterialPlacementRule);
			}
			return myPlanetMaterialGroup;
		}
	}
}
