using ParallelTasks;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using VRage.Game;

namespace Sandbox.Game.Gui
{
	public class LoadPrefabData : WorkData
	{
		private MyObjectBuilder_Definitions m_prefab;

		private string m_path;

		private MyGuiBlueprintScreen_Reworked m_blueprintScreen;

		private ulong? m_id;

		private MyBlueprintItemInfo m_info;

		public MyObjectBuilder_Definitions Prefab => m_prefab;

		public LoadPrefabData(MyObjectBuilder_Definitions prefab, string path, MyGuiBlueprintScreen_Reworked blueprintScreen, ulong? id = null)
		{
			m_prefab = prefab;
			m_path = path;
			m_blueprintScreen = blueprintScreen;
			m_id = id;
		}

		public LoadPrefabData(MyObjectBuilder_Definitions prefab, MyBlueprintItemInfo info, MyGuiBlueprintScreen_Reworked blueprintScreen)
		{
			m_prefab = prefab;
			m_blueprintScreen = blueprintScreen;
			m_info = info;
		}

		public void CallLoadPrefab(WorkData workData)
		{
			m_prefab = MyBlueprintUtils.LoadPrefab(m_path);
			CallOnPrefabLoaded();
		}

		public void CallLoadWorkshopPrefab(WorkData workData)
		{
			m_prefab = MyBlueprintUtils.LoadWorkshopPrefab(m_path, m_id, isOldBlueprintScreen: false);
			CallOnPrefabLoaded();
		}

		public void CallLoadPrefabFromCloud(WorkData workData)
		{
			m_prefab = MyBlueprintUtils.LoadPrefabFromCloud(m_info);
			CallOnPrefabLoaded();
		}

		public void CallOnPrefabLoaded()
		{
			if (m_blueprintScreen != null && m_blueprintScreen.State == MyGuiScreenState.OPENED)
			{
				m_blueprintScreen.OnPrefabLoaded(m_prefab);
			}
		}
	}
}
