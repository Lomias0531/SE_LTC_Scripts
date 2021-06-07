using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyPlanetMaterialLayer
	{
		protected class VRage_Game_MyPlanetMaterialLayer_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialLayer, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialLayer owner, in string value)
			{
				owner.Material = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialLayer owner, out string value)
			{
				value = owner.Material;
			}
		}

		protected class VRage_Game_MyPlanetMaterialLayer_003C_003EDepth_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialLayer, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialLayer owner, in float value)
			{
				owner.Depth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialLayer owner, out float value)
			{
				value = owner.Depth;
			}
		}

		private class VRage_Game_MyPlanetMaterialLayer_003C_003EActor : IActivator, IActivator<MyPlanetMaterialLayer>
		{
			private sealed override object CreateInstance()
			{
				return default(MyPlanetMaterialLayer);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetMaterialLayer CreateInstance()
			{
				return (MyPlanetMaterialLayer)(object)default(MyPlanetMaterialLayer);
			}

			MyPlanetMaterialLayer IActivator<MyPlanetMaterialLayer>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute(AttributeName = "Material")]
		public string Material;

		[ProtoMember(2)]
		[XmlAttribute(AttributeName = "Depth")]
		public float Depth;
	}
}
