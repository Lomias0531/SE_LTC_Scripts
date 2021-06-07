using System.Reflection;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// This node represents a Method signature / entrypoint to method.
	/// Contains only general information about the method signature.
	/// Generates no syntax.
	/// </summary>
	public class MyVisualSyntaxEventNode : MyVisualSyntaxNode, IMyVisualSyntaxEntryPoint
	{
		private MyVisualSyntaxNode m_nextSequenceNode;

		protected FieldInfo m_fieldInfo;

		public new MyObjectBuilder_EventScriptNode ObjectBuilder => (MyObjectBuilder_EventScriptNode)m_objectBuilder;

		public virtual string EventName => m_fieldInfo.Name;

		public MyVisualSyntaxEventNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
			if (!(ObjectBuilder is MyObjectBuilder_InputScriptNode))
			{
				m_fieldInfo = MyVisualScriptingProxy.GetField(ObjectBuilder.Name);
			}
		}

		public void AddSequenceInput(MyVisualSyntaxNode node)
		{
			SequenceInputs.Add(node);
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			foreach (string outputName in ObjectBuilder.OutputNames)
			{
				if (outputName == variableIdentifier)
				{
					return variableIdentifier;
				}
			}
			return null;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed && ObjectBuilder.SequenceOutputID != -1)
			{
				m_nextSequenceNode = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOutputID);
				SequenceOutputs.Add(m_nextSequenceNode);
			}
			base.Preprocess(currentDepth);
		}
	}
}
