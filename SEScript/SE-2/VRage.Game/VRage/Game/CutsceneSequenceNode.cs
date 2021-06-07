using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class CutsceneSequenceNode
	{
		protected class VRage_Game_CutsceneSequenceNode_003C_003ETime_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in float value)
			{
				owner.Time = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out float value)
			{
				value = owner.Time;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003ELookAt_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.LookAt = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.LookAt;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EEvent_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.Event = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.Event;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EEventDelay_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in float value)
			{
				owner.EventDelay = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out float value)
			{
				value = owner.EventDelay;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003ELockRotationTo_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.LockRotationTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.LockRotationTo;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EAttachTo_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.AttachTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.AttachTo;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EAttachPositionTo_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.AttachPositionTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.AttachPositionTo;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EAttachRotationTo_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.AttachRotationTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.AttachRotationTo;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EMoveTo_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.MoveTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.MoveTo;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003ESetPositionTo_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.SetPositionTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.SetPositionTo;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EChangeFOVTo_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in float value)
			{
				owner.ChangeFOVTo = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out float value)
			{
				value = owner.ChangeFOVTo;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003ERotateTowards_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.RotateTowards = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.RotateTowards;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003ESetRorationLike_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.SetRorationLike = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.SetRorationLike;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003ERotateLike_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in string value)
			{
				owner.RotateLike = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out string value)
			{
				value = owner.RotateLike;
			}
		}

		protected class VRage_Game_CutsceneSequenceNode_003C_003EWaypoints_003C_003EAccessor : IMemberAccessor<CutsceneSequenceNode, List<CutsceneSequenceNodeWaypoint>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref CutsceneSequenceNode owner, in List<CutsceneSequenceNodeWaypoint> value)
			{
				owner.Waypoints = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref CutsceneSequenceNode owner, out List<CutsceneSequenceNodeWaypoint> value)
			{
				value = owner.Waypoints;
			}
		}

		private class VRage_Game_CutsceneSequenceNode_003C_003EActor : IActivator, IActivator<CutsceneSequenceNode>
		{
			private sealed override object CreateInstance()
			{
				return new CutsceneSequenceNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override CutsceneSequenceNode CreateInstance()
			{
				return new CutsceneSequenceNode();
			}

			CutsceneSequenceNode IActivator<CutsceneSequenceNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(28)]
		[XmlAttribute]
		public float Time;

		[ProtoMember(31)]
		[XmlAttribute]
		public string LookAt;

		[ProtoMember(34)]
		[XmlAttribute]
		public string Event;

		[ProtoMember(37)]
		[XmlAttribute]
		public float EventDelay;

		[ProtoMember(40)]
		[XmlAttribute]
		public string LockRotationTo;

		[ProtoMember(43)]
		[XmlAttribute]
		public string AttachTo;

		[ProtoMember(46)]
		[XmlAttribute]
		public string AttachPositionTo;

		[ProtoMember(49)]
		[XmlAttribute]
		public string AttachRotationTo;

		[ProtoMember(52)]
		[XmlAttribute]
		public string MoveTo;

		[ProtoMember(55)]
		[XmlAttribute]
		public string SetPositionTo;

		[ProtoMember(58)]
		[XmlAttribute]
		public float ChangeFOVTo;

		[ProtoMember(61)]
		[XmlAttribute]
		public string RotateTowards;

		[ProtoMember(64)]
		[XmlAttribute]
		public string SetRorationLike;

		[ProtoMember(67)]
		[XmlAttribute]
		public string RotateLike;

		[ProtoMember(70)]
		[XmlArrayItem("Waypoint")]
		public List<CutsceneSequenceNodeWaypoint> Waypoints;

		public string GetNodeSummary()
		{
			string str = string.IsNullOrEmpty(Event) ? "" : (" - \"" + Event + "\" event" + ((EventDelay > 0f) ? (" (" + EventDelay + "s delay)") : ""));
			return Time + "s" + str;
		}

		public string GetNodeDescription()
		{
			StringBuilder stringBuilder = new StringBuilder(Time + "s");
			if (!string.IsNullOrEmpty(Event))
			{
				stringBuilder.Append(", \"" + Event + "\" event" + ((EventDelay > 0f) ? (" (" + EventDelay + "s delay)") : ""));
			}
			if (ChangeFOVTo > 0f)
			{
				stringBuilder.Append(", change FOV to " + ChangeFOVTo + " over time");
			}
			if (!string.IsNullOrEmpty(SetPositionTo))
			{
				stringBuilder.Append(", set position to \"" + SetPositionTo + "\"");
			}
			if (!string.IsNullOrEmpty(MoveTo))
			{
				stringBuilder.Append(", move over time to \"" + MoveTo + "\"");
			}
			if (!string.IsNullOrEmpty(LookAt))
			{
				stringBuilder.Append(", look at \"" + LookAt + "\" instantly");
			}
			if (!string.IsNullOrEmpty(RotateTowards))
			{
				stringBuilder.Append(", look at \"" + RotateTowards + "\" over time");
			}
			if (!string.IsNullOrEmpty(SetRorationLike))
			{
				stringBuilder.Append(", set rotation like \"" + SetRorationLike + "\" instantly");
			}
			if (!string.IsNullOrEmpty(RotateLike))
			{
				stringBuilder.Append(", change rotation like \"" + RotateLike + "\" over time");
			}
			if (LockRotationTo != null)
			{
				if (string.IsNullOrEmpty(LockRotationTo))
				{
					stringBuilder.Append(", stop looking at target");
				}
				else
				{
					stringBuilder.Append(", look at \"" + LockRotationTo + "\" until disabled");
				}
			}
			if (AttachTo != null)
			{
				if (string.IsNullOrEmpty(AttachTo))
				{
					stringBuilder.Append(", stop attachment");
				}
				else
				{
					stringBuilder.Append(", attach to \"" + AttachTo + "\"");
				}
			}
			else
			{
				if (AttachPositionTo != null)
				{
					if (string.IsNullOrEmpty(AttachPositionTo))
					{
						stringBuilder.Append(", stop position attachment");
					}
					else
					{
						stringBuilder.Append(", attach position to \"" + AttachPositionTo + "\"");
					}
				}
				if (AttachRotationTo != null)
				{
					if (string.IsNullOrEmpty(AttachRotationTo))
					{
						stringBuilder.Append(", stop rotation attachment");
					}
					else
					{
						stringBuilder.Append(", attach rotation to \"" + AttachRotationTo + "\"");
					}
				}
			}
			if (Waypoints != null && Waypoints.Count > 1)
			{
				stringBuilder.Append(", movement spline over " + (int)Waypoints.Count + " points");
			}
			return stringBuilder.ToString();
		}
	}
}
