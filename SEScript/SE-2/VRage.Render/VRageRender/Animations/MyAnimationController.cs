using System.Collections.Generic;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Animation controller contains and drives skeletal animations.
	/// It also serves as an abstraction layer, hiding low/level classes.
	/// </summary>
	public class MyAnimationController
	{
		/// <summary>
		/// Simple pool allocator for bone results.
		/// </summary>
		public class MyResultBonesPool
		{
			private int m_boneCount;

			private readonly List<List<MyAnimationClip.BoneState>> m_freeToUse = new List<List<MyAnimationClip.BoneState>>(8);

			private readonly List<List<MyAnimationClip.BoneState>> m_taken = new List<List<MyAnimationClip.BoneState>>(8);

			private List<MyAnimationClip.BoneState> m_restPose;

			private List<MyAnimationClip.BoneState> m_currentDefaultPose;

			/// <summary>
			/// Set the new bone count and default (rest) pose.
			/// </summary>
			public void Reset(MyCharacterBone[] restPoseBones)
			{
				m_freeToUse.Clear();
				m_taken.Clear();
				if (restPoseBones == null)
				{
					m_boneCount = 0;
					m_restPose = null;
					return;
				}
				int num = m_boneCount = restPoseBones.Length;
				m_restPose = new List<MyAnimationClip.BoneState>(num);
				for (int i = 0; i < num; i++)
				{
					m_restPose.Add(new MyAnimationClip.BoneState
					{
						Translation = restPoseBones[i].BindTransform.Translation,
						Rotation = Quaternion.CreateFromRotationMatrix(restPoseBones[i].BindTransform)
					});
				}
				m_currentDefaultPose = m_restPose;
			}

			/// <summary>
			/// Set the link to default pose = default bone positions and rotations given when using this allocator.
			/// If null is given, rest pose is used.
			/// </summary>
			public void SetDefaultPose(List<MyAnimationClip.BoneState> linkToDefaultPose)
			{
				m_currentDefaultPose = (linkToDefaultPose ?? m_restPose);
			}

			public bool IsValid()
			{
				return m_currentDefaultPose != null;
			}

			public void FreeAll()
			{
				foreach (List<MyAnimationClip.BoneState> item in m_taken)
				{
					m_freeToUse.Add(item);
				}
				m_taken.Clear();
			}

			/// <summary>
			/// Allocate array of bones from pool. Bones are in the rest (bind) position by default.
			/// </summary>
			/// <returns></returns>
			public List<MyAnimationClip.BoneState> Alloc()
			{
				if (m_freeToUse.Count == 0)
				{
					List<MyAnimationClip.BoneState> list = new List<MyAnimationClip.BoneState>(m_boneCount);
					for (int i = 0; i < m_boneCount; i++)
					{
						list.Add(new MyAnimationClip.BoneState
						{
							Translation = m_currentDefaultPose[i].Translation,
							Rotation = m_currentDefaultPose[i].Rotation
						});
					}
					m_taken.Add(list);
					return list;
				}
				List<MyAnimationClip.BoneState> list2 = m_freeToUse[m_freeToUse.Count - 1];
				m_freeToUse.RemoveAt(m_freeToUse.Count - 1);
				m_taken.Add(list2);
				for (int j = 0; j < m_boneCount; j++)
				{
					list2[j].Translation = m_currentDefaultPose[j].Translation;
					list2[j].Rotation = m_currentDefaultPose[j].Rotation;
				}
				return list2;
			}

			public void Free(List<MyAnimationClip.BoneState> toBeFreed)
			{
				int num = -1;
				for (int num2 = m_taken.Count - 1; num2 >= 0; num2--)
				{
					if (m_taken[num2] == toBeFreed)
					{
						num = num2;
						break;
					}
				}
				if (num != -1)
				{
					m_freeToUse.Add(m_taken[num]);
					m_taken.RemoveAtFast(num);
				}
			}
		}

		private readonly List<MyAnimationStateMachine> m_layers;

		private readonly Dictionary<string, int> m_tableLayerNameToIndex;

		public readonly MyResultBonesPool ResultBonesPool;

		public readonly MyAnimationInverseKinematics InverseKinematics;

		public int FrameCounter
		{
			get;
			private set;
		}

		private bool IkUpdateEnabled
		{
			get;
			set;
		}

		public MyAnimationVariableStorage Variables
		{
			get;
			private set;
		}

		public MyAnimationController()
		{
			m_layers = new List<MyAnimationStateMachine>(1);
			m_tableLayerNameToIndex = new Dictionary<string, int>(1);
			Variables = new MyAnimationVariableStorage();
			ResultBonesPool = new MyResultBonesPool();
			InverseKinematics = new MyAnimationInverseKinematics();
			FrameCounter = 0;
			IkUpdateEnabled = true;
		}

		public MyAnimationStateMachine GetLayerByName(string layerName)
		{
			if (m_tableLayerNameToIndex.TryGetValue(layerName, out int value))
			{
				return m_layers[value];
			}
			return null;
		}

		public MyAnimationStateMachine GetLayerByIndex(int index)
		{
			if (index >= 0 && index < m_layers.Count)
			{
				return m_layers[index];
			}
			return null;
		}

		/// <summary>
		/// Create animation layer with unique name. Parameter insertionIndex can be left -1 to add the layer at the end.
		/// If layer with same name is already present, method fails and returns null.
		/// </summary>
		public MyAnimationStateMachine CreateLayer(string name, int insertionIndex = -1)
		{
			if (GetLayerByName(name) != null)
			{
				return null;
			}
			MyAnimationStateMachine myAnimationStateMachine = new MyAnimationStateMachine();
			myAnimationStateMachine.Name = name;
			if (insertionIndex != -1)
			{
				m_tableLayerNameToIndex.Add(name, insertionIndex);
				m_layers.Insert(insertionIndex, myAnimationStateMachine);
			}
			else
			{
				m_tableLayerNameToIndex.Add(name, m_layers.Count);
				m_layers.Add(myAnimationStateMachine);
			}
			return myAnimationStateMachine;
		}

		public void DeleteAllLayers()
		{
			m_tableLayerNameToIndex.Clear();
			m_layers.Clear();
		}

		public int GetLayerCount()
		{
			return m_layers.Count;
		}

		/// <summary>
		/// Update this animation controller.
		/// </summary>
		/// <param name="animationUpdateData">See commentary in MyAnimationUpdateData</param>
		public void Update(ref MyAnimationUpdateData animationUpdateData)
		{
			FrameCounter++;
			if (animationUpdateData.CharacterBones == null || !ResultBonesPool.IsValid())
			{
				return;
			}
			if (animationUpdateData.Controller == null)
			{
				animationUpdateData.Controller = this;
			}
			ResultBonesPool.FreeAll();
			Variables.SetValue(MyAnimationVariableStorageHints.StrIdRandomStable, MyRandom.Instance.NextFloat());
			if (m_layers.Count > 0)
			{
				ResultBonesPool.SetDefaultPose(null);
				m_layers[0].Update(ref animationUpdateData);
			}
			for (int i = 1; i < m_layers.Count; i++)
			{
				MyAnimationStateMachine myAnimationStateMachine = m_layers[i];
				MyAnimationUpdateData myAnimationUpdateData = animationUpdateData;
				animationUpdateData.LayerBoneMask = null;
				animationUpdateData.BonesResult = null;
				ResultBonesPool.SetDefaultPose((myAnimationStateMachine.Mode == MyAnimationStateMachine.MyBlendingMode.Replace) ? myAnimationUpdateData.BonesResult : null);
				m_layers[i].Update(ref animationUpdateData);
				if (animationUpdateData.BonesResult == null || m_layers[i].CurrentNode == null || ((MyAnimationStateMachineNode)m_layers[i].CurrentNode).RootAnimationNode == null || ((MyAnimationStateMachineNode)m_layers[i].CurrentNode).RootAnimationNode is MyAnimationTreeNodeDummy)
				{
					animationUpdateData = myAnimationUpdateData;
					continue;
				}
				int count = animationUpdateData.BonesResult.Count;
				List<MyAnimationClip.BoneState> bonesResult = myAnimationUpdateData.BonesResult;
				List<MyAnimationClip.BoneState> bonesResult2 = animationUpdateData.BonesResult;
				MyCharacterBone[] characterBones = myAnimationUpdateData.CharacterBones;
				if (myAnimationStateMachine.Mode == MyAnimationStateMachine.MyBlendingMode.Replace)
				{
					for (int j = 0; j < count; j++)
					{
						if (!animationUpdateData.LayerBoneMask[j])
						{
							bonesResult2[j].Translation = bonesResult[j].Translation;
							bonesResult2[j].Rotation = bonesResult[j].Rotation;
						}
					}
				}
				else if (myAnimationStateMachine.Mode == MyAnimationStateMachine.MyBlendingMode.Add)
				{
					for (int k = 0; k < count; k++)
					{
						if (animationUpdateData.LayerBoneMask[k])
						{
							characterBones[k].GetCompleteTransform(ref bonesResult2[k].Translation, ref bonesResult2[k].Rotation, out Vector3 completeTranslation, out Quaternion completeRotation);
							bonesResult2[k].Translation = bonesResult[k].Translation + Vector3.Transform(completeTranslation, bonesResult[k].Rotation);
							bonesResult2[k].Rotation = bonesResult[k].Rotation * completeRotation;
						}
						else
						{
							bonesResult2[k].Translation = bonesResult[k].Translation;
							bonesResult2[k].Rotation = bonesResult[k].Rotation;
						}
					}
				}
				ResultBonesPool.Free(myAnimationUpdateData.BonesResult);
			}
		}

		/// <summary>
		/// Trigger an action in all layers. 
		/// If there is a transition having given (non-null) name, it is followed immediatelly.
		/// Conditions of transition are ignored.
		/// </summary>
		public void TriggerAction(MyStringId actionName)
		{
			foreach (MyAnimationStateMachine layer in m_layers)
			{
				layer.TriggerAction(actionName);
			}
		}

		/// <summary>
		/// Trigger an action in the specific layers. 
		/// If there is a transition having given (non-null) name, it is followed immediatelly.
		/// Conditions of transition are ignored.
		/// </summary>
		public void TriggerAction(MyStringId actionName, string[] layers)
		{
			if (layers != null && layers.Length != 0)
			{
				for (int i = 0; i < layers.Length; i++)
				{
					GetLayerByName(layers[i])?.TriggerAction(actionName);
				}
			}
		}

		/// <summary>
		/// Perform inverse kinematics.
		/// </summary>
		public void UpdateInverseKinematics(MyCharacterBone[] characterBonesStorage)
		{
			if (Variables != null && IkUpdateEnabled)
			{
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdFlying, out float value);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdFalling, out float value2);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdDead, out float value3);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdSitting, out float value4);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdJumping, out float value5);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdSpeed, out float value6);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdFirstPerson, out float value7);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdForcedFirstPerson, out float value8);
				Variables.GetValue(MyAnimationVariableStorageHints.StrIdLadder, out float value9);
				if (value6 < 0.25f)
				{
					InverseKinematics.ClearCharacterOffsetFilteringSamples();
				}
				if (value4 > 0f)
				{
					InverseKinematics.ResetIkInfluence();
					InverseKinematics.ClearCharacterOffsetFilteringSamples();
				}
				InverseKinematics.SolveFeet(value <= 0f && value2 <= 0f && value3 <= 0f && value5 <= 0f && value4 <= 0f && value9 <= 0f, characterBonesStorage, value7 <= 0f || value8 > 0f);
			}
		}
	}
}
