using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VRage.Game.Definitions.Animation;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Animation;
using VRage.Generics;
using VRage.Generics.StateMachine;
using VRage.Utils;
using VRageRender.Animations;

namespace VRage.Game.Components
{
	public static class MyAnimationControllerComponentLoadFromDef
	{
		private struct MyAnimationVirtualNodeData
		{
			public bool ExceptTarget;

			public string AnyNodePrefix;
		}

		private class MyAnimationVirtualNodes
		{
			public readonly Dictionary<string, MyAnimationVirtualNodeData> NodesAny = new Dictionary<string, MyAnimationVirtualNodeData>();
		}

		private static readonly char[] m_boneListSeparators = new char[1]
		{
			' '
		};

		public static bool InitFromDefinition(this MyAnimationControllerComponent thisController, MyAnimationControllerDefinition animControllerDefinition, bool forceReloadMwm = false)
		{
			bool flag = true;
			thisController.Clear();
			thisController.SourceId = animControllerDefinition.Id;
			foreach (MyObjectBuilder_AnimationLayer layer in animControllerDefinition.Layers)
			{
				MyAnimationStateMachine myAnimationStateMachine = thisController.Controller.CreateLayer(layer.Name);
				if (myAnimationStateMachine != null)
				{
					switch (layer.Mode)
					{
					case MyObjectBuilder_AnimationLayer.MyLayerMode.Add:
						myAnimationStateMachine.Mode = MyAnimationStateMachine.MyBlendingMode.Add;
						break;
					case MyObjectBuilder_AnimationLayer.MyLayerMode.Replace:
						myAnimationStateMachine.Mode = MyAnimationStateMachine.MyBlendingMode.Replace;
						break;
					default:
						myAnimationStateMachine.Mode = MyAnimationStateMachine.MyBlendingMode.Replace;
						break;
					}
					if (layer.BoneMask != null)
					{
						string[] array = layer.BoneMask.Split(m_boneListSeparators);
						foreach (string str in array)
						{
							myAnimationStateMachine.BoneMaskStrIds.Add(MyStringId.GetOrCompute(str));
						}
					}
					else
					{
						myAnimationStateMachine.BoneMaskStrIds.Clear();
					}
					myAnimationStateMachine.BoneMask = null;
					MyAnimationVirtualNodes virtualNodes = new MyAnimationVirtualNodes();
					flag = (InitLayerNodes(myAnimationStateMachine, layer.StateMachine, animControllerDefinition, thisController.Controller, myAnimationStateMachine.Name + "/", virtualNodes, forceReloadMwm) && flag);
					myAnimationStateMachine.SetState(myAnimationStateMachine.Name + "/" + layer.InitialSMNode);
					if (myAnimationStateMachine.ActiveCursors.Count > 0 && myAnimationStateMachine.ActiveCursors[0].Node != null)
					{
						MyAnimationStateMachineNode myAnimationStateMachineNode = myAnimationStateMachine.ActiveCursors[0].Node as MyAnimationStateMachineNode;
						if (myAnimationStateMachineNode != null)
						{
							foreach (MyAnimationStateMachineNode.VarAssignmentData variableAssignment in myAnimationStateMachineNode.VariableAssignments)
							{
								thisController.Controller.Variables.SetValue(variableAssignment.VariableId, variableAssignment.Value);
							}
						}
					}
					myAnimationStateMachine.SortTransitions();
				}
			}
			foreach (MyObjectBuilder_AnimationFootIkChain footIkChain in animControllerDefinition.FootIkChains)
			{
				thisController.InverseKinematics.RegisterFootBone(footIkChain.FootBone, footIkChain.ChainLength, footIkChain.AlignBoneWithTerrain);
			}
			foreach (string ikIgnoredBone in animControllerDefinition.IkIgnoredBones)
			{
				thisController.InverseKinematics.RegisterIgnoredBone(ikIgnoredBone);
			}
			if (flag)
			{
				thisController.MarkAsValid();
			}
			return flag;
		}

