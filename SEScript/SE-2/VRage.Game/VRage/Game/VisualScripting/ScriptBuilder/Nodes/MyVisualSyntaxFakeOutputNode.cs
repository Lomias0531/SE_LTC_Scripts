namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxFakeOutputNode : MyVisualSyntaxNode
	{
		public int ID
		{
			get;
			private set;
		}

		public MyVisualSyntaxFakeOutputNode(int id)
			: base(null)
		{
			ID = id;
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null)
		{
			return "outParamFunctionNode_" + (int)ID + "_" + variableIdentifier;
		}
	}
}
