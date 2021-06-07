namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// This node represents a class variable name getter.
	/// Genertes no syntax and only provides other node with
	/// variable name.
	/// </summary>
	public class MyVisualSyntaxGetterNode : MyVisualSyntaxNode
	{
		internal override bool SequenceDependent => false;

		public new MyObjectBuilder_GetterScriptNode ObjectBuilder => (MyObjectBuilder_GetterScriptNode)m_objectBuilder;

		public MyVisualSyntaxGetterNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return ObjectBuilder.BoundVariableName;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				for (int i = 0; i < ObjectBuilder.OutputIDs.Ids.Count; i++)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.OutputIDs.Ids[i].NodeID);
					if (nodeByID != null)
					{
						Outputs.Add(nodeByID);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
