using BulletXNA.BulletCollision;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using VRage.Import;
using VRage.Security;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Animations;
using VRageRender.Fractures;

namespace VRageRender.Import
{
	public class MyModelExporter : IDisposable
	{
		private BinaryWriter m_writer;

		private BinaryWriter m_originalWriter;

		private MemoryStream m_cacheStream;

		/// <summary>
		/// c-tor
		/// </summary>
		/// <param name="filePath"></param>
		public MyModelExporter(string filePath)
		{
			FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			m_writer = new BinaryWriter(output);
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		}

		/// <summary>
		/// c-tor
		/// </summary>
		/// <param name="filePath"></param>
		public MyModelExporter()
		{
		}

		/// <summary>
		/// Close
		/// </summary>
		public void Dispose()
		{
			if (m_writer != null)
			{
				m_writer.Close();
				m_writer = null;
			}
			if (m_originalWriter != null)
			{
				m_originalWriter.Close();
				m_originalWriter = null;
			}
		}

		public void StartCacheWrite()
		{
			m_originalWriter = m_writer;
			m_cacheStream = new MemoryStream();
			m_writer = new BinaryWriter(m_cacheStream);
		}

		public void StopCacheWrite()
		{
			m_writer.Close();
			m_writer = m_originalWriter;
		}

		public int GetCachePosition()
		{
			return (int)m_writer.BaseStream.Position;
		}

		public void FlushCache()
		{
			m_writer.Write(m_cacheStream.GetBuffer());
		}

		private int CalculateIndexSize(Dictionary<string, int> dict)
		{
			int num = 4;
			foreach (KeyValuePair<string, int> item in dict)
			{
				num += Encoding.ASCII.GetByteCount(item.Key) + 1;
				num += 4;
			}
			return num;
		}

		public void WriteIndexDictionary(Dictionary<string, int> dict)
		{
			int num = (int)m_writer.BaseStream.Position;
			int num2 = CalculateIndexSize(dict);
			m_writer.Write(dict.Count);
			foreach (KeyValuePair<string, int> item in dict)
			{
				m_writer.Write(item.Key);
				m_writer.Write(item.Value + num2 + num);
			}
		}

		/// <summary>
		/// WriteTag
		/// </summary>
		/// <param name="tagName"></param>
		private void WriteTag(string tagName)
		{
			m_writer.Write(tagName);
		}

		/// <summary>
		/// WriteVector2
		/// </summary>
		/// <param name="vct"></param>
		private void WriteVector(Vector2 vct)
		{
			m_writer.Write(vct.X);
			m_writer.Write(vct.Y);
		}

		/// <summary>
		/// WriteVector3
		/// </summary>
		/// <param name="vct"></param>
		private void WriteVector(Vector3 vct)
		{
			m_writer.Write(vct.X);
			m_writer.Write(vct.Y);
			m_writer.Write(vct.Z);
		}

		/// <summary>
		/// WriteVector4
		/// </summary>
		/// <param name="vct"></param>
		private void WriteVector(Vector4 vct)
		{
			m_writer.Write(vct.X);
			m_writer.Write(vct.Y);
			m_writer.Write(vct.Z);
			m_writer.Write(vct.W);
		}

		/// <summary>
		/// WriteMatrix
		/// </summary>
		/// <param name="matrix"></param>
		private void WriteMatrix(Matrix matrix)
		{
			m_writer.Write(matrix.M11);
			m_writer.Write(matrix.M12);
			m_writer.Write(matrix.M13);
			m_writer.Write(matrix.M14);
			m_writer.Write(matrix.M21);
			m_writer.Write(matrix.M22);
			m_writer.Write(matrix.M23);
			m_writer.Write(matrix.M24);
			m_writer.Write(matrix.M31);
			m_writer.Write(matrix.M32);
			m_writer.Write(matrix.M33);
			m_writer.Write(matrix.M34);
			m_writer.Write(matrix.M41);
			m_writer.Write(matrix.M42);
			m_writer.Write(matrix.M43);
			m_writer.Write(matrix.M44);
		}

		/// <summary>
		/// WriteVector2
		/// </summary>
		/// <param name="vct"></param>
		private void WriteVector(Vector2I vct)
		{
			m_writer.Write(vct.X);
			m_writer.Write(vct.Y);
		}

		/// <summary>
		/// WriteVector3
		/// </summary>
		/// <param name="vct"></param>
		private void WriteVector(Vector3I vct)
		{
			m_writer.Write(vct.X);
			m_writer.Write(vct.Y);
			m_writer.Write(vct.Z);
		}

