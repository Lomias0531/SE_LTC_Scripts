using Sandbox.Definitions;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Engine.Voxels.Storage;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("Game", "Voxels")]
	public class MyGuiScreenDebugVoxels : MyGuiScreenDebugBase
	{
		private MyGuiControlCombobox m_filesCombo;

		private MyGuiControlCombobox m_materialsCombo;

		private MyGuiControlCombobox m_shapesCombo;

		private MyGuiControlCombobox m_rangesCombo;

		private string m_selectedVoxelFile;

		private string m_selectedVoxelMaterial;

		private static MyRenderQualityEnum m_voxelRangesQuality;

		private static MyRenderQualityEnum VoxelRangesQuality
		{
			get
			{
				return m_voxelRangesQuality;
			}
			set
			{
				if (m_voxelRangesQuality != value)
				{
					m_voxelRangesQuality = value;
					MyRenderComponentVoxelMap.SetLodQuality(m_voxelRangesQuality);
				}
			}
		}

		private bool UseTriangleCache
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public MyGuiScreenDebugVoxels()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugVoxels";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_voxelRangesQuality = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality;
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Voxels", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddSlider("Max precalc time", 0f, 20f, null, MemberHelper.GetMember(() => MyFakes.MAX_PRECALC_TIME_IN_MILLIS));
			AddCheckBox("Enable yielding", null, MemberHelper.GetMember(() => MyFakes.ENABLE_YIELDING_IN_PRECALC_TASK));
			AddCheckBox("Enable storage cache", MyVoxelOperationsSessionComponent.EnableCache, delegate(MyGuiControlCheckbox x)
			{
				MyVoxelOperationsSessionComponent.EnableCache = x.IsChecked;
			});
			m_filesCombo = MakeComboFromFiles(Path.Combine(MyFileSystem.ContentPath, "VoxelMaps"));
			m_filesCombo.ItemSelected += filesCombo_OnSelect;
			m_materialsCombo = AddCombo();
			foreach (MyVoxelMaterialDefinition voxelMaterialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
			{
				m_materialsCombo.AddItem(voxelMaterialDefinition.Index, new StringBuilder(voxelMaterialDefinition.Id.SubtypeName));
			}
			m_materialsCombo.ItemSelected += materialsCombo_OnSelect;
			m_materialsCombo.SelectItemByIndex(0);
			AddCombo<MyVoxelDebugDrawMode>(null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXELS_MODE));
			AddLabel("Voxel ranges", Color.Yellow.ToVector4(), 0.7f);
			AddCombo<MyRenderQualityEnum>(null, MemberHelper.GetMember(() => VoxelRangesQuality));
			AddButton(new StringBuilder("Remove all"), RemoveAllAsteroids);
			AddButton(new StringBuilder("Generate render"), GenerateRender);
			AddButton(new StringBuilder("Generate physics"), GeneratePhysics);
			AddButton(new StringBuilder("Reset all"), ResetAll);
			AddButton(new StringBuilder("Sweep all"), SweepAll);
			AddButton(new StringBuilder("Revert first"), RevertFirst);
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Freeze terrain queries", MyRenderProxy.Settings.FreezeTerrainQueries, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.FreezeTerrainQueries = x.IsChecked;
			});
			AddCheckBox("Draw clipmap cells", MyRenderProxy.Settings.DebugRenderClipmapCells, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DebugRenderClipmapCells = x.IsChecked;
			});
			AddCheckBox("Draw edited cells", MyDebugDrawSettings.DEBUG_DRAW_VOXEL_ACCESS, delegate(MyGuiControlCheckbox x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_VOXEL_ACCESS = x.IsChecked;
			});
			AddCheckBox("Wireframe", MyRenderProxy.Settings.Wireframe, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.Wireframe = x.IsChecked;
			});
			AddCheckBox("Debug texture lod colors", MyRenderProxy.Settings.DebugTextureLodColor, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DebugTextureLodColor = x.IsChecked;
			});
			AddCheckBox("Enable physics shape discard", null, MemberHelper.GetMember(() => MyFakes.ENABLE_VOXEL_PHYSICS_SHAPE_DISCARDING));
			AddCheckBox("Use triangle cache", this, MemberHelper.GetMember(() => UseTriangleCache));
			AddCheckBox("Use storage cache", null, MemberHelper.GetMember(() => MyStorageBase.UseStorageCache));
			AddCheckBox("Voxel AO", null, MemberHelper.GetMember(() => MyFakes.ENABLE_VOXEL_COMPUTED_OCCLUSION));
			m_currentPosition.Y += 0.01f;
		}

		private MyGuiControlCombobox MakeComboFromFiles(string path, string filter = "*", MySearchOption search = MySearchOption.AllDirectories)
		{
			MyGuiControlCombobox myGuiControlCombobox = AddCombo();
			long num = 0L;
			myGuiControlCombobox.AddItem(num++, "");
			foreach (string file in MyFileSystem.GetFiles(path, filter, search))
			{
				myGuiControlCombobox.AddItem(num++, Path.GetFileNameWithoutExtension(file));
			}
			myGuiControlCombobox.SelectItemByIndex(0);
			return myGuiControlCombobox;
		}

		private void filesCombo_OnSelect()
		{
			if (m_filesCombo.GetSelectedKey() != 0L)
			{
				m_selectedVoxelFile = Path.Combine(MyFileSystem.ContentPath, m_filesCombo.GetSelectedValue().ToString() + ".vx2");
			}
		}

		private void materialsCombo_OnSelect()
		{
			m_selectedVoxelMaterial = m_materialsCombo.GetSelectedValue().ToString();
		}

		private void RemoveAllAsteroids(MyGuiControlButton sender)
		{
			MySession.Static.VoxelMaps.Clear();
		}

		private void GenerateRender(MyGuiControlButton sender)
		{
			foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
			{
				_ = instance;
			}
		}

		private void GeneratePhysics(MyGuiControlButton sender)
		{
			foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
			{
				if (instance.Physics != null)
				{
					(instance.Physics as MyVoxelPhysicsBody).GenerateAllShapes();
				}
			}
		}

		private void ResavePrefabs(MyGuiControlButton sender)
		{
			string[] array = MyFileSystem.GetFiles(MyFileSystem.ContentPath, "*.vx2", MySearchOption.AllDirectories).ToArray();
			foreach (string obj in array)
			{
				MyStorageBase.LoadFromFile(obj).Save(out byte[] outCompressedData);
				using (Stream stream = MyFileSystem.OpenWrite(obj, FileMode.Open))
				{
					stream.Write(outCompressedData, 0, outCompressedData.Length);
				}
			}
		}

		private void ForceVoxelizeAllVoxelMaps(MyGuiControlBase sender)
		{
			DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
			int num = 0;
			foreach (MyVoxelBase item in instances)
			{
				num++;
				(item.Storage as MyOctreeStorage)?.Voxelize(MyStorageDataTypeFlags.Content);
			}
		}

		private void ResetAll(MyGuiControlBase sender)
		{
			DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
			int num = 0;
			foreach (MyVoxelBase item in instances)
			{
				num++;
				if (!(item is MyVoxelPhysics))
				{
					(item.Storage as MyOctreeStorage)?.Reset(MyStorageDataTypeFlags.ContentAndMaterial);
				}
			}
		}

		private void SweepAll(MyGuiControlBase sender)
		{
			DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
			int num = 0;
			foreach (MyVoxelBase item in instances)
			{
				num++;
				if (!(item is MyVoxelPhysics))
				{
					(item.Storage as MyStorageBase)?.Sweep(MyStorageDataTypeFlags.ContentAndMaterial);
				}
			}
		}

		private void RevertFirst(MyGuiControlBase sender)
		{
			DictionaryValuesReader<long, MyVoxelBase> instances = MySession.Static.VoxelMaps.Instances;
			int num = 0;
			foreach (MyVoxelBase item in instances)
			{
				num++;
				if (!(item is MyVoxelPhysics))
				{
					(item.Storage as MyStorageBase)?.AccessDeleteFirst();
				}
			}
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}
	}
}
