using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;

namespace VRage.Groups
{
	public class MyGroups<TNode, TGroupData> : MyGroupsBase<TNode> where TNode : class where TGroupData : IGroupData<TNode>, new()
	{
		/// <summary>
		/// Return true when "major" is really major group, otherwise false.
		/// </summary>
		public delegate bool MajorGroupComparer(Group major, Group minor);

		public class Node
		{
			private Group m_currentGroup;

			internal TNode m_node;

			internal readonly SortedDictionary<long, Node> m_children = new SortedDictionary<long, Node>();

			internal readonly Dictionary<long, Node> m_parents = new Dictionary<long, Node>();

			internal Group m_group
			{
				get
				{
					return m_currentGroup;
				}
				set
				{
					Group currentGroup = m_currentGroup;
					m_currentGroup = null;
					if (currentGroup != null)
					{
						TGroupData groupData = currentGroup.GroupData;
						groupData.OnNodeRemoved(m_node);
					}
					m_currentGroup = value;
					if (m_currentGroup != null)
					{
						TGroupData groupData = m_currentGroup.GroupData;
						groupData.OnNodeAdded(m_node);
					}
				}
			}

			public int LinkCount => m_children.Count + m_parents.Count;

			public TNode NodeData => m_node;

			public Group Group => m_group;

			public int ChainLength
			{
				get;
				set;
			}

			public SortedDictionaryValuesReader<long, Node> Children => new SortedDictionaryValuesReader<long, Node>(m_children);

			public SortedDictionaryReader<long, Node> ChildLinks => new SortedDictionaryReader<long, Node>(m_children);

			public DictionaryReader<long, Node> ParentLinks => new DictionaryReader<long, Node>(m_parents);

			public override string ToString()
			{
				return m_node.ToString();
			}
		}

		public class Group
		{
			internal readonly HashSet<Node> m_members = new HashSet<Node>();

			public readonly TGroupData GroupData = new TGroupData();

			public HashSetReader<Node> Nodes => new HashSetReader<Node>(m_members);
		}

		private Stack<Group> m_groupPool = new Stack<Group>(32);

		private Stack<Node> m_nodePool = new Stack<Node>(32);

		private Dictionary<TNode, Node> m_nodes = new Dictionary<TNode, Node>();

		private HashSet<Group> m_groups = new HashSet<Group>();

		private HashSet<Node> m_disconnectHelper = new HashSet<Node>();

		private MajorGroupComparer m_groupSelector;

		private bool m_isRecalculating;

		private HashSet<Node> m_tmpClosed = new HashSet<Node>();

		private Queue<Node> m_tmpOpen = new Queue<Node>();

		private List<Node> m_tmpList = new List<Node>();

		/// <summary>
		/// When true, groups with one member are supported.
		/// You can use AddNode and RemoveNode.
		/// You have to manually call RemoveNode!
		/// </summary>
		public bool SupportsOphrans
		{
			get;
			protected set;
		}

		protected bool SupportsChildToChild
		{
			get;
			set;
		}

		public HashSetReader<Group> Groups => new HashSetReader<Group>(m_groups);

		/// <summary>
		/// Initializes a new instance of MyGroups class.
		/// </summary>
		/// <param name="supportOphrans">When true, groups with one member are supported and you have to manually call RemoveNode!</param>
		/// <param name="groupSelector">Major group selector, when merging two groups, major group is preserved. By default it's larger group.</param>
		public MyGroups(bool supportOphrans = false, MajorGroupComparer groupSelector = null)
		{
			SupportsOphrans = supportOphrans;
			m_groupSelector = (groupSelector ?? new MajorGroupComparer(IsMajorGroup));
		}

		public void ApplyOnNodes(Action<TNode, Node> action)
		{
			foreach (KeyValuePair<TNode, Node> node in m_nodes)
			{
				action(node.Key, node.Value);
			}
		}

