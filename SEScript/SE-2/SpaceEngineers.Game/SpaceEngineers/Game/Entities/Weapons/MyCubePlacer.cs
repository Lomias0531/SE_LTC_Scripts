using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Weapons
{
	[MyEntityType(typeof(MyObjectBuilder_CubePlacer), true)]
	public class MyCubePlacer : MyBlockPlacerBase
	{
		private static MyDefinitionId m_handItemDefId = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));

		protected override MyBlockBuilderBase BlockBuilder => MyCubeBuilder.Static;

		public override bool IsSkinnable => false;

		public MyCubePlacer()
			: base(MyDefinitionManager.Static.TryGetHandItemDefinition(ref m_handItemDefId))
		{
		}

		public override void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction)
		{
			if ((MySession.Static.CreativeToolsEnabled(Sync.MyId) && MySession.Static.HasCreativeRights) || MySession.Static.CreativeMode)
			{
				return;
			}
			base.Shoot(action, direction, overrideWeaponPos, gunAction);
			if (action == MyShootActionEnum.PrimaryAction && !m_firstShot && MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastKeyPress >= 500 && GetTargetBlock() != null)
			{
				MyDefinitionId value = new MyDefinitionId(typeof(MyObjectBuilder_Welder));
				if (Owner.CanSwitchToWeapon(value))
				{
					Owner.SetupAutoswitch(new MyDefinitionId(typeof(MyObjectBuilder_Welder)), new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer)));
				}
			}
		}
	}
}
