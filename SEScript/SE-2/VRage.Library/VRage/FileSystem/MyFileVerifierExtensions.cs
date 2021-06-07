using System.IO;

namespace VRage.FileSystem
{
	public static class MyFileVerifierExtensions
	{
		public static Stream Verify(this IFileVerifier verifier, string path, Stream stream)
		{
			return verifier.Verify(path, stream);
		}
	}
}
