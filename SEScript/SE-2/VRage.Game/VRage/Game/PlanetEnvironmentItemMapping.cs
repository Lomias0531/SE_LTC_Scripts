using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct PlanetEnvironmentItemMapping
	{
		protected class VRage_Game_PlanetEnvironmentItemMapping_003C_003EMaterials_003C_003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlanetEnvironmentItemMapping owner, in string[] value)
			{
				owner.Materials = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlanetEnvironmentItemMapping owner, out string[] value)
			{
				value = owner.Materials;
			}
		}

		protected class VRage_Game_PlanetEnvironmentItemMapping_003C_003EBiomes_003C_003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, int[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlanetEnvironmentItemMapping owner, in int[] value)
			{
				owner.Biomes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlanetEnvironmentItemMapping owner, out int[] value)
			{
				value = owner.Biomes;
			}
		}

		protected class VRage_Game_PlanetEnvironmentItemMapping_003C_003EItems_003C_003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, MyPlanetEnvironmentItemDef[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlanetEnvironmentItemMapping owner, in MyPlanetEnvironmentItemDef[] value)
			{
				owner.Items = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlanetEnvironmentItemMapping owner, out MyPlanetEnvironmentItemDef[] value)
			{
				value = owner.Items;
			}
		}

		protected class VRage_Game_PlanetEnvironmentItemMapping_003C_003ERule_003C_003EAccessor : IMemberAccessor<PlanetEnvironmentItemMapping, MyPlanetSurfaceRule>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlanetEnvironmentItemMapping owner, in MyPlanetSurfaceRule value)
			{
				owner.Rule = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlanetEnvironmentItemMapping owner, out MyPlanetSurfaceRule value)
			{
				value = owner.Rule;
			}
		}

		private class VRage_Game_PlanetEnvironmentItemMapping_003C_003EActor : IActivator, IActivator<PlanetEnvironmentItemMapping>
		{
			private sealed override object CreateInstance()
			{
				return default(PlanetEnvironmentItemMapping);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override PlanetEnvironmentItemMapping CreateInstance()
			{
				return (PlanetEnvironmentItemMapping)(object)default(PlanetEnvironmentItemMapping);
			}

			PlanetEnvironmentItemMapping IActivator<PlanetEnvironmentItemMapping>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(65)]
		[XmlArrayItem("Material")]
		public string[] Materials;

		[ProtoMember(66)]
		[XmlArrayItem("Biome")]
		public int[] Biomes;

		[ProtoMember(67)]
		[XmlArrayItem("Item")]
		public MyPlanetEnvironmentItemDef[] Items;

		[ProtoMember(68)]
		public MyPlanetSurfaceRule Rule;
	}
}
