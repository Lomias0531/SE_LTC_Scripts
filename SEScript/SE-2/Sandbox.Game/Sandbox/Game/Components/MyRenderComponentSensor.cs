using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentSensor : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentSensor_003C_003EActor : IActivator, IActivator<MyRenderComponentSensor>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentSensor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentSensor CreateInstance()
			{
				return new MyRenderComponentSensor();
			}

			MyRenderComponentSensor IActivator<MyRenderComponentSensor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MySensorBase m_sensor;

		private float m_lastHighlight;

		protected Vector4 m_color;

		private bool DrawSensor = true;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_sensor = (base.Container.Entity as MySensorBase);
		}

		public override void Draw()
		{
			if (DrawSensor)
			{
				SetHighlight();
				MatrixD worldMatrix = base.Container.Entity.PositionComp.WorldMatrix;
				if (MySession.Static.ControlledEntity == this)
				{
					Vector4 color = Color.Red.ToVector4();
					MySimpleObjectDraw.DrawLine(worldMatrix.Translation, worldMatrix.Translation + worldMatrix.Forward * base.Container.Entity.PositionComp.LocalVolume.Radius * 1.2000000476837158, null, ref color, 0.05f);
				}
			}
		}

		protected void SetHighlight()
		{
			SetHighlight(new Vector4(0f, 0f, 0f, 0.3f));
			if (m_sensor.AnyEntityWithState(MySensorBase.EventType.Add))
			{
				SetHighlight(new Vector4(1f, 0f, 0f, 0.3f), keepForMinimalTime: true);
			}
			else if (m_sensor.AnyEntityWithState(MySensorBase.EventType.Delete))
			{
				SetHighlight(new Vector4(1f, 0f, 1f, 0.3f), keepForMinimalTime: true);
			}
			else if (m_sensor.HasAnyMoved())
			{
				SetHighlight(new Vector4(0f, 0f, 1f, 0.3f));
			}
			else if (m_sensor.AnyEntityWithState(MySensorBase.EventType.None))
			{
				SetHighlight(new Vector4(0.4f, 0.4f, 0.4f, 0.3f));
			}
		}

		private void SetHighlight(Vector4 color, bool keepForMinimalTime = false)
		{
			if ((float)MySandboxGame.TotalGamePlayTimeInMilliseconds > m_lastHighlight + 300f)
			{
				m_color = color;
				if (keepForMinimalTime)
				{
					m_lastHighlight = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
			}
		}
	}
}
