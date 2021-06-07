namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// This node does not generate any syntax, but runs the preprocessing
	/// part and initializes the collecting of syntax.
	/// </summary>
	public class MyVisualSyntaxBuilderNode : MyVisualSyntaxNode
	{
		public MyVisualSyntaxBuilderNode()
			: base(null)
		{
		}

		public void Preprocess()
		{
			Depth = 0;
			base.Navigator.FreshNodes.Clear();
			Preprocess(Depth + 1);
			foreach (MyVisualSyntaxNode freshNode in base.Navigator.FreshNodes)
			{
				if (freshNode.Outputs.Count == 1)
				{
					freshNode.Outputs[0].SubTreeNodes.Add(freshNode);
				}
				else
				{
					MyVisualSyntaxNode myVisualSyntaxNode = MyVisualSyntaxNode.CommonParent(freshNode.GetSequenceDependentOutputs());
					if (myVisualSyntaxNode == null)
					{
						SubTreeNodes.Add(freshNode);
					}
					else
					{
						myVisualSyntaxNode.SubTreeNodes.Add(freshNode);
					}
				}
			}
		}
	}
}
