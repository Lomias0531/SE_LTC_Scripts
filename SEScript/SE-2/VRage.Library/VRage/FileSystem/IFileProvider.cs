using System.Collections.Generic;
using System.IO;

namespace VRage.FileSystem
{
	public interface IFileProvider
	{
		/// <summary>
		/// Opens file, returns null when file does not exists
		/// </summary>
		Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

		/// <summary>
		/// True if directory exists
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool DirectoryExists(string path);

		/// <summary>
		/// Returns list of files in directory
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		IEnumerable<string> GetFiles(string path, string filter, MySearchOption searchOption);

		/// <summary>
		/// True if file exists
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool FileExists(string path);
	}
}
