using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage.Collections;
using VRage.Library.Utils;
using VRage.Profiler;
using VRageMath;

namespace VRageRender.Profiler
{
	public abstract class MyRenderProfilerRendering : MyRenderProfiler
	{
		private bool m_initialized;

		private readonly NumberFormatInfo m_numberInfo = new NumberFormatInfo();

		private readonly Stack<MyProfiler.TaskInfo> m_taskStack = new Stack<MyProfiler.TaskInfo>(10);

		/// <summary>
		/// Returns viewport size in pixels
		/// </summary>
		protected abstract Vector2 ViewportSize
		{
			get;
		}

		protected abstract void Init();

		protected abstract Vector2 MeasureText(StringBuilder text, float scale);

		protected abstract void BeginDraw();

		protected abstract float DrawText(Vector2 screenCoord, StringBuilder text, Color color, float scale);

		protected abstract float DrawTextShadow(Vector2 screenCoord, StringBuilder text, Color color, float scale);

		private void DrawTextShadow(float x, float y, StringBuilder text, Color color, float scale)
		{
			DrawText(new Vector2(x, y), text, color, scale);
		}

		protected abstract void BeginLineBatch();

		protected abstract void DrawOnScreenLine(Vector3 v0, Vector3 v1, Color color);

		private void DrawOnScreenLine(float x1, float y1, float x2, float y2, Color color)
		{
			DrawOnScreenLine(new Vector3(x1, y1, 0f), new Vector3(x2, y2, 0f), color);
		}

		private void DrawOnScreenQuadOutline(float x1, float y1, float x2, float y2, Color color)
		{
			DrawOnScreenLine(x1, y1, x2, y1, color);
			DrawOnScreenLine(x2, y1, x2, y2, color);
			DrawOnScreenLine(x2, y2, x1, y2, color);
			DrawOnScreenLine(x1, y2, x1, y1, color);
		}

		protected abstract void EndLineBatch();

		protected abstract void BeginPrimitiveBatch();

		protected abstract void DrawOnScreenTriangle(Vector3 v0, Vector3 v1, Vector3 v3, Color color);

		protected abstract void DrawOnScreenQuad(Vector3 v0, Vector3 v1, Vector3 v3, Vector3 v4, Color color);

		private void DrawOnScreenQuad(float x1, float y1, float x2, float y2, Color color)
		{
			DrawOnScreenQuad(new Vector3(x1, y1, 0f), new Vector3(x2, y1, 0f), new Vector3(x2, y2, 0f), new Vector3(x1, y2, 0f), color);
		}

		protected abstract void EndPrimitiveBatch();

		private void DrawTask(ref MyProfiler.TaskInfo task, long timeBegin, long timeEnd, float y, MyDrawArea area, Color color, int taskDepth, bool isSelected = false)
		{
			long num = task.Started;
			if (num < timeBegin)
			{
				num = timeBegin;
			}
			float num2 = -1f + area.XStart;
			float value = num2 + area.XScale * 2f;
			float yStart = area.YStart;
			float value2 = yStart + area.YScale;
			float num3 = timeEnd - timeBegin;
			float num4 = num - timeBegin;
			float num5 = task.Finished - timeBegin;
			float amount = num4 / num3;
			float amount2 = num5 / num3;
			float num6 = MathHelper.Lerp(num2, value, amount);
			float num7 = MathHelper.Lerp(num2, value, amount2);
			float num8 = (float)taskDepth * 0.01f;
			float y2 = MathHelper.Lerp(yStart, value2, y);
			float y3 = MathHelper.Lerp(yStart, value2, y + 0.05f - num8);
			bool num9 = (double)(num7 - num6) > 0.004;
			float a = 1f;
			if (num9)
			{
				a = MathHelper.Lerp(0.4f, 0.9f, MathHelper.Clamp((num3 - 10000f) / 1000000f, 0f, 1f));
			}
			Color color2 = color.Shade(1.1f).Alpha(a);
			DrawOnScreenQuad(num6, y2, num7, y3, color2);
			if (isSelected)
			{
				color = Color.Black;
			}
			if (num9)
			{
				DrawOnScreenQuadOutline(num6, y2, num7, y3, color);
			}
		}