		private static bool InitLayerNodes(MyAnimationStateMachine layer, string stateMachineName, MyAnimationControllerDefinition animControllerDefinition, MyAnimationController animationController, string currentNodeNamePrefix, MyAnimationVirtualNodes virtualNodes, bool forceReloadMwm)
		{
			MyObjectBuilder_AnimationSM myObjectBuilder_AnimationSM = animControllerDefinition.StateMachines.FirstOrDefault((MyObjectBuilder_AnimationSM x) => x.Name == stateMachineName);
			if (myObjectBuilder_AnimationSM == null)
			{
				return false;
			}
			bool result = true;
			if (myObjectBuilder_AnimationSM.Nodes != null)
			{
				MyObjectBuilder_AnimationSMNode[] nodes = myObjectBuilder_AnimationSM.Nodes;
				foreach (MyObjectBuilder_AnimationSMNode myObjectBuilder_AnimationSMNode in nodes)
				{
					string text = currentNodeNamePrefix + myObjectBuilder_AnimationSMNode.Name;
					if (myObjectBuilder_AnimationSMNode.StateMachineName != null)
					{
						if (!InitLayerNodes(layer, myObjectBuilder_AnimationSMNode.StateMachineName, animControllerDefinition, animationController, text + "/", virtualNodes, forceReloadMwm))
						{
							result = false;
						}
						continue;
					}
					MyAnimationStateMachineNode myAnimationStateMachineNode = new MyAnimationStateMachineNode(text);
					if (myObjectBuilder_AnimationSMNode.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.PassThrough || myObjectBuilder_AnimationSMNode.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.Any || myObjectBuilder_AnimationSMNode.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.AnyExceptTarget)
					{
						myAnimationStateMachineNode.PassThrough = true;
					}
					else
					{
						myAnimationStateMachineNode.PassThrough = false;
					}
					if (myObjectBuilder_AnimationSMNode.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.Any || myObjectBuilder_AnimationSMNode.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.AnyExceptTarget)
					{
						virtualNodes.NodesAny.Add(text, new MyAnimationVirtualNodeData
						{
							AnyNodePrefix = currentNodeNamePrefix,
							ExceptTarget = (myObjectBuilder_AnimationSMNode.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.AnyExceptTarget)
						});
					}
					layer.AddNode(myAnimationStateMachineNode);
					if (myObjectBuilder_AnimationSMNode.AnimationTree != null)
					{
						MyAnimationTreeNode myAnimationTreeNode2 = myAnimationStateMachineNode.RootAnimationNode = InitNodeAnimationTree(myObjectBuilder_AnimationSMNode.AnimationTree.Child, forceReloadMwm);
					}
					else
					{
						myAnimationStateMachineNode.RootAnimationNode = new MyAnimationTreeNodeDummy();
					}
					if (myObjectBuilder_AnimationSMNode.Variables != null)
					{
						myAnimationStateMachineNode.VariableAssignments = new List<MyAnimationStateMachineNode.VarAssignmentData>(myObjectBuilder_AnimationSMNode.Variables.Select(delegate(MyObjectBuilder_AnimationSMVariable builder)
						{
							MyAnimationStateMachineNode.VarAssignmentData result2 = default(MyAnimationStateMachineNode.VarAssignmentData);
							result2.VariableId = MyStringId.GetOrCompute(builder.Name);
							result2.Value = builder.Value;
							return result2;
						}));
					}
				}
			}
			if (myObjectBuilder_AnimationSM.Transitions != null)
			{
				MyObjectBuilder_AnimationSMTransition[] transitions = myObjectBuilder_AnimationSM.Transitions;
				foreach (MyObjectBuilder_AnimationSMTransition myObjectBuilder_AnimationSMTransition in transitions)
				{
					string text2 = currentNodeNamePrefix + myObjectBuilder_AnimationSMTransition.From;
					string text3 = currentNodeNamePrefix + myObjectBuilder_AnimationSMTransition.To;
					if (virtualNodes.NodesAny.TryGetValue(text2, out MyAnimationVirtualNodeData value))
					{
						foreach (KeyValuePair<string, MyStateMachineNode> allNode in layer.AllNodes)
						{
							if (allNode.Key.StartsWith(value.AnyNodePrefix) && allNode.Key != text2 && (!value.ExceptTarget || text3 != allNode.Key))
							{
								CreateTransition(layer, animationController, allNode.Key, text3, myObjectBuilder_AnimationSMTransition);
							}
						}
					}
					CreateTransition(layer, animationController, text2, text3, myObjectBuilder_AnimationSMTransition);
				}
			}
			return result;
		}

