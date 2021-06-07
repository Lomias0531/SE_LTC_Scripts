using System;
using System.Runtime.InteropServices;

namespace VRage.Utils
{
	public static class MyExternalDebugStructures
	{
		public interface IExternalDebugMsg
		{
			string GetTypeStr();
		}

		public struct CommonMsgHeader
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
			public string MsgHeader;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
			public string MsgType;

			[MarshalAs(UnmanagedType.I4)]
			public int MsgSize;

			public bool IsValid
			{
				get
				{
					if (MsgHeader == "VRAGEMS")
					{
						return MsgSize > 0;
					}
					return false;
				}
			}

			public static CommonMsgHeader Create(string msgType, int msgSize = 0)
			{
				CommonMsgHeader result = default(CommonMsgHeader);
				result.MsgHeader = "VRAGEMS";
				result.MsgType = msgType;
				result.MsgSize = msgSize;
				return result;
			}
		}

		public struct SelectedTreeMsg : IExternalDebugMsg
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
			public string BehaviorTreeName;

			string IExternalDebugMsg.GetTypeStr()
			{
				return "SELTREE";
			}
		}

		public struct ACConnectToEditorMsg : IExternalDebugMsg
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
			public string ACName;

			string IExternalDebugMsg.GetTypeStr()
			{
				return "AC_CON";
			}
		}

		public struct ACSendStateToEditorMsg : IExternalDebugMsg
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 240)]
			public string CurrentNodeAddress;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public int[] VisitedTreeNodesPath;

			public static ACSendStateToEditorMsg Create(string currentNodeAddress, int[] visitedTreeNodesPath)
			{
				ACSendStateToEditorMsg aCSendStateToEditorMsg = default(ACSendStateToEditorMsg);
				aCSendStateToEditorMsg.CurrentNodeAddress = currentNodeAddress;
				aCSendStateToEditorMsg.VisitedTreeNodesPath = new int[64];
				ACSendStateToEditorMsg result = aCSendStateToEditorMsg;
				if (visitedTreeNodesPath != null)
				{
					Array.Copy(visitedTreeNodesPath, result.VisitedTreeNodesPath, Math.Min(visitedTreeNodesPath.Length, 64));
				}
				return result;
			}

			string IExternalDebugMsg.GetTypeStr()
			{
				return "AC_STA";
			}
		}

		public struct ACReloadInGameMsg : IExternalDebugMsg
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
			public string ACName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
			public string ACAddress;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
			public string ACContentAddress;

			string IExternalDebugMsg.GetTypeStr()
			{
				return "AC_LOAD";
			}
		}

		public static readonly int MsgHeaderSize = Marshal.SizeOf(typeof(CommonMsgHeader));

		/// <summary>
		/// Convert from raw data to message.
		/// Message must be struct with sequential layout having first field "Header" of type "CommonMsg".
		/// </summary>
		public static bool ReadMessageFromPtr<TMessage>(ref CommonMsgHeader header, IntPtr data, out TMessage outMsg) where TMessage : IExternalDebugMsg
		{
			outMsg = default(TMessage);
			if (data == IntPtr.Zero || header.MsgSize != Marshal.SizeOf(typeof(TMessage)) || header.MsgType != outMsg.GetTypeStr())
			{
				return false;
			}
			outMsg = (TMessage)Marshal.PtrToStructure(data, typeof(TMessage));
			return true;
		}
	}
}