		private void DrawBlockLine(MyProfilerBlock.DataReader data, int start, int end, MyDrawArea area, Color color)
		{
			Vector3 zero = Vector3.Zero;
			Vector3 zero2 = Vector3.Zero;
			for (int i = start + 1; i <= end; i++)
			{
				zero.X = -1f + area.XStart + area.XScale * (float)(i - 1) / 512f;
				zero.Y = area.YStart + data[i - 1] * area.YScale / area.YRange;
				zero.Z = 0f;
				zero2.X = -1f + area.XStart + area.XScale * (float)i / 512f;
				zero2.Y = area.YStart + data[i] * area.YScale / area.YRange;
				zero2.Z = 0f;
				if (zero.Y - area.YStart > 0.001f || zero2.Y - area.YStart > 0.001f)
				{
					DrawOnScreenLine(zero, zero2, color);
				}
			}
		}

		private void DrawBlockLineSeparated(MyProfilerBlock.DataReader data, int lastFrameIndex, int windowEnd, MyDrawArea scale, Color color)
		{
			if (lastFrameIndex > windowEnd)
			{
				DrawBlockLine(data, windowEnd, lastFrameIndex, scale, color);
				return;
			}
			DrawBlockLine(data, 0, lastFrameIndex, scale, color);
			DrawBlockLine(data, windowEnd, MyProfiler.MAX_FRAMES - 1, scale, color);
		}

		private void DrawEvent(float textPosY, MyProfilerBlock profilerBlock, bool useOptimizations, int blockIndex, int frameIndex, int lastValidFrame, ref Color color, bool isHeaderLine)
		{
			int arg = -1;
			float num = 0f;
			float arg2 = 0f;
			float arg3 = 0f;
			if (MyRenderProfiler.IsValidIndex(frameIndex, lastValidFrame))
			{
				arg = profilerBlock.NumCallsArray[frameIndex];
				arg2 = profilerBlock.CustomValues[frameIndex];
				arg3 = profilerBlock.GetMillisecondsReader(useOptimizations)[frameIndex];
				num = profilerBlock.GetAllocationsReader(useOptimizations)[frameIndex];
			}
			if (blockIndex >= 0)
			{
				Text.Clear().Append(blockIndex + 1).Append(" ");
			}
			else
			{
				Text.Clear().Append("- ");
			}
			if (MyRenderProfiler.m_selectedProfiler.ShallowProfileEnabled && profilerBlock.IsDeepTreeRoot)
			{
				Text.Append("[S] ");
			}
			switch (profilerBlock.BlockType)
			{
			case MyProfilerBlock.BlockTypes.Added:
				Text.Append("[A] ");
				break;
			case MyProfilerBlock.BlockTypes.Diffed:
				Text.Append("[D] ");
				break;
			case MyProfilerBlock.BlockTypes.Inverted:
				Text.Append("[I] ");
				break;
			}
			switch (MyRenderProfiler.m_blockRender)
			{
			case BlockRender.Name:
				Text.Append(profilerBlock.Name);
				break;
			case BlockRender.Source:
			{
				string file = profilerBlock.Key.File;
				int startIndex = 0;
				int num2 = file.Length;
				if (num2 > 40)
				{
					startIndex = num2 - 40;
					num2 = 40;
				}
				Text.Append(file, startIndex, num2).Append(':').Append(profilerBlock.Key.Line);
				break;
			}
			}
			DrawTextShadow(new Vector2(20f, textPosY), Text, color, 0.7f);
			float num3 = 500f;
			Text.Clear();
			Text.Append("(").Append(profilerBlock.Children.Count).Append(") ");
			DrawTextShadow(new Vector2(20f + num3, textPosY), Text, color, 0.7f);
			num3 += 35f;
			Text.Clear();
			num3 += 108.5f;
			if (isHeaderLine)
			{
				float num4 = 0f;
				foreach (MyProfilerBlock child in profilerBlock.Children)
				{
					num4 += child.GetMillisecondsReader(useOptimizations)[frameIndex];
				}
				Text.Clear();
				Text.ConcatFormat(profilerBlock.TimeFormat ?? "{0:.00}", num4);
				DrawTextShadow(new Vector2(20f + num3 - 63f, textPosY), Text, color, 0.7f);
				Text.Clear().Append('/');
				DrawTextShadow(new Vector2(20f + num3 - 14f, textPosY), Text, color, 0.7f);
			}
			Text.Clear();
			Text.ConcatFormat(profilerBlock.TimeFormat ?? "{0:.00}ms", arg3);
			DrawTextShadow(new Vector2(20f + num3, textPosY), Text, color, 0.7f);
			num3 += 108.5f;
			Text.Clear();
			Text.Append(isHeaderLine ? "-- / -- B" : "    -- B");
			DrawTextShadow(new Vector2(100f + num3, textPosY), Text, color, 0.7f);
			num3 += 150.6f;
			Text.Clear();
			num3 += 68f;
			Text.ConcatFormat(profilerBlock.CallFormat ?? "{0} calls", arg);
			DrawTextShadow(new Vector2(20f + num3, textPosY), Text, color, 0.7f);
			num3 += 105f;
			Text.Clear();
			Text.ConcatFormat(profilerBlock.ValueFormat ?? "Custom: {0:.00}", arg2);
			DrawTextShadow(new Vector2(20f + num3, textPosY), Text, color, 0.7f);
			num3 += 175f;
			int maxIndex;
			float arg4 = MyRenderProfiler.FindMaxWrap(profilerBlock.GetMillisecondsReader(useOptimizations), frameIndex - MyRenderProfiler.m_frameLocalArea / 2, frameIndex + MyRenderProfiler.m_frameLocalArea / 2, lastValidFrame, out maxIndex);
			Text.Clear();
			Text.ConcatFormat(profilerBlock.TimeFormat ?? "{0:.00}ms", arg4);
			DrawTextShadow(new Vector2(20f + num3, textPosY), Text, color, 0.7f);
			DrawTextShadow(new Vector2(20f + num3, textPosY), Text, color, 0.7f);
		}

