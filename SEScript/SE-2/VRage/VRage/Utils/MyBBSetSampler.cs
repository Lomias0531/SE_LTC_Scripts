using System.Collections.Generic;
using VRageMath;

namespace VRage.Utils
{
	/// <summary>
	/// This class allows for uniform generation of points from a set of bounding boxes.
	///
	/// You start by constructing a bounding box from where the points will be sampled.
	/// Then you can incrementally subtract bounding boxes and the resulting structure will allow you
	/// to generate uniformly distributed points using the Sample() function.
	/// </summary>
	public class MyBBSetSampler
	{
		private class IntervalSampler
		{
			private struct SamplingEntry
			{
				public double UpperLimit;

				public double CumulativeWeight;

				public bool Full;

				public IntervalSampler Sampler;

				public SamplingEntry(double limit, IntervalSampler sampler, double weight)
				{
					UpperLimit = limit;
					Sampler = sampler;
					CumulativeWeight = weight;
					Full = false;
				}

				public SamplingEntry(SamplingEntry other)
				{
					UpperLimit = other.UpperLimit;
					CumulativeWeight = other.CumulativeWeight;
					Full = other.Full;
					if (other.Sampler == null)
					{
						Sampler = null;
					}
					else
					{
						Sampler = new IntervalSampler(other.Sampler, 1.0, clone: true);
					}
				}

				public static SamplingEntry Divide(ref SamplingEntry oldEntry, double prevUpperLimit, double prevCumulativeWeight, double weightMult, double newUpperLimit)
				{
					SamplingEntry result = default(SamplingEntry);
					result.UpperLimit = newUpperLimit;
					double num = newUpperLimit - prevUpperLimit;
					double num2 = oldEntry.UpperLimit - newUpperLimit;
					double t = num / (num + num2);
					result.Full = oldEntry.Full;
					if (oldEntry.Sampler != null)
					{
						result.Sampler = new IntervalSampler(oldEntry.Sampler, t, clone: false);
						result.CumulativeWeight = prevCumulativeWeight + result.Sampler.TotalWeight;
						oldEntry.CumulativeWeight = result.CumulativeWeight + oldEntry.Sampler.TotalWeight;
					}
					else
					{
						result.Sampler = null;
						if (oldEntry.Full)
						{
							result.CumulativeWeight = (oldEntry.CumulativeWeight = prevCumulativeWeight);
						}
						else
						{
							result.CumulativeWeight = prevCumulativeWeight + weightMult * num;
							oldEntry.CumulativeWeight = result.CumulativeWeight + weightMult * num2;
						}
					}
					return result;
				}
			}

			private Base6Directions.Axis m_axis;

			private double m_min;

			private double m_max;

			private double m_weightMult;

			private List<SamplingEntry> m_entries;

			private double m_totalWeight;

			public double TotalWeight => m_totalWeight;

			public IntervalSampler(double min, double max, double weightMultiplier, Base6Directions.Axis axis)
			{
				m_min = min;
				m_max = max;
				m_axis = axis;
				m_weightMult = weightMultiplier;
				m_totalWeight = weightMultiplier * (m_max - m_min);
				m_entries = new List<SamplingEntry>();
				m_entries.Add(new SamplingEntry(m_max, null, m_totalWeight));
			}

			private IntervalSampler(IntervalSampler other, double t, bool clone)
			{
				m_min = other.m_min;
				m_max = other.m_max;
				m_axis = other.m_axis;
				m_weightMult = other.m_weightMult;
				m_totalWeight = other.m_totalWeight;
				m_entries = new List<SamplingEntry>(other.m_entries);
				for (int i = 0; i < other.m_entries.Count; i++)
				{
					m_entries[i] = new SamplingEntry(other.m_entries[i]);
				}
				Multiply(t);
				if (!clone)
				{
					other.Multiply(1.0 - t);
				}
			}

			private void Multiply(double t)
			{
				m_weightMult *= t;
				m_totalWeight *= t;
				for (int i = 0; i < m_entries.Count; i++)
				{
					SamplingEntry value = m_entries[i];
					value.CumulativeWeight *= t;
					m_entries[i] = value;
					if (value.Sampler != null)
					{
						value.Sampler.Multiply(t);
					}
				}
			}

