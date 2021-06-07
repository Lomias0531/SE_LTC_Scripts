using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;

namespace Sandbox.Game.World.Generator
{
	internal class MyFactionRelationGenerator
	{
		private enum RelationStatusPicker
		{
			None,
			Miner_Miner,
			Miner_Trader,
			Miner_Builder,
			Trader_Trader,
			Trader_Builder,
			Builder_Builder
		}

		private class MyFactionIndexer
		{
			private Dictionary<long, int> indexes = new Dictionary<long, int>();

			public MyFactionIndexer(List<MyFaction> miners, List<MyFaction> traders, List<MyFaction> builders)
			{
				int num = 0;
				indexes.Clear();
				foreach (MyFaction miner in miners)
				{
					indexes.Add(miner.FactionId, num);
					num++;
				}
				foreach (MyFaction trader in traders)
				{
					indexes.Add(trader.FactionId, num);
					num++;
				}
				foreach (MyFaction builder in builders)
				{
					indexes.Add(builder.FactionId, num);
					num++;
				}
			}

			public MyFactionIndexer(List<MyFaction> factions)
			{
				int num = 0;
				indexes.Clear();
				foreach (MyFaction faction in factions)
				{
					indexes.Add(faction.FactionId, num);
					num++;
				}
			}

			public int GetIndex(MyFaction fac)
			{
				if (!indexes.ContainsKey(fac.FactionId))
				{
					return -1;
				}
				return indexes[fac.FactionId];
			}

			public int GetIndex(long factionId)
			{
				if (!indexes.ContainsKey(factionId))
				{
					return -1;
				}
				return indexes[factionId];
			}
		}

		private delegate double MySelector(Tuple<double, double, double, double> stats);

		private MySessionComponentEconomyDefinition m_def;

		public MyFactionRelationGenerator(MySessionComponentEconomyDefinition def)
		{
			m_def = def;
		}

		private static double SelectorMin(Tuple<double, double, double, double> stats)
		{
			return stats.Item1;
		}

