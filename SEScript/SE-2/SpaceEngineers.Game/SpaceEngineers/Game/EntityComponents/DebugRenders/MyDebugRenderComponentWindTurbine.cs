using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
	public class MyDebugRenderComponentWindTurbine : MyDebugRenderComponent
	{
		public new MyWindTurbine Entity => (MyWindTurbine)base.Entity;

		public MyDebugRenderComponentWindTurbine(MyWindTurbine turbine)
			: base(turbine)
		{
		}

		public override void DebugDraw()
		{
			base.DebugDraw();
			float[] rayEffectivities = Entity.RayEffectivities;
			for (int i = 0; i < rayEffectivities.Length; i++)
			{
				Entity.GetRaycaster(i, out Vector3D start, out Vector3D end);
				Vector3D vector3D = Vector3D.Lerp(start, end, Entity.BlockDefinition.MinRaycasterClearance);
				Vector3D vector3D2 = Vector3D.Lerp(vector3D, end, rayEffectivities[i]);
				MyRenderProxy.DebugDrawText3D(end, rayEffectivities[i].ToString("F2"), Color.Green, 0.7f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(start, vector3D, Color.Black, Color.Black, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, Color.Green, Color.Green, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(vector3D2, end, Color.Red, Color.Red, depthRead: false);
			}
		}
	}
}
