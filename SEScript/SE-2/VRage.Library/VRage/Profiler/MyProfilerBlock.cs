using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage.Library.Utils;

namespace VRage.Profiler
{
	public class MyProfilerBlock
	{
		public enum BlockTypes
		{
			Normal,
			Diffed,
			Inverted,
			Added
		}

		public class MyProfilerBlockObjectBuilderInfo
		{
			public int Id;

			public MyProfilerBlockKey Key;

			public string TimeFormat;

			public string ValueFormat;

			public string CallFormat;

			public int[] NumCallsArray;

			public float[] Allocations;

			public float[] Milliseconds;

			public float[] CustomValues;

			public MyProfilerBlock Parent;

			public List<MyProfilerBlock> Children;
		}

		public struct DataReader
		{
			private readonly bool m_useOptimizations;

			private readonly OptimizableDataCache Data;

			public float this[int frame]
			{
				get
				{
					float num = Data[frame];
					if (m_useOptimizations)
					{
						num -= Data.GetOptimizedCutout(frame);
					}
					return num;
				}
			}

			public DataReader(OptimizableDataCache data, bool useOptimizations)
			{
				Data = data;
				m_useOptimizations = useOptimizations;
			}
		}

		public abstract class OptimizableDataCache
		{
			public readonly float[] RawData;

			private readonly bool[] m_valid;

			private readonly float[] m_optimizedCutout;

			private readonly MyProfilerBlock m_block;

			public float this[int frame]
			{
				get
				{
					return RawData[frame];
				}
				set
				{
					RawData[frame] = value;
					InvalidateFrameOptimizations(frame);
				}
			}

			protected OptimizableDataCache(MyProfilerBlock mBlock, float[] data = null)
			{
				m_block = mBlock;
				m_valid = new bool[MyProfiler.MAX_FRAMES];
				RawData = (data ?? new float[MyProfiler.MAX_FRAMES]);
				m_optimizedCutout = new float[MyProfiler.MAX_FRAMES];
			}

			public void InvalidateFrameOptimizations(int frame)
			{
				m_valid[frame] = false;
			}

			public float GetOptimizedCutout(int frame)
			{
				if (!m_valid[frame])
				{
					float num = 0f;
					if (m_block.IsOptimized)
					{
						num = RawData[frame];
					}
					else
					{
						foreach (MyProfilerBlock child in m_block.Children)
						{
							num += GetBlockData(child).GetOptimizedCutout(frame);
						}
					}
					m_valid[frame] = true;
					m_optimizedCutout[frame] = num;
				}
				return m_optimizedCutout[frame];
			}

			protected abstract OptimizableDataCache GetBlockData(MyProfilerBlock block);
		}

		private sealed class AllocationCache : OptimizableDataCache
		{
			public AllocationCache(MyProfilerBlock block, float[] data = null)
				: base(block, data)
			{
			}

			protected override OptimizableDataCache GetBlockData(MyProfilerBlock block)
			{
				return block.RawAllocations;
			}
		}

		private sealed class TimeCache : OptimizableDataCache
		{
			public TimeCache(MyProfilerBlock block, float[] data = null)
				: base(block, data)
			{
			}

			protected override OptimizableDataCache GetBlockData(MyProfilerBlock block)
			{
				return block.RawMilliseconds;
			}
		}

		public static Func<ulong> GetThreadAllocationStamp;

		public int NumCalls;

		public int Allocated;

		public float CustomValue;

		public MyTimeSpan Elapsed;

		public int ForceOrder;

		public string TimeFormat;

		public string ValueFormat;

		public string CallFormat;

		private bool m_isOptimized;

		public MyProfilerBlock Parent;

		public List<MyProfilerBlock> Children = new List<MyProfilerBlock>();

		public float AverageMilliseconds;

		public OptimizableDataCache RawAllocations;

		public OptimizableDataCache RawMilliseconds;

		public int[] NumCallsArray = new int[MyProfiler.MAX_FRAMES];

		public float[] CustomValues = new float[MyProfiler.MAX_FRAMES];

		private int m_beginThreadId;

		private ulong m_beginAllocationStamp;

		private long m_measureStartTimestamp;

		public BlockTypes BlockType;

		public int Id
		{
			get;
			private set;
		}

		public MyProfilerBlockKey Key
		{
			get;
			private set;
		}

		public string Name => Key.Name;

		public bool IsOptimized
		{
			get
			{
				return m_isOptimized;
			}
			set
			{
				if (m_isOptimized != value)
				{
					m_isOptimized = value;
					ForceInvalidateSelfAndParentsOptimizationsRecursive(this);
				}
			}
		}

		public bool IsDeepTreeRoot
		{
			get;
			private set;
		}

		public MyProfilerBlock()
		{
			RawMilliseconds = new TimeCache(this);
			RawAllocations = new AllocationCache(this);
		}

		public DataReader GetAllocationsReader(bool useOptimizations)
		{
			return new DataReader(RawAllocations, useOptimizations);
		}

		public DataReader GetMillisecondsReader(bool useOptimizations)
		{
			return new DataReader(RawMilliseconds, useOptimizations);
		}

		public void SetBlockData(ref MyProfilerBlockKey key, int blockId, int forceOrder = int.MaxValue, bool isDeepTreeRoot = true)
		{
			Id = blockId;
			Key = key;
			ForceOrder = forceOrder;
			IsDeepTreeRoot = isDeepTreeRoot;
		}

		public void Reset()
		{
			m_measureStartTimestamp = Stopwatch.GetTimestamp();
			Elapsed = MyTimeSpan.Zero;
			Allocated = 0;
		}

