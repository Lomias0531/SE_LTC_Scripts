using System;
using System.Reflection;
using VRage.Meta;
using VRage.Serialization;

namespace VRage.Factory
{
	/// <summary>
	/// Base game metadata context.
	///
	/// Base game context has compatibility observers for features that are not usable by mods.
	/// </summary>
	internal class MyGameMetadataContext : MyMetadataContext
	{
		internal class Crawler : IMyAttributeIndexer, IMyMetadataIndexer
		{
			public AttributeObserver Observer;

			public Crawler(AttributeObserver observer)
			{
				Observer = observer;
			}

			public void SetParent(IMyMetadataIndexer indexer)
			{
			}

			public void Activate()
			{
			}

			public void Observe(Attribute attribute, Type type)
			{
				Observer(type, attribute);
			}

			public void Close()
			{
			}

			public void Process()
			{
			}
		}

		/// <summary>
		/// Add a attribute crawler to the metadata system, this will cause the crawler
		/// to be invoked whenever any types are declared with the observed attribute.
		///
		/// There can be any number of crawlers on attributes.
		/// </summary>
		/// <param name="attributeType">The type of the attribute to observe</param>
		/// <param name="observer">The observer to be invoked when the attribute is encountered.</param>
		public void RegisterAttributeObserver(Type attributeType, AttributeObserver observer)
		{
			AttributeIndexers.Add(attributeType, new Crawler(observer));
		}

		protected override void Index(Assembly assembly, bool batch = false)
		{
			MyFactory.RegisterFromAssembly(assembly);
			base.Index(assembly, batch);
		}
	}
}
