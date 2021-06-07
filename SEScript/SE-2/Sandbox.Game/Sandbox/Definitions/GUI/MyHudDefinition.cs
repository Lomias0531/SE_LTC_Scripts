using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions.GUI
{
	[MyDefinitionType(typeof(MyObjectBuilder_HudDefinition), null)]
	public class MyHudDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_GUI_MyHudDefinition_003C_003EActor : IActivator, IActivator<MyHudDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyHudDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHudDefinition CreateInstance()
			{
				return new MyHudDefinition();
			}

			MyHudDefinition IActivator<MyHudDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyObjectBuilder_ToolbarControlVisualStyle m_toolbar;

		private MyObjectBuilder_StatControls[] m_statControlses;

		private MyObjectBuilder_GravityIndicatorVisualStyle m_gravityIndicator;

		private MyObjectBuilder_CrosshairStyle m_crosshair;

		private Vector2I? m_optimalScreenRatio;

		private float? m_customUIScale;

		private MyStringHash? m_visorOverlayTexture;

		private MyObjectBuilder_DPadControlVisualStyle m_DPad;

		public MyObjectBuilder_ToolbarControlVisualStyle Toolbar => m_toolbar;

		public MyObjectBuilder_StatControls[] StatControls => m_statControlses;

		public MyObjectBuilder_GravityIndicatorVisualStyle GravityIndicator => m_gravityIndicator;

		public MyObjectBuilder_CrosshairStyle Crosshair => m_crosshair;

		public Vector2I? OptimalScreenRatio => m_optimalScreenRatio;

		public float? CustomUIScale => m_customUIScale;

		public MyStringHash? VisorOverlayTexture => m_visorOverlayTexture;

		public MyObjectBuilder_DPadControlVisualStyle DPad => m_DPad;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_HudDefinition myObjectBuilder_HudDefinition = builder as MyObjectBuilder_HudDefinition;
			m_toolbar = myObjectBuilder_HudDefinition.Toolbar;
			m_statControlses = myObjectBuilder_HudDefinition.StatControls;
			m_gravityIndicator = myObjectBuilder_HudDefinition.GravityIndicator;
			m_crosshair = myObjectBuilder_HudDefinition.Crosshair;
			m_optimalScreenRatio = myObjectBuilder_HudDefinition.OptimalScreenRatio;
			m_customUIScale = myObjectBuilder_HudDefinition.CustomUIScale;
			m_visorOverlayTexture = myObjectBuilder_HudDefinition.VisorOverlayTexture;
			m_DPad = myObjectBuilder_HudDefinition.DPad;
		}
	}
}
