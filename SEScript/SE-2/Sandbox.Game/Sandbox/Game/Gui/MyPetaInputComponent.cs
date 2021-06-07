using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyPetaInputComponent : MyDebugComponent
	{
		public static float SI_DYNAMICS_MULTIPLIER = 1f;

		public static bool SHOW_HUD_ALWAYS = false;

		public static bool DRAW_WARNINGS = true;

		public static int DEBUG_INDEX = 0;

		public static Vector3D MovementDistanceStart;

		public static float MovementDistance = 1f;

		public static int MovementDistanceCounter = -1;

		private static Matrix[] s_viewVectors;

		private MyConcurrentDictionary<MyCubePart, List<uint>> m_cubeParts = new MyConcurrentDictionary<MyCubePart, List<uint>>();

		private int pauseCounter;

		private bool xPressed;

		private bool cPressed;

		private bool spacePressed;

		private bool objectiveInited;

		private int OBJECTIVE_PAUSE = 200;

		private bool generalObjective;

		private bool f1Pressed;

		private bool gPressed;

		private bool iPressed;

		private const int N = 9;

		private const int NT = 181;

		private MyVoxelMap m_voxelMap;

		private bool recording;

		private bool recorded;

		private bool introduceObjective;

		private bool keysObjective;

		private bool wPressed;

		private bool sPressed;

		private bool aPressed;

		private bool dPressed;

		private bool jetpackObjective;

		private List<MySkinnedEntity> m_skins = new List<MySkinnedEntity>();

		public override string GetName()
		{
			return "Peta";
		}

		public MyPetaInputComponent()
		{
			AddShortcut(MyKeys.OemBackslash, newPress: true, control: true, shift: false, alt: false, () => "Debug draw physics clusters: " + MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS, delegate
			{
				MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS = !MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS;
				return true;
			});
			AddShortcut(MyKeys.OemBackslash, newPress: false, control: false, shift: false, alt: false, () => "Advance all moving entities", delegate
			{
				AdvanceEntities();
				return true;
			});
			AddShortcut(MyKeys.S, newPress: true, control: true, shift: false, alt: false, () => "Insert safe zone", delegate
			{
				InsertSafeZone();
				return true;
			});
			AddShortcut(MyKeys.Back, newPress: true, control: true, shift: false, alt: false, () => "Freeze gizmo: " + MyCubeBuilder.Static.FreezeGizmo, delegate
			{
				MyCubeBuilder.Static.FreezeGizmo = !MyCubeBuilder.Static.FreezeGizmo;
				return true;
			});
			AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Test movement distance: " + MovementDistance, delegate
			{
				if (MovementDistance != 0f)
				{
					MovementDistance = 0f;
					MovementDistanceStart = ((MyEntity)MySession.Static.ControlledEntity).PositionComp.GetPosition();
					MovementDistanceCounter = (int)SI_DYNAMICS_MULTIPLIER;
				}
				return true;
			});
			AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Show warnings: " + DRAW_WARNINGS, delegate
			{
				DRAW_WARNINGS = !DRAW_WARNINGS;
				return true;
			});
			AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Show logos", delegate
			{
				MyGuiSandbox.BackToIntroLogos(MySandboxGame.AfterLogos);
				return true;
			});
			AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Highlight GScreen", delegate
			{
				HighlightGScreen();
				return true;
			});
			AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Reset Ingame Help", delegate
			{
				MySession.Static.GetComponent<MySessionComponentIngameHelp>()?.Reset();
				return true;
			});
			AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Move VCs to ships and fly at 20m/s speed", delegate
			{
				MoveVCToShips();
				return true;
			});
			AddShortcut(MyKeys.Left, newPress: true, control: false, shift: false, alt: false, () => "Debug index--", delegate
			{
				DEBUG_INDEX--;
				if (DEBUG_INDEX < 0)
				{
					DEBUG_INDEX = 6;
				}
				MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES = true;
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
				return true;
			});
			AddShortcut(MyKeys.Right, newPress: true, control: false, shift: false, alt: false, () => "Debug index++", delegate
			{
				DEBUG_INDEX++;
				if (DEBUG_INDEX > 6)
				{
					DEBUG_INDEX = 0;
				}
				MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES = true;
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
				return true;
			});
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Spawn simple skinned object", delegate
			{
				SpawnSimpleSkinnedObject();
				return true;
			});
		}

		private void TestParallelDictionary()
		{
			Parallel.For(0, 1000, delegate
			{
				switch (MyRandom.Instance.Next(5))
				{
				case 0:
					m_cubeParts.TryAdd(new MyCubePart(), new List<uint>
					{
						0u,
						1u,
						2u
					});
					break;
				case 1:
					foreach (KeyValuePair<MyCubePart, List<uint>> cubePart in m_cubeParts)
					{
						_ = cubePart;
						Thread.Sleep(10);
					}
					break;
				case 2:
					if (m_cubeParts.Count > 0)
					{
						m_cubeParts.Remove(m_cubeParts.First().Key);
					}
					break;
				case 3:
					foreach (KeyValuePair<MyCubePart, List<uint>> cubePart2 in m_cubeParts)
					{
						_ = cubePart2;
						Thread.Sleep(1);
					}
					break;
				}
			});
		}

		private static void AdvanceEntities()
		{
			foreach (MyEntity item in MyEntities.GetEntities().ToList())
			{
				if (item.Physics != null && item.Physics.LinearVelocity.Length() > 0.1f)
				{
					Vector3D vector3D = item.Physics.LinearVelocity * SI_DYNAMICS_MULTIPLIER * 100000f;
					MatrixD worldMatrix = item.WorldMatrix;
					worldMatrix.Translation += vector3D;
					item.WorldMatrix = worldMatrix;
				}
			}
		}

		public override bool HandleInput()
		{
			if (base.HandleInput())
			{
				return true;
			}
			bool flag = false;
			return false;
		}

		private int viewNumber(int i, int j)
		{
			return i * (19 - Math.Abs(i)) + j + 90;
		}

		private void findViews(int species, Vector3 cDIR, out Vector3I vv, out Vector3 rr)
		{
			Vector3 vector = new Vector3(cDIR.X, Math.Max(0f - cDIR.Y, 0.01f), cDIR.Z);
			float num = (Math.Abs(vector.X) > Math.Abs(vector.Z)) ? ((0f - vector.Z) / vector.X) : ((0f - vector.X) / (0f - vector.Z));
			float num2 = 9f * (1f - num) * (float)Math.Acos(MathHelper.Clamp(vector.Y, -1f, 1f)) / MathF.PI;
			float num3 = 9f * (1f + num) * (float)Math.Acos(MathHelper.Clamp(vector.Y, -1f, 1f)) / MathF.PI;
			int num4 = (int)Math.Floor(num2);
			int num5 = (int)Math.Floor(num3);
			float num6 = num2 - (float)num4;
			float num7 = num3 - (float)num5;
			float num8 = 1f - num6 - num7;
			bool flag = (double)num8 > 0.0;
			Vector3I vector3I = new Vector3I(flag ? num4 : (num4 + 1), num4 + 1, num4);
			Vector3I a = new Vector3I(flag ? num5 : (num5 + 1), num5, num5 + 1);
			rr = new Vector3(Math.Abs(num8), flag ? ((double)num6) : (1.0 - (double)num7), flag ? ((double)num7) : (1.0 - (double)num6));
			if (Math.Abs(vector.Z) >= Math.Abs(vector.X))
			{
				Vector3I vector3I2 = vector3I;
				vector3I = -a;
				a = vector3I2;
			}
			if (Math.Abs(vector.X + (0f - vector.Z)) > 1E-05f)
			{
				vector3I *= Math.Sign(vector.X + (0f - vector.Z));
				a *= Math.Sign(vector.X + (0f - vector.Z));
			}
			vv = new Vector3I(species * 181) + new Vector3I(viewNumber(vector3I.X, a.X), viewNumber(vector3I.Y, a.Y), viewNumber(vector3I.Z, a.Z));
		}

		public override void Draw()
		{
			if (MySector.MainCamera != null)
			{
				base.Draw();
				if (m_voxelMap != null)
				{
					MyRenderProxy.DebugDrawAxis(m_voxelMap.WorldMatrix, 100f, depthRead: false);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_FRACTURED_PIECES)
				{
					foreach (MyEntity entity in MyEntities.GetEntities())
					{
						MyFracturedPiece myFracturedPiece = entity as MyFracturedPiece;
						if (myFracturedPiece != null)
						{
							MyPhysicsDebugDraw.DebugDrawBreakable(myFracturedPiece.Physics.BreakableBody, myFracturedPiece.Physics.ClusterToWorld(Vector3D.Zero));
						}
					}
				}
			}
		}

		private void InsertTree()
		{
			MyDefinitionId id = new MyDefinitionId(MyObjectBuilderType.Parse("MyObjectBuilder_Tree"), "Tree04_v2");
			MyEnvironmentItemDefinition environmentItemDefinition = MyDefinitionManager.Static.GetEnvironmentItemDefinition(id);
			if (MyModels.GetModelOnlyData(environmentItemDefinition.Model).HavokBreakableShapes != null)
			{
				HkdBreakableShape hkdBreakableShape = MyModels.GetModelOnlyData(environmentItemDefinition.Model).HavokBreakableShapes[0].Clone();
				MatrixD worldMatrix = MatrixD.CreateWorld(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + 2.0 * MySession.Static.ControlledEntity.Entity.WorldMatrix.Forward, Vector3.Forward, Vector3.Up);
				List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
				hkdBreakableShape.GetChildren(list);
				list[0].Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
				MyDestructionHelper.CreateFracturePiece(hkdBreakableShape, ref worldMatrix, isStatic: false, environmentItemDefinition.Id, sync: true);
			}
		}

		private void TestIngameHelp()
		{
			MyHud.Questlog.Visible = true;
			objectiveInited = false;
			introduceObjective = true;
			keysObjective = false;
			wPressed = false;
			sPressed = false;
			aPressed = false;
			dPressed = false;
		}

		private void SpawnSimpleSkinnedObject()
		{
			MySkinnedEntity mySkinnedEntity = new MySkinnedEntity();
			MyObjectBuilder_Character myObjectBuilder_Character = new MyObjectBuilder_Character();
			myObjectBuilder_Character.EntityDefinitionId = new SerializableDefinitionId(typeof(MyObjectBuilder_Character), "Medieval_barbarian");
			myObjectBuilder_Character.PositionAndOrientation = new MyPositionAndOrientation(MySector.MainCamera.Position + 2f * MySector.MainCamera.ForwardVector, MySector.MainCamera.ForwardVector, MySector.MainCamera.UpVector);
			mySkinnedEntity.Init(null, "Models\\Characters\\Basic\\ME_barbar.mwm", null, null);
			mySkinnedEntity.Init(myObjectBuilder_Character);
			MyEntities.Add(mySkinnedEntity);
			MyAnimationCommand myAnimationCommand = default(MyAnimationCommand);
			myAnimationCommand.AnimationSubtypeName = "IdleBarbar";
			myAnimationCommand.FrameOption = MyFrameOption.Loop;
			myAnimationCommand.TimeScale = 1f;
			MyAnimationCommand command = myAnimationCommand;
			mySkinnedEntity.AddCommand(command);
			m_skins.Add(mySkinnedEntity);
		}

		private static void HighlightGScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			MyGuiControlBase control = screenWithFocus.GetControlByName("ScrollablePanel").Elements[0];
			MyGuiControlBase controlByName = screenWithFocus.GetControlByName("MyGuiControlGridDragAndDrop");
			MyGuiControlBase control2 = screenWithFocus.GetControlByName("MyGuiControlToolbar").Elements[2];
			MyGuiScreenHighlight.MyHighlightControl[] array = new MyGuiScreenHighlight.MyHighlightControl[3];
			MyGuiScreenHighlight.MyHighlightControl myHighlightControl = new MyGuiScreenHighlight.MyHighlightControl
			{
				Control = control,
				Indices = new int[3]
				{
					0,
					1,
					2
				}
			};
			array[0] = myHighlightControl;
			myHighlightControl = new MyGuiScreenHighlight.MyHighlightControl
			{
				Control = controlByName
			};
			array[1] = myHighlightControl;
			myHighlightControl = new MyGuiScreenHighlight.MyHighlightControl
			{
				Control = control2,
				Indices = new int[1]
			};
			array[2] = myHighlightControl;
			MyGuiScreenHighlight.HighlightControls(array);
		}

		private void MoveVCToShips()
		{
			List<MyCharacter> list = new List<MyCharacter>();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyCharacter myCharacter = entity as MyCharacter;
				if (myCharacter != null && !myCharacter.ControllerInfo.IsLocallyHumanControlled() && myCharacter.ControllerInfo.IsLocallyControlled())
				{
					list.Add(myCharacter);
				}
			}
			List<MyCubeGrid> list2 = new List<MyCubeGrid>();
			foreach (MyEntity entity2 in MyEntities.GetEntities())
			{
				MyCubeGrid myCubeGrid = entity2 as MyCubeGrid;
				if (myCubeGrid != null && !myCubeGrid.GridSystems.ControlSystem.IsControlled && myCubeGrid.GridSizeEnum == MyCubeSize.Large && !myCubeGrid.IsStatic)
				{
					list2.Add(myCubeGrid);
				}
			}
			while (list.Count > 0 && list2.Count > 0)
			{
				MyCharacter user = list[0];
				list.RemoveAt(0);
				MyCubeGrid myCubeGrid2 = list2[0];
				list2.RemoveAt(0);
				List<MyCockpit> list3 = new List<MyCockpit>();
				foreach (MyCubeBlock fatBlock in myCubeGrid2.GetFatBlocks())
				{
					MyCockpit myCockpit = fatBlock as MyCockpit;
					if (myCockpit != null && myCockpit.BlockDefinition.EnableShipControl)
					{
						list3.Add(myCockpit);
					}
				}
				list3[0].RequestUse(UseActionEnum.Manipulate, user);
			}
		}

		private void InsertSafeZone()
		{
			((MyEntity)MySession.Static.ControlledEntity).PositionComp.SetPosition(((MyEntity)MySession.Static.ControlledEntity).PositionComp.GetPosition() + new Vector3D(double.PositiveInfinity));
		}
	}
}
