using LitJson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using VRage.Compression;
using VRage.GameServices;
using VRage.Http;
using VRage.Mod.Io.Data;
using VRage.Utils;

namespace VRage.Mod.Io
{
	internal static class MyModIo
	{
		public enum Sort
		{
			Name,
			Subscribers
		}

		private enum ServiceType
		{
			Steam,
			XboxLive,
			Unknown
		}

		private class MyRequestSetup
		{
			public string Url;

			public HttpMethod Method;

			public List<HttpData> Parameters;
		}

		private static IMyGameService m_service;

		private static MyModIoServiceInternal m_modIoService;

		private static string m_gameId;

		private static string m_gameName;

		private static string m_apiKey;

		private static string URL_BASE;

		private static string API_BASE_URL;

		private static string API_URL;

		public static string WEB_URL;

		private static ulong EMAIL_AUTH;

		private static string MODS_API;

		private static string EDIT_MOD_API;

		private static string ADD_MOD_MEDIA_API;

		private static string MY_SUBSCRIPTIONS_API;

		private static string SUBSCRIBE_API;

		private static string MOD_DEPENDENCY_API;

		private static string AUTHENTICATE_STEAM_API;

		private static string AUTHENTICATE_XBOX_LIVE_API;

		private static string ADD_MOD_FILE_API;

		private static string LINK_ACCOUNT_API;

		private static string EMAIL_REQUEST_API;

		private static string EMAIL_EXCHANGE_API;

		private static readonly ConcurrentQueue<Action> m_invoke;

		private static readonly List<string> m_paramsTemp;

		private static bool m_suspendDownloads;

		private static bool m_authenticated;

		private static ulong m_authenticatedUserId;

		private static AccessToken m_authenticatedToken;

		static MyModIo()
		{
			URL_BASE = "test.mod.io";
			API_BASE_URL = "https://api." + URL_BASE + "/";
			API_URL = API_BASE_URL + "v1/";
			WEB_URL = "https://{0}." + URL_BASE + "/";
			EMAIL_AUTH = ulong.MaxValue;
			MODS_API = "games/{0}/mods";
			EDIT_MOD_API = "games/{{0}}/mods/{0}";
			ADD_MOD_MEDIA_API = "games/{{0}}/mods/{0}/media";
			MY_SUBSCRIPTIONS_API = "me/subscribed";
			SUBSCRIBE_API = "games/{{0}}/mods/{0}/subscribe";
			MOD_DEPENDENCY_API = "games/{{0}}/mods/{0}/dependencies";
			AUTHENTICATE_STEAM_API = "external/steamauth";
			AUTHENTICATE_XBOX_LIVE_API = "external/xboxauth";
			ADD_MOD_FILE_API = "games/{{0}}/mods/{0}/files";
			LINK_ACCOUNT_API = "external/link";
			EMAIL_REQUEST_API = "oauth/emailrequest";
			EMAIL_EXCHANGE_API = "oauth/emailexchange";
			m_invoke = new ConcurrentQueue<Action>();
			m_paramsTemp = new List<string>();
		}

		public static void SuspendDownloads(bool state)
		{
			m_suspendDownloads = state;
		}

		public static void Init(IMyGameService service, MyModIoServiceInternal modIoService, string gameId, string gameName, string apiKey)
		{
			m_service = service;
			m_modIoService = modIoService;
			m_gameId = gameId;
			m_gameName = gameName;
			m_apiKey = apiKey;
		}

		public static void InvokeOnMainThread(Action action)
		{
			m_invoke.Enqueue(action);
		}

		public static void Update()
		{
			Action result;
			while (m_invoke.TryDequeue(out result))
			{
				result.InvokeIfNotNull();
			}
		}

		private static ServiceType GetServiceType()
		{
			if (m_service.ServiceName.ToLower() == "steam")
			{
				return ServiceType.Steam;
			}
			return ServiceType.Unknown;
		}