		protected sealed override void Draw(MyProfiler drawProfiler, int lastFrameIndex, int frameToDraw)
		{
			if (!m_initialized)
			{
				Init();
				m_initialized = true;
				MyRenderProfiler.FpsBlock.Start(memoryProfiling: false);
			}
			MyRenderProfiler.FpsBlock.End(memoryProfiling: false);
			float num = (float)MyRenderProfiler.FpsBlock.Elapsed.Seconds;
			float num2 = (num > 0f) ? (1f / num) : 0f;
			MyRenderProfiler.m_fpsPctg = 0.9f * MyRenderProfiler.m_fpsPctg + 0.1f * num2;
			MyRenderProfiler.FpsBlock.CustomValues[lastFrameIndex] = MyRenderProfiler.FpsBlock.CustomValue;
			MyRenderProfiler.FpsBlock.Reset();
			MyRenderProfiler.FpsBlock.Start(memoryProfiling: false);
			if (MyRenderProfiler.m_enabled)
			{
				BeginDraw();
				if (MyRenderProfiler.m_graphContent != ProfilerGraphContent.Tasks)
				{
					float num3 = 20f;
					float num4 = 28f;
					float num5 = ViewportSize.Y / 2f - 11f * num4;
					Text.Clear();
					switch (MyRenderProfiler.m_dataType)
					{
					case SnapshotType.Online:
						Text.Append("Online");
						break;
					case SnapshotType.Server:
						Text.Append("Server");
						break;
					case SnapshotType.Snapshot:
						Text.Append("Snapshot");
						break;
					}
					Text.AppendLine();
					Text.ConcatFormat("\"{2}\" ({0}/{1})", MyRenderProfiler.ThreadProfilers.IndexOf(MyRenderProfiler.m_selectedProfiler) + 1, MyRenderProfiler.ThreadProfilers.Count, MyRenderProfiler.m_selectedProfiler.DisplayName ?? "Invalid").AppendLine();
					Text.Append("Level limit: ");
					if (MyRenderProfiler.m_dataType == SnapshotType.Online)
					{
						Text.AppendInt32(MyRenderProfiler.m_selectedProfiler.LevelLimit);
					}
					else
					{
						Text.Append("Unavailable");
					}
					DrawText(new Vector2(20f, num5), Text, Color.LightGray, 1f);
					num5 += num4 * 3f + 10f;
					Text.Clear();
					Text.Append("Profile type: ").Append(MyRenderProfiler.m_selectedProfiler.PendingShallowProfileState ? "Shallow" : "Deep");
					DrawText(new Vector2(20f, num5), Text, Color.LightGray, 1f);
					num5 += num4 * 2f + 10f;
					Text.Clear();
					Text.Append("Frame: ").AppendInt32(frameToDraw).AppendLine();
					Text.Append("Local area: ").AppendInt32(MyRenderProfiler.m_frameLocalArea);
					DrawText(new Vector2(20f, num5), Text, Color.Yellow, 1f);
					num5 += num4 * 2f + 10f;
					Text.Clear();
					Text.Append(MyRenderProfiler.FpsBlock.Name).Append(" ");
					if (!MyRenderProfiler.m_useCustomFrame)
					{
						Text.AppendDecimal(MyRenderProfiler.m_fpsPctg, 3);
					}
					Text.AppendLine();
					Text.Append("Total calls: ").AppendInt32(MyRenderProfiler.IsValidIndex(frameToDraw, lastFrameIndex) ? MyRenderProfiler.m_selectedProfiler.TotalCalls[frameToDraw] : (-1));
					DrawText(new Vector2(20f, num5), Text, Color.Red, 1f);
					num5 += num4;
					Text.Clear();
					Text.Append("MyCompilationSymbols.PerformanceProfiling NOT ENABLED!").AppendLine();
					if (!MyRenderProfiler.ProfilerProcessingEnabled)
					{
						Text.Append("Profiler processing disabled, F12 -> Profiler").AppendLine();
					}
					DrawText(new Vector2(0f, 0f), Text, Color.Yellow, 0.6f);
					num5 = ViewportSize.Y / 2f;
					List<MyProfilerBlock> selectedRootChildren = MyRenderProfiler.m_selectedProfiler.SelectedRootChildren;
					List<MyProfilerBlock> sortedChildren = MyRenderProfiler.GetSortedChildren(frameToDraw);
					Text.Clear();
					for (MyProfilerBlock myProfilerBlock = MyRenderProfiler.m_selectedProfiler.SelectedRoot; myProfilerBlock != null; myProfilerBlock = myProfilerBlock.Parent)
					{
						if (myProfilerBlock.Name.Length + 3 + Text.Length > 170)
						{
							Text.Insert(0, "... > ");
							break;
						}
						if (Text.Length > 0)
						{
							Text.Insert(0, " > ");
						}
						Text.Insert(0, myProfilerBlock.Name);
					}
					DrawTextShadow(new Vector2(20f, num5), Text, Color.White, 0.7f);
					num5 += num3;
					if (MyRenderProfiler.m_selectedProfiler.SelectedRoot != null)
					{
						Color color = Color.White;
						DrawEvent(num5, MyRenderProfiler.m_selectedProfiler.SelectedRoot, MyRenderProfiler.m_selectedProfiler.EnableOptimizations, -1, frameToDraw, lastFrameIndex, ref color, isHeaderLine: true);
						num5 += num3;
					}
					if (sortedChildren.Count > 0)
					{
						Text.Clear().Append("\\/ ");
						float x;
						switch (MyRenderProfiler.m_sortingOrder)
						{
						case RenderProfilerSortingOrder.Id:
							x = 20f;
							Text.Append("ASC");
							break;
						case RenderProfilerSortingOrder.MillisecondsLastFrame:
							x = 660f;
							Text.Append("DESC");
							break;
						case RenderProfilerSortingOrder.MillisecondsAverage:
							x = 1270f;
							Text.Append("DESC");
							break;
						case RenderProfilerSortingOrder.AllocatedLastFrame:
							x = 845f;
							Text.Append("DESC");
							break;
						default:
							throw new Exception("Unhandled enum value " + MyRenderProfiler.m_sortingOrder);
						}
						DrawTextShadow(new Vector2(x, num5), Text, Color.White, 0.7f);
						num5 += num3;
						for (int i = 0; i < sortedChildren.Count; i++)
						{
							MyProfilerBlock myProfilerBlock2 = sortedChildren[i];
							Color color2 = Color.DarkRed;
							if (!myProfilerBlock2.IsOptimized)
							{
								color2 = MyRenderProfiler.IndexToColor(selectedRootChildren.IndexOf(myProfilerBlock2));
							}
							DrawEvent(num5, myProfilerBlock2, MyRenderProfiler.m_selectedProfiler.EnableOptimizations, i, frameToDraw, lastFrameIndex, ref color2, isHeaderLine: false);
							num5 += num3;
						}
					}
					else
					{
						Text.Clear().Append("No more blocks at this point!");
						num5 += num3;
						DrawTextShadow(new Vector2(20f, num5), Text, Color.White, 0.7f);
						num5 += num3;
					}
				}
				BeginLineBatch();
				BeginPrimitiveBatch();
				DrawPerfEvents(lastFrameIndex);
				EndPrimitiveBatch();
				EndLineBatch();
			}
			if (!MyRenderProfiler.Paused)
			{
				MyRenderProfiler.m_selectedFrame = lastFrameIndex;
			}
		}

