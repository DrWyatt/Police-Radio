using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace PoliceRadioServer
{
    public class PoliceRadioServer : BaseScript
    {
        public PoliceRadioServer()
        {
            EventHandlers.Add("chatMessage", new Action<int, int, string, string>(ChatMessage));
        }

        private void ChatMessage([FromSource]int sourceCID, int sourceSID, string sourceName, string message)
        {
            string[] splitMessage = message.Split(' ');
            if (splitMessage[0] == "/pr")
            {
                TriggerEvent("pm:triggerToAllCops", "PolRa:spr", new object[] { message.Replace("/pr ", "") });
            }
            else if(splitMessage[0] == "/dr")
            {
                TriggerServerEvent("pm:triggerToAllDepartment", pm.department, "PolRa:sdr", new object[] { message });
            }
        }
    }
}
