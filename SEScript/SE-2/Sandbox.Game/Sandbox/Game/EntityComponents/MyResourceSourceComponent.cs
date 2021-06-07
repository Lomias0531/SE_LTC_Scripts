using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
	public class MyResourceSourceComponent : MyResourceSourceComponentBase
	{
		private struct PerTypeData
		{
			public float CurrentOutput;

			public float MaxOutput;

			public float DefinedOutput;

			public float RemainingCapacity;

			public float ProductionToCapacityMultiplier;

			public bool HasRemainingCapacity;

			public bool IsProducerEnabled;
		}

		private class Sandbox_Game_EntityComponents_MyResourceSourceComponent_003C_003EActor
		{
		}

		private int m_allocatedTypeCount;

		private PerTypeData[] m_dataPerType;

		private bool m_enabled;

		private readonly StringBuilder m_textCache = new StringBuilder();

		[ThreadStatic]
		private static List<MyResourceSourceInfo> m_singleHelperList;

		private readonly Dictionary<MyDefinitionId, int> m_resourceTypeToIndex = new Dictionary<MyDefinitionId, int>(1, MyDefinitionId.Comparer);

		private readonly List<MyDefinitionId> m_resourceIds = new List<MyDefinitionId>(1);

		public MyEntity TemporaryConnectedEntity
		{
			get;
			set;
		}

		public MyStringHash Group
		{
			get;
			private set;
		}

		public float CurrentOutput => CurrentOutputByType(m_resourceTypeToIndex.FirstPair().Key);

		public float MaxOutput => MaxOutputByType(m_resourceTypeToIndex.FirstPair().Key);

		public float DefinedOutput => DefinedOutputByType(m_resourceTypeToIndex.FirstPair().Key);

		public bool ProductionEnabled => ProductionEnabledByType(m_resourceTypeToIndex.FirstPair().Key);

		public float RemainingCapacity => RemainingCapacityByType(m_resourceTypeToIndex.FirstPair().Key);

		public bool IsInfiniteCapacity => float.IsInfinity(RemainingCapacity);

		public float ProductionToCapacityMultiplier => ProductionToCapacityMultiplierByType(m_resourceTypeToIndex.FirstPair().Key);

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				SetEnabled(value);
			}
		}

		public bool HasCapacityRemaining => HasCapacityRemainingByType(m_resourceTypeToIndex.FirstPair().Key);

		public ListReader<MyDefinitionId> ResourceTypes => new ListReader<MyDefinitionId>(m_resourceIds);

		public override string ComponentTypeDebugString => "Resource Source";

		public event MyResourceCapacityRemainingChangedDelegate HasCapacityRemainingChanged;

		public event MyResourceCapacityRemainingChangedDelegate ProductionEnabledChanged;

		public event MyResourceOutputChangedDelegate OutputChanged;

		public event MyResourceOutputChangedDelegate MaxOutputChanged;

		public MyResourceSourceComponent(int initialAllocationSize = 1)
		{
			AllocateData(initialAllocationSize);
		}

		public void Init(MyStringHash sourceGroup, MyResourceSourceInfo sourceResourceData)
		{
			MyUtils.Init(ref m_singleHelperList);
			m_singleHelperList.Add(sourceResourceData);
			Init(sourceGroup, m_singleHelperList);
			m_singleHelperList.Clear();
		}

		public void Init(MyStringHash sourceGroup, List<MyResourceSourceInfo> sourceResourceData)
		{
			Group = sourceGroup;
			bool num = sourceResourceData != null && sourceResourceData.Count != 0;
			int num2 = (!num) ? 1 : sourceResourceData.Count;
			Enabled = true;
			if (num2 != m_allocatedTypeCount)
			{
				AllocateData(num2);
			}
			int num3 = 0;
			if (!num)
			{
				m_resourceTypeToIndex.Add(MyResourceDistributorComponent.ElectricityId, num3++);
				m_resourceIds.Add(MyResourceDistributorComponent.ElectricityId);
			}
			else
			{
				foreach (MyResourceSourceInfo sourceResourceDatum in sourceResourceData)
				{
					m_resourceTypeToIndex.Add(sourceResourceDatum.ResourceTypeId, num3++);
					m_resourceIds.Add(sourceResourceDatum.ResourceTypeId);
					m_dataPerType[num3 - 1].DefinedOutput = sourceResourceDatum.DefinedOutput;
					SetOutputByType(sourceResourceDatum.ResourceTypeId, 0f);
					SetMaxOutputByType(sourceResourceDatum.ResourceTypeId, m_dataPerType[GetTypeIndex(sourceResourceDatum.ResourceTypeId)].DefinedOutput);
					SetProductionEnabledByType(sourceResourceDatum.ResourceTypeId, newProducerEnabled: true);
					m_dataPerType[num3 - 1].ProductionToCapacityMultiplier = ((sourceResourceDatum.ProductionToCapacityMultiplier != 0f) ? sourceResourceDatum.ProductionToCapacityMultiplier : 1f);
					if (sourceResourceDatum.IsInfiniteCapacity)
					{
						SetRemainingCapacityByType(sourceResourceDatum.ResourceTypeId, float.PositiveInfinity);
					}
				}
			}
		}

		private void AllocateData(int allocationSize)
		{
			m_dataPerType = new PerTypeData[allocationSize];
			m_allocatedTypeCount = allocationSize;
		}

		public override float CurrentOutputByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].CurrentOutput;
		}

		public void SetOutput(float newOutput)
		{
			SetOutputByType(m_resourceTypeToIndex.FirstPair().Key, newOutput);
		}

		public void SetOutputByType(MyDefinitionId resourceTypeId, float newOutput)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			float currentOutput = m_dataPerType[typeIndex].CurrentOutput;
			m_dataPerType[typeIndex].CurrentOutput = newOutput;
			if (currentOutput != newOutput && this.OutputChanged != null)
			{
				this.OutputChanged(resourceTypeId, currentOutput, this);
			}
		}

		public float RemainingCapacityByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].RemainingCapacity;
		}

		public void SetRemainingCapacityByType(MyDefinitionId resourceTypeId, float newRemainingCapacity)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			float remainingCapacity = m_dataPerType[typeIndex].RemainingCapacity;
			float num = MaxOutputLimitedByCapacity(typeIndex);
			m_dataPerType[typeIndex].RemainingCapacity = newRemainingCapacity;
			if (remainingCapacity != newRemainingCapacity)
			{
				SetHasCapacityRemainingByType(resourceTypeId, newRemainingCapacity > 0f);
			}
			if (this.MaxOutputChanged != null && MaxOutputLimitedByCapacity(typeIndex) != num)
			{
				this.MaxOutputChanged(resourceTypeId, num, this);
			}
		}

		public override float MaxOutputByType(MyDefinitionId resourceTypeId)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			return MaxOutputLimitedByCapacity(typeIndex);
		}

		private float MaxOutputLimitedByCapacity(int typeIndex)
		{
			return Math.Min(m_dataPerType[typeIndex].MaxOutput, m_dataPerType[typeIndex].RemainingCapacity * m_dataPerType[typeIndex].ProductionToCapacityMultiplier * 60f);
		}

		public void SetMaxOutput(float newMaxOutput)
		{
			SetMaxOutputByType(m_resourceTypeToIndex.FirstPair().Key, newMaxOutput);
		}

		public void SetMaxOutputByType(MyDefinitionId resourceTypeId, float newMaxOutput)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			if (m_dataPerType[typeIndex].MaxOutput != newMaxOutput)
			{
				float maxOutput = m_dataPerType[typeIndex].MaxOutput;
				m_dataPerType[typeIndex].MaxOutput = newMaxOutput;
				if (this.MaxOutputChanged != null)
				{
					this.MaxOutputChanged(resourceTypeId, maxOutput, this);
				}
			}
		}

		public override float DefinedOutputByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].DefinedOutput;
		}

		public float ProductionToCapacityMultiplierByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].ProductionToCapacityMultiplier;
		}

		public bool HasCapacityRemainingByType(MyDefinitionId resourceTypeId)
		{
			if (!IsInfiniteCapacity && !MySession.Static.CreativeMode)
			{
				return m_dataPerType[GetTypeIndex(resourceTypeId)].HasRemainingCapacity;
			}
			return true;
		}

		private void SetHasCapacityRemainingByType(MyDefinitionId resourceTypeId, bool newHasCapacity)
		{
			if (IsInfiniteCapacity)
			{
				return;
			}
			int typeIndex = GetTypeIndex(resourceTypeId);
			if (m_dataPerType[typeIndex].HasRemainingCapacity != newHasCapacity)
			{
				m_dataPerType[typeIndex].HasRemainingCapacity = newHasCapacity;
				if (this.HasCapacityRemainingChanged != null)
				{
					this.HasCapacityRemainingChanged(resourceTypeId, this);
				}
				if (!newHasCapacity)
				{
					m_dataPerType[typeIndex].CurrentOutput = 0f;
				}
			}
		}

		public override bool ProductionEnabledByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].IsProducerEnabled;
		}

		public void SetProductionEnabledByType(MyDefinitionId resourceTypeId, bool newProducerEnabled)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			bool num = m_dataPerType[typeIndex].IsProducerEnabled != newProducerEnabled;
			m_dataPerType[typeIndex].IsProducerEnabled = newProducerEnabled;
			if (num && this.ProductionEnabledChanged != null)
			{
				this.ProductionEnabledChanged(resourceTypeId, this);
			}
			if (!newProducerEnabled)
			{
				SetOutputByType(resourceTypeId, 0f);
			}
		}

		internal void SetEnabled(bool newValue, bool fireEvents = true)
		{
			if (m_enabled != newValue)
			{
				m_enabled = newValue;
				if (fireEvents)
				{
					OnProductionEnabledChanged();
				}
				if (!m_enabled)
				{
					foreach (MyDefinitionId resourceId in m_resourceIds)
					{
						SetOutputByType(resourceId, 0f);
					}
				}
			}
		}

		public void OnProductionEnabledChanged(MyDefinitionId? resId = null)
		{
			if (resId.HasValue)
			{
				if (this.ProductionEnabledChanged != null)
				{
					this.ProductionEnabledChanged(resId.Value, this);
				}
			}
			else
			{
				foreach (MyDefinitionId resourceId in m_resourceIds)
				{
					if (this.ProductionEnabledChanged != null)
					{
						this.ProductionEnabledChanged(resourceId, this);
					}
				}
			}
		}

		protected int GetTypeIndex(MyDefinitionId resourceTypeId)
		{
			int result = 0;
			if (m_resourceTypeToIndex.Count > 1)
			{
				result = m_resourceTypeToIndex[resourceTypeId];
			}
			return result;
		}

		public override string ToString()
		{
			m_textCache.Clear();
			m_textCache.AppendFormat("Enabled: {0}", Enabled).Append("; \n");
			m_textCache.Append("Output: ");
			MyValueFormatter.AppendWorkInBestUnit(CurrentOutput, m_textCache);
			m_textCache.Append("; \n");
			m_textCache.Append("Max Output: ");
			MyValueFormatter.AppendWorkInBestUnit(MaxOutput, m_textCache);
			m_textCache.Append("; \n");
			m_textCache.AppendFormat("ProductionEnabled: {0}", ProductionEnabled);
			return m_textCache.ToString();
		}

		public void DebugDraw(Matrix worldMatrix)
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				return;
			}
			double scaleFactor = 2.5 * 0.045;
			Vector3D vector3D = worldMatrix.Translation + worldMatrix.Up;
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(vector3D, position);
			double num = Math.Atan(2.5 / Math.Max(val, 0.001));
			if (!(num <= 0.27000001072883606))
			{
				if (base.Entity != null)
				{
					MyRenderProxy.DebugDrawText3D(vector3D, base.Entity.ToString(), Color.Yellow, (float)num, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				}
				if (m_resourceIds != null && m_resourceIds.Count != 0)
				{
					Vector3D vector3D2 = vector3D;
					int num2 = -1;
					foreach (MyDefinitionId resourceId in m_resourceIds)
					{
						vector3D2 = vector3D + num2 * up * scaleFactor;
						DebugDrawResource(resourceId, vector3D2, right, (float)num);
						num2--;
					}
				}
			}
		}

		private void DebugDrawResource(MyDefinitionId resourceId, Vector3D origin, Vector3D rightVector, float textSize)
		{
			Vector3D value = 0.05000000074505806 * rightVector;
			Vector3D worldCoord = origin + value + rightVector * 0.014999999664723873;
			int value2 = 0;
			string text = resourceId.SubtypeName;
			if (m_resourceTypeToIndex.TryGetValue(resourceId, out value2))
			{
				PerTypeData perTypeData = m_dataPerType[value2];
				text = $"{resourceId.SubtypeName} Max:{perTypeData.MaxOutput} Current:{perTypeData.CurrentOutput} Remaining:{perTypeData.RemainingCapacity}";
			}
			MyRenderProxy.DebugDrawLine3D(origin, origin + value, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
		}
	}
}
