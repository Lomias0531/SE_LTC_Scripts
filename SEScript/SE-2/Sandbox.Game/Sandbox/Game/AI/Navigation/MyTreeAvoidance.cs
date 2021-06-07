using Havok;
using Sandbox.Engine.Physics;
using System.Collections.Generic;
using System.Diagnostics;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
	public class MyTreeAvoidance : MySteeringBase
	{
		private List<HkBodyCollision> m_trees = new List<HkBodyCollision>();

		public MyTreeAvoidance(MyBotNavigation navigation, float weight)
			: base(navigation, weight)
		{
		}

		public override string GetName()
		{
			return "Tree avoidance steering";
		}

		public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
		{
			if (!((double)base.Parent.Speed < 0.01))
			{
				Vector3D translation = base.Parent.PositionAndOrientation.Translation;
				Quaternion rotation = Quaternion.Identity;
				MyPhysics.GetPenetrationsShape(new HkSphereShape(6f), ref translation, ref rotation, m_trees, 9);
				foreach (HkBodyCollision tree in m_trees)
				{
					if (!(tree.Body == null))
					{
						MyPhysicsBody myPhysicsBody = tree.Body.UserObject as MyPhysicsBody;
						if (myPhysicsBody != null)
						{
							HkShape shape = tree.Body.GetShape();
							if (shape.ShapeType == HkShapeType.StaticCompound)
							{
								HkStaticCompoundShape hkStaticCompoundShape = (HkStaticCompoundShape)shape;
								hkStaticCompoundShape.DecomposeShapeKey(tree.ShapeKey, out int instanceId, out uint _);
								Vector3D vector3D = hkStaticCompoundShape.GetInstanceTransform(instanceId).Translation;
								vector3D += myPhysicsBody.GetWorldMatrix().Translation;
								Vector3D vector3D2 = vector3D - translation;
								double num = vector3D2.Normalize();
								vector3D2 = Vector3D.Reject(base.Parent.ForwardVector, vector3D2);
								vector3D2.Y = 0.0;
								if (vector3D2.Z * vector3D2.Z + vector3D2.X * vector3D2.X < 0.1)
								{
									Vector3D vector3D3 = Vector3D.TransformNormal(vector3D2, base.Parent.PositionAndOrientationInverted);
									vector3D2 = translation - vector3D;
									vector3D2 = Vector3D.Cross(Vector3D.Up, vector3D2);
									if (vector3D3.X < 0.0)
									{
										vector3D2 = -vector3D2;
									}
								}
								vector3D2.Normalize();
								correction += (6.0 - num) * (double)base.Weight * vector3D2;
								if (!correction.IsValid())
								{
									Debugger.Break();
								}
							}
						}
					}
				}
				m_trees.Clear();
				weight += base.Weight;
			}
		}
	}
}
