using Sandbox.Game.Entities.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_CharacterDefinition), null)]
	public class MyCharacterDefinition : MyDefinitionBase
	{
		public class RagdollBoneSet
		{
			public string[] Bones;

			public float CollisionRadius;

			public RagdollBoneSet(string bones, float radius)
			{
				Bones = bones.Split(new char[1]
				{
					' '
				});
				CollisionRadius = radius;
			}
		}

		private class Sandbox_Definitions_MyCharacterDefinition_003C_003EActor : IActivator, IActivator<MyCharacterDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterDefinition CreateInstance()
			{
				return new MyCharacterDefinition();
			}

			MyCharacterDefinition IActivator<MyCharacterDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Name;

		public string Model;

		public string ReflectorTexture;

		public string LeftGlare;

		public string RightGlare;

		public string LeftLightBone;

		public string RightLightBone;

		public Vector3 LightOffset;

		public float LightGlareSize;

		public string HeadBone;

		public string Camera3rdBone;

		public string LeftHandIKStartBone;

		public string LeftHandIKEndBone;

		public string RightHandIKStartBone;

		public string RightHandIKEndBone;

		public string WeaponBone;

		public string LeftHandItemBone;

		public string Skeleton;

		public string LeftForearmBone;

		public string LeftUpperarmBone;

		public string RightForearmBone;

		public string RightUpperarmBone;

		public string SpineBone;

		public float BendMultiplier1st;

		public float BendMultiplier3rd;

		public bool UsesAtmosphereDetector;

		public bool UsesReverbDetector;

		[Obsolete("Dont ever use again.")]
		public bool NeedsOxygen;

		public float OxygenConsumptionMultiplier;

		public float OxygenConsumption;

		public float OxygenSuitRefillTime;

		public float MinOxygenLevelForSuitRefill;

		public float PressureLevelForLowDamage;

		public float DamageAmountAtZeroPressure;

		public bool LoopingFootsteps;

		public bool VisibleOnHud;

		public bool UsableByPlayer;

		public string PhysicalMaterial;

		public float JumpForce;

		public bool EnableFirstPersonView;

		public string JumpSoundName;

		public string JetpackIdleSoundName;

		public string JetpackRunSoundName;

		public string CrouchDownSoundName;

		public string CrouchUpSoundName;

		public string PainSoundName;

		public string SuffocateSoundName;

		public string DeathSoundName;

		public string DeathBySuffocationSoundName;

		public string IronsightActSoundName;

		public string IronsightDeactSoundName;

		public string FastFlySoundName;

		public string HelmetOxygenNormalSoundName;

		public string HelmetOxygenLowSoundName;

		public string HelmetOxygenCriticalSoundName;

		public string HelmetOxygenNoneSoundName;

		public string MovementSoundName;

		public string MagnetBootsStartSoundName;

		public string MagnetBootsEndSoundName;

		public string MagnetBootsStepsSoundName;

		public string MagnetBootsProximitySoundName;

		public string BreathCalmSoundName;

		public string BreathHeavySoundName;

		public string OxygenChokeNormalSoundName;

		public string OxygenChokeLowSoundName;

		public string OxygenChokeCriticalSoundName;

		public bool FeetIKEnabled;

		public string ModelRootBoneName;

		public string LeftHipBoneName;

		public string LeftKneeBoneName;

		public string LeftAnkleBoneName;

		public string RightHipBoneName;

		public string RightKneeBoneName;

		public string RightAnkleBoneName;

		public string RagdollDataFile;

		public Dictionary<string, RagdollBoneSet> RagdollBonesMappings = new Dictionary<string, RagdollBoneSet>();

		public Dictionary<string, string[]> RagdollPartialSimulations = new Dictionary<string, string[]>();

		public string RagdollRootBody;

		public Dictionary<MyCharacterMovementEnum, MyFeetIKSettings> FeetIKSettings;

		public List<SuitResourceDefinition> SuitResourceStorage;

		public MyObjectBuilder_JetpackDefinition Jetpack;

		public Dictionary<string, string[]> BoneSets = new Dictionary<string, string[]>();

		public Dictionary<float, string[]> BoneLODs = new Dictionary<float, string[]>();

		public Dictionary<string, string> AnimationNameToSubtypeName = new Dictionary<string, string>();

		public string[] MaterialsDisabledIn1st;

		public float Mass;

		public float ImpulseLimit;

		public string RighHandItemBone;

		public bool VerticalPositionFlyingOnly;

		public bool UseOnlyWalking;

		public float MaxSlope;

		public float MaxSprintSpeed;

		public float MaxRunSpeed;

		public float MaxBackrunSpeed;

		public float MaxRunStrafingSpeed;

		public float MaxWalkSpeed;

		public float MaxBackwalkSpeed;

		public float MaxWalkStrafingSpeed;

		public float MaxCrouchWalkSpeed;

		public float MaxCrouchBackwalkSpeed;

		public float MaxCrouchStrafingSpeed;

		public float CharacterHeadSize;

		public float CharacterHeadHeight;

		public float CharacterCollisionScale;

		public float CharacterCollisionHeight;

		public float CharacterCollisionWidth;

		public float CharacterCollisionCrouchHeight;

		public float CharacterWidth;

		public float CharacterHeight;

		public float CharacterLength;

		public bool CanCrouch;

		public bool CanIronsight;

		public MyObjectBuilder_InventoryDefinition InventoryDefinition;

		public bool EnableSpawnInventoryAsContainer;

		public MyDefinitionId? InventorySpawnContainerId;

		public bool SpawnInventoryOnBodyRemoval;

		[Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
		public List<string> EnabledComponents = new List<string>();

		public float LootingTime;

		public string InitialAnimation;

		public MyObjectBuilder_DeadBodyShape DeadBodyShape;

		public string AnimationController;

		public float? MaxForce;

		public MyEnumCharacterRotationToSupport RotationToSupport;

		public string HUD;

		public float SuitConsumptionInTemperatureExtreme;

		public bool UseNewAnimationSystem => AnimationController != null;

		protected override void Init(MyObjectBuilder_DefinitionBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_CharacterDefinition myObjectBuilder_CharacterDefinition = (MyObjectBuilder_CharacterDefinition)objectBuilder;
			Name = myObjectBuilder_CharacterDefinition.Name;
			Model = myObjectBuilder_CharacterDefinition.Model;
			ReflectorTexture = myObjectBuilder_CharacterDefinition.ReflectorTexture;
			LeftGlare = myObjectBuilder_CharacterDefinition.LeftGlare;
			RightGlare = myObjectBuilder_CharacterDefinition.RightGlare;
			LeftLightBone = myObjectBuilder_CharacterDefinition.LeftLightBone;
			RightLightBone = myObjectBuilder_CharacterDefinition.RightLightBone;
			LightOffset = myObjectBuilder_CharacterDefinition.LightOffset;
			LightGlareSize = myObjectBuilder_CharacterDefinition.LightGlareSize;
			HeadBone = myObjectBuilder_CharacterDefinition.HeadBone;
			Camera3rdBone = myObjectBuilder_CharacterDefinition.Camera3rdBone;
			LeftHandIKStartBone = myObjectBuilder_CharacterDefinition.LeftHandIKStartBone;
			LeftHandIKEndBone = myObjectBuilder_CharacterDefinition.LeftHandIKEndBone;
			RightHandIKStartBone = myObjectBuilder_CharacterDefinition.RightHandIKStartBone;
			RightHandIKEndBone = myObjectBuilder_CharacterDefinition.RightHandIKEndBone;
			WeaponBone = myObjectBuilder_CharacterDefinition.WeaponBone;
			LeftHandItemBone = myObjectBuilder_CharacterDefinition.LeftHandItemBone;
			RighHandItemBone = myObjectBuilder_CharacterDefinition.RightHandItemBone;
			Skeleton = myObjectBuilder_CharacterDefinition.Skeleton;
			LeftForearmBone = myObjectBuilder_CharacterDefinition.LeftForearmBone;
			LeftUpperarmBone = myObjectBuilder_CharacterDefinition.LeftUpperarmBone;
			RightForearmBone = myObjectBuilder_CharacterDefinition.RightForearmBone;
			RightUpperarmBone = myObjectBuilder_CharacterDefinition.RightUpperarmBone;
			SpineBone = myObjectBuilder_CharacterDefinition.SpineBone;
			BendMultiplier1st = myObjectBuilder_CharacterDefinition.BendMultiplier1st;
			BendMultiplier3rd = myObjectBuilder_CharacterDefinition.BendMultiplier3rd;
			MaterialsDisabledIn1st = myObjectBuilder_CharacterDefinition.MaterialsDisabledIn1st;
			FeetIKEnabled = myObjectBuilder_CharacterDefinition.FeetIKEnabled;
			ModelRootBoneName = myObjectBuilder_CharacterDefinition.ModelRootBoneName;
			LeftHipBoneName = myObjectBuilder_CharacterDefinition.LeftHipBoneName;
			LeftKneeBoneName = myObjectBuilder_CharacterDefinition.LeftKneeBoneName;
			LeftAnkleBoneName = myObjectBuilder_CharacterDefinition.LeftAnkleBoneName;
			RightHipBoneName = myObjectBuilder_CharacterDefinition.RightHipBoneName;
			RightKneeBoneName = myObjectBuilder_CharacterDefinition.RightKneeBoneName;
			RightAnkleBoneName = myObjectBuilder_CharacterDefinition.RightAnkleBoneName;
			UsesAtmosphereDetector = myObjectBuilder_CharacterDefinition.UsesAtmosphereDetector;
			UsesReverbDetector = myObjectBuilder_CharacterDefinition.UsesReverbDetector;
			NeedsOxygen = myObjectBuilder_CharacterDefinition.NeedsOxygen;
			OxygenConsumptionMultiplier = myObjectBuilder_CharacterDefinition.OxygenConsumptionMultiplier;
			OxygenConsumption = myObjectBuilder_CharacterDefinition.OxygenConsumption;
			OxygenSuitRefillTime = myObjectBuilder_CharacterDefinition.OxygenSuitRefillTime;
			MinOxygenLevelForSuitRefill = myObjectBuilder_CharacterDefinition.MinOxygenLevelForSuitRefill;
			PressureLevelForLowDamage = myObjectBuilder_CharacterDefinition.PressureLevelForLowDamage;
			DamageAmountAtZeroPressure = myObjectBuilder_CharacterDefinition.DamageAmountAtZeroPressure;
			RagdollDataFile = myObjectBuilder_CharacterDefinition.RagdollDataFile;
			JumpSoundName = myObjectBuilder_CharacterDefinition.JumpSoundName;
			JetpackIdleSoundName = myObjectBuilder_CharacterDefinition.JetpackIdleSoundName;
			JetpackRunSoundName = myObjectBuilder_CharacterDefinition.JetpackRunSoundName;
			CrouchDownSoundName = myObjectBuilder_CharacterDefinition.CrouchDownSoundName;
			CrouchUpSoundName = myObjectBuilder_CharacterDefinition.CrouchUpSoundName;
			PainSoundName = myObjectBuilder_CharacterDefinition.PainSoundName;
			SuffocateSoundName = myObjectBuilder_CharacterDefinition.SuffocateSoundName;
			DeathSoundName = myObjectBuilder_CharacterDefinition.DeathSoundName;
			DeathBySuffocationSoundName = myObjectBuilder_CharacterDefinition.DeathBySuffocationSoundName;
			IronsightActSoundName = myObjectBuilder_CharacterDefinition.IronsightActSoundName;
			IronsightDeactSoundName = myObjectBuilder_CharacterDefinition.IronsightDeactSoundName;
			FastFlySoundName = myObjectBuilder_CharacterDefinition.FastFlySoundName;
			HelmetOxygenNormalSoundName = myObjectBuilder_CharacterDefinition.HelmetOxygenNormalSoundName;
			HelmetOxygenLowSoundName = myObjectBuilder_CharacterDefinition.HelmetOxygenLowSoundName;
			HelmetOxygenCriticalSoundName = myObjectBuilder_CharacterDefinition.HelmetOxygenCriticalSoundName;
			HelmetOxygenNoneSoundName = myObjectBuilder_CharacterDefinition.HelmetOxygenNoneSoundName;
			MovementSoundName = myObjectBuilder_CharacterDefinition.MovementSoundName;
			MagnetBootsStartSoundName = myObjectBuilder_CharacterDefinition.MagnetBootsStartSoundName;
			MagnetBootsStepsSoundName = myObjectBuilder_CharacterDefinition.MagnetBootsStepsSoundName;
			MagnetBootsEndSoundName = myObjectBuilder_CharacterDefinition.MagnetBootsEndSoundName;
			MagnetBootsProximitySoundName = myObjectBuilder_CharacterDefinition.MagnetBootsProximitySoundName;
			LoopingFootsteps = myObjectBuilder_CharacterDefinition.LoopingFootsteps;
			VisibleOnHud = myObjectBuilder_CharacterDefinition.VisibleOnHud;
			UsableByPlayer = myObjectBuilder_CharacterDefinition.UsableByPlayer;
			RagdollRootBody = myObjectBuilder_CharacterDefinition.RagdollRootBody;
			InitialAnimation = myObjectBuilder_CharacterDefinition.InitialAnimation;
			PhysicalMaterial = myObjectBuilder_CharacterDefinition.PhysicalMaterial;
			JumpForce = myObjectBuilder_CharacterDefinition.JumpForce;
			RotationToSupport = myObjectBuilder_CharacterDefinition.RotationToSupport;
			HUD = myObjectBuilder_CharacterDefinition.HUD;
			EnableFirstPersonView = myObjectBuilder_CharacterDefinition.EnableFirstPersonView;
			BreathCalmSoundName = myObjectBuilder_CharacterDefinition.BreathCalmSoundName;
			BreathHeavySoundName = myObjectBuilder_CharacterDefinition.BreathHeavySoundName;
			OxygenChokeNormalSoundName = myObjectBuilder_CharacterDefinition.OxygenChokeNormalSoundName;
			OxygenChokeLowSoundName = myObjectBuilder_CharacterDefinition.OxygenChokeLowSoundName;
			OxygenChokeCriticalSoundName = myObjectBuilder_CharacterDefinition.OxygenChokeCriticalSoundName;
			FeetIKSettings = new Dictionary<MyCharacterMovementEnum, MyFeetIKSettings>();
			if (myObjectBuilder_CharacterDefinition.IKSettings != null)
			{
				MyObjectBuilder_MyFeetIKSettings[] iKSettings = myObjectBuilder_CharacterDefinition.IKSettings;
				foreach (MyObjectBuilder_MyFeetIKSettings myObjectBuilder_MyFeetIKSettings in iKSettings)
				{
					string[] array = myObjectBuilder_MyFeetIKSettings.MovementState.Split(new char[1]
					{
						','
					});
					for (int j = 0; j < array.Length; j++)
					{
						string text = array[j].Trim();
						if (text != "" && Enum.TryParse(text, ignoreCase: true, out MyCharacterMovementEnum result))
						{
							MyFeetIKSettings value = default(MyFeetIKSettings);
							value.Enabled = myObjectBuilder_MyFeetIKSettings.Enabled;
							value.AboveReachableDistance = myObjectBuilder_MyFeetIKSettings.AboveReachableDistance;
							value.BelowReachableDistance = myObjectBuilder_MyFeetIKSettings.BelowReachableDistance;
							value.VerticalShiftDownGain = myObjectBuilder_MyFeetIKSettings.VerticalShiftDownGain;
							value.VerticalShiftUpGain = myObjectBuilder_MyFeetIKSettings.VerticalShiftUpGain;
							value.FootSize = new Vector3(myObjectBuilder_MyFeetIKSettings.FootWidth, myObjectBuilder_MyFeetIKSettings.AnkleHeight, myObjectBuilder_MyFeetIKSettings.FootLenght);
							FeetIKSettings.Add(result, value);
						}
					}
				}
			}
			SuitResourceStorage = myObjectBuilder_CharacterDefinition.SuitResourceStorage;
			Jetpack = myObjectBuilder_CharacterDefinition.Jetpack;
			if (myObjectBuilder_CharacterDefinition.BoneSets != null)
			{
				BoneSets = myObjectBuilder_CharacterDefinition.BoneSets.ToDictionary((MyBoneSetDefinition x) => x.Name, (MyBoneSetDefinition x) => x.Bones.Split(new char[1]
				{
					' '
				}));
			}
			if (myObjectBuilder_CharacterDefinition.BoneLODs != null)
			{
				BoneLODs = myObjectBuilder_CharacterDefinition.BoneLODs.ToDictionary((MyBoneSetDefinition x) => Convert.ToSingle(x.Name), (MyBoneSetDefinition x) => x.Bones.Split(new char[1]
				{
					' '
				}));
			}
			if (myObjectBuilder_CharacterDefinition.AnimationMappings != null)
			{
				AnimationNameToSubtypeName = myObjectBuilder_CharacterDefinition.AnimationMappings.ToDictionary((MyMovementAnimationMapping mapping) => mapping.Name, (MyMovementAnimationMapping mapping) => mapping.AnimationSubtypeName);
			}
			if (myObjectBuilder_CharacterDefinition.RagdollBonesMappings != null)
			{
				RagdollBonesMappings = myObjectBuilder_CharacterDefinition.RagdollBonesMappings.ToDictionary((MyRagdollBoneSetDefinition x) => x.Name, (MyRagdollBoneSetDefinition x) => new RagdollBoneSet(x.Bones, x.CollisionRadius));
			}
			if (myObjectBuilder_CharacterDefinition.RagdollPartialSimulations != null)
			{
				RagdollPartialSimulations = myObjectBuilder_CharacterDefinition.RagdollPartialSimulations.ToDictionary((MyBoneSetDefinition x) => x.Name, (MyBoneSetDefinition x) => x.Bones.Split(new char[1]
				{
					' '
				}));
			}
			Mass = myObjectBuilder_CharacterDefinition.Mass;
			ImpulseLimit = myObjectBuilder_CharacterDefinition.ImpulseLimit;
			VerticalPositionFlyingOnly = myObjectBuilder_CharacterDefinition.VerticalPositionFlyingOnly;
			UseOnlyWalking = myObjectBuilder_CharacterDefinition.UseOnlyWalking;
			MaxSlope = myObjectBuilder_CharacterDefinition.MaxSlope;
			MaxSprintSpeed = myObjectBuilder_CharacterDefinition.MaxSprintSpeed;
			MaxRunSpeed = myObjectBuilder_CharacterDefinition.MaxRunSpeed;
			MaxBackrunSpeed = myObjectBuilder_CharacterDefinition.MaxBackrunSpeed;
			MaxRunStrafingSpeed = myObjectBuilder_CharacterDefinition.MaxRunStrafingSpeed;
			MaxWalkSpeed = myObjectBuilder_CharacterDefinition.MaxWalkSpeed;
			MaxBackwalkSpeed = myObjectBuilder_CharacterDefinition.MaxBackwalkSpeed;
			MaxWalkStrafingSpeed = myObjectBuilder_CharacterDefinition.MaxWalkStrafingSpeed;
			MaxCrouchWalkSpeed = myObjectBuilder_CharacterDefinition.MaxCrouchWalkSpeed;
			MaxCrouchBackwalkSpeed = myObjectBuilder_CharacterDefinition.MaxCrouchBackwalkSpeed;
			MaxCrouchStrafingSpeed = myObjectBuilder_CharacterDefinition.MaxCrouchStrafingSpeed;
			CharacterHeadSize = myObjectBuilder_CharacterDefinition.CharacterHeadSize;
			CharacterHeadHeight = myObjectBuilder_CharacterDefinition.CharacterHeadHeight;
			CharacterCollisionScale = myObjectBuilder_CharacterDefinition.CharacterCollisionScale;
			CharacterCollisionWidth = myObjectBuilder_CharacterDefinition.CharacterCollisionWidth;
			CharacterCollisionHeight = myObjectBuilder_CharacterDefinition.CharacterCollisionHeight;
			CharacterCollisionCrouchHeight = myObjectBuilder_CharacterDefinition.CharacterCollisionCrouchHeight;
			CanCrouch = myObjectBuilder_CharacterDefinition.CanCrouch;
			CanIronsight = myObjectBuilder_CharacterDefinition.CanIronsight;
			if (myObjectBuilder_CharacterDefinition.Inventory == null)
			{
				InventoryDefinition = new MyObjectBuilder_InventoryDefinition();
			}
			else
			{
				InventoryDefinition = myObjectBuilder_CharacterDefinition.Inventory;
			}
			if (myObjectBuilder_CharacterDefinition.EnabledComponents != null)
			{
				EnabledComponents = myObjectBuilder_CharacterDefinition.EnabledComponents.Split(new char[1]
				{
					' '
				}).ToList();
			}
			EnableSpawnInventoryAsContainer = myObjectBuilder_CharacterDefinition.EnableSpawnInventoryAsContainer;
			if (EnableSpawnInventoryAsContainer)
			{
				if (myObjectBuilder_CharacterDefinition.InventorySpawnContainerId.HasValue)
				{
					InventorySpawnContainerId = myObjectBuilder_CharacterDefinition.InventorySpawnContainerId.Value;
				}
				SpawnInventoryOnBodyRemoval = myObjectBuilder_CharacterDefinition.SpawnInventoryOnBodyRemoval;
			}
			LootingTime = myObjectBuilder_CharacterDefinition.LootingTime;
			DeadBodyShape = myObjectBuilder_CharacterDefinition.DeadBodyShape;
			AnimationController = myObjectBuilder_CharacterDefinition.AnimationController;
			MaxForce = myObjectBuilder_CharacterDefinition.MaxForce;
			SuitConsumptionInTemperatureExtreme = myObjectBuilder_CharacterDefinition.SuitConsumptionInTemperatureExtreme;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_CharacterDefinition myObjectBuilder_CharacterDefinition = (MyObjectBuilder_CharacterDefinition)base.GetObjectBuilder();
			myObjectBuilder_CharacterDefinition.Name = Name;
			myObjectBuilder_CharacterDefinition.Model = Model;
			myObjectBuilder_CharacterDefinition.ReflectorTexture = ReflectorTexture;
			myObjectBuilder_CharacterDefinition.LeftGlare = LeftGlare;
			myObjectBuilder_CharacterDefinition.RightGlare = RightGlare;
			myObjectBuilder_CharacterDefinition.LightGlareSize = LightGlareSize;
			myObjectBuilder_CharacterDefinition.Skeleton = Skeleton;
			myObjectBuilder_CharacterDefinition.LeftForearmBone = LeftForearmBone;
			myObjectBuilder_CharacterDefinition.LeftUpperarmBone = LeftUpperarmBone;
			myObjectBuilder_CharacterDefinition.RightForearmBone = RightForearmBone;
			myObjectBuilder_CharacterDefinition.RightUpperarmBone = RightUpperarmBone;
			myObjectBuilder_CharacterDefinition.SpineBone = SpineBone;
			myObjectBuilder_CharacterDefinition.MaterialsDisabledIn1st = MaterialsDisabledIn1st;
			myObjectBuilder_CharacterDefinition.UsesAtmosphereDetector = UsesAtmosphereDetector;
			myObjectBuilder_CharacterDefinition.UsesReverbDetector = UsesReverbDetector;
			myObjectBuilder_CharacterDefinition.NeedsOxygen = NeedsOxygen;
			myObjectBuilder_CharacterDefinition.OxygenConsumptionMultiplier = OxygenConsumptionMultiplier;
			myObjectBuilder_CharacterDefinition.OxygenConsumption = OxygenConsumption;
			myObjectBuilder_CharacterDefinition.OxygenSuitRefillTime = OxygenSuitRefillTime;
			myObjectBuilder_CharacterDefinition.MinOxygenLevelForSuitRefill = MinOxygenLevelForSuitRefill;
			myObjectBuilder_CharacterDefinition.PressureLevelForLowDamage = PressureLevelForLowDamage;
			myObjectBuilder_CharacterDefinition.DamageAmountAtZeroPressure = DamageAmountAtZeroPressure;
			myObjectBuilder_CharacterDefinition.JumpSoundName = JumpSoundName;
			myObjectBuilder_CharacterDefinition.JetpackIdleSoundName = JetpackIdleSoundName;
			myObjectBuilder_CharacterDefinition.JetpackRunSoundName = JetpackRunSoundName;
			myObjectBuilder_CharacterDefinition.CrouchDownSoundName = CrouchDownSoundName;
			myObjectBuilder_CharacterDefinition.CrouchUpSoundName = CrouchUpSoundName;
			myObjectBuilder_CharacterDefinition.SuffocateSoundName = SuffocateSoundName;
			myObjectBuilder_CharacterDefinition.PainSoundName = PainSoundName;
			myObjectBuilder_CharacterDefinition.DeathSoundName = DeathSoundName;
			myObjectBuilder_CharacterDefinition.DeathBySuffocationSoundName = DeathBySuffocationSoundName;
			myObjectBuilder_CharacterDefinition.IronsightActSoundName = IronsightActSoundName;
			myObjectBuilder_CharacterDefinition.IronsightDeactSoundName = IronsightDeactSoundName;
			myObjectBuilder_CharacterDefinition.LoopingFootsteps = LoopingFootsteps;
			myObjectBuilder_CharacterDefinition.MagnetBootsStartSoundName = MagnetBootsStartSoundName;
			myObjectBuilder_CharacterDefinition.MagnetBootsEndSoundName = MagnetBootsEndSoundName;
			myObjectBuilder_CharacterDefinition.MagnetBootsStepsSoundName = MagnetBootsStepsSoundName;
			myObjectBuilder_CharacterDefinition.MagnetBootsProximitySoundName = MagnetBootsProximitySoundName;
			myObjectBuilder_CharacterDefinition.VisibleOnHud = VisibleOnHud;
			myObjectBuilder_CharacterDefinition.UsableByPlayer = UsableByPlayer;
			myObjectBuilder_CharacterDefinition.SuitResourceStorage = SuitResourceStorage;
			myObjectBuilder_CharacterDefinition.Jetpack = Jetpack;
			myObjectBuilder_CharacterDefinition.VerticalPositionFlyingOnly = VerticalPositionFlyingOnly;
			myObjectBuilder_CharacterDefinition.UseOnlyWalking = UseOnlyWalking;
			myObjectBuilder_CharacterDefinition.MaxSlope = MaxSlope;
			myObjectBuilder_CharacterDefinition.MaxSprintSpeed = MaxSprintSpeed;
			myObjectBuilder_CharacterDefinition.MaxRunSpeed = MaxRunSpeed;
			myObjectBuilder_CharacterDefinition.MaxBackrunSpeed = MaxBackrunSpeed;
			myObjectBuilder_CharacterDefinition.MaxRunStrafingSpeed = MaxRunStrafingSpeed;
			myObjectBuilder_CharacterDefinition.MaxWalkSpeed = MaxWalkSpeed;
			myObjectBuilder_CharacterDefinition.MaxBackwalkSpeed = MaxBackwalkSpeed;
			myObjectBuilder_CharacterDefinition.MaxWalkStrafingSpeed = MaxWalkStrafingSpeed;
			myObjectBuilder_CharacterDefinition.MaxCrouchWalkSpeed = MaxCrouchWalkSpeed;
			myObjectBuilder_CharacterDefinition.MaxCrouchBackwalkSpeed = MaxCrouchBackwalkSpeed;
			myObjectBuilder_CharacterDefinition.MaxCrouchStrafingSpeed = MaxCrouchStrafingSpeed;
			myObjectBuilder_CharacterDefinition.CharacterHeadSize = CharacterHeadSize;
			myObjectBuilder_CharacterDefinition.CharacterHeadHeight = CharacterHeadHeight;
			myObjectBuilder_CharacterDefinition.CharacterCollisionScale = CharacterCollisionScale;
			myObjectBuilder_CharacterDefinition.CharacterCollisionWidth = CharacterCollisionWidth;
			myObjectBuilder_CharacterDefinition.CharacterCollisionHeight = CharacterCollisionHeight;
			myObjectBuilder_CharacterDefinition.CharacterCollisionCrouchHeight = CharacterCollisionCrouchHeight;
			myObjectBuilder_CharacterDefinition.CanCrouch = CanCrouch;
			myObjectBuilder_CharacterDefinition.CanIronsight = CanIronsight;
			myObjectBuilder_CharacterDefinition.Inventory = InventoryDefinition;
			myObjectBuilder_CharacterDefinition.PhysicalMaterial = PhysicalMaterial;
			myObjectBuilder_CharacterDefinition.EnabledComponents = string.Join(" ", EnabledComponents);
			myObjectBuilder_CharacterDefinition.EnableSpawnInventoryAsContainer = EnableSpawnInventoryAsContainer;
			if (EnableSpawnInventoryAsContainer)
			{
				if (InventorySpawnContainerId.HasValue)
				{
					myObjectBuilder_CharacterDefinition.InventorySpawnContainerId = InventorySpawnContainerId.Value;
				}
				myObjectBuilder_CharacterDefinition.SpawnInventoryOnBodyRemoval = SpawnInventoryOnBodyRemoval;
			}
			myObjectBuilder_CharacterDefinition.LootingTime = LootingTime;
			myObjectBuilder_CharacterDefinition.DeadBodyShape = DeadBodyShape;
			myObjectBuilder_CharacterDefinition.AnimationController = AnimationController;
			myObjectBuilder_CharacterDefinition.MaxForce = MaxForce;
			myObjectBuilder_CharacterDefinition.RotationToSupport = RotationToSupport;
			myObjectBuilder_CharacterDefinition.BreathCalmSoundName = BreathCalmSoundName;
			myObjectBuilder_CharacterDefinition.BreathHeavySoundName = BreathHeavySoundName;
			myObjectBuilder_CharacterDefinition.OxygenChokeNormalSoundName = OxygenChokeNormalSoundName;
			myObjectBuilder_CharacterDefinition.OxygenChokeLowSoundName = OxygenChokeLowSoundName;
			myObjectBuilder_CharacterDefinition.OxygenChokeCriticalSoundName = OxygenChokeCriticalSoundName;
			myObjectBuilder_CharacterDefinition.SuitConsumptionInTemperatureExtreme = SuitConsumptionInTemperatureExtreme;
			return myObjectBuilder_CharacterDefinition;
		}
	}
}
