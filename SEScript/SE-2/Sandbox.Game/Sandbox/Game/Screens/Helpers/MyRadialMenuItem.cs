using VRage;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	public abstract class MyRadialMenuItem
	{
		public string Icon;

		public virtual string Label
		{
			get;
			set;
		}

		public virtual bool CanBeActivated => Enabled();

		public virtual void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			Icon = builder.Icon;
			Label = MyTexts.GetString(builder.Label);
		}

		public virtual bool Enabled()
		{
			return true;
		}

		public abstract void Activate(params object[] parameters);
	}
}
