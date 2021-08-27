using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI.Ingame;

namespace SEScript.SE_2
{
    class LTC_Ophelia_Personality:API
    {
        IMyTextSurface opheliaSpeech; //Ophelia文字输出
        List<string> opheliaSpeeches; //文字记录
        bool isCheckReady = false;
        void Main(string msg)
        {

        }
        void CheckComponents()
        {
            opheliaSpeech = GridTerminalSystem.GetBlockWithName("LTC_Ophelia_Speech") as IMyTextSurface;
            if(opheliaSpeech == null)
            {
                return;
            }
        }
        void ExecuteCommand(string msg)
        {

        }
        void RandomSpeech()
        {

        }
        void OpheliaSpeaks(string msg)
        {
            if (opheliaSpeeches.Count <= 11)
            {
                opheliaSpeeches.Add(msg);
            }
            else
            {
                for (int i = 1; i < 12; i++)
                {
                    opheliaSpeeches[i - 1] = opheliaSpeeches[i];
                }
                opheliaSpeeches[11] = msg;
            }

            string displayedMSg = "==========Ophelia========\r\n";
            for (int i = 0; i < opheliaSpeeches.Count; i++)
            {
                displayedMSg += opheliaSpeeches[i] + "\r\n";
            }
            opheliaSpeech.WriteText(displayedMSg);
        }
    }
}
