namespace VRage.Algorithms
{
	/// <summary>
	///  Fast representation for disjoint sets.
	///
	/// This data structure guarantees virtually constant time operations
	/// for union and finding the representative element of disjoint sets.
	/// </summary>
	///
	/// Still wondering weather the iterator makes sence
	public class MyUnionFind
	{
		private struct UF
		{
			public int Parent;

			public int Rank;
		}

		private struct step
		{
			public unsafe step* Prev;

			public int Element;
		}

		private UF[] m_indices;

		private int m_size;

		private bool IsInRange(int index)
		{
			if (index >= 0)
			{
				return index < m_size;
			}
			return false;
		}

		public MyUnionFind()
		{
		}

		public MyUnionFind(int initialSize)
		{
			Resize(initialSize);
		}

		public void Resize(int count = 0)
		{
			if (m_indices == null || m_indices.Length < count)
			{
				m_indices = new UF[count];
			}
			m_size = count;
			Clear();
		}

		public unsafe void Clear()
		{
			fixed (UF* ptr = m_indices)
			{
				for (int i = 0; i < m_size; i++)
				{
					ptr[i].Parent = i;
					ptr[i].Rank = 0;
				}
			}
		}

		public unsafe void Union(int a, int b)
		{
			fixed (UF* ptr = m_indices)
			{
				int num = Find(ptr, a);
				int num2 = Find(ptr, b);
				if (num != num2)
				{
					if (ptr[num].Rank < ptr[num2].Rank)
					{
						ptr[num].Parent = num2;
					}
					else if (ptr[num].Rank > ptr[num2].Rank)
					{
						ptr[num2].Parent = num;
					}
					else
					{
						ptr[num2].Parent = num;
						ptr[num].Rank++;
					}
				}
			}
		}

		private unsafe int Find(UF* uf, int a)
		{
			step* ptr = null;
			while (uf[a].Parent != a)
			{
				step* ptr2 = stackalloc step[1];
				ptr2->Element = a;
				ptr2->Prev = ptr;
				ptr = ptr2;
				a = uf[a].Parent;
			}
			while (ptr != null)
			{
				uf[ptr->Element].Parent = a;
				ptr = ptr->Prev;
			}
			return a;
		}

		public unsafe int Find(int a)
		{
			fixed (UF* uf = m_indices)
			{
				return Find(uf, a);
			}
		}
	}
}