		private static string GetUrl(string api, params string[] p)
		{
			string text = string.Format(API_URL + api + "?api_key=" + m_apiKey, m_gameId);
			foreach (string text2 in p)
			{
				if (text2 != null)
				{
					text = text + "&" + text2;
				}
			}
			return text;
		}

		private static void AddAuthorizationHeader(MyRequestSetup request)
		{
			int num = request.Parameters.FindIndex((HttpData x) => x.Name == "Authorization");
			if (num != -1)
			{
				request.Parameters.RemoveAt(num);
			}
			if (m_authenticated)
			{
				request.Parameters.Add(new HttpData("Authorization", "Bearer " + m_authenticatedToken.access_token, HttpDataType.HttpHeader));
			}
		}

		private static MyRequestSetup CreateRequest(string function, HttpMethod method, string contentType, params string[] p)
		{
			MyRequestSetup myRequestSetup = new MyRequestSetup
			{
				Url = GetUrl(function, p),
				Method = method,
				Parameters = new List<HttpData>
				{
					new HttpData("Accept", "application/json", HttpDataType.HttpHeader)
				}
			};
			if (contentType != null)
			{
				myRequestSetup.Parameters.Add(new HttpData("Content-Type", contentType, HttpDataType.HttpHeader));
			}
			AddAuthorizationHeader(myRequestSetup);
			return myRequestSetup;
		}

		private static void SendRequest<T>(MyRequestSetup request, Action<T, MyGameServiceCallResult> action) where T : class
		{
			MyVRage.Platform.Http.SendRequestAsync(request.Url, request.Parameters.ToArray(), request.Method, delegate(HttpStatusCode x, string y)
			{
				ResponseJson(request, x, y, action);
			});
		}

		private static void ResponseJson<T>(MyRequestSetup request, HttpStatusCode code, string content, Action<T, MyGameServiceCallResult> onDone) where T : class
		{
			T data = null;
			MyGameServiceCallResult result = GetError(code, content);
			if (result == MyGameServiceCallResult.OK && content != null)
			{
				try
				{
					data = JsonMapper.ToObject<T>(content);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
					result = MyGameServiceCallResult.InvalidParam;
				}
			}
			string text = "";
			if (request.Parameters != null)
			{
				foreach (HttpData parameter in request.Parameters)
				{
					text = string.Concat(text, parameter.Type, ": ", parameter.Name, " / ", parameter.Value, "\n");
				}
			}
			MyLog.Default.WriteLine(string.Concat("ModIo API call\n-- Request:\nUrl: ", request.Url, "\nMethod: ", request.Method, "\nParameters:\n", text, "\n-- Response:\nCode: ", code, " / ", result, "\nContent: ", content));
			if (onDone != null)
			{
				InvokeOnMainThread(delegate
				{
					onDone(data, result);
				});
			}
		}

		private static void ResponseDownload(MyRequestSetup request, HttpStatusCode code, Action<MyGameServiceCallResult> onDone)
		{
			MyGameServiceCallResult result = GetError(code, null);
			InvokeOnMainThread(delegate
			{
				onDone(result);
			});
		}

		private static MyGameServiceCallResult GetError(HttpStatusCode code, string content)
		{
			MyGameServiceCallResult result = MyGameServiceCallResult.OK;
			switch (code)
			{
			case HttpStatusCode.OK:
			case HttpStatusCode.Created:
				return result;
			default:
				return MyGameServiceCallResult.ServiceUnavailable;
			case HttpStatusCode.NoContent:
				return MyGameServiceCallResult.FileNotFound;
			case HttpStatusCode.BadRequest:
				return MyGameServiceCallResult.InvalidParam;
			case HttpStatusCode.Unauthorized:
				return MyGameServiceCallResult.InvalidLoginAuthCode;
			case HttpStatusCode.Forbidden:
				return MyGameServiceCallResult.AccessDenied;
			case HttpStatusCode.NotFound:
				return MyGameServiceCallResult.FileNotFound;
			case HttpStatusCode.MethodNotAllowed:
				return MyGameServiceCallResult.InvalidParam;
			case HttpStatusCode.NotAcceptable:
				return MyGameServiceCallResult.InvalidParam;
			case HttpStatusCode.Gone:
				return MyGameServiceCallResult.FileNotFound;
			}
		}

