using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using System.Drawing;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API;

namespace InvisPlugin;

[MinimumApiVersion(80)]
public class InvisPlugin : BasePlugin
{
    public override string ModuleName => "InvisPlugin";

    public override string ModuleVersion => "0.1.1";
    public override string ModuleAuthor => "Manio";
    public override string ModuleDescription => "Invisibility plugin";

    HashSet<int?> InvisIds = new HashSet<int?>(); 
    public override void Load(bool hotReload)
    {
        Console.WriteLine(" ");
        Console.WriteLine(" ___   __    _  __   __  ___   _______      _______  ___      __   __  _______  ___   __    _ ");
        Console.WriteLine("|   | |  |  | ||  | |  ||   | |       |    |       ||   |    |  | |  ||       ||   | |  |  | |");
        Console.WriteLine("|   | |   |_| ||  |_|  ||   | |  _____|    |    _  ||   |    |  | |  ||    ___||   | |   |_| |");
        Console.WriteLine("|   | |       ||       ||   | | |_____     |   |_| ||   |    |  |_|  ||   | __ |   | |       |");
        Console.WriteLine("|   | |  _    ||       ||   | |_____  |    |    ___||   |___ |       ||   ||  ||   | |  _    |");
        Console.WriteLine("|   | | | |   | |     | |   |  _____| |    |   |    |       ||       ||   |_| ||   | | | |   |");
        Console.WriteLine("|___| |_|  |__|  |___|  |___| |_______|    |___|    |_______||_______||_______||___| |_|  |__|");
        Console.WriteLine("			     >> Version: 0.1.1");
        Console.WriteLine("		>> GitHub: https://github.com/maniolos/Cs2Invis");
        Console.WriteLine(" ");
    }
    [ConsoleCommand("css_invis", "Invisible command")]
    [RequiresPermissions("@css/invis")]
    public void OnInvisCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null)
            return;
        if (!InvisIds.Contains(player.UserId))
        {
            SetPlayerInvisible(player);
            InvisIds.Add(player.UserId);
            commandInfo.ReplyToCommand("Invisiblity enabled");
        }
        
    }

    [ConsoleCommand("css_uninvis", "Make Visible command")]
    [RequiresPermissions("@css/invis")]
    public void OnUnInvisCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null || !player.IsValid || !player.PawnIsAlive)
            return;
        if (InvisIds.Contains(player.UserId))
        {
            InvisIds.Remove(player.UserId);
            SetPlayerVisible(player);
            commandInfo.ReplyToCommand("Invisiblity disabled");
        }
        
    }
    public static void SetPlayerVisible(CCSPlayerController player)
    {
        var playerPawnValue = player.PlayerPawn.Value;
        if (playerPawnValue == null)
            return;
        playerPawnValue.Render = Color.FromArgb(255, 255, 255, 255);
        Utilities.SetStateChanged(playerPawnValue, "CBaseModelEntity", "m_clrRender");
        var activeWeapon = playerPawnValue!.WeaponServices?.ActiveWeapon.Value;
        if (activeWeapon != null && activeWeapon.IsValid)
        {
            activeWeapon.Render = Color.FromArgb(255, 255, 255, 255);
            activeWeapon.ShadowStrength = 1.0f;
            Utilities.SetStateChanged(activeWeapon, "CBaseModelEntity", "m_clrRender");
        }

        var myWeapons = playerPawnValue.WeaponServices?.MyWeapons;
        if (myWeapons != null)
        {
            foreach (var gun in myWeapons)
            {
                var weapon = gun.Value;
                if (weapon != null)
                {
                    weapon.Render = Color.FromArgb(255, 255, 255, 255);
                    weapon.ShadowStrength = 1.0f;
                    Utilities.SetStateChanged(weapon, "CBaseModelEntity", "m_clrRender");
                }
            }
        }
    }
    public static void SetPlayerInvisible(CCSPlayerController player)
    {
        
        var playerPawnValue = player.PlayerPawn.Value;
        if (playerPawnValue == null || !playerPawnValue.IsValid)
        {
            return;
        }

        if (playerPawnValue != null && playerPawnValue.IsValid)
        {
            playerPawnValue.Render = Color.FromArgb(0, 255, 255, 255);
            Utilities.SetStateChanged(playerPawnValue, "CBaseModelEntity", "m_clrRender");
        }

        var activeWeapon = playerPawnValue!.WeaponServices?.ActiveWeapon.Value;
        if (activeWeapon != null && activeWeapon.IsValid)
        {
            activeWeapon.Render = Color.FromArgb(0, 255, 255, 255);
            activeWeapon.ShadowStrength = 0.0f;
            Utilities.SetStateChanged(activeWeapon, "CBaseModelEntity", "m_clrRender");
        }

        var myWeapons = playerPawnValue.WeaponServices?.MyWeapons;
        if (myWeapons != null)
        {
            foreach (var gun in myWeapons)
            {
                var weapon = gun.Value;
                if (weapon != null)
                {
                    weapon.Render = Color.FromArgb(0, 255, 255, 255);
                    weapon.ShadowStrength = 0.0f;
                    Utilities.SetStateChanged(weapon, "CBaseModelEntity", "m_clrRender");
                }
            }
        }
       
    }
    // Output hooks can use wildcards to match multiple entities
    [GameEventHandler]
    public HookResult OnItemPickup(EventItemPickup @event, GameEventInfo info)
    {
        CCSPlayerController player = @event.Userid!;

        int playerevent = (int)player.UserId!;
 
        if (InvisIds.Contains(playerevent))
        {
            SetPlayerInvisible(player);
        }

        return HookResult.Continue;
    }
}