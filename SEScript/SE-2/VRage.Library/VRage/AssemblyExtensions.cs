using System.Reflection;

namespace VRage
{
	public static class AssemblyExtensions
	{
		public static ProcessorArchitecture ToProcessorArchitecture(this PortableExecutableKinds peKind)
		{
			switch (peKind & ~PortableExecutableKinds.ILOnly)
			{
			case PortableExecutableKinds.PE32Plus:
				return ProcessorArchitecture.Amd64;
			case PortableExecutableKinds.Required32Bit:
				return ProcessorArchitecture.X86;
			case PortableExecutableKinds.Unmanaged32Bit:
				return ProcessorArchitecture.X86;
			default:
				if ((peKind & PortableExecutableKinds.ILOnly) == 0)
				{
					return ProcessorArchitecture.None;
				}
				return ProcessorArchitecture.MSIL;
			}
		}

		public static PortableExecutableKinds GetPeKind(this Assembly assembly)
		{
			assembly.ManifestModule.GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine _);
			return peKind;
		}

		public static ProcessorArchitecture GetArchitecture(this Assembly assembly)
		{
			return assembly.GetPeKind().ToProcessorArchitecture();
		}

		public static ProcessorArchitecture TryGetArchitecture(string assemblyName)
		{
			try
			{
				return AssemblyName.GetAssemblyName(assemblyName).ProcessorArchitecture;
			}
			catch
			{
				return ProcessorArchitecture.None;
			}
		}

		public static ProcessorArchitecture TryGetArchitecture(this Assembly assembly)
		{
			try
			{
				return assembly.GetArchitecture();
			}
			catch
			{
				return ProcessorArchitecture.None;
			}
		}
	}
}