		private static string[] CreateParams(Sort sort, string searchString, List<ulong> itemIds, List<string> requiredTags, List<string> excludedTags, int page, uint itemsPerPage, bool gameId = false)
		{
			m_paramsTemp.Clear();
			if (!string.IsNullOrWhiteSpace(searchString))
			{
				m_paramsTemp.Add("_q=" + searchString);
			}
			if (requiredTags != null && requiredTags.Count > 0)
			{
				m_paramsTemp.Add("tags-in=" + string.Join(",", requiredTags));
			}
			if (excludedTags != null && excludedTags.Count > 0)
			{
				m_paramsTemp.Add("tags-not-in=" + string.Join(",", excludedTags));
			}
			if (itemIds != null && itemIds.Count > 0)
			{
				m_paramsTemp.Add("id-in=" + string.Join(",", itemIds));
			}
			m_paramsTemp.Add("_sort=" + sort.ToString().ToLower());
			m_paramsTemp.Add("_offset=" + page * itemsPerPage);
			m_paramsTemp.Add("_limit=" + itemsPerPage);
			if (gameId)
			{
				m_paramsTemp.Add("game_id=" + m_gameId);
			}
			return m_paramsTemp.ToArray();
		}

		public static void GetMods(Action<RequestPage<ModProfile>, MyGameServiceCallResult> onDone, Sort sort, string searchString, List<ulong> itemIds, List<string> requiredTags, List<string> excludedTags, int page, uint itemsPerPage)
		{
			string[] p = CreateParams(sort, searchString, itemIds, requiredTags, excludedTags, page, itemsPerPage);
			SendRequest(CreateRequest(MODS_API, HttpMethod.GET, null, p), onDone);
		}

		public static void GetModDependencies(ulong id, Action<RequestPage<ModDependency>, MyGameServiceCallResult> onDone)
		{
			SendRequest(CreateRequest(string.Format(MOD_DEPENDENCY_API, id), HttpMethod.GET, null), onDone);
		}

		public static void DownloadFile(string url, string filename, Action<MyGameServiceCallResult> onDone, Action<ulong> onProgress)
		{
			MyRequestSetup request = new MyRequestSetup
			{
				Method = HttpMethod.GET,
				Url = url
			};
			MyVRage.Platform.Http.DownloadAsync(url, filename, onProgress, delegate(HttpStatusCode x)
			{
				ResponseDownload(request, x, onDone);
			});
		}

		private static void Authenticate(bool share, Action onDone)
		{
			MyLog.Default.WriteLine("ModIo: Authentication started");
			if (m_authenticated && (m_authenticatedUserId == EMAIL_AUTH || m_service.UserId == m_authenticatedUserId))
			{
				onDone();
			}
			else if (!m_service.IsActive)
			{
				MyLog.Default.WriteLine("ModIo: No User");
				m_modIoService.AuthenticationFailed(UGCAuthenticationFailReason.NoUser, onDone);
			}
			else if (GetServiceType() == ServiceType.Unknown)
			{
				MyLog.Default.WriteLine("ModIo: Unknown service type");
				m_modIoService.AuthenticationFailed(UGCAuthenticationFailReason.SSOUnsupported, onDone);
			}
			else
			{
				m_service.RequestPermissions(share ? Permissions.ShareContent : Permissions.UGC, attemptResolution: true, delegate(bool x)
				{
					OnPermissions(x, share, onDone);
				});
			}
		}

