using ProtoBuf.Meta;
using System;
using System.Collections.Generic;

namespace VRage.Serialization
{
	/// <summary>
	/// Universal representation of a ProtoBuf type model.
	/// </summary>
	public interface IProtoTypeModel
	{
		/// <summary>
		/// The ProtoBuf Type Model.
		/// </summary>
		TypeModel Model
		{
			get;
		}

		/// <summary>
		/// Register a range of types with the type model.
		/// </summary>
		/// <param name="types"></param>
		void RegisterTypes(IEnumerable<Type> types);

		/// <summary>
		/// Flush all metadata caches in this model.
		/// </summary>
		void FlushCaches();
	}
}
