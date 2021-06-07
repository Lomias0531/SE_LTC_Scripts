using System;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace VRage.Game
{
	[GenerateActivator]
	[MyDefinitionType(typeof(MyObjectBuilder_DefinitionBase), null)]
	public class MyDefinitionBase
	{
		private class VRage_Game_MyDefinitionBase_003C_003EActor : IActivator, IActivator<MyDefinitionBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyDefinitionBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDefinitionBase CreateInstance()
			{
				return new MyDefinitionBase();
			}

			MyDefinitionBase IActivator<MyDefinitionBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId Id;

		/// <summary>
		/// Enum used for localization of display name. Null for player created definitions.
		/// </summary>
		public MyStringId? DisplayNameEnum;

		/// <summary>
		/// Enum used for localization of description. Null for player created definitions.
		/// </summary>
		public MyStringId? DescriptionEnum;

		/// <summary>
		/// String name used for user created definitions which do not have localization support.
		/// </summary>
		public string DisplayNameString;

		/// <summary>
		/// String used for user created description which do not have localization support.
		/// </summary>
		public string DescriptionString;

		/// <summary>
		/// String used for shortcuts used in description
		/// </summary>
		public string DescriptionArgs;

		/// <summary>
		/// Icons for the definition, they are used from top to bottom.
		/// </summary>
		public string[] Icons;

		/// <summary>
		/// Definition can be disabled by mod, then it will be removed from definition manager
		/// </summary>
		public bool Enabled = true;

		/// <summary>
		/// Indicates if definition should be offered in Cube builder
		/// </summary>
		public bool Public = true;

		public bool AvailableInSurvival;

		public MyModContext Context;

		public string[] DLCs
		{
			get;
			private set;
		}

		/// <summary>
		/// Use this property when showing name in GUI instead of DisplayName. This takes into
		/// account more complex name construction.
		/// </summary>
		public virtual string DisplayNameText
		{
			get
			{
				if (!DisplayNameEnum.HasValue)
				{
					return DisplayNameString;
				}
				return MyTexts.GetString(DisplayNameEnum.Value);
			}
		}

		/// <summary>
		/// Use this property when showing description in GUI, as it takes into account more
		/// complex description construction.
		/// </summary>
		public virtual string DescriptionText
		{
			get
			{
				if (!DescriptionEnum.HasValue)
				{
					return DescriptionString;
				}
				return MyTexts.GetString(DescriptionEnum.Value);
			}
		}

		public void Init(MyObjectBuilder_DefinitionBase builder, MyModContext modContext)
		{
			Context = modContext;
			Init(builder);
		}

		protected virtual void Init(MyObjectBuilder_DefinitionBase builder)
		{
			Id = builder.Id;
			Public = builder.Public;
			Enabled = builder.Enabled;
			AvailableInSurvival = builder.AvailableInSurvival;
			Icons = builder.Icons;
			DescriptionArgs = builder.DescriptionArgs;
			DLCs = builder.DLCs;
			if (builder.DisplayName != null && builder.DisplayName.StartsWith("DisplayName_"))
			{
				DisplayNameEnum = MyStringId.GetOrCompute(builder.DisplayName);
			}
			else
			{
				DisplayNameString = builder.DisplayName;
			}
			if (builder.Description != null && builder.Description.StartsWith("Description_"))
			{
				DescriptionEnum = MyStringId.GetOrCompute(builder.Description);
			}
			else
			{
				DescriptionString = builder.Description;
			}
		}

		/// <summary>
		/// Override this in case you want to do some postprocessing of the definition before the game starts.
		///
		/// TODO: Obsolete me
		/// <para>Postprocess is useful if you want to process the definition before the game begins,</para>
		/// <para>but you only want to do it when all the definitions are loaded and merged.</para>
		/// </summary>
		[Obsolete("Prefer to use MyDefinitionPostprocessor instead.")]
		public virtual void Postprocess()
		{
		}

		public void Save(string filepath)
		{
			GetObjectBuilder().Save(filepath);
		}

		public virtual MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_DefinitionBase myObjectBuilder_DefinitionBase = MyDefinitionManagerBase.GetObjectFactory().CreateObjectBuilder<MyObjectBuilder_DefinitionBase>(this);
			myObjectBuilder_DefinitionBase.Id = Id;
			myObjectBuilder_DefinitionBase.Description = (DescriptionEnum.HasValue ? DescriptionEnum.Value.ToString() : ((DescriptionString != null) ? DescriptionString.ToString() : null));
			myObjectBuilder_DefinitionBase.DisplayName = (DisplayNameEnum.HasValue ? DisplayNameEnum.Value.ToString() : ((DisplayNameString != null) ? DisplayNameString.ToString() : null));
			myObjectBuilder_DefinitionBase.Icons = Icons;
			myObjectBuilder_DefinitionBase.Public = Public;
			myObjectBuilder_DefinitionBase.Enabled = Enabled;
			myObjectBuilder_DefinitionBase.DescriptionArgs = DescriptionArgs;
			myObjectBuilder_DefinitionBase.AvailableInSurvival = AvailableInSurvival;
			return myObjectBuilder_DefinitionBase;
		}

		public override string ToString()
		{
			return Id.ToString();
		}
	}
}
