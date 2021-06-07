using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;

namespace VRageMath
{
	/// <summary>
	/// Dynamic aabb tree implementation as a prunning structure
	/// </summary>
	public class MyDynamicAABBTree
	{
		/// <summary>
		/// A node in the dynamic tree. The client does not interact with this directly.
		/// </summary>
		public class DynamicTreeNode
		{
			/// This is the fattened BoundingBox.
			public BoundingBox Aabb;

			public int Child1;

			public int Child2;

			public int Height;

			public int ParentOrNext;

			public object UserData;

			public uint UserFlag;

			public bool IsLeaf()
			{
				return Child1 == -1;
			}
		}

		/// <summary>
		/// A dynamic tree arranges data in a binary tree to accelerate
		/// queries such as volume queries and ray casts. Leafs are proxies
		/// with an BoundingBox. In the tree we expand the proxy BoundingBox by Settings.b2_fatAABBFactor
		/// so that the proxy BoundingBox is bigger than the client object. This allows the client
		/// object to move by small amounts without triggering a tree update.
		/// Nodes are pooled and relocatable, so we use node indices rather than pointers.
		/// </summary>
		public const int NullNode = -1;

		private int m_freeList;

		private int m_nodeCapacity;

		private int m_nodeCount;

		private DynamicTreeNode[] m_nodes;

		private Dictionary<int, DynamicTreeNode> m_leafElementCache;

		private int m_root;

		[ThreadStatic]
		private static Stack<int> m_queryStack;

		private static List<Stack<int>> m_StackCacheCollection = new List<Stack<int>>();

		private Vector3 m_extension;

		private float m_aabbMultiplier;

		private FastResourceLock m_rwLock = new FastResourceLock();

		public DictionaryValuesReader<int, DynamicTreeNode> Leaves => new DictionaryValuesReader<int, DynamicTreeNode>(m_leafElementCache);

		private Stack<int> CurrentThreadStack
		{
			get
			{
				if (m_queryStack == null)
				{
					m_queryStack = new Stack<int>(32);
					lock (m_StackCacheCollection)
					{
						m_StackCacheCollection.Add(m_queryStack);
					}
				}
				return m_queryStack;
			}
		}

		public MyDynamicAABBTree()
		{
			Init(Vector3.One, 1f);
		}

		/// constructing the tree initializes the node pool.
		public MyDynamicAABBTree(Vector3 extension, float aabbMultiplier = 1f)
		{
			Init(extension, aabbMultiplier);
		}

		private void Init(Vector3 extension, float aabbMultiplier)
		{
			m_extension = extension;
			m_aabbMultiplier = aabbMultiplier;
			_ = CurrentThreadStack;
			Clear();
		}

		private Stack<int> GetStack()
		{
			Stack<int> currentThreadStack = CurrentThreadStack;
			currentThreadStack.Clear();
			return currentThreadStack;
		}

		private void PushStack(Stack<int> stack)
		{
		}

		/// <summary>
		/// Create a proxy. Provide a tight fitting BoundingBox and a userData pointer.
		/// </summary>
		/// <param name="aabb">The aabb.</param>
		/// <param name="userData">The user data.</param>
		/// <returns></returns>
		public int AddProxy(ref BoundingBox aabb, object userData, uint userFlags, bool rebalance = true)
		{
			using (m_rwLock.AcquireExclusiveUsing())
			{
				int num = AllocateNode();
				m_nodes[num].Aabb = aabb;
				m_nodes[num].Aabb.Min -= m_extension;
				m_nodes[num].Aabb.Max += m_extension;
				m_nodes[num].UserData = userData;
				m_nodes[num].UserFlag = userFlags;
				m_nodes[num].Height = 0;
				m_leafElementCache[num] = m_nodes[num];
				InsertLeaf(num, rebalance);
				return num;
			}
		}

		/// <summary>
		/// Destroy a proxy. This asserts if the id is invalid.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		public void RemoveProxy(int proxyId)
		{
			using (m_rwLock.AcquireExclusiveUsing())
			{
				m_leafElementCache.Remove(proxyId);
				RemoveLeaf(proxyId);
				FreeNode(proxyId);
			}
		}

