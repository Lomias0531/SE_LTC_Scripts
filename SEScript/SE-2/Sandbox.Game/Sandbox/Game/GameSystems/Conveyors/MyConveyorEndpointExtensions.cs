using Sandbox.Engine.Utils;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.Conveyors
{
	internal static class MyConveyorEndpointExtensions
	{
		private enum EndpointDebugShape
		{
			SHAPE_SPHERE,
			SHAPE_CAPSULE
		}

		public static void DebugDraw(this IMyConveyorEndpoint endpoint)
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)
			{
				return;
			}
			Vector3 value = default(Vector3);
			for (int i = 0; i < endpoint.GetLineCount(); i++)
			{
				ConveyorLinePosition position = endpoint.GetPosition(i);
				Vector3 vector = new Vector3(position.LocalGridPosition) + 0.5f * new Vector3(position.VectorDirection);
				value += vector;
			}
			value = value * endpoint.CubeBlock.CubeGrid.GridSize / endpoint.GetLineCount();
			value = Vector3.Transform(value, endpoint.CubeBlock.CubeGrid.WorldMatrix);
			for (int j = 0; j < endpoint.GetLineCount(); j++)
			{
				ConveyorLinePosition position2 = endpoint.GetPosition(j);
				MyConveyorLine conveyorLine = endpoint.GetConveyorLine(j);
				Vector3 position3 = (new Vector3(position2.LocalGridPosition) + 0.5f * new Vector3(position2.VectorDirection)) * endpoint.CubeBlock.CubeGrid.GridSize;
				Vector3 position4 = (new Vector3(position2.LocalGridPosition) + 0.4f * new Vector3(position2.VectorDirection)) * endpoint.CubeBlock.CubeGrid.GridSize;
				position3 = Vector3.Transform(position3, endpoint.CubeBlock.CubeGrid.WorldMatrix);
				position4 = Vector3.Transform(position4, endpoint.CubeBlock.CubeGrid.WorldMatrix);
				Vector3 value2 = Vector3.TransformNormal(position2.VectorDirection * endpoint.CubeBlock.CubeGrid.GridSize * 0.5f, endpoint.CubeBlock.CubeGrid.WorldMatrix);
				Color color = conveyorLine.IsFunctional ? Color.Orange : Color.DarkRed;
				color = (conveyorLine.IsWorking ? Color.GreenYellow : color);
				EndpointDebugShape endpointDebugShape = EndpointDebugShape.SHAPE_SPHERE;
				float num = 1f;
				float num2 = 0.05f;
				if (conveyorLine.GetEndpoint(0) == null || conveyorLine.GetEndpoint(1) == null)
				{
					if (conveyorLine.Type == MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE)
					{
						num = 0.2f;
						num2 = 0.015f;
						endpointDebugShape = EndpointDebugShape.SHAPE_SPHERE;
					}
					else
					{
						num = 0.1f;
						num2 = 0.015f;
						endpointDebugShape = EndpointDebugShape.SHAPE_CAPSULE;
					}
				}
				else if (conveyorLine.Type == MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE)
				{
					num = 1f;
					num2 = 0.05f;
					endpointDebugShape = EndpointDebugShape.SHAPE_SPHERE;
				}
				else
				{
					num = 0.2f;
					num2 = 0.05f;
					endpointDebugShape = EndpointDebugShape.SHAPE_CAPSULE;
				}
				MyRenderProxy.DebugDrawLine3D(position3, position3 + value2 * num, color, color, depthRead: true);
				switch (endpointDebugShape)
				{
				case EndpointDebugShape.SHAPE_SPHERE:
					MyRenderProxy.DebugDrawSphere(position3, num2 * endpoint.CubeBlock.CubeGrid.GridSize, color.ToVector3(), 1f, depthRead: false);
					break;
				case EndpointDebugShape.SHAPE_CAPSULE:
					MyRenderProxy.DebugDrawCapsule(position3 - value2 * num, position3 + value2 * num, num2 * endpoint.CubeBlock.CubeGrid.GridSize, color, depthRead: false);
					break;
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_IDS)
				{
					MyRenderProxy.DebugDrawText3D(position4, conveyorLine.GetHashCode().ToString(), color, 0.6f, depthRead: false);
				}
				MyRenderProxy.DebugDrawLine3D(position3, value, color, color, depthRead: false);
			}
		}
	}
}
