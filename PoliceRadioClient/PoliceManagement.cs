using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace PoliceRadioClient
{
    public class PoliceManagement : BaseScript
    {
        public bool isCop = false;
        public string callsign;
        public int department;
        public bool isAdmin = false;
        public string pluginName;

        public async void Checks(string pluginAcronym)
        {
            pluginName = pluginAcronym;
            while (true)
            {
                await Delay(1000);
                TriggerServerEvent("pm:isCop", GetPlayerServerId(PlayerId()), pluginName+":isCop", pluginName+":isNotCop");
                TriggerServerEvent("pm:isAdmin", GetPlayerServerId(PlayerId()), pluginName + ":isAdmin", pluginName + ":isNotAdmin");
            }
        }

        public void IsCop(string callsignR, int departmentR)
        {
            isCop = true;
            callsign = callsignR;
            department = departmentR;
        }

        public void IsNotCop()
        {
            isCop = false;
        }

        public void IsAdmin()
        {
            isAdmin = true;
        }

        public void IsNotAdmin()
        {
            isAdmin = false;
        }
    }
}