		private static void OnPermissions(bool granted, bool share, Action onDone)
		{
			if (granted)
			{
				m_service.RequestEncryptedAppTicket(API_BASE_URL, delegate(string x)
				{
					AuthenticateWithTicket(x, onDone);
				});
			}
			else
			{
				m_modIoService.AuthenticationFailed(share ? UGCAuthenticationFailReason.PublishRestricted : UGCAuthenticationFailReason.Restricted, onDone);
			}
		}

		private static void AuthenticateWithTicket(string ticket, Action onDone)
		{
			if (ticket != null)
			{
				MyRequestSetup myRequestSetup;
				switch (GetServiceType())
				{
				case ServiceType.Steam:
					myRequestSetup = CreateRequest(AUTHENTICATE_STEAM_API, HttpMethod.POST, "application/x-www-form-urlencoded", "appdata=" + ticket);
					break;
				case ServiceType.XboxLive:
					myRequestSetup = CreateRequest(AUTHENTICATE_XBOX_LIVE_API, HttpMethod.POST, "application/x-www-form-urlencoded");
					myRequestSetup.Parameters.Add(new HttpData("xbox_token", ticket, HttpDataType.GetOrPost));
					myRequestSetup.Parameters.Add(new HttpData("ao_feature", "WMbghHrmTsg70TQcYlQWO9", HttpDataType.GetOrPost));
					break;
				default:
					m_modIoService.AuthenticationFailed(UGCAuthenticationFailReason.SSOUnsupported, onDone);
					return;
				}
				SendRequest(myRequestSetup, delegate(AccessToken z, MyGameServiceCallResult w)
				{
					OnAuthenticated(m_service.UserId, z, w, onDone);
				});
			}
			else
			{
				m_modIoService.AuthenticationFailed(UGCAuthenticationFailReason.SSOUnsupported, onDone);
			}
		}

