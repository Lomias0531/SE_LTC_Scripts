using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.GameSystems
{
	public class MyDroneAIData
	{
		public string Name = "";

		public float Height = 10f;

		public float Depth = 5f;

		public float Width = 10f;

		public bool AvoidCollisions = true;

		public float SpeedLimit = 25f;

		public bool RotateToPlayer = true;

		public float PlayerYAxisOffset = 0.9f;

		public int WaypointDelayMsMin = 1000;

		public int WaypointDelayMsMax = 3000;

		public float WaypointThresholdDistance = 0.5f;

		public float PlayerTargetDistance = 200f;

		public float MaxManeuverDistance = 250f;

		public float MaxManeuverDistanceSq = 62500f;

		public int WaypointMaxTime = 15000;

		public int LostTimeMs = 20000;

		public float MinStrafeDistance = 2f;

		public float MinStrafeDistanceSq = 4f;

		public bool UseStaticWeaponry = true;

		public float StaticWeaponryUsage = 300f;

		public float StaticWeaponryUsageSq = 90000f;

		public bool UseTools = true;

		public float ToolsUsage = 5f;

		public float ToolsUsageSq = 25f;

		public bool UseKamikazeBehavior = true;

		public bool CanBeDisabled = true;

		public float KamikazeBehaviorDistance = 75f;

		public string AlternativeBehavior = "";

		public bool UsePlanetHover;

		public float PlanetHoverMin = 2f;

		public float PlanetHoverMax = 25f;

		public float RotationLimitSq;

		public bool UsesWeaponBehaviors;

		public float WeaponBehaviorNotFoundDelay = 3f;

		public List<MyWeaponBehavior> WeaponBehaviors;

		public string SoundLoop = "";

		public MyDroneAIData()
		{
			PostProcess();
		}

		public MyDroneAIData(MyObjectBuilder_DroneBehaviorDefinition definition)
		{
			Name = definition.Id.SubtypeId;
			Height = definition.StrafeHeight;
			Depth = definition.StrafeDepth;
			Width = definition.StrafeWidth;
			AvoidCollisions = definition.AvoidCollisions;
			SpeedLimit = definition.SpeedLimit;
			RotateToPlayer = definition.RotateToPlayer;
			PlayerYAxisOffset = definition.PlayerYAxisOffset;
			WaypointDelayMsMin = definition.WaypointDelayMsMin;
			WaypointDelayMsMax = definition.WaypointDelayMsMax;
			WaypointThresholdDistance = definition.WaypointThresholdDistance;
			PlayerTargetDistance = definition.TargetDistance;
			MaxManeuverDistance = definition.MaxManeuverDistance;
			WaypointMaxTime = definition.WaypointMaxTime;
			LostTimeMs = definition.LostTimeMs;
			MinStrafeDistance = definition.MinStrafeDistance;
			UseStaticWeaponry = definition.UseStaticWeaponry;
			StaticWeaponryUsage = definition.StaticWeaponryUsage;
			UseKamikazeBehavior = definition.UseRammingBehavior;
			KamikazeBehaviorDistance = definition.RammingBehaviorDistance;
			AlternativeBehavior = definition.AlternativeBehavior;
			UseTools = definition.UseTools;
			ToolsUsage = definition.ToolsUsage;
			UsePlanetHover = definition.UsePlanetHover;
			PlanetHoverMin = definition.PlanetHoverMin;
			PlanetHoverMax = definition.PlanetHoverMax;
			UsesWeaponBehaviors = (definition.UsesWeaponBehaviors && definition.WeaponBehaviors.Count > 0 && UseStaticWeaponry);
			WeaponBehaviorNotFoundDelay = definition.WeaponBehaviorNotFoundDelay;
			WeaponBehaviors = definition.WeaponBehaviors;
			SoundLoop = definition.SoundLoop;
			PostProcess();
		}

		private void PostProcess()
		{
			MaxManeuverDistanceSq = MaxManeuverDistance * MaxManeuverDistance;
			MinStrafeDistanceSq = MinStrafeDistance * MinStrafeDistance;
			ToolsUsageSq = ToolsUsage * ToolsUsage;
			StaticWeaponryUsageSq = StaticWeaponryUsage * StaticWeaponryUsage;
			RotationLimitSq = Math.Max(ToolsUsageSq, Math.Max(StaticWeaponryUsageSq, MaxManeuverDistanceSq));
			if (WeaponBehaviors != null)
			{
				foreach (MyWeaponBehavior weaponBehavior in WeaponBehaviors)
				{
					for (int i = 0; i < weaponBehavior.Requirements.Count; i++)
					{
						if (!weaponBehavior.Requirements[i].Contains("MyObjectBuilder_"))
						{
							weaponBehavior.Requirements[i] = "MyObjectBuilder_" + weaponBehavior.Requirements[i];
						}
					}
					foreach (MyWeaponRule weaponRule in weaponBehavior.WeaponRules)
					{
						if (!string.IsNullOrEmpty(weaponRule.Weapon) && !weaponRule.Weapon.Contains("MyObjectBuilder_"))
						{
							weaponRule.Weapon = "MyObjectBuilder_" + weaponRule.Weapon;
						}
					}
				}
			}
		}
	}
}
