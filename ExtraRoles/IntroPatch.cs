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
            if (!PlayerControl.LocalPlayer.isPlayerRole("Joker"))
                return true;

            var jokerTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            jokerTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = jokerTeam;
            return true;
        }

        static void Postfix(IntroCutscene.CoBegin__d __instance)
        {
            var officer = Main.Logic.getRolePlayer("Officer");
            if (officer != null)
                officer.LastAbilityTime = DateTime.UtcNow;

            if (PlayerControl.LocalPlayer.isPlayerRole("Medic"))
            {
                __instance.__this.Title.Text = "Médico";
                __instance.__this.Title.Color = Main.Palette.medicColor;
                __instance.__this.ImpostorText.Text = "Crie um escudo para proteger um [8DFFFF]Tripulante";
                __instance.__this.BackgroundBar.material.color = Main.Palette.medicColor;
                return;
            }

            if (PlayerControl.LocalPlayer.isPlayerRole("Officer"))
            {
                __instance.__this.Title.Text = "Xerife";
                __instance.__this.Title.Color = Main.Palette.officerColor;
                __instance.__this.ImpostorText.Text = "Atire no [FF0000FF]Impostor";
                __instance.__this.BackgroundBar.material.color = Main.Palette.officerColor;
                return;
            }

            if (PlayerControl.LocalPlayer.isPlayerRole("Engineer"))
            {
                __instance.__this.Title.Text = "Engenheiro";
                __instance.__this.Title.Color = Main.Palette.engineerColor;
                __instance.__this.ImpostorText.Text = "Dê manutenção à sistemas importantes na instalação";
                __instance.__this.BackgroundBar.material.color = Main.Palette.engineerColor;
                return;
            }

            if (PlayerControl.LocalPlayer.isPlayerRole("Joker"))
            {
                __instance.__this.Title.Text = "Coringa";
                __instance.__this.Title.Color = Main.Palette.jokerColor;
                __instance.__this.ImpostorText.Text = "Seja votado para sair da instalação para ganhar";
                __instance.__this.BackgroundBar.material.color = Main.Palette.jokerColor;
            }
        }
    }
}