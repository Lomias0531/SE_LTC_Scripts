using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class Cutscene
	{
		protected class VRage_Game_Cutscene_003C_003EName_003C_003EAccessor : IMemberAccessor<Cutscene, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_Cutscene_003C_003EStartEntity_003C_003EAccessor : IMemberAccessor<Cutscene, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in string value)
			{
				owner.StartEntity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out string value)
			{
				value = owner.StartEntity;
			}
		}

		protected class VRage_Game_Cutscene_003C_003EStartLookAt_003C_003EAccessor : IMemberAccessor<Cutscene, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in string value)
			{
				owner.StartLookAt = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out string value)
			{
				value = owner.StartLookAt;
			}
		}

		protected class VRage_Game_Cutscene_003C_003ENextCutscene_003C_003EAccessor : IMemberAccessor<Cutscene, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in string value)
			{
				owner.NextCutscene = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out string value)
			{
				value = owner.NextCutscene;
			}
		}

		protected class VRage_Game_Cutscene_003C_003EStartingFOV_003C_003EAccessor : IMemberAccessor<Cutscene, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in float value)
			{
				owner.StartingFOV = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out float value)
			{
				value = owner.StartingFOV;
			}
		}

		protected class VRage_Game_Cutscene_003C_003ECanBeSkipped_003C_003EAccessor : IMemberAccessor<Cutscene, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in bool value)
			{
				owner.CanBeSkipped = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out bool value)
			{
				value = owner.CanBeSkipped;
			}
		}

		protected class VRage_Game_Cutscene_003C_003EFireEventsDuringSkip_003C_003EAccessor : IMemberAccessor<Cutscene, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in bool value)
			{
				owner.FireEventsDuringSkip = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out bool value)
			{
				value = owner.FireEventsDuringSkip;
			}
		}

		protected class VRage_Game_Cutscene_003C_003ESequenceNodes_003C_003EAccessor : IMemberAccessor<Cutscene, List<CutsceneSequenceNode>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref Cutscene owner, in List<CutsceneSequenceNode> value)
			{
				owner.SequenceNodes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref Cutscene owner, out List<CutsceneSequenceNode> value)
			{
				value = owner.SequenceNodes;
			}
		}

		private class VRage_Game_Cutscene_003C_003EActor : IActivator, IActivator<Cutscene>
		{
			private sealed override object CreateInstance()
			{
				return new Cutscene();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override Cutscene CreateInstance()
			{
				return new Cutscene();
			}

			Cutscene IActivator<Cutscene>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(4)]
		public string Name = "";

		[ProtoMember(7)]
		public string StartEntity = "";

		[ProtoMember(10)]
		public string StartLookAt = "";

		[ProtoMember(13)]
		public string NextCutscene = "";

		[ProtoMember(16)]
		public float StartingFOV = 70f;

		[ProtoMember(19)]
		public bool CanBeSkipped = true;

		[ProtoMember(22)]
		public bool FireEventsDuringSkip = true;

		[ProtoMember(25)]
		[XmlArrayItem("Node")]
		public List<CutsceneSequenceNode> SequenceNodes;
	}
}
