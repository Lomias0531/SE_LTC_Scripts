using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Definitions.Animation
{
	internal class MyAnimationControllerDefinitionPostprocess : MyDefinitionPostprocessor
	{
		public override void AfterLoaded(ref Bundle definitions)
		{
			foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in definitions.Definitions)
			{
				MyAnimationControllerDefinition myAnimationControllerDefinition = definition.Value as MyAnimationControllerDefinition;
				if (myAnimationControllerDefinition != null && myAnimationControllerDefinition.StateMachines != null && !definition.Value.Context.IsBaseGame && definition.Value.Context != null && definition.Value.Context.ModPath != null)
				{
					foreach (MyObjectBuilder_AnimationSM stateMachine in myAnimationControllerDefinition.StateMachines)
					{
						MyObjectBuilder_AnimationSMNode[] nodes = stateMachine.Nodes;
						foreach (MyObjectBuilder_AnimationSMNode myObjectBuilder_AnimationSMNode in nodes)
						{
							if (myObjectBuilder_AnimationSMNode.AnimationTree != null && myObjectBuilder_AnimationSMNode.AnimationTree.Child != null)
							{
								ResolveMwmPaths(definition.Value.Context, myObjectBuilder_AnimationSMNode.AnimationTree.Child);
							}
						}
					}
				}
			}
		}

		private void ResolveMwmPaths(MyModContext modContext, MyObjectBuilder_AnimationTreeNode objBuilderNode)
		{
			MyObjectBuilder_AnimationTreeNodeTrack myObjectBuilder_AnimationTreeNodeTrack = objBuilderNode as MyObjectBuilder_AnimationTreeNodeTrack;
			if (myObjectBuilder_AnimationTreeNodeTrack != null && myObjectBuilder_AnimationTreeNodeTrack.PathToModel != null)
			{
				string text = Path.Combine(modContext.ModPath, myObjectBuilder_AnimationTreeNodeTrack.PathToModel);
				if (MyFileSystem.FileExists(text))
				{
					myObjectBuilder_AnimationTreeNodeTrack.PathToModel = text;
				}
			}
			MyObjectBuilder_AnimationTreeNodeMix1D myObjectBuilder_AnimationTreeNodeMix1D = objBuilderNode as MyObjectBuilder_AnimationTreeNodeMix1D;
			if (myObjectBuilder_AnimationTreeNodeMix1D != null && myObjectBuilder_AnimationTreeNodeMix1D.Children != null)
			{
				MyParameterAnimTreeNodeMapping[] children = myObjectBuilder_AnimationTreeNodeMix1D.Children;
				for (int i = 0; i < children.Length; i++)
				{
					MyParameterAnimTreeNodeMapping myParameterAnimTreeNodeMapping = children[i];
					if (myParameterAnimTreeNodeMapping.Node != null)
					{
						ResolveMwmPaths(modContext, myParameterAnimTreeNodeMapping.Node);
					}
				}
			}
			MyObjectBuilder_AnimationTreeNodeAdd myObjectBuilder_AnimationTreeNodeAdd = objBuilderNode as MyObjectBuilder_AnimationTreeNodeAdd;
			if (myObjectBuilder_AnimationTreeNodeAdd != null)
			{
				if (myObjectBuilder_AnimationTreeNodeAdd.BaseNode.Node != null)
				{
					ResolveMwmPaths(modContext, myObjectBuilder_AnimationTreeNodeAdd.BaseNode.Node);
				}
				if (myObjectBuilder_AnimationTreeNodeAdd.AddNode.Node != null)
				{
					ResolveMwmPaths(modContext, myObjectBuilder_AnimationTreeNodeAdd.AddNode.Node);
				}
			}
		}

		public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
		{
		}

		public override void OverrideBy(ref Bundle currentDefinitions, ref Bundle overrideBySet)
		{
			foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in overrideBySet.Definitions)
			{
				MyAnimationControllerDefinition myAnimationControllerDefinition = definition.Value as MyAnimationControllerDefinition;
				if (definition.Value.Enabled && myAnimationControllerDefinition != null)
				{
					bool flag = true;
					if (currentDefinitions.Definitions.ContainsKey(definition.Key))
					{
						MyAnimationControllerDefinition myAnimationControllerDefinition2 = currentDefinitions.Definitions[definition.Key] as MyAnimationControllerDefinition;
						if (myAnimationControllerDefinition2 != null)
						{
							foreach (MyObjectBuilder_AnimationSM stateMachine in myAnimationControllerDefinition.StateMachines)
							{
								bool flag2 = false;
								foreach (MyObjectBuilder_AnimationSM stateMachine2 in myAnimationControllerDefinition2.StateMachines)
								{
									if (stateMachine.Name == stateMachine2.Name)
									{
										stateMachine2.Nodes = stateMachine.Nodes;
										stateMachine2.Transitions = stateMachine.Transitions;
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									myAnimationControllerDefinition2.StateMachines.Add(stateMachine);
								}
							}
							foreach (MyObjectBuilder_AnimationLayer layer in myAnimationControllerDefinition.Layers)
							{
								bool flag3 = false;
								foreach (MyObjectBuilder_AnimationLayer layer2 in myAnimationControllerDefinition2.Layers)
								{
									if (layer.Name == layer2.Name)
									{
										layer2.Name = layer.Name;
										layer2.BoneMask = layer.BoneMask;
										layer2.InitialSMNode = layer.InitialSMNode;
										layer2.StateMachine = layer.StateMachine;
										layer2.Mode = layer.Mode;
										flag3 = true;
									}
								}
								if (!flag3)
								{
									myAnimationControllerDefinition2.Layers.Add(layer);
								}
							}
							flag = false;
						}
					}
					if (flag)
					{
						currentDefinitions.Definitions[definition.Key] = definition.Value;
					}
				}
			}
		}
	}
}
