namespace Sandbox.Game.Entities
{
	public static class MyEntityQueryTypeExtensions
	{
		public static bool HasDynamic(this MyEntityQueryType qtype)
		{
			return (qtype & MyEntityQueryType.Dynamic) != 0;
		}

		public static bool HasStatic(this MyEntityQueryType qtype)
		{
			return (qtype & MyEntityQueryType.Static) != 0;
		}
	}
}
