using System;
using VRage.Game;

namespace Sandbox.Definitions
{
	public class MyCubeBlockDefinitionGroup
	{
		private static int m_sizeCount = Enum.GetValues(typeof(MyCubeSize)).Length;

		private readonly MyCubeBlockDefinition[] m_definitions;

		public MyCubeBlockDefinition this[MyCubeSize size]
		{
			get
			{
				return m_definitions[(uint)size];
			}
			set
			{
				m_definitions[(uint)size] = value;
			}
		}

		public int SizeCount => m_sizeCount;

		public MyCubeBlockDefinition Large => this[MyCubeSize.Large];

		public MyCubeBlockDefinition Small => this[MyCubeSize.Small];

		public MyCubeBlockDefinition Any
		{
			get
			{
				MyCubeBlockDefinition[] definitions = m_definitions;
				foreach (MyCubeBlockDefinition myCubeBlockDefinition in definitions)
				{
					if (myCubeBlockDefinition != null)
					{
						return myCubeBlockDefinition;
					}
				}
				return null;
			}
		}

		public MyCubeBlockDefinition AnyPublic
		{
			get
			{
				MyCubeBlockDefinition[] definitions = m_definitions;
				foreach (MyCubeBlockDefinition myCubeBlockDefinition in definitions)
				{
					if (myCubeBlockDefinition != null && myCubeBlockDefinition.Public)
					{
						return myCubeBlockDefinition;
					}
				}
				return null;
			}
		}

		public bool Contains(MyCubeBlockDefinition defCnt)
		{
			MyCubeBlockDefinition[] definitions = m_definitions;
			foreach (MyCubeBlockDefinition myCubeBlockDefinition in definitions)
			{
				if (myCubeBlockDefinition == defCnt)
				{
					return true;
				}
				MyDefinitionId[] blockStages = myCubeBlockDefinition.BlockStages;
				foreach (MyDefinitionId r in blockStages)
				{
					if (defCnt.Id == r)
					{
						return true;
					}
				}
			}
			return false;
		}

		internal MyCubeBlockDefinitionGroup()
		{
			m_definitions = new MyCubeBlockDefinition[m_sizeCount];
		}
	}
}
