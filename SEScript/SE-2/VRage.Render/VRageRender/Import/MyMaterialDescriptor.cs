using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Import
{
	/// <summary>
	/// material params for export
	/// </summary>
	public class MyMaterialDescriptor
	{
		public Dictionary<string, string> Textures = new Dictionary<string, string>();

		public Dictionary<string, string> UserData = new Dictionary<string, string>();

		public string MaterialName
		{
			get;
			private set;
		}

		public string Technique
		{
			get;
			set;
		}

		public MyMeshDrawTechnique TechniqueEnum
		{
			get
			{
				Enum.TryParse(Technique, out MyMeshDrawTechnique result);
				return result;
			}
			set
			{
				Technique = value.ToString();
			}
		}

		public string GlassCW
		{
			get;
			set;
		}

		public string GlassCCW
		{
			get;
			set;
		}

		public bool GlassSmoothNormals
		{
			get;
			set;
		}

		public MyFacingEnum Facing
		{
			get
			{
				if (UserData.TryGetValue("Facing", out string value))
				{
					if (!Enum.TryParse(value, out MyFacingEnum result))
					{
						return MyFacingEnum.None;
					}
					return result;
				}
				return MyFacingEnum.None;
			}
		}

		public Vector2 WindScaleAndFreq
		{
			get
			{
				Vector2 zero = Vector2.Zero;
				if (UserData.TryGetValue("WindScale", out string value))
				{
					if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
					{
						return zero;
					}
					zero.X = result;
					if (UserData.TryGetValue("WindFrequency", out value) && !float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
					{
						return zero;
					}
					zero.Y = result;
				}
				return zero;
			}
		}

		/// <summary>
		/// c-tor
		/// </summary>
		/// <param name="materialName"></param>
		public MyMaterialDescriptor(string materialName)
		{
			MaterialName = materialName;
			Technique = "MESH";
			GlassCCW = string.Empty;
			GlassCW = string.Empty;
			GlassSmoothNormals = true;
		}

		public MyMaterialDescriptor()
		{
		}

		/// <summary>
		/// Write to binary file
		/// </summary>
		/// <param name="writer"></param>
		/// <returns></returns>
		public bool Write(BinaryWriter writer)
		{
			writer.Write((MaterialName != null) ? MaterialName : "");
			writer.Write(Textures.Count);
			foreach (KeyValuePair<string, string> texture in Textures)
			{
				writer.Write(texture.Key);
				writer.Write((texture.Value == null) ? "" : texture.Value);
			}
			writer.Write(UserData.Count);
			foreach (KeyValuePair<string, string> userDatum in UserData)
			{
				writer.Write(userDatum.Key);
				writer.Write((userDatum.Value == null) ? "" : userDatum.Value);
			}
			writer.Write(Technique);
			if (Technique == "GLASS")
			{
				writer.Write(GlassCW);
				writer.Write(GlassCCW);
				writer.Write(GlassSmoothNormals);
			}
			return true;
		}

		public bool Read(BinaryReader reader, int version)
		{
			Textures.Clear();
			UserData.Clear();
			MaterialName = reader.ReadString();
			if (string.IsNullOrEmpty(MaterialName))
			{
				MaterialName = null;
			}
			if (version < 1052002)
			{
				string value = reader.ReadString();
				if (!string.IsNullOrEmpty(value))
				{
					Textures.Add("DiffuseTexture", value);
				}
				string value2 = reader.ReadString();
				if (!string.IsNullOrEmpty(value2))
				{
					Textures.Add("NormalTexture", value2);
				}
			}
			else
			{
				int num = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					string key = reader.ReadString();
					string value3 = reader.ReadString();
					Textures.Add(key, value3);
				}
			}
			if (version >= 1068001)
			{
				int num2 = reader.ReadInt32();
				for (int j = 0; j < num2; j++)
				{
					string key2 = reader.ReadString();
					string value4 = reader.ReadString();
					UserData.Add(key2, value4);
				}
			}
			if (version < 1157001)
			{
				reader.ReadSingle();
				reader.ReadSingle();
				reader.ReadSingle();
				reader.ReadSingle();
				reader.ReadSingle();
				reader.ReadSingle();
				reader.ReadSingle();
			}
			if (version < 1052001)
			{
				string text2 = Technique = ((MyMeshDrawTechniqueOld)reader.ReadInt32()).ToString();
			}
			else
			{
				Technique = reader.ReadString();
			}
			if (Technique == "GLASS")
			{
				if (version >= 1043001)
				{
					GlassCW = reader.ReadString();
					GlassCCW = reader.ReadString();
					GlassSmoothNormals = reader.ReadBoolean();
					if (!string.IsNullOrEmpty(GlassCCW) && !MyTransparentMaterials.ContainsMaterial(MyStringId.GetOrCompute(MaterialName)) && MyTransparentMaterials.ContainsMaterial(MyStringId.GetOrCompute(GlassCCW)))
					{
						MaterialName = GlassCCW;
					}
				}
				else
				{
					reader.ReadSingle();
					reader.ReadSingle();
					reader.ReadSingle();
					reader.ReadSingle();
					GlassCW = "GlassCW";
					GlassCCW = "GlassCCW";
					GlassSmoothNormals = false;
				}
			}
			return true;
		}
	}
}
