using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyAIDebugInputComponent : MyDebugComponent
	{
		private struct DebugDrawPoint
		{
			public Vector3D Position;

			public Color Color;
		}

		private struct DebugDrawSphere
		{
			public Vector3D Position;

			public float Radius;

			public Color Color;
		}

		private struct DebugDrawBox
		{
			public BoundingBoxD Box;

			public Color Color;
		}

		private bool m_drawSphere;

		private BoundingSphere m_sphere;

		private Matrix m_sphereMatrix;

		private string m_string;

		private Vector3D m_point1;

		private Vector3D m_point2;

		private MySmartPath m_smartPath;

		private Vector3D m_currentTarget;

		private List<Vector3D> m_pastTargets = new List<Vector3D>();

		public static int FaceToRemove;

		public static int BinIndex = -1;

		private static List<DebugDrawPoint> DebugDrawPoints = new List<DebugDrawPoint>();

		private static List<DebugDrawSphere> DebugDrawSpheres = new List<DebugDrawSphere>();

		private static List<DebugDrawBox> DebugDrawBoxes = new List<DebugDrawBox>();

		private static MyWingedEdgeMesh DebugDrawMesh = null;

		private static List<MyPolygon> DebugDrawPolys = new List<MyPolygon>();

		public static List<BoundingBoxD> Boxes = null;

		private static bool m_drawDebug = false;

		private static bool m_drawNavesh = false;

		private static bool m_drawPhysicalMesh = false;

		public MyAIDebugInputComponent()
		{
			if (MyPerGameSettings.EnableAi)
			{
				AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Toggle Draw Grid Physical Mesh", ToggleDrawPhysicalMesh);
				AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Add bot", AddBot);
				AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Remove bot", RemoveBot);
				AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Toggle Draw Debug", ToggleDrawDebug);
				AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Toggle Wireframe", ToggleWireframe);
				AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Set PF target", SetPathfindingDebugTarget);
				AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Toggle Draw Navmesh", ToggleDrawNavmesh);
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Generate Navmesh Tile", GenerateNavmeshTile);
				AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Invalidate Navmesh Position", InvalidateNavmeshPosition);
			}
		}

		private bool ToggleDrawDebug()
		{
			m_drawDebug = !m_drawDebug;
			MyAIComponent.Static.PathfindingSetDrawDebug(m_drawDebug);
			return true;
		}

		private bool ToggleWireframe()
		{
			MyRenderProxy.Settings.Wireframe = !MyRenderProxy.Settings.Wireframe;
			return true;
		}

		private bool SetPathfindingDebugTarget()
		{
			Vector3D? targetPosition = GetTargetPosition();
			MyAIComponent.Static.SetPathfindingDebugTarget(targetPosition);
			return true;
		}

		private bool GenerateNavmeshTile()
		{
			Vector3D? targetPosition = GetTargetPosition();
			MyAIComponent.Static.GenerateNavmeshTile(targetPosition);
			return true;
		}

		private bool InvalidateNavmeshPosition()
		{
			Vector3D? targetPosition = GetTargetPosition();
			MyAIComponent.Static.InvalidateNavmeshPosition(targetPosition);
			return true;
		}

		private bool ToggleDrawNavmesh()
		{
			m_drawNavesh = !m_drawNavesh;
			MyAIComponent.Static.PathfindingSetDrawNavmesh(m_drawNavesh);
			return true;
		}

		private bool ToggleDrawPhysicalMesh()
		{
			m_drawPhysicalMesh = !m_drawPhysicalMesh;
			return true;
		}

		private Vector3D? GetTargetPosition()
		{
			LineD lineD = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f);
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(lineD.From, lineD.To, list, 15);
			list.RemoveAll((MyPhysics.HitInfo hit) => hit.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity);
			if (list.Count == 0)
			{
				return null;
			}
			return list[0].Position;
		}

		private bool AddBot()
		{
			MyAgentDefinition agentDefinition = (MyPerGameSettings.Game != GameEnum.SE_GAME) ? (MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_HumanoidBot), "NormalBarbarian")) as MyAgentDefinition) : (MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AnimalBot), "Wolf")) as MyAgentDefinition);
			MyAIComponent.Static.TrySpawnBot(agentDefinition);
			return true;
		}

		private bool RemoveBot()
		{
			int num = -1;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SteamId == Sync.MyId)
				{
					num = Math.Max(num, onlinePlayer.Id.SerialId);
				}
			}
			if (num > 0)
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, num));
				Sync.Players.RemovePlayer(playerById);
			}
			return true;
		}

		public override string GetName()
		{
			return "A.I.";
		}

		public override bool HandleInput()
		{
			if (MySession.Static == null)
			{
				return false;
			}
			if (MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogPrefabCheat)
			{
				return false;
			}
			if (MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogRemoveTriangle)
			{
				return false;
			}
			if (MyScreenManager.GetScreenWithFocus() is MyGuiScreenDialogViewEdge)
			{
				return false;
			}
			return base.HandleInput();
		}

		public static void AddDebugPoint(Vector3D point, Color color)
		{
			DebugDrawPoints.Add(new DebugDrawPoint
			{
				Position = point,
				Color = color
			});
		}

		public static void ClearDebugPoints()
		{
			DebugDrawPoints.Clear();
		}

		public static void AddDebugSphere(Vector3D position, float radius, Color color)
		{
			DebugDrawSpheres.Add(new DebugDrawSphere
			{
				Position = position,
				Radius = radius,
				Color = color
			});
		}

		public static void ClearDebugSpheres()
		{
			DebugDrawSpheres.Clear();
		}

		public static void AddDebugBox(BoundingBoxD box, Color color)
		{
			DebugDrawBoxes.Add(new DebugDrawBox
			{
				Box = box,
				Color = color
			});
		}

		public static void ClearDebugBoxes()
		{
			DebugDrawBoxes.Clear();
		}

		public override void Draw()
		{
			base.Draw();
			if (MySector.MainCamera != null)
			{
				Vector3D position = MySector.MainCamera.Position;
				Vector3 forwardVector = MySector.MainCamera.ForwardVector;
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(position, position + 500f * forwardVector);
				if (hitInfo.HasValue)
				{
					IMyEntity hitEntity = hitInfo.Value.HkHitInfo.GetHitEntity();
					if (hitEntity != null)
					{
						MyVoxelPhysics myVoxelPhysics = hitEntity.GetTopMostParent() as MyVoxelPhysics;
						if (myVoxelPhysics != null)
						{
							MyPlanet parent = myVoxelPhysics.Parent;
							IMyGravityProvider myGravityProvider = parent as IMyGravityProvider;
							if (myGravityProvider != null)
							{
								Vector3 worldGravity = myGravityProvider.GetWorldGravity(hitInfo.Value.Position);
								worldGravity.Normalize();
								Vector3D vector3D = parent.PositionComp.GetPosition() - worldGravity * 9503f;
								MyRenderProxy.DebugDrawSphere(vector3D, 0.5f, Color.Red, 1f, depthRead: false);
								MyRenderProxy.DebugDrawSphere(vector3D, 5.5f, Color.Yellow, 1f, depthRead: false);
								hitInfo = MyPhysics.CastRay(vector3D, vector3D + worldGravity * 500f);
								if (hitInfo.HasValue)
								{
									MyRenderProxy.DebugDrawText2D(new Vector2(10f, 10f), (hitInfo.Value.HkHitInfo.HitFraction * 500f).ToString(), Color.White, 0.8f);
								}
							}
						}
					}
				}
			}
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyCubeBuilder.Static == null)
			{
				return;
			}
			if (m_smartPath != null)
			{
				m_smartPath.DebugDraw();
				MyRenderProxy.DebugDrawSphere(m_currentTarget, 2f, Color.HotPink, 1f, depthRead: false);
				for (int i = 1; i < m_pastTargets.Count; i++)
				{
					MyRenderProxy.DebugDrawLine3D(m_pastTargets[i], m_pastTargets[i - 1], Color.Blue, Color.Blue, depthRead: false);
				}
			}
			MyRenderProxy.DebugDrawOBB(MyCubeBuilder.Static.GetBuildBoundingBox(), Color.Red, 0.25f, depthRead: false, smooth: false);
			MyScreenManager.GetScreenWithFocus();
			if (MyScreenManager.GetScreenWithFocus() != null && !(MyScreenManager.GetScreenWithFocus().DebugNamePath != "MyGuiScreenGamePlay"))
			{
				if (m_drawSphere)
				{
					MyRenderProxy.DebugDrawSphere(m_sphere.Center, m_sphere.Radius, Color.Red, 1f, depthRead: false);
					MyRenderProxy.DebugDrawAxis(m_sphereMatrix, 50f, depthRead: false);
					MyRenderProxy.DebugDrawText2D(new Vector2(200f, 0f), m_string, Color.Red, 0.5f);
				}
				MyRenderProxy.DebugDrawSphere(m_point1, 0.5f, Color.Orange.ToVector3());
				MyRenderProxy.DebugDrawSphere(m_point2, 0.5f, Color.Orange.ToVector3());
				foreach (DebugDrawPoint debugDrawPoint in DebugDrawPoints)
				{
					MyRenderProxy.DebugDrawSphere(debugDrawPoint.Position, 0.03f, debugDrawPoint.Color, 1f, depthRead: false);
				}
				foreach (DebugDrawSphere debugDrawSphere in DebugDrawSpheres)
				{
					MyRenderProxy.DebugDrawSphere(debugDrawSphere.Position, debugDrawSphere.Radius, debugDrawSphere.Color, 1f, depthRead: false);
				}
				foreach (DebugDrawBox debugDrawBox in DebugDrawBoxes)
				{
					MyRenderProxy.DebugDrawAABB(debugDrawBox.Box, debugDrawBox.Color, 1f, 1f, depthRead: false);
				}
				if (DebugDrawMesh != null)
				{
					Matrix drawMatrix = Matrix.Identity;
					DebugDrawMesh.DebugDraw(ref drawMatrix, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES);
				}
				foreach (MyPolygon debugDrawPoly in DebugDrawPolys)
				{
					MatrixD drawMatrix2 = MatrixD.Identity;
					debugDrawPoly.DebugDraw(ref drawMatrix2);
				}
				MyPolygonBoolOps.Static.DebugDraw(MatrixD.Identity);
				if (Boxes != null)
				{
					foreach (BoundingBoxD box in Boxes)
					{
						MyRenderProxy.DebugDrawAABB(box, Color.Red);
					}
				}
			}
		}
	}
}
