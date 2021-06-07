using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Game.Entities.Cube
{
	public struct MyFatBlockReader<TBlock> : IEnumerator<TBlock>, IEnumerator, IDisposable where TBlock : MyCubeBlock
	{
		private HashSet<MySlimBlock>.Enumerator m_enumerator;

		public TBlock Current => (TBlock)m_enumerator.Current.FatBlock;

		object IEnumerator.Current => Current;

		public MyFatBlockReader(MyCubeGrid grid)
			: this(grid.GetBlocks().GetEnumerator())
		{
		}

		public MyFatBlockReader(HashSet<MySlimBlock> set)
			: this(set.GetEnumerator())
		{
		}

		public MyFatBlockReader(HashSet<MySlimBlock>.Enumerator enumerator)
		{
			m_enumerator = enumerator;
		}

		public MyFatBlockReader<TBlock> GetEnumerator()
		{
			return this;
		}

		public void Dispose()
		{
			m_enumerator.Dispose();
		}

		public bool MoveNext()
		{
			while (m_enumerator.MoveNext())
			{
				if (m_enumerator.Current.FatBlock as TBlock != null)
				{
					return true;
				}
			}
			return false;
		}

		public void Reset()
		{
			IEnumerator<MySlimBlock> enumerator = m_enumerator;
			enumerator.Reset();
			m_enumerator = (HashSet<MySlimBlock>.Enumerator)(object)enumerator;
		}
	}
}
