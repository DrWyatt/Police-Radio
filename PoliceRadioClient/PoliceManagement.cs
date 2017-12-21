using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace PoliceRadioClient
{
    public class PoliceManagement : BaseScript
    {
        public bool isCop = false;
        public bool isAdmin = false;
        public bool isFire = false;

        public string callsign;
        public int department; 
        public string pluginName;

        public async void Checks(string pluginAcronym)
        {
            pluginName = pluginAcronym;
            while (true)
            {
                await Delay(1000);
                TriggerServerEvent("pm:isCop", GetPlayerServerId(PlayerId()), pluginName+":isCop", pluginName+":isNotCop");
                TriggerServerEvent("pm:isAdmin", GetPlayerServerId(PlayerId()), pluginName + ":isAdmin", pluginName + ":isNotAdmin");
                TriggerServerEvent("pm:isFire", GetPlayerServerId(PlayerId()), pluginName + ":isFire", pluginName + ":isNotFire");
            }
        }

        public void IsCop(string callsignR, int departmentR)
        {
            isCop = true;
            isFire = false;
            callsign = callsignR;
            department = departmentR;
        }

        public void IsNotCop()
        {
            isCop = false;
        }

        public void IsFire(string callsignR, int departmentR)
        {
            isFire = true;
            isCop = false;
            callsign = callsignR;
            department = departmentR;
        }

        public void IsNotFire()
        {
            isFire = false;
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