		private static void OnAuthenticated(ulong userId, AccessToken token, MyGameServiceCallResult result, Action onDone)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				m_authenticated = true;
				m_authenticatedUserId = userId;
				m_authenticatedToken = token;
			}
			else
			{
				m_authenticated = false;
			}
			onDone.InvokeIfNotNull();
		}

		public static void GetMySubscriptions(Action<RequestPage<ModProfile>, MyGameServiceCallResult> onDone, Sort sort, string searchString, List<ulong> itemIds, List<string> requiredTags, List<string> excludedTags, int page, uint itemsPerPage)
		{
			string[] custom = CreateParams(sort, searchString, itemIds, requiredTags, excludedTags, page, itemsPerPage, gameId: true);
			Authenticate(share: false, delegate
			{
				GetMySubscriptionsInternal(custom, onDone);
			});
		}

		private static void GetMySubscriptionsInternal(string[] custom, Action<RequestPage<ModProfile>, MyGameServiceCallResult> onDone)
		{
			if (!m_authenticated)
			{
				onDone.InvokeIfNotNull(null, MyGameServiceCallResult.InvalidLoginAuthCode);
			}
			else
			{
				SendRequest(CreateRequest(MY_SUBSCRIPTIONS_API, HttpMethod.GET, null, custom), onDone);
			}
		}

		public static void Subscribe(ulong modId)
		{
			string function = string.Format(SUBSCRIBE_API, modId);
			Authenticate(share: false, delegate
			{
				SubscribeInternal(function);
			});
		}

		private static void SubscribeInternal(string function)
		{
			if (m_authenticated)
			{
				SendRequest<ModProfile>(CreateRequest(function, HttpMethod.POST, "application/x-www-form-urlencoded"), null);
			}
		}

		public static void AddOrEditMod(ulong id, string name, string description, MyPublishedFileVisibility visibility, List<string> tags, string metadata, string thumbnailFile, string contentFolder, Action<Modfile, MyGameServiceCallResult> onPublished)
		{
			MyRequestSetup request;
			if (id == 0L)
			{
				request = CreateRequest(MODS_API, HttpMethod.POST, "multipart/form-data");
				if (!string.IsNullOrEmpty(thumbnailFile))
				{
					request.Parameters.Add(new HttpData("logo", thumbnailFile, HttpDataType.Filename));
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(thumbnailFile))
				{
					string function = string.Format(ADD_MOD_MEDIA_API, id);
					MyRequestSetup requestLogo = CreateRequest(function, HttpMethod.POST, "multipart/form-data");
					requestLogo.Parameters.Add(new HttpData("logo", thumbnailFile, HttpDataType.Filename));
					Authenticate(share: true, delegate
					{
						AddModLogoInternal(requestLogo, delegate
						{
							AddOrEditMod(id, name, description, visibility, tags, metadata, null, contentFolder, onPublished);
						});
					});
					return;
				}
				string function2 = string.Format(EDIT_MOD_API, id);
				request = CreateRequest(function2, HttpMethod.PUT, "application/x-www-form-urlencoded");
			}
			request.Parameters.Add(new HttpData("name", name, HttpDataType.GetOrPost));
			request.Parameters.Add(new HttpData("summary", name, HttpDataType.GetOrPost));
			if (!string.IsNullOrEmpty(description))
			{
				request.Parameters.Add(new HttpData("description", description, HttpDataType.GetOrPost));
			}
			request.Parameters.Add(new HttpData("visibility", ((visibility != MyPublishedFileVisibility.Private) ? 1 : 0).ToString(), HttpDataType.GetOrPost));
			if (tags != null)
			{
				foreach (string tag in tags)
				{
					request.Parameters.Add(new HttpData("tags[]", tag, HttpDataType.GetOrPost));
				}
			}
			if (!string.IsNullOrEmpty(metadata))
			{
				request.Parameters.Add(new HttpData("metadata_blob", metadata, HttpDataType.GetOrPost));
			}
			Authenticate(share: true, delegate
			{
				AddOrEditModInternal(request, contentFolder, onPublished);
			});
		}

		private static void AddModLogoInternal(MyRequestSetup request, Action action)
		{
			if (!m_authenticated)
			{
				action.InvokeIfNotNull();
				return;
			}
			AddAuthorizationHeader(request);
			SendRequest<GenericResponse>(request, delegate
			{
				action();
			});
		}

		private static void AddOrEditModInternal(MyRequestSetup request, string contentFolder, Action<Modfile, MyGameServiceCallResult> onPublished)
		{
			if (!m_authenticated)
			{
				onPublished.InvokeIfNotNull(null, MyGameServiceCallResult.InvalidLoginAuthCode);
				return;
			}
			AddAuthorizationHeader(request);
			SendRequest(request, delegate(ModProfile z, MyGameServiceCallResult w)
			{
				OnAddOrEditMod(z, w, contentFolder, onPublished);
			});
		}

		private static void OnAddOrEditMod(ModProfile mod, MyGameServiceCallResult result, string contentFolder, Action<Modfile, MyGameServiceCallResult> onPublished)
		{
			if (result != MyGameServiceCallResult.OK)
			{
				onPublished.InvokeIfNotNull(null, result);
			}
			else
			{
				AddModFile((ulong)mod.id, contentFolder, onPublished);
			}
		}

		public static void AddModFile(ulong modId, string contentFolder, Action<Modfile, MyGameServiceCallResult> onPublished)
		{
			Authenticate(share: true, delegate
			{
				AddModFileInternal(modId, contentFolder, onPublished);
			});
		}

		private static void AddModFileInternal(ulong modId, string contentFolder, Action<Modfile, MyGameServiceCallResult> onPublished)
		{
			if (!m_authenticated)
			{
				onPublished.InvokeIfNotNull(null, MyGameServiceCallResult.InvalidLoginAuthCode);
				return;
			}
			string text = contentFolder;
			try
			{
				if (!File.Exists(contentFolder))
				{
					text += ".zip";
					File.Delete(text);
					using (MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(text, ZipArchiveMode.Create))
					{
						int startIndex = contentFolder.Length + 1;
						string[] files = Directory.GetFiles(contentFolder, "*.*", SearchOption.AllDirectories);
						foreach (string text2 in files)
						{
							using (FileStream fileStream = File.Open(text2, FileMode.Open, FileAccess.Read, FileShare.Read))
							{
								using (Stream destination = myZipArchive.AddFile(text2.Substring(startIndex), CompressionLevel.Optimal).GetStream())
								{
									fileStream.CopyTo(destination, 4096);
								}
							}
						}
					}
				}
				MyRequestSetup myRequestSetup = CreateRequest(string.Format(ADD_MOD_FILE_API, modId), HttpMethod.POST, "multipart/form-data");
				myRequestSetup.Parameters.Add(new HttpData("filedata", text, HttpDataType.Filename));
				SendRequest(myRequestSetup, onPublished);
			}
			catch
			{
				onPublished(null, MyGameServiceCallResult.AccessDenied);
			}
		}

		public static void LinkAccount(string email, Action<MyGameServiceCallResult> onDone)
		{
			Authenticate(share: false, delegate
			{
				LinkAccountInternal(email, onDone);
			});
		}

		private static void LinkAccountInternal(string email, Action<MyGameServiceCallResult> onDone)
		{
			if (!m_authenticated)
			{
				onDone.InvokeIfNotNull(MyGameServiceCallResult.InvalidLoginAuthCode);
				return;
			}
			MyRequestSetup myRequestSetup = CreateRequest(LINK_ACCOUNT_API, HttpMethod.POST, "application/x-www-form-urlencoded");
			myRequestSetup.Parameters.Add(new HttpData("service", GetServiceType().ToString().ToLower(), HttpDataType.GetOrPost));
			myRequestSetup.Parameters.Add(new HttpData("service_id", m_service.UserId, HttpDataType.GetOrPost));
			myRequestSetup.Parameters.Add(new HttpData("email", email, HttpDataType.GetOrPost));
			SendRequest(myRequestSetup, delegate(GenericResponse z, MyGameServiceCallResult w)
			{
				onDone(w);
			});
		}

		public static void EmailRequest(string email, Action<MyGameServiceCallResult> onCompleted)
		{
			MyRequestSetup myRequestSetup = CreateRequest(EMAIL_REQUEST_API, HttpMethod.POST, "application/x-www-form-urlencoded");
			myRequestSetup.Parameters.Add(new HttpData("email", email, HttpDataType.GetOrPost));
			SendRequest(myRequestSetup, delegate(GenericResponse z, MyGameServiceCallResult w)
			{
				MyLog.Default.WriteLine("EmailRequest: " + w);
				onCompleted(w);
			});
		}

		public static void EmailExchange(string code, Action<MyGameServiceCallResult> onCompleted)
		{
			MyRequestSetup myRequestSetup = CreateRequest(EMAIL_EXCHANGE_API, HttpMethod.POST, "application/x-www-form-urlencoded");
			myRequestSetup.Parameters.Add(new HttpData("security_code", code, HttpDataType.GetOrPost));
			SendRequest(myRequestSetup, delegate(AccessToken z, MyGameServiceCallResult w)
			{
				EmailExchangeResponse(z, w, onCompleted);
			});
		}

		private static void EmailExchangeResponse(AccessToken token, MyGameServiceCallResult result, Action<MyGameServiceCallResult> onCompleted)
		{
			if (result == MyGameServiceCallResult.OK)
			{
				m_authenticated = true;
				m_authenticatedUserId = EMAIL_AUTH;
				m_authenticatedToken = token;
			}
			onCompleted(result);
		}

		public static string GetWebUrl()
		{
			return string.Format(WEB_URL, m_gameName);
		}
	}
}
