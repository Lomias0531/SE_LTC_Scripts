using System;
using System.Collections.Generic;
using VRage.Utils;

namespace Sandbox.Game.Entities.Character.Components
{
	[Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
	public static class MyCharacterComponentTypes
	{
		[Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
		private static Dictionary<MyStringId, Tuple<Type, Type>> m_types;

		[Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
		public static Dictionary<MyStringId, Tuple<Type, Type>> CharacterComponents
		{
			get
			{
				if (m_types == null)
				{
					m_types = new Dictionary<MyStringId, Tuple<Type, Type>>
					{
						{
							MyStringId.GetOrCompute("RagdollComponent"),
							new Tuple<Type, Type>(typeof(MyCharacterRagdollComponent), typeof(MyCharacterRagdollComponent))
						},
						{
							MyStringId.GetOrCompute("InventorySpawnComponent"),
							new Tuple<Type, Type>(typeof(MyInventorySpawnComponent), typeof(MyInventorySpawnComponent))
						}
					};
				}
				return m_types;
			}
		}
	}
}
