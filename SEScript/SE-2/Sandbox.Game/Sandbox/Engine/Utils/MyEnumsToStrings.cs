using System;
using System.Collections.Generic;
using VRage.Utils;

namespace Sandbox.Engine.Utils
{
	internal static class MyEnumsToStrings
	{
		public static string[] HudTextures;

		public static string[] Particles;

		public static string[] HudRadarTextures;

		public static string[] Decals;

		public static string[] SessionType;

		static MyEnumsToStrings()
		{
			HudTextures = new string[13]
			{
				"corner.png",
				"crosshair.png",
				"HudOre.png",
				"Target_enemy.png",
				"Target_friend.png",
				"Target_neutral.png",
				"Target_me.png",
				"TargetTurret.png",
				"DirectionIndicator.png",
				"gravity_point_red.png",
				"gravity_point_white.png",
				"gravity_arrow.png",
				"hit_confirmation.png"
			};
			Particles = new string[78]
			{
				"Explosion.dds",
				"ExplosionSmokeDebrisLine.dds",
				"Smoke.dds",
				"Test.dds",
				"EngineThrustMiddle.dds",
				"ReflectorCone.dds",
				"ReflectorGlareAdditive.dds",
				"ReflectorGlareAlphaBlended.dds",
				"MuzzleFlashMachineGunFront.dds",
				"MuzzleFlashMachineGunSide.dds",
				"ProjectileTrailLine.dds",
				"ContainerBorder.dds",
				"Dust.dds",
				"Crosshair.dds",
				"Sun.dds",
				"LightRay.dds",
				"LightGlare.dds",
				"SolarMapOrbitLine.dds",
				"SolarMapSun.dds",
				"SolarMapAsteroidField.dds",
				"SolarMapFactionMap.dds",
				"SolarMapAsteroid.dds",
				"SolarMapZeroPlaneLine.dds",
				"SolarMapSmallShip.dds",
				"SolarMapLargeShip.dds",
				"SolarMapOutpost.dds",
				"Grid.dds",
				"ContainerBorderSelected.dds",
				"FactionRussia.dds",
				"FactionChina.dds",
				"FactionJapan.dds",
				"FactionUnitedKorea.dds",
				"FactionFreeAsia.dds",
				"FactionSaudi.dds",
				"FactionEAC.dds",
				"FactionCSR.dds",
				"FactionIndia.dds",
				"FactionChurch.dds",
				"FactionOmnicorp.dds",
				"FactionFourthReich.dds",
				"FactionSlavers.dds",
				"Smoke_b.dds",
				"Smoke_c.dds",
				"Sparks_a.dds",
				"Sparks_b.dds",
				"particle_stone.dds",
				"Stardust.dds",
				"particle_trash_a.dds",
				"particle_trash_b.dds",
				"particle_glare.dds",
				"smoke_field.dds",
				"Explosion_pieces.dds",
				"particle_laser.dds",
				"particle_nuclear.dds",
				"Explosion_line.dds",
				"particle_flash_a.dds",
				"particle_flash_b.dds",
				"particle_flash_c.dds",
				"snap_point.dds",
				"SolarMapNavigationMark.dds",
				"Impostor_StaticAsteroid20m_A.dds",
				"Impostor_StaticAsteroid20m_C.dds",
				"Impostor_StaticAsteroid50m_D.dds",
				"Impostor_StaticAsteroid50m_E.dds",
				"GPS.dds",
				"GPSBack.dds",
				"ShotgunParticle.dds",
				"ObjectiveDummyFace.dds",
				"ObjectiveDummyLine.dds",
				"SunDisk.dds",
				"scanner_01.dds",
				"Smoke_square.dds",
				"Smoke_lit.dds",
				"SolarMapSideMission.dds",
				"SolarMapStoryMission.dds",
				"SolarMapTemplateMission.dds",
				"SolarMapPlayer.dds",
				"ReflectorConeCharacter.dds"
			};
			HudRadarTextures = new string[26]
			{
				"Arrow.png",
				"ImportantObject.tga",
				"LargeShip.tga",
				"Line.tga",
				"RadarBackground.tga",
				"RadarPlane.tga",
				"SectorBorder.tga",
				"SmallShip.tga",
				"Sphere.png",
				"SphereGrid.tga",
				"Sun.tga",
				"OreDeposit_Treasure.png",
				"OreDeposit_Helium.png",
				"OreDeposit_Ice.png",
				"OreDeposit_Iron.png",
				"OreDeposit_Lava.png",
				"OreDeposit_Gold.png",
				"OreDeposit_Platinum.png",
				"OreDeposit_Silver.png",
				"OreDeposit_Silicon.png",
				"OreDeposit_Organic.png",
				"OreDeposit_Nickel.png",
				"OreDeposit_Magnesium.png",
				"OreDeposit_Uranite.png",
				"OreDeposit_Cobalt.png",
				"OreDeposit_Snow.png"
			};
			Decals = new string[3]
			{
				"ExplosionSmut",
				"BulletHoleOnMetal",
				"BulletHoleOnRock"
			};
			SessionType = new string[11]
			{
				"NEW_STORY",
				"LOAD_CHECKPOINT",
				"JOIN_FRIEND_STORY",
				"MMO",
				"SANDBOX_OWN",
				"SANDBOX_FRIENDS",
				"JOIN_SANDBOX_FRIEND",
				"EDITOR_SANDBOX",
				"EDITOR_STORY",
				"EDITOR_MMO",
				"SANDBOX_RANDOM"
			};
		}

		private static void Validate<T>(Type type, T list) where T : IList<string>
		{
			Array values = Enum.GetValues(type);
			Type underlyingType = Enum.GetUnderlyingType(type);
			if (underlyingType == typeof(byte))
			{
				foreach (byte item in values)
				{
					MyDebug.AssertRelease(list[item] != null);
				}
				return;
			}
			if (underlyingType == typeof(short))
			{
				foreach (short item2 in values)
				{
					MyDebug.AssertRelease(list[item2] != null);
				}
				return;
			}
			if (underlyingType == typeof(ushort))
			{
				foreach (ushort item3 in values)
				{
					MyDebug.AssertRelease(list[item3] != null);
				}
				return;
			}
			if (underlyingType == typeof(int))
			{
				foreach (int item4 in values)
				{
					MyDebug.AssertRelease(list[item4] != null);
				}
				return;
			}
			throw new InvalidBranchException();
		}
	}
}