		/// <summary>
		/// Move a proxy with a swepted BoundingBox. If the proxy has moved outside of its fattened BoundingBox,
		/// then the proxy is removed from the tree and re-inserted. Otherwise
		/// the function returns immediately.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <param name="aabb">The aabb.</param>
		/// <param name="displacement">The displacement.</param>
		/// <returns>true if the proxy was re-inserted.</returns>
		public bool MoveProxy(int proxyId, ref BoundingBox aabb, Vector3 displacement)
		{
			using (m_rwLock.AcquireExclusiveUsing())
			{
				if (m_nodes[proxyId].Aabb.Contains(aabb) == ContainmentType.Contains)
				{
					return false;
				}
				RemoveLeaf(proxyId);
				BoundingBox aabb2 = aabb;
				Vector3 extension = m_extension;
				aabb2.Min -= extension;
				aabb2.Max += extension;
				Vector3 vector = m_aabbMultiplier * displacement;
				if (vector.X < 0f)
				{
					aabb2.Min.X += vector.X;
				}
				else
				{
					aabb2.Max.X += vector.X;
				}
				if (vector.Y < 0f)
				{
					aabb2.Min.Y += vector.Y;
				}
				else
				{
					aabb2.Max.Y += vector.Y;
				}
				if (vector.Z < 0f)
				{
					aabb2.Min.Z += vector.Z;
				}
				else
				{
					aabb2.Max.Z += vector.Z;
				}
				m_nodes[proxyId].Aabb = aabb2;
				InsertLeaf(proxyId, rebalance: true);
			}
			return true;
		}

		/// <summary>
		/// Get proxy user data.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <returns>the proxy user data or 0 if the id is invalid.</returns>
		public T GetUserData<T>(int proxyId)
		{
			return (T)m_nodes[proxyId].UserData;
		}

		public int GetRoot()
		{
			return m_root;
		}

		public int GetLeafCount()
		{
			return m_leafElementCache.Count;
		}

		public int GetLeafCount(int proxyId)
		{
			int num = 0;
			Stack<int> stack = GetStack();
			stack.Push(proxyId);
			while (stack.Count > 0)
			{
				int num2 = stack.Pop();
				if (num2 != -1)
				{
					DynamicTreeNode dynamicTreeNode = m_nodes[num2];
					if (dynamicTreeNode.IsLeaf())
					{
						num++;
						continue;
					}
					stack.Push(dynamicTreeNode.Child1);
					stack.Push(dynamicTreeNode.Child2);
				}
			}
			PushStack(stack);
			return num;
		}

		public void GetNodeLeaves(int proxyId, List<int> children)
		{
			Stack<int> stack = GetStack();
			stack.Push(proxyId);
			while (stack.Count > 0)
			{
				int num = stack.Pop();
				if (num != -1)
				{
					DynamicTreeNode dynamicTreeNode = m_nodes[num];
					if (dynamicTreeNode.IsLeaf())
					{
						children.Add(num);
						continue;
					}
					stack.Push(dynamicTreeNode.Child1);
					stack.Push(dynamicTreeNode.Child2);
				}
			}
			PushStack(stack);
		}

		public BoundingBox GetAabb(int proxyId)
		{
			return m_nodes[proxyId].Aabb;
		}

		public void GetChildren(int proxyId, out int left, out int right)
		{
			left = m_nodes[proxyId].Child1;
			right = m_nodes[proxyId].Child2;
		}

		/// <summary>
		/// Get proxy user data.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <returns>the proxy user data or 0 if the id is invalid.</returns>
		private uint GetUserFlag(int proxyId)
		{
			return m_nodes[proxyId].UserFlag;
		}

		/// <summary>
		/// Get the fat BoundingBox for a proxy.
		/// </summary>
		/// <param name="proxyId">The proxy id.</param>
		/// <param name="fatAABB">The fat BoundingBox.</param>
		public void GetFatAABB(int proxyId, out BoundingBox fatAABB)
		{
			using (m_rwLock.AcquireSharedUsing())
			{
				fatAABB = m_nodes[proxyId].Aabb;
			}
		}

		/// Query an BoundingBox for overlapping proxies. The callback class
		/// is called for each proxy that overlaps the supplied BoundingBox.
		public void Query(Func<int, bool> callback, ref BoundingBox aabb)
		{
			using (m_rwLock.AcquireSharedUsing())
			{
				Stack<int> stack = GetStack();
				stack.Push(m_root);
				while (stack.Count > 0)
				{
					int num = stack.Pop();
					if (num != -1)
					{
						DynamicTreeNode dynamicTreeNode = m_nodes[num];
						if (dynamicTreeNode.Aabb.Intersects(aabb))
						{
							if (dynamicTreeNode.IsLeaf())
							{
								if (!callback(num))
								{
									break;
								}
							}
							else
							{
								stack.Push(dynamicTreeNode.Child1);
								stack.Push(dynamicTreeNode.Child2);
							}
						}
					}
				}
			}
		}

		public int CountLeaves(int nodeId)
		{
			using (m_rwLock.AcquireSharedUsing())
			{
				if (nodeId == -1)
				{
					return 0;
				}
				DynamicTreeNode dynamicTreeNode = m_nodes[nodeId];
				if (dynamicTreeNode.IsLeaf())
				{
					return 1;
				}
				int num = CountLeaves(dynamicTreeNode.Child1);
				int num2 = CountLeaves(dynamicTreeNode.Child2);
				return num + num2;
			}
		}

