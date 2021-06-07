using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WorldEnvironmentBase), null)]
	public abstract class MyWorldEnvironmentDefinition : MyDefinitionBase
	{
		public int SyncLod;

		public MyRuntimeEnvironmentItemInfo[] Items;

		public double SectorSize;

		public double ItemDensity;

		public abstract Type SectorType
		{
			get;
		}

		public MyEnvironmentSector CreateSector()
		{
			return (MyEnvironmentSector)Activator.CreateInstance(SectorType);
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WorldEnvironmentBase myObjectBuilder_WorldEnvironmentBase = (MyObjectBuilder_WorldEnvironmentBase)builder;
			SectorSize = myObjectBuilder_WorldEnvironmentBase.SectorSize;
			ItemDensity = myObjectBuilder_WorldEnvironmentBase.ItemsPerSqMeter;
			SyncLod = myObjectBuilder_WorldEnvironmentBase.MaxSyncLod;
		}
	}
}
