using Sandbox.Engine.Platform;
using VRage.Game.Components;
using VRage.Generics;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Lights
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, Priority = 600)]
	public class MyLights : MySessionComponentBase
	{
		private static MyObjectsPool<MyLight> m_preallocatedLights;

		private static int m_lightsCount;

		private static BoundingSphere m_lastBoundingSphere;

		private static bool m_initialized;

		static MyLights()
		{
		}

		public override void LoadData()
		{
			MySandboxGame.Log.WriteLine("MyLights.LoadData() - START");
			MySandboxGame.Log.IncreaseIndent();
			if (m_preallocatedLights == null)
			{
				m_preallocatedLights = new MyObjectsPool<MyLight>(4000);
				MyLog.Default.Log(MyLogSeverity.Info, "MyLights preallocated lights cache created.");
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyLights.LoadData() - END");
			MyLog.Default.Log(MyLogSeverity.Info, "MyLights initialized.");
			m_initialized = true;
		}

		protected override void UnloadData()
		{
			if (m_preallocatedLights != null)
			{
				MyLog.Default.Log(MyLogSeverity.Info, "MyLights Unloading data.");
				m_preallocatedLights.DeallocateAll();
				m_preallocatedLights = null;
			}
		}

		public static MyLight AddLight()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return null;
			}
			m_preallocatedLights.AllocateOrCreate(out MyLight item);
			return item;
		}

		public static void RemoveLight(MyLight light)
		{
			if (!m_initialized)
			{
				MyLog.Default.Error("MyLights.RemoveLigt() not initialized, yet trying to remove lights");
			}
			else if (light != null)
			{
				light.Clear();
				if (m_preallocatedLights != null)
				{
					m_preallocatedLights.Deallocate(light);
				}
			}
		}
	}
}
