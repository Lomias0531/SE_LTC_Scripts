using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;
using VRage.Utils;

namespace VRage.Game.Models
{
	public static class MyModels
	{
		private static MyConcurrentDictionary<string, MyModel> m_models;

		/// <summary>
		/// Event that occures when some model needs to be loaded.
		/// </summary>
		private static readonly AutoResetEvent m_loadModelEvent;

		static MyModels()
		{
			m_models = new MyConcurrentDictionary<string, MyModel>();
			m_loadModelEvent = new AutoResetEvent(initialState: false);
		}

		public static void UnloadData()
		{
			foreach (MyModel loadedModel in GetLoadedModels())
			{
				loadedModel.UnloadData();
			}
			m_models.Clear();
		}

		public static MyModel GetModelOnlyData(string modelAsset)
		{
			if (string.IsNullOrEmpty(modelAsset))
			{
				return null;
			}
			if (!m_models.TryGetValue(modelAsset, out MyModel value))
			{
				value = new MyModel(modelAsset);
				m_models[modelAsset] = value;
			}
			value.LoadData();
			return value;
		}

		public static MyModel GetModelOnlyAnimationData(string modelAsset, bool forceReloadMwm = false)
		{
			if (forceReloadMwm || !m_models.TryGetValue(modelAsset, out MyModel value))
			{
				value = new MyModel(modelAsset);
				m_models[modelAsset] = value;
			}
			try
			{
				value.LoadAnimationData();
				return value;
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				return null;
			}
		}

		public static MyModel GetModelOnlyDummies(string modelAsset)
		{
			if (!m_models.TryGetValue(modelAsset, out MyModel value))
			{
				value = new MyModel(modelAsset);
				m_models[modelAsset] = value;
			}
			value.LoadOnlyDummies();
			return value;
		}

		public static MyModel GetModelOnlyModelInfo(string modelAsset)
		{
			if (!m_models.TryGetValue(modelAsset, out MyModel value))
			{
				value = new MyModel(modelAsset);
				m_models[modelAsset] = value;
			}
			value.LoadOnlyModelInfo();
			return value;
		}

		public static MyModel GetModel(string modelAsset)
		{
			if (modelAsset == null)
			{
				return null;
			}
			if (m_models.TryGetValue(modelAsset, out MyModel value))
			{
				return value;
			}
			return null;
		}

		public static List<MyModel> GetLoadedModels()
		{
			List<MyModel> result = new List<MyModel>();
			m_models.GetValues(result);
			return result;
		}
	}
}
