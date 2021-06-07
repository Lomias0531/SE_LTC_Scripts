using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
	[MyBotType(typeof(MyObjectBuilder_HumanoidBot))]
	public class MyHumanoidBot : MyAgentBot
	{
		public MyCharacter HumanoidEntity => base.AgentEntity;

		public MyHumanoidBotActions HumanoidActions => m_actions as MyHumanoidBotActions;

		public MyHumanoidBotDefinition HumanoidDefinition => m_botDefinition as MyHumanoidBotDefinition;

		public MyHumanoidBotLogic HumanoidLogic => base.AgentLogic as MyHumanoidBotLogic;

		public override bool IsValidForUpdate => base.IsValidForUpdate;

		protected MyDefinitionId StartingWeaponId
		{
			get
			{
				if (HumanoidDefinition == null)
				{
					return default(MyDefinitionId);
				}
				return HumanoidDefinition.StartingWeaponDefinitionId;
			}
		}

		public MyHumanoidBot(MyPlayer player, MyBotDefinition botDefinition)
			: base(player, botDefinition)
		{
		}

		public override void DebugDraw()
		{
			base.DebugDraw();
			if (HumanoidEntity == null)
			{
				return;
			}
			HumanoidActions.AiTargetBase.DebugDraw();
			MatrixD headMatrix = HumanoidEntity.GetHeadMatrix(includeY: true, includeX: true, forceHeadAnim: false, forceHeadBone: true);
			if (HumanoidActions.AiTargetBase.HasTarget())
			{
				HumanoidActions.AiTargetBase.DrawLineToTarget(headMatrix.Translation);
				HumanoidActions.AiTargetBase.GetTargetPosition(headMatrix.Translation, out Vector3D targetPosition, out float _);
				if (targetPosition != Vector3D.Zero)
				{
					MyRenderProxy.DebugDrawSphere(targetPosition, 0.3f, Color.Red, 0.4f, depthRead: false);
					MyRenderProxy.DebugDrawText3D(targetPosition, "GetTargetPosition", Color.Red, 1f, depthRead: false);
				}
			}
			MyRenderProxy.DebugDrawAxis(HumanoidEntity.PositionComp.WorldMatrix, 1f, depthRead: false);
			MatrixD m = headMatrix;
			m.Translation = Vector3.Zero;
			m = Matrix.Transpose(m);
			m.Translation = headMatrix.Translation;
		}
	}
}