		private static void CreateTransition(MyAnimationStateMachine layer, MyAnimationController animationController, string absoluteNameNodeFrom, string absoluteNameNodeTo, MyObjectBuilder_AnimationSMTransition objBuilderTransition)
		{
			int num = 0;
			do
			{
				MyAnimationStateMachineTransition myAnimationStateMachineTransition = layer.AddTransition(absoluteNameNodeFrom, absoluteNameNodeTo, new MyAnimationStateMachineTransition()) as MyAnimationStateMachineTransition;
				if (myAnimationStateMachineTransition != null)
				{
					myAnimationStateMachineTransition.Name = MyStringId.GetOrCompute((objBuilderTransition.Name != null) ? objBuilderTransition.Name.ToLower() : null);
					myAnimationStateMachineTransition.TransitionTimeInSec = objBuilderTransition.TimeInSec;
					myAnimationStateMachineTransition.Sync = objBuilderTransition.Sync;
					myAnimationStateMachineTransition.Curve = objBuilderTransition.Curve;
					myAnimationStateMachineTransition.Priority = objBuilderTransition.Priority;
					if (objBuilderTransition.Conditions != null && objBuilderTransition.Conditions[num] != null)
					{
						MyObjectBuilder_AnimationSMCondition[] conditions = objBuilderTransition.Conditions[num].Conditions;
						foreach (MyObjectBuilder_AnimationSMCondition objBuilderCondition in conditions)
						{
							MyCondition<float> myCondition = ParseOneCondition(animationController, objBuilderCondition);
							if (myCondition != null)
							{
								myAnimationStateMachineTransition.Conditions.Add(myCondition);
							}
						}
					}
				}
				num++;
			}
			while (objBuilderTransition.Conditions != null && num < objBuilderTransition.Conditions.Length);
		}

