using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 900, typeof(MyObjectBuilder_CutsceneSessionComponent), null)]
	public class MySessionComponentCutscenes : MySessionComponentBase
	{
		private MyObjectBuilder_CutsceneSessionComponent m_objectBuilder;

		private Dictionary<string, Cutscene> m_cutsceneLibrary = new Dictionary<string, Cutscene>();

		private Cutscene m_currentCutscene;

		private CutsceneSequenceNode m_currentNode;

		private float m_currentTime;

		private float m_currentFOV = 70f;

		private int m_currentNodeIndex;

		private bool m_nodeActivated;

		private float MINIMUM_FOV = 10f;

		private float MAXIMUM_FOV = 300f;

		private float m_eventDelay = float.MaxValue;

		private bool m_releaseCamera;

		private bool m_overlayEnabled;

		private bool m_registerEvents = true;

		private string m_cameraOverlay = "";

		private string m_cameraOverlayOriginal = "";

		private MatrixD m_nodeStartMatrix;

		private float m_nodeStartFOV = 70f;

		private MatrixD m_nodeEndMatrix;

		private MatrixD m_currentCameraMatrix;

		private MyEntity m_lookTarget;

		private MyEntity m_rotateTarget;

		private MyEntity m_moveTarget;

		private MyEntity m_attachedPositionTo;

		private Vector3D m_attachedPositionOffset = Vector3D.Zero;

		private MyEntity m_attachedRotationTo;

		private MatrixD m_attachedRotationOffset;

		private Vector3D m_lastUpVector = Vector3D.Up;

		private List<MatrixD> m_waypoints = new List<MatrixD>();

		private IMyCameraController m_originalCameraController;

		private MyCutsceneCamera m_cameraEntity = new MyCutsceneCamera();

		public MatrixD CameraMatrix => m_currentCameraMatrix;

		public bool IsCutsceneRunning => m_currentCutscene != null;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_objectBuilder = (sessionComponent as MyObjectBuilder_CutsceneSessionComponent);
			if (m_objectBuilder == null || m_objectBuilder.Cutscenes == null || m_objectBuilder.Cutscenes.Length == 0)
			{
				return;
			}
			Cutscene[] cutscenes = m_objectBuilder.Cutscenes;
			foreach (Cutscene cutscene in cutscenes)
			{
				if (cutscene.Name != null && cutscene.Name.Length > 0 && !m_cutsceneLibrary.ContainsKey(cutscene.Name))
				{
					m_cutsceneLibrary.Add(cutscene.Name, cutscene);
				}
			}
		}

		public override void BeforeStart()
		{
			if (m_objectBuilder != null)
			{
				foreach (string voxelPrecachingWaypointName in m_objectBuilder.VoxelPrecachingWaypointNames)
				{
					if (MyEntities.TryGetEntityByName(voxelPrecachingWaypointName, out MyEntity entity))
					{
						MyRenderProxy.PointsForVoxelPrecache.Add(entity.PositionComp.GetPosition());
					}
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if (m_releaseCamera && MySession.Static.ControlledEntity != null)
			{
				m_releaseCamera = false;
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.ControlledEntity.Entity);
				MyHud.CutsceneHud = false;
				MyGuiScreenGamePlay.DisableInput = false;
			}
			if (IsCutsceneRunning)
			{
				if (MySession.Static.CameraController != m_cameraEntity)
				{
					m_originalCameraController = MySession.Static.CameraController;
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, m_cameraEntity);
				}
				if (m_currentCutscene.SequenceNodes != null && m_currentCutscene.SequenceNodes.Count > m_currentNodeIndex)
				{
					m_currentNode = m_currentCutscene.SequenceNodes[m_currentNodeIndex];
					CutsceneUpdate();
				}
				else if (m_currentCutscene.NextCutscene != null && m_currentCutscene.NextCutscene.Length > 0)
				{
					PlayCutscene(m_currentCutscene.NextCutscene, m_registerEvents);
				}
				else
				{
					CutsceneEnd();
				}
				m_cameraEntity.WorldMatrix = m_currentCameraMatrix;
			}
		}

		public void CutsceneUpdate()
		{
			if (!m_nodeActivated)
			{
				m_nodeActivated = true;
				m_nodeStartMatrix = m_currentCameraMatrix;
				m_nodeEndMatrix = m_currentCameraMatrix;
				m_nodeStartFOV = m_currentFOV;
				m_moveTarget = null;
				m_rotateTarget = null;
				m_waypoints.Clear();
				m_eventDelay = float.MaxValue;
				if (m_registerEvents && m_currentNode.Event != null && m_currentNode.Event.Length > 0 && MyVisualScriptLogicProvider.CutsceneNodeEvent != null)
				{
					if (m_currentNode.EventDelay <= 0f)
					{
						MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentNode.Event);
					}
					else
					{
						m_eventDelay = m_currentNode.EventDelay;
					}
				}
				if (m_currentNode.LookAt != null && m_currentNode.LookAt.Length > 0)
				{
					MyEntity entityByName = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.LookAt);
					if (entityByName != null)
					{
						m_nodeStartMatrix = MatrixD.CreateLookAtInverse(m_currentCameraMatrix.Translation, entityByName.PositionComp.GetPosition(), m_currentCameraMatrix.Up);
						m_nodeEndMatrix = m_nodeStartMatrix;
					}
				}
				if (m_currentNode.SetRorationLike != null && m_currentNode.SetRorationLike.Length > 0)
				{
					MyEntity entityByName2 = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.SetRorationLike);
					if (entityByName2 != null)
					{
						m_nodeStartMatrix = entityByName2.WorldMatrix;
						m_nodeEndMatrix = m_nodeStartMatrix;
					}
				}
				if (m_currentNode.RotateLike != null && m_currentNode.RotateLike.Length > 0)
				{
					MyEntity entityByName3 = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.RotateLike);
					if (entityByName3 != null)
					{
						m_nodeEndMatrix = entityByName3.WorldMatrix;
					}
				}
				if (m_currentNode.RotateTowards != null && m_currentNode.RotateTowards.Length > 0)
				{
					m_rotateTarget = ((m_currentNode.RotateTowards.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.RotateTowards) : null);
				}
				if (m_currentNode.LockRotationTo != null)
				{
					m_lookTarget = ((m_currentNode.LockRotationTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.LockRotationTo) : null);
				}
				m_nodeStartMatrix.Translation = m_currentCameraMatrix.Translation;
				m_nodeEndMatrix.Translation = m_currentCameraMatrix.Translation;
				if (m_currentNode.SetPositionTo != null && m_currentNode.SetPositionTo.Length > 0)
				{
					MyEntity entityByName4 = MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.SetPositionTo);
					if (entityByName4 != null)
					{
						m_nodeStartMatrix.Translation = entityByName4.WorldMatrix.Translation;
						m_nodeEndMatrix.Translation = entityByName4.WorldMatrix.Translation;
					}
				}
				if (m_currentNode.AttachTo != null)
				{
					if (m_currentNode.AttachTo != null)
					{
						m_attachedRotationOffset = MatrixD.Identity;
						m_attachedPositionOffset = Vector3D.Zero;
						m_attachedPositionTo = ((m_currentNode.AttachTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.AttachTo) : null);
						if (m_attachedPositionTo != null)
						{
							m_attachedPositionOffset = Vector3D.Transform(m_currentCameraMatrix.Translation, m_attachedPositionTo.PositionComp.WorldMatrixInvScaled);
							m_attachedRotationTo = m_attachedPositionTo;
							m_attachedRotationOffset = m_currentCameraMatrix * m_attachedRotationTo.PositionComp.WorldMatrixInvScaled;
							m_attachedRotationOffset.Translation = Vector3D.Zero;
						}
					}
				}
				else
				{
					if (m_currentNode.AttachPositionTo != null)
					{
						m_attachedPositionTo = ((m_currentNode.AttachPositionTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.AttachPositionTo) : null);
						m_attachedPositionOffset = ((m_attachedPositionTo != null) ? Vector3D.Transform(m_currentCameraMatrix.Translation, m_attachedPositionTo.PositionComp.WorldMatrixInvScaled) : Vector3D.Zero);
					}
					if (m_currentNode.AttachRotationTo != null)
					{
						m_attachedRotationOffset = MatrixD.Identity;
						m_attachedRotationTo = ((m_currentNode.AttachRotationTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.AttachRotationTo) : null);
						if (m_attachedRotationTo != null)
						{
							m_attachedRotationOffset = m_currentCameraMatrix * m_attachedRotationTo.PositionComp.WorldMatrixInvScaled;
							m_attachedRotationOffset.Translation = Vector3D.Zero;
						}
					}
				}
				if (m_currentNode.MoveTo != null && m_currentNode.MoveTo.Length > 0)
				{
					m_moveTarget = ((m_currentNode.MoveTo.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentNode.MoveTo) : null);
				}
				if (m_currentNode.Waypoints != null && m_currentNode.Waypoints.Count > 0)
				{
					bool flag = true;
					foreach (CutsceneSequenceNodeWaypoint waypoint in m_currentNode.Waypoints)
					{
						if (waypoint.Name.Length > 0)
						{
							MyEntity entityByName5 = MyVisualScriptLogicProvider.GetEntityByName(waypoint.Name);
							if (entityByName5 != null)
							{
								m_waypoints.Add(entityByName5.WorldMatrix);
								if (flag)
								{
									m_lastUpVector = entityByName5.WorldMatrix.Up;
									flag = false;
								}
							}
						}
					}
					if (m_waypoints.Count > 0)
					{
						if (m_waypoints.Count < 3)
						{
							m_nodeEndMatrix.Translation = m_waypoints[m_waypoints.Count - 1].Translation;
							m_waypoints.Clear();
						}
						else if (m_waypoints.Count == 2)
						{
							m_nodeStartMatrix = m_waypoints[0];
							m_nodeEndMatrix = m_waypoints[1];
						}
					}
				}
				m_currentCameraMatrix = m_nodeStartMatrix;
			}
			m_currentTime += 0.0166666675f;
			float num = (m_currentNode.Time > 0f) ? MathHelper.Clamp(m_currentTime / m_currentNode.Time, 0f, 1f) : 1f;
			if (m_registerEvents && m_currentTime >= m_eventDelay)
			{
				m_eventDelay = float.MaxValue;
				MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentNode.Event);
			}
			if (m_moveTarget != null)
			{
				m_nodeEndMatrix.Translation = m_moveTarget.PositionComp.GetPosition();
			}
			Vector3D vector3D = m_currentCameraMatrix.Translation;
			if (m_attachedPositionTo != null)
			{
				if (!m_attachedPositionTo.Closed)
				{
					vector3D = Vector3D.Transform(m_attachedPositionOffset, m_attachedPositionTo.PositionComp.WorldMatrix);
				}
			}
			else if (m_waypoints.Count > 2)
			{
				double num2 = 1f / (float)(m_waypoints.Count - 1);
				int num3 = (int)Math.Floor((double)num / num2);
				if (num3 > m_waypoints.Count - 2)
				{
					num3 = m_waypoints.Count - 2;
				}
				double t = ((double)num - (double)num3 * num2) / num2;
				vector3D = ((num3 == 0) ? MathHelper.CalculateBezierPoint(t, m_waypoints[num3].Translation, m_waypoints[num3].Translation, m_waypoints[num3 + 1].Translation - (m_waypoints[num3 + 2].Translation - m_waypoints[num3].Translation) / 4.0, m_waypoints[num3 + 1].Translation) : ((num3 < m_waypoints.Count - 2) ? MathHelper.CalculateBezierPoint(t, m_waypoints[num3].Translation, m_waypoints[num3].Translation + (m_waypoints[num3 + 1].Translation - m_waypoints[num3 - 1].Translation) / 4.0, m_waypoints[num3 + 1].Translation - (m_waypoints[num3 + 2].Translation - m_waypoints[num3].Translation) / 4.0, m_waypoints[num3 + 1].Translation) : MathHelper.CalculateBezierPoint(t, m_waypoints[num3].Translation, m_waypoints[num3].Translation + (m_waypoints[num3 + 1].Translation - m_waypoints[num3 - 1].Translation) / 4.0, m_waypoints[num3 + 1].Translation, m_waypoints[num3 + 1].Translation)));
			}
			else if (m_nodeStartMatrix.Translation != m_nodeEndMatrix.Translation)
			{
				vector3D = new Vector3D(MathHelper.SmoothStep(m_nodeStartMatrix.Translation.X, m_nodeEndMatrix.Translation.X, num), MathHelper.SmoothStep(m_nodeStartMatrix.Translation.Y, m_nodeEndMatrix.Translation.Y, num), MathHelper.SmoothStep(m_nodeStartMatrix.Translation.Z, m_nodeEndMatrix.Translation.Z, num));
			}
			if (m_rotateTarget != null)
			{
				m_nodeEndMatrix = MatrixD.CreateLookAtInverse(m_currentCameraMatrix.Translation, m_rotateTarget.PositionComp.GetPosition(), m_nodeStartMatrix.Up);
			}
			if (m_lookTarget != null)
			{
				if (!m_lookTarget.Closed)
				{
					m_currentCameraMatrix = MatrixD.CreateLookAtInverse(vector3D, m_lookTarget.PositionComp.GetPosition(), (m_waypoints.Count > 2) ? m_lastUpVector : m_currentCameraMatrix.Up);
				}
			}
			else if (m_attachedRotationTo != null)
			{
				m_currentCameraMatrix = m_attachedRotationOffset * m_attachedRotationTo.WorldMatrix;
			}
			else if (m_waypoints.Count > 2)
			{
				float num4 = 1f / (float)(m_waypoints.Count - 1);
				int num5 = (int)Math.Floor(num / num4);
				if (num5 > m_waypoints.Count - 2)
				{
					num5 = m_waypoints.Count - 2;
				}
				float num6 = (num - (float)num5 * num4) / num4;
				QuaternionD quaternion = QuaternionD.CreateFromRotationMatrix(m_waypoints[num5]);
				QuaternionD quaternion2 = QuaternionD.CreateFromRotationMatrix(m_waypoints[num5 + 1]);
				QuaternionD quaternion3 = QuaternionD.Slerp(quaternion, quaternion2, MathHelper.SmoothStepStable((double)num6));
				m_currentCameraMatrix = MatrixD.CreateFromQuaternion(quaternion3);
			}
			else if (!m_nodeStartMatrix.EqualsFast(ref m_nodeEndMatrix))
			{
				QuaternionD quaternion4 = QuaternionD.CreateFromRotationMatrix(m_nodeStartMatrix);
				QuaternionD quaternion5 = QuaternionD.CreateFromRotationMatrix(m_nodeEndMatrix);
				QuaternionD quaternion6 = QuaternionD.Slerp(quaternion4, quaternion5, MathHelper.SmoothStepStable((double)num));
				m_currentCameraMatrix = MatrixD.CreateFromQuaternion(quaternion6);
			}
			m_currentCameraMatrix.Translation = vector3D;
			if (m_currentNode.ChangeFOVTo > MINIMUM_FOV)
			{
				m_currentFOV = MathHelper.SmoothStep(m_nodeStartFOV, MathHelper.Clamp(m_currentNode.ChangeFOVTo, MINIMUM_FOV, MAXIMUM_FOV), num);
			}
			m_cameraEntity.FOV = m_currentFOV;
			if (m_currentTime >= m_currentNode.Time)
			{
				CutsceneNext(setToZero: false);
			}
		}

		public void CutsceneEnd(bool releaseCamera = true)
		{
			MyHudWarnings.EnableWarnings = true;
			if (m_currentCutscene != null)
			{
				if (MyVisualScriptLogicProvider.CutsceneEnded != null)
				{
					MyVisualScriptLogicProvider.CutsceneEnded(m_currentCutscene.Name);
				}
				m_currentCutscene = null;
				if (releaseCamera)
				{
					m_cameraEntity.FOV = MathHelper.ToDegrees(MySandboxGame.Config.FieldOfView);
					m_releaseCamera = true;
				}
				MyHudCameraOverlay.TextureName = m_cameraOverlayOriginal;
				MyHudCameraOverlay.Enabled = m_overlayEnabled;
			}
		}

		public void CutsceneNext(bool setToZero)
		{
			m_nodeActivated = false;
			m_currentNodeIndex++;
			m_currentTime -= (setToZero ? m_currentTime : m_currentNode.Time);
		}

		public void CutsceneSkip()
		{
			if (m_currentCutscene == null)
			{
				return;
			}
			if (m_currentCutscene.CanBeSkipped)
			{
				if (m_currentCutscene.FireEventsDuringSkip && MyVisualScriptLogicProvider.CutsceneNodeEvent != null && m_registerEvents)
				{
					if (m_currentNode != null && m_currentNode.EventDelay > 0f && m_eventDelay != float.MaxValue)
					{
						MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentNode.Event);
					}
					for (int i = m_currentNodeIndex + 1; i < m_currentCutscene.SequenceNodes.Count; i++)
					{
						if (!string.IsNullOrEmpty(m_currentCutscene.SequenceNodes[i].Event))
						{
							MyVisualScriptLogicProvider.CutsceneNodeEvent(m_currentCutscene.SequenceNodes[i].Event);
						}
					}
				}
				m_currentNodeIndex = m_currentCutscene.SequenceNodes.Count;
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
			}
			else
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_cutsceneLibrary.Clear();
			MyHudWarnings.EnableWarnings = true;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			m_objectBuilder.Cutscenes = new Cutscene[m_cutsceneLibrary.Count];
			int num = 0;
			foreach (Cutscene value in m_cutsceneLibrary.Values)
			{
				m_objectBuilder.Cutscenes[num] = value;
				num++;
			}
			return m_objectBuilder;
		}

		public bool PlayCutscene(string cutsceneName, bool registerEvents = true, string overlay = "")
		{
			if (m_cutsceneLibrary.ContainsKey(cutsceneName))
			{
				return PlayCutscene(m_cutsceneLibrary[cutsceneName], registerEvents, overlay);
			}
			CutsceneEnd();
			return false;
		}

		public bool PlayCutscene(Cutscene cutscene, bool registerEvents = true, string overlay = "")
		{
			if (cutscene != null)
			{
				MySandboxGame.Log.WriteLineAndConsole("Cutscene start: " + cutscene.Name);
				if (IsCutsceneRunning)
				{
					CutsceneEnd(releaseCamera: false);
				}
				else
				{
					m_cameraOverlayOriginal = MyHudCameraOverlay.TextureName;
					m_overlayEnabled = MyHudCameraOverlay.Enabled;
				}
				m_registerEvents = registerEvents;
				m_cameraOverlay = overlay;
				m_currentCutscene = cutscene;
				m_currentNode = null;
				m_currentNodeIndex = 0;
				m_currentTime = 0f;
				m_nodeActivated = false;
				m_lookTarget = null;
				m_attachedPositionTo = null;
				m_attachedRotationTo = null;
				m_rotateTarget = null;
				m_moveTarget = null;
				m_currentFOV = MathHelper.Clamp(m_currentCutscene.StartingFOV, MINIMUM_FOV, MAXIMUM_FOV);
				MyGuiScreenGamePlay.DisableInput = true;
				if (MyCubeBuilder.Static.IsActivated)
				{
					MyCubeBuilder.Static.Deactivate();
				}
				MyHud.CutsceneHud = true;
				MyHudCameraOverlay.TextureName = overlay;
				MyHudCameraOverlay.Enabled = (overlay.Length > 0);
				MyHudWarnings.EnableWarnings = false;
				MatrixD matrixD = MatrixD.Identity;
				MyEntity myEntity = (m_currentCutscene.StartEntity.Length > 0) ? MyVisualScriptLogicProvider.GetEntityByName(m_currentCutscene.StartEntity) : null;
				if (myEntity != null)
				{
					matrixD = myEntity.WorldMatrix;
				}
				if (m_currentCutscene.StartLookAt.Length > 0 && !m_currentCutscene.StartLookAt.Equals(m_currentCutscene.StartEntity))
				{
					myEntity = MyVisualScriptLogicProvider.GetEntityByName(m_currentCutscene.StartLookAt);
					if (myEntity != null)
					{
						matrixD = MatrixD.CreateLookAtInverse(matrixD.Translation, myEntity.PositionComp.GetPosition(), matrixD.Up);
					}
				}
				m_nodeStartMatrix = matrixD;
				m_currentCameraMatrix = matrixD;
				m_originalCameraController = MySession.Static.CameraController;
				m_cameraEntity.WorldMatrix = matrixD;
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, m_cameraEntity);
				return true;
			}
			CutsceneEnd();
			return false;
		}

		public Dictionary<string, Cutscene> GetCutscenes()
		{
			return m_cutsceneLibrary;
		}

		public Cutscene GetCutscene(string name)
		{
			if (m_cutsceneLibrary.ContainsKey(name))
			{
				return m_cutsceneLibrary[name];
			}
			return null;
		}

		public Cutscene GetCutsceneCopy(string name)
		{
			if (m_cutsceneLibrary.ContainsKey(name))
			{
				Cutscene cutscene = m_cutsceneLibrary[name];
				Cutscene cutscene2 = new Cutscene();
				cutscene2.CanBeSkipped = cutscene.CanBeSkipped;
				cutscene2.FireEventsDuringSkip = cutscene.FireEventsDuringSkip;
				cutscene2.Name = cutscene.Name;
				cutscene2.NextCutscene = cutscene.NextCutscene;
				cutscene2.StartEntity = cutscene.StartEntity;
				cutscene2.StartingFOV = cutscene.StartingFOV;
				cutscene2.StartLookAt = cutscene.StartLookAt;
				if (cutscene.SequenceNodes != null)
				{
					cutscene2.SequenceNodes = new List<CutsceneSequenceNode>();
					for (int i = 0; i < cutscene.SequenceNodes.Count; i++)
					{
						cutscene2.SequenceNodes.Add(new CutsceneSequenceNode());
						cutscene2.SequenceNodes[i].AttachPositionTo = cutscene.SequenceNodes[i].AttachPositionTo;
						cutscene2.SequenceNodes[i].AttachRotationTo = cutscene.SequenceNodes[i].AttachRotationTo;
						cutscene2.SequenceNodes[i].AttachTo = cutscene.SequenceNodes[i].AttachTo;
						cutscene2.SequenceNodes[i].ChangeFOVTo = cutscene.SequenceNodes[i].ChangeFOVTo;
						cutscene2.SequenceNodes[i].Event = cutscene.SequenceNodes[i].Event;
						cutscene2.SequenceNodes[i].EventDelay = cutscene.SequenceNodes[i].EventDelay;
						cutscene2.SequenceNodes[i].LockRotationTo = cutscene.SequenceNodes[i].LockRotationTo;
						cutscene2.SequenceNodes[i].LookAt = cutscene.SequenceNodes[i].LookAt;
						cutscene2.SequenceNodes[i].MoveTo = cutscene.SequenceNodes[i].MoveTo;
						cutscene2.SequenceNodes[i].RotateLike = cutscene.SequenceNodes[i].RotateLike;
						cutscene2.SequenceNodes[i].RotateTowards = cutscene.SequenceNodes[i].RotateTowards;
						cutscene2.SequenceNodes[i].SetPositionTo = cutscene.SequenceNodes[i].SetPositionTo;
						cutscene2.SequenceNodes[i].SetRorationLike = cutscene.SequenceNodes[i].SetRorationLike;
						cutscene2.SequenceNodes[i].Time = cutscene.SequenceNodes[i].Time;
						if (cutscene.SequenceNodes[i].Waypoints != null && cutscene.SequenceNodes[i].Waypoints.Count > 0)
						{
							cutscene2.SequenceNodes[i].Waypoints = new List<CutsceneSequenceNodeWaypoint>();
							for (int j = 0; j < cutscene.SequenceNodes[i].Waypoints.Count; j++)
							{
								cutscene2.SequenceNodes[i].Waypoints.Add(new CutsceneSequenceNodeWaypoint());
								cutscene2.SequenceNodes[i].Waypoints[j].Name = cutscene.SequenceNodes[i].Waypoints[j].Name;
								cutscene2.SequenceNodes[i].Waypoints[j].Time = cutscene.SequenceNodes[i].Waypoints[j].Time;
							}
						}
					}
				}
				return cutscene2;
			}
			return null;
		}
	}
}
