using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using System.Linq;
using System.Net;
using Reactor;
using Essentials.CustomOptions;
using static ExtraRolesMod.ExtraRoles;
using Reactor.Unstrip;
using UnityEngine;
using System.IO;
using Reactor.Extensions;

namespace ExtraRolesMod
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class HarmonyMain : BasePlugin
    {
        public const string Id = "gg.reactor.extraroles";

        public Harmony Harmony { get; } = new Harmony(Id);

        //This section uses the https://github.com/DorCoMaNdO/Reactor-Essentials framework, but I disabled the watermark.
        //The code said that you were allowed, as long as you provided credit elsewhere. 
        //I added a link in the Credits of the GitHub page, and I'm also mentioning it here.
        //If the owner of this library has any problems with this, just message me on discord and we'll find a solution

        //Hunter101#1337

        public static CustomToggleOption showMedic = CustomOption.AddToggle("Mostrar Médico", false);
        public static CustomStringOption showShieldedPlayer = CustomOption.AddString("Mostrar jogador com escudo para", new string[] { "Self", "Medic", "Self+Medic", "Everyone" });
        public static CustomToggleOption playerMurderIndicator = CustomOption.AddToggle("Indicador de tentativa de assassinato à jogador com escudo", true);
        public static CustomToggleOption showOfficer = CustomOption.AddToggle("Mostrar Xerife", false);
        public static CustomNumberOption OfficerKillCooldown = CustomOption.AddNumber("Tempo de recarga do tiro do Xerife", 30f, 10f, 60f, 2.5f);
        public static CustomToggleOption showEngineer = CustomOption.AddToggle("Mostrar Engenheiro", false);
        public static CustomToggleOption showJoker = CustomOption.AddToggle("Mostrar Coringa", false);
        public static CustomToggleOption jokerCanDieToOfficer = CustomOption.AddToggle("Coringa pode morrer para Xerife", true);
        public static CustomToggleOption medicReportSwitch = CustomOption.AddToggle("Mostrar relatórios do Médico", true);
        public static CustomNumberOption medicReportNameDuration = CustomOption.AddNumber("Tempo em que o relatório do Médico mostra o nome", 5, 0, 60, 2.5f);
        public static CustomNumberOption medicReportColorDuration = CustomOption.AddNumber("Tempo em que o relatório do Médico mostra o tipo de cor", 20, 0, 120, 2.5f);
        public static CustomNumberOption medicSpawnChance = CustomOption.AddNumber("Chance de ter um Médico", 100, 0, 100, 5);
        public static CustomNumberOption officerSpawnChance = CustomOption.AddNumber("Chance de ter um Xerife", 100, 0, 100, 5);
        public static CustomNumberOption engineerSpawnChance = CustomOption.AddNumber("Chance de ter um Engenheiro", 100, 0, 100, 5);
        public static CustomNumberOption jokerSpawnChance = CustomOption.AddNumber("Chance de ter um Coringa", 100, 0, 100, 5);
        
        public ConfigEntry<string> Ip { get; set; }
        public ConfigEntry<ushort> Port { get; set; }

        public override void Load()
        {
            Ip = Config.Bind("Custom", "Ipv4 or Hostname", "127.0.0.1");
            Port = Config.Bind("Custom", "Port", (ushort)22023);

            bundle = AssetBundle.LoadFromFile(Directory.GetCurrentDirectory() + "\\Assets\\bundle");
            breakClip = bundle.LoadAsset<AudioClip>("SB").DontUnload();
            repairIco = bundle.LoadAsset<Sprite>("RE").DontUnload();
            shieldIco = bundle.LoadAsset<Sprite>("SA").DontUnload();
            smallShieldIco = bundle.LoadAsset<Sprite>("RESmall").DontUnload();

            var defaultRegions = ServerManager.DefaultRegions.ToList();
            var ip = Ip.Value;
            if (Uri.CheckHostName(Ip.Value).ToString() == "Dns")
            {
                foreach (IPAddress address in Dns.GetHostAddresses(Ip.Value))
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ip = address.ToString(); break;
                    }
                }
            }

            defaultRegions.Insert(0, new RegionInfo(
                "Custom", ip, new[]
                {
                    new ServerInfo($"Custom-Server", ip, Port.Value)
                })
            );

            ServerManager.DefaultRegions = defaultRegions.ToArray();
            Harmony.PatchAll();
        }
    }
}
