using Sandbox.Engine.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Game.Entities
{
	internal class AnimationPlayer
	{
		private class BoneInfo
		{
			private int m_currentKeyframe;

			private bool m_isConst;

			private MyCharacterBone m_assignedBone;

			public Quaternion Rotation;

			public Vector3 Translation;

			public AnimationPlayer Player;

			public MyAnimationClip.Keyframe Keyframe1;

			public MyAnimationClip.Keyframe Keyframe2;

			private MyAnimationClip.Bone m_clipBone;

			public int CurrentKeyframe
			{
				get
				{
					return m_currentKeyframe;
				}
				set
				{
					m_currentKeyframe = value;
					SetKeyframes();
				}
			}

			public MyAnimationClip.Bone ClipBone
			{
				get
				{
					return m_clipBone;
				}
				set
				{
					m_clipBone = value;
				}
			}

			public MyCharacterBone ModelBone => m_assignedBone;

			public BoneInfo()
			{
			}

			public BoneInfo(MyAnimationClip.Bone bone, AnimationPlayer player)
			{
				Init(bone, player);
			}

			public void Init(MyAnimationClip.Bone bone, AnimationPlayer player)
			{
				ClipBone = bone;
				Player = player;
				SetKeyframes();
				SetPosition(0f);
				m_isConst = (ClipBone.Keyframes.Count == 1);
			}

			public void Clear()
			{
				m_currentKeyframe = 0;
				m_isConst = false;
				m_assignedBone = null;
				Rotation = default(Quaternion);
				Translation = Vector3.Zero;
				Player = null;
				Keyframe1 = null;
				Keyframe2 = null;
				m_clipBone = null;
			}

			public void SetPosition(float position)
			{
				if (ClipBone == null)
				{
					return;
				}
				List<MyAnimationClip.Keyframe> keyframes = ClipBone.Keyframes;
				if (keyframes == null || Keyframe1 == null || Keyframe2 == null || keyframes.Count == 0)
				{
					return;
				}
				if (!m_isConst)
				{
					while ((double)position < Keyframe1.Time && m_currentKeyframe > 0)
					{
						m_currentKeyframe--;
						SetKeyframes();
					}
					while ((double)position >= Keyframe2.Time && m_currentKeyframe < ClipBone.Keyframes.Count - 2)
					{
						m_currentKeyframe++;
						SetKeyframes();
					}
					if (Keyframe1 == Keyframe2)
					{
						Rotation = Keyframe1.Rotation;
						Translation = Keyframe1.Translation;
					}
					else
					{
						float value = (float)(((double)position - Keyframe1.Time) * Keyframe2.InvTimeDiff);
						value = MathHelper.Clamp(value, 0f, 1f);
						Quaternion.Slerp(ref Keyframe1.Rotation, ref Keyframe2.Rotation, value, out Rotation);
						Vector3.Lerp(ref Keyframe1.Translation, ref Keyframe2.Translation, value, out Translation);
					}
				}
				AssignToCharacterBone();
			}

			public void AssignToCharacterBone()
			{
				if (m_assignedBone != null)
				{
					Quaternion rotation = Rotation;
					Quaternion additionalRotation = Player.m_skinnedEntity.GetAdditionalRotation(m_assignedBone.Name);
					rotation = Rotation * additionalRotation;
					m_assignedBone.SetCompleteTransform(ref Translation, ref rotation, Player.Weight);
				}
			}

			private void SetKeyframes()
			{
				if (ClipBone == null)
				{
					return;
				}
				if (ClipBone.Keyframes.Count > 0)
				{
					Keyframe1 = ClipBone.Keyframes[m_currentKeyframe];
					if (m_currentKeyframe == ClipBone.Keyframes.Count - 1)
					{
						Keyframe2 = Keyframe1;
					}
					else
					{
						Keyframe2 = ClipBone.Keyframes[m_currentKeyframe + 1];
					}
				}
				else
				{
					Keyframe1 = null;
					Keyframe2 = null;
				}
			}

			public void SetModel(MySkinnedEntity skinnedEntity)
			{
				if (ClipBone != null)
				{
					m_assignedBone = skinnedEntity.AnimationController.FindBone(ClipBone.Name, out int _);
				}
			}
		}

		public static bool ENABLE_ANIMATION_CACHE = false;

		public static bool ENABLE_ANIMATION_LODS = true;

		private static Dictionary<int, AnimationPlayer> CachedAnimationPlayers = new Dictionary<int, AnimationPlayer>();

		private float m_position;

		private float m_duration;

		private BoneInfo[] m_boneInfos;

		private Dictionary<float, List<BoneInfo>> m_boneLODs = new Dictionary<float, List<BoneInfo>>();

		private int m_boneCount;

		private MySkinnedEntity m_skinnedEntity;

		private MyFrameOption m_frameOption = MyFrameOption.PlayOnce;

		private float m_weight = 1f;

		private float m_timeScale = 1f;

		private bool m_initialized;

		private int m_currentLODIndex;

		private List<BoneInfo> m_currentLOD;

		private int m_hash;

		public string AnimationMwmPathDebug;

		public string AnimationNameDebug;

		private bool t = true;

		public MyStringId Name
		{
			get;
			private set;
		}

		[Browsable(false)]
		public float Position
		{
			get
			{
				return m_position;
			}
			set
			{
				float num = value;
				if (num > m_duration)
				{
					if (Looping)
					{
						num -= m_duration;
					}
					else
					{
						value = m_duration;
					}
				}
				m_position = num;
			}
		}

		public float Weight
		{
			get
			{
				return m_weight;
			}
			set
			{
				m_weight = value;
			}
		}

		public float TimeScale
		{
			get
			{
				return m_timeScale;
			}
			set
			{
				m_timeScale = value;
			}
		}

		public bool Looping => m_frameOption == MyFrameOption.Loop;

		public bool AtEnd
		{
			get
			{
				if (Position >= m_duration)
				{
					return m_frameOption != MyFrameOption.StayOnLastFrame;
				}
				return false;
			}
		}

		public bool IsInitialized => m_initialized;

		private static int GetAnimationPlayerHash(AnimationPlayer player)
		{
			float num = 10f;
			return (player.Name.GetHashCode() * 397) ^ (MyUtils.GetHash(player.m_skinnedEntity.Model.UniqueId) * 397) ^ ((int)((float)player.m_position.GetHashCode() * num) * 397) ^ player.m_currentLODIndex.GetHashCode();
		}

		public void Advance(float value)
		{
			if (m_frameOption != MyFrameOption.JustFirstFrame)
			{
				Position += value * m_timeScale;
				if (m_frameOption == MyFrameOption.StayOnLastFrame && Position > m_duration)
				{
					Position = m_duration;
				}
			}
			else
			{
				Position = 0f;
			}
		}

		public void UpdateBones(float distance)
		{
			if (!ENABLE_ANIMATION_LODS)
			{
				for (int i = 0; i < m_boneCount; i++)
				{
					m_boneInfos[i].SetPosition(m_position);
				}
				return;
			}
			m_currentLODIndex = -1;
			m_currentLOD = null;
			int num = 0;
			List<BoneInfo> list = null;
			foreach (KeyValuePair<float, List<BoneInfo>> boneLOD in m_boneLODs)
			{
				if (!(distance > boneLOD.Key))
				{
					break;
				}
				list = boneLOD.Value;
				m_currentLODIndex = num;
				m_currentLOD = list;
				num++;
			}
			if (list == null)
			{
				return;
			}
			if (CachedAnimationPlayers.TryGetValue(m_hash, out AnimationPlayer value) && value == this)
			{
				CachedAnimationPlayers.Remove(m_hash);
			}
			m_hash = GetAnimationPlayerHash(this);
			if (CachedAnimationPlayers.TryGetValue(m_hash, out value))
			{
				int animationPlayerHash = GetAnimationPlayerHash(value);
				if (m_hash != animationPlayerHash)
				{
					CachedAnimationPlayers.Remove(m_hash);
					value = null;
				}
			}
			if (value != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					list[j].Translation = value.m_currentLOD[j].Translation;
					list[j].Rotation = value.m_currentLOD[j].Rotation;
					list[j].AssignToCharacterBone();
				}
			}
			else if (list.Count > 0)
			{
				if (ENABLE_ANIMATION_CACHE)
				{
					CachedAnimationPlayers[m_hash] = this;
				}
				foreach (BoneInfo item in list)
				{
					item.SetPosition(m_position);
				}
			}
		}

		public void Initialize(AnimationPlayer player)
		{
			if (m_hash != 0)
			{
				CachedAnimationPlayers.Remove(m_hash);
				m_hash = 0;
			}
			Name = player.Name;
			m_duration = player.m_duration;
			m_skinnedEntity = player.m_skinnedEntity;
			m_weight = player.Weight;
			m_timeScale = player.m_timeScale;
			m_frameOption = player.m_frameOption;
			foreach (List<BoneInfo> value2 in m_boneLODs.Values)
			{
				value2.Clear();
			}
			m_boneCount = player.m_boneCount;
			if (m_boneInfos == null || m_boneInfos.Length < m_boneCount)
			{
				m_boneInfos = new BoneInfo[m_boneCount];
			}
			Position = player.Position;
			for (int i = 0; i < m_boneCount; i++)
			{
				BoneInfo boneInfo = m_boneInfos[i];
				if (boneInfo == null)
				{
					boneInfo = new BoneInfo();
					m_boneInfos[i] = boneInfo;
				}
				boneInfo.ClipBone = player.m_boneInfos[i].ClipBone;
				boneInfo.Player = this;
				boneInfo.SetModel(m_skinnedEntity);
				boneInfo.CurrentKeyframe = player.m_boneInfos[i].CurrentKeyframe;
				boneInfo.SetPosition(Position);
				if (player.m_boneLODs != null && boneInfo.ModelBone != null && ENABLE_ANIMATION_LODS)
				{
					foreach (KeyValuePair<float, List<BoneInfo>> boneLOD in player.m_boneLODs)
					{
						if (!m_boneLODs.TryGetValue(boneLOD.Key, out List<BoneInfo> value))
						{
							value = new List<BoneInfo>();
							m_boneLODs.Add(boneLOD.Key, value);
						}
						foreach (BoneInfo item in boneLOD.Value)
						{
							if (item.ModelBone != null && boneInfo.ModelBone.Name == item.ModelBone.Name)
							{
								value.Add(boneInfo);
								break;
							}
						}
					}
				}
				_ = MyFakes.ENABLE_BONES_AND_ANIMATIONS_DEBUG;
			}
			m_initialized = true;
		}

		public void Initialize(MyModel animationModel, string playerName, int clipIndex, MySkinnedEntity skinnedEntity, float weight, float timeScale, MyFrameOption frameOption, string[] explicitBones = null, Dictionary<float, string[]> boneLODs = null)
		{
			if (m_hash != 0)
			{
				CachedAnimationPlayers.Remove(m_hash);
				m_hash = 0;
			}
			MyAnimationClip myAnimationClip = animationModel.Animations.Clips[clipIndex];
			Name = MyStringId.GetOrCompute(animationModel.AssetName + " : " + playerName);
			m_duration = (float)myAnimationClip.Duration;
			m_skinnedEntity = skinnedEntity;
			m_weight = weight;
			m_timeScale = timeScale;
			m_frameOption = frameOption;
			foreach (List<BoneInfo> value4 in m_boneLODs.Values)
			{
				value4.Clear();
			}
			if (!m_boneLODs.TryGetValue(0f, out List<BoneInfo> value))
			{
				value = new List<BoneInfo>();
				m_boneLODs.Add(0f, value);
			}
			int num = (explicitBones == null) ? myAnimationClip.Bones.Count : explicitBones.Length;
			if (m_boneInfos == null || m_boneInfos.Length < num)
			{
				m_boneInfos = new BoneInfo[num];
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				MyAnimationClip.Bone bone = (explicitBones == null) ? myAnimationClip.Bones[i] : FindBone(myAnimationClip.Bones, explicitBones[i]);
				if (bone == null || bone.Keyframes.Count == 0)
				{
					continue;
				}
				BoneInfo boneInfo = m_boneInfos[num2];
				if (m_boneInfos[num2] == null)
				{
					boneInfo = new BoneInfo(bone, this);
				}
				else
				{
					boneInfo.Clear();
					boneInfo.Init(bone, this);
				}
				m_boneInfos[num2] = boneInfo;
				m_boneInfos[num2].SetModel(skinnedEntity);
				if (boneInfo.ModelBone != null)
				{
					value.Add(boneInfo);
					if (boneLODs != null)
					{
						foreach (KeyValuePair<float, string[]> boneLOD in boneLODs)
						{
							if (!m_boneLODs.TryGetValue(boneLOD.Key, out List<BoneInfo> value2))
							{
								value2 = new List<BoneInfo>();
								m_boneLODs.Add(boneLOD.Key, value2);
							}
							string[] value3 = boneLOD.Value;
							foreach (string b in value3)
							{
								if (boneInfo.ModelBone.Name == b)
								{
									value2.Add(boneInfo);
									break;
								}
							}
						}
					}
				}
				num2++;
			}
			m_boneCount = num2;
			Position = 0f;
			m_initialized = true;
		}

		public void Done()
		{
			m_initialized = false;
			CachedAnimationPlayers.Remove(m_hash);
		}

		private MyAnimationClip.Bone FindBone(List<MyAnimationClip.Bone> bones, string name)
		{
			foreach (MyAnimationClip.Bone bone in bones)
			{
				if (bone.Name == name)
				{
					return bone;
				}
			}
			return null;
		}
	}
}
