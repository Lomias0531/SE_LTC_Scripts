using Sandbox.Game.WorldEnvironment.__helper_namespace;
using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
	public class My2DClipmap<THandler> where THandler : class, IMy2DClipmapNodeHandler, new()
	{
		private struct StackInfo
		{
			public int Node;

			public Vector2D Center;

			public double Size;

			public int Lod;

			public StackInfo(int node, Vector2D center, double size, int lod)
			{
				Node = node;
				Center = center;
				Size = size;
				Lod = lod;
			}
		}

		private int m_root;

		private double m_size;

		private int m_splits;

		private double[] m_lodSizes;

		private double[] m_keepLodSizes;

		private MyFreeList<THandler> m_leafHandlers;

		private Node[] m_nodes;

		private THandler[] m_nodeHandlers;

		private int m_firstFree;

		private const int NullNode = int.MinValue;

		public int NodeAllocDeallocs;

		private readonly List<int> m_nodesToDealloc = new List<int>();

		private IMy2DClipmapManager m_manager;

		private readonly Stack<StackInfo> m_nodesToScanNext = new Stack<StackInfo>();

		private BoundingBox2D[] m_queryBounds;

		private BoundingBox2D[] m_keepBounds;

		private readonly BoundingBox2D[] m_nodeBoundsTmp = new BoundingBox2D[4];

		private readonly IMy2DClipmapNodeHandler[] m_tmpNodeHandlerList = new IMy2DClipmapNodeHandler[4];

		public Vector3D LastPosition
		{
			get;
			set;
		}

		public MatrixD WorldMatrix
		{
			get;
			private set;
		}

		public MatrixD InverseWorldMatrix
		{
			get;
			private set;
		}

		public double FaceHalf => m_lodSizes[m_splits];

		public double LeafSize => m_lodSizes[1];

		public int Depth => m_splits;

		private unsafe void PrepareAllocator()
		{
			int num = 16;
			m_nodes = new Node[num];
			fixed (Node* ptr = m_nodes)
			{
				for (int i = 0; i < num; i++)
				{
					ptr[i].Lod = ~(i + 1);
				}
			}
			m_firstFree = 0;
			m_nodeHandlers = new THandler[num];
			m_leafHandlers = new MyFreeList<THandler>();
		}

		private unsafe int AllocNode()
		{
			NodeAllocDeallocs++;
			if (m_firstFree == m_nodes.Length)
			{
				int num = m_nodes.Length;
				Array.Resize(ref m_nodes, m_nodes.Length << 1);
				Array.Resize(ref m_nodeHandlers, m_nodes.Length);
				fixed (Node* ptr = m_nodes)
				{
					for (int i = num; i < m_nodes.Length; i++)
					{
						ptr[i].Lod = ~(i + 1);
					}
				}
				m_firstFree = num;
			}
			int firstFree = m_firstFree;
			fixed (Node* ptr2 = m_nodes)
			{
				for (int j = 0; j < 4; j++)
				{
					ptr2[m_firstFree].Children[j] = int.MinValue;
				}
				m_firstFree = ~ptr2[m_firstFree].Lod;
			}
			return firstFree;
		}

		private unsafe void FreeNode(int node)
		{
			NodeAllocDeallocs++;
			fixed (Node* ptr = m_nodes)
			{
				ptr[node].Lod = ~m_firstFree;
				m_firstFree = node;
				m_nodeHandlers[node] = null;
			}
		}

		private void Compact()
		{
		}

		private unsafe int Child(int node, int index)
		{
			fixed (Node* ptr = m_nodes)
			{
				return ptr[node].Children[index];
			}
		}

		private unsafe void CollapseSubtree(int parent, int childIndex, Node* nodes)
		{
			int num = nodes[parent].Children[childIndex];
			m_nodesToDealloc.Add(num);
			Node* ptr = nodes + num;
			for (int i = 0; i < 4; i++)
			{
				if (ptr->Children[i] >= 0)
				{
					CollapseSubtree(num, i, nodes);
				}
			}
			IMy2DClipmapNodeHandler[] tmpNodeHandlerList = m_tmpNodeHandlerList;
			for (int j = 0; j < 4; j++)
			{
				tmpNodeHandlerList[j] = m_leafHandlers[~ptr->Children[j]];
			}
			THandler val = m_nodeHandlers[num];
			val.InitJoin(tmpNodeHandlerList);
			for (int k = 0; k < 4; k++)
			{
				m_leafHandlers.Free(~ptr->Children[k]);
				tmpNodeHandlerList[k].Close();
			}
			int num2 = m_leafHandlers.Allocate();
			nodes[parent].Children[childIndex] = ~num2;
			m_leafHandlers[num2] = val;
		}

		private unsafe void CollapseRoot()
		{
			fixed (Node* ptr = m_nodes)
			{
				Node* ptr2 = ptr + m_root;
				if (*ptr2->Children == int.MinValue)
				{
					return;
				}
				for (int i = 0; i < 4; i++)
				{
					if (ptr2->Children[i] >= 0)
					{
						CollapseSubtree(m_root, i, ptr);
					}
				}
				IMy2DClipmapNodeHandler[] tmpNodeHandlerList = m_tmpNodeHandlerList;
				for (int j = 0; j < 4; j++)
				{
					tmpNodeHandlerList[j] = m_leafHandlers[~ptr2->Children[j]];
				}
				m_nodeHandlers[m_root].InitJoin(tmpNodeHandlerList);
				for (int k = 0; k < 4; k++)
				{
					m_leafHandlers.Free(~ptr2->Children[k]);
					tmpNodeHandlerList[k].Close();
					ptr2->Children[k] = int.MinValue;
				}
			}
			foreach (int item in m_nodesToDealloc)
			{
				FreeNode(item);
			}
			m_nodesToDealloc.Clear();
		}

		public unsafe void Init(IMy2DClipmapManager manager, ref MatrixD worldMatrix, double sectorSize, double faceSize)
		{
			m_manager = manager;
			WorldMatrix = worldMatrix;
			InverseWorldMatrix = Matrix.Invert(worldMatrix);
			m_size = faceSize;
			m_splits = Math.Max(MathHelper.Log2Floor((int)(faceSize / sectorSize)), 1);
			m_lodSizes = new double[m_splits + 1];
			for (int i = 0; i <= m_splits; i++)
			{
				m_lodSizes[m_splits - i] = faceSize / (double)(1 << i + 1);
			}
			m_keepLodSizes = new double[m_splits + 1];
			for (int j = 0; j <= m_splits; j++)
			{
				m_keepLodSizes[j] = 1.5 * m_lodSizes[j];
			}
			m_queryBounds = new BoundingBox2D[m_splits + 1];
			m_keepBounds = new BoundingBox2D[m_splits + 1];
			PrepareAllocator();
			m_root = AllocNode();
			fixed (Node* ptr = m_nodes)
			{
				ptr[m_root].Lod = m_splits;
			}
			BoundingBox2D bounds = new BoundingBox2D(new Vector2D((0.0 - faceSize) / 2.0), new Vector2D(faceSize / 2.0));
			m_nodeHandlers[m_root] = new THandler();
			m_nodeHandlers[m_root].Init(m_manager, 0, 0, m_splits, ref bounds);
		}

		public unsafe void Update(Vector3D localPosition)
		{
			double num = localPosition.Z * 0.1;
			num *= num;
			if (Vector3D.DistanceSquared(LastPosition, localPosition) < num)
			{
				return;
			}
			LastPosition = localPosition;
			Vector2D value = new Vector2D(localPosition.X, localPosition.Y);
			for (int num2 = m_splits; num2 >= 0; num2--)
			{
				m_queryBounds[num2] = new BoundingBox2D(value - m_lodSizes[num2], value + m_lodSizes[num2]);
				m_keepBounds[num2] = new BoundingBox2D(value - m_keepLodSizes[num2], value + m_keepLodSizes[num2]);
			}
			if (localPosition.Z > m_keepLodSizes[m_splits])
			{
				if (Child(m_root, 0) != int.MinValue)
				{
					CollapseRoot();
				}
			}
			else
			{
				m_nodesToScanNext.Push(new StackInfo(m_root, Vector2D.Zero, m_size / 2.0, m_splits));
				fixed (BoundingBox2D* ptr = m_nodeBoundsTmp)
				{
					fixed (BoundingBox2D* ptr5 = m_keepBounds)
					{
						fixed (BoundingBox2D* ptr2 = m_queryBounds)
						{
							while (m_nodesToScanNext.Count != 0)
							{
								StackInfo stackInfo = m_nodesToScanNext.Pop();
								double num3 = stackInfo.Size / 2.0;
								int num4 = stackInfo.Lod - 1;
								int num5 = 0;
								for (int i = 0; i < 4; i++)
								{
									ptr[i].Min = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[i] * stackInfo.Size - stackInfo.Size;
									ptr[i].Max = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[i] * stackInfo.Size;
									if (ptr[i].Intersects(ref ptr2[num4]) && localPosition.Z <= ptr2[num4].Height)
									{
										num5 |= 1 << i;
									}
								}
								if (Child(stackInfo.Node, 0) == int.MinValue)
								{
									THandler val = m_nodeHandlers[stackInfo.Node];
									IMy2DClipmapNodeHandler[] children = m_tmpNodeHandlerList;
									fixed (Node* ptr3 = m_nodes)
									{
										for (int j = 0; j < 4; j++)
										{
											int num6 = m_leafHandlers.Allocate();
											m_leafHandlers[num6] = new THandler();
											children[j] = m_leafHandlers[num6];
											ptr3[stackInfo.Node].Children[j] = ~num6;
										}
									}
									val.Split(ptr, ref children);
									val.Close();
								}
								if (stackInfo.Lod != 1)
								{
									for (int k = 0; k < 4; k++)
									{
										int num7 = Child(stackInfo.Node, k);
										if ((num5 & (1 << k)) != 0)
										{
											if (num7 < 0)
											{
												THandler val2 = m_leafHandlers[~num7];
												m_leafHandlers.Free(~num7);
												num7 = AllocNode();
												m_nodeHandlers[num7] = val2;
												fixed (Node* ptr4 = m_nodes)
												{
													ptr4[num7].Lod = num4;
													ptr4[stackInfo.Node].Children[k] = num7;
												}
											}
										}
										else if (num7 >= 0 && (!ptr[k].Intersects(ref ptr5[num4]) || !(localPosition.Z <= ptr5[num4].Height)))
										{
											fixed (Node* nodes = m_nodes)
											{
												CollapseSubtree(stackInfo.Node, k, nodes);
											}
										}
										if (num7 >= 0)
										{
											m_nodesToScanNext.Push(new StackInfo(num7, stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[k] * stackInfo.Size - num3, num3, num4));
										}
									}
								}
							}
						}
					}
				}
			}
			foreach (int item in m_nodesToDealloc)
			{
				FreeNode(item);
			}
			m_nodesToDealloc.Clear();
		}

		public unsafe THandler GetHandler(Vector2D point)
		{
			m_nodesToScanNext.Push(new StackInfo(m_root, Vector2D.Zero, m_size / 2.0, m_splits));
			int num = m_root;
			fixed (Node* ptr = m_nodes)
			{
				BoundingBox2D boundingBox2D = default(BoundingBox2D);
				while (m_nodesToScanNext.Count != 0)
				{
					StackInfo stackInfo = m_nodesToScanNext.Pop();
					double num2 = stackInfo.Size / 2.0;
					int num3 = stackInfo.Lod - 1;
					for (int i = 0; i < 4; i++)
					{
						boundingBox2D.Min = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[i] * stackInfo.Size - stackInfo.Size;
						boundingBox2D.Max = stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[i] * stackInfo.Size;
						if (boundingBox2D.Contains(point) != 0)
						{
							int num4 = ptr[stackInfo.Node].Children[i];
							if (num4 != int.MinValue)
							{
								num = num4;
								if (num3 > 0 && num4 >= 0)
								{
									m_nodesToScanNext.Push(new StackInfo(num, stackInfo.Center + My2DClipmapHelpers.CoordsFromIndex[i] * stackInfo.Size - num2, num2, num3));
								}
							}
						}
					}
				}
			}
			if (num < 0)
			{
				return m_leafHandlers[~num];
			}
			return m_nodeHandlers[num];
		}

		public void Clear()
		{
			CollapseRoot();
		}
	}
}
