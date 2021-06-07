using System;

namespace VRage.Factory
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class MyDependencyAttribute : Attribute
	{
		public readonly Type Dependency;

		/// <summary>
		/// Whether this dependency applies to children.
		/// </summary>
		public bool Recursive;

		/// <summary>
		/// Whether this dependency is a requirement.
		///
		/// Sometimes the dependency only means ordering when the dependant is available.
		/// </summary>
		public bool Critical;

		public MyDependencyAttribute(Type dependency)
		{
			Dependency = dependency;
		}
	}
}
