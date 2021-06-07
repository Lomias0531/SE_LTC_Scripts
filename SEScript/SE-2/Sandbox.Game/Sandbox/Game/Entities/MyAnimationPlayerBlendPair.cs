using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.Definitions.Animation;
using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal class MyAnimationPlayerBlendPair
	{
		public enum AnimationBlendState
		{
			Stopped,
			BlendIn,
			Playing,
			BlendOut
		}

		public AnimationPlayer BlendPlayer = new AnimationPlayer();

		public AnimationPlayer ActualPlayer = new AnimationPlayer();

		private AnimationBlendState m_state;

		public float m_currentBlendTime;

		public float m_totalBlendTime;

		private string[] m_bones;

		private MySkinnedEntity m_skinnedEntity;

		private string m_name;

		private Dictionary<float, string[]> m_boneLODs;

		public MyAnimationPlayerBlendPair(MySkinnedEntity skinnedEntity, string[] bones, Dictionary<float, string[]> boneLODs, string name)
		{
			m_bones = bones;
			m_skinnedEntity = skinnedEntity;
			m_boneLODs = boneLODs;
			m_name = name;
		}

		public void UpdateBones(float distance)
		{
			if (m_state != 0)
			{
				if (BlendPlayer.IsInitialized)
				{
					BlendPlayer.UpdateBones(distance);
				}
				if (ActualPlayer.IsInitialized)
				{
					ActualPlayer.UpdateBones(distance);
				}
			}
		}

		public bool Advance()
		{
			if (m_state != 0)
			{
				float num = 0.0166666675f;
				m_currentBlendTime += num * ActualPlayer.TimeScale;
				ActualPlayer.Advance(num);
				if (!ActualPlayer.Looping && ActualPlayer.AtEnd && m_state == AnimationBlendState.Playing)
				{
					Stop(m_totalBlendTime);
				}
				return true;
			}
			return false;
		}

		public void UpdateAnimationState()
		{
			float num = 0f;
			if (ActualPlayer.IsInitialized && m_currentBlendTime > 0f)
			{
				num = 1f;
				if (m_totalBlendTime > 0f)
				{
					num = MathHelper.Clamp(m_currentBlendTime / m_totalBlendTime, 0f, 1f);
				}
			}
			if (!ActualPlayer.IsInitialized)
			{
				return;
			}
			if (m_state == AnimationBlendState.BlendOut)
			{
				ActualPlayer.Weight = 1f - num;
				if (num == 1f)
				{
					ActualPlayer.Done();
					m_state = AnimationBlendState.Stopped;
				}
			}
			if (m_state == AnimationBlendState.BlendIn)
			{
				if (m_totalBlendTime == 0f)
				{
					num = 1f;
				}
				ActualPlayer.Weight = num;
				if (BlendPlayer.IsInitialized)
				{
					BlendPlayer.Weight = 1f;
				}
				if (num == 1f)
				{
					m_state = AnimationBlendState.Playing;
					BlendPlayer.Done();
				}
			}
		}

		public void Play(MyAnimationDefinition animationDefinition, bool firstPerson, MyFrameOption frameOption, float blendTime, float timeScale)
		{
			string text = (firstPerson && !string.IsNullOrEmpty(animationDefinition.AnimationModelFPS)) ? animationDefinition.AnimationModelFPS : animationDefinition.AnimationModel;
			if (string.IsNullOrEmpty(animationDefinition.AnimationModel))
			{
				return;
			}
			if (animationDefinition.Status == MyAnimationDefinition.AnimationStatus.Unchecked && !MyFileSystem.FileExists(Path.IsPathRooted(text) ? text : Path.Combine(MyFileSystem.ContentPath, text)))
			{
				animationDefinition.Status = MyAnimationDefinition.AnimationStatus.Failed;
				return;
			}
			animationDefinition.Status = MyAnimationDefinition.AnimationStatus.OK;
			MyModel modelOnlyAnimationData = MyModels.GetModelOnlyAnimationData(text);
			if ((modelOnlyAnimationData == null || modelOnlyAnimationData.Animations != null) && modelOnlyAnimationData.Animations.Clips.Count != 0 && modelOnlyAnimationData.Animations.Clips.Count > animationDefinition.ClipIndex)
			{
				if (ActualPlayer.IsInitialized)
				{
					BlendPlayer.Initialize(ActualPlayer);
				}
				ActualPlayer.Initialize(modelOnlyAnimationData, m_name, animationDefinition.ClipIndex, m_skinnedEntity, 1f, timeScale, frameOption, m_bones, m_boneLODs);
				ActualPlayer.AnimationMwmPathDebug = text;
				ActualPlayer.AnimationNameDebug = animationDefinition.Id.SubtypeName;
				m_state = AnimationBlendState.BlendIn;
				m_currentBlendTime = 0f;
				m_totalBlendTime = blendTime;
			}
		}

		public void Stop(float blendTime)
		{
			if (m_state != 0)
			{
				BlendPlayer.Done();
				m_state = AnimationBlendState.BlendOut;
				m_currentBlendTime = 0f;
				m_totalBlendTime = blendTime;
			}
		}

		public AnimationBlendState GetState()
		{
			return m_state;
		}

		public void SetBoneLODs(Dictionary<float, string[]> boneLODs)
		{
			m_boneLODs = boneLODs;
		}
	}
}
