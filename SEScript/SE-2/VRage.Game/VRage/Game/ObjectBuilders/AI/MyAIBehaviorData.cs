using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.AI
{
	[MyObjectBuilderDefinition(null, null)]
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyAIBehaviorData : MyObjectBuilder_Base
	{
		[ProtoContract]
		public class CategorizedData
		{
			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003ECategorizedData_003C_003ECategory_003C_003EAccessor : IMemberAccessor<CategorizedData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CategorizedData owner, in string value)
				{
					owner.Category = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CategorizedData owner, out string value)
				{
					value = owner.Category;
				}
			}

			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003ECategorizedData_003C_003EDescriptors_003C_003EAccessor : IMemberAccessor<CategorizedData, ActionData[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CategorizedData owner, in ActionData[] value)
				{
					owner.Descriptors = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CategorizedData owner, out ActionData[] value)
				{
					value = owner.Descriptors;
				}
			}

			private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003ECategorizedData_003C_003EActor : IActivator, IActivator<CategorizedData>
			{
				private sealed override object CreateInstance()
				{
					return new CategorizedData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CategorizedData CreateInstance()
				{
					return new CategorizedData();
				}

				CategorizedData IActivator<CategorizedData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public string Category;

			[XmlArrayItem("Action")]
			[ProtoMember(4)]
			public ActionData[] Descriptors;
		}

		[ProtoContract]
		public class ParameterData
		{
			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EParameterData_003C_003EName_003C_003EAccessor : IMemberAccessor<ParameterData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParameterData owner, in string value)
				{
					owner.Name = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParameterData owner, out string value)
				{
					value = owner.Name;
				}
			}

			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EParameterData_003C_003ETypeFullName_003C_003EAccessor : IMemberAccessor<ParameterData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParameterData owner, in string value)
				{
					owner.TypeFullName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParameterData owner, out string value)
				{
					value = owner.TypeFullName;
				}
			}

			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EParameterData_003C_003EMemType_003C_003EAccessor : IMemberAccessor<ParameterData, MyMemoryParameterType>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParameterData owner, in MyMemoryParameterType value)
				{
					owner.MemType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParameterData owner, out MyMemoryParameterType value)
				{
					value = owner.MemType;
				}
			}

			private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EParameterData_003C_003EActor : IActivator, IActivator<ParameterData>
			{
				private sealed override object CreateInstance()
				{
					return new ParameterData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ParameterData CreateInstance()
				{
					return new ParameterData();
				}

				ParameterData IActivator<ParameterData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(7)]
			[XmlAttribute]
			public string Name;

			[ProtoMember(10)]
			[XmlAttribute]
			public string TypeFullName;

			[ProtoMember(13)]
			[XmlAttribute]
			public MyMemoryParameterType MemType;
		}

		[ProtoContract]
		public class ActionData
		{
			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EActionData_003C_003EActionName_003C_003EAccessor : IMemberAccessor<ActionData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ActionData owner, in string value)
				{
					owner.ActionName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ActionData owner, out string value)
				{
					value = owner.ActionName;
				}
			}

			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EActionData_003C_003EReturnsRunning_003C_003EAccessor : IMemberAccessor<ActionData, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ActionData owner, in bool value)
				{
					owner.ReturnsRunning = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ActionData owner, out bool value)
				{
					value = owner.ReturnsRunning;
				}
			}

			protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EActionData_003C_003EParameters_003C_003EAccessor : IMemberAccessor<ActionData, ParameterData[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ActionData owner, in ParameterData[] value)
				{
					owner.Parameters = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ActionData owner, out ParameterData[] value)
				{
					value = owner.Parameters;
				}
			}

			private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EActionData_003C_003EActor : IActivator, IActivator<ActionData>
			{
				private sealed override object CreateInstance()
				{
					return new ActionData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ActionData CreateInstance()
				{
					return new ActionData();
				}

				ActionData IActivator<ActionData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(16)]
			[XmlAttribute]
			public string ActionName;

			[ProtoMember(19)]
			[XmlAttribute]
			public bool ReturnsRunning;

			[XmlArrayItem("Param")]
			[ProtoMember(22)]
			public ParameterData[] Parameters;
		}

		protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EEntries_003C_003EAccessor : IMemberAccessor<MyAIBehaviorData, CategorizedData[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyAIBehaviorData owner, in CategorizedData[] value)
			{
				owner.Entries = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyAIBehaviorData owner, out CategorizedData[] value)
			{
				value = owner.Entries;
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyAIBehaviorData, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyAIBehaviorData owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyAIBehaviorData owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyAIBehaviorData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyAIBehaviorData owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyAIBehaviorData owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyAIBehaviorData, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyAIBehaviorData owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyAIBehaviorData owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyAIBehaviorData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyAIBehaviorData owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyAIBehaviorData owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_AI_MyAIBehaviorData_003C_003EActor : IActivator, IActivator<MyAIBehaviorData>
		{
			private sealed override object CreateInstance()
			{
				return new MyAIBehaviorData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAIBehaviorData CreateInstance()
			{
				return new MyAIBehaviorData();
			}

			MyAIBehaviorData IActivator<MyAIBehaviorData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlArrayItem("AICategory")]
		[ProtoMember(25)]
		public CategorizedData[] Entries;
	}
}
