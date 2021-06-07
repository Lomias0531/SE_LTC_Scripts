using System.Collections.Generic;
using System.Text;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyTextSurface
	{
		/// <summary>
		/// The image that is currently shown on the screen.
		///
		/// Returns NULL if there are no images selected OR the screen is in text mode.
		/// </summary>
		string CurrentlyShownImage
		{
			get;
		}

		/// <summary>
		/// Gets or sets font size
		/// </summary>
		float FontSize
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets font color
		/// </summary>
		Color FontColor
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets background color
		/// </summary>
		Color BackgroundColor
		{
			get;
			set;
		}

		/// <summary>
		/// Value for offscreen texture alpha channel
		/// - for PBR material it is metalness (should be 0)
		/// - for transparent texture it is opacity
		/// </summary>
		byte BackgroundAlpha
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the change interval for selected textures
		/// </summary>
		float ChangeInterval
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the font
		/// </summary>
		string Font
		{
			get;
			set;
		}

		/// <summary>
		/// How should the text be aligned
		/// </summary>
		TextAlignment Alignment
		{
			get;
			set;
		}

		/// <summary>
		/// Currently running script
		/// </summary>
		string Script
		{
			get;
			set;
		}

		/// <summary>
		/// Type of content to be displayed on the screen.
		/// </summary>
		ContentType ContentType
		{
			get;
			set;
		}

		/// <summary>
		/// Size of the drawing surface.
		/// </summary>
		Vector2 SurfaceSize
		{
			get;
		}

		/// <summary>
		/// Size of the texture the drawing surface is rendered to.
		/// </summary>
		Vector2 TextureSize
		{
			get;
		}

		/// <summary>
		/// Preserve aspect ratio of images.
		/// </summary>
		bool PreserveAspectRatio
		{
			get;
			set;
		}

		/// <summary>
		/// Text padding from all sides of the panel.
		/// </summary>
		float TextPadding
		{
			get;
			set;
		}

		/// <summary>
		/// Background color used for scripts.
		/// </summary>
		Color ScriptBackgroundColor
		{
			get;
			set;
		}

		/// <summary>
		/// Foreground color used for scripts.
		/// </summary>
		Color ScriptForegroundColor
		{
			get;
			set;
		}

		/// <summary>
		/// Identifier name of this surface.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Localized name of this surface.
		/// </summary>
		string DisplayName
		{
			get;
		}

		bool WriteText(string value, bool append = false);

		string GetText();

		bool WriteText(StringBuilder value, bool append = false);

		void ReadText(StringBuilder buffer, bool append = false);

		void AddImageToSelection(string id, bool checkExistence = false);

		void AddImagesToSelection(List<string> ids, bool checkExistence = false);

		void RemoveImageFromSelection(string id, bool removeDuplicates = false);

		void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false);

		void ClearImagesFromSelection();

		/// <summary>
		/// Outputs the selected image ids to the specified list.
		///
		/// NOTE: List is not cleared internally.
		/// </summary>
		/// <param name="output"></param>
		void GetSelectedImages(List<string> output);

		/// <summary>
		/// Gets a list of available fonts
		/// </summary>
		/// <param name="fonts"></param>
		void GetFonts(List<string> fonts);

		/// <summary>
		/// Gets a list of available sprites
		/// </summary>
		void GetSprites(List<string> sprites);

		/// <summary>
		/// Gets a list of available scripts
		/// </summary>
		/// <param name="scripts"></param>
		void GetScripts(List<string> scripts);

		/// <summary>
		/// Creates a new draw frame where you can add sprites to be rendered.
		/// </summary>
		/// <returns></returns>
		MySpriteDrawFrame DrawFrame();

		/// <summary>
		/// Calculates how many pixels a string of a given font and scale will take up.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="font"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale);
	}
}