		private void DrawCustomFrameLine()
		{
			if (MyRenderProfiler.m_useCustomFrame && MyRenderProfiler.m_selectedFrame >= 0 && MyRenderProfiler.m_selectedFrame < MyProfiler.MAX_FRAMES)
			{
				Vector3 v = default(Vector3);
				v.X = -1f + MyRenderProfiler.MemoryGraphScale.XStart + MyRenderProfiler.MemoryGraphScale.XScale * (float)MyRenderProfiler.m_selectedFrame / 512f;
				v.Y = MyRenderProfiler.MemoryGraphScale.YStart;
				v.Z = 0f;
				Vector3 v2 = default(Vector3);
				v2.X = v.X;
				v2.Y = 1f;
				v2.Z = 0f;
				DrawOnScreenLine(v, v2, Color.Yellow);
			}
		}

		private void DrawGraphs(int lastFrameIndex)
		{
			int windowEnd = MyRenderProfiler.GetWindowEnd(lastFrameIndex);
			MyDrawArea currentGraphScale = MyRenderProfiler.GetCurrentGraphScale();
			List<MyProfilerBlock> selectedRootChildren = MyRenderProfiler.m_selectedProfiler.SelectedRootChildren;
			if (MyRenderProfiler.m_selectedProfiler.SelectedRoot != null && (!MyRenderProfiler.m_selectedProfiler.IgnoreRoot || selectedRootChildren.Count == 0))
			{
				DrawBlockLineSeparated(MyRenderProfiler.GetGraphData(MyRenderProfiler.m_selectedProfiler.SelectedRoot), lastFrameIndex, windowEnd, currentGraphScale, Color.White);
			}
			for (int i = 0; i < selectedRootChildren.Count; i++)
			{
				DrawBlockLineSeparated(MyRenderProfiler.GetGraphData(selectedRootChildren[i]), lastFrameIndex, windowEnd, currentGraphScale, MyRenderProfiler.IndexToColor(i));
			}
		}

