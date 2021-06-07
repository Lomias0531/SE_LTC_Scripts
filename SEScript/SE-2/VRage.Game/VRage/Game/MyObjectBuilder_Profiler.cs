using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Profiler : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003EProfilingBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, List<MyObjectBuilder_ProfilerBlock>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in List<MyObjectBuilder_ProfilerBlock> value)
			{
				owner.ProfilingBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out List<MyObjectBuilder_ProfilerBlock> value)
			{
				value = owner.ProfilingBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003ERootBlocks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, List<MyProfilerBlockKey>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in List<MyProfilerBlockKey> value)
			{
				owner.RootBlocks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out List<MyProfilerBlockKey> value)
			{
				value = owner.RootBlocks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003ETasks_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, List<MyProfiler.TaskInfo>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in List<MyProfiler.TaskInfo> value)
			{
				owner.Tasks = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out List<MyProfiler.TaskInfo> value)
			{
				value = owner.Tasks;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003ETotalCalls_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, int[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in int[] value)
			{
				owner.TotalCalls = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out int[] value)
			{
				value = owner.TotalCalls;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003ECommitTimes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, long[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in long[] value)
			{
				owner.CommitTimes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out long[] value)
			{
				value = owner.CommitTimes;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003ECustomName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in string value)
			{
				owner.CustomName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out string value)
			{
				value = owner.CustomName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003EAxisName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in string value)
			{
				owner.AxisName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out string value)
			{
				value = owner.AxisName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003EShallowProfile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Profiler owner, in bool value)
			{
				owner.ShallowProfile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Profiler owner, out bool value)
			{
				value = owner.ShallowProfile;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Profiler owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Profiler owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Profiler owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Profiler owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Profiler owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Profiler owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Profiler_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Profiler owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Profiler owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Profiler_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Profiler>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Profiler();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Profiler CreateInstance()
			{
				return new MyObjectBuilder_Profiler();
			}

			MyObjectBuilder_Profiler IActivator<MyObjectBuilder_Profiler>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_ProfilerBlock> ProfilingBlocks;

		public List<MyProfilerBlockKey> RootBlocks;

		public List<MyProfiler.TaskInfo> Tasks;

		public int[] TotalCalls;

		public long[] CommitTimes;

		public string CustomName = "";

		public string AxisName = "";

		public bool ShallowProfile;

		public static MyObjectBuilder_Profiler GetObjectBuilder(MyProfiler profiler)
		{
			MyProfiler.MyProfilerObjectBuilderInfo objectBuilderInfo = profiler.GetObjectBuilderInfo();
			MyObjectBuilder_Profiler myObjectBuilder_Profiler = new MyObjectBuilder_Profiler();
			myObjectBuilder_Profiler.ProfilingBlocks = new List<MyObjectBuilder_ProfilerBlock>();
			foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock in objectBuilderInfo.ProfilingBlocks)
			{
				myObjectBuilder_Profiler.ProfilingBlocks.Add(MyObjectBuilder_ProfilerBlock.GetObjectBuilder(profilingBlock.Value, profiler.AllocationProfiling));
			}
			myObjectBuilder_Profiler.RootBlocks = new List<MyProfilerBlockKey>();
			foreach (MyProfilerBlock rootBlock in objectBuilderInfo.RootBlocks)
			{
				myObjectBuilder_Profiler.RootBlocks.Add(rootBlock.Key);
			}
			myObjectBuilder_Profiler.Tasks = objectBuilderInfo.Tasks;
			myObjectBuilder_Profiler.TotalCalls = objectBuilderInfo.TotalCalls;
			myObjectBuilder_Profiler.CustomName = objectBuilderInfo.CustomName;
			myObjectBuilder_Profiler.AxisName = objectBuilderInfo.AxisName;
			myObjectBuilder_Profiler.ShallowProfile = objectBuilderInfo.ShallowProfile;
			myObjectBuilder_Profiler.CommitTimes = objectBuilderInfo.CommitTimes;
			return myObjectBuilder_Profiler;
		}

		public static MyProfiler Init(MyObjectBuilder_Profiler objectBuilder)
		{
			MyProfiler.MyProfilerObjectBuilderInfo myProfilerObjectBuilderInfo = new MyProfiler.MyProfilerObjectBuilderInfo();
			myProfilerObjectBuilderInfo.ProfilingBlocks = new Dictionary<MyProfilerBlockKey, MyProfilerBlock>();
			foreach (MyObjectBuilder_ProfilerBlock profilingBlock in objectBuilder.ProfilingBlocks)
			{
				myProfilerObjectBuilderInfo.ProfilingBlocks.Add(profilingBlock.Key, new MyProfilerBlock());
			}
			foreach (MyObjectBuilder_ProfilerBlock profilingBlock2 in objectBuilder.ProfilingBlocks)
			{
				MyObjectBuilder_ProfilerBlock.Init(profilingBlock2, myProfilerObjectBuilderInfo);
			}
			myProfilerObjectBuilderInfo.RootBlocks = new List<MyProfilerBlock>();
			foreach (MyProfilerBlockKey rootBlock in objectBuilder.RootBlocks)
			{
				myProfilerObjectBuilderInfo.RootBlocks.Add(myProfilerObjectBuilderInfo.ProfilingBlocks[rootBlock]);
			}
			myProfilerObjectBuilderInfo.TotalCalls = objectBuilder.TotalCalls;
			myProfilerObjectBuilderInfo.CustomName = objectBuilder.CustomName;
			myProfilerObjectBuilderInfo.AxisName = objectBuilder.AxisName;
			myProfilerObjectBuilderInfo.ShallowProfile = objectBuilder.ShallowProfile;
			myProfilerObjectBuilderInfo.Tasks = objectBuilder.Tasks;
			myProfilerObjectBuilderInfo.CommitTimes = objectBuilder.CommitTimes;
			MyProfiler myProfiler = new MyProfiler(allocationProfiling: false, myProfilerObjectBuilderInfo.CustomName, myProfilerObjectBuilderInfo.AxisName, shallowProfile: false);
			myProfiler.Init(myProfilerObjectBuilderInfo);
			return myProfiler;
		}

		public static void SaveToFile(int index)
		{
			try
			{
				MyObjectBuilder_Profiler objectBuilder = GetObjectBuilder(MyRenderProfiler.SelectedProfiler);
				MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.UserDataPath, "Profiler-" + (int)index), compress: true, objectBuilder);
			}
			catch
			{
			}
		}

		public static void LoadFromFile(int index)
		{
			try
			{
				MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.UserDataPath, "Profiler-" + (int)index), out MyObjectBuilder_Profiler objectBuilder);
				MyRenderProfiler.SelectedProfiler = Init(objectBuilder);
			}
			catch
			{
			}
		}
	}
}
