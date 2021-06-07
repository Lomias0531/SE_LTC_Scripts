using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Replication.History;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("VRage", "Network")]
	[StaticEventOwner]
	internal class MyGuiScreenDebugNetwork : MyGuiScreenDebugBase
	{
		protected sealed class OnSnapshotsMechanicalPivotsChange_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool state, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSnapshotsMechanicalPivotsChange(state);
			}
		}

		protected sealed class OnWorldSnapshotsChange_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool state, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnWorldSnapshotsChange(state);
			}
		}

		private MyGuiControlLabel m_entityLabel;

		private MyEntity m_currentEntity;

		private MyGuiControlSlider m_up;

		private MyGuiControlSlider m_right;

		private MyGuiControlSlider m_forward;

		private MyGuiControlButton m_kickButton;

		private MyGuiControlLabel m_profileLabel;

		private bool m_profileEntityLocked;

		private const float FORCED_PRIORITY = 1f;

		private readonly MyPredictedSnapshotSyncSetup m_kickSetup = new MyPredictedSnapshotSyncSetup
		{
			AllowForceStop = false,
			ApplyPhysicsAngular = false,
			ApplyPhysicsLinear = false,
			ApplyRotation = false,
			ApplyPosition = true,
			ExtrapolationSmoothing = true
		};

		public MyGuiScreenDebugNetwork()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 1f;
			AddCaption("Network", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			if (MyMultiplayer.Static != null)
			{
				AddSlider("Priority multiplier", 1f, 0f, 16f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnSetPriorityMultiplier, slider.Value);
				});
				m_currentPosition.Y += 0.01f;
				AddCheckBox("Smooth ping", MyMultiplayer.Static.ReplicationLayer.UseSmoothPing, delegate(MyGuiControlCheckbox x)
				{
					MyMultiplayer.Static.ReplicationLayer.UseSmoothPing = x.IsChecked;
				});
				AddSlider("Ping smooth factor", MyMultiplayer.Static.ReplicationLayer.PingSmoothFactor, 0f, 3f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.Static.ReplicationLayer.PingSmoothFactor = slider.Value;
				});
				AddSlider("Timestamp correction minimum", MyMultiplayer.Static.ReplicationLayer.TimestampCorrectionMinimum, 0f, 100f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.Static.ReplicationLayer.TimestampCorrectionMinimum = (int)slider.Value;
				});
				AddCheckBox("Smooth timestamp correction", MyMultiplayer.Static.ReplicationLayer.UseSmoothCorrection, delegate(MyGuiControlCheckbox x)
				{
					MyMultiplayer.Static.ReplicationLayer.UseSmoothCorrection = x.IsChecked;
				});
				AddSlider("Smooth timestamp correction amplitude", MyMultiplayer.Static.ReplicationLayer.SmoothCorrectionAmplitude, 0f, 5f, delegate(MyGuiControlSlider slider)
				{
					MyMultiplayer.Static.ReplicationLayer.SmoothCorrectionAmplitude = (int)slider.Value;
				});
			}
			AddCheckBox("Physics World Locking", MyFakes.WORLD_LOCKING_IN_CLIENTUPDATE, delegate(MyGuiControlCheckbox x)
			{
				MyFakes.WORLD_LOCKING_IN_CLIENTUPDATE = x.IsChecked;
			});
			AddCheckBox("Pause physics", null, MemberHelper.GetMember(() => MyFakes.PAUSE_PHYSICS));
			AddCheckBox("Client physics constraints", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_CLIENT_CONSTRAINTS));
			AddCheckBox("New timing", MyReplicationClient.SynchronizationTimingType == MyReplicationClient.TimingType.LastServerTime, delegate(MyGuiControlCheckbox x)
			{
				MyReplicationClient.SynchronizationTimingType = ((!x.IsChecked) ? MyReplicationClient.TimingType.ServerTimestep : MyReplicationClient.TimingType.LastServerTime);
			});
			AddSlider("Animation time shift [ms]", (float)MyAnimatedSnapshotSync.TimeShift.Milliseconds, 0f, 1000f, delegate(MyGuiControlSlider slider)
			{
				MyAnimatedSnapshotSync.TimeShift = MyTimeSpan.FromMilliseconds(slider.Value);
			});
			AddCheckBox("Prediction in jetpack", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER_IN_JETPACK));
			AddCheckBox("Prediction for grids", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID));
			AddCheckBox("Skip prediction", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_SKIP_PREDICTION));
			AddCheckBox("Skip prediction subgrids", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_SKIP_PREDICTION_SUBGRIDS));
			AddCheckBox("Extrapolation smoothing", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_EXTRAPOLATION_SMOOTHING));
			AddCheckBox("Skip animation", null, MemberHelper.GetMember(() => MyFakes.MULTIPLAYER_SKIP_ANIMATION));
			AddCheckBox("SnapshotCache Hierarchy Propagation", null, MemberHelper.GetMember(() => MyFakes.SNAPSHOTCACHE_HIERARCHY));
			AddCheckBox("World snapshots", MyFakes.WORLD_SNAPSHOTS, delegate(MyGuiControlCheckbox x)
			{
				MyFakes.WORLD_SNAPSHOTS = x.IsChecked;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner y) => OnWorldSnapshotsChange, x.IsChecked);
			});
			AddCheckBox("Mechanical Pivots in Snapshots", MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS, delegate(MyGuiControlCheckbox x)
			{
				MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS = x.IsChecked;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner y) => OnSnapshotsMechanicalPivotsChange, x.IsChecked);
			});
		}

		[Event(null, 106)]
		[Reliable]
		[Server]
		private static void OnSnapshotsMechanicalPivotsChange(bool state)
		{
			MyFakes.SNAPSHOTS_MECHANICAL_PIVOTS = state;
		}

		[Event(null, 112)]
		[Reliable]
		[Server]
		private static void OnWorldSnapshotsChange(bool state)
		{
			MyFakes.WORLD_SNAPSHOTS = state;
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (m_kickButton == null || m_entityLabel == null)
			{
				return result;
			}
			if (MySession.Static != null)
			{
				MyEntity myEntity = null;
				if (MySession.Static != null)
				{
					LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 500.0);
					MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, MySession.Static.LocalCharacter, null, ignoreChildren: false, ignoreFloatingObjects: true, ignoreHandWeapons: true, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: false);
					if (intersectionWithLine.HasValue)
					{
						myEntity = (intersectionWithLine.Value.Entity as MyEntity);
					}
				}
				if (myEntity != m_currentEntity && !m_profileEntityLocked)
				{
					m_currentEntity = myEntity;
					m_kickButton.Enabled = (m_currentEntity != null);
					m_entityLabel.Text = ((m_currentEntity != null) ? m_currentEntity.DisplayName : "");
					m_profileLabel.Text = m_entityLabel.Text;
					MySnapshotCache.DEBUG_ENTITY_ID = ((m_currentEntity != null) ? m_currentEntity.EntityId : 0);
					MyFakes.VDB_ENTITY = m_currentEntity;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnSetDebugEntity, (m_currentEntity == null) ? 0 : m_currentEntity.EntityId);
				}
			}
			return result;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugNetwork";
		}
	}
}
