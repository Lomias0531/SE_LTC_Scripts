using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxTriggerNode : MyVisualSyntaxNode
	{
		public new MyObjectBuilder_TriggerScriptNode ObjectBuilder => (MyObjectBuilder_TriggerScriptNode)m_objectBuilder;

		public MyVisualSyntaxTriggerNode(MyObjectBuilder_ScriptNode ob)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < ObjectBuilder.InputNames.Count; i++)
			{
				list.Add(Inputs[i].VariableSyntaxName(ObjectBuilder.InputIDs[i].VariableName));
			}
			expressions.Add(SyntaxFactory.ExpressionStatement(MySyntaxFactory.MethodInvocation(ObjectBuilder.TriggerName, list)));
			base.CollectInputExpressions(expressions);
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions)
		{
			CollectInputExpressions(expressions);
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceInputID != -1)
				{
					MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceInputID);
					SequenceInputs.Add(nodeByID);
				}
				for (int i = 0; i < ObjectBuilder.InputNames.Count; i++)
				{
					if (ObjectBuilder.InputIDs[i].NodeID == -1)
					{
						throw new Exception("TriggerNode is missing an input of " + ObjectBuilder.InputNames[i] + " . NodeId: " + (int)ObjectBuilder.ID);
					}
					MyVisualSyntaxNode nodeByID2 = base.Navigator.GetNodeByID(ObjectBuilder.InputIDs[i].NodeID);
					Inputs.Add(nodeByID2);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
