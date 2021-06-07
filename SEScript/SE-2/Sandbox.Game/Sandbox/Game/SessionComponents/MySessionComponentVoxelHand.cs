using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MySessionComponentVoxelHand : MySessionComponentBase
	{
		private IMyVoxelBrush[] m_brushes;

		public static MySessionComponentVoxelHand Static;

		internal const float VOXEL_SIZE = 1f;

		internal const float VOXEL_HALF = 0.5f;

		internal static float GRID_SIZE = MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large);

		internal static float SCALE_MAX = MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) * 10f;

		internal static float MIN_BRUSH_ZOOM = GRID_SIZE;

		internal static float MAX_BRUSH_ZOOM = GRID_SIZE * 20f;

		private static float DEG_IN_RADIANS = MathHelper.ToRadians(1f);

		private static readonly float GAMEPAD_ZOOM_SPEED = 1f;

		private byte m_selectedMaterial;

		private int m_materialCount;

		private float m_position;

		public MatrixD m_rotation;

		private MyVoxelBase m_currentVoxelMap;

		private readonly List<MyVoxelBase> m_previousVoxelMaps = new List<MyVoxelBase>();

		private readonly List<MyVoxelBase> m_currentVoxelMaps = new List<MyVoxelBase>();

		private MyGuiCompositeTexture m_texture;

		public Color ShapeColor;

		private MyHudNotification m_safezoneNotification;

		private MyHudNotification m_noVoxelMapNotification;

		private int m_selectedAxis;

		private bool m_showAxis;

		private bool m_buildMode;

		private bool m_enabled;

		private bool m_editing;

		private int m_currentBrushIndex = -1;

		private MyHudNotification m_voxelMaterialHint;

		private MyHudNotification m_voxelSettingsHint;

		private static List<MyEntity> m_foundElements = new List<MyEntity>();

		public override Type[] Dependencies => new Type[2]
		{
			typeof(MyToolbarComponent),
			typeof(MyCubeBuilder)
		};

		public bool BuildMode
		{
			get
			{
				return m_buildMode;
			}
			private set
			{
				m_buildMode = value;
				MyHud.IsBuildMode = value;
				if (value)
				{
					ActivateHudBuildModeNotifications();
				}
				else
				{
					DeactivateHudBuildModeNotifications();
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					if (value)
					{
						Activate();
					}
					else
					{
						Deactivate();
					}
					m_enabled = value;
					if (this.OnEnabledChanged != null)
					{
						this.OnEnabledChanged();
					}
				}
			}
		}

		public bool SnapToVoxel
		{
			get;
			set;
		}

		public bool ProjectToVoxel
		{
			get;
			set;
		}

		public bool ShowGizmos
		{
			get;
			set;
		}

		public bool FreezePhysics
		{
			get;
			set;
		}

		public IMyVoxelBrush CurrentShape
		{
			get;
			set;
		}

		public MyVoxelHandDefinition CurrentDefinition
		{
			get;
			set;
		}

		public event Action OnEnabledChanged;

		public event Action OnBrushChanged;

		public MySessionComponentVoxelHand()
		{
			Static = this;
			SnapToVoxel = true;
			ShowGizmos = true;
			ShapeColor = new Vector4(0.6f, 0.6f, 0.6f, 0.25f);
			m_selectedMaterial = 1;
			m_materialCount = MyDefinitionManager.Static.VoxelMaterialCount;
			m_position = MIN_BRUSH_ZOOM * 2f;
			m_rotation = MatrixD.Identity;
			m_texture = new MyGuiCompositeTexture();
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(m_selectedMaterial);
			MyDx11VoxelMaterialDefinition myDx11VoxelMaterialDefinition = voxelMaterialDefinition as MyDx11VoxelMaterialDefinition;
			MyGuiSizedTexture center;
			if (myDx11VoxelMaterialDefinition != null)
			{
				MyGuiCompositeTexture texture = m_texture;
				center = new MyGuiSizedTexture
				{
					Texture = myDx11VoxelMaterialDefinition.VoxelHandPreview
				};
				texture.Center = center;
			}
			else
			{
				MyGuiCompositeTexture texture2 = m_texture;
				center = new MyGuiSizedTexture
				{
					Texture = voxelMaterialDefinition.VoxelHandPreview
				};
				texture2.Center = center;
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			MyToolbarComponent.CurrentToolbar.SelectedSlotChanged += CurrentToolbar_SelectedSlotChanged;
			MyToolbarComponent.CurrentToolbar.SlotActivated += CurrentToolbar_SlotActivated;
			MyToolbarComponent.CurrentToolbar.Unselected += CurrentToolbar_Unselected;
			MyCubeBuilder.Static.OnActivated += OnCubeBuilderActivated;
			InitializeHints();
		}

		protected override void UnloadData()
		{
			MyToolbarComponent.CurrentToolbar.Unselected -= CurrentToolbar_Unselected;
			MyToolbarComponent.CurrentToolbar.SlotActivated -= CurrentToolbar_SlotActivated;
			MyToolbarComponent.CurrentToolbar.SelectedSlotChanged -= CurrentToolbar_SelectedSlotChanged;
			MyCubeBuilder.Static.OnActivated -= OnCubeBuilderActivated;
			base.UnloadData();
		}

		private void InitializeHints()
		{
			string text = string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_LEFT), "]");
			string text2 = string.Concat("[", MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_RIGHT), "]");
			m_voxelMaterialHint = MyHudNotifications.CreateControlNotification(MyCommonTexts.NotificationVoxelMaterialFormat, text, text2);
			m_voxelSettingsHint = MyHudNotifications.CreateControlNotification(MyCommonTexts.NotificationVoxelHandHintFormat, "[Ctrl + H]");
		}

		private void CurrentToolbar_SelectedSlotChanged(MyToolbar toolbar, MyToolbar.SlotArgs args)
		{
			if (!(toolbar.SelectedItem is MyToolbarItemVoxelHand) && Enabled)
			{
				Enabled = false;
			}
		}

		private void CurrentToolbar_SlotActivated(MyToolbar toolbar, MyToolbar.SlotArgs args, bool userActivated)
		{
			if (!(toolbar.GetItemAtIndex(toolbar.SlotToIndex(args.SlotNumber.Value)) is MyToolbarItemVoxelHand) && Enabled)
			{
				Enabled = false;
			}
		}

		private void CurrentToolbar_Unselected(MyToolbar toolbar)
		{
			if (Enabled)
			{
				Enabled = false;
			}
		}

		private void OnCubeBuilderActivated()
		{
			if (Enabled)
			{
				Enabled = false;
			}
		}

		public override void HandleInput()
		{
			if (!Enabled || !(MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay))
			{
				return;
			}
			if (!MySession.Static.CreativeMode && !MySession.Static.IsUserAdmin(Sync.MyId))
			{
				Enabled = false;
				return;
			}
			base.HandleInput();
			MyStringId aX_VOXEL = MySpaceBindingCreator.AX_VOXEL;
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.SLOT0))
			{
				Enabled = false;
				return;
			}
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.VOXEL_HAND_SETTINGS))
			{
				MyScreenManager.AddScreen(new MyGuiScreenVoxelHandSetting());
			}
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.CHANGE_ROTATION_AXIS))
			{
				m_selectedAxis = (m_selectedAxis + 1) % 3;
				m_showAxis = true;
			}
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.ROTATE_AXIS_LEFT, MyControlStateType.PRESSED))
			{
				CreateRotationMatrix(ref m_rotation, m_selectedAxis, DEG_IN_RADIANS, out MatrixD result);
				m_rotation *= result;
				m_showAxis = true;
			}
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.ROTATE_AXIS_RIGHT, MyControlStateType.PRESSED))
			{
				CreateRotationMatrix(ref m_rotation, m_selectedAxis, 0f - DEG_IN_RADIANS, out MatrixD result2);
				m_rotation *= result2;
				m_showAxis = true;
			}
			MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.NEXT_BLOCK_STAGE);
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE, MyControlStateType.PRESSED))
			{
				m_rotation *= MatrixD.CreateFromAxisAngle(m_rotation.Forward, DEG_IN_RADIANS);
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE, MyControlStateType.PRESSED))
			{
				m_rotation *= MatrixD.CreateFromAxisAngle(m_rotation.Forward, 0f - DEG_IN_RADIANS);
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE, MyControlStateType.PRESSED))
			{
				m_rotation *= MatrixD.CreateFromAxisAngle(m_rotation.Up, 0f - DEG_IN_RADIANS);
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE, MyControlStateType.PRESSED))
			{
				m_rotation *= MatrixD.CreateFromAxisAngle(m_rotation.Up, DEG_IN_RADIANS);
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE, MyControlStateType.PRESSED))
			{
				m_rotation *= MatrixD.CreateFromAxisAngle(m_rotation.Right, 0f - DEG_IN_RADIANS);
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE, MyControlStateType.PRESSED))
			{
				m_rotation *= MatrixD.CreateFromAxisAngle(m_rotation.Right, DEG_IN_RADIANS);
			}
			CurrentShape.SetRotation(ref m_rotation);
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.SWITCH_LEFT))
			{
				SetMaterial(m_selectedMaterial, next: false);
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.SWITCH_RIGHT))
			{
				SetMaterial(m_selectedMaterial);
			}
			MyBrushAutoLevel myBrushAutoLevel = CurrentShape as MyBrushAutoLevel;
			if (myBrushAutoLevel != null)
			{
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.PRIMARY_TOOL_ACTION) || MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SECONDARY_TOOL_ACTION))
				{
					myBrushAutoLevel.FixAxis();
				}
				else if (MyInput.Static.IsNewGameControlReleased(MyControlsSpace.PRIMARY_TOOL_ACTION) || MyInput.Static.IsNewGameControlReleased(MyControlsSpace.SECONDARY_TOOL_ACTION))
				{
					myBrushAutoLevel.UnFix();
				}
			}
			bool flag = false;
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION, MyControlStateType.PRESSED))
			{
				if (m_currentVoxelMap == null)
				{
					ShowNoVoxelMapNotification();
					return;
				}
				for (int i = 0; i < m_currentVoxelMaps.Count; i++)
				{
					MyVoxelPhysicsBody myVoxelPhysicsBody = (MyVoxelPhysicsBody)m_currentVoxelMaps[i].Physics;
					if (myVoxelPhysicsBody != null)
					{
						flag = (myVoxelPhysicsBody.QueueInvalidate = FreezePhysics);
					}
				}
				if (MySessionComponentSafeZones.IsActionAllowed(CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand, 0L, Sync.MyId))
				{
					for (int j = 0; j < m_currentVoxelMaps.Count; j++)
					{
						CurrentShape.Fill(m_currentVoxelMaps[j], m_selectedMaterial);
					}
				}
				else
				{
					ShowSafeZoneNotification();
				}
			}
			else if (MyInput.Static.IsMiddleMousePressed() || MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.VOXEL_PAINT, MyControlStateType.PRESSED))
			{
				if (m_currentVoxelMap == null)
				{
					ShowNoVoxelMapNotification();
					return;
				}
				if (MySessionComponentSafeZones.IsActionAllowed(CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand, 0L, Sync.MyId))
				{
					for (int k = 0; k < m_currentVoxelMaps.Count; k++)
					{
						CurrentShape.Paint(m_currentVoxelMaps[k], m_selectedMaterial);
					}
				}
				else
				{
					ShowSafeZoneNotification();
				}
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION, MyControlStateType.PRESSED))
			{
				if (m_currentVoxelMap == null)
				{
					ShowNoVoxelMapNotification();
					return;
				}
				for (int l = 0; l < m_currentVoxelMaps.Count; l++)
				{
					MyVoxelPhysicsBody myVoxelPhysicsBody2 = (MyVoxelPhysicsBody)m_currentVoxelMaps[l].Physics;
					if (myVoxelPhysicsBody2 != null)
					{
						flag = (myVoxelPhysicsBody2.QueueInvalidate = FreezePhysics);
					}
				}
				if (MySessionComponentSafeZones.IsActionAllowed(CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand, 0L, Sync.MyId))
				{
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						for (int m = 0; m < m_currentVoxelMaps.Count; m++)
						{
							if (m_currentVoxelMaps[m].Storage.DeleteSupported)
							{
								CurrentShape.Revert(m_currentVoxelMaps[m]);
							}
						}
					}
					else
					{
						for (int n = 0; n < m_currentVoxelMaps.Count; n++)
						{
							CurrentShape.CutOut(m_currentVoxelMaps[n]);
						}
					}
				}
				else
				{
					ShowSafeZoneNotification();
				}
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.VOXEL_REVERT, MyControlStateType.PRESSED))
			{
				for (int num = 0; num < m_currentVoxelMaps.Count; num++)
				{
					MyVoxelPhysicsBody myVoxelPhysicsBody3 = (MyVoxelPhysicsBody)m_currentVoxelMaps[num].Physics;
					if (myVoxelPhysicsBody3 != null)
					{
						flag = (myVoxelPhysicsBody3.QueueInvalidate = FreezePhysics);
					}
				}
				if (MySessionComponentSafeZones.IsActionAllowed(CurrentShape.GetWorldBoundaries(), MySafeZoneAction.VoxelHand, 0L, 0uL))
				{
					if (m_currentVoxelMap.Storage.DeleteSupported)
					{
						CurrentShape.Revert(m_currentVoxelMap);
					}
				}
				else
				{
					ShowSafeZoneNotification();
				}
			}
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.VOXEL_SCALE_UP))
			{
				if (CurrentShape != null)
				{
					CurrentShape.ScaleShapeUp();
				}
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.VOXEL_SCALE_DOWN) && CurrentShape != null)
			{
				CurrentShape.ScaleShapeDown();
			}
			int num2 = Math.Sign(MyInput.Static.DeltaMouseScrollWheelValue());
			if (num2 != 0 && MyInput.Static.IsAnyCtrlKeyPressed())
			{
				float num3 = (float)CurrentShape.GetBoundaries().HalfExtents.Length() * 0.5f;
				SetBrushZoom(m_position + (float)num2 * num3);
			}
			if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.VOXEL_FURTHER, MyControlStateType.PRESSED))
			{
				float num4 = (float)CurrentShape.GetBoundaries().HalfExtents.Length() * 0.5f;
				SetBrushZoom(m_position + num4 * GAMEPAD_ZOOM_SPEED);
			}
			else if (MyControllerHelper.IsControl(aX_VOXEL, MyControlsSpace.VOXEL_CLOSER, MyControlStateType.PRESSED))
			{
				float num5 = (float)CurrentShape.GetBoundaries().HalfExtents.Length() * 0.5f;
				SetBrushZoom(m_position + num5 * (0f - GAMEPAD_ZOOM_SPEED));
			}
			if (m_editing == flag)
			{
				return;
			}
			for (int num6 = 0; num6 < m_currentVoxelMaps.Count; num6++)
			{
				MyVoxelPhysicsBody myVoxelPhysicsBody4 = (MyVoxelPhysicsBody)m_currentVoxelMaps[num6].Physics;
				if (myVoxelPhysicsBody4 != null)
				{
					myVoxelPhysicsBody4.QueueInvalidate = flag;
				}
			}
			m_editing = flag;
		}

		protected void CreateRotationMatrix(ref MatrixD source, int index, double angleDelta, out MatrixD result)
		{
			Vector3D axis;
			switch (index)
			{
			case 0:
				axis = source.Right;
				break;
			case 1:
				axis = source.Up;
				break;
			case 2:
				axis = source.Forward;
				break;
			default:
				axis = source.Right;
				break;
			}
			result = MatrixD.CreateFromAxisAngle(axis, angleDelta);
		}

		public float GetBrushZoom()
		{
			return m_position;
		}

		public void SetBrushZoom(float value)
		{
			m_position = MathHelper.Clamp(value, MIN_BRUSH_ZOOM, MAX_BRUSH_ZOOM);
		}

		public void SetMaterial(SerializableDefinitionId id)
		{
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(id.SubtypeId);
			if (voxelMaterialDefinition != null)
			{
				m_selectedMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinitionIndex(id.SubtypeId);
				MyDx11VoxelMaterialDefinition myDx11VoxelMaterialDefinition = voxelMaterialDefinition as MyDx11VoxelMaterialDefinition;
				MyGuiSizedTexture center;
				if (myDx11VoxelMaterialDefinition != null)
				{
					MyGuiCompositeTexture texture = m_texture;
					center = new MyGuiSizedTexture
					{
						Texture = myDx11VoxelMaterialDefinition.VoxelHandPreview
					};
					texture.Center = center;
				}
				else
				{
					MyGuiCompositeTexture texture2 = m_texture;
					center = new MyGuiSizedTexture
					{
						Texture = voxelMaterialDefinition.VoxelHandPreview
					};
					texture2.Center = center;
				}
			}
		}

		private void SetMaterial(byte idx, bool next = true)
		{
			idx = ((!next) ? (idx = (byte)(idx - 1)) : (idx = (byte)(idx + 1)));
			if (idx == byte.MaxValue)
			{
				idx = (byte)(m_materialCount - 1);
			}
			m_selectedMaterial = (byte)((int)idx % m_materialCount);
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(m_selectedMaterial);
			if (voxelMaterialDefinition.Id.SubtypeName == "BrownMaterial" || voxelMaterialDefinition.Id.SubtypeName == "DebugMaterial")
			{
				SetMaterial(idx, next);
				return;
			}
			MyDx11VoxelMaterialDefinition myDx11VoxelMaterialDefinition = voxelMaterialDefinition as MyDx11VoxelMaterialDefinition;
			MyGuiSizedTexture center;
			if (myDx11VoxelMaterialDefinition != null)
			{
				MyGuiCompositeTexture texture = m_texture;
				center = new MyGuiSizedTexture
				{
					Texture = myDx11VoxelMaterialDefinition.VoxelHandPreview
				};
				texture.Center = center;
			}
			else
			{
				MyGuiCompositeTexture texture2 = m_texture;
				center = new MyGuiSizedTexture
				{
					Texture = voxelMaterialDefinition.VoxelHandPreview
				};
				texture2.Center = center;
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if (!Enabled)
			{
				return;
			}
			base.UpdateBeforeSimulation();
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return;
			}
			if (localCharacter.ControllerInfo.Controller == null)
			{
				Enabled = false;
				return;
			}
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera == null)
			{
				return;
			}
			Vector3D vector3D = MySession.Static.IsCameraUserControlledSpectator() ? mainCamera.Position : localCharacter.GetHeadMatrix(includeY: true).Translation;
			Vector3D targetPosition = vector3D + (Vector3D)mainCamera.ForwardVector * Math.Max(2.0 * CurrentShape.GetBoundaries().TransformFast(mainCamera.ViewMatrix).HalfExtents.Z, m_position);
			MyVoxelBase currentVoxelMap = m_currentVoxelMap;
			BoundingBoxD boundingBox = CurrentShape.PeekWorldBoundingBox(ref targetPosition);
			m_currentVoxelMap = MySession.Static.VoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, null);
			m_previousVoxelMaps.Clear();
			m_previousVoxelMaps.AddRange(m_currentVoxelMaps);
			m_currentVoxelMaps.Clear();
			MySession.Static.VoxelMaps.GetVoxelMapsWhoseBoundingBoxesIntersectBox(ref boundingBox, null, m_currentVoxelMaps);
			if (ProjectToVoxel && m_currentVoxelMap != null)
			{
				List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
				MyPhysics.CastRay(vector3D, vector3D + mainCamera.ForwardVector * m_currentVoxelMap.SizeInMetres, list, 11);
				bool flag = false;
				foreach (MyPhysics.HitInfo item in list)
				{
					IMyEntity hitEntity = item.HkHitInfo.GetHitEntity();
					if (hitEntity is MyVoxelBase && ((MyVoxelBase)hitEntity).RootVoxel == m_currentVoxelMap.RootVoxel)
					{
						targetPosition = item.Position;
						m_currentVoxelMap = (MyVoxelBase)hitEntity;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					m_currentVoxelMap = null;
				}
			}
			if (currentVoxelMap != m_currentVoxelMap && currentVoxelMap != null && currentVoxelMap.Physics != null)
			{
				((MyVoxelPhysicsBody)currentVoxelMap.Physics).QueueInvalidate = false;
			}
			foreach (MyVoxelBase previousVoxelMap in m_previousVoxelMaps)
			{
				if (!m_currentVoxelMaps.Contains(previousVoxelMap) && previousVoxelMap.Physics != null)
				{
					((MyVoxelPhysicsBody)previousVoxelMap.Physics).QueueInvalidate = false;
				}
			}
			if (m_currentVoxelMap == null)
			{
				return;
			}
			m_currentVoxelMap = m_currentVoxelMap.RootVoxel;
			int count = m_currentVoxelMaps.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					MyVoxelBase myVoxelBase = m_currentVoxelMaps[i];
					myVoxelBase = myVoxelBase.RootVoxel;
					m_currentVoxelMaps.Add(myVoxelBase);
				}
				m_currentVoxelMaps.RemoveRange(0, count);
			}
			if (SnapToVoxel)
			{
				targetPosition += 0.5f;
				MyVoxelCoordSystems.WorldPositionToVoxelCoord(m_currentVoxelMap.PositionLeftBottomCorner, ref targetPosition, out Vector3I voxelCoord);
				MyVoxelCoordSystems.VoxelCoordToWorldPosition(m_currentVoxelMap.PositionLeftBottomCorner, ref voxelCoord, out targetPosition);
				CurrentShape.SetPosition(ref targetPosition);
			}
			else
			{
				CurrentShape.SetPosition(ref targetPosition);
			}
		}

		public override void Draw()
		{
			if (!Enabled || m_currentVoxelMap == null)
			{
				return;
			}
			base.Draw();
			m_foundElements.Clear();
			BoundingBoxD boundingBox = m_currentVoxelMap.PositionComp.WorldAABB;
			Color color = new Color(0.2f, 0f, 0f, 0.2f);
			if (ShowGizmos)
			{
				if (MyFakes.SHOW_FORBIDDEN_ENITIES_VOXEL_HAND)
				{
					MyEntities.GetElementsInBox(ref boundingBox, m_foundElements);
					foreach (MyEntity foundElement in m_foundElements)
					{
						if (!(foundElement is MyCharacter) && MyVoxelBase.IsForbiddenEntity(foundElement))
						{
							MatrixD worldMatrix = foundElement.PositionComp.WorldMatrix;
							boundingBox = foundElement.PositionComp.LocalAABB;
							MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref boundingBox, ref color, MySimpleObjectRasterizer.SolidAndWireframe, 0, 1f, null, null, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
						}
					}
				}
				if (MyFakes.SHOW_CURRENT_VOXEL_MAP_AABB_IN_VOXEL_HAND)
				{
					boundingBox = m_currentVoxelMap.PositionComp.LocalAABB;
					color = new Vector4(0f, 0.2f, 0f, 0.1f);
					MatrixD worldMatrix = m_currentVoxelMap.PositionComp.WorldMatrix;
					MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref boundingBox, ref color, MySimpleObjectRasterizer.Solid, 0, 1f, null, null, onlyFrontFaces: false, -1, MyBillboard.BlendTypeEnum.LDR);
				}
			}
			CurrentShape.Draw(ref ShapeColor);
			ConditionBase visibleCondition = MyHud.HudDefinition.Toolbar.VisibleCondition;
			if (visibleCondition != null && visibleCondition.Eval() && MyGuiScreenHudSpace.Static != null && MyGuiScreenHudSpace.Static.Visible)
			{
				DrawMaterial();
			}
			if (m_showAxis && CurrentShape != null && CurrentShape.ShowRotationGizmo())
			{
				DrawRotationAxis(m_selectedAxis);
			}
		}

		private void DrawRotationAxis(int axis)
		{
			Vector3D value = Vector3D.Zero;
			Color color = Color.White;
			switch (axis)
			{
			case 0:
				value = m_rotation.Left;
				color = Color.Red;
				break;
			case 1:
				value = m_rotation.Up;
				color = Color.Green;
				break;
			case 2:
				value = m_rotation.Forward;
				color = Color.Blue;
				break;
			}
			Vector3D position = CurrentShape.GetPosition();
			MyRenderProxy.DebugDrawLine3D(position + value, position - value, color, color, depthRead: false);
		}

		public void DrawMaterial()
		{
			MyObjectBuilder_ToolbarControlVisualStyle toolbar = MyHud.HudDefinition.Toolbar;
			Vector2 voxelHandPosition = toolbar.ColorPanelStyle.VoxelHandPosition;
			Vector2 size = toolbar.ColorPanelStyle.Size;
			Vector2 size2 = new Vector2(size.X, size.Y);
			m_texture.Draw(voxelHandPosition, size2, Color.White);
			Vector2 value = new Vector2(size.X + 0.005f, -0.0018f);
			string @string = MyTexts.GetString(MyCommonTexts.VoxelHandSettingScreen_HandMaterial);
			string subtypeName = MyDefinitionManager.Static.GetVoxelMaterialDefinition(m_selectedMaterial).Id.SubtypeName;
			MyGuiManager.DrawString("White", new StringBuilder(string.Format("{1}", @string, subtypeName)), voxelHandPosition + value, 0.5f);
		}

		private void ActivateHudNotifications()
		{
			if (MySession.Static.CreativeMode && (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed))
			{
				MyHud.Notifications.Add(m_voxelMaterialHint);
				MyHud.Notifications.Add(m_voxelSettingsHint);
			}
		}

		private void DeactivateHudNotifications()
		{
			if (MySession.Static.CreativeMode)
			{
				MyHud.Notifications.Remove(m_voxelMaterialHint);
				MyHud.Notifications.Remove(m_voxelSettingsHint);
			}
		}

		private void ActivateHudBuildModeNotifications()
		{
			if (MySession.Static.CreativeMode && MyInput.Static.IsJoystickConnected())
			{
				_ = MyInput.Static.IsJoystickLastUsed;
			}
		}

		private void DeactivateHudBuildModeNotifications()
		{
			_ = MySession.Static.CreativeMode;
		}

		private void Activate()
		{
			AlignToGravity();
			ActivateHudNotifications();
		}

		private void Deactivate()
		{
			DeactivateHudNotifications();
			CurrentShape = null;
			m_currentBrushIndex = -1;
			BuildMode = false;
		}

		private void AlignToGravity()
		{
			if (CurrentShape.AutoRotate)
			{
				Vector3D vector3D = MyGravityProviderSystem.CalculateNaturalGravityInPoint(MySector.MainCamera.Position);
				if (!vector3D.Equals(Vector3.Zero))
				{
					vector3D.Normalize();
					Vector3D result = vector3D;
					vector3D.CalculatePerpendicularVector(out result);
					MatrixD rotationMat = MatrixD.CreateFromDir(result, -vector3D);
					CurrentShape.SetRotation(ref rotationMat);
					m_rotation = rotationMat;
				}
			}
		}

		public bool TrySetBrush(string brushSubtypeName)
		{
			if (m_brushes == null)
			{
				InitializeBrushes();
			}
			int num = 0;
			IMyVoxelBrush[] brushes = m_brushes;
			foreach (IMyVoxelBrush myVoxelBrush in brushes)
			{
				if (brushSubtypeName == myVoxelBrush.SubtypeName)
				{
					CurrentShape = myVoxelBrush;
					m_currentBrushIndex = num;
					if (this.OnBrushChanged != null)
					{
						this.OnBrushChanged();
					}
					return true;
				}
				num++;
			}
			return false;
		}

		public void SwitchToNextBrush()
		{
			if (m_brushes == null)
			{
				InitializeBrushes();
			}
			int num = (m_currentBrushIndex + 1) % m_brushes.Length;
			CurrentShape = m_brushes[num];
			m_currentBrushIndex = num;
			if (this.OnBrushChanged != null)
			{
				this.OnBrushChanged();
			}
		}

		public void EquipVoxelHand()
		{
			if (m_brushes == null)
			{
				InitializeBrushes();
			}
			if (m_currentBrushIndex == -1)
			{
				m_currentBrushIndex = 0;
			}
			CurrentShape = m_brushes[m_currentBrushIndex];
			Enabled = true;
		}

		public void EquipVoxelHand(string brushName)
		{
			if (m_brushes == null)
			{
				InitializeBrushes();
			}
			if (m_currentBrushIndex == -1)
			{
				m_currentBrushIndex = 0;
			}
			TrySetBrush(brushName);
			Enabled = true;
		}

		private void InitializeBrushes()
		{
			m_brushes = new IMyVoxelBrush[6]
			{
				MyBrushBox.Static,
				MyBrushCapsule.Static,
				MyBrushRamp.Static,
				MyBrushSphere.Static,
				MyBrushAutoLevel.Static,
				MyBrushEllipsoid.Static
			};
			m_currentBrushIndex = 0;
			CurrentShape = m_brushes[m_currentBrushIndex];
		}

		private void ShowSafeZoneNotification()
		{
			MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			if (m_safezoneNotification == null)
			{
				m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_VoxelhandDisabled, 2000, "Red");
			}
			MyHud.Notifications.Add(m_safezoneNotification);
		}

		private void ShowNoVoxelMapNotification()
		{
			MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			if (m_noVoxelMapNotification == null)
			{
				m_noVoxelMapNotification = new MyHudNotification(MyCommonTexts.VoxelHand_VoxelMapNeeded, 2000, "Red");
			}
			MyHud.Notifications.Add(m_noVoxelMapNotification);
		}
	}
}
