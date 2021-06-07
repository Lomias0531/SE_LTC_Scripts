using ProtoBuf.Meta;
using System;
using System.Collections.Generic;

namespace VRage.Serialization
{
	/// <summary>
	/// A precompiled ProtoBuf type model.
	/// </summary>
	public class StaticTypeModel : IProtoTypeModel
	{
		public TypeModel Model
		{
			get;
		}

		/// <inheritdoc />
		public StaticTypeModel()
		{
			Model = TypeModel.LoadCompiled("ProtoContracts.dll", "ProtoContracts", setDefault: true);
		}

		public StaticTypeModel(string assembly, string typeName)
		{
			Model = TypeModel.LoadCompiled(assembly, typeName, setDefault: true);
		}

		/// <inheritdoc />
		public void RegisterTypes(IEnumerable<Type> types)
		{
		}

		/// <inheritdoc />
		public void FlushCaches()
		{
		}
	}
}
