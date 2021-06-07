using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[MyDefinitionType(typeof(MyObjectBuilder_CurveDefinition), null)]
	public class MyCurveDefinition : MyDefinitionBase
	{
		private class VRage_Game_MyCurveDefinition_003C_003EActor : IActivator, IActivator<MyCurveDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCurveDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCurveDefinition CreateInstance()
			{
				return new MyCurveDefinition();
			}

			MyCurveDefinition IActivator<MyCurveDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Curve Curve;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CurveDefinition obj = builder as MyObjectBuilder_CurveDefinition;
			Curve = new Curve();
			foreach (MyObjectBuilder_CurveDefinition.Point point in obj.Points)
			{
				Curve.Keys.Add(new CurveKey(point.Time, point.Value));
			}
		}
	}
}
