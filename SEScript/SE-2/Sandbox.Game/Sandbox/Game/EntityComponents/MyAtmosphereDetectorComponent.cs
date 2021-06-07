using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentBuilder(typeof(MyObjectBuilder_AtmosphereDetectorComponent), true)]
	public class MyAtmosphereDetectorComponent : MyEntityComponentBase
	{
		private enum AtmosphereStatus
		{
			NotSet,
			Space,
			ShipOrStation,
			Atmosphere
		}

		private class Sandbox_Game_EntityComponents_MyAtmosphereDetectorComponent_003C_003EActor : IActivator, IActivator<MyAtmosphereDetectorComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyAtmosphereDetectorComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAtmosphereDetectorComponent CreateInstance()
			{
				return new MyAtmosphereDetectorComponent();
			}

			MyAtmosphereDetectorComponent IActivator<MyAtmosphereDetectorComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyCharacter m_character;

		private bool m_localPlayer = true;

		private bool m_inAtmosphere;

		private AtmosphereStatus m_atmosphereStatus;

		public bool InAtmosphere => m_atmosphereStatus == AtmosphereStatus.Atmosphere;

		public bool InShipOrStation => m_atmosphereStatus == AtmosphereStatus.ShipOrStation;

		public bool InVoid => m_atmosphereStatus == AtmosphereStatus.Space;

		public override string ComponentTypeDebugString => "AtmosphereDetector";

		public void InitComponent(bool onlyLocalPlayer, MyCharacter character)
		{
			m_localPlayer = onlyLocalPlayer;
			m_character = character;
		}

		public void UpdateAtmosphereStatus()
		{
			if (m_character == null || (m_localPlayer && (MySession.Static == null || m_character != MySession.Static.LocalCharacter)))
			{
				return;
			}
			AtmosphereStatus atmosphereStatus = m_atmosphereStatus;
			Vector3D position = m_character.PositionComp.GetPosition();
			if (MyGravityProviderSystem.CalculateNaturalGravityInPoint(position).LengthSquared() > 0f)
			{
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
				if (closestPlanet != null && closestPlanet.HasAtmosphere && closestPlanet.GetAirDensity(position) > 0.5f)
				{
					m_atmosphereStatus = AtmosphereStatus.Atmosphere;
				}
				else
				{
					m_atmosphereStatus = AtmosphereStatus.Space;
				}
			}
			else
			{
				m_atmosphereStatus = AtmosphereStatus.Space;
			}
			if (m_atmosphereStatus == AtmosphereStatus.Space)
			{
				float num = 0f;
				if (m_character.OxygenComponent != null)
				{
					num = ((!m_localPlayer) ? m_character.EnvironmentOxygenLevel : ((!(MySession.Static.ControlledEntity is MyCharacter)) ? ((float)m_character.OxygenLevelAtCharacterLocation) : m_character.EnvironmentOxygenLevel));
				}
				if (num > 0.1f)
				{
					m_atmosphereStatus = AtmosphereStatus.ShipOrStation;
				}
			}
			if (MyFakes.ENABLE_REALISTIC_LIMITER && MyFakes.ENABLE_NEW_SOUNDS && atmosphereStatus != m_atmosphereStatus && MySession.Static != null && MySession.Static.Settings.RealisticSound)
			{
				MyAudio.Static.EnableMasterLimiter(!InAtmosphere && !InShipOrStation);
			}
		}
	}
}
