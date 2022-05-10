using System;
using System.Linq;
using RFPocketResizer.Models;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RFPocketResizer.Utils
{
    internal static class PocketUtil
    {
        internal static PocketModel GetBestPocket(UnturnedPlayer player)
        {
            var permissions = player.GetPermissions().Select(a => a.Name).Where(p =>
                p.ToLower().StartsWith($"{Plugin.Conf.PermissionPrefix}.") && !p.Equals($"{Plugin.Conf.PermissionPrefix}.",
                    StringComparison.InvariantCultureIgnoreCase));
            var enumerable = permissions as string[] ?? permissions.ToArray();
            if (enumerable.Length == 0)
                return null;
            var bestPocket = new PocketModel(0, 0);
            foreach (var pocket in enumerable)
            {
                var pocketSplit = pocket.Split('.');
                if (pocketSplit.Length != 3)
                {
                    Logger.LogError($"[{Plugin.Inst.Name}] Error: PermissionPrefix must not contain '.'");
                    Logger.LogError($"[{Plugin.Inst.Name}] Invalid permission format: {pocket}");
                    Logger.LogError($"[{Plugin.Inst.Name}] Correct format: 'permPrefix'.'width'.'height'");
                    continue;
                }

                try
                {
                    byte.TryParse(pocketSplit[1], out var w);
                    byte.TryParse(pocketSplit[2], out var h);
                    if (w * h > bestPocket.Width * bestPocket.Height)
                        bestPocket = new PocketModel(w, h);
                }
                catch (Exception ex)
                {
                    bestPocket = new PocketModel(5, 3);

                    Logger.LogError($"[{Plugin.Inst.Name}] Error: " + ex);
                }
            }
#if DEBUG
            Logger.LogWarning($"[{Plugin.Inst.Name}] Player: {player.CharacterName}");
            Logger.LogWarning($"[{Plugin.Inst.Name}] Found Permissions: " + string.Join(", ", enumerable.ToArray()));
            Logger.LogWarning($"[{Plugin.Inst.Name}] Pocket size taken: {bestPocket.Width} × {bestPocket.Height}");
#endif
            return bestPocket;
        }

        internal static void RevertPocket()
        {
            foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
            {
                player.Inventory.items[2].resize(5, 3);
#if DEBUG
                Logger.LogWarning($"[{Plugin.Inst.Name}] Player: {player.CharacterName}");
                Logger.LogWarning($"[{Plugin.Inst.Name}] Pocket size: 5 × 3");
#endif
            }
        }

        internal static void Modify(Player player)
        {
            var bestPocket = GetBestPocket(UnturnedPlayer.FromPlayer(player));
            if (bestPocket == null)
                return;

            var items = player.inventory.items[2];
            if (items.height != bestPocket.Height || items.width != bestPocket.Width)
                items.resize((byte)bestPocket.Width, (byte)bestPocket.Height);
#if DEBUG
            Logger.LogWarning($"[{Plugin.Inst.Name}] Player: {player.channel.owner.playerID.characterName}");
            Logger.LogWarning($"[{Plugin.Inst.Name}] Pocket size: {bestPocket.Width} × {bestPocket.Height}");
#endif
        }
    }
}