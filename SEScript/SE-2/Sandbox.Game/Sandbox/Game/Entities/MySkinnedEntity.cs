using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	public class MySkinnedEntity : MyEntity, IMySkinnedEntity
	{
		private class Sandbox_Game_Entities_MySkinnedEntity_003C_003EActor : IActivator, IActivator<MySkinnedEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MySkinnedEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySkinnedEntity CreateInstance()
			{
				return new MySkinnedEntity();
			}

			MySkinnedEntity IActivator<MySkinnedEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool UseNewAnimationSystem;

		private const int MAX_BONE_DECALS_COUNT = 10;

		private MyAnimationControllerComponent m_compAnimationController;

		private Dictionary<int, List<uint>> m_boneDecals = new Dictionary<int, List<uint>>();

		protected ulong m_actualUpdateFrame;

		protected ulong m_actualDrawFrame;

		protected Dictionary<string, Quaternion> m_additionalRotations = new Dictionary<string, Quaternion>();

		private Dictionary<string, MyAnimationPlayerBlendPair> m_animationPlayers = new Dictionary<string, MyAnimationPlayerBlendPair>();

		private Queue<MyAnimationCommand> m_commandQueue = new Queue<MyAnimationCommand>();

		private BoundingBoxD m_actualWorldAABB;

		private BoundingBoxD m_aabb;

		private List<MyAnimationSetData> m_continuingAnimSets = new List<MyAnimationSetData>();

		public MyAnimationControllerComponent AnimationController => m_compAnimationController;

		public Matrix[] BoneAbsoluteTransforms => m_compAnimationController.BoneAbsoluteTransforms;

		public Matrix[] BoneRelativeTransforms => m_compAnimationController.BoneRelativeTransforms;

		public List<MyBoneDecalUpdate> DecalBoneUpdates
		{
			get;
			private set;
		}

		internal ulong ActualUpdateFrame => m_actualUpdateFrame;

		public MySkinnedEntity()
		{
			base.Render = new MyRenderComponentSkinnedEntity();
			base.Render.EnableColorMaskHsv = true;
			base.Render.NeedsDraw = true;
			base.Render.CastShadows = true;
			base.Render.NeedsResolveCastShadow = false;
			base.Render.SkipIfTooSmall = false;
			MyEntityTerrainHeightProviderComponent myEntityTerrainHeightProviderComponent = new MyEntityTerrainHeightProviderComponent();
			base.Components.Add(myEntityTerrainHeightProviderComponent);
			m_compAnimationController = new MyAnimationControllerComponent(this, ObtainBones, myEntityTerrainHeightProviderComponent);
			base.Components.Add(m_compAnimationController);
			DecalBoneUpdates = new List<MyBoneDecalUpdate>();
		}

		public override void Init(StringBuilder displayName, string model, MyEntity parentObject, float? scale, string modelCollision = null)
		{
			base.Init(displayName, model, parentObject, scale, modelCollision);
			InitBones();
		}

		protected void InitBones()
		{
			ObtainBones();
			m_animationPlayers.Clear();
			AddAnimationPlayer("", null);
		}

		public void SetBoneLODs(Dictionary<float, string[]> boneLODs)
		{
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				animationPlayer.Value.SetBoneLODs(boneLODs);
			}
		}

		public virtual void UpdateControl(float distance)
		{
		}

		public virtual void UpdateAnimation(float distance)
		{
			m_compAnimationController.CameraDistance = distance;
			if ((!MyPerGameSettings.AnimateOnlyVisibleCharacters || Sandbox.Engine.Platform.Game.IsDedicated || (base.Render != null && base.Render.RenderObjectIDs.Length != 0 && MyRenderProxy.VisibleObjectsRead != null && MyRenderProxy.VisibleObjectsRead.Contains(base.Render.RenderObjectIDs[0]))) && distance < MyFakes.ANIMATION_UPDATE_DISTANCE)
			{
				if (UseNewAnimationSystem)
				{
					CalculateTransforms(distance);
					UpdateRenderObject();
				}
				else
				{
					UpdateContinuingSets();
					bool num = AdvanceAnimation();
					bool flag = ProcessCommands();
					UpdateAnimationState();
					if (num || flag)
					{
						CalculateTransforms(distance);
						UpdateRenderObject();
					}
				}
			}
			UpdateBoneDecals();
		}

		private void UpdateContinuingSets()
		{
			foreach (MyAnimationSetData continuingAnimSet in m_continuingAnimSets)
			{
				PlayAnimationSet(continuingAnimSet);
			}
		}

		private void UpdateBones(float distance)
		{
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				animationPlayer.Value.UpdateBones(distance);
			}
		}

		private bool AdvanceAnimation()
		{
			bool flag = false;
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				flag = (animationPlayer.Value.Advance() || flag);
			}
			return flag;
		}

		private void UpdateAnimationState()
		{
			foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in m_animationPlayers)
			{
				animationPlayer.Value.UpdateAnimationState();
			}
		}

		public virtual void ObtainBones()
		{
			MyCharacterBone[] array = new MyCharacterBone[base.Model.Bones.Length];
			Matrix[] array2 = new Matrix[base.Model.Bones.Length];
			Matrix[] array3 = new Matrix[base.Model.Bones.Length];
			for (int i = 0; i < base.Model.Bones.Length; i++)
			{
				MyModelBone myModelBone = base.Model.Bones[i];
				Matrix transform = myModelBone.Transform;
				MyCharacterBone parent = (myModelBone.Parent != -1) ? array[myModelBone.Parent] : null;
				MyCharacterBone myCharacterBone = array[i] = new MyCharacterBone(myModelBone.Name, parent, transform, i, array2, array3);
			}
			m_compAnimationController.SetCharacterBones(array, array2, array3);
		}

		public Quaternion GetAdditionalRotation(string bone)
		{
			Quaternion value = Quaternion.Identity;
			if (string.IsNullOrEmpty(bone))
			{
				return value;
			}
			if (m_additionalRotations.TryGetValue(bone, out value))
			{
				return value;
			}
			return Quaternion.Identity;
		}

		internal void AddAnimationPlayer(string name, string[] bones)
		{
			m_animationPlayers.Add(name, new MyAnimationPlayerBlendPair(this, bones, null, name));
		}

		internal bool TryGetAnimationPlayer(string name, out MyAnimationPlayerBlendPair player)
		{
			if (name == null)
			{
				name = "";
			}
			if (name == "Body")
			{
				name = "";
			}
			return m_animationPlayers.TryGetValue(name, out player);
		}

		internal DictionaryReader<string, MyAnimationPlayerBlendPair> GetAllAnimationPlayers()
		{
			return m_animationPlayers;
		}

		private void PlayAnimationSet(MyAnimationSetData animationSetData)
		{
			if (!(MyRandom.Instance.NextFloat(0f, 1f) < animationSetData.AnimationSet.Probability))
			{
				return;
			}
			float num = animationSetData.AnimationSet.AnimationItems.Sum((AnimationItem x) => x.Ratio);
			if (!(num > 0f))
			{
				return;
			}
			float num2 = MyRandom.Instance.NextFloat(0f, 1f);
			float num3 = 0f;
			AnimationItem[] animationItems = animationSetData.AnimationSet.AnimationItems;
			int num4 = 0;
			AnimationItem animationItem;
			while (true)
			{
				if (num4 < animationItems.Length)
				{
					animationItem = animationItems[num4];
					num3 += animationItem.Ratio / num;
					if (num2 < num3)
					{
						break;
					}
					num4++;
					continue;
				}
				return;
			}
			MyAnimationCommand myAnimationCommand = default(MyAnimationCommand);
			myAnimationCommand.AnimationSubtypeName = animationItem.Animation;
			myAnimationCommand.PlaybackCommand = MyPlaybackCommand.Play;
			myAnimationCommand.Area = animationSetData.Area;
			myAnimationCommand.BlendTime = animationSetData.BlendTime;
			myAnimationCommand.TimeScale = 1f;
			myAnimationCommand.KeepContinuingAnimations = true;
			MyAnimationCommand command = myAnimationCommand;
			ProcessCommand(ref command);
		}

		internal void PlayersPlay(string bonesArea, MyAnimationDefinition animDefinition, bool firstPerson, MyFrameOption frameOption, float blendTime, float timeScale)
		{
			string[] array = bonesArea.Split(new char[1]
			{
				' '
			});
			if (animDefinition.AnimationSets != null)
			{
				AnimationSet[] animationSets = animDefinition.AnimationSets;
				for (int i = 0; i < animationSets.Length; i++)
				{
					AnimationSet animationSet = animationSets[i];
					MyAnimationSetData myAnimationSetData = default(MyAnimationSetData);
					myAnimationSetData.BlendTime = blendTime;
					myAnimationSetData.Area = bonesArea;
					myAnimationSetData.AnimationSet = animationSet;
					MyAnimationSetData myAnimationSetData2 = myAnimationSetData;
					if (animationSet.Continuous)
					{
						m_continuingAnimSets.Add(myAnimationSetData2);
					}
					else
					{
						PlayAnimationSet(myAnimationSetData2);
					}
				}
			}
			else
			{
				string[] array2 = array;
				foreach (string playerName in array2)
				{
					PlayerPlay(playerName, animDefinition, firstPerson, frameOption, blendTime, timeScale);
				}
			}
		}

		internal void PlayerPlay(string playerName, MyAnimationDefinition animDefinition, bool firstPerson, MyFrameOption frameOption, float blendTime, float timeScale)
		{
			if (TryGetAnimationPlayer(playerName, out MyAnimationPlayerBlendPair player))
			{
				player.Play(animDefinition, firstPerson, frameOption, blendTime, timeScale);
			}
		}

		internal void PlayerStop(string playerName, float blendTime)
		{
			if (TryGetAnimationPlayer(playerName, out MyAnimationPlayerBlendPair player))
			{
				player.Stop(blendTime);
			}
		}

		protected virtual void CalculateTransforms(float distance)
		{
			if (!UseNewAnimationSystem)
			{
				UpdateBones(distance);
			}
			AnimationController.UpdateTransformations();
		}

		[Obsolete]
		protected bool TryGetAnimationDefinition(string animationSubtypeName, out MyAnimationDefinition animDefinition)
		{
			if (animationSubtypeName == null)
			{
				animDefinition = null;
				return false;
			}
			animDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(animationSubtypeName);
			if (animDefinition == null)
			{
				string text = Path.Combine(MyFileSystem.ContentPath, animationSubtypeName);
				if (MyFileSystem.FileExists(text))
				{
					animDefinition = new MyAnimationDefinition
					{
						AnimationModel = text,
						ClipIndex = 0
					};
					return true;
				}
				animDefinition = null;
				return false;
			}
			return true;
		}

		protected bool ProcessCommands()
		{
			if (m_commandQueue.Count > 0)
			{
				MyAnimationCommand command = m_commandQueue.Dequeue();
				ProcessCommand(ref command);
				return true;
			}
			return false;
		}

		protected void AddBoneDecal(uint decalId, int boneIndex)
		{
			if (!m_boneDecals.TryGetValue(boneIndex, out List<uint> value))
			{
				value = new List<uint>(10);
				m_boneDecals.Add(boneIndex, value);
			}
			if (value.Count == value.Capacity)
			{
				MyDecals.RemoveDecal(value[0]);
				value.RemoveAt(0);
			}
			value.Add(decalId);
		}

		private void UpdateBoneDecals()
		{
			DecalBoneUpdates.Clear();
			foreach (KeyValuePair<int, List<uint>> boneDecal in m_boneDecals)
			{
				foreach (uint item in boneDecal.Value)
				{
					DecalBoneUpdates.Add(new MyBoneDecalUpdate
					{
						BoneID = boneDecal.Key,
						DecalID = item
					});
				}
			}
		}

		protected void FlushAnimationQueue()
		{
			while (m_commandQueue.Count > 0)
			{
				ProcessCommands();
			}
		}

		private void ProcessCommand(ref MyAnimationCommand command)
		{
			if (command.PlaybackCommand == MyPlaybackCommand.Play)
			{
				if (TryGetAnimationDefinition(command.AnimationSubtypeName, out MyAnimationDefinition animDefinition))
				{
					string bonesArea = animDefinition.InfluenceArea;
					MyFrameOption frameOption = command.FrameOption;
					if (frameOption == MyFrameOption.Default)
					{
						frameOption = ((!animDefinition.Loop) ? MyFrameOption.PlayOnce : MyFrameOption.Loop);
					}
					bool useFirstPersonVersion = false;
					OnAnimationPlay(animDefinition, command, ref bonesArea, ref frameOption, ref useFirstPersonVersion);
					if (!string.IsNullOrEmpty(command.Area))
					{
						bonesArea = command.Area;
					}
					if (bonesArea == null)
					{
						bonesArea = "";
					}
					if (!command.KeepContinuingAnimations)
					{
						m_continuingAnimSets.Clear();
					}
					if (!UseNewAnimationSystem)
					{
						PlayersPlay(bonesArea, animDefinition, useFirstPersonVersion, frameOption, command.BlendTime, command.TimeScale);
					}
				}
			}
			else
			{
				if (command.PlaybackCommand != MyPlaybackCommand.Stop)
				{
					return;
				}
				string[] array = ((command.Area == null) ? "" : command.Area).Split(new char[1]
				{
					' '
				});
				if (!UseNewAnimationSystem)
				{
					string[] array2 = array;
					foreach (string playerName in array2)
					{
						PlayerStop(playerName, command.BlendTime);
					}
				}
			}
		}

		public virtual void AddCommand(MyAnimationCommand command, bool sync = false)
		{
			m_commandQueue.Enqueue(command);
		}

		protected virtual void OnAnimationPlay(MyAnimationDefinition animDefinition, MyAnimationCommand command, ref string bonesArea, ref MyFrameOption frameOption, ref bool useFirstPersonVersion)
		{
		}

		protected void UpdateRenderObject()
		{
		}
	}
}
