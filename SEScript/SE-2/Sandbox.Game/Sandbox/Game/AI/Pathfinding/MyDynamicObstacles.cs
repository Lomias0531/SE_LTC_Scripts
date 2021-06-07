using Sandbox.Game.Entities;
using VRage.Collections;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyDynamicObstacles
	{
		private CachingList<IMyObstacle> m_obstacles;

		public MyDynamicObstacles()
		{
			m_obstacles = new CachingList<IMyObstacle>();
		}

		public void Clear()
		{
			m_obstacles.ClearImmediate();
		}

		public void Update()
		{
			foreach (IMyObstacle obstacle in m_obstacles)
			{
				obstacle.Update();
			}
			m_obstacles.ApplyChanges();
		}

		public bool IsInObstacle(Vector3D point)
		{
			foreach (IMyObstacle obstacle in m_obstacles)
			{
				if (obstacle.Contains(ref point))
				{
					return true;
				}
			}
			return false;
		}

		public void DebugDraw()
		{
			foreach (IMyObstacle obstacle in m_obstacles)
			{
				obstacle.DebugDraw();
			}
		}

		public void TryCreateObstacle(MyEntity newEntity)
		{
			if (newEntity.Physics != null && newEntity is MyCubeGrid && newEntity.PositionComp != null)
			{
				IMyObstacle myObstacle = MyObstacleFactory.CreateObstacleForEntity(newEntity);
				if (myObstacle != null)
				{
					m_obstacles.Add(myObstacle);
				}
			}
		}
	}
}
