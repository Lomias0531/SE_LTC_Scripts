using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenProgressAsync : MyGuiScreenProgressBase
	{
		private Func<IMyAsyncResult> m_beginAction;

		private Action<IMyAsyncResult, MyGuiScreenProgressAsync> m_endAction;

		private IMyAsyncResult m_asyncResult;

		public string FriendlyName
		{
			get;
			set;
		}

		public object UserData
		{
			get;
			private set;
		}

		public StringBuilder Text
		{
			get
			{
				return m_progressTextLabel.TextToDraw;
			}
			set
			{
				m_progressTextLabel.TextToDraw = value;
			}
		}

		public new MyStringId ProgressText
		{
			get
			{
				return base.ProgressText;
			}
			set
			{
				if (base.ProgressText != value)
				{
					m_progressTextLabel.PrepareForAsyncTextUpdate();
					base.ProgressText = value;
				}
			}
		}

		public new string ProgressTextString
		{
			get
			{
				return base.ProgressTextString;
			}
			set
			{
				if (base.ProgressTextString != value)
				{
					m_progressTextLabel.PrepareForAsyncTextUpdate();
					base.ProgressTextString = value;
				}
			}
		}

		public MyGuiScreenProgressAsync(MyStringId text, MyStringId? cancelText, Func<IMyAsyncResult> beginAction, Action<IMyAsyncResult, MyGuiScreenProgressAsync> endAction, object userData = null)
			: base(text, cancelText)
		{
			FriendlyName = "MyGuiScreenProgressAsync";
			m_beginAction = beginAction;
			m_endAction = endAction;
			UserData = userData;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_rotatingWheel.MultipleSpinningWheels = MyPerGameSettings.GUI.MultipleSpinningWheels;
		}

		protected override void ProgressStart()
		{
			m_asyncResult = m_beginAction();
		}

		public override string GetFriendlyName()
		{
			return FriendlyName;
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			if (base.State != MyGuiScreenState.OPENED)
			{
				return false;
			}
			if (m_asyncResult.IsCompleted)
			{
				m_endAction(m_asyncResult, this);
			}
			if (m_asyncResult != null && m_asyncResult.Task.Exceptions != null)
			{
				Exception[] exceptions = m_asyncResult.Task.Exceptions;
				foreach (Exception ex in exceptions)
				{
					MySandboxGame.Log.WriteLine(ex);
				}
			}
			return true;
		}
	}
}
