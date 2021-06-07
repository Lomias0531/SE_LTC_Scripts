using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.GUI.TextPanel;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MySerializedTextPanelData
	{
		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EChangeInterval_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in float value)
			{
				owner.ChangeInterval = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out float value)
			{
				value = owner.ChangeInterval;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003ESelectedImages_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in List<string> value)
			{
				owner.SelectedImages = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out List<string> value)
			{
				value = owner.SelectedImages;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EFont_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in SerializableDefinitionId value)
			{
				owner.Font = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out SerializableDefinitionId value)
			{
				value = owner.Font;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EFontSize_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in float value)
			{
				owner.FontSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out float value)
			{
				value = owner.FontSize;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EText_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EShowText_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, ShowTextOnScreenFlag>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in ShowTextOnScreenFlag value)
			{
				owner.ShowText = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out ShowTextOnScreenFlag value)
			{
				value = owner.ShowText;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EFontColor_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in Color value)
			{
				owner.FontColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out Color value)
			{
				value = owner.FontColor;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EBackgroundColor_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in Color value)
			{
				owner.BackgroundColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out Color value)
			{
				value = owner.BackgroundColor;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003ECurrentShownTexture_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in int value)
			{
				owner.CurrentShownTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out int value)
			{
				value = owner.CurrentShownTexture;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EAlignment_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in int value)
			{
				owner.Alignment = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out int value)
			{
				value = owner.Alignment;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EContentType_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, ContentType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in ContentType value)
			{
				owner.ContentType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out ContentType value)
			{
				value = owner.ContentType;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003ESelectedScript_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in string value)
			{
				owner.SelectedScript = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out string value)
			{
				value = owner.SelectedScript;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003ETextPadding_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in float value)
			{
				owner.TextPadding = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out float value)
			{
				value = owner.TextPadding;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EPreserveAspectRatio_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in bool value)
			{
				owner.PreserveAspectRatio = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out bool value)
			{
				value = owner.PreserveAspectRatio;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003ECustomizeScripts_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in bool value)
			{
				owner.CustomizeScripts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out bool value)
			{
				value = owner.CustomizeScripts;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EScriptBackgroundColor_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in Color value)
			{
				owner.ScriptBackgroundColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out Color value)
			{
				value = owner.ScriptBackgroundColor;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EScriptForegroundColor_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, Color>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in Color value)
			{
				owner.ScriptForegroundColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out Color value)
			{
				value = owner.ScriptForegroundColor;
			}
		}

		protected class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003ESprites_003C_003EAccessor : IMemberAccessor<MySerializedTextPanelData, MySerializableSpriteCollection>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySerializedTextPanelData owner, in MySerializableSpriteCollection value)
			{
				owner.Sprites = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySerializedTextPanelData owner, out MySerializableSpriteCollection value)
			{
				value = owner.Sprites;
			}
		}

		private class VRage_Game_ObjectBuilders_MySerializedTextPanelData_003C_003EActor : IActivator, IActivator<MySerializedTextPanelData>
		{
			private sealed override object CreateInstance()
			{
				return new MySerializedTextPanelData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySerializedTextPanelData CreateInstance()
			{
				return new MySerializedTextPanelData();
			}

			MySerializedTextPanelData IActivator<MySerializedTextPanelData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public float ChangeInterval;

		[ProtoMember(4)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<string> SelectedImages;

		[ProtoMember(7)]
		public SerializableDefinitionId Font = new MyDefinitionId(typeof(MyObjectBuilder_FontDefinition), "Debug");

		[ProtoMember(10)]
		public float FontSize = 1f;

		[ProtoMember(13)]
		[DefaultValue("")]
		public string Text = "";

		[ProtoMember(16)]
		public ShowTextOnScreenFlag ShowText;

		[ProtoMember(19)]
		public Color FontColor = Color.White;

		[ProtoMember(22)]
		public Color BackgroundColor = Color.Black;

		[ProtoMember(25)]
		public int CurrentShownTexture;

		[ProtoMember(28, IsRequired = false)]
		[DefaultValue(0)]
		public int Alignment;

		[ProtoMember(31)]
		[DefaultValue(ContentType.NONE)]
		public ContentType ContentType;

		[ProtoMember(34)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string SelectedScript;

		[ProtoMember(37)]
		public float TextPadding = 2f;

		[ProtoMember(40)]
		[DefaultValue(false)]
		public bool PreserveAspectRatio;

		[ProtoMember(43)]
		[DefaultValue(false)]
		public bool CustomizeScripts;

		[ProtoMember(46)]
		public Color ScriptBackgroundColor = new Color(0, 88, 151);

		[ProtoMember(49)]
		public Color ScriptForegroundColor = new Color(179, 237, 255);

		[ProtoMember(54)]
		public MySerializableSpriteCollection Sprites;
	}
}