		public override bool HasSameGroup(TNode a, TNode b)
		{
			Group group = GetGroup(a);
			Group group2 = GetGroup(b);
			if (group != null)
			{
				return group == group2;
			}
			return false;
		}

		public Group GetGroup(TNode Node)
		{
			if (m_nodes.TryGetValue(Node, out Node value))
			{
				return value.m_group;
			}
			return null;
		}

		/// <summary>
		/// Adds node, asserts when node already exists
		/// </summary>
		public override void AddNode(TNode nodeToAdd)
		{
			if (!SupportsOphrans)
			{
				throw new InvalidOperationException("Cannot add/remove node when ophrans are not supported");
			}
			Node orCreateNode = GetOrCreateNode(nodeToAdd);
			if (orCreateNode.m_group == null)
			{
				orCreateNode.m_group = AcquireGroup();
				orCreateNode.m_group.m_members.Add(orCreateNode);
			}
		}

		/// <summary>
		/// Removes node, asserts when node is not here or node has some existing links
		/// </summary>
		public override void RemoveNode(TNode nodeToRemove)
		{
			if (!SupportsOphrans)
			{
				throw new InvalidOperationException("Cannot add/remove node when ophrans are not supported");
			}
			if (m_nodes.TryGetValue(nodeToRemove, out Node value))
			{
				BreakAllLinks(value);
				bool flag = TryReleaseNode(value);
			}
		}

		private void BreakAllLinks(Node node)
		{
			while (node.m_parents.Count > 0)
			{
				Dictionary<long, Node>.Enumerator enumerator = node.m_parents.GetEnumerator();
				enumerator.MoveNext();
				KeyValuePair<long, Node> current = enumerator.Current;
				BreakLinkInternal(current.Key, current.Value, node);
			}
			while (node.m_children.Count > 0)
			{
				SortedDictionary<long, Node>.Enumerator enumerator2 = node.m_children.GetEnumerator();
				enumerator2.MoveNext();
				KeyValuePair<long, Node> current2 = enumerator2.Current;
				BreakLinkInternal(current2.Key, node, current2.Value);
			}
		}

