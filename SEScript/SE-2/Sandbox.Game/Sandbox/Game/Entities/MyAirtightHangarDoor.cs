using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_AirtightHangarDoor))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyAirtightHangarDoor),
		typeof(Sandbox.ModAPI.Ingame.IMyAirtightHangarDoor)
	})]
	public class MyAirtightHangarDoor : MyAirtightDoorGeneric, Sandbox.ModAPI.IMyAirtightHangarDoor, Sandbox.ModAPI.IMyAirtightDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAirtightDoorBase, Sandbox.ModAPI.Ingame.IMyAirtightHangarDoor
	{
		private class Sandbox_Game_Entities_MyAirtightHangarDoor_003C_003EActor : IActivator, IActivator<MyAirtightHangarDoor>
		{
			private sealed override object CreateInstance()
			{
				return new MyAirtightHangarDoor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAirtightHangarDoor CreateInstance()
			{
				return new MyAirtightHangarDoor();
			}

			MyAirtightHangarDoor IActivator<MyAirtightHangarDoor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void UpdateDoorPosition()
		{
			if (base.CubeGrid.Physics != null)
			{
				bool flag = !Sync.IsServer;
				if (m_subpartConstraints.Count != 0 || flag)
				{
					float num = (m_currOpening - 1f) * (float)m_subparts.Count * m_subpartMovementDistance;
					float num2 = 0f;
					int num3 = 0;
					foreach (MyEntitySubpart subpart in m_subparts)
					{
						num2 -= m_subpartMovementDistance;
						if (subpart != null && subpart.Physics != null)
						{
							float num4 = (num < num2) ? num2 : num;
							Vector3 position = new Vector3(0f, num4, 0f);
							Matrix.CreateTranslation(ref position, out Matrix result);
							Matrix renderLocal = result * base.PositionComp.LocalMatrix;
							subpart.PositionComp.SetLocalMatrix(ref result, flag ? null : subpart.Physics, updateWorld: true, ref renderLocal, forceUpdateRender: true);
							if (m_subpartConstraintsData.Count > 0)
							{
								if (base.CubeGrid.Physics != null)
								{
									base.CubeGrid.Physics.RigidBody.Activate();
								}
								subpart.Physics.RigidBody.Activate();
								position = new Vector3(0f, 0f - num4, 0f);
								Matrix.CreateTranslation(ref position, out result);
								m_subpartConstraintsData[num3].SetInBodySpace(base.PositionComp.LocalMatrix, result, base.CubeGrid.Physics, (MyPhysicsBody)subpart.Physics);
							}
							num3++;
						}
					}
				}
			}
		}

		protected override void FillSubparts()
		{
			m_subparts.Clear();
			MyEntitySubpart value;
			for (int i = 1; base.Subparts.TryGetValue("HangarDoor_door" + i, out value); i++)
			{
				m_subparts.Add(value);
			}
		}
	}
}
