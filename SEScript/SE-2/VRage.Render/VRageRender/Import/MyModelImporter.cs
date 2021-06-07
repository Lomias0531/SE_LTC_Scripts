using BulletXNA.BulletCollision;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.FileSystem;
using VRage.Security;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Animations;
using VRageRender.Fractures;

namespace VRageRender.Import
{
	public class MyModelImporter
	{
		private interface ITagReader
		{
			object Read(BinaryReader reader, int version);
		}

		private struct TagReader<T> : ITagReader
		{
			private Func<BinaryReader, int, T> m_tagReader;

			public TagReader(Func<BinaryReader, T> tagReader)
			{
				m_tagReader = ((BinaryReader x, int y) => tagReader(x));
			}

			public TagReader(Func<BinaryReader, int, T> tagReader)
			{
				m_tagReader = tagReader;
			}

			private T ReadTag(BinaryReader reader, int version)
			{
				return m_tagReader(reader, version);
			}

			public object Read(BinaryReader reader, int version)
			{
				return ReadTag(reader, version);
			}
		}

		public struct ReductionInfo
		{
			public string BoneName;

			public int OriginalKeys;

			public int OptimizedKeys;
		}

		private static Dictionary<string, ITagReader> TagReaders = new Dictionary<string, ITagReader>
		{
			{
				"Vertices",
				new TagReader<HalfVector4[]>(ReadArrayOfHalfVector4)
			},
			{
				"Normals",
				new TagReader<Byte4[]>(ReadArrayOfByte4)
			},
			{
				"TexCoords0",
				new TagReader<HalfVector2[]>(ReadArrayOfHalfVector2)
			},
			{
				"Binormals",
				new TagReader<Byte4[]>(ReadArrayOfByte4)
			},
			{
				"Tangents",
				new TagReader<Byte4[]>(ReadArrayOfByte4)
			},
			{
				"TexCoords1",
				new TagReader<HalfVector2[]>(ReadArrayOfHalfVector2)
			},
			{
				"UseChannelTextures",
				new TagReader<bool>((BinaryReader x) => x.ReadBoolean())
			},
			{
				"BoundingBox",
				new TagReader<BoundingBox>(ReadBoundingBox)
			},
			{
				"BoundingSphere",
				new TagReader<BoundingSphere>(ReadBoundingSphere)
			},
			{
				"RescaleFactor",
				new TagReader<float>((BinaryReader x) => x.ReadSingle())
			},
			{
				"SwapWindingOrder",
				new TagReader<bool>((BinaryReader x) => x.ReadBoolean())
			},
			{
				"Dummies",
				new TagReader<Dictionary<string, MyModelDummy>>(ReadDummies)
			},
			{
				"MeshParts",
				new TagReader<List<MyMeshPartInfo>>(ReadMeshParts)
			},
			{
				"Sections",
				new TagReader<List<MyMeshSectionInfo>>(ReadMeshSections)
			},
			{
				"ModelBvh",
				new TagReader<GImpactQuantizedBvh>(delegate(BinaryReader reader)
				{
					GImpactQuantizedBvh gImpactQuantizedBvh = new GImpactQuantizedBvh();
					gImpactQuantizedBvh.Load(ReadArrayOfBytes(reader));
					return gImpactQuantizedBvh;
				})
			},
			{
				"ModelInfo",
				new TagReader<MyModelInfo>(delegate(BinaryReader reader)
				{
					int triCnt = reader.ReadInt32();
					int vertCnt = reader.ReadInt32();
					Vector3 bBsize = ImportVector3(reader);
					return new MyModelInfo(triCnt, vertCnt, bBsize);
				})
			},
			{
				"BlendIndices",
				new TagReader<Vector4I[]>(ReadArrayOfVector4Int)
			},
			{
				"BlendWeights",
				new TagReader<Vector4[]>(ReadArrayOfVector4)
			},
			{
				"Animations",
				new TagReader<ModelAnimations>(ReadAnimations)
			},
			{
				"Bones",
				new TagReader<MyModelBone[]>(ReadBones)
			},
			{
				"BoneMapping",
				new TagReader<Vector3I[]>(ReadArrayOfVector3Int)
			},
			{
				"HavokCollisionGeometry",
				new TagReader<byte[]>(ReadArrayOfBytes)
			},
			{
				"PatternScale",
				new TagReader<float>((BinaryReader x) => x.ReadSingle())
			},
			{
				"LODs",
				new TagReader<MyLODDescriptor[]>(ReadLODs)
			},
			{
				"HavokDestructionGeometry",
				new TagReader<byte[]>(ReadArrayOfBytes)
			},
			{
				"HavokDestruction",
				new TagReader<byte[]>(ReadArrayOfBytes)
			},
			{
				"FBXHash",
				new TagReader<Md5.Hash>(ReadHash)
			},
			{
				"HKTHash",
				new TagReader<Md5.Hash>(ReadHash)
			},
			{
				"XMLHash",
				new TagReader<Md5.Hash>(ReadHash)
			},
			{
				"ModelFractures",
				new TagReader<MyModelFractures>(ReadModelFractures)
			}
		};

