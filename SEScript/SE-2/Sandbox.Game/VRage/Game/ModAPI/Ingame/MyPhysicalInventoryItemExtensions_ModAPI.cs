using Sandbox.Definitions;

namespace VRage.Game.ModAPI.Ingame
{
	public static class MyPhysicalInventoryItemExtensions_ModAPI
	{
		public static MyItemInfo GetItemInfo(this MyItemType itemType)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(itemType);
			MyItemInfo result;
			if (physicalItemDefinition == null)
			{
				result = default(MyItemInfo);
				return result;
			}
			result = default(MyItemInfo);
			result.Size = physicalItemDefinition.Size;
			result.Mass = physicalItemDefinition.Mass;
			result.Volume = physicalItemDefinition.Volume;
			result.MaxStackAmount = physicalItemDefinition.MaxStackAmount;
			result.UsesFractions = !physicalItemDefinition.HasIntegralAmounts;
			result.IsOre = (physicalItemDefinition.Id.TypeId == typeof(MyObjectBuilder_Ore));
			result.IsIngot = (physicalItemDefinition.Id.TypeId == typeof(MyObjectBuilder_Ingot));
			result.IsAmmo = (physicalItemDefinition.Id.TypeId == typeof(MyObjectBuilder_AmmoMagazine));
			result.IsComponent = (physicalItemDefinition.Id.TypeId == typeof(MyObjectBuilder_Component));
			result.IsTool = (physicalItemDefinition.Id.TypeId == typeof(MyObjectBuilder_PhysicalGunObject));
			return result;
		}
	}
}
