using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyPlanetMaterialBlendSettings
	{
		protected class VRage_Game_MyPlanetMaterialBlendSettings_003C_003ETexture_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialBlendSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialBlendSettings owner, in string value)
			{
				owner.Texture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialBlendSettings owner, out string value)
			{
				value = owner.Texture;
			}
		}

		protected class VRage_Game_MyPlanetMaterialBlendSettings_003C_003ECellSize_003C_003EAccessor : IMemberAccessor<MyPlanetMaterialBlendSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyPlanetMaterialBlendSettings owner, in int value)
			{
				owner.CellSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyPlanetMaterialBlendSettings owner, out int value)
			{
				value = owner.CellSize;
			}
		}

		private class VRage_Game_MyPlanetMaterialBlendSettings_003C_003EActor : IActivator, IActivator<MyPlanetMaterialBlendSettings>
		{
			private sealed override object CreateInstance()
			{
				return default(MyPlanetMaterialBlendSettings);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPlanetMaterialBlendSettings CreateInstance()
			{
				return (MyPlanetMaterialBlendSettings)(object)default(MyPlanetMaterialBlendSettings);
			}

			MyPlanetMaterialBlendSettings IActivator<MyPlanetMaterialBlendSettings>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(72)]
		public string Texture;

		[ProtoMember(73)]
		public int CellSize;
	}
}