		private Dictionary<string, object> m_retTagData = new Dictionary<string, object>();

		private int m_version;

		private static string m_debugAssetName;

		public static bool USE_LINEAR_KEYFRAME_REDUCTION = true;

		public static bool LINEAR_KEYFRAME_REDUCTION_STATS = false;

		public static Dictionary<string, List<ReductionInfo>> ReductionStats = new Dictionary<string, List<ReductionInfo>>();

		private const float TinyLength = 1E-08f;

		private const float TinyCosAngle = 0.9999999f;

		public int DataVersion => m_version;

		public Dictionary<string, object> GetTagData()
		{
			return m_retTagData;
		}

		/// <summary>
		/// Read Vector34
		/// </summary>
		private static Vector3 ReadVector3(BinaryReader reader)
		{
			Vector3 result = default(Vector3);
			result.X = reader.ReadSingle();
			result.Y = reader.ReadSingle();
			result.Z = reader.ReadSingle();
			return result;
		}

		/// <summary>
		/// Read HalfVector4
		/// </summary>
		private static HalfVector4 ReadHalfVector4(BinaryReader reader)
		{
			HalfVector4 result = default(HalfVector4);
			result.PackedValue = reader.ReadUInt64();
			return result;
		}

		/// <summary>
		/// Read HalfVector2
		/// </summary>
		private static HalfVector2 ReadHalfVector2(BinaryReader reader)
		{
			HalfVector2 result = default(HalfVector2);
			result.PackedValue = reader.ReadUInt32();
			return result;
		}

		/// <summary>
		/// Read Byte4
		/// </summary>
		private static Byte4 ReadByte4(BinaryReader reader)
		{
			Byte4 result = default(Byte4);
			result.PackedValue = reader.ReadUInt32();
			return result;
		}

		/// <summary>
		/// ImportVector3
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Vector3 ImportVector3(BinaryReader reader)
		{
			Vector3 result = default(Vector3);
			result.X = reader.ReadSingle();
			result.Y = reader.ReadSingle();
			result.Z = reader.ReadSingle();
			return result;
		}

		/// <summary>
		/// ImportVector4
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Vector4 ImportVector4(BinaryReader reader)
		{
			Vector4 result = default(Vector4);
			result.X = reader.ReadSingle();
			result.Y = reader.ReadSingle();
			result.Z = reader.ReadSingle();
			result.W = reader.ReadSingle();
			return result;
		}

		/// <summary>
		/// ImportQuaternion
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Quaternion ImportQuaternion(BinaryReader reader)
		{
			Quaternion result = default(Quaternion);
			result.X = reader.ReadSingle();
			result.Y = reader.ReadSingle();
			result.Z = reader.ReadSingle();
			result.W = reader.ReadSingle();
			return result;
		}

