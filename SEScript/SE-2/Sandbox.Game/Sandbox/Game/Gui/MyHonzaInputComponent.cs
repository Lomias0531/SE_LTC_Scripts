using Havok;
using ParallelTasks;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyHonzaInputComponent : MyMultiDebugInputComponent
	{
		public class DefaultComponent : MyDebugComponent
		{
			public enum ShownMassEnum
			{
				Havok,
				Real,
				SI,
				None,
				MaxVal
			}

			public static float MassMultiplier = 100f;

			private static long m_lastMemory;

			private static HkMemorySnapshot m_snapA;

			public static bool ApplyMassMultiplier;

			public static ShownMassEnum ShowRealBlockMass = ShownMassEnum.None;

			private int m_memoryB;

			private int m_memoryA;

			private static bool HammerForce;

			private float RADIUS = 0.005f;

			private bool m_drawBodyInfo = true;

			private bool m_drawUpdateInfo;

			private List<Type> m_dbgComponents = new List<Type>();

			public override string GetName()
			{
				return "Honza";
			}

			public DefaultComponent()
			{
				AddShortcut(MyKeys.S, newPress: true, control: true, shift: false, alt: false, () => "Insert safe zone", delegate
				{
					TestParallelBatch();
					return true;
				});
				AddShortcut(MyKeys.None, newPress: false, control: false, shift: false, alt: false, () => "Hammer (CTRL + Mouse left)", null);
				AddShortcut(MyKeys.H, newPress: true, control: true, shift: true, alt: false, () => "Hammer force: " + (HammerForce ? "ON" : "OFF"), delegate
				{
					HammerForce = !HammerForce;
					return true;
				});
				AddShortcut(MyKeys.OemPlus, newPress: true, control: true, shift: false, alt: false, () => "Radius+: " + RADIUS, delegate
				{
					RADIUS += 0.5f;
					return true;
				});
				AddShortcut(MyKeys.OemMinus, newPress: true, control: true, shift: false, alt: false, () => "", delegate
				{
					RADIUS -= 0.5f;
					return true;
				});
				AddShortcut(MyKeys.NumPad7, newPress: true, control: false, shift: false, alt: false, () => "Shown mass: " + ShowRealBlockMass, delegate
				{
					ShowRealBlockMass++;
					ShowRealBlockMass = (ShownMassEnum)((int)ShowRealBlockMass % 4);
					return true;
				});
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "MemA: " + m_memoryA + " MemB: " + m_memoryB + " Diff:" + (m_memoryB - m_memoryA), Diff);
				AddShortcut(MyKeys.NumPad9, newPress: true, control: false, shift: false, alt: false, () => "", delegate
				{
					m_drawBodyInfo = !m_drawBodyInfo;
					m_drawUpdateInfo = !m_drawUpdateInfo;
					return true;
				});
				AddShortcut(MyKeys.NumPad6, newPress: true, control: false, shift: false, alt: false, () => "Prioritize: " + (MyFakes.PRIORITIZE_PRECALC_JOBS ? "On" : "Off"), delegate
				{
					MyFakes.PRIORITIZE_PRECALC_JOBS = !MyFakes.PRIORITIZE_PRECALC_JOBS;
					return true;
				});
				m_dbgComponents.Clear();
				Type[] types = Assembly.GetExecutingAssembly().GetTypes();
				foreach (Type type in types)
				{
					if (!type.IsSubclassOf(typeof(MyRenderComponentBase)) && !type.IsSubclassOf(typeof(MySyncComponentBase)) && type.IsSubclassOf(typeof(MyEntityComponentBase)))
					{
						m_dbgComponents.Add(type);
					}
				}
			}

			private bool Diff()
			{
				foreach (MyEntity entity in MyEntities.GetEntities())
				{
					if ((entity.PositionComp.GetPosition() - MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition()).Length() > 100.0)
					{
						entity.Close();
					}
				}
				return true;
			}

			public override bool HandleInput()
			{
				m_counter++;
				if (base.HandleInput())
				{
					return true;
				}
				bool handled = false;
				if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewLeftMouseReleased())
				{
					Hammer();
				}
				handled = HandleMassMultiplier(handled);
				handled = HandleMemoryDiff(handled);
				if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9))
				{
					MyScriptManager.Static.LoadData();
				}
				if (SelectedEntity != null && MyInput.Static.IsNewKeyPressed(MyKeys.NumPad3))
				{
					object obj = Activator.CreateInstance(m_dbgComponents[m_memoryA]);
					SelectedEntity.Components.Add(m_dbgComponents[m_memoryA], obj as MyComponentBase);
				}
				if (MyAudio.Static != null)
				{
					foreach (IMy3DSoundEmitter item in MyAudio.Static.Get3DSounds())
					{
						MyRenderProxy.DebugDrawSphere(item.SourcePosition, 0.1f, Color.Red, 1f, depthRead: false);
					}
					return handled;
				}
				return handled;
			}

			private static bool HandleMassMultiplier(bool handled)
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad1))
				{
					ApplyMassMultiplier = !ApplyMassMultiplier;
					handled = true;
				}
				int num = 1;
				if (MyInput.Static.IsKeyPress(MyKeys.N))
				{
					num = 10;
				}
				if (MyInput.Static.IsKeyPress(MyKeys.B))
				{
					num = 100;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.OemQuotes))
				{
					if (MassMultiplier > 1f)
					{
						MassMultiplier += num;
					}
					else
					{
						MassMultiplier *= num;
					}
					handled = true;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.OemSemicolon))
				{
					if (MassMultiplier > 1f)
					{
						MassMultiplier -= num;
					}
					else
					{
						MassMultiplier /= num;
					}
					handled = true;
				}
				return handled;
			}

			private void DrawBodyInfo()
			{
				Vector2 screenCoord = new Vector2(400f, 10f);
				MyEntity myEntity = null;
				HkRigidBody hkRigidBody = null;
				if (SelectedEntity != null && SelectedEntity.Physics != null)
				{
					hkRigidBody = ((MyEntity)SelectedEntity).Physics.RigidBody;
				}
				if (MySector.MainCamera != null && hkRigidBody == null)
				{
					List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
					MyPhysics.CastRay(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 100f, list);
					foreach (MyPhysics.HitInfo item in list)
					{
						hkRigidBody = item.HkHitInfo.Body;
						if (!(hkRigidBody == null) && hkRigidBody.Layer != 19)
						{
							myEntity = (item.HkHitInfo.GetHitEntity() as MyEntity);
							StringBuilder stringBuilder = new StringBuilder("ShapeKeys: ");
							int num = 0;
							while (true)
							{
								int num2 = num;
								HkHitInfo hkHitInfo = item.HkHitInfo;
								if (num2 >= hkHitInfo.ShapeKeyCount)
								{
									break;
								}
								hkHitInfo = item.HkHitInfo;
								uint shapeKey = hkHitInfo.GetShapeKey(num);
								if (shapeKey == uint.MaxValue)
								{
									break;
								}
								stringBuilder.Append($"{shapeKey} ");
								num++;
							}
							MyRenderProxy.DebugDrawText2D(screenCoord, stringBuilder.ToString(), Color.White, 0.7f);
							screenCoord.Y += 20f;
							if (myEntity != null && myEntity.GetPhysicsBody() != null)
							{
								MyRenderProxy.DebugDrawText2D(screenCoord, $"Weld: {myEntity.GetPhysicsBody().WeldInfo.Children.Count}", Color.White, 0.7f);
							}
							screenCoord.Y += 20f;
							break;
						}
					}
				}
				if (hkRigidBody != null && m_drawBodyInfo)
				{
					MyEntity myEntity2 = null;
					MyPhysicsBody myPhysicsBody = (MyPhysicsBody)hkRigidBody.UserObject;
					if (myPhysicsBody != null)
					{
						myEntity2 = (MyEntity)myPhysicsBody.Entity;
					}
					if (hkRigidBody.GetBody() != null)
					{
						MyRenderProxy.DebugDrawText2D(screenCoord, $"Name: {hkRigidBody.GetBody().Entity.DisplayName}", Color.White, 0.7f);
					}
					screenCoord.Y += 20f;
					uint collisionFilterInfo = hkRigidBody.GetCollisionFilterInfo();
					int layerFromFilterInfo = HkGroupFilter.GetLayerFromFilterInfo(collisionFilterInfo);
					int systemGroupFromFilterInfo = HkGroupFilter.GetSystemGroupFromFilterInfo(collisionFilterInfo);
					int subSystemIdFromFilterInfo = HkGroupFilter.GetSubSystemIdFromFilterInfo(collisionFilterInfo);
					int subSystemDontCollideWithFromFilterInfo = HkGroupFilter.getSubSystemDontCollideWithFromFilterInfo(collisionFilterInfo);
					MyRenderProxy.DebugDrawText2D(screenCoord, $"Layer: {layerFromFilterInfo}  Group: {systemGroupFromFilterInfo} SubSys: {subSystemIdFromFilterInfo} SubSysDont: {subSystemDontCollideWithFromFilterInfo} ", (layerFromFilterInfo == 0) ? Color.Red : Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "ShapeType: " + hkRigidBody.GetShape().ShapeType, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Mass: " + hkRigidBody.Mass, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Friction: " + hkRigidBody.Friction, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Restitution: " + hkRigidBody.Restitution, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "LinDamping: " + hkRigidBody.LinearDamping, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "AngDamping: " + hkRigidBody.AngularDamping, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "PenetrationDepth: " + hkRigidBody.AllowedPenetrationDepth, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Lin: " + hkRigidBody.LinearVelocity.Length(), Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Ang: " + hkRigidBody.AngularVelocity.Length(), Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Act: " + (hkRigidBody.IsActive ? "true" : "false"), Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Stat: " + (hkRigidBody.IsFixedOrKeyframed ? "true" : "false"), Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Solver: " + hkRigidBody.Motion.GetDeactivationClass(), Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Mass: " + hkRigidBody.Mass, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "MotionType: " + hkRigidBody.GetMotionType(), Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "QualityType: " + hkRigidBody.Quality, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "DeactCtr0: " + hkRigidBody.DeactivationCounter0, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "DeactCtr1: " + hkRigidBody.DeactivationCounter1, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "EntityId: " + myEntity2.EntityId, Color.White, 0.7f);
					screenCoord.Y += 20f;
				}
				if (SelectedEntity != null && m_drawUpdateInfo)
				{
					MyRenderProxy.DebugDrawText2D(screenCoord, "Updates: " + m_counter, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "PositionUpd: " + dbgPosCounter, Color.White, 0.7f);
					screenCoord.Y += 20f;
					MyRenderProxy.DebugDrawText2D(screenCoord, "Frames per update: " + (float)m_counter / (float)dbgPosCounter, Color.White, 0.7f);
					screenCoord.Y += 20f;
				}
			}

			private bool HandleMemoryDiff(bool handled)
			{
				if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.PageUp))
				{
					m_memoryA--;
					handled = true;
				}
				if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.PageDown))
				{
					m_memoryA++;
					handled = true;
				}
				m_memoryA = (m_memoryA + m_dbgComponents.Count) % m_dbgComponents.Count;
				return handled;
			}

			private void Hammer()
			{
				Vector3D position = MySector.MainCamera.Position;
				Vector3 forwardVector = MySector.MainCamera.ForwardVector;
				LineD lineD = new LineD(position, position + forwardVector * 200f);
				List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
				MyPhysics.CastRay(lineD.From, lineD.To, list, 24);
				list.RemoveAll((MyPhysics.HitInfo hit) => hit.HkHitInfo.GetHitEntity() == MySession.Static.ControlledEntity.Entity);
				if (list.Count == 0)
				{
					return;
				}
				MyEntity myEntity = null;
				MyPhysics.HitInfo hitInfo = default(MyPhysics.HitInfo);
				foreach (MyPhysics.HitInfo item in list)
				{
					if (item.HkHitInfo.Body != null)
					{
						myEntity = (item.HkHitInfo.GetHitEntity() as MyEntity);
						hitInfo = item;
						break;
					}
				}
				if (myEntity != null)
				{
					HkdFractureImpactDetails hkdFractureImpactDetails = HkdFractureImpactDetails.Create();
					hkdFractureImpactDetails.SetBreakingBody(myEntity.Physics.RigidBody);
					hkdFractureImpactDetails.SetContactPoint(myEntity.Physics.WorldToCluster(hitInfo.Position));
					hkdFractureImpactDetails.SetDestructionRadius(RADIUS);
					hkdFractureImpactDetails.SetBreakingImpulse(MyDestructionConstants.STRENGTH * 10f);
					if (HammerForce)
					{
						hkdFractureImpactDetails.SetParticleVelocity(-lineD.Direction * 20.0);
					}
					hkdFractureImpactDetails.SetParticlePosition(myEntity.Physics.WorldToCluster(hitInfo.Position));
					hkdFractureImpactDetails.SetParticleMass(1000000f);
					hkdFractureImpactDetails.Flag |= HkdFractureImpactDetails.Flags.FLAG_DONT_RECURSE;
					if (myEntity.GetPhysicsBody().HavokWorld.DestructionWorld != null)
					{
						MyPhysics.FractureImpactDetails details = default(MyPhysics.FractureImpactDetails);
						details.Details = hkdFractureImpactDetails;
						details.World = myEntity.GetPhysicsBody().HavokWorld;
						details.Entity = myEntity;
						MyPhysics.EnqueueDestruction(details);
					}
				}
			}

			private static bool SpawnBreakable(bool handled)
			{
				return handled;
			}

			public override void Draw()
			{
				base.Draw();
				Vector2 vector = new Vector2(600f, 100f);
				foreach (Type dbgComponent in m_dbgComponents)
				{
					if (SelectedEntity != null)
					{
						SelectedEntity.Components.Contains(dbgComponent);
					}
					else
						_ = 0;
					vector.Y += 10f;
				}
				vector = new Vector2(580f, 100 + 10 * m_memoryA);
				if (SelectedEntity != null)
				{
					BoundingBoxD box = new BoundingBoxD(SelectedEntity.PositionComp.LocalAABB.Min, SelectedEntity.PositionComp.LocalAABB.Max);
					new MyOrientedBoundingBoxD(box, SelectedEntity.WorldMatrix);
					MyRenderProxy.DebugDrawAABB(SelectedEntity.PositionComp.WorldAABB, Color.White, 1f, 1f, depthRead: false);
				}
				DrawBodyInfo();
			}

			private void TestParallelBatch()
			{
				Parallel.For(0, 10, delegate
				{
					int[] RunJournal = new int[1000];
					DependencyBatch dependencyBatch = new DependencyBatch(WorkPriority.VeryHigh);
					dependencyBatch.Preallocate(1500);
					int id = default(int);
					for (int i = 0; i < 1000; i++)
					{
						id = i;
						dependencyBatch.Add(delegate
						{
							Thread.Sleep(TimeSpan.FromMilliseconds(5 + MyRandom.Instance.Next(10)));
							if (id > 0 && id != 999)
							{
								int num = (id - 1) / 2 * 2;
								Interlocked.Exchange(ref RunJournal[num], 1);
							}
							Interlocked.Exchange(ref RunJournal[id], 1);
						});
					}
					for (int j = 0; j < 997; j += 2)
					{
						using (DependencyBatch.StartToken startToken = dependencyBatch.Job(j))
						{
							startToken.Starts(j + 1);
							startToken.Starts(j + 2);
						}
					}
					dependencyBatch.Execute();
					int[] array = RunJournal;
					for (int k = 0; k < array.Length; k++)
					{
						_ = array[k];
					}
				});
			}
		}

		public class LiveWatchComponent : MyDebugComponent
		{
			private int MAX_HISTORY = 10000;

			private object m_currentInstance;

			private Type m_selectedType;

			private Type m_lastType;

			private readonly List<MemberInfo> m_members = new List<MemberInfo>();

			private readonly List<MemberInfo> m_currentPath = new List<MemberInfo>();

			private readonly Dictionary<Type, MyListDictionary<MemberInfo, MemberInfo>> m_watch = new Dictionary<Type, MyListDictionary<MemberInfo, MemberInfo>>();

			private List<List<object>> m_history = new List<List<object>>();

			private bool m_showWatch;

			private bool m_showOnScreenWatch;

			private float m_scale = 2f;

			private HashSet<int> m_toPlot = new HashSet<int>();

			private int m_frame;

			protected static Color[] m_colors = new Color[19]
			{
				new Color(0, 192, 192),
				Color.Orange,
				Color.BlueViolet * 1.5f,
				Color.BurlyWood,
				Color.Chartreuse,
				Color.CornflowerBlue,
				Color.Cyan,
				Color.ForestGreen,
				Color.Fuchsia,
				Color.Gold,
				Color.GreenYellow,
				Color.LightBlue,
				Color.LightGreen,
				Color.LimeGreen,
				Color.Magenta,
				Color.MintCream,
				Color.Orchid,
				Color.PeachPuff,
				Color.Purple
			};

			private int SelectedMember
			{
				get
				{
					int val = (int)((float)m_counter * 0.005f);
					if (m_showWatch)
					{
						if (m_watch.ContainsKey(m_selectedType))
						{
							return Math.Min(Math.Max(val, 0), m_watch[m_selectedType].Values.Count - 1);
						}
						return 0;
					}
					return Math.Min(Math.Max(val, 0), m_members.Count - 1);
				}
			}

			public LiveWatchComponent()
			{
				OnSelectedEntityChanged += MyHonzaInputComponent_OnSelectedEntityChanged;
				AddSwitch(MyKeys.NumPad8, delegate
				{
					m_showOnScreenWatch = !m_showOnScreenWatch;
					return true;
				}, new MyRef<bool>(() => m_showOnScreenWatch, null), "External viewer");
			}

			private void MyHonzaInputComponent_OnSelectedEntityChanged()
			{
				if (SelectedEntity != null && !(m_selectedType == SelectedEntity.GetType()))
				{
					m_selectedType = SelectedEntity.GetType();
					m_members.Clear();
					m_currentPath.Clear();
				}
			}

			public override string GetName()
			{
				return "LiveWatch";
			}

			public override bool HandleInput()
			{
				return base.HandleInput();
			}

			public override void Draw()
			{
				base.Draw();
				if (SelectedEntity == null || !m_showOnScreenWatch)
				{
					return;
				}
				MyListDictionary<MemberInfo, MemberInfo> value = null;
				m_watch.TryGetValue(m_selectedType, out value);
				if (m_showWatch)
				{
					DrawWatch(value);
					return;
				}
				StringBuilder stringBuilder = new StringBuilder(SelectedEntity.GetType().Name);
				Type type = m_selectedType;
				m_currentInstance = SelectedEntity;
				foreach (MemberInfo item in m_currentPath)
				{
					stringBuilder.Append(".");
					stringBuilder.Append(item.Name);
					m_currentInstance = item.GetValue(m_currentInstance);
					type = m_currentInstance.GetType();
				}
				if (type != m_lastType)
				{
					m_lastType = type;
					m_members.Clear();
					MemberInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					fields = fields;
					foreach (MemberInfo memberInfo in fields)
					{
						if (memberInfo.DeclaringType == type)
						{
							m_members.Add(memberInfo);
						}
					}
					fields = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					fields = fields;
					foreach (MemberInfo memberInfo2 in fields)
					{
						if (memberInfo2.DeclaringType == type)
						{
							m_members.Add(memberInfo2);
						}
					}
					m_members.Sort((MemberInfo x, MemberInfo y) => string.Compare(x.Name, y.Name));
				}
				Vector2 screenCoord = new Vector2(100f, 50f);
				MyRenderProxy.DebugDrawText2D(screenCoord, stringBuilder.ToString(), Color.White, 0.65f);
				screenCoord.Y += 20f;
				for (int j = SelectedMember; j < m_members.Count; j++)
				{
					object value2 = m_members[j].GetValue(m_currentInstance);
					((value2 != null) ? value2.ToString() : "null").Replace("\n", "");
					screenCoord.Y += 12f;
				}
			}

			private void DrawWatch(MyListDictionary<MemberInfo, MemberInfo> watch)
			{
				PlotHistory();
				if (watch != null)
				{
					List<object> list = new CacheList<object>(watch.Values.Count);
					StringBuilder stringBuilder = new StringBuilder();
					Vector2 screenCoord = new Vector2(100f, 50f);
					int num = -1;
					foreach (List<MemberInfo> value in watch.Values)
					{
						num++;
						if (num >= SelectedMember)
						{
							object obj = SelectedEntity;
							foreach (MemberInfo item in value)
							{
								stringBuilder.Append(".");
								stringBuilder.Append(item.Name);
								obj = item.GetValue(obj);
							}
							stringBuilder.Append(":");
							stringBuilder.Append(obj.ToString());
							MyRenderProxy.DebugDrawText2D(screenCoord, stringBuilder.ToString(), m_toPlot.Contains(num) ? m_colors[num] : Color.White, 0.55f);
							screenCoord.Y += 12f;
							stringBuilder.Clear();
							list.Add(obj);
						}
					}
					screenCoord.X = 90f;
					foreach (int item2 in m_toPlot)
					{
						int num2 = item2 - SelectedMember;
						if (num2 >= 0)
						{
							screenCoord.Y = 50 + num2 * 12;
							MyRenderProxy.DebugDrawText2D(screenCoord, "*", m_colors[item2], 0.55f);
						}
					}
					m_history.Add(list);
					if (m_history.Count >= MAX_HISTORY)
					{
						m_history.RemoveAtFast(m_frame);
					}
					m_frame++;
					m_frame %= MAX_HISTORY;
				}
			}

			private void PlotHistory()
			{
				int num = 0;
				Vector2 vector = new Vector2(100f, 400f);
				Vector2 pointTo = vector;
				pointTo.X += 1f;
				MyRenderProxy.DebugDrawLine2D(new Vector2(vector.X, vector.Y - 200f), new Vector2(vector.X + 1000f, vector.Y - 200f), Color.Gray, Color.Gray);
				MyRenderProxy.DebugDrawLine2D(new Vector2(vector.X, vector.Y + 200f), new Vector2(vector.X + 1000f, vector.Y + 200f), Color.Gray, Color.Gray);
				MyRenderProxy.DebugDrawLine2D(new Vector2(vector.X, vector.Y), new Vector2(vector.X + 1000f, vector.Y), Color.Gray, Color.Gray);
				MyRenderProxy.DebugDrawText2D(new Vector2(90f, 200f), (200f / m_scale).ToString(), Color.White, 0.55f, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
				MyRenderProxy.DebugDrawText2D(new Vector2(90f, 600f), (-200f / m_scale).ToString(), Color.White, 0.55f, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
				for (int num2 = Math.Min(1000, m_history.Count); num2 > 0; num2--)
				{
					int num3 = (m_frame + m_history.Count - num2) % m_history.Count;
					List<object> list = m_history[num3];
					List<object> list2 = m_history[(num3 + 1) % m_history.Count];
					num++;
					foreach (int item in m_toPlot)
					{
						if (list.Count > item && list2.Count > item)
						{
							object obj = list[item];
							if (obj.GetType().IsPrimitive)
							{
								float num4 = ConvertToFloat(obj);
								float num5 = ConvertToFloat(list2[item]);
								vector.Y = 400f - num4 * m_scale;
								pointTo.Y = 400f - num5 * m_scale;
								if (num == 1)
								{
									vector.Y = pointTo.Y;
								}
								if (num2 < 3)
								{
									pointTo.Y = vector.Y;
								}
								MyRenderProxy.DebugDrawLine2D(vector, pointTo, m_colors[item], m_colors[item]);
							}
						}
					}
					vector.X += 1f;
					pointTo.X += 1f;
				}
			}

			private static float ConvertToFloat(object o)
			{
				float result = float.NaN;
				int? num = o as int?;
				if (num.HasValue)
				{
					result = num.Value;
				}
				float? num2 = o as float?;
				if (num2.HasValue)
				{
					result = num2.Value;
				}
				double? num3 = o as double?;
				if (num3.HasValue)
				{
					result = (float)num3.Value;
				}
				return result;
			}
		}

		public class PhysicsComponent : MyDebugComponent
		{
			public PhysicsComponent()
			{
				AddShortcut(MyKeys.W, newPress: true, control: true, shift: false, alt: false, () => "Debug Draw", delegate
				{
					MyDebugDrawSettings.ENABLE_DEBUG_DRAW = !MyDebugDrawSettings.ENABLE_DEBUG_DRAW;
					return true;
				});
				AddShortcut(MyKeys.Q, newPress: true, control: true, shift: false, alt: false, () => "Draw Physics Shapes", delegate
				{
					MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = true;
					MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES = !MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES;
					return true;
				});
				AddShortcut(MyKeys.C, newPress: true, control: true, shift: false, alt: false, () => "Draw Physics Constraints", delegate
				{
					MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = true;
					MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS = !MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS;
					return false;
				});
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Wheel multiplier x1.5x: " + MyPhysicsConfig.WheelSlipCutAwayRatio.ToString("F2"), delegate
				{
					MyPhysicsConfig.WheelSlipCutAwayRatio *= 1.5f;
					return true;
				});
				AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Wheel multiplier /1.5x: " + MyPhysicsConfig.WheelSlipCutAwayRatio.ToString("F2"), delegate
				{
					MyPhysicsConfig.WheelSlipCutAwayRatio /= 1.5f;
					return true;
				});
			}

			public override string GetName()
			{
				return "Physics";
			}
		}

		private static IMyEntity m_selectedEntity;

		private static long m_counter;

		public static long dbgPosCounter;

		private MyDebugComponent[] m_components;

		public static IMyEntity SelectedEntity
		{
			get
			{
				return m_selectedEntity;
			}
			set
			{
				if (m_selectedEntity != value)
				{
					m_selectedEntity = value;
					m_counter = (dbgPosCounter = 0L);
					if (MyHonzaInputComponent.OnSelectedEntityChanged != null)
					{
						MyHonzaInputComponent.OnSelectedEntityChanged();
					}
				}
			}
		}

		public override MyDebugComponent[] Components => m_components;

		public static event Action OnSelectedEntityChanged;

		public override string GetName()
		{
			return "Honza";
		}

		static MyHonzaInputComponent()
		{
		}

		public override bool HandleInput()
		{
			bool result = base.HandleInput();
			HandleEntitySelect();
			return result;
		}

		private void HandleEntitySelect()
		{
			if (!MyInput.Static.IsAnyShiftKeyPressed() || !MyInput.Static.IsNewLeftMousePressed())
			{
				return;
			}
			if (SelectedEntity != null && SelectedEntity.Physics != null)
			{
				if (SelectedEntity is MyCubeGrid)
				{
					HkGridShape hkGridShape = (HkGridShape)((MyPhysicsBody)SelectedEntity.Physics).GetShape();
					hkGridShape.DebugDraw = false;
				}
				((MyEntity)SelectedEntity).ClearDebugRenderComponents();
				SelectedEntity = null;
			}
			else if (MySector.MainCamera != null)
			{
				List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
				MyPhysics.CastRay(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 100f, list);
				foreach (MyPhysics.HitInfo item in list)
				{
					HkRigidBody body = item.HkHitInfo.Body;
					if (!(body == null) && body.Layer != 19)
					{
						SelectedEntity = item.HkHitInfo.GetHitEntity();
						if (SelectedEntity is MyCubeGrid)
						{
							HkGridShape hkGridShape2 = (HkGridShape)((MyPhysicsBody)SelectedEntity.Physics).GetShape();
							hkGridShape2.DebugRigidBody = body;
							hkGridShape2.DebugDraw = true;
						}
						break;
					}
				}
			}
		}

		public MyHonzaInputComponent()
		{
			m_components = new MyDebugComponent[3]
			{
				new DefaultComponent(),
				new PhysicsComponent(),
				new LiveWatchComponent()
			};
		}
	}
}
