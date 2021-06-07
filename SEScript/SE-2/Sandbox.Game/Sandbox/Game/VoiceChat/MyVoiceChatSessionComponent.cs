using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game.Components;
using VRage.GameServices;
using VRage.Library;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.VoiceChat
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	[StaticEventOwner]
	public class MyVoiceChatSessionComponent : MySessionComponentBase
	{
		private class SendBuffer : IBitSerializable
		{
			public byte[] CompressedVoiceBuffer;

			public byte StartingKilobyte;

			public int NumElements;

			public long SenderUserId;

			public bool Serialize(BitStream stream, bool validate, bool acceptAndSetValue = true)
			{
				if (stream.Reading)
				{
					SenderUserId = stream.ReadInt64();
					NumElements = stream.ReadInt32();
					stream.ReadBytes(CompressedVoiceBuffer, 0, NumElements);
				}
				else
				{
					stream.WriteInt64(SenderUserId);
					stream.WriteInt32(NumElements);
					stream.WriteBytes(CompressedVoiceBuffer, StartingKilobyte * 1024, NumElements);
				}
				return true;
			}

			public static implicit operator BitReaderWriter(SendBuffer buffer)
			{
				return new BitReaderWriter(buffer);
			}
		}

		private struct ReceivedData
		{
			public MyList<byte> UncompressedBuffer;

			public MyTimeSpan Timestamp;

			public MyTimeSpan SpeakerTimestamp;

			public void ClearData()
			{
				UncompressedBuffer.Clear();
				Timestamp = MyTimeSpan.Zero;
			}

			public void ClearSpeakerTimestamp()
			{
				SpeakerTimestamp = MyTimeSpan.Zero;
			}
		}

		protected sealed class MutePlayerRequest_Implementation_003C_003ESystem_UInt64_0023System_Boolean : ICallSite<IMyEventOwner, ulong, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong mutedPlayerId, in bool mute, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				MutePlayerRequest_Implementation(mutedPlayerId, mute);
			}
		}

		protected sealed class MutePlayer_Implementation_003C_003ESystem_UInt64_0023System_Boolean : ICallSite<IMyEventOwner, ulong, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerSettingMute, in bool mute, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				MutePlayer_Implementation(playerSettingMute, mute);
			}
		}

		protected sealed class SendVoice_003C_003EVRage_Library_Collections_BitReaderWriter : ICallSite<IMyEventOwner, BitReaderWriter, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in BitReaderWriter data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendVoice(data);
			}
		}

		protected sealed class SendVoicePlayer_003C_003ESystem_UInt64_0023VRage_Library_Collections_BitReaderWriter : ICallSite<IMyEventOwner, ulong, BitReaderWriter, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong user, in BitReaderWriter data, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendVoicePlayer(user, data);
			}
		}

		private bool m_recording;

		private byte[] m_compressedVoiceBuffer;

		private byte[] m_uncompressedVoiceBuffer;

		private Dictionary<ulong, MyEntity3DSoundEmitter> m_voices;

		private Dictionary<ulong, ReceivedData> m_receivedVoiceData;

		private int m_frameCount;

		private List<ulong> m_keys;

		private IMyVoiceChatLogic m_voiceChatLogic;

		private bool m_enabled;

		private const uint COMPRESSED_SIZE = 8192u;

		private const uint UNCOMPRESSED_SIZE = 22528u;

		private Dictionary<ulong, bool> m_debugSentVoice = new Dictionary<ulong, bool>();

		private Dictionary<ulong, MyTuple<int, TimeSpan>> m_debugReceivedVoice = new Dictionary<ulong, MyTuple<int, TimeSpan>>();

		private int lastMessageTime;

		private static SendBuffer Recievebuffer = new SendBuffer
		{
			CompressedVoiceBuffer = new byte[8192]
		};

		public static MyVoiceChatSessionComponent Static
		{
			get;
			private set;
		}

		public bool IsRecording => m_recording;

		public override bool IsRequiredByGame => MyPerGameSettings.VoiceChatEnabled;

		public event Action<ulong, bool> OnPlayerMutedStateChanged;

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			MyGameService.InitializeVoiceRecording();
			m_voiceChatLogic = (Activator.CreateInstance(MyPerGameSettings.VoiceChatLogic) as IMyVoiceChatLogic);
			m_recording = false;
			m_compressedVoiceBuffer = new byte[8192];
			m_uncompressedVoiceBuffer = new byte[22528];
			m_voices = new Dictionary<ulong, MyEntity3DSoundEmitter>();
			m_receivedVoiceData = new Dictionary<ulong, ReceivedData>();
			m_keys = new List<ulong>();
			Sync.Players.PlayerRemoved += Players_PlayerRemoved;
			Sync.Players.PlayersChanged += OnOnlinePlayersChanged;
			m_enabled = MyAudio.Static.EnableVoiceChat;
			MyAudio.Static.VoiceChatEnabled += Static_VoiceChatEnabled;
			MyHud.VoiceChat.VisibilityChanged += VoiceChat_VisibilityChanged;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				ulong steamId = onlinePlayer.Id.SteamId;
				if (MySandboxGame.Config.MutedPlayers.Contains(steamId))
				{
					MyGameService.SetPlayerMuted(steamId, muted: true);
				}
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (m_recording)
			{
				StopRecording();
			}
			foreach (KeyValuePair<ulong, MyEntity3DSoundEmitter> voice in m_voices)
			{
				m_voices[voice.Key].StopSound(forced: true);
				m_voices[voice.Key].Cleanup();
			}
			m_compressedVoiceBuffer = null;
			m_uncompressedVoiceBuffer = null;
			m_voiceChatLogic = null;
			MyGameService.DisposeVoiceRecording();
			Static = null;
			m_receivedVoiceData = null;
			m_voices = null;
			m_keys = null;
			Sync.Players.PlayerRemoved -= Players_PlayerRemoved;
			Sync.Players.PlayersChanged -= OnOnlinePlayersChanged;
			MyAudio.Static.VoiceChatEnabled -= Static_VoiceChatEnabled;
			MyHud.VoiceChat.VisibilityChanged -= VoiceChat_VisibilityChanged;
		}

		private void OnOnlinePlayersChanged(bool connected, MyPlayer.PlayerId player)
		{
			ulong steamId = player.SteamId;
			if (connected && MySandboxGame.Config.MutedPlayers.Contains(steamId))
			{
				MyGameService.SetPlayerMuted(steamId, muted: true);
			}
		}

		private void Players_PlayerRemoved(MyPlayer.PlayerId pid)
		{
			if (pid.SerialId == 0)
			{
				ulong steamId = pid.SteamId;
				if (m_receivedVoiceData.ContainsKey(steamId))
				{
					m_receivedVoiceData.Remove(steamId);
				}
				if (m_voices.ContainsKey(steamId))
				{
					m_voices[steamId].StopSound(forced: true);
					m_voices[steamId].Cleanup();
					m_voices[steamId] = null;
					m_voices.Remove(steamId);
				}
			}
		}

		private void Static_VoiceChatEnabled(bool isEnabled)
		{
			m_enabled = isEnabled;
			if (!m_enabled)
			{
				if (m_recording)
				{
					m_recording = false;
					StopRecording();
				}
				foreach (KeyValuePair<ulong, MyEntity3DSoundEmitter> voice in m_voices)
				{
					voice.Value.StopSound(forced: true);
					voice.Value.Cleanup();
				}
				m_voices.Clear();
				m_receivedVoiceData.Clear();
			}
		}

		private void VoiceChat_VisibilityChanged(bool isVisible)
		{
			if (m_recording != isVisible)
			{
				if (m_recording)
				{
					m_recording = false;
					StopRecording();
				}
				else
				{
					StartRecording();
				}
			}
		}

		public void StartRecording()
		{
			if (m_enabled)
			{
				m_recording = true;
				MyGameService.StartVoiceRecording();
				MyHud.VoiceChat.Show();
			}
		}

		public void StopRecording()
		{
			if (m_enabled)
			{
				MyGameService.StopVoiceRecording();
				MyHud.VoiceChat.Hide();
			}
		}

		public void ClearDebugData()
		{
			m_debugSentVoice.Clear();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (!m_enabled)
			{
				return;
			}
			if (MySandboxGame.Static.SimulationFrameCounter % 100uL == 0L)
			{
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					if (!onlinePlayer.IsLocalPlayer)
					{
						ulong steamId = onlinePlayer.Id.SteamId;
						MyPlayerVoiceChatState playerVoiceChatState = MyGameService.GetPlayerVoiceChatState(steamId);
						SetPlayerMuted(steamId, playerVoiceChatState == MyPlayerVoiceChatState.Muted);
					}
				}
			}
			if (IsCharacterValid(MySession.Static.LocalCharacter))
			{
				if (m_recording)
				{
					UpdateRecording();
				}
				UpdatePlayback();
			}
		}

		private static bool IsCharacterValid(MyCharacter character)
		{
			if (character != null && !character.IsDead)
			{
				return !character.MarkedForClose;
			}
			return false;
		}

		private void VoiceMessageReceived(ulong sender)
		{
			if (m_enabled && IsCharacterValid(MySession.Static.LocalCharacter))
			{
				ProcessBuffer(Recievebuffer.CompressedVoiceBuffer, Recievebuffer.NumElements / 1, sender);
			}
		}

		private void PlayVoice(byte[] uncompressedBuffer, int uncompressedSize, ulong playerId, MySoundDimensions dimension, float maxDistance)
		{
			if (!m_voices.ContainsKey(playerId))
			{
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(playerId));
				m_voices[playerId] = new MyEntity3DSoundEmitter(playerById.Character);
			}
			m_voices[playerId].PlaySound(uncompressedBuffer, uncompressedSize, MyGameService.GetVoiceSampleRate(), MyAudio.Static.VolumeVoiceChat, maxDistance, dimension);
		}

		private void ProcessBuffer(byte[] compressedBuffer, int bufferSize, ulong sender)
		{
			if (MyGameService.DecompressVoice(compressedBuffer, (uint)bufferSize, m_uncompressedVoiceBuffer, out uint writtenBytes) == MyVoiceResult.OK)
			{
				if (!m_receivedVoiceData.TryGetValue(sender, out ReceivedData value))
				{
					ReceivedData receivedData = default(ReceivedData);
					receivedData.UncompressedBuffer = new MyList<byte>();
					receivedData.Timestamp = MyTimeSpan.Zero;
					value = receivedData;
				}
				if (value.Timestamp == MyTimeSpan.Zero)
				{
					value.Timestamp = MySandboxGame.Static.TotalTime;
				}
				value.SpeakerTimestamp = MySandboxGame.Static.TotalTime;
				value.UncompressedBuffer.InsertFrom(m_uncompressedVoiceBuffer, 0, value.UncompressedBuffer.Count, (int)writtenBytes);
				m_receivedVoiceData[sender] = value;
			}
		}

		private void UpdatePlayback()
		{
			if (m_voiceChatLogic != null)
			{
				MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
				float num = 1000f;
				m_keys.AddRange(m_receivedVoiceData.Keys);
				foreach (ulong key in m_keys)
				{
					bool flag = false;
					ulong num2 = key;
					ReceivedData value = m_receivedVoiceData[key];
					MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(num2));
					if (playerById != null && value.Timestamp != MyTimeSpan.Zero && m_voiceChatLogic.ShouldPlayVoice(playerById, value.Timestamp, out MySoundDimensions dimension, out float maxDistance))
					{
						if (!MySandboxGame.Config.MutedPlayers.Contains(playerById.Id.SteamId))
						{
							PlayVoice(value.UncompressedBuffer.ToArray(), value.UncompressedBuffer.Count, num2, dimension, maxDistance);
							value.ClearData();
							flag = true;
						}
						else if (lastMessageTime == 0 || MyEnvironment.TickCount > lastMessageTime + 5000)
						{
							MutePlayerRequest(playerById.Id.SteamId, mute: true);
							lastMessageTime = MyEnvironment.TickCount;
						}
					}
					if (value.SpeakerTimestamp != MyTimeSpan.Zero && (totalTime - value.SpeakerTimestamp).Milliseconds > (double)num)
					{
						value.ClearSpeakerTimestamp();
						flag = true;
					}
					if (flag)
					{
						m_receivedVoiceData[key] = value;
					}
				}
				m_keys.Clear();
			}
		}

		private void UpdateRecording()
		{
			uint size = 0u;
			switch (MyGameService.GetAvailableVoice(out size))
			{
			case MyVoiceResult.OK:
			{
				MyVoiceResult voice = MyGameService.GetVoice(m_compressedVoiceBuffer, out size);
				if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
				{
					ProcessBuffer(m_compressedVoiceBuffer, (int)size, Sync.MyId);
				}
				if (!Sync.IsDedicated)
				{
					for (int i = 0; i < size / 1u / 1024u + 1; i++)
					{
						SendBuffer buffer = new SendBuffer
						{
							CompressedVoiceBuffer = m_compressedVoiceBuffer,
							StartingKilobyte = (byte)i,
							NumElements = (int)((i > 0) ? ((long)(size / 1u) % (long)(i * 1024)) : (size / 1u)),
							SenderUserId = (long)MySession.Static.LocalHumanPlayer.Id.SteamId
						};
						MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<BitReaderWriter>>)((IMyEventOwner x) => SendVoice), (BitReaderWriter)buffer, default(EndpointId), (Vector3D?)null);
					}
				}
				break;
			}
			case MyVoiceResult.NotRecording:
				m_recording = false;
				if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
				{
					ulong myId = Sync.MyId;
					if (!m_voices.ContainsKey(myId))
					{
						MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(myId));
						m_voices[myId] = new MyEntity3DSoundEmitter(playerById.Character);
					}
					MyEntity3DSoundEmitter myEntity3DSoundEmitter = m_voices[myId];
					if (m_receivedVoiceData.ContainsKey(myId))
					{
						ReceivedData value = m_receivedVoiceData[myId];
						myEntity3DSoundEmitter.PlaySound(value.UncompressedBuffer.ToArray(), value.UncompressedBuffer.Count, MyGameService.GetVoiceSampleRate());
						value.ClearData();
						value.ClearSpeakerTimestamp();
						m_receivedVoiceData[myId] = value;
					}
				}
				break;
			}
		}

		public void SetPlayerMuted(ulong playerId, bool muted)
		{
			HashSet<ulong> mutedPlayers = MySandboxGame.Config.MutedPlayers;
			if ((!muted) ? mutedPlayers.Remove(playerId) : mutedPlayers.Add(playerId))
			{
				MySandboxGame.Config.MutedPlayers = mutedPlayers;
				MySandboxGame.Config.Save();
				MutePlayerRequest(playerId, muted);
				MyGameService.SetPlayerMuted(playerId, muted);
				this.OnPlayerMutedStateChanged.InvokeIfNotNull(playerId, muted);
			}
		}

		public static void MutePlayerRequest(ulong mutedPlayerId, bool mute)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MutePlayerRequest_Implementation, mutedPlayerId, mute);
		}

		[Event(null, 488)]
		[Reliable]
		[Server]
		private static void MutePlayerRequest_Implementation(ulong mutedPlayerId, bool mute)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MutePlayer_Implementation, MyEventContext.Current.Sender.Value, mute, new EndpointId(mutedPlayerId));
		}

		[Event(null, 495)]
		[Reliable]
		[Broadcast]
		public static void MutePlayer_Implementation(ulong playerSettingMute, bool mute)
		{
			HashSet<ulong> dontSendVoicePlayers = MySandboxGame.Config.DontSendVoicePlayers;
			if (mute)
			{
				dontSendVoicePlayers.Add(playerSettingMute);
			}
			else
			{
				dontSendVoicePlayers.Remove(playerSettingMute);
			}
			MySandboxGame.Config.DontSendVoicePlayers = dontSendVoicePlayers;
			MySandboxGame.Config.Save();
		}

		[Event(null, 510)]
		[Server]
		private static void SendVoice(BitReaderWriter data)
		{
			if (Static != null && data.ReadData(Recievebuffer, validate: false))
			{
				ulong senderUserId = (ulong)Recievebuffer.SenderUserId;
				MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(senderUserId));
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					if (onlinePlayer.Id.SerialId == 0 && onlinePlayer.Id.SteamId != senderUserId && IsCharacterValid(onlinePlayer.Character) && Static.m_voiceChatLogic.ShouldSendVoice(playerById, onlinePlayer) && !MySandboxGame.Config.DontSendVoicePlayers.Contains(onlinePlayer.Id.SteamId))
					{
						MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<ulong, BitReaderWriter>>)((IMyEventOwner x) => SendVoicePlayer), onlinePlayer.Id.SteamId, (BitReaderWriter)Recievebuffer, new EndpointId(onlinePlayer.Id.SteamId), (Vector3D?)null);
						if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
						{
							Static.m_debugSentVoice[onlinePlayer.Id.SteamId] = true;
						}
					}
					else if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
					{
						Static.m_debugSentVoice[onlinePlayer.Id.SteamId] = false;
					}
				}
			}
		}

		[Event(null, 539)]
		[Client]
		private static void SendVoicePlayer(ulong user, BitReaderWriter data)
		{
			data.ReadData(Recievebuffer, validate: false);
			Static.VoiceMessageReceived((ulong)Recievebuffer.SenderUserId);
		}

		public override void Draw()
		{
			base.Draw();
			if (m_receivedVoiceData != null)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_VOICE_CHAT && MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
				{
					DebugDraw();
				}
				BoundingSphereD boundingSphereD = new BoundingSphereD(MySector.MainCamera.Position, 500.0);
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					if (onlinePlayer.Character != null && !onlinePlayer.IsLocalPlayer)
					{
						MyPositionComponentBase positionComp = onlinePlayer.Character.PositionComp;
						MatrixD worldMatrix = positionComp.WorldMatrix;
						if (boundingSphereD.Contains(worldMatrix.Translation) != 0)
						{
							ulong steamId = onlinePlayer.Id.SteamId;
							if (MyGameService.GetPlayerVoiceChatState(steamId) == MyPlayerVoiceChatState.Talking || (m_receivedVoiceData.TryGetValue(steamId, out ReceivedData value) && value.SpeakerTimestamp != MyTimeSpan.Zero))
							{
								Vector3D position = worldMatrix.Translation + positionComp.LocalAABB.Height * worldMatrix.Up + worldMatrix.Up * 0.20000000298023224;
								MyGuiPaddedTexture tEXTURE_VOICE_CHAT = MyGuiConstants.TEXTURE_VOICE_CHAT;
								MatrixD matrix = MySector.MainCamera.ViewMatrix * MySector.MainCamera.ProjectionMatrix;
								Vector3D vector3D = Vector3D.Transform(position, matrix);
								if (vector3D.Z < 1.0)
								{
									Vector2 hudPos = new Vector2((float)vector3D.X, (float)vector3D.Y);
									hudPos = hudPos * 0.5f + 0.5f * Vector2.One;
									hudPos.Y = 1f - hudPos.Y;
									MyGuiManager.DrawSpriteBatch(tEXTURE_VOICE_CHAT.Texture, MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos), tEXTURE_VOICE_CHAT.SizeGui * 0.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
								}
							}
						}
					}
				}
			}
		}

		private void DebugDraw()
		{
			Vector2 screenCoord = new Vector2(300f, 100f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Sent voice to:", Color.White, 1f);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<ulong, bool> item in m_debugSentVoice)
			{
				string text = string.Format("id: {0} => {1}", item.Key, item.Value ? "SENT" : "NOT");
				MyRenderProxy.DebugDrawText2D(screenCoord, text, Color.White, 1f);
				screenCoord.Y += 30f;
			}
			MyRenderProxy.DebugDrawText2D(screenCoord, "Received voice from:", Color.White, 1f);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<ulong, MyTuple<int, TimeSpan>> item2 in m_debugReceivedVoice)
			{
				string text2 = $"id: {item2.Key} => size: {item2.Value.Item1} (timestamp {item2.Value.Item2.ToString()})";
				MyRenderProxy.DebugDrawText2D(screenCoord, text2, Color.White, 1f);
				screenCoord.Y += 30f;
			}
			MyRenderProxy.DebugDrawText2D(screenCoord, "Uncompressed buffers:", Color.White, 1f);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<ulong, ReceivedData> receivedVoiceDatum in m_receivedVoiceData)
			{
				string text3 = $"id: {receivedVoiceDatum.Key} => size: {receivedVoiceDatum.Value.UncompressedBuffer.Count}";
				MyRenderProxy.DebugDrawText2D(screenCoord, text3, Color.White, 1f);
				screenCoord.Y += 30f;
			}
		}
	}
}
