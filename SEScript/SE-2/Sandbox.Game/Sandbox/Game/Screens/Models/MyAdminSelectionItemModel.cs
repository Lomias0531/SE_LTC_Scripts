using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.Models
{
	public class MyAdminSelectionItemModel : BindableBase
	{
		public static readonly int NAME_MAX_ALLOWED_LENGTH = 25;

		public static readonly int NAME_MAX_CUT_OFF_LENGTH = 11;

		public static readonly int FORMATER_ADDED_LENGTH = 3;

		private string m_namePrefix;

		private string m_nameSuffix;

		private long m_id;

		public string NamePrefix
		{
			get
			{
				return m_namePrefix;
			}
			set
			{
				SetProperty(ref m_namePrefix, value, "NamePrefix");
				RaisePropertyChanged("NameCombined");
			}
		}

		public string NameSuffix
		{
			get
			{
				return m_nameSuffix;
			}
			set
			{
				SetProperty(ref m_nameSuffix, value, "NameSuffix");
				RaisePropertyChanged("NameCombined");
				RaisePropertyChanged("NameCombinedShort");
			}
		}

		public long Id
		{
			get
			{
				return m_id;
			}
			set
			{
				SetProperty(ref m_id, value, "Id");
			}
		}

		public string NameCombined
		{
			get
			{
				if (string.IsNullOrEmpty(NamePrefix))
				{
					if (string.IsNullOrEmpty(NameSuffix))
					{
						return string.Empty;
					}
					return NameSuffix;
				}
				if (string.IsNullOrEmpty(NameSuffix))
				{
					return NamePrefix;
				}
				return $"{NamePrefix} - {NameSuffix}";
			}
		}

		public string NameCombinedShort
		{
			get
			{
				if (string.IsNullOrEmpty(NamePrefix))
				{
					if (string.IsNullOrEmpty(NameSuffix))
					{
						return string.Empty;
					}
					return NameSuffix;
				}
				if (string.IsNullOrEmpty(NameSuffix))
				{
					return NamePrefix;
				}
				if (NamePrefix.Length + NameSuffix.Length + FORMATER_ADDED_LENGTH <= NAME_MAX_ALLOWED_LENGTH)
				{
					return $"{NamePrefix} - {NameSuffix}";
				}
				if (NamePrefix.Length < NAME_MAX_CUT_OFF_LENGTH)
				{
					int length = NAME_MAX_ALLOWED_LENGTH - FORMATER_ADDED_LENGTH - NamePrefix.Length;
					return $"{NamePrefix} - {NameSuffix.Substring(0, length)}";
				}
				if (NameSuffix.Length < NAME_MAX_CUT_OFF_LENGTH)
				{
					int length2 = NAME_MAX_ALLOWED_LENGTH - FORMATER_ADDED_LENGTH - NameSuffix.Length;
					return $"{NamePrefix.Substring(0, length2)} - {NameSuffix}";
				}
				_ = NAME_MAX_ALLOWED_LENGTH;
				_ = FORMATER_ADDED_LENGTH;
				_ = NameSuffix.Length;
				return $"{NamePrefix.Substring(0, NAME_MAX_CUT_OFF_LENGTH)} - {NameSuffix.Substring(0, NAME_MAX_CUT_OFF_LENGTH)}";
			}
		}

		public MyAdminSelectionItemModel(string prefix, string suffix, long id)
		{
			NamePrefix = prefix;
			NameSuffix = suffix;
			Id = id;
		}
	}
}
