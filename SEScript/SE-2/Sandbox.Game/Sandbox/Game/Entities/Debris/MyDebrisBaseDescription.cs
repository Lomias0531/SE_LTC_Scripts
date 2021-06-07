using System;

namespace Sandbox.Game.Entities.Debris
{
	public class MyDebrisBaseDescription
	{
		public string Model;

		public int LifespanMinInMiliseconds;

		public int LifespanMaxInMiliseconds;

		public Action<MyDebrisBase> OnCloseAction;
	}
}