		/// <summary>
		/// WriteVector4
		/// </summary>
		/// <param name="vct"></param>
		private void WriteVector(Vector4I vct)
		{
			m_writer.Write(vct.X);
			m_writer.Write(vct.Y);
			m_writer.Write(vct.Z);
			m_writer.Write(vct.W);
		}

		private void WriteVector(HalfVector4 val)
		{
			m_writer.Write(val.PackedValue);
		}

		private void WriteVector(HalfVector2 val)
		{
			m_writer.Write(val.PackedValue);
		}

		/// <summary>
		/// Write Byte4
		/// </summary>
		private void WriteByte4(Byte4 val)
		{
			m_writer.Write(val.PackedValue);
		}

		public bool ExportDataPackedAsHV4(string tagName, Vector3[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			for (int i = 0; i < vctArr.Length; i++)
			{
				Vector3 position = vctArr[i];
				WriteVector(VF_Packer.PackPosition(ref position));
			}
			return true;
		}

		public bool ExportData(string tagName, HalfVector4[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			foreach (HalfVector4 val in vctArr)
			{
				WriteVector(val);
			}
			return true;
		}

		public bool ExportData(string tagName, Byte4[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			foreach (Byte4 val in vctArr)
			{
				WriteByte4(val);
			}
			return true;
		}

		public bool ExportDataPackedAsB4(string tagName, Vector3[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			for (int i = 0; i < vctArr.Length; i++)
			{
				Vector3 normal = vctArr[i];
				Byte4 val = default(Byte4);
				val.PackedValue = VF_Packer.PackNormal(ref normal);
				WriteByte4(val);
			}
			return true;
		}

		public bool ExportDataPackedAsHV2(string tagName, Vector2[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			foreach (Vector2 vector in vctArr)
			{
				HalfVector2 val = new HalfVector2(vector);
				WriteVector(val);
			}
			return true;
		}

		public bool ExportData(string tagName, HalfVector2[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			foreach (HalfVector2 val in vctArr)
			{
				WriteVector(val);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="vctArray"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, Vector3[] vctArr)
		{
			if (vctArr == null)
			{
				return true;
			}
			WriteTag(tagName);
			m_writer.Write(vctArr.Length);
			foreach (Vector3 vct in vctArr)
			{
				WriteVector(vct);
			}
			return true;
		}

		public bool ExportData(string tagName, Vector3I[] vctArr)
		{
			if (vctArr == null)
			{
				return true;
			}
			WriteTag(tagName);
			m_writer.Write(vctArr.Length);
			foreach (Vector3I vct in vctArr)
			{
				WriteVector(vct);
			}
			return true;
		}

		public bool ExportData(string tagName, Vector4I[] vctArr)
		{
			if (vctArr == null)
			{
				return true;
			}
			WriteTag(tagName);
			m_writer.Write(vctArr.Length);
			foreach (Vector4I vct in vctArr)
			{
				WriteVector(vct);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="vctArray"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, Matrix[] matArr)
		{
			if (matArr == null)
			{
				return true;
			}
			WriteTag(tagName);
			m_writer.Write(matArr.Length);
			foreach (Matrix matrix in matArr)
			{
				WriteMatrix(matrix);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="vctArray"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, Vector2[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			foreach (Vector2 vct in vctArr)
			{
				WriteVector(vct);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="strArr"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, Vector4[] vctArr)
		{
			WriteTag(tagName);
			if (vctArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(vctArr.Length);
			foreach (Vector4 vct in vctArr)
			{
				WriteVector(vct);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="strArr"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, string[] strArr)
		{
			WriteTag(tagName);
			if (strArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(strArr.Length);
			foreach (string value in strArr)
			{
				m_writer.Write(value);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="strArr"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, int[] intArr)
		{
			WriteTag(tagName);
			if (intArr == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(intArr.Length);
			foreach (int value in intArr)
			{
				m_writer.Write(value);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="strArr"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, byte[] byteArray)
		{
			WriteTag(tagName);
			if (byteArray == null)
			{
				m_writer.Write(0);
				return true;
			}
			m_writer.Write(byteArray.Length);
			m_writer.Write(byteArray);
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="strArr"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, MyModelInfo modelInfo)
		{
			WriteTag(tagName);
			m_writer.Write(modelInfo.TrianglesCount);
			m_writer.Write(modelInfo.VerticesCount);
			WriteVector(modelInfo.BoundingBoxSize);
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="strArr"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, BoundingBox boundingBox)
		{
			WriteTag(tagName);
			WriteVector(boundingBox.Min);
			WriteVector(boundingBox.Max);
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="strArr"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, BoundingSphere boundingSphere)
		{
			WriteTag(tagName);
			WriteVector(boundingSphere.Center);
			m_writer.Write(boundingSphere.Radius);
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="dict"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, Dictionary<string, Matrix> dict)
		{
			WriteTag(tagName);
			m_writer.Write(dict.Count);
			foreach (KeyValuePair<string, Matrix> item in dict)
			{
				m_writer.Write(item.Key);
				WriteMatrix(item.Value);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="dict"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, List<MyMeshPartInfo> list)
		{
			WriteTag(tagName);
			m_writer.Write(list.Count);
			foreach (MyMeshPartInfo item in list)
			{
				item.Export(m_writer);
			}
			return true;
		}

		public bool ExportData(string tagName, List<MyMeshSectionInfo> list)
		{
			WriteTag(tagName);
			m_writer.Write(list.Count);
			foreach (MyMeshSectionInfo item in list)
			{
				item.Export(m_writer);
			}
			return true;
		}

		/// <summary>
		/// ExportData
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="dict"></param>
		/// <returns></returns>
		public bool ExportData(string tagName, Dictionary<string, MyModelDummy> dict)
		{
			WriteTag(tagName);
			m_writer.Write(dict.Count);
			foreach (KeyValuePair<string, MyModelDummy> item in dict)
			{
				m_writer.Write(item.Key);
				WriteMatrix(item.Value.Matrix);
				m_writer.Write(item.Value.CustomData.Count);
				foreach (KeyValuePair<string, object> customDatum in item.Value.CustomData)
				{
					m_writer.Write(customDatum.Key);
					m_writer.Write(customDatum.Value.ToString());
				}
			}
			return true;
		}

		/// <summary>
		/// ExportFloat
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool ExportFloat(string tagName, float value)
		{
			WriteTag(tagName);
			m_writer.Write(value);
			return true;
		}

		/// <summary>
		/// ExportFloat
		/// </summary>
		/// <param name="tagName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool ExportBool(string tagName, bool value)
		{
			WriteTag(tagName);
			m_writer.Write(value);
			return true;
		}

		protected void Write(MyAnimationClip clip)
		{
			m_writer.Write(clip.Name);
			m_writer.Write(clip.Duration);
			m_writer.Write(clip.Bones.Count);
			foreach (MyAnimationClip.Bone bone in clip.Bones)
			{
				m_writer.Write(bone.Name);
				m_writer.Write(bone.Keyframes.Count);
				foreach (MyAnimationClip.Keyframe keyframe in bone.Keyframes)
				{
					m_writer.Write(keyframe.Time);
					WriteQuaternion(keyframe.Rotation);
					WriteVector(keyframe.Translation);
				}
			}
		}

		/// <summary>
		/// WriteQuaternion
		/// </summary>
		/// <param name="vct"></param>
		private void WriteQuaternion(Quaternion q)
		{
			m_writer.Write(q.X);
			m_writer.Write(q.Y);
			m_writer.Write(q.Z);
			m_writer.Write(q.W);
		}

		public bool ExportData(string tagName, ModelAnimations modelAnimations)
		{
			WriteTag(tagName);
			m_writer.Write(modelAnimations.Clips.Count);
			foreach (MyAnimationClip clip in modelAnimations.Clips)
			{
				Write(clip);
			}
			m_writer.Write(modelAnimations.Skeleton.Count);
			foreach (int item in modelAnimations.Skeleton)
			{
				m_writer.Write(item);
			}
			return true;
		}

		public bool ExportData(string tagName, MyModelBone[] bones)
		{
			WriteTag(tagName);
			m_writer.Write(bones.Length);
			foreach (MyModelBone myModelBone in bones)
			{
				m_writer.Write(myModelBone.Name);
				m_writer.Write(myModelBone.Parent);
				WriteMatrix(myModelBone.Transform);
			}
			return true;
		}

		public bool ExportData(string tagName, MyLODDescriptor[] lodDescriptions)
		{
			WriteTag(tagName);
			m_writer.Write(lodDescriptions.Length);
			for (int i = 0; i < lodDescriptions.Length; i++)
			{
				lodDescriptions[i].Write(m_writer);
			}
			return true;
		}

		public void ExportData(string tagName, Md5.Hash hash)
		{
			WriteTag(tagName);
			m_writer.Write(hash.A);
			m_writer.Write(hash.B);
			m_writer.Write(hash.C);
			m_writer.Write(hash.D);
		}

		public void ExportData(string tagName, MyModelFractures modelFractures)
		{
			WriteTag(tagName);
			m_writer.Write(modelFractures.Version);
			m_writer.Write((modelFractures.Fractures != null) ? modelFractures.Fractures.Length : 0);
			MyFractureSettings[] fractures = modelFractures.Fractures;
			foreach (MyFractureSettings myFractureSettings in fractures)
			{
				if (myFractureSettings is RandomSplitFractureSettings)
				{
					RandomSplitFractureSettings randomSplitFractureSettings = (RandomSplitFractureSettings)myFractureSettings;
					m_writer.Write("RandomSplit");
					m_writer.Write(randomSplitFractureSettings.NumObjectsOnLevel1);
					m_writer.Write(randomSplitFractureSettings.NumObjectsOnLevel2);
					m_writer.Write(randomSplitFractureSettings.RandomRange);
					m_writer.Write(randomSplitFractureSettings.RandomSeed1);
					m_writer.Write(randomSplitFractureSettings.RandomSeed2);
					m_writer.Write(randomSplitFractureSettings.SplitPlane);
				}
				else if (myFractureSettings is VoronoiFractureSettings)
				{
					VoronoiFractureSettings voronoiFractureSettings = (VoronoiFractureSettings)myFractureSettings;
					m_writer.Write("Voronoi");
					m_writer.Write(voronoiFractureSettings.Seed);
					m_writer.Write(voronoiFractureSettings.NumSitesToGenerate);
					m_writer.Write(voronoiFractureSettings.NumIterations);
					m_writer.Write(voronoiFractureSettings.SplitPlane);
				}
				else if (myFractureSettings is WoodFractureSettings)
				{
					WoodFractureSettings woodFractureSettings = (WoodFractureSettings)myFractureSettings;
					m_writer.Write("WoodFracture");
					m_writer.Write(woodFractureSettings.BoardCustomSplittingPlaneAxis);
					m_writer.Write(woodFractureSettings.BoardFractureLineShearingRange);
					m_writer.Write(woodFractureSettings.BoardFractureNormalShearingRange);
					m_writer.Write(woodFractureSettings.BoardNumSubparts);
					m_writer.Write((int)woodFractureSettings.BoardRotateSplitGeom);
					WriteVector(woodFractureSettings.BoardScale);
					WriteVector(woodFractureSettings.BoardScaleRange);
					m_writer.Write(woodFractureSettings.BoardSplitGeomShiftRangeY);
					m_writer.Write(woodFractureSettings.BoardSplitGeomShiftRangeZ);
					WriteVector(woodFractureSettings.BoardSplittingAxis);
					m_writer.Write(woodFractureSettings.BoardSplittingPlane);
					m_writer.Write(woodFractureSettings.BoardSurfaceNormalShearingRange);
					m_writer.Write(woodFractureSettings.BoardWidthRange);
					m_writer.Write(woodFractureSettings.SplinterCustomSplittingPlaneAxis);
					m_writer.Write(woodFractureSettings.SplinterFractureLineShearingRange);
					m_writer.Write(woodFractureSettings.SplinterFractureNormalShearingRange);
					m_writer.Write(woodFractureSettings.SplinterNumSubparts);
					m_writer.Write((int)woodFractureSettings.SplinterRotateSplitGeom);
					WriteVector(woodFractureSettings.SplinterScale);
					WriteVector(woodFractureSettings.SplinterScaleRange);
					m_writer.Write(woodFractureSettings.SplinterSplitGeomShiftRangeY);
					m_writer.Write(woodFractureSettings.SplinterSplitGeomShiftRangeZ);
					WriteVector(woodFractureSettings.SplinterSplittingAxis);
					m_writer.Write(woodFractureSettings.SplinterSplittingPlane);
					m_writer.Write(woodFractureSettings.SplinterSurfaceNormalShearingRange);
					m_writer.Write(woodFractureSettings.SplinterWidthRange);
				}
			}
		}

		public static void ExportModelData(string filename, Dictionary<string, object> tagData)
		{
			using (MyModelExporter myModelExporter = new MyModelExporter(filename))
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				List<string> list = new List<string>((string[])tagData["Debug"]);
				list.RemoveAll((string x) => x.Contains("Version:"));
				list.Add("Version:01157001");
				myModelExporter.ExportData("Debug", list.ToArray());
				myModelExporter.StartCacheWrite();
				dictionary.Add("Dummies", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Dummies", (Dictionary<string, MyModelDummy>)tagData["Dummies"]);
				dictionary.Add("Vertices", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Vertices", (HalfVector4[])tagData["Vertices"]);
				dictionary.Add("Normals", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Normals", (Byte4[])tagData["Normals"]);
				dictionary.Add("TexCoords0", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("TexCoords0", (HalfVector2[])tagData["TexCoords0"]);
				dictionary.Add("Binormals", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Binormals", (Byte4[])tagData["Binormals"]);
				dictionary.Add("Tangents", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Tangents", (Byte4[])tagData["Tangents"]);
				dictionary.Add("TexCoords1", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("TexCoords1", (HalfVector2[])tagData["TexCoords1"]);
				dictionary.Add("RescaleFactor", myModelExporter.GetCachePosition());
				myModelExporter.ExportFloat("RescaleFactor", (float)tagData["RescaleFactor"]);
				dictionary.Add("UseChannelTextures", myModelExporter.GetCachePosition());
				myModelExporter.ExportBool("UseChannelTextures", (bool)tagData["UseChannelTextures"]);
				dictionary.Add("BoundingBox", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("BoundingBox", (BoundingBox)tagData["BoundingBox"]);
				dictionary.Add("BoundingSphere", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("BoundingSphere", (BoundingSphere)tagData["BoundingSphere"]);
				dictionary.Add("SwapWindingOrder", myModelExporter.GetCachePosition());
				myModelExporter.ExportBool("SwapWindingOrder", (bool)tagData["SwapWindingOrder"]);
				dictionary.Add("MeshParts", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("MeshParts", (List<MyMeshPartInfo>)tagData["MeshParts"]);
				dictionary.Add("Sections", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Sections", (List<MyMeshSectionInfo>)tagData["Sections"]);
				dictionary.Add("ModelBvh", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("ModelBvh", ((GImpactQuantizedBvh)tagData["ModelBvh"]).Save());
				dictionary.Add("ModelInfo", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("ModelInfo", (MyModelInfo)tagData["ModelInfo"]);
				dictionary.Add("BlendIndices", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("BlendIndices", (Vector4I[])tagData["BlendIndices"]);
				dictionary.Add("BlendWeights", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("BlendWeights", (Vector4[])tagData["BlendWeights"]);
				dictionary.Add("Animations", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Animations", (ModelAnimations)tagData["Animations"]);
				dictionary.Add("Bones", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("Bones", (MyModelBone[])tagData["Bones"]);
				dictionary.Add("BoneMapping", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("BoneMapping", (Vector3I[])tagData["BoneMapping"]);
				dictionary.Add("HavokCollisionGeometry", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("HavokCollisionGeometry", (byte[])tagData["HavokCollisionGeometry"]);
				dictionary.Add("PatternScale", myModelExporter.GetCachePosition());
				myModelExporter.ExportFloat("PatternScale", (float)tagData["PatternScale"]);
				dictionary.Add("LODs", myModelExporter.GetCachePosition());
				myModelExporter.ExportData("LODs", (MyLODDescriptor[])tagData["LODs"]);
				if (tagData.ContainsKey("FBXHash"))
				{
					dictionary.Add("FBXHash", myModelExporter.GetCachePosition());
					myModelExporter.ExportData("FBXHash", (Md5.Hash)tagData["FBXHash"]);
				}
				if (tagData.ContainsKey("HKTHash"))
				{
					dictionary.Add("HKTHash", myModelExporter.GetCachePosition());
					myModelExporter.ExportData("HKTHash", (Md5.Hash)tagData["HKTHash"]);
				}
				if (tagData.ContainsKey("XMLHash"))
				{
					dictionary.Add("XMLHash", myModelExporter.GetCachePosition());
					myModelExporter.ExportData("XMLHash", (Md5.Hash)tagData["XMLHash"]);
				}
				if (tagData.ContainsKey("ModelFractures"))
				{
					dictionary.Add("ModelFractures", myModelExporter.GetCachePosition());
					myModelExporter.ExportData("ModelFractures", (MyModelFractures)tagData["ModelFractures"]);
				}
				myModelExporter.StopCacheWrite();
				myModelExporter.WriteIndexDictionary(dictionary);
				myModelExporter.FlushCache();
			}
		}
	}
}
