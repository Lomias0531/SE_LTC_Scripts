using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	public class MyContainerGPS
	{
		protected class VRage_Game_ObjectBuilders_Components_MyContainerGPS_003C_003ETimeLeft_003C_003EAccessor : IMemberAccessor<MyContainerGPS, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContainerGPS owner, in int value)
			{
				owner.TimeLeft = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContainerGPS owner, out int value)
			{
				value = owner.TimeLeft;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyContainerGPS_003C_003EGPSName_003C_003EAccessor : IMemberAccessor<MyContainerGPS, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyContainerGPS owner, in string value)
			{
				owner.GPSName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyContainerGPS owner, out string value)
			{
				value = owner.GPSName;
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyContainerGPS_003C_003EActor : IActivator, IActivator<MyContainerGPS>
		{
			private sealed override object CreateInstance()
			{
				return new MyContainerGPS();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContainerGPS CreateInstance()
			{
				return new MyContainerGPS();
			}

			MyContainerGPS IActivator<MyContainerGPS>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int TimeLeft;

		[ProtoMember(4)]
		public string GPSName;

		public MyContainerGPS()
		{
		}

		public MyContainerGPS(int time, string name)
		{
			TimeLeft = time;
			GPSName = name;
		}
	}
}
