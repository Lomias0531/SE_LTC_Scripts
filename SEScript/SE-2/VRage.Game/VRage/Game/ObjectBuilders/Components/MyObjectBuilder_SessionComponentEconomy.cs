using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SessionComponentEconomy : MyObjectBuilder_SessionComponent
	{
		public struct MyIdBalancePair
		{
			public long Id;

			public long Balance;
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EGenerateFactionsOnStart_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in bool value)
			{
				owner.GenerateFactionsOnStart = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out bool value)
			{
				value = owner.GenerateFactionsOnStart;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EAnalysisTotalCurrency_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value)
			{
				owner.AnalysisTotalCurrency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value)
			{
				value = owner.AnalysisTotalCurrency;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EAnalysisCurrencyFaucet_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value)
			{
				owner.AnalysisCurrencyFaucet = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value)
			{
				value = owner.AnalysisCurrencyFaucet;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EAnalysisCurrencySink_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value)
			{
				owner.AnalysisCurrencySink = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value)
			{
				value = owner.AnalysisCurrencySink;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003ECurrencyGeneratedThisTick_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value)
			{
				owner.CurrencyGeneratedThisTick = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value)
			{
				value = owner.CurrencyGeneratedThisTick;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003ECurrencyDestroyedThisTick_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in long value)
			{
				owner.CurrencyDestroyedThisTick = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out long value)
			{
				value = owner.CurrencyDestroyedThisTick;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EAnalysisPerPlayerCurrency_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MySerializableList<MyIdBalancePair>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in MySerializableList<MyIdBalancePair> value)
			{
				owner.AnalysisPerPlayerCurrency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out MySerializableList<MyIdBalancePair> value)
			{
				value = owner.AnalysisPerPlayerCurrency;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EAnalysisPerFactionCurrency_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MySerializableList<MyIdBalancePair>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in MySerializableList<MyIdBalancePair> value)
			{
				owner.AnalysisPerFactionCurrency = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out MySerializableList<MyIdBalancePair> value)
			{
				value = owner.AnalysisPerFactionCurrency;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentEconomy, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentEconomy owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentEconomy owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentEconomy_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentEconomy>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentEconomy();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SessionComponentEconomy CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentEconomy();
			}

			MyObjectBuilder_SessionComponentEconomy IActivator<MyObjectBuilder_SessionComponentEconomy>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool GenerateFactionsOnStart = true;

		public long AnalysisTotalCurrency;

		public long AnalysisCurrencyFaucet;

		public long AnalysisCurrencySink;

		public long CurrencyGeneratedThisTick;

		public long CurrencyDestroyedThisTick;

		public MySerializableList<MyIdBalancePair> AnalysisPerPlayerCurrency;

		public MySerializableList<MyIdBalancePair> AnalysisPerFactionCurrency;
	}
}
