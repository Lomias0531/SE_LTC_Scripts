using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VRage;
using VRage.FileSystem;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender.ExternalApp;
using VRageRender.Import;
using VRageRender.Messages;
using VRageRender.Voxels;

namespace VRageRender
{
	public static class MyRenderProxy
	{
		public enum MyStatsState
		{
			NoDraw,
			Last,
			SimpleTimingStats,
			ComplexTimingStats,
			Draw,
			MoveNext
		}

		public enum ObjectType
		{
			Entity,
			InstanceBuffer,
			Light,
			Video,
			DebugDrawMesh,
			ScreenDecal,
			Cloud,
			Atmosphere,
			GPUEmitter,
			ManualCull,
			Invalid,
			Max
		}

		private static readonly char[] m_invalidGeneratedTextureChars = new char[12]
		{
			'(',
			')',
			'<',
			'>',
			'|',
			'\\',
			'/',
			'\0',
			'\t',
			' ',
			'.',
			','
		};

		public static MyStatsState DrawRenderStats = MyStatsState.NoDraw;

		public static List<Vector3D> PointsForVoxelPrecache = new List<Vector3D>();

		private static bool m_settingsDirty = true;

		private static IMyRender m_render;

		public const uint RENDER_ID_UNASSIGNED = uint.MaxValue;

		public static MyRenderSettings Settings = MyRenderSettings.Default;

		public static MyRenderDebugOverrides DebugOverrides = new MyRenderDebugOverrides();

		public static MyMessagePool MessagePool = new MyMessagePool();

		public static Action WaitForFlushDelegate = null;

		public static bool LimitMaxQueueSize = false;

		public static bool EnableAppEventsCall = true;

		public static bool EnableAutoReenablingAsserts = true;

		private static readonly bool[] m_trackObjectType = new bool[11];

		private static readonly Dictionary<uint, ObjectType> m_objectTypes = new Dictionary<uint, ObjectType>();

		private static readonly HashSet<uint> m_objectsToRemove = new HashSet<uint>();

		public static float CPULoad;

		public static float CPULoadSmooth;

		public static float CPUTimeSmooth;

		public static float GPULoad;

		public static float GPULoadSmooth;

		public static float GPUTimeSmooth;

		private static ObjectType[] TYPE_ENTITY = new ObjectType[1];

		private static ObjectType[] TYPE_ENTITY_AND_CULL = new ObjectType[2]
		{
			ObjectType.Entity,
			ObjectType.ManualCull
		};

		public static MyRenderThread RenderThread
		{
			get;
			private set;
		}

		public static List<MyBillboard> BillboardsRead => m_render.SharedData.Billboards.Read.Billboards;

		public static List<MyBillboard> BillboardsWrite => m_render.SharedData.Billboards.Write.Billboards;

		public static Dictionary<int, MyBillboardViewProjection> BillboardsViewProjectionRead => m_render.SharedData.Billboards.Read.Matrices;

		public static Dictionary<int, MyBillboardViewProjection> BillboardsViewProjectionWrite => m_render.SharedData.Billboards.Write.Matrices;

		public static MyObjectsPool<MyBillboard> BillboardsPoolRead => m_render.SharedData.Billboards.Read.Pool;

		public static MyObjectsPool<MyBillboard> BillboardsPoolWrite => m_render.SharedData.Billboards.Write.Pool;

		public static MyObjectsPool<MyTriangleBillboard> TriangleBillboardsPoolRead => m_render.SharedData.TriangleBillboards.Read.Pool;

		public static MyObjectsPool<MyTriangleBillboard> TriangleBillboardsPoolWrite => m_render.SharedData.TriangleBillboards.Write.Pool;

		public static HashSet<uint> VisibleObjectsRead
		{
			get
			{
				if (m_render.SharedData == null)
				{
					return null;
				}
				return m_render.SharedData.VisibleObjects.Read;
			}
		}

		public static HashSet<uint> VisibleObjectsWrite
		{
			get
			{
				if (m_render.SharedData == null)
				{
					return null;
				}
				return m_render.SharedData.VisibleObjects.Write;
			}
		}

		public static MyTimeSpan CurrentDrawTime
		{
			get
			{
				return m_render.CurrentDrawTime;
			}
			set
			{
				m_render.CurrentDrawTime = value;
			}
		}

		public static MyViewport MainViewport => m_render.MainViewport;

		public static Vector2I BackBufferResolution => m_render.BackBufferResolution;

		public static MyLog Log => m_render.Log;

		public static bool IsInstantiated => m_render != null;

		public static bool SettingsDirty => m_settingsDirty;

		public static MyMessageQueue OutputQueue => m_render.OutputQueue;

		public static FrameProcessStatusEnum FrameProcessStatus => m_render.FrameProcessStatus;

		public static int PersistentBillboardsCount => m_render.SharedData.PersistentBillboardsCount;

		public static MyRenderDeviceSettings CreateDevice(MyRenderThread renderThread, MyRenderDeviceSettings? settingsToTry, out MyAdapterInfo[] adaptersList)
		{
			RenderThread = renderThread;
			return m_render.CreateDevice(settingsToTry, out adaptersList);
		}

		public static void DisposeDevice()
		{
			if (m_render != null)
			{
				m_render.DisposeDevice();
			}
			m_render = new MyNullRender();
			RenderThread = null;
		}

		public static long GetAvailableTextureMemory()
		{
			return m_render.GetAvailableTextureMemory();
		}

		public static bool SettingsChanged(MyRenderDeviceSettings settings)
		{
			return m_render.SettingsChanged(settings);
		}

		public static void ApplySettings(MyRenderDeviceSettings settings)
		{
			m_render.ApplySettings(settings);
		}

		public static void Present()
		{
			m_render.Present();
		}

		public static string RendererInterfaceName()
		{
			return m_render.ToString();
		}

		[Conditional("DEBUG")]
		public static void AssertRenderThread()
		{
		}

		private static void EnqueueMessage(MyRenderMessageBase message)
		{
			m_render.EnqueueMessage(message, LimitMaxQueueSize);
		}

		public static void BeforeRender(MyTimeSpan? currentDrawTime)
		{
			m_render.SharedData?.BeforeRender(currentDrawTime);
		}

		public static void AfterRender()
		{
			m_render.SharedData?.AfterRender();
		}

		public static void BeforeUpdate()
		{
			m_render.SharedData?.BeforeUpdate();
		}

		public static void AfterUpdate(MyTimeSpan? updateTimestamp)
		{
			m_render.SharedData?.AfterUpdate(updateTimestamp);
		}

		public static void ProcessMessages()
		{
			m_render.Draw(draw: false);
		}

		public static void Draw()
		{
			m_render.Draw();
		}

		public static string GetLastExecutedAnnotation()
		{
			return m_render.GetLastExecutedAnnotation();
		}

		public static void Ansel_DrawScene()
		{
			m_render.Ansel_DrawScene();
		}

		public static MyRenderProfiler GetRenderProfiler()
		{
			return m_render.GetRenderProfiler();
		}

		public static ObjectType GetObjectType(uint GID)
		{
			lock (m_objectTypes)
			{
				if (m_objectTypes.TryGetValue(GID, out ObjectType value))
				{
					return value;
				}
				return ObjectType.Invalid;
			}
		}

		private static void TrackNewMessageId(ObjectType type)
		{
			if (m_trackObjectType[(int)type])
			{
				m_objectTypes.Add(m_render.GlobalMessageCounter, type);
			}
		}

		private static uint GetMessageId(ObjectType type, bool track = true)
		{
			lock (m_objectTypes)
			{
				if (track)
				{
					TrackNewMessageId(type);
				}
				return m_render.GlobalMessageCounter++;
			}
		}

		public static void SetSettingsDirty()
		{
			m_settingsDirty = true;
		}

		public static uint AllocateObjectId(ObjectType type, bool track = true)
		{
			return GetMessageId(type, track);
		}

		public static void RemoveMessageId(uint GID, ObjectType type, bool now = true)
		{
			if (m_trackObjectType[(int)type])
			{
				lock (m_objectTypes)
				{
					ObjectType objectType = GetObjectType(GID);
					if (objectType != ObjectType.Invalid && m_objectTypes[GID] == type)
					{
						if (now)
						{
							m_objectTypes.Remove(GID);
							m_objectsToRemove.Remove(GID);
						}
						else
						{
							m_objectsToRemove.Add(GID);
						}
					}
					else
					{
						MyLog.Default.Error("Invalid object type Expected:{0} Real:{1} \n{2}", type.ToString(), objectType.ToString(), Environment.StackTrace);
					}
				}
			}
		}

		[Conditional("DEBUG")]
		public static void CheckMessageId(uint GID, ObjectType[] allowedTypes = null)
		{
			ObjectType objectType = GetObjectType(GID);
			if (objectType != ObjectType.Invalid && allowedTypes != null)
			{
				for (int i = 0; i < allowedTypes.Length && allowedTypes[i] != objectType; i++)
				{
				}
			}
		}

		public static void Initialize(IMyRender render)
		{
			for (int i = 0; i < 11; i++)
			{
				m_trackObjectType[i] = true;
			}
			m_trackObjectType[5] = false;
			m_render = render;
			UpdateDebugOverrides();
			ProfilerShort.SetProfiler(render.GetRenderProfiler());
		}

		public static void UnloadContent()
		{
			ClearLargeMessages();
		}

		public static void ClearLargeMessages()
		{
			MessagePool.Clear(MyRenderMessageEnum.CreateRenderInstanceBuffer);
			MessagePool.Clear(MyRenderMessageEnum.UpdateRenderCubeInstanceBuffer);
		}

		public static void UnloadData()
		{
			ClearLargeMessages();
			EnqueueMessage(MessagePool.Get<MyRenderMessageUnloadData>(MyRenderMessageEnum.UnloadData));
		}

		private static void CheckRenderObjectIds()
		{
			lock (m_objectTypes)
			{
			}
		}

		public static void SetGlobalValues(string rootDirectory, string rootDirectoryEffects, string rootDirectoryDebug)
		{
			m_render.RootDirectory = rootDirectory;
			m_render.RootDirectoryEffects = rootDirectoryEffects;
			m_render.RootDirectoryDebug = rootDirectoryDebug;
		}

