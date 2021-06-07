using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Utils;
using VRageRender;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ProfilerSnapshot : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003EProfilers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, List<MyObjectBuilder_Profiler>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in List<MyObjectBuilder_Profiler> value)
			{
				owner.Profilers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out List<MyObjectBuilder_Profiler> value)
			{
				value = owner.Profilers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003ESimulationFrames_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, List<MyRenderProfiler.FrameInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in List<MyRenderProfiler.FrameInfo> value)
			{
				owner.SimulationFrames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out List<MyRenderProfiler.FrameInfo> value)
			{
				value = owner.SimulationFrames;
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_ProfilerSnapshot_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProfilerSnapshot>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProfilerSnapshot();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProfilerSnapshot CreateInstance()
			{
				return new MyObjectBuilder_ProfilerSnapshot();
			}

			MyObjectBuilder_ProfilerSnapshot IActivator<MyObjectBuilder_ProfilerSnapshot>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_Profiler> Profilers;

		public List<MyRenderProfiler.FrameInfo> SimulationFrames;

		public static MyObjectBuilder_ProfilerSnapshot GetObjectBuilder(MyRenderProfiler profiler)
		{
			MyObjectBuilder_ProfilerSnapshot myObjectBuilder_ProfilerSnapshot = new MyObjectBuilder_ProfilerSnapshot();
			List<MyProfiler> threadProfilers = MyRenderProfiler.ThreadProfilers;
			lock (threadProfilers)
			{
				myObjectBuilder_ProfilerSnapshot.Profilers = new List<MyObjectBuilder_Profiler>(threadProfilers.Count);
				myObjectBuilder_ProfilerSnapshot.Profilers.AddRange(threadProfilers.Select(MyObjectBuilder_Profiler.GetObjectBuilder));
			}
			myObjectBuilder_ProfilerSnapshot.SimulationFrames = MyRenderProfiler.FrameTimestamps.ToList();
			return myObjectBuilder_ProfilerSnapshot;
		}

		public void Init(MyRenderProfiler profiler, SnapshotType type, bool subtract)
		{
			List<MyProfiler> list = Profilers.Select(MyObjectBuilder_Profiler.Init).ToList();
			ConcurrentQueue<MyRenderProfiler.FrameInfo> frameTimestamps = new ConcurrentQueue<MyRenderProfiler.FrameInfo>(SimulationFrames);
			if (subtract)
			{
				MyRenderProfiler.SubtractOnlineSnapshot(type, list, frameTimestamps);
			}
			else
			{
				MyRenderProfiler.PushOnlineSnapshot(type, list, frameTimestamps);
			}
			MyRenderProfiler.SelectedProfiler = ((list.Count > 0) ? list[0] : null);
		}

		private static void SaveToFile(int index)
		{
			try
			{
				MyObjectBuilder_ProfilerSnapshot objectBuilder = GetObjectBuilder(MyRenderProxy.GetRenderProfiler());
				MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.UserDataPath, "FullProfiler-" + (int)index), compress: true, objectBuilder);
			}
			catch
			{
			}
		}

		private static void LoadFromFile(int index, bool subtract)
		{
			try
			{
				MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.UserDataPath, "FullProfiler-" + (int)index), out MyObjectBuilder_ProfilerSnapshot objectBuilder);
				objectBuilder.Init(MyRenderProxy.GetRenderProfiler(), SnapshotType.Snapshot, subtract);
			}
			catch
			{
			}
		}

		public static void SetDelegates()
		{
			MyRenderProfiler.SaveProfilerToFile = SaveToFile;
			MyRenderProfiler.LoadProfilerFromFile = LoadFromFile;
		}
	}
}
