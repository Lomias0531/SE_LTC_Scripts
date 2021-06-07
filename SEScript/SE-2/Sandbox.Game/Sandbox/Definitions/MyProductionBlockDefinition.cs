using Sandbox.Game;
using Sandbox.Game.Localization;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ProductionBlockDefinition), null)]
	public class MyProductionBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyProductionBlockDefinition_003C_003EActor : IActivator, IActivator<MyProductionBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProductionBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProductionBlockDefinition CreateInstance()
			{
				return new MyProductionBlockDefinition();
			}

			MyProductionBlockDefinition IActivator<MyProductionBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float InventoryMaxVolume;

		public Vector3 InventorySize;

		public MyStringHash ResourceSinkGroup;

		public float StandbyPowerConsumption;

		public float OperationalPowerConsumption;

		public List<MyBlueprintClassDefinition> BlueprintClasses;

		public MyInventoryConstraint InputInventoryConstraint;

		public MyInventoryConstraint OutputInventoryConstraint;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ProductionBlockDefinition myObjectBuilder_ProductionBlockDefinition = builder as MyObjectBuilder_ProductionBlockDefinition;
			InventoryMaxVolume = myObjectBuilder_ProductionBlockDefinition.InventoryMaxVolume;
			InventorySize = myObjectBuilder_ProductionBlockDefinition.InventorySize;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ProductionBlockDefinition.ResourceSinkGroup);
			StandbyPowerConsumption = myObjectBuilder_ProductionBlockDefinition.StandbyPowerConsumption;
			OperationalPowerConsumption = myObjectBuilder_ProductionBlockDefinition.OperationalPowerConsumption;
			if (myObjectBuilder_ProductionBlockDefinition.BlueprintClasses == null)
			{
				InitializeLegacyBlueprintClasses(myObjectBuilder_ProductionBlockDefinition);
			}
			BlueprintClasses = new List<MyBlueprintClassDefinition>();
			string[] blueprintClasses = myObjectBuilder_ProductionBlockDefinition.BlueprintClasses;
			foreach (string className in blueprintClasses)
			{
				MyBlueprintClassDefinition blueprintClass = MyDefinitionManager.Static.GetBlueprintClass(className);
				if (blueprintClass != null)
				{
					BlueprintClasses.Add(blueprintClass);
				}
			}
		}

		protected virtual bool BlueprintClassCanBeUsed(MyBlueprintClassDefinition blueprintClass)
		{
			return true;
		}

		protected virtual void InitializeLegacyBlueprintClasses(MyObjectBuilder_ProductionBlockDefinition ob)
		{
			ob.BlueprintClasses = new string[0];
		}

		public void LoadPostProcess()
		{
			int num = 0;
			while (num < BlueprintClasses.Count)
			{
				if (!BlueprintClassCanBeUsed(BlueprintClasses[num]))
				{
					BlueprintClasses.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			InputInventoryConstraint = PrepareConstraint(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockInput, GetInputClasses(), input: true);
			OutputInventoryConstraint = PrepareConstraint(MySpaceTexts.ToolTipItemFilter_GenericProductionBlockOutput, GetOutputClasses(), input: false);
		}

		private MyInventoryConstraint PrepareConstraint(MyStringId descriptionId, IEnumerable<MyBlueprintClassDefinition> classes, bool input)
		{
			string text = null;
			foreach (MyBlueprintClassDefinition @class in classes)
			{
				string text2 = input ? @class.InputConstraintIcon : @class.OutputConstraintIcon;
				if (text2 != null)
				{
					if (text == null)
					{
						text = text2;
					}
					else if (text != text2)
					{
						text = null;
						break;
					}
				}
			}
			MyInventoryConstraint myInventoryConstraint = new MyInventoryConstraint(string.Format(MyTexts.GetString(descriptionId), DisplayNameText), text);
			foreach (MyBlueprintClassDefinition class2 in classes)
			{
				foreach (MyBlueprintDefinitionBase item2 in class2)
				{
					MyBlueprintDefinitionBase.Item[] array = input ? item2.Prerequisites : item2.Results;
					for (int i = 0; i < array.Length; i++)
					{
						MyBlueprintDefinitionBase.Item item = array[i];
						myInventoryConstraint.Add(item.Id);
					}
				}
			}
			return myInventoryConstraint;
		}

		protected virtual List<MyBlueprintClassDefinition> GetInputClasses()
		{
			return BlueprintClasses;
		}

		protected virtual List<MyBlueprintClassDefinition> GetOutputClasses()
		{
			return BlueprintClasses;
		}
	}
}