		private int AllocateNode()
		{
			if (m_freeList == -1)
			{
				DynamicTreeNode[] nodes = m_nodes;
				m_nodeCapacity *= 2;
				m_nodes = new DynamicTreeNode[m_nodeCapacity];
				Array.Copy(nodes, m_nodes, m_nodeCount);
				for (int i = m_nodeCount; i < m_nodeCapacity - 1; i++)
				{
					m_nodes[i] = new DynamicTreeNode
					{
						ParentOrNext = i + 1,
						Height = 1
					};
				}
				m_nodes[m_nodeCapacity - 1] = new DynamicTreeNode
				{
					ParentOrNext = -1,
					Height = 1
				};
				m_freeList = m_nodeCount;
			}
			int freeList = m_freeList;
			m_freeList = m_nodes[freeList].ParentOrNext;
			m_nodes[freeList].ParentOrNext = -1;
			m_nodes[freeList].Child1 = -1;
			m_nodes[freeList].Child2 = -1;
			m_nodes[freeList].Height = 0;
			m_nodes[freeList].UserData = null;
			m_nodeCount++;
			return freeList;
		}

		private void FreeNode(int nodeId)
		{
			m_nodes[nodeId].ParentOrNext = m_freeList;
			m_nodes[nodeId].Height = -1;
			m_nodes[nodeId].UserData = null;
			m_freeList = nodeId;
			m_nodeCount--;
		}

		private void InsertLeaf(int leaf, bool rebalance)
		{
			if (m_root == -1)
			{
				m_root = leaf;
				m_nodes[m_root].ParentOrNext = -1;
				return;
			}
			BoundingBox original = m_nodes[leaf].Aabb;
			int num = m_root;
			while (!m_nodes[num].IsLeaf())
			{
				int child = m_nodes[num].Child1;
				int child2 = m_nodes[num].Child2;
				if (rebalance)
				{
					float perimeter = m_nodes[num].Aabb.Perimeter;
					float perimeter2 = BoundingBox.CreateMerged(m_nodes[num].Aabb, original).Perimeter;
					float num2 = 2f * perimeter2;
					float num3 = 2f * (perimeter2 - perimeter);
					float num4;
					if (m_nodes[child].IsLeaf())
					{
						BoundingBox.CreateMerged(ref original, ref m_nodes[child].Aabb, out BoundingBox result);
						num4 = result.Perimeter + num3;
					}
					else
					{
						BoundingBox.CreateMerged(ref original, ref m_nodes[child].Aabb, out BoundingBox result2);
						float perimeter3 = m_nodes[child].Aabb.Perimeter;
						num4 = result2.Perimeter - perimeter3 + num3;
					}
					float num5;
					if (m_nodes[child2].IsLeaf())
					{
						BoundingBox.CreateMerged(ref original, ref m_nodes[child2].Aabb, out BoundingBox result3);
						num5 = result3.Perimeter + num3;
					}
					else
					{
						BoundingBox.CreateMerged(ref original, ref m_nodes[child2].Aabb, out BoundingBox result4);
						float perimeter4 = m_nodes[child2].Aabb.Perimeter;
						num5 = result4.Perimeter - perimeter4 + num3;
					}
					if (num2 < num4 && num4 < num5)
					{
						break;
					}
					num = ((!(num4 < num5)) ? child2 : child);
				}
				else
				{
					BoundingBox.CreateMerged(ref original, ref m_nodes[child].Aabb, out BoundingBox result5);
					BoundingBox.CreateMerged(ref original, ref m_nodes[child2].Aabb, out BoundingBox result6);
					float num6 = (float)(m_nodes[child].Height + 1) * result5.Perimeter;
					float num7 = (float)(m_nodes[child2].Height + 1) * result6.Perimeter;
					num = ((!(num6 < num7)) ? child2 : child);
				}
			}
			int num8 = num;
			int parentOrNext = m_nodes[num].ParentOrNext;
			int num9 = AllocateNode();
			m_nodes[num9].ParentOrNext = parentOrNext;
			m_nodes[num9].UserData = null;
			m_nodes[num9].Aabb = BoundingBox.CreateMerged(original, m_nodes[num8].Aabb);
			m_nodes[num9].Height = m_nodes[num8].Height + 1;
			if (parentOrNext != -1)
			{
				if (m_nodes[parentOrNext].Child1 == num8)
				{
					m_nodes[parentOrNext].Child1 = num9;
				}
				else
				{
					m_nodes[parentOrNext].Child2 = num9;
				}
				m_nodes[num9].Child1 = num8;
				m_nodes[num9].Child2 = leaf;
				m_nodes[num].ParentOrNext = num9;
				m_nodes[leaf].ParentOrNext = num9;
			}
			else
			{
				m_nodes[num9].Child1 = num8;
				m_nodes[num9].Child2 = leaf;
				m_nodes[num].ParentOrNext = num9;
				m_nodes[leaf].ParentOrNext = num9;
				m_root = num9;
			}
			for (num = m_nodes[leaf].ParentOrNext; num != -1; num = m_nodes[num].ParentOrNext)
			{
				if (rebalance)
				{
					num = Balance(num);
				}
				int child3 = m_nodes[num].Child1;
				int child4 = m_nodes[num].Child2;
				m_nodes[num].Height = 1 + Math.Max(m_nodes[child3].Height, m_nodes[child4].Height);
				BoundingBox.CreateMerged(ref m_nodes[child3].Aabb, ref m_nodes[child4].Aabb, out m_nodes[num].Aabb);
			}
		}

