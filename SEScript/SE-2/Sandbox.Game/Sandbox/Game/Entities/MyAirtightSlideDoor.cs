using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_AirtightSlideDoor))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyAirtightSlideDoor),
		typeof(Sandbox.ModAPI.Ingame.IMyAirtightSlideDoor)
	})]
	public class MyAirtightSlideDoor : MyAirtightDoorGeneric, Sandbox.ModAPI.IMyAirtightSlideDoor, Sandbox.ModAPI.IMyAirtightDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAirtightDoorBase, Sandbox.ModAPI.Ingame.IMyAirtightSlideDoor
	{
		private class Sandbox_Game_Entities_MyAirtightSlideDoor_003C_003EActor : IActivator, IActivator<MyAirtightSlideDoor>
		{
			private sealed override object CreateInstance()
			{
				return new MyAirtightSlideDoor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAirtightSlideDoor CreateInstance()
			{
				return new MyAirtightSlideDoor();
			}

			MyAirtightSlideDoor IActivator<MyAirtightSlideDoor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private bool hadEnoughPower;

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			MyAirtightDoorGeneric.m_emissiveTextureNames = new string[1]
			{
				"Emissive"
			};
			base.Init(builder, cubeGrid);
		}

		protected override void UpdateDoorPosition()
		{
			if (m_subparts.Count != 0)
			{
				float num = (float)Math.Sqrt(1.1375000476837158);
				float num2 = m_currOpening * 1.75f;
				float num3 = m_currOpening * (MathF.E * 449f / 777f);
				if (num2 < num)
				{
					num3 = (float)Math.Asin(num2 / 1.2f);
				}
				else
				{
					float num4 = (1.75f - num2) / (1.75f - num);
					num3 = MathF.E * 449f / 777f - num4 * num4 * (float)(1.570796012878418 - Math.Asin(num / 1.2f));
				}
				num2 -= 1f;
				MyGridPhysics physics = base.CubeGrid.Physics;
				bool flag = !Sync.IsServer;
				int num5 = 0;
				bool flag2 = true;
				foreach (MyEntitySubpart subpart in m_subparts)
				{
					if (subpart != null)
					{
						Matrix.CreateRotationY(flag2 ? num3 : (0f - num3), out Matrix result);
						result.Translation = new Vector3(flag2 ? (-1.2f) : 1.2f, 0f, num2);
						Matrix renderLocal = result * base.PositionComp.LocalMatrix;
						MyPhysicsComponentBase physics2 = subpart.Physics;
						if (flag && physics2 != null)
						{
							Matrix matrix = Matrix.Identity;
							matrix.Translation = new Vector3(flag2 ? (-0.549999952f) : 0.549999952f, 0f, 0f);
							Matrix.Multiply(ref matrix, ref result, out matrix);
							subpart.PositionComp.SetLocalMatrix(ref matrix, null, updateWorld: true);
						}
						subpart.PositionComp.SetLocalMatrix(ref result, physics2, updateWorld: true, ref renderLocal, forceUpdateRender: true);
						if (physics != null && physics2 != null && m_subpartConstraintsData.Count > num5)
						{
							physics.RigidBody.Activate();
							physics2.RigidBody.Activate();
							result = Matrix.Invert(result);
							m_subpartConstraintsData[num5].SetInBodySpace(base.PositionComp.LocalMatrix, result, physics, (MyPhysicsBody)physics2);
						}
					}
					flag2 = !flag2;
					num5++;
				}
			}
		}

		protected override void FillSubparts()
		{
			m_subparts.Clear();
			if (base.Subparts.TryGetValue("DoorLeft", out MyEntitySubpart value))
			{
				m_subparts.Add(value);
			}
			if (base.Subparts.TryGetValue("DoorRight", out value))
			{
				m_subparts.Add(value);
			}
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		protected override void UpdateEmissivity(bool force)
		{
			Color color = Color.Red;
			float emissivity = 1f;
			MyEmissiveColorStateResult result;
			if (base.IsWorking)
			{
				color = Color.Green;
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
				{
					color = result.EmissiveColor;
				}
			}
			else if (base.IsFunctional)
			{
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					color = result.EmissiveColor;
				}
				if (!IsEnoughPower())
				{
					emissivity = 0f;
				}
			}
			else
			{
				if (MyEmissiveColorPresets.LoadPresetState(base.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
				{
					color = result.EmissiveColor;
				}
				emissivity = 0f;
			}
			SetEmissive(color, emissivity, force);
		}
	}
}