		public bool GenerateFactionRelations(MyFactionCollection factionsRaw)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			MySelector mySelector = SelectorMin;
			List<List<List<double>>> list = new List<List<List<double>>>();
			List<List<Tuple<double, double, double, double>>> list2 = new List<List<Tuple<double, double, double, double>>>();
			List<List<double>> cubeNormalizedDistances = new List<List<double>>();
			List<List<double>> cubeReputations = new List<List<double>>();
			List<List<RelationStatusPicker>> list3 = new List<List<RelationStatusPicker>>();
			MyFaction pirateFaction = GetPirateFaction(factionsRaw);
			List<MyFaction> factions = new List<MyFaction>();
			List<MyFaction> factions2 = new List<MyFaction>();
			List<MyFaction> factions3 = new List<MyFaction>();
			List<MyFaction> list4 = new List<MyFaction>();
			foreach (KeyValuePair<long, MyFaction> item in factionsRaw)
			{
				switch (item.Value.FactionType)
				{
				case MyFactionTypes.Miner:
					if (item.Value != pirateFaction)
					{
						factions.Add(item.Value);
					}
					break;
				case MyFactionTypes.Trader:
					if (item.Value != pirateFaction)
					{
						factions2.Add(item.Value);
					}
					break;
				case MyFactionTypes.Builder:
					if (item.Value != pirateFaction)
					{
						factions3.Add(item.Value);
					}
					break;
				}
			}
			foreach (MyFaction item2 in factions)
			{
				list4.Add(item2);
			}
			foreach (MyFaction item3 in factions2)
			{
				list4.Add(item3);
			}
			foreach (MyFaction item4 in factions3)
			{
				list4.Add(item4);
			}
			if (pirateFaction != null)
			{
				switch (pirateFaction.FactionType)
				{
				case MyFactionTypes.Miner:
					factions.Add(pirateFaction);
					list4.Add(pirateFaction);
					break;
				case MyFactionTypes.Trader:
					factions2.Add(pirateFaction);
					list4.Add(pirateFaction);
					break;
				case MyFactionTypes.Builder:
					factions3.Add(pirateFaction);
					list4.Add(pirateFaction);
					break;
				default:
					list4.Add(pirateFaction);
					break;
				}
			}
			MyFactionIndexer myFactionIndexer = new MyFactionIndexer(list4);
			int num = -1;
			long pIRATE_FACTION_ID = -1L;
			if (pirateFaction != null)
			{
				num = myFactionIndexer.GetIndex(pirateFaction.FactionId);
				pIRATE_FACTION_ID = pirateFaction.FactionId;
			}
			foreach (MyFaction item5 in list4)
			{
				list.Add(new List<List<double>>());
				list2.Add(new List<Tuple<double, double, double, double>>());
				cubeNormalizedDistances.Add(new List<double>());
				cubeReputations.Add(new List<double>());
				list3.Add(new List<RelationStatusPicker>());
				foreach (MyFaction item6 in list4)
				{
					list2[list2.Count - 1].Add(null);
					cubeNormalizedDistances[cubeNormalizedDistances.Count - 1].Add(0.0);
					cubeReputations[cubeReputations.Count - 1].Add(0.0);
					list[list.Count - 1].Add(CountDistances(item5, item6));
					list3[list3.Count - 1].Add(GetFactionRelation(item5, item6));
				}
			}
			foreach (MyFaction item7 in list4)
			{
				foreach (MyFaction item8 in list4)
				{
					int index = myFactionIndexer.GetIndex(item7.FactionId);
					int index2 = myFactionIndexer.GetIndex(item8.FactionId);
					list2[index][index2] = ProcessDistances(list[index][index2]);
				}
			}
			double num6;
			double num5;
			double num4;
			double num3;
			double num2;
			double num7 = num6 = (num5 = (num4 = (num3 = (num2 = double.MaxValue))));
			double num12;
			double num11;
			double num10;
			double num9;
			double num8;
			double num13 = num12 = (num11 = (num10 = (num9 = (num8 = double.MinValue))));
			foreach (MyFaction item9 in factions)
			{
				foreach (MyFaction item10 in factions)
				{
					if (item9.FactionId != item10.FactionId)
					{
						double num14 = mySelector(list2[myFactionIndexer.GetIndex(item9.FactionId)][myFactionIndexer.GetIndex(item10.FactionId)]);
						num7 = ((num7 < num14) ? num7 : num14);
						num13 = ((num13 > num14) ? num13 : num14);
					}
				}
				foreach (MyFaction item11 in factions2)
				{
					double num15 = mySelector(list2[myFactionIndexer.GetIndex(item9.FactionId)][myFactionIndexer.GetIndex(item11.FactionId)]);
					num6 = ((num6 < num15) ? num6 : num15);
					num12 = ((num12 > num15) ? num12 : num15);
				}
				foreach (MyFaction item12 in factions3)
				{
					double num16 = mySelector(list2[myFactionIndexer.GetIndex(item9.FactionId)][myFactionIndexer.GetIndex(item12.FactionId)]);
					num5 = ((num5 < num16) ? num5 : num16);
					num11 = ((num11 > num16) ? num11 : num16);
				}
			}
			double norm = num13 - num7;
			double norm2 = num12 - num6;
			double norm3 = num11 - num5;
			foreach (MyFaction item13 in factions2)
			{
				foreach (MyFaction item14 in factions2)
				{
					if (item13.FactionId != item14.FactionId)
					{
						double num17 = mySelector(list2[myFactionIndexer.GetIndex(item13.FactionId)][myFactionIndexer.GetIndex(item14.FactionId)]);
						num4 = ((num4 < num17) ? num4 : num17);
						num10 = ((num10 > num17) ? num10 : num17);
					}
				}
				foreach (MyFaction item15 in factions3)
				{
					double num18 = mySelector(list2[myFactionIndexer.GetIndex(item13.FactionId)][myFactionIndexer.GetIndex(item15.FactionId)]);
					num3 = ((num3 < num18) ? num3 : num18);
					num9 = ((num9 > num18) ? num9 : num18);
				}
			}
			double norm4 = num10 - num4;
			double norm5 = num9 - num3;
			foreach (MyFaction item16 in factions3)
			{
				foreach (MyFaction item17 in factions3)
				{
					if (item16.FactionId != item17.FactionId)
					{
						double num19 = mySelector(list2[myFactionIndexer.GetIndex(item16.FactionId)][myFactionIndexer.GetIndex(item17.FactionId)]);
						num2 = ((num2 < num19) ? num2 : num19);
						num8 = ((num8 > num19) ? num8 : num19);
					}
				}
			}
			double norm6 = num8 - num2;
			foreach (MyFaction item18 in factions)
			{
				foreach (MyFaction item19 in factions)
				{
					int index3 = myFactionIndexer.GetIndex(item18.FactionId);
					int index4 = myFactionIndexer.GetIndex(item19.FactionId);
					cubeNormalizedDistances[index3][index4] = ProcessRelations(mySelector(list2[index3][index4]), num7, num13, norm, RelationStatusPicker.Miner_Miner);
				}
				foreach (MyFaction item20 in factions2)
				{
					int index5 = myFactionIndexer.GetIndex(item18.FactionId);
					int index6 = myFactionIndexer.GetIndex(item20.FactionId);
					double num22 = cubeNormalizedDistances[index5][index6] = (cubeNormalizedDistances[index6][index5] = ProcessRelations(mySelector(list2[index5][index6]), num6, num12, norm2, RelationStatusPicker.Miner_Trader));
				}
				foreach (MyFaction item21 in factions3)
				{
					int index7 = myFactionIndexer.GetIndex(item18.FactionId);
					int index8 = myFactionIndexer.GetIndex(item21.FactionId);
					double num22 = cubeNormalizedDistances[index7][index8] = (cubeNormalizedDistances[index8][index7] = ProcessRelations(mySelector(list2[index7][index8]), num5, num11, norm3, RelationStatusPicker.Miner_Builder));
				}
			}
			foreach (MyFaction item22 in factions2)
			{
				foreach (MyFaction item23 in factions2)
				{
					int index9 = myFactionIndexer.GetIndex(item22.FactionId);
					int index10 = myFactionIndexer.GetIndex(item23.FactionId);
					cubeNormalizedDistances[index9][index10] = ProcessRelations(mySelector(list2[index9][index10]), num4, num10, norm4, RelationStatusPicker.Trader_Trader);
				}
				foreach (MyFaction item24 in factions3)
				{
					int index11 = myFactionIndexer.GetIndex(item22.FactionId);
					int index12 = myFactionIndexer.GetIndex(item24.FactionId);
					double num22 = cubeNormalizedDistances[index11][index12] = (cubeNormalizedDistances[index12][index11] = ProcessRelations(mySelector(list2[index11][index12]), num3, num9, norm5, RelationStatusPicker.Trader_Builder));
				}
			}
			foreach (MyFaction item25 in factions3)
			{
				foreach (MyFaction item26 in factions3)
				{
					int index13 = myFactionIndexer.GetIndex(item25.FactionId);
					int index14 = myFactionIndexer.GetIndex(item26.FactionId);
					cubeNormalizedDistances[index13][index14] = ProcessRelations(mySelector(list2[index13][index14]), num2, num8, norm6, RelationStatusPicker.Trader_Builder);
				}
			}
			foreach (MyFaction item27 in list4)
			{
				foreach (MyFaction item28 in list4)
				{
					int index15 = myFactionIndexer.GetIndex(item27.FactionId);
					int index16 = myFactionIndexer.GetIndex(item28.FactionId);
					cubeReputations[index15][index16] = NormalToRep(cubeNormalizedDistances[index15][index16], list3[index15][index16]);
				}
			}
			if (pirateFaction != null)
			{
				foreach (MyFaction item29 in list4)
				{
					int index17 = myFactionIndexer.GetIndex(item29.FactionId);
					cubeReputations[index17][num] = m_def.ReputationHostileMin;
					cubeReputations[num][index17] = m_def.ReputationHostileMin;
				}
			}
			foreach (MyFaction item30 in list4)
			{
				int index18 = myFactionIndexer.GetIndex(item30.FactionId);
				cubeReputations[index18][index18] = m_def.ReputationNeutralMid;
			}
			BefriendClosest(ref factions2, ref factions, ref cubeNormalizedDistances, ref cubeReputations, pIRATE_FACTION_ID, num, myFactionIndexer);
			BefriendClosest(ref factions2, ref factions3, ref cubeNormalizedDistances, ref cubeReputations, pIRATE_FACTION_ID, num, myFactionIndexer);
			BefriendClosest(ref factions, ref factions2, ref cubeNormalizedDistances, ref cubeReputations, pIRATE_FACTION_ID, num, myFactionIndexer);
			BefriendClosest(ref factions3, ref factions2, ref cubeNormalizedDistances, ref cubeReputations, pIRATE_FACTION_ID, num, myFactionIndexer);
			foreach (MyFaction item31 in list4)
			{
				int index19 = myFactionIndexer.GetIndex(item31);
				foreach (MyFaction item32 in list4)
				{
					int index20 = myFactionIndexer.GetIndex(item32);
					factionsRaw.SetReputationBetweenFactions(item31.FactionId, item32.FactionId, (int)cubeReputations[index19][index20]);
				}
			}
			if (MyFakes.ENABLE_RELATION_GENERATOR_DEBUG_DRAW)
			{
				Console.WriteLine("\n\n Factions:");
				int num27 = 0;
				string text = " -- ";
				foreach (MyFaction item33 in list4)
				{
					Console.WriteLine(string.Format("F {0} - {1}", (num27 < 10) ? (" " + num27) : num27.ToString(), item33.Tag));
					text += string.Format("{0} ", (num27 < 10) ? (" " + num27) : num27.ToString());
					num27++;
				}
				Console.WriteLine("");
				Console.WriteLine(text);
				int num28 = 0;
				foreach (MyFaction item34 in list4)
				{
					int index21 = myFactionIndexer.GetIndex(item34);
					string text2 = string.Format(" {0} ", (num28 < 10) ? (" " + num28) : num28.ToString());
					foreach (MyFaction item35 in list4)
					{
						int index22 = myFactionIndexer.GetIndex(item35);
						text2 += $"{DebugRelationDraw(cubeReputations[index21][index22])} ";
					}
					Console.WriteLine(text2);
					num28++;
				}
			}
			return true;
		}