		private void RemoveLeaf(int leaf)
		{
			if (m_root == -1)
			{
				return;
			}
			if (leaf == m_root)
			{
				m_root = -1;
				return;
			}
			int parentOrNext = m_nodes[leaf].ParentOrNext;
			int parentOrNext2 = m_nodes[parentOrNext].ParentOrNext;
			int num = (m_nodes[parentOrNext].Child1 != leaf) ? m_nodes[parentOrNext].Child1 : m_nodes[parentOrNext].Child2;
			if (parentOrNext2 != -1)
			{
				if (m_nodes[parentOrNext2].Child1 == parentOrNext)
				{
					m_nodes[parentOrNext2].Child1 = num;
				}
				else
				{
					m_nodes[parentOrNext2].Child2 = num;
				}
				m_nodes[num].ParentOrNext = parentOrNext2;
				FreeNode(parentOrNext);
				int num2;
				for (num2 = parentOrNext2; num2 != -1; num2 = m_nodes[num2].ParentOrNext)
				{
					num2 = Balance(num2);
					int child = m_nodes[num2].Child1;
					int child2 = m_nodes[num2].Child2;
					m_nodes[num2].Aabb = BoundingBox.CreateMerged(m_nodes[child].Aabb, m_nodes[child2].Aabb);
					m_nodes[num2].Height = 1 + Math.Max(m_nodes[child].Height, m_nodes[child2].Height);
				}
			}
			else
			{
				m_root = num;
				m_nodes[num].ParentOrNext = -1;
				FreeNode(parentOrNext);
			}
		}

		/// Compute the height of the binary tree in O(N) time. Should not be
		/// called often.
		public int GetHeight()
		{
			using (m_rwLock.AcquireSharedUsing())
			{
				if (m_root == -1)
				{
					return 0;
				}
				return m_nodes[m_root].Height;
			}
		}

