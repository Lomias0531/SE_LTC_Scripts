using VRage.Game.ObjectBuilders.VisualScripting;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxSequenceNode : MyVisualSyntaxNode
	{
		public new MyObjectBuilder_SequenceScriptNode ObjectBuilder => (MyObjectBuilder_SequenceScriptNode)m_objectBuilder;

		public MyVisualSyntaxSequenceNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceInput != -1)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInput);
					SequenceInputs.Add(nodeByID);
				}
				foreach (int sequenceOutput in ObjectBuilder.SequenceOutputs)
				{
					if (sequenceOutput != -1)
					{
						MyVisualSyntaxNode nodeByID2 = base.Navigator.GetNodeByID(sequenceOutput);
						SequenceOutputs.Add(nodeByID2);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
