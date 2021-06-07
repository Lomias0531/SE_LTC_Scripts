using Sandbox.Common.ObjectBuilders.Definitions;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_MedicalRoomDefinition), null)]
	public class MyMedicalRoomDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyMedicalRoomDefinition_003C_003EActor : IActivator, IActivator<MyMedicalRoomDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyMedicalRoomDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMedicalRoomDefinition CreateInstance()
			{
				return new MyMedicalRoomDefinition();
			}

			MyMedicalRoomDefinition IActivator<MyMedicalRoomDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ResourceSinkGroup;

		public string IdleSound;

		public string ProgressSound;

		public string RespawnSuitName;

		public HashSet<string> CustomWardrobeNames;

		public bool RespawnAllowed;

		public bool HealingAllowed;

		public bool RefuelAllowed;

		public bool SuitChangeAllowed;

		public bool CustomWardrobesEnabled;

		public bool ForceSuitChangeOnRespawn;

		public bool SpawnWithoutOxygenEnabled;

		public Vector3D WardrobeCharacterOffset;

		public float WardrobeCharacterOffsetLength;

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_MedicalRoomDefinition myObjectBuilder_MedicalRoomDefinition = builder as MyObjectBuilder_MedicalRoomDefinition;
			ResourceSinkGroup = myObjectBuilder_MedicalRoomDefinition.ResourceSinkGroup;
			IdleSound = myObjectBuilder_MedicalRoomDefinition.IdleSound;
			ProgressSound = myObjectBuilder_MedicalRoomDefinition.ProgressSound;
			RespawnSuitName = myObjectBuilder_MedicalRoomDefinition.RespawnSuitName;
			RespawnAllowed = myObjectBuilder_MedicalRoomDefinition.RespawnAllowed;
			HealingAllowed = myObjectBuilder_MedicalRoomDefinition.HealingAllowed;
			RefuelAllowed = myObjectBuilder_MedicalRoomDefinition.RefuelAllowed;
			SuitChangeAllowed = myObjectBuilder_MedicalRoomDefinition.SuitChangeAllowed;
			CustomWardrobesEnabled = myObjectBuilder_MedicalRoomDefinition.CustomWardrobesEnabled;
			ForceSuitChangeOnRespawn = myObjectBuilder_MedicalRoomDefinition.ForceSuitChangeOnRespawn;
			SpawnWithoutOxygenEnabled = myObjectBuilder_MedicalRoomDefinition.SpawnWithoutOxygenEnabled;
			WardrobeCharacterOffset = myObjectBuilder_MedicalRoomDefinition.WardrobeCharacterOffset;
			WardrobeCharacterOffsetLength = (float)WardrobeCharacterOffset.Length();
			if (myObjectBuilder_MedicalRoomDefinition.CustomWardRobeNames == null)
			{
				CustomWardrobeNames = new HashSet<string>();
			}
			else
			{
				CustomWardrobeNames = new HashSet<string>(myObjectBuilder_MedicalRoomDefinition.CustomWardRobeNames);
			}
			ScreenAreas = ((myObjectBuilder_MedicalRoomDefinition.ScreenAreas != null) ? myObjectBuilder_MedicalRoomDefinition.ScreenAreas.ToList() : null);
		}
	}
}