		public string DebugRelationDraw(double rep)
		{
			if (rep < -500.0)
			{
				return " -";
			}
			if (rep < 500.0)
			{
				return "  ";
			}
			return " +";
		}

		private static RelationStatusPicker GetFactionRelation(MyFaction f1, MyFaction f2)
		{
			if (f1.FactionType == MyFactionTypes.Miner)
			{
				if (f2.FactionType == MyFactionTypes.Miner)
				{
					return RelationStatusPicker.Miner_Miner;
				}
				if (f2.FactionType == MyFactionTypes.Trader)
				{
					return RelationStatusPicker.Miner_Trader;
				}
				return RelationStatusPicker.Miner_Builder;
			}
			if (f1.FactionType == MyFactionTypes.Trader)
			{
				if (f2.FactionType == MyFactionTypes.Miner)
				{
					return RelationStatusPicker.Miner_Trader;
				}
				if (f2.FactionType == MyFactionTypes.Trader)
				{
					return RelationStatusPicker.Trader_Trader;
				}
				return RelationStatusPicker.Trader_Builder;
			}
			if (f2.FactionType == MyFactionTypes.Miner)
			{
				return RelationStatusPicker.Miner_Builder;
			}
			if (f2.FactionType == MyFactionTypes.Trader)
			{
				return RelationStatusPicker.Trader_Builder;
			}
			return RelationStatusPicker.Builder_Builder;
		}

