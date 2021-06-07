using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
	[Serializable]
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
	public class MyInputSnapshot
	{
		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputSnapshot_003C_003EMouseSnapshot_003C_003EAccessor : IMemberAccessor<MyInputSnapshot, MyMouseSnapshot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputSnapshot owner, in MyMouseSnapshot value)
			{
				owner.MouseSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputSnapshot owner, out MyMouseSnapshot value)
			{
				value = owner.MouseSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputSnapshot_003C_003EKeyboardSnapshot_003C_003EAccessor : IMemberAccessor<MyInputSnapshot, List<byte>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputSnapshot owner, in List<byte> value)
			{
				owner.KeyboardSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputSnapshot owner, out List<byte> value)
			{
				value = owner.KeyboardSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputSnapshot_003C_003EKeyboardSnapshotText_003C_003EAccessor : IMemberAccessor<MyInputSnapshot, List<char>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputSnapshot owner, in List<char> value)
			{
				owner.KeyboardSnapshotText = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputSnapshot owner, out List<char> value)
			{
				value = owner.KeyboardSnapshotText;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputSnapshot_003C_003EJoystickSnapshot_003C_003EAccessor : IMemberAccessor<MyInputSnapshot, MyJoystickStateSnapshot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputSnapshot owner, in MyJoystickStateSnapshot value)
			{
				owner.JoystickSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputSnapshot owner, out MyJoystickStateSnapshot value)
			{
				value = owner.JoystickSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputSnapshot_003C_003ESnapshotTimestamp_003C_003EAccessor : IMemberAccessor<MyInputSnapshot, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputSnapshot owner, in int value)
			{
				owner.SnapshotTimestamp = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputSnapshot owner, out int value)
			{
				value = owner.SnapshotTimestamp;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputSnapshot_003C_003EMouseCursorPosition_003C_003EAccessor : IMemberAccessor<MyInputSnapshot, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputSnapshot owner, in Vector2 value)
			{
				owner.MouseCursorPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputSnapshot owner, out Vector2 value)
			{
				value = owner.MouseCursorPosition;
			}
		}

		public MyMouseSnapshot MouseSnapshot
		{
			get;
			set;
		}

		public List<byte> KeyboardSnapshot
		{
			get;
			set;
		}

		public List<char> KeyboardSnapshotText
		{
			get;
			set;
		}

		public MyJoystickStateSnapshot JoystickSnapshot
		{
			get;
			set;
		}

		public int SnapshotTimestamp
		{
			get;
			set;
		}

		public Vector2 MouseCursorPosition
		{
			get;
			set;
		}
	}
}