		private static MyCondition<float> ParseOneCondition(MyAnimationController animationController, MyObjectBuilder_AnimationSMCondition objBuilderCondition)
		{
			objBuilderCondition.ValueLeft = ((objBuilderCondition.ValueLeft != null) ? objBuilderCondition.ValueLeft.ToLower() : "0");
			objBuilderCondition.ValueRight = ((objBuilderCondition.ValueRight != null) ? objBuilderCondition.ValueRight.ToLower() : "0");
			double result2;
			MyCondition<float> result3;
			if (double.TryParse(objBuilderCondition.ValueLeft, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
			{
				if (double.TryParse(objBuilderCondition.ValueRight, NumberStyles.Float, CultureInfo.InvariantCulture, out result2))
				{
					result3 = new MyCondition<float>(animationController.Variables, ConvertOperation(objBuilderCondition.Operation), (float)result, (float)result2);
				}
				else
				{
					result3 = new MyCondition<float>(animationController.Variables, ConvertOperation(objBuilderCondition.Operation), (float)result, objBuilderCondition.ValueRight);
					MyStringId orCompute = MyStringId.GetOrCompute(objBuilderCondition.ValueRight);
					if (!animationController.Variables.AllVariables.ContainsKey(orCompute))
					{
						animationController.Variables.SetValue(orCompute, 0f);
					}
				}
			}
			else if (double.TryParse(objBuilderCondition.ValueRight, NumberStyles.Float, CultureInfo.InvariantCulture, out result2))
			{
				result3 = new MyCondition<float>(animationController.Variables, ConvertOperation(objBuilderCondition.Operation), objBuilderCondition.ValueLeft, (float)result2);
				MyStringId orCompute2 = MyStringId.GetOrCompute(objBuilderCondition.ValueLeft);
				if (!animationController.Variables.AllVariables.ContainsKey(orCompute2))
				{
					animationController.Variables.SetValue(orCompute2, 0f);
				}
			}
			else
			{
				result3 = new MyCondition<float>(animationController.Variables, ConvertOperation(objBuilderCondition.Operation), objBuilderCondition.ValueLeft, objBuilderCondition.ValueRight);
				MyStringId orCompute3 = MyStringId.GetOrCompute(objBuilderCondition.ValueLeft);
				MyStringId orCompute4 = MyStringId.GetOrCompute(objBuilderCondition.ValueRight);
				if (!animationController.Variables.AllVariables.ContainsKey(orCompute3))
				{
					animationController.Variables.SetValue(orCompute3, 0f);
				}
				if (!animationController.Variables.AllVariables.ContainsKey(orCompute4))
				{
					animationController.Variables.SetValue(orCompute4, 0f);
				}
			}
			return result3;
		}

		private static MyAnimationTreeNode InitNodeAnimationTree(MyObjectBuilder_AnimationTreeNode objBuilderNode, bool forceReloadMwm)
		{
			MyObjectBuilder_AnimationTreeNodeDynamicTrack myObjectBuilder_AnimationTreeNodeDynamicTrack = objBuilderNode as MyObjectBuilder_AnimationTreeNodeDynamicTrack;
			if (myObjectBuilder_AnimationTreeNodeDynamicTrack != null)
			{
				return new MyAnimationTreeNodeDynamicTrack
				{
					Loop = myObjectBuilder_AnimationTreeNodeDynamicTrack.Loop,
					Speed = myObjectBuilder_AnimationTreeNodeDynamicTrack.Speed,
					DefaultAnimation = MyStringId.GetOrCompute(myObjectBuilder_AnimationTreeNodeDynamicTrack.DefaultAnimation),
					Interpolate = myObjectBuilder_AnimationTreeNodeDynamicTrack.Interpolate,
					SynchronizeWithLayer = myObjectBuilder_AnimationTreeNodeDynamicTrack.SynchronizeWithLayer
				};
			}
			MyObjectBuilder_AnimationTreeNodeTrack objBuilderNodeTrack = objBuilderNode as MyObjectBuilder_AnimationTreeNodeTrack;
			if (objBuilderNodeTrack != null)
			{
				MyAnimationTreeNodeTrack myAnimationTreeNodeTrack = new MyAnimationTreeNodeTrack();
				MyModel myModel = (objBuilderNodeTrack.PathToModel != null) ? MyModels.GetModelOnlyAnimationData(objBuilderNodeTrack.PathToModel, forceReloadMwm) : null;
				if (myModel != null && myModel.Animations != null && myModel.Animations.Clips != null && myModel.Animations.Clips.Count > 0)
				{
					MyAnimationClip myAnimationClip = myModel.Animations.Clips.FirstOrDefault((MyAnimationClip clipItem) => clipItem.Name == objBuilderNodeTrack.AnimationName);
					myAnimationClip = (myAnimationClip ?? myModel.Animations.Clips[0]);
					myAnimationTreeNodeTrack.SetClip(myAnimationClip);
					myAnimationTreeNodeTrack.Loop = objBuilderNodeTrack.Loop;
					myAnimationTreeNodeTrack.Speed = objBuilderNodeTrack.Speed;
					myAnimationTreeNodeTrack.Interpolate = objBuilderNodeTrack.Interpolate;
					myAnimationTreeNodeTrack.SynchronizeWithLayer = objBuilderNodeTrack.SynchronizeWithLayer;
				}
				else if (objBuilderNodeTrack.PathToModel != null)
				{
					MyLog.Default.Log(MyLogSeverity.Error, "Cannot load MWM track {0}.", objBuilderNodeTrack.PathToModel);
				}
				return myAnimationTreeNodeTrack;
			}
			MyObjectBuilder_AnimationTreeNodeMix1D myObjectBuilder_AnimationTreeNodeMix1D = objBuilderNode as MyObjectBuilder_AnimationTreeNodeMix1D;
			if (myObjectBuilder_AnimationTreeNodeMix1D != null)
			{
				MyAnimationTreeNodeMix1D myAnimationTreeNodeMix1D = new MyAnimationTreeNodeMix1D();
				if (myObjectBuilder_AnimationTreeNodeMix1D.Children != null)
				{
					MyParameterAnimTreeNodeMapping[] children = myObjectBuilder_AnimationTreeNodeMix1D.Children;
					for (int i = 0; i < children.Length; i++)
					{
						MyParameterAnimTreeNodeMapping myParameterAnimTreeNodeMapping = children[i];
						MyAnimationTreeNodeMix1D.MyParameterNodeMapping myParameterNodeMapping = default(MyAnimationTreeNodeMix1D.MyParameterNodeMapping);
						myParameterNodeMapping.ParamValueBinding = myParameterAnimTreeNodeMapping.Param;
						myParameterNodeMapping.Child = InitNodeAnimationTree(myParameterAnimTreeNodeMapping.Node, forceReloadMwm);
						MyAnimationTreeNodeMix1D.MyParameterNodeMapping item = myParameterNodeMapping;
						myAnimationTreeNodeMix1D.ChildMappings.Add(item);
					}
					myAnimationTreeNodeMix1D.ChildMappings.Sort((MyAnimationTreeNodeMix1D.MyParameterNodeMapping x, MyAnimationTreeNodeMix1D.MyParameterNodeMapping y) => x.ParamValueBinding.CompareTo(y.ParamValueBinding));
				}
				myAnimationTreeNodeMix1D.ParameterName = MyStringId.GetOrCompute(myObjectBuilder_AnimationTreeNodeMix1D.ParameterName);
				myAnimationTreeNodeMix1D.Circular = myObjectBuilder_AnimationTreeNodeMix1D.Circular;
				myAnimationTreeNodeMix1D.Sensitivity = myObjectBuilder_AnimationTreeNodeMix1D.Sensitivity;
				myAnimationTreeNodeMix1D.MaxChange = (myObjectBuilder_AnimationTreeNodeMix1D.MaxChange ?? float.PositiveInfinity);
				if (myAnimationTreeNodeMix1D.MaxChange <= 0f)
				{
					myAnimationTreeNodeMix1D.MaxChange = float.PositiveInfinity;
				}
				return myAnimationTreeNodeMix1D;
			}
			_ = (objBuilderNode is MyObjectBuilder_AnimationTreeNodeAdd);
			return null;
		}

		private static MyCondition<float>.MyOperation ConvertOperation(MyObjectBuilder_AnimationSMCondition.MyOperationType operation)
		{
			switch (operation)
			{
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.AlwaysFalse:
				return MyCondition<float>.MyOperation.AlwaysFalse;
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.AlwaysTrue:
				return MyCondition<float>.MyOperation.AlwaysTrue;
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.Equal:
				return MyCondition<float>.MyOperation.Equal;
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.Greater:
				return MyCondition<float>.MyOperation.Greater;
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.GreaterOrEqual:
				return MyCondition<float>.MyOperation.GreaterOrEqual;
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.Less:
				return MyCondition<float>.MyOperation.Less;
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.LessOrEqual:
				return MyCondition<float>.MyOperation.LessOrEqual;
			case MyObjectBuilder_AnimationSMCondition.MyOperationType.NotEqual:
				return MyCondition<float>.MyOperation.NotEqual;
			default:
				return MyCondition<float>.MyOperation.AlwaysFalse;
			}
		}
	}
}
