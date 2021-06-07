using Havok;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SensorBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMySensorBlock),
		typeof(Sandbox.ModAPI.Ingame.IMySensorBlock)
	})]
	public class MySensorBlock : MyFunctionalBlock, Sandbox.ModAPI.IMySensorBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMySensorBlock, IMyGizmoDrawableObject
	{
		protected sealed class SendToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32 : ICallSite<MySensorBlock, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySensorBlock @this, in ToolbarItem sentItem, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index);
			}
		}

		protected sealed class PlayActionSound_003C_003E : ICallSite<MySensorBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySensorBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PlayActionSound();
			}
		}

		protected class m_playProximitySound_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType playProximitySound;
				ISyncType result = playProximitySound = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MySensorBlock)P_0).m_playProximitySound = (Sync<bool, SyncDirection.BothWays>)playProximitySound;
				return result;
			}
		}

		protected class m_active_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType active;
				ISyncType result = active = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MySensorBlock)P_0).m_active = (Sync<bool, SyncDirection.BothWays>)active;
				return result;
			}
		}

		protected class m_fieldMin_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType fieldMin;
				ISyncType result = fieldMin = new Sync<Vector3, SyncDirection.BothWays>(P_1, P_2);
				((MySensorBlock)P_0).m_fieldMin = (Sync<Vector3, SyncDirection.BothWays>)fieldMin;
				return result;
			}
		}

		protected class m_fieldMax_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType fieldMax;
				ISyncType result = fieldMax = new Sync<Vector3, SyncDirection.BothWays>(P_1, P_2);
				((MySensorBlock)P_0).m_fieldMax = (Sync<Vector3, SyncDirection.BothWays>)fieldMax;
				return result;
			}
		}

		protected class m_flags_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType flags;
				ISyncType result = flags = new Sync<MySensorFilterFlags, SyncDirection.BothWays>(P_1, P_2);
				((MySensorBlock)P_0).m_flags = (Sync<MySensorFilterFlags, SyncDirection.BothWays>)flags;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MySensorBlock_003C_003EActor : IActivator, IActivator<MySensorBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MySensorBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySensorBlock CreateInstance()
			{
				return new MySensorBlock();
			}

			MySensorBlock IActivator<MySensorBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Color m_gizmoColor;

		private const float m_maxGizmoDrawDistance = 400f;

		private BoundingBox m_gizmoBoundingBox;

		private readonly Sync<bool, SyncDirection.BothWays> m_playProximitySound;

		private bool m_enablePlaySoundEvent;

		private readonly MyConcurrentHashSet<MyDetectedEntityInfo> m_detectedEntities = new MyConcurrentHashSet<MyDetectedEntityInfo>();

		private Sync<bool, SyncDirection.BothWays> m_active;

		private List<ToolbarItem> m_items;

		private static readonly List<MyEntity> m_potentialPenetrations = new List<MyEntity>();

		private static readonly List<MyVoxelBase> m_potentialVoxelPenetrations = new List<MyVoxelBase>();

		protected HkShape m_fieldShape;

		private bool m_recreateField;

		private readonly Sync<Vector3, SyncDirection.BothWays> m_fieldMin;

		private readonly Sync<Vector3, SyncDirection.BothWays> m_fieldMax;

		private readonly Sync<MySensorFilterFlags, SyncDirection.BothWays> m_flags;

		private static List<MyToolbar> m_openedToolbars;

		private static bool m_shouldSetOtherToolbars;

		private bool m_syncing;

		private new MySensorBlockDefinition BlockDefinition => (MySensorBlockDefinition)base.BlockDefinition;

		public bool IsActive
		{
			get
			{
				return m_active;
			}
			set
			{
				m_active.Value = value;
			}
		}

		public MyEntity LastDetectedEntity
		{
			get;
			private set;
		}

		public MyToolbar Toolbar
		{
			get;
			set;
		}

		public Vector3 FieldMin
		{
			get
			{
				return m_fieldMin;
			}
			set
			{
				m_fieldMin.Value = value;
			}
		}

		public Vector3 FieldMax
		{
			get
			{
				return m_fieldMax;
			}
			set
			{
				m_fieldMax.Value = value;
			}
		}

		public float MaxRange => BlockDefinition.MaxRange;

		public MySensorFilterFlags Filters
		{
			get
			{
				return m_flags;
			}
			set
			{
				m_flags.Value = value;
			}
		}

		public bool PlayProximitySound
		{
			get
			{
				return m_playProximitySound;
			}
			set
			{
				m_playProximitySound.Value = value;
			}
		}

		public bool DetectPlayers
		{
			get
			{
				return (Filters & MySensorFilterFlags.Players) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Players;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Players;
				}
			}
		}

		public bool DetectFloatingObjects
		{
			get
			{
				return (Filters & MySensorFilterFlags.FloatingObjects) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.FloatingObjects;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.FloatingObjects;
				}
			}
		}

		public bool DetectSmallShips
		{
			get
			{
				return (Filters & MySensorFilterFlags.SmallShips) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.SmallShips;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.SmallShips;
				}
			}
		}

		public bool DetectLargeShips
		{
			get
			{
				return (Filters & MySensorFilterFlags.LargeShips) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.LargeShips;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.LargeShips;
				}
			}
		}

		public bool DetectStations
		{
			get
			{
				return (Filters & MySensorFilterFlags.Stations) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Stations;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Stations;
				}
			}
		}

		public bool DetectSubgrids
		{
			get
			{
				return (Filters & MySensorFilterFlags.Subgrids) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Subgrids;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Subgrids;
				}
			}
		}

		public bool DetectAsteroids
		{
			get
			{
				return (Filters & MySensorFilterFlags.Asteroids) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Asteroids;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Asteroids;
				}
			}
		}

		public bool DetectOwner
		{
			get
			{
				return (Filters & MySensorFilterFlags.Owner) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Owner;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Owner;
				}
			}
		}

		public bool DetectFriendly
		{
			get
			{
				return (Filters & MySensorFilterFlags.Friendly) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Friendly;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Friendly;
				}
			}
		}

		public bool DetectNeutral
		{
			get
			{
				return (Filters & MySensorFilterFlags.Neutral) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Neutral;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Neutral;
				}
			}
		}

		public bool DetectEnemy
		{
			get
			{
				return (Filters & MySensorFilterFlags.Enemy) != 0;
			}
			set
			{
				if (value)
				{
					Filters |= MySensorFilterFlags.Enemy;
				}
				else
				{
					Filters &= ~MySensorFilterFlags.Enemy;
				}
			}
		}

		public float LeftExtend
		{
			get
			{
				return 0f - m_fieldMin.Value.X;
			}
			set
			{
				Vector3 fieldMin = FieldMin;
				if (fieldMin.X != 0f - value)
				{
					fieldMin.X = 0f - value;
					FieldMin = fieldMin;
				}
			}
		}

		public float RightExtend
		{
			get
			{
				return m_fieldMax.Value.X;
			}
			set
			{
				Vector3 fieldMax = FieldMax;
				if (fieldMax.X != value)
				{
					fieldMax.X = value;
					FieldMax = fieldMax;
				}
			}
		}

		public float BottomExtend
		{
			get
			{
				return 0f - m_fieldMin.Value.Y;
			}
			set
			{
				Vector3 fieldMin = FieldMin;
				if (fieldMin.Y != 0f - value)
				{
					fieldMin.Y = 0f - value;
					FieldMin = fieldMin;
				}
			}
		}

		public float TopExtend
		{
			get
			{
				return m_fieldMax.Value.Y;
			}
			set
			{
				Vector3 fieldMax = FieldMax;
				if (fieldMax.Y != value)
				{
					fieldMax.Y = value;
					FieldMax = fieldMax;
				}
			}
		}

		public float FrontExtend
		{
			get
			{
				return 0f - m_fieldMin.Value.Z;
			}
			set
			{
				Vector3 fieldMin = FieldMin;
				if (fieldMin.Z != 0f - value)
				{
					fieldMin.Z = 0f - value;
					FieldMin = fieldMin;
				}
			}
		}

		public float BackExtend
		{
			get
			{
				return m_fieldMax.Value.Z;
			}
			set
			{
				Vector3 fieldMax = FieldMax;
				if (fieldMax.Z != value)
				{
					fieldMax.Z = value;
					FieldMax = fieldMax;
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMySensorBlock.LeftExtend
		{
			get
			{
				return LeftExtend;
			}
			set
			{
				LeftExtend = MathHelper.Clamp(value, 1f, BlockDefinition.MaxRange);
			}
		}

		float Sandbox.ModAPI.Ingame.IMySensorBlock.RightExtend
		{
			get
			{
				return RightExtend;
			}
			set
			{
				RightExtend = MathHelper.Clamp(value, 1f, BlockDefinition.MaxRange);
			}
		}

		float Sandbox.ModAPI.Ingame.IMySensorBlock.TopExtend
		{
			get
			{
				return TopExtend;
			}
			set
			{
				TopExtend = MathHelper.Clamp(value, 1f, BlockDefinition.MaxRange);
			}
		}

		float Sandbox.ModAPI.Ingame.IMySensorBlock.BottomExtend
		{
			get
			{
				return BottomExtend;
			}
			set
			{
				BottomExtend = MathHelper.Clamp(value, 1f, BlockDefinition.MaxRange);
			}
		}

		float Sandbox.ModAPI.Ingame.IMySensorBlock.FrontExtend
		{
			get
			{
				return FrontExtend;
			}
			set
			{
				FrontExtend = MathHelper.Clamp(value, 1f, BlockDefinition.MaxRange);
			}
		}

		float Sandbox.ModAPI.Ingame.IMySensorBlock.BackExtend
		{
			get
			{
				return BackExtend;
			}
			set
			{
				BackExtend = MathHelper.Clamp(value, 1f, BlockDefinition.MaxRange);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.PlayProximitySound
		{
			get
			{
				return PlayProximitySound;
			}
			set
			{
				PlayProximitySound = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectPlayers
		{
			get
			{
				return DetectPlayers;
			}
			set
			{
				DetectPlayers = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectFloatingObjects
		{
			get
			{
				return DetectFloatingObjects;
			}
			set
			{
				DetectFloatingObjects = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectSmallShips
		{
			get
			{
				return DetectSmallShips;
			}
			set
			{
				DetectSmallShips = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectLargeShips
		{
			get
			{
				return DetectLargeShips;
			}
			set
			{
				DetectLargeShips = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectStations
		{
			get
			{
				return DetectStations;
			}
			set
			{
				DetectStations = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectAsteroids
		{
			get
			{
				return DetectAsteroids;
			}
			set
			{
				DetectAsteroids = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectOwner
		{
			get
			{
				return DetectOwner;
			}
			set
			{
				DetectOwner = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectFriendly
		{
			get
			{
				return DetectFriendly;
			}
			set
			{
				DetectFriendly = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectNeutral
		{
			get
			{
				return DetectNeutral;
			}
			set
			{
				DetectNeutral = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.DetectEnemy
		{
			get
			{
				return DetectEnemy;
			}
			set
			{
				DetectEnemy = value;
			}
		}

		Vector3 Sandbox.ModAPI.IMySensorBlock.FieldMin
		{
			get
			{
				return FieldMin;
			}
			set
			{
				FieldMin = value;
			}
		}

		Vector3 Sandbox.ModAPI.IMySensorBlock.FieldMax
		{
			get
			{
				return FieldMax;
			}
			set
			{
				FieldMax = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMySensorBlock.IsActive => IsActive;

		MyDetectedEntityInfo Sandbox.ModAPI.Ingame.IMySensorBlock.LastDetectedEntity => MyDetectedEntityInfoHelper.Create(LastDetectedEntity, base.OwnerId);

		private event Action<bool> StateChanged;

		event Action<bool> Sandbox.ModAPI.IMySensorBlock.StateChanged
		{
			add
			{
				StateChanged += value;
			}
			remove
			{
				StateChanged -= value;
			}
		}

		public MySensorBlock()
		{
			CreateTerminalControls();
			m_active.ValueChanged += delegate
			{
				IsActiveChanged();
			};
			m_fieldMax.ValueChanged += delegate
			{
				UpdateField();
			};
			m_fieldMin.ValueChanged += delegate
			{
				UpdateField();
			};
		}

		protected new void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MySensorBlock>())
			{
				base.CreateTerminalControls();
				m_openedToolbars = new List<MyToolbar>();
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MySensorBlock>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, delegate(MySensorBlock self)
				{
					m_openedToolbars.Add(self.Toolbar);
					if (MyGuiScreenToolbarConfigBase.Static == null)
					{
						m_shouldSetOtherToolbars = true;
						MyToolbarComponent.CurrentToolbar = self.Toolbar;
						MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
						MyToolbarComponent.AutoUpdate = false;
						myGuiScreenBase.Closed += delegate
						{
							MyToolbarComponent.AutoUpdate = true;
							m_openedToolbars.Clear();
						};
						MyGuiSandbox.AddScreen(myGuiScreenBase);
					}
				}));
				MyTerminalControlSlider<MySensorBlock> myTerminalControlSlider = new MyTerminalControlSlider<MySensorBlock>("Left", MySpaceTexts.BlockPropertyTitle_SensorFieldWidthMin, MySpaceTexts.BlockPropertyDescription_SensorFieldLeft);
				myTerminalControlSlider.SetLimits((MySensorBlock block) => 0.1f, (MySensorBlock block) => block.MaxRange);
				myTerminalControlSlider.DefaultValue = 5f;
				myTerminalControlSlider.Getter = ((MySensorBlock x) => x.LeftExtend);
				myTerminalControlSlider.Setter = delegate(MySensorBlock x, float v)
				{
					x.LeftExtend = v;
				};
				myTerminalControlSlider.Writer = delegate(MySensorBlock x, StringBuilder result)
				{
					result.AppendInt32((int)x.LeftExtend).Append(" m");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MySensorBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MySensorBlock>("Right", MySpaceTexts.BlockPropertyTitle_SensorFieldWidthMax, MySpaceTexts.BlockPropertyDescription_SensorFieldRight);
				myTerminalControlSlider2.SetLimits((MySensorBlock block) => 0.1f, (MySensorBlock block) => block.MaxRange);
				myTerminalControlSlider2.DefaultValue = 5f;
				myTerminalControlSlider2.Getter = ((MySensorBlock x) => x.RightExtend);
				myTerminalControlSlider2.Setter = delegate(MySensorBlock x, float v)
				{
					x.RightExtend = v;
				};
				myTerminalControlSlider2.Writer = delegate(MySensorBlock x, StringBuilder result)
				{
					result.AppendInt32((int)x.RightExtend).Append(" m");
				};
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MySensorBlock> myTerminalControlSlider3 = new MyTerminalControlSlider<MySensorBlock>("Bottom", MySpaceTexts.BlockPropertyTitle_SensorFieldHeightMin, MySpaceTexts.BlockPropertyDescription_SensorFieldBottom);
				myTerminalControlSlider3.SetLimits((MySensorBlock block) => 0.1f, (MySensorBlock block) => block.MaxRange);
				myTerminalControlSlider3.DefaultValue = 5f;
				myTerminalControlSlider3.Getter = ((MySensorBlock x) => x.BottomExtend);
				myTerminalControlSlider3.Setter = delegate(MySensorBlock x, float v)
				{
					x.BottomExtend = v;
				};
				myTerminalControlSlider3.Writer = delegate(MySensorBlock x, StringBuilder result)
				{
					result.AppendInt32((int)x.BottomExtend).Append(" m");
				};
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MySensorBlock> myTerminalControlSlider4 = new MyTerminalControlSlider<MySensorBlock>("Top", MySpaceTexts.BlockPropertyTitle_SensorFieldHeightMax, MySpaceTexts.BlockPropertyDescription_SensorFieldTop);
				myTerminalControlSlider4.SetLimits((MySensorBlock block) => 0.1f, (MySensorBlock block) => block.MaxRange);
				myTerminalControlSlider4.DefaultValue = 5f;
				myTerminalControlSlider4.Getter = ((MySensorBlock x) => x.TopExtend);
				myTerminalControlSlider4.Setter = delegate(MySensorBlock x, float v)
				{
					x.TopExtend = v;
				};
				myTerminalControlSlider4.Writer = delegate(MySensorBlock x, StringBuilder result)
				{
					result.AppendInt32((int)x.TopExtend).Append(" m");
				};
				myTerminalControlSlider4.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
				MyTerminalControlSlider<MySensorBlock> myTerminalControlSlider5 = new MyTerminalControlSlider<MySensorBlock>("Back", MySpaceTexts.BlockPropertyTitle_SensorFieldDepthMax, MySpaceTexts.BlockPropertyDescription_SensorFieldBack);
				myTerminalControlSlider5.SetLimits((MySensorBlock block) => 0.1f, (MySensorBlock block) => block.MaxRange);
				myTerminalControlSlider5.DefaultValue = 5f;
				myTerminalControlSlider5.Getter = ((MySensorBlock x) => x.BackExtend);
				myTerminalControlSlider5.Setter = delegate(MySensorBlock x, float v)
				{
					x.BackExtend = v;
				};
				myTerminalControlSlider5.Writer = delegate(MySensorBlock x, StringBuilder result)
				{
					result.AppendInt32((int)x.BackExtend).Append(" m");
				};
				myTerminalControlSlider5.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
				MyTerminalControlSlider<MySensorBlock> myTerminalControlSlider6 = new MyTerminalControlSlider<MySensorBlock>("Front", MySpaceTexts.BlockPropertyTitle_SensorFieldDepthMin, MySpaceTexts.BlockPropertyDescription_SensorFieldFront);
				myTerminalControlSlider6.SetLimits((MySensorBlock block) => 0.1f, (MySensorBlock block) => block.MaxRange);
				myTerminalControlSlider6.DefaultValue = 5f;
				myTerminalControlSlider6.Getter = ((MySensorBlock x) => x.FrontExtend);
				myTerminalControlSlider6.Setter = delegate(MySensorBlock x, float v)
				{
					x.FrontExtend = v;
				};
				myTerminalControlSlider6.Writer = delegate(MySensorBlock x, StringBuilder result)
				{
					result.AppendInt32((int)x.FrontExtend).Append(" m");
				};
				myTerminalControlSlider6.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider6);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MySensorBlock>());
				MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MySensorBlock>("Audible Proximity Alert", MySpaceTexts.BlockPropertyTitle_SensorPlaySound, MySpaceTexts.BlockPropertyTitle_SensorPlaySound)
				{
					Getter = ((MySensorBlock x) => x.PlayProximitySound),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.PlayProximitySound = v;
					}
				});
				MyTerminalControlOnOffSwitch<MySensorBlock> obj = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Players", MySpaceTexts.BlockPropertyTitle_SensorDetectPlayers, MySpaceTexts.BlockPropertyTitle_SensorDetectPlayers)
				{
					Getter = ((MySensorBlock x) => x.DetectPlayers),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectPlayers = v;
					}
				};
				obj.EnableToggleAction(MyTerminalActionIcons.CHARACTER_TOGGLE);
				obj.EnableOnOffActions(MyTerminalActionIcons.CHARACTER_ON, MyTerminalActionIcons.CHARACTER_OFF);
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj2 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Floating Objects", MySpaceTexts.BlockPropertyTitle_SensorDetectFloatingObjects, MySpaceTexts.BlockPropertyTitle_SensorDetectFloatingObjects)
				{
					Getter = ((MySensorBlock x) => x.DetectFloatingObjects),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectFloatingObjects = v;
					}
				};
				obj2.EnableToggleAction(MyTerminalActionIcons.MOVING_OBJECT_TOGGLE);
				obj2.EnableOnOffActions(MyTerminalActionIcons.MOVING_OBJECT_ON, MyTerminalActionIcons.MOVING_OBJECT_OFF);
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj3 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Small Ships", MySpaceTexts.BlockPropertyTitle_SensorDetectSmallShips, MySpaceTexts.BlockPropertyTitle_SensorDetectSmallShips)
				{
					Getter = ((MySensorBlock x) => x.DetectSmallShips),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectSmallShips = v;
					}
				};
				obj3.EnableToggleAction(MyTerminalActionIcons.SMALLSHIP_TOGGLE);
				obj3.EnableOnOffActions(MyTerminalActionIcons.SMALLSHIP_ON, MyTerminalActionIcons.SMALLSHIP_OFF);
				MyTerminalControlFactory.AddControl(obj3);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj4 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Large Ships", MySpaceTexts.BlockPropertyTitle_SensorDetectLargeShips, MySpaceTexts.BlockPropertyTitle_SensorDetectLargeShips)
				{
					Getter = ((MySensorBlock x) => x.DetectLargeShips),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectLargeShips = v;
					}
				};
				obj4.EnableToggleAction(MyTerminalActionIcons.LARGESHIP_TOGGLE);
				obj4.EnableOnOffActions(MyTerminalActionIcons.LARGESHIP_ON, MyTerminalActionIcons.LARGESHIP_OFF);
				MyTerminalControlFactory.AddControl(obj4);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj5 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Stations", MySpaceTexts.BlockPropertyTitle_SensorDetectStations, MySpaceTexts.BlockPropertyTitle_SensorDetectStations)
				{
					Getter = ((MySensorBlock x) => x.DetectStations),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectStations = v;
					}
				};
				obj5.EnableToggleAction(MyTerminalActionIcons.STATION_TOGGLE);
				obj5.EnableOnOffActions(MyTerminalActionIcons.STATION_ON, MyTerminalActionIcons.STATION_OFF);
				MyTerminalControlFactory.AddControl(obj5);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj6 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Subgrids", MySpaceTexts.BlockPropertyTitle_SensorDetectSubgrids, MySpaceTexts.BlockPropertyTitle_SensorDetectSubgrids)
				{
					Getter = ((MySensorBlock x) => x.DetectSubgrids),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectSubgrids = v;
					}
				};
				obj6.EnableToggleAction(MyTerminalActionIcons.SUBGRID_TOGGLE);
				obj6.EnableOnOffActions(MyTerminalActionIcons.SUBGRID_ON, MyTerminalActionIcons.SUBGRID_OFF);
				MyTerminalControlFactory.AddControl(obj6);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj7 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Asteroids", MySpaceTexts.BlockPropertyTitle_SensorDetectAsteroids, MySpaceTexts.BlockPropertyTitle_SensorDetectAsteroids)
				{
					Getter = ((MySensorBlock x) => x.DetectAsteroids),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectAsteroids = v;
					}
				};
				obj7.EnableToggleAction();
				obj7.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj7);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MySensorBlock>());
				MyTerminalControlOnOffSwitch<MySensorBlock> obj8 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Owner", MySpaceTexts.BlockPropertyTitle_SensorDetectOwner, MySpaceTexts.BlockPropertyTitle_SensorDetectOwner)
				{
					Getter = ((MySensorBlock x) => x.DetectOwner),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectOwner = v;
					}
				};
				obj8.EnableToggleAction();
				obj8.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj8);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj9 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Friendly", MySpaceTexts.BlockPropertyTitle_SensorDetectFriendly, MySpaceTexts.BlockPropertyTitle_SensorDetectFriendly)
				{
					Getter = ((MySensorBlock x) => x.DetectFriendly),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectFriendly = v;
					}
				};
				obj9.EnableToggleAction();
				obj9.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj9);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj10 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Neutral", MySpaceTexts.BlockPropertyTitle_SensorDetectNeutral, MySpaceTexts.BlockPropertyTitle_SensorDetectNeutral)
				{
					Getter = ((MySensorBlock x) => x.DetectNeutral),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectNeutral = v;
					}
				};
				obj10.EnableToggleAction();
				obj10.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj10);
				MyTerminalControlOnOffSwitch<MySensorBlock> obj11 = new MyTerminalControlOnOffSwitch<MySensorBlock>("Detect Enemy", MySpaceTexts.BlockPropertyTitle_SensorDetectEnemy, MySpaceTexts.BlockPropertyTitle_SensorDetectEnemy)
				{
					Getter = ((MySensorBlock x) => x.DetectEnemy),
					Setter = delegate(MySensorBlock x, bool v)
					{
						x.DetectEnemy = v;
					}
				};
				obj11.EnableToggleAction();
				obj11.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj11);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, CalculateRequiredPowerInput);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			m_items = new List<ToolbarItem>(2);
			for (int i = 0; i < 2; i++)
			{
				m_items.Add(new ToolbarItem
				{
					EntityID = 0L
				});
			}
			Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, 2, 1);
			Toolbar.DrawNumbers = false;
			MyObjectBuilder_SensorBlock myObjectBuilder_SensorBlock = (MyObjectBuilder_SensorBlock)objectBuilder;
			m_fieldMin.SetLocalValue(Vector3.Clamp(myObjectBuilder_SensorBlock.FieldMin, new Vector3(0f - MaxRange), -Vector3.One));
			m_fieldMax.SetLocalValue(Vector3.Clamp(myObjectBuilder_SensorBlock.FieldMax, Vector3.One, new Vector3(MaxRange)));
			m_playProximitySound.SetLocalValue(myObjectBuilder_SensorBlock.PlaySound);
			if (Sync.IsServer)
			{
				DetectPlayers = myObjectBuilder_SensorBlock.DetectPlayers;
				DetectFloatingObjects = myObjectBuilder_SensorBlock.DetectFloatingObjects;
				DetectSmallShips = myObjectBuilder_SensorBlock.DetectSmallShips;
				DetectLargeShips = myObjectBuilder_SensorBlock.DetectLargeShips;
				DetectStations = myObjectBuilder_SensorBlock.DetectStations;
				DetectSubgrids = myObjectBuilder_SensorBlock.DetectSubgrids;
				DetectAsteroids = myObjectBuilder_SensorBlock.DetectAsteroids;
				DetectOwner = myObjectBuilder_SensorBlock.DetectOwner;
				DetectFriendly = myObjectBuilder_SensorBlock.DetectFriendly;
				DetectNeutral = myObjectBuilder_SensorBlock.DetectNeutral;
				DetectEnemy = myObjectBuilder_SensorBlock.DetectEnemy;
			}
			m_active.SetLocalValue(myObjectBuilder_SensorBlock.IsActive);
			Toolbar.Init(myObjectBuilder_SensorBlock.Toolbar, this);
			for (int j = 0; j < 2; j++)
			{
				MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(j);
				if (itemAtIndex != null)
				{
					m_items.RemoveAt(j);
					m_items.Insert(j, ToolbarItem.FromItem(itemAtIndex));
				}
			}
			Toolbar.ItemChanged += Toolbar_ItemChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.RequiredInputChanged += Receiver_RequiredInputChanged;
			base.ResourceSink.Update();
			m_fieldShape = GetHkShape();
			base.OnClose += delegate
			{
				m_fieldShape.RemoveReference();
			};
			m_gizmoColor = new Vector4(0.35f, 0f, 0f, 0.5f);
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.ResourceSink.Update();
			UpdateEmissive();
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public bool UpdateEmissive()
		{
			if (base.IsWorking && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return SetEmissiveState(IsActive ? MyCubeBlock.m_emissiveNames.Alternative : MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return SetEmissiveState(base.IsFunctional ? MyCubeBlock.m_emissiveNames.Disabled : MyCubeBlock.m_emissiveNames.Damaged, base.Render.RenderObjectIDs[0]);
		}

		protected void UpdateField()
		{
			m_recreateField = true;
		}

		protected HkShape GetHkShape()
		{
			return new HkBoxShape((m_fieldMax.Value - m_fieldMin.Value) * 0.5f);
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			UpdateEmissive();
			base.OnEnabledChanged();
		}

		protected float CalculateRequiredPowerInput()
		{
			if (base.Enabled && base.IsFunctional)
			{
				return 0.0003f * (float)Math.Pow((m_fieldMax.Value - m_fieldMin.Value).Volume, 0.3333333432674408);
			}
			return 0f;
		}

		protected void Receiver_IsPoweredChanged()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					UpdateIsWorking();
					base.ResourceSink.Update();
					UpdateEmissive();
					SetDetailedInfoDirty();
					RaisePropertiesChanged();
				}
			}, "MySensorBlock::Receiver_IsPoweredChanged");
		}

		protected void Receiver_RequiredInputChanged(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver, float oldRequirement, float newRequirement)
		{
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			UpdateEmissive();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SensorBlock obj = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_SensorBlock;
			obj.FieldMin = FieldMin;
			obj.FieldMax = FieldMax;
			obj.PlaySound = PlayProximitySound;
			obj.DetectPlayers = DetectPlayers;
			obj.DetectFloatingObjects = DetectFloatingObjects;
			obj.DetectSmallShips = DetectSmallShips;
			obj.DetectLargeShips = DetectLargeShips;
			obj.DetectStations = DetectStations;
			obj.DetectSubgrids = DetectSubgrids;
			obj.DetectAsteroids = DetectAsteroids;
			obj.DetectOwner = DetectOwner;
			obj.DetectFriendly = DetectFriendly;
			obj.DetectNeutral = DetectNeutral;
			obj.DetectEnemy = DetectEnemy;
			obj.IsActive = IsActive;
			obj.Toolbar = Toolbar.GetObjectBuilder();
			return obj;
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index)
		{
			if (m_syncing)
			{
				return;
			}
			ToolbarItem toolbarItem = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
			ToolbarItem toolbarItem2 = m_items[index.ItemIndex];
			if ((toolbarItem.EntityID != 0L || toolbarItem2.EntityID != 0L) && (toolbarItem.EntityID == 0L || toolbarItem2.EntityID == 0L || !toolbarItem.Equals(toolbarItem2)))
			{
				m_items.RemoveAt(index.ItemIndex);
				m_items.Insert(index.ItemIndex, toolbarItem);
				MyMultiplayer.RaiseEvent(this, (MySensorBlock x) => x.SendToolbarItemChanged, toolbarItem, index.ItemIndex);
				if (m_shouldSetOtherToolbars)
				{
					m_shouldSetOtherToolbars = false;
					foreach (MyToolbar openedToolbar in m_openedToolbars)
					{
						if (openedToolbar != self)
						{
							openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
						}
					}
					m_shouldSetOtherToolbars = true;
				}
			}
		}

		private void OnFirstEnter()
		{
			UpdateEmissive();
			Toolbar.UpdateItem(0);
			if (!Sync.IsServer)
			{
				return;
			}
			Toolbar.ActivateItemAtSlot(0, checkIfWantsToBeActivated: false, playActivationSound: false);
			if (PlayProximitySound)
			{
				PlayActionSound();
				if (m_enablePlaySoundEvent)
				{
					MyMultiplayer.RaiseEvent(this, (MySensorBlock x) => x.PlayActionSound);
				}
			}
		}

		private void OnLastLeave()
		{
			UpdateEmissive();
			Toolbar.UpdateItem(1);
			if (Sync.IsServer)
			{
				Toolbar.ActivateItemAtSlot(1, checkIfWantsToBeActivated: false, playActivationSound: false);
			}
		}

		public bool ShouldDetectRelation(MyRelationsBetweenPlayerAndBlock relation)
		{
			switch (relation)
			{
			case MyRelationsBetweenPlayerAndBlock.Owner:
				return DetectOwner;
			case MyRelationsBetweenPlayerAndBlock.NoOwnership:
			case MyRelationsBetweenPlayerAndBlock.FactionShare:
			case MyRelationsBetweenPlayerAndBlock.Friends:
				return DetectFriendly;
			case MyRelationsBetweenPlayerAndBlock.Neutral:
				return DetectNeutral;
			case MyRelationsBetweenPlayerAndBlock.Enemies:
				return DetectEnemy;
			default:
				throw new InvalidBranchException();
			}
		}

		public bool ShouldDetectGrid(MyCubeGrid grid)
		{
			bool flag = true;
			foreach (long bigOwner in grid.BigOwners)
			{
				MyRelationsBetweenPlayerAndBlock relationBetweenPlayers = MyPlayer.GetRelationBetweenPlayers(base.OwnerId, bigOwner);
				if (ShouldDetectRelation(relationBetweenPlayers))
				{
					return true;
				}
				flag = false;
			}
			if (flag)
			{
				return ShouldDetectRelation(MyRelationsBetweenPlayerAndBlock.Enemies);
			}
			return false;
		}

		private bool ShouldDetect(MyEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (entity == base.CubeGrid)
			{
				return false;
			}
			if (DetectPlayers && entity is MyCharacter)
			{
				return ShouldDetectRelation((entity as MyCharacter).GetRelationTo(base.OwnerId));
			}
			if (DetectFloatingObjects && entity is MyFloatingObject)
			{
				return true;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (DetectSubgrids && myCubeGrid != null && MyCubeGridGroups.Static.Logical.HasSameGroup(myCubeGrid, base.CubeGrid))
			{
				return ShouldDetectGrid(myCubeGrid);
			}
			if (myCubeGrid != null && MyCubeGridGroups.Static.Logical.HasSameGroup(myCubeGrid, base.CubeGrid))
			{
				return false;
			}
			if (DetectSmallShips && myCubeGrid != null && myCubeGrid.GridSizeEnum == MyCubeSize.Small)
			{
				return ShouldDetectGrid(myCubeGrid);
			}
			if (DetectLargeShips && myCubeGrid != null && myCubeGrid.GridSizeEnum == MyCubeSize.Large && !myCubeGrid.IsStatic)
			{
				return ShouldDetectGrid(myCubeGrid);
			}
			if (DetectStations && myCubeGrid != null && myCubeGrid.GridSizeEnum == MyCubeSize.Large && myCubeGrid.IsStatic)
			{
				return ShouldDetectGrid(myCubeGrid);
			}
			if (DetectAsteroids && entity is MyVoxelBase)
			{
				return true;
			}
			return false;
		}

		private bool GetPropertiesFromEntity(MyEntity entity, ref Vector3D position1, out Quaternion rotation2, out Vector3 posDiff, out HkShape? shape2)
		{
			rotation2 = default(Quaternion);
			posDiff = Vector3.Zero;
			shape2 = null;
			if (entity.Physics == null || !entity.Physics.Enabled)
			{
				return false;
			}
			if (entity.Physics.RigidBody != null)
			{
				shape2 = entity.Physics.RigidBody.GetShape();
				MatrixD worldMatrix = entity.WorldMatrix;
				rotation2 = Quaternion.CreateFromForwardUp(worldMatrix.Forward, worldMatrix.Up);
				posDiff = entity.PositionComp.GetPosition() - position1;
				if (entity is MyVoxelBase)
				{
					MyVoxelBase myVoxelBase = entity as MyVoxelBase;
					posDiff -= (Vector3)(myVoxelBase.Size / 2);
				}
			}
			else
			{
				if (entity.GetPhysicsBody().CharacterProxy == null)
				{
					return false;
				}
				shape2 = entity.GetPhysicsBody().CharacterProxy.GetShape();
				MatrixD worldMatrix2 = entity.WorldMatrix;
				rotation2 = Quaternion.CreateFromForwardUp(worldMatrix2.Forward, worldMatrix2.Up);
				posDiff = entity.PositionComp.WorldAABB.Center - position1;
			}
			return true;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			m_enablePlaySoundEvent = true;
			if (!Sync.IsServer || !base.IsWorking)
			{
				return;
			}
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				if (!base.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, BlockDefinition.RequiredPowerInput))
				{
					return;
				}
				float newRequiredInput = base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId);
				base.ResourceSink.SetRequiredInputByType(MyResourceDistributorComponent.ElectricityId, 0f);
				base.ResourceSink.SetRequiredInputByType(MyResourceDistributorComponent.ElectricityId, newRequiredInput);
			}
			Quaternion rotation = Quaternion.CreateFromForwardUp(base.WorldMatrix.Forward, base.WorldMatrix.Up);
			Vector3D position = base.PositionComp.GetPosition() + Vector3D.Transform(base.PositionComp.LocalVolume.Center + (m_fieldMax.Value + m_fieldMin.Value) * 0.5f, rotation);
			if (m_recreateField)
			{
				m_recreateField = false;
				m_fieldShape.RemoveReference();
				m_fieldShape = GetHkShape();
				base.ResourceSink.Update();
			}
			BoundingBoxD box = new BoundingBoxD(m_fieldMin.Value, m_fieldMax.Value).Translate(base.PositionComp.LocalVolume.Center).TransformFast(base.WorldMatrix.GetOrientation()).Translate(base.PositionComp.GetPosition());
			m_potentialPenetrations.Clear();
			MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, m_potentialPenetrations);
			m_potentialVoxelPenetrations.Clear();
			MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_potentialVoxelPenetrations);
			LastDetectedEntity = null;
			m_detectedEntities.Clear();
			Parallel.ForEach(m_potentialPenetrations, delegate(MyEntity entity)
			{
				if (!(entity is MyVoxelBase) && ShouldDetect(entity) && GetPropertiesFromEntity(entity, ref position, out Quaternion rotation3, out Vector3 posDiff2, out HkShape? shape2) && entity.GetPhysicsBody().HavokWorld.IsPenetratingShapeShape(m_fieldShape, ref Vector3.Zero, ref rotation, shape2.Value, ref posDiff2, ref rotation3))
				{
					LastDetectedEntity = entity;
					m_detectedEntities.Add(MyDetectedEntityInfoHelper.Create(entity, base.OwnerId));
				}
			});
			if (DetectAsteroids)
			{
				foreach (MyVoxelBase potentialVoxelPenetration in m_potentialVoxelPenetrations)
				{
					MyVoxelPhysics myVoxelPhysics = potentialVoxelPenetration as MyVoxelPhysics;
					Quaternion rotation2;
					Vector3 posDiff;
					HkShape? shape;
					if (myVoxelPhysics != null)
					{
						MyVoxelCoordSystems.WorldPositionToLocalPosition(box.Min, myVoxelPhysics.PositionComp.WorldMatrix, myVoxelPhysics.PositionComp.WorldMatrixInvScaled, myVoxelPhysics.SizeInMetresHalf, out Vector3 localPosition);
						MyVoxelCoordSystems.WorldPositionToLocalPosition(box.Max, myVoxelPhysics.PositionComp.WorldMatrix, myVoxelPhysics.PositionComp.WorldMatrixInvScaled, myVoxelPhysics.SizeInMetresHalf, out Vector3 localPosition2);
						BoundingBoxI box2 = new BoundingBoxI(new Vector3I(localPosition), new Vector3I(localPosition2));
						box2.Translate(myVoxelPhysics.StorageMin);
						if (myVoxelPhysics.Storage.Intersect(ref box2, 1, exhaustiveContainmentCheck: false) != 0)
						{
							LastDetectedEntity = potentialVoxelPenetration;
							m_detectedEntities.Add(MyDetectedEntityInfoHelper.Create(potentialVoxelPenetration, base.OwnerId));
						}
					}
					else if (GetPropertiesFromEntity(potentialVoxelPenetration, ref position, out rotation2, out posDiff, out shape) && potentialVoxelPenetration.GetPhysicsBody().HavokWorld.IsPenetratingShapeShape(m_fieldShape, ref Vector3.Zero, ref rotation, shape.Value, ref posDiff, ref rotation2))
					{
						LastDetectedEntity = potentialVoxelPenetration;
						m_detectedEntities.Add(MyDetectedEntityInfoHelper.Create(potentialVoxelPenetration, base.OwnerId));
					}
				}
			}
			IsActive = (m_detectedEntities.Count > 0);
			m_potentialPenetrations.Clear();
			m_potentialVoxelPenetrations.Clear();
		}

		public Color GetGizmoColor()
		{
			return m_gizmoColor;
		}

		public bool CanBeDrawn()
		{
			if (!MyCubeGrid.ShowSenzorGizmos || !base.ShowOnHUD || !base.IsWorking || !HasLocalPlayerAccess() || GetDistanceBetweenPlayerPositionAndBoundingSphere() > 400.0)
			{
				return false;
			}
			return true;
		}

		public BoundingBox? GetBoundingBox()
		{
			m_gizmoBoundingBox.Min = base.PositionComp.LocalVolume.Center + FieldMin;
			m_gizmoBoundingBox.Max = base.PositionComp.LocalVolume.Center + FieldMax;
			return m_gizmoBoundingBox;
		}

		public float GetRadius()
		{
			return -1f;
		}

		public MatrixD GetWorldMatrix()
		{
			return base.WorldMatrix;
		}

		public Vector3 GetPositionInGrid()
		{
			return base.Position;
		}

		public bool EnableLongDrawDistance()
		{
			return false;
		}

		private void IsActiveChanged()
		{
			if ((bool)m_active)
			{
				OnFirstEnter();
			}
			else
			{
				OnLastLeave();
			}
			if ((bool)m_active)
			{
				m_gizmoColor = new Vector4(0f, 0.35f, 0f, 0.5f);
			}
			else
			{
				m_gizmoColor = new Vector4(0.35f, 0f, 0f, 0.5f);
			}
			this.StateChanged?.Invoke(m_active);
		}

		[Event(null, 1180)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
		{
			m_syncing = true;
			MyToolbarItem item = null;
			if (sentItem.EntityID != 0L)
			{
				item = ToolbarItem.ToItem(sentItem);
			}
			Toolbar.SetItemAtIndex(index, item);
			m_syncing = false;
		}

		[Event(null, 1193)]
		[Reliable]
		[Broadcast]
		private void PlayActionSound()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.PlaySound(m_actionSound, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, true);
			}
		}

		void Sandbox.ModAPI.Ingame.IMySensorBlock.DetectedEntities(List<MyDetectedEntityInfo> result)
		{
			result.Clear();
			result.AddRange(m_detectedEntities);
		}
	}
}
