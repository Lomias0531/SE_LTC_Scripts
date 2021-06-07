using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public struct ConveyorLineEnumerator : IEnumerator<MyConveyorLine>, IEnumerator, IDisposable
	{
		private int index;

		private IMyConveyorEndpoint m_enumerated;

		private MyConveyorLine m_line;

		public MyConveyorLine Current => m_line;

		object IEnumerator.Current => m_line;

		public ConveyorLineEnumerator(IMyConveyorEndpoint enumerated)
		{
			index = -1;
			m_enumerated = enumerated;
			m_line = null;
		}

		public void Dispose()
		{
			m_enumerated = null;
			m_line = null;
		}

		public bool MoveNext()
		{
			while (MoveNextInternal())
			{
			}
			if (index >= m_enumerated.GetLineCount())
			{
				return false;
			}
			return true;
		}

		private bool MoveNextInternal()
		{
			index++;
			if (index >= m_enumerated.GetLineCount())
			{
				return false;
			}
			m_line = m_enumerated.GetConveyorLine(index);
			if (!m_line.IsWorking)
			{
				return true;
			}
			return false;
		}

		public void Reset()
		{
			index = 0;
		}
	}
}
