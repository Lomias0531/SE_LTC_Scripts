using VRage.Network;

namespace VRage.Game.Definitions.Animation
{
	[MyDefinitionType(typeof(MyObjectBuilder_AnimationDefinition), null)]
	public class MyAnimationDefinition : MyDefinitionBase
	{
		public enum AnimationStatus
		{
			Unchecked,
			OK,
			Failed
		}

		private class VRage_Game_Definitions_Animation_MyAnimationDefinition_003C_003EActor : IActivator, IActivator<MyAnimationDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyAnimationDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAnimationDefinition CreateInstance()
			{
				return new MyAnimationDefinition();
			}

			MyAnimationDefinition IActivator<MyAnimationDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string AnimationModel;

		public string AnimationModelFPS;

		public int ClipIndex;

		public string InfluenceArea;

		public string[] InfluenceAreas;

		public bool AllowInCockpit;

		public bool AllowWithWeapon;

		public bool Loop;

		public string[] SupportedSkeletons;

		public AnimationStatus Status;

		public MyDefinitionId LeftHandItem;

		public AnimationSet[] AnimationSets;

		public string ChatCommand;

		public string ChatCommandName;

		public string ChatCommandDescription;

		public int Priority;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_AnimationDefinition myObjectBuilder_AnimationDefinition = builder as MyObjectBuilder_AnimationDefinition;
			AnimationModel = myObjectBuilder_AnimationDefinition.AnimationModel;
			AnimationModelFPS = myObjectBuilder_AnimationDefinition.AnimationModelFPS;
			ClipIndex = myObjectBuilder_AnimationDefinition.ClipIndex;
			InfluenceArea = myObjectBuilder_AnimationDefinition.InfluenceArea;
			if (!string.IsNullOrEmpty(myObjectBuilder_AnimationDefinition.InfluenceArea))
			{
				InfluenceAreas = myObjectBuilder_AnimationDefinition.InfluenceArea.Split(new char[1]
				{
					' '
				});
			}
			AllowInCockpit = myObjectBuilder_AnimationDefinition.AllowInCockpit;
			AllowWithWeapon = myObjectBuilder_AnimationDefinition.AllowWithWeapon;
			if (!string.IsNullOrEmpty(myObjectBuilder_AnimationDefinition.SupportedSkeletons))
			{
				SupportedSkeletons = myObjectBuilder_AnimationDefinition.SupportedSkeletons.Split(new char[1]
				{
					' '
				});
			}
			Loop = myObjectBuilder_AnimationDefinition.Loop;
			if (!myObjectBuilder_AnimationDefinition.LeftHandItem.TypeId.IsNull)
			{
				LeftHandItem = myObjectBuilder_AnimationDefinition.LeftHandItem;
			}
			AnimationSets = myObjectBuilder_AnimationDefinition.AnimationSets;
			ChatCommand = myObjectBuilder_AnimationDefinition.ChatCommand;
			ChatCommandName = myObjectBuilder_AnimationDefinition.ChatCommandName;
			ChatCommandDescription = myObjectBuilder_AnimationDefinition.ChatCommandDescription;
			Priority = myObjectBuilder_AnimationDefinition.Priority;
		}
	}
}
