using ObjectBuilders.Definitions.SafeZone;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;

namespace SpaceEngineers.Game.Definitions.SafeZone
{
	[MyDefinitionType(typeof(MyObjectBuilder_SafeZoneBlockDefinition), null)]
	public class MySafeZoneBlockDefinition : MyCubeBlockDefinition
	{
		public string ResourceSinkGroup;

		public float MaxSafeZoneRadius;

		public float MinSafeZoneRadius;

		public float DefaultSafeZoneRadius;

		public float MaxSafeZonePowerDrainkW;

		public float MinSafeZonePowerDrainkW;

		public uint SafeZoneActivationTimeS;

		public uint SafeZoneUpkeep;

		public uint SafeZoneUpkeepTimeM;

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SafeZoneBlockDefinition myObjectBuilder_SafeZoneBlockDefinition = (MyObjectBuilder_SafeZoneBlockDefinition)builder;
			ResourceSinkGroup = myObjectBuilder_SafeZoneBlockDefinition.ResourceSinkGroup;
			MaxSafeZoneRadius = myObjectBuilder_SafeZoneBlockDefinition.MaxSafeZoneRadius;
			MinSafeZoneRadius = myObjectBuilder_SafeZoneBlockDefinition.MinSafeZoneRadius;
			DefaultSafeZoneRadius = myObjectBuilder_SafeZoneBlockDefinition.DefaultSafeZoneRadius;
			MaxSafeZonePowerDrainkW = myObjectBuilder_SafeZoneBlockDefinition.MaxSafeZonePowerDrainkW;
			MinSafeZonePowerDrainkW = myObjectBuilder_SafeZoneBlockDefinition.MinSafeZonePowerDrainkW;
			SafeZoneActivationTimeS = myObjectBuilder_SafeZoneBlockDefinition.SafeZoneActivationTimeS;
			SafeZoneUpkeep = myObjectBuilder_SafeZoneBlockDefinition.SafeZoneUpkeep;
			SafeZoneUpkeepTimeM = myObjectBuilder_SafeZoneBlockDefinition.SafeZoneUpkeepTimeM;
			ScreenAreas = ((myObjectBuilder_SafeZoneBlockDefinition.ScreenAreas != null) ? myObjectBuilder_SafeZoneBlockDefinition.ScreenAreas.ToList() : null);
		}
	}
}
