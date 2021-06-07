using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_CoordinateSystemDefinition), null)]
	public class MyCoordinateSystemDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MyCoordinateSystemDefinition_003C_003EActor : IActivator, IActivator<MyCoordinateSystemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCoordinateSystemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCoordinateSystemDefinition CreateInstance()
			{
				return new MyCoordinateSystemDefinition();
			}

			MyCoordinateSystemDefinition IActivator<MyCoordinateSystemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public double AngleTolerance = 0.0001;

		public double PositionTolerance = 0.001;

		public int CoordSystemSize = 1000;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CoordinateSystemDefinition myObjectBuilder_CoordinateSystemDefinition = builder as MyObjectBuilder_CoordinateSystemDefinition;
			AngleTolerance = myObjectBuilder_CoordinateSystemDefinition.AngleTolerance;
			PositionTolerance = myObjectBuilder_CoordinateSystemDefinition.PositionTolerance;
			CoordSystemSize = myObjectBuilder_CoordinateSystemDefinition.CoordSystemSize;
		}
	}
}
