using System;

namespace VRageRender
{
	public static class MyRenderConstants
	{
		public const int RENDER_STEP_IN_MILLISECONDS = 16;

		public static readonly MyRenderQualityProfile[] m_renderQualityProfiles;

		public static MyRenderQualityProfile RenderQualityProfile => m_renderQualityProfiles[(int)MyRenderProxy.Settings.User.VoxelQuality];

		static MyRenderConstants()
		{
			m_renderQualityProfiles = new MyRenderQualityProfile[Enum.GetValues(typeof(MyRenderQualityEnum)).Length];
			float[] array = new float[11]
			{
				66f,
				200f,
				550f,
				1000f,
				1700f,
				3000f,
				6000f,
				15000f,
				40000f,
				100000f,
				250000f
			};
			float[] array2 = new float[11]
			{
				60f,
				180f,
				500f,
				900f,
				1600f,
				2800f,
				5500f,
				14000f,
				35000f,
				90000f,
				220000f
			};
			float[] array3 = new float[11]
			{
				55f,
				150f,
				450f,
				800f,
				1500f,
				2600f,
				4000f,
				13000f,
				30000f,
				80000f,
				200000f
			};
			m_renderQualityProfiles[1] = new MyRenderQualityProfile
			{
				LodClipmapRanges = new float[2][]
				{
					new float[8]
					{
						100f,
						300f,
						800f,
						2000f,
						4500f,
						13500f,
						30000f,
						100000f
					},
					array2
				},
				ExplosionDebrisCountMultiplier = 0.5f
			};
			m_renderQualityProfiles[0] = new MyRenderQualityProfile
			{
				LodClipmapRanges = new float[2][]
				{
					new float[8]
					{
						80f,
						240f,
						600f,
						1600f,
						4800f,
						14000f,
						35000f,
						100000f
					},
					array3
				},
				ExplosionDebrisCountMultiplier = 0f
			};
			m_renderQualityProfiles[2] = new MyRenderQualityProfile
			{
				LodClipmapRanges = new float[2][]
				{
					new float[8]
					{
						120f,
						360f,
						900f,
						2000f,
						4500f,
						13500f,
						30000f,
						100000f
					},
					array
				},
				ExplosionDebrisCountMultiplier = 0.8f
			};
			m_renderQualityProfiles[3] = new MyRenderQualityProfile
			{
				LodClipmapRanges = new float[2][]
				{
					new float[8]
					{
						140f,
						400f,
						1000f,
						2000f,
						4500f,
						13500f,
						30000f,
						100000f
					},
					array
				},
				ExplosionDebrisCountMultiplier = 3f
			};
		}
	}
}
