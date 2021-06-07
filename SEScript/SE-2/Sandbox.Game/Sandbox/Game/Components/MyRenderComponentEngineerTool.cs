using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentEngineerTool : MyRenderComponent
	{
		private class Sandbox_Game_Components_MyRenderComponentEngineerTool_003C_003EActor : IActivator, IActivator<MyRenderComponentEngineerTool>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentEngineerTool();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentEngineerTool CreateInstance()
			{
				return new MyRenderComponentEngineerTool();
			}

			MyRenderComponentEngineerTool IActivator<MyRenderComponentEngineerTool>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyEngineerToolBase m_tool;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_tool = (base.Container.Entity as MyEngineerToolBase);
		}

		public override void Draw()
		{
			base.Draw();
			if (m_tool.CanBeDrawn())
			{
				DrawHighlight();
			}
		}

		public void DrawHighlight()
		{
			if (m_tool.GetTargetGrid() != null && m_tool.HasHitBlock && MySandboxGame.Config.ShowCrosshair)
			{
				MySlimBlock cubeBlock = m_tool.GetTargetGrid().GetCubeBlock(m_tool.TargetCube);
				if (cubeBlock != null)
				{
					cubeBlock.Orientation.GetMatrix(out Matrix result);
					MatrixD matrix = result;
					MatrixD worldMatrix = m_tool.GetTargetGrid().Physics.GetWorldMatrix();
					matrix = matrix * Matrix.CreateTranslation(cubeBlock.Position) * Matrix.CreateScale(m_tool.GetTargetGrid().GridSize) * worldMatrix;
					float lineWidth = (m_tool.GetTargetGrid().GridSizeEnum == MyCubeSize.Large) ? 0.06f : 0.03f;
					Vector3 value = new Vector3(0.5f, 0.5f, 0.5f);
					_ = MySession.Static.ElapsedPlayTime;
					Vector3 value2 = new Vector3(0.05f);
					BoundingBoxD localbox = new BoundingBoxD(-cubeBlock.BlockDefinition.Center - value - value2, cubeBlock.BlockDefinition.Size - cubeBlock.BlockDefinition.Center - value + value2);
					Color color = m_tool.HighlightColor;
					MyStringId highlightMaterial = m_tool.HighlightMaterial;
					MySimpleObjectDraw.DrawTransparentBox(ref matrix, ref localbox, ref color, MySimpleObjectRasterizer.Wireframe, 1, lineWidth, null, highlightMaterial);
				}
			}
		}
	}
}