		public int Balance(int iA)
		{
			DynamicTreeNode dynamicTreeNode = m_nodes[iA];
			if (dynamicTreeNode.IsLeaf() || dynamicTreeNode.Height < 2)
			{
				return iA;
			}
			int child = dynamicTreeNode.Child1;
			int child2 = dynamicTreeNode.Child2;
			DynamicTreeNode dynamicTreeNode2 = m_nodes[child];
			DynamicTreeNode dynamicTreeNode3 = m_nodes[child2];
			int num = dynamicTreeNode3.Height - dynamicTreeNode2.Height;
			if (num > 1)
			{
				int child3 = dynamicTreeNode3.Child1;
				int child4 = dynamicTreeNode3.Child2;
				DynamicTreeNode dynamicTreeNode4 = m_nodes[child3];
				DynamicTreeNode dynamicTreeNode5 = m_nodes[child4];
				dynamicTreeNode3.Child1 = iA;
				dynamicTreeNode3.ParentOrNext = dynamicTreeNode.ParentOrNext;
				dynamicTreeNode.ParentOrNext = child2;
				if (dynamicTreeNode3.ParentOrNext != -1)
				{
					if (m_nodes[dynamicTreeNode3.ParentOrNext].Child1 == iA)
					{
						m_nodes[dynamicTreeNode3.ParentOrNext].Child1 = child2;
					}
					else
					{
						m_nodes[dynamicTreeNode3.ParentOrNext].Child2 = child2;
					}
				}
				else
				{
					m_root = child2;
				}
				if (dynamicTreeNode4.Height > dynamicTreeNode5.Height)
				{
					dynamicTreeNode3.Child2 = child3;
					dynamicTreeNode.Child2 = child4;
					dynamicTreeNode5.ParentOrNext = iA;
					BoundingBox.CreateMerged(ref dynamicTreeNode2.Aabb, ref dynamicTreeNode5.Aabb, out dynamicTreeNode.Aabb);
					BoundingBox.CreateMerged(ref dynamicTreeNode.Aabb, ref dynamicTreeNode4.Aabb, out dynamicTreeNode3.Aabb);
					dynamicTreeNode.Height = 1 + Math.Max(dynamicTreeNode2.Height, dynamicTreeNode5.Height);
					dynamicTreeNode3.Height = 1 + Math.Max(dynamicTreeNode.Height, dynamicTreeNode4.Height);
				}
				else
				{
					dynamicTreeNode3.Child2 = child4;
					dynamicTreeNode.Child2 = child3;
					dynamicTreeNode4.ParentOrNext = iA;
					BoundingBox.CreateMerged(ref dynamicTreeNode2.Aabb, ref dynamicTreeNode4.Aabb, out dynamicTreeNode.Aabb);
					BoundingBox.CreateMerged(ref dynamicTreeNode.Aabb, ref dynamicTreeNode5.Aabb, out dynamicTreeNode3.Aabb);
					dynamicTreeNode.Height = 1 + Math.Max(dynamicTreeNode2.Height, dynamicTreeNode4.Height);
					dynamicTreeNode3.Height = 1 + Math.Max(dynamicTreeNode.Height, dynamicTreeNode5.Height);
				}
				return child2;
			}
			if (num < -1)
			{
				int child5 = dynamicTreeNode2.Child1;
				int child6 = dynamicTreeNode2.Child2;
				DynamicTreeNode dynamicTreeNode6 = m_nodes[child5];
				DynamicTreeNode dynamicTreeNode7 = m_nodes[child6];
				dynamicTreeNode2.Child1 = iA;
				dynamicTreeNode2.ParentOrNext = dynamicTreeNode.ParentOrNext;
				dynamicTreeNode.ParentOrNext = child;
				if (dynamicTreeNode2.ParentOrNext != -1)
				{
					if (m_nodes[dynamicTreeNode2.ParentOrNext].Child1 == iA)
					{
						m_nodes[dynamicTreeNode2.ParentOrNext].Child1 = child;
					}
					else
					{
						m_nodes[dynamicTreeNode2.ParentOrNext].Child2 = child;
					}
				}
				else
				{
					m_root = child;
				}
				if (dynamicTreeNode6.Height > dynamicTreeNode7.Height)
				{
					dynamicTreeNode2.Child2 = child5;
					dynamicTreeNode.Child1 = child6;
					dynamicTreeNode7.ParentOrNext = iA;
					BoundingBox.CreateMerged(ref dynamicTreeNode3.Aabb, ref dynamicTreeNode7.Aabb, out dynamicTreeNode.Aabb);
					BoundingBox.CreateMerged(ref dynamicTreeNode.Aabb, ref dynamicTreeNode6.Aabb, out dynamicTreeNode2.Aabb);
					dynamicTreeNode.Height = 1 + Math.Max(dynamicTreeNode3.Height, dynamicTreeNode7.Height);
					dynamicTreeNode2.Height = 1 + Math.Max(dynamicTreeNode.Height, dynamicTreeNode6.Height);
				}
				else
				{
					dynamicTreeNode2.Child2 = child6;
					dynamicTreeNode.Child1 = child5;
					dynamicTreeNode6.ParentOrNext = iA;
					BoundingBox.CreateMerged(ref dynamicTreeNode3.Aabb, ref dynamicTreeNode6.Aabb, out dynamicTreeNode.Aabb);
					BoundingBox.CreateMerged(ref dynamicTreeNode.Aabb, ref dynamicTreeNode7.Aabb, out dynamicTreeNode2.Aabb);
					dynamicTreeNode.Height = 1 + Math.Max(dynamicTreeNode3.Height, dynamicTreeNode6.Height);
					dynamicTreeNode2.Height = 1 + Math.Max(dynamicTreeNode.Height, dynamicTreeNode7.Height);
				}
				return child;
			}
			return iA;
		}

		public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, List<T> elementsList, bool clear = true)
		{
			OverlapAllFrustum(ref frustum, elementsList, 0u, clear);
		}

