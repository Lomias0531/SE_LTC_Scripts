using System;
using System.Collections.Generic;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.ObjectBuilders.Voxels;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace VRage.Entities.Components
{
	[VoxelPostprocessing(typeof(MyObjectBuilder_VoxelPostprocessingDecimate), true)]
	public class MyDecimatePostprocessing : MyVoxelPostprocessing
	{
		public struct Settings
		{
			public int FromLod;

			public float FeatureAngle;

			public float EdgeThreshold;

			public float PlaneThreshold;

			public bool IgnoreEdges;

			public Settings(MyObjectBuilder_VoxelPostprocessingDecimate.Settings obSettings)
			{
				FromLod = obSettings.FromLod;
				FeatureAngle = MathHelper.ToRadians(obSettings.FeatureAngle);
				EdgeThreshold = obSettings.EdgeThreshold;
				PlaneThreshold = obSettings.PlaneThreshold;
				IgnoreEdges = obSettings.IgnoreEdges;
			}
		}

		/// <summary>
		/// Native decimator is shared across all definitions but before use setup with correct data.
		/// </summary>
		[ThreadStatic]
		private static VrDecimatePostprocessing m_instance;

		private List<Settings> m_perLodSettings = new List<Settings>();

		protected internal override void Init(MyObjectBuilder_VoxelPostprocessing builder)
		{
			base.Init(builder);
			MyObjectBuilder_VoxelPostprocessingDecimate obj = (MyObjectBuilder_VoxelPostprocessingDecimate)builder;
			int num = -1;
			foreach (MyObjectBuilder_VoxelPostprocessingDecimate.Settings lodSetting in obj.LodSettings)
			{
				if (lodSetting.FromLod <= num)
				{
					MyLog.Default.Error("Decimation lod sets must have strictly ascending lod indices.");
				}
				else
				{
					m_perLodSettings.Add(new Settings(lodSetting));
				}
			}
		}

		public override bool Get(int lod, out VrPostprocessing postprocess)
		{
			if (m_instance == null)
			{
				m_instance = new VrDecimatePostprocessing();
			}
			int num = m_perLodSettings.BinaryIntervalSearch((Settings x) => x.FromLod <= lod) - 1;
			if (num == -1)
			{
				postprocess = null;
				return false;
			}
			Settings settings = m_perLodSettings[num];
			m_instance.FeatureAngle = settings.FeatureAngle;
			m_instance.EdgeThreshold = settings.EdgeThreshold;
			m_instance.PlaneThreshold = settings.PlaneThreshold;
			m_instance.IgnoreEdges = settings.IgnoreEdges;
			postprocess = m_instance;
			return true;
		}
	}
}
