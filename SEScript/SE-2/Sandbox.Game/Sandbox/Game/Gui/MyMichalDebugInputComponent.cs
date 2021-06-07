using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.AI;
using Sandbox.Game.AI.BehaviorTree;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyMichalDebugInputComponent : MyDebugComponent
	{
		public bool CastLongRay;

		public int DebugPacketCount;

		public int CurrentQueuedBytes;

		public bool Reliable = true;

		public bool DebugDraw;

		public bool CustomGridCreation;

		public IMyBot SelectedBot;

		public int BotPointer;

		public int SelectedTreeIndex;

		public MyBehaviorTree SelectedTree;

		public int[] BotsIndexes = new int[0];

		private Dictionary<MyJoystickAxesEnum, float?> AxesCollection;

		private List<MyJoystickAxesEnum> Axes;

		public MatrixD HeadMatrix = MatrixD.Identity;

		private const int HeadMatrixFlag = 15;

		private int CurrentHeadMatrixFlag;

		public bool OnSelectDebugBot;

		private string multiplayerStats = string.Empty;

		private Vector3D? m_lineStart;

		private Vector3D? m_lineEnd;

		private Vector3D? m_sphereCen;

		private float? m_rad;

		private List<MyAgentDefinition> m_agentDefinitions = new List<MyAgentDefinition>();

		private int? m_selectedDefinition;

		private string m_selectBotName;

		public static MyMichalDebugInputComponent Static
		{
			get;
			private set;
		}

		static MyMichalDebugInputComponent()
		{
		}

		public MyMichalDebugInputComponent()
		{
			Static = this;
			Axes = new List<MyJoystickAxesEnum>();
			AxesCollection = new Dictionary<MyJoystickAxesEnum, float?>();
			foreach (MyJoystickAxesEnum value in Enum.GetValues(typeof(MyJoystickAxesEnum)))
			{
				AxesCollection[value] = null;
				Axes.Add(value);
			}
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Debug draw", DebugDrawFunc);
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, OnRecording, ToggleVoiceChat);
			if (MyPerGameSettings.Game == GameEnum.SE_GAME)
			{
				AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Remove grids with space balls", RemoveGridsWithSpaceBallsFunc);
				AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Throw 50 ores and 50 scrap metals", ThrowFloatingObjectsFunc);
			}
			if (MyPerGameSettings.EnableAi)
			{
				AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Next head matrix", NextHeadMatrix);
				AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Previous head matrix", PreviousHeadMatrix);
				AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, OnSelectBotForDebugMsg, delegate
				{
					OnSelectDebugBot = !OnSelectDebugBot;
					return true;
				});
				AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Remove bot", delegate
				{
					MyAIComponent.Static.DebugRemoveFirstBot();
					return true;
				});
				AddShortcut(MyKeys.L, newPress: true, control: true, shift: false, alt: false, () => "Add animal bot", SpawnAnimalAroundPlayer);
				AddShortcut(MyKeys.OemSemicolon, newPress: true, control: true, shift: false, alt: false, () => "Spawn selected bot " + ((m_selectBotName != null) ? m_selectBotName : "NOT SELECTED"), SpawnBot);
				AddShortcut(MyKeys.OemMinus, newPress: true, control: true, shift: false, alt: false, () => "Previous bot definition", PreviousBot);
				AddShortcut(MyKeys.OemPlus, newPress: true, control: true, shift: false, alt: false, () => "Next bot definition", NextBot);
				AddShortcut(MyKeys.OemQuotes, newPress: true, control: true, shift: false, alt: false, () => "Reload bot definitions", ReloadDefinitions);
				AddShortcut(MyKeys.OemComma, newPress: true, control: true, shift: false, alt: false, () => "RemoveAllTimbers", RemoveAllTimbers);
				AddShortcut(MyKeys.N, newPress: true, control: true, shift: false, alt: false, () => "Cast long ray", ChangeAlgo);
			}
		}

		private bool RemoveAllTimbers()
		{
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null && myCubeBlock.BlockDefinition.Id.SubtypeName == "Timber1")
				{
					myCubeBlock.Close();
				}
			}
			return true;
		}

		private bool ChangeAlgo()
		{
			CastLongRay = !CastLongRay;
			return true;
		}

		public override string GetName()
		{
			return "Michal";
		}

		public override bool HandleInput()
		{
			foreach (MyJoystickAxesEnum axis in Axes)
			{
				if (MyInput.Static.IsJoystickAxisValid(axis))
				{
					float joystickAxisStateForGameplay = MyInput.Static.GetJoystickAxisStateForGameplay(axis);
					AxesCollection[axis] = joystickAxisStateForGameplay;
				}
				else
				{
					AxesCollection[axis] = null;
				}
			}
			return base.HandleInput();
		}

		public override void Draw()
		{
			base.Draw();
			if (!DebugDraw)
			{
				return;
			}
			if (MySession.Static.LocalCharacter != null)
			{
				HeadMatrix = MySession.Static.LocalCharacter.GetHeadMatrix((CurrentHeadMatrixFlag & 1) == 1, (CurrentHeadMatrixFlag & 2) == 2, (CurrentHeadMatrixFlag & 4) == 4, (CurrentHeadMatrixFlag & 8) == 8);
				MyRenderProxy.DebugDrawAxis(HeadMatrix, 1f, depthRead: false);
				string text = $"GetHeadMatrix({(CurrentHeadMatrixFlag & 1) == 1}, {(CurrentHeadMatrixFlag & 2) == 2}, {(CurrentHeadMatrixFlag & 4) == 4}, {(CurrentHeadMatrixFlag & 8) == 8})";
				MyRenderProxy.DebugDrawText2D(new Vector2(600f, 20f), text, Color.Red, 1f);
				MatrixD worldMatrix = MySession.Static.LocalCharacter.WorldMatrix;
				_ = worldMatrix.Forward;
				float num = MathHelper.ToRadians(15f);
				Math.Cos(num);
				Math.Sin(num);
				MatrixD matrix = MatrixD.CreateRotationY(num);
				MatrixD matrix2 = MatrixD.Transpose(matrix);
				Vector3D pointTo = worldMatrix.Translation + worldMatrix.Forward;
				Vector3D pointTo2 = worldMatrix.Translation + Vector3D.TransformNormal(worldMatrix.Forward, matrix);
				Vector3D pointTo3 = worldMatrix.Translation + Vector3D.TransformNormal(worldMatrix.Forward, matrix2);
				MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, pointTo, Color.Aqua, Color.Aqua, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, pointTo2, Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(worldMatrix.Translation, pointTo3, Color.Green, Color.Green, depthRead: false);
				if (MyToolbarComponent.CurrentToolbar != null)
				{
					Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
					new Vector2(safeGuiRectangle.Right, (float)safeGuiRectangle.Top + (float)safeGuiRectangle.Height * 0.5f);
				}
			}
			if (MyAIComponent.Static != null && MyAIComponent.Static.Bots != null)
			{
				new Vector2(10f, 150f);
				Dictionary<int, IMyBot>.KeyCollection keys = MyAIComponent.Static.Bots.BotsDictionary.Keys;
				BotsIndexes = new int[keys.Count];
				keys.CopyTo(BotsIndexes, 0);
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					if (entity is MyCubeGrid)
					{
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						if (myCubeGrid.BlocksCount == 1)
						{
							MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(new Vector3I(0, 0, 0));
							if (cubeBlock != null && cubeBlock.FatBlock != null)
							{
								MyRenderProxy.DebugDrawText3D(cubeBlock.FatBlock.PositionComp.GetPosition(), cubeBlock.BlockDefinition.Id.SubtypeName, Color.Aqua, 1f, depthRead: false);
								MyRenderProxy.DebugDrawPoint(cubeBlock.FatBlock.PositionComp.GetPosition(), Color.Aqua, depthRead: false);
							}
						}
					}
				}
			}
			if (m_lineStart.HasValue && m_lineEnd.HasValue)
			{
				MyRenderProxy.DebugDrawLine3D(m_lineStart.Value, m_lineEnd.Value, Color.Red, Color.Green, depthRead: true);
			}
			if (m_sphereCen.HasValue && m_rad.HasValue)
			{
				MyRenderProxy.DebugDrawSphere(m_sphereCen.Value, m_rad.Value, Color.Red);
			}
			Vector2 screenCoord = new Vector2(10f, 250f);
			Vector2 vector = new Vector2(0f, 10f);
			foreach (MyJoystickAxesEnum axis in Axes)
			{
				if (AxesCollection[axis].HasValue)
				{
					MyRenderProxy.DebugDrawText2D(screenCoord, axis.ToString() + ": " + AxesCollection[axis].Value, Color.Aqua, 0.4f);
				}
				else
				{
					MyRenderProxy.DebugDrawText2D(screenCoord, axis.ToString() + ": INVALID", Color.Aqua, 0.4f);
				}
				screenCoord += vector;
			}
			MyRenderProxy.DebugDrawText2D(text: "Mouse coords: " + MyGuiManager.MouseCursorPosition.ToString(), screenCoord: screenCoord, color: Color.BlueViolet, scale: 0.4f);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, 450f), multiplayerStats, Color.Yellow, 0.6f);
		}

		public override void Update10()
		{
			base.Update10();
			multiplayerStats = MyMultiplayer.GetMultiplayerStats();
		}

		public void SetDebugDrawLine(Vector3D start, Vector3D end)
		{
			m_lineStart = start;
			m_lineEnd = end;
		}

		public void SetDebugSphere(Vector3D cen, float rad)
		{
			m_sphereCen = cen;
			m_rad = rad;
		}

		public bool DebugDrawFunc()
		{
			DebugDraw = !DebugDraw;
			return true;
		}

		private bool ThrowFloatingObjectsFunc()
		{
			MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
			Matrix inv = Matrix.Invert(viewMatrix);
			MyObjectBuilder_Ore content = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Stone");
			MyObjectBuilder_Ore scrapBuilder = MyFloatingObject.ScrapBuilder;
			for (int i = 1; i <= 25; i++)
			{
				MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(MyRandom.Instance.Next() % 200 + 1, content), inv.Translation + inv.Forward * i * 1f, inv.Forward, inv.Up, null, delegate(MyEntity entity)
				{
					entity.Physics.LinearVelocity = inv.Forward * 50f;
				});
			}
			Vector3D value = inv.Translation;
			value.X += 10.0;
			for (int j = 1; j <= 25; j++)
			{
				MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(MyRandom.Instance.Next() % 200 + 1, scrapBuilder), value + inv.Forward * j * 1f, inv.Forward, inv.Up, null, delegate(MyEntity entity)
				{
					entity.Physics.LinearVelocity = inv.Forward * 50f;
				});
			}
			return true;
		}

		private bool RemoveGridsWithSpaceBallsFunc()
		{
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				_ = (entity is MyCubeGrid);
			}
			return true;
		}

		private string OnSelectBotForDebugMsg()
		{
			return string.Format("Auto select bot for debug: {0}", OnSelectDebugBot ? "TRUE" : "FALSE");
		}

		private string OnRecording()
		{
			if (MyVoiceChatSessionComponent.Static != null)
			{
				return string.Format("VoIP recording: {0}", MyVoiceChatSessionComponent.Static.IsRecording ? "TRUE" : "FALSE");
			}
			return $"VoIP unavailable";
		}

		private bool ToggleVoiceChat()
		{
			if (MyVoiceChatSessionComponent.Static.IsRecording)
			{
				MyVoiceChatSessionComponent.Static.StopRecording();
			}
			else
			{
				MyVoiceChatSessionComponent.Static.StartRecording();
			}
			return true;
		}

		private bool NextHeadMatrix()
		{
			CurrentHeadMatrixFlag++;
			if (CurrentHeadMatrixFlag > 15)
			{
				CurrentHeadMatrixFlag = 15;
			}
			if (MySession.Static.LocalCharacter != null)
			{
				HeadMatrix = MySession.Static.LocalCharacter.GetHeadMatrix((CurrentHeadMatrixFlag & 1) == 1, (CurrentHeadMatrixFlag & 2) == 2, (CurrentHeadMatrixFlag & 4) == 4, (CurrentHeadMatrixFlag & 8) == 8);
			}
			return true;
		}

		private bool PreviousHeadMatrix()
		{
			CurrentHeadMatrixFlag--;
			if (CurrentHeadMatrixFlag < 0)
			{
				CurrentHeadMatrixFlag = 0;
			}
			if (MySession.Static.LocalCharacter != null)
			{
				HeadMatrix = MySession.Static.LocalCharacter.GetHeadMatrix((CurrentHeadMatrixFlag & 1) == 1, (CurrentHeadMatrixFlag & 2) == 2, (CurrentHeadMatrixFlag & 4) == 4, (CurrentHeadMatrixFlag & 8) == 8);
			}
			return true;
		}

		private bool SpawnAnimalAroundPlayer()
		{
			if (MySession.Static.LocalCharacter != null)
			{
				MySession.Static.LocalCharacter.PositionComp.GetPosition();
				MyBotDefinition botDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_BotDefinition), "NormalDeer"));
				MyAIComponent.Static.SpawnNewBot(botDefinition as MyAgentDefinition);
			}
			return true;
		}

		private bool ReloadDefinitions()
		{
			m_selectedDefinition = null;
			m_selectBotName = null;
			m_agentDefinitions.Clear();
			foreach (MyBotDefinition botDefinition in MyDefinitionManager.Static.GetBotDefinitions())
			{
				if (botDefinition is MyAgentDefinition)
				{
					m_agentDefinitions.Add(botDefinition as MyAgentDefinition);
				}
			}
			return true;
		}

		private bool NextBot()
		{
			if (m_agentDefinitions.Count == 0)
			{
				return true;
			}
			if (!m_selectedDefinition.HasValue)
			{
				m_selectedDefinition = 0;
			}
			else
			{
				m_selectedDefinition = (m_selectedDefinition.Value + 1) % m_agentDefinitions.Count;
			}
			m_selectBotName = m_agentDefinitions[m_selectedDefinition.Value].Id.SubtypeName;
			return true;
		}

		private bool PreviousBot()
		{
			if (m_agentDefinitions.Count == 0)
			{
				return true;
			}
			if (!m_selectedDefinition.HasValue)
			{
				m_selectedDefinition = m_agentDefinitions.Count - 1;
			}
			else
			{
				m_selectedDefinition = m_selectedDefinition.Value - 1;
				if (m_selectedDefinition.Value == -1)
				{
					m_selectedDefinition = m_agentDefinitions.Count - 1;
				}
			}
			m_selectBotName = m_agentDefinitions[m_selectedDefinition.Value].Id.SubtypeName;
			return true;
		}

		private bool SpawnBot()
		{
			if (MySession.Static.LocalCharacter != null && m_selectedDefinition.HasValue)
			{
				MatrixD headMatrix = MySession.Static.LocalCharacter.GetHeadMatrix(includeY: true);
				Vector3D translation = headMatrix.Translation;
				MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_BotDefinition), "BarbarianTest"));
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(translation, translation + headMatrix.Forward * 30.0);
				if (hitInfo.HasValue)
				{
					MyAgentDefinition agentDefinition = m_agentDefinitions[m_selectedDefinition.Value];
					MyAIComponent.Static.SpawnNewBot(agentDefinition, hitInfo.Value.Position);
				}
			}
			return true;
		}
	}
}
