using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class CutsceneSequenceNodeWaypoint
	{
		protected class VRage_Game_CutsceneSequenceNodeWaypoint_003C_003EName_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNodeWaypoint, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNodeWaypoint owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNodeWaypoint owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_CutsceneSequenceNodeWaypoint_003C_003ETime_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNodeWaypoint, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNodeWaypoint owner, in float value)
			{
				owner.Time = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNodeWaypoint owner, out float value)
			{
				value = owner.Time;
			}
		}

		private class VRage_Game_CutsceneSequenceNodeWaypoint_003C_003EActor : IActivator, IActivator<CutsceneSequenceNodeWaypoint>
		{
			private sealed override object CreateInstance()
			{
				return new CutsceneSequenceNodeWaypoint();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override CutsceneSequenceNodeWaypoint CreateInstance()
			{
				return new CutsceneSequenceNodeWaypoint();
			}

			CutsceneSequenceNodeWaypoint IActivator<CutsceneSequenceNodeWaypoint>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(73)]
		[XmlAttribute]
		public string Name = "";

		[ProtoMember(76)]
		[XmlAttribute]
		public float Time;
	}
}