		/// <summary>
		/// Clears immediate data
		/// </summary>
		public void Clear()
		{
			Reset();
			NumCalls = 0;
			Allocated = 0;
			CustomValue = 0f;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Start(bool memoryProfiling)
		{
			NumCalls++;
			m_measureStartTimestamp = Stopwatch.GetTimestamp();
			if (memoryProfiling)
			{
				m_beginAllocationStamp = GetThreadAllocationStamp();
				m_beginThreadId = Thread.CurrentThread.ManagedThreadId;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End(bool memoryProfiling, MyTimeSpan? customTime = null)
		{
			if (memoryProfiling)
			{
				Allocated += (int)(GetThreadAllocationStamp() - m_beginAllocationStamp);
			}
			MyTimeSpan myTimeSpan;
			if (customTime.HasValue)
			{
				myTimeSpan = customTime.Value;
			}
			else
			{
				long timestamp = Stopwatch.GetTimestamp();
				myTimeSpan = MyTimeSpan.FromTicks(timestamp - m_measureStartTimestamp);
			}
			Elapsed += myTimeSpan;
		}

		private static void ForceInvalidateSelfAndParentsOptimizationsRecursive(MyProfilerBlock block)
		{
			while (block != null)
			{
				for (int i = 0; i < MyProfiler.MAX_FRAMES; i++)
				{
					block.RawAllocations.InvalidateFrameOptimizations(i);
					block.RawMilliseconds.InvalidateFrameOptimizations(i);
				}
				block = block.Parent;
			}
		}

		public override string ToString()
		{
			return Key.Name + " (" + NumCalls + " calls)";
		}

		internal void Dump(StringBuilder sb, int frame)
		{
			if (!((double)NumCallsArray[frame] < 0.01))
			{
				sb.Append($"<Block Name=\"{Name}\">\n");
				sb.Append($"<Time>{RawMilliseconds[frame]}</Time>\n<Calls>{NumCallsArray[frame]}</Calls>\n");
				foreach (MyProfilerBlock child in Children)
				{
					child.Dump(sb, frame);
				}
				sb.Append("</Block>\n");
			}
		}

		public MyProfilerBlockObjectBuilderInfo GetObjectBuilderInfo(bool serializeAllocations)
		{
			return new MyProfilerBlockObjectBuilderInfo
			{
				Id = Id,
				Key = Key,
				Parent = Parent,
				Children = Children,
				CallFormat = CallFormat,
				TimeFormat = TimeFormat,
				ValueFormat = ValueFormat,
				CustomValues = CustomValues,
				NumCallsArray = NumCallsArray,
				Milliseconds = RawMilliseconds.RawData,
				Allocations = (serializeAllocations ? RawAllocations.RawData : null)
			};
		}

		public void Init(MyProfilerBlockObjectBuilderInfo data)
		{
			Id = data.Id;
			Key = data.Key;
			CallFormat = data.CallFormat;
			TimeFormat = data.TimeFormat;
			ValueFormat = data.ValueFormat;
			CustomValues = data.CustomValues;
			NumCallsArray = data.NumCallsArray;
			Parent = data.Parent;
			Children = data.Children;
			RawMilliseconds = new TimeCache(this, data.Milliseconds);
			RawAllocations = new AllocationCache(this, data.Allocations);
		}

		private void Init(MyProfilerBlockObjectBuilderInfo data, int id, MyProfilerBlock parent)
		{
			Init(data);
			Children = new List<MyProfilerBlock>();
			CustomValues = (float[])data.CustomValues.Clone();
			NumCallsArray = (int[])data.NumCallsArray.Clone();
			RawMilliseconds = new TimeCache(this, (float[])data.Milliseconds.Clone());
			RawAllocations = new AllocationCache(this, (float[])data.Allocations.Clone());
			Id = id;
			BlockType = BlockTypes.Added;
			if (parent != null)
			{
				Parent = parent;
				MyProfilerBlockKey key = Key;
				key.ParentId = parent.Id;
				Key = key;
				parent.Children.Add(this);
			}
		}

		public void SubtractFrom(MyProfilerBlock otherBlock)
		{
			NumCalls = otherBlock.NumCalls - NumCalls;
			Allocated = otherBlock.Allocated - Allocated;
			CustomValue = otherBlock.CustomValue - CustomValue;
			Elapsed = otherBlock.Elapsed - Elapsed;
			AverageMilliseconds = otherBlock.AverageMilliseconds - AverageMilliseconds;
			for (int i = 0; i < MyProfiler.MAX_FRAMES; i++)
			{
				RawAllocations[i] = otherBlock.RawAllocations[i] - RawAllocations[i];
				RawMilliseconds[i] = otherBlock.RawMilliseconds[i] - RawMilliseconds[i];
				NumCallsArray[i] = otherBlock.NumCallsArray[i] - NumCallsArray[i];
				CustomValues[i] = otherBlock.CustomValues[i] - CustomValues[i];
			}
			BlockType = BlockTypes.Diffed;
		}

		public void Invert()
		{
			Allocated = -Allocated;
			Elapsed -= Elapsed;
			AverageMilliseconds -= AverageMilliseconds;
			for (int i = 0; i < MyProfiler.MAX_FRAMES; i++)
			{
				RawAllocations[i] = 0f - RawAllocations[i];
				RawMilliseconds[i] = 0f - RawMilliseconds[i];
			}
			BlockType = BlockTypes.Inverted;
		}

		public MyProfilerBlock Duplicate(int id, MyProfilerBlock parent)
		{
			MyProfilerBlockObjectBuilderInfo objectBuilderInfo = GetObjectBuilderInfo(serializeAllocations: true);
			MyProfilerBlock myProfilerBlock = new MyProfilerBlock();
			myProfilerBlock.Init(objectBuilderInfo, id, parent);
			return myProfilerBlock;
		}
	}
}
