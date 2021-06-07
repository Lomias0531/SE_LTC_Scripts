using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.ModAPI.Ingame;
using System.Text;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
	[MyTextSurfaceScript("TSS_VendingMachine", "DisplayName_TSS_VendingMachine")]
	public class MyTSSVendingMachine : MyTSSCommon
	{
		private static float DEFAULT_SCREEN_SIZE = 512f;

		private static int MESSAGE_TIME_FRAMES = 18;

		private MyVendingMachine m_vendingMachine;

		private int m_messageTimer = -1;

		private MyStoreBuyItemResult m_lastStoreResult;

		private long m_tickCtr;

		public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10;

		public MyTSSVendingMachine(IMyTextSurface surface, IMyCubeBlock block, Vector2 size)
			: base(surface, block, size)
		{
			m_vendingMachine = (block as MyVendingMachine);
			m_fontId = "White";
			if (m_vendingMachine != null)
			{
				m_vendingMachine.OnBuyItemResult += OnBuyItemResult;
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			if (m_vendingMachine != null)
			{
				m_vendingMachine.OnBuyItemResult -= OnBuyItemResult;
			}
		}

		private void OnBuyItemResult(MyStoreBuyItemResult obj)
		{
			m_lastStoreResult = obj;
			m_messageTimer = 0;
		}

		public override void Run()
		{
			base.Run();
			using (MySpriteDrawFrame frame = m_surface.DrawFrame())
			{
				Vector2 vector = m_halfSize - m_surface.SurfaceSize * 0.5f;
				_ = m_surface.SurfaceSize.Y / m_surface.SurfaceSize.X;
				if (m_vendingMachine == null)
				{
					MySprite sprite = MySprite.CreateText(MyTexts.GetString(MySpaceTexts.VendingMachine_Script_DataUnavailable), m_fontId, m_foregroundColor, DEFAULT_SCREEN_SIZE / m_size.X);
					sprite.Position = m_halfSize;
					frame.Add(sprite);
					return;
				}
				if (m_vendingMachine.SelectedItemIdx < 0 || m_vendingMachine.SelectedItem == null)
				{
					DrawErrorMessage(frame, vector);
				}
				else
				{
					MyPhysicalItemDefinition definition = null;
					if (m_vendingMachine.SelectedItem.Item.HasValue && !MyDefinitionManager.Static.TryGetDefinition(m_vendingMachine.SelectedItem.Item.Value, out definition))
					{
						DrawErrorMessage(frame, vector);
						return;
					}
					MySprite sprite2 = MySprite.CreateSprite("LCD_Economy_Vending_Bg", vector + m_surface.SurfaceSize * 0.5f, m_surface.SurfaceSize);
					sprite2.Color = m_foregroundColor;
					frame.Add(sprite2);
					float num = m_scale.Y;
					Vector2 vector2 = m_surface.MeasureStringInPixels(new StringBuilder(definition.DisplayNameText), m_fontId, num);
					float num2 = m_surface.SurfaceSize.X * 0.43f;
					if (vector2.X > num2)
					{
						num *= num2 / vector2.X;
					}
					MySprite sprite3 = MySprite.CreateText(definition.DisplayNameText, m_fontId, m_foregroundColor, num);
					sprite3.Position = vector + new Vector2(m_surface.SurfaceSize.X * 0.69f, m_surface.SurfaceSize.Y * 0.26f - vector2.Y * 0.5f * num);
					frame.Add(sprite3);
					Vector2 size = (m_surface.SurfaceSize.X < m_surface.SurfaceSize.Y) ? new Vector2(m_surface.SurfaceSize.X) : new Vector2(m_surface.SurfaceSize.Y);
					size *= 0.45f;
					Vector2 position = vector + new Vector2(size.X * 0.9f, m_surface.SurfaceSize.Y * 0.48f);
					MySprite sprite4 = MySprite.CreateSprite(definition.Id.ToString(), position, size);
					frame.Add(sprite4);
					string text = MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ItemAmount) + m_vendingMachine.SelectedItem.Amount;
					Color color = (m_vendingMachine.SelectedItem.Amount <= 0) ? Color.Red : m_foregroundColor;
					MySprite sprite5 = MySprite.CreateText(text, m_fontId, color, m_scale.Y * 0.8f);
					sprite5.Position = vector + new Vector2(m_surface.SurfaceSize.X * 0.69f, m_surface.SurfaceSize.Y * 0.48f);
					frame.Add(sprite5);
					Vector2 vector3 = m_surface.MeasureStringInPixels(new StringBuilder(text), m_fontId, m_scale.Y * 0.8f);
					string formatedValue = MyBankingSystem.GetFormatedValue(m_vendingMachine.SelectedItem.PricePerUnit, addCurrencyShortName: true);
					MySprite sprite6 = MySprite.CreateText(MyTexts.GetString(MySpaceTexts.VendingMachine_Script_PricePerUnit) + formatedValue, m_fontId, m_foregroundColor, m_scale.Y * 0.8f);
					sprite6.Position = sprite5.Position + new Vector2(0f, vector3.Y + 5f);
					frame.Add(sprite6);
				}
				if (m_messageTimer >= 0)
				{
					string messageString = GetMessageString();
					DrawMessage(frame, vector + m_surface.SurfaceSize * 0.5f, messageString, m_scale.Y);
					m_messageTimer++;
					if (m_messageTimer > MESSAGE_TIME_FRAMES)
					{
						m_messageTimer = -1;
					}
				}
			}
			m_tickCtr++;
		}

		public string GetMessageString()
		{
			string text = null;
			switch (m_lastStoreResult.Result)
			{
			case MyStoreBuyItemResults.Success:
				return MyTexts.GetString(MySpaceTexts.VendingMachine_Script_MessageBuy);
			case MyStoreBuyItemResults.NotEnoughMoney:
				return MyTexts.GetString(MySpaceTexts.VendingMachine_Script_NoMoney);
			case MyStoreBuyItemResults.ItemsTimeout:
				return MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ItemsTimeout);
			case MyStoreBuyItemResults.NotEnoughInventorySpace:
				return MyTexts.GetString(MySpaceTexts.VendingMachine_Script_NotEnoughSpace);
			case MyStoreBuyItemResults.NotEnoughAmount:
				return MyTexts.GetString(MySpaceTexts.VendingMachine_Script_OutOfStock);
			default:
				return MyTexts.GetString(MySpaceTexts.VendingMachine_Script_MessageError);
			}
		}

		private void DrawErrorMessage(MySpriteDrawFrame frame, Vector2 topLeftCorner)
		{
			Vector2 vector = topLeftCorner + new Vector2(m_surface.SurfaceSize.X * 0.5f, m_surface.SurfaceSize.Y * 0.32f);
			DrawMessage(frame, vector, MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ConnectingToServer), m_scale.Y * 0.9f, drawBg: false);
			Vector2 position = vector + new Vector2(0f, m_surface.SurfaceSize.Y * 0.09f);
			DrawMessage(frame, position, MyTexts.GetString(MySpaceTexts.VendingMachine_Script_ContactAdmin), m_scale.Y * 0.4f, drawBg: false);
			Vector2 value = vector + new Vector2(0f, m_surface.SurfaceSize.Y * 0.3f);
			Vector2 value2 = new Vector2(m_surface.SurfaceSize.X * 0.1f);
			MySprite sprite = new MySprite(SpriteType.TEXTURE, "Screen_LoadingBar", value, value2, null, null, TextAlignment.CENTER, (float)(-m_tickCtr) * 0.12f);
			frame.Add(sprite);
			MySprite sprite2 = new MySprite(SpriteType.TEXTURE, "Screen_LoadingBar", value, value2 * 0.66f, null, null, TextAlignment.CENTER, (float)m_tickCtr * 0.12f);
			frame.Add(sprite2);
			MySprite sprite3 = new MySprite(SpriteType.TEXTURE, "Screen_LoadingBar", value, value2 * 0.41f, null, null, TextAlignment.CENTER, (float)(-m_tickCtr) * 0.12f);
			frame.Add(sprite3);
		}

		private void DrawMessage(MySpriteDrawFrame frame, Vector2 position, string messageString, float fontSize, bool drawBg = true)
		{
			Vector2 value = m_surface.MeasureStringInPixels(new StringBuilder(messageString), m_fontId, fontSize * 1.5f);
			if (drawBg)
			{
				MySprite sprite = MySprite.CreateSprite("SquareSimple", position, value * 1.05f);
				sprite.Color = Color.Black;
				frame.Add(sprite);
			}
			MySprite sprite2 = MySprite.CreateText(messageString, m_fontId, m_foregroundColor, fontSize * 1.5f);
			sprite2.Position = position - new Vector2(0f, value.Y * 0.5f);
			frame.Add(sprite2);
		}
	}
}
