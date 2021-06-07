using System;
using VRageMath;

namespace VRageRender.Animations
{
	/// <summary>
	/// Node of animation tree: single track. Contains reference to animation clip.
	/// </summary>
	public class MyAnimationTreeNodeTrack : MyAnimationTreeNode
	{
		private MyAnimationClip m_animationClip;

		private double m_localTime;

		private double m_speed = 1.0;

		private int[] m_currentKeyframes;

		private int[] m_boneIndicesMapping;

		private bool m_loop = true;

		private bool m_interpolate = true;

		private int m_timeAdvancedOnFrameNum;

		private MyAnimationStateMachine m_synchronizeWithLayerRef;

		private string m_synchronizeWithLayerName;

		public bool Loop
		{
			get
			{
				return m_loop;
			}
			set
			{
				m_loop = value;
			}
		}

		public double Speed
		{
			get
			{
				return m_speed;
			}
			set
			{
				m_speed = value;
			}
		}

		public bool Interpolate
		{
			get
			{
				return m_interpolate;
			}
			set
			{
				m_interpolate = value;
			}
		}

		public string SynchronizeWithLayer
		{
			get
			{
				return m_synchronizeWithLayerName;
			}
			set
			{
				m_synchronizeWithLayerName = value;
				m_synchronizeWithLayerRef = null;
			}
		}

		public MyAnimationClip AnimationClip => m_animationClip;

		public bool SetClip(MyAnimationClip animationClip)
		{
			m_animationClip = animationClip;
			m_currentKeyframes = ((animationClip != null) ? new int[animationClip.Bones.Count] : null);
			m_boneIndicesMapping = null;
			return true;
		}

		public override void Update(ref MyAnimationUpdateData data)
		{
			data.BonesResult = data.Controller.ResultBonesPool.Alloc();
			if (m_animationClip != null && m_animationClip.Bones != null)
			{
				if (m_boneIndicesMapping == null)
				{
					RebuildBoneIndices(data.CharacterBones);
				}
				if (!ProcessLayerTimeSync(ref data) && m_timeAdvancedOnFrameNum != data.Controller.FrameCounter)
				{
					m_timeAdvancedOnFrameNum = data.Controller.FrameCounter;
					m_localTime += data.DeltaTimeInSeconds * Speed;
					if (m_loop)
					{
						while (m_localTime >= m_animationClip.Duration)
						{
							m_localTime -= m_animationClip.Duration;
						}
						while (m_localTime < 0.0)
						{
							m_localTime += m_animationClip.Duration;
						}
					}
					else if (m_localTime >= m_animationClip.Duration)
					{
						m_localTime = m_animationClip.Duration;
					}
					else if (m_localTime < 0.0)
					{
						m_localTime = 0.0;
					}
				}
				UpdateKeyframeIndices();
				for (int i = 0; i < m_animationClip.Bones.Count; i++)
				{
					MyAnimationClip.Bone bone = m_animationClip.Bones[i];
					int num = m_currentKeyframes[i];
					int num2 = num + 1;
					if (num2 >= bone.Keyframes.Count)
					{
						num2 = Math.Max(0, bone.Keyframes.Count - 1);
					}
					int num3 = m_boneIndicesMapping[i];
					if (num3 >= 0 && num3 < data.BonesResult.Count && data.LayerBoneMask[num3])
					{
						if (num != num2 && m_interpolate)
						{
							MyAnimationClip.Keyframe keyframe = bone.Keyframes[num];
							MyAnimationClip.Keyframe keyframe2 = bone.Keyframes[num2];
							float value = (float)((m_localTime - keyframe.Time) * keyframe2.InvTimeDiff);
							value = MathHelper.Clamp(value, 0f, 1f);
							Quaternion.Slerp(ref keyframe.Rotation, ref keyframe2.Rotation, value, out data.BonesResult[num3].Rotation);
							Vector3.Lerp(ref keyframe.Translation, ref keyframe2.Translation, value, out data.BonesResult[num3].Translation);
						}
						else if (bone.Keyframes.Count != 0)
						{
							data.BonesResult[num3].Rotation = bone.Keyframes[num].Rotation;
							data.BonesResult[num3].Translation = bone.Keyframes[num].Translation;
						}
					}
				}
			}
			data.AddVisitedTreeNodesPathPoint(-1);
		}

		/// <summary>
		/// Synchronize time with defined layer. Returns false if the time is not synchronized.
		/// </summary>
		private bool ProcessLayerTimeSync(ref MyAnimationUpdateData data)
		{
			if (m_synchronizeWithLayerRef == null)
			{
				if (m_synchronizeWithLayerName == null)
				{
					return false;
				}
				m_synchronizeWithLayerRef = data.Controller.GetLayerByName(m_synchronizeWithLayerName);
				if (m_synchronizeWithLayerRef == null)
				{
					return false;
				}
			}
			MyAnimationStateMachineNode myAnimationStateMachineNode = m_synchronizeWithLayerRef.CurrentNode as MyAnimationStateMachineNode;
			if (myAnimationStateMachineNode == null || myAnimationStateMachineNode.RootAnimationNode == null)
			{
				return false;
			}
			SetLocalTimeNormalized(myAnimationStateMachineNode.RootAnimationNode.GetLocalTimeNormalized());
			return true;
		}

		public override float GetLocalTimeNormalized()
		{
			if (m_animationClip != null && m_animationClip.Duration > 0.0)
			{
				if (!(m_localTime < m_animationClip.Duration))
				{
					if (!Loop)
					{
						return 1f;
					}
					return 0.99999f;
				}
				return (float)(m_localTime / m_animationClip.Duration);
			}
			return 0f;
		}

		public override void SetLocalTimeNormalized(float normalizedTime)
		{
			if (m_animationClip != null)
			{
				m_localTime = (double)normalizedTime * m_animationClip.Duration;
			}
		}

		private void UpdateKeyframeIndices()
		{
			if (m_animationClip == null || m_animationClip.Bones == null)
			{
				return;
			}
			for (int i = 0; i < m_animationClip.Bones.Count; i++)
			{
				MyAnimationClip.Bone bone = m_animationClip.Bones[i];
				int j;
				for (j = m_currentKeyframes[i]; j < bone.Keyframes.Count - 2 && m_localTime > bone.Keyframes[j + 1].Time; j++)
				{
				}
				while (j > 0 && m_localTime < bone.Keyframes[j].Time)
				{
					j--;
				}
				m_currentKeyframes[i] = j;
			}
		}

		private void RebuildBoneIndices(MyCharacterBone[] characterBones)
		{
			m_boneIndicesMapping = new int[m_animationClip.Bones.Count];
			for (int i = 0; i < m_animationClip.Bones.Count; i++)
			{
				m_boneIndicesMapping[i] = -1;
				for (int j = 0; j < characterBones.Length; j++)
				{
					if (m_animationClip.Bones[i].Name == characterBones[j].Name)
					{
						m_boneIndicesMapping[i] = j;
						break;
					}
				}
				_ = m_boneIndicesMapping[i];
				_ = -1;
			}
		}
	}
}
