using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using Sandbox.Engine.Voxels.Storage;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI.DebugInputComponents
{
	public class MyVoxelDebugInputComponent : MyMultiDebugInputComponent
	{
		private class IntersectBBComponent : MyDebugComponent
		{
			private enum ProbeMode
			{
				Content,
				Material,
				Intersect
			}

			private struct MatInfo : IComparable<MatInfo>
			{
				public byte Material;

				public int Count;

				public int CompareTo(MatInfo other)
				{
					return Count - other.Count;
				}
			}

			private MyVoxelDebugInputComponent m_comp;

			private bool m_moveProbe = true;

			private bool m_showVoxelProbe;

			private byte m_valueToSet = 128;

			private ProbeMode m_mode = ProbeMode.Intersect;

			private float m_probeSize = 1f;

			private int m_probeLod;

			private List<MyVoxelBase> m_voxels = new List<MyVoxelBase>();

			private MyStorageData m_target = new MyStorageData();

			private MyVoxelBase m_probedVoxel;

			private Vector3 m_probePosition;

			public IntersectBBComponent(MyVoxelDebugInputComponent comp)
			{
				m_comp = comp;
				AddShortcut(MyKeys.OemOpenBrackets, newPress: true, control: false, shift: false, alt: false, () => "Toggle voxel probe box.", () => ToggleProbeBox());
				AddShortcut(MyKeys.OemCloseBrackets, newPress: true, control: false, shift: false, alt: false, () => "Toggle probe mode", () => SwitchProbeMode());
				AddShortcut(MyKeys.OemBackslash, newPress: true, control: false, shift: false, alt: false, () => "Freeze/Unfreeze probe", () => FreezeProbe());
				AddShortcut(MyKeys.OemSemicolon, newPress: true, control: false, shift: false, alt: false, () => "Increase Probe Size.", () => ResizeProbe(1, 0));
				AddShortcut(MyKeys.OemSemicolon, newPress: true, control: true, shift: false, alt: false, () => "Decrease Probe Size.", () => ResizeProbe(-1, 0));
				AddShortcut(MyKeys.OemSemicolon, newPress: true, control: false, shift: true, alt: false, () => "Increase Probe Size (x128).", () => ResizeProbe(128, 0));
				AddShortcut(MyKeys.OemSemicolon, newPress: true, control: true, shift: true, alt: false, () => "Decrease Probe Size (x128).", () => ResizeProbe(-128, 0));
				AddShortcut(MyKeys.OemQuotes, newPress: true, control: false, shift: false, alt: false, () => "Increase LOD Size.", () => ResizeProbe(0, 1));
				AddShortcut(MyKeys.OemQuotes, newPress: true, control: true, shift: false, alt: false, () => "Decrease LOD Size.", () => ResizeProbe(0, -1));
			}

			private bool ResizeProbe(int sizeDelta, int lodDelta)
			{
				m_probeLod = MathHelper.Clamp(m_probeLod + lodDelta, 0, 16);
				if (m_mode != ProbeMode.Intersect)
				{
					m_probeSize = MathHelper.Clamp(m_probeSize + (float)(sizeDelta << m_probeLod), 1 << m_probeLod, 32 * (1 << m_probeLod));
				}
				else
				{
					m_probeSize = MathHelper.Clamp(m_probeSize + (float)(sizeDelta << m_probeLod), 1f, float.PositiveInfinity);
				}
				return true;
			}

			private bool ToggleProbeBox()
			{
				m_showVoxelProbe = !m_showVoxelProbe;
				ResizeProbe(0, 0);
				return true;
			}

			private bool SwitchProbeMode()
			{
				m_mode = (ProbeMode)((int)(m_mode + 1) % 3);
				ResizeProbe(0, 0);
				return true;
			}

			private bool FreezeProbe()
			{
				m_moveProbe = !m_moveProbe;
				return true;
			}

			public override bool HandleInput()
			{
				int num = MyInput.Static.DeltaMouseScrollWheelValue();
				if (num != 0 && MyInput.Static.IsAnyCtrlKeyPressed() && m_showVoxelProbe)
				{
					m_valueToSet += (byte)(num / 120);
					return true;
				}
				return base.HandleInput();
			}

			public override void Draw()
			{
				base.Draw();
				if (MySession.Static == null || !m_showVoxelProbe)
				{
					return;
				}
				float value = m_probeSize * 0.5f;
				_ = m_probeLod;
				if (m_moveProbe)
				{
					m_probePosition = MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * m_probeSize * 3f;
				}
				BoundingBox box = new BoundingBox(m_probePosition - value, m_probePosition + value);
				BoundingBoxD box2 = box;
				m_voxels.Clear();
				MyGamePruningStructure.GetAllVoxelMapsInBox(ref box2, m_voxels);
				MyVoxelBase myVoxelBase = null;
				double num = double.PositiveInfinity;
				foreach (MyVoxelBase voxel in m_voxels)
				{
					double num2 = Vector3D.Distance(voxel.WorldMatrix.Translation, m_probePosition);
					if (num2 < num)
					{
						num = num2;
						myVoxelBase = voxel;
					}
				}
				ContainmentType cont = ContainmentType.Disjoint;
				if (myVoxelBase != null)
				{
					myVoxelBase = myVoxelBase.RootVoxel;
					Vector3 vector = Vector3.Transform(m_probePosition, myVoxelBase.PositionComp.WorldMatrixInvScaled);
					vector += myVoxelBase.SizeInMetresHalf;
					box = new BoundingBox(vector - value, vector + value);
					m_probedVoxel = myVoxelBase;
					Section("Probing {1}: {0}", myVoxelBase.StorageName, myVoxelBase.GetType().Name);
					Text("Probe mode: {0}", m_mode);
					if (m_mode == ProbeMode.Intersect)
					{
						Text("Local Pos: {0}", vector);
						Text("Probe Size: {0}", m_probeSize);
						cont = myVoxelBase.Storage.Intersect(ref box, lazy: false);
						Text("Result: {0}", cont.ToString());
						box2 = box;
					}
				}
				else
				{
					Section("No Voxel Found");
					Text("Probe mode: {0}", m_mode);
					Text("Probe Size: {0}", m_probeSize);
				}
				Color color = ColorForContainment(cont);
				if (myVoxelBase != null)
				{
					box2 = box2.Translate(-myVoxelBase.SizeInMetresHalf);
					MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box2, myVoxelBase.WorldMatrix), color, 0.5f, depthRead: true, smooth: false);
				}
				else
				{
					MyRenderProxy.DebugDrawAABB(box2, color, 0.5f);
				}
			}

			private void DrawContentsInfo(MyStorageData data)
			{
				uint num = 0u;
				uint num2 = 0u;
				uint num3 = 0u;
				byte b = byte.MaxValue;
				byte b2 = 0;
				int num4 = data.SizeLinear / data.StepLinear;
				for (int i = 0; i < data.SizeLinear; i += data.StepLinear)
				{
					byte b3 = data.Content(i);
					if (b > b3)
					{
						b = b3;
					}
					if (b2 < b3)
					{
						b2 = b3;
					}
					num += b3;
					if (b3 != 0)
					{
						num2++;
					}
					if (b3 != byte.MaxValue)
					{
						num3++;
					}
				}
				Section("Probing Contents ({0} {1})", num4, (num4 > 1) ? "voxels" : "voxel");
				Text("Min: {0}", b);
				Text("Average: {0}", (long)num / (long)num4);
				Text("Max: {0}", b2);
				VSpace(5f);
				Text("Non-Empty: {0}", num2);
				Text("Non-Full: {0}", num3);
			}

			private unsafe void DrawMaterialsInfo(MyStorageData data)
			{
				int* ptr = stackalloc int[256];
				int num = data.SizeLinear / data.StepLinear;
				for (int i = 0; i < data.SizeLinear; i += data.StepLinear)
				{
					byte b = data.Material(i);
					ptr[(int)b]++;
				}
				Section("Probing Materials ({0} {1})", num, (num > 1) ? "voxels" : "voxel");
				List<MatInfo> list = new List<MatInfo>();
				for (int j = 0; j < 256; j++)
				{
					if (ptr[j] > 0)
					{
						list.Add(new MatInfo
						{
							Material = (byte)j,
							Count = ptr[j]
						});
					}
				}
				list.Sort();
				_ = MyDefinitionManager.Static.VoxelMaterialCount;
			}

			private Color ColorForContainment(ContainmentType cont)
			{
				switch (cont)
				{
				default:
					return Color.Red;
				case ContainmentType.Contains:
					return Color.Yellow;
				case ContainmentType.Disjoint:
					return Color.Green;
				}
			}

			public override string GetName()
			{
				return "Intersect BB";
			}
		}

		private class IntersectRayComponent : MyDebugComponent
		{
			private MyVoxelDebugInputComponent m_comp;

			private bool m_moveProbe = true;

			private bool m_showVoxelProbe;

			private float m_rayLength = 25f;

			private MyVoxelBase m_probedVoxel;

			private LineD m_probedLine;

			private Vector3D m_forward;

			private Vector3D m_up;

			private int m_probeCount = 1;

			private float m_probeGap = 1f;

			public IntersectRayComponent(MyVoxelDebugInputComponent comp)
			{
				m_comp = comp;
				AddShortcut(MyKeys.OemOpenBrackets, newPress: true, control: false, shift: false, alt: false, () => "Toggle voxel probe ray.", () => ToggleProbeRay());
				AddShortcut(MyKeys.OemBackslash, newPress: true, control: false, shift: false, alt: false, () => "Freeze/Unfreeze probe", () => FreezeProbe());
			}

			private bool ToggleProbeRay()
			{
				m_showVoxelProbe = !m_showVoxelProbe;
				return true;
			}

			private bool FreezeProbe()
			{
				m_moveProbe = !m_moveProbe;
				return true;
			}

			public override bool HandleInput()
			{
				int num = MyInput.Static.DeltaMouseScrollWheelValue();
				if (num != 0 && m_showVoxelProbe)
				{
					if (MyInput.Static.IsAnyCtrlKeyPressed())
					{
						if (MyInput.Static.IsAnyShiftKeyPressed())
						{
							m_rayLength += (float)num / 12f;
						}
						else
						{
							m_rayLength += (float)num / 120f;
						}
						m_probedLine.To = m_probedLine.From + m_rayLength * m_probedLine.Direction;
						m_probedLine.Length = m_rayLength;
						return true;
					}
					if (MyInput.Static.IsKeyPress(MyKeys.G))
					{
						m_probeGap = MathHelper.Clamp(m_probeGap + (float)num / 240f, 0.5f, 32f);
						return true;
					}
					if (MyInput.Static.IsKeyPress(MyKeys.N))
					{
						m_probeCount = MathHelper.Clamp(m_probeCount + num / 120, 1, 33);
						return true;
					}
				}
				return base.HandleInput();
			}

			private void Probe(Vector3D pos)
			{
				LineD ray = m_probedLine;
				ray.From += pos;
				ray.To += pos;
				List<MyLineSegmentOverlapResult<MyEntity>> list = new List<MyLineSegmentOverlapResult<MyEntity>>();
				MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, list, MyEntityQueryType.Static);
				double num = double.PositiveInfinity;
				foreach (MyLineSegmentOverlapResult<MyEntity> item in list)
				{
					MyVoxelBase myVoxelBase = item.Element as MyVoxelBase;
					if (myVoxelBase != null && item.Distance < num)
					{
						m_probedVoxel = myVoxelBase;
					}
				}
				if (m_probedVoxel is MyVoxelPhysics)
				{
					m_probedVoxel = ((MyVoxelPhysics)m_probedVoxel).Parent;
				}
				if (m_probedVoxel != null && m_probedVoxel.Storage.DataProvider != null)
				{
					MyRenderProxy.DebugDrawLine3D(ray.From, ray.To, Color.Green, Color.Green, depthRead: true);
					Vector3D vector3D = Vector3D.Transform(ray.From, m_probedVoxel.PositionComp.WorldMatrixInvScaled);
					vector3D += m_probedVoxel.SizeInMetresHalf;
					Vector3D to = Vector3D.Transform(ray.To, m_probedVoxel.PositionComp.WorldMatrixInvScaled);
					to += m_probedVoxel.SizeInMetresHalf;
					LineD line = new LineD(vector3D, to);
					double startOffset;
					double endOffset;
					bool flag = m_probedVoxel.Storage.DataProvider.Intersect(ref line, out startOffset, out endOffset);
					Vector3D from = line.From;
					line.From = from + line.Direction * line.Length * startOffset;
					line.To = from + line.Direction * line.Length * endOffset;
					if (m_probeCount == 1)
					{
						Text(Color.Yellow, 1.5f, "Probing voxel map {0}:{1}", m_probedVoxel.StorageName, m_probedVoxel.EntityId);
						Text("Local Pos: {0}", vector3D);
						Text("Intersects: {0}", flag);
					}
					if (flag)
					{
						vector3D = line.From - m_probedVoxel.SizeInMetresHalf;
						vector3D = Vector3D.Transform(vector3D, m_probedVoxel.PositionComp.WorldMatrix);
						to = line.To - m_probedVoxel.SizeInMetresHalf;
						to = Vector3D.Transform(to, m_probedVoxel.PositionComp.WorldMatrix);
						MyRenderProxy.DebugDrawLine3D(vector3D, to, Color.Red, Color.Red, depthRead: true);
					}
				}
				else
				{
					if (m_probeCount == 1)
					{
						Text(Color.Yellow, 1.5f, "No voxel found");
					}
					MyRenderProxy.DebugDrawLine3D(ray.From, ray.To, Color.Yellow, Color.Yellow, depthRead: true);
				}
			}

			public override void Draw()
			{
				base.Draw();
				if (MySession.Static == null || !m_showVoxelProbe)
				{
					return;
				}
				Text("Probe Controlls:");
				Text("\tCtrl + Mousewheel: Chage probe size");
				Text("\tCtrl + Shift+Mousewheel: Chage probe size (x10)");
				Text("\tN + Mousewheel: Chage probe count");
				Text("\tG + Mousewheel: Chage probe gap");
				Text("Probe Size: {0}", m_rayLength);
				Text("Probe Count: {0}", m_probeCount * m_probeCount);
				if (m_moveProbe)
				{
					m_up = MySector.MainCamera.UpVector;
					m_forward = MySector.MainCamera.ForwardVector;
					Vector3D vector3D = MySector.MainCamera.Position - m_up * 0.5 + m_forward * 0.5;
					m_probedLine = new LineD(vector3D, vector3D + m_rayLength * m_forward);
				}
				Vector3D value = Vector3D.Cross(m_forward, m_up);
				float num = (float)m_probeCount / 2f;
				for (int i = 0; i < m_probeCount; i++)
				{
					for (int j = 0; j < m_probeCount; j++)
					{
						Vector3D pos = ((float)i - num) * m_probeGap * value + ((float)j - num) * m_probeGap * m_up;
						Probe(pos);
					}
				}
			}

			public override string GetName()
			{
				return "Intersect Ray";
			}
		}

		public class PhysicsComponent : MyDebugComponent
		{
			private class PredictionInfo
			{
				public MyVoxelBase Body;

				public Vector4I Id;

				public MyOrientedBoundingBoxD Bounds;
			}

			private MyVoxelDebugInputComponent m_comp;

			private bool m_debugDraw;

			private ConcurrentCachingList<PredictionInfo> m_list = new ConcurrentCachingList<PredictionInfo>();

			public static PhysicsComponent Static;

			[Conditional("DEBUG")]
			public void Add(MatrixD worldMatrix, BoundingBox box, Vector4I id, MyVoxelBase voxel)
			{
				if (m_list.Count > 1900)
				{
					m_list.ClearList();
				}
				voxel = voxel.RootVoxel;
				box.Translate(-voxel.SizeInMetresHalf);
				m_list.Add(new PredictionInfo
				{
					Id = id,
					Bounds = MyOrientedBoundingBoxD.Create(box, voxel.WorldMatrix),
					Body = voxel
				});
			}

			public PhysicsComponent(MyVoxelDebugInputComponent comp)
			{
				m_comp = comp;
				Static = this;
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Clear boxes", delegate
				{
					m_list.ClearList();
					return false;
				});
			}

			public override void Draw()
			{
				base.Draw();
				if (MySession.Static == null)
				{
					m_list.ClearList();
				}
				m_list.ApplyChanges();
				Text("Queried Out Areas: {0}", m_list.Count);
				foreach (PredictionInfo item in m_list)
				{
					MyRenderProxy.DebugDrawOBB(item.Bounds, Color.Cyan, 0.2f, depthRead: true, smooth: true);
				}
			}

			public override string GetName()
			{
				return "Physics";
			}
		}

		private class StorageWriteCacheComponent : MyDebugComponent
		{
			private MyVoxelDebugInputComponent m_comp;

			private bool DisplayDetails;

			private bool DebugDraw;

			public StorageWriteCacheComponent(MyVoxelDebugInputComponent comp)
			{
				m_comp = comp;
				AddShortcut(MyKeys.NumPad1, newPress: true, control: false, shift: false, alt: false, () => "Toggle detailed details.", () => DisplayDetails = !DisplayDetails);
				AddShortcut(MyKeys.NumPad2, newPress: true, control: false, shift: false, alt: false, () => "Toggle debug draw.", () => DebugDraw = !DebugDraw);
				AddShortcut(MyKeys.NumPad3, newPress: true, control: false, shift: false, alt: false, () => "Toggle cache writing.", ToggleWrite);
				AddShortcut(MyKeys.NumPad4, newPress: true, control: false, shift: false, alt: false, () => "Toggle cache flushing.", ToggleFlush);
				AddShortcut(MyKeys.NumPad5, newPress: true, control: false, shift: false, alt: false, () => "Toggle cache.", ToggleCache);
			}

			private bool ToggleWrite()
			{
				MyVoxelOperationsSessionComponent component = MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();
				component.ShouldWrite = !component.ShouldWrite;
				return true;
			}

			private bool ToggleFlush()
			{
				MyVoxelOperationsSessionComponent component = MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();
				component.ShouldFlush = !component.ShouldFlush;
				return true;
			}

			private bool ToggleCache()
			{
				MyVoxelOperationsSessionComponent.EnableCache = !MyVoxelOperationsSessionComponent.EnableCache;
				return true;
			}

			public override void Draw()
			{
				base.Draw();
				if (MySession.Static == null)
				{
					return;
				}
				MyVoxelOperationsSessionComponent component = MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();
				if (component == null)
				{
					return;
				}
				Text("Cache Enabled: {0}", MyVoxelOperationsSessionComponent.EnableCache);
				Text("Cache Writing: {0}", component.ShouldWrite ? "Enabled" : "Disabled");
				Text("Cache Flushing: {0}", component.ShouldFlush ? "Enabled" : "Disabled");
				MyStorageBase[] array = component.QueuedStorages.ToArray();
				if (array.Length == 0)
				{
					Text(Color.Orange, "No queued storages.");
					return;
				}
				Text(Color.Yellow, 1.2f, "{0} Queued storages:", array.Length);
				MyStorageBase[] array2 = array;
				foreach (MyStorageBase storage in array2)
				{
					storage.GetStats(out MyStorageBase.WriteCacheStats stats);
					Text("Voxel storage {0}:", storage.ToString());
					Text(Color.White, 0.9f, "Pending Writes: {0}", stats.QueuedWrites);
					Text(Color.White, 0.9f, "Cached Chunks: {0}", stats.CachedChunks);
					if (DisplayDetails)
					{
						foreach (KeyValuePair<Vector3I, MyStorageBase.VoxelChunk> chunk in stats.Chunks)
						{
							MyStorageBase.VoxelChunk value = chunk.Value;
							Text(Color.Wheat, 0.9f, "Chunk {0}: {1} hits; pending {2}", chunk.Key, value.HitCount, value.Dirty);
						}
					}
					if (DebugDraw)
					{
						MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.Instances.FirstOrDefault((MyVoxelBase x) => x.Storage == storage);
						if (myVoxelBase != null)
						{
							foreach (KeyValuePair<Vector3I, MyStorageBase.VoxelChunk> chunk2 in stats.Chunks)
							{
								BoundingBoxD box = new BoundingBoxD(chunk2.Key << 3, chunk2.Key + 1 << 3);
								box.Translate(-((Vector3D)storage.Size * 0.5) - 0.5);
								MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box, myVoxelBase.WorldMatrix), GetColorForDirty(chunk2.Value.Dirty), 0.1f, depthRead: true, smooth: true);
							}
						}
					}
				}
			}

			private static Color GetColorForDirty(MyStorageDataTypeFlags dirty)
			{
				switch (dirty)
				{
				case MyStorageDataTypeFlags.Content:
					return Color.Blue;
				case MyStorageDataTypeFlags.Material:
					return Color.Red;
				case MyStorageDataTypeFlags.ContentAndMaterial:
					return Color.Magenta;
				case MyStorageDataTypeFlags.None:
					return Color.Green;
				default:
					return Color.White;
				}
			}

			public override string GetName()
			{
				return "Storage Write Cache";
			}
		}

		private class ToolsComponent : MyDebugComponent
		{
			private MyVoxelDebugInputComponent m_comp;

			private MyVoxelBase m_selectedVoxel;

			public ToolsComponent(MyVoxelDebugInputComponent comp)
			{
				m_comp = comp;
				AddShortcut(MyKeys.NumPad8, newPress: true, control: false, shift: false, alt: false, () => "Shrink selected storage to fit.", () => StorageShrinkToFit());
			}

			private static void ShowAlert(string message, params object[] args)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(string.Format(message, args)), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
			}

			private static void Confirm(string message, Action successCallback)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder(message), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum x)
				{
					if (x == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						successCallback();
					}
				}));
			}

			private bool StorageShrinkToFit()
			{
				if (m_selectedVoxel == null)
				{
					ShowAlert("Please select a voxel map with the voxel probe box.");
					return true;
				}
				if (m_selectedVoxel is MyPlanet)
				{
					ShowAlert("Planets cannot be shrunk to fit.");
					return true;
				}
				long num = m_selectedVoxel.Size.Size;
				Confirm($"Are you sure you want to shrink \"{m_selectedVoxel.StorageName}\" ({num} voxels total)? This will overwrite the original storage.", ShrinkVMap);
				return true;
			}

			private void ShrinkVMap()
			{
				m_selectedVoxel.GetFilledStorageBounds(out Vector3I min, out Vector3I max);
				MyVoxelMapStorageDefinition definition = null;
				if (m_selectedVoxel.AsteroidName != null)
				{
					MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(m_selectedVoxel.AsteroidName, out definition);
				}
				Vector3I size = m_selectedVoxel.Size;
				Vector3I vector3I = max - min + 1;
				MyOctreeStorage myOctreeStorage = new MyOctreeStorage(null, vector3I);
				Vector3I vector3I2 = (myOctreeStorage.Size - vector3I) / 2 + 1;
				MyStorageData myStorageData = new MyStorageData();
				myStorageData.Resize(vector3I);
				m_selectedVoxel.Storage.ReadRange(myStorageData, MyStorageDataTypeFlags.ContentAndMaterial, 0, min, max);
				min = vector3I2;
				max = vector3I2 + vector3I - 1;
				myOctreeStorage.WriteRange(myStorageData, MyStorageDataTypeFlags.ContentAndMaterial, min, max);
				MyVoxelMap myVoxelMap = MyWorldGenerator.AddVoxelMap(m_selectedVoxel.StorageName, myOctreeStorage, m_selectedVoxel.WorldMatrix, 0L);
				m_selectedVoxel.Close();
				myVoxelMap.Save = true;
				if (definition == null)
				{
					ShowAlert("Voxel map {0} does not have a definition, the shrunk voxel map will be saved with the world instead.", m_selectedVoxel.StorageName);
					return;
				}
				myVoxelMap.Storage.Save(out byte[] outCompressedData);
				using (Stream stream = MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.ContentPath, definition.StorageFile), FileMode.Open))
				{
					stream.Write(outCompressedData, 0, outCompressedData.Length);
				}
				MyHudNotification myHudNotification = new MyHudNotification(MyStringId.GetOrCompute("Voxel prefab {0} updated succesfuly (size changed from {1} to {2})."), 4000);
				myHudNotification.SetTextFormatArguments(definition.Id.SubtypeName, size, myOctreeStorage.Size);
				MyHud.Notifications.Add(myHudNotification);
			}

			public override void Draw()
			{
				base.Draw();
				if (MySession.Static != null)
				{
					LineD ray = new LineD(MySector.MainCamera.Position, MySector.MainCamera.Position + 200f * MySector.MainCamera.ForwardVector);
					List<MyLineSegmentOverlapResult<MyEntity>> list = new List<MyLineSegmentOverlapResult<MyEntity>>();
					MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, list, MyEntityQueryType.Static);
					double num = double.PositiveInfinity;
					foreach (MyLineSegmentOverlapResult<MyEntity> item in list)
					{
						MyVoxelBase myVoxelBase = item.Element as MyVoxelBase;
						if (myVoxelBase != null && item.Distance < num)
						{
							m_selectedVoxel = myVoxelBase;
						}
					}
					if (m_selectedVoxel != null)
					{
						Text(Color.DarkOrange, 1.5f, "Selected Voxel: {0}:{1}", m_selectedVoxel.StorageName, m_selectedVoxel.EntityId);
					}
				}
			}

			public override string GetName()
			{
				return "Tools";
			}
		}

		private MyDebugComponent[] m_components;

		public override MyDebugComponent[] Components => m_components;

		public MyVoxelDebugInputComponent()
		{
			m_components = new MyDebugComponent[5]
			{
				new IntersectBBComponent(this),
				new IntersectRayComponent(this),
				new ToolsComponent(this),
				new StorageWriteCacheComponent(this),
				new PhysicsComponent(this)
			};
		}

		public override string GetName()
		{
			return "Voxels";
		}
	}
}
