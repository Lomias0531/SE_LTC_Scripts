using System;
using System.Reflection;
using System.Text;
using VRage.Library.Collections;
using VRage.Network;

namespace VRage.Serialization
{
	public class MySerializeInfo : ISerializerInfo
	{
		public static readonly MySerializeInfo Default = new MySerializeInfo();

		public readonly MyObjectFlags Flags;

		public readonly MyPrimitiveFlags PrimitiveFlags;

		public readonly ushort FixedLength;

		public readonly DynamicSerializerDelegate DynamicSerializer;

		/// <summary>
		/// Serialization settings for dictionary key.
		/// </summary>
		public readonly MySerializeInfo KeyInfo;

		/// <summary>
		/// Serialization settings for dictionary value or collection / array elements
		/// </summary>
		public readonly MySerializeInfo ItemInfo;

		public bool IsNullable
		{
			get
			{
				if ((Flags & MyObjectFlags.DefaultZero) == 0)
				{
					return IsNullOrEmpty;
				}
				return true;
			}
		}

		public bool IsDynamic
		{
			get
			{
				if ((Flags & MyObjectFlags.Dynamic) == 0)
				{
					return IsDynamicDefault;
				}
				return true;
			}
		}

		public bool IsNullOrEmpty => (Flags & MyObjectFlags.DefaultValueOrEmpty) != 0;

		public bool IsDynamicDefault => (Flags & MyObjectFlags.DynamicDefault) != 0;

		public bool IsSigned => (PrimitiveFlags & MyPrimitiveFlags.Signed) != 0;

		public bool IsNormalized => (PrimitiveFlags & MyPrimitiveFlags.Normalized) != 0;

		public bool IsVariant
		{
			get
			{
				if (!IsSigned)
				{
					return (PrimitiveFlags & MyPrimitiveFlags.Variant) != 0;
				}
				return false;
			}
		}

		public bool IsVariantSigned => (PrimitiveFlags & MyPrimitiveFlags.VariantSigned) != 0;

		public bool IsFixed8 => (PrimitiveFlags & MyPrimitiveFlags.FixedPoint8) != 0;

		public bool IsFixed16 => (PrimitiveFlags & MyPrimitiveFlags.FixedPoint16) != 0;

		public Encoding Encoding
		{
			get
			{
				if ((PrimitiveFlags & MyPrimitiveFlags.Ascii) == 0)
				{
					return Encoding.UTF8;
				}
				return Encoding.ASCII;
			}
		}

		private MySerializeInfo()
		{
		}

		public MySerializeInfo(MyObjectFlags flags, MyPrimitiveFlags primitiveFlags, ushort fixedLength, DynamicSerializerDelegate dynamicSerializer, MySerializeInfo keyInfo, MySerializeInfo itemInfo)
		{
			Flags = flags;
			PrimitiveFlags = primitiveFlags;
			FixedLength = fixedLength;
			KeyInfo = keyInfo;
			ItemInfo = itemInfo;
			DynamicSerializer = dynamicSerializer;
		}

		public MySerializeInfo(SerializeAttribute attribute, MySerializeInfo keyInfo, MySerializeInfo itemInfo)
		{
			if (attribute != null)
			{
				Flags = attribute.Flags;
				PrimitiveFlags = attribute.PrimitiveFlags;
				FixedLength = attribute.FixedLength;
				if (IsDynamic)
				{
					DynamicSerializer = ((IDynamicResolver)Activator.CreateInstance(attribute.DynamicSerializerType)).Serialize;
				}
			}
			KeyInfo = keyInfo;
			ItemInfo = itemInfo;
		}

		public static MySerializeInfo Create(ICustomAttributeProvider reflectionInfo)
		{
			SerializeAttribute serializeAttribute = new SerializeAttribute();
			SerializeAttribute serializeAttribute2 = null;
			SerializeAttribute serializeAttribute3 = null;
			object[] customAttributes = reflectionInfo.GetCustomAttributes(typeof(SerializeAttribute), inherit: false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				SerializeAttribute serializeAttribute4 = (SerializeAttribute)customAttributes[i];
				if (serializeAttribute4.Kind == MySerializeKind.Default)
				{
					serializeAttribute = Merge(serializeAttribute, serializeAttribute4);
				}
				else if (serializeAttribute4.Kind == MySerializeKind.Key)
				{
					serializeAttribute2 = Merge(serializeAttribute2, serializeAttribute4);
				}
				else if (serializeAttribute4.Kind == MySerializeKind.Item)
				{
					serializeAttribute3 = Merge(serializeAttribute3, serializeAttribute4);
				}
			}
			return new MySerializeInfo(serializeAttribute, ToInfo(serializeAttribute2), ToInfo(serializeAttribute3));
		}

		public static MySerializeInfo CreateForParameter(ParameterInfo[] parameters, int index)
		{
			if (index >= parameters.Length)
			{
				return Default;
			}
			return Create(parameters[index]);
		}

		private static SerializeAttribute Merge(SerializeAttribute first, SerializeAttribute second)
		{
			if (first == null)
			{
				return second;
			}
			if (second == null)
			{
				return first;
			}
			return new SerializeAttribute
			{
				Flags = (first.Flags | second.Flags),
				PrimitiveFlags = (first.PrimitiveFlags | second.PrimitiveFlags),
				FixedLength = ((first.FixedLength != 0) ? first.FixedLength : second.FixedLength),
				DynamicSerializerType = (first.DynamicSerializerType ?? second.DynamicSerializerType)
			};
		}

		private static MySerializeInfo ToInfo(SerializeAttribute attr)
		{
			if (attr == null)
			{
				return null;
			}
			return new MySerializeInfo(attr, null, null);
		}
	}
}
