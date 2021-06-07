using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.History;
using Sandbox.Game.Replication.StateGroups;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("VRage", "Network Watch")]
	internal class MyGuiScreenDebugNetworkWatch : MyGuiScreenDebugBase
	{
		private MyEntity m_currentEntity;

		private MyGuiControlSlider m_up;

		private MyGuiControlSlider m_right;

		private MyGuiControlSlider m_forward;

		private MyGuiControlButton m_kickButton;

		private MyGuiControlLabel m_debugEntityLabel;

		private MyGuiControlLabel m_watchLabel;

		private bool m_debugEntityLocked;

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

		private bool m_debugEntityMyself;

		public MyGuiScreenDebugNetworkWatch()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 1f;
			AddCaption("Network Watch", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_debugEntityLabel = AddLabel("", Color.Yellow, 1f);
			AddCheckBox("Sync VDB camera", null, MemberHelper.GetMember(() => MyPhysics.SyncVDBCamera));
			AddCheckBox("Debug Myself", checkedState: false, delegate(MyGuiControlCheckbox x)
			{
				m_debugEntityMyself = x.IsChecked;
			});
			AddCheckBox("Lock Debug Entity", checkedState: false, delegate(MyGuiControlCheckbox x)
			{
				m_debugEntityLocked = x.IsChecked;
			});
			AddCheckBox("Skip corrections for Debug Entity", MyPredictedSnapshotSync.SKIP_CORRECTIONS_FOR_DEBUG_ENTITY, delegate(MyGuiControlCheckbox x)
			{
				MyPredictedSnapshotSync.SKIP_CORRECTIONS_FOR_DEBUG_ENTITY = x.IsChecked;
			});
			AddLabel("Cendos Desync Simulator (tm)", Color.White, 1f);
			m_up = AddSlider("Up", 0f, -50f, 50f);
			m_right = AddSlider("Right", 0f, -50f, 50f);
			m_forward = AddSlider("Forward", 0f, -50f, 50f);
			m_kickButton = AddButton("Kick", delegate
			{
				MatrixD worldMatrix = m_currentEntity.WorldMatrix;
				MySnapshot snapshot = new MySnapshot(m_currentEntity);
				snapshot.Position += Vector3.TransformNormal(new Vector3(m_up.Value, m_right.Value, m_forward.Value), worldMatrix);
				MySnapshotCache.Add(m_currentEntity, ref snapshot, m_kickSetup, reset: true);
				MySnapshotCache.Apply();
			});
			m_kickButton = AddButton("Log hierarchy", delegate
			{
				(m_currentEntity as MyCubeGrid)?.LogHierarchy();
			});
			m_watchLabel = AddLabel("", Color.Yellow, 1f);
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (MySession.Static != null && m_kickButton != null && m_debugEntityLabel != null)
			{
				MyEntity myEntity = null;
				if (!m_debugEntityLocked)
				{
					LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * 500.0);
					MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, MySession.Static.LocalCharacter, null, ignoreChildren: true, ignoreFloatingObjects: false, ignoreHandWeapons: false, IntersectionFlags.ALL_TRIANGLES, 0f, ignoreObjectsWithoutPhysics: false);
					if (intersectionWithLine.HasValue)
					{
						myEntity = (intersectionWithLine.Value.Entity as MyEntity);
					}
				}
				if (m_debugEntityMyself)
				{
					myEntity = MySession.Static.TopMostControlledEntity;
				}
				if (myEntity != m_currentEntity)
				{
					m_currentEntity = myEntity;
					m_kickButton.Enabled = (m_currentEntity != null);
					m_debugEntityLabel.Text = ((m_currentEntity != null) ? m_currentEntity.DisplayName : "");
					MySnapshotCache.DEBUG_ENTITY_ID = ((m_currentEntity != null) ? m_currentEntity.EntityId : 0);
					MyFakes.VDB_ENTITY = m_currentEntity;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyMultiplayerBase.OnSetDebugEntity, (m_currentEntity == null) ? 0 : m_currentEntity.EntityId);
				}
			}
			if (m_currentEntity != null)
			{
				MyCharacter myCharacter = m_currentEntity as MyCharacter;
				MyCubeGrid myCubeGrid = m_currentEntity as MyCubeGrid;
				MyExternalReplicable myExternalReplicable = MyExternalReplicable.FindByObject(m_currentEntity);
				_ = myExternalReplicable?.PhysicsSync;
				MyCharacterPhysicsStateGroup myCharacterPhysicsStateGroup = (myExternalReplicable != null) ? (myExternalReplicable.PhysicsSync as MyCharacterPhysicsStateGroup) : null;
				MyEntity entity = null;
				if (myCharacter != null)
				{
					MyEntities.TryGetEntityById(myCharacter.ClosestParentId, out entity);
				}
				else if (myCubeGrid != null)
				{
					MyEntities.TryGetEntityById(myCubeGrid.ClosestParentId, out entity);
				}
				m_watchLabel.Text = string.Format("Predicted: {4}\nCorrection: {0}\nSupport: {1}\nParentId: {2}\nPredictedContactsCounter: {3}\nLinearVelocity: {5}\nLinearVelocityLocal: {6}\n", myCharacterPhysicsStateGroup?.AverageCorrection ?? 0.0, myCharacter != null && myCharacter.Physics.CharacterProxy != null && myCharacter.Physics.CharacterProxy.Supported, (entity != null) ? entity.DebugName : "-", myCubeGrid?.Physics.PredictedContactsCounter ?? 0, myCubeGrid?.IsClientPredicted ?? myCharacter?.IsClientPredicted ?? false, (m_currentEntity.Physics != null) ? m_currentEntity.Physics.LinearVelocity.Length() : 0f, (m_currentEntity.Physics != null) ? m_currentEntity.Physics.LinearVelocityLocal.Length() : 0f);
			}
			return result;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugNetworkWatch";
		}
	}
}
