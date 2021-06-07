using VRageRender;

namespace VRage.Render.Scene
{
	public static class MyViewIds
	{
		public const int MAX_MAIN_VIEWS = 1;

		public const int MAX_SHADOW_CASCADES = 8;

		public const int MAX_SHADOW_PROJECTIONS = 4;

		public const int MAX_FORWARD_VIEWS = 6;

		public const int MAX_VIEW_COUNT = 19;

		public const int MAIN_VIEW_ID = 0;

		public static string[] ViewNames
		{
			get;
			private set;
		}

		static MyViewIds()
		{
			ViewNames = new string[19];
			for (int i = 0; i < 19; i++)
			{
				string text = "Undefined";
				if (IsMainId(i))
				{
					text = "GBuffer";
				}
				else if (IsShadowCascadeId(i))
				{
					text = "CascadeDepth" + (i - GetShadowCascadeId(0));
				}
				else if (IsShadowProjectionId(i))
				{
					text = "SingleDepth" + (i - GetShadowProjectionId(0));
				}
				else if (IsForwardId(i))
				{
					text = "Forward" + (i - GetForwardId(0));
				}
				else
				{
					MyRenderProxy.Error("Unknown view id");
				}
				ViewNames[i] = text;
			}
		}

		public static bool IsMainId(int viewId)
		{
			return viewId == 0;
		}

		public static bool IsShadowId(int viewId)
		{
			if (viewId >= 1)
			{
				return viewId < GetForwardId(0);
			}
			return false;
		}

		public static bool IsShadowCascadeId(int viewId)
		{
			if (viewId >= GetShadowCascadeId(0))
			{
				return viewId <= GetShadowCascadeId(7);
			}
			return false;
		}

		public static bool IsShadowProjectionId(int viewId)
		{
			if (viewId >= GetShadowProjectionId(0))
			{
				return viewId <= GetShadowProjectionId(3);
			}
			return false;
		}

		public static bool IsForwardId(int viewId)
		{
			if (viewId >= GetForwardId(0))
			{
				return viewId <= GetForwardId(5);
			}
			return false;
		}

		public static int GetMainId(int i)
		{
			return i;
		}

		public static int GetMainIndex(int id)
		{
			return id;
		}

		public static int GetShadowCascadeId(int i)
		{
			return i + 1;
		}

		public static int GetShadowCascadeIndex(int id)
		{
			return id - 1;
		}

		public static int GetShadowProjectionId(int i)
		{
			return i + 1 + 8;
		}

		public static int GetShadowProjectionIndex(int id)
		{
			return id - 1 - 8;
		}

		public static int GetForwardId(int i)
		{
			return i + 1 + 8 + 4;
		}

		public static int GetForwardIndex(int id)
		{
			return id - 1 - 8 - 4;
		}

		public static int GetViewCount(MyViewType viewType)
		{
			switch (viewType)
			{
			case MyViewType.Main:
				return 1;
			case MyViewType.ShadowCascade:
				return 8;
			case MyViewType.ShadowProjection:
				return 4;
			case MyViewType.EnvironmentProbe:
				return 6;
			default:
				return -1;
			}
		}

		public static int GetId(MyViewType viewType, int viewIndex)
		{
			switch (viewType)
			{
			case MyViewType.Main:
				return GetMainId(viewIndex);
			case MyViewType.ShadowCascade:
				return GetShadowCascadeId(viewIndex);
			case MyViewType.ShadowProjection:
				return GetShadowProjectionId(viewIndex);
			case MyViewType.EnvironmentProbe:
				return GetForwardId(viewIndex);
			default:
				return -1;
			}
		}
	}
}
