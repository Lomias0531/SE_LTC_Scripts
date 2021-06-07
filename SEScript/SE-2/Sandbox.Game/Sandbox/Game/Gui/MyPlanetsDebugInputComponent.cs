using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	public class MyPlanetsDebugInputComponent : MyMultiDebugInputComponent
	{
		private class InfoComponent : MyDebugComponent
		{
			private MyPlanetsDebugInputComponent m_comp;

			private Vector3 m_lastCameraPosition = Vector3.Invalid;

			private Queue<float> m_speeds = new Queue<float>(60);

			public InfoComponent(MyPlanetsDebugInputComponent comp)
			{
				m_comp = comp;
			}

			public override void Draw()
			{
				base.Draw();
				if (MySession.Static == null || m_comp.CameraPlanet == null)
				{
					return;
				}
				MyPlanetStorageProvider provider = m_comp.CameraPlanet.Provider;
				if (provider == null)
				{
					return;
				}
				Vector3 vector = MySector.MainCamera.Position;
				float num = 0f;
				float num2 = 0f;
				if (m_lastCameraPosition.IsValid())
				{
					num = (vector - m_lastCameraPosition).Length() * 60f;
					if (m_speeds.Count == 60)
					{
						m_speeds.Dequeue();
					}
					m_speeds.Enqueue(num);
					foreach (float speed in m_speeds)
					{
						num2 += speed;
					}
					num2 /= (float)m_speeds.Count;
				}
				m_lastCameraPosition = vector;
				Vector3 value = vector;
				value -= m_comp.CameraPlanet.PositionLeftBottomCorner;
				provider.ComputeCombinedMaterialAndSurfaceExtended(value, out MyPlanetStorageProvider.SurfacePropertiesExtended props);
				Section("Position");
				Text("Position: {0}", props.Position);
				Text("Speed: {0:F2}ms -- {1:F2}m/s", num, num2);
				Text("Latitude: {0}", MathHelper.ToDegrees((float)Math.Asin(props.Latitude)));
				Text("Longitude: {0}", MathHelper.ToDegrees(MathHelper.MonotonicAcos(props.Longitude)));
				Text("Altitude: {0}", props.Altitude);
				VSpace(5f);
				Text("Height: {0}", props.Depth);
				Text("HeightRatio: {0}", props.HeightRatio);
				Text("Slope: {0}", MathHelper.ToDegrees((float)Math.Acos(props.Slope)));
				Text("Air Density: {0}", m_comp.CameraPlanet.GetAirDensity(vector));
				Text("Oxygen: {0}", m_comp.CameraPlanet.GetOxygenForPosition(vector));
				Section("Cube Position");
				Text("Face: {0}", MyCubemapHelpers.GetNameForFace(props.Face));
				Text("Texcoord: {0}", props.Texcoord);
				Text("Texcoord Position: {0}", (Vector2I)(props.Texcoord * 2048f));
				Section("Material");
				Text("Material: {0}", (props.Material != null) ? props.Material.Id.SubtypeName : "null");
				Text("Material Origin: {0}", props.Origin);
				Text("Biome: {0}", (props.Biome != null) ? props.Biome.Name : "");
				MultilineText("EffectiveRule: {0}", props.EffectiveRule);
				Text("Ore: {0}", props.Ore);
				Section("Map values");
				Text("BiomeValue: {0}", props.BiomeValue);
				Text("MaterialValue: {0}", props.MaterialValue);
				Text("OreValue: {0}", props.OreValue);
			}

			public override string GetName()
			{
				return "Info";
			}
		}

		private class MiscComponent : MyDebugComponent
		{
			private MyPlanetsDebugInputComponent m_comp;

			private Vector3 m_lastCameraPosition = Vector3.Invalid;

			private Queue<float> m_speeds = new Queue<float>(60);

			public MiscComponent(MyPlanetsDebugInputComponent comp)
			{
				m_comp = comp;
			}

			public override void Draw()
			{
				base.Draw();
				if (MySession.Static == null)
				{
					return;
				}
				Text("Game time: {0}", MySession.Static.ElapsedGameTime);
				Vector3 vector = MySector.MainCamera.Position;
				float num = 0f;
				float num2 = 0f;
				if (m_lastCameraPosition.IsValid())
				{
					num = (vector - m_lastCameraPosition).Length() * 60f;
					if (m_speeds.Count == 60)
					{
						m_speeds.Dequeue();
					}
					m_speeds.Enqueue(num);
					foreach (float speed in m_speeds)
					{
						num2 += speed;
					}
					num2 /= (float)m_speeds.Count;
				}
				m_lastCameraPosition = vector;
				Section("Controlled Entity/Camera");
				Text("Speed: {0:F2}ms -- {1:F2}m/s", num, num2);
				if (MySession.Static.LocalHumanPlayer != null && MySession.Static.LocalHumanPlayer.Controller.ControlledEntity != null)
				{
					MyEntity myEntity = (MyEntity)MySession.Static.LocalHumanPlayer.Controller.ControlledEntity;
					if (myEntity is MyCubeBlock)
					{
						myEntity = ((MyCubeBlock)myEntity).CubeGrid;
					}
					StringBuilder stringBuilder = new StringBuilder();
					if (myEntity.Physics != null)
					{
						stringBuilder.Clear();
						stringBuilder.Append("Mass: ");
						MyValueFormatter.AppendWeightInBestUnit(myEntity.Physics.Mass, stringBuilder);
						Text(stringBuilder.ToString());
					}
					if (myEntity.Components.TryGet(out MyEntityThrustComponent component))
					{
						stringBuilder.Clear();
						stringBuilder.Append("Current Thrust: ");
						MyValueFormatter.AppendForceInBestUnit(component.FinalThrust.Length(), stringBuilder);
						stringBuilder.AppendFormat(" : {0}", component.FinalThrust);
						Text(stringBuilder.ToString());
					}
				}
			}

			public override string GetName()
			{
				return "Misc";
			}
		}

		private class SectorsComponent : MyDebugComponent
		{
			private MyPlanetsDebugInputComponent m_comp;

			private bool m_updateRange = true;

			private Vector3D m_center;

			private double m_radius;

			private double m_height;

			private QuaternionD m_orientation;

			public SectorsComponent(MyPlanetsDebugInputComponent comp)
			{
				m_comp = comp;
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Toggle update range", () => m_updateRange = !m_updateRange);
			}

			private bool ToggleSectors()
			{
				MyPlanet.RUN_SECTORS = !MyPlanet.RUN_SECTORS;
				return true;
			}

			public override void Draw()
			{
				base.Draw();
				MyPlanet cameraPlanet = m_comp.CameraPlanet;
				if (cameraPlanet == null)
				{
					return;
				}
				MyPlanetEnvironmentComponent myPlanetEnvironmentComponent = cameraPlanet.Components.Get<MyPlanetEnvironmentComponent>();
				if (myPlanetEnvironmentComponent == null)
				{
					return;
				}
				bool flag = false;
				MyEnvironmentSector activeSector = MyPlanetEnvironmentSessionComponent.ActiveSector;
				if (activeSector != null && activeSector.DataView != null)
				{
					List<MyLogicalEnvironmentSectorBase> logicalSectors = activeSector.DataView.LogicalSectors;
					Text(Color.White, 1.5f, "Current sector: {0}", activeSector.ToString());
					Text("Storage sectors:");
					foreach (MyLogicalEnvironmentSectorBase item in logicalSectors)
					{
						Text("   {0}", item.DebugData);
					}
				}
				Text("Horizon Distance: {0}", m_radius);
				if (m_updateRange)
				{
					UpdateViewRange(cameraPlanet);
				}
				IMyEnvironmentDataProvider[] providers = myPlanetEnvironmentComponent.Providers;
				for (int i = 0; i < providers.Length; i++)
				{
					MyLogicalEnvironmentSectorBase[] array = providers[i].LogicalSectors.ToArray();
					if (array.Length != 0 && !flag)
					{
						flag = true;
						Text(Color.Yellow, 1.5f, "Synchronized:");
					}
					MyLogicalEnvironmentSectorBase[] array2 = array;
					foreach (MyLogicalEnvironmentSectorBase myLogicalEnvironmentSectorBase in array2)
					{
						if (myLogicalEnvironmentSectorBase != null && myLogicalEnvironmentSectorBase.ServerOwned)
						{
							Text("Sector {0}", myLogicalEnvironmentSectorBase.ToString());
						}
					}
				}
				Text("Physics");
				foreach (MyEnvironmentSector value in cameraPlanet.Components.Get<MyPlanetEnvironmentComponent>().PhysicsSectors.Values)
				{
					Text(Color.White, 0.8f, "Sector {0}", value.ToString());
				}
				Text("Graphics");
				foreach (MyPlanetEnvironmentClipmapProxy value2 in cameraPlanet.Components.Get<MyPlanetEnvironmentComponent>().Proxies.Values)
				{
					if (value2.EnvironmentSector != null)
					{
						Text(Color.White, 0.8f, "Sector {0}", value2.EnvironmentSector.ToString());
					}
				}
				MyRenderProxy.DebugDrawCylinder(m_center, m_orientation, (float)m_radius, m_height, Color.Orange, 1f, depthRead: true, smooth: false);
			}

			private void UpdateViewRange(MyPlanet planet)
			{
				Vector3D point = MySector.MainCamera.Position;
				double num = double.MaxValue;
				foreach (MyPlanet planet2 in MyPlanets.GetPlanets())
				{
					double num2 = Vector3D.DistanceSquared(point, planet2.WorldMatrix.Translation);
					if (num2 < num)
					{
						planet = planet2;
						num = num2;
					}
				}
				float minimumRadius = planet.MinimumRadius;
				m_height = planet.MaximumRadius - minimumRadius;
				Vector3D center = planet.WorldMatrix.Translation;
				m_radius = HyperSphereHelpers.DistanceToTangentProjected(ref center, ref point, minimumRadius, out double distance);
				Vector3D vector3D = center - point;
				vector3D.Normalize();
				m_center = point + vector3D * distance;
				Vector3D forward = Vector3D.CalculatePerpendicularVector(vector3D);
				m_orientation = QuaternionD.CreateFromForwardUp(forward, vector3D);
			}

			private string FormatWorkTracked(Vector4I workStats)
			{
				return $"{workStats.X:D3}/{workStats.Y:D3}/{workStats.Z:D3}/{workStats.W:D3}";
			}

			public override string GetName()
			{
				return "Sectors";
			}
		}

		private class SectorTreeComponent : MyDebugComponent, IMy2DClipmapManager
		{
			[StructLayout(LayoutKind.Sequential, Size = 1)]
			private struct DebugDrawSorter : IComparer<DebugDrawHandler>
			{
				public int Compare(DebugDrawHandler x, DebugDrawHandler y)
				{
					return x.Lod - y.Lod;
				}
			}

			private class DebugDrawHandler : IMy2DClipmapNodeHandler
			{
				private SectorTreeComponent m_parent;

				public Vector2I Coords;

				public BoundingBoxD Bounds;

				public int Lod;

				public Vector3D[] FrustumBounds;

				public void Init(IMy2DClipmapManager parent, int x, int y, int lod, ref BoundingBox2D bounds)
				{
					m_parent = (SectorTreeComponent)parent;
					Bounds = new BoundingBoxD(new Vector3D(bounds.Min, 0.0), new Vector3D(bounds.Max, 50.0));
					Lod = lod;
					MatrixD worldMatrix = m_parent.m_tree[m_parent.m_activeClipmap].WorldMatrix;
					Bounds = Bounds.TransformFast(worldMatrix);
					Coords = new Vector2I(x, y);
					m_parent.m_handlers.Add(this);
					_ = Bounds.Center;
					Vector3D[] array = new Vector3D[8]
					{
						Vector3D.Transform(new Vector3D(bounds.Min.X, bounds.Min.Y, 0.0), worldMatrix),
						Vector3D.Transform(new Vector3D(bounds.Max.X, bounds.Min.Y, 0.0), worldMatrix),
						Vector3D.Transform(new Vector3D(bounds.Min.X, bounds.Max.Y, 0.0), worldMatrix),
						Vector3D.Transform(new Vector3D(bounds.Max.X, bounds.Max.Y, 0.0), worldMatrix),
						default(Vector3D),
						default(Vector3D),
						default(Vector3D),
						default(Vector3D)
					};
					for (int i = 0; i < 4; i++)
					{
						array[i].Normalize();
						array[i + 4] = array[i] * m_parent.Radius;
						array[i] *= m_parent.Radius + 50.0;
					}
					FrustumBounds = array;
				}

				public void Close()
				{
					m_parent.m_handlers.Remove(this);
				}

				public void InitJoin(IMy2DClipmapNodeHandler[] children)
				{
					DebugDrawHandler debugDrawHandler = (DebugDrawHandler)children[0];
					Lod = debugDrawHandler.Lod + 1;
					Coords = new Vector2I(debugDrawHandler.Coords.X >> 1, debugDrawHandler.Coords.Y >> 1);
					m_parent.m_handlers.Add(this);
				}

				public unsafe void Split(BoundingBox2D* childBoxes, ref IMy2DClipmapNodeHandler[] children)
				{
					for (int i = 0; i < 4; i++)
					{
						children[i].Init(m_parent, (Coords.X << 1) + (i & 1), (Coords.Y << 1) + ((i >> 1) & 1), Lod - 1, ref childBoxes[i]);
					}
				}
			}

			private MyPlanetsDebugInputComponent m_comp;

			private readonly HashSet<DebugDrawHandler> m_handlers = new HashSet<DebugDrawHandler>();

			private List<DebugDrawHandler> m_sortedHandlers = new List<DebugDrawHandler>();

			private My2DClipmap<DebugDrawHandler>[] m_tree;

			private int m_allocs;

			private int m_activeClipmap;

			private Vector3D Origin = Vector3D.Zero;

			private double Radius = 60000.0;

			private double Size;

			private bool m_update = true;

			private Vector3D m_lastUpdate;

			private int m_activeFace;

			public SectorTreeComponent(MyPlanetsDebugInputComponent comp)
			{
				Size = Radius * Math.Sqrt(2.0);
				double sectorSize = 64.0;
				m_comp = comp;
				m_tree = new My2DClipmap<DebugDrawHandler>[6];
				for (m_activeClipmap = 0; m_activeClipmap < m_tree.Length; m_activeClipmap++)
				{
					Vector3 vector = Base6Directions.Directions[m_activeClipmap];
					Vector3 v = Base6Directions.Directions[(uint)Base6Directions.GetPerpendicular((Base6Directions.Direction)m_activeClipmap)];
					MatrixD worldMatrix = MatrixD.CreateFromDir(-vector, v);
					worldMatrix.Translation = (Vector3D)vector * Size / 2.0;
					m_tree[m_activeClipmap] = new My2DClipmap<DebugDrawHandler>();
					m_tree[m_activeClipmap].Init(this, ref worldMatrix, sectorSize, Size);
				}
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Toggle clipmap update", () => m_update = !m_update);
			}

			public override void Update10()
			{
				base.Update10();
				if (MySession.Static == null)
				{
					return;
				}
				if (m_update)
				{
					m_lastUpdate = MySector.MainCamera.Position;
				}
				Vector3D localPos = m_lastUpdate;
				ProjectToCube(ref localPos, out int direction, out Vector2D texcoords);
				m_activeFace = direction;
				Vector3D vector3D = Base6Directions.Directions[direction];
				vector3D.X *= m_lastUpdate.X;
				vector3D.Y *= m_lastUpdate.Y;
				vector3D.Z *= m_lastUpdate.Z;
				double num = Math.Abs(m_lastUpdate.Length() - Radius);
				m_allocs = 0;
				Vector3D localPosition = default(Vector3D);
				for (m_activeClipmap = 0; m_activeClipmap < m_tree.Length; m_activeClipmap++)
				{
					My2DClipmap<DebugDrawHandler> my2DClipmap = m_tree[m_activeClipmap];
					Vector2D newCoords = texcoords;
					MyPlanetCubemapHelper.TranslateTexcoordsToFace(ref texcoords, direction, m_activeClipmap, out newCoords);
					localPosition.X = newCoords.X * my2DClipmap.FaceHalf;
					localPosition.Y = newCoords.Y * my2DClipmap.FaceHalf;
					if ((m_activeClipmap ^ direction) == 1)
					{
						localPosition.Z = num + Radius * 2.0;
					}
					else
					{
						localPosition.Z = num;
					}
					m_tree[m_activeClipmap].NodeAllocDeallocs = 0;
					m_tree[m_activeClipmap].Update(localPosition);
					m_allocs += m_tree[m_activeClipmap].NodeAllocDeallocs;
				}
				m_sortedHandlers = m_handlers.ToList();
				m_sortedHandlers.Sort(default(DebugDrawSorter));
			}

			public override void Draw()
			{
				base.Draw();
				Text("Node Allocs/Deallocs from last update: {0}", m_allocs);
				foreach (DebugDrawHandler sortedHandler in m_sortedHandlers)
				{
					MyRenderProxy.DebugDraw6FaceConvex(sortedHandler.FrustumBounds, new Color(My2DClipmapHelpers.LodColors[sortedHandler.Lod], 1f), 0.2f, depthRead: true, fill: true);
				}
				for (m_activeClipmap = 0; m_activeClipmap < m_tree.Length; m_activeClipmap++)
				{
					My2DClipmap<DebugDrawHandler> my2DClipmap = m_tree[m_activeClipmap];
					Vector3D vector3D = Vector3.Transform(m_tree[m_activeClipmap].LastPosition, m_tree[m_activeClipmap].WorldMatrix);
					MyRenderProxy.DebugDrawSphere(vector3D, 500f, Color.Red);
					MyRenderProxy.DebugDrawText3D(vector3D, ((Base6Directions.Direction)m_activeClipmap).ToString(), Color.Blue, 1f, depthRead: true);
					Vector3D translation = my2DClipmap.WorldMatrix.Translation;
					Vector3D pointTo = Vector3D.Transform(Vector3D.Right * 10000.0, my2DClipmap.WorldMatrix);
					Vector3D pointTo2 = Vector3D.Transform(Vector3D.Up * 10000.0, my2DClipmap.WorldMatrix);
					MyRenderProxy.DebugDrawText3D(translation, ((Base6Directions.Direction)m_activeClipmap).ToString(), Color.Blue, 1f, depthRead: true);
					MyRenderProxy.DebugDrawLine3D(translation, pointTo2, Color.Green, Color.Green, depthRead: true);
					MyRenderProxy.DebugDrawLine3D(translation, pointTo, Color.Red, Color.Red, depthRead: true);
				}
			}

			public override string GetName()
			{
				return "Sector Tree";
			}
		}

		private class ShapeComponent : MyDebugComponent
		{
			private MyPlanetsDebugInputComponent m_comp;

			public ShapeComponent(MyPlanetsDebugInputComponent comp)
			{
				m_comp = comp;
			}

			public override void Update100()
			{
				base.Update100();
				MyPlanetShapeProvider.PruningStats.CycleWork();
				MyPlanetShapeProvider.CacheStats.CycleWork();
				MyPlanetShapeProvider.CullStats.CycleWork();
			}

			public override void Draw()
			{
				Text("Planet Shape request culls: {0}", MyPlanetShapeProvider.CullStats.History);
				Text("Planet Shape coefficient cache hits: {0}", MyPlanetShapeProvider.CacheStats.History);
				Text("Planet Shape pruning tree hits: {0}", MyPlanetShapeProvider.PruningStats.History);
				base.Draw();
			}

			public override string GetName()
			{
				return "Shape";
			}
		}

		private MyDebugComponent[] m_components;

		private List<MyVoxelBase> m_voxels = new List<MyVoxelBase>();

		public MyPlanet CameraPlanet;

		public MyPlanet CharacterPlanet;

		private static uint[] AdjacentFaceTransforms = new uint[36]
		{
			0u,
			0u,
			0u,
			16u,
			10u,
			26u,
			0u,
			0u,
			16u,
			0u,
			6u,
			22u,
			16u,
			0u,
			0u,
			0u,
			3u,
			31u,
			0u,
			16u,
			0u,
			0u,
			15u,
			19u,
			25u,
			5u,
			19u,
			15u,
			0u,
			0u,
			9u,
			21u,
			31u,
			3u,
			0u,
			0u
		};

		public override MyDebugComponent[] Components => m_components;

		public MyPlanetsDebugInputComponent()
		{
			m_components = new MyDebugComponent[5]
			{
				new ShapeComponent(this),
				new InfoComponent(this),
				new SectorsComponent(this),
				new SectorTreeComponent(this),
				new MiscComponent(this)
			};
		}

		public override string GetName()
		{
			return "Planets";
		}

		public override void DrawInternal()
		{
			if (CameraPlanet != null)
			{
				Text(Color.DarkOrange, "Current Planet: {0}", CameraPlanet.StorageName);
			}
		}

		public MyPlanet GetClosestContainingPlanet(Vector3D point)
		{
			m_voxels.Clear();
			BoundingBoxD box = new BoundingBoxD(point, point);
			MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_voxels);
			double num = double.PositiveInfinity;
			MyPlanet result = null;
			foreach (MyVoxelBase voxel in m_voxels)
			{
				if (voxel is MyPlanet)
				{
					float num2 = Vector3.Distance(voxel.WorldMatrix.Translation, point);
					if ((double)num2 < num)
					{
						num = num2;
						result = (MyPlanet)voxel;
					}
				}
			}
			return result;
		}

		public override void Draw()
		{
			if (MySession.Static != null)
			{
				base.Draw();
			}
		}

		public override void Update100()
		{
			CameraPlanet = GetClosestContainingPlanet(MySector.MainCamera.Position);
			if (MySession.Static.LocalCharacter != null)
			{
				CharacterPlanet = GetClosestContainingPlanet(MySession.Static.LocalCharacter.PositionComp.GetPosition());
			}
			base.Update100();
		}

		private static void ProjectToCube(ref Vector3D localPos, out int direction, out Vector2D texcoords)
		{
			Vector3D.Abs(ref localPos, out Vector3D abs);
			if (abs.X > abs.Y)
			{
				if (abs.X > abs.Z)
				{
					localPos /= abs.X;
					texcoords.Y = localPos.Y;
					if (localPos.X > 0.0)
					{
						texcoords.X = 0.0 - localPos.Z;
						direction = 3;
					}
					else
					{
						texcoords.X = localPos.Z;
						direction = 2;
					}
				}
				else
				{
					localPos /= abs.Z;
					texcoords.Y = localPos.Y;
					if (localPos.Z > 0.0)
					{
						texcoords.X = localPos.X;
						direction = 1;
					}
					else
					{
						texcoords.X = 0.0 - localPos.X;
						direction = 0;
					}
				}
			}
			else if (abs.Y > abs.Z)
			{
				localPos /= abs.Y;
				texcoords.Y = localPos.X;
				if (localPos.Y > 0.0)
				{
					texcoords.X = localPos.Z;
					direction = 4;
				}
				else
				{
					texcoords.X = 0.0 - localPos.Z;
					direction = 5;
				}
			}
			else
			{
				localPos /= abs.Z;
				texcoords.Y = localPos.Y;
				if (localPos.Z > 0.0)
				{
					texcoords.X = localPos.X;
					direction = 1;
				}
				else
				{
					texcoords.X = 0.0 - localPos.X;
					direction = 0;
				}
			}
		}
	}
}
