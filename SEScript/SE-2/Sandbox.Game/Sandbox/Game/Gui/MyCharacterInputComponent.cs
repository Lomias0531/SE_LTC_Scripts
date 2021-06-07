using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.Game.SessionComponents;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Animations;

namespace Sandbox.Game.Gui
{
	internal class MyCharacterInputComponent : MyDebugComponent
	{
		private bool m_toggleMovementState;

		private bool m_toggleShowSkeleton;

		private const int m_maxLastAnimationActions = 20;

		private List<string> m_lastAnimationActions = new List<string>(20);

		private Dictionary<MyCharacterBone, int> m_boneRefToIndex;

		private string m_animationControllerName;

		public override string GetName()
		{
			return "Character";
		}

		public MyCharacterInputComponent()
		{
			AddShortcut(MyKeys.U, newPress: true, control: false, shift: false, alt: false, () => "Spawn new character", delegate
			{
				SpawnCharacter();
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: false, control: false, shift: false, alt: false, () => "Kill everyone around you", delegate
			{
				KillEveryoneAround();
				return true;
			});
			AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Use next ship", delegate
			{
				UseNextShip();
				return true;
			});
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Toggle skeleton view", delegate
			{
				ToggleSkeletonView();
				return true;
			});
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Reload animation tracks", delegate
			{
				ReloadAnimations();
				return true;
			});
			AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Toggle character movement status", delegate
			{
				ShowMovementState();
				return true;
			});
		}

		private void KillEveryoneAround()
		{
			if (MySession.Static.LocalCharacter != null && Sync.IsServer && MySession.Static.HasCreativeRights && MySession.Static.IsAdminMenuEnabled)
			{
				Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
				Vector3D value = new Vector3D(25.0, 25.0, 25.0);
				BoundingBoxD box = new BoundingBoxD(position - value, position + value);
				List<MyEntity> list = new List<MyEntity>();
				MyGamePruningStructure.GetAllEntitiesInBox(ref box, list);
				foreach (MyEntity item in list)
				{
					MyCharacter myCharacter = item as MyCharacter;
					if (myCharacter != null && item != MySession.Static.LocalCharacter)
					{
						myCharacter.DoDamage(1000000f, MyDamageType.Debug, updateSync: true, 0L);
					}
				}
				MyRenderProxy.DebugDrawAABB(box, Color.Red, 0.5f, 1f, depthRead: true, shaded: true);
			}
		}

		public override bool HandleInput()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			return base.HandleInput();
		}

		private void ToggleSkeletonView()
		{
			m_toggleShowSkeleton = !m_toggleShowSkeleton;
		}

		private void ReloadAnimations()
		{
			if (MySession.Static.LocalCharacter != null)
			{
				foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> allAnimationPlayer in MySession.Static.LocalCharacter.GetAllAnimationPlayers())
				{
					MySession.Static.LocalCharacter.PlayerStop(allAnimationPlayer.Key, 0f);
				}
			}
			foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				MyModels.GetModel(animationDefinition.AnimationModel)?.UnloadData();
				MyModels.GetModel(animationDefinition.AnimationModelFPS)?.UnloadData();
			}
			MySessionComponentAnimationSystem.Static.ReloadMwmTracks();
		}

		public static MyCharacter SpawnCharacter(string model = null)
		{
			MyCharacter myCharacter = (MySession.Static.LocalHumanPlayer == null) ? null : MySession.Static.LocalHumanPlayer.Identity.Character;
			Vector3? colorMask = null;
			string characterName = (MySession.Static.LocalHumanPlayer == null) ? "" : MySession.Static.LocalHumanPlayer.Identity.DisplayName;
			string text = (MySession.Static.LocalHumanPlayer == null) ? MyCharacter.DefaultModel : MySession.Static.LocalHumanPlayer.Identity.Model;
			long identityId = (MySession.Static.LocalHumanPlayer == null) ? 0 : MySession.Static.LocalHumanPlayer.Identity.IdentityId;
			if (myCharacter != null)
			{
				colorMask = myCharacter.ColorMask;
			}
			return MyCharacter.CreateCharacter(MatrixD.CreateTranslation(MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 6f + MySector.MainCamera.LeftVector * 3f), Vector3.Zero, characterName, model ?? text, colorMask, null, findNearPos: false, AIMode: false, null, useInventory: true, identityId);
		}

		public static void UseNextShip()
		{
			MyCockpit myCockpit = null;
			object obj = null;
			foreach (MyCubeGrid item in MyEntities.GetEntities().OfType<MyCubeGrid>())
			{
				foreach (MyCockpit item2 in from s in item.GetBlocks()
					select s.FatBlock as MyCockpit into s
					where s != null
					select s)
				{
					if (myCockpit == null && item2.Pilot == null)
					{
						myCockpit = item2;
					}
					if (obj == MySession.Static.ControlledEntity)
					{
						if (item2.Pilot == null)
						{
							UseCockpit(item2);
							return;
						}
					}
					else
					{
						obj = item2;
					}
				}
			}
			if (myCockpit != null)
			{
				UseCockpit(myCockpit);
			}
		}

		private static void UseCockpit(MyCockpit cockpit)
		{
			if (MySession.Static.LocalHumanPlayer != null)
			{
				if (MySession.Static.ControlledEntity is MyCockpit)
				{
					MySession.Static.ControlledEntity.Use();
				}
				cockpit.RequestUse(UseActionEnum.Manipulate, MySession.Static.LocalHumanPlayer.Identity.Character);
				cockpit.RemoveOriginalPilotPosition();
			}
		}

		private void ShowMovementState()
		{
			m_toggleMovementState = !m_toggleMovementState;
		}

		public override void Draw()
		{
			base.Draw();
			if (MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				MyAnimationInverseKinematics.DebugTransform = MySession.Static.LocalCharacter.WorldMatrix;
			}
			if (m_toggleMovementState)
			{
				IEnumerable<MyCharacter> enumerable = MyEntities.GetEntities().OfType<MyCharacter>();
				Vector2 screenCoord = new Vector2(10f, 200f);
				foreach (MyCharacter item in enumerable)
				{
					MyRenderProxy.DebugDrawText2D(screenCoord, item.GetCurrentMovementState().ToString(), Color.Green, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
					screenCoord += new Vector2(0f, 20f);
				}
			}
			if (MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				Text("Character look speed: {0}", MySession.Static.LocalCharacter.RotationSpeed);
			}
			if (MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				Text("Character state: {0}", MySession.Static.LocalCharacter.CurrentMovementState);
				Text("Character ground state: {0}", MySession.Static.LocalCharacter.CharacterGroundState);
			}
			if (MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				Text("Character head offset: {0} {1}", MySession.Static.LocalCharacter.HeadMovementXOffset, MySession.Static.LocalCharacter.HeadMovementYOffset);
			}
			if (MySession.Static != null && MySession.Static.LocalCharacter != null)
			{
				MyAnimationControllerComponent animationController = MySession.Static.LocalCharacter.AnimationController;
				StringBuilder stringBuilder = new StringBuilder(1024);
				MyAnimationController controller = animationController.Controller;
				if (animationController != null && controller != null && controller.GetLayerByIndex(0) != null)
				{
					stringBuilder.Clear();
					int[] visitedTreeNodesPath = controller.GetLayerByIndex(0).VisitedTreeNodesPath;
					foreach (int num in visitedTreeNodesPath)
					{
						if (num == 0)
						{
							break;
						}
						stringBuilder.Append(num);
						stringBuilder.Append(",");
					}
					Text(stringBuilder.ToString());
				}
				if (animationController != null && animationController.Variables != null)
				{
					foreach (KeyValuePair<MyStringId, float> variable in animationController.Variables)
					{
						stringBuilder.Clear();
						stringBuilder.Append(variable.Key);
						stringBuilder.Append(" = ");
						stringBuilder.Append(variable.Value);
						Text(stringBuilder.ToString());
					}
				}
				if (animationController != null)
				{
					if (animationController.LastFrameActions != null)
					{
						foreach (MyStringId lastFrameAction in animationController.LastFrameActions)
						{
							m_lastAnimationActions.Add(lastFrameAction.ToString());
						}
						if (m_lastAnimationActions.Count > 20)
						{
							m_lastAnimationActions.RemoveRange(0, m_lastAnimationActions.Count - 20);
						}
					}
					Text(Color.Red, "--- RECENTLY TRIGGERED ACTIONS ---");
					foreach (string lastAnimationAction in m_lastAnimationActions)
					{
						Text(Color.Yellow, lastAnimationAction);
					}
				}
				if (animationController != null && controller != null)
				{
					lock (controller)
					{
						int layerCount = controller.GetLayerCount();
						for (int j = 0; j < layerCount; j++)
						{
							MyAnimationStateMachine layerByIndex = controller.GetLayerByIndex(j);
							if (layerByIndex != null && layerByIndex.CurrentNode != null)
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								foreach (MyAnimationStateMachine.MyStateTransitionBlending item2 in layerByIndex.StateTransitionBlending)
								{
									stringBuilder2.AppendFormat(" + {0}(+{1:0.0})", item2.SourceState.Name, item2.TimeLeftInSeconds);
								}
								string text = $"{layerByIndex.Name} ... {layerByIndex.CurrentNode.Name}{stringBuilder2}";
								MyRenderProxy.DebugDrawText2D(new Vector2(250f, 150 + j * 10), text, Color.Lime, 0.5f);
							}
						}
					}
				}
			}
			if (m_toggleShowSkeleton)
			{
				DrawSkeleton();
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(300f, 10f), "Debugging AC " + m_animationControllerName, Color.Yellow, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			if (MySession.Static != null && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.Definition != null && MySession.Static.LocalCharacter.Definition.AnimationController == null)
			{
				DictionaryReader<string, MyAnimationPlayerBlendPair> allAnimationPlayers = MySession.Static.LocalCharacter.GetAllAnimationPlayers();
				float num2 = 40f;
				foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> item3 in allAnimationPlayers)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(400f, num2), ((item3.Key != "") ? item3.Key : "Body") + ": " + item3.Value.ActualPlayer.AnimationNameDebug + " (" + item3.Value.ActualPlayer.AnimationMwmPathDebug + ")", Color.Lime, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
					num2 += 30f;
				}
			}
		}

		private void DrawSkeleton()
		{
			if (m_boneRefToIndex == null)
			{
				m_boneRefToIndex = new Dictionary<MyCharacterBone, int>(256);
			}
			if (MySessionComponentAnimationSystem.Static != null)
			{
				foreach (MyAnimationControllerComponent registeredAnimationComponent in MySessionComponentAnimationSystem.Static.RegisteredAnimationComponents)
				{
					MyCharacter myCharacter = (registeredAnimationComponent != null) ? (registeredAnimationComponent.Entity as MyCharacter) : null;
					if (myCharacter == null)
					{
						break;
					}
					List<MyAnimationClip.BoneState> lastRawBoneResult = myCharacter.AnimationController.LastRawBoneResult;
					MyCharacterBone[] characterBones = myCharacter.AnimationController.CharacterBones;
					m_boneRefToIndex.Clear();
					for (int i = 0; i < characterBones.Length; i++)
					{
						m_boneRefToIndex.Add(myCharacter.AnimationController.CharacterBones[i], i);
					}
					for (int j = 0; j < characterBones.Length; j++)
					{
						if (characterBones[j].Parent == null)
						{
							MatrixD parentTransform = myCharacter.PositionComp.WorldMatrix;
							DrawBoneHierarchy(myCharacter, ref parentTransform, characterBones, lastRawBoneResult, j);
						}
					}
				}
			}
		}

		private void DrawBoneHierarchy(MyCharacter character, ref MatrixD parentTransform, MyCharacterBone[] characterBones, List<MyAnimationClip.BoneState> rawBones, int boneIndex)
		{
			MatrixD matrixD = (rawBones != null) ? (Matrix.CreateTranslation(rawBones[boneIndex].Translation) * parentTransform) : MatrixD.Identity;
			matrixD = ((rawBones != null) ? (Matrix.CreateFromQuaternion(rawBones[boneIndex].Rotation) * matrixD) : matrixD);
			if (rawBones != null)
			{
				MyRenderProxy.DebugDrawLine3D(matrixD.Translation, parentTransform.Translation, Color.Green, Color.Green, depthRead: false);
			}
			bool flag = false;
			for (int i = 0; characterBones[boneIndex].GetChildBone(i) != null; i++)
			{
				MyCharacterBone childBone = characterBones[boneIndex].GetChildBone(i);
				DrawBoneHierarchy(character, ref matrixD, characterBones, rawBones, m_boneRefToIndex[childBone]);
				flag = true;
			}
			if (!flag && rawBones != null)
			{
				MyRenderProxy.DebugDrawLine3D(matrixD.Translation, matrixD.Translation + matrixD.Left * 0.05000000074505806, Color.Green, Color.Cyan, depthRead: false);
			}
			MyRenderProxy.DebugDrawText3D(Vector3D.Transform(characterBones[boneIndex].AbsoluteTransform.Translation, character.PositionComp.WorldMatrix), characterBones[boneIndex].Name, Color.Lime, 0.4f, depthRead: false);
			if (characterBones[boneIndex].Parent != null)
			{
				Vector3D pointFrom = Vector3D.Transform(characterBones[boneIndex].AbsoluteTransform.Translation, character.PositionComp.WorldMatrix);
				Vector3D pointTo = Vector3D.Transform(characterBones[boneIndex].Parent.AbsoluteTransform.Translation, character.PositionComp.WorldMatrix);
				MyRenderProxy.DebugDrawLine3D(pointFrom, pointTo, Color.Purple, Color.Purple, depthRead: false);
			}
			if (!flag)
			{
				Vector3D pointFrom2 = Vector3D.Transform(characterBones[boneIndex].AbsoluteTransform.Translation, character.PositionComp.WorldMatrix);
				Vector3D pointTo2 = Vector3D.Transform(characterBones[boneIndex].AbsoluteTransform.Translation + characterBones[boneIndex].AbsoluteTransform.Left * 0.05f, character.PositionComp.WorldMatrix);
				MyRenderProxy.DebugDrawLine3D(pointFrom2, pointTo2, Color.Purple, Color.Red, depthRead: false);
			}
		}
	}
}
