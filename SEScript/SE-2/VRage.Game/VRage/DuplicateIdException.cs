using System;
using VRage.ModAPI;

namespace VRage
{
	public class DuplicateIdException : Exception
	{
		public IMyEntity NewEntity;

		public IMyEntity OldEntity;

		public DuplicateIdException(IMyEntity newEntity, IMyEntity oldEntity)
		{
			NewEntity = newEntity;
			OldEntity = oldEntity;
		}

		public override string ToString()
		{
			return string.Concat("newEntity: ", OldEntity.GetType(), ", oldEntity: ", NewEntity.GetType(), base.ToString());
		}
	}
}