		private void DrawLegend()
		{
			Color color = new Color(200, 200, 200);
			Color color2 = new Color(130, 130, 130);
			MyDrawArea currentGraphScale = MyRenderProfiler.GetCurrentGraphScale();
			float xStart = currentGraphScale.XStart;
			float num = 0.01f;
			DrawOnScreenLine(new Vector3(-1f + xStart, 0f, 0f), new Vector3(-1f + xStart, currentGraphScale.YScale, 0f), color);
			float x = ViewportSize.X;
			float y = ViewportSize.Y;
			int num2 = 0;
			float num3 = currentGraphScale.YLegendMsIncrement;
			while (num3 != (float)(int)num3 && num2 < 5)
			{
				num3 *= 10f;
				num2++;
			}
			m_numberInfo.NumberDecimalDigits = num2;
			for (int i = 0; i <= currentGraphScale.YLegendMsCount; i++)
			{
				Text.Clear();
				Text.ConcatFormat("{0}", (float)i * currentGraphScale.YLegendMsIncrement, m_numberInfo);
				Vector2 vector = MeasureText(Text, 0.7f);
				DrawText(new Vector2(0.5f * x * xStart - vector.X - 6f + 3f * num, -10f + 0.5f * y - currentGraphScale.YLegendIncrement * (float)i * 0.5f * y), Text, Color.Silver, 0.7f);
				Vector3 v = new Vector3(-1f + xStart, (float)i * currentGraphScale.YLegendIncrement, 0f);
				DrawOnScreenLine(v1: new Vector3(v.X + currentGraphScale.XScale * 2f, (float)i * currentGraphScale.YLegendIncrement, 0f), v0: v, color: color2);
			}
			Text.Clear().Append((MyRenderProfiler.m_graphContent == ProfilerGraphContent.Elapsed) ? MyRenderProfiler.m_selectedProfiler.AxisName : "[B/Tick]");
			DrawText(new Vector2(0.5f * x * xStart - 25f + 3f * num, -10f + 0.5f * y - currentGraphScale.YScale * 0.5f * y * 1.05f), Text, Color.Silver, 0.7f);
		}

