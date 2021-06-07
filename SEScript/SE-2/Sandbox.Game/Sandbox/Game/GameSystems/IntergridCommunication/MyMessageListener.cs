using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.SessionComponents;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.IntergridCommunication
{
	internal class MyMessageListener : IMyMessageProvider
	{
		private string m_callback;

		public bool m_hasPendingCallback;

		private Queue<MyIGCMessage> m_pendingMessages;

		private static Action<MyProgrammableBlock, string> m_invokeOverride;

		public MyIntergridCommunicationContext Context
		{
			get;
			private set;
		}

		public int MaxWaitingMessages => 25;

		public bool HasPendingMessage
		{
			get
			{
				if (m_pendingMessages != null)
				{
					return m_pendingMessages.Count > 0;
				}
				return false;
			}
		}

		public MyMessageListener(MyIntergridCommunicationContext context)
		{
			Context = context;
		}

		public void EnqueueMessage(MyIGCMessage message)
		{
			if (m_pendingMessages == null)
			{
				m_pendingMessages = new Queue<MyIGCMessage>();
			}
			else if (m_pendingMessages.Count >= MaxWaitingMessages)
			{
				m_pendingMessages.Dequeue();
			}
			m_pendingMessages.Enqueue(message);
			RegisterForCallback();
			if (MyDebugDrawSettings.DEBUG_DRAW_IGC)
			{
				Vector3D to = Context.ProgrammableBlock.WorldMatrix.Translation;
				Vector3D from = MyEntities.GetEntityById(message.Source).WorldMatrix.Translation;
				Color color = (this is IMyBroadcastListener) ? Color.Blue : Color.Green;
				MyIGCSystemSessionComponent.Static.AddDebugDraw(delegate
				{
					MyRenderProxy.DebugDrawArrow3D(from, to, color);
				});
			}
		}

		public void InvokeCallback()
		{
			UnregisterCallback();
			MyProgrammableBlock programmableBlock = Context.ProgrammableBlock;
			if (m_invokeOverride != null)
			{
				m_invokeOverride(programmableBlock, m_callback);
			}
			else
			{
				programmableBlock.Run(m_callback, UpdateType.IGC);
			}
		}

		public virtual MyIGCMessage AcceptMessage()
		{
			if (HasPendingMessage)
			{
				return m_pendingMessages.Dequeue();
			}
			return default(MyIGCMessage);
		}

		public virtual void SetMessageCallback(string argument)
		{
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			m_callback = argument;
		}

		public void DisableMessageCallback()
		{
			UnregisterCallback();
			m_callback = null;
		}

		private void RegisterForCallback()
		{
			if (m_callback != null && !m_hasPendingCallback)
			{
				m_hasPendingCallback = true;
				Context.RegisterForCallback(this);
			}
		}

		private void UnregisterCallback()
		{
			if (m_hasPendingCallback)
			{
				m_hasPendingCallback = false;
				Context.UnregisterFromCallback(this);
			}
		}
	}
}
