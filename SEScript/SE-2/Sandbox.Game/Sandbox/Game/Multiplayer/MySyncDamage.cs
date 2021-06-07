using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using System;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	public class MySyncDamage
	{
		protected sealed class OnDoDamage_003C_003ESystem_Int64_0023System_Single_0023VRage_Utils_MyStringHash_0023System_Int64 : ICallSite<IMyEventOwner, long, float, MyStringHash, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long destroyableId, in float damage, in MyStringHash type, in long attackerId, in DBNull arg5, in DBNull arg6)
			{
				OnDoDamage(destroyableId, damage, type, attackerId);
			}
		}

		public static void DoDamageSynced(MyEntity entity, float damage, MyStringHash type, long attackerId)
		{
			IMyDestroyableObject myDestroyableObject = entity as IMyDestroyableObject;
			if (myDestroyableObject != null)
			{
				myDestroyableObject.DoDamage(damage, type, sync: false, null, attackerId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnDoDamage, entity.EntityId, damage, type, attackerId);
			}
		}

		[Event(null, 34)]
		[Reliable]
		[Broadcast]
		private static void OnDoDamage(long destroyableId, float damage, MyStringHash type, long attackerId)
		{
			if (MyEntities.TryGetEntityById(destroyableId, out MyEntity entity))
			{
				(entity as IMyDestroyableObject)?.DoDamage(damage, type, sync: false, null, attackerId);
			}
		}
	}
}
