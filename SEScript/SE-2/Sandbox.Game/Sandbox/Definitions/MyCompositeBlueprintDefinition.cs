using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_CompositeBlueprintDefinition), null)]
	public class MyCompositeBlueprintDefinition : MyBlueprintDefinitionBase
	{
		private class Sandbox_Definitions_MyCompositeBlueprintDefinition_003C_003EActor : IActivator, IActivator<MyCompositeBlueprintDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCompositeBlueprintDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCompositeBlueprintDefinition CreateInstance()
			{
				return new MyCompositeBlueprintDefinition();
			}

			MyCompositeBlueprintDefinition IActivator<MyCompositeBlueprintDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyBlueprintDefinitionBase[] m_blueprints;

		private Item[] m_items;

		private static List<Item> m_tmpPrerequisiteList = new List<Item>();

		private static List<Item> m_tmpResultList = new List<Item>();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CompositeBlueprintDefinition myObjectBuilder_CompositeBlueprintDefinition = builder as MyObjectBuilder_CompositeBlueprintDefinition;
			m_items = new Item[(myObjectBuilder_CompositeBlueprintDefinition.Blueprints != null) ? myObjectBuilder_CompositeBlueprintDefinition.Blueprints.Length : 0];
			for (int i = 0; i < m_items.Length; i++)
			{
				m_items[i] = Item.FromObjectBuilder(myObjectBuilder_CompositeBlueprintDefinition.Blueprints[i]);
			}
			base.PostprocessNeeded = true;
		}

		public override void Postprocess()
		{
			Item[] items = m_items;
			for (int i = 0; i < items.Length; i++)
			{
				Item item = items[i];
				if (!MyDefinitionManager.Static.HasBlueprint(item.Id) || MyDefinitionManager.Static.GetBlueprintDefinition(item.Id).PostprocessNeeded)
				{
					return;
				}
			}
			float num = 0f;
			bool flag = false;
			float num2 = 0f;
			m_blueprints = new MyBlueprintDefinitionBase[m_items.Length];
			m_tmpPrerequisiteList.Clear();
			m_tmpResultList.Clear();
			for (int j = 0; j < m_items.Length; j++)
			{
				MyFixedPoint amount = m_items[j].Amount;
				MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(m_items[j].Id);
				m_blueprints[j] = blueprintDefinition;
				flag = (flag || blueprintDefinition.Atomic);
				num += blueprintDefinition.OutputVolume * (float)amount;
				num2 += blueprintDefinition.BaseProductionTimeInSeconds * (float)amount;
				PostprocessAddSubblueprint(blueprintDefinition, amount);
			}
			Prerequisites = m_tmpPrerequisiteList.ToArray();
			Results = m_tmpResultList.ToArray();
			m_tmpPrerequisiteList.Clear();
			m_tmpResultList.Clear();
			Atomic = flag;
			OutputVolume = num;
			BaseProductionTimeInSeconds = num2;
			base.PostprocessNeeded = false;
		}

		private void PostprocessAddSubblueprint(MyBlueprintDefinitionBase blueprint, MyFixedPoint blueprintAmount)
		{
			for (int i = 0; i < blueprint.Prerequisites.Length; i++)
			{
				Item toAdd = blueprint.Prerequisites[i];
				toAdd.Amount *= blueprintAmount;
				AddToItemList(m_tmpPrerequisiteList, toAdd);
			}
			for (int j = 0; j < blueprint.Results.Length; j++)
			{
				Item toAdd2 = blueprint.Results[j];
				toAdd2.Amount *= blueprintAmount;
				AddToItemList(m_tmpResultList, toAdd2);
			}
		}

		private void AddToItemList(List<Item> items, Item toAdd)
		{
			int num = 0;
			Item value = default(Item);
			for (num = 0; num < items.Count; num++)
			{
				value = items[num];
				if (value.Id == toAdd.Id)
				{
					break;
				}
			}
			if (num >= items.Count)
			{
				items.Add(toAdd);
				return;
			}
			value.Amount += toAdd.Amount;
			items[num] = value;
		}

		public override int GetBlueprints(List<ProductionInfo> blueprints)
		{
			int num = 0;
			for (int i = 0; i < m_blueprints.Length; i++)
			{
				int blueprints2 = m_blueprints[i].GetBlueprints(blueprints);
				int count = blueprints.Count;
				for (int num2 = count - 1; num2 >= count - blueprints2; num2--)
				{
					ProductionInfo value = blueprints[num2];
					value.Amount *= m_items[i].Amount;
					blueprints[num2] = value;
				}
				num += blueprints2;
			}
			return num;
		}
	}
}
