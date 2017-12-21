using System;
using System.Collections.Generic;
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
            RegisterCommand("fr", new Action<int, List<dynamic>, string>((source, args, rawCommand) => { FireRadio(source, args, rawCommand); }), false);
            RegisterCommand("sr", new Action<int, List<dynamic>, string>((source, args, rawCommand) => { SharedRadio(source, args, rawCommand); }), false);
            RegisterCommand("dr", new Action<int, List<dynamic>, string>((source, args, rawCommand) => { DepartmentRadio(source, args, rawCommand); }), false);
            RegisterCommand("ar", new Action<int, List<dynamic>, string>((source, args, rawCommand) => { AdminRadio(source, args, rawCommand); }), false);
            EventHandlers.Add("PolRa:spr", new Action<List<object>>(SPR));
            EventHandlers.Add("PolRa:sfr", new Action<List<object>>(SFR));
            EventHandlers.Add("PolRa:ssr", new Action<List<object>>(SSR));
            EventHandlers.Add("PolRa:sdr", new Action<List<object>>(SDR));
            EventHandlers.Add("PolRa:sar", new Action<List<object>>(SAR));
            PolM("PolRa");
        }

        private void SPR(List<object> arg)
        {
            int id = (int)arg[0];
            arg.Remove(arg[0]);

            string callsign = (string)arg[0];
            arg.Remove(arg[0]);

            string message = string.Join(" ", arg);

            TriggerEvent("chatMessage", "", new[] { 0, 191, 255 }, "^*(PD) ["+id+"] " + callsign + ": "+ message);
        }

        private void SFR(List<object> arg)
        {
            int id = (int)arg[0];
            arg.Remove(arg[0]);

            string callsign = (string)arg[0];
            arg.Remove(arg[0]);

            string message = string.Join(" ", arg);

            TriggerEvent("chatMessage", "", new[] { 0, 191, 255 }, "^*(FD) [" + id + "] " + callsign + ": " + message);
        }

        private void SDR(List<object> arg)
        {
            int id = (int)arg[0];
            arg.Remove(arg[0]);

            string callsign = (string)arg[0];
            arg.Remove(arg[0]);

            string message = string.Join(" ", arg);

            TriggerEvent("chatMessage", "", new[] { 255, 255, 0 }, "^*(DEPARTMENT) [" + id + "] " + callsign + ": " + message);
        }

        private void SSR(List<object> arg)
        {
            int id = (int)arg[0];
            arg.Remove(arg[0]);

            string callsign = (string)arg[0];
            arg.Remove(arg[0]);

            string message = string.Join(" ", arg);

            TriggerEvent("chatMessage", "", new[] { 255, 255, 0 }, "^*(PD + FD) [" + id + "] " + callsign + ": " + message);
        }

        private void SAR(List<object> arg)
        {
            int id = (int)arg[0];
            arg.Remove(arg[0]);

            string callsign = (string)arg[0];
            arg.Remove(arg[0]);

            string message = string.Join(" ", arg);

            TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "^*(AR) [" + id + "] " + callsign + ": " + message);
        }

        private void PoliceRadio(int sourceID, List<dynamic> args, string rawCommand)
        {
            string message = string.Join(" ", args);
            if (pm.isCop)
                TriggerServerEvent("pm:triggerToAllCops", "PolRa:spr", new List<object> { GetPlayerServerId(PlayerId()), pm.callsign, message });
            else
                TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "YOU ARE NOT A COP!");
        }

        private void FireRadio(int sourceID, List<dynamic> args, string rawCommand)
        {
            string message = string.Join(" ", args);
            if (pm.isFire)
                TriggerServerEvent("pm:triggerToAllFire", "PolRa:sfr", new List<object> { GetPlayerServerId(PlayerId()), pm.callsign, message });
            else
                TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "YOU ARE NOT A FIREFIGHTER!");
        }

        private void DepartmentRadio(int sourceID, List<dynamic> args, string rawCommand)
        {
            string message = string.Join(" ", args);
            if (pm.isCop)
                TriggerServerEvent("pm:triggerToAllDepartment", false, pm.department, "PolRa:sdr", new List<object> { GetPlayerServerId(PlayerId()), pm.callsign, message });
            else if (pm.isFire)
                TriggerServerEvent("pm:triggerToAllDepartment", true, pm.department, "PolRa:sdr", new List<object> { GetPlayerServerId(PlayerId()), pm.callsign, message });
            else
                TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "YOU ARE NOT A COP!");
        }

        private void SharedRadio(int sourceID, List<dynamic> args, string rawCommand)
        {
            string message = string.Join(" ", args);
            if (pm.isCop | pm.isFire)
                TriggerServerEvent("pm:triggerToAllOnDuty", "PolRa:ssr", new List<object> { GetPlayerServerId(PlayerId()), pm.callsign, message });
            else
                TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "YOU ARE NOT A COP OR A FIREFIGHTER!");
        }

        private void AdminRadio(int sourceID, List<dynamic> args, string rawCommand)
        {
            string message = string.Join(" ", args);
            if (pm.isAdmin)
                TriggerServerEvent("pm:triggerToAllAdmins", "PolRa:sar", new List<object> { GetPlayerServerId(PlayerId()), GetPlayerName(PlayerId()), message });
            else
                TriggerEvent("chatMessage", "", new[] { 255, 0, 0 }, "YOU ARE NOT AN ADMIN!");
        }

        private void PolM(string pluginName)
        {
            pm.Checks(pluginName);
            EventHandlers.Add(pluginName + ":isCop", new Action<string, int>(pm.IsCop));
            EventHandlers.Add(pluginName + ":isNotCop", new Action(pm.IsNotCop));
            EventHandlers.Add(pluginName + ":isFire", new Action<string, int>(pm.IsFire));
            EventHandlers.Add(pluginName + ":isNotFire", new Action(pm.IsNotFire));
            EventHandlers.Add(pluginName + ":isAdmin", new Action(pm.IsAdmin));
            EventHandlers.Add(pluginName + ":isNotAdmin", new Action(pm.IsNotAdmin));
        }
    }
}
