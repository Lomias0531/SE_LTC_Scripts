using System;
using System.Collections.Generic;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame
{
	public struct MyItemType : IComparable<MyItemType>, IEquatable<MyItemType>
	{
		private readonly MyStringHash m_subtypeId;

		private readonly MyObjectBuilderType m_typeId;

		public string TypeId
		{
			get
			{
				MyObjectBuilderType typeId = m_typeId;
				return typeId.ToString();
			}
		}

		public string SubtypeId
		{
			get
			{
				MyStringHash subtypeId = m_subtypeId;
				return subtypeId.ToString();
			}
		}

		public MyItemType(string typeId, string subtypeId)
			: this(MyObjectBuilderType.Parse(typeId), MyStringHash.GetOrCompute(subtypeId))
		{
		}

		public MyItemType(MyObjectBuilderType typeId, MyStringHash subTypeIdHash)
		{
			m_typeId = typeId;
			m_subtypeId = subTypeIdHash;
		}

		public static bool operator ==(MyItemType a, MyItemType b)
		{
			if (a.m_subtypeId == b.m_subtypeId)
			{
				return a.m_typeId == b.m_typeId;
			}
			return false;
		}

		public static bool operator !=(MyItemType a, MyItemType b)
		{
			return !(a == b);
		}

		public static implicit operator MyItemType(MyObjectBuilder_PhysicalObject ob)
		{
			return ob.GetObjectId();
		}

		public static implicit operator MyItemType(SerializableDefinitionId definitionId)
		{
			return (MyDefinitionId)definitionId;
		}

		public static implicit operator MyItemType(MyDefinitionId definitionId)
		{
			return new MyItemType(definitionId.TypeId, definitionId.SubtypeId);
		}

		public static implicit operator MyDefinitionId(MyItemType itemType)
		{
			return new MyDefinitionId(itemType.m_typeId, itemType.m_subtypeId);
		}

		public bool Equals(MyItemType other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MyItemType))
			{
				return false;
			}
			return Equals((MyItemType)obj);
		}

		public override int GetHashCode()
		{
			MyObjectBuilderType typeId = m_typeId;
			int hashCode = typeId.GetHashCode();
			MyStringHash subtypeId = m_subtypeId;
			return MyTuple.CombineHashCodes(hashCode, subtypeId.GetHashCode());
		}

		public int CompareTo(MyItemType other)
		{
			int num = Comparer<Type>.Default.Compare(m_typeId, other.m_typeId);
			if (num == 0)
			{
				num = ((int)(int)m_subtypeId).CompareTo((int)other.m_subtypeId);
			}
			return num;
		}

		public override string ToString()
		{
			return ((MyDefinitionId)this).ToString();
		}

		public static MyItemType Parse(string itemType)
		{
			return MyDefinitionId.Parse(itemType);
		}

		public static MyItemType MakeOre(string subTypeId)
		{
			return new MyItemType(typeof(MyObjectBuilder_Ore), MyStringHash.GetOrCompute(subTypeId));
		}

		public static MyItemType MakeIngot(string subTypeId)
		{
			return new MyItemType(typeof(MyObjectBuilder_Ingot), MyStringHash.GetOrCompute(subTypeId));
		}

		public static MyItemType MakeComponent(string subTypeId)
		{
			return new MyItemType(typeof(MyObjectBuilder_Component), MyStringHash.GetOrCompute(subTypeId));
		}

		public static MyItemType MakeAmmo(string subTypeId)
		{
			return new MyItemType(typeof(MyObjectBuilder_AmmoMagazine), MyStringHash.GetOrCompute(subTypeId));
		}

		public static MyItemType MakeTool(string subTypeId)
		{
			return new MyItemType(typeof(MyObjectBuilder_PhysicalGunObject), MyStringHash.GetOrCompute(subTypeId));
		}
	}
}
