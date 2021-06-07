using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_EntityStatDefinition), null)]
	public class MyEntityStatDefinition : MyDefinitionBase
	{
		public struct GuiDefinition
		{
			public float HeightMultiplier;

			public int Priority;

			public Vector3I Color;

			public float CriticalRatio;

			public bool DisplayCriticalDivider;

			public Vector3I CriticalColorFrom;

			public Vector3I CriticalColorTo;
		}

		private class Sandbox_Definitions_MyEntityStatDefinition_003C_003EActor : IActivator, IActivator<MyEntityStatDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityStatDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityStatDefinition CreateInstance()
			{
				return new MyEntityStatDefinition();
			}

			MyEntityStatDefinition IActivator<MyEntityStatDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MinValue;

		public float MaxValue;

		public float DefaultValue;

		public bool EnabledInCreative;

		public string Name;

		public GuiDefinition GuiDef;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EntityStatDefinition myObjectBuilder_EntityStatDefinition = builder as MyObjectBuilder_EntityStatDefinition;
			MinValue = myObjectBuilder_EntityStatDefinition.MinValue;
			MaxValue = myObjectBuilder_EntityStatDefinition.MaxValue;
			DefaultValue = myObjectBuilder_EntityStatDefinition.DefaultValue;
			EnabledInCreative = myObjectBuilder_EntityStatDefinition.EnabledInCreative;
			Name = myObjectBuilder_EntityStatDefinition.Name;
			if (float.IsNaN(DefaultValue))
			{
				DefaultValue = MaxValue;
			}
			GuiDef = default(GuiDefinition);
			if (myObjectBuilder_EntityStatDefinition.GuiDef != null)
			{
				GuiDef.HeightMultiplier = myObjectBuilder_EntityStatDefinition.GuiDef.HeightMultiplier;
				GuiDef.Priority = myObjectBuilder_EntityStatDefinition.GuiDef.Priority;
				GuiDef.Color = myObjectBuilder_EntityStatDefinition.GuiDef.Color;
				GuiDef.CriticalRatio = myObjectBuilder_EntityStatDefinition.GuiDef.CriticalRatio;
				GuiDef.DisplayCriticalDivider = myObjectBuilder_EntityStatDefinition.GuiDef.DisplayCriticalDivider;
				GuiDef.CriticalColorFrom = myObjectBuilder_EntityStatDefinition.GuiDef.CriticalColorFrom;
				GuiDef.CriticalColorTo = myObjectBuilder_EntityStatDefinition.GuiDef.CriticalColorTo;
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_EntityStatDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_EntityStatDefinition;
			obj.MinValue = MinValue;
			obj.MaxValue = MaxValue;
			obj.DefaultValue = DefaultValue;
			obj.EnabledInCreative = EnabledInCreative;
			obj.Name = Name;
			obj.GuiDef = new MyObjectBuilder_EntityStatDefinition.GuiDefinition();
			obj.GuiDef.HeightMultiplier = GuiDef.HeightMultiplier;
			obj.GuiDef.Priority = GuiDef.Priority;
			obj.GuiDef.Color = GuiDef.Color;
			obj.GuiDef.CriticalRatio = GuiDef.CriticalRatio;
			obj.GuiDef.DisplayCriticalDivider = GuiDef.DisplayCriticalDivider;
			obj.GuiDef.CriticalColorFrom = GuiDef.CriticalColorFrom;
			obj.GuiDef.CriticalColorTo = GuiDef.CriticalColorTo;
			return obj;
		}
	}
}
