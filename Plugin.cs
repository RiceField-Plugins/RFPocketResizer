using System;
using System.Linq;
using RFPocketResizer.Models;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RFPocketResizer
{
    public class Plugin : RocketPlugin<Configuration>
    {
		internal static Plugin Inst;
		internal static Configuration Conf;

		protected override void Load()
		{
			Inst = this;
			Conf = Configuration.Instance;

			if (Conf.Enabled)
				UnturnedPlayerEvents.OnPlayerUpdateGesture += Events_OnGestureChanged;
			else
				Logger.LogError("[RFPocketResizer] Plugin: DISABLED");
			
			Logger.LogWarning("[RFPocketResizer] Plugin loaded successfully!");
			Logger.LogWarning("[RFPocketResizer] RFPocketResizer v1.0.1");
			Logger.LogWarning("[RFPocketResizer] Made with 'rice' by RiceField Plugins!");
		}
		protected override void Unload()
		{
			if (Conf.Enabled)
				UnturnedPlayerEvents.OnPlayerUpdateGesture -= Events_OnGestureChanged;
			if (Conf.RevertOnUnload)
				RevertPocket();

			Inst = null;
			Conf = null;
			
			Logger.LogWarning("[RFPocketResizer] Plugin unloaded successfully!");
		}

		private static void Events_OnGestureChanged(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
		{
			if (gesture != UnturnedPlayerEvents.PlayerGesture.InventoryOpen)
				return;
			var bestPocket = GetBestPocket(player);
			if (bestPocket == null)
				return;
			player.Inventory.items[2].resize((byte)bestPocket.Width, (byte)bestPocket.Height);
#if DEBUG
			Logger.LogWarning($"[RFPocketResizer] Player: {player.CharacterName}");
			Logger.LogWarning($"[RFPocketResizer] Pocket size: {bestPocket.Width} × {bestPocket.Height}");
#endif
		}

		private static PocketModel GetBestPocket(UnturnedPlayer player)
		{
			var permissions = player.GetPermissions().Select(a => a.Name).Where(p => p.ToLower().StartsWith($"{Conf.PermissionPrefix}.") && !p.Equals($"{Conf.PermissionPrefix}.", StringComparison.InvariantCultureIgnoreCase));
			var enumerable = permissions as string[] ?? permissions.ToArray();
			if (enumerable.Length == 0)
				return null;
			var bestPocket = new PocketModel (0, 0);
			foreach (var pocket in enumerable)
			{
				var pocketSplit = pocket.Split('.');
				if (pocketSplit.Length != 3)
				{
					Logger.LogError("[RFPocketResizer] Error: PermissionPrefix must not contain '.'");
					Logger.LogError($"[RFPocketResizer] Invalid permission format: {pocket}");
					Logger.LogError("[RFPocketResizer] Correct format: 'permPrefix'.'width'.'height'");
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
					
					Logger.LogError("[RFPocketResizer] Error: " + ex);
				}
			}
#if DEBUG
			Logger.LogWarning($"[RFPocketResizer] Player: {player.CharacterName}");
			Logger.LogWarning("[RFPocketResizer] Found Permissions: " + string.Join(", ",enumerable.ToArray()));
			Logger.LogWarning($"[RFPocketResizer] Pocket size taken: {bestPocket.Width} × {bestPocket.Height}");
#endif
			return bestPocket;
		}
		private static void RevertPocket()
		{
			foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
			{
				player.Inventory.items[2].resize(5, 3);
#if DEBUG
				Logger.LogWarning($"[RFPocketResizer] Player: {player.CharacterName}");
				Logger.LogWarning($"[RFPocketResizer] Pocket size: 5 × 3");
#endif
			}
		}
	}
}
