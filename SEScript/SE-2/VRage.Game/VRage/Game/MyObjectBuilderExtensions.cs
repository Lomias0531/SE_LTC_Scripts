using VRage.ObjectBuilders;

namespace VRage.Game
{
	public static class MyObjectBuilderExtensions
	{
		public static MyDefinitionId GetId(this MyObjectBuilder_Base self)
		{
			return new MyDefinitionId(self.TypeId, self.SubtypeId);
		}
	}
}
