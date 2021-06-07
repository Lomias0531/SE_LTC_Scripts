using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
	internal class MyGuiScreenDebugPlanets : MyGuiScreenDebugBase
	{
		private float[] m_lodRanges;

		private static bool m_massive;

		private static MyGuiScreenDebugPlanets m_instance;

		private MyClipmapScaleEnum ScaleGroup
		{
			get
			{
				if (!m_massive)
				{
					return MyClipmapScaleEnum.Normal;
				}
				return MyClipmapScaleEnum.Massive;
			}
		}

		private static bool Massive
		{
			get
			{
				return m_massive;
			}
			set
			{
				if (m_massive != value)
				{
					m_instance.RecreateControls(constructor: false);
					m_massive = value;
				}
			}
		}

		public MyGuiScreenDebugPlanets()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugPlanets";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_instance = this;
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCheckBox("Debug draw areas: ", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_FLORA_BOXES));
			AddCheckBox("Massive", this, MemberHelper.GetMember(() => Massive));
			m_lodRanges = new float[MyRenderConstants.RenderQualityProfile.LodClipmapRanges[(int)ScaleGroup].Length];
			for (int i = 0; i < m_lodRanges.Length; i++)
			{
				int lod = i;
				AddSlider("LOD " + i, m_lodRanges[i], 0f, (i < 4) ? 1000 : ((i < 7) ? 10000 : 300000), delegate(MyGuiControlSlider s)
				{
					ChangeValue(s.Value, lod);
				});
			}
		}

		private void ChangeValue(float value, int lod)
		{
			m_lodRanges[lod] = value;
			float[][] lodClipmapRanges = MyRenderConstants.RenderQualityProfile.LodClipmapRanges;
			for (int i = 0; i < m_lodRanges.Length; i++)
			{
				lodClipmapRanges[(int)ScaleGroup][i] = m_lodRanges[i];
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
		}
	}
}
