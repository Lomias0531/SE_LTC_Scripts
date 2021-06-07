using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using VRage.Game.Entity;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
	[PreloadRequired]
	public class MyServerDebugCommands
	{
		private static readonly char[] m_separators;

		private static readonly Dictionary<string, Action<string[]>> m_commands;

		private static ulong m_commandAuthor;

		private static MyReplicationServer Replication => (MyReplicationServer)MyMultiplayer.Static.ReplicationLayer;

		static MyServerDebugCommands()
		{
			m_separators = new char[3]
			{
				' ',
				'\r',
				'\n'
			};
			m_commands = new Dictionary<string, Action<string[]>>(StringComparer.InvariantCultureIgnoreCase);
			MethodInfo[] methods = typeof(MyServerDebugCommands).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				DisplayNameAttribute customAttribute = methodInfo.GetCustomAttribute<DisplayNameAttribute>();
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (customAttribute != null && methodInfo.ReturnType == typeof(void) && parameters.Length == 1 && parameters[0].ParameterType == typeof(string[]))
				{
					m_commands[customAttribute.DisplayName] = methodInfo.CreateDelegate<Action<string[]>>();
				}
			}
		}

		public static bool Process(string msg, ulong author)
		{
			m_commandAuthor = author;
			string[] array = msg.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != 0 && m_commands.TryGetValue(array[0], out Action<string[]> value))
			{
				value(array.Skip(1).ToArray());
				return true;
			}
			return false;
		}

		[DisplayName("+stress")]
		private static void StressTest(string[] args)
		{
			if (args.Length > 1)
			{
				if (args[0] == "server" || args[0] == "all")
				{
					if (args.Length > 3)
					{
						MyReplicationServer.StressSleep.X = Convert.ToInt32(args[1]);
						MyReplicationServer.StressSleep.Y = Convert.ToInt32(args[2]);
						MyReplicationServer.StressSleep.Z = Convert.ToInt32(args[3]);
					}
					else if (args.Length > 2)
					{
						MyReplicationServer.StressSleep.X = Convert.ToInt32(args[1]);
						MyReplicationServer.StressSleep.Y = Convert.ToInt32(args[2]);
						MyReplicationServer.StressSleep.Z = 0;
					}
					else
					{
						MyReplicationServer.StressSleep.X = Convert.ToInt32(args[1]);
						MyReplicationServer.StressSleep.Y = MyReplicationServer.StressSleep.X;
						MyReplicationServer.StressSleep.Z = 0;
					}
				}
			}
			else
			{
				MyReplicationServer.StressSleep.X = 0;
				MyReplicationServer.StressSleep.Y = 0;
			}
		}

		[DisplayName("+dump")]
		private static void Dump(string[] args)
		{
			MySession.InitiateDump();
		}

		[DisplayName("+save")]
		private static void Save(string[] args)
		{
			MySandboxGame.Log.WriteLineAndConsole("Executing +save command");
			MyAsyncSaving.Start();
		}

		[DisplayName("+stop")]
		private static void Stop(string[] args)
		{
			MySandboxGame.Log.WriteLineAndConsole("Executing +stop command");
			MySandboxGame.ExitThreadSafe();
		}

		[DisplayName("+unban")]
		private static void Unban(string[] args)
		{
			if (args.Length != 0)
			{
				ulong result = 0uL;
				if (ulong.TryParse(args[0], out result))
				{
					MyMultiplayer.Static.BanClient(result, banned: false);
				}
			}
		}

		[DisplayName("+resetplayers")]
		private static void ResetPlayers(string[] args)
		{
			Vector3D zero = Vector3D.Zero;
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MatrixD worldMatrix = MatrixD.CreateTranslation(zero);
				entity.PositionComp.SetWorldMatrix(worldMatrix);
				entity.Physics.LinearVelocity = Vector3D.Forward;
				zero.X += 50.0;
			}
		}

		[DisplayName("+forcereorder")]
		private static void ForceReorder(string[] args)
		{
			MyPhysics.ForceClustersReorder();
		}
	}
}