		private MyFaction GetPirateFaction(MyFactionCollection collection)
		{
			_ = m_def.PirateId;
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.GetDefinition(m_def.PirateId) as MyFactionDefinition;
			MyFaction result = null;
			foreach (KeyValuePair<long, MyFaction> item in collection)
			{
				if (item.Value.Tag == myFactionDefinition.Tag)
				{
					return item.Value;
				}
			}
			return result;
		}

		private void BefriendClosest(ref List<MyFaction> factions1, ref List<MyFaction> factions2, ref List<List<double>> cubeNormalizedDistances, ref List<List<double>> cubeReputations, long PIRATE_FACTION_ID, int PIRATE_ID, MyFactionIndexer idxs)
		{
			foreach (MyFaction item in factions1)
			{
				int index = -1;
				float num = float.MaxValue;
				bool flag = false;
				bool flag2 = false;
				int index2 = idxs.GetIndex(item.FactionId);
				foreach (MyFaction item2 in factions2)
				{
					int index3 = idxs.GetIndex(item2.FactionId);
					if (item.FactionId != PIRATE_FACTION_ID && item2.FactionId != PIRATE_FACTION_ID)
					{
						if (cubeReputations[index2][index3] >= (double)m_def.ReputationFriendlyMin)
						{
							flag = true;
							break;
						}
						if ((double)num > cubeNormalizedDistances[index2][index3])
						{
							num = (float)cubeNormalizedDistances[index2][index3];
							index = index3;
							flag2 = true;
						}
					}
				}
				if (!flag && flag2)
				{
					cubeReputations[index2][index] = m_def.ReputationFriendlyMax;
					cubeReputations[index][index2] = m_def.ReputationFriendlyMax;
				}
			}
		}

