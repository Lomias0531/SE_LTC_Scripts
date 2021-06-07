using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
	public class MyCharacterAvoidance : MySteeringBase
	{
		private Vector3D m_debugDirection = Vector3D.Forward;

		public MyCharacterAvoidance(MyBotNavigation botNavigation, float weight)
			: base(botNavigation, weight)
		{
		}

		public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
		{
			if (!(base.Parent.Speed < 0.01f))
			{
				MyCharacter myCharacter = base.Parent.BotEntity as MyCharacter;
				if (myCharacter != null)
				{
					Vector3D translation = base.Parent.PositionAndOrientation.Translation;
					BoundingBoxD boundingBox = new BoundingBoxD(translation - Vector3D.One * 3.0, translation + Vector3D.One * 3.0);
					Vector3D vector = base.Parent.ForwardVector;
					List<MyEntity> entitiesInAABB = MyEntities.GetEntitiesInAABB(ref boundingBox);
					foreach (MyEntity item in entitiesInAABB)
					{
						MyCharacter myCharacter2 = item as MyCharacter;
						if (myCharacter2 != null && myCharacter2 != myCharacter && !(myCharacter2.ModelName == myCharacter.ModelName))
						{
							Vector3D vector3D = myCharacter2.PositionComp.GetPosition() - translation;
							double value = vector3D.Normalize();
							value = MathHelper.Clamp(value, 0.0, 6.0);
							double num = Vector3D.Dot(vector3D, vector);
							Vector3D value2 = -vector3D;
							if (num > -0.807)
							{
								correction += (6.0 - value) * (double)base.Weight * value2;
							}
							if (!correction.IsValid())
							{
								Debugger.Break();
							}
						}
					}
					entitiesInAABB.Clear();
					weight += base.Weight;
				}
			}
		}

		public override void DebugDraw()
		{
		}

		public override string GetName()
		{
			return "Character avoidance steering";
		}
	}
}
