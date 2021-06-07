using Sandbox.Game.GUI;
using VRage.Audio;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.Entities.Character.Components
{
	[MyComponentType(typeof(MyCharacterPickupComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_CharacterPickupComponent), true)]
	public class MyCharacterPickupComponent : MyCharacterComponent
	{
		private class Sandbox_Game_Entities_Character_Components_MyCharacterPickupComponent_003C_003EActor : IActivator, IActivator<MyCharacterPickupComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyCharacterPickupComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCharacterPickupComponent CreateInstance()
			{
				return new MyCharacterPickupComponent();
			}

			MyCharacterPickupComponent IActivator<MyCharacterPickupComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public virtual void PickUp()
		{
			MyCharacterDetectorComponent myCharacterDetectorComponent = base.Character.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent != null && myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.PickUp))
			{
				if (myCharacterDetectorComponent.UseObject.PlayIndicatorSound)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
					base.Character.SoundComp.StopStateSound();
				}
				myCharacterDetectorComponent.UseObject.Use(UseActionEnum.PickUp, base.Character);
			}
		}

		public virtual void PickUpContinues()
		{
			MyCharacterDetectorComponent myCharacterDetectorComponent = base.Character.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent != null && myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.PickUp) && myCharacterDetectorComponent.UseObject.ContinuousUsage)
			{
				myCharacterDetectorComponent.UseObject.Use(UseActionEnum.PickUp, base.Character);
			}
		}

		public virtual void PickUpFinished()
		{
			MyCharacterDetectorComponent myCharacterDetectorComponent = base.Character.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent.UseObject != null && myCharacterDetectorComponent.UseObject.IsActionSupported(UseActionEnum.UseFinished))
			{
				myCharacterDetectorComponent.UseObject.Use(UseActionEnum.UseFinished, base.Character);
			}
		}
	}
}
