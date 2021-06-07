using System;

namespace VRage.Voxels
{
	public class MyStorageDataProviderAttribute : Attribute
	{
		public readonly int ProviderTypeId;

		public Type ProviderType;

		public MyStorageDataProviderAttribute(int typeId)
		{
			ProviderTypeId = typeId;
		}
	}
}