		private static double ProcessRelations(double value, double min, double max, double norm, RelationStatusPicker relation)
		{
			return (value - min) / ((norm != 0.0) ? norm : 1.0);
		}

		private static List<double> CountDistances(MyFaction f1, MyFaction f2)
		{
			List<double> list = new List<double>();
			foreach (MyStation station in f1.Stations)
			{
				foreach (MyStation station2 in f2.Stations)
				{
					double item = (station.Position - station2.Position).Length();
					list.Add(item);
				}
			}
			return list;
		}

		private static Tuple<double, double, double, double> ProcessDistances(List<double> distances)
		{
			double num = double.MaxValue;
			double num2 = double.MinValue;
			double num3 = 0.0;
			double num4 = 0.0;
			foreach (double distance in distances)
			{
				num = ((num < distance) ? num : distance);
				num2 = ((num2 > distance) ? num2 : distance);
				num4 += distance;
			}
			num3 = num4 / (double)distances.Count;
			return new Tuple<double, double, double, double>(num, num2, num3, num4);
		}

		private double NormalToRep(double value, RelationStatusPicker status)
		{
			switch (status)
			{
			case RelationStatusPicker.Miner_Miner:
				return (1.0 - value) * (double)m_def.ReputationHostileMin + value * (double)m_def.ReputationNeutralMid;
			case RelationStatusPicker.Miner_Trader:
				return (1.0 - value) * (double)m_def.ReputationFriendlyMax + value * (double)m_def.ReputationHostileMid;
			case RelationStatusPicker.Miner_Builder:
				return 0.0;
			case RelationStatusPicker.Trader_Trader:
				return (1.0 - value) * (double)m_def.ReputationHostileMin + value * (double)m_def.ReputationNeutralMid;
			case RelationStatusPicker.Trader_Builder:
				return (1.0 - value) * (double)m_def.ReputationFriendlyMax + value * (double)m_def.ReputationHostileMid;
			case RelationStatusPicker.Builder_Builder:
				return (1.0 - value) * (double)m_def.ReputationHostileMin + value * (double)m_def.ReputationNeutralMid;
			default:
				return 0.0;
			}
		}
	}
}
