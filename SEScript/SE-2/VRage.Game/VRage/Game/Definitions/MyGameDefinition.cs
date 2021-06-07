using System.Collections.Generic;
using System.Linq;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GameDefinition), typeof(Postprocess))]
	public class MyGameDefinition : MyDefinitionBase
	{
		private new class Postprocess : MyDefinitionPostprocessor
		{
			public override void AfterLoaded(ref Bundle definitions)
			{
			}

			public override void AfterPostprocess(MyDefinitionSet set, Dictionary<MyStringHash, MyDefinitionBase> definitions)
			{
				if (!set.ContainsDefinition(Default))
				{
					set.GetDefinitionsOfType<MyGameDefinition>().First().SetDefault();
				}
			}
		}

		private class VRage_Game_Definitions_MyGameDefinition_003C_003EActor : IActivator, IActivator<MyGameDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGameDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGameDefinition CreateInstance()
			{
				return new MyGameDefinition();
			}

			MyGameDefinition IActivator<MyGameDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static readonly MyDefinitionId Default = new MyDefinitionId(typeof(MyObjectBuilder_GameDefinition), "Default");

		public Dictionary<string, MyDefinitionId?> SessionComponents;

		public static readonly MyGameDefinition DefaultDefinition = new MyGameDefinition
		{
			Id = Default,
			SessionComponents = new Dictionary<string, MyDefinitionId?>()
		};

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GameDefinition myObjectBuilder_GameDefinition = (MyObjectBuilder_GameDefinition)builder;
			if (myObjectBuilder_GameDefinition.InheritFrom != null)
			{
				MyGameDefinition definition = MyDefinitionManagerBase.Static.GetLoadingSet().GetDefinition<MyGameDefinition>(new MyDefinitionId(typeof(MyObjectBuilder_GameDefinition), myObjectBuilder_GameDefinition.InheritFrom));
				if (definition == null)
				{
					MyLog.Default.Error("Could not find parent definition {0} for game definition {1}.", myObjectBuilder_GameDefinition.InheritFrom, myObjectBuilder_GameDefinition.SubtypeId);
				}
				else
				{
					SessionComponents = new Dictionary<string, MyDefinitionId?>(definition.SessionComponents);
				}
			}
			if (SessionComponents == null)
			{
				SessionComponents = new Dictionary<string, MyDefinitionId?>();
			}
			foreach (MyObjectBuilder_GameDefinition.Comp sessionComponent in myObjectBuilder_GameDefinition.SessionComponents)
			{
				if (sessionComponent.Type != null)
				{
					SessionComponents[sessionComponent.ComponentName] = new MyDefinitionId(MyObjectBuilderType.Parse(sessionComponent.Type), sessionComponent.Subtype);
				}
				else
				{
					SessionComponents[sessionComponent.ComponentName] = null;
				}
			}
			if (myObjectBuilder_GameDefinition.Default)
			{
				SetDefault();
			}
		}

		private void SetDefault()
		{
			MyGameDefinition def = new MyGameDefinition
			{
				SessionComponents = SessionComponents,
				Id = Default
			};
			MyDefinitionManagerBase.Static.GetLoadingSet().AddOrRelaceDefinition(def);
		}
	}
}