		/// <summary>
		/// Creates link between parent and child.
		/// Parent is owner of constraint.
		/// LinkId must be unique for parent and for child; LinkId is unique node-node identifier.
		/// </summary>
		public override void CreateLink(long linkId, TNode parentNode, TNode childNode)
		{
			Node orCreateNode = GetOrCreateNode(parentNode);
			Node orCreateNode2 = GetOrCreateNode(childNode);
			if (orCreateNode.m_group != null && orCreateNode2.m_group != null)
			{
				if (orCreateNode.m_group == orCreateNode2.m_group)
				{
					AddLink(linkId, orCreateNode, orCreateNode2);
					return;
				}
				MergeGroups(orCreateNode.m_group, orCreateNode2.m_group);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
			else if (orCreateNode.m_group != null)
			{
				orCreateNode2.m_group = orCreateNode.m_group;
				orCreateNode.m_group.m_members.Add(orCreateNode2);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
			else if (orCreateNode2.m_group != null)
			{
				orCreateNode.m_group = orCreateNode2.m_group;
				orCreateNode2.m_group.m_members.Add(orCreateNode);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
			else
			{
				Group group2 = orCreateNode.m_group = AcquireGroup();
				group2.m_members.Add(orCreateNode);
				orCreateNode2.m_group = group2;
				group2.m_members.Add(orCreateNode2);
				AddLink(linkId, orCreateNode, orCreateNode2);
			}
		}

		/// <summary>
		/// Breaks link between parent and child, you can set child to null to find it by linkId.
		/// Returns true when link was removed, returns false when link was not found.
		/// </summary>
		public override bool BreakLink(long linkId, TNode parentNode, TNode childNode = null)
		{
			if (m_nodes.TryGetValue(parentNode, out Node value) && value.m_children.TryGetValue(linkId, out Node value2))
			{
				return BreakLinkInternal(linkId, value, value2);
			}
			return false;
		}

		public void BreakAllLinks(TNode node)
		{
			if (m_nodes.TryGetValue(node, out Node value))
			{
				BreakAllLinks(value);
			}
		}

		public Node GetNode(TNode node)
		{
			return m_nodes.GetValueOrDefault(node);
		}

		public override bool LinkExists(long linkId, TNode parentNode, TNode childNode = null)
		{
			if (m_nodes.TryGetValue(parentNode, out Node value) && value.m_children.TryGetValue(linkId, out Node value2))
			{
				if (childNode == null)
				{
					return true;
				}
				return childNode == value2.m_node;
			}
			return false;
		}

		private bool BreakLinkInternal(long linkId, Node parent, Node child)
		{
			bool flag = parent.m_children.Remove(linkId);
			flag &= child.m_parents.Remove(linkId);
			if (!flag && SupportsChildToChild)
			{
				flag &= child.m_children.Remove(linkId);
			}
			RecalculateConnectivity(parent, child);
			return flag;
		}

		[Conditional("DEBUG")]
		private void DebugCheckConsistency(long linkId, Node parent, Node child)
		{
		}

		private void AddNeighbours(HashSet<Node> nodes, Node nodeToAdd)
		{
			if (!nodes.Contains(nodeToAdd))
			{
				nodes.Add(nodeToAdd);
				foreach (KeyValuePair<long, Node> child in nodeToAdd.m_children)
				{
					AddNeighbours(nodes, child.Value);
				}
				foreach (KeyValuePair<long, Node> parent in nodeToAdd.m_parents)
				{
					AddNeighbours(nodes, parent.Value);
				}
			}
		}

		/// <summary>
		/// Returns true when node was released completely and returned to pool.
		/// </summary>
		private bool TryReleaseNode(Node node)
		{
			if (node.m_node != null && node.m_group != null && node.m_children.Count == 0 && node.m_parents.Count == 0)
			{
				Group group = node.m_group;
				node.m_group.m_members.Remove(node);
				m_nodes.Remove(node.m_node);
				node.m_group = null;
				node.m_node = null;
				ReturnNode(node);
				if (group.m_members.Count == 0)
				{
					ReturnGroup(group);
				}
				return true;
			}
			return false;
		}

		private void RecalculateConnectivity(Node parent, Node child)
		{
			if (!m_isRecalculating && parent != null && parent.Group != null && child != null && child.Group != null)
			{
				try
				{
					m_isRecalculating = true;
					if (SupportsOphrans || (!TryReleaseNode(parent) & !TryReleaseNode(child)))
					{
						AddNeighbours(m_disconnectHelper, parent);
						if (!m_disconnectHelper.Contains(child))
						{
							if ((float)m_disconnectHelper.Count > (float)parent.Group.m_members.Count / 2f)
							{
								foreach (Node member in parent.Group.m_members)
								{
									if (!m_disconnectHelper.Add(member))
									{
										m_disconnectHelper.Remove(member);
									}
								}
							}
							Group group = AcquireGroup();
							foreach (Node item in m_disconnectHelper)
							{
								if (item.m_group != null && item.m_group.m_members != null)
								{
									bool flag = item.m_group.m_members.Remove(item);
									item.m_group = group;
									group.m_members.Add(item);
								}
							}
						}
					}
				}
				finally
				{
					m_disconnectHelper.Clear();
					m_isRecalculating = false;
				}
			}
		}

		public static bool IsMajorGroup(Group groupA, Group groupB)
		{
			return groupA.m_members.Count >= groupB.m_members.Count;
		}

		private void MergeGroups(Group groupA, Group groupB)
		{
			if (!m_groupSelector(groupA, groupB))
			{
				Group group = groupA;
				groupA = groupB;
				groupB = group;
			}
			if (m_tmpList.Capacity < groupB.m_members.Count)
			{
				m_tmpList.Capacity = groupB.m_members.Count;
			}
			m_tmpList.AddRange(groupB.m_members);
			foreach (Node tmp in m_tmpList)
			{
				groupB.m_members.Remove(tmp);
				tmp.m_group = groupA;
				groupA.m_members.Add(tmp);
			}
			m_tmpList.Clear();
			groupB.m_members.Clear();
			ReturnGroup(groupB);
		}

		private void AddLink(long linkId, Node parent, Node child)
		{
			parent.m_children[linkId] = child;
			child.m_parents[linkId] = parent;
		}

		private Node GetOrCreateNode(TNode nodeData)
		{
			if (!m_nodes.TryGetValue(nodeData, out Node value))
			{
				value = AcquireNode();
				value.m_node = nodeData;
				m_nodes[nodeData] = value;
			}
			return value;
		}

		private Group GetNodeOrNull(TNode Node)
		{
			m_nodes.TryGetValue(Node, out Node value);
			return value?.m_group;
		}

		private Group AcquireGroup()
		{
			Group group = (m_groupPool.Count > 0) ? m_groupPool.Pop() : new Group();
			m_groups.Add(group);
			TGroupData groupData = group.GroupData;
			groupData.OnCreate(group);
			return group;
		}

		private void ReturnGroup(Group group)
		{
			TGroupData groupData = group.GroupData;
			groupData.OnRelease();
			m_groups.Remove(group);
			m_groupPool.Push(group);
		}

		private Node AcquireNode()
		{
			return (m_nodePool.Count > 0) ? m_nodePool.Pop() : new Node();
		}

		private void ReturnNode(Node node)
		{
			m_nodePool.Push(node);
		}

		public override List<TNode> GetGroupNodes(TNode nodeInGroup)
		{
			List<TNode> list = null;
			Group group = GetGroup(nodeInGroup);
			if (group != null)
			{
				list = new List<TNode>(group.Nodes.Count);
				{
					foreach (Node node in group.Nodes)
					{
						list.Add(node.NodeData);
					}
					return list;
				}
			}
			list = new List<TNode>(1);
			list.Add(nodeInGroup);
			return list;
		}

		public override void GetGroupNodes(TNode nodeInGroup, List<TNode> result)
		{
			Group group = GetGroup(nodeInGroup);
			if (group != null)
			{
				foreach (Node node in group.Nodes)
				{
					result.Add(node.NodeData);
				}
			}
			else
			{
				result.Add(nodeInGroup);
			}
		}

		public void ReplaceRoot(TNode newRoot)
		{
			Group group = GetGroup(newRoot);
			foreach (Node member in group.m_members)
			{
				foreach (KeyValuePair<long, Node> parent in member.m_parents)
				{
					member.m_children[parent.Key] = parent.Value;
				}
				member.m_parents.Clear();
			}
			Node node = GetNode(newRoot);
			node.ChainLength = 0;
			ReplaceParents(node);
		}

		private void ReplaceParents(Node newParent)
		{
			m_tmpOpen.Enqueue(newParent);
			m_tmpClosed.Add(newParent);
			while (m_tmpOpen.Count > 0)
			{
				Node node = m_tmpOpen.Dequeue();
				foreach (KeyValuePair<long, Node> child in node.m_children)
				{
					child.Value.ChainLength = node.ChainLength + 1;
					if (!m_tmpClosed.Contains(child.Value) && !child.Value.m_parents.ContainsKey(child.Key))
					{
						child.Value.m_parents.Add(child.Key, node);
						child.Value.m_children.Remove(child.Key);
						m_tmpOpen.Enqueue(child.Value);
						m_tmpClosed.Add(child.Value);
					}
				}
			}
			m_tmpOpen.Clear();
			m_tmpClosed.Clear();
		}
	}
}
