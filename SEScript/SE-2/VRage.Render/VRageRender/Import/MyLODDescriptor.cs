using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;

namespace VRageRender.Import
{
	public class MyLODDescriptor
	{
		public float Distance;

		public string Model;

		public string RenderQuality;

		public List<int> RenderQualityList;

		/// <summary>
		/// Absolute file path to the LOD model related to the parent assetFilePath.
		/// </summary>
		/// <param name="parentAssetFilePath">File path of parent asset.</param>
		/// <returns>Absolute file path.</returns>
		public string GetModelAbsoluteFilePath(string parentAssetFilePath)
		{
			if (Model == null)
			{
				return null;
			}
			string text = parentAssetFilePath.ToLower();
			string text2 = Model;
			if (!text2.Contains(".mwm"))
			{
				text2 += ".mwm";
			}
			if (Path.IsPathRooted(parentAssetFilePath) && text.Contains("models"))
			{
				string text3 = Path.Combine(parentAssetFilePath.Substring(0, text.IndexOf("models")), text2);
				if (MyFileSystem.FileExists(text3))
				{
					return text3;
				}
				text3 = Path.Combine(MyFileSystem.ContentPath, text2);
				if (!MyFileSystem.FileExists(text3))
				{
					return null;
				}
				return text3;
			}
			return Path.Combine(MyFileSystem.ContentPath, text2);
		}

		public bool Write(BinaryWriter writer)
		{
			writer.Write(Distance);
			writer.Write((Model != null) ? Model : "");
			writer.Write((RenderQuality != null) ? RenderQuality : "");
			return true;
		}

		public bool Read(BinaryReader reader)
		{
			Distance = reader.ReadSingle();
			Model = reader.ReadString();
			if (string.IsNullOrEmpty(Model))
			{
				Model = null;
			}
			RenderQuality = reader.ReadString();
			if (string.IsNullOrEmpty(RenderQuality))
			{
				RenderQuality = null;
			}
			return true;
		}
	}
}
