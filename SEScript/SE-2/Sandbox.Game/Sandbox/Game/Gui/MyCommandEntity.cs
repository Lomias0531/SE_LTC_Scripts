using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game.Entity;

namespace Sandbox.Game.GUI
{
	[PreloadRequired]
	public class MyCommandEntity : MyCommand
	{
		private class MyCommandArgsDisplayName : MyCommandArgs
		{
			public long EntityId;

			public string newDisplayName;
		}

		static MyCommandEntity()
		{
			MyConsole.AddCommand(new MyCommandEntity());
		}

		public override string Prefix()
		{
			return "Entity";
		}

		private MyCommandEntity()
		{
			m_methods.Add("SetDisplayName", new MyCommandAction
			{
				AutocompleteHint = new StringBuilder("long_EntityId string_NewDisplayName"),
				Parser = ((List<string> x) => ParseDisplayName(x)),
				CallAction = ((MyCommandArgs x) => ChangeDisplayName(x))
			});
			m_methods.Add("MethodA", new MyCommandAction
			{
				Parser = ((List<string> x) => ParseDisplayName(x)),
				CallAction = ((MyCommandArgs x) => ChangeDisplayName(x))
			});
			m_methods.Add("MethodB", new MyCommandAction
			{
				Parser = ((List<string> x) => ParseDisplayName(x)),
				CallAction = ((MyCommandArgs x) => ChangeDisplayName(x))
			});
			m_methods.Add("MethodC", new MyCommandAction
			{
				Parser = ((List<string> x) => ParseDisplayName(x)),
				CallAction = ((MyCommandArgs x) => ChangeDisplayName(x))
			});
			m_methods.Add("MethodD", new MyCommandAction
			{
				Parser = ((List<string> x) => ParseDisplayName(x)),
				CallAction = ((MyCommandArgs x) => ChangeDisplayName(x))
			});
		}

		private StringBuilder ChangeDisplayName(MyCommandArgs args)
		{
			MyCommandArgsDisplayName myCommandArgsDisplayName = args as MyCommandArgsDisplayName;
			if (MyEntities.TryGetEntityById(myCommandArgsDisplayName.EntityId, out MyEntity entity))
			{
				if (myCommandArgsDisplayName.newDisplayName != null)
				{
					string displayName = entity.DisplayName;
					entity.DisplayName = myCommandArgsDisplayName.newDisplayName;
					return new StringBuilder().Append("Changed name from entity ").Append(myCommandArgsDisplayName.EntityId).Append(" from ")
						.Append(displayName)
						.Append(" to ")
						.Append(entity.DisplayName);
				}
				return new StringBuilder().Append("Invalid Display name");
			}
			return new StringBuilder().Append("Entity not found");
		}

		private MyCommandArgs ParseDisplayName(List<string> args)
		{
			return new MyCommandArgsDisplayName
			{
				EntityId = long.Parse(args[0]),
				newDisplayName = args[1]
			};
		}
	}
}