		private void DrawTaskProfilerHeader()
		{
			float num = 100f;
			Color white = Color.White;
			float y = 0.53f * ViewportSize.Y;
			Text.Clear();
			Text.Append("Thread name");
			DrawTextShadow(num, y, Text, white, 0.7f);
			num += 245f;
			num -= 14f;
			Text.Clear();
			Text.Append("Task name");
			DrawTextShadow(num, y, Text, white, 0.7f);
			num += 560f;
			Text.Clear();
			Text.Append("Duration");
			DrawTextShadow(num, y, Text, white, 0.7f);
			num += 70f;
			Text.Clear();
			Text.Append("Run delay");
			DrawTextShadow(num, y, Text, white, 0.7f);
			num += 70f;
			Text.Clear();
			Text.Append("Custom");
			DrawTextShadow(num, y, Text, white, 0.7f);
			num += 70f;
		}

		private void DrawProfilerInfo(MyProfiler profiler, MyProfiler.TaskInfo? taskArg, float y, Color color)
		{
			float num = 100f;
			Text.Clear();
			Text.Append(profiler.DisplayName);
			DrawTextShadow(num, y, Text, color, 0.7f);
			num += 245f;
			if (taskArg.HasValue)
			{
				MyProfiler.TaskInfo value = taskArg.Value;
				Text.Clear();
				Text.Append(value.Name);
				DrawTextShadow(num, y, Text, color, 0.7f);
				num += 560f;
				Text.Clear();
				Text.AppendDecimal(MyTimeSpan.FromTicks(value.Finished - value.Started).Milliseconds, 2);
				DrawTextShadow(num, y, Text, color, 0.7f);
				num += 70f;
				Text.Clear();
				if (value.Scheduled > 0)
				{
					long ticks = value.Started - value.Scheduled;
					Text.AppendDecimal(MyTimeSpan.FromTicks(ticks).Milliseconds, 2);
				}
				else
				{
					Text.Append("-----");
				}
				DrawTextShadow(num, y, Text, color, 0.7f);
				num += 70f;
				Text.Clear();
				Text.Append(value.CustomValue);
				DrawTextShadow(num, y, Text, color, 0.7f);
				num += 70f;
			}
			else
			{
				Text.Clear();
				Text.Append("No tasks");
				DrawTextShadow(num, y, Text, color, 0.7f);
				num += 770f;
			}
		}

