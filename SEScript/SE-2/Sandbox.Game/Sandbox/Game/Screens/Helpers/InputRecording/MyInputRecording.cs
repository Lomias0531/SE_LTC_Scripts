using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
	[Serializable]
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
	public class MyInputRecording
	{
		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003EName_003C_003EAccessor : IMemberAccessor<MyInputRecording, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyInputRecording, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in string value)
			{
				owner.Description = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out string value)
			{
				value = owner.Description;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003ESnapshotSequence_003C_003EAccessor : IMemberAccessor<MyInputRecording, List<MyInputSnapshot>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in List<MyInputSnapshot> value)
			{
				owner.SnapshotSequence = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out List<MyInputSnapshot> value)
			{
				value = owner.SnapshotSequence;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003ESession_003C_003EAccessor : IMemberAccessor<MyInputRecording, MyInputRecordingSession>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in MyInputRecordingSession value)
			{
				owner.Session = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out MyInputRecordingSession value)
			{
				value = owner.Session;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003EOriginalWidth_003C_003EAccessor : IMemberAccessor<MyInputRecording, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in int value)
			{
				owner.OriginalWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out int value)
			{
				value = owner.OriginalWidth;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003EOriginalHeight_003C_003EAccessor : IMemberAccessor<MyInputRecording, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in int value)
			{
				owner.OriginalHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out int value)
			{
				value = owner.OriginalHeight;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003EUseReplayInstead_003C_003EAccessor : IMemberAccessor<MyInputRecording, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in bool value)
			{
				owner.UseReplayInstead = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out bool value)
			{
				value = owner.UseReplayInstead;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003Em_currentSnapshotNumber_003C_003EAccessor : IMemberAccessor<MyInputRecording, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in int value)
			{
				owner.m_currentSnapshotNumber = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out int value)
			{
				value = owner.m_currentSnapshotNumber;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003Em_startScreenWidth_003C_003EAccessor : IMemberAccessor<MyInputRecording, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in int value)
			{
				owner.m_startScreenWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out int value)
			{
				value = owner.m_startScreenWidth;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyInputRecording_003C_003Em_startScreenHeight_003C_003EAccessor : IMemberAccessor<MyInputRecording, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyInputRecording owner, in int value)
			{
				owner.m_startScreenHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyInputRecording owner, out int value)
			{
				value = owner.m_startScreenHeight;
			}
		}

		public string Name;

		public string Description;

		public List<MyInputSnapshot> SnapshotSequence;

		public MyInputRecordingSession Session;

		public int OriginalWidth;

		public int OriginalHeight;

		public bool UseReplayInstead;

		private int m_currentSnapshotNumber;

		private int m_startScreenWidth;

		private int m_startScreenHeight;

		public MyInputRecording()
		{
			m_currentSnapshotNumber = 0;
			SnapshotSequence = new List<MyInputSnapshot>();
		}

		public bool IsDone()
		{
			return m_currentSnapshotNumber == SnapshotSequence.Count;
		}

		public void Save()
		{
			Directory.CreateDirectory(Name);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(MyInputRecording));
			using (TextWriter textWriter = new StreamWriter(Path.Combine(Name, "input.xml"), append: false))
			{
				xmlSerializer.Serialize(textWriter, this);
			}
		}

		public void SetStartingScreenDimensions(int width, int height)
		{
			m_startScreenWidth = width;
			m_startScreenHeight = height;
		}

		public int GetStartingScreenWidth()
		{
			return m_startScreenWidth;
		}

		public int GetStartingScreenHeight()
		{
			return m_startScreenHeight;
		}

		public Vector2 GetMouseNormalizationFactor()
		{
			return new Vector2((float)m_startScreenWidth / (float)OriginalWidth, (float)m_startScreenHeight / (float)OriginalHeight);
		}

		public MyInputSnapshot GetNextSnapshot()
		{
			return SnapshotSequence[m_currentSnapshotNumber++];
		}

		public void RemoveRest()
		{
			m_currentSnapshotNumber--;
			SnapshotSequence.RemoveRange(m_currentSnapshotNumber, SnapshotSequence.Count - m_currentSnapshotNumber);
		}

		public MyInputSnapshot GetCurrentSnapshot()
		{
			return SnapshotSequence[m_currentSnapshotNumber];
		}

		public static MyInputRecording FromFile(string filename)
		{
			using (StreamReader textReader = new StreamReader(filename))
			{
				return (MyInputRecording)new XmlSerializer(typeof(MyInputRecording)).Deserialize(textReader);
			}
		}

		public void AddSnapshot(MyInputSnapshot snapshot)
		{
			SnapshotSequence.Add(snapshot);
		}
	}
}
