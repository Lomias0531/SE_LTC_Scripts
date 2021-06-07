using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_BehaviorTreeActionNode : MyObjectBuilder_BehaviorTreeNode
	{
		[ProtoContract]
		[XmlType("TypeValue")]
		public abstract class TypeValue
		{
			public abstract object GetValue();
		}

		[ProtoContract]
		[XmlType("IntType")]
		public class IntType : TypeValue
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EIntType_003C_003EIntValue_003C_003EAccessor : IMemberAccessor<IntType, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref IntType owner, in int value)
				{
					owner.IntValue = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref IntType owner, out int value)
				{
					value = owner.IntValue;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EIntType_003C_003EActor : IActivator, IActivator<IntType>
			{
				private sealed override object CreateInstance()
				{
					return new IntType();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override IntType CreateInstance()
				{
					return new IntType();
				}

				IntType IActivator<IntType>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(1)]
			public int IntValue;

			public override object GetValue()
			{
				return (int)IntValue;
			}
		}

		[ProtoContract]
		[XmlType("StringType")]
		public class StringType : TypeValue
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EStringType_003C_003EStringValue_003C_003EAccessor : IMemberAccessor<StringType, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref StringType owner, in string value)
				{
					owner.StringValue = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref StringType owner, out string value)
				{
					value = owner.StringValue;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EStringType_003C_003EActor : IActivator, IActivator<StringType>
			{
				private sealed override object CreateInstance()
				{
					return new StringType();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override StringType CreateInstance()
				{
					return new StringType();
				}

				StringType IActivator<StringType>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(4)]
			public string StringValue;

			public override object GetValue()
			{
				return StringValue;
			}
		}

		[ProtoContract]
		[XmlType("FloatType")]
		public class FloatType : TypeValue
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EFloatType_003C_003EFloatValue_003C_003EAccessor : IMemberAccessor<FloatType, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref FloatType owner, in float value)
				{
					owner.FloatValue = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref FloatType owner, out float value)
				{
					value = owner.FloatValue;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EFloatType_003C_003EActor : IActivator, IActivator<FloatType>
			{
				private sealed override object CreateInstance()
				{
					return new FloatType();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override FloatType CreateInstance()
				{
					return new FloatType();
				}

				FloatType IActivator<FloatType>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(7)]
			public float FloatValue;

			public override object GetValue()
			{
				return FloatValue;
			}
		}

		[ProtoContract]
		[XmlType("BoolType")]
		public class BoolType : TypeValue
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EBoolType_003C_003EBoolValue_003C_003EAccessor : IMemberAccessor<BoolType, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoolType owner, in bool value)
				{
					owner.BoolValue = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoolType owner, out bool value)
				{
					value = owner.BoolValue;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EBoolType_003C_003EActor : IActivator, IActivator<BoolType>
			{
				private sealed override object CreateInstance()
				{
					return new BoolType();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BoolType CreateInstance()
				{
					return new BoolType();
				}

				BoolType IActivator<BoolType>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(10)]
			public bool BoolValue;

			public override object GetValue()
			{
				return BoolValue;
			}
		}

		[ProtoContract]
		[XmlType("MemType")]
		public class MemType : TypeValue
		{
			protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EMemType_003C_003EMemName_003C_003EAccessor : IMemberAccessor<MemType, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MemType owner, in string value)
				{
					owner.MemName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MemType owner, out string value)
				{
					value = owner.MemName;
				}
			}

			private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EMemType_003C_003EActor : IActivator, IActivator<MemType>
			{
				private sealed override object CreateInstance()
				{
					return new MemType();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MemType CreateInstance()
				{
					return new MemType();
				}

				MemType IActivator<MemType>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[XmlAttribute]
			[ProtoMember(13)]
			public string MemName;

			public override object GetValue()
			{
				return MemName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EActionName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in string value)
			{
				owner.ActionName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out string value)
			{
				value = owner.ActionName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EParameters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, TypeValue[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in TypeValue[] value)
			{
				owner.Parameters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out TypeValue[] value)
			{
				value = owner.Parameters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeActionNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_BehaviorTreeActionNode owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_BehaviorTreeActionNode owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_BehaviorTreeActionNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeActionNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeActionNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BehaviorTreeActionNode CreateInstance()
			{
				return new MyObjectBuilder_BehaviorTreeActionNode();
			}

			MyObjectBuilder_BehaviorTreeActionNode IActivator<MyObjectBuilder_BehaviorTreeActionNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		public string ActionName;

		[ProtoMember(19)]
		[XmlArrayItem("Parameter", Type = typeof(MyAbstractXmlSerializer<TypeValue>))]
		public TypeValue[] Parameters;
	}
}
