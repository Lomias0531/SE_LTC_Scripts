using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	/// <summary>
	/// Prefer getting definition ID using object builder used to create the item.
	/// If you have automatic rifle, in its Init method create new MyDefinitionId
	/// using TypeId and SubtypeName of object builder.
	/// Do not write specific values in code, as data comes from XML and if those
	/// change, code needs to change as well.
	/// </summary>
	[Serializable]
	public struct MyDefinitionId : IEquatable<MyDefinitionId>
	{
		public class DefinitionIdComparerType : IEqualityComparer<MyDefinitionId>
		{
			public bool Equals(MyDefinitionId x, MyDefinitionId y)
			{
				if (x.TypeId == y.TypeId)
				{
					return x.SubtypeId == y.SubtypeId;
				}
				return false;
			}

			public int GetHashCode(MyDefinitionId obj)
			{
				return obj.GetHashCode();
			}
		}

		protected class VRage_Game_MyDefinitionId_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<MyDefinitionId, MyObjectBuilderType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDefinitionId owner, in MyObjectBuilderType value)
			{
				owner.TypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDefinitionId owner, out MyObjectBuilderType value)
			{
				value = owner.TypeId;
			}
		}

		protected class VRage_Game_MyDefinitionId_003C_003ESubtypeId_003C_003EAccessor : IMemberAccessor<MyDefinitionId, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDefinitionId owner, in MyStringHash value)
			{
				owner.SubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDefinitionId owner, out MyStringHash value)
			{
				value = owner.SubtypeId;
			}
		}

		public static readonly DefinitionIdComparerType Comparer = new DefinitionIdComparerType();

		public readonly MyObjectBuilderType TypeId;

		public readonly MyStringHash SubtypeId;

		public string SubtypeName
		{
			get
			{
				MyStringHash subtypeId = SubtypeId;
				return subtypeId.ToString();
			}
		}

		/// <summary>
		/// Creates a new definition ID from a given content.
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		public static MyDefinitionId FromContent(MyObjectBuilder_Base content)
		{
			return new MyDefinitionId(content.TypeId, content.SubtypeId);
		}

		/// <summary>
		/// Attempts to create a definition ID from a definition string, which has the form (using ores as an example) "MyObjectBuilder_Ore/Iron".
		/// The first part must represent an existing type. If it does not, an exception will be thrown. The second (the subtype) is not enforced.
		/// See <see cref="M:VRage.Game.MyDefinitionId.TryParse(System.String,VRage.Game.MyDefinitionId@)" /> for a parsing method that does not throw an exception.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static MyDefinitionId Parse(string id)
		{
			if (!TryParse(id, out MyDefinitionId definitionId))
			{
				throw new ArgumentException("The provided type does not conform to a definition ID.", "id");
			}
			return definitionId;
		}

		/// <summary>
		/// Attempts to create a definition ID from a definition string, which has the form (using ores as an example) "MyObjectBuilder_Ore/Iron".
		/// The first part must represent an existing type, while the second (the subtype) is not enforced.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="definitionId"></param>
		/// <returns></returns>
		public static bool TryParse(string id, out MyDefinitionId definitionId)
		{
			if (string.IsNullOrEmpty(id))
			{
				definitionId = default(MyDefinitionId);
				return false;
			}
			int num = id.IndexOf('/');
			if (num == -1)
			{
				definitionId = default(MyDefinitionId);
				return false;
			}
			if (MyObjectBuilderType.TryParse(id.Substring(0, num).Trim(), out MyObjectBuilderType result))
			{
				string text = id.Substring(num + 1).Trim();
				if (text == "(null)")
				{
					text = null;
				}
				definitionId = new MyDefinitionId(result, text);
				return true;
			}
			definitionId = default(MyDefinitionId);
			return false;
		}

		public static bool TryParse(string type, string subtype, out MyDefinitionId definitionId)
		{
			if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(subtype))
			{
				definitionId = default(MyDefinitionId);
				return false;
			}
			if (MyObjectBuilderType.TryParse(type, out MyObjectBuilderType result))
			{
				definitionId = new MyDefinitionId(result, subtype);
				return true;
			}
			definitionId = default(MyDefinitionId);
			return false;
		}

		public MyDefinitionId(MyObjectBuilderType type)
			: this(type, MyStringHash.GetOrCompute(null))
		{
		}

		public MyDefinitionId(MyObjectBuilderType type, string subtypeName)
			: this(type, MyStringHash.GetOrCompute(subtypeName))
		{
		}

		public MyDefinitionId(MyObjectBuilderType type, MyStringHash subtypeId)
		{
			TypeId = type;
			SubtypeId = subtypeId;
		}

		public MyDefinitionId(MyRuntimeObjectBuilderId type, MyStringHash subtypeId)
		{
			TypeId = (MyObjectBuilderType)type;
			SubtypeId = subtypeId;
		}

		public override int GetHashCode()
		{
			MyObjectBuilderType typeId = TypeId;
			int num = typeId.GetHashCode() << 16;
			MyStringHash subtypeId = SubtypeId;
			return num ^ subtypeId.GetHashCode();
		}

		/// <summary>
		/// Safer hash code. It is unique in more situations than GetHashCode would be,
		/// but it may still require full check.
		/// </summary>
		/// <returns>64-bit hash code.</returns>
		public long GetHashCodeLong()
		{
			MyObjectBuilderType typeId = TypeId;
			long num = (long)typeId.GetHashCode() << 32;
			MyStringHash subtypeId = SubtypeId;
			return num ^ subtypeId.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is MyDefinitionId)
			{
				return Equals((MyDefinitionId)obj);
			}
			return false;
		}

		public override string ToString()
		{
			object obj;
			if (TypeId.IsNull)
			{
				obj = "(null)";
			}
			else
			{
				MyObjectBuilderType typeId = TypeId;
				obj = typeId.ToString();
			}
			string arg = (string)obj;
			string arg2 = (!string.IsNullOrEmpty(SubtypeName)) ? SubtypeName : "(null)";
			return $"{arg}/{arg2}";
		}

		public bool Equals(MyDefinitionId other)
		{
			if (TypeId == other.TypeId)
			{
				return SubtypeId == other.SubtypeId;
			}
			return false;
		}

		public static bool operator ==(MyDefinitionId l, MyDefinitionId r)
		{
			return l.Equals(r);
		}

		public static bool operator !=(MyDefinitionId l, MyDefinitionId r)
		{
			return !l.Equals(r);
		}

		public static implicit operator MyDefinitionId(SerializableDefinitionId v)
		{
			return new MyDefinitionId(v.TypeId, v.SubtypeName);
		}

		public static implicit operator SerializableDefinitionId(MyDefinitionId v)
		{
			return new SerializableDefinitionId(v.TypeId, v.SubtypeName);
		}
	}
}
