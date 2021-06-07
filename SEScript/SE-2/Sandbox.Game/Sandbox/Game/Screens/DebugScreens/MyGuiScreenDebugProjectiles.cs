using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Projectiles")]
	internal class MyGuiScreenDebugProjectiles : MyGuiScreenDebugBase
	{
		private static MyGuiScreenDebugProjectiles m_instance;

		public MyGuiScreenDebugProjectiles()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugProjectiles";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_instance = this;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddSlider("Impact snd timeout", MyProjectile.CollisionSoundsTimedCache.EventTimeoutMs, 0f, 1000f, delegate(MyGuiControlSlider x)
			{
				MyProjectile.CollisionSoundsTimedCache.EventTimeoutMs = (int)x.Value;
			});
			AddSlider("Impact part. timeout", MyProjectile.CollisionParticlesTimedCache.EventTimeoutMs, 0f, 1000f, delegate(MyGuiControlSlider x)
			{
				MyProjectile.CollisionParticlesTimedCache.EventTimeoutMs = (int)x.Value;
			});
			AddSlider("Impact part. cube size", 1f / (float)MyProjectile.CollisionParticlesSpaceMapping, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyProjectile.CollisionParticlesSpaceMapping = 1f / x.Value;
			});
		}
	}
}
