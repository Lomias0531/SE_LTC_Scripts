using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContainerDefinition), null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyContainerDefinition : MyDefinitionBase
	{
		public class DefaultComponent
		{
			public MyObjectBuilderType BuilderType = null;

			public Type InstanceType;

			public bool ForceCreate;

			public MyStringHash? SubtypeId;

			public bool IsValid()
			{
				if (!(InstanceType != null))
				{
					return !BuilderType.IsNull;
				}
				return true;
			}
		}

		private class VRage_Game_MyContainerDefinition_003C_003EActor : IActivator, IActivator<MyContainerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContainerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContainerDefinition CreateInstance()
			{
				return new MyContainerDefinition();
			}

			MyContainerDefinition IActivator<MyContainerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<DefaultComponent> DefaultComponents = new List<DefaultComponent>();

		public EntityFlags? Flags;

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_ContainerDefinition myObjectBuilder_ContainerDefinition = (MyObjectBuilder_ContainerDefinition)base.GetObjectBuilder();
			myObjectBuilder_ContainerDefinition.Flags = Flags;
			if (DefaultComponents != null && DefaultComponents.Count > 0)
			{
				myObjectBuilder_ContainerDefinition.DefaultComponents = new MyObjectBuilder_ContainerDefinition.DefaultComponentBuilder[DefaultComponents.Count];
				int num = 0;
				{
					foreach (DefaultComponent defaultComponent in DefaultComponents)
					{
						if (!defaultComponent.BuilderType.IsNull)
						{
							myObjectBuilder_ContainerDefinition.DefaultComponents[num].BuilderType = defaultComponent.BuilderType.ToString();
						}
						if (defaultComponent.InstanceType != null)
						{
							myObjectBuilder_ContainerDefinition.DefaultComponents[num].InstanceType = defaultComponent.InstanceType.Name;
						}
						if (defaultComponent.SubtypeId.HasValue)
						{
							myObjectBuilder_ContainerDefinition.DefaultComponents[num].SubtypeId = defaultComponent.SubtypeId.Value.ToString();
						}
						myObjectBuilder_ContainerDefinition.DefaultComponents[num].ForceCreate = defaultComponent.ForceCreate;
						num++;
					}
					return myObjectBuilder_ContainerDefinition;
				}
			}
			return myObjectBuilder_ContainerDefinition;
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ContainerDefinition myObjectBuilder_ContainerDefinition = builder as MyObjectBuilder_ContainerDefinition;
			Flags = myObjectBuilder_ContainerDefinition.Flags;
			if (myObjectBuilder_ContainerDefinition.DefaultComponents == null || myObjectBuilder_ContainerDefinition.DefaultComponents.Length == 0)
			{
				return;
			}
			if (DefaultComponents == null)
			{
				DefaultComponents = new List<DefaultComponent>();
			}
			MyObjectBuilder_ContainerDefinition.DefaultComponentBuilder[] defaultComponents = myObjectBuilder_ContainerDefinition.DefaultComponents;
			foreach (MyObjectBuilder_ContainerDefinition.DefaultComponentBuilder defaultComponentBuilder in defaultComponents)
			{
				DefaultComponent defaultComponent = new DefaultComponent();
				try
				{
					if (defaultComponentBuilder.BuilderType != null)
					{
						MyObjectBuilderType myObjectBuilderType = defaultComponent.BuilderType = MyObjectBuilderType.Parse(defaultComponentBuilder.BuilderType);
					}
				}
				catch (Exception)
				{
					MyLog.Default.WriteLine($"Container definition error: can not parse defined component type {defaultComponentBuilder} for container {Id.ToString()}");
				}
				try
				{
					if (defaultComponentBuilder.InstanceType != null)
					{
						Type type = defaultComponent.InstanceType = Type.GetType(defaultComponentBuilder.InstanceType, throwOnError: true);
					}
				}
				catch (Exception)
				{
					MyLog.Default.WriteLine($"Container definition error: can not parse defined component type {defaultComponentBuilder} for container {Id.ToString()}");
				}
				defaultComponent.ForceCreate = defaultComponentBuilder.ForceCreate;
				if (defaultComponentBuilder.SubtypeId != null)
				{
					defaultComponent.SubtypeId = MyStringHash.GetOrCompute(defaultComponentBuilder.SubtypeId);
				}
				if (defaultComponent.IsValid())
				{
					DefaultComponents.Add(defaultComponent);
				}
				else
				{
					MyLog.Default.WriteLine($"Defined component {defaultComponentBuilder} for container {Id.ToString()} is invalid, none builder type or instance type is defined! Skipping it.");
				}
			}
		}

		/// <summary>
		/// This will search through definitions to find if Default Components contains the searched component either as BuilderType, InstanceType, or ComponentType
		/// </summary>
		/// <param name="component">Name of the type to search for in defined default components</param>
		/// <returns>true if is defined component with the matching BuilderType, InstanceType or ComponentType </returns>
		public bool HasDefaultComponent(string component)
		{
			foreach (DefaultComponent defaultComponent in DefaultComponents)
			{
				if ((!defaultComponent.BuilderType.IsNull && defaultComponent.BuilderType.ToString() == component) || (defaultComponent.InstanceType != null && defaultComponent.InstanceType.ToString() == component))
				{
					return true;
				}
			}
			return false;
		}
	}
}
