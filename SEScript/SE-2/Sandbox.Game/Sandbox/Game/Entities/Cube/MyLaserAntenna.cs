using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_LaserAntenna))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyLaserAntenna),
		typeof(Sandbox.ModAPI.Ingame.IMyLaserAntenna)
	})]
	public class MyLaserAntenna : MyFunctionalBlock, IMyGizmoDrawableObject, Sandbox.ModAPI.IMyLaserAntenna, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyLaserAntenna
	{
		public enum StateEnum : byte
		{
			idle,
			rot_GPS,
			search_GPS,
			rot_Rec,
			contact_Rec,
			connected
		}

		protected sealed class PasteCoordinatesSuccess_003C_003ESystem_String : ICallSite<MyLaserAntenna, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLaserAntenna @this, in string coords, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.PasteCoordinatesSuccess(coords);
			}
		}

		protected sealed class ChangePermRequest_003C_003ESystem_Boolean : ICallSite<MyLaserAntenna, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLaserAntenna @this, in bool isPerm, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ChangePermRequest(isPerm);
			}
		}

		protected sealed class OnChangeModeRequest_003C_003ESandbox_Game_Entities_Cube_MyLaserAntenna_003C_003EStateEnum_0023System_Nullable_00601_003CVRageMath_Vector3D_003E : ICallSite<MyLaserAntenna, StateEnum, Vector3D?, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLaserAntenna @this, in StateEnum mode, in Vector3D? coords, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeModeRequest(mode, coords);
			}
		}

		protected sealed class OnConnectToRecRequest_003C_003ESystem_Int64 : ICallSite<MyLaserAntenna, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLaserAntenna @this, in long targetEntityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConnectToRecRequest(targetEntityId);
			}
		}

		protected sealed class OnConnectToRecSuccess_003C_003ESystem_Int64_0023VRageMath_Vector3D_0023System_String : ICallSite<MyLaserAntenna, long, Vector3D, string, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyLaserAntenna @this, in long targetEntityId, in Vector3D targetCoords, in string name, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnConnectToRecSuccess(targetEntityId, targetCoords, name);
			}
		}

		protected class m_range_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType range;
				ISyncType result = range = new Sync<float, SyncDirection.BothWays>(P_1, P_2);
				((MyLaserAntenna)P_0).m_range = (Sync<float, SyncDirection.BothWays>)range;
				return result;
			}
		}

		protected class m_permanentConnection_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType permanentConnection;
				ISyncType result = permanentConnection = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyLaserAntenna)P_0).m_permanentConnection = (Sync<bool, SyncDirection.FromServer>)permanentConnection;
				return result;
			}
		}

		protected class m_canLaseTargetCoords_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType canLaseTargetCoords;
				ISyncType result = canLaseTargetCoords = new Sync<bool, SyncDirection.FromServer>(P_1, P_2);
				((MyLaserAntenna)P_0).m_canLaseTargetCoords = (Sync<bool, SyncDirection.FromServer>)canLaseTargetCoords;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyLaserAntenna_003C_003EActor : IActivator, IActivator<MyLaserAntenna>
		{
			private sealed override object CreateInstance()
			{
				return new MyLaserAntenna();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLaserAntenna CreateInstance()
			{
				return new MyLaserAntenna();
			}

			MyLaserAntenna IActivator<MyLaserAntenna>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected Color m_gizmoColor = new Vector4(0.1f, 0.1f, 0f, 0.1f);

		protected const float m_maxGizmoDrawDistance = 10000f;

		private bool m_onceUpdated;

		private const double PHYSICS_CAST_LENGTH = 25.0;

		private const float INFINITE_RANGE = 1E+08f;

		private bool resetPartsParent;

		private StateEnum m_state;

		private long? m_targetId;

		private readonly StringBuilder m_lastKnownTargetName = new StringBuilder();

		private static string m_clipboardText;

		private readonly StringBuilder m_termGpsName = new StringBuilder();

		private Vector3D? m_termGpsCoords;

		private long? m_selectedEntityId;

		private bool m_rotationFinished = true;

		private float m_needRotation;

		private float m_needElevation;

		private float m_minElevationRadians;

		private float m_maxElevationRadians = MathF.PI * 2f;

		private float m_minAzimuthRadians;

		private float m_maxAzimuthRadians = MathF.PI * 2f;

		private bool m_outsideLimits;

		private Vector3D m_targetCoords;

		private float m_maxRange;

		private Sync<float, SyncDirection.BothWays> m_range;

		protected static float m_Max_LosDist;

		private Sync<bool, SyncDirection.FromServer> m_permanentConnection;

		private bool m_OnlyPermanentExists;

		public bool m_needLineOfSight = true;

		private static MyTerminalControlButton<MyLaserAntenna> idleButton;

		private static MyTerminalControlButton<MyLaserAntenna> connectGPS;

		private static MyTerminalControlListbox<MyLaserAntenna> receiversList;

		private static MyTerminalControlTextbox<MyLaserAntenna> gpsCoords;

		private static MyTerminalControlButton<MyLaserAntenna> PasteGpsCoords;

		private static MyTerminalControlButton<MyLaserAntenna> ConnectReceiver;

		private Vector3D m_temp;

		protected StringBuilder m_tempSB = new StringBuilder();

		protected Sync<bool, SyncDirection.FromServer> m_canLaseTargetCoords;

		private List<MyPhysics.HitInfo> m_hits = new List<MyPhysics.HitInfo>();

		private List<MyLineSegmentOverlapResult<MyVoxelBase>> m_voxelHits = new List<MyLineSegmentOverlapResult<MyVoxelBase>>();

		private List<MyLineSegmentOverlapResult<MyEntity>> m_entityHits = new List<MyLineSegmentOverlapResult<MyEntity>>();

		private bool m_wasVisible;

		private float m_rotation;

		private float m_elevation;

		protected MyEntity m_base1;

		protected MyEntity m_base2;

		protected int m_rotationInterval_ms;

		protected int m_elevationInterval_ms;

		private MyLaserBroadcaster Broadcaster
		{
			get
			{
				return (MyLaserBroadcaster)base.Components.Get<MyDataBroadcaster>();
			}
			set
			{
				base.Components.Add((MyDataBroadcaster)value);
			}
		}

		private MyLaserReceiver Receiver
		{
			get
			{
				return (MyLaserReceiver)base.Components.Get<MyDataReceiver>();
			}
			set
			{
				base.Components.Add((MyDataReceiver)value);
			}
		}

		public StateEnum State
		{
			get
			{
				return m_state;
			}
			private set
			{
				m_state = value;
				if (m_state == StateEnum.idle || m_state == StateEnum.rot_GPS)
				{
					m_targetId = null;
				}
			}
		}

		public long? TargetId => m_targetId;

		public Vector3D HeadPos
		{
			get
			{
				if (m_base2 != null)
				{
					return m_base2.PositionComp.GetPosition();
				}
				return base.PositionComp.GetPosition();
			}
		}

		public MatrixD InitializationMatrix
		{
			get;
			private set;
		}

		public Vector3D TargetCoords => m_targetCoords;

		public bool CanLaseTargetCoords => m_canLaseTargetCoords;

		public new MyLaserAntennaDefinition BlockDefinition => (MyLaserAntennaDefinition)base.BlockDefinition;

		private MatrixD InitializationMatrixWorld => InitializationMatrix * base.Parent.WorldMatrix;

		Vector3D Sandbox.ModAPI.Ingame.IMyLaserAntenna.TargetCoords => m_targetCoords;

		bool Sandbox.ModAPI.Ingame.IMyLaserAntenna.IsPermanent
		{
			get
			{
				return m_permanentConnection.Value;
			}
			set
			{
				ChangePerm(value);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyLaserAntenna.RequireLoS => m_needLineOfSight;

		bool Sandbox.ModAPI.Ingame.IMyLaserAntenna.IsOutsideLimits => m_outsideLimits;

		MyLaserAntennaStatus Sandbox.ModAPI.Ingame.IMyLaserAntenna.Status
		{
			get
			{
				if (m_outsideLimits)
				{
					return MyLaserAntennaStatus.OutOfRange;
				}
				switch (State)
				{
				case StateEnum.idle:
					return MyLaserAntennaStatus.Idle;
				case StateEnum.rot_GPS:
					return MyLaserAntennaStatus.RotatingToTarget;
				case StateEnum.search_GPS:
					return MyLaserAntennaStatus.SearchingTargetForAntenna;
				case StateEnum.rot_Rec:
					return MyLaserAntennaStatus.RotatingToTarget;
				case StateEnum.contact_Rec:
					return MyLaserAntennaStatus.Connecting;
				case StateEnum.connected:
					return MyLaserAntennaStatus.Connected;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		Sandbox.ModAPI.IMyLaserAntenna Sandbox.ModAPI.IMyLaserAntenna.Other => GetOther();

		float Sandbox.ModAPI.Ingame.IMyLaserAntenna.Range
		{
			get
			{
				return m_range.Value;
			}
			set
			{
				m_range.Value = value;
			}
		}

		public Color GetGizmoColor()
		{
			return m_gizmoColor;
		}

		public Vector3 GetPositionInGrid()
		{
			return base.Position;
		}

		public float GetRadius()
		{
			return 100f;
		}

		public bool CanBeDrawn()
		{
			if (!MyCubeGrid.ShowAntennaGizmos || !base.IsWorking || !HasLocalPlayerAccess() || GetDistanceBetweenCameraAndBoundingSphere() > 10000.0)
			{
				return false;
			}
			return true;
		}

		public BoundingBox? GetBoundingBox()
		{
			return null;
		}

		public MatrixD GetWorldMatrix()
		{
			return base.PositionComp.WorldMatrix;
		}

		public bool EnableLongDrawDistance()
		{
			return true;
		}

		public MyLaserAntenna()
		{
			CreateTerminalControls();
		}

		static MyLaserAntenna()
		{
			m_Max_LosDist = 10000f;
			m_Max_LosDist = MySession.Static.Settings.ViewDistance;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyLaserAntenna>())
			{
				base.CreateTerminalControls();
				idleButton = new MyTerminalControlButton<MyLaserAntenna>("Idle", MySpaceTexts.LaserAntennaIdleButton, MySpaceTexts.Blank, delegate(MyLaserAntenna self)
				{
					self.SetIdle();
					idleButton.UpdateVisual();
				});
				idleButton.Enabled = ((MyLaserAntenna x) => x.m_state != StateEnum.idle);
				idleButton.EnableAction();
				MyTerminalControlFactory.AddControl(idleButton);
				MyTerminalControlSlider<MyLaserAntenna> myTerminalControlSlider = new MyTerminalControlSlider<MyLaserAntenna>("Range", MySpaceTexts.BlockPropertyTitle_LaserRange, MySpaceTexts.BlockPropertyDescription_LaserRange);
				myTerminalControlSlider.SetLogLimits((MyLaserAntenna block) => 1f, (MyLaserAntenna block) => (!(block.m_maxRange < 0f)) ? block.m_maxRange : 1E+08f);
				myTerminalControlSlider.DefaultValueGetter = ((MyLaserAntenna block) => (!(block.m_maxRange < 0f)) ? block.m_maxRange : 1E+08f);
				myTerminalControlSlider.Getter = ((MyLaserAntenna x) => x.m_range.Value);
				myTerminalControlSlider.Setter = delegate(MyLaserAntenna x, float v)
				{
					x.m_range.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MyLaserAntenna x, StringBuilder result)
				{
					if (x.m_range.Value < 1E+08f)
					{
						MyValueFormatter.AppendDistanceInBestUnit((int)x.m_range.Value, result);
					}
					else
					{
						result.Append(MyTexts.GetString(MySpaceTexts.ScreenTerminal_Infinite));
					}
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyLaserAntenna>());
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyLaserAntenna>("CopyCoords", MySpaceTexts.LaserAntennaCopyCoords, MySpaceTexts.LaserAntennaCopyCoordsHelp, delegate(MyLaserAntenna self)
				{
					StringBuilder stringBuilder3 = new StringBuilder(self.DisplayNameText);
					stringBuilder3.Replace(':', ' ');
					StringBuilder stringBuilder4 = new StringBuilder("GPS:", 256);
					stringBuilder4.Append((object)stringBuilder3);
					stringBuilder4.Append(":");
					stringBuilder4.Append(Math.Round(self.HeadPos.X, 2).ToString(CultureInfo.InvariantCulture));
					stringBuilder4.Append(":");
					stringBuilder4.Append(Math.Round(self.HeadPos.Y, 2).ToString(CultureInfo.InvariantCulture));
					stringBuilder4.Append(":");
					stringBuilder4.Append(Math.Round(self.HeadPos.Z, 2).ToString(CultureInfo.InvariantCulture));
					stringBuilder4.Append(":");
					MyVRage.Platform.Clipboard = stringBuilder4.ToString();
				}));
				MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyLaserAntenna>("CopyTargetCoords", MySpaceTexts.LaserAntennaCopyTargetCoords, MySpaceTexts.LaserAntennaCopyTargetCoordsHelp, delegate(MyLaserAntenna self)
				{
					if (self.m_targetId.HasValue)
					{
						StringBuilder stringBuilder = new StringBuilder(self.m_lastKnownTargetName.ToString());
						stringBuilder.Replace(':', ' ');
						StringBuilder stringBuilder2 = new StringBuilder("GPS:", 256);
						stringBuilder2.Append((object)stringBuilder);
						stringBuilder2.Append(":");
						stringBuilder2.Append(Math.Round(self.m_targetCoords.X, 2).ToString(CultureInfo.InvariantCulture));
						stringBuilder2.Append(":");
						stringBuilder2.Append(Math.Round(self.m_targetCoords.Y, 2).ToString(CultureInfo.InvariantCulture));
						stringBuilder2.Append(":");
						stringBuilder2.Append(Math.Round(self.m_targetCoords.Z, 2).ToString(CultureInfo.InvariantCulture));
						stringBuilder2.Append(":");
						MyVRage.Platform.Clipboard = stringBuilder2.ToString();
					}
				})
				{
					Enabled = ((MyLaserAntenna x) => x.m_targetId.HasValue)
				});
				PasteGpsCoords = new MyTerminalControlButton<MyLaserAntenna>("PasteGpsCoords", MySpaceTexts.LaserAntennaPasteGPS, MySpaceTexts.Blank, delegate(MyLaserAntenna self)
				{
					Thread thread = new Thread((ThreadStart)delegate
					{
						PasteFromClipboard();
					});
					thread.SetApartmentState(ApartmentState.STA);
					thread.Start();
					thread.Join();
					self.PasteCoordinates(m_clipboardText);
				});
				PasteGpsCoords.EnableAction();
				MyTerminalControlFactory.AddControl(PasteGpsCoords);
				gpsCoords = new MyTerminalControlTextbox<MyLaserAntenna>("gpsCoords", MySpaceTexts.LaserAntennaSelectedCoords, MySpaceTexts.Blank);
				gpsCoords.Getter = ((MyLaserAntenna x) => x.m_termGpsName);
				gpsCoords.Enabled = ((MyLaserAntenna x) => false);
				MyTerminalControlFactory.AddControl(gpsCoords);
				connectGPS = new MyTerminalControlButton<MyLaserAntenna>("ConnectGPS", MySpaceTexts.LaserAntennaConnectGPS, MySpaceTexts.Blank, delegate(MyLaserAntenna self)
				{
					if (self.m_termGpsCoords.HasValue)
					{
						self.ConnectToGps();
					}
				});
				connectGPS.Enabled = ((MyLaserAntenna x) => x.CanConnectToGPS());
				connectGPS.EnableAction();
				MyTerminalControlFactory.AddControl(connectGPS);
				MyTerminalControlCheckbox<MyLaserAntenna> obj = new MyTerminalControlCheckbox<MyLaserAntenna>("isPerm", MySpaceTexts.LaserAntennaPermanentCheckbox, MySpaceTexts.Blank)
				{
					Getter = ((MyLaserAntenna self) => self.m_permanentConnection.Value),
					Setter = delegate(MyLaserAntenna self, bool v)
					{
						self.ChangePerm(v);
					},
					Enabled = ((MyLaserAntenna self) => self.State == StateEnum.connected)
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyLaserAntenna>());
				receiversList = new MyTerminalControlListbox<MyLaserAntenna>("receiversList", MySpaceTexts.LaserAntennaReceiversList, MySpaceTexts.LaserAntennaReceiversListHelp);
				receiversList.ListContent = delegate(MyLaserAntenna x, ICollection<MyGuiControlListbox.Item> population, ICollection<MyGuiControlListbox.Item> selected)
				{
					x.PopulatePossibleReceivers(population, selected);
				};
				receiversList.ItemSelected = delegate(MyLaserAntenna x, List<MyGuiControlListbox.Item> y)
				{
					x.ReceiverSelected(y);
				};
				MyTerminalControlFactory.AddControl(receiversList);
				ConnectReceiver = new MyTerminalControlButton<MyLaserAntenna>("ConnectReceiver", MySpaceTexts.LaserAntennaConnectButton, MySpaceTexts.Blank, delegate(MyLaserAntenna self)
				{
					self.ConnectToId();
				});
				ConnectReceiver.Enabled = ((MyLaserAntenna x) => x.m_selectedEntityId.HasValue);
				MyTerminalControlFactory.AddControl(ConnectReceiver);
			}
		}

		private static void UpdateVisuals()
		{
			gpsCoords.UpdateVisual();
			idleButton.UpdateVisual();
			connectGPS.UpdateVisual();
			receiversList.UpdateVisual();
			ConnectReceiver.UpdateVisual();
		}

		private bool CanConnectToGPS()
		{
			if (!m_termGpsCoords.HasValue)
			{
				return false;
			}
			if (Dist2To(m_termGpsCoords) < 1.0)
			{
				return false;
			}
			return true;
		}

		private static void PasteFromClipboard()
		{
			m_clipboardText = MyVRage.Platform.Clipboard;
		}

		public void DoPasteCoords(string str)
		{
			if (MyGpsCollection.ParseOneGPS(str, m_termGpsName, ref m_temp))
			{
				if (!m_termGpsCoords.HasValue)
				{
					m_termGpsCoords = m_temp;
				}
				m_termGpsCoords = m_temp;
				m_termGpsName.Append(" ").Append(m_temp.X).Append(":")
					.Append(m_temp.Y)
					.Append(":")
					.Append(m_temp.Z);
				ConnectToGps(m_termGpsCoords);
			}
			UpdateVisuals();
		}

		protected void UpdateMyStateText()
		{
			StringBuilder stateText = Broadcaster.StateText;
			stateText.Clear().Append((object)base.CustomName);
			stateText.Append(" [");
			switch (State)
			{
			case StateEnum.idle:
				stateText.Append(State);
				break;
			case StateEnum.connected:
				if (m_permanentConnection.Value)
				{
					stateText.Append("#=>");
				}
				else
				{
					stateText.Append("=>");
				}
				break;
			case StateEnum.rot_GPS:
			case StateEnum.rot_Rec:
				if (m_permanentConnection.Value)
				{
					stateText.Append("#>>");
				}
				else
				{
					stateText.Append(">>");
				}
				break;
			case StateEnum.search_GPS:
				stateText.Append("?>");
				break;
			case StateEnum.contact_Rec:
				if (m_permanentConnection.Value)
				{
					stateText.Append("#~>");
				}
				else
				{
					stateText.Append("~>");
				}
				break;
			}
			if (State == StateEnum.connected || State == StateEnum.contact_Rec || State == StateEnum.rot_Rec)
			{
				stateText.Append((object)m_lastKnownTargetName);
			}
			else if (State == StateEnum.rot_GPS || State == StateEnum.search_GPS)
			{
				stateText.Append((object)m_termGpsName);
				stateText.Append(" ");
				stateText.Append(m_termGpsCoords);
			}
			stateText.Append("]");
			Broadcaster.RaiseChangeStateText();
		}

		protected void PopulatePossibleReceivers(ICollection<MyGuiControlListbox.Item> population, ICollection<MyGuiControlListbox.Item> selected)
		{
			if (MySession.Static != null && !base.Closed)
			{
				foreach (MyLaserBroadcaster value in MyAntennaSystem.Static.LaserAntennas.Values)
				{
					if (value != Broadcaster && (value.RealAntenna == null || (value.RealAntenna.Enabled && value.RealAntenna.IsFunctional && base.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f)) && value.CanBeUsedByPlayer(base.OwnerId) && Broadcaster.CanBeUsedByPlayer(value.Owner) && MyAntennaSystem.Static.GetAllRelayedBroadcasters(Receiver, base.OwnerId, mutual: false).Contains(value))
					{
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(ref value.StateText, null, null, value);
						population.Add(item);
						if (m_selectedEntityId == value.AntennaEntityId)
						{
							selected.Add(item);
						}
					}
				}
				ConnectReceiver.UpdateVisual();
			}
		}

		protected void ReceiverSelected(List<MyGuiControlListbox.Item> y)
		{
			m_selectedEntityId = (y.First().UserData as MyLaserBroadcaster).AntennaEntityId;
			ConnectReceiver.UpdateVisual();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.ResourceSink = new MyResourceSinkComponent();
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.PowerInputLasing, UpdatePowerInput);
			Broadcaster = new MyLaserBroadcaster();
			Receiver = new MyLaserReceiver();
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_LaserAntenna myObjectBuilder_LaserAntenna = (MyObjectBuilder_LaserAntenna)objectBuilder;
			State = (StateEnum)(myObjectBuilder_LaserAntenna.State & 7);
			m_permanentConnection.ValueChanged += PermanentConnectionOnValueChanged;
			m_permanentConnection.SetLocalValue((myObjectBuilder_LaserAntenna.State & 8) != 0);
			m_targetId = myObjectBuilder_LaserAntenna.targetEntityId;
			m_lastKnownTargetName.Append(myObjectBuilder_LaserAntenna.LastKnownTargetName);
			if (myObjectBuilder_LaserAntenna.gpsTarget.HasValue)
			{
				m_termGpsCoords = myObjectBuilder_LaserAntenna.gpsTarget;
			}
			m_termGpsName.Clear().Append(myObjectBuilder_LaserAntenna.gpsTargetName);
			m_rotation = myObjectBuilder_LaserAntenna.HeadRotation.X;
			m_elevation = myObjectBuilder_LaserAntenna.HeadRotation.Y;
			m_targetCoords = myObjectBuilder_LaserAntenna.LastTargetPosition;
			m_maxRange = BlockDefinition.MaxRange;
			m_range.Value = myObjectBuilder_LaserAntenna.Range;
			m_needLineOfSight = BlockDefinition.RequireLineOfSight;
			if (BlockDefinition != null)
			{
				m_minElevationRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MinElevationDegrees));
				m_maxElevationRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MaxElevationDegrees));
				if (m_minElevationRadians > m_maxElevationRadians)
				{
					m_minElevationRadians -= MathF.PI * 2f;
				}
				m_minAzimuthRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MinAzimuthDegrees));
				m_maxAzimuthRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MaxAzimuthDegrees));
				if (m_minAzimuthRadians > m_maxAzimuthRadians)
				{
					m_minAzimuthRadians -= MathF.PI * 2f;
				}
				ClampRotationAndElevation();
			}
			InitializationMatrix = base.PositionComp.LocalMatrix;
			base.ResourceSink.IsPoweredChanged += IsPoweredChanged;
			base.ResourceSink.Update();
			base.OnClose += delegate
			{
				OnClosed();
			};
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			Receiver.Enabled = base.IsWorking;
			UpdateEmissivity();
			base.NeedsUpdate = (MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME);
		}

		private void PermanentConnectionOnValueChanged(SyncBase obj)
		{
			if (base.IsReadyForReplication)
			{
				UpdateMyStateText();
			}
		}

		protected float NormalizeAngle(int angle)
		{
			int num = angle % 360;
			if (num == 0 && angle != 0)
			{
				return 360f;
			}
			return num;
		}

		protected void ClampRotationAndElevation()
		{
			float num = ClampRotation(m_rotation);
			float num2 = ClampElevation(m_elevation);
			if (num != m_rotation || num2 != m_elevation)
			{
				m_outsideLimits = true;
			}
			else
			{
				m_outsideLimits = false;
			}
			m_rotation = num;
			m_elevation = num2;
		}

		private float ClampRotation(float value)
		{
			if (IsRotationLimited())
			{
				value = Math.Min(m_maxAzimuthRadians, Math.Max(m_minAzimuthRadians, value));
			}
			return value;
		}

		private bool IsRotationLimited()
		{
			return (double)Math.Abs(m_maxAzimuthRadians - m_minAzimuthRadians - MathF.PI * 2f) > 0.01;
		}

		private float ClampElevation(float value)
		{
			if (IsElevationLimited())
			{
				value = Math.Min(m_maxElevationRadians, Math.Max(m_minElevationRadians, value));
			}
			return value;
		}

		private bool IsElevationLimited()
		{
			return (double)Math.Abs(m_maxElevationRadians - m_minElevationRadians - MathF.PI * 2f) > 0.01;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			SetParent();
			if (!m_onceUpdated)
			{
				if (base.CubeGrid.Physics != null)
				{
					MyAntennaSystem.Static.AddLaser(Broadcaster.AntennaEntityId, Broadcaster);
				}
				base.ResourceSink.Update();
				UpdateMyStateText();
				UpdateEmissivity();
			}
			m_onceUpdated = true;
		}

		private void SetParent()
		{
			MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
			if (m_base1 != null)
			{
				m_base1.Render.SetParent(0, orAddCell.ParentCullObject);
				m_base1.NeedsWorldMatrix = false;
				m_base1.InvalidateOnMove = false;
			}
			if (m_base2 != null)
			{
				m_base2.Render.SetParent(0, orAddCell.ParentCullObject);
				m_base2.NeedsWorldMatrix = false;
				m_base2.InvalidateOnMove = false;
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			resetPartsParent = true;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateEmissivity();
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			Broadcaster.RaiseOwnerChanged();
			Receiver.UpdateBroadcastersInRange();
			UpdateVisuals();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_LaserAntenna obj = (MyObjectBuilder_LaserAntenna)base.GetObjectBuilderCubeBlock(copy);
			obj.State = (byte)((int)State | (m_permanentConnection.Value ? 8 : 0));
			obj.targetEntityId = m_targetId;
			obj.gpsTarget = m_termGpsCoords;
			obj.gpsTargetName = m_termGpsName.ToString();
			obj.HeadRotation = new Vector2(m_rotation, m_elevation);
			obj.LastTargetPosition = m_targetCoords;
			obj.LastKnownTargetName = m_lastKnownTargetName.ToString();
			obj.Range = m_range.Value;
			return obj;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (resetPartsParent)
			{
				SetParent();
				resetPartsParent = false;
			}
			if (base.Enabled && base.IsFunctional && base.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f && !base.NeedsUpdate.HasFlag(MyEntityUpdateEnum.BEFORE_NEXT_FRAME))
			{
				if (State != 0)
				{
					GetRotationAndElevation(m_targetCoords, ref m_needRotation, ref m_needElevation);
				}
				RotationAndElevation(m_needRotation, m_needElevation);
				TryLaseTargetCoords();
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (base.Enabled && base.IsFunctional && base.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f)
			{
				TryUpdateTargetCoords();
				base.ResourceSink.Update();
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (!base.Enabled || !base.IsFunctional || !(base.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f))
			{
				return;
			}
			Receiver.UpdateBroadcastersInRange();
			TryUpdateTargetCoords();
			if (Sync.IsServer)
			{
				m_canLaseTargetCoords.Value = false;
			}
			switch (State)
			{
			case StateEnum.idle:
				break;
			case StateEnum.rot_GPS:
				if (m_rotationFinished)
				{
					ShiftModeSync(StateEnum.search_GPS);
				}
				break;
			case StateEnum.search_GPS:
				if (!m_rotationFinished)
				{
					ShiftModeSync(StateEnum.rot_GPS);
				}
				else
				{
					if (Sync.MultiplayerActive && !Sync.IsServer)
					{
						break;
					}
					MyLaserAntenna myLaserAntenna = null;
					double num = double.MaxValue;
					float num2 = float.MaxValue;
					bool flag = false;
					MyLaserAntenna myLaserAntenna2 = null;
					foreach (MyLaserBroadcaster value in MyAntennaSystem.Static.LaserAntennas.Values)
					{
						MyLaserAntenna realAntenna = value.RealAntenna;
						if (realAntenna.Enabled && realAntenna.IsFunctional && realAntenna.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f && !(realAntenna.m_permanentConnection.Value && flag) && realAntenna.Broadcaster.CanBeUsedByPlayer(base.OwnerId) && Broadcaster.CanBeUsedByPlayer(realAntenna.OwnerId) && realAntenna.EntityId != base.EntityId)
						{
							num = realAntenna.Dist2To(m_targetCoords);
							if (num < 100.0)
							{
								if (realAntenna.m_permanentConnection.Value)
								{
									flag = true;
									myLaserAntenna2 = value.RealAntenna;
								}
								else
								{
									if (realAntenna.State == StateEnum.idle)
									{
										myLaserAntenna = realAntenna;
										break;
									}
									if (num < (double)num2)
									{
										num = num2;
										myLaserAntenna = realAntenna;
									}
								}
							}
						}
					}
					if (myLaserAntenna == null)
					{
						if (m_OnlyPermanentExists)
						{
							if (!flag)
							{
								m_OnlyPermanentExists = false;
								SetDetailedInfoDirty();
								RaisePropertiesChanged();
							}
						}
						else if (flag && IsInRange(myLaserAntenna2) && LosTests(myLaserAntenna2))
						{
							m_OnlyPermanentExists = true;
							SetDetailedInfoDirty();
							RaisePropertiesChanged();
						}
					}
					else if (IsInRange(myLaserAntenna) && LosTests(myLaserAntenna))
					{
						ConnectToRec(myLaserAntenna.EntityId);
					}
				}
				break;
			case StateEnum.rot_Rec:
				if (m_rotationFinished)
				{
					ShiftModeSync(StateEnum.contact_Rec);
				}
				break;
			case StateEnum.contact_Rec:
			{
				if (!m_targetId.HasValue)
				{
					break;
				}
				if (!m_rotationFinished)
				{
					ShiftModeSync(StateEnum.rot_Rec);
					break;
				}
				MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
				if (Sync.IsServer && laserById != null && (laserById.State == StateEnum.contact_Rec || laserById.State == StateEnum.connected || laserById.State == StateEnum.rot_Rec) && laserById.m_targetId == base.EntityId && laserById.Enabled && laserById.IsFunctional && laserById.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f && IsInRange(laserById) && laserById.Broadcaster.CanBeUsedByPlayer(base.OwnerId) && Broadcaster.CanBeUsedByPlayer(laserById.OwnerId) && !(laserById.Dist2To(m_targetCoords) > 100.0) && LosTests(laserById))
				{
					if (Dist2To(laserById.m_targetCoords) > 100.0 || !laserById.m_rotationFinished)
					{
						SetupLaseTargetCoords();
						laserById.m_targetCoords = HeadPos;
						laserById.m_rotationFinished = false;
					}
					else
					{
						m_canLaseTargetCoords.Value = true;
						ShiftModeSync(StateEnum.connected);
					}
				}
				break;
			}
			case StateEnum.connected:
				if (Sync.IsServer)
				{
					if (!m_rotationFinished)
					{
						ShiftModeSync(StateEnum.rot_Rec);
					}
					if (!m_targetId.HasValue)
					{
						ShiftModeSync(StateEnum.contact_Rec);
					}
					MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
					if (laserById == null || laserById.m_targetId != base.EntityId || laserById.State != StateEnum.connected || !laserById.Enabled || !laserById.IsFunctional || !(laserById.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f) || !laserById.m_rotationFinished || !IsInRange(laserById) || !laserById.Broadcaster.CanBeUsedByPlayer(base.OwnerId) || !Broadcaster.CanBeUsedByPlayer(laserById.OwnerId) || !LosTest(laserById.HeadPos))
					{
						ShiftModeSync(StateEnum.contact_Rec);
						break;
					}
					m_targetCoords = laserById.HeadPos;
					m_canLaseTargetCoords.Value = true;
				}
				break;
			}
		}

		protected void SetupLaseTargetCoords()
		{
			m_canLaseTargetCoords.Value = false;
			if (m_rotationFinished && m_wasVisible && m_targetId.HasValue)
			{
				MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
				if (laserById != null && laserById.m_targetId == base.EntityId && laserById.Enabled && laserById.IsFunctional && laserById.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f && IsInRange(laserById) && laserById.Broadcaster.CanBeUsedByPlayer(base.OwnerId) && Broadcaster.CanBeUsedByPlayer(laserById.OwnerId))
				{
					m_canLaseTargetCoords.Value = true;
				}
			}
		}

		protected void TryLaseTargetCoords()
		{
			if ((bool)m_canLaseTargetCoords && m_targetId.HasValue)
			{
				MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
				if (laserById != null)
				{
					laserById.m_targetCoords = HeadPos;
				}
			}
		}

		protected void TryUpdateTargetCoords()
		{
			if (!m_targetId.HasValue)
			{
				return;
			}
			MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
			if (laserById != null)
			{
				if (laserById.Enabled && laserById.IsFunctional && base.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f && laserById.m_targetId != base.EntityId)
				{
					ShiftModeSync(StateEnum.idle);
				}
				else if (MyAntennaSystem.Static.GetAllRelayedBroadcasters(Receiver, base.OwnerId, mutual: false).Contains(laserById.Broadcaster))
				{
					m_targetCoords = laserById.HeadPos;
					if (m_lastKnownTargetName.CompareTo(laserById.CustomName) != 0)
					{
						m_lastKnownTargetName.Clear().Append((object)laserById.CustomName);
						UpdateMyStateText();
					}
				}
			}
			else
			{
				MyLaserBroadcaster laserBroadcasterById = GetLaserBroadcasterById(m_targetId.Value);
				if (laserBroadcasterById != null)
				{
					m_targetCoords = laserBroadcasterById.BroadcastPosition;
				}
			}
		}

		private double Dist2To(Vector3D? here)
		{
			if (here.HasValue)
			{
				return Vector3D.DistanceSquared(here.Value, HeadPos);
			}
			return 3.4028234663852886E+38;
		}

		public bool IsInRange(MyEntity target)
		{
			MyLaserAntenna myLaserAntenna = target as MyLaserAntenna;
			if (myLaserAntenna != null)
			{
				float num = Math.Min(myLaserAntenna.m_range, m_range);
				if (num >= 1E+08f)
				{
					return true;
				}
				if (Dist2To(myLaserAntenna.HeadPos) > (double)(num * num))
				{
					return false;
				}
			}
			else
			{
				if ((float)m_range >= 1E+08f)
				{
					return true;
				}
				if (Dist2To(target.PositionComp.GetPosition()) > (double)((float)m_range * (float)m_range))
				{
					return false;
				}
			}
			return true;
		}

		public MyLaserAntenna GetOther()
		{
			if (State == StateEnum.connected)
			{
				return GetLaserById(m_targetId.Value);
			}
			return null;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			if (base.Enabled && State == StateEnum.connected)
			{
				ShiftModeSync(StateEnum.rot_Rec);
			}
			Receiver.UpdateBroadcastersInRange();
			base.OnEnabledChanged();
		}

		protected override void OnStopWorking()
		{
			UpdateEmissivity();
			base.OnStopWorking();
		}

		protected override void OnStartWorking()
		{
			UpdateEmissivity();
			base.OnStartWorking();
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		private void IsPoweredChanged()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					UpdateIsWorking();
					if (State == StateEnum.connected && !base.IsWorking)
					{
						ShiftModeSync(StateEnum.rot_Rec);
					}
					if (Receiver != null)
					{
						Receiver.Enabled = base.IsWorking;
					}
					m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					SetDetailedInfoDirty();
					RaisePropertiesChanged();
					UpdateEmissivity();
				}
			}, "MyLaserAntenna::IsPoweredChanged");
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			UpdateEmissivity();
		}

		internal override void OnIntegrityChanged(float buildIntegrity, float integrity, bool setOwnership, long owner, MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction)
		{
			base.OnIntegrityChanged(buildIntegrity, integrity, setOwnership, owner, sharing);
			m_termGpsCoords = null;
			m_termGpsName.Clear();
			ShiftModeSync(StateEnum.idle);
		}

		public void OnClosed()
		{
			MyAntennaSystem.Static.RemoveLaser(Broadcaster.AntennaEntityId);
		}

		private float UpdatePowerInput()
		{
			if (!base.Enabled || !base.IsFunctional)
			{
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
				return 0f;
			}
			float result = 0f;
			switch (State)
			{
			case StateEnum.idle:
				result = (m_rotationFinished ? BlockDefinition.PowerInputIdle : BlockDefinition.PowerInputTurning);
				break;
			case StateEnum.rot_GPS:
			case StateEnum.rot_Rec:
				result = BlockDefinition.PowerInputTurning;
				break;
			case StateEnum.search_GPS:
			case StateEnum.contact_Rec:
			case StateEnum.connected:
			{
				double num = BlockDefinition.PowerInputLasing;
				double num2 = num / 2.0 / 200000.0;
				double num3 = num * 200000.0 - num2 * 200000.0 * 200000.0;
				double num4 = Math.Min(Dist2To(m_targetCoords), (float)m_range * (float)m_range);
				result = ((!(num4 > 40000000000.0)) ? ((float)(num * Math.Sqrt(num4)) / 1000000f) : ((float)(num4 * num2 + num3) / 1000000f));
				break;
			}
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			return result;
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		private void UpdateEmissivity()
		{
			if (!base.InScene || m_base2 == null || m_base2.Render == null)
			{
				return;
			}
			Color emissivePartColor = Color.Red;
			MyEmissiveColorStateResult result;
			if (!base.IsWorking)
			{
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				MyEntity.UpdateNamedEmissiveParts(m_base2.Render.RenderObjectIDs[0], "Emissive0", emissivePartColor, 0f);
				return;
			}
			switch (State)
			{
			case StateEnum.idle:
				emissivePartColor = Color.Green;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				break;
			case StateEnum.rot_GPS:
			case StateEnum.rot_Rec:
				emissivePartColor = Color.Yellow;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				break;
			case StateEnum.connected:
				emissivePartColor = Color.SteelBlue;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Alternative, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				break;
			default:
				emissivePartColor = Color.GreenYellow;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
				{
					emissivePartColor = result.EmissiveColor;
				}
				break;
			}
			MyEntity.UpdateNamedEmissiveParts(m_base2.Render.RenderObjectIDs[0], "Emissive0", emissivePartColor, 1f);
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
			detailedInfo.Append("\n");
			if (!base.Enabled)
			{
				return;
			}
			switch (State)
			{
			case StateEnum.idle:
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.LaserAntennaModeIdle));
				break;
			case StateEnum.rot_GPS:
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.LaserAntennaModeRotGPS));
				break;
			case StateEnum.search_GPS:
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.LaserAntennaModeSearchGPS));
				if (m_OnlyPermanentExists)
				{
					detailedInfo.Append("\n");
					detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.LaserAntennaOnlyPerm));
				}
				break;
			case StateEnum.rot_Rec:
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.LaserAntennaModeRotRec));
				detailedInfo.Append((object)m_lastKnownTargetName);
				break;
			case StateEnum.contact_Rec:
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.LaserAntennaModeContactRec));
				detailedInfo.Append((object)m_lastKnownTargetName);
				break;
			case StateEnum.connected:
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.LaserAntennaModeConnectedTo));
				detailedInfo.Append((object)m_lastKnownTargetName);
				break;
			}
			if (m_outsideLimits)
			{
				detailedInfo.Append("\n");
				detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.LaserAntennaOutsideLimits));
			}
		}

		protected void SetIdle()
		{
			ChangeModeSync(StateEnum.idle);
			receiversList.UpdateVisual();
		}

		protected void ConnectToId()
		{
			ConnectToRec(m_selectedEntityId.Value);
		}

		protected void ConnectToGps(Vector3D? coords = null)
		{
			ChangeModeSync(StateEnum.rot_GPS, coords);
		}

		internal void ChangeMode(StateEnum Mode, Vector3D? coords = null)
		{
			switch (Mode)
			{
			case StateEnum.idle:
				m_state = StateEnum.idle;
				IdleOther();
				break;
			case StateEnum.rot_GPS:
				m_state = StateEnum.idle;
				IdleOther();
				break;
			}
			DoChangeMode(Mode, coords);
			Receiver.UpdateBroadcastersInRange();
		}

		internal void DoChangeMode(StateEnum Mode, Vector3D? coords = null)
		{
			State = Mode;
			Broadcaster.RaiseChangeSuccessfullyContacting();
			m_OnlyPermanentExists = false;
			Receiver.UpdateBroadcastersInRange();
			if (MySession.Static.LocalCharacter != null)
			{
				MySession.Static.LocalCharacter.RadioReceiver.UpdateBroadcastersInRange();
			}
			receiversList.UpdateVisual();
			if (m_targetId.HasValue)
			{
				MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
				if (laserById != null)
				{
					laserById.UpdateVisual();
					laserById.ResourceSink.Update();
				}
			}
			base.ResourceSink.Update();
			UpdateVisual();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			UpdateEmissivity();
			UpdateMyStateText();
			switch (Mode)
			{
			case StateEnum.idle:
				m_needRotation = 0f;
				m_needElevation = 0f;
				m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				m_lastKnownTargetName.Clear();
				if (Sync.IsServer)
				{
					m_permanentConnection.Value = false;
				}
				break;
			case StateEnum.rot_GPS:
				if (coords.HasValue)
				{
					m_termGpsCoords = coords;
				}
				m_targetCoords = m_termGpsCoords.Value;
				m_lastKnownTargetName.Clear().Append((object)m_termGpsName).Append(" ")
					.Append(m_termGpsCoords);
				if (Sync.IsServer)
				{
					m_permanentConnection.Value = false;
				}
				break;
			}
		}

		protected bool IsInContact(MyLaserAntenna la)
		{
			if (la == null)
			{
				return false;
			}
			return Receiver.BroadcastersInRange.Contains(la.Broadcaster);
		}

		protected bool IdleOther()
		{
			if (m_targetId.HasValue)
			{
				MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
				if (laserById == null)
				{
					return false;
				}
				if (!laserById.Enabled || !laserById.IsFunctional || !(laserById.ResourceSink.SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId) > 0.99f))
				{
					return false;
				}
				if (laserById.State == StateEnum.idle)
				{
					return true;
				}
				if (laserById.m_targetId == base.EntityId && IsInContact(laserById))
				{
					laserById.ChangeModeSync(StateEnum.idle);
					return true;
				}
			}
			return true;
		}

		internal bool ConnectTo(long DestId)
		{
			MyLaserAntenna laserById = GetLaserById(DestId);
			if (laserById == null)
			{
				return false;
			}
			if (!laserById.Broadcaster.CanBeUsedByPlayer(base.OwnerId) || !Broadcaster.CanBeUsedByPlayer(laserById.OwnerId))
			{
				return false;
			}
			IdleOther();
			DoConnectTo(DestId);
			if (laserById != null && laserById.m_targetId != base.EntityId)
			{
				laserById.ConnectToRec(base.EntityId);
			}
			return true;
		}

		internal void DoConnectTo(long DestId, Vector3D? targetCoords = null, string name = null)
		{
			State = StateEnum.rot_Rec;
			if (Sync.IsServer)
			{
				m_permanentConnection.Value = false;
			}
			m_targetId = DestId;
			Broadcaster.RaiseChangeSuccessfullyContacting();
			m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			MyLaserAntenna laserById = GetLaserById(DestId);
			if (laserById != null)
			{
				m_targetCoords = laserById.HeadPos;
				m_lastKnownTargetName.Clear().Append((object)laserById.CustomName);
			}
			else if (targetCoords.HasValue && name != null)
			{
				m_targetCoords = targetCoords.Value;
				m_lastKnownTargetName.Clear().Append(name);
			}
			else
			{
				m_targetCoords = Vector3D.Zero;
				m_lastKnownTargetName.Clear().Append("???");
			}
			base.ResourceSink.Update();
			Receiver.UpdateBroadcastersInRange();
			UpdateVisuals();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			UpdateEmissivity();
			UpdateMyStateText();
		}

		internal bool DoSetIsPerm(bool isPerm)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (m_permanentConnection.Value != isPerm)
			{
				if (State != StateEnum.connected)
				{
					return false;
				}
				if (!m_targetId.HasValue)
				{
					return false;
				}
				MyLaserAntenna laserById = GetLaserById(m_targetId.Value);
				if (laserById == null)
				{
					return false;
				}
				laserById.m_permanentConnection.Value = isPerm;
				m_permanentConnection.Value = isPerm;
				return true;
			}
			return false;
		}

		protected static MyLaserAntenna GetLaserById(long id)
		{
			return GetLaserBroadcasterById(id)?.RealAntenna;
		}

		protected static MyLaserBroadcaster GetLaserBroadcasterById(long id)
		{
			MyLaserBroadcaster value = null;
			MyAntennaSystem.Static.LaserAntennas.TryGetValue(id, out value);
			return value;
		}

		protected bool LosTests(MyLaserAntenna la)
		{
			m_wasVisible = true;
			if (!LosTest(la.HeadPos))
			{
				la.m_wasVisible = false;
			}
			if (m_wasVisible)
			{
				m_wasVisible = la.LosTest(HeadPos);
			}
			return m_wasVisible;
		}

		protected bool LosTest(Vector3D target)
		{
			if (!m_needLineOfSight)
			{
				return true;
			}
			target = (HeadPos + target) * 0.5;
			LineD ray = new LineD(HeadPos, target);
			if (ray.Length > 25.0)
			{
				m_voxelHits.Clear();
				MyGamePruningStructure.GetVoxelMapsOverlappingRay(ref ray, m_voxelHits);
				foreach (MyLineSegmentOverlapResult<MyVoxelBase> voxelHit in m_voxelHits)
				{
					MyPlanet myPlanet = voxelHit.Element as MyPlanet;
					if (myPlanet != null)
					{
						BoundingSphereD boundingSphereD = new BoundingSphereD(myPlanet.PositionComp.GetPosition(), myPlanet.MaximumRadius);
						RayD ray2 = new RayD(ray.From, ray.Direction);
						double? num = boundingSphereD.Intersects(ray2);
						if (num.HasValue && !(ray.Length < num.Value) && myPlanet.RootVoxel.Storage.Intersect(ref ray))
						{
							m_wasVisible = false;
							return false;
						}
					}
				}
				LineD ray3 = (ray.Length <= (double)m_Max_LosDist) ? new LineD(ray.From + ray.Direction * 25.0, ray.To) : new LineD(ray.From + ray.Direction * 25.0, ray.From + ray.Direction * m_Max_LosDist);
				m_entityHits.Clear();
				MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray3, m_entityHits);
				foreach (MyLineSegmentOverlapResult<MyEntity> entityHit in m_entityHits)
				{
					MyVoxelBase myVoxelBase = entityHit.Element as MyVoxelBase;
					if (myVoxelBase != null)
					{
						if (!(myVoxelBase is MyPlanet) && myVoxelBase.RootVoxel.Storage.Intersect(ref ray3))
						{
							m_wasVisible = false;
							return false;
						}
					}
					else
					{
						MyCubeGrid myCubeGrid = entityHit.Element as MyCubeGrid;
						if (myCubeGrid == null)
						{
							m_wasVisible = false;
							return false;
						}
						if (myCubeGrid.Physics != null)
						{
							Vector3I? vector3I = myCubeGrid.RayCastBlocks(ray.To, ray.From);
							if (vector3I.HasValue && myCubeGrid.GetCubeBlock(vector3I.Value) != SlimBlock)
							{
								m_wasVisible = false;
								return false;
							}
						}
					}
				}
			}
			MyPhysics.CastRay(ray.From, ray.From + ray.Direction * Math.Min(25.0, ray.Length), m_hits, 9);
			foreach (MyPhysics.HitInfo hit in m_hits)
			{
				VRage.ModAPI.IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
				if (hitEntity != base.CubeGrid)
				{
					m_wasVisible = false;
					return false;
				}
				MyCubeGrid myCubeGrid2 = hitEntity as MyCubeGrid;
				if (myCubeGrid2.Physics != null)
				{
					Vector3I? vector3I2 = myCubeGrid2.RayCastBlocks(ray.To, ray.From);
					if (vector3I2.HasValue && myCubeGrid2.GetCubeBlock(vector3I2.Value) != SlimBlock)
					{
						m_wasVisible = false;
						return false;
					}
				}
			}
			return true;
		}

		private Vector3 LookAt(Vector3D target)
		{
			return MyMath.QuaternionToEuler(Quaternion.CreateFromRotationMatrix(MatrixD.Normalize(MatrixD.Invert(MatrixD.CreateLookAt(GetWorldMatrix().Translation, target, GetWorldMatrix().Up))) * MatrixD.Invert(MatrixD.Normalize(InitializationMatrixWorld))));
		}

		protected void ResetRotation()
		{
			m_rotation = 0f;
			m_elevation = 0f;
			ClampRotationAndElevation();
			m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				m_base1 = base.Subparts["LaserComTurret"];
				m_base2 = m_base1.Subparts["LaserCom"];
			}
			else
			{
				m_base1 = null;
				m_base2 = null;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			UpdateEmissivity();
		}

		protected void RotateModels()
		{
			ClampRotationAndElevation();
			if (m_base1 != null && m_base2 != null)
			{
				Matrix.CreateRotationY(m_rotation, out Matrix result);
				result.Translation = m_base1.PositionComp.LocalMatrix.Translation;
				Matrix matrix = base.PositionComp.LocalMatrix;
				Matrix.Multiply(ref result, ref matrix, out Matrix result2);
				m_base1.PositionComp.SetLocalMatrix(ref result, m_base1.Physics, updateWorld: false, ref result2);
				Matrix.CreateRotationX(m_elevation, out Matrix result3);
				result3.Translation = m_base2.PositionComp.LocalMatrix.Translation;
				Matrix.Multiply(ref result3, ref result2, out Matrix result4);
				Matrix localMatrix = result4;
				localMatrix.Translation = m_base2.PositionComp.LocalMatrix.Translation;
				m_base2.PositionComp.SetLocalMatrix(ref localMatrix, m_base2.Physics, updateWorld: true, ref result4);
			}
		}

		protected void GetRotationAndElevation(Vector3D target, ref float needRotation, ref float needElevation)
		{
			Vector3 zero = Vector3.Zero;
			zero = LookAt(target);
			needRotation = zero.Y;
			needElevation = zero.X;
		}

		public bool RotationAndElevation(float needRotation, float needElevation)
		{
			float value = BlockDefinition.RotationRate * (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_rotationInterval_ms);
			float num = needRotation - m_rotation;
			if (num > 3.141593f)
			{
				num -= MathF.PI * 2f;
			}
			else if (num < -3.141593f)
			{
				num += MathF.PI * 2f;
			}
			float num2 = Math.Abs(num);
			if (num2 > 0.001f)
			{
				float num3 = MathHelper.Clamp(value, float.Epsilon, num2);
				m_rotation += ((num > 0f) ? num3 : (0f - num3));
			}
			else
			{
				m_rotation = needRotation;
			}
			if (m_rotation > 3.141593f)
			{
				m_rotation -= MathF.PI * 2f;
			}
			else if (m_rotation < -3.141593f)
			{
				m_rotation += MathF.PI * 2f;
			}
			value = BlockDefinition.RotationRate * (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_elevationInterval_ms);
			float num4 = needElevation - m_elevation;
			float num5 = Math.Abs(num4);
			if (num5 > 0.001f)
			{
				float num6 = MathHelper.Clamp(value, float.Epsilon, num5);
				m_elevation += ((num4 > 0f) ? num6 : (0f - num6));
			}
			else
			{
				m_elevation = needElevation;
			}
			m_elevationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_rotationInterval_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			RotateModels();
			float num7 = Math.Abs(Math.Abs(needRotation) - Math.Abs(m_rotation));
			float num8 = Math.Abs(Math.Abs(needElevation) - Math.Abs(m_elevation));
			m_rotationFinished = (num7 <= float.Epsilon && num8 <= float.Epsilon);
			return m_rotationFinished;
		}

		private void PasteCoordinates(string coords)
		{
			if (!Sync.MultiplayerActive)
			{
				DoPasteCoords(coords);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyLaserAntenna x) => x.PasteCoordinatesSuccess, coords);
			}
		}

		[Event(null, 1704)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void PasteCoordinatesSuccess(string coords)
		{
			DoPasteCoords(coords);
		}

		private void ChangePerm(bool isPerm)
		{
			if (!Sync.MultiplayerActive)
			{
				DoSetIsPerm(isPerm);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyLaserAntenna x) => x.ChangePermRequest, isPerm);
			}
		}

		[Event(null, 1722)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void ChangePermRequest(bool isPerm)
		{
			DoSetIsPerm(isPerm);
		}

		private void ChangeModeSync(StateEnum Mode, Vector3D? coords = null)
		{
			ChangeMode(Mode, uploadFromClient: true, coords);
		}

		private void ShiftModeSync(StateEnum Mode)
		{
			ChangeMode(Mode, uploadFromClient: false);
		}

		private void ChangeMode(StateEnum mode, bool uploadFromClient, Vector3D? coords = null)
		{
			if (!Sync.MultiplayerActive)
			{
				ChangeMode(mode);
			}
			else if (uploadFromClient || Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyLaserAntenna x) => x.OnChangeModeRequest, mode, coords);
			}
		}

		[Event(null, 1748)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeModeRequest(StateEnum mode, Vector3D? coords)
		{
			ChangeMode(mode, coords);
		}

		public void ConnectToRec(long TgtReceiver)
		{
			if (!Sync.MultiplayerActive)
			{
				ConnectTo(TgtReceiver);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyLaserAntenna x) => x.OnConnectToRecRequest, TgtReceiver);
			}
		}

		[Event(null, 1766)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnConnectToRecRequest(long targetEntityId)
		{
			if (ConnectTo(targetEntityId))
			{
				MyMultiplayer.RaiseEvent(this, (MyLaserAntenna x) => x.OnConnectToRecSuccess, targetEntityId, m_targetCoords, m_lastKnownTargetName.ToString());
			}
		}

		[Event(null, 1773)]
		[Reliable]
		[Broadcast]
		private void OnConnectToRecSuccess(long targetEntityId, Vector3D targetCoords, string name)
		{
			DoConnectTo(targetEntityId, targetCoords, name);
		}

		void Sandbox.ModAPI.Ingame.IMyLaserAntenna.SetTargetCoords(string coords)
		{
			if (coords != null)
			{
				PasteCoordinates(coords);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyLaserAntenna.Connect()
		{
			if (CanConnectToGPS())
			{
				ConnectToGps();
			}
		}

		bool Sandbox.ModAPI.IMyLaserAntenna.IsInRange(Sandbox.ModAPI.IMyLaserAntenna target)
		{
			return IsInRange((MyLaserAntenna)target);
		}
	}
}
