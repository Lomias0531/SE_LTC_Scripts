using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScript
{
    class Example:API
    {
		/*
 ======= [ MEA ] 全自动转子基座炮台控制程序 FCS-R v3.0.1 by MEA群主 QQ群530683714 =======
 【安装】
	1、把一个炮台上所有方块都放入一个编组里，编组的名字将作为炮台的名字。有多个炮台的时候必须各是各的编组。
	2、炮台必须至少有一个远程控制块（或驾驶舱，或控制座椅），并且它的名字必须包含 AimBlockKey 所设定的关键字。默认是 FCS#R
	3、将所有炮台放置完成以后，找一个编程块，把本代码全部复制进去即可
	4、每个炮台都有属于自己的独立参数，如需调整请前往每个炮台的远程控制块的[自定义数据]即CustomData中修改。如需修改全局默认配置，可在Ship类顶部直接修改
	5、每个炮台都依然保留了探测器保护模式，你可以装一些探测器并设置探测自己。将这些探测器的范围设置为仅覆盖炮台前方，当探测器响应，炮台在任何情况下都不会开火。
	6、可以安装一些摄像头，它们可以用来自主锁定。这样当你的自动武器被打掉，依然能保持相当的战斗力。

 【v3.0】
	1、使用修改后的Ship类作为底层框架来封装每个转子炮台
	2、对转子基座炮台的基础方块控制做了统一的控制方法，例如转子的批量控制，采用了RotorsField来做方向判别，同时在方块控制层检测了转子的角度限制，防止转子超限导致卡死
	3、本版本的指令将采用战略意图指令，摒弃了繁琐的具体动作指令操作。
	4、本版本大大增强了玩家手动控制炮台时的功能操作，因此也要求炮台的瞄准块必须是远程控制块（或驾驶舱、飞行座椅、主控座椅）
 v 3.0.1
		修复了一个锁定不准确而且目标丢失后不会解除锁定的bug

 【工作模式】
	每个炮台都有自己独立的工作模式，可以单独设置也可以群体控制。模式如下：

	Stop:停止所有运动功能
	Attention:被动模式，此时会检测自动武器是否有目标，如果有并且位置合适则进行攻击
	Active:主动模式，此时会检测所有自动武器和其他转子基座炮台主动锁定的目标，发现目标后启用自主锁定并攻击至目标消失或丢失

	你可以直接对编程块使用指令，输入工作模式的名字来切换所有炮台的工作模式
	例如：Active
	也可以在指令名后加一个英文冒号，紧跟一个炮台名字，来针对性的控制某个炮台
	例如：Stop:PT-1

 【集群控制指令表】
	当任何玩家手动控制了任何炮台后，该炮台作为主炮台，自动将其他炮台等比例分配给该炮台（例如2个玩家控制了2个炮台，其余4个未被控制的炮台会按距离就近原则，平均的分配给两个主炮台）
	每个炮台都有自己作为主炮台的独立参数，例如是否开启从炮台跟随瞄准，跟随瞄准的瞄准聚焦点距离等。这些参数每个炮台都是独立的。
	你可以通过下方的指令来控制主炮台的这些参数，当你手动控制了任意炮台，它就是当前的主炮台，你下达的指令都只会传达给这个炮台。
	而自动分配给这个主炮台的其他未被玩家控制的从炮台，都会根据主炮台的参数来决定自己的行为。
	例如，当你控制了A炮台，A炮台开启了从炮台跟随自己瞄准，这时从炮台就会按照A炮台的瞄准聚焦点距离参数来跟随A炮台进行瞄准。
	你只能通过一种方式来改变主炮台的这些参数，就是当你手动控制主炮台时，按下对应的控制键。
	控制键只有W A S D C Space 这几个，你可以在下方 【集群控制按键指令】 中绑定每个按键对应的指令，如果不绑定，按下按键后什么也不会发生。
	可以绑定的指令如下：

	AllFire:全部开火
	Fire:自己开火
	Aim:开启从炮台跟随自己瞄准
	Free:关闭从炮台跟随自己瞄准
	Far:增加瞄准聚焦点距离
	Near:减少聚焦点距离
	Scan:开启扫描，向自己前方设定扫描距离发射摄像头扫描激光，扫描到目标自动追踪目标。扫描到目标后炮台并不会视角锁定或

	例如：当你设置了 TotalControl_CMD_C = "Scan" 
	表示当你控制任意炮台的远程控制块，并按下C键时，该炮台接收Scan指令，并开启扫描开关，此时该炮台会利用自己的摄像头向前发射激光。
	一旦激光碰到任何目标，就会将该目标存为自己的锁定目标。
	其他任何处于Attention模式的炮台，都会马上追踪这个目标并进行自主锁定和攻击。你可以在离线模式按F11打开右上角的激活调试模组功能查看摄像头的激光。

	例如：当你设置了 TotalControl_CMD_Space = "AllFire"
	表示当你手动控制了任意炮台的远程控制块，并按下空格键时，该炮台接收全部开火指令。
	此时该炮台自己和它的所有从炮台都会开火。这个开火会响应探测器的保护。
*/

		// ---- 获取方块设置----
		static string[] AutoWeaponsNameTag = { "ALL", "ALL" }; //自动武器编组名、方块名。设为""时不获取，设为"ALL"时获取所有。获取结果是编组获取与名字获取的并集。
		const string AimBlockKey = "FCS#R"; //炮台远程控制块（或主控座椅、驾驶舱）关键字，只要包含这个关键字即可。
		const string RotorNagtiveTag = "[-]"; //转子负转标签。当转子名字里完全包含这个标签的时候，它会被强制认为是反向控制的转子。用来解决某些特殊结构的转子问题
		static string LCDNameTag = "FCSR_LCD"; //FCSR主要LCD面板名字

		static bool DebugMode = false; //开关调试模式，开启后会在编程块右下角显示获取炮台编组的具体情况

		// ---- 战斗设置 ----
		const string FireActionBlockNameTag = "[FIRE]"; //自动开火时同步触发的方块名，不填写留空
		const string FireActionName = "TriggerNow"; //自动开火时同步触发的方块的动作指令名，定时块的立即出发指令是TriggerNow

		// ---- 集群控制按键指令 ----
		static string TotalControl_CMD_W = ""; //W按键指令
		static string TotalControl_CMD_S = ""; //S按键指令
		static string TotalControl_CMD_A = ""; //A按键指令
		static string TotalControl_CMD_D = ""; //D按键指令
		static string TotalControl_CMD_C = "Free"; //C按键指令
		static string TotalControl_CMD_Space = "Aim"; //空格键指令

		bool init;
		List<Ship.Target> AT_Targets = new List<Ship.Target>();
		List<Ship.Target> FCSR_Targets = new List<Ship.Target>();
		void Main(string arguments)
		{
			if (!init)
			{
				GetBlocks();
				return;
			}

			//解析指令
			if (arguments.StartsWith("Stop") || arguments.StartsWith("Attention") || arguments.StartsWith("Active"))
			{
				if (arguments.Split(':').Length >= 2)
				{
					string cmd_R = arguments.Split(':')[1];
					foreach (var R in FCSR)
					{
						if (R.Name == cmd_R)
						{
							R.WorkMode = arguments.Split(':')[0];
						}
					}
				}
				else if (arguments == "Stop" || arguments == "Attention" || arguments == "Active")
				{
					foreach (var R in FCSR)
					{
						R.WorkMode = arguments.Split(':')[0];
					}
				}
			}

			//获取自动炮台的目标
			AT_Targets = new List<Ship.Target>();
			foreach (IMyLargeTurretBase w in AutoWeapons)
			{
				MyDetectedEntityInfo t = w.GetTargetedEntity();
				if (!t.IsEmpty())
				{
					AT_Targets.Add(new Ship.Target(t));
				}
			}

			//获取转子炮台的目标
			FCSR_Targets = new List<Ship.Target>();
			foreach (Ship r in FCSR)
			{
				Ship.Target t = r.MyTarget;
				if (t != null && !t.IsEmpty())
				{
					FCSR_Targets.Add(t);
				}
			}

			//更新时间，动态显示系统运行信息
			Ship.timetick++;
			Echo(Ship.timetick % 60 <= 5 ? "" : Ship.timetick % 60 <= 10 ? "-" : Ship.timetick % 60 <= 15 ? "- ME" : Ship.timetick % 60 <= 20 ? "- MEA" : Ship.timetick % 60 <= 25 ? "- MEA FC" : Ship.timetick % 60 <= 30 ? "- MEA FCS" : Ship.timetick % 60 <= 35 ? "- MEA FCS-" : "- MEA FCS-R -");
			Echo("RotorBase Count: " + FCSR.Count);
			Echo("Auto Weapons Count: " + AutoWeapons.Count.ToString());
			Echo("FCS-R Running " + Ship.timetick);

			//执行炮台常规控制
			foreach (Ship R in FCSR)
			{
				R.GetSetFromCustomData();//从主控的自定义参数中读写配置参数
				R.UpdatePhysical(); //更新物理运动信息
				R.Attention(R.AttentionMode); //待命瞄准控制
				if (R.CheckPlayerControl()) { continue; } //检测玩家控制
				switch (R.WorkMode)
				{
					case "Stop":
						R.Attention(R.AttentionMode); //待命瞄准控制
						break;
					case "Attention":
						R.AttackCloestTarget(AT_Targets);
						break;
					case "Active":
						List<Ship.Target> allTargets = new List<Ship.Target>();
						allTargets.AddList(AT_Targets);
						allTargets.AddList(FCSR_Targets);
						R.AttackCloestTarget(allTargets);
						if (R.MyTarget != null && !R.MyTarget.IsEmpty())
						{
							R.MyTarget = R.TrackTarget(R.MyTarget);
						}
						break;
					default:
						R.Attention(R.AttentionMode);
						break;
				}

				//清空过期目标
				if (R.MyTarget == null || Ship.timetick - R.MyTarget.TimeStamp >= 90)
				{
					R.MyTarget = new Ship.Target();
				}
			}

			//集群控制
			List<Ship> Controled_R = FCSR.Where(b => b.Cockpit.IsUnderControl).ToList();
			List<Ship> Free_R = FCSR.Where(b => !b.Cockpit.IsUnderControl).ToList();

			//以主炮台角度执行集群按键控制
			foreach (var R in Controled_R)
			{
				List<string> cmds = new List<string>();
				if (R.Cockpit.MoveIndicator.X < 0) { cmds.Add(TotalControl_CMD_A); }
				if (R.Cockpit.MoveIndicator.X > 0) { cmds.Add(TotalControl_CMD_D); }
				if (R.Cockpit.MoveIndicator.Y < 0) { cmds.Add(TotalControl_CMD_C); }
				if (R.Cockpit.MoveIndicator.Y > 0) { cmds.Add(TotalControl_CMD_Space); }
				if (R.Cockpit.MoveIndicator.Z < 0) { cmds.Add(TotalControl_CMD_W); }
				if (R.Cockpit.MoveIndicator.Z > 0) { cmds.Add(TotalControl_CMD_S); }
				foreach (string cmd in cmds)
				{
					if (cmd == "Fire")
					{
						R.FCSR_Fire();
					}
					if (cmd == "Scan")
					{
						R.needScan = true;
						R.MyTarget = null; //重置该炮台的目标是为了在该炮台有目标时也能执行手控搜索
					}
					if (cmd == "AllFire")
					{
						R.FCSR_Fire();
					}
					if (cmd == "Aim")
					{
						R.needAimFollowMe = true;
					}
					if (cmd == "Free")
					{
						R.needAimFollowMe = false;
					}
					if (cmd == "Far")
					{
						R.TotalControlDistance += 1;
					}
					if (cmd == "Near")
					{
						R.TotalControlDistance -= 1;
					}
				}

				if (R.needScan)
				{
					if (R.MyTarget == null || R.MyTarget.IsEmpty())
					{
						Ship.Target t = R.ScanPoint(R.Position + R.Cockpit.WorldMatrix.Forward * R.ScanDistance);
						if (!t.IsEmpty())
						{
							R.MyTarget = t;
						}
					}
					else
					{
						R.MyTarget = R.TrackTarget(R.MyTarget);
					}
				}
			}

			//以从炮台角度执行集群按键控制
			foreach (var R in Free_R)
			{
				Ship Cloest_R = null;
				double c_f_distance = double.MaxValue;
				foreach (var CR in Controled_R)
				{
					if (Vector3D.Distance(R.Position, CR.Position) <= c_f_distance)
					{
						Cloest_R = CR;
						c_f_distance = Vector3D.Distance(R.Position, CR.Position);
					}
				}
				if (Cloest_R != null)
				{
					List<string> cmds = new List<string>();
					if (Cloest_R.Cockpit.MoveIndicator.X < 0) { cmds.Add(TotalControl_CMD_A); }
					if (Cloest_R.Cockpit.MoveIndicator.X > 0) { cmds.Add(TotalControl_CMD_D); }
					if (Cloest_R.Cockpit.MoveIndicator.Y < 0) { cmds.Add(TotalControl_CMD_C); }
					if (Cloest_R.Cockpit.MoveIndicator.Y > 0) { cmds.Add(TotalControl_CMD_Space); }
					if (Cloest_R.Cockpit.MoveIndicator.Z < 0) { cmds.Add(TotalControl_CMD_W); }
					if (Cloest_R.Cockpit.MoveIndicator.Z > 0) { cmds.Add(TotalControl_CMD_S); }
					foreach (string cmd in cmds)
					{
						if (cmd == "AllFire")
						{
							R.FCSR_Fire();
						}
					}

					if (Cloest_R.needAimFollowMe)
					{
						R.AimAtPositionByRotor(Cloest_R.Position + Cloest_R.Cockpit.WorldMatrix.Forward * Cloest_R.TotalControlDistance);
					}
				}
			}

			ShowMainLCD();

			Echo("InstructionCount: " + Runtime.CurrentInstructionCount.ToString() + " / " + Runtime.MaxInstructionCount.ToString());
		}

		// ========== 显示主要LCD内容 =========
		public void ShowMainLCD()
		{
			List<IMyTextPanel> Lcds = new List<IMyTextPanel>();
			GridTerminalSystem.GetBlocksOfType(Lcds, b => b.CustomName == LCDNameTag);
			string info = "";
			string br = "\n";
			info += " =========== [ MEA ] FCS-R ========== " + br;
			info += "  炮台总数 : " + FCSR.Count + "          " + "自动武器 ：" + AutoWeapons.Count + br;
			info += "  " + (Ship.timetick % 60 <= 5 ? "" : Ship.timetick % 60 <= 10 ? "-" : Ship.timetick % 60 <= 15 ? "- ME" : Ship.timetick % 60 <= 20 ? "- MEA" : Ship.timetick % 60 <= 25 ? "- MEA FC" : Ship.timetick % 60 <= 30 ? "- MEA FCS" : Ship.timetick % 60 <= 35 ? "- MEA FCS-" : "- MEA FCS-R -");
			info += br + " --------------------- 炮台组 ----------------------- " + br;
			info += "  组名      指令       完整度" + br;
			foreach (Ship R in FCSR)
			{
				List<IMyTerminalBlock> functionalblocks = R.AllBlocks.Where(b => b.IsFunctional).ToList();
				double FunctionalPersent = 100 * functionalblocks.Count / R.AllBlocks.Count;
				info += "  " + R.Name + "     " + R.WorkMode + "       " + FunctionalPersent + "%" + br;
			}
			info += br + " ======= 自动武器目标列表 ======= " + br;
			info += "  名字         距离         半径" + br;
			for (int i = 0; i < AT_Targets.Count; i++)
			{
				if (AT_Targets[i].EntityId != 0)
				{
					info += "  " + AT_Targets[i].Name + "        " + Math.Round(Vector3D.Distance(AT_Targets[i].Position, Me.GetPosition()), 0) + "      " + Math.Round(AT_Targets[i].Diameter, 0) + br;
				}
			}
			info += br + " ======= 转子炮塔目标列表 ======= " + br;
			info += "  名字         距离         半径" + br;
			for (int i = 0; i < FCSR_Targets.Count; i++)
			{
				if (FCSR_Targets[i].EntityId != 0)
				{
					info += "  " + FCSR_Targets[i].Name + "        " + Math.Round(Vector3D.Distance(FCSR_Targets[i].Position, Me.GetPosition()), 0) + "      " + Math.Round(FCSR_Targets[i].Diameter, 0) + br;
				}
			}
			foreach (IMyTextPanel lcd in Lcds)
			{
				lcd.ShowPublicTextOnScreen();
				lcd.WritePublicText(info);
			}
		}

		//初始化方块方法
		List<Ship> FCSR = new List<Ship>();
		List<IMyLargeTurretBase> AutoWeapons = new List<IMyLargeTurretBase>();
		void GetBlocks()
		{
			//获取所有自动武器
			GridTerminalSystem.GetBlocksOfType(AutoWeapons, b => AutoWeaponsNameTag[1] == "" ? false : (AutoWeaponsNameTag[1] == "ALL" ? true : b.CustomName == AutoWeaponsNameTag[1]));

			//获取所有编组
			List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
			GridTerminalSystem.GetBlockGroups(groups);

			//初始化炮台组
			FCSR = new List<Ship>();

			foreach (var g in groups)
			{
				List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
				g.GetBlocks(blocks);
				FCSR.Add(new Ship(blocks, g.Name));
				//顺便获取自动武器
				if (g.Name == AutoWeaponsNameTag[0])
				{
					foreach (IMyTerminalBlock b in blocks)
					{
						if (b is IMyLargeTurretBase)
						{
							AutoWeapons.Add(b as IMyLargeTurretBase);
						}
					}
				}
			}

			if (DebugMode)
			{
				foreach (var r in FCSR)
				{
					Echo(r.Name + " : " + r.Debug);
				}
			}
			else
			{
				FCSR = FCSR.Where(b => b.Debug == "Normal").ToList();
				init = true;
			}
		}

		// ==== 调起保存后自动运行 ====
		Program()
		{
			Runtime.UpdateFrequency = UpdateFrequency.Update1;
		}

		// ===== Ship v1.0 for FCS-R定制版 =====
		public class Ship
		{
			// ----- 转子基座炮台配置参数 -----
			public string WorkMode = "Attention"; //工作模式
			public double PlayerAimRatio = 0.6; //玩家手动操控灵敏度
			public int AttentionMode = 1; //待命时瞄准模式
			public bool isFireWhenAimOK = true; //是否精确开火（等待瞄准完成后才开火）
			public double AimRatio = 1; //瞄准精度，单位：度。用来是否瞄准，以便其他动作判断。不影响瞄准的效率。当瞄准块的正前方向量与瞄准块和目标的连线向量夹角小于这个值时，整个系统判定瞄准了目标。
			public double TotalControlDistance = 1000; //默认当该炮台作为主炮台时，集群控制瞄准聚焦点距离
			public double ScanDistance = 1000; //默认摄像头扫描距离

			public bool needScan = false; //开关扫描状态
			public bool needAimFollowMe = true; //开关是否让从炮台跟随自己瞄准
			public Ship.Target MyTarget = null; //自己攻击锁定的目标

			// ----- 武器子弹参数设置 -----
			public double BulletInitialSpeed = 540; //子弹初速度
			public double BulletAcceleration = 0; //子弹加速度
			public double BulletMaxSpeed = 540; //子弹最大速度
			public double ShootDistance = 970; //开火距离

			// ----- 静态属性 -----
			public static int timetick; //帧计时器。注意：这是一个静态变量，使用Ship.timetick访问它，它是这个类共有的变量而不是某个实例化后的飞船特有的。

			// ----- 方块类变量 -----
			public List<IMyTerminalBlock> AllBlocks = new List<IMyTerminalBlock>();
			public IMyShipController Cockpit;
			public IMyShipConnector Connector;
			public IMyShipConnector MotherConnector;
			public List<IMyLargeTurretBase> AutoWeapons = new List<IMyLargeTurretBase>();
			public List<IMySmallGatlingGun> GatlingGuns = new List<IMySmallGatlingGun>();
			public List<IMySmallMissileLauncher> RocketLaunchers = new List<IMySmallMissileLauncher>();
			public List<IMyCameraBlock> Cameras = new List<IMyCameraBlock>();
			public List<IMySensorBlock> Sensors = new List<IMySensorBlock>();
			public List<IMyTerminalBlock> FireActionBlocks = new List<IMyTerminalBlock>();
			public List<IMyGyro> Gyroscopes = new List<IMyGyro>();
			public List<IMyThrust> Thrusts = new List<IMyThrust>();
			public List<IMyMotorStator> Rotors = new List<IMyMotorStator>();
			public List<string> RotorsField = new List<string>();
			public List<float> RotorsFactor = new List<float>();
			public List<string> ThrustField = new List<string>();
			public List<string> gyroYawField = new List<string>();
			public List<string> gyroPitchField = new List<string>();
			public List<string> gyroRollField = new List<string>();
			public List<float> gyroYawFactor = new List<float>();
			public List<float> gyroPitchFactor = new List<float>();
			public List<float> gyroRollFactor = new List<float>();

			// ---- 必要配置变量 ----
			public string Name = "";
			public string Debug = "Normal"; //错误报告，通过这个变量判断是否初始化成功

			// ----- 运动信息和相关变量 -----
			public Vector3D Position;
			public Vector3D Velocity;
			public Vector3D Acceleration;
			public double Diameter;

			// ---- 瞄准PID算法参数 ------
			public int AimPID_T = 5; //PID 采样周期（单位：帧），周期越小效果越好，但太小的周期会让积分系数难以发挥效果
			public double AimPID_P = 0.8; //0.8 比例系数：可以理解为整个PID控制的总力度，建议范围0到1.2，1是完全出力。
			public double AimPID_I = 0.2; //3 积分系数：增加这个系数会让静态误差增加（即高速环绕误差），但会减少瞄准的震荡。反之同理
			public double AimPID_D = 1; //10 微分系数：增加这个系数会减少瞄准的震荡幅度，但会加剧在小角度偏差时的震荡幅度。反之同理

			// ----- 构造方法 ------
			// 传入方块后自动处理方块并实例化
			public Ship()
			{
				this.Debug = "Empty Init Ship";
			}
			public Ship(List<IMyTerminalBlock> Blocks, string _Name = "")
			{
				this.Name = _Name;
				this.AllBlocks = Blocks;
				//这里面可以写入更详细的判断方块是否需要获取的条件，例如名字匹配
				foreach (IMyTerminalBlock b in Blocks)
				{
					if (b.CustomName.Contains(AimBlockKey) && b is IMyShipController)
					{
						this.Cockpit = b as IMyShipController;
						b.CustomData = "";
					}
					if (b is IMyCameraBlock)
					{
						this.Cameras.Add(b as IMyCameraBlock);
					}
					if (b is IMySensorBlock)
					{
						this.Sensors.Add(b as IMySensorBlock);
					}
					if (b is IMyLargeTurretBase)
					{
						this.AutoWeapons.Add(b as IMyLargeTurretBase);
					}
					if (b is IMySmallGatlingGun)
					{
						this.GatlingGuns.Add(b as IMySmallGatlingGun);
					}
					if (b is IMySmallMissileLauncher)
					{
						this.RocketLaunchers.Add(b as IMySmallMissileLauncher);
					}
					if (b is IMyGyro)
					{
						this.Gyroscopes.Add(b as IMyGyro);
					}
					if (b is IMyThrust)
					{
						this.Thrusts.Add(b as IMyThrust);
					}
					if (b is IMyShipConnector && (b as IMyShipConnector).OtherConnector != null)
					{
						this.Connector = b as IMyShipConnector;
						this.MotherConnector = Connector.OtherConnector;
					}
					if (b is IMyMotorStator)
					{
						this.Rotors.Add(b as IMyMotorStator);
					}
					if (b.CustomName == FireActionBlockNameTag)
					{
						this.FireActionBlocks.Add(b);
					}
				}

				if (Cockpit == null) { Debug = "AimBlock Not Found \n AimBlock Must Be Cockpit/Remote"; return; }
				Cameras.ForEach(delegate (IMyCameraBlock cam) { cam.ApplyAction("OnOff_On"); cam.EnableRaycast = true; });

				//处理推进器
				for (int i = 0; i < Thrusts.Count; i++)
				{
					Base6Directions.Direction CockpitForward = Thrusts[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Forward);
					Base6Directions.Direction CockpitUp = Thrusts[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Up);
					Base6Directions.Direction CockpitLeft = Thrusts[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Left);
					switch (CockpitForward)
					{ case Base6Directions.Direction.Forward: ThrustField.Add("Forward"); break; case Base6Directions.Direction.Backward: ThrustField.Add("Backward"); break; }
					switch (CockpitUp)
					{ case Base6Directions.Direction.Forward: ThrustField.Add("Up"); break; case Base6Directions.Direction.Backward: ThrustField.Add("Down"); break; }
					switch (CockpitLeft)
					{ case Base6Directions.Direction.Forward: ThrustField.Add("Left"); break; case Base6Directions.Direction.Backward: ThrustField.Add("Right"); break; }

					Thrusts[i].ApplyAction("OnOff_On");
				}

				//处理陀螺仪
				for (int i = 0; i < Gyroscopes.Count; i++)
				{
					Base6Directions.Direction gyroUp = Gyroscopes[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Up);
					Base6Directions.Direction gyroLeft = Gyroscopes[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Left);
					Base6Directions.Direction gyroForward = Gyroscopes[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Forward);
					switch (gyroUp)
					{
						case Base6Directions.Direction.Up: gyroYawField.Add("Yaw"); gyroYawFactor.Add(1f); break;
						case Base6Directions.Direction.Down: gyroYawField.Add("Yaw"); gyroYawFactor.Add(-1f); break;
						case Base6Directions.Direction.Left: gyroYawField.Add("Pitch"); gyroYawFactor.Add(1f); break;
						case Base6Directions.Direction.Right: gyroYawField.Add("Pitch"); gyroYawFactor.Add(-1f); break;
						case Base6Directions.Direction.Forward: gyroYawField.Add("Roll"); gyroYawFactor.Add(-1f); break;
						case Base6Directions.Direction.Backward: gyroYawField.Add("Roll"); gyroYawFactor.Add(1f); break;
					}
					switch (gyroLeft)
					{
						case Base6Directions.Direction.Up: gyroPitchField.Add("Yaw"); gyroPitchFactor.Add(1f); break;
						case Base6Directions.Direction.Down: gyroPitchField.Add("Yaw"); gyroPitchFactor.Add(-1f); break;
						case Base6Directions.Direction.Left: gyroPitchField.Add("Pitch"); gyroPitchFactor.Add(1f); break;
						case Base6Directions.Direction.Right: gyroPitchField.Add("Pitch"); gyroPitchFactor.Add(-1f); break;
						case Base6Directions.Direction.Forward: gyroPitchField.Add("Roll"); gyroPitchFactor.Add(-1f); break;
						case Base6Directions.Direction.Backward: gyroPitchField.Add("Roll"); gyroPitchFactor.Add(1f); break;
					}

					switch (gyroForward)
					{
						case Base6Directions.Direction.Up: gyroRollField.Add("Yaw"); gyroRollFactor.Add(1f); break;
						case Base6Directions.Direction.Down: gyroRollField.Add("Yaw"); gyroRollFactor.Add(-1f); break;
						case Base6Directions.Direction.Left: gyroRollField.Add("Pitch"); gyroRollFactor.Add(1f); break;
						case Base6Directions.Direction.Right: gyroRollField.Add("Pitch"); gyroRollFactor.Add(-1f); break;
						case Base6Directions.Direction.Forward: gyroRollField.Add("Roll"); gyroRollFactor.Add(-1f); break;
						case Base6Directions.Direction.Backward: gyroRollField.Add("Roll"); gyroRollFactor.Add(1f); break;
					}

					Gyroscopes[i].ApplyAction("OnOff_On");
				}

				//处理转子，注意转子头是Up而不是Forward
				for (int i = 0; i < Rotors.Count; i++)
				{
					Base6Directions.Direction CockpitForward = Rotors[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Forward);
					Base6Directions.Direction CockpitUp = Rotors[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Up);
					Base6Directions.Direction CockpitLeft = Rotors[i].WorldMatrix.GetClosestDirection(Cockpit.WorldMatrix.Left);
					switch (CockpitForward)
					{ case Base6Directions.Direction.Up: RotorsField.Add("Forward"); break; case Base6Directions.Direction.Down: RotorsField.Add("Backward"); break; }
					switch (CockpitUp)
					{ case Base6Directions.Direction.Up: RotorsField.Add("Up"); break; case Base6Directions.Direction.Down: RotorsField.Add("Down"); break; }
					switch (CockpitLeft)
					{ case Base6Directions.Direction.Up: RotorsField.Add("Left"); break; case Base6Directions.Direction.Down: RotorsField.Add("Right"); break; }
					RotorsFactor.Add(Rotors[i].CustomName.Contains(RotorNagtiveTag) ? -1f : 1f);
					Rotors[i].ApplyAction("OnOff_On");
				}
			}

			/* ===== 转子基座炮台相关 ===== */
			// ----- 检测玩家操控 -----
			public bool CheckPlayerControl()
			{
				if (this.Cockpit.IsUnderControl)
				{
					Vector2 MouseInput = (this.Cockpit as IMyShipController).RotationIndicator;
					this.SetRotorsValue("Up", MouseInput.Y * this.PlayerAimRatio);
					this.SetRotorsValue("Down", -MouseInput.Y * this.PlayerAimRatio);
					this.SetRotorsValue("Right", -MouseInput.X * this.PlayerAimRatio);
					this.SetRotorsValue("Left", MouseInput.X * this.PlayerAimRatio);
					return true;
				}
				return false;
			}

			// ----- 待命归位 -----
			// 0 停止转子
			// 1 瞄准向上的第一个X转子前方500米
			public void Attention(int mode = 0)
			{
				switch (mode)
				{
					case 0:
						this.MotionInit();
						break;
					case 1:
						IMyMotorStator rtx = null;
						for (int i = 0; i < this.Rotors.Count; i++)
						{
							if (this.RotorsField[i] == "Up")
							{
								rtx = this.Rotors[i]; break;
							}
						}
						if (rtx != null)
						{
							this.AimAtPositionByRotor(rtx.GetPosition() + rtx.WorldMatrix.Forward * 500);
						}
						break;
				}
			}

			// ----- 使用转子瞄准坐标点 -----
			// 使用PID算法，控制转子瞄准某个坐标点
			private List<Vector3D> Aim_PID_Data_Rotor = new List<Vector3D>();
			public bool AimAtPositionByRotor(Vector3D Position)
			{
				MatrixD refLookAtMatrix = MatrixD.CreateLookAt(new Vector3D(), this.Cockpit.WorldMatrix.Forward, this.Cockpit.WorldMatrix.Up);
				Vector3D TargetPositionToMe = Vector3D.Normalize(Vector3D.TransformNormal(Position - this.Position, refLookAtMatrix));

				//储存采样点
				int need_data_diff = AimPID_T - Aim_PID_Data_Rotor.Count;
				for (int i = 0; i < need_data_diff; i++)
				{
					Aim_PID_Data_Rotor.Add(new Vector3D());
				}

				Aim_PID_Data_Rotor.Remove(Aim_PID_Data_Rotor[0]); Aim_PID_Data_Rotor.Add(TargetPositionToMe);

				//获得采样点积分
				double X_I = 0;
				double Y_I = 0;
				foreach (Vector3D datapoint in Aim_PID_Data_Rotor)
				{
					X_I += datapoint.X;
					Y_I += datapoint.Y;
				}

				//计算输出
				double YawValue = (AimPID_P * TargetPositionToMe.X) + (X_I * AimPID_I) + AimPID_D * (Aim_PID_Data_Rotor[AimPID_T - 1].X - X_I) / AimPID_T;
				YawValue *= 60;
				double PitchValue = (AimPID_P * TargetPositionToMe.Y) + (Y_I * AimPID_I) + AimPID_D * (Aim_PID_Data_Rotor[AimPID_T - 1].Y - Y_I) / AimPID_T;
				PitchValue *= 60;

				this.SetRotorsValue("Up", YawValue);
				this.SetRotorsValue("Down", -YawValue);
				if (TargetPositionToMe.Z < 0)
				{
					this.SetRotorsValue("Left", -PitchValue);
					this.SetRotorsValue("Right", PitchValue);
				}

				// 计算当前与预期瞄准点的瞄准夹角
				Vector3D V_A = Position - this.Position;
				Vector3D V_B = this.Cockpit.WorldMatrix.Forward;
				double Angle = Math.Acos(Vector3D.Dot(V_A, V_B) / (V_A.Length() * V_B.Length())) * 180 / Math.PI;
				if (Angle <= AimRatio) return true;
				return false;
			}

			// ----- 使用转子瞄准某个目标 -----
			public bool AimAtTargetByRotor(Ship.Target Target)
			{
				Vector3D HitPoint = Ship.HitPointCaculate(this.Position, this.Velocity, this.Acceleration, Target.Position, Target.Velocity, Target.Acceleration, this.BulletInitialSpeed, this.BulletAcceleration, this.BulletMaxSpeed);
				return this.AimAtPositionByRotor(HitPoint);
			}

			// ----- 能否瞄准某个坐标 -----
			// 如果该坐标在第一个向上的转子的上方，返回true
			public bool CanAimPosition(Vector3D Position)
			{
				IMyMotorStator r = null;
				for (int i = 0; i < this.Rotors.Count; i++)
				{
					if (this.RotorsField[i] == "Up")
					{
						r = this.Rotors[i];
						break;
					}
				}
				MatrixD refLookAtMatrix = MatrixD.CreateLookAt(new Vector3D(), r.WorldMatrix.Forward, r.WorldMatrix.Up);
				Vector3D TargetPositionToMyRotor = Vector3D.TransformNormal(Position - r.GetPosition(), refLookAtMatrix);
				return TargetPositionToMyRotor.Y > 0;
			}

			// ----- 开火 -----
			// 根据探测器检测后决定是否武器开火
			public void FCSR_Fire()
			{
				bool canFire = true;
				this.Sensors.ForEach(delegate (IMySensorBlock s) {
					if (s.IsActive) { canFire = false; }
				});
				if (canFire)
				{
					this.WeaponsShoot();
				}
			}

			// ------- 攻击并搜索最近的目标 -------
			public void AttackCloestTarget(List<Ship.Target> targetList)
			{
				double currentDis = double.MaxValue;
				Ship.Target MyAttackTarget = null;
				for (int i = 0; i < targetList.Count; i++)
				{
					double distance = Vector3D.Distance(targetList[i].Position, this.Position);
					if (targetList[i].EntityId != 0 && distance <= currentDis && this.CanAimPosition(targetList[i].Position))
					{
						MyAttackTarget = targetList[i];
						currentDis = distance;
					}
				}

				if (MyAttackTarget != null)
				{
					bool aim_ok = this.AimAtTargetByRotor(MyAttackTarget);
					if (currentDis <= this.ShootDistance)
					{
						if (this.isFireWhenAimOK)
						{
							if (aim_ok) { this.FCSR_Fire(); }
						}
						else
						{
							this.FCSR_Fire();
						}
					}
					this.MyTarget = MyAttackTarget;
				}
				else
				{
					this.MyTarget = null;
				}
			}

			// ------ 从瞄准块的CustomData中获取设置参数 ------
			public void GetSetFromCustomData()
			{
				string br = "\n";
				string info = "";
				info += "[FCSR_Info]" + br;
				info += "isFireWhenAimOK=" + this.isFireWhenAimOK + br;
				info += "PlayerAimRatio=" + this.PlayerAimRatio + br;
				info += "AttentionMode=" + this.AttentionMode + br;
				info += "BulletInitialSpeed=" + this.BulletInitialSpeed + br;
				info += "BulletAcceleration=" + this.BulletAcceleration + br;
				info += "BulletMaxSpeed=" + this.BulletMaxSpeed + br;
				info += "ShootDistance=" + this.ShootDistance + br;
				info += "AimRatio=" + this.AimRatio + br;
				info += "ScanDistance=" + this.ScanDistance + br;
				info += "[/FCSR_Info]" + br;

				double double_temp = 0;
				int int_temp = 0;
				bool bool_temp = false;
				if (bool.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "isFireWhenAimOK"), out bool_temp) && this.isFireWhenAimOK != bool_temp)
				{
					this.isFireWhenAimOK = bool_temp; return;
				}
				if (int.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "AttentionMode"), out int_temp) && this.AttentionMode != int_temp)
				{
					this.AttentionMode = int_temp; return;
				}
				if (double.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "PlayerAimRatio"), out double_temp) && this.PlayerAimRatio != double_temp)
				{
					this.PlayerAimRatio = double_temp; return;
				}
				if (double.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "BulletInitialSpeed"), out double_temp) && this.BulletInitialSpeed != double_temp)
				{
					this.BulletInitialSpeed = double_temp; return;
				}
				if (double.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "BulletAcceleration"), out double_temp) && this.BulletAcceleration != double_temp)
				{
					this.BulletAcceleration = double_temp; return;
				}
				if (double.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "BulletMaxSpeed"), out double_temp) && this.BulletMaxSpeed != double_temp)
				{
					this.BulletMaxSpeed = double_temp; return;
				}
				if (double.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "ShootDistance"), out double_temp) && this.ShootDistance != double_temp)
				{
					this.ShootDistance = double_temp; return;
				}
				if (double.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "AimRatio"), out double_temp) && this.AimRatio != double_temp)
				{
					this.AimRatio = double_temp; return;
				}
				if (double.TryParse(Ship.GetOneInfo(this.Cockpit.CustomData, "FCSR_Info", "ScanDistance"), out double_temp) && this.ScanDistance != double_temp)
				{
					this.ScanDistance = double_temp; return;
				}

				this.Cockpit.CustomData = info;
			}

			/* ===== 摄像头扫描相关 ===== */

			// ----- 摄像头搜索某个坐标点 -----
			// 持续搜索座标
			private int SP_Now_i;
			public Target ScanPoint(Vector3D Point)
			{
				MyDetectedEntityInfo FoundTarget = new MyDetectedEntityInfo();

				List<IMyCameraBlock> RightAngleCameras = Ship.GetCanScanCameras(this.Cameras, Point);
				if (SP_Now_i >= RightAngleCameras.Count) { SP_Now_i = 0; }
				double ScanSpeed = (RightAngleCameras.Count * 2000) / (Vector3D.Distance(Point, this.Position) * 60);//每个循环可用于扫描的摄像头个数   

				if (ScanSpeed >= 1)//每循环可扫描多个   
				{
					FoundTarget = RightAngleCameras[SP_Now_i].Raycast(Point);
					SP_Now_i += 1;
					if (SP_Now_i >= RightAngleCameras.Count) { SP_Now_i = 0; }
				}
				else
				{
					if (Ship.timetick % Math.Round(1 / ScanSpeed, 0) == 0)
					{
						FoundTarget = RightAngleCameras[SP_Now_i].Raycast(Point);
						SP_Now_i += 1;
					}
				}
				return new Target(FoundTarget);
			}

			// ----- 摄像头对多个坐标点执行脉冲扫描 -----
			// 返回第一个扫描到的目标后跳出
			// 可选传入一个EntityId作为匹配项
			public Target PulseScanSingle(List<Vector3D> Points, long EntityId = 0)
			{
				MyDetectedEntityInfo FoundTarget = new MyDetectedEntityInfo();

				int x = 0;//这样做是为了减少不必要的运算量
				foreach (Vector3D p in Points)
				{
					for (int i = x; i < this.Cameras.Count; i++)
					{
						if (this.Cameras[i].CanScan(p))
						{
							FoundTarget = this.Cameras[i].Raycast(p);
							if (!FoundTarget.IsEmpty())
							{
								if (EntityId == 0)
								{
									return new Target(FoundTarget);
								}
								else if (FoundTarget.EntityId == EntityId)
								{
									return new Target(FoundTarget);
								}
							}
							x = i;
							break;
						}
					}
				}

				return new Target(FoundTarget);
			}

			// ----- 摄像头对多个坐标进行完全脉冲扫描 -----
			// 将瞬间执行完整的扫描并返回所有扫描到的目标 （运算量较大）
			public List<Target> PulseScanMultiple(List<Vector3D> Points)
			{
				List<Target> Res = new List<Target>();
				List<MyDetectedEntityInfo> FoundTargets = new List<MyDetectedEntityInfo>();

				int x = 0;//这样做是为了减少不必要的运算量
				foreach (Vector3D p in Points)
				{
					for (int i = x; i < this.Cameras.Count; i++)
					{
						if (this.Cameras[i].CanScan(p))
						{
							MyDetectedEntityInfo FoundTarget = this.Cameras[i].Raycast(p);
							if (!FoundTarget.IsEmpty())
							{
								FoundTargets.Add(FoundTarget);
							}
							x = i;
							break;
						}
					}
				}
				foreach (MyDetectedEntityInfo t in FoundTargets)
				{
					Res.Add(new Target(t));
				}

				return Res;
			}

			// ----- 追踪给定目标 -----
			// 可传入Target类或MyDetectedEntityInfo类
			private int TT_Now_i;
			public Vector3D TrackDeviationToTarget; //基于目标坐标系的偏移量，用来修正中心锁定的问题
			public Target TrackTarget(Target Tgt)
			{

				MyDetectedEntityInfo FoundTarget = new MyDetectedEntityInfo();

				Vector3D posmove = Vector3D.TransformNormal(this.TrackDeviationToTarget, Tgt.Orientation);

				Vector3D lidarHitPoint = Tgt.Position + posmove + (Ship.timetick - Tgt.TimeStamp) * Tgt.Velocity / 60; //这个碰撞点算法是最正确的

				List<IMyCameraBlock> RightAngleCameras = Ship.GetCanScanCameras(this.Cameras, lidarHitPoint);//获取方向正确的摄像头数量
				if (TT_Now_i >= RightAngleCameras.Count) { TT_Now_i = 0; }

				//执行常规追踪
				double ScanSpeed = (RightAngleCameras.Count * 2000) / ((Vector3D.Distance(lidarHitPoint, this.Position)) * 60);//每个循环可用于扫描的摄像头个数		
				if (ScanSpeed >= 1)
				{
					for (int i = 1; i < ScanSpeed; i++)
					{
						FoundTarget = RightAngleCameras[TT_Now_i].Raycast(lidarHitPoint);
						TT_Now_i += 1;
						if (TT_Now_i >= RightAngleCameras.Count) { TT_Now_i = 0; }
						if (!FoundTarget.IsEmpty() && FoundTarget.EntityId == Tgt.EntityId)
						{
							return new Target(FoundTarget);
						}
					}
				}
				else
				{
					//这里向上取整实际上是采用了更低一点的频率在扫描，有利于恢复储能
					if (Ship.timetick % Math.Ceiling(1 / ScanSpeed) == 0)
					{
						FoundTarget = RightAngleCameras[TT_Now_i].Raycast(lidarHitPoint);
						TT_Now_i += 1;
						if (TT_Now_i >= RightAngleCameras.Count) { TT_Now_i = 0; }
						if (!FoundTarget.IsEmpty() && FoundTarget.EntityId == Tgt.EntityId)
						{
							return new Target(FoundTarget);
						}

						//常规未找到，继续遍历摄像头进行搜索
						if (FoundTarget.IsEmpty() || FoundTarget.EntityId != Tgt.EntityId)
						{
							for (int i = 0; i < RightAngleCameras.Count; i++)
							{
								FoundTarget = RightAngleCameras[TT_Now_i].Raycast(lidarHitPoint);
								TT_Now_i += 1;
								if (TT_Now_i >= RightAngleCameras.Count) { TT_Now_i = 0; }
								if (!FoundTarget.IsEmpty() && FoundTarget.EntityId == Tgt.EntityId)
								{
									return new Target(FoundTarget);
								}
							}
						}
					}
				}
				//遍历搜索也未找到，进行脉冲阵面扫描
				if (FoundTarget.IsEmpty() || FoundTarget.EntityId != Tgt.EntityId)
				{
					if (ScanSpeed >= 1 || Ship.timetick % Math.Ceiling(1 / ScanSpeed) == 0)
					{
						int LostTick = Ship.timetick - Tgt.TimeStamp;
						double S_Radius = Tgt.Diameter * 1.5; //搜索半径为目标直径1.5倍
						double S_Interval = Tgt.Diameter / 5; //搜索间隙是目标直径的1/5
						Vector3D CenterPoint = Tgt.Position + Tgt.Velocity * LostTick / 60 + Vector3D.Normalize(Tgt.Position - this.Position) * Tgt.Diameter / 2;
						List<Vector3D> Points = new List<Vector3D>();
						Points.Add(CenterPoint);

						//这里计算出与飞船和目标连线垂直，且互相垂直的两个向量，用作x和y方向遍历
						Vector3D Vertical_X = Ship.CaculateVerticalVector((CenterPoint - this.Position), CenterPoint);
						Vector3D Vertical_Y = Vector3D.Normalize(Vector3D.Cross((CenterPoint - this.Position), Vertical_X));
						for (double x = 0; x < S_Radius; x += S_Interval)
						{
							for (double y = 0; y < S_Radius; y += S_Interval)
							{
								Points.Add(CenterPoint + Vertical_X * x + Vertical_Y * y);
								Points.Add(CenterPoint + Vertical_X * (-x) + Vertical_Y * y);
								Points.Add(CenterPoint + Vertical_X * x + Vertical_Y * (-y));
								Points.Add(CenterPoint + Vertical_X * (-x) + Vertical_Y * (-y));
							}
						}

						FoundTarget = this.PulseScanSingle(Points, Tgt.EntityId).EntityInfo;
						if (!FoundTarget.IsEmpty() && FoundTarget.EntityId == Tgt.EntityId)
						{
							MatrixD TargetMainMatrix = FoundTarget.Orientation;
							MatrixD TargetLookAtMatrix = MatrixD.CreateLookAt(new Vector3D(), TargetMainMatrix.Forward, TargetMainMatrix.Up);
							Vector3D hitpoint = new Vector3D();
							Vector3D.TryParse(FoundTarget.HitPosition.ToString(), out hitpoint);
							hitpoint = hitpoint + Vector3D.Normalize(hitpoint - this.Position) * 2;
							this.TrackDeviationToTarget = Vector3D.TransformNormal(hitpoint - FoundTarget.Position, TargetLookAtMatrix);
							return new Target(FoundTarget);
						}
					}
				}

				return new Target(FoundTarget);
			}

			/* ===== 运动控制相关 ===== */

			// ----- 运动方块还原 -----
			public void MotionInit()
			{
				this.SetThrustOverride("All", 0);
				this.SetGyroOverride(false);
				this.SetRotorsValue("All", 0);
			}

			// ----- 更新飞船物理信息 -----
			// 会自动更新本飞船的基本物理状态信息和时钟变量timetick
			public void UpdatePhysical()
			{
				this.Diameter = (this.Cockpit.CubeGrid.Max - this.Cockpit.CubeGrid.Min).Length() * this.Cockpit.CubeGrid.GridSize;
				this.Acceleration = ((this.Cockpit.GetPosition() - this.Position) * 60 - this.Velocity) * 60;
				this.Velocity = (this.Cockpit.GetPosition() - this.Position) * 60;
				this.Position = this.Cockpit.GetPosition();
			}

			// ----- 瞄准坐标点 -----
			// 使用PID算法，控制陀螺仪瞄准
			// 返回是否瞄准完成
			private List<Vector3D> Aim_PID_Data = new List<Vector3D>();
			public bool AimAtPosition(Vector3D TargetPos)
			{
				MatrixD refLookAtMatrix = MatrixD.CreateLookAt(new Vector3D(), this.Cockpit.WorldMatrix.Forward, this.Cockpit.WorldMatrix.Up);
				Vector3D PositionToMe = Vector3D.Normalize(Vector3D.TransformNormal(TargetPos - this.Position, refLookAtMatrix));

				//储存采样点
				if (Aim_PID_Data.Count < AimPID_T)
				{
					for (int i = 0; i < AimPID_T; i++)
					{
						Aim_PID_Data.Add(new Vector3D());
					}
				}
				else { Aim_PID_Data.Remove(Aim_PID_Data[0]); Aim_PID_Data.Add(PositionToMe); }

				//获得采样点积分
				double X_I = 0;
				double Y_I = 0;
				foreach (Vector3D datapoint in Aim_PID_Data)
				{
					X_I += datapoint.X;
					Y_I += datapoint.Y;
				}

				//计算输出
				double YawValue = AimPID_P * (PositionToMe.X + (1 / AimPID_I) * X_I + AimPID_D * (Aim_PID_Data[AimPID_T - 1].X - Aim_PID_Data[0].X) / AimPID_T) * 60;
				double PitchValue = AimPID_P * (PositionToMe.Y + (1 / AimPID_I) * Y_I + AimPID_D * (Aim_PID_Data[AimPID_T - 1].Y - Aim_PID_Data[0].Y) / AimPID_T) * 60;
				this.SetGyroValue(YawValue, PitchValue, 0);
				this.SetGyroOverride(true);

				// 计算当前与预期瞄准点的瞄准夹角
				Vector3D V_A = TargetPos - this.Position;
				Vector3D V_B = this.Cockpit.WorldMatrix.Forward;
				double Angle = Math.Acos(Vector3D.Dot(V_A, V_B) / (V_A.Length() * V_B.Length())) * 180 / Math.PI;
				if (Angle <= AimRatio) { return true; }
				else { return false; }
			}

			// ----- 导航到坐标点 -----
			// 支持传入一个参考速度来跟踪目标进行速度匹配
			// 返回是否导航完成
			// 依赖SingleDirectionThrustControl()方法来执行对xyz三轴的独立运算，运算考虑了推进器是否可以工作，但不考虑供电带来的效率问题。
			// 本方法的结果路径是一个加速-减速-停止路径，通常不会错过目标，在此前提下本方法时间最优，在减速阶段处于对向推进器频繁满载震荡状态，在物理结果上是匀减速运动。
			public bool NavigationTo(Vector3D Pos, Vector3D TargetVelocity = new Vector3D())
			{
				double ShipMass = this.Cockpit.CalculateShipMass().PhysicalMass;
				//这个ThrustsPowers经过计算后，分别代表前后左右上下6个方向的理论最大加速度
				double[] ThrustsPowers = new double[6];
				for (int i = 0; i < this.Thrusts.Count; i++)
				{
					if (this.Thrusts[i].IsFunctional)
					{
						switch (this.ThrustField[i])
						{
							case ("Backward"): ThrustsPowers[0] += this.Thrusts[i].MaxEffectiveThrust; break;
							case ("Forward"): ThrustsPowers[1] += this.Thrusts[i].MaxEffectiveThrust; break;
							case ("Right"): ThrustsPowers[2] += this.Thrusts[i].MaxEffectiveThrust; break;
							case ("Left"): ThrustsPowers[3] += this.Thrusts[i].MaxEffectiveThrust; break;
							case ("Down"): ThrustsPowers[4] += this.Thrusts[i].MaxEffectiveThrust; break;
							case ("Up"): ThrustsPowers[5] += this.Thrusts[i].MaxEffectiveThrust; break;
						}
					}
				}
				for (int i = 0; i < ThrustsPowers.Length; i++)
				{
					ThrustsPowers[i] /= ShipMass;
				}

				MatrixD refLookAtMatrix = MatrixD.CreateLookAt(new Vector3D(), this.Cockpit.WorldMatrix.Forward, this.Cockpit.WorldMatrix.Up);
				//这里的PosToMe表示目标坐标基于自己坐标系的座标，x左-右+，y下-上+，z前-后+，MeVelocityToMe使用相同的坐标系规则，表示自己的速度基于自己坐标系
				Vector3D PosToMe = Vector3D.TransformNormal(Pos - this.Position, refLookAtMatrix);
				Vector3D MeVelocityToMe = Vector3D.TransformNormal(this.Velocity, refLookAtMatrix);

				this.SingleDirectionThrustControl(PosToMe.Z, MeVelocityToMe.Z, ThrustsPowers[0], ThrustsPowers[1], "Backward", "Forward", 0.5);
				this.SingleDirectionThrustControl(PosToMe.X, MeVelocityToMe.X, ThrustsPowers[2], ThrustsPowers[3], "Right", "Left", 0.5);
				this.SingleDirectionThrustControl(PosToMe.Y, MeVelocityToMe.Y, ThrustsPowers[5], ThrustsPowers[4], "Up", "Down", 0.5);

				if (TargetVelocity.Length() != 0)
				{
					Vector3D TargetVelocityToMe = Vector3D.TransformNormal(TargetVelocity - this.Velocity, refLookAtMatrix);
					if (TargetVelocityToMe.X > 0) { SetThrustOverride("Left", 100); } else if (TargetVelocityToMe.X < 0) { SetThrustOverride("Right", 100); }
					if (TargetVelocityToMe.Y > 0) { SetThrustOverride("Down", 100); } else if (TargetVelocityToMe.Y < 0) { SetThrustOverride("Up", 100); }
					if (TargetVelocityToMe.Z > 0) { SetThrustOverride("Forward", 100); } else if (TargetVelocityToMe.X < 0) { SetThrustOverride("Backward", 100); }
				}

				if (PosToMe.Length() <= 1) { return true; }
				return false;
			}

			// ----- 导航到坐标点（辅助方法） -----
			// 用于辅助NavigationTo()方法
			// 传入基于自己坐标系的目标单向方向，自己的单向速度，正向加速度，反向加速度，正向推进器方向名，反向推进器方向名，导航精度
			public void SingleDirectionThrustControl(double PosToMe, double VelocityToMe, double PostiveMaxAcceleration, double NagtiveMaxAcceleration, string PostiveThrustDirection, string NagtiveThrustDirection, double StopRatio)
			{
				if (PosToMe < -StopRatio)
				{
					double StopTime = -VelocityToMe / NagtiveMaxAcceleration;
					if (StopTime < 0)
					{
						this.SetThrustOverride(PostiveThrustDirection, 100);
						this.SetThrustOverride(NagtiveThrustDirection, 0);
					}
					else
					{
						double StopDistance = -VelocityToMe * StopTime + 0.5 * NagtiveMaxAcceleration * StopTime * StopTime;
						if (Math.Abs(PosToMe) > StopDistance)
						{
							this.SetThrustOverride(PostiveThrustDirection, 100);
							this.SetThrustOverride(NagtiveThrustDirection, 0);
						}
						else
						{
							this.SetThrustOverride(PostiveThrustDirection, 0);
							this.SetThrustOverride(NagtiveThrustDirection, 100);
						}
					}
				}
				else if (PosToMe > StopRatio)
				{
					double StopTime = VelocityToMe / NagtiveMaxAcceleration;
					if (StopTime < 0)
					{
						//此时目标在后，运动速度是向前的
						this.SetThrustOverride(PostiveThrustDirection, 0);
						this.SetThrustOverride(NagtiveThrustDirection, 100);
					}
					else
					{
						double StopDistance = VelocityToMe * StopTime + 0.5 * NagtiveMaxAcceleration * StopTime * StopTime;
						if (Math.Abs(PosToMe) > StopDistance)
						{
							//实际距离大于刹车距离，执行推进
							this.SetThrustOverride(PostiveThrustDirection, 0);
							this.SetThrustOverride(NagtiveThrustDirection, 100);
						}
						else
						{
							//实际距离小于刹车距离，执行刹车
							this.SetThrustOverride(PostiveThrustDirection, 100);
							this.SetThrustOverride(NagtiveThrustDirection, 0);
						}
					}
				}
				else
				{
					this.SetThrustOverride(PostiveThrustDirection, 0);
					this.SetThrustOverride(NagtiveThrustDirection, 0);
				}
			}

			/* ===== 方块控制相关 ===== */
			// ----- 基础控制类方法 -----
			public void TurnBlocksOnOff(List<IMyTerminalBlock> B, bool o)
			{ foreach (var b in B) { b.ApplyAction(o ? "OnOff_On" : "OnOff_Off"); } }
			public void TurnBlocksOnOff(List<IMyGyro> B, bool o)
			{ foreach (var b in B) { b.ApplyAction(o ? "OnOff_On" : "OnOff_Off"); } }
			public void TurnBlocksOnOff(List<IMyThrust> B, bool o)
			{ foreach (var b in B) { b.ApplyAction(o ? "OnOff_On" : "OnOff_Off"); } }
			public void TurnBlocksOnOff(List<IMyLargeTurretBase> B, bool o)
			{ foreach (var b in B) { b.ApplyAction(o ? "OnOff_On" : "OnOff_Off"); } }

			// ----- 控制推进器越级 -----
			// 可传入Direcation = "All"、"Backward"、"Forward"、"Left"、"Right"、"Up"、"Down"
			public void SetThrustOverride(string Direction, double Value)
			{
				if (Value > 100) { Value = 100; }
				if (Value < 0) { Value = 0; }
				for (int i = 0; i < this.Thrusts.Count; i++)
				{
					if (Direction == "All") { this.Thrusts[i].ThrustOverridePercentage = (float)Value; }
					else { if (this.ThrustField[i] == Direction) { this.Thrusts[i].ThrustOverridePercentage = (float)Value; } }
				}
			}

			// ----- 开关陀螺仪越级 -----
			public void SetGyroOverride(bool bOverride)
			{ foreach (IMyGyro g in this.Gyroscopes) { g.GyroOverride = bOverride; } }

			// ----- 控制陀螺仪越级 -----
			// 传入基于主控Cockpit的Yaw Pitch Roll，自动检测所有陀螺仪执行对应控制
			public void SetGyroValue(double Y, double P, double R)
			{
				for (int i = 0; i < this.Gyroscopes.Count; i++)
				{
					this.Gyroscopes[i].SetValue(gyroYawField[i], (float)Y * gyroYawFactor[i]);
					this.Gyroscopes[i].SetValue(gyroPitchField[i], (float)P * gyroPitchFactor[i]);
					this.Gyroscopes[i].SetValue(gyroRollField[i], (float)R * gyroRollFactor[i]);
				}
			}

			// ----- 控制陀螺仪越级 -----
			// 可传入基于主控Cockpit的Yaw、Pitch、Roll中的单个轴进行控制，而不影响其他轴的状态
			public void SetGyroValue(string Field, double Value)
			{
				switch (Field)
				{
					case ("Yaw"):
						for (int i = 0; i < this.Gyroscopes.Count; i++)
						{
							this.Gyroscopes[i].SetValue(gyroYawField[i], (float)Value * gyroYawFactor[i]);
						}
						break;
					case ("Pitch"):
						for (int i = 0; i < this.Gyroscopes.Count; i++)
						{
							this.Gyroscopes[i].SetValue(gyroPitchField[i], (float)Value * gyroPitchFactor[i]);
						}
						break;
					case ("Roll"):
						for (int i = 0; i < this.Gyroscopes.Count; i++)
						{
							this.Gyroscopes[i].SetValue(gyroRollField[i], (float)Value * gyroRollFactor[i]);
						}
						break;
				}

			}

			// ----- 让所有武器射击一次 -----
			public void WeaponsShoot()
			{
				this.GatlingGuns.ForEach(delegate (IMySmallGatlingGun g) { g.ApplyAction("ShootOnce"); });
				this.RocketLaunchers.ForEach(delegate (IMySmallMissileLauncher g) { g.ApplyAction("ShootOnce"); });
				try
				{
					this.FireActionBlocks.ForEach(delegate (IMyTerminalBlock g) { g.ApplyAction(FireActionName); });
				}
				catch { }
			}

			// ----- 设置转子转速 -----
			// 根据转子设定的限制做了判断，防止转子转动超过限制
			public void SetRotorsValue(string Direction, double value)
			{
				for (int i = 0; i < this.Rotors.Count; i++)
				{
					if (Direction == "All")
					{
						if (this.Rotors[i].Angle < this.Rotors[i].UpperLimitRad && this.Rotors[i].Angle > this.Rotors[i].LowerLimitRad)
						{
							this.Rotors[i].TargetVelocityRPM = (float)value * this.RotorsFactor[i];
						}
					}
					else if (this.RotorsField[i] == Direction)
					{
						if (this.Rotors[i].Angle < this.Rotors[i].UpperLimitRad && this.Rotors[i].Angle > this.Rotors[i].LowerLimitRad)
						{
							this.Rotors[i].TargetVelocityRPM = (float)value * this.RotorsFactor[i];
						}
					}
				}
			}

			/* ===== Target类 ===== */
			// 由于摄像头、探测器等获取到的目标是 MyDetectedEntityInfo 类型，不方便计算和更新
			// 本类中统一将其转换为Target类进行操作
			public class Target
			{
				public string Name; //名字
				public long EntityId; //唯一ID，当这个值为0可判断该Target是空的
				public double Diameter; //半径
				public int TimeStamp; //记录时间戳、基于Ship类中的timetick的值来记录
				public Vector3D Position;
				public Vector3D Velocity;
				public Vector3D Acceleration;
				public Vector3D HitPosition;
				public MatrixD Orientation;
				public Vector3D AccurateLockPositionToTarget;
				public MyDetectedEntityInfo EntityInfo;

				public Target()
				{
					this.EntityId = 0;
					this.TimeStamp = 0;
					this.EntityInfo = new MyDetectedEntityInfo();
				}
				public Target(MyDetectedEntityInfo thisEntity)
				{
					this.EntityId = thisEntity.EntityId;
					this.Name = thisEntity.Name;
					this.Diameter = Vector3D.Distance(thisEntity.BoundingBox.Max, thisEntity.BoundingBox.Min) / 2;
					Vector3D.TryParse(thisEntity.Position.ToString(), out this.Position);
					Vector3D.TryParse(thisEntity.Velocity.ToString(), out this.Velocity);
					Vector3D.TryParse(thisEntity.HitPosition.ToString(), out this.HitPosition);
					this.Acceleration = new Vector3D();
					this.Orientation = thisEntity.Orientation;
					this.TimeStamp = Ship.timetick;
					this.EntityInfo = thisEntity;
				}
				public void UpdatePhysical(MyDetectedEntityInfo NewInfo)
				{
					this.Diameter = Vector3D.Distance(NewInfo.BoundingBox.Max, NewInfo.BoundingBox.Min) / 2;
					Vector3D.TryParse(NewInfo.Position.ToString(), out this.Position);
					Vector3D.TryParse(NewInfo.HitPosition.ToString(), out this.HitPosition);
					Vector3D velocity = new Vector3D();
					Vector3D.TryParse(NewInfo.Velocity.ToString(), out velocity);
					this.Acceleration = (velocity - this.Velocity) * 60 / (Ship.timetick - this.TimeStamp > 0 ? Ship.timetick - this.TimeStamp : 1);
					this.Velocity = velocity;
					this.Orientation = NewInfo.Orientation;
					this.TimeStamp = Ship.timetick;
					this.EntityInfo = NewInfo;
				}
				public void UpdatePhysical(Target _T)
				{
					this.UpdatePhysical(_T.EntityInfo);
				}
				public bool IsEmpty()
				{
					return this.EntityId == 0;
				}
			}

			/* ===== 静态方法 ===== */
			// 静态方法通常是一些纯计算的方法，静态方法属于Ship这个类而不是某个实例化出来的Ship对象。
			// 使用静态方法时，请直接使用 Ship.GetCanScanCameras()，而不是对实例化出来的某个Ship实体使用。

			// -- 按FCS 8.0通讯标准读取一条参数 --
			public static string GetOneInfo(string CustomData, string Title, string ArgName)
			{
				string[] infos = CustomData.Split('\n');
				bool right = false;
				for (int i = 0; i < infos.Length; i++)
				{
					if (infos[i].Contains("[" + Title + "]"))
					{
						right = true;
					}
					if (right && infos[i].Split('=')[0] == ArgName)
					{
						return infos[i].Split('=')[1];
					}
					if (infos[i].Contains("[\\" + Title + "]"))
					{
						break;
					}
				}
				return "";
			}

			// -- 计算子弹碰撞点 --
			public static Vector3D HitPointCaculate(Vector3D Me_Position, Vector3D Me_Velocity, Vector3D Me_Acceleration, Vector3D Target_Position, Vector3D Target_Velocity, Vector3D Target_Acceleration,
										double Bullet_InitialSpeed, double Bullet_Acceleration, double Bullet_MaxSpeed)
			{
				//迭代算法   
				Vector3D HitPoint = new Vector3D();
				Vector3D Smt = Target_Position - Me_Position;//发射点指向目标的矢量   
				Vector3D Velocity = Target_Velocity - Me_Velocity; //目标飞船和自己飞船总速度   
				Vector3D Acceleration = Target_Acceleration; //目标飞船和自己飞船总加速度   

				double AccTime = (Bullet_Acceleration == 0 ? 0 : (Bullet_MaxSpeed - Bullet_InitialSpeed) / Bullet_Acceleration);//子弹加速到最大速度所需时间   
				double AccDistance = Bullet_InitialSpeed * AccTime + 0.5 * Bullet_Acceleration * AccTime * AccTime;//子弹加速到最大速度经过的路程   

				double HitTime = 0;

				if (AccDistance < Smt.Length())//目标在炮弹加速过程外   
				{
					HitTime = (Smt.Length() - Bullet_InitialSpeed * AccTime - 0.5 * Bullet_Acceleration * AccTime * AccTime + Bullet_MaxSpeed * AccTime) / Bullet_MaxSpeed;
					HitPoint = Target_Position + Velocity * HitTime + 0.5 * Acceleration * HitTime * HitTime;
				}
				else//目标在炮弹加速过程内 
				{
					double HitTime_Z = (-Bullet_InitialSpeed + Math.Pow((Bullet_InitialSpeed * Bullet_InitialSpeed + 2 * Bullet_Acceleration * Smt.Length()), 0.5)) / Bullet_Acceleration;
					double HitTime_F = (-Bullet_InitialSpeed - Math.Pow((Bullet_InitialSpeed * Bullet_InitialSpeed + 2 * Bullet_Acceleration * Smt.Length()), 0.5)) / Bullet_Acceleration;
					HitTime = (HitTime_Z > 0 ? (HitTime_F > 0 ? (HitTime_Z < HitTime_F ? HitTime_Z : HitTime_F) : HitTime_Z) : HitTime_F);
					HitPoint = Target_Position + Velocity * HitTime + 0.5 * Acceleration * HitTime * HitTime;
				}
				//迭代，仅迭代更新碰撞时间，每次迭代更新右5位数量级   
				for (int i = 0; i < 3; i++)
				{
					if (AccDistance < Vector3D.Distance(HitPoint, Me_Position))//目标在炮弹加速过程外   
					{
						HitTime = (Vector3D.Distance(HitPoint, Me_Position) - Bullet_InitialSpeed * AccTime - 0.5 * Bullet_Acceleration * AccTime * AccTime + Bullet_MaxSpeed * AccTime) / Bullet_MaxSpeed;
						HitPoint = Target_Position + Velocity * HitTime + 0.5 * Acceleration * HitTime * HitTime;
					}
					else//目标在炮弹加速过程内   
					{
						double HitTime_Z = (-Bullet_InitialSpeed + Math.Pow((Bullet_InitialSpeed * Bullet_InitialSpeed + 2 * Bullet_Acceleration * Vector3D.Distance(HitPoint, Me_Position)), 0.5)) / Bullet_Acceleration;
						double HitTime_F = (-Bullet_InitialSpeed - Math.Pow((Bullet_InitialSpeed * Bullet_InitialSpeed + 2 * Bullet_Acceleration * Vector3D.Distance(HitPoint, Me_Position)), 0.5)) / Bullet_Acceleration;
						HitTime = (HitTime_Z > 0 ? (HitTime_F > 0 ? (HitTime_Z < HitTime_F ? HitTime_Z : HitTime_F) : HitTime_Z) : HitTime_F);
						HitPoint = Target_Position + Velocity * HitTime + 0.5 * Acceleration * HitTime * HitTime;
					}
				}
				return HitPoint;
			}

			// -- 计算可扫描的摄像头 --
			public static List<IMyCameraBlock> GetCanScanCameras(List<IMyCameraBlock> Cams, Vector3D Point)
			{
				List<IMyCameraBlock> res = new List<IMyCameraBlock>();
				foreach (IMyCameraBlock cm in Cams)
				{
					if (cm.IsFunctional && cm.CanScan(Point))
					{
						res.Add(cm);
					}
				}
				return res;
			}
			// -- 计算某向量的垂直向量 --
			// 传入一个向量，和一个点，返回沿这个点出发与传入向量垂直的归一化向量
			public static Vector3D CaculateVerticalVector(Vector3D Vector, Vector3D Point)
			{
				double x = 1;
				double y = 1;
				double z = (Point.X * Vector.X + Point.Y * Vector.Y + Point.Z * Vector.Z) / Vector.Z;
				return Vector3D.Normalize(new Vector3D(x, y, z));
			}
		}
	}
}
