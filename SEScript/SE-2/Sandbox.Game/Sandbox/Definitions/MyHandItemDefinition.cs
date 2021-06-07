using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_HandItemDefinition), null)]
	public class MyHandItemDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyHandItemDefinition_003C_003EActor : IActivator, IActivator<MyHandItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyHandItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHandItemDefinition CreateInstance()
			{
				return new MyHandItemDefinition();
			}

			MyHandItemDefinition IActivator<MyHandItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public Matrix LeftHand;

		public Matrix RightHand;

		public Matrix ItemLocation;

		public Matrix ItemLocation3rd;

		public Matrix ItemWalkingLocation;

		public Matrix ItemWalkingLocation3rd;

		public Matrix ItemShootLocation;

		public Matrix ItemShootLocation3rd;

		public Matrix ItemIronsightLocation;

		public float BlendTime;

		public float XAmplitudeOffset;

		public float YAmplitudeOffset;

		public float ZAmplitudeOffset;

		public float XAmplitudeScale;

		public float YAmplitudeScale;

		public float ZAmplitudeScale;

		public float RunMultiplier;

		public float AmplitudeMultiplier3rd = 1f;

		public bool SimulateLeftHand = true;

		public bool SimulateRightHand = true;

		public bool SimulateLeftHandFps = true;

		public bool SimulateRightHandFps = true;

		public string FingersAnimation;

		public float ShootBlend;

		public Vector3 MuzzlePosition;

		public Vector3 ShootScatter;

		public float ScatterSpeed;

		public MyDefinitionId PhysicalItemId;

		public Vector4 LightColor;

		public float LightFalloff;

		public float LightRadius;

		public float LightGlareSize;

		public float LightGlareIntensity;

		public float LightIntensityLower;

		public float LightIntensityUpper;

		public float ShakeAmountTarget;

		public float ShakeAmountNoTarget;

		public MyItemPositioningEnum ItemPositioning;

		public MyItemPositioningEnum ItemPositioning3rd;

		public MyItemPositioningEnum ItemPositioningWalk;

		public MyItemPositioningEnum ItemPositioningWalk3rd;

		public MyItemPositioningEnum ItemPositioningShoot;

		public MyItemPositioningEnum ItemPositioningShoot3rd;

		public MyItemPositioningEnum ItemPositioningIronsight;

		public MyItemPositioningEnum ItemPositioningIronsight3rd;

		public List<ToolSound> ToolSounds;

		public MyStringHash ToolMaterial;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_HandItemDefinition myObjectBuilder_HandItemDefinition = builder as MyObjectBuilder_HandItemDefinition;
			Id = builder.Id;
			LeftHand = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.LeftHandOrientation));
			LeftHand.Translation = myObjectBuilder_HandItemDefinition.LeftHandPosition;
			RightHand = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.RightHandOrientation));
			RightHand.Translation = myObjectBuilder_HandItemDefinition.RightHandPosition;
			ItemLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.ItemOrientation));
			ItemLocation.Translation = myObjectBuilder_HandItemDefinition.ItemPosition;
			ItemWalkingLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.ItemWalkingOrientation));
			ItemWalkingLocation.Translation = myObjectBuilder_HandItemDefinition.ItemWalkingPosition;
			BlendTime = myObjectBuilder_HandItemDefinition.BlendTime;
			XAmplitudeOffset = myObjectBuilder_HandItemDefinition.XAmplitudeOffset;
			YAmplitudeOffset = myObjectBuilder_HandItemDefinition.YAmplitudeOffset;
			ZAmplitudeOffset = myObjectBuilder_HandItemDefinition.ZAmplitudeOffset;
			XAmplitudeScale = myObjectBuilder_HandItemDefinition.XAmplitudeScale;
			YAmplitudeScale = myObjectBuilder_HandItemDefinition.YAmplitudeScale;
			ZAmplitudeScale = myObjectBuilder_HandItemDefinition.ZAmplitudeScale;
			RunMultiplier = myObjectBuilder_HandItemDefinition.RunMultiplier;
			ItemLocation3rd = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.ItemOrientation3rd));
			ItemLocation3rd.Translation = myObjectBuilder_HandItemDefinition.ItemPosition3rd;
			ItemWalkingLocation3rd = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.ItemWalkingOrientation3rd));
			ItemWalkingLocation3rd.Translation = myObjectBuilder_HandItemDefinition.ItemWalkingPosition3rd;
			AmplitudeMultiplier3rd = myObjectBuilder_HandItemDefinition.AmplitudeMultiplier3rd;
			SimulateLeftHand = myObjectBuilder_HandItemDefinition.SimulateLeftHand;
			SimulateRightHand = myObjectBuilder_HandItemDefinition.SimulateRightHand;
			SimulateLeftHandFps = (myObjectBuilder_HandItemDefinition.SimulateLeftHandFps ?? SimulateLeftHand);
			SimulateRightHandFps = (myObjectBuilder_HandItemDefinition.SimulateRightHandFps ?? SimulateRightHand);
			FingersAnimation = MyDefinitionManager.Static.GetAnimationDefinitionCompatibility(myObjectBuilder_HandItemDefinition.FingersAnimation);
			ItemShootLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.ItemShootOrientation));
			ItemShootLocation.Translation = myObjectBuilder_HandItemDefinition.ItemShootPosition;
			ItemShootLocation3rd = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.ItemShootOrientation3rd));
			ItemShootLocation3rd.Translation = myObjectBuilder_HandItemDefinition.ItemShootPosition3rd;
			ShootBlend = myObjectBuilder_HandItemDefinition.ShootBlend;
			ItemIronsightLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(myObjectBuilder_HandItemDefinition.ItemIronsightOrientation));
			ItemIronsightLocation.Translation = myObjectBuilder_HandItemDefinition.ItemIronsightPosition;
			MuzzlePosition = myObjectBuilder_HandItemDefinition.MuzzlePosition;
			ShootScatter = myObjectBuilder_HandItemDefinition.ShootScatter;
			ScatterSpeed = myObjectBuilder_HandItemDefinition.ScatterSpeed;
			PhysicalItemId = myObjectBuilder_HandItemDefinition.PhysicalItemId;
			LightColor = myObjectBuilder_HandItemDefinition.LightColor;
			LightFalloff = myObjectBuilder_HandItemDefinition.LightFalloff;
			LightRadius = myObjectBuilder_HandItemDefinition.LightRadius;
			LightGlareSize = myObjectBuilder_HandItemDefinition.LightGlareSize;
			LightGlareIntensity = myObjectBuilder_HandItemDefinition.LightGlareIntensity;
			LightIntensityLower = myObjectBuilder_HandItemDefinition.LightIntensityLower;
			LightIntensityUpper = myObjectBuilder_HandItemDefinition.LightIntensityUpper;
			ShakeAmountTarget = myObjectBuilder_HandItemDefinition.ShakeAmountTarget;
			ShakeAmountNoTarget = myObjectBuilder_HandItemDefinition.ShakeAmountNoTarget;
			ToolSounds = myObjectBuilder_HandItemDefinition.ToolSounds;
			ToolMaterial = MyStringHash.GetOrCompute(myObjectBuilder_HandItemDefinition.ToolMaterial);
			ItemPositioning = myObjectBuilder_HandItemDefinition.ItemPositioning;
			ItemPositioning3rd = myObjectBuilder_HandItemDefinition.ItemPositioning3rd;
			ItemPositioningWalk = myObjectBuilder_HandItemDefinition.ItemPositioningWalk;
			ItemPositioningWalk3rd = myObjectBuilder_HandItemDefinition.ItemPositioningWalk3rd;
			ItemPositioningShoot = myObjectBuilder_HandItemDefinition.ItemPositioningShoot;
			ItemPositioningShoot3rd = myObjectBuilder_HandItemDefinition.ItemPositioningShoot3rd;
			ItemPositioningIronsight = myObjectBuilder_HandItemDefinition.ItemPositioningIronsight;
			ItemPositioningIronsight3rd = myObjectBuilder_HandItemDefinition.ItemPositioningIronsight3rd;
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_HandItemDefinition obj = (MyObjectBuilder_HandItemDefinition)base.GetObjectBuilder();
			obj.Id = Id;
			obj.LeftHandOrientation = Quaternion.CreateFromRotationMatrix(LeftHand);
			obj.LeftHandPosition = LeftHand.Translation;
			obj.RightHandOrientation = Quaternion.CreateFromRotationMatrix(RightHand);
			obj.RightHandPosition = RightHand.Translation;
			obj.ItemOrientation = Quaternion.CreateFromRotationMatrix(ItemLocation);
			obj.ItemPosition = ItemLocation.Translation;
			obj.ItemWalkingOrientation = Quaternion.CreateFromRotationMatrix(ItemWalkingLocation);
			obj.ItemWalkingPosition = ItemWalkingLocation.Translation;
			obj.BlendTime = BlendTime;
			obj.XAmplitudeOffset = XAmplitudeOffset;
			obj.YAmplitudeOffset = YAmplitudeOffset;
			obj.ZAmplitudeOffset = ZAmplitudeOffset;
			obj.XAmplitudeScale = XAmplitudeScale;
			obj.YAmplitudeScale = YAmplitudeScale;
			obj.ZAmplitudeScale = ZAmplitudeScale;
			obj.RunMultiplier = RunMultiplier;
			obj.ItemWalkingOrientation3rd = Quaternion.CreateFromRotationMatrix(ItemWalkingLocation3rd);
			obj.ItemWalkingPosition3rd = ItemWalkingLocation3rd.Translation;
			obj.ItemOrientation3rd = Quaternion.CreateFromRotationMatrix(ItemLocation3rd);
			obj.ItemPosition3rd = ItemLocation3rd.Translation;
			obj.AmplitudeMultiplier3rd = AmplitudeMultiplier3rd;
			obj.SimulateLeftHand = SimulateLeftHand;
			obj.SimulateRightHand = SimulateRightHand;
			obj.SimulateLeftHandFps = ((SimulateLeftHandFps != SimulateLeftHand) ? new bool?(SimulateLeftHandFps) : null);
			obj.SimulateRightHandFps = ((SimulateRightHandFps != SimulateRightHand) ? new bool?(SimulateRightHandFps) : null);
			obj.FingersAnimation = FingersAnimation;
			obj.ItemShootOrientation = Quaternion.CreateFromRotationMatrix(ItemShootLocation);
			obj.ItemShootPosition = ItemShootLocation.Translation;
			obj.ItemShootOrientation3rd = Quaternion.CreateFromRotationMatrix(ItemShootLocation3rd);
			obj.ItemShootPosition3rd = ItemShootLocation3rd.Translation;
			obj.ShootBlend = ShootBlend;
			obj.ItemIronsightOrientation = Quaternion.CreateFromRotationMatrix(ItemIronsightLocation);
			obj.ItemIronsightPosition = ItemIronsightLocation.Translation;
			obj.MuzzlePosition = MuzzlePosition;
			obj.ShootScatter = ShootScatter;
			obj.ScatterSpeed = ScatterSpeed;
			obj.PhysicalItemId = PhysicalItemId;
			obj.LightColor = LightColor;
			obj.LightFalloff = LightFalloff;
			obj.LightRadius = LightRadius;
			obj.LightGlareSize = LightGlareSize;
			obj.LightGlareIntensity = LightGlareIntensity;
			obj.LightIntensityLower = LightIntensityLower;
			obj.LightIntensityUpper = LightIntensityUpper;
			obj.ShakeAmountTarget = ShakeAmountTarget;
			obj.ShakeAmountNoTarget = ShakeAmountNoTarget;
			obj.ToolSounds = ToolSounds;
			obj.ToolMaterial = ToolMaterial.ToString();
			obj.ItemPositioning = ItemPositioning;
			obj.ItemPositioning3rd = ItemPositioning3rd;
			obj.ItemPositioningWalk = ItemPositioningWalk;
			obj.ItemPositioningWalk3rd = ItemPositioningWalk3rd;
			obj.ItemPositioningShoot = ItemPositioningShoot;
			obj.ItemPositioningShoot3rd = ItemPositioningShoot3rd;
			obj.ItemPositioningIronsight = ItemPositioningIronsight;
			obj.ItemPositioningIronsight3rd = ItemPositioningIronsight3rd;
			return obj;
		}
	}
}
