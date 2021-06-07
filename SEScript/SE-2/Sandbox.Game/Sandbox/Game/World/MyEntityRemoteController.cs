using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.Helpers;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MyEntityRemoteController
	{
		private static readonly Random m_random = new Random();

		private readonly string[] m_animations = new string[4]
		{
			"Wave",
			"Thumb-Up",
			"FacePalm",
			"Victory"
		};

		private readonly float m_animationTimer = 20f;

		private readonly float m_doubleClickPause = 0.2f;

		private MyEntity m_controlledEntity;

		private float m_currentAnimationTime;

		private bool m_canPlayAnimation;

		private int m_buttonClicks;

		private float m_lastClickTime;

		private float m_currentTime;

		private float m_rotationSpeed;

		private float m_rotationSpeedDecay = 0.95f;

		private Vector3 m_rotationDirection = Vector3.Zero;

		private GlobalAxis m_rotationLocks;

		private Vector3 m_rotationVector = Vector3.One;

		private Dictionary<string, MyGameInventoryItemSlot> m_toolsNames;

		public GlobalAxis RotationLocks
		{
			get
			{
				return m_rotationLocks;
			}
			private set
			{
				m_rotationLocks = value;
				m_rotationVector = (((m_rotationLocks & GlobalAxis.X) == 0) ? Vector3.Right : Vector3.Zero) + (((m_rotationLocks & GlobalAxis.Y) == 0) ? Vector3.Up : Vector3.Zero) + (((m_rotationLocks & GlobalAxis.Z) == 0) ? Vector3.Backward : Vector3.Zero);
			}
		}

		public MyEntityRemoteController(MyEntity entity)
		{
			m_controlledEntity = entity;
			m_rotationLocks = GlobalAxis.None;
			m_rotationVector = Vector3.One;
			m_toolsNames = new Dictionary<string, MyGameInventoryItemSlot>();
			m_toolsNames.Add("AutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
			m_toolsNames.Add("RapidFireAutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
			m_toolsNames.Add("PreciseAutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
			m_toolsNames.Add("UltimateAutomaticRifleItem", MyGameInventoryItemSlot.Rifle);
			m_toolsNames.Add("WelderItem", MyGameInventoryItemSlot.Welder);
			m_toolsNames.Add("Welder2Item", MyGameInventoryItemSlot.Welder);
			m_toolsNames.Add("Welder3Item", MyGameInventoryItemSlot.Welder);
			m_toolsNames.Add("Welder4Item", MyGameInventoryItemSlot.Welder);
			m_toolsNames.Add("AngleGrinderItem", MyGameInventoryItemSlot.Grinder);
			m_toolsNames.Add("AngleGrinder2Item", MyGameInventoryItemSlot.Grinder);
			m_toolsNames.Add("AngleGrinder3Item", MyGameInventoryItemSlot.Grinder);
			m_toolsNames.Add("AngleGrinder4Item", MyGameInventoryItemSlot.Grinder);
			m_toolsNames.Add("HandDrillItem", MyGameInventoryItemSlot.Drill);
			m_toolsNames.Add("HandDrill2Item", MyGameInventoryItemSlot.Drill);
			m_toolsNames.Add("HandDrill3Item", MyGameInventoryItemSlot.Drill);
			m_toolsNames.Add("HandDrill4Item", MyGameInventoryItemSlot.Drill);
		}

		public void Update(bool isMouseOverAnyControl)
		{
			m_currentTime += 0.0166666675f;
			m_currentAnimationTime += 0.0166666675f;
			if (m_currentAnimationTime > m_animationTimer)
			{
				m_currentAnimationTime = 0f;
				m_canPlayAnimation = true;
			}
			if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Left) && !isMouseOverAnyControl)
			{
				SetRotationWithSpeed(Vector3.One, MyInput.Static.GetCursorPositionDelta().X * 50f);
			}
			if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Right) && !isMouseOverAnyControl)
			{
				PlayRandomCharacterAnimation();
			}
			if (MyInput.Static.IsNewMousePressed(MyMouseButtonsEnum.Left) && !isMouseOverAnyControl)
			{
				if (m_lastClickTime + m_doubleClickPause > m_currentTime)
				{
					m_buttonClicks++;
				}
				m_lastClickTime = m_currentTime;
			}
			if (m_currentTime > m_lastClickTime + m_doubleClickPause)
			{
				m_buttonClicks = 0;
				m_lastClickTime = m_currentTime;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.HELMET) || m_buttonClicks == 2)
			{
				ToggleCharacterHelmet();
				m_buttonClicks = 0;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.D1))
			{
				PlayCharacterAnimation(m_animations[0]);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.D2))
			{
				PlayCharacterAnimation(m_animations[1]);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.D3))
			{
				PlayCharacterAnimation(m_animations[2]);
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.D4))
			{
				PlayCharacterAnimation(m_animations[3]);
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CROUCH))
			{
				MySession.Static.LocalCharacter.Crouch();
			}
			if (m_rotationSpeed != 0f)
			{
				if (m_rotationDirection != Vector3.Zero)
				{
					RotateEntity(m_rotationDirection * m_rotationSpeed * 0.0166666675f);
				}
				m_rotationSpeed *= m_rotationSpeedDecay;
				if (Math.Abs(m_rotationSpeed) < 0.001f)
				{
					m_rotationSpeed = 0f;
				}
			}
		}

		public void LockRotationAxis(GlobalAxis axis)
		{
			RotationLocks |= axis;
		}

		public void UnlockRotationAxis(GlobalAxis axis)
		{
			RotationLocks &= ~axis;
		}

		public void SetRotationWithSpeed(Vector3 rotation, float speed)
		{
			m_rotationDirection = rotation;
			m_rotationSpeed = speed;
		}

		public void RotateEntity(Vector3 rotation)
		{
			MyCharacter myCharacter = m_controlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				myCharacter.MoveAndRotate(Vector3.Zero, new Vector2(0f, rotation.Y) * -3f, 0f);
			}
			else if (m_controlledEntity != null && m_controlledEntity.InScene)
			{
				rotation = rotation * MathF.PI / 180f;
				MatrixD.CreateFromYawPitchRoll(rotation.X * m_rotationVector.X, rotation.Y * m_rotationVector.Y, rotation.Z * m_rotationVector.Z, out MatrixD result);
				result.Translation = Vector3D.Zero;
				MatrixD matrix = m_controlledEntity.WorldMatrix;
				MatrixD.Multiply(ref result, ref matrix, out MatrixD result2);
				m_controlledEntity.WorldMatrix = result2;
			}
		}

		public List<MyPhysicalInventoryItem> GetInventoryTools()
		{
			List<MyPhysicalInventoryItem> list = new List<MyPhysicalInventoryItem>();
			MyCharacter myCharacter = m_controlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				foreach (MyPhysicalInventoryItem item in myCharacter.GetInventoryBase().GetItems())
				{
					if (m_toolsNames.ContainsKey(item.Content.SubtypeName))
					{
						list.Add(item);
					}
				}
				return list;
			}
			return list;
		}

		public MyGameInventoryItemSlot GetToolSlot(string name)
		{
			if (m_toolsNames.ContainsKey(name))
			{
				return m_toolsNames[name];
			}
			return MyGameInventoryItemSlot.None;
		}

		public void ToggleCharacterHelmet()
		{
			(m_controlledEntity as IMyControllableEntity)?.SwitchHelmet();
		}

		public void PlayCharacterAnimation(string animationName)
		{
			MyCharacter myCharacter = m_controlledEntity as MyCharacter;
			if (myCharacter != null && MyDefinitionManager.Static.TryGetAnimationDefinition(animationName) != null && myCharacter.UseNewAnimationSystem)
			{
				myCharacter.TriggerCharacterAnimationEvent(animationName.ToLower(), sync: true);
			}
		}

		public void PlayRandomCharacterAnimation()
		{
			if (m_canPlayAnimation)
			{
				int num = m_random.Next(0, m_animations.Length);
				PlayCharacterAnimation(m_animations[num]);
				m_canPlayAnimation = false;
			}
		}

		public void ActivateCharacterToolbarItem(MyDefinitionId item)
		{
			MyCharacter myCharacter = m_controlledEntity as MyCharacter;
			if (myCharacter == null)
			{
				return;
			}
			MyToolbar toolbar = myCharacter.Toolbar;
			if (toolbar == null)
			{
				return;
			}
			MyDefinitionBase definition;
			if (item.TypeId.IsNull)
			{
				toolbar.Unselect(unselectSound: false);
			}
			else if (MyDefinitionManager.Static.TryGetDefinition(item, out definition))
			{
				MyToolbarItemWeapon myToolbarItemWeapon = MyToolbarItemFactory.CreateToolbarItem(MyToolbarItemFactory.ObjectBuilderFromDefinition(definition)) as MyToolbarItemWeapon;
				if (myToolbarItemWeapon != null)
				{
					myCharacter.SwitchToWeapon(myToolbarItemWeapon);
				}
			}
		}

		public void ToggleCharacterBackpack()
		{
			MyCharacter myCharacter = m_controlledEntity as MyCharacter;
			myCharacter?.EnableBag(!myCharacter.EnabledBag);
		}

		public void ChangeCharacterColor(Color color)
		{
			ChangeCharacterColor(color.ColorToHSVDX11());
		}

		public void ChangeCharacterColor(Vector3 hsvColor)
		{
			MyCharacter myCharacter = m_controlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				myCharacter.ChangeModelAndColor(myCharacter.ModelName, hsvColor, resetToDefault: false, 0L);
				MyLocalCache.SaveInventoryConfig(myCharacter);
			}
		}
	}
}
