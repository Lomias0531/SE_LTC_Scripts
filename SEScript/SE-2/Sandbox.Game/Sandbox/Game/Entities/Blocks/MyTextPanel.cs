using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Entity.UseObject;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TextPanel))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyTextPanel),
		typeof(Sandbox.ModAPI.Ingame.IMyTextPanel)
	})]
	public class MyTextPanel : MyFunctionalBlock, IMyTextPanelComponentOwner, Sandbox.ModAPI.IMyTextPanel, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextPanel, Sandbox.ModAPI.IMyTextSurfaceProvider, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
	{
		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_003C_0023_003E : ICallSite<MyTextPanel, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in int[] selection, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_003C_0023_003E : ICallSite<MyTextPanel, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in int[] selection, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(selection);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003EVRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyTextPanel, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in MySerializableSpriteCollection sprites, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(sprites);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyTextPanel, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected sealed class OnChangeTitle_003C_003ESystem_String_0023System_Boolean : ICallSite<MyTextPanel, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in string title, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTitle(title, isPublic);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyTextPanel, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyTextPanel, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyTextPanel_003C_003EActor : IActivator, IActivator<MyTextPanel>
		{
			private sealed override object CreateInstance()
			{
				return new MyTextPanel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTextPanel CreateInstance()
			{
				return new MyTextPanel();
			}

			MyTextPanel IActivator<MyTextPanel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const double MAX_DRAW_DISTANCE = 200.0;

		private readonly StringBuilder m_publicDescription = new StringBuilder();

		private readonly StringBuilder m_publicTitle = new StringBuilder();

		private readonly StringBuilder m_privateDescription = new StringBuilder();

		private readonly StringBuilder m_privateTitle = new StringBuilder();

		private bool m_isTextPanelOpen;

		private ulong m_userId;

		private MyGuiScreenTextPanel m_textBox;

		private int m_previousUpdateTime;

		private bool m_isOutofRange;

		private MyTextPanelComponent m_panelComponent;

		private bool m_isEditingPublic;

		private StringBuilder m_publicTitleHelper = new StringBuilder();

		private StringBuilder m_privateTitleHelper = new StringBuilder();

		private StringBuilder m_publicDescriptionHelper = new StringBuilder();

		private StringBuilder m_privateDescriptionHelper = new StringBuilder();

		public ContentType ContentType
		{
			get
			{
				return PanelComponent.ContentType;
			}
			set
			{
				PanelComponent.ContentType = value;
			}
		}

		public ShowTextOnScreenFlag ShowTextFlag
		{
			get
			{
				return PanelComponent.ShowTextFlag;
			}
			set
			{
				PanelComponent.ShowTextFlag = value;
			}
		}

		public bool ShowTextOnScreen => PanelComponent.ShowTextOnScreen;

		public MyTextPanelComponent PanelComponent => m_panelComponent;

		public StringBuilder PublicDescription
		{
			get
			{
				return m_publicDescription;
			}
			set
			{
				if (m_publicDescription.CompareUpdate(value))
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				if (m_publicDescriptionHelper != value)
				{
					m_publicDescriptionHelper.Clear().Append((object)value);
				}
			}
		}

		public StringBuilder PublicTitle
		{
			get
			{
				return m_publicTitle;
			}
			set
			{
				m_publicTitle.CompareUpdate(value);
				if (m_publicTitleHelper != value)
				{
					m_publicTitleHelper.Clear().Append((object)value);
				}
			}
		}

		public StringBuilder PrivateTitle
		{
			get
			{
				return m_privateTitle;
			}
			set
			{
				m_privateTitle.CompareUpdate(value);
				if (m_privateTitleHelper != value)
				{
					m_privateTitleHelper.Clear().Append((object)value);
				}
			}
		}

		public StringBuilder PrivateDescription
		{
			get
			{
				return m_privateDescription;
			}
			set
			{
				if (m_privateDescription.CompareUpdate(value))
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				if (m_privateDescriptionHelper != value)
				{
					m_privateDescriptionHelper.Clear().Append((object)value);
				}
			}
		}

		public bool IsTextPanelOpen
		{
			get
			{
				return m_isTextPanelOpen;
			}
			set
			{
				if (m_isTextPanelOpen != value)
				{
					m_isTextPanelOpen = value;
					RaisePropertiesChanged();
				}
			}
		}

		public ulong UserId
		{
			get
			{
				return m_userId;
			}
			set
			{
				m_userId = value;
			}
		}

		public Vector2 SurfaceSize => m_panelComponent.SurfaceSize;

		public Vector2 TextureSize => m_panelComponent.TextureSize;

		internal new MyRenderComponentTextPanel Render
		{
			get
			{
				return base.Render as MyRenderComponentTextPanel;
			}
			set
			{
				base.Render = value;
			}
		}

		public new MyTextPanelDefinition BlockDefinition => (MyTextPanelDefinition)base.BlockDefinition;

		public float FontSize
		{
			get
			{
				return m_panelComponent.FontSize;
			}
			set
			{
				m_panelComponent.FontSize = (float)Math.Round(value, 3);
			}
		}

		public Color FontColor
		{
			get
			{
				return m_panelComponent.FontColor;
			}
			set
			{
				m_panelComponent.FontColor = value;
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return m_panelComponent.BackgroundColor;
			}
			set
			{
				m_panelComponent.BackgroundColor = value;
			}
		}

		public byte BackgroundAlpha
		{
			get
			{
				return m_panelComponent.BackgroundAlpha;
			}
			set
			{
				m_panelComponent.BackgroundAlpha = value;
			}
		}

		public float ChangeInterval
		{
			get
			{
				return m_panelComponent.ChangeInterval;
			}
			set
			{
				m_panelComponent.ChangeInterval = (float)Math.Round(value, 3);
			}
		}

		ShowTextOnScreenFlag Sandbox.ModAPI.Ingame.IMyTextPanel.ShowOnScreen => ShowTextFlag;

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.ShowText => ShowTextOnScreen;

		string Sandbox.ModAPI.Ingame.IMyTextSurface.CurrentlyShownImage
		{
			get
			{
				if (PanelComponent.SelectedTexturesToDraw.Count == 0)
				{
					return null;
				}
				if (PanelComponent.CurrentSelectedTexture >= PanelComponent.SelectedTexturesToDraw.Count)
				{
					return PanelComponent.SelectedTexturesToDraw[0].Id.SubtypeName;
				}
				return PanelComponent.SelectedTexturesToDraw[PanelComponent.CurrentSelectedTexture].Id.SubtypeName;
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Font
		{
			get
			{
				return PanelComponent.Font.SubtypeName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value) != null)
				{
					PanelComponent.Font = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value).Id;
				}
			}
		}

		TextAlignment Sandbox.ModAPI.Ingame.IMyTextSurface.Alignment
		{
			get
			{
				if (m_panelComponent == null)
				{
					return TextAlignment.LEFT;
				}
				return m_panelComponent.Alignment;
			}
			set
			{
				if (m_panelComponent != null)
				{
					m_panelComponent.Alignment = value;
				}
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Script
		{
			get
			{
				if (m_panelComponent == null)
				{
					return string.Empty;
				}
				return m_panelComponent.Script;
			}
			set
			{
				if (m_panelComponent != null)
				{
					m_panelComponent.Script = value;
				}
			}
		}

		ContentType Sandbox.ModAPI.Ingame.IMyTextSurface.ContentType
		{
			get
			{
				return ContentType;
			}
			set
			{
				ContentType = value;
			}
		}

		Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.SurfaceSize => SurfaceSize;

		Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.TextureSize => TextureSize;

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.PreserveAspectRatio
		{
			get
			{
				if (m_panelComponent == null)
				{
					return false;
				}
				return m_panelComponent.PreserveAspectRatio;
			}
			set
			{
				if (m_panelComponent != null)
				{
					m_panelComponent.PreserveAspectRatio = value;
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyTextSurface.TextPadding
		{
			get
			{
				if (m_panelComponent == null)
				{
					return 0f;
				}
				return m_panelComponent.TextPadding;
			}
			set
			{
				if (m_panelComponent != null)
				{
					m_panelComponent.TextPadding = value;
				}
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptBackgroundColor
		{
			get
			{
				if (m_panelComponent == null)
				{
					return Color.White;
				}
				return m_panelComponent.ScriptBackgroundColor;
			}
			set
			{
				if (m_panelComponent != null)
				{
					m_panelComponent.ScriptBackgroundColor = value;
				}
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptForegroundColor
		{
			get
			{
				if (m_panelComponent == null)
				{
					return Color.White;
				}
				return m_panelComponent.ScriptForegroundColor;
			}
			set
			{
				if (m_panelComponent != null)
				{
					m_panelComponent.ScriptForegroundColor = value;
				}
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Name
		{
			get
			{
				if (m_panelComponent == null)
				{
					return null;
				}
				return m_panelComponent.Name;
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.DisplayName
		{
			get
			{
				if (m_panelComponent == null)
				{
					return null;
				}
				return m_panelComponent.DisplayName;
			}
		}

		int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => 1;

		public MyTextPanel()
		{
			CreateTerminalControls();
			m_isTextPanelOpen = false;
			m_privateDescription = new StringBuilder();
			m_privateTitle = new StringBuilder();
			Render = new MyRenderComponentTextPanel(this);
			Render.NeedsDraw = false;
			base.NeedsWorldMatrix = true;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (base.IsFunctional)
			{
				m_panelComponent.UpdateAfterSimulation(base.IsWorking, IsInRange(), PublicDescription);
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (base.IsBeingHacked)
			{
				PrivateDescription.Clear();
				SendChangeDescriptionMessage(PrivateDescription, isPublic: false);
			}
			base.ResourceSink.Update();
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			SetDetailedInfoDirty();
			UpdateIsWorking();
			if (Render != null)
			{
				UpdateScreen();
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStartWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			ComponentStack_IsFunctionalChanged();
			PanelComponent.Reset();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
			if (orAddCell.ParentCullObject != uint.MaxValue)
			{
				Render.SetParent(0, orAddCell.ParentCullObject, base.PositionComp.LocalMatrix);
			}
			PanelComponent.SetRender(Render);
			UpdateScreen();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyTextPanel>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyTextPanel>("Title", MySpaceTexts.BlockPropertyTitle_TextPanelPublicTitle, MySpaceTexts.Blank)
				{
					Getter = ((MyTextPanel x) => x.PublicTitle),
					Setter = delegate(MyTextPanel x, StringBuilder v)
					{
						x.SendChangeTitleMessage(v, isPublic: true);
					},
					SupportsMultipleBlocks = false
				});
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyTextPanel>());
				MyTextPanelComponent.CreateTerminalControls<MyTextPanel>();
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, () => (!base.Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId));
			base.ResourceSink = myResourceSinkComponent;
			m_panelComponent = new MyTextPanelComponent(0, this, BlockDefinition.PanelMaterialName, BlockDefinition.PanelMaterialName, BlockDefinition.TextureResolution, BlockDefinition.ScreenWidth, BlockDefinition.ScreenHeight);
			MyObjectBuilder_TextPanel myObjectBuilder_TextPanel = (MyObjectBuilder_TextPanel)objectBuilder;
			base.SyncType.Append(m_panelComponent);
			m_panelComponent.Init(myObjectBuilder_TextPanel?.Sprites ?? default(MySerializableSpriteCollection), null, SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			base.Init(objectBuilder, cubeGrid);
			if (myObjectBuilder_TextPanel != null)
			{
				PrivateTitle.Append(myObjectBuilder_TextPanel.Title);
				PrivateDescription.Append(myObjectBuilder_TextPanel.Description);
				PublicDescription.Append(MyStatControlText.SubstituteTexts(myObjectBuilder_TextPanel.PublicDescription));
				PublicTitle.Append(myObjectBuilder_TextPanel.PublicTitle);
				PanelComponent.CurrentSelectedTexture = myObjectBuilder_TextPanel.CurrentShownTexture;
				if (Sync.IsServer && Sync.Clients != null)
				{
					MyClientCollection clients = Sync.Clients;
					clients.ClientRemoved = (Action<ulong>)Delegate.Combine(clients.ClientRemoved, new Action<ulong>(TextPanel_ClientRemoved));
				}
				MyTextPanelComponent.ContentMetadata contentMetadata = default(MyTextPanelComponent.ContentMetadata);
				contentMetadata.ContentType = myObjectBuilder_TextPanel.ContentType;
				contentMetadata.BackgroundColor = myObjectBuilder_TextPanel.BackgroundColor;
				contentMetadata.ChangeInterval = MathHelper.Clamp(myObjectBuilder_TextPanel.ChangeInterval, 0f, BlockDefinition.MaxChangingSpeed);
				contentMetadata.PreserveAspectRatio = myObjectBuilder_TextPanel.PreserveAspectRatio;
				contentMetadata.TextPadding = myObjectBuilder_TextPanel.TextPadding;
				MyTextPanelComponent.ContentMetadata content = contentMetadata;
				MyTextPanelComponent.FontData fontData = default(MyTextPanelComponent.FontData);
				fontData.Alignment = (TextAlignment)myObjectBuilder_TextPanel.Alignment;
				fontData.Size = MathHelper.Clamp(myObjectBuilder_TextPanel.FontSize, BlockDefinition.MinFontSize, BlockDefinition.MaxFontSize);
				fontData.TextColor = myObjectBuilder_TextPanel.FontColor;
				MyTextPanelComponent.FontData font = fontData;
				MyTextPanelComponent.ScriptData scriptData = default(MyTextPanelComponent.ScriptData);
				scriptData.Script = (myObjectBuilder_TextPanel.SelectedScript ?? string.Empty);
				scriptData.CustomizeScript = myObjectBuilder_TextPanel.CustomizeScripts;
				scriptData.BackgroundColor = myObjectBuilder_TextPanel.ScriptBackgroundColor;
				scriptData.ForegroundColor = myObjectBuilder_TextPanel.ScriptForegroundColor;
				MyTextPanelComponent.ScriptData script = scriptData;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
				Render.NeedsDrawFromParent = true;
				if (!myObjectBuilder_TextPanel.Font.IsNull())
				{
					font.Name = myObjectBuilder_TextPanel.Font.SubtypeName;
				}
				if (myObjectBuilder_TextPanel.SelectedImages != null)
				{
					foreach (string selectedImage in myObjectBuilder_TextPanel.SelectedImages)
					{
						foreach (MyLCDTextureDefinition definition in PanelComponent.Definitions)
						{
							if (definition.Id.SubtypeName == selectedImage)
							{
								PanelComponent.SelectedTexturesToDraw.Add(definition);
								break;
							}
						}
					}
					PanelComponent.CurrentSelectedTexture = Math.Min(PanelComponent.CurrentSelectedTexture, PanelComponent.SelectedTexturesToDraw.Count);
					RaisePropertiesChanged();
				}
				if (myObjectBuilder_TextPanel.Version == 0)
				{
					if (myObjectBuilder_TextPanel.ContentType == ContentType.NONE && ((myObjectBuilder_TextPanel.SelectedImages != null && myObjectBuilder_TextPanel.SelectedImages.Count > 0) || myObjectBuilder_TextPanel.ShowText != 0 || myObjectBuilder_TextPanel.PublicDescription != string.Empty))
					{
						if (myObjectBuilder_TextPanel.ShowText != 0)
						{
							PanelComponent.SelectedTexturesToDraw.Clear();
						}
						else
						{
							PublicDescription.Clear();
						}
						content.ContentType = ContentType.TEXT_AND_IMAGE;
					}
					else if (myObjectBuilder_TextPanel.ContentType == ContentType.IMAGE)
					{
						content.ContentType = ContentType.TEXT_AND_IMAGE;
					}
					else
					{
						content.ContentType = myObjectBuilder_TextPanel.ContentType;
					}
				}
				PanelComponent.SetLocalValues(content, font, script);
			}
			base.ResourceSink.Update();
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TextPanel myObjectBuilder_TextPanel = (MyObjectBuilder_TextPanel)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_TextPanel.Description = m_privateDescription.ToString();
			myObjectBuilder_TextPanel.Title = m_privateTitle.ToString();
			myObjectBuilder_TextPanel.PublicDescription = m_publicDescription.ToString();
			myObjectBuilder_TextPanel.PublicTitle = m_publicTitle.ToString();
			myObjectBuilder_TextPanel.ChangeInterval = ChangeInterval;
			myObjectBuilder_TextPanel.Font = PanelComponent.Font;
			myObjectBuilder_TextPanel.FontSize = FontSize;
			myObjectBuilder_TextPanel.FontColor = FontColor;
			myObjectBuilder_TextPanel.BackgroundColor = BackgroundColor;
			myObjectBuilder_TextPanel.CurrentShownTexture = PanelComponent.CurrentSelectedTexture;
			myObjectBuilder_TextPanel.ShowText = ShowTextOnScreenFlag.NONE;
			myObjectBuilder_TextPanel.Alignment = (TextAlignmentEnum)PanelComponent.Alignment;
			myObjectBuilder_TextPanel.ContentType = ((PanelComponent.ContentType == ContentType.IMAGE) ? ContentType.TEXT_AND_IMAGE : PanelComponent.ContentType);
			myObjectBuilder_TextPanel.SelectedScript = PanelComponent.Script;
			myObjectBuilder_TextPanel.CustomizeScripts = PanelComponent.CustomizeScripts;
			myObjectBuilder_TextPanel.ScriptBackgroundColor = PanelComponent.ScriptBackgroundColor;
			myObjectBuilder_TextPanel.ScriptForegroundColor = PanelComponent.ScriptForegroundColor;
			myObjectBuilder_TextPanel.TextPadding = PanelComponent.TextPadding;
			myObjectBuilder_TextPanel.PreserveAspectRatio = PanelComponent.PreserveAspectRatio;
			myObjectBuilder_TextPanel.Version = 1;
			if (PanelComponent.SelectedTexturesToDraw.Count > 0)
			{
				myObjectBuilder_TextPanel.SelectedImages = new List<string>();
				foreach (MyLCDTextureDefinition item in PanelComponent.SelectedTexturesToDraw)
				{
					myObjectBuilder_TextPanel.SelectedImages.Add(item.Id.SubtypeName);
				}
			}
			myObjectBuilder_TextPanel.Sprites = PanelComponent.ExternalSprites;
			return myObjectBuilder_TextPanel;
		}

		public void Use(UseActionEnum actionEnum, VRage.ModAPI.IMyEntity entity)
		{
			if (m_isTextPanelOpen)
			{
				return;
			}
			MyCharacter myCharacter = entity as MyCharacter;
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId);
			if (base.OwnerId == 0L)
			{
				OnOwnerUse(actionEnum, myCharacter);
				return;
			}
			switch (userRelationToOwner)
			{
			case MyRelationsBetweenPlayerAndBlock.Neutral:
			case MyRelationsBetweenPlayerAndBlock.Enemies:
			case MyRelationsBetweenPlayerAndBlock.Friends:
				if (MySession.Static.Factions.TryGetPlayerFaction(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId) == MySession.Static.Factions.TryGetPlayerFaction(base.IDModule.Owner) && actionEnum == UseActionEnum.Manipulate)
				{
					OnFactionUse(actionEnum, myCharacter);
				}
				else
				{
					OnEnemyUse(actionEnum, myCharacter);
				}
				break;
			case MyRelationsBetweenPlayerAndBlock.NoOwnership:
			case MyRelationsBetweenPlayerAndBlock.FactionShare:
				if (base.OwnerId == 0L)
				{
					OnOwnerUse(actionEnum, myCharacter);
				}
				else
				{
					OnFactionUse(actionEnum, myCharacter);
				}
				break;
			case MyRelationsBetweenPlayerAndBlock.Owner:
				OnOwnerUse(actionEnum, myCharacter);
				break;
			}
		}

		private void OnEnemyUse(UseActionEnum actionEnum, MyCharacter user)
		{
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				OpenWindow(isEditable: false, sync: true, isPublic: true);
				break;
			case UseActionEnum.OpenTerminal:
				MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				break;
			}
		}

		private void OnFactionUse(UseActionEnum actionEnum, MyCharacter user)
		{
			bool flag = false;
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				if (GetUserRelationToOwner(user.GetPlayerIdentityId()) == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					OpenWindow(isEditable: true, sync: true, isPublic: true);
				}
				else
				{
					OpenWindow(isEditable: false, sync: true, isPublic: true);
				}
				break;
			case UseActionEnum.OpenTerminal:
				if (GetUserRelationToOwner(user.GetPlayerIdentityId()) == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, this);
				}
				else
				{
					flag = true;
				}
				break;
			}
			if (user.ControllerInfo.Controller.Player == MySession.Static.LocalHumanPlayer && flag)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.TextPanelReadOnly);
			}
		}

		private void OnOwnerUse(UseActionEnum actionEnum, MyCharacter user)
		{
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				OpenWindow(isEditable: true, sync: true, isPublic: true);
				break;
			case UseActionEnum.OpenTerminal:
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, this);
				break;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (PanelComponent != null)
			{
				PanelComponent.SetRender(null);
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (Sync.IsServer && Sync.Clients != null)
			{
				MyClientCollection clients = Sync.Clients;
				clients.ClientRemoved = (Action<ulong>)Delegate.Remove(clients.ClientRemoved, new Action<ulong>(TextPanel_ClientRemoved));
			}
		}

		private void TextPanel_ClientRemoved(ulong playerId)
		{
			if (playerId == m_userId)
			{
				SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
			}
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
		}

		public bool IsInRange()
		{
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera == null)
			{
				return false;
			}
			return Vector3D.Distance((MatrixD.CreateTranslation(base.PositionComp.LocalVolume.Center) * base.WorldMatrix).Translation, mainCamera.Position) < (double)MyMultiTextPanelComponent.GetDrawDistanceForQuality(MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.TextureQuality);
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (m_panelComponent != null)
			{
				m_panelComponent.Reset();
			}
			if (base.ResourceSink != null)
			{
				UpdateScreen();
			}
			if (CheckIsWorking() && ShowTextOnScreen)
			{
				Render.UpdateModelProperties();
			}
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			m_isEditingPublic = isPublic;
			CreateTextBox(isEditable, isPublic ? PublicDescription : PrivateDescription, isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBox);
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string missionTitle = isPublic ? m_publicTitle.ToString() : m_privateTitle.ToString();
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBox = new MyGuiScreenTextPanel(missionTitle, "", "", description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBox != null)
			{
				if (m_textBox.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(m_isEditingPublic);
				}
			}
		}

		public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBox.Description.Text.Remove(100000, m_textBox.Description.Text.Length - 100000);
				CloseWindow(m_isEditingPublic);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBox.Description.Text, m_isEditingPublic);
				MyScreenManager.AddScreen(m_textBox);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			MySession.Static.Gpss.ScanText(m_textBox.Description.Text.ToString(), PublicTitle);
			foreach (MySlimBlock cubeBlock in base.CubeGrid.CubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == base.EntityId)
				{
					SendChangeDescriptionMessage(m_textBox.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}

		public void UpdateScreen()
		{
			if (m_panelComponent != null)
			{
				m_panelComponent.UpdateAfterSimulation(CheckIsWorking(), IsInRange(), PublicDescription);
			}
		}

		private void SendRemoveSelectedImageRequest(Sandbox.ModAPI.IMyTextSurface panel, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnRemoveSelectedImageRequest, selection);
		}

		[Event(null, 780)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int[] selection)
		{
			PanelComponent.RemoveItems(selection);
		}

		private void SendAddImagesToSelectionRequest(Sandbox.ModAPI.IMyTextSurface panel, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnSelectImageRequest, selection);
		}

		[Event(null, 791)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int[] selection)
		{
			PanelComponent.SelectItems(selection);
		}

		private void ChangeTextRequest(MyTextPanelComponent panel, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeDescription, text, arg3: true);
		}

		private void UpdateSpriteCollection(MyTextPanelComponent panel, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnUpdateSpriteCollection, sprites);
			}
		}

		[Event(null, 810)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnUpdateSpriteCollection(MySerializableSpriteCollection sprites)
		{
			m_panelComponent?.UpdateSpriteCollection(sprites);
		}

		[Event(null, 816)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			if (isPublic)
			{
				PublicDescription = stringBuilder;
			}
			else
			{
				PrivateDescription = stringBuilder;
			}
		}

		[Event(null, 831)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTitle(string title, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(title);
			if (isPublic)
			{
				PublicTitle = stringBuilder;
			}
			else
			{
				PrivateTitle = stringBuilder;
			}
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 851)]
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		[Event(null, 862)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			UserId = user;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				if (isPublic)
				{
					PublicDescription = description;
				}
				else
				{
					PrivateDescription = description;
				}
			}
			else if (!(description.CompareTo(PublicDescription) == 0 && isPublic) && (description.CompareTo(PrivateDescription) != 0 || isPublic))
			{
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		private void SendChangeTitleMessage(StringBuilder title, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				if (isPublic)
				{
					PublicTitle = title;
				}
				else
				{
					PrivateTitle = title;
				}
			}
			else if (!(title.CompareTo(PublicTitle) == 0 && isPublic) && (title.CompareTo(PrivateTitle) != 0 || isPublic))
			{
				if (isPublic)
				{
					PublicTitle = title;
				}
				else
				{
					PrivateTitle = title;
				}
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeTitle, title.ToString(), isPublic);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowPrivateTextOnScreen()
		{
			ShowTextFlag = ShowTextOnScreenFlag.PRIVATE;
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowPublicTextOnScreen()
		{
			ContentType = ContentType.TEXT_AND_IMAGE;
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowTextureOnScreen()
		{
			ContentType = ContentType.TEXT_AND_IMAGE;
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.SetShowOnScreen(ShowTextOnScreenFlag set)
		{
			ShowTextFlag = set;
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPublicTitle()
		{
			return m_publicTitleHelper.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicTitle(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_publicTitleHelper.Clear();
			}
			m_publicTitleHelper.Append(value);
			SendChangeTitleMessage(m_publicTitleHelper, isPublic: true);
			return true;
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicText(string value, bool append)
		{
			return ((Sandbox.ModAPI.Ingame.IMyTextSurface)this).WriteText(value, append);
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPublicText()
		{
			return ((Sandbox.ModAPI.Ingame.IMyTextSurface)this).GetText();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicText(StringBuilder value, bool append)
		{
			return ((Sandbox.ModAPI.Ingame.IMyTextSurface)this).WriteText(value, append);
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ReadPublicText(StringBuilder buffer, bool append)
		{
			((Sandbox.ModAPI.Ingame.IMyTextSurface)this).ReadText(buffer, append);
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePrivateTitle(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_privateTitleHelper.Clear();
			}
			m_privateTitleHelper.Append(value);
			SendChangeTitleMessage(m_privateTitleHelper, isPublic: false);
			return true;
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPrivateTitle()
		{
			return m_privateTitle.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePrivateText(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_privateDescriptionHelper.Clear();
			}
			m_privateDescriptionHelper.Append(value);
			SendChangeDescriptionMessage(m_privateDescriptionHelper, isPublic: false);
			return true;
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPrivateText()
		{
			return m_privateDescription.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_publicDescriptionHelper.Clear();
			}
			if (value.Length + m_publicDescriptionHelper.Length > 100000)
			{
				value = value.Remove(100000 - m_publicDescriptionHelper.Length);
			}
			m_publicDescriptionHelper.Append(value);
			SendChangeDescriptionMessage(m_publicDescriptionHelper, isPublic: true);
			return true;
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.GetText()
		{
			return m_publicDescription.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(StringBuilder value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_publicDescriptionHelper.Clear();
			}
			m_publicDescriptionHelper.Append((object)value);
			SendChangeDescriptionMessage(m_publicDescriptionHelper, isPublic: true);
			return true;
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.ReadText(StringBuilder buffer, bool append)
		{
			if (!append)
			{
				buffer.Clear();
			}
			buffer.AppendStringBuilder(m_publicDescription);
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImageToSelection(string id, bool checkExistence)
		{
			if (id == null)
			{
				return;
			}
			int num = 0;
			while (true)
			{
				if (num < PanelComponent.Definitions.Count)
				{
					if (PanelComponent.Definitions[num].Id.SubtypeName == id)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			if (checkExistence)
			{
				for (int i = 0; i < PanelComponent.SelectedTexturesToDraw.Count; i++)
				{
					if (PanelComponent.SelectedTexturesToDraw[i].Id.SubtypeName == id)
					{
						return;
					}
				}
			}
			SendAddImagesToSelectionRequest(this, new int[1]
			{
				num
			});
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImagesToSelection(List<string> ids, bool checkExistence)
		{
			if (ids != null)
			{
				List<int> list = new List<int>();
				foreach (string id in ids)
				{
					for (int i = 0; i < PanelComponent.Definitions.Count; i++)
					{
						if (PanelComponent.Definitions[i].Id.SubtypeName == id)
						{
							bool flag = false;
							if (checkExistence)
							{
								for (int j = 0; j < PanelComponent.SelectedTexturesToDraw.Count; j++)
								{
									if (PanelComponent.SelectedTexturesToDraw[j].Id.SubtypeName == id)
									{
										flag = true;
										break;
									}
								}
							}
							if (!flag)
							{
								list.Add(i);
							}
							break;
						}
					}
				}
				if (list.Count > 0)
				{
					SendAddImagesToSelectionRequest(this, list.ToArray());
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImageFromSelection(string id, bool removeDuplicates)
		{
			if (id == null)
			{
				return;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < PanelComponent.Definitions.Count; i++)
			{
				if (!(PanelComponent.Definitions[i].Id.SubtypeName == id))
				{
					continue;
				}
				if (removeDuplicates)
				{
					for (int j = 0; j < PanelComponent.SelectedTexturesToDraw.Count; j++)
					{
						if (PanelComponent.SelectedTexturesToDraw[j].Id.SubtypeName == id)
						{
							list.Add(i);
						}
					}
				}
				else
				{
					list.Add(i);
				}
				break;
			}
			if (list.Count > 0)
			{
				SendRemoveSelectedImageRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImagesFromSelection(List<string> ids, bool removeDuplicates)
		{
			if (ids != null)
			{
				List<int> list = new List<int>();
				foreach (string id in ids)
				{
					for (int i = 0; i < PanelComponent.Definitions.Count; i++)
					{
						if (PanelComponent.Definitions[i].Id.SubtypeName == id)
						{
							if (removeDuplicates)
							{
								for (int j = 0; j < PanelComponent.SelectedTexturesToDraw.Count; j++)
								{
									if (PanelComponent.SelectedTexturesToDraw[j].Id.SubtypeName == id)
									{
										list.Add(i);
									}
								}
							}
							else
							{
								list.Add(i);
							}
							break;
						}
					}
				}
				if (list.Count > 0)
				{
					SendRemoveSelectedImageRequest(this, list.ToArray());
				}
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.ClearImagesFromSelection()
		{
			if (PanelComponent.SelectedTexturesToDraw.Count == 0)
			{
				return;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < PanelComponent.SelectedTexturesToDraw.Count; i++)
			{
				for (int j = 0; j < PanelComponent.Definitions.Count; j++)
				{
					if (PanelComponent.Definitions[j].Id.SubtypeName == PanelComponent.SelectedTexturesToDraw[i].Id.SubtypeName)
					{
						list.Add(j);
						break;
					}
				}
			}
			SendRemoveSelectedImageRequest(this, list.ToArray());
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetSelectedImages(List<string> output)
		{
			foreach (MyLCDTextureDefinition item in PanelComponent.SelectedTexturesToDraw)
			{
				output.Add(item.Id.SubtypeName);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetFonts(List<string> fonts)
		{
			if (fonts != null)
			{
				foreach (MyFontDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyFontDefinition>())
				{
					fonts.Add(definition.Id.SubtypeName);
				}
			}
		}

		public void GetSprites(List<string> sprites)
		{
			PanelComponent.GetSprites(sprites);
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetScripts(List<string> scripts)
		{
			if (m_panelComponent != null)
			{
				m_panelComponent.GetScripts(scripts);
			}
		}

		MySpriteDrawFrame Sandbox.ModAPI.Ingame.IMyTextSurface.DrawFrame()
		{
			if (m_panelComponent != null)
			{
				return m_panelComponent.DrawFrame();
			}
			return new MySpriteDrawFrame(null);
		}

		public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
		{
			return MyGuiManager.MeasureStringRaw(font, text, scale);
		}

		Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (index != 0)
			{
				return null;
			}
			return m_panelComponent;
		}
	}
}