		public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, List<T> elementsList, uint requiredFlags, bool clear = true)
		{
			if (clear)
			{
				elementsList.Clear();
			}
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							frustum.Contains(ref dynamicTreeNode.Aabb, out ContainmentType result);
							switch (result)
							{
							case ContainmentType.Contains:
							{
								int count = stack.Count;
								stack.Push(num);
								while (stack.Count > count)
								{
									int num2 = stack.Pop();
									DynamicTreeNode dynamicTreeNode2 = m_nodes[num2];
									if (dynamicTreeNode2.IsLeaf())
									{
										if ((GetUserFlag(num2) & requiredFlags) == requiredFlags)
										{
											elementsList.Add(GetUserData<T>(num2));
										}
									}
									else
									{
										if (dynamicTreeNode2.Child1 != -1)
										{
											stack.Push(dynamicTreeNode2.Child1);
										}
										if (dynamicTreeNode2.Child2 != -1)
										{
											stack.Push(dynamicTreeNode2.Child2);
										}
									}
								}
								break;
							}
							case ContainmentType.Intersects:
								if (dynamicTreeNode.IsLeaf())
								{
									if ((GetUserFlag(num) & requiredFlags) == requiredFlags)
									{
										elementsList.Add(GetUserData<T>(num));
									}
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
								break;
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, List<T> elementsList, List<bool> isInsideList, bool clear = true)
		{
			if (clear)
			{
				elementsList.Clear();
				isInsideList.Clear();
			}
			OverlapAllFrustum(ref frustum, delegate(T x, bool y)
			{
				elementsList.Add(x);
				isInsideList.Add(y);
			});
		}

		public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, Action<T, bool> add)
		{
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							frustum.Contains(ref dynamicTreeNode.Aabb, out ContainmentType result);
							switch (result)
							{
							case ContainmentType.Contains:
							{
								int count = stack.Count;
								stack.Push(num);
								while (stack.Count > count)
								{
									int num2 = stack.Pop();
									DynamicTreeNode dynamicTreeNode2 = m_nodes[num2];
									if (dynamicTreeNode2.IsLeaf())
									{
										add(GetUserData<T>(num2), arg2: true);
									}
									else
									{
										if (dynamicTreeNode2.Child1 != -1)
										{
											stack.Push(dynamicTreeNode2.Child1);
										}
										if (dynamicTreeNode2.Child2 != -1)
										{
											stack.Push(dynamicTreeNode2.Child2);
										}
									}
								}
								break;
							}
							case ContainmentType.Intersects:
								if (dynamicTreeNode.IsLeaf())
								{
									add(GetUserData<T>(num), arg2: false);
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
								break;
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllFrustum<T, Op>(ref BoundingFrustum frustum, ref Op add) where Op : struct, AddOp<T>
		{
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							frustum.Contains(ref dynamicTreeNode.Aabb, out ContainmentType result);
							switch (result)
							{
							case ContainmentType.Contains:
							{
								int count = stack.Count;
								stack.Push(num);
								while (stack.Count > count)
								{
									int num2 = stack.Pop();
									DynamicTreeNode dynamicTreeNode2 = m_nodes[num2];
									if (dynamicTreeNode2.IsLeaf())
									{
										add.Add(GetUserData<T>(num2), contained: true);
									}
									else
									{
										if (dynamicTreeNode2.Child1 != -1)
										{
											stack.Push(dynamicTreeNode2.Child1);
										}
										if (dynamicTreeNode2.Child2 != -1)
										{
											stack.Push(dynamicTreeNode2.Child2);
										}
									}
								}
								break;
							}
							case ContainmentType.Intersects:
								if (dynamicTreeNode.IsLeaf())
								{
									add.Add(GetUserData<T>(num), contained: false);
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
								break;
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, List<T> elementsList, List<bool> isInsideList, float tSqr, bool clear = true)
		{
			if (clear)
			{
				elementsList.Clear();
				isInsideList.Clear();
			}
			OverlapAllFrustum(ref frustum, delegate(T x, bool y)
			{
				elementsList.Add(x);
				isInsideList.Add(y);
			}, tSqr);
		}

		public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, Action<T, bool> add, float tSqr)
		{
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							frustum.Contains(ref dynamicTreeNode.Aabb, out ContainmentType result);
							switch (result)
							{
							case ContainmentType.Contains:
							{
								int count = stack.Count;
								stack.Push(num);
								while (stack.Count > count)
								{
									int num2 = stack.Pop();
									DynamicTreeNode dynamicTreeNode2 = m_nodes[num2];
									if (dynamicTreeNode2.IsLeaf())
									{
										if (dynamicTreeNode.Aabb.Size.LengthSquared() > tSqr)
										{
											add(GetUserData<T>(num2), arg2: true);
										}
									}
									else
									{
										if (dynamicTreeNode2.Child1 != -1)
										{
											stack.Push(dynamicTreeNode2.Child1);
										}
										if (dynamicTreeNode2.Child2 != -1)
										{
											stack.Push(dynamicTreeNode2.Child2);
										}
									}
								}
								break;
							}
							case ContainmentType.Intersects:
								if (dynamicTreeNode.IsLeaf())
								{
									if (dynamicTreeNode.Aabb.Size.LengthSquared() > tSqr)
									{
										add(GetUserData<T>(num), arg2: false);
									}
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
								break;
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllFrustum<T, Op>(ref BoundingFrustum frustum, float tSqr, ref Op add) where Op : struct, AddOp<T>
		{
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							frustum.Contains(ref dynamicTreeNode.Aabb, out ContainmentType result);
							switch (result)
							{
							case ContainmentType.Contains:
							{
								int count = stack.Count;
								stack.Push(num);
								while (stack.Count > count)
								{
									int num2 = stack.Pop();
									DynamicTreeNode dynamicTreeNode2 = m_nodes[num2];
									if (dynamicTreeNode2.IsLeaf())
									{
										if (dynamicTreeNode.Aabb.Size.LengthSquared() > tSqr)
										{
											add.Add(GetUserData<T>(num2), contained: true);
										}
									}
									else
									{
										if (dynamicTreeNode2.Child1 != -1)
										{
											stack.Push(dynamicTreeNode2.Child1);
										}
										if (dynamicTreeNode2.Child2 != -1)
										{
											stack.Push(dynamicTreeNode2.Child2);
										}
									}
								}
								break;
							}
							case ContainmentType.Intersects:
								if (dynamicTreeNode.IsLeaf())
								{
									if (dynamicTreeNode.Aabb.Size.LengthSquared() > tSqr)
									{
										add.Add(GetUserData<T>(num), contained: false);
									}
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
								break;
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllFrustumConservative<T>(ref BoundingFrustum frustum, List<T> elementsList, uint requiredFlags, bool clear = true)
		{
			if (clear)
			{
				elementsList.Clear();
			}
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					BoundingBox box = BoundingBox.CreateInvalid();
					box.Include(ref frustum);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							if (dynamicTreeNode.Aabb.Intersects(box))
							{
								frustum.Contains(ref dynamicTreeNode.Aabb, out ContainmentType result);
								switch (result)
								{
								case ContainmentType.Contains:
								{
									int count = stack.Count;
									stack.Push(num);
									while (stack.Count > count)
									{
										int num2 = stack.Pop();
										DynamicTreeNode dynamicTreeNode2 = m_nodes[num2];
										if (dynamicTreeNode2.IsLeaf())
										{
											if ((GetUserFlag(num2) & requiredFlags) == requiredFlags)
											{
												elementsList.Add(GetUserData<T>(num2));
											}
										}
										else
										{
											if (dynamicTreeNode2.Child1 != -1)
											{
												stack.Push(dynamicTreeNode2.Child1);
											}
											if (dynamicTreeNode2.Child2 != -1)
											{
												stack.Push(dynamicTreeNode2.Child2);
											}
										}
									}
									break;
								}
								case ContainmentType.Intersects:
									if (dynamicTreeNode.IsLeaf())
									{
										if ((GetUserFlag(num) & requiredFlags) == requiredFlags)
										{
											elementsList.Add(GetUserData<T>(num));
										}
									}
									else
									{
										stack.Push(dynamicTreeNode.Child1);
										stack.Push(dynamicTreeNode.Child2);
									}
									break;
								}
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllFrustumAny<T>(ref BoundingFrustum frustum, List<T> elementsList, bool clear = true)
		{
			if (clear)
			{
				elementsList.Clear();
			}
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							frustum.Contains(ref dynamicTreeNode.Aabb, out ContainmentType result);
							switch (result)
							{
							case ContainmentType.Contains:
							{
								int count = stack.Count;
								stack.Push(num);
								while (stack.Count > count)
								{
									int num2 = stack.Pop();
									DynamicTreeNode dynamicTreeNode2 = m_nodes[num2];
									if (dynamicTreeNode2.IsLeaf())
									{
										T userData2 = GetUserData<T>(num2);
										elementsList.Add(userData2);
									}
									else
									{
										if (dynamicTreeNode2.Child1 != -1)
										{
											stack.Push(dynamicTreeNode2.Child1);
										}
										if (dynamicTreeNode2.Child2 != -1)
										{
											stack.Push(dynamicTreeNode2.Child2);
										}
									}
								}
								break;
							}
							case ContainmentType.Intersects:
								if (dynamicTreeNode.IsLeaf())
								{
									T userData = GetUserData<T>(num);
									elementsList.Add(userData);
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
								break;
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllLineSegment<T>(ref Line line, List<MyLineSegmentOverlapResult<T>> elementsList)
		{
			OverlapAllLineSegment(ref line, elementsList, 0u);
		}

		public void OverlapAllLineSegment<T>(ref Line line, List<MyLineSegmentOverlapResult<T>> elementsList, uint requiredFlags)
		{
			elementsList.Clear();
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					BoundingBox box = BoundingBox.CreateInvalid();
					box.Include(ref line);
					Ray ray = new Ray(line.From, line.Direction);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							if (dynamicTreeNode.Aabb.Intersects(box))
							{
								float? num2 = dynamicTreeNode.Aabb.Intersects(ray);
								if (num2.HasValue && num2.Value <= line.Length && num2.Value >= 0f)
								{
									if (dynamicTreeNode.IsLeaf())
									{
										if ((GetUserFlag(num) & requiredFlags) == requiredFlags)
										{
											elementsList.Add(new MyLineSegmentOverlapResult<T>
											{
												Element = GetUserData<T>(num),
												Distance = num2.Value
											});
										}
									}
									else
									{
										stack.Push(dynamicTreeNode.Child1);
										stack.Push(dynamicTreeNode.Child2);
									}
								}
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllBoundingBox<T>(ref BoundingBox bbox, List<T> elementsList, uint requiredFlags = 0u, bool clear = true)
		{
			if (clear)
			{
				elementsList.Clear();
			}
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							if (dynamicTreeNode.Aabb.Intersects(bbox))
							{
								if (dynamicTreeNode.IsLeaf())
								{
									if ((GetUserFlag(num) & requiredFlags) == requiredFlags)
									{
										elementsList.Add(GetUserData<T>(num));
									}
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public bool OverlapsAnyLeafBoundingBox(ref BoundingBox bbox)
		{
			if (m_root == -1)
			{
				return false;
			}
			using (m_rwLock.AcquireSharedUsing())
			{
				Stack<int> stack = GetStack();
				stack.Push(m_root);
				while (stack.Count > 0)
				{
					int num = stack.Pop();
					if (num != -1)
					{
						DynamicTreeNode dynamicTreeNode = m_nodes[num];
						if (dynamicTreeNode.Aabb.Intersects(bbox))
						{
							if (dynamicTreeNode.IsLeaf())
							{
								return true;
							}
							stack.Push(dynamicTreeNode.Child1);
							stack.Push(dynamicTreeNode.Child2);
						}
					}
				}
				PushStack(stack);
			}
			return false;
		}

		/// Use the tree to produce a list of clusters aproximatelly the requested size, while intersecting those with the provided bounding box.
		public void OverlapSizeableClusters(ref BoundingBox bbox, List<BoundingBox> boundList, double minSize)
		{
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							if (dynamicTreeNode.Aabb.Intersects(bbox))
							{
								if (dynamicTreeNode.IsLeaf() || (double)dynamicTreeNode.Aabb.Size.Max() <= minSize)
								{
									boundList.Add(dynamicTreeNode.Aabb);
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void OverlapAllBoundingSphere<T>(ref BoundingSphere sphere, List<T> overlapElementsList, bool clear = true)
		{
			if (clear)
			{
				overlapElementsList.Clear();
			}
			if (m_root != -1)
			{
				using (m_rwLock.AcquireSharedUsing())
				{
					Stack<int> stack = GetStack();
					stack.Push(m_root);
					while (stack.Count > 0)
					{
						int num = stack.Pop();
						if (num != -1)
						{
							DynamicTreeNode dynamicTreeNode = m_nodes[num];
							if (dynamicTreeNode.Aabb.Intersects(sphere))
							{
								if (dynamicTreeNode.IsLeaf())
								{
									overlapElementsList.Add(GetUserData<T>(num));
								}
								else
								{
									stack.Push(dynamicTreeNode.Child1);
									stack.Push(dynamicTreeNode.Child2);
								}
							}
						}
					}
					PushStack(stack);
				}
			}
		}

		public void GetAll<T>(List<T> elementsList, bool clear, List<BoundingBox> boxsList = null)
		{
			if (clear)
			{
				elementsList.Clear();
				boxsList?.Clear();
			}
			using (m_rwLock.AcquireSharedUsing())
			{
				foreach (KeyValuePair<int, DynamicTreeNode> item in m_leafElementCache)
				{
					elementsList.Add((T)item.Value.UserData);
				}
				if (boxsList != null)
				{
					foreach (KeyValuePair<int, DynamicTreeNode> item2 in m_leafElementCache)
					{
						boxsList.Add(item2.Value.Aabb);
					}
				}
			}
		}

		public void GetAllNodeBounds(List<BoundingBox> boxsList)
		{
			using (m_rwLock.AcquireSharedUsing())
			{
				int i = 0;
				int num = 0;
				for (; i < m_nodeCapacity; i++)
				{
					if (num >= m_nodeCount)
					{
						break;
					}
					if (m_nodes[i].Height != -1)
					{
						num++;
						boxsList.Add(m_nodes[i].Aabb);
					}
				}
			}
		}

		private void ResetNodes()
		{
			m_leafElementCache.Clear();
			m_root = -1;
			m_nodeCount = 0;
			for (int i = 0; i < m_nodeCapacity - 1; i++)
			{
				m_nodes[i].ParentOrNext = i + 1;
				m_nodes[i].Height = 1;
				m_nodes[i].UserData = null;
			}
			m_nodes[m_nodeCapacity - 1].ParentOrNext = -1;
			m_nodes[m_nodeCapacity - 1].Height = 1;
			m_freeList = 0;
		}

		public void Clear()
		{
			using (m_rwLock.AcquireExclusiveUsing())
			{
				if (m_nodeCapacity < 256 || m_nodeCapacity > 512)
				{
					m_nodeCapacity = 256;
					m_nodes = new DynamicTreeNode[m_nodeCapacity];
					m_leafElementCache = new Dictionary<int, DynamicTreeNode>(m_nodeCapacity / 4);
					for (int i = 0; i < m_nodeCapacity; i++)
					{
						m_nodes[i] = new DynamicTreeNode();
					}
				}
				ResetNodes();
			}
		}

		public static void Dispose()
		{
			lock (m_StackCacheCollection)
			{
				m_StackCacheCollection.Clear();
			}
		}
	}
}
