namespace VRage.Game.ModAPI.Ingame
{
	public static class MyInventoryItemExtension
	{
		public static MyDefinitionId GetDefinitionId(this IMyInventoryItem self)
		{
			return (self.Content as MyObjectBuilder_PhysicalObject)?.GetObjectId() ?? new MyDefinitionId(self.Content.TypeId, self.Content.SubtypeId);
		}
	}
}
