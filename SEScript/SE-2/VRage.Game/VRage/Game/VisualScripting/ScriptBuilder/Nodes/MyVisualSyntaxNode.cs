using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxNode
	{
		protected struct HeapNodeWrapper
		{
			public MyVisualSyntaxNode Node;
		}

		/// <summary>
		/// Depth of the node in the graph.
		/// </summary>
		internal int Depth = int.MaxValue;

		/// <summary>
		/// Nodes that got referenced more than once at one syntax level.
		/// </summary>
		internal HashSet<MyVisualSyntaxNode> SubTreeNodes = new HashSet<MyVisualSyntaxNode>();

		/// <summary>
		/// Data container;
		/// </summary>
		protected MyObjectBuilder_ScriptNode m_objectBuilder;

		private static readonly MyBinaryStructHeap<int, HeapNodeWrapper> m_activeHeap = new MyBinaryStructHeap<int, HeapNodeWrapper>();

		private static readonly HashSet<MyVisualSyntaxNode> m_commonParentSet = new HashSet<MyVisualSyntaxNode>();

		private static readonly HashSet<MyVisualSyntaxNode> m_sequenceHelper = new HashSet<MyVisualSyntaxNode>();

		/// <summary>
		/// Tells if the node was already preprocessed.
		/// (default value is false)
		/// </summary>
		protected bool Preprocessed
		{
			get;
			set;
		}

		/// <summary>
		/// Tells whenever the node has sequence or not.
		/// </summary>
		internal virtual bool SequenceDependent => true;

		/// <summary>
		/// Is getting set to true the first time the syntax from this node is collected.
		/// Prevents duplicities in syntax.
		/// </summary>
		internal bool Collected
		{
			get;
			private set;
		}

		/// <summary>
		/// List of sequence input nodes connected to this one.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> SequenceInputs
		{
			get;
			private set;
		}

		/// <summary>
		/// List of sequence output nodes connected to this one.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> SequenceOutputs
		{
			get;
			private set;
		}

		/// <summary>
		/// Output nodes.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> Outputs
		{
			get;
			private set;
		}

		/// <summary>
		/// Input Nodes.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> Inputs
		{
			get;
			private set;
		}

		/// <summary>
		/// Data container getter.
		/// </summary>
		public MyObjectBuilder_ScriptNode ObjectBuilder => m_objectBuilder;

		/// <summary>
		/// Member used for in-graph navigation.
		/// </summary>
		internal MyVisualScriptNavigator Navigator
		{
			get;
			set;
		}

		/// <summary>
		/// Resets nodes to state when they are ready for new run of the builder.
		/// </summary>
		internal virtual void Reset()
		{
			Depth = int.MaxValue;
			SubTreeNodes.Clear();
			Inputs.Clear();
			Outputs.Clear();
			SequenceOutputs.Clear();
			SequenceInputs.Clear();
			Collected = false;
			Preprocessed = false;
		}

		/// <summary>
		/// Unique identifier within class syntax.
		/// </summary>
		protected internal virtual string VariableSyntaxName(string variableIdentifier = null)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns ordered set of expressions from value suppliers.
		/// </summary>
		/// <returns></returns>
		internal virtual void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			Collected = true;
			foreach (MyVisualSyntaxNode subTreeNode in SubTreeNodes)
			{
				if (!subTreeNode.Collected)
				{
					subTreeNode.CollectInputExpressions(expressions);
				}
			}
		}

		/// <summary>
		/// Returns ordered set of all expressions.
		/// </summary>
		/// <param name="expressions"></param>
		internal virtual void CollectSequenceExpressions(List<StatementSyntax> expressions)
		{
			CollectInputExpressions(expressions);
			foreach (MyVisualSyntaxNode sequenceOutput in SequenceOutputs)
			{
				sequenceOutput.CollectSequenceExpressions(expressions);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ob">Should never be a base of _scriptNode.</param>
		internal MyVisualSyntaxNode(MyObjectBuilder_ScriptNode ob)
		{
			m_objectBuilder = ob;
			Inputs = new List<MyVisualSyntaxNode>();
			Outputs = new List<MyVisualSyntaxNode>();
			SequenceInputs = new List<MyVisualSyntaxNode>();
			SequenceOutputs = new List<MyVisualSyntaxNode>();
		}

		/// <summary>
		/// Pre-generation process that loads necessary data in derived nodes
		/// and adjusts the internal state of graph to generation ready state.
		/// </summary>
		/// <param name="currentDepth"></param>
		protected internal virtual void Preprocess(int currentDepth)
		{
			if (currentDepth < Depth)
			{
				Depth = currentDepth;
			}
			if (!Preprocessed)
			{
				foreach (MyVisualSyntaxNode sequenceOutput in SequenceOutputs)
				{
					sequenceOutput.Preprocess(Depth + 1);
				}
			}
			foreach (MyVisualSyntaxNode input in Inputs)
			{
				if (!input.SequenceDependent)
				{
					input.Preprocess(Depth);
				}
			}
			if (!SequenceDependent && !Preprocessed)
			{
				if (Outputs.Count == 1 && !Outputs[0].SequenceDependent)
				{
					Outputs[0].SubTreeNodes.Add(this);
				}
				else if (Outputs.Count > 0)
				{
					Navigator.FreshNodes.Add(this);
				}
			}
			Preprocessed = true;
		}

		/// <summary>
		/// Tries to put the node of id into collection.
		/// </summary>
		/// <param name="nodeID">Id of looked for node.</param>
		/// <param name="collection">Target collection.</param>
		protected MyVisualSyntaxNode TryRegisterNode(int nodeID, List<MyVisualSyntaxNode> collection)
		{
			if (nodeID == -1)
			{
				return null;
			}
			MyVisualSyntaxNode nodeByID = Navigator.GetNodeByID(nodeID);
			if (nodeByID != null)
			{
				collection.Add(nodeByID);
			}
			return nodeByID;
		}

		/// <summary>
		/// Method that is needed for Hashing purposes.
		/// Id should be a unique identifier within a graph.
		/// </summary>
		/// <returns>Id of node.</returns>
		public override int GetHashCode()
		{
			if (ObjectBuilder == null)
			{
				return GetType().GetHashCode();
			}
			return ObjectBuilder.ID;
		}

		protected static MyVisualSyntaxNode CommonParent(IEnumerable<MyVisualSyntaxNode> nodes)
		{
			m_commonParentSet.Clear();
			m_activeHeap.Clear();
			foreach (MyVisualSyntaxNode node in nodes)
			{
				if (m_commonParentSet.Add(node))
				{
					m_activeHeap.Insert(new HeapNodeWrapper
					{
						Node = node
					}, -node.Depth);
				}
			}
			HeapNodeWrapper current;
			while (true)
			{
				current = m_activeHeap.RemoveMin();
				if (m_activeHeap.Count == 0)
				{
					break;
				}
				if (current.Node.SequenceInputs.Count == 0)
				{
					if (m_activeHeap.Count > 0)
					{
						return null;
					}
				}
				else
				{
					current.Node.SequenceInputs.ForEach(delegate(MyVisualSyntaxNode node)
					{
						if (m_activeHeap.Count > 0 && m_commonParentSet.Add(node))
						{
							current.Node = node;
							m_activeHeap.Insert(current, -current.Node.Depth);
						}
					});
				}
			}
			if (current.Node is MyVisualSyntaxForLoopNode)
			{
				return current.Node.SequenceInputs.FirstOrDefault();
			}
			return current.Node;
		}

		public IEnumerable<MyVisualSyntaxNode> GetSequenceDependentOutputs()
		{
			m_sequenceHelper.Clear();
			SequenceDependentChildren(m_sequenceHelper);
			return m_sequenceHelper;
		}

		private void SequenceDependentChildren(HashSet<MyVisualSyntaxNode> results)
		{
			if (Outputs.Count != 0 && Depth != int.MaxValue)
			{
				foreach (MyVisualSyntaxNode output in Outputs)
				{
					if (output.Depth != int.MaxValue)
					{
						if (output.SequenceDependent)
						{
							results.Add(output);
						}
						else
						{
							output.SequenceDependentChildren(results);
						}
					}
				}
			}
		}
	}
}
