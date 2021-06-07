using Sandbox.Game;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Generics;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_CubeBlockDefinition), null)]
	public class MyCubeBlockDefinition : MyPhysicalModelDefinition
	{
		public class Component
		{
			public MyComponentDefinition Definition;

			public int Count;

			public MyPhysicalItemDefinition DeconstructItem;
		}

		public class BuildProgressModel
		{
			public float BuildRatioUpperBound;

			public string File;

			public bool RandomOrientation;

			public MountPoint[] MountPoints;

			public bool Visible;
		}

		public struct MountPoint
		{
			public Vector3I Normal;

			public Vector3 Start;

			public Vector3 End;

			public byte ExclusionMask;

			public byte PropertiesMask;

			public bool Enabled;

			public bool Default;

			public MyObjectBuilder_CubeBlockDefinition.MountPoint GetObjectBuilder(Vector3I cubeSize)
			{
				MyObjectBuilder_CubeBlockDefinition.MountPoint mountPoint = new MyObjectBuilder_CubeBlockDefinition.MountPoint();
				mountPoint.Side = NormalToBlockSide(Normal);
				UntransformMountPointPosition(ref Start, (int)mountPoint.Side, cubeSize, out Vector3 result);
				UntransformMountPointPosition(ref End, (int)mountPoint.Side, cubeSize, out Vector3 result2);
				mountPoint.Start = new SerializableVector2(result.X, result.Y);
				mountPoint.End = new SerializableVector2(result2.X, result2.Y);
				mountPoint.ExclusionMask = ExclusionMask;
				mountPoint.PropertiesMask = PropertiesMask;
				mountPoint.Enabled = Enabled;
				mountPoint.Default = Default;
				return mountPoint;
			}
		}

		private class Sandbox_Definitions_MyCubeBlockDefinition_003C_003EActor : IActivator, IActivator<MyCubeBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCubeBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCubeBlockDefinition CreateInstance()
			{
				return new MyCubeBlockDefinition();
			}

			MyCubeBlockDefinition IActivator<MyCubeBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyCubeSize CubeSize;

		public MyBlockTopology BlockTopology = MyBlockTopology.TriangleMesh;

		public Vector3I Size;

		public Vector3 ModelOffset;

		public bool UseModelIntersection;

		public MyCubeDefinition CubeDefinition;

		public bool SilenceableByShipSoundSystem;

		public Component[] Components;

		public ushort CriticalGroup;

		public float CriticalIntegrityRatio;

		public float OwnershipIntegrityRatio;

		public float MaxIntegrityRatio;

		public float MaxIntegrity;

		public int? DamageEffectID;

		public string DamageEffectName = string.Empty;

		public string DestroyEffect = "";

		public Vector3? DestroyEffectOffset;

		public MySoundPair DestroySound = MySoundPair.Empty;

		public CubeBlockEffectBase[] Effects;

		public MountPoint[] MountPoints;

		public Dictionary<Vector3I, Dictionary<Vector3I, bool>> IsCubePressurized;

		public MyBlockNavigationDefinition NavigationDefinition;

		public Color Color;

		public List<MyCubeBlockDefinition> Variants = new List<MyCubeBlockDefinition>();

		public MyCubeBlockDefinition UniqueVersion;

		public MyPhysicsOption PhysicsOption;

		public MyStringId? DisplayNameVariant;

		public string BlockPairName;

		public bool UsesDeformation;

		public float DeformationRatio;

		public float IntegrityPointsPerSec;

		public string EdgeType;

		public List<BoneInfo> Skeleton;

		public Dictionary<Vector3I, Vector3> Bones;

		public bool? IsAirTight;

		public bool IsStandAlone = true;

		public bool HasPhysics = true;

		public bool UseNeighbourOxygenRooms;

		public MyStringId BuildType;

		public string BuildMaterial;

		public MyDefinitionId[] GeneratedBlockDefinitions;

		public MyStringId GeneratedBlockType;

		public float BuildProgressToPlaceGeneratedBlocks;

		public bool CreateFracturedPieces;

		public MyStringHash EmissiveColorPreset = MyStringHash.NullOrEmpty;

		public string[] CompoundTemplates;

		public bool CompoundEnabled;

		public string MultiBlock;

		public Dictionary<string, MyDefinitionId> SubBlockDefinitions;

		[Obsolete("Use new block variant group system")]
		public MyDefinitionId[] BlockStages;

		public BuildProgressModel[] BuildProgressModels;

		private Vector3I m_center;

		private MySymmetryAxisEnum m_symmetryX;

		private MySymmetryAxisEnum m_symmetryY;

		private MySymmetryAxisEnum m_symmetryZ;

		private StringBuilder m_displayNameTextCache;

		public float DisassembleRatio;

		public MyAutorotateMode AutorotateMode;

		private string m_mirroringBlock;

		public MySoundPair PrimarySound;

		public MySoundPair ActionSound;

		public MySoundPair DamagedSound;

		public int PCU;

		public static readonly int PCU_CONSTRUCTION_STAGE_COST = 1;

		public bool PlaceDecals;

		public Dictionary<string, MyObjectBuilder_ComponentBase> EntityComponents;

		public VoxelPlacementOverride? VoxelPlacement;

		public float GeneralDamageMultiplier;

		public MyBlockVariantGroup BlockVariantsGroup;

		private static Matrix[] m_mountPointTransforms = new Matrix[6]
		{
			Matrix.CreateFromDir(Vector3.Right, Vector3.Up) * Matrix.CreateScale(1f, 1f, -1f),
			Matrix.CreateFromDir(Vector3.Up, Vector3.Forward) * Matrix.CreateScale(-1f, 1f, 1f),
			Matrix.CreateFromDir(Vector3.Forward, Vector3.Up) * Matrix.CreateScale(-1f, 1f, 1f),
			Matrix.CreateFromDir(Vector3.Left, Vector3.Up) * Matrix.CreateScale(1f, 1f, -1f),
			Matrix.CreateFromDir(Vector3.Down, Vector3.Backward) * Matrix.CreateScale(-1f, 1f, 1f),
			Matrix.CreateFromDir(Vector3.Backward, Vector3.Up) * Matrix.CreateScale(-1f, 1f, 1f)
		};

		private static Vector3[] m_mountPointWallOffsets = new Vector3[6]
		{
			new Vector3(1f, 0f, 1f),
			new Vector3(0f, 1f, 1f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 1f)
		};

		private static int[] m_mountPointWallIndices = new int[6]
		{
			2,
			5,
			3,
			0,
			1,
			4
		};

		private const float OFFSET_CONST = 0.001f;

		private const float THICKNESS_HALF = 0.0004f;

		private static List<int> m_tmpIndices = new List<int>();

		private static List<MyObjectBuilder_CubeBlockDefinition.MountPoint> m_tmpMounts = new List<MyObjectBuilder_CubeBlockDefinition.MountPoint>();

		private static readonly MyDynamicObjectPool<List<string>> m_stringPool = new MyDynamicObjectPool<List<string>>(10);

		private static readonly HashSet<MyCubeBlockDefinition> m_preloadedDefinitions = new HashSet<MyCubeBlockDefinition>();

		public MyBlockDirection Direction
		{
			get;
			private set;
		}

		public MyBlockRotation Rotation
		{
			get;
			private set;
		}

		public bool IsGeneratedBlock => GeneratedBlockType != MyStringId.NullOrEmpty;

		public Vector3I Center => m_center;

		public MySymmetryAxisEnum SymmetryX => m_symmetryX;

		public MySymmetryAxisEnum SymmetryY => m_symmetryY;

		public MySymmetryAxisEnum SymmetryZ => m_symmetryZ;

		public string MirroringBlock => m_mirroringBlock;

		public override string DisplayNameText
		{
			get
			{
				if (!DisplayNameVariant.HasValue)
				{
					return base.DisplayNameText;
				}
				if (m_displayNameTextCache == null)
				{
					m_displayNameTextCache = new StringBuilder();
				}
				m_displayNameTextCache.Clear();
				return m_displayNameTextCache.Append(base.DisplayNameText).Append(' ').Append(MyTexts.GetString(DisplayNameVariant.Value))
					.ToString();
			}
		}

		public bool GuiVisible
		{
			get;
			set;
		}

		public bool Mirrored
		{
			get;
			private set;
		}

		public bool RandomRotation
		{
			get;
			private set;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CubeBlockDefinition myObjectBuilder_CubeBlockDefinition = builder as MyObjectBuilder_CubeBlockDefinition;
			Size = myObjectBuilder_CubeBlockDefinition.Size;
			Model = myObjectBuilder_CubeBlockDefinition.Model;
			UseModelIntersection = myObjectBuilder_CubeBlockDefinition.UseModelIntersection;
			CubeSize = myObjectBuilder_CubeBlockDefinition.CubeSize;
			ModelOffset = myObjectBuilder_CubeBlockDefinition.ModelOffset;
			BlockTopology = myObjectBuilder_CubeBlockDefinition.BlockTopology;
			PhysicsOption = myObjectBuilder_CubeBlockDefinition.PhysicsOption;
			BlockPairName = myObjectBuilder_CubeBlockDefinition.BlockPairName;
			m_center = (myObjectBuilder_CubeBlockDefinition.Center ?? ((SerializableVector3I)((Size - 1) / 2)));
			m_symmetryX = myObjectBuilder_CubeBlockDefinition.MirroringX;
			m_symmetryY = myObjectBuilder_CubeBlockDefinition.MirroringY;
			m_symmetryZ = myObjectBuilder_CubeBlockDefinition.MirroringZ;
			UsesDeformation = myObjectBuilder_CubeBlockDefinition.UsesDeformation;
			DeformationRatio = myObjectBuilder_CubeBlockDefinition.DeformationRatio;
			SilenceableByShipSoundSystem = myObjectBuilder_CubeBlockDefinition.SilenceableByShipSoundSystem;
			EdgeType = myObjectBuilder_CubeBlockDefinition.EdgeType;
			AutorotateMode = myObjectBuilder_CubeBlockDefinition.AutorotateMode;
			m_mirroringBlock = myObjectBuilder_CubeBlockDefinition.MirroringBlock;
			MultiBlock = myObjectBuilder_CubeBlockDefinition.MultiBlock;
			GuiVisible = myObjectBuilder_CubeBlockDefinition.GuiVisible;
			Rotation = myObjectBuilder_CubeBlockDefinition.Rotation;
			Direction = myObjectBuilder_CubeBlockDefinition.Direction;
			Mirrored = myObjectBuilder_CubeBlockDefinition.Mirrored;
			RandomRotation = myObjectBuilder_CubeBlockDefinition.RandomRotation;
			BuildType = MyStringId.GetOrCompute((myObjectBuilder_CubeBlockDefinition.BuildType != null) ? myObjectBuilder_CubeBlockDefinition.BuildType.ToLower() : null);
			BuildMaterial = ((myObjectBuilder_CubeBlockDefinition.BuildMaterial != null) ? myObjectBuilder_CubeBlockDefinition.BuildMaterial.ToLower() : null);
			BuildProgressToPlaceGeneratedBlocks = myObjectBuilder_CubeBlockDefinition.BuildProgressToPlaceGeneratedBlocks;
			GeneratedBlockType = MyStringId.GetOrCompute((myObjectBuilder_CubeBlockDefinition.GeneratedBlockType != null) ? myObjectBuilder_CubeBlockDefinition.GeneratedBlockType.ToLower() : null);
			CompoundEnabled = myObjectBuilder_CubeBlockDefinition.CompoundEnabled;
			CreateFracturedPieces = myObjectBuilder_CubeBlockDefinition.CreateFracturedPieces;
			EmissiveColorPreset = ((myObjectBuilder_CubeBlockDefinition.EmissiveColorPreset != null) ? MyStringHash.GetOrCompute(myObjectBuilder_CubeBlockDefinition.EmissiveColorPreset) : MyStringHash.NullOrEmpty);
			VoxelPlacement = myObjectBuilder_CubeBlockDefinition.VoxelPlacement;
			GeneralDamageMultiplier = myObjectBuilder_CubeBlockDefinition.GeneralDamageMultiplier;
			if (myObjectBuilder_CubeBlockDefinition.PhysicalMaterial != null)
			{
				PhysicalMaterial = MyDefinitionManager.Static.GetPhysicalMaterialDefinition(myObjectBuilder_CubeBlockDefinition.PhysicalMaterial);
			}
			if (myObjectBuilder_CubeBlockDefinition.Effects != null)
			{
				Effects = new CubeBlockEffectBase[myObjectBuilder_CubeBlockDefinition.Effects.Length];
				for (int i = 0; i < myObjectBuilder_CubeBlockDefinition.Effects.Length; i++)
				{
					Effects[i] = new CubeBlockEffectBase(myObjectBuilder_CubeBlockDefinition.Effects[i].Name, myObjectBuilder_CubeBlockDefinition.Effects[i].ParameterMin, myObjectBuilder_CubeBlockDefinition.Effects[i].ParameterMax);
					if (myObjectBuilder_CubeBlockDefinition.Effects[i].ParticleEffects != null && myObjectBuilder_CubeBlockDefinition.Effects[i].ParticleEffects.Length != 0)
					{
						Effects[i].ParticleEffects = new CubeBlockEffect[myObjectBuilder_CubeBlockDefinition.Effects[i].ParticleEffects.Length];
						for (int j = 0; j < myObjectBuilder_CubeBlockDefinition.Effects[i].ParticleEffects.Length; j++)
						{
							Effects[i].ParticleEffects[j] = new CubeBlockEffect(myObjectBuilder_CubeBlockDefinition.Effects[i].ParticleEffects[j]);
						}
					}
					else
					{
						Effects[i].ParticleEffects = null;
					}
				}
			}
			if (myObjectBuilder_CubeBlockDefinition.DamageEffectId != 0)
			{
				DamageEffectID = myObjectBuilder_CubeBlockDefinition.DamageEffectId;
			}
			if (!string.IsNullOrEmpty(myObjectBuilder_CubeBlockDefinition.DamageEffectName))
			{
				DamageEffectName = myObjectBuilder_CubeBlockDefinition.DamageEffectName;
			}
			if (myObjectBuilder_CubeBlockDefinition.DestroyEffect != null && myObjectBuilder_CubeBlockDefinition.DestroyEffect.Length > 0)
			{
				DestroyEffect = myObjectBuilder_CubeBlockDefinition.DestroyEffect;
			}
			if (myObjectBuilder_CubeBlockDefinition.DestroyEffectOffset.HasValue)
			{
				DestroyEffectOffset = myObjectBuilder_CubeBlockDefinition.DestroyEffectOffset.Value;
			}
			InitEntityComponents(myObjectBuilder_CubeBlockDefinition.EntityComponents);
			CompoundTemplates = myObjectBuilder_CubeBlockDefinition.CompoundTemplates;
			if (myObjectBuilder_CubeBlockDefinition.SubBlockDefinitions != null)
			{
				SubBlockDefinitions = new Dictionary<string, MyDefinitionId>();
				MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] subBlockDefinitions = myObjectBuilder_CubeBlockDefinition.SubBlockDefinitions;
				foreach (MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition mySubBlockDefinition in subBlockDefinitions)
				{
					if (!SubBlockDefinitions.TryGetValue(mySubBlockDefinition.SubBlock, out MyDefinitionId value))
					{
						value = mySubBlockDefinition.Id;
						SubBlockDefinitions.Add(mySubBlockDefinition.SubBlock, value);
					}
				}
			}
			if (myObjectBuilder_CubeBlockDefinition.BlockVariants != null)
			{
				MyLog.Default.Warning("BlockVariants are obsolete. Use block groups instead for block: " + Id);
				BlockStages = new MyDefinitionId[myObjectBuilder_CubeBlockDefinition.BlockVariants.Length];
				for (int l = 0; l < myObjectBuilder_CubeBlockDefinition.BlockVariants.Length; l++)
				{
					BlockStages[l] = myObjectBuilder_CubeBlockDefinition.BlockVariants[l];
				}
			}
			MyObjectBuilder_CubeBlockDefinition.PatternDefinition cubeDefinition = myObjectBuilder_CubeBlockDefinition.CubeDefinition;
			if (cubeDefinition != null)
			{
				MyCubeDefinition myCubeDefinition = new MyCubeDefinition();
				myCubeDefinition.CubeTopology = cubeDefinition.CubeTopology;
				myCubeDefinition.ShowEdges = cubeDefinition.ShowEdges;
				MyObjectBuilder_CubeBlockDefinition.Side[] sides = cubeDefinition.Sides;
				myCubeDefinition.Model = new string[sides.Length];
				myCubeDefinition.PatternSize = new Vector2I[sides.Length];
				myCubeDefinition.ScaleTile = new Vector2I[sides.Length];
				for (int m = 0; m < sides.Length; m++)
				{
					MyObjectBuilder_CubeBlockDefinition.Side side = sides[m];
					myCubeDefinition.Model[m] = side.Model;
					myCubeDefinition.PatternSize[m] = side.PatternSize;
					myCubeDefinition.ScaleTile[m] = new Vector2I(side.ScaleTileU, side.ScaleTileV);
				}
				CubeDefinition = myCubeDefinition;
			}
			MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] components = myObjectBuilder_CubeBlockDefinition.Components;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			MaxIntegrityRatio = 1f;
			if (components != null && components.Length != 0)
			{
				Components = new Component[components.Length];
				float num4 = 0f;
				int num5 = 0;
				for (int n = 0; n < components.Length; n++)
				{
					MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent cubeBlockComponent = components[n];
					MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition(new MyDefinitionId(cubeBlockComponent.Type, cubeBlockComponent.Subtype));
					MyPhysicalItemDefinition definition = null;
					if (!cubeBlockComponent.DeconstructId.IsNull() && !MyDefinitionManager.Static.TryGetPhysicalItemDefinition(cubeBlockComponent.DeconstructId, out definition))
					{
						definition = componentDefinition;
					}
					if (definition == null)
					{
						definition = componentDefinition;
					}
					Component component = new Component
					{
						Definition = componentDefinition,
						Count = cubeBlockComponent.Count,
						DeconstructItem = definition
					};
					if (cubeBlockComponent.Type == typeof(MyObjectBuilder_Component) && cubeBlockComponent.Subtype == "Computer" && num3 == 0f)
					{
						num3 = num4 + (float)component.Definition.MaxIntegrity;
					}
					num4 += (float)(component.Count * component.Definition.MaxIntegrity);
					if (cubeBlockComponent.Type == myObjectBuilder_CubeBlockDefinition.CriticalComponent.Type && cubeBlockComponent.Subtype == myObjectBuilder_CubeBlockDefinition.CriticalComponent.Subtype)
					{
						if (num5 == myObjectBuilder_CubeBlockDefinition.CriticalComponent.Index)
						{
							CriticalGroup = (ushort)n;
							num2 = num4 - (float)component.Definition.MaxIntegrity;
						}
						num5++;
					}
					num += (float)component.Count * component.Definition.Mass;
					Components[n] = component;
				}
				MaxIntegrity = num4;
				IntegrityPointsPerSec = MaxIntegrity / myObjectBuilder_CubeBlockDefinition.BuildTimeSeconds;
				DisassembleRatio = myObjectBuilder_CubeBlockDefinition.DisassembleRatio;
				if (myObjectBuilder_CubeBlockDefinition.MaxIntegrity != 0)
				{
					MaxIntegrityRatio = (float)myObjectBuilder_CubeBlockDefinition.MaxIntegrity / MaxIntegrity;
					DeformationRatio /= MaxIntegrityRatio;
				}
				if (!MyPerGameSettings.Destruction)
				{
					Mass = num;
				}
			}
			else if (myObjectBuilder_CubeBlockDefinition.MaxIntegrity != 0)
			{
				MaxIntegrity = myObjectBuilder_CubeBlockDefinition.MaxIntegrity;
			}
			CriticalIntegrityRatio = num2 / MaxIntegrity;
			OwnershipIntegrityRatio = num3 / MaxIntegrity;
			if (myObjectBuilder_CubeBlockDefinition.BuildProgressModels != null)
			{
				myObjectBuilder_CubeBlockDefinition.BuildProgressModels.Sort((MyObjectBuilder_CubeBlockDefinition.BuildProgressModel a, MyObjectBuilder_CubeBlockDefinition.BuildProgressModel b) => a.BuildPercentUpperBound.CompareTo(b.BuildPercentUpperBound));
				BuildProgressModels = new BuildProgressModel[myObjectBuilder_CubeBlockDefinition.BuildProgressModels.Count];
				for (int num6 = 0; num6 < BuildProgressModels.Length; num6++)
				{
					MyObjectBuilder_CubeBlockDefinition.BuildProgressModel buildProgressModel = myObjectBuilder_CubeBlockDefinition.BuildProgressModels[num6];
					if (!string.IsNullOrEmpty(buildProgressModel.File))
					{
						BuildProgressModels[num6] = new BuildProgressModel
						{
							BuildRatioUpperBound = ((CriticalIntegrityRatio > 0f) ? (buildProgressModel.BuildPercentUpperBound * CriticalIntegrityRatio) : buildProgressModel.BuildPercentUpperBound),
							File = buildProgressModel.File,
							RandomOrientation = buildProgressModel.RandomOrientation
						};
					}
				}
			}
			if (myObjectBuilder_CubeBlockDefinition.GeneratedBlocks != null)
			{
				GeneratedBlockDefinitions = new MyDefinitionId[myObjectBuilder_CubeBlockDefinition.GeneratedBlocks.Length];
				for (int num7 = 0; num7 < myObjectBuilder_CubeBlockDefinition.GeneratedBlocks.Length; num7++)
				{
					SerializableDefinitionId v = myObjectBuilder_CubeBlockDefinition.GeneratedBlocks[num7];
					GeneratedBlockDefinitions[num7] = v;
				}
			}
			Skeleton = myObjectBuilder_CubeBlockDefinition.Skeleton;
			if (Skeleton != null)
			{
				Bones = new Dictionary<Vector3I, Vector3>(myObjectBuilder_CubeBlockDefinition.Skeleton.Count);
				foreach (BoneInfo item in Skeleton)
				{
					Bones[item.BonePosition] = Vector3UByte.Denormalize(item.BoneOffset, MyDefinitionManager.Static.GetCubeSize(myObjectBuilder_CubeBlockDefinition.CubeSize));
				}
			}
			IsAirTight = myObjectBuilder_CubeBlockDefinition.IsAirTight;
			IsStandAlone = myObjectBuilder_CubeBlockDefinition.IsStandAlone;
			HasPhysics = myObjectBuilder_CubeBlockDefinition.HasPhysics;
			UseNeighbourOxygenRooms = myObjectBuilder_CubeBlockDefinition.UseNeighbourOxygenRooms;
			InitMountPoints(myObjectBuilder_CubeBlockDefinition);
			InitPressurization();
			InitNavigationInfo(myObjectBuilder_CubeBlockDefinition, myObjectBuilder_CubeBlockDefinition.NavigationDefinition);
			PrimarySound = new MySoundPair(myObjectBuilder_CubeBlockDefinition.PrimarySound);
			ActionSound = new MySoundPair(myObjectBuilder_CubeBlockDefinition.ActionSound);
			if (myObjectBuilder_CubeBlockDefinition.DamagedSound != null && myObjectBuilder_CubeBlockDefinition.DamagedSound.Length > 0)
			{
				DamagedSound = new MySoundPair(myObjectBuilder_CubeBlockDefinition.DamagedSound);
			}
			if (myObjectBuilder_CubeBlockDefinition.DestroySound != null && myObjectBuilder_CubeBlockDefinition.DestroySound.Length > 0)
			{
				DestroySound = new MySoundPair(myObjectBuilder_CubeBlockDefinition.DestroySound);
			}
			PCU = myObjectBuilder_CubeBlockDefinition.PCU;
			PlaceDecals = myObjectBuilder_CubeBlockDefinition.PlaceDecals;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_CubeBlockDefinition myObjectBuilder_CubeBlockDefinition = (MyObjectBuilder_CubeBlockDefinition)base.GetObjectBuilder();
			myObjectBuilder_CubeBlockDefinition.Size = Size;
			myObjectBuilder_CubeBlockDefinition.Model = Model;
			myObjectBuilder_CubeBlockDefinition.UseModelIntersection = UseModelIntersection;
			myObjectBuilder_CubeBlockDefinition.CubeSize = CubeSize;
			myObjectBuilder_CubeBlockDefinition.SilenceableByShipSoundSystem = SilenceableByShipSoundSystem;
			myObjectBuilder_CubeBlockDefinition.ModelOffset = ModelOffset;
			myObjectBuilder_CubeBlockDefinition.BlockTopology = BlockTopology;
			myObjectBuilder_CubeBlockDefinition.PhysicsOption = PhysicsOption;
			myObjectBuilder_CubeBlockDefinition.BlockPairName = BlockPairName;
			myObjectBuilder_CubeBlockDefinition.Center = m_center;
			myObjectBuilder_CubeBlockDefinition.MirroringX = m_symmetryX;
			myObjectBuilder_CubeBlockDefinition.MirroringY = m_symmetryY;
			myObjectBuilder_CubeBlockDefinition.MirroringZ = m_symmetryZ;
			myObjectBuilder_CubeBlockDefinition.UsesDeformation = UsesDeformation;
			myObjectBuilder_CubeBlockDefinition.DeformationRatio = DeformationRatio;
			myObjectBuilder_CubeBlockDefinition.EdgeType = EdgeType;
			myObjectBuilder_CubeBlockDefinition.AutorotateMode = AutorotateMode;
			myObjectBuilder_CubeBlockDefinition.MirroringBlock = m_mirroringBlock;
			myObjectBuilder_CubeBlockDefinition.MultiBlock = MultiBlock;
			myObjectBuilder_CubeBlockDefinition.GuiVisible = GuiVisible;
			myObjectBuilder_CubeBlockDefinition.Rotation = Rotation;
			myObjectBuilder_CubeBlockDefinition.Direction = Direction;
			myObjectBuilder_CubeBlockDefinition.Mirrored = Mirrored;
			myObjectBuilder_CubeBlockDefinition.BuildType = BuildType.ToString();
			myObjectBuilder_CubeBlockDefinition.BuildMaterial = BuildMaterial;
			myObjectBuilder_CubeBlockDefinition.GeneratedBlockType = GeneratedBlockType.ToString();
			myObjectBuilder_CubeBlockDefinition.DamageEffectName = DamageEffectName;
			myObjectBuilder_CubeBlockDefinition.DestroyEffect = ((DestroyEffect.Length > 0) ? DestroyEffect : "");
			myObjectBuilder_CubeBlockDefinition.DestroyEffectOffset = DestroyEffectOffset;
			myObjectBuilder_CubeBlockDefinition.Icons = Icons;
			myObjectBuilder_CubeBlockDefinition.VoxelPlacement = VoxelPlacement;
			myObjectBuilder_CubeBlockDefinition.GeneralDamageMultiplier = GeneralDamageMultiplier;
			if (PhysicalMaterial != null)
			{
				myObjectBuilder_CubeBlockDefinition.PhysicalMaterial = PhysicalMaterial.Id.SubtypeName;
			}
			myObjectBuilder_CubeBlockDefinition.CompoundEnabled = CompoundEnabled;
			myObjectBuilder_CubeBlockDefinition.PCU = PCU;
			myObjectBuilder_CubeBlockDefinition.PlaceDecals = PlaceDecals;
			if (Components != null)
			{
				List<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent> list = new List<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>();
				Component[] components = Components;
				foreach (Component component in components)
				{
					MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent cubeBlockComponent = new MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent();
					cubeBlockComponent.Count = (ushort)component.Count;
					cubeBlockComponent.Type = component.Definition.Id.TypeId;
					cubeBlockComponent.Subtype = component.Definition.Id.SubtypeName;
					list.Add(cubeBlockComponent);
				}
				myObjectBuilder_CubeBlockDefinition.Components = list.ToArray();
			}
			myObjectBuilder_CubeBlockDefinition.CriticalComponent = new MyObjectBuilder_CubeBlockDefinition.CriticalPart
			{
				Index = 0,
				Subtype = myObjectBuilder_CubeBlockDefinition.Components[0].Subtype,
				Type = myObjectBuilder_CubeBlockDefinition.Components[0].Type
			};
			List<MyObjectBuilder_CubeBlockDefinition.MountPoint> list2 = null;
			if (MountPoints != null)
			{
				list2 = new List<MyObjectBuilder_CubeBlockDefinition.MountPoint>();
				MountPoint[] mountPoints = MountPoints;
				foreach (MountPoint mountPoint in mountPoints)
				{
					MyObjectBuilder_CubeBlockDefinition.MountPoint objectBuilder = mountPoint.GetObjectBuilder(Size);
					list2.Add(objectBuilder);
				}
				myObjectBuilder_CubeBlockDefinition.MountPoints = list2.ToArray();
			}
			return myObjectBuilder_CubeBlockDefinition;
		}

		public bool RatioEnoughForOwnership(float ratio)
		{
			return ratio >= OwnershipIntegrityRatio;
		}

		public bool RatioEnoughForDamageEffect(float ratio)
		{
			return ratio < CriticalIntegrityRatio;
		}

		public bool ModelChangeIsNeeded(float percentageA, float percentageB)
		{
			if (percentageA >= percentageB)
			{
				return false;
			}
			if (percentageA == 0f)
			{
				return true;
			}
			if (BuildProgressModels == null)
			{
				return false;
			}
			int i;
			for (i = 0; i < BuildProgressModels.Length && !(percentageA <= BuildProgressModels[i].BuildRatioUpperBound); i++)
			{
			}
			if (i >= BuildProgressModels.Length)
			{
				return false;
			}
			if (percentageB >= BuildProgressModels[i].BuildRatioUpperBound)
			{
				return true;
			}
			return false;
		}

		public float FinalModelThreshold()
		{
			if (BuildProgressModels == null || BuildProgressModels.Length == 0)
			{
				return 0f;
			}
			return BuildProgressModels[BuildProgressModels.Length - 1].BuildRatioUpperBound;
		}

		[Conditional("DEBUG")]
		private void CheckBuildProgressModels()
		{
			if (BuildProgressModels == null)
			{
				return;
			}
			BuildProgressModel[] buildProgressModels = BuildProgressModels;
			foreach (BuildProgressModel buildProgressModel in buildProgressModels)
			{
				if (buildProgressModel != null)
				{
					string file = buildProgressModel.File;
					if (!Path.IsPathRooted(file))
					{
						Path.Combine(MyFileSystem.ContentPath, file);
					}
				}
			}
		}

		internal static void TransformMountPointPosition(ref Vector3 position, int wallIndex, Vector3I cubeSize, out Vector3 result)
		{
			Vector3.Transform(ref position, ref m_mountPointTransforms[wallIndex], out result);
			result += m_mountPointWallOffsets[wallIndex] * cubeSize;
		}

		internal static void UntransformMountPointPosition(ref Vector3 position, int wallIndex, Vector3I cubeSize, out Vector3 result)
		{
			Vector3 position2 = position - m_mountPointWallOffsets[wallIndex] * cubeSize;
			Matrix matrix = Matrix.Invert(m_mountPointTransforms[wallIndex]);
			Vector3.Transform(ref position2, ref matrix, out result);
		}

		public static int GetMountPointWallIndex(Base6Directions.Direction direction)
		{
			return m_mountPointWallIndices[(uint)direction];
		}

		public Vector3 MountPointLocalToBlockLocal(Vector3 coord, Base6Directions.Direction mountPointDirection)
		{
			Vector3 result = default(Vector3);
			int wallIndex = m_mountPointWallIndices[(uint)mountPointDirection];
			TransformMountPointPosition(ref coord, wallIndex, Size, out result);
			return result - Center;
		}

		public Vector3 MountPointLocalNormalToBlockLocal(Vector3 normal, Base6Directions.Direction mountPointDirection)
		{
			Vector3 result = default(Vector3);
			int num = m_mountPointWallIndices[(uint)mountPointDirection];
			Vector3.TransformNormal(ref normal, ref m_mountPointTransforms[num], out result);
			return result;
		}

		public static BlockSideEnum NormalToBlockSide(Vector3I normal)
		{
			for (int i = 0; i < m_mountPointTransforms.Length; i++)
			{
				Vector3I b = new Vector3I(m_mountPointTransforms[i].Forward);
				if (normal == b)
				{
					return (BlockSideEnum)i;
				}
			}
			return BlockSideEnum.Right;
		}

		private void InitMountPoints(MyObjectBuilder_CubeBlockDefinition def)
		{
			if (MountPoints != null)
			{
				return;
			}
			_ = (Size - 1) / 2;
			if (!Context.IsBaseGame && def.MountPoints != null && def.MountPoints.Length == 0)
			{
				def.MountPoints = null;
				string message = "Obsolete default definition of mount points in " + def.Id;
				MyDefinitionErrors.Add(Context, message, TErrorSeverity.Warning);
			}
			if (def.MountPoints == null)
			{
				List<MountPoint> list = new List<MountPoint>(6);
				Vector3I.TransformNormal(ref Vector3I.Forward, ref m_mountPointTransforms[0], out Vector3I result);
				Vector3I.TransformNormal(ref Vector3I.Forward, ref m_mountPointTransforms[1], out Vector3I result2);
				Vector3I.TransformNormal(ref Vector3I.Forward, ref m_mountPointTransforms[2], out Vector3I result3);
				Vector3I.TransformNormal(ref Vector3I.Forward, ref m_mountPointTransforms[3], out Vector3I result4);
				Vector3I.TransformNormal(ref Vector3I.Forward, ref m_mountPointTransforms[4], out Vector3I result5);
				Vector3I.TransformNormal(ref Vector3I.Forward, ref m_mountPointTransforms[5], out Vector3I result6);
				Vector3 position = new Vector3(0.001f, 0.001f, 0.0004f);
				Vector3 position2 = new Vector3((float)Size.Z - 0.001f, (float)Size.Y - 0.001f, -0.0004f);
				TransformMountPointPosition(ref position, 0, Size, out Vector3 result7);
				TransformMountPointPosition(ref position2, 0, Size, out Vector3 result8);
				TransformMountPointPosition(ref position, 3, Size, out Vector3 result9);
				TransformMountPointPosition(ref position2, 3, Size, out Vector3 result10);
				MountPoint item = new MountPoint
				{
					Start = result7,
					End = result8,
					Normal = result,
					Enabled = true
				};
				list.Add(item);
				item = new MountPoint
				{
					Start = result9,
					End = result10,
					Normal = result4,
					Enabled = true
				};
				list.Add(item);
				Vector3 position3 = new Vector3(0.001f, 0.001f, 0.0004f);
				Vector3 position4 = new Vector3((float)Size.X - 0.001f, (float)Size.Z - 0.001f, -0.0004f);
				TransformMountPointPosition(ref position3, 1, Size, out Vector3 result11);
				TransformMountPointPosition(ref position4, 1, Size, out Vector3 result12);
				TransformMountPointPosition(ref position3, 4, Size, out Vector3 result13);
				TransformMountPointPosition(ref position4, 4, Size, out Vector3 result14);
				item = new MountPoint
				{
					Start = result11,
					End = result12,
					Normal = result2,
					Enabled = true
				};
				list.Add(item);
				item = new MountPoint
				{
					Start = result13,
					End = result14,
					Normal = result5,
					Enabled = true
				};
				list.Add(item);
				Vector3 position5 = new Vector3(0.001f, 0.001f, 0.0004f);
				Vector3 position6 = new Vector3((float)Size.X - 0.001f, (float)Size.Y - 0.001f, -0.0004f);
				TransformMountPointPosition(ref position5, 2, Size, out Vector3 result15);
				TransformMountPointPosition(ref position6, 2, Size, out Vector3 result16);
				TransformMountPointPosition(ref position5, 5, Size, out Vector3 result17);
				TransformMountPointPosition(ref position6, 5, Size, out Vector3 result18);
				item = new MountPoint
				{
					Start = result15,
					End = result16,
					Normal = result3,
					Enabled = true
				};
				list.Add(item);
				item = new MountPoint
				{
					Start = result17,
					End = result18,
					Normal = result6,
					Enabled = true
				};
				list.Add(item);
				MountPoints = list.ToArray();
				return;
			}
			SetMountPoints(ref MountPoints, def.MountPoints, m_tmpMounts);
			if (def.BuildProgressModels != null)
			{
				for (int i = 0; i < def.BuildProgressModels.Count; i++)
				{
					BuildProgressModel buildProgressModel = BuildProgressModels[i];
					if (buildProgressModel == null)
					{
						continue;
					}
					MyObjectBuilder_CubeBlockDefinition.BuildProgressModel buildProgressModel2 = def.BuildProgressModels[i];
					if (buildProgressModel2.MountPoints == null)
					{
						continue;
					}
					MyObjectBuilder_CubeBlockDefinition.MountPoint[] mountPoints = buildProgressModel2.MountPoints;
					foreach (MyObjectBuilder_CubeBlockDefinition.MountPoint mountPoint in mountPoints)
					{
						int sideId = (int)mountPoint.Side;
						if (!m_tmpIndices.Contains(sideId))
						{
							m_tmpMounts.RemoveAll((MyObjectBuilder_CubeBlockDefinition.MountPoint mount) => mount.Side == (BlockSideEnum)sideId);
							m_tmpIndices.Add(sideId);
						}
						m_tmpMounts.Add(mountPoint);
					}
					m_tmpIndices.Clear();
					buildProgressModel.MountPoints = new MountPoint[m_tmpMounts.Count];
					SetMountPoints(ref buildProgressModel.MountPoints, m_tmpMounts.ToArray(), null);
				}
			}
			m_tmpMounts.Clear();
		}

		private void SetMountPoints(ref MountPoint[] mountPoints, MyObjectBuilder_CubeBlockDefinition.MountPoint[] mpBuilders, List<MyObjectBuilder_CubeBlockDefinition.MountPoint> addedMounts)
		{
			if (mountPoints == null)
			{
				mountPoints = new MountPoint[mpBuilders.Length];
			}
			for (int i = 0; i < mountPoints.Length; i++)
			{
				MyObjectBuilder_CubeBlockDefinition.MountPoint mountPoint = mpBuilders[i];
				addedMounts?.Add(mountPoint);
				Vector3 position = new Vector3(Vector2.Min(mountPoint.Start, mountPoint.End) + 0.001f, 0.0004f);
				Vector3 position2 = new Vector3(Vector2.Max(mountPoint.Start, mountPoint.End) - 0.001f, -0.0004f);
				int side = (int)mountPoint.Side;
				Vector3I normal = Vector3I.Forward;
				TransformMountPointPosition(ref position, side, Size, out position);
				TransformMountPointPosition(ref position2, side, Size, out position2);
				Vector3I.TransformNormal(ref normal, ref m_mountPointTransforms[side], out normal);
				mountPoints[i].Start = position;
				mountPoints[i].End = position2;
				mountPoints[i].Normal = normal;
				mountPoints[i].ExclusionMask = mountPoint.ExclusionMask;
				mountPoints[i].PropertiesMask = mountPoint.PropertiesMask;
				mountPoints[i].Enabled = mountPoint.Enabled;
				mountPoints[i].Default = mountPoint.Default;
			}
		}

		public MountPoint[] GetBuildProgressModelMountPoints(float currentIntegrityRatio)
		{
			if (BuildProgressModels == null || BuildProgressModels.Length == 0 || currentIntegrityRatio >= BuildProgressModels[BuildProgressModels.Length - 1].BuildRatioUpperBound)
			{
				return MountPoints;
			}
			int i;
			for (i = 0; i < BuildProgressModels.Length - 1; i++)
			{
				BuildProgressModel buildProgressModel = BuildProgressModels[i];
				if (currentIntegrityRatio <= buildProgressModel.BuildRatioUpperBound)
				{
					break;
				}
			}
			return BuildProgressModels[i].MountPoints ?? MountPoints;
		}

		public void InitPressurization()
		{
			IsCubePressurized = new Dictionary<Vector3I, Dictionary<Vector3I, bool>>();
			for (int i = 0; i < Size.X; i++)
			{
				for (int j = 0; j < Size.Y; j++)
				{
					for (int k = 0; k < Size.Z; k++)
					{
						Vector3 position = new Vector3(i, j, k);
						Vector3 position2 = new Vector3(i, j, k) + Vector3.One;
						Vector3I key = new Vector3I(i, j, k);
						IsCubePressurized[key] = new Dictionary<Vector3I, bool>();
						Vector3I[] intDirections = Base6Directions.IntDirections;
						for (int l = 0; l < intDirections.Length; l++)
						{
							Vector3I vec = intDirections[l];
							IsCubePressurized[key][vec] = false;
							if ((vec.X == 1 && i != Size.X - 1) || (vec.X == -1 && i != 0) || (vec.Y == 1 && j != Size.Y - 1) || (vec.Y == -1 && j != 0) || (vec.Z == 1 && k != Size.Z - 1) || (vec.Z == -1 && k != 0))
							{
								continue;
							}
							MountPoint[] mountPoints = MountPoints;
							for (int m = 0; m < mountPoints.Length; m++)
							{
								MountPoint mountPoint = mountPoints[m];
								if (vec == mountPoint.Normal)
								{
									int mountPointWallIndex = GetMountPointWallIndex(Base6Directions.GetDirection(ref vec));
									Vector3I size = Size;
									Vector3 position3 = mountPoint.Start;
									Vector3 position4 = mountPoint.End;
									UntransformMountPointPosition(ref position3, mountPointWallIndex, size, out Vector3 result);
									UntransformMountPointPosition(ref position4, mountPointWallIndex, size, out Vector3 result2);
									UntransformMountPointPosition(ref position, mountPointWallIndex, size, out Vector3 result3);
									UntransformMountPointPosition(ref position2, mountPointWallIndex, size, out Vector3 result4);
									Vector3 vector = new Vector3(Math.Max(result3.X, result4.X), Math.Max(result3.Y, result4.Y), Math.Max(result3.Z, result4.Z));
									Vector3 vector2 = new Vector3(Math.Min(result3.X, result4.X), Math.Min(result3.Y, result4.Y), Math.Min(result3.Z, result4.Z));
									if ((double)result.X - 0.05 <= (double)vector2.X && (double)result2.X + 0.05 > (double)vector.X && (double)result.Y - 0.05 <= (double)vector2.Y && (double)result2.Y + 0.05 > (double)vector.Y)
									{
										IsCubePressurized[key][vec] = true;
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		public void InitNavigationInfo(MyObjectBuilder_CubeBlockDefinition blockDef, string infoSubtypeId)
		{
			if (MyPerGameSettings.EnableAi)
			{
				if (infoSubtypeId == "Default")
				{
					MyDefinitionManager.Static.SetDefaultNavDef(this);
				}
				else
				{
					MyDefinitionId defId = new MyDefinitionId(typeof(MyObjectBuilder_BlockNavigationDefinition), infoSubtypeId);
					MyDefinitionManager.Static.TryGetDefinition(defId, out NavigationDefinition);
				}
				if (NavigationDefinition != null && NavigationDefinition.Mesh != null)
				{
					NavigationDefinition.Mesh.MakeStatic();
				}
			}
		}

		private void InitEntityComponents(MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] entityComponentDefinitions)
		{
			if (entityComponentDefinitions == null)
			{
				return;
			}
			EntityComponents = new Dictionary<string, MyObjectBuilder_ComponentBase>(entityComponentDefinitions.Length);
			foreach (MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition entityComponentDefinition in entityComponentDefinitions)
			{
				MyObjectBuilderType type = MyObjectBuilderType.Parse(entityComponentDefinition.BuilderType);
				if (!type.IsNull)
				{
					MyObjectBuilder_ComponentBase myObjectBuilder_ComponentBase = MyObjectBuilderSerializer.CreateNewObject(type) as MyObjectBuilder_ComponentBase;
					if (myObjectBuilder_ComponentBase != null)
					{
						EntityComponents.Add(entityComponentDefinition.ComponentType, myObjectBuilder_ComponentBase);
					}
				}
			}
		}

		public bool ContainsComputer()
		{
			return Components.Count((Component x) => x.Definition.Id.TypeId == typeof(MyObjectBuilder_Component) && x.Definition.Id.SubtypeName == "Computer") > 0;
		}

		public MyCubeBlockDefinition GetGeneratedBlockDefinition(MyStringId additionalModelType)
		{
			if (GeneratedBlockDefinitions == null)
			{
				return null;
			}
			MyDefinitionId[] generatedBlockDefinitions = GeneratedBlockDefinitions;
			foreach (MyDefinitionId defId in generatedBlockDefinitions)
			{
				MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out MyCubeBlockDefinition blockDefinition);
				if (blockDefinition != null && blockDefinition.IsGeneratedBlock && blockDefinition.GeneratedBlockType == additionalModelType)
				{
					return blockDefinition;
				}
			}
			return null;
		}

		public static void PreloadConstructionModels(MyCubeBlockDefinition block)
		{
			if (block == null || m_preloadedDefinitions.Contains(block))
			{
				return;
			}
			List<string> list = m_stringPool.Allocate();
			list.Clear();
			for (int i = 0; i < block.BuildProgressModels.Length; i++)
			{
				BuildProgressModel buildProgressModel = block.BuildProgressModels[i];
				if (buildProgressModel != null && !string.IsNullOrEmpty(buildProgressModel.File))
				{
					list.Add(buildProgressModel.File);
				}
			}
			MyRenderProxy.PreloadModels(list, forInstancedComponent: true);
			m_stringPool.Deallocate(list);
			m_preloadedDefinitions.Add(block);
		}

		public static void ClearPreloadedConstructionModels()
		{
			m_preloadedDefinitions.Clear();
		}
	}
}