		public static void GenerateShaderCache(bool clean, bool fastBuild, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
			m_render.GenerateShaderCache(clean, fastBuild, onShaderCacheProgress);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="texture">Texture name (path).</param>
		/// <param name="destination">Screen coord destination.</param>
		/// <param name="sourceRectangle">Source rectangle in texture coordinates</param>
		/// <param name="color"></param>
		/// <param name="rotation">NOT USED</param>
		/// <param name="waitTillLoaded"></param>
		/// <param name="targetTexture">NOT USED</param>
		/// <param name="maskTexture"></param>
		/// <param name="rotSpeed"></param>
		public static void DrawSprite(string texture, ref RectangleF destination, Rectangle? sourceRectangle, Color color, float rotation, bool waitTillLoaded = true, string targetTexture = null, string maskTexture = null, float rotSpeed = 0f)
		{
			MyRenderMessageDrawSprite myRenderMessageDrawSprite = MessagePool.Get<MyRenderMessageDrawSprite>(MyRenderMessageEnum.DrawSprite);
			myRenderMessageDrawSprite.Texture = texture;
			myRenderMessageDrawSprite.DestinationRectangle = destination;
			myRenderMessageDrawSprite.SourceRectangle = sourceRectangle;
			myRenderMessageDrawSprite.Color = color;
			myRenderMessageDrawSprite.WaitTillLoaded = waitTillLoaded;
			myRenderMessageDrawSprite.TargetTexture = targetTexture;
			myRenderMessageDrawSprite.MaskTexture = maskTexture;
			myRenderMessageDrawSprite.RotationSpeed = rotSpeed;
			myRenderMessageDrawSprite.Rotation = rotation;
			EnqueueMessage(myRenderMessageDrawSprite);
		}

		public static void DrawSpriteExt(string texture, ref RectangleF destination, Rectangle? sourceRectangle, Color color, ref Vector2 rightVector, ref Vector2 origin, bool waitTillLoaded = true, string targetTexture = null, string maskTexture = null)
		{
			MyRenderMessageDrawSpriteExt myRenderMessageDrawSpriteExt = MessagePool.Get<MyRenderMessageDrawSpriteExt>(MyRenderMessageEnum.DrawSpriteExt);
			myRenderMessageDrawSpriteExt.Texture = texture;
			myRenderMessageDrawSpriteExt.DestinationRectangle = destination;
			myRenderMessageDrawSpriteExt.SourceRectangle = sourceRectangle;
			myRenderMessageDrawSpriteExt.Color = color;
			myRenderMessageDrawSpriteExt.WaitTillLoaded = waitTillLoaded;
			myRenderMessageDrawSpriteExt.TargetTexture = targetTexture;
			myRenderMessageDrawSpriteExt.MaskTexture = maskTexture;
			myRenderMessageDrawSpriteExt.RightVector = rightVector;
			myRenderMessageDrawSpriteExt.Origin = origin;
			EnqueueMessage(myRenderMessageDrawSpriteExt);
		}

		public static void DrawSpriteAtlas(string texture, Vector2 position, Vector2 textureOffset, Vector2 textureSize, Vector2 rightVector, Vector2 scale, Color color, Vector2 halfSize, string targetTexture = null)
		{
			MyRenderMessageDrawSpriteAtlas myRenderMessageDrawSpriteAtlas = MessagePool.Get<MyRenderMessageDrawSpriteAtlas>(MyRenderMessageEnum.DrawSpriteAtlas);
			myRenderMessageDrawSpriteAtlas.Texture = texture;
			myRenderMessageDrawSpriteAtlas.Position = position;
			myRenderMessageDrawSpriteAtlas.TextureOffset = textureOffset;
			myRenderMessageDrawSpriteAtlas.TextureSize = textureSize;
			myRenderMessageDrawSpriteAtlas.RightVector = rightVector;
			myRenderMessageDrawSpriteAtlas.Scale = scale;
			myRenderMessageDrawSpriteAtlas.Color = color;
			myRenderMessageDrawSpriteAtlas.HalfSize = halfSize;
			myRenderMessageDrawSpriteAtlas.TargetTexture = targetTexture;
			EnqueueMessage(myRenderMessageDrawSpriteAtlas);
		}

		public static void SpriteScissorPop(string targetTexture = null)
		{
			MyRenderMessageSpriteScissorPop myRenderMessageSpriteScissorPop = MessagePool.Get<MyRenderMessageSpriteScissorPop>(MyRenderMessageEnum.SpriteScissorPop);
			EnqueueMessage(myRenderMessageSpriteScissorPop);
			myRenderMessageSpriteScissorPop.TargetTexture = targetTexture;
		}

		public static void SpriteScissorPush(Rectangle screenRectangle, string targetTexture = null)
		{
			MyRenderMessageSpriteScissorPush myRenderMessageSpriteScissorPush = MessagePool.Get<MyRenderMessageSpriteScissorPush>(MyRenderMessageEnum.SpriteScissorPush);
			myRenderMessageSpriteScissorPush.ScreenRectangle = screenRectangle;
			myRenderMessageSpriteScissorPush.TargetTexture = targetTexture;
			EnqueueMessage(myRenderMessageSpriteScissorPush);
		}

		public static void CreateFont(int fontId, string fontPath, bool isDebugFont = false, string targetTexture = null, Color? colorMask = null)
		{
			MyRenderMessageCreateFont myRenderMessageCreateFont = MessagePool.Get<MyRenderMessageCreateFont>(MyRenderMessageEnum.CreateFont);
			myRenderMessageCreateFont.FontId = fontId;
			myRenderMessageCreateFont.FontPath = fontPath;
			myRenderMessageCreateFont.IsDebugFont = isDebugFont;
			myRenderMessageCreateFont.ColorMask = colorMask;
			EnqueueMessage(myRenderMessageCreateFont);
		}

		public static void DrawString(int fontIndex, Vector2 screenCoord, Color colorMask, string text, float screenScale, float screenMaxWidth, string targetTexture = null)
		{
			MyRenderMessageDrawString myRenderMessageDrawString = MessagePool.Get<MyRenderMessageDrawString>(MyRenderMessageEnum.DrawString);
			myRenderMessageDrawString.Text = text;
			myRenderMessageDrawString.FontIndex = fontIndex;
			myRenderMessageDrawString.ScreenCoord = screenCoord;
			myRenderMessageDrawString.ColorMask = colorMask;
			myRenderMessageDrawString.ScreenScale = screenScale;
			myRenderMessageDrawString.ScreenMaxWidth = screenMaxWidth;
			myRenderMessageDrawString.TargetTexture = targetTexture;
			EnqueueMessage(myRenderMessageDrawString);
		}

		public static void DrawStringAligned(int fontIndex, Vector2 screenCoord, Color colorMask, string text, float screenScale, float screenMaxWidth, string targetTexture = null, int textureWidthinPx = 512, MyRenderTextAlignmentEnum align = MyRenderTextAlignmentEnum.Align_Left)
		{
			MyRenderMessageDrawStringAligned myRenderMessageDrawStringAligned = MessagePool.Get<MyRenderMessageDrawStringAligned>(MyRenderMessageEnum.DrawStringAligned);
			myRenderMessageDrawStringAligned.Text = text;
			myRenderMessageDrawStringAligned.FontIndex = fontIndex;
			myRenderMessageDrawStringAligned.ScreenCoord = screenCoord;
			myRenderMessageDrawStringAligned.ColorMask = colorMask;
			myRenderMessageDrawStringAligned.ScreenScale = screenScale;
			myRenderMessageDrawStringAligned.ScreenMaxWidth = screenMaxWidth;
			myRenderMessageDrawStringAligned.TargetTexture = targetTexture;
			myRenderMessageDrawStringAligned.TextureWidthInPx = textureWidthinPx;
			myRenderMessageDrawStringAligned.Alignment = align;
			EnqueueMessage(myRenderMessageDrawStringAligned);
		}

		public static void PreloadTextures(List<string> texturesToLoad, TextureType textureType)
		{
			for (int i = 0; i < texturesToLoad.Count; i++)
			{
				string text = texturesToLoad[i];
				if (text.Contains(MyFileSystem.ContentPath))
				{
					text = text.Remove(0, MyFileSystem.ContentPath.Length);
					text = (texturesToLoad[i] = text.TrimStart(new char[1]
					{
						Path.DirectorySeparatorChar
					}));
				}
			}
			MyRenderMessagePreloadTextures myRenderMessagePreloadTextures = MessagePool.Get<MyRenderMessagePreloadTextures>(MyRenderMessageEnum.PreloadTextures);
			myRenderMessagePreloadTextures.TextureType = textureType;
			myRenderMessagePreloadTextures.Files = new List<string>(texturesToLoad);
			EnqueueMessage(myRenderMessagePreloadTextures);
		}

		public static void AddToParticleTextureArray(HashSet<string> textures)
		{
			MyRenderMessageAddToParticleTextureArray myRenderMessageAddToParticleTextureArray = MessagePool.Get<MyRenderMessageAddToParticleTextureArray>(MyRenderMessageEnum.AddToParticleTextureArray);
			myRenderMessageAddToParticleTextureArray.Files = textures;
			EnqueueMessage(myRenderMessageAddToParticleTextureArray);
		}

		public static void UnloadTexture(string textureName)
		{
			MyRenderMessageUnloadTexture myRenderMessageUnloadTexture = MessagePool.Get<MyRenderMessageUnloadTexture>(MyRenderMessageEnum.UnloadTexture);
			myRenderMessageUnloadTexture.Texture = textureName;
			EnqueueMessage(myRenderMessageUnloadTexture);
		}

		/// <returns>True if the texture name is valid and doesn't contant reserved characters</returns>
		public static bool IsValidGeneratedTextureName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			return name.IndexOfAny(m_invalidGeneratedTextureChars) < 0;
		}

		public static void CheckValidGeneratedTextureName(string name)
		{
			if (!IsValidGeneratedTextureName(name))
			{
				throw new Exception("Generated texture must not contain any of the following characters: '(', ')','<', '>', '|', '\\', '/', '\0', '\t', ' ', '.', ','");
			}
		}

		/// <returns>Qualified texture</returns>
		public static void CreateGeneratedTexture(string textureName, int width, int height, MyGeneratedTextureType type = MyGeneratedTextureType.RGBA, int numMiplevels = 1, byte[] data = null, bool generateMipmaps = true)
		{
			CheckValidGeneratedTextureName(textureName);
			if (numMiplevels <= 0)
			{
				throw new ArgumentOutOfRangeException("numMiplevels");
			}
			MyRenderMessageCreateGeneratedTexture myRenderMessageCreateGeneratedTexture = MessagePool.Get<MyRenderMessageCreateGeneratedTexture>(MyRenderMessageEnum.CreateGeneratedTexture);
			myRenderMessageCreateGeneratedTexture.TextureName = textureName;
			myRenderMessageCreateGeneratedTexture.Width = width;
			myRenderMessageCreateGeneratedTexture.Height = height;
			myRenderMessageCreateGeneratedTexture.Type = type;
			myRenderMessageCreateGeneratedTexture.Data = data;
			myRenderMessageCreateGeneratedTexture.GenerateMipMaps = generateMipmaps;
			EnqueueMessage(myRenderMessageCreateGeneratedTexture);
		}

		public static void ResetGeneratedTexture(string textureName, byte[] data)
		{
			MyRenderMessageResetGeneratedTexture myRenderMessageResetGeneratedTexture = MessagePool.Get<MyRenderMessageResetGeneratedTexture>(MyRenderMessageEnum.ResetGeneratedTexture);
			myRenderMessageResetGeneratedTexture.TextureName = textureName;
			myRenderMessageResetGeneratedTexture.Data = data;
			EnqueueMessage(myRenderMessageResetGeneratedTexture);
		}

