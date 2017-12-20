using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace PoliceRadioClient
{
    public class PoliceRadioClient : BaseScript
    {
        PoliceManagement pm = new PoliceManagement();

        public PoliceRadioClient()
        {
            RegisterCommand("pr", new Action<int, List<dynamic>, string>((source, args, rawCommand) => { PoliceRadio(source, args, rawCommand); }), false);
            RegisterCommand("dr", new Action<int, List<dynamic>, string>((source, args, rawCommand) => { DepartmentRadio(source, args, rawCommand); }), false);
            EventHandlers.Add("PolRa:spr", new Action<string>(SPR));
            EventHandlers.Add("PolRa:sdr", new Action<string>(SDR));
            PolM("PolRa");
        }

        private void SPR(string arg)
        {
            TriggerEvent("chatMessage", "", new[] { 0, 191, 255 }, "^*(PR) ["+GetPlayerServerId(PlayerId())+"] " + pm.callsign + ": "+arg);
        }

        private void SDR(string arg)
        {
            TriggerEvent("chatMessage", "", new[] { 255, 255, 0 }, "^*(DR) ["+ GetPlayerServerId(PlayerId()) + "] " + pm.callsign + ": "+arg);
        }

        private void PoliceRadio(int sourceID, List<dynamic> args, string rawCommand)
        {
            string message = string.Join(" ", args);
            if (pm.isCop)
                TriggerServerEvent("pm:triggerToAllCops", "PolRa:spr", message );
            else
                TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "YOU ARE NOT A COP!");
        }

        private void DepartmentRadio(int sourceID, List<dynamic> args, string rawCommand)
        {
            string message = string.Join(" ", args);
            if (pm.isCop)
                TriggerServerEvent("pm:triggerToAllDepartment", pm.department, "PolRa:sdr", message);
            else
                TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "YOU ARE NOT A COP!");
        }

        private void PolM(string pluginName)
        {
            pm.Checks(pluginName);
            EventHandlers.Add(pluginName + ":isCop", new Action<string, int>(pm.IsCop));
            EventHandlers.Add(pluginName + ":isNotCop", new Action(pm.IsNotCop));
            EventHandlers.Add(pluginName + ":isAdmin", new Action(pm.IsAdmin));
            EventHandlers.Add(pluginName + ":isNotAdmin", new Action(pm.IsNotAdmin));
        }
    }
}
