using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.World;
using VRage.Library.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatPlayerGasRefillingBase : MyStatBase
	{
		private static readonly MyGameTimer TIMER = new MyGameTimer();

		private static readonly double VISIBLE_TIME_MS = 2000.0;

		private float m_lastGasLevel;

		private double m_lastTimeChecked;

		public override void Update()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.OxygenComponent != null)
			{
				double totalMilliseconds = TIMER.ElapsedTimeSpan.TotalMilliseconds;
				if (totalMilliseconds - m_lastTimeChecked > VISIBLE_TIME_MS)
				{
					float gassLevel = GetGassLevel(localCharacter.OxygenComponent);
					base.CurrentValue = ((gassLevel > m_lastGasLevel) ? 1 : 0);
					m_lastTimeChecked = totalMilliseconds;
					m_lastGasLevel = gassLevel;
				}
			}
		}

		protected virtual float GetGassLevel(MyCharacterOxygenComponent oxygenComp)
		{
			return 0f;
		}
	}
}