		/// <summary>
		/// ImportVector4Int
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Vector4I ImportVector4Int(BinaryReader reader)
		{
			Vector4I result = default(Vector4I);
			result.X = reader.ReadInt32();
			result.Y = reader.ReadInt32();
			result.Z = reader.ReadInt32();
			result.W = reader.ReadInt32();
			return result;
		}

		/// <summary>
		/// ImportVector3Int
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Vector3I ImportVector3Int(BinaryReader reader)
		{
			Vector3I result = default(Vector3I);
			result.X = reader.ReadInt32();
			result.Y = reader.ReadInt32();
			result.Z = reader.ReadInt32();
			return result;
		}

		/// <summary>
		/// ImportVector2
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Vector2 ImportVector2(BinaryReader reader)
		{
			Vector2 result = default(Vector2);
			result.X = reader.ReadSingle();
			result.Y = reader.ReadSingle();
			return result;
		}

		/// <summary>
		/// Read array of HalfVector4
		/// </summary>
		private static HalfVector4[] ReadArrayOfHalfVector4(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			HalfVector4[] array = new HalfVector4[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadHalfVector4(reader);
			}
			return array;
		}

		/// <summary>
		/// Read array of Byte4
		/// </summary>
		private static Byte4[] ReadArrayOfByte4(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			Byte4[] array = new Byte4[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadByte4(reader);
			}
			return array;
		}

		/// <summary>
		/// Read array of HalfVector2
		/// </summary>
		private static HalfVector2[] ReadArrayOfHalfVector2(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			HalfVector2[] array = new HalfVector2[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ReadHalfVector2(reader);
			}
			return array;
		}

		/// <summary>
		/// ReadArrayOfVector3
		/// </summary>
		/// <param name="br"></param>
		/// <returns></returns>
		private static Vector3[] ReadArrayOfVector3(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			Vector3[] array = new Vector3[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ImportVector3(reader);
			}
			return array;
		}

		/// <summary>
		/// ReadArrayOfVector4
		/// </summary>
		/// <param name="br"></param>
		/// <returns></returns>
		private static Vector4[] ReadArrayOfVector4(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			Vector4[] array = new Vector4[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ImportVector4(reader);
			}
			return array;
		}

		/// <summary>
		/// ReadArrayOfVector4
		/// </summary>
		/// <param name="br"></param>
		/// <returns></returns>
		private static Vector4I[] ReadArrayOfVector4Int(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			Vector4I[] array = new Vector4I[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ImportVector4Int(reader);
			}
			return array;
		}

		/// <summary>
		/// ReadArrayOfVector3I
		/// </summary>
		/// <param name="br"></param>
		/// <returns></returns>
		private static Vector3I[] ReadArrayOfVector3Int(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			Vector3I[] array = new Vector3I[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ImportVector3Int(reader);
			}
			return array;
		}

		/// <summary>
		/// ReadArrayOfVector2
		/// </summary>
		/// <param name="br"></param>
		/// <returns></returns>
		private static Vector2[] ReadArrayOfVector2(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			Vector2[] array = new Vector2[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = ImportVector2(reader);
			}
			return array;
		}

		/// <summary>
		/// ReadArrayOfString
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static string[] ReadArrayOfString(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadString();
			}
			return array;
		}

		/// <summary>
		/// ReadBoundingBox
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static BoundingBox ReadBoundingBox(BinaryReader reader)
		{
			BoundingBox result = default(BoundingBox);
			result.Min = ImportVector3(reader);
			result.Max = ImportVector3(reader);
			return result;
		}

		/// <summary>
		/// ReadBoundingSphere
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static BoundingSphere ReadBoundingSphere(BinaryReader reader)
		{
			BoundingSphere result = default(BoundingSphere);
			result.Center = ImportVector3(reader);
			result.Radius = reader.ReadSingle();
			return result;
		}

