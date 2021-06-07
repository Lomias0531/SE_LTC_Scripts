using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyPlanetMaterialDefinition : ICloneable
	{
		protected class VRage_Game_MyPlanetMaterialDefinition_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialDefinition owner, in string value)
			{
				owner.Material = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialDefinition owner, out string value)
			{
				value = owner.Material;
			}
		}

		protected class VRage_Game_MyPlanetMaterialDefinition_003C_003EValue_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialDefinition owner, in byte value)
			{
				owner.Value = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialDefinition owner, out byte value)
			{
				value = owner.Value;
			}
		}

		protected class VRage_Game_MyPlanetMaterialDefinition_003C_003EMaxDepth_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialDefinition owner, in float value)
			{
				owner.MaxDepth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialDefinition owner, out float value)
			{
				value = owner.MaxDepth;
			}
		}

		protected class VRage_Game_MyPlanetMaterialDefinition_003C_003ELayers_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialDefinition, MyPlanetMaterialLayer[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialDefinition owner, in MyPlanetMaterialLayer[] value)
			{
				owner.Layers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialDefinition owner, out MyPlanetMaterialLayer[] value)
			{
				value = owner.Layers;
			}
		}

		private class VRage_Game_MyPlanetMaterialDefinition_003C_003EActor : IActivator, IActivator<MyPlanetMaterialDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyPlanetMaterialDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetMaterialDefinition CreateInstance()
			{
				return new MyPlanetMaterialDefinition();
			}

			MyPlanetMaterialDefinition IActivator<MyPlanetMaterialDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(3)]
		[XmlAttribute(AttributeName = "Material")]
		public string Material;

		[ProtoMember(4)]
		[XmlAttribute(AttributeName = "Value")]
		public byte Value;

		[ProtoMember(5)]
		[XmlAttribute(AttributeName = "MaxDepth")]
		public float MaxDepth = 1f;

		[ProtoMember(6)]
		[XmlArrayItem("Layer")]
		public MyPlanetMaterialLayer[] Layers;

		public virtual bool IsRule => false;

		/// Weather this material has layers.
		public bool HasLayers
		{
			get
			{
				if (Layers != null)
				{
					return Layers.Length != 0;
				}
				return false;
			}
		}

		public string FirstOrDefault
		{
			get
			{
				if (Material != null)
				{
					return Material;
				}
				if (HasLayers)
				{
					return Layers[0].Material;
				}
				return null;
			}
		}

		public object Clone()
		{
			MyPlanetMaterialDefinition myPlanetMaterialDefinition = new MyPlanetMaterialDefinition();
			myPlanetMaterialDefinition.Material = Material;
			myPlanetMaterialDefinition.Value = Value;
			myPlanetMaterialDefinition.MaxDepth = MaxDepth;
			if (Layers != null)
			{
				myPlanetMaterialDefinition.Layers = (Layers.Clone() as MyPlanetMaterialLayer[]);
			}
			else
			{
				myPlanetMaterialDefinition.Layers = null;
			}
			return myPlanetMaterialDefinition;
		}
	}
}