		private void DrawTasks(MyDrawArea drawArea)
		{
			DrawTaskProfilerHeader();
			long num;
			long num2;
			if (MyRenderProfiler.Paused)
			{
				num = MyRenderProfiler.m_targetTaskRenderTime + MyRenderProfiler.m_taskRenderDispersion;
				num2 = MyRenderProfiler.m_targetTaskRenderTime - MyRenderProfiler.m_taskRenderDispersion;
			}
			else
			{
				num = MyProfiler.LastFrameTime + MyTimeSpan.FromMilliseconds(1.0).Ticks;
				num2 = num - MyRenderProfiler.m_taskRenderDispersion * 2;
			}
			double num3 = -1f + drawArea.XStart;
			double value = num3 + (double)(drawArea.XScale * 2f);
			double num4 = drawArea.YStart;
			double num5 = num4 + (double)drawArea.YScale;
			double num6 = num - num2;
			foreach (FrameInfo frameTimestamp in MyRenderProfiler.FrameTimestamps)
			{
				long time = frameTimestamp.Time;
				if (time > num2 && time < num)
				{
					double amount = (double)(time - num2) / num6;
					double num7 = MathHelper.Lerp(num3, value, amount);
					Vector3 v = new Vector3(num7, num4, 0.0);
					Vector3 v2 = new Vector3(num7, num5, 0.0);
					Color yellow = Color.Yellow;
					yellow.A = 20;
					DrawOnScreenLine(v, v2, yellow);
					Text.Clear();
					Text.Append(frameTimestamp.FrameNumber % 100);
					DrawTextShadow((float)(1.0 + num7) * ViewportSize.X / 2f, (float)(num4 + 1.0) * ViewportSize.Y / 2f, Text, Color.Red, 0.9f);
				}
			}
			lock (MyRenderProfiler.ThreadProfilers)
			{
				float num8 = 0.05f;
				foreach (MyProfiler threadProfiler in MyRenderProfiler.ThreadProfilers)
				{
					string displayName = threadProfiler.DisplayName;
					if (!string.IsNullOrEmpty(displayName))
					{
						lock (threadProfiler.TaskLock)
						{
							if (threadProfiler.FinishedTasks.Count != 0)
							{
								double num9 = 0.0 - MathHelper.Lerp(num4, num5, num8 + 0.05f);
								num9 += 1.0;
								num9 /= 2.0;
								Text.Clear();
								Text.Append(displayName);
								DrawTextShadow(0.1f * ViewportSize.X / 2f, (float)num9 * ViewportSize.Y, Text, Color.White, 0.5f);
								float num10 = (float)MathHelper.Lerp(num4, num5, num8) + 0.002f;
								DrawOnScreenLine(-1f, num10, 1f, num10, Color.Black.Alpha(0.5f));
								int taskDepth = 0;
								Color color = Color.White;
								MyProfiler.TaskInfo? taskArg = null;
								float y = (num8 + 0.1f + 1f) / 2f * ViewportSize.Y;
								MyQueue<MyProfiler.TaskInfo> finishedTasks = threadProfiler.FinishedTasks;
								m_taskStack.Clear();
								for (int i = 0; i < finishedTasks.Count; i++)
								{
									MyProfiler.TaskInfo task = finishedTasks[i];
									if (task.Finished >= num2 && num >= task.Started)
									{
										Color color2;
										switch (task.TaskType)
										{
										case MyProfiler.TaskType.Wait:
											color2 = Color.Transparent;
											break;
										case MyProfiler.TaskType.SyncWait:
										case MyProfiler.TaskType.HK_AwaitTasks:
											color2 = Color.Red;
											break;
										case MyProfiler.TaskType.Physics:
											color2 = Color.LightGreen;
											break;
										case MyProfiler.TaskType.Block:
											color2 = Color.AliceBlue;
											break;
										case MyProfiler.TaskType.Voxels:
											color2 = Color.Orange;
											break;
										case MyProfiler.TaskType.RenderCull:
										case MyProfiler.TaskType.PreparePass:
										case MyProfiler.TaskType.RenderPass:
										case MyProfiler.TaskType.ClipMap:
											color2 = Color.Blue;
											break;
										case MyProfiler.TaskType.Deformations:
											color2 = Color.DarkGreen;
											break;
										default:
											color2 = Color.White.Alpha(0.1f);
											break;
										}
										if (color2 != Color.Transparent)
										{
											while (m_taskStack.Count != 0 && m_taskStack.Peek().Finished < task.Finished)
											{
												m_taskStack.Pop();
											}
											m_taskStack.Push(task);
											if (!taskArg.HasValue || (task.Started < num2 && task.Finished < taskArg.Value.Finished))
											{
												if (taskArg.HasValue)
												{
													MyProfiler.TaskInfo task2 = taskArg.Value;
													DrawTask(ref task2, num2, num, num8, drawArea, color, taskDepth);
												}
												color = color2;
												taskArg = task;
												taskDepth = m_taskStack.Count - 1;
											}
											else
											{
												DrawTask(ref task, num2, num, num8, drawArea, color2, m_taskStack.Count - 1);
											}
										}
									}
								}
								if (taskArg.HasValue)
								{
									MyProfiler.TaskInfo task3 = taskArg.Value;
									DrawTask(ref task3, num2, num, num8, drawArea, color.Shade(0.5f), taskDepth, isSelected: true);
								}
								DrawProfilerInfo(threadProfiler, taskArg, y, color.Alpha(1f));
								goto IL_0524;
							}
						}
					}
					continue;
					IL_0524:
					num8 += 0.06f;
				}
			}
		}

		private void DrawPerfEvents(int lastFrameIndex)
		{
			if (MyRenderProfiler.m_graphContent == ProfilerGraphContent.Tasks)
			{
				DrawTasks(MyRenderProfiler.GetCurrentGraphScale());
				return;
			}
			MyRenderProfiler.UpdateAutoScale(lastFrameIndex);
			DrawLegend();
			DrawGraphs(lastFrameIndex);
			DrawCustomFrameLine();
		}

		private static float GetAppropriateMemoryUnits(float value, out string units)
		{
			if (value < 10240f)
			{
				units = "B";
				return 1f;
			}
			if (value < 1.048576E+07f)
			{
				units = "KB";
				return 0.0009765625f;
			}
			units = "MB";
			return 9.536743E-07f;
		}
	}
}
