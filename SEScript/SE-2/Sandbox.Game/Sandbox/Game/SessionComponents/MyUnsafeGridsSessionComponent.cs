using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyUnsafeGridsSessionComponent : MySessionComponentBase
	{
		private static MyUnsafeGridsSessionComponent m_static;

		public Dictionary<long, MyCubeGrid> m_UnsafeGrids;

		public static MyUnsafeGridsSessionComponent Static => m_static;

		public static DictionaryReader<long, MyCubeGrid> UnsafeGrids
		{
			get
			{
				if (Static != null)
				{
					return Static.m_UnsafeGrids;
				}
				return null;
			}
		}

		public override bool IsRequiredByGame => true;

		public static void RegisterGrid(MyCubeGrid grid)
		{
			if (!grid.IsPreview)
			{
				Static.m_UnsafeGrids[grid.EntityId] = grid;
				RequestWarningUpdate();
			}
		}

		public static void UnregisterGrid(MyCubeGrid grid)
		{
			Static.m_UnsafeGrids.Remove(grid.EntityId);
			RequestWarningUpdate();
		}

		public static void OnGridChanged(MyCubeGrid grid)
		{
			RequestWarningUpdate();
		}

		private static void RequestWarningUpdate()
		{
			MySessionComponentWarningSystem.Static?.RequestUpdate();
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_UnsafeGrids = new Dictionary<long, MyCubeGrid>();
			m_static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_UnsafeGrids = null;
			m_static = null;
		}
	}
}
