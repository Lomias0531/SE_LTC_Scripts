#define VRAGE
using LitJson;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using VRage;
using VRage.Game.Components;
using VRage.Http;
using VRage.Library.Utils;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 900)]
	public class MyChatBot : MySessionComponentBase
	{
		private struct Substitute
		{
			public Regex Source;

			public MyStringId Dest;
		}

		private enum ResponseType
		{
			Garbage,
			Misunderstanding,
			SmallTalk,
			ChatBot,
			Unavailable,
			Error,
			Count
		}

		private struct ChatBotResponse
		{
			public string intent
			{
				get;
				set;
			}

			public string error
			{
				get;
				set;
			}
		}

		private const string CHATBOT_URL = "https://chatbot.keenswh.com:8011/";

		private const string CHATBOT_DEV_URL = "https://chatbot2.keenswh.com:8011/";

		private static readonly char[] m_separators = new char[3]
		{
			' ',
			'\r',
			'\n'
		};

		private static readonly string[] m_nicks = new string[5]
		{
			"+bot",
			"/bot",
			"+?",
			"/?",
			"?"
		};

		private const string MISUNDERSTANDING_TEXTID = "ChatBotMisunderstanding";

		private const string UNAVAILABLE_TEXTID = "ChatBotUnavailable";

		private static readonly MyStringId[] m_smallTalk = new MyStringId[10]
		{
			MySpaceTexts.ChatBot_Rude,
			MySpaceTexts.ChatBot_ThankYou,
			MySpaceTexts.ChatBot_Generic,
			MySpaceTexts.ChatBot_HowAreYou,
			MySpaceTexts.Description_FAQ_Objective,
			MySpaceTexts.Description_FAQ_GoodBot,
			MySpaceTexts.Description_FAQ_Begin,
			MySpaceTexts.Description_FAQ_Bug,
			MySpaceTexts.Description_FAQ_Test,
			MySpaceTexts.Description_FAQ_Clang
		};

		private static readonly Regex[] m_smallTalkRegex = new Regex[m_smallTalk.Length];

		private const int MAX_MISUNDERSTANDING = 1;

		private readonly List<Substitute> m_substitutes = new List<Substitute>();

		private Regex m_stripSymbols;

		private const string OUPTUT_FILE = "c:\\x\\stats_out.csv";

		private const string INPUT_FILE = "c:\\x\\stats.csv";

		public override bool IsRequiredByGame => true;

		public MyChatBot()
		{
			int num = 0;
			while (true)
			{
				MyStringId orCompute = MyStringId.GetOrCompute("ChatBot_Substitute" + num + "_S");
				MyStringId orCompute2 = MyStringId.GetOrCompute("ChatBot_Substitute" + num + "_D");
				if (!MyTexts.Exists(orCompute) || !MyTexts.Exists(orCompute2))
				{
					break;
				}
				m_substitutes.Add(new Substitute
				{
					Source = new Regex(MyTexts.GetString(orCompute) + "(?:[ ,.?;\\-()*]|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
					Dest = orCompute2
				});
				num++;
			}
			for (num = 0; num < m_smallTalk.Length; num++)
			{
				int num2 = 0;
				string str = "";
				while (true)
				{
					MyStringId orCompute3 = MyStringId.GetOrCompute(string.Concat(m_smallTalk[num], "_Q", num2));
					if (!MyTexts.Exists(orCompute3))
					{
						break;
					}
					if (num2 != 0)
					{
						str += "(?:[ ,.?!;\\-()*]|$)|";
					}
					str += MyTexts.GetString(orCompute3);
					num2++;
				}
				str += "(?:[ ,.?!;\\-()*]|$)";
				m_smallTalkRegex[num] = new Regex(str, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
			m_stripSymbols = new Regex("(?:[^a-z0-9 ])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		public bool FilterMessage(string message, Action<string> responseAction)
		{
			string[] array = message.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length > 1)
			{
				string[] nicks = m_nicks;
				for (int i = 0; i < nicks.Length; i++)
				{
					if (nicks[i] == array[0].ToLower())
					{
						string text = "";
						for (int j = 1; j < array.Length; j++)
						{
							text = text + array[j] + " ";
						}
						text = text.Trim();
						string preprocessedText;
						string responseId;
						ResponseType responseType = Preprocess(text, out preprocessedText, out responseId);
						if (responseType == ResponseType.ChatBot)
						{
							SendMessage(text, preprocessedText, responseId, responseAction);
						}
						else
						{
							Respond(text, responseId, responseType, responseAction);
						}
						return true;
					}
				}
			}
			return false;
		}

		private ResponseType Preprocess(string messageText, out string preprocessedText, out string responseId)
		{
			preprocessedText = messageText;
			responseId = GetMisunderstandingTextId();
			ResponseType result = ResponseType.Garbage;
			string text = m_stripSymbols.Replace(messageText, "").Trim();
			if (text.Length != 0)
			{
				result = ResponseType.SmallTalk;
				string text2 = ExtractPhrases(text, out responseId);
				if (text2 != null)
				{
					preprocessedText = ApplySubstitutions(text2);
					result = ResponseType.ChatBot;
				}
			}
			return result;
		}

		private void PerformDebugTest()
		{
			File.Delete("c:\\x\\stats_out.csv");
			List<string>[] array = new List<string>[12];
			int[][] array2 = new int[6][];
			for (int i = 0; i < 6; i++)
			{
				array[i] = new List<string>();
				array[i + 6] = new List<string>();
				array2[i] = new int[6];
			}
			using (StreamWriter streamWriter = new StreamWriter("c:\\x\\stats_out.csv", append: false))
			{
				using (StreamReader reader = new StreamReader("c:\\x\\stats.csv"))
				{
					streamWriter.WriteLine("No change: ");
					int num = 0;
					foreach (IList<string> item in CsvParser.Parse(reader, ';', '"'))
					{
						_ = item.Count;
						_ = 3;
						if (item[0] != "")
						{
							if (!Enum.TryParse(item[0], out ResponseType result))
							{
								result = ResponseType.Misunderstanding;
							}
							string text = item[1];
							string text2 = item[2];
							string preprocessedText;
							string responseId;
							ResponseType responseType = Preprocess(text, out preprocessedText, out responseId);
							if (responseType == ResponseType.ChatBot)
							{
								bool done = false;
								SendRequest(preprocessedText, delegate(HttpStatusCode code, string content)
								{
									string potentialResponseId = responseId;
									responseType = Postprocess(code, content, potentialResponseId, out responseId);
									done = true;
								});
								while (!done)
								{
									Thread.Sleep(0);
								}
							}
							array2[(int)result][(int)responseType]++;
							string text3 = $"{responseType};\"{text}\";{responseId};{text2}";
							if (result == responseType && responseId == text2)
							{
								streamWriter.WriteLine(text3);
							}
							else
							{
								array[(int)(result + ((result == responseType) ? 6 : 0))].Add(text3);
							}
						}
						num++;
						_ = num % 100;
					}
				}
				streamWriter.WriteLine("---");
				for (int j = 0; j < 6; j++)
				{
					streamWriter.WriteLine(string.Concat((ResponseType)j, ": "));
					for (int k = 0; k < 2; k++)
					{
						foreach (string item2 in array[j + k * 6])
						{
							streamWriter.WriteLine(item2);
						}
						streamWriter.WriteLine("---");
					}
				}
				for (int l = 0; l < 6; l++)
				{
					string arg = string.Concat((ResponseType)l, ": ");
					for (int m = 0; m < 6; m++)
					{
						arg = arg + array2[l][m] + " ";
					}
				}
			}
		}

		private string ApplySubstitutions(string text)
		{
			foreach (Substitute substitute in m_substitutes)
			{
				text = substitute.Source.Replace(text, MyTexts.GetString(substitute.Dest));
			}
			return text;
		}

		private string ExtractPhrases(string messageText, out string potentialResponseId)
		{
			potentialResponseId = null;
			for (int i = 0; i < m_smallTalkRegex.Length; i++)
			{
				string text = m_smallTalkRegex[i].Replace(messageText, "");
				if (text.Length != messageText.Length)
				{
					potentialResponseId = m_smallTalk[i].ToString();
					if (text.Trim().Length < 4)
					{
						return null;
					}
					return text;
				}
			}
			return messageText;
		}

		private HttpData[] CreateChatbotRequest(string preprocessedQuestion)
		{
			string value = DateTime.UtcNow.ToString("r", CultureInfo.InvariantCulture);
			string value2 = $"{{\"state\": \"DEFAULT\", \"utterance\": \"{preprocessedQuestion}\"}}";
			return new HttpData[3]
			{
				new HttpData("Date", value, HttpDataType.HttpHeader),
				new HttpData("Content-Type", "application/json", HttpDataType.HttpHeader),
				new HttpData("application/json", value2, HttpDataType.RequestBody)
			};
		}

		private void SendRequest(string preprocessedQuestion, Action<HttpStatusCode, string> onDone)
		{
			HttpData[] parameters = CreateChatbotRequest(preprocessedQuestion);
			MyVRage.Platform.Http.SendRequestAsync((MyFakes.USE_GOODBOT_DEV_SERVER ? "https://chatbot2.keenswh.com:8011/" : "https://chatbot.keenswh.com:8011/") + "intent", parameters, HttpMethod.POST, onDone);
		}

		private void SendMessage(string originalQuestion, string preprocessedQuestion, string potentialResponseId, Action<string> responseAction)
		{
			SendRequest(originalQuestion, delegate(HttpStatusCode x, string y)
			{
				OnResponse(x, y, responseAction, potentialResponseId, originalQuestion);
			});
		}

		private ResponseType Postprocess(HttpStatusCode code, string content, string potentialResponseId, out string responseId)
		{
			responseId = "ChatBotUnavailable";
			ResponseType result = ResponseType.ChatBot;
			if (code >= HttpStatusCode.OK && code <= (HttpStatusCode)299)
			{
				ChatBotResponse chatBotResponse;
				try
				{
					chatBotResponse = JsonMapper.ToObject<ChatBotResponse>(content);
				}
				catch (Exception arg)
				{
					MyLog.Default.WriteLine($"Chatbot reponse error: {arg}\n{content}");
					throw;
				}
				if (chatBotResponse.error == null)
				{
					if (chatBotResponse.intent == null)
					{
						if (potentialResponseId == null)
						{
							responseId = GetMisunderstandingTextId();
							result = ResponseType.Misunderstanding;
						}
						else
						{
							responseId = potentialResponseId;
							result = ResponseType.SmallTalk;
						}
					}
					else
					{
						responseId = chatBotResponse.intent;
					}
				}
				else
				{
					result = ResponseType.Error;
				}
			}
			else
			{
				result = ResponseType.Unavailable;
			}
			return result;
		}

		private void OnResponse(HttpStatusCode code, string content, Action<string> responseAction, string potentialResponseId, string question)
		{
			string responseId;
			ResponseType responseType = Postprocess(code, content, potentialResponseId, out responseId);
			Respond(question, responseId, responseType, responseAction);
		}

		private string GetMisunderstandingTextId()
		{
			return "ChatBotMisunderstanding" + MyRandom.Instance.Next(0, 1);
		}

		private void Respond(string question, string responseId, ResponseType responseType, Action<string> responseAction)
		{
			MyAnalyticsHelper.ReportBug(string.Concat("GoodBot(", responseType, "): ", question.Replace("\"", ""), " / ", responseId), null, firstTimeOnly: false, string.Empty, 354);
			string text = MyTexts.GetString(responseId);
			MySandboxGame.Static.Invoke(delegate
			{
				responseAction(text);
			}, "OnChatBotResponse");
		}
	}
}
