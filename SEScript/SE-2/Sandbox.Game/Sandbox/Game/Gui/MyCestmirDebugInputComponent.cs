using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyCestmirDebugInputComponent : MyDebugComponent
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

		private class Vector3Comparer : IComparer<Vector3>
		{
			private Vector3 m_right;

			private Vector3 m_up;

			public Vector3Comparer(Vector3 right, Vector3 up)
			{
				m_right = right;
				m_up = up;
			}

			public int Compare(Vector3 x, Vector3 y)
			{
				Vector3.Dot(ref x, ref m_right, out float result);
				Vector3.Dot(ref y, ref m_right, out float result2);
				float num = result - result2;
				if (num < 0f)
				{
					return -1;
				}
				if (num > 0f)
				{
					return 1;
				}
				Vector3.Dot(ref x, ref m_up, out result);
				Vector3.Dot(ref y, ref m_up, out result2);
				num = result - result2;
				if (num < 0f)
				{
					return -1;
				}
				if (num > 0f)
				{
					return 1;
				}
				return 0;
			}
		}

		protected sealed class AddPrefabServer_003C_003ESystem_String_0023VRageMath_MatrixD : ICallSite<IMyEventOwner, string, MatrixD, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string prefabId, in MatrixD worldMatrix, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddPrefabServer(prefabId, worldMatrix);
			}
		}

		private bool m_drawSphere;

		private BoundingSphere m_sphere;

		private Matrix m_sphereMatrix;

		private string m_string;

		private Vector3D m_point1;

		private Vector3D m_point2;

		private IMyPath m_smartPath;

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

		private static List<Tuple<Vector2[], Vector2[]>> m_testList = null;

		private static int m_testIndex = 0;

		private static int m_testOperation = 0;

		private static int m_prevTestIndex = 0;

		private static int m_prevTestOperation = 0;

		public static event Action TestAction;

		public static event Action<Vector3D, MyEntity> PlacedAction;

		public MyCestmirDebugInputComponent()
		{
			AddShortcut(MyKeys.NumPad0, newPress: true, control: false, shift: false, alt: false, () => "Add prefab...", AddPrefab);
			AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Copy target grid position to clipboard", CaptureGridPosition);
			if (MyPerGameSettings.EnableAi)
			{
				AddShortcut(MyKeys.Multiply, newPress: true, control: false, shift: false, alt: false, () => "Next navmesh connection helper bin", NextBin);
				AddShortcut(MyKeys.Divide, newPress: true, control: false, shift: false, alt: false, () => "Prev navmesh connection helper bin", PrevBin);
				AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Add bot", AddBot);
				AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Remove bot", RemoveBot);
				AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Find path for first bot", FindBotPath);
				AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Find path between points", FindPath);
				AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Find smart path between points", FindSmartPath);
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Get next smart path target", GetNextTarget);
				AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "Test", EmitTestAction);
				AddShortcut(MyKeys.Add, newPress: true, control: false, shift: false, alt: false, () => "Next funnel segment", delegate
				{
					MyNavigationMesh.m_debugFunnelIdx++;
					return true;
				});
				AddShortcut(MyKeys.Subtract, newPress: true, control: false, shift: false, alt: false, () => "Previous funnel segment", delegate
				{
					if (MyNavigationMesh.m_debugFunnelIdx > 0)
					{
						MyNavigationMesh.m_debugFunnelIdx--;
					}
					return true;
				});
				AddShortcut(MyKeys.O, newPress: true, control: false, shift: false, alt: false, () => "Remove navmesh tri...", delegate
				{
					MyGuiSandbox.AddScreen(new MyGuiScreenDialogRemoveTriangle());
					return true;
				});
				AddShortcut(MyKeys.M, newPress: true, control: false, shift: false, alt: false, () => "View all navmesh edges", delegate
				{
					MyWingedEdgeMesh.DebugDrawEdgesReset();
					return true;
				});
				AddShortcut(MyKeys.L, newPress: true, control: false, shift: false, alt: false, () => "View single navmesh edge...", delegate
				{
					MyGuiSandbox.AddScreen(new MyGuiScreenDialogViewEdge());
					return true;
				});
			}
			else
			{
				AddShortcut(MyKeys.I, newPress: true, control: true, shift: false, alt: false, () => "Place an environment item in front of the player", AddEnvironmentItem);
			}
		}

		private bool AddEnvironmentItem()
		{
			return true;
		}

		private bool AddPrefab()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenDialogPrefabCheat());
			return true;
		}

		[Event(null, 336)]
		[Reliable]
		[Server]
		public static void AddPrefabServer(string prefabId, MatrixD worldMatrix)
		{
			int num;
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				List<string> administrators = MySandboxGame.ConfigDedicated.Administrators;
				EndpointId sender = MyEventContext.Current.Sender;
				num = (administrators.Contains(sender.ToString()) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			if (num != 0 || MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				MyPrefabManager.Static.SpawnPrefab(prefabId, worldMatrix.Translation, worldMatrix.Forward, worldMatrix.Up, Vector3.Zero, Vector3.Zero, prefabId, prefabId, SpawningOptions.None, 0L, updateSync: true);
			}
		}

		private bool CaptureGridPosition()
		{
			Vector3D position = MySector.MainCamera.Position;
			Vector3D to = MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 1000f;
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(position, to, list);
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				MyCubeGrid myCubeGrid = list[i].HkHitInfo.GetHitEntity() as MyCubeGrid;
				if (myCubeGrid != null)
				{
					MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = myCubeGrid.GetObjectBuilder() as MyObjectBuilder_CubeGrid;
					if (myObjectBuilder_CubeGrid != null)
					{
						m_sphere = myObjectBuilder_CubeGrid.CalculateBoundingSphere();
						m_sphere = m_sphere.Transform(myCubeGrid.WorldMatrix);
						m_sphereMatrix = myCubeGrid.WorldMatrix;
						m_sphereMatrix.Translation = m_sphere.Center;
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendFormat("<Position x=\"{0}\" y=\"{1}\" z=\"{2}\" />\n<Forward x=\"{3}\" y=\"{4}\" z=\"{5}\" />\n<Up x=\"{6}\" y=\"{7}\" z=\"{8}\" />", m_sphereMatrix.Translation.X, m_sphereMatrix.Translation.Y, m_sphereMatrix.Translation.Z, m_sphereMatrix.Forward.X, m_sphereMatrix.Forward.Y, m_sphereMatrix.Forward.Z, m_sphereMatrix.Up.X, m_sphereMatrix.Up.Y, m_sphereMatrix.Up.Z);
						m_string = stringBuilder.ToString();
						MyVRage.Platform.Clipboard = m_string;
						flag = true;
						break;
					}
				}
			}
			m_drawSphere = flag;
			return flag;
		}

		private bool EmitTestAction()
		{
			if (MyCestmirDebugInputComponent.TestAction != null)
			{
				MyCestmirDebugInputComponent.TestAction();
			}
			return true;
		}

		private bool Test()
		{
			for (int i = 0; i < 1; i++)
			{
				ClearDebugPoints();
				DebugDrawPolys.Clear();
				float num = 8f;
				Vector3D vector3D = MySector.MainCamera.ForwardVector;
				Vector3D center = MySector.MainCamera.Position + vector3D * num;
				Vector3D tangent = MySector.MainCamera.WorldMatrix.Right;
				Vector3D bitangent = MySector.MainCamera.WorldMatrix.Up;
				Matrix transformation = MySector.MainCamera.WorldMatrix;
				transformation.Translation += vector3D * num;
				Plane p = new Plane(center, vector3D);
				List<DebugDrawPoint> debugDrawPoints = DebugDrawPoints;
				DebugDrawPoint item = new DebugDrawPoint
				{
					Position = center,
					Color = Color.Pink
				};
				debugDrawPoints.Add(item);
				List<DebugDrawPoint> debugDrawPoints2 = DebugDrawPoints;
				item = new DebugDrawPoint
				{
					Position = center + vector3D,
					Color = Color.Pink
				};
				debugDrawPoints2.Add(item);
				bool flag = true;
				bool flag2 = true;
				List<Vector3> list = new List<Vector3>();
				while (flag || flag2)
				{
					flag = false;
					flag2 = true;
					list.Clear();
					for (int j = 0; j < 6; j++)
					{
						Vector3 item2 = MyUtils.GetRandomDiscPosition(ref center, 4.5, ref tangent, ref bitangent);
						list.Add(item2);
					}
					for (int k = 0; k < list.Count; k++)
					{
						Line line = new Line(list[k], list[(k + 1) % list.Count]);
						Vector3 vector = Vector3.Normalize(line.Direction);
						for (int l = 0; l < list.Count; l++)
						{
							if (Math.Abs(l - k) <= 1 || (l == 0 && k == list.Count - 1) || (k == 0 && l == list.Count - 1))
							{
								continue;
							}
							Vector3 vector2 = list[l] - list[k];
							Vector3 vector3 = list[(l + 1) % list.Count] - list[k];
							if (!(Vector3.Dot(Vector3.Cross(vector2, vector), Vector3.Cross(vector3, vector)) >= 0f))
							{
								float num2 = Vector3.Dot(vector2, vector);
								float num3 = Vector3.Dot(vector3, vector);
								float num4 = Vector3.Reject(vector2, vector).Length();
								float num5 = Vector3.Reject(vector3, vector).Length();
								float num6 = num4 + num5;
								num4 /= num6;
								num5 /= num6;
								float num7 = num2 * num5 + num3 * num4;
								if (num7 <= line.Length && num7 >= 0f)
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
					float num8 = 0f;
					for (int m = 0; m < list.Count; m++)
					{
						Vector3 vector4 = list[m];
						Vector3 vector5 = list[(m + 1) % list.Count];
						num8 += (vector5.X - vector4.X) * (vector5.Y + vector4.Y);
					}
					if (num8 < 0f)
					{
						flag2 = false;
					}
				}
				foreach (Vector3 item3 in list)
				{
					AddDebugPoint(item3, Color.Yellow);
				}
				MyWingedEdgeMesh myWingedEdgeMesh = MyDefinitionManager.Static.GetCubeBlockDefinition(new MyDefinitionId(typeof(MyObjectBuilder_CubeBlock), "StoneCube")).NavigationDefinition.Mesh.Mesh.Copy();
				myWingedEdgeMesh.Transform(transformation);
				HashSet<int> hashSet = new HashSet<int>();
				MyWingedEdgeMesh.EdgeEnumerator edges = myWingedEdgeMesh.GetEdges();
				List<Vector3> list2 = new List<Vector3>();
				while (edges.MoveNext())
				{
					int currentIndex = edges.CurrentIndex;
					if (hashSet.Contains(currentIndex))
					{
						continue;
					}
					MyWingedEdgeMesh.Edge edge = myWingedEdgeMesh.GetEdge(currentIndex);
					list2.Clear();
					if (!myWingedEdgeMesh.IntersectEdge(ref edge, ref p, out Vector3 intersection))
					{
						continue;
					}
					list2.Add(intersection);
					int num9 = currentIndex;
					int num10 = edge.LeftFace;
					currentIndex = edge.GetNextFaceEdge(num10);
					edge = myWingedEdgeMesh.GetEdge(currentIndex);
					while (currentIndex != num9)
					{
						if (myWingedEdgeMesh.IntersectEdge(ref edge, ref p, out intersection))
						{
							num10 = edge.OtherFace(num10);
							if (Vector3.DistanceSquared(list2[list2.Count - 1], intersection) > 1E-06f)
							{
								list2.Add(intersection);
							}
						}
						currentIndex = edge.GetNextFaceEdge(num10);
						edge = myWingedEdgeMesh.GetEdge(currentIndex);
					}
					break;
				}
				edges.Dispose();
				List<int> list3 = new List<int>();
				DebugDrawMesh = myWingedEdgeMesh;
				list3.Clear();
				MyPolygon myPolygon = new MyPolygon(p);
				myPolygon.AddLoop(list2);
				DebugDrawPolys.Add(myPolygon);
				MyPolygon myPolygon2 = new MyPolygon(p);
				myPolygon2.AddLoop(list);
				DebugDrawPolys.Add(myPolygon2);
				MyPolygon myPolygon3 = MyPolygonBoolOps.Static.Difference(myPolygon, myPolygon2);
				Matrix transformationMatrix = Matrix.CreateTranslation(vector3D * -1.0);
				myPolygon3.Transform(ref transformationMatrix);
				DebugDrawPolys.Add(myPolygon3);
			}
			return true;
		}

		private bool Test2()
		{
			Plane polygonPlane = new Plane(Vector3.Forward, 0f);
			if (m_testList == null)
			{
				m_testList = new List<Tuple<Vector2[], Vector2[]>>();
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(1f, 1f),
					new Vector2(1f, 3f),
					new Vector2(3f, 3f),
					new Vector2(3f, 1f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(-1f, 1f),
					new Vector2(-1f, 3f),
					new Vector2(1f, 3f),
					new Vector2(1f, 1f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(-1f, -1f),
					new Vector2(-1f, 1f),
					new Vector2(1f, 1f),
					new Vector2(1f, -1f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(1f, -1f),
					new Vector2(1f, 1f),
					new Vector2(3f, 1f),
					new Vector2(3f, -1f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(-1f, 0f),
					new Vector2(-1f, 2f),
					new Vector2(1f, 2f),
					new Vector2(1f, 0f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(1f, 0f),
					new Vector2(1f, 2f),
					new Vector2(3f, 2f),
					new Vector2(3f, 0f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(0f, 1f),
					new Vector2(0f, 3f),
					new Vector2(2f, 3f),
					new Vector2(2f, 1f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(0f, -1f),
					new Vector2(0f, 1f),
					new Vector2(2f, 1f),
					new Vector2(2f, -1f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 2f),
					new Vector2(2f, 2f),
					new Vector2(2f, 0f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(-1f, 1f),
					new Vector2(0f, 2f),
					new Vector2(1f, 1f)
				}, new Vector2[4]
				{
					new Vector2(-2f, 1.3f),
					new Vector2(-2f, 2.3f),
					new Vector2(2f, 2.7f),
					new Vector2(2f, 1.7f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[5]
				{
					new Vector2(0f, 0f),
					new Vector2(1f, 5f),
					new Vector2(3f, 2f),
					new Vector2(4f, 4f),
					new Vector2(5f, 1f)
				}, new Vector2[4]
				{
					new Vector2(-1f, 4f),
					new Vector2(1f, 7f),
					new Vector2(6f, 4f),
					new Vector2(5f, 3f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[5]
				{
					new Vector2(0f, 3f),
					new Vector2(4f, 7f),
					new Vector2(9f, 8f),
					new Vector2(5f, 2f),
					new Vector2(2f, 0f)
				}, new Vector2[6]
				{
					new Vector2(0f, 9f),
					new Vector2(4f, 12f),
					new Vector2(7f, 9f),
					new Vector2(9f, 1f),
					new Vector2(4f, 9f),
					new Vector2(2f, 4f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(0f, 4.1f),
					new Vector2(4f, 4f),
					new Vector2(4f, 0.1f)
				}, new Vector2[4]
				{
					new Vector2(2f, 1f),
					new Vector2(1f, 2f),
					new Vector2(2f, 3f),
					new Vector2(3f, 2f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(3f, 0f),
					new Vector2(0f, 3f),
					new Vector2(3f, 6f),
					new Vector2(6f, 3f)
				}, new Vector2[4]
				{
					new Vector2(6f, 7f),
					new Vector2(8f, 5f),
					new Vector2(5f, 2f),
					new Vector2(3f, 4f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(3f, 0f),
					new Vector2(0f, 3f),
					new Vector2(3f, 6f),
					new Vector2(6f, 3f)
				}, new Vector2[3]
				{
					new Vector2(6f, 3f),
					new Vector2(3f, 6f),
					new Vector2(6f, 7f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(-2f, 2f),
					new Vector2(0f, 4f),
					new Vector2(2f, 2f)
				}, new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(-1f, 1f),
					new Vector2(0f, 2f),
					new Vector2(1f, 1f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(-2f, 2f),
					new Vector2(0f, 4f),
					new Vector2(2f, 2f)
				}, new Vector2[4]
				{
					new Vector2(1f, 1f),
					new Vector2(-0f, 2f),
					new Vector2(1f, 3f),
					new Vector2(2f, 2f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(-2f, 2f),
					new Vector2(0f, 4f),
					new Vector2(2f, 2f)
				}, new Vector2[4]
				{
					new Vector2(0f, 2f),
					new Vector2(-1f, 3f),
					new Vector2(0f, 4f),
					new Vector2(1f, 3f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[4]
				{
					new Vector2(0f, 0f),
					new Vector2(-2f, 2f),
					new Vector2(0f, 4f),
					new Vector2(2f, 2f)
				}, new Vector2[4]
				{
					new Vector2(-1f, 1f),
					new Vector2(-2f, 2f),
					new Vector2(-1f, 3f),
					new Vector2(0f, 2f)
				}));
				m_testList.Add(new Tuple<Vector2[], Vector2[]>(new Vector2[8]
				{
					new Vector2(2f, 0f),
					new Vector2(0f, 2f),
					new Vector2(4f, 6f),
					new Vector2(0f, 10f),
					new Vector2(2f, 12f),
					new Vector2(4f, 10f),
					new Vector2(0f, 6f),
					new Vector2(4f, 2f)
				}, new Vector2[4]
				{
					new Vector2(1f, 2f),
					new Vector2(1f, 8f),
					new Vector2(3f, 10f),
					new Vector2(3f, 4f)
				}));
			}
			DebugDrawPolys.Clear();
			m_prevTestIndex = m_testIndex;
			m_prevTestOperation = m_testOperation;
			Vector2[] source = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 4f),
				new Vector2(4f, 4f),
				new Vector2(4f, 0f)
			};
			Vector2[] source2 = new Vector2[4]
			{
				new Vector2(1f, 2f),
				new Vector2(2f, 1f),
				new Vector2(3f, 2f),
				new Vector2(2f, 3f)
			};
			Vector2[] source3 = new Vector2[4]
			{
				new Vector2(-1f, 2f),
				new Vector2(-1f, 5f),
				new Vector2(5f, 5f),
				new Vector2(5f, 2f)
			};
			_ = m_testList[m_testIndex];
			MyPolygon myPolygon = new MyPolygon(polygonPlane);
			MyPolygon myPolygon2 = new MyPolygon(polygonPlane);
			myPolygon.AddLoop(new List<Vector3>(source.Select((Vector2 i) => new Vector3(i.X, i.Y, 0f))));
			myPolygon.AddLoop(new List<Vector3>(source2.Select((Vector2 i) => new Vector3(i.X, i.Y, 0f))));
			myPolygon2.AddLoop(new List<Vector3>(source3.Select((Vector2 i) => new Vector3(i.X, i.Y, 0f))));
			DebugDrawPolys.Add(myPolygon);
			DebugDrawPolys.Add(myPolygon2);
			MyPolygon myPolygon3 = null;
			Stopwatch stopwatch = Stopwatch.StartNew();
			myPolygon3 = ((m_testOperation == 0) ? MyPolygonBoolOps.Static.Intersection(myPolygon, myPolygon2) : ((m_testOperation == 1) ? MyPolygonBoolOps.Static.Union(myPolygon, myPolygon2) : ((m_testOperation == 2) ? MyPolygonBoolOps.Static.Difference(myPolygon, myPolygon2) : ((m_testOperation == 3) ? MyPolygonBoolOps.Static.Intersection(myPolygon2, myPolygon) : ((m_testOperation != 4) ? MyPolygonBoolOps.Static.Difference(myPolygon2, myPolygon) : MyPolygonBoolOps.Static.Union(myPolygon2, myPolygon))))));
			TimeSpan elapsed = stopwatch.Elapsed;
			_ = default(TimeSpan) + elapsed;
			Matrix transformationMatrix = Matrix.CreateTranslation(Vector3.Right * 12f);
			myPolygon3.Transform(ref transformationMatrix);
			DebugDrawPolys.Add(myPolygon3);
			m_testIndex++;
			m_testIndex = 0;
			m_testOperation = (m_testOperation + 1) % 6;
			return true;
		}

		private bool EmitPlacedAction(Vector3D position, IMyEntity entity)
		{
			if (MyCestmirDebugInputComponent.PlacedAction != null)
			{
				MyCestmirDebugInputComponent.PlacedAction(position, entity as MyEntity);
			}
			return true;
		}

		private bool NextBin()
		{
			BinIndex++;
			return true;
		}

		private bool PrevBin()
		{
			BinIndex--;
			if (BinIndex < -1)
			{
				BinIndex = -1;
			}
			return true;
		}

		private bool AddBot()
		{
			MyAgentDefinition agentDefinition = MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AnimalBot), "Wolf")) as MyAgentDefinition;
			MyAIComponent.Static.SpawnNewBot(agentDefinition);
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

		private bool FindPath()
		{
			Raycast(out Vector3D? firstHit, out IMyEntity _);
			if (firstHit.HasValue)
			{
				m_point1 = m_point2;
				m_point2 = firstHit.Value;
				MyCestmirPathfindingShorts.Pathfinding.FindPathLowlevel(m_point1, m_point2);
			}
			return true;
		}

		private bool FindSmartPath()
		{
			if (MyAIComponent.Static.Pathfinding == null)
			{
				return false;
			}
			Raycast(out Vector3D? firstHit, out IMyEntity _);
			if (firstHit.HasValue)
			{
				m_point1 = m_point2;
				m_point2 = firstHit.Value;
				MyDestinationSphere end = new MyDestinationSphere(ref m_point2, 3f);
				if (m_smartPath != null)
				{
					m_smartPath.Invalidate();
				}
				m_smartPath = MyAIComponent.Static.Pathfinding.FindPathGlobal(m_point1, end, null);
				m_pastTargets.Clear();
				m_currentTarget = m_point1;
				m_pastTargets.Add(m_currentTarget);
			}
			return true;
		}

		private bool GetNextTarget()
		{
			if (m_smartPath == null)
			{
				return false;
			}
			m_smartPath.GetNextTarget(m_currentTarget, out m_currentTarget, out float _, out IMyEntity _);
			m_pastTargets.Add(m_currentTarget);
			return true;
		}

		private bool FindBotPath()
		{
			Raycast(out Vector3D? firstHit, out IMyEntity entity);
			if (firstHit.HasValue)
			{
				EmitPlacedAction(firstHit.Value, entity);
			}
			return true;
		}

		public override string GetName()
		{
			return "Cestmir";
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

		private static void Raycast(out Vector3D? firstHit, out IMyEntity entity)
		{
			MyCamera mainCamera = MySector.MainCamera;
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(mainCamera.Position, mainCamera.Position + mainCamera.ForwardVector * 1000f, list);
			if (list.Count > 0)
			{
				firstHit = list[0].Position;
				entity = list[0].HkHitInfo.GetHitEntity();
			}
			else
			{
				firstHit = null;
				entity = null;
			}
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
				MyRenderProxy.DebugDrawText2D(new Vector2(300f, 0f), "Test index: " + m_prevTestIndex + "/" + ((m_testList == null) ? "-" : m_testList.Count.ToString()) + ", Test operation: " + m_prevTestOperation, Color.Red, 1f);
				if (m_prevTestOperation % 3 == 0)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(300f, 20f), "Intersection", Color.Red, 1f);
				}
				else if (m_prevTestOperation % 3 == 1)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(300f, 20f), "Union", Color.Red, 1f);
				}
				else if (m_prevTestOperation % 3 == 2)
				{
					MyRenderProxy.DebugDrawText2D(new Vector2(300f, 20f), "Difference", Color.Red, 1f);
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
