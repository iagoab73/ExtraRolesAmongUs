using HarmonyLib;
using System;
using static ExtraRolesMod.ExtraRoles;

namespace ExtraRolesMod
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    class IntroCutscenePath
    {
        static bool Prefix(IntroCutscene.CoBegin__d __instance)
        {
            if (PlayerControl.LocalPlayer == JokerSettings.Joker)
            {
                var jokerTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                jokerTeam.Add(PlayerControl.LocalPlayer);
                __instance.yourTeam = jokerTeam;
                return true;
            }
            return true;
        }

        static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            OfficerSettings.lastKilled = DateTime.UtcNow.AddSeconds((OfficerSettings.OfficerCD * -1) + 10 + __instance.timer_0);
            //change the name and titles accordingly
            if (PlayerControl.LocalPlayer == MedicSettings.Medic)
            {
                __instance.__this.Title.Text = "Médico";
                __instance.__this.Title.Color = ModdedPalette.medicColor;
                __instance.__this.ImpostorText.Text = "Crie um escudo para proteger um [8DFFFF]Tripulante";
                __instance.__this.BackgroundBar.material.color = ModdedPalette.medicColor;
            }
            if (PlayerControl.LocalPlayer == OfficerSettings.Officer)
            {
                __instance.__this.Title.Text = "Xerife";
                __instance.__this.Title.Color = ModdedPalette.officerColor;
                __instance.__this.ImpostorText.Text = "Atire no [FF0000FF]Impostor";
                __instance.__this.BackgroundBar.material.color = ModdedPalette.officerColor;
            }
            if (PlayerControl.LocalPlayer == EngineerSettings.Engineer)
            {
                __instance.__this.Title.Text = "Engenheiro";
                __instance.__this.Title.Color = ModdedPalette.engineerColor;
                __instance.__this.ImpostorText.Text = "Cuide dos sistemas importantes da instalação";
                __instance.__this.BackgroundBar.material.color = ModdedPalette.engineerColor;
            }
            if (PlayerControl.LocalPlayer == JokerSettings.Joker)
            {
                __instance.__this.Title.Text = "Coringa";
                __instance.__this.Title.Color = ModdedPalette.jokerColor;
                __instance.__this.ImpostorText.Text = "Seja votado para sair da instalação para ganhar";
                __instance.__this.BackgroundBar.material.color = ModdedPalette.jokerColor;
            }
        }
    }
}