		public static void PrintAllFileTexturesIntoLog()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageDebugPrintAllFileTexturesIntoLog>(MyRenderMessageEnum.DebugPrintAllFileTexturesIntoLog));
		}

		public static void RenderProfilerInput(RenderProfilerCommand command, int index, string value)
		{
			MyRenderMessageRenderProfiler myRenderMessageRenderProfiler = MessagePool.Get<MyRenderMessageRenderProfiler>(MyRenderMessageEnum.RenderProfiler);
			myRenderMessageRenderProfiler.Command = command;
			myRenderMessageRenderProfiler.Index = index;
			myRenderMessageRenderProfiler.Value = value;
			EnqueueMessage(myRenderMessageRenderProfiler);
		}

		public static uint CreateRenderEntityCloudLayer(uint atmosphereId, string debugName, string model, List<string> textures, Vector3D centerPoint, double altitude, double minScaledAltitude, bool scalingEnabled, double fadeOutRelativeAltitudeStart, double fadeOutRelativeAltitudeEnd, float applyFogRelativeDistance, double maxPlanetHillRadius, Vector3D rotationAxis, float angularVelocity, float initialRotation, Vector4 color, bool fadeIn)
		{
			MyRenderMessageCreateRenderEntityClouds myRenderMessageCreateRenderEntityClouds = MessagePool.Get<MyRenderMessageCreateRenderEntityClouds>(MyRenderMessageEnum.CreateRenderEntityClouds);
			uint messageId = GetMessageId(ObjectType.Cloud);
			myRenderMessageCreateRenderEntityClouds.Settings = new MyCloudLayerSettingsRender
			{
				ID = messageId,
				AtmosphereID = atmosphereId,
				Model = model,
				Textures = textures,
				CenterPoint = centerPoint,
				Altitude = altitude,
				MinScaledAltitude = minScaledAltitude,
				ScalingEnabled = scalingEnabled,
				DebugName = debugName,
				RotationAxis = rotationAxis,
				AngularVelocity = angularVelocity,
				InitialRotation = initialRotation,
				MaxPlanetHillRadius = maxPlanetHillRadius,
				FadeOutRelativeAltitudeStart = fadeOutRelativeAltitudeStart,
				FadeOutRelativeAltitudeEnd = fadeOutRelativeAltitudeEnd,
				ApplyFogRelativeDistance = applyFogRelativeDistance,
				Color = color,
				FadeIn = fadeIn
			};
			EnqueueMessage(myRenderMessageCreateRenderEntityClouds);
			return messageId;
		}

		public static uint CreateRenderEntityAtmosphere(string debugName, string model, MatrixD worldMatrix, MyMeshDrawTechnique technique, RenderFlags flags, CullingOptions cullingOptions, float atmosphereRadius, float planetRadius, Vector3 atmosphereWavelengths, float dithering = 0f, float maxViewDistance = float.MaxValue, bool fadeIn = false)
		{
			MyRenderMessageCreateRenderEntityAtmosphere myRenderMessageCreateRenderEntityAtmosphere = MessagePool.Get<MyRenderMessageCreateRenderEntityAtmosphere>(MyRenderMessageEnum.CreateRenderEntityAtmosphere);
			uint result = myRenderMessageCreateRenderEntityAtmosphere.ID = GetMessageId(ObjectType.Atmosphere);
			myRenderMessageCreateRenderEntityAtmosphere.DebugName = debugName;
			myRenderMessageCreateRenderEntityAtmosphere.Model = model;
			myRenderMessageCreateRenderEntityAtmosphere.WorldMatrix = worldMatrix;
			myRenderMessageCreateRenderEntityAtmosphere.Technique = technique;
			myRenderMessageCreateRenderEntityAtmosphere.Flags = flags;
			myRenderMessageCreateRenderEntityAtmosphere.CullingOptions = cullingOptions;
			myRenderMessageCreateRenderEntityAtmosphere.MaxViewDistance = maxViewDistance;
			myRenderMessageCreateRenderEntityAtmosphere.AtmosphereRadius = atmosphereRadius;
			myRenderMessageCreateRenderEntityAtmosphere.PlanetRadius = planetRadius;
			myRenderMessageCreateRenderEntityAtmosphere.AtmosphereWavelengths = atmosphereWavelengths;
			myRenderMessageCreateRenderEntityAtmosphere.FadeIn = fadeIn;
			EnqueueMessage(myRenderMessageCreateRenderEntityAtmosphere);
			return result;
		}

		public static uint CreateRenderEntity(string debugName, string model, MatrixD worldMatrix, MyMeshDrawTechnique technique, RenderFlags flags, CullingOptions cullingOptions, Color diffuseColor, Vector3 colorMaskHsv, float dithering = 0f, float maxViewDistance = float.MaxValue, byte depthBias = 0, float rescale = 1f, bool fadeIn = false)
		{
			MyRenderMessageCreateRenderEntity myRenderMessageCreateRenderEntity = MessagePool.Get<MyRenderMessageCreateRenderEntity>(MyRenderMessageEnum.CreateRenderEntity);
			uint num = myRenderMessageCreateRenderEntity.ID = GetMessageId(ObjectType.Entity);
			myRenderMessageCreateRenderEntity.DebugName = debugName;
			myRenderMessageCreateRenderEntity.Model = model;
			myRenderMessageCreateRenderEntity.WorldMatrix = worldMatrix;
			myRenderMessageCreateRenderEntity.Technique = technique;
			myRenderMessageCreateRenderEntity.Flags = flags;
			myRenderMessageCreateRenderEntity.CullingOptions = cullingOptions;
			myRenderMessageCreateRenderEntity.MaxViewDistance = maxViewDistance;
			myRenderMessageCreateRenderEntity.Rescale = rescale;
			myRenderMessageCreateRenderEntity.DepthBias = depthBias;
			EnqueueMessage(myRenderMessageCreateRenderEntity);
			UpdateRenderEntity(num, diffuseColor, colorMaskHsv, dithering, fadeIn);
			return num;
		}

		public static uint CreateLineBasedObject(string colorMetalTexture, string normalGlossTexture, string extensionTexture, string debugName)
		{
			MyRenderMessageCreateLineBasedObject myRenderMessageCreateLineBasedObject = MessagePool.Get<MyRenderMessageCreateLineBasedObject>(MyRenderMessageEnum.CreateLineBasedObject);
			uint result = myRenderMessageCreateLineBasedObject.ID = GetMessageId(ObjectType.Entity);
			myRenderMessageCreateLineBasedObject.ColorMetalTexture = colorMetalTexture;
			myRenderMessageCreateLineBasedObject.NormalGlossTexture = normalGlossTexture;
			myRenderMessageCreateLineBasedObject.ExtensionTexture = extensionTexture;
			myRenderMessageCreateLineBasedObject.DebugName = debugName;
			EnqueueMessage(myRenderMessageCreateLineBasedObject);
			return result;
		}

		public static MyRenderMessageSetRenderEntityData PrepareSetRenderEntityData()
		{
			MyRenderMessageSetRenderEntityData myRenderMessageSetRenderEntityData = MessagePool.Get<MyRenderMessageSetRenderEntityData>(MyRenderMessageEnum.SetRenderEntityData);
			myRenderMessageSetRenderEntityData.ModelData.Clear();
			return myRenderMessageSetRenderEntityData;
		}

		public static void SetRenderEntityData(uint renderObjectId, MyRenderMessageSetRenderEntityData message)
		{
			message.ID = renderObjectId;
			EnqueueMessage(message);
		}

		public static MyRenderMessageAddRuntimeModel PrepareAddRuntimeModel()
		{
			MyRenderMessageAddRuntimeModel myRenderMessageAddRuntimeModel = MessagePool.Get<MyRenderMessageAddRuntimeModel>(MyRenderMessageEnum.AddRuntimeModel);
			myRenderMessageAddRuntimeModel.ModelData.Clear();
			return myRenderMessageAddRuntimeModel;
		}

		public static void PreloadModel(string name, float rescale = 1f, bool forceOldPipeline = false)
		{
			MyRenderMessagePreloadModel myRenderMessagePreloadModel = MessagePool.Get<MyRenderMessagePreloadModel>(MyRenderMessageEnum.PreloadModel);
			myRenderMessagePreloadModel.Name = name;
			myRenderMessagePreloadModel.Rescale = rescale;
			myRenderMessagePreloadModel.ForceOldPipeline = forceOldPipeline;
			EnqueueMessage(myRenderMessagePreloadModel);
		}

		/// <summary>
		/// Implements message to render to preload models with higher priority than the others
		/// </summary>
		/// <param name="models"></param>
		public static void PreloadModels(List<string> models, bool forInstancedComponent)
		{
			MyRenderMessagePreloadModels myRenderMessagePreloadModels = MessagePool.Get<MyRenderMessagePreloadModels>(MyRenderMessageEnum.PreloadModels);
			myRenderMessagePreloadModels.Models = models;
			myRenderMessagePreloadModels.ForInstancedComponent = forInstancedComponent;
			EnqueueMessage(myRenderMessagePreloadModels);
		}

		public static void PreloadMaterials(string name)
		{
			MyRenderMessagePreloadMaterials myRenderMessagePreloadMaterials = MessagePool.Get<MyRenderMessagePreloadMaterials>(MyRenderMessageEnum.PreloadMaterials);
			myRenderMessagePreloadMaterials.Name = name;
			EnqueueMessage(myRenderMessagePreloadMaterials);
		}

		public static void AddRuntimeModel(string name, MyRenderMessageAddRuntimeModel message)
		{
			message.Name = name;
			EnqueueMessage(message);
		}

		/// <param name="parentId">Parent of the instance. Currently used for debugging. May be left unassingned</param>
		public static uint CreateRenderInstanceBuffer(string debugName, MyRenderInstanceBufferType type, uint parentId = uint.MaxValue)
		{
			MyRenderMessageCreateRenderInstanceBuffer myRenderMessageCreateRenderInstanceBuffer = MessagePool.Get<MyRenderMessageCreateRenderInstanceBuffer>(MyRenderMessageEnum.CreateRenderInstanceBuffer);
			uint result = myRenderMessageCreateRenderInstanceBuffer.ID = GetMessageId(ObjectType.InstanceBuffer);
			myRenderMessageCreateRenderInstanceBuffer.ParentID = parentId;
			myRenderMessageCreateRenderInstanceBuffer.DebugName = debugName;
			myRenderMessageCreateRenderInstanceBuffer.Type = type;
			EnqueueMessage(myRenderMessageCreateRenderInstanceBuffer);
			return result;
		}

		public static void UpdateRenderCubeInstanceBuffer(uint id, ref List<MyCubeInstanceData> instanceData, int capacity, ref List<MyCubeInstanceDecalData> decalData)
		{
			MyRenderMessageUpdateRenderCubeInstanceBuffer myRenderMessageUpdateRenderCubeInstanceBuffer = MessagePool.Get<MyRenderMessageUpdateRenderCubeInstanceBuffer>(MyRenderMessageEnum.UpdateRenderCubeInstanceBuffer);
			myRenderMessageUpdateRenderCubeInstanceBuffer.ID = id;
			myRenderMessageUpdateRenderCubeInstanceBuffer.Capacity = capacity;
			MyUtils.Swap(ref myRenderMessageUpdateRenderCubeInstanceBuffer.DecalsData, ref decalData);
			MyUtils.Swap(ref myRenderMessageUpdateRenderCubeInstanceBuffer.InstanceData, ref instanceData);
			EnqueueMessage(myRenderMessageUpdateRenderCubeInstanceBuffer);
		}

		public static void UpdateRenderInstanceBufferRange(uint id, MyInstanceData[] instanceData, int offset = 0, bool trimEnd = false)
		{
			MyRenderMessageUpdateRenderInstanceBufferRange myRenderMessageUpdateRenderInstanceBufferRange = MessagePool.Get<MyRenderMessageUpdateRenderInstanceBufferRange>(MyRenderMessageEnum.UpdateRenderInstanceBufferRange);
			myRenderMessageUpdateRenderInstanceBufferRange.ID = id;
			myRenderMessageUpdateRenderInstanceBufferRange.InstanceData = instanceData;
			myRenderMessageUpdateRenderInstanceBufferRange.StartOffset = offset;
			myRenderMessageUpdateRenderInstanceBufferRange.Trim = trimEnd;
			EnqueueMessage(myRenderMessageUpdateRenderInstanceBufferRange);
		}

		public static void UpdateLineBasedObject(uint id, Vector3D worldPointA, Vector3D worldPointB)
		{
			MyRenderMessageUpdateLineBasedObject myRenderMessageUpdateLineBasedObject = MessagePool.Get<MyRenderMessageUpdateLineBasedObject>(MyRenderMessageEnum.UpdateLineBasedObject);
			myRenderMessageUpdateLineBasedObject.ID = id;
			myRenderMessageUpdateLineBasedObject.WorldPointA = worldPointA;
			myRenderMessageUpdateLineBasedObject.WorldPointB = worldPointB;
			EnqueueMessage(myRenderMessageUpdateLineBasedObject);
		}

		public static uint CreateManualCullObject(string debugName, MatrixD worldMatrix)
		{
			MyRenderMessageCreateManualCullObject myRenderMessageCreateManualCullObject = MessagePool.Get<MyRenderMessageCreateManualCullObject>(MyRenderMessageEnum.CreateManualCullObject);
			uint result = myRenderMessageCreateManualCullObject.ID = GetMessageId(ObjectType.ManualCull);
			myRenderMessageCreateManualCullObject.DebugName = debugName;
			myRenderMessageCreateManualCullObject.WorldMatrix = worldMatrix;
			EnqueueMessage(myRenderMessageCreateManualCullObject);
			return result;
		}

		public static void SetParentCullObject(uint renderObject, uint parentCullObject, Matrix? childToParent = null)
		{
			MyRenderMessageSetParentCullObject myRenderMessageSetParentCullObject = MessagePool.Get<MyRenderMessageSetParentCullObject>(MyRenderMessageEnum.SetParentCullObject);
			myRenderMessageSetParentCullObject.ID = renderObject;
			myRenderMessageSetParentCullObject.CullObjectID = parentCullObject;
			myRenderMessageSetParentCullObject.ChildToParent = childToParent;
			EnqueueMessage(myRenderMessageSetParentCullObject);
		}

		public static void SetCameraViewMatrix(MatrixD viewMatrix, Matrix projectionMatrix, Matrix projectionFarMatrix, float fov, float fovSkybox, float nearPlane, float farPlane, float farFarPlane, Vector3D cameraPosition, float projectionOffsetX = 0f, float projectionOffsetY = 0f, int lastMomentUpdateIndex = 1)
		{
			MyRenderMessageSetCameraViewMatrix myRenderMessageSetCameraViewMatrix = MessagePool.Get<MyRenderMessageSetCameraViewMatrix>(MyRenderMessageEnum.SetCameraViewMatrix);
			myRenderMessageSetCameraViewMatrix.ViewMatrix = viewMatrix;
			myRenderMessageSetCameraViewMatrix.ProjectionMatrix = projectionMatrix;
			myRenderMessageSetCameraViewMatrix.ProjectionFarMatrix = projectionFarMatrix;
			myRenderMessageSetCameraViewMatrix.FOV = fov;
			myRenderMessageSetCameraViewMatrix.FOVForSkybox = fovSkybox;
			myRenderMessageSetCameraViewMatrix.NearPlane = nearPlane;
			myRenderMessageSetCameraViewMatrix.FarPlane = farPlane;
			myRenderMessageSetCameraViewMatrix.FarFarPlane = farFarPlane;
			myRenderMessageSetCameraViewMatrix.CameraPosition = cameraPosition;
			myRenderMessageSetCameraViewMatrix.LastMomentUpdateIndex = lastMomentUpdateIndex;
			myRenderMessageSetCameraViewMatrix.ProjectionOffsetX = projectionOffsetX;
			myRenderMessageSetCameraViewMatrix.ProjectionOffsetY = projectionOffsetY;
			EnqueueMessage(myRenderMessageSetCameraViewMatrix);
		}

		public static void Draw3DScene()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageDrawScene>(MyRenderMessageEnum.DrawScene));
		}

		public static void UpdateRenderComponent<TData, TContext>(uint id, TContext context, Action<TData, TContext> activator) where TData : UpdateData
		{
			MyRenderMessageUpdateComponent myRenderMessageUpdateComponent = MessagePool.Get<MyRenderMessageUpdateComponent>(MyRenderMessageEnum.UpdateRenderComponent);
			myRenderMessageUpdateComponent.ID = id;
			myRenderMessageUpdateComponent.Type = MyRenderMessageUpdateComponent.UpdateType.Update;
			activator(myRenderMessageUpdateComponent.Initialize<TData>(), context);
			EnqueueMessage(myRenderMessageUpdateComponent);
		}

		public static void RemoveRenderComponent<TComponent>(uint id)
		{
			MyRenderMessageUpdateComponent myRenderMessageUpdateComponent = MessagePool.Get<MyRenderMessageUpdateComponent>(MyRenderMessageEnum.UpdateRenderComponent);
			myRenderMessageUpdateComponent.ID = id;
			myRenderMessageUpdateComponent.Type = MyRenderMessageUpdateComponent.UpdateType.Delete;
			myRenderMessageUpdateComponent.Initialize<DeleteComponentData>().SetComponent<TComponent>();
			EnqueueMessage(myRenderMessageUpdateComponent);
		}

		public static void UpdateRenderObject(uint id, MatrixD? worldMatrix, BoundingBox? aabb = null, int lastMomentUpdateIndex = -1, Matrix? localMatrix = null)
		{
			MyRenderMessageUpdateRenderObject myRenderMessageUpdateRenderObject = MessagePool.Get<MyRenderMessageUpdateRenderObject>(MyRenderMessageEnum.UpdateRenderObject);
			myRenderMessageUpdateRenderObject.ID = id;
			myRenderMessageUpdateRenderObject.Data.WorldMatrix = worldMatrix;
			myRenderMessageUpdateRenderObject.Data.LocalAABB = aabb;
			myRenderMessageUpdateRenderObject.LastMomentUpdateIndex = lastMomentUpdateIndex;
			myRenderMessageUpdateRenderObject.Data.LocalMatrix = localMatrix;
			EnqueueMessage(myRenderMessageUpdateRenderObject);
		}

		public static void UpdateRenderObjectVisibility(uint id, bool visible, bool near)
		{
			MyRenderMessageUpdateRenderObjectVisibility myRenderMessageUpdateRenderObjectVisibility = MessagePool.Get<MyRenderMessageUpdateRenderObjectVisibility>(MyRenderMessageEnum.UpdateRenderObjectVisibility);
			myRenderMessageUpdateRenderObjectVisibility.ID = id;
			myRenderMessageUpdateRenderObjectVisibility.Visible = visible;
			myRenderMessageUpdateRenderObjectVisibility.NearFlag = near;
			EnqueueMessage(myRenderMessageUpdateRenderObjectVisibility);
		}

		public static void RemoveRenderObject(uint id, ObjectType objectType, bool fadeOut = false)
		{
			if (objectType == ObjectType.Invalid)
			{
				objectType = GetObjectType(id);
			}
			RemoveMessageId(id, objectType, now: false);
			MyRenderMessageRemoveRenderObject myRenderMessageRemoveRenderObject = MessagePool.Get<MyRenderMessageRemoveRenderObject>(MyRenderMessageEnum.RemoveRenderObject);
			myRenderMessageRemoveRenderObject.ID = id;
			myRenderMessageRemoveRenderObject.FadeOut = fadeOut;
			EnqueueMessage(myRenderMessageRemoveRenderObject);
		}

		public static void UpdateRenderEntity(uint id, Color? diffuseColor, Vector3? colorMaskHsv, float? dithering = null, bool fadeIn = false)
		{
			_ = dithering.HasValue;
			MyRenderMessageUpdateRenderEntity myRenderMessageUpdateRenderEntity = MessagePool.Get<MyRenderMessageUpdateRenderEntity>(MyRenderMessageEnum.UpdateRenderEntity);
			myRenderMessageUpdateRenderEntity.ID = id;
			myRenderMessageUpdateRenderEntity.DiffuseColor = diffuseColor;
			myRenderMessageUpdateRenderEntity.ColorMaskHSV = colorMaskHsv;
			myRenderMessageUpdateRenderEntity.Dithering = dithering;
			myRenderMessageUpdateRenderEntity.FadeIn = fadeIn;
			EnqueueMessage(myRenderMessageUpdateRenderEntity);
		}

		public static void SetInstanceBuffer(uint entityId, uint instanceBufferId, int instanceStart, int instanceCount, BoundingBox entityLocalAabb, MyInstanceData[] instanceData = null)
		{
			MyRenderMessageSetInstanceBuffer myRenderMessageSetInstanceBuffer = MessagePool.Get<MyRenderMessageSetInstanceBuffer>(MyRenderMessageEnum.SetInstanceBuffer);
			myRenderMessageSetInstanceBuffer.ID = entityId;
			myRenderMessageSetInstanceBuffer.InstanceBufferId = instanceBufferId;
			myRenderMessageSetInstanceBuffer.InstanceStart = instanceStart;
			myRenderMessageSetInstanceBuffer.InstanceCount = instanceCount;
			myRenderMessageSetInstanceBuffer.LocalAabb = entityLocalAabb;
			myRenderMessageSetInstanceBuffer.InstanceData = instanceData;
			EnqueueMessage(myRenderMessageSetInstanceBuffer);
		}

		public static uint CreateStaticGroup(string model, Vector3D translation, Matrix[] localMatrices)
		{
			uint messageId = GetMessageId(ObjectType.Entity);
			MyRenderMessageCreateStaticGroup myRenderMessageCreateStaticGroup = MessagePool.Get<MyRenderMessageCreateStaticGroup>(MyRenderMessageEnum.CreateStaticGroup);
			myRenderMessageCreateStaticGroup.ID = messageId;
			myRenderMessageCreateStaticGroup.Model = model;
			myRenderMessageCreateStaticGroup.Translation = translation;
			myRenderMessageCreateStaticGroup.LocalMatrices = localMatrices;
			EnqueueMessage(myRenderMessageCreateStaticGroup);
			return messageId;
		}

		/// <summary>
		/// Create a new render entity for voxel meshes.
		/// </summary>
		/// <param name="debugName">Debug name of this object.</param>
		/// <param name="worldMatrix">World matrix of the created object, scale not allowed.</param>
		/// <param name="scale">Scale of the voxel entity.</param>
		/// <param name="clipmap">The clipmap that will control rendering for this voxel entity.</param>
		/// <param name="flags">Additional flags for the voxel entity.</param>
		/// <returns>The id for the new render entity.</returns>
		public static uint RenderVoxelCreate(string debugName, MatrixD worldMatrix, IMyLodController clipmap, RenderFlags flags = RenderFlags.Visible, float dithering = 0f)
		{
			if (clipmap == null)
			{
				throw new ArgumentNullException("clipmap");
			}
			MyRenderMessageVoxelCreate myRenderMessageVoxelCreate = MessagePool.Get<MyRenderMessageVoxelCreate>(MyRenderMessageEnum.VoxelCreate);
			myRenderMessageVoxelCreate.Id = GetMessageId(ObjectType.Entity);
			myRenderMessageVoxelCreate.DebugName = debugName;
			myRenderMessageVoxelCreate.WorldMatrix = worldMatrix;
			myRenderMessageVoxelCreate.Clipmap = clipmap;
			myRenderMessageVoxelCreate.Size = clipmap.Size;
			myRenderMessageVoxelCreate.SpherizeRadius = clipmap.SpherizeRadius;
			myRenderMessageVoxelCreate.SpherizePosition = clipmap.SpherizePosition;
			myRenderMessageVoxelCreate.RenderFlags = flags;
			myRenderMessageVoxelCreate.Dithering = dithering;
			EnqueueMessage(myRenderMessageVoxelCreate);
			return myRenderMessageVoxelCreate.Id;
		}

		public static void RebuildCullingStructure()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageRebuildCullingStructure>(MyRenderMessageEnum.RebuildCullingStructure));
		}

		public static void ReloadEffects()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageReloadEffects>(MyRenderMessageEnum.ReloadEffects));
		}

		public static void ReloadModels()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageReloadModels>(MyRenderMessageEnum.ReloadModels));
		}

		public static void ReloadTextures()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageReloadTextures>(MyRenderMessageEnum.ReloadTextures));
		}

		public static void UpdateEnvironmentMap()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageUpdateEnvironmentMap>(MyRenderMessageEnum.UpdateEnvironmentMap));
		}

		public static void CreateRenderVoxelMaterials(MyRenderVoxelMaterialData[] materials)
		{
			MyRenderMessageCreateRenderVoxelMaterials myRenderMessageCreateRenderVoxelMaterials = MessagePool.Get<MyRenderMessageCreateRenderVoxelMaterials>(MyRenderMessageEnum.CreateRenderVoxelMaterials);
			myRenderMessageCreateRenderVoxelMaterials.Materials = materials;
			EnqueueMessage(myRenderMessageCreateRenderVoxelMaterials);
		}

		public static void PreloadVoxelMaterials(byte[] materials)
		{
			MyRenderMessagePreloadVoxelMaterials myRenderMessagePreloadVoxelMaterials = MessagePool.Get<MyRenderMessagePreloadVoxelMaterials>(MyRenderMessageEnum.PreloadVoxelMaterials);
			myRenderMessagePreloadVoxelMaterials.Materials = materials;
			EnqueueMessage(myRenderMessagePreloadVoxelMaterials);
		}

		public static void UpdateRenderVoxelMaterials(MyRenderVoxelMaterialData[] materials)
		{
			MyRenderMessageUpdateRenderVoxelMaterials myRenderMessageUpdateRenderVoxelMaterials = MessagePool.Get<MyRenderMessageUpdateRenderVoxelMaterials>(MyRenderMessageEnum.UpdateRenderVoxelMaterials);
			myRenderMessageUpdateRenderVoxelMaterials.Materials = materials;
			EnqueueMessage(myRenderMessageUpdateRenderVoxelMaterials);
		}

		public static uint CreateRenderVoxelDebris(string debugName, string model, MatrixD worldMatrix, float textureCoordOffset, float textureCoordScale, float textureColorMultiplier, byte voxelMaterialIndex, bool fadeIn)
		{
			MyRenderMessageCreateRenderVoxelDebris myRenderMessageCreateRenderVoxelDebris = MessagePool.Get<MyRenderMessageCreateRenderVoxelDebris>(MyRenderMessageEnum.CreateRenderVoxelDebris);
			uint result = myRenderMessageCreateRenderVoxelDebris.ID = GetMessageId(ObjectType.Entity);
			myRenderMessageCreateRenderVoxelDebris.DebugName = debugName;
			myRenderMessageCreateRenderVoxelDebris.Model = model;
			myRenderMessageCreateRenderVoxelDebris.WorldMatrix = worldMatrix;
			myRenderMessageCreateRenderVoxelDebris.TextureCoordOffset = textureCoordOffset;
			myRenderMessageCreateRenderVoxelDebris.TextureCoordScale = textureCoordScale;
			myRenderMessageCreateRenderVoxelDebris.TextureColorMultiplier = textureColorMultiplier;
			myRenderMessageCreateRenderVoxelDebris.VoxelMaterialIndex = voxelMaterialIndex;
			myRenderMessageCreateRenderVoxelDebris.FadeIn = fadeIn;
			EnqueueMessage(myRenderMessageCreateRenderVoxelDebris);
			return result;
		}

		public static void UpdateModelProperties(uint id, string materialName, RenderFlags addFlags, RenderFlags removeFlags, Color? diffuseColor, float? emissivity)
		{
			if (!string.IsNullOrEmpty(materialName))
			{
				MyRenderMessageUpdateModelProperties myRenderMessageUpdateModelProperties = MessagePool.Get<MyRenderMessageUpdateModelProperties>(MyRenderMessageEnum.UpdateModelProperties);
				myRenderMessageUpdateModelProperties.ID = id;
				myRenderMessageUpdateModelProperties.MaterialName = materialName;
				if (addFlags != 0 || removeFlags != 0)
				{
					myRenderMessageUpdateModelProperties.FlagsChange = new RenderFlagsChange
					{
						Add = addFlags,
						Remove = removeFlags
					};
				}
				else
				{
					myRenderMessageUpdateModelProperties.FlagsChange = null;
				}
				myRenderMessageUpdateModelProperties.DiffuseColor = diffuseColor;
				myRenderMessageUpdateModelProperties.Emissivity = emissivity;
				EnqueueMessage(myRenderMessageUpdateModelProperties);
			}
		}

		/// <param name="thickness">Zero or negative to remove highlight</param>
		public static void UpdateModelHighlight(uint id, string[] sectionNames, uint[] subpartIndices, Color? outlineColor, float thickness = -1f, float pulseTimeInSeconds = 0f, int instanceIndex = -1)
		{
			MyRenderMessageUpdateModelHighlight myRenderMessageUpdateModelHighlight = MessagePool.Get<MyRenderMessageUpdateModelHighlight>(MyRenderMessageEnum.UpdateModelHighlight);
			myRenderMessageUpdateModelHighlight.ID = id;
			myRenderMessageUpdateModelHighlight.SectionNames = sectionNames;
			myRenderMessageUpdateModelHighlight.SubpartIndices = subpartIndices;
			myRenderMessageUpdateModelHighlight.OutlineColor = outlineColor;
			myRenderMessageUpdateModelHighlight.Thickness = thickness;
			myRenderMessageUpdateModelHighlight.PulseTimeInSeconds = pulseTimeInSeconds;
			myRenderMessageUpdateModelHighlight.InstanceIndex = instanceIndex;
			EnqueueMessage(myRenderMessageUpdateModelHighlight);
		}

		/// <summary>
		/// Makes the actor of given RenderId overlap highlights.
		/// If possible use MyHighlightSystem session component instead.
		/// </summary>
		/// <param name="modelRenderId">Actor Id.</param>
		/// <param name="enable">Enable flag.</param>
		public static void UpdateHighlightOverlappingModel(uint modelRenderId, bool enable = true)
		{
			MyRenderMessageUpdateOverlappingModelsForHighlight myRenderMessageUpdateOverlappingModelsForHighlight = MessagePool.Get<MyRenderMessageUpdateOverlappingModelsForHighlight>(MyRenderMessageEnum.UpdateOverlappingModelsForHighlight);
			myRenderMessageUpdateOverlappingModelsForHighlight.Enable = enable;
			myRenderMessageUpdateOverlappingModelsForHighlight.OverlappingModelID = modelRenderId;
			EnqueueMessage(myRenderMessageUpdateOverlappingModelsForHighlight);
		}

		public static void UpdateColorEmissivity(uint id, int lod, string materialName, Color diffuseColor, float emissivity)
		{
			if (id != uint.MaxValue && !string.IsNullOrEmpty(materialName))
			{
				MyRenderMessageUpdateColorEmissivity myRenderMessageUpdateColorEmissivity = MessagePool.Get<MyRenderMessageUpdateColorEmissivity>(MyRenderMessageEnum.UpdateColorEmissivity);
				myRenderMessageUpdateColorEmissivity.ID = id;
				myRenderMessageUpdateColorEmissivity.LOD = lod;
				myRenderMessageUpdateColorEmissivity.MaterialName = materialName;
				myRenderMessageUpdateColorEmissivity.DiffuseColor = diffuseColor;
				myRenderMessageUpdateColorEmissivity.Emissivity = emissivity;
				EnqueueMessage(myRenderMessageUpdateColorEmissivity);
			}
		}

		/// <summary>
		/// New model should have similar size to previous model because of prunning structure recalculation
		/// </summary>
		/// <param name="id"></param>
		/// <param name="LOD"></param>
		/// <param name="model"></param>
		/// <param name="useForShadow"></param>
		[Obsolete]
		public static void ChangeModel(uint id, string model, float scale = 1f)
		{
			MyRenderMessageChangeModel myRenderMessageChangeModel = MessagePool.Get<MyRenderMessageChangeModel>(MyRenderMessageEnum.ChangeModel);
			myRenderMessageChangeModel.ID = id;
			myRenderMessageChangeModel.Model = model;
			myRenderMessageChangeModel.Scale = scale;
			EnqueueMessage(myRenderMessageChangeModel);
		}

		public static void UpdateGameplayFrame(int frame)
		{
			MyRenderMessageUpdateGameplayFrame myRenderMessageUpdateGameplayFrame = MessagePool.Get<MyRenderMessageUpdateGameplayFrame>(MyRenderMessageEnum.UpdateGameplayFrame);
			myRenderMessageUpdateGameplayFrame.GameplayFrame = frame;
			EnqueueMessage(myRenderMessageUpdateGameplayFrame);
		}

		public static void ChangeMaterialTexture(uint id, string materialName, string colorMetalFileName = null, string normalGlossFileName = null, string extensionsFileName = null, string alphamaskFileName = null)
		{
			MyRenderMessageChangeMaterialTexture myRenderMessageChangeMaterialTexture = MessagePool.Get<MyRenderMessageChangeMaterialTexture>(MyRenderMessageEnum.ChangeMaterialTexture);
			if (myRenderMessageChangeMaterialTexture.Changes == null)
			{
				myRenderMessageChangeMaterialTexture.Changes = new Dictionary<string, MyTextureChange>();
			}
			myRenderMessageChangeMaterialTexture.Changes.Add(materialName, new MyTextureChange
			{
				ColorMetalFileName = colorMetalFileName,
				NormalGlossFileName = normalGlossFileName,
				ExtensionsFileName = extensionsFileName,
				AlphamaskFileName = alphamaskFileName
			});
			myRenderMessageChangeMaterialTexture.RenderObjectID = id;
			EnqueueMessage(myRenderMessageChangeMaterialTexture);
		}

		public static void ChangeMaterialTexture(uint id, Dictionary<string, MyTextureChange> textureChanges)
		{
			MyRenderMessageChangeMaterialTexture myRenderMessageChangeMaterialTexture = MessagePool.Get<MyRenderMessageChangeMaterialTexture>(MyRenderMessageEnum.ChangeMaterialTexture);
			_ = myRenderMessageChangeMaterialTexture.Changes;
			myRenderMessageChangeMaterialTexture.Changes = textureChanges;
			myRenderMessageChangeMaterialTexture.RenderObjectID = id;
			EnqueueMessage(myRenderMessageChangeMaterialTexture);
		}

		/// <param name="backgroundColor">null means no background</param>
		/// <param name="blendAlphaChannel">Blend alpha channel</param>
		public static void RenderOffscreenTexture(string offscreenTexture, Vector2? aspectRatio = null, Color? backgroundColor = null)
		{
			CheckValidGeneratedTextureName(offscreenTexture);
			MyRenderMessageRenderOffscreenTexture myRenderMessageRenderOffscreenTexture = MessagePool.Get<MyRenderMessageRenderOffscreenTexture>(MyRenderMessageEnum.RenderOffscreenTexture);
			myRenderMessageRenderOffscreenTexture.OffscreenTexture = offscreenTexture;
			myRenderMessageRenderOffscreenTexture.BackgroundColor = backgroundColor;
			myRenderMessageRenderOffscreenTexture.AspectRatio = (aspectRatio ?? Vector2.One);
			EnqueueMessage(myRenderMessageRenderOffscreenTexture);
		}

		private static void EnqueueOutputMessage(MyRenderMessageBase message)
		{
			m_render.EnqueueOutputMessage(message);
		}

		public static uint CreateRenderLight(string debugName)
		{
			MyRenderMessageCreateRenderLight myRenderMessageCreateRenderLight = MessagePool.Get<MyRenderMessageCreateRenderLight>(MyRenderMessageEnum.CreateRenderLight);
			uint result = myRenderMessageCreateRenderLight.ID = GetMessageId(ObjectType.Light);
			myRenderMessageCreateRenderLight.DebugName = debugName;
			EnqueueMessage(myRenderMessageCreateRenderLight);
			return result;
		}

		public static void UpdateRenderLight(ref UpdateRenderLightData data)
		{
			MyRenderMessageUpdateRenderLight myRenderMessageUpdateRenderLight = MessagePool.Get<MyRenderMessageUpdateRenderLight>(MyRenderMessageEnum.UpdateRenderLight);
			myRenderMessageUpdateRenderLight.Data = data;
			EnqueueMessage(myRenderMessageUpdateRenderLight);
		}

		public static void SetLightShadowIgnore(uint id, uint ignoreId)
		{
			MyRenderMessageSetLightShadowIgnore myRenderMessageSetLightShadowIgnore = MessagePool.Get<MyRenderMessageSetLightShadowIgnore>(MyRenderMessageEnum.SetLightShadowIgnore);
			myRenderMessageSetLightShadowIgnore.ID = id;
			myRenderMessageSetLightShadowIgnore.ID2 = ignoreId;
			EnqueueMessage(myRenderMessageSetLightShadowIgnore);
		}

		public static void ClearLightShadowIgnore(uint id)
		{
			MyRenderMessageClearLightShadowIgnore myRenderMessageClearLightShadowIgnore = MessagePool.Get<MyRenderMessageClearLightShadowIgnore>(MyRenderMessageEnum.ClearLightShadowIgnore);
			myRenderMessageClearLightShadowIgnore.ID = id;
			EnqueueMessage(myRenderMessageClearLightShadowIgnore);
		}

		public static void UpdateShadowsSettings(MyShadowsSettings settings)
		{
			MyRenderMessageUpdateShadowSettings myRenderMessageUpdateShadowSettings = MessagePool.Get<MyRenderMessageUpdateShadowSettings>(MyRenderMessageEnum.UpdateShadowSettings);
			myRenderMessageUpdateShadowSettings.Settings.CopyFrom(settings);
			EnqueueMessage(myRenderMessageUpdateShadowSettings);
		}

		public static void UpdateNewLoddingSettings(MyNewLoddingSettings settings)
		{
			MyRenderMessageUpdateNewLoddingSettings myRenderMessageUpdateNewLoddingSettings = MessagePool.Get<MyRenderMessageUpdateNewLoddingSettings>(MyRenderMessageEnum.UpdateNewLoddingSettings);
			myRenderMessageUpdateNewLoddingSettings.Settings.CopyFrom(settings);
			EnqueueMessage(myRenderMessageUpdateNewLoddingSettings);
		}

		public static void UpdateRenderEnvironment(ref MyEnvironmentData data, bool resetEyeAdaptation)
		{
			if (!string.IsNullOrEmpty(data.Skybox))
			{
				MyRenderMessageUpdateRenderEnvironment myRenderMessageUpdateRenderEnvironment = MessagePool.Get<MyRenderMessageUpdateRenderEnvironment>(MyRenderMessageEnum.UpdateRenderEnvironment);
				myRenderMessageUpdateRenderEnvironment.Data = data;
				myRenderMessageUpdateRenderEnvironment.ResetEyeAdaptation = resetEyeAdaptation;
				EnqueueMessage(myRenderMessageUpdateRenderEnvironment);
			}
		}

		public static void UpdateSSAOSettings(ref MySSAOSettings settings)
		{
			MyRenderMessageUpdateSSAOSettings myRenderMessageUpdateSSAOSettings = MessagePool.Get<MyRenderMessageUpdateSSAOSettings>(MyRenderMessageEnum.UpdateSSAOSettings);
			myRenderMessageUpdateSSAOSettings.Settings = settings;
			EnqueueMessage(myRenderMessageUpdateSSAOSettings);
		}

		public static void UpdateHBAOSettings(ref MyHBAOData settings)
		{
			MyRenderMessageUpdateHBAO myRenderMessageUpdateHBAO = MessagePool.Get<MyRenderMessageUpdateHBAO>(MyRenderMessageEnum.UpdateHBAO);
			myRenderMessageUpdateHBAO.Settings = settings;
			EnqueueMessage(myRenderMessageUpdateHBAO);
		}

		public static void UpdateFogSettings(ref MyRenderFogSettings settings)
		{
			MyRenderMessageUpdateFogSettings myRenderMessageUpdateFogSettings = MessagePool.Get<MyRenderMessageUpdateFogSettings>(MyRenderMessageEnum.UpdateFogSettings);
			myRenderMessageUpdateFogSettings.Settings = settings;
			EnqueueMessage(myRenderMessageUpdateFogSettings);
		}

		public static void UpdateCloudLayerFogFlag(bool shouldDrawFog)
		{
			MyRenderMessageUpdateCloudLayerFogFlag myRenderMessageUpdateCloudLayerFogFlag = MessagePool.Get<MyRenderMessageUpdateCloudLayerFogFlag>(MyRenderMessageEnum.UpdateCloudLayerFogFlag);
			myRenderMessageUpdateCloudLayerFogFlag.ShouldDrawFog = shouldDrawFog;
			EnqueueMessage(myRenderMessageUpdateCloudLayerFogFlag);
		}

		public static void UpdateAtmosphereSettings(uint id, MyAtmosphereSettings settings)
		{
			MyRenderMessageUpdateAtmosphereSettings myRenderMessageUpdateAtmosphereSettings = MessagePool.Get<MyRenderMessageUpdateAtmosphereSettings>(MyRenderMessageEnum.UpdateAtmosphereSettings);
			myRenderMessageUpdateAtmosphereSettings.ID = id;
			myRenderMessageUpdateAtmosphereSettings.Settings = settings;
			EnqueueMessage(myRenderMessageUpdateAtmosphereSettings);
		}

		public static void EnableAtmosphere(bool enabled)
		{
			MyRenderMessageEnableAtmosphere myRenderMessageEnableAtmosphere = MessagePool.Get<MyRenderMessageEnableAtmosphere>(MyRenderMessageEnum.EnableAtmosphere);
			myRenderMessageEnableAtmosphere.Enabled = enabled;
			EnqueueMessage(myRenderMessageEnableAtmosphere);
		}

		public static uint PlayVideo(string videoFile, float volume)
		{
			MyRenderMessagePlayVideo myRenderMessagePlayVideo = MessagePool.Get<MyRenderMessagePlayVideo>(MyRenderMessageEnum.PlayVideo);
			uint result = myRenderMessagePlayVideo.ID = GetMessageId(ObjectType.Video);
			myRenderMessagePlayVideo.VideoFile = videoFile;
			myRenderMessagePlayVideo.Volume = volume;
			EnqueueMessage(myRenderMessagePlayVideo);
			return result;
		}

		public static void CloseVideo(uint id)
		{
			MyRenderMessageCloseVideo myRenderMessageCloseVideo = MessagePool.Get<MyRenderMessageCloseVideo>(MyRenderMessageEnum.CloseVideo);
			myRenderMessageCloseVideo.ID = id;
			EnqueueMessage(myRenderMessageCloseVideo);
		}

		public static void DrawVideo(uint id, Rectangle rect, Color color, MyVideoRectangleFitMode fitMode)
		{
			MyRenderMessageDrawVideo myRenderMessageDrawVideo = MessagePool.Get<MyRenderMessageDrawVideo>(MyRenderMessageEnum.DrawVideo);
			myRenderMessageDrawVideo.ID = id;
			myRenderMessageDrawVideo.Rectangle = rect;
			myRenderMessageDrawVideo.Color = color;
			myRenderMessageDrawVideo.FitMode = fitMode;
			EnqueueMessage(myRenderMessageDrawVideo);
		}

		public static void UpdateVideo(uint id)
		{
			MyRenderMessageUpdateVideo myRenderMessageUpdateVideo = MessagePool.Get<MyRenderMessageUpdateVideo>(MyRenderMessageEnum.UpdateVideo);
			myRenderMessageUpdateVideo.ID = id;
			EnqueueMessage(myRenderMessageUpdateVideo);
		}

		public static void SetVideoVolume(uint id, float volume)
		{
			MyRenderMessageSetVideoVolume myRenderMessageSetVideoVolume = MessagePool.Get<MyRenderMessageSetVideoVolume>(MyRenderMessageEnum.SetVideoVolume);
			myRenderMessageSetVideoVolume.ID = id;
			myRenderMessageSetVideoVolume.Volume = volume;
			EnqueueMessage(myRenderMessageSetVideoVolume);
		}

		public static bool IsVideoValid(uint id)
		{
			return m_render.IsVideoValid(id);
		}

		public static VideoState GetVideoState(uint id)
		{
			return m_render.GetVideoState(id);
		}

		public static void AddBillboard(MyBillboard billboard)
		{
			if (DebugOverrides.BillboardsStatic && m_render.SharedData != null)
			{
				BillboardsWrite.Add(billboard);
			}
		}

		public static void AddBillboards(List<MyBillboard> billboards)
		{
			if (DebugOverrides.BillboardsStatic && m_render.SharedData != null)
			{
				BillboardsWrite.AddRange(billboards);
			}
		}

		public static void AddBillboardViewProjection(int id, MyBillboardViewProjection billboardViewProjection)
		{
			if (m_render.SharedData != null)
			{
				if (!BillboardsViewProjectionWrite.TryGetValue(id, out MyBillboardViewProjection _))
				{
					BillboardsViewProjectionWrite.Add(id, billboardViewProjection);
				}
				else
				{
					BillboardsViewProjectionWrite[id] = billboardViewProjection;
				}
			}
		}

		public static void RemoveBillboardViewProjection(int id)
		{
			if (m_render.SharedData != null)
			{
				BillboardsViewProjectionWrite.Remove(id);
			}
		}

		public static void TakeScreenshot(Vector2 sizeMultiplier, string pathToSave, bool debug, bool ignoreSprites, bool showNotification)
		{
			if (debug && pathToSave != null)
			{
				throw new ArgumentException("When taking debug screenshot, path to save must be null, becase debug takes a lot of screenshots");
			}
			MyRenderMessageTakeScreenshot myRenderMessageTakeScreenshot = MessagePool.Get<MyRenderMessageTakeScreenshot>(MyRenderMessageEnum.TakeScreenshot);
			myRenderMessageTakeScreenshot.IgnoreSprites = ignoreSprites;
			myRenderMessageTakeScreenshot.SizeMultiplier = sizeMultiplier;
			myRenderMessageTakeScreenshot.PathToSave = pathToSave;
			myRenderMessageTakeScreenshot.Debug = debug;
			myRenderMessageTakeScreenshot.ShowNotification = showNotification;
			EnqueueMessage(myRenderMessageTakeScreenshot);
		}

		public static void RenderColoredTextures(List<renderColoredTextureProperties> texturesToRender)
		{
			MyRenderMessageRenderColoredTexture myRenderMessageRenderColoredTexture = MessagePool.Get<MyRenderMessageRenderColoredTexture>(MyRenderMessageEnum.RenderColoredTexture);
			myRenderMessageRenderColoredTexture.texturesToRender = texturesToRender;
			EnqueueMessage(myRenderMessageRenderColoredTexture);
		}

		public static uint CreateGPUEmitter(string debugName)
		{
			MyRenderMessageCreateGPUEmitter myRenderMessageCreateGPUEmitter = MessagePool.Get<MyRenderMessageCreateGPUEmitter>(MyRenderMessageEnum.CreateGPUEmitter);
			uint result = myRenderMessageCreateGPUEmitter.ID = GetMessageId(ObjectType.GPUEmitter);
			myRenderMessageCreateGPUEmitter.DebugName = debugName;
			EnqueueMessage(myRenderMessageCreateGPUEmitter);
			return result;
		}

		public static void UpdateGPUEmitters(ref List<MyGPUEmitter> emitters)
		{
			MyRenderMessageUpdateGPUEmitters myRenderMessageUpdateGPUEmitters = MessagePool.Get<MyRenderMessageUpdateGPUEmitters>(MyRenderMessageEnum.UpdateGPUEmitters);
			MyUtils.Swap(ref emitters, ref myRenderMessageUpdateGPUEmitters.Emitters);
			EnqueueMessage(myRenderMessageUpdateGPUEmitters);
		}

		public static void UpdateGPUEmittersTransform(ref List<MyGPUEmitterTransformUpdate> emitters)
		{
			MyRenderMessageUpdateGPUEmittersTransform myRenderMessageUpdateGPUEmittersTransform = MessagePool.Get<MyRenderMessageUpdateGPUEmittersTransform>(MyRenderMessageEnum.UpdateGPUEmittersTransform);
			MyUtils.Swap(ref emitters, ref myRenderMessageUpdateGPUEmittersTransform.Emitters);
			EnqueueMessage(myRenderMessageUpdateGPUEmittersTransform);
		}

		public static void UpdateGPUEmittersLite(ref List<MyGPUEmitterLite> emitters)
		{
			MyRenderMessageUpdateGPUEmittersLite myRenderMessageUpdateGPUEmittersLite = MessagePool.Get<MyRenderMessageUpdateGPUEmittersLite>(MyRenderMessageEnum.UpdateGPUEmittersLite);
			MyUtils.Swap(ref emitters, ref myRenderMessageUpdateGPUEmittersLite.Emitters);
			EnqueueMessage(myRenderMessageUpdateGPUEmittersLite);
		}

		public static void RemoveGPUEmitter(uint GID, bool instant)
		{
			MyRenderMessageRemoveGPUEmitter myRenderMessageRemoveGPUEmitter = MessagePool.Get<MyRenderMessageRemoveGPUEmitter>(MyRenderMessageEnum.RemoveGPUEmitter);
			myRenderMessageRemoveGPUEmitter.GID = GID;
			myRenderMessageRemoveGPUEmitter.Instant = instant;
			EnqueueMessage(myRenderMessageRemoveGPUEmitter);
			RemoveMessageId(myRenderMessageRemoveGPUEmitter.GID, ObjectType.GPUEmitter, now: false);
		}

		public static void ScreenshotTaken(bool success, string filename, bool showNotification)
		{
			MyRenderMessageScreenshotTaken myRenderMessageScreenshotTaken = MessagePool.Get<MyRenderMessageScreenshotTaken>(MyRenderMessageEnum.ScreenshotTaken);
			myRenderMessageScreenshotTaken.Success = success;
			myRenderMessageScreenshotTaken.Filename = filename;
			myRenderMessageScreenshotTaken.ShowNotification = showNotification;
			EnqueueOutputMessage(myRenderMessageScreenshotTaken);
		}

		public static void Error(string messageText, int skipStack = 0, bool shouldTerminate = false)
		{
			MyRenderMessageError myRenderMessageError = MessagePool.Get<MyRenderMessageError>(MyRenderMessageEnum.Error);
			StackTrace stackTrace = new StackTrace(1 + skipStack, fNeedFileInfo: true);
			myRenderMessageError.Callstack = stackTrace.ToString();
			myRenderMessageError.Message = messageText;
			myRenderMessageError.ShouldTerminate = shouldTerminate;
			EnqueueOutputMessage(myRenderMessageError);
		}

		public static void ExportToObjComplete(bool success, string filename)
		{
			MyRenderMessageExportToObjComplete myRenderMessageExportToObjComplete = MessagePool.Get<MyRenderMessageExportToObjComplete>(MyRenderMessageEnum.ExportToObjComplete);
			myRenderMessageExportToObjComplete.Success = success;
			myRenderMessageExportToObjComplete.Filename = filename;
			EnqueueOutputMessage(myRenderMessageExportToObjComplete);
		}

		public static uint CreateRenderCharacter(string debugName, string lod0, MatrixD worldMatrix, Color? diffuseColor, Vector3? colorMaskHSV, RenderFlags flags, bool fadeIn)
		{
			MyRenderMessageCreateRenderCharacter myRenderMessageCreateRenderCharacter = MessagePool.Get<MyRenderMessageCreateRenderCharacter>(MyRenderMessageEnum.CreateRenderCharacter);
			uint num = myRenderMessageCreateRenderCharacter.ID = GetMessageId(ObjectType.Entity);
			myRenderMessageCreateRenderCharacter.DebugName = debugName;
			myRenderMessageCreateRenderCharacter.Model = lod0;
			myRenderMessageCreateRenderCharacter.WorldMatrix = worldMatrix;
			myRenderMessageCreateRenderCharacter.DiffuseColor = diffuseColor;
			myRenderMessageCreateRenderCharacter.ColorMaskHSV = colorMaskHSV;
			myRenderMessageCreateRenderCharacter.Flags = flags;
			myRenderMessageCreateRenderCharacter.FadeIn = fadeIn;
			EnqueueMessage(myRenderMessageCreateRenderCharacter);
			UpdateRenderEntity(num, diffuseColor, colorMaskHSV);
			return num;
		}

		public static void SetCharacterSkeleton(uint characterID, MySkeletonBoneDescription[] skeletonBones, int[] skeletonIndices)
		{
			MyRenderMessageSetCharacterSkeleton myRenderMessageSetCharacterSkeleton = MessagePool.Get<MyRenderMessageSetCharacterSkeleton>(MyRenderMessageEnum.SetCharacterSkeleton);
			myRenderMessageSetCharacterSkeleton.CharacterID = characterID;
			myRenderMessageSetCharacterSkeleton.SkeletonBones = skeletonBones;
			myRenderMessageSetCharacterSkeleton.SkeletonIndices = skeletonIndices;
			EnqueueMessage(myRenderMessageSetCharacterSkeleton);
		}

		public static bool SetCharacterTransforms(uint characterID, Matrix[] boneTransforms, IReadOnlyList<MyBoneDecalUpdate> boneDecalUpdates)
		{
			MyRenderMessageSetCharacterTransforms myRenderMessageSetCharacterTransforms = MessagePool.Get<MyRenderMessageSetCharacterTransforms>(MyRenderMessageEnum.SetCharacterTransforms);
			myRenderMessageSetCharacterTransforms.CharacterID = characterID;
			if (myRenderMessageSetCharacterTransforms.BoneAbsoluteTransforms == null || myRenderMessageSetCharacterTransforms.BoneAbsoluteTransforms.Length < boneTransforms.Length)
			{
				myRenderMessageSetCharacterTransforms.BoneAbsoluteTransforms = new Matrix[boneTransforms.Length];
			}
			Array.Copy(boneTransforms, myRenderMessageSetCharacterTransforms.BoneAbsoluteTransforms, boneTransforms.Length);
			myRenderMessageSetCharacterTransforms.BoneDecalUpdates.AddRange(boneDecalUpdates);
			EnqueueMessage(myRenderMessageSetCharacterTransforms);
			return false;
		}

		public static void DebugDrawCross(Vector3D center, Vector3D normal, Vector3D face, Color color, bool depthRead = false, bool persistent = false)
		{
			Vector3D value = Vector3D.Cross(face, Vector3.Normalize(normal));
			Vector3D pointFrom = center + face;
			Vector3D pointTo = center - face;
			Vector3D pointFrom2 = center + value;
			Vector3D pointTo2 = center - value;
			DebugDrawLine3D(pointFrom, pointTo, color, color, depthRead, persistent);
			DebugDrawLine3D(pointFrom2, pointTo2, color, color, depthRead, persistent);
		}

		public static void DebugDrawArrow3DDir(Vector3D posFrom, Vector3D direction, Color color, Color? colorTo = null, bool depthRead = false, double tipScale = 0.1, string text = null, float textSize = 0.5f, bool persistent = false)
		{
			DebugDrawArrow3D(posFrom, posFrom + direction, color, colorTo ?? color, depthRead, tipScale, text, textSize, persistent);
		}

		public static void DebugDrawArrow3D(Vector3D pointFrom, Vector3D pointTo, Color colorFrom, Color? colorTo = null, bool depthRead = false, double tipScale = 0.1, string text = null, float textSize = 0.5f, bool persistent = false)
		{
			Color color = colorTo ?? colorFrom;
			Vector3D vector3D = pointTo - pointFrom;
			double num = vector3D.Length();
			if (num > 9.9999997473787516E-05)
			{
				tipScale *= num;
				vector3D /= num;
				Vector3D vector3D2 = Vector3D.CalculatePerpendicularVector(vector3D);
				Vector3D value = Vector3D.Cross(vector3D2, vector3D);
				vector3D *= tipScale;
				value *= tipScale;
				vector3D2 *= tipScale;
				DebugDrawLine3D(pointTo, pointTo + vector3D2 - vector3D, color, color, depthRead, persistent);
				DebugDrawLine3D(pointTo, pointTo - vector3D2 - vector3D, color, color, depthRead, persistent);
				DebugDrawLine3D(pointTo, pointTo + value - vector3D, color, color, depthRead, persistent);
				DebugDrawLine3D(pointTo, pointTo - value - vector3D, color, color, depthRead, persistent);
			}
			DebugDrawLine3D(pointFrom, pointTo, colorFrom, color, depthRead, persistent);
			if (text != null && num > 9.9999997473787516E-05)
			{
				DebugDrawText3D(pointTo + vector3D, text, color, textSize, depthRead, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, -1, persistent);
			}
		}

		public static void DebugDrawFrustrum(BoundingFrustumD frustrum, Color color, float alpha, bool depthRead, bool smooth = false, bool persistent = false)
		{
			MyRenderMessageDebugDrawFrustrum myRenderMessageDebugDrawFrustrum = MessagePool.Get<MyRenderMessageDebugDrawFrustrum>(MyRenderMessageEnum.DebugDrawFrustrum);
			myRenderMessageDebugDrawFrustrum.Frustum = frustrum;
			myRenderMessageDebugDrawFrustrum.Color = color;
			myRenderMessageDebugDrawFrustrum.Alpha = alpha;
			myRenderMessageDebugDrawFrustrum.DepthRead = depthRead;
			myRenderMessageDebugDrawFrustrum.Smooth = smooth;
			myRenderMessageDebugDrawFrustrum.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawFrustrum);
		}

		public static MyRenderMessageDebugDrawLine3DBatch DebugDrawLine3DOpenBatch(bool depthRead, bool persistent = false)
		{
			MyRenderMessageDebugDrawLine3DBatch myRenderMessageDebugDrawLine3DBatch = MessagePool.Get<MyRenderMessageDebugDrawLine3DBatch>(MyRenderMessageEnum.DebugDrawLine3DBatch);
			myRenderMessageDebugDrawLine3DBatch.DepthRead = depthRead;
			myRenderMessageDebugDrawLine3DBatch.Persistent = persistent;
			return myRenderMessageDebugDrawLine3DBatch;
		}

		public static void DebugDrawLine3DSubmitBatch(MyRenderMessageDebugDrawLine3DBatch message)
		{
			EnqueueMessage(message);
		}

		public static void DebugDrawLine3D(Vector3D pointFrom, Vector3D pointTo, Color colorFrom, Color colorTo, bool depthRead, bool persistent = false)
		{
			MyRenderMessageDebugDrawLine3D myRenderMessageDebugDrawLine3D = MessagePool.Get<MyRenderMessageDebugDrawLine3D>(MyRenderMessageEnum.DebugDrawLine3D);
			myRenderMessageDebugDrawLine3D.PointFrom = pointFrom;
			myRenderMessageDebugDrawLine3D.PointTo = pointTo;
			myRenderMessageDebugDrawLine3D.ColorFrom = colorFrom;
			myRenderMessageDebugDrawLine3D.ColorTo = colorTo;
			myRenderMessageDebugDrawLine3D.DepthRead = depthRead;
			myRenderMessageDebugDrawLine3D.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawLine3D);
		}

		public static void DebugDrawLine2D(Vector2 pointFrom, Vector2 pointTo, Color colorFrom, Color colorTo, Matrix? projection = null, bool persistent = false)
		{
			MyRenderMessageDebugDrawLine2D myRenderMessageDebugDrawLine2D = MessagePool.Get<MyRenderMessageDebugDrawLine2D>(MyRenderMessageEnum.DebugDrawLine2D);
			myRenderMessageDebugDrawLine2D.PointFrom = pointFrom;
			myRenderMessageDebugDrawLine2D.PointTo = pointTo;
			myRenderMessageDebugDrawLine2D.ColorFrom = colorFrom;
			myRenderMessageDebugDrawLine2D.ColorTo = colorTo;
			myRenderMessageDebugDrawLine2D.Projection = projection;
			myRenderMessageDebugDrawLine2D.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawLine2D);
		}

		public static void DebugDrawPoint(Vector3D position, Color color, bool depthRead, bool persistent = false)
		{
			MyRenderMessageDebugDrawPoint myRenderMessageDebugDrawPoint = MessagePool.Get<MyRenderMessageDebugDrawPoint>(MyRenderMessageEnum.DebugDrawPoint);
			myRenderMessageDebugDrawPoint.Position = position;
			myRenderMessageDebugDrawPoint.Color = color;
			myRenderMessageDebugDrawPoint.DepthRead = depthRead;
			myRenderMessageDebugDrawPoint.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawPoint);
		}

		public static void DebugDrawText2D(Vector2 screenCoord, string text, Color color, float scale, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, bool persistent = false)
		{
			MyRenderMessageDebugDrawText2D myRenderMessageDebugDrawText2D = MessagePool.Get<MyRenderMessageDebugDrawText2D>(MyRenderMessageEnum.DebugDrawText2D);
			myRenderMessageDebugDrawText2D.Coord = screenCoord;
			myRenderMessageDebugDrawText2D.Text = text;
			myRenderMessageDebugDrawText2D.Color = color;
			myRenderMessageDebugDrawText2D.Scale = scale;
			myRenderMessageDebugDrawText2D.Align = align;
			myRenderMessageDebugDrawText2D.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawText2D);
		}

		public static void DebugDrawText3D(Vector3D worldCoord, string text, Color color, float scale, bool depthRead, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, int customViewProjection = -1, bool persistent = false)
		{
			MyRenderMessageDebugDrawText3D myRenderMessageDebugDrawText3D = MessagePool.Get<MyRenderMessageDebugDrawText3D>(MyRenderMessageEnum.DebugDrawText3D);
			myRenderMessageDebugDrawText3D.Coord = worldCoord;
			myRenderMessageDebugDrawText3D.Text = text;
			myRenderMessageDebugDrawText3D.Color = color;
			myRenderMessageDebugDrawText3D.Scale = scale;
			myRenderMessageDebugDrawText3D.DepthRead = depthRead;
			myRenderMessageDebugDrawText3D.Align = align;
			myRenderMessageDebugDrawText3D.CustomViewProjection = customViewProjection;
			myRenderMessageDebugDrawText3D.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawText3D);
		}

		public static void DebugDrawSphere(Vector3D position, float radius, Color color, float alpha = 1f, bool depthRead = true, bool smooth = false, bool cull = true, bool persistent = false)
		{
			MyRenderMessageDebugDrawSphere myRenderMessageDebugDrawSphere = MessagePool.Get<MyRenderMessageDebugDrawSphere>(MyRenderMessageEnum.DebugDrawSphere);
			myRenderMessageDebugDrawSphere.Position = position;
			myRenderMessageDebugDrawSphere.Radius = radius;
			myRenderMessageDebugDrawSphere.Color = color;
			myRenderMessageDebugDrawSphere.Alpha = alpha;
			myRenderMessageDebugDrawSphere.DepthRead = depthRead;
			myRenderMessageDebugDrawSphere.Smooth = smooth;
			myRenderMessageDebugDrawSphere.Cull = cull;
			myRenderMessageDebugDrawSphere.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawSphere);
		}

		public static IMyDebugDrawBatchAabb DebugDrawBatchAABB(MatrixD worldMatrix, Color color, bool depthRead = true, bool shaded = true)
		{
			if (shaded)
			{
				return new MyDebugDrawBatchAabbShaded(PrepareDebugDrawTriangles(), ref worldMatrix, color, depthRead);
			}
			return new MyDebugDrawBatchAabbLines(DebugDrawLine3DOpenBatch(depthRead), ref worldMatrix, color, depthRead);
		}

		public static void DebugDrawAABB(BoundingBoxD aabb, Color color, float alpha = 1f, float scale = 1f, bool depthRead = true, bool shaded = false, bool persistent = false)
		{
			MyRenderMessageDebugDrawAABB myRenderMessageDebugDrawAABB = MessagePool.Get<MyRenderMessageDebugDrawAABB>(MyRenderMessageEnum.DebugDrawAABB);
			myRenderMessageDebugDrawAABB.AABB = aabb;
			myRenderMessageDebugDrawAABB.Color = color;
			myRenderMessageDebugDrawAABB.Alpha = alpha;
			myRenderMessageDebugDrawAABB.Scale = scale;
			myRenderMessageDebugDrawAABB.DepthRead = depthRead;
			myRenderMessageDebugDrawAABB.Shaded = shaded;
			myRenderMessageDebugDrawAABB.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawAABB);
		}

		public static void DebugDrawAxis(MatrixD matrix, float axisLength, bool depthRead, bool skipScale = false, bool persistent = false)
		{
			MyRenderMessageDebugDrawAxis myRenderMessageDebugDrawAxis = MessagePool.Get<MyRenderMessageDebugDrawAxis>(MyRenderMessageEnum.DebugDrawAxis);
			myRenderMessageDebugDrawAxis.Matrix = matrix;
			myRenderMessageDebugDrawAxis.AxisLength = axisLength;
			myRenderMessageDebugDrawAxis.DepthRead = depthRead;
			myRenderMessageDebugDrawAxis.SkipScale = skipScale;
			myRenderMessageDebugDrawAxis.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawAxis);
		}

		public static void DebugDrawOBB(MyOrientedBoundingBoxD obb, Color color, float alpha, bool depthRead, bool smooth, bool persistent = false)
		{
			MatrixD matrix = MatrixD.CreateFromQuaternion(obb.Orientation);
			matrix.Right *= obb.HalfExtent.X * 2.0;
			matrix.Up *= obb.HalfExtent.Y * 2.0;
			matrix.Forward *= obb.HalfExtent.Z * 2.0;
			matrix.Translation = obb.Center;
			DebugDrawOBB(matrix, color, alpha, depthRead, smooth, cull: true, persistent);
		}

		public static void DebugDraw6FaceConvex(Vector3D[] vertices, Color color, float alpha, bool depthRead, bool fill, bool persistent = false)
		{
			MyRenderMessageDebugDraw6FaceConvex myRenderMessageDebugDraw6FaceConvex = MessagePool.Get<MyRenderMessageDebugDraw6FaceConvex>(MyRenderMessageEnum.DebugDraw6FaceConvex);
			myRenderMessageDebugDraw6FaceConvex.Vertices = (Vector3D[])vertices.Clone();
			myRenderMessageDebugDraw6FaceConvex.Color = color;
			myRenderMessageDebugDraw6FaceConvex.Alpha = alpha;
			myRenderMessageDebugDraw6FaceConvex.DepthRead = depthRead;
			myRenderMessageDebugDraw6FaceConvex.Fill = fill;
			myRenderMessageDebugDraw6FaceConvex.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDraw6FaceConvex);
		}

		public static void DebugDrawCone(Vector3D translation, Vector3D directionVec, Vector3D baseVec, Color color, bool depthRead, bool persistent = false)
		{
			MyRenderMessageDebugDrawCone myRenderMessageDebugDrawCone = MessagePool.Get<MyRenderMessageDebugDrawCone>(MyRenderMessageEnum.DebugDrawCone);
			myRenderMessageDebugDrawCone.Translation = translation;
			myRenderMessageDebugDrawCone.DirectionVector = directionVec;
			myRenderMessageDebugDrawCone.BaseVector = baseVec;
			myRenderMessageDebugDrawCone.DepthRead = depthRead;
			myRenderMessageDebugDrawCone.Color = color;
			myRenderMessageDebugDrawCone.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawCone);
		}

		public static void DebugDrawOBB(MatrixD matrix, Color color, float alpha, bool depthRead, bool smooth, bool cull = true, bool persistent = false)
		{
			MyRenderMessageDebugDrawOBB myRenderMessageDebugDrawOBB = MessagePool.Get<MyRenderMessageDebugDrawOBB>(MyRenderMessageEnum.DebugDrawOBB);
			myRenderMessageDebugDrawOBB.Matrix = matrix;
			myRenderMessageDebugDrawOBB.Color = color;
			myRenderMessageDebugDrawOBB.Alpha = alpha;
			myRenderMessageDebugDrawOBB.DepthRead = depthRead;
			myRenderMessageDebugDrawOBB.Smooth = smooth;
			myRenderMessageDebugDrawOBB.Cull = cull;
			myRenderMessageDebugDrawOBB.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawOBB);
		}

		public static void DebugDrawCylinder(MatrixD worldMatrix, Vector3D vertexA, Vector3D vertexB, float radius, Color color, float alpha, bool depthRead, bool smooth, bool persistent = false)
		{
			Vector3 value = vertexB - vertexA;
			float num = value.Length();
			float num2 = 2f * radius;
			Matrix identity = Matrix.Identity;
			identity.Up = value / num;
			identity.Right = Vector3.CalculatePerpendicularVector(identity.Up);
			identity.Forward = Vector3.Cross(identity.Up, identity.Right);
			identity = Matrix.CreateScale(num2, num, num2) * identity;
			identity.Translation = (vertexA + vertexB) * 0.5;
			identity *= worldMatrix;
			DebugDrawCylinder(identity, color, alpha, depthRead, smooth, persistent);
		}

		public static void DebugDrawCylinder(Vector3D position, Quaternion orientation, float radius, float height, Color color, float alpha, bool depthRead, bool smooth, bool persistent = false)
		{
			MatrixD matrix = MatrixD.CreateFromQuaternion(orientation);
			matrix.Right *= (double)(2f * radius);
			matrix.Forward *= (double)(2f * radius);
			matrix.Up *= (double)height;
			matrix.Translation = position;
			DebugDrawCylinder(matrix, color, alpha, depthRead, smooth, persistent);
		}

		public static void DebugDrawCylinder(Vector3D position, QuaternionD orientation, double radius, double height, Color color, float alpha, bool depthRead, bool smooth, bool persistent = false)
		{
			MatrixD matrix = MatrixD.CreateFromQuaternion(orientation);
			matrix.Right *= 2.0 * radius;
			matrix.Forward *= 2.0 * radius;
			matrix.Up *= height;
			matrix.Translation = position;
			DebugDrawCylinder(matrix, color, alpha, depthRead, smooth, persistent);
		}

		public static void DebugDrawCylinder(MatrixD matrix, Color color, float alpha, bool depthRead, bool smooth, bool persistent = false)
		{
			MyRenderMessageDebugDrawCylinder myRenderMessageDebugDrawCylinder = MessagePool.Get<MyRenderMessageDebugDrawCylinder>(MyRenderMessageEnum.DebugDrawCylinder);
			myRenderMessageDebugDrawCylinder.Matrix = matrix;
			myRenderMessageDebugDrawCylinder.Color = color;
			myRenderMessageDebugDrawCylinder.Alpha = alpha;
			myRenderMessageDebugDrawCylinder.DepthRead = depthRead;
			myRenderMessageDebugDrawCylinder.Smooth = smooth;
			myRenderMessageDebugDrawCylinder.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawCylinder);
		}

		public static void DebugDrawTriangle(Vector3D vertex0, Vector3D vertex1, Vector3D vertex2, Color color, bool smooth, bool depthRead, bool persistent = false)
		{
			MyRenderMessageDebugDrawTriangle myRenderMessageDebugDrawTriangle = MessagePool.Get<MyRenderMessageDebugDrawTriangle>(MyRenderMessageEnum.DebugDrawTriangle);
			myRenderMessageDebugDrawTriangle.Vertex0 = vertex0;
			myRenderMessageDebugDrawTriangle.Vertex1 = vertex1;
			myRenderMessageDebugDrawTriangle.Vertex2 = vertex2;
			myRenderMessageDebugDrawTriangle.Color = color;
			myRenderMessageDebugDrawTriangle.DepthRead = depthRead;
			myRenderMessageDebugDrawTriangle.Smooth = smooth;
			myRenderMessageDebugDrawTriangle.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawTriangle);
		}

		public static void DebugDrawPlane(Vector3D position, Vector3 normal, Color color, bool depthRead, bool persistent = false)
		{
			MyRenderMessageDebugDrawPlane myRenderMessageDebugDrawPlane = MessagePool.Get<MyRenderMessageDebugDrawPlane>(MyRenderMessageEnum.DebugDrawPlane);
			myRenderMessageDebugDrawPlane.Position = position;
			myRenderMessageDebugDrawPlane.Normal = normal;
			myRenderMessageDebugDrawPlane.Color = color;
			myRenderMessageDebugDrawPlane.DepthRead = depthRead;
			myRenderMessageDebugDrawPlane.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawPlane);
		}

		public static uint DebugDrawMesh(List<MyFormatPositionColor> vertices, MatrixD worldMatrix, bool depthRead, bool shaded)
		{
			MyRenderMessageDebugDrawMesh myRenderMessageDebugDrawMesh = MessagePool.Get<MyRenderMessageDebugDrawMesh>(MyRenderMessageEnum.DebugDrawMesh);
			myRenderMessageDebugDrawMesh.ID = GetMessageId(ObjectType.DebugDrawMesh);
			myRenderMessageDebugDrawMesh.Vertices = vertices;
			myRenderMessageDebugDrawMesh.WorldMatrix = worldMatrix;
			myRenderMessageDebugDrawMesh.DepthRead = depthRead;
			myRenderMessageDebugDrawMesh.Shaded = shaded;
			EnqueueMessage(myRenderMessageDebugDrawMesh);
			return myRenderMessageDebugDrawMesh.ID;
		}

		public static void DebugDrawUpdateMesh(uint ID, List<MyFormatPositionColor> vertices, MatrixD worldMatrix, bool depthRead, bool shaded)
		{
			MyRenderMessageDebugDrawMesh myRenderMessageDebugDrawMesh = MessagePool.Get<MyRenderMessageDebugDrawMesh>(MyRenderMessageEnum.DebugDrawMesh);
			myRenderMessageDebugDrawMesh.ID = ID;
			myRenderMessageDebugDrawMesh.Vertices = vertices;
			myRenderMessageDebugDrawMesh.WorldMatrix = worldMatrix;
			myRenderMessageDebugDrawMesh.DepthRead = depthRead;
			myRenderMessageDebugDrawMesh.Shaded = shaded;
			EnqueueMessage(myRenderMessageDebugDrawMesh);
		}

		public static MyRenderMessageDebugDrawTriangles PrepareDebugDrawTriangles()
		{
			MyRenderMessageDebugDrawTriangles myRenderMessageDebugDrawTriangles = MessagePool.Get<MyRenderMessageDebugDrawTriangles>(MyRenderMessageEnum.DebugDrawTriangles);
			myRenderMessageDebugDrawTriangles.Color = Color.White;
			myRenderMessageDebugDrawTriangles.Indices.Clear();
			myRenderMessageDebugDrawTriangles.Vertices.Clear();
			return myRenderMessageDebugDrawTriangles;
		}

		public static void DebugDrawTriangles(IDrawTrianglesMessage msgInterface, MatrixD? worldMatrix = null, bool depthRead = true, bool shaded = true, bool overlayWireframe = false, bool persistent = false)
		{
			MyRenderMessageDebugDrawTriangles obj = (MyRenderMessageDebugDrawTriangles)msgInterface;
			obj.WorldMatrix = (worldMatrix ?? MatrixD.Identity);
			obj.DepthRead = depthRead;
			obj.Shaded = shaded;
			obj.Edges = (overlayWireframe || !shaded);
			obj.Persistent = persistent;
			EnqueueMessage(obj);
		}

		public static void DebugDrawCapsule(Vector3D p0, Vector3D p1, float radius, Color color, bool depthRead, bool shaded = false, bool persistent = false)
		{
			MyRenderMessageDebugDrawCapsule myRenderMessageDebugDrawCapsule = MessagePool.Get<MyRenderMessageDebugDrawCapsule>(MyRenderMessageEnum.DebugDrawCapsule);
			myRenderMessageDebugDrawCapsule.P0 = p0;
			myRenderMessageDebugDrawCapsule.P1 = p1;
			myRenderMessageDebugDrawCapsule.Radius = radius;
			myRenderMessageDebugDrawCapsule.Color = color;
			myRenderMessageDebugDrawCapsule.DepthRead = depthRead;
			myRenderMessageDebugDrawCapsule.Shaded = shaded;
			myRenderMessageDebugDrawCapsule.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawCapsule);
		}

		public static void DebugDrawModel(string model, MatrixD worldMatrix, Color color, bool depthRead, bool persistent = false)
		{
			MyRenderMessageDebugDrawModel myRenderMessageDebugDrawModel = MessagePool.Get<MyRenderMessageDebugDrawModel>(MyRenderMessageEnum.DebugDrawModel);
			myRenderMessageDebugDrawModel.Model = model;
			myRenderMessageDebugDrawModel.WorldMatrix = worldMatrix;
			myRenderMessageDebugDrawModel.Color = color;
			myRenderMessageDebugDrawModel.DepthRead = depthRead;
			myRenderMessageDebugDrawModel.Persistent = persistent;
			EnqueueMessage(myRenderMessageDebugDrawModel);
		}

		public static void DebugClearPersistentMessages()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageDebugClearPersistentMessages>(MyRenderMessageEnum.DebugClearPersistentMessages));
		}

		public static void DebugCrashRenderThread()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageDebugCrashRenderThread>(MyRenderMessageEnum.DebugCrashRenderThread));
		}

		/// <param name="timestep">Greter than zero: fixed timestep in seconds. If timestep is zero, the time step is computed</param>
		public static void SetFrameTimeStep(float timestepInSeconds = 0f)
		{
			MyRenderMessageSetFrameTimeStep myRenderMessageSetFrameTimeStep = MessagePool.Get<MyRenderMessageSetFrameTimeStep>(MyRenderMessageEnum.SetFrameTimeStep);
			myRenderMessageSetFrameTimeStep.TimeStepInSeconds = timestepInSeconds;
			EnqueueMessage(myRenderMessageSetFrameTimeStep);
		}

		public static void ResetRandomness(int? seed = null)
		{
			MyRenderMessageResetRandomness myRenderMessageResetRandomness = MessagePool.Get<MyRenderMessageResetRandomness>(MyRenderMessageEnum.ResetRandomness);
			myRenderMessageResetRandomness.Seed = seed;
			EnqueueMessage(myRenderMessageResetRandomness);
		}

		public static void CollectGarbage()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageCollectGarbage>(MyRenderMessageEnum.CollectGarbage));
		}

		public static void RequestVideoAdapters()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageVideoAdaptersRequest>(MyRenderMessageEnum.VideoAdaptersRequest));
		}

		public static void SendVideoAdapters(MyAdapterInfo[] adapters)
		{
			MyRenderMessageVideoAdaptersResponse myRenderMessageVideoAdaptersResponse = MessagePool.Get<MyRenderMessageVideoAdaptersResponse>(MyRenderMessageEnum.VideoAdaptersResponse);
			myRenderMessageVideoAdaptersResponse.Adapters = adapters;
			EnqueueOutputMessage(myRenderMessageVideoAdaptersResponse);
		}

		public static void SendCreatedDeviceSettings(MyRenderDeviceSettings settings)
		{
			MyRenderMessageCreatedDeviceSettings myRenderMessageCreatedDeviceSettings = MessagePool.Get<MyRenderMessageCreatedDeviceSettings>(MyRenderMessageEnum.CreatedDeviceSettings);
			myRenderMessageCreatedDeviceSettings.Settings = settings;
			EnqueueOutputMessage(myRenderMessageCreatedDeviceSettings);
		}

		public static void SwitchDeviceSettings(MyRenderDeviceSettings settings)
		{
			MyRenderMessageSwitchDeviceSettings myRenderMessageSwitchDeviceSettings = MessagePool.Get<MyRenderMessageSwitchDeviceSettings>(MyRenderMessageEnum.SwitchDeviceSettings);
			myRenderMessageSwitchDeviceSettings.Settings = settings;
			EnqueueMessage(myRenderMessageSwitchDeviceSettings);
		}

		public static void SwitchRenderSettings(MyRenderSettings settings)
		{
			MyRenderMessageSwitchRenderSettings myRenderMessageSwitchRenderSettings = MessagePool.Get<MyRenderMessageSwitchRenderSettings>(MyRenderMessageEnum.SwitchRenderSettings);
			myRenderMessageSwitchRenderSettings.Settings = settings;
			m_settingsDirty = false;
			EnqueueMessage(myRenderMessageSwitchRenderSettings);
		}

		public static void SwitchRenderSettings(MyRenderSettings1 settings)
		{
			Settings.User = settings;
			if (settings.GrassDensityFactor == 0f)
			{
				Settings.User.GrassDrawDistance = 0f;
			}
			SwitchRenderSettings(Settings);
		}

		public static void SwitchPostprocessSettings(ref MyPostprocessSettings settings)
		{
			MyRenderMessageUpdatePostprocessSettings myRenderMessageUpdatePostprocessSettings = MessagePool.Get<MyRenderMessageUpdatePostprocessSettings>(MyRenderMessageEnum.UpdatePostprocessSettings);
			myRenderMessageUpdatePostprocessSettings.Settings = settings;
			EnqueueMessage(myRenderMessageUpdatePostprocessSettings);
		}

		public static void SendClipmapsReady()
		{
			EnqueueOutputMessage(MessagePool.Get<MyRenderMessageClipmapsReady>(MyRenderMessageEnum.ClipmapsReady));
		}

		public static void SendTasksFinished()
		{
			EnqueueOutputMessage(MessagePool.Get<MyRenderMessageTasksFinished>(MyRenderMessageEnum.TasksFinished));
		}

		public static void EnqueueMainThreadCallback(Action callback)
		{
			MyRenderMessageMainThreadCallback myRenderMessageMainThreadCallback = MessagePool.Get<MyRenderMessageMainThreadCallback>(MyRenderMessageEnum.MainThreadCallback);
			myRenderMessageMainThreadCallback.Callback = callback;
			EnqueueOutputMessage(myRenderMessageMainThreadCallback);
		}

		public static uint CreateDecal(uint[] parentIds, ref MyDecalTopoData data, MyDecalFlags flags, string sourceTarget, string material, int matIndex)
		{
			MyRenderMessageCreateScreenDecal myRenderMessageCreateScreenDecal = MessagePool.Get<MyRenderMessageCreateScreenDecal>(MyRenderMessageEnum.CreateScreenDecal);
			myRenderMessageCreateScreenDecal.ID = GetMessageId(ObjectType.ScreenDecal, track: false);
			myRenderMessageCreateScreenDecal.ParentIDs = parentIds;
			myRenderMessageCreateScreenDecal.TopoData = data;
			myRenderMessageCreateScreenDecal.SourceTarget = sourceTarget;
			myRenderMessageCreateScreenDecal.Flags = flags;
			myRenderMessageCreateScreenDecal.Material = material;
			myRenderMessageCreateScreenDecal.MaterialIndex = matIndex;
			EnqueueMessage(myRenderMessageCreateScreenDecal);
			return myRenderMessageCreateScreenDecal.ID;
		}

		public static void UpdateDecals(List<MyDecalPositionUpdate> decals)
		{
			MyRenderMessageUpdateScreenDecal myRenderMessageUpdateScreenDecal = MessagePool.Get<MyRenderMessageUpdateScreenDecal>(MyRenderMessageEnum.UpdateScreenDecal);
			myRenderMessageUpdateScreenDecal.Decals.AddRange(decals);
			EnqueueMessage(myRenderMessageUpdateScreenDecal);
		}

		public static void RemoveDecal(uint decalId)
		{
			MyRenderMessageRemoveDecal myRenderMessageRemoveDecal = MessagePool.Get<MyRenderMessageRemoveDecal>(MyRenderMessageEnum.RemoveDecal);
			myRenderMessageRemoveDecal.ID = decalId;
			EnqueueMessage(myRenderMessageRemoveDecal);
		}

		public static void SetDecalGlobals(MyDecalGlobals globals)
		{
			MyRenderMessageSetDecalGlobals myRenderMessageSetDecalGlobals = MessagePool.Get<MyRenderMessageSetDecalGlobals>(MyRenderMessageEnum.SetDecalGlobals);
			myRenderMessageSetDecalGlobals.Globals = globals;
			EnqueueMessage(myRenderMessageSetDecalGlobals);
		}

		public static void RegisterDecals(Dictionary<string, List<MyDecalMaterialDesc>> descriptions)
		{
			MyRenderMessageRegisterScreenDecalsMaterials myRenderMessageRegisterScreenDecalsMaterials = MessagePool.Get<MyRenderMessageRegisterScreenDecalsMaterials>(MyRenderMessageEnum.RegisterDecalsMaterials);
			myRenderMessageRegisterScreenDecalsMaterials.MaterialDescriptions = descriptions;
			EnqueueMessage(myRenderMessageRegisterScreenDecalsMaterials);
		}

		public static void ClearDecals()
		{
			EnqueueMessage(MessagePool.Get<MyRenderMessageClearScreenDecals>(MyRenderMessageEnum.ClearDecals));
		}

		public static void UpdateDebugOverrides()
		{
			MyRenderMessageUpdateDebugOverrides myRenderMessageUpdateDebugOverrides = MessagePool.Get<MyRenderMessageUpdateDebugOverrides>(MyRenderMessageEnum.UpdateDebugOverrides);
			myRenderMessageUpdateDebugOverrides.Overrides = DebugOverrides.Clone();
			EnqueueMessage(myRenderMessageUpdateDebugOverrides);
		}

		public static void SetVisibilityUpdates(uint id, bool state)
		{
			MyRenderMessageSetVisibilityUpdates myRenderMessageSetVisibilityUpdates = MessagePool.Get<MyRenderMessageSetVisibilityUpdates>(MyRenderMessageEnum.SetVisibilityUpdates);
			myRenderMessageSetVisibilityUpdates.ID = id;
			myRenderMessageSetVisibilityUpdates.State = state;
			EnqueueMessage(myRenderMessageSetVisibilityUpdates);
		}

		public static string GetStatistics()
		{
			return m_render.GetStatistics();
		}

		public static void UpdatePlanetSettings(ref MyRenderPlanetSettings settings)
		{
			MyRenderMessageUpdatePlanetSettings myRenderMessageUpdatePlanetSettings = MessagePool.Get<MyRenderMessageUpdatePlanetSettings>(MyRenderMessageEnum.UpdatePlanetSettings);
			myRenderMessageUpdatePlanetSettings.Settings = settings;
			EnqueueMessage(myRenderMessageUpdatePlanetSettings);
		}

		/// <summary>
		/// Request that the level of detail for a given actor be updated instantaneously, instead of cross fading models.
		///
		/// This request remains until the next lod update and is then discarded.
		/// </summary>
		/// <param name="actor"></param>
		public static void UpdateLodImmediately(uint id)
		{
			MyRenderMessageUpdateLodImmediately myRenderMessageUpdateLodImmediately = MessagePool.Get<MyRenderMessageUpdateLodImmediately>(MyRenderMessageEnum.UpdateLodImmediately);
			myRenderMessageUpdateLodImmediately.Id = id;
			EnqueueMessage(myRenderMessageUpdateLodImmediately);
		}

		public static void SetGravityProvider(Func<Vector3D, Vector3> calculateGravityInPoint)
		{
			MyRenderMessageSetGravityProvider myRenderMessageSetGravityProvider = MessagePool.Get<MyRenderMessageSetGravityProvider>(MyRenderMessageEnum.SetGravityProvider);
			myRenderMessageSetGravityProvider.CalculateGravityInPoint = calculateGravityInPoint;
			EnqueueMessage(myRenderMessageSetGravityProvider);
		}

		public static void SetTimings(MyTimeSpan cpuDraw, MyTimeSpan cpuWait)
		{
			m_render.SetTimings(cpuDraw, cpuWait);
			CPULoadSmooth = MathHelper.Smooth(CPULoad = (float)(cpuDraw.Seconds / (cpuDraw.Seconds + cpuWait.Seconds) * 100.0), CPULoadSmooth);
			CPUTimeSmooth = MathHelper.Smooth((float)cpuDraw.Seconds * 1000f, CPUTimeSmooth);
		}

		public static MyBillboard AddPersistentBillboard()
		{
			return m_render.SharedData.AddPersistentBillboard();
		}

		public static void RemovePersistentBillboard(MyBillboard billboard)
		{
			m_render.SharedData.RemovePersistentBillboard(billboard);
		}

		public static void ApplyActionOnPersistentBillboards(Action<MyBillboard> a)
		{
			m_render.SharedData.ApplyActionOnPersistentBillboards(a);
		}

		public static Vector2 GetTextureSize(string name)
		{
			return m_render.GetTextureSize(name);
		}
	}
}
