using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ControllerSchemaDefinition), null)]
	public class MyControllerSchemaDefinition : MyDefinitionBase
	{
		public class ControlGroup
		{
			public string Type;

			public string Name;

			public Dictionary<string, MyControllerSchemaEnum> ControlBinding;
		}

		private class Sandbox_Definitions_MyControllerSchemaDefinition_003C_003EActor : IActivator, IActivator<MyControllerSchemaDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyControllerSchemaDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyControllerSchemaDefinition CreateInstance()
			{
				return new MyControllerSchemaDefinition();
			}

			MyControllerSchemaDefinition IActivator<MyControllerSchemaDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<int> CompatibleDevices;

		public Dictionary<string, List<ControlGroup>> Schemas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ControllerSchemaDefinition myObjectBuilder_ControllerSchemaDefinition = builder as MyObjectBuilder_ControllerSchemaDefinition;
			if (myObjectBuilder_ControllerSchemaDefinition.CompatibleDeviceIds != null)
			{
				CompatibleDevices = new List<int>(myObjectBuilder_ControllerSchemaDefinition.CompatibleDeviceIds.Count);
				byte[] arr = new byte[4];
				foreach (string compatibleDeviceId in myObjectBuilder_ControllerSchemaDefinition.CompatibleDeviceIds)
				{
					if (compatibleDeviceId.Length >= 8 && TryGetByteArray(compatibleDeviceId, 8, out arr))
					{
						int item = BitConverter.ToInt32(arr, 0);
						CompatibleDevices.Add(item);
					}
				}
			}
			if (myObjectBuilder_ControllerSchemaDefinition.Schemas != null)
			{
				Schemas = new Dictionary<string, List<ControlGroup>>(myObjectBuilder_ControllerSchemaDefinition.Schemas.Count);
				foreach (MyObjectBuilder_ControllerSchemaDefinition.Schema schema in myObjectBuilder_ControllerSchemaDefinition.Schemas)
				{
					if (schema.ControlGroups != null)
					{
						List<ControlGroup> value = new List<ControlGroup>(schema.ControlGroups.Count);
						Schemas[schema.SchemaName] = value;
						foreach (MyObjectBuilder_ControllerSchemaDefinition.ControlGroup controlGroup2 in schema.ControlGroups)
						{
							ControlGroup controlGroup = new ControlGroup();
							controlGroup.Type = controlGroup2.Type;
							controlGroup.Name = controlGroup2.Name;
							if (controlGroup2.ControlDefs != null)
							{
								controlGroup.ControlBinding = new Dictionary<string, MyControllerSchemaEnum>(controlGroup2.ControlDefs.Count);
								foreach (MyObjectBuilder_ControllerSchemaDefinition.ControlDef controlDef in controlGroup2.ControlDefs)
								{
									controlGroup.ControlBinding[controlDef.Type] = controlDef.Control;
								}
							}
						}
					}
				}
			}
		}

		private bool TryGetByteArray(string str, int count, out byte[] arr)
		{
			arr = null;
			if (count % 2 == 1)
			{
				return false;
			}
			if (str.Length < count)
			{
				return false;
			}
			arr = new byte[count / 2];
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = 0;
			while (num < count)
			{
				stringBuilder.Clear().Append(str[num]).Append(str[num + 1]);
				if (!byte.TryParse(stringBuilder.ToString(), NumberStyles.HexNumber, null, out arr[num2]))
				{
					return false;
				}
				num += 2;
				num2++;
			}
			return true;
		}
	}
}