		/// <summary>
		/// ReadMatrix
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Matrix ReadMatrix(BinaryReader reader)
		{
			Matrix result = default(Matrix);
			result.M11 = reader.ReadSingle();
			result.M12 = reader.ReadSingle();
			result.M13 = reader.ReadSingle();
			result.M14 = reader.ReadSingle();
			result.M21 = reader.ReadSingle();
			result.M22 = reader.ReadSingle();
			result.M23 = reader.ReadSingle();
			result.M24 = reader.ReadSingle();
			result.M31 = reader.ReadSingle();
			result.M32 = reader.ReadSingle();
			result.M33 = reader.ReadSingle();
			result.M34 = reader.ReadSingle();
			result.M41 = reader.ReadSingle();
			result.M42 = reader.ReadSingle();
			result.M43 = reader.ReadSingle();
			result.M44 = reader.ReadSingle();
			return result;
		}

		/// <summary>
		/// ReadMeshParts
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static List<MyMeshPartInfo> ReadMeshParts(BinaryReader reader, int version)
		{
			List<MyMeshPartInfo> list = new List<MyMeshPartInfo>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				MyMeshPartInfo myMeshPartInfo = new MyMeshPartInfo();
				myMeshPartInfo.Import(reader, version);
				list.Add(myMeshPartInfo);
			}
			return list;
		}

		private static List<MyMeshSectionInfo> ReadMeshSections(BinaryReader reader, int version)
		{
			List<MyMeshSectionInfo> list = new List<MyMeshSectionInfo>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				MyMeshSectionInfo myMeshSectionInfo = new MyMeshSectionInfo();
				myMeshSectionInfo.Import(reader, version);
				list.Add(myMeshSectionInfo);
			}
			return list;
		}

		/// <summary>
		/// ReadDummies
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static Dictionary<string, MyModelDummy> ReadDummies(BinaryReader reader)
		{
			Dictionary<string, MyModelDummy> dictionary = new Dictionary<string, MyModelDummy>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadString();
				Matrix matrix = ReadMatrix(reader);
				MyModelDummy myModelDummy = new MyModelDummy();
				myModelDummy.Name = text;
				myModelDummy.Matrix = matrix;
				myModelDummy.CustomData = new Dictionary<string, object>();
				int num2 = reader.ReadInt32();
				for (int j = 0; j < num2; j++)
				{
					string key = reader.ReadString();
					string value = reader.ReadString();
					myModelDummy.CustomData.Add(key, value);
				}
				dictionary.Add(text, myModelDummy);
			}
			return dictionary;
		}

		/// <summary>
		/// ReadArrayOfInt
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static int[] ReadArrayOfInt(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadInt32();
			}
			return array;
		}

		private static byte[] ReadArrayOfBytes(BinaryReader reader)
		{
			int count = reader.ReadInt32();
			return reader.ReadBytes(count);
		}

		private static Md5.Hash ReadHash(BinaryReader reader)
		{
			return new Md5.Hash
			{
				A = reader.ReadUInt32(),
				B = reader.ReadUInt32(),
				C = reader.ReadUInt32(),
				D = reader.ReadUInt32()
			};
		}

		private static MyAnimationClip ReadClip(BinaryReader reader)
		{
			MyAnimationClip myAnimationClip = new MyAnimationClip();
			myAnimationClip.Name = reader.ReadString();
			myAnimationClip.Duration = reader.ReadDouble();
			int num = reader.ReadInt32();
			while (num-- > 0)
			{
				MyAnimationClip.Bone bone = new MyAnimationClip.Bone();
				bone.Name = reader.ReadString();
				int num2 = reader.ReadInt32();
				while (num2-- > 0)
				{
					MyAnimationClip.Keyframe keyframe = new MyAnimationClip.Keyframe();
					keyframe.Time = reader.ReadDouble();
					keyframe.Rotation = ImportQuaternion(reader);
					keyframe.Translation = ImportVector3(reader);
					bone.Keyframes.Add(keyframe);
				}
				myAnimationClip.Bones.Add(bone);
				int count = bone.Keyframes.Count;
				int optimizedKeys = 0;
				if (count > 3)
				{
					if (USE_LINEAR_KEYFRAME_REDUCTION)
					{
						LinkedList<MyAnimationClip.Keyframe> linkedList = new LinkedList<MyAnimationClip.Keyframe>();
						foreach (MyAnimationClip.Keyframe keyframe2 in bone.Keyframes)
						{
							linkedList.AddLast(keyframe2);
						}
						LinearKeyframeReduction(linkedList, 1E-08f, 0.9999999f);
						bone.Keyframes.Clear();
						bone.Keyframes.AddRange(linkedList.ToArray());
						optimizedKeys = bone.Keyframes.Count;
					}
					if (LINEAR_KEYFRAME_REDUCTION_STATS)
					{
						ReductionInfo reductionInfo = default(ReductionInfo);
						reductionInfo.BoneName = bone.Name;
						reductionInfo.OriginalKeys = count;
						reductionInfo.OptimizedKeys = optimizedKeys;
						ReductionInfo item = reductionInfo;
						if (!ReductionStats.TryGetValue(m_debugAssetName, out List<ReductionInfo> value))
						{
							value = new List<ReductionInfo>();
							ReductionStats.Add(m_debugAssetName, value);
						}
						value.Add(item);
					}
				}
				CalculateKeyframeDeltas(bone.Keyframes);
			}
			return myAnimationClip;
		}

		private static void PercentageKeyframeReduction(LinkedList<MyAnimationClip.Keyframe> keyframes, float ratio)
		{
			if (keyframes.Count < 3)
			{
				return;
			}
			float num = 0f;
			int num2 = (int)((float)keyframes.Count * ratio);
			if (num2 == 0)
			{
				return;
			}
			float num3 = (float)num2 / (float)keyframes.Count;
			LinkedListNode<MyAnimationClip.Keyframe> linkedListNode = keyframes.First.Next;
			while (true)
			{
				LinkedListNode<MyAnimationClip.Keyframe> next = linkedListNode.Next;
				if (next == null)
				{
					break;
				}
				if (num >= 1f)
				{
					while (num >= 1f)
					{
						keyframes.Remove(linkedListNode);
						linkedListNode = next;
						next = linkedListNode.Next;
						num -= 1f;
					}
				}
				else
				{
					linkedListNode = next;
				}
				num += num3;
			}
		}

		/// <summary>
		/// This function filters out keyframes that can be approximated well with 
		/// linear interpolation.
		/// </summary>
		/// <param name="keyframes"></param>
		private static void LinearKeyframeReduction(LinkedList<MyAnimationClip.Keyframe> keyframes, float translationThreshold, float rotationThreshold)
		{
			if (keyframes.Count < 3)
			{
				return;
			}
			LinkedListNode<MyAnimationClip.Keyframe> linkedListNode = keyframes.First.Next;
			while (true)
			{
				LinkedListNode<MyAnimationClip.Keyframe> next = linkedListNode.Next;
				if (next != null)
				{
					MyAnimationClip.Keyframe value = linkedListNode.Previous.Value;
					MyAnimationClip.Keyframe value2 = linkedListNode.Value;
					MyAnimationClip.Keyframe value3 = next.Value;
					float amount = (float)((linkedListNode.Value.Time - linkedListNode.Previous.Value.Time) / (next.Value.Time - linkedListNode.Previous.Value.Time));
					Vector3 value4 = Vector3.Lerp(value.Translation, value3.Translation, amount);
					Quaternion quaternion = Quaternion.Slerp(value.Rotation, value3.Rotation, amount);
					if ((value4 - value2.Translation).LengthSquared() < translationThreshold && Quaternion.Dot(quaternion, value2.Rotation) > rotationThreshold)
					{
						keyframes.Remove(linkedListNode);
					}
					linkedListNode = next;
					continue;
				}
				break;
			}
		}

		private static void CalculateKeyframeDeltas(List<MyAnimationClip.Keyframe> keyframes)
		{
			for (int i = 1; i < keyframes.Count; i++)
			{
				MyAnimationClip.Keyframe keyframe = keyframes[i - 1];
				MyAnimationClip.Keyframe keyframe2 = keyframes[i];
				keyframe2.InvTimeDiff = 1.0 / (keyframe2.Time - keyframe.Time);
			}
		}

		private static ModelAnimations ReadAnimations(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			ModelAnimations modelAnimations = new ModelAnimations();
			while (num-- > 0)
			{
				MyAnimationClip item = ReadClip(reader);
				modelAnimations.Clips.Add(item);
			}
			int num2 = reader.ReadInt32();
			while (num2-- > 0)
			{
				int item2 = reader.ReadInt32();
				modelAnimations.Skeleton.Add(item2);
			}
			return modelAnimations;
		}

		private static MyModelBone[] ReadBones(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			MyModelBone[] array = new MyModelBone[num];
			int num2 = 0;
			while (num-- > 0)
			{
				MyModelBone myModelBone = array[num2] = new MyModelBone();
				myModelBone.Name = reader.ReadString();
				myModelBone.Index = num2++;
				myModelBone.Parent = reader.ReadInt32();
				myModelBone.Transform = ReadMatrix(reader);
			}
			return array;
		}

		private static MyLODDescriptor[] ReadLODs(BinaryReader reader, int version)
		{
			int num = reader.ReadInt32();
			MyLODDescriptor[] array = new MyLODDescriptor[num];
			int num2 = 0;
			while (num-- > 0)
			{
				MyLODDescriptor myLODDescriptor = new MyLODDescriptor();
				array[num2++] = myLODDescriptor;
				myLODDescriptor.Read(reader);
			}
			return array;
		}

		private static MyModelFractures ReadModelFractures(BinaryReader reader)
		{
			MyModelFractures myModelFractures = new MyModelFractures();
			myModelFractures.Version = reader.ReadInt32();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string a = reader.ReadString();
				if (a == "RandomSplit")
				{
					RandomSplitFractureSettings randomSplitFractureSettings = new RandomSplitFractureSettings();
					randomSplitFractureSettings.NumObjectsOnLevel1 = reader.ReadInt32();
					randomSplitFractureSettings.NumObjectsOnLevel2 = reader.ReadInt32();
					randomSplitFractureSettings.RandomRange = reader.ReadInt32();
					randomSplitFractureSettings.RandomSeed1 = reader.ReadInt32();
					randomSplitFractureSettings.RandomSeed2 = reader.ReadInt32();
					randomSplitFractureSettings.SplitPlane = reader.ReadString();
					myModelFractures.Fractures = new MyFractureSettings[1]
					{
						randomSplitFractureSettings
					};
				}
				else if (a == "Voronoi")
				{
					VoronoiFractureSettings voronoiFractureSettings = new VoronoiFractureSettings();
					voronoiFractureSettings.Seed = reader.ReadInt32();
					voronoiFractureSettings.NumSitesToGenerate = reader.ReadInt32();
					voronoiFractureSettings.NumIterations = reader.ReadInt32();
					voronoiFractureSettings.SplitPlane = reader.ReadString();
					myModelFractures.Fractures = new MyFractureSettings[1]
					{
						voronoiFractureSettings
					};
				}
				else if (a == "WoodFracture")
				{
					WoodFractureSettings woodFractureSettings = new WoodFractureSettings();
					woodFractureSettings.BoardCustomSplittingPlaneAxis = reader.ReadBoolean();
					woodFractureSettings.BoardFractureLineShearingRange = reader.ReadSingle();
					woodFractureSettings.BoardFractureNormalShearingRange = reader.ReadSingle();
					woodFractureSettings.BoardNumSubparts = reader.ReadInt32();
					woodFractureSettings.BoardRotateSplitGeom = (WoodFractureSettings.Rotation)reader.ReadInt32();
					woodFractureSettings.BoardScale = ReadVector3(reader);
					woodFractureSettings.BoardScaleRange = ReadVector3(reader);
					woodFractureSettings.BoardSplitGeomShiftRangeY = reader.ReadSingle();
					woodFractureSettings.BoardSplitGeomShiftRangeZ = reader.ReadSingle();
					woodFractureSettings.BoardSplittingAxis = ReadVector3(reader);
					woodFractureSettings.BoardSplittingPlane = reader.ReadString();
					woodFractureSettings.BoardSurfaceNormalShearingRange = reader.ReadSingle();
					woodFractureSettings.BoardWidthRange = reader.ReadSingle();
					woodFractureSettings.SplinterCustomSplittingPlaneAxis = reader.ReadBoolean();
					woodFractureSettings.SplinterFractureLineShearingRange = reader.ReadSingle();
					woodFractureSettings.SplinterFractureNormalShearingRange = reader.ReadSingle();
					woodFractureSettings.SplinterNumSubparts = reader.ReadInt32();
					woodFractureSettings.SplinterRotateSplitGeom = (WoodFractureSettings.Rotation)reader.ReadInt32();
					woodFractureSettings.SplinterScale = ReadVector3(reader);
					woodFractureSettings.SplinterScaleRange = ReadVector3(reader);
					woodFractureSettings.SplinterSplitGeomShiftRangeY = reader.ReadSingle();
					woodFractureSettings.SplinterSplitGeomShiftRangeZ = reader.ReadSingle();
					woodFractureSettings.SplinterSplittingAxis = ReadVector3(reader);
					woodFractureSettings.SplinterSplittingPlane = reader.ReadString();
					woodFractureSettings.SplinterSurfaceNormalShearingRange = reader.ReadSingle();
					woodFractureSettings.SplinterWidthRange = reader.ReadSingle();
					myModelFractures.Fractures = new MyFractureSettings[1]
					{
						woodFractureSettings
					};
				}
			}
			return myModelFractures;
		}

		public void ImportData(string assetFileName, string[] tags = null)
		{
			Clear();
			m_debugAssetName = assetFileName;
			using (Stream stream = MyFileSystem.OpenRead(Path.IsPathRooted(assetFileName) ? assetFileName : Path.Combine(MyFileSystem.ContentPath, assetFileName)))
			{
				if (stream != null)
				{
					byte[] array = null;
					try
					{
						Stream input = stream;
						if (stream.Length < 4194304)
						{
							int num = (int)stream.Length;
							array = ArrayPool<byte>.Shared.Rent(num);
							stream.Read(array, 0, num);
							input = new MemoryStream(array, 0, num, writable: false);
						}
						using (BinaryReader reader = new BinaryReader(input))
						{
							LoadTagData(reader, tags);
						}
					}
					finally
					{
						stream.Close();
						if (array != null)
						{
							ArrayPool<byte>.Shared.Return(array);
						}
					}
				}
			}
		}

		public void Clear()
		{
			m_retTagData.Clear();
			m_version = 0;
		}

		private void LoadTagData(BinaryReader reader, string[] tags)
		{
			string key = reader.ReadString();
			string[] array = ReadArrayOfString(reader);
			m_retTagData.Add(key, array);
			string text = "Version:";
			if (array.Length != 0 && array[0].Contains(text))
			{
				string value = array[0].Replace(text, "");
				m_version = Convert.ToInt32(value);
			}
			if (m_version >= 1066002)
			{
				Dictionary<string, int> dictionary = ReadIndexDictionary(reader);
				if (tags == null)
				{
					tags = dictionary.Keys.ToArray();
				}
				string[] array2 = tags;
				foreach (string key2 in array2)
				{
					if (dictionary.ContainsKey(key2))
					{
						int num = dictionary[key2];
						reader.BaseStream.Seek(num, SeekOrigin.Begin);
						reader.ReadString();
						if (TagReaders.ContainsKey(key2))
						{
							m_retTagData.Add(key2, TagReaders[key2].Read(reader, m_version));
						}
					}
				}
			}
			else
			{
				LoadOldVersion(reader);
			}
		}

		private Dictionary<string, int> ReadIndexDictionary(BinaryReader reader)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		private void LoadOldVersion(BinaryReader reader)
		{
			string key = reader.ReadString();
			Dictionary<string, MyModelDummy> value = ReadDummies(reader);
			m_retTagData.Add(key, value);
			key = reader.ReadString();
			HalfVector4[] value2 = ReadArrayOfHalfVector4(reader);
			m_retTagData.Add(key, value2);
			key = reader.ReadString();
			Byte4[] value3 = ReadArrayOfByte4(reader);
			m_retTagData.Add(key, value3);
			key = reader.ReadString();
			HalfVector2[] value4 = ReadArrayOfHalfVector2(reader);
			m_retTagData.Add(key, value4);
			key = reader.ReadString();
			value3 = ReadArrayOfByte4(reader);
			m_retTagData.Add(key, value3);
			key = reader.ReadString();
			value3 = ReadArrayOfByte4(reader);
			m_retTagData.Add(key, value3);
			key = reader.ReadString();
			value4 = ReadArrayOfHalfVector2(reader);
			m_retTagData.Add(key, value4);
			key = reader.ReadString();
			bool flag = reader.ReadBoolean();
			m_retTagData.Add(key, flag);
			key = reader.ReadString();
			float num = reader.ReadSingle();
			m_retTagData.Add(key, num);
			key = reader.ReadString();
			num = reader.ReadSingle();
			m_retTagData.Add(key, num);
			key = reader.ReadString();
			flag = reader.ReadBoolean();
			m_retTagData.Add(key, flag);
			key = reader.ReadString();
			flag = reader.ReadBoolean();
			m_retTagData.Add(key, flag);
			key = reader.ReadString();
			num = reader.ReadSingle();
			m_retTagData.Add(key, num);
			key = reader.ReadString();
			num = reader.ReadSingle();
			m_retTagData.Add(key, num);
			key = reader.ReadString();
			BoundingBox boundingBox = ReadBoundingBox(reader);
			m_retTagData.Add(key, boundingBox);
			key = reader.ReadString();
			BoundingSphere boundingSphere = ReadBoundingSphere(reader);
			m_retTagData.Add(key, boundingSphere);
			key = reader.ReadString();
			flag = reader.ReadBoolean();
			m_retTagData.Add(key, flag);
			key = reader.ReadString();
			List<MyMeshPartInfo> value5 = ReadMeshParts(reader, m_version);
			m_retTagData.Add(key, value5);
			key = reader.ReadString();
			GImpactQuantizedBvh gImpactQuantizedBvh = new GImpactQuantizedBvh();
			gImpactQuantizedBvh.Load(ReadArrayOfBytes(reader));
			m_retTagData.Add(key, gImpactQuantizedBvh);
			key = reader.ReadString();
			int triCnt = reader.ReadInt32();
			int vertCnt = reader.ReadInt32();
			Vector3 bBsize = ImportVector3(reader);
			m_retTagData.Add(key, new MyModelInfo(triCnt, vertCnt, bBsize));
			key = reader.ReadString();
			Vector4I[] value6 = ReadArrayOfVector4Int(reader);
			m_retTagData.Add(key, value6);
			key = reader.ReadString();
			Vector4[] value7 = ReadArrayOfVector4(reader);
			m_retTagData.Add(key, value7);
			key = reader.ReadString();
			ModelAnimations value8 = ReadAnimations(reader);
			m_retTagData.Add(key, value8);
			key = reader.ReadString();
			MyModelBone[] value9 = ReadBones(reader);
			m_retTagData.Add(key, value9);
			key = reader.ReadString();
			Vector3I[] value10 = ReadArrayOfVector3Int(reader);
			m_retTagData.Add(key, value10);
			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				key = reader.ReadString();
				byte[] value11 = ReadArrayOfBytes(reader);
				m_retTagData.Add(key, value11);
			}
			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				key = reader.ReadString();
				m_retTagData.Add(key, reader.ReadSingle());
			}
			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				key = reader.ReadString();
				m_retTagData.Add(key, ReadLODs(reader, 1066002));
			}
			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				key = reader.ReadString();
				byte[] value12 = ReadArrayOfBytes(reader);
				m_retTagData.Add(key, value12);
			}
			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				key = reader.ReadString();
				byte[] value13 = ReadArrayOfBytes(reader);
				m_retTagData.Add(key, value13);
			}
		}
	}
}
