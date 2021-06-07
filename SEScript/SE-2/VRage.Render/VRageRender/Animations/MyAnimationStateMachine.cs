using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Generics;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Animation state machine selects the animation to match current state.
	/// When it finds valid transition to some next state, transition is performed automatically.
	/// </summary>
	public class MyAnimationStateMachine : MySingleStateMachine
	{
		public enum MyBlendingMode
		{
			Replace,
			Add
		}

		public struct MyStateTransitionBlending
		{
			public double TimeLeftInSeconds;

			public double InvTotalTime;

			public MyAnimationStateMachineNode SourceState;

			public MyAnimationTransitionCurve Curve;
		}

		public MyAnimationUpdateData CurrentUpdateData;

		public MyBlendingMode Mode;

		public readonly HashSet<MyStringId> BoneMaskStrIds = new HashSet<MyStringId>();

		public bool[] BoneMask;

		private readonly List<MyStateTransitionBlending> m_stateTransitionBlending;

		private int[] m_lastVisitedTreeNodesPath;

		public int[] VisitedTreeNodesPath
		{
			get;
			private set;
		}

		public ListReader<MyStateTransitionBlending> StateTransitionBlending => m_stateTransitionBlending;

		public MyAnimationStateMachine()
		{
			VisitedTreeNodesPath = new int[64];
			m_lastVisitedTreeNodesPath = new int[64];
			m_stateTransitionBlending = new List<MyStateTransitionBlending>();
			base.OnStateChanged += AnimationStateChanged;
		}

		public void Update(ref MyAnimationUpdateData data)
		{
			if (data.CharacterBones == null)
			{
				return;
			}
			CurrentUpdateData = data;
			CurrentUpdateData.VisitedTreeNodesCounter = 0;
			CurrentUpdateData.VisitedTreeNodesPath = m_lastVisitedTreeNodesPath;
			CurrentUpdateData.VisitedTreeNodesPath[0] = 0;
			if (BoneMask == null)
			{
				RebuildBoneMask();
			}
			data.LayerBoneMask = (CurrentUpdateData.LayerBoneMask = BoneMask);
			MyAnimationStateMachineNode myAnimationStateMachineNode = base.CurrentNode as MyAnimationStateMachineNode;
			if (myAnimationStateMachineNode != null && myAnimationStateMachineNode.RootAnimationNode != null)
			{
				float localTimeNormalized = myAnimationStateMachineNode.RootAnimationNode.GetLocalTimeNormalized();
				data.Controller.Variables.SetValue(MyAnimationVariableStorageHints.StrIdAnimationFinished, localTimeNormalized);
			}
			else
			{
				data.Controller.Variables.SetValue(MyAnimationVariableStorageHints.StrIdAnimationFinished, 0f);
			}
			base.Update();
			int[] visitedTreeNodesPath = VisitedTreeNodesPath;
			VisitedTreeNodesPath = m_lastVisitedTreeNodesPath;
			m_lastVisitedTreeNodesPath = visitedTreeNodesPath;
			CurrentUpdateData.VisitedTreeNodesPath = null;
			float num = 1f;
			for (int i = 0; i < m_stateTransitionBlending.Count; i++)
			{
				MyStateTransitionBlending value = m_stateTransitionBlending[i];
				float num2 = (float)(value.TimeLeftInSeconds * value.InvTotalTime);
				num *= num2;
				if (num > 0f)
				{
					List<MyAnimationClip.BoneState> bonesResult = CurrentUpdateData.BonesResult;
					CurrentUpdateData.BonesResult = null;
					value.SourceState.OnUpdate(this);
					if (bonesResult != null && CurrentUpdateData.BonesResult != null)
					{
						for (int j = 0; j < bonesResult.Count; j++)
						{
							if (data.LayerBoneMask[j])
							{
								float amount = ComputeEaseInEaseOut(MathHelper.Clamp(num, 0f, 1f), value.Curve);
								CurrentUpdateData.BonesResult[j].Rotation = Quaternion.Slerp(bonesResult[j].Rotation, CurrentUpdateData.BonesResult[j].Rotation, amount);
								CurrentUpdateData.BonesResult[j].Translation = Vector3.Lerp(bonesResult[j].Translation, CurrentUpdateData.BonesResult[j].Translation, amount);
							}
						}
						data.Controller.ResultBonesPool.Free(bonesResult);
					}
				}
				value.TimeLeftInSeconds -= data.DeltaTimeInSeconds;
				m_stateTransitionBlending[i] = value;
				if (value.TimeLeftInSeconds <= 0.0 || num <= 0f)
				{
					for (int k = i + 1; k < m_stateTransitionBlending.Count; k++)
					{
						MyStateTransitionBlending value2 = m_stateTransitionBlending[k];
						value2.TimeLeftInSeconds = 0.0;
						m_stateTransitionBlending[k] = value2;
					}
					break;
				}
			}
			m_stateTransitionBlending.RemoveAll((MyStateTransitionBlending s) => s.TimeLeftInSeconds <= 0.0);
			data.BonesResult = CurrentUpdateData.BonesResult;
		}

		/// <summary>
		/// Computing transition weight from the normalized time.
		/// </summary>
		/// <param name="t">normalized remaining time of the animation that is being phased out, going from 1 to 0</param>
		/// <param name="curve">used transition curve</param>
		/// <returns>weight of the animation that is being phased out, 1 to 0</returns>
		private static float ComputeEaseInEaseOut(float t, MyAnimationTransitionCurve curve)
		{
			switch (curve)
			{
			case MyAnimationTransitionCurve.Smooth:
				return t * t * (3f - 2f * t);
			case MyAnimationTransitionCurve.EaseIn:
				return t * t * t;
			default:
				return t;
			}
		}

		private void RebuildBoneMask()
		{
			if (CurrentUpdateData.CharacterBones == null)
			{
				return;
			}
			BoneMask = new bool[CurrentUpdateData.CharacterBones.Length];
			if (BoneMaskStrIds.Count == 0)
			{
				for (int i = 0; i < CurrentUpdateData.CharacterBones.Length; i++)
				{
					BoneMask[i] = true;
				}
				return;
			}
			for (int j = 0; j < CurrentUpdateData.CharacterBones.Length; j++)
			{
				MyStringId myStringId = MyStringId.TryGet(CurrentUpdateData.CharacterBones[j].Name);
				if (myStringId != MyStringId.NullOrEmpty && BoneMaskStrIds.Contains(myStringId))
				{
					BoneMask[j] = true;
				}
			}
		}

		public override string ToString()
		{
			return $"MyAnimationStateMachine, Name='{base.Name}', Mode='{Mode}'";
		}

		private void AnimationStateChanged(MyStateMachineTransitionWithStart transitionWithStart, MyStringId action)
		{
			MyAnimationStateMachineTransition myAnimationStateMachineTransition = transitionWithStart.Transition as MyAnimationStateMachineTransition;
			if (myAnimationStateMachineTransition == null)
			{
				return;
			}
			MyAnimationStateMachineNode myAnimationStateMachineNode = transitionWithStart.StartNode as MyAnimationStateMachineNode;
			MyAnimationStateMachineNode myAnimationStateMachineNode2 = myAnimationStateMachineTransition.TargetNode as MyAnimationStateMachineNode;
			if (myAnimationStateMachineNode == null)
			{
				return;
			}
			if (myAnimationStateMachineNode2 != null)
			{
				AssignVariableValues(myAnimationStateMachineNode2);
				bool flag = false;
				foreach (MyStateTransitionBlending item2 in m_stateTransitionBlending)
				{
					if (item2.SourceState == myAnimationStateMachineNode2)
					{
						flag = true;
						break;
					}
				}
				if (myAnimationStateMachineNode2.RootAnimationNode != null)
				{
					myAnimationStateMachineNode2.RootAnimationNode.SetAction(action);
				}
				switch (myAnimationStateMachineTransition.Sync)
				{
				case MyAnimationTransitionSyncType.Restart:
					if (myAnimationStateMachineNode2.RootAnimationNode != null)
					{
						myAnimationStateMachineNode2.RootAnimationNode.SetLocalTimeNormalized(0f);
					}
					break;
				case MyAnimationTransitionSyncType.Synchronize:
					if (!flag && myAnimationStateMachineNode.RootAnimationNode != null && myAnimationStateMachineNode2.RootAnimationNode != null)
					{
						float localTimeNormalized = myAnimationStateMachineNode.RootAnimationNode.GetLocalTimeNormalized();
						myAnimationStateMachineNode2.RootAnimationNode.SetLocalTimeNormalized(localTimeNormalized);
					}
					break;
				}
			}
			if ((myAnimationStateMachineTransition.TransitionTimeInSec > 0.0 || transitionWithStart.Transition.TargetNode.PassThrough) && !transitionWithStart.StartNode.PassThrough)
			{
				MyStateTransitionBlending myStateTransitionBlending = default(MyStateTransitionBlending);
				myStateTransitionBlending.SourceState = myAnimationStateMachineNode;
				myStateTransitionBlending.TimeLeftInSeconds = myAnimationStateMachineTransition.TransitionTimeInSec;
				myStateTransitionBlending.InvTotalTime = 1.0 / myAnimationStateMachineTransition.TransitionTimeInSec;
				myStateTransitionBlending.Curve = myAnimationStateMachineTransition.Curve;
				MyStateTransitionBlending item = myStateTransitionBlending;
				if (!item.InvTotalTime.IsValid())
				{
					item.InvTotalTime = 1.0;
				}
				m_stateTransitionBlending.Insert(0, item);
			}
			else if (transitionWithStart.StartNode.PassThrough && m_stateTransitionBlending.Count > 0)
			{
				MyStateTransitionBlending value = m_stateTransitionBlending[0];
				value.TimeLeftInSeconds = Math.Max(myAnimationStateMachineTransition.TransitionTimeInSec, m_stateTransitionBlending[0].TimeLeftInSeconds);
				value.InvTotalTime = 1.0 / value.TimeLeftInSeconds;
				value.Curve = myAnimationStateMachineTransition.Curve;
				if (!value.InvTotalTime.IsValid())
				{
					value.InvTotalTime = 1.0;
				}
				m_stateTransitionBlending[0] = value;
			}
			else if (myAnimationStateMachineTransition.TransitionTimeInSec <= 9.9999997473787516E-06)
			{
				m_stateTransitionBlending.Clear();
			}
		}

		private void AssignVariableValues(MyAnimationStateMachineNode freshNode)
		{
			freshNode.VariableAssignments.ForEach(delegate(MyAnimationStateMachineNode.VarAssignmentData data)
			{
				CurrentUpdateData.Controller.Variables.SetValue(data.VariableId, data.Value);
			});
		}
	}
}
