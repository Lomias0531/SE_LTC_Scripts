using System.Collections;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_BlueprintClassDefinition), null)]
	public class MyBlueprintClassDefinition : MyDefinitionBase, IEnumerable<MyBlueprintDefinitionBase>, IEnumerable
	{
		private class SubtypeComparer : IComparer<MyBlueprintDefinitionBase>
		{
			public static SubtypeComparer Static = new SubtypeComparer();

			public int Compare(MyBlueprintDefinitionBase x, MyBlueprintDefinitionBase y)
			{
				return x.Id.SubtypeName.CompareTo(y.Id.SubtypeName);
			}
		}

		private class PrioritizedSubtypeComparer : IComparer<MyBlueprintDefinitionBase>
		{
			public static PrioritizedSubtypeComparer Static = new PrioritizedSubtypeComparer();

			public int Compare(MyBlueprintDefinitionBase x, MyBlueprintDefinitionBase y)
			{
				if (x.Priority > y.Priority)
				{
					return -1;
				}
				if (x.Priority < y.Priority)
				{
					return 1;
				}
				return x.Id.SubtypeName.CompareTo(y.Id.SubtypeName);
			}
		}

		private class Sandbox_Definitions_MyBlueprintClassDefinition_003C_003EActor : IActivator, IActivator<MyBlueprintClassDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlueprintClassDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlueprintClassDefinition CreateInstance()
			{
				return new MyBlueprintClassDefinition();
			}

			MyBlueprintClassDefinition IActivator<MyBlueprintClassDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string HighlightIcon;

		public string InputConstraintIcon;

		public string OutputConstraintIcon;

		public string ProgressBarSoundCue;

		private SortedSet<MyBlueprintDefinitionBase> m_blueprints;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_BlueprintClassDefinition myObjectBuilder_BlueprintClassDefinition = builder as MyObjectBuilder_BlueprintClassDefinition;
			HighlightIcon = myObjectBuilder_BlueprintClassDefinition.HighlightIcon;
			InputConstraintIcon = myObjectBuilder_BlueprintClassDefinition.InputConstraintIcon;
			OutputConstraintIcon = myObjectBuilder_BlueprintClassDefinition.OutputConstraintIcon;
			ProgressBarSoundCue = myObjectBuilder_BlueprintClassDefinition.ProgressBarSoundCue;
			m_blueprints = new SortedSet<MyBlueprintDefinitionBase>(PrioritizedSubtypeComparer.Static);
		}

		public void AddBlueprint(MyBlueprintDefinitionBase blueprint)
		{
			if (!m_blueprints.Contains(blueprint))
			{
				m_blueprints.Add(blueprint);
			}
		}

		public void ClearBlueprints()
		{
			m_blueprints.Clear();
		}

		public bool ContainsBlueprint(MyBlueprintDefinitionBase blueprint)
		{
			return m_blueprints.Contains(blueprint);
		}

		public IEnumerator<MyBlueprintDefinitionBase> GetEnumerator()
		{
			return m_blueprints.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_blueprints.GetEnumerator();
		}
	}
}
