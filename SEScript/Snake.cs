using System;
using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using System.Numerics;

namespace SE
{
    class Program : API
    {
        IMyShipController controller;
        List<IMyTextSurface> LCDPanels;

        bool isInitReady = false;
        Dictionary<Vector2, int> maps;
        List<Vector2> snakeBody;
        Vector2 fruitPos;

        int tick = 0;
        int inputTick = 0;
        int score = 0;
        int spd = 10;
        int MovePos = 1;

        bool gameOver = false;

        void Main(String arg, UpdateType updateSource)
        {
            if (!isInitReady)
            {
                SelfCheck();
                return;
            }
            if (arg == "5")
            {
                SelfCheck();
            }
            if (gameOver)
            {
                ShowMessage("Game Over\r\nScore: " + score);
                return;
            }

            spd = 10 - score / 10;

            Vector3 dir = controller.MoveIndicator;
            tick += 1;
            inputTick += 1;
            if (dir.X == 0 && dir.Z == 0)
            {
                if (tick >= spd)
                {
                    tick = 0;
                    MoveSnake();
                    RefreshDisplay();
                }
            }
            else
            {
                if (inputTick >= spd)
                {
                    if (dir.X > 0)
                    {
                        MovePos = 1;
                    }
                    if (dir.X < 0)
                    {
                        MovePos = 2;
                    }
                    if (dir.Z > 0)
                    {
                        MovePos = 3;
                    }
                    if (dir.Z < 0)
                    {
                        MovePos = 4;
                    }
                    inputTick = 0;
                    MoveSnake();
                    RefreshDisplay();
                }
            }
        }
        void SelfCheck()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            LCDPanels = new List<IMyTextSurface>();
            GridTerminalSystem.GetBlocksOfType(LCDPanels);
            if (LCDPanels.Count == 0)
            {
                return;
            }
            controller = GridTerminalSystem.GetBlockWithName("controller") as IMyShipController;
            if (controller == null)
            {
                ShowMessage("Cockpit not found");
                return;
            }

            tick = 0;
            inputTick = 0;
            score = 0;
            spd = 10;
            MovePos = 1;

            gameOver = false;

            snakeBody = new List<Vector2>();
            snakeBody.Add(new Vector2(6, 4));
            snakeBody.Add(new Vector2(6, 5));
            snakeBody.Add(new Vector2(6, 6));
            InitMap();
            MoveSnake();
            SpawnFruit();
            isInitReady = true;
        }
        void RefreshDisplay()
        {
            string output = "";
            for (int y = 0; y < 11; y++)
            {
                for (int x = 0; x <= 11; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    if (x == 11)
                    {
                        output += "\r\n";
                        continue;
                    }
                    switch (maps[pos])
                    {
                        case 0:
                            {
                                output += "o";
                                break;
                            }
                        case 1:
                            {
                                output += "@";
                                break;
                            }
                        case 2:
                            {
                                output += "#";
                                break;
                            }
                        case 3:
                            {
                                output += "X";
                                break;
                            }
                    }
                }
            }
            output += "\r\nSCORE: " + score.ToString();
            foreach (IMyTextSurface lcd in LCDPanels)
            {
                lcd.WriteText(output);
            }
        }
        void MoveSnake()
        {
            for (int i = snakeBody.Count - 1; i >= 1; i--)
            {
                snakeBody[i] = new Vector2(snakeBody[i - 1].X, snakeBody[i - 1].Y);
            }
            switch (MovePos)
            {
                case 1:
                    {
                        snakeBody[0] = new Vector2(snakeBody[0].X + 1, snakeBody[0].Y);
                        break;
                    }
                case 2:
                    {
                        snakeBody[0] = new Vector2(snakeBody[0].X - 1, snakeBody[0].Y);
                        break;
                    }
                case 3:
                    {
                        snakeBody[0] = new Vector2(snakeBody[0].X, snakeBody[0].Y + 1);
                        break;
                    }
                case 4:
                    {
                        snakeBody[0] = new Vector2(snakeBody[0].X, snakeBody[0].Y - 1);
                        break;
                    }
            }
            if (snakeBody[0].X < 0)
            {
                snakeBody[0] = new Vector2(10, snakeBody[0].Y);
            }
            if (snakeBody[0].X >= 11)
            {
                snakeBody[0] = new Vector2(0, snakeBody[0].Y);
            }
            if (snakeBody[0].Y < 0)
            {
                snakeBody[0] = new Vector2(snakeBody[0].X, 10);
            }
            if (snakeBody[0].Y >= 11)
            {
                snakeBody[0] = new Vector2(snakeBody[0].X, 0);
            }

            for (int i = 1; i < snakeBody.Count; i++)
            {
                if (snakeBody[0] == snakeBody[i])
                {
                    gameOver = true;
                }
            }
            if (snakeBody[0] == fruitPos)
            {
                snakeBody.Add(fruitPos);
                score += 1;
                SpawnFruit();
            }

            InitMap();

            maps[snakeBody[0]] = 1;
            for (int i = 1; i < snakeBody.Count; i++)
            {
                maps[snakeBody[i]] = 2;
            }
            maps[fruitPos] = 3;
        }
        void InitMap()
        {
            maps = new Dictionary<Vector2, int>();
            for (int y = 0; y < 11; y++)
            {
                for (int x = 0; x < 11; x++)
                {
                    maps[new Vector2(x, y)] = 0;
                }
            }
        }
        void SpawnFruit()
        {
            Vector2 pos;
            Random rnd = new Random();
            do
            {
                pos = new Vector2(rnd.Next(0, 10), rnd.Next(0, 10));
            } while (maps[pos] != 0);
            fruitPos = pos;
        }
        void ShowMessage(string msg)
        {
            foreach (IMyTextSurface lcd in LCDPanels)
            {
                lcd.WriteText(msg);
            }
        }
    }
}