			public void Subtract(ref BoundingBoxD originalBox, ref BoundingBoxD bb)
			{
				SelectMinMax(ref bb, m_axis, out double min, out double max);
				bool flag = false;
				double num = m_min;
				double num2 = 0.0;
				for (int i = 0; i < m_entries.Count; i++)
				{
					SamplingEntry oldEntry = m_entries[i];
					if (!flag)
					{
						if (oldEntry.UpperLimit >= min)
						{
							if (oldEntry.UpperLimit == min)
							{
								flag = true;
							}
							else
							{
								if (num == min)
								{
									flag = true;
									i--;
									continue;
								}
								flag = true;
								SamplingEntry samplingEntry = SamplingEntry.Divide(ref oldEntry, num, num2, m_weightMult, min);
								m_entries[i] = oldEntry;
								m_entries.Insert(i, samplingEntry);
								oldEntry = samplingEntry;
							}
						}
					}
					else if (num < max)
					{
						if (oldEntry.UpperLimit > max)
						{
							SamplingEntry samplingEntry2 = SamplingEntry.Divide(ref oldEntry, num, num2, m_weightMult, max);
							m_entries[i] = oldEntry;
							m_entries.Insert(i, samplingEntry2);
							oldEntry = samplingEntry2;
						}
						if (oldEntry.UpperLimit <= max)
						{
							if (oldEntry.Sampler == null)
							{
								if (m_axis == Base6Directions.Axis.ForwardBackward)
								{
									oldEntry.Full = true;
									oldEntry.CumulativeWeight = num2;
								}
								else if (!oldEntry.Full)
								{
									Base6Directions.Axis axis = (m_axis == Base6Directions.Axis.LeftRight) ? Base6Directions.Axis.UpDown : Base6Directions.Axis.ForwardBackward;
									SelectMinMax(ref originalBox, axis, out double min2, out double max2);
									double num3 = m_max - m_min;
									double num4 = m_weightMult * num3;
									double num5 = (oldEntry.UpperLimit - num) / num3;
									double num6 = max2 - min2;
									oldEntry.Sampler = new IntervalSampler(min2, max2, num4 * num5 / num6, axis);
								}
							}
							if (oldEntry.Sampler != null)
							{
								oldEntry.Sampler.Subtract(ref originalBox, ref bb);
								oldEntry.CumulativeWeight = num2 + oldEntry.Sampler.TotalWeight;
							}
							m_entries[i] = oldEntry;
						}
					}
					else
					{
						if (oldEntry.Sampler == null)
						{
							if (oldEntry.Full)
							{
								oldEntry.CumulativeWeight = num2;
							}
							else
							{
								oldEntry.CumulativeWeight = num2 + (oldEntry.UpperLimit - num) * m_weightMult;
							}
						}
						else
						{
							oldEntry.CumulativeWeight = num2 + oldEntry.Sampler.TotalWeight;
						}
						m_entries[i] = oldEntry;
					}
					num = oldEntry.UpperLimit;
					num2 = oldEntry.CumulativeWeight;
				}
				m_totalWeight = num2;
			}

			private void SelectMinMax(ref BoundingBoxD bb, Base6Directions.Axis axis, out double min, out double max)
			{
				switch (axis)
				{
				case Base6Directions.Axis.UpDown:
					min = bb.Min.Y;
					max = bb.Max.Y;
					break;
				case Base6Directions.Axis.ForwardBackward:
					min = bb.Min.Z;
					max = bb.Max.Z;
					break;
				default:
					min = bb.Min.X;
					max = bb.Max.X;
					break;
				}
			}

			public double Sample(out IntervalSampler childSampler)
			{
				double randomDouble = MyUtils.GetRandomDouble(0.0, TotalWeight);
				double num = m_min;
				double num2 = 0.0;
				for (int i = 0; i < m_entries.Count; i++)
				{
					if (m_entries[i].CumulativeWeight >= randomDouble)
					{
						childSampler = m_entries[i].Sampler;
						double num3 = m_entries[i].CumulativeWeight - num2;
						double num4 = (randomDouble - num2) / num3;
						return num4 * m_entries[i].UpperLimit + (1.0 - num4) * num;
					}
					num = m_entries[i].UpperLimit;
					num2 = m_entries[i].CumulativeWeight;
				}
				childSampler = null;
				return m_max;
			}
		}

		private IntervalSampler m_sampler;

		private BoundingBoxD m_bBox;

		public bool Valid
		{
			get
			{
				if (m_sampler == null)
				{
					return m_bBox.Volume > 0.0;
				}
				return m_sampler.TotalWeight > 0.0;
			}
		}

		public MyBBSetSampler(Vector3D min, Vector3D max)
		{
			Vector3D max2 = Vector3D.Max(min, max);
			Vector3D min2 = Vector3D.Min(min, max);
			m_bBox = new BoundingBoxD(min2, max2);
			m_sampler = new IntervalSampler(min2.X, max2.X, (max2.Y - min2.Y) * (max2.Z - min2.Z), Base6Directions.Axis.LeftRight);
		}

		public void SubtractBB(ref BoundingBoxD bb)
		{
			if (m_bBox.Intersects(ref bb))
			{
				BoundingBoxD bb2 = m_bBox.Intersect(bb);
				m_sampler.Subtract(ref m_bBox, ref bb2);
			}
		}

		public Vector3D Sample()
		{
			IntervalSampler childSampler = m_sampler;
			Vector3D result = default(Vector3D);
			result.X = childSampler.Sample(out childSampler);
			if (childSampler != null)
			{
				result.Y = childSampler.Sample(out childSampler);
			}
			else
			{
				result.Y = MyUtils.GetRandomDouble(m_bBox.Min.Y, m_bBox.Max.Y);
			}
			if (childSampler != null)
			{
				result.Z = childSampler.Sample(out childSampler);
			}
			else
			{
				result.Z = MyUtils.GetRandomDouble(m_bBox.Min.Z, m_bBox.Max.Z);
			}
			return result;
		}
	}
}
