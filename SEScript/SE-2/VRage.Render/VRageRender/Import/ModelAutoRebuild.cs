using System.Collections.Generic;
using System.IO;
using VRage.Security;

namespace VRageRender.Import
{
	public static class ModelAutoRebuild
	{
		private static MyModelImporter m_importer = new MyModelImporter();

		/// <summary>
		/// Checks whether that model file was build with current sources files. If current sources of this model - FBX, XML, HKT etc. were changed, this returns false.
		/// </summary>
		/// <param name="modelFile"></param>
		/// <returns>true - if data hashes of source files are valid </returns>
		/// <returns>false - if data has been changed</returns>
		public static bool IsModelActual(string modelFile, string FBXFile, string HKTFile, string XMLFile)
		{
			m_importer.ImportData(modelFile);
			Dictionary<string, object> tagData = m_importer.GetTagData();
			if (File.Exists(FBXFile))
			{
				if (tagData.GetValueOrDefault("FBXHash") == null)
				{
					return false;
				}
				Md5.Hash fileHash = GetFileHash(FBXFile);
				Md5.Hash hash = (Md5.Hash)tagData.GetValueOrDefault("FBXHash");
				if (fileHash.A != hash.A || fileHash.B != hash.B || fileHash.C != hash.C || fileHash.D != hash.D)
				{
					return false;
				}
			}
			if (File.Exists(HKTFile))
			{
				if (tagData.GetValueOrDefault("HKTHash") == null)
				{
					return false;
				}
				Md5.Hash fileHash2 = GetFileHash(HKTFile);
				Md5.Hash hash2 = (Md5.Hash)tagData.GetValueOrDefault("HKTHash");
				if (fileHash2.A != hash2.A || fileHash2.B != hash2.B || fileHash2.C != hash2.C || fileHash2.D != hash2.D)
				{
					return false;
				}
			}
			if (File.Exists(XMLFile))
			{
				if (tagData.GetValueOrDefault("XMLHash") == null || !File.Exists(XMLFile))
				{
					return false;
				}
				Md5.Hash fileHash3 = GetFileHash(XMLFile);
				Md5.Hash hash3 = (Md5.Hash)tagData.GetValueOrDefault("XMLHash");
				if (fileHash3.A != hash3.A || fileHash3.B != hash3.B || fileHash3.C != hash3.C || fileHash3.D != hash3.D)
				{
					return false;
				}
			}
			return true;
		}

		public static Md5.Hash GetFileHash(string fileName)
		{
			return Md5.ComputeHash(File.ReadAllBytes(fileName));
		}
	}
}
