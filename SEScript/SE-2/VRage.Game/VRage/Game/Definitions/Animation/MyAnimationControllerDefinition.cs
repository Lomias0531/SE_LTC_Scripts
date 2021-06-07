using System.Collections.Generic;
using VRage.Game.ObjectBuilders;
using VRage.Network;

namespace VRage.Game.Definitions.Animation
{
	[MyDefinitionType(typeof(MyObjectBuilder_AnimationControllerDefinition), typeof(MyAnimationControllerDefinitionPostprocess))]
	public class MyAnimationControllerDefinition : MyDefinitionBase
	{
		private class VRage_Game_Definitions_Animation_MyAnimationControllerDefinition_003C_003EActor : IActivator, IActivator<MyAnimationControllerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAnimationControllerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAnimationControllerDefinition CreateInstance()
			{
				return new MyAnimationControllerDefinition();
			}

			MyAnimationControllerDefinition IActivator<MyAnimationControllerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_AnimationLayer> Layers = new List<MyObjectBuilder_AnimationLayer>();

		public List<MyObjectBuilder_AnimationSM> StateMachines = new List<MyObjectBuilder_AnimationSM>();

		public List<MyObjectBuilder_AnimationFootIkChain> FootIkChains = new List<MyObjectBuilder_AnimationFootIkChain>();

		public List<string> IkIgnoredBones = new List<string>();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AnimationControllerDefinition myObjectBuilder_AnimationControllerDefinition = builder as MyObjectBuilder_AnimationControllerDefinition;
			if (myObjectBuilder_AnimationControllerDefinition.Layers != null)
			{
				Layers.AddRange(myObjectBuilder_AnimationControllerDefinition.Layers);
			}
			if (myObjectBuilder_AnimationControllerDefinition.StateMachines != null)
			{
				StateMachines.AddRange(myObjectBuilder_AnimationControllerDefinition.StateMachines);
			}
			if (myObjectBuilder_AnimationControllerDefinition.FootIkChains != null)
			{
				FootIkChains.AddRange(myObjectBuilder_AnimationControllerDefinition.FootIkChains);
			}
			if (myObjectBuilder_AnimationControllerDefinition.IkIgnoredBones != null)
			{
				IkIgnoredBones.AddRange(myObjectBuilder_AnimationControllerDefinition.IkIgnoredBones);
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_AnimationControllerDefinition myObjectBuilder_AnimationControllerDefinition = MyDefinitionManagerBase.GetObjectFactory().CreateObjectBuilder<MyObjectBuilder_AnimationControllerDefinition>(this);
			myObjectBuilder_AnimationControllerDefinition.Id = Id;
			myObjectBuilder_AnimationControllerDefinition.Description = (DescriptionEnum.HasValue ? DescriptionEnum.Value.ToString() : DescriptionString);
			myObjectBuilder_AnimationControllerDefinition.DisplayName = (DisplayNameEnum.HasValue ? DisplayNameEnum.Value.ToString() : DisplayNameString);
			myObjectBuilder_AnimationControllerDefinition.Icons = Icons;
			myObjectBuilder_AnimationControllerDefinition.Public = Public;
			myObjectBuilder_AnimationControllerDefinition.Enabled = Enabled;
			myObjectBuilder_AnimationControllerDefinition.AvailableInSurvival = AvailableInSurvival;
			myObjectBuilder_AnimationControllerDefinition.StateMachines = StateMachines.ToArray();
			myObjectBuilder_AnimationControllerDefinition.Layers = Layers.ToArray();
			myObjectBuilder_AnimationControllerDefinition.FootIkChains = FootIkChains.ToArray();
			myObjectBuilder_AnimationControllerDefinition.IkIgnoredBones = IkIgnoredBones.ToArray();
			return myObjectBuilder_AnimationControllerDefinition;
		}

		public void Clear()
		{
			Layers.Clear();
			StateMachines.Clear();
			FootIkChains.Clear();
			IkIgnoredBones.Clear();
		}
	}
}
