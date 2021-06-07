using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Utils;

namespace Sandbox.Graphics.GUI
{
	internal abstract class MyGuiScreenProgressBaseAsync<T> : MyGuiScreenProgressBase
	{
		private struct ProgressAction
		{
			public IAsyncResult AsyncResult;

			public ActionDoneHandler<T> ActionDoneHandler;

			public ErrorHandler<T> ErrorHandler;
		}

		private LinkedList<ProgressAction> m_actions = new LinkedList<ProgressAction>();

		private string m_constructorStackTrace;

		protected MyGuiScreenProgressBaseAsync(MyStringId progressText, MyStringId? cancelText = null)
			: base(progressText, cancelText)
		{
			if (Debugger.IsAttached)
			{
				m_constructorStackTrace = Environment.StackTrace;
			}
		}

		protected void AddAction(IAsyncResult asyncResult, ErrorHandler<T> errorHandler = null)
		{
			AddAction(asyncResult, OnActionCompleted, errorHandler);
		}

		protected void AddAction(IAsyncResult asyncResult, ActionDoneHandler<T> doneHandler, ErrorHandler<T> errorHandler = null)
		{
			m_actions.AddFirst(new ProgressAction
			{
				AsyncResult = asyncResult,
				ActionDoneHandler = doneHandler,
				ErrorHandler = (errorHandler ?? new ErrorHandler<T>(OnError))
			});
		}

		protected void CancelAll()
		{
			m_actions.Clear();
		}

		protected override void OnCancelClick(MyGuiControlButton sender)
		{
			CancelAll();
			base.OnCancelClick(sender);
		}

		public override bool Update(bool hasFocus)
		{
			if (!base.Update(hasFocus))
			{
				return false;
			}
			LinkedListNode<ProgressAction> linkedListNode = m_actions.First;
			while (linkedListNode != null)
			{
				if (linkedListNode.Value.AsyncResult.IsCompleted)
				{
					try
					{
						linkedListNode.Value.ActionDoneHandler(linkedListNode.Value.AsyncResult, (T)linkedListNode.Value.AsyncResult.AsyncState);
					}
					catch (Exception exception)
					{
						linkedListNode.Value.ErrorHandler(exception, (T)linkedListNode.Value.AsyncResult.AsyncState);
					}
					LinkedListNode<ProgressAction> node = linkedListNode;
					linkedListNode = linkedListNode.Next;
					m_actions.Remove(node);
				}
				else
				{
					linkedListNode = linkedListNode.Next;
				}
			}
			return base.State == MyGuiScreenState.OPENED;
		}

		protected virtual void OnActionCompleted(IAsyncResult asyncResult, T asyncState)
		{
		}

		protected virtual void OnError(Exception exception, T asyncState)
		{
			MyLog.Default.WriteLine(exception);
			throw exception;
		}

		protected void Retry()
		{
			m_actions.Clear();
			ProgressStart();
		}
	}
}
