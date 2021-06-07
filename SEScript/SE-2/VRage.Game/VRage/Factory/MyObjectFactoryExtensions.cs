using VRage.Core;
using VRage.Game.Common;
using VRage.ObjectBuilders;

namespace VRage.Factory
{
	public static class MyObjectFactoryExtensions
	{
		/// <summary>
		/// Create a new instance of an object and deserialize it.
		///
		/// Only valid for factories that create IMyObject instances.
		/// </summary>
		/// <typeparam name="TAttribute">Attribute type of the factory.</typeparam>
		/// <typeparam name="TCreated">Created type of the factory.</typeparam>
		/// <param name="self">The factory.</param>
		/// <param name="builder">The object builder for the type.</param>
		/// <returns></returns>
		public static TCreated CreateAndDeserialize<TAttribute, TCreated>(this MyObjectFactory<TAttribute, TCreated> self, MyObjectBuilder_Base builder) where TAttribute : MyFactoryTagAttribute where TCreated : class, IMyObject
		{
			TCreated val = self.CreateInstance(builder.TypeId);
			val.Deserialize(builder);
			return val;
		}
	}
}
