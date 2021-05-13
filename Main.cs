using System;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using System.Linq;
using PocketResizer.Models;
using Rocket.API;
using Rocket.Core.Logging;
using SDG.Unturned;

namespace PocketResizer
{
    public class Main : RocketPlugin<Configuration>
    {
		private static Main _inst;
		private static Configuration _conf;

		protected override void Load()
		{
			_inst = this;
			_conf = Configuration.Instance;
			
			UnturnedPlayerEvents.OnPlayerUpdateGesture += Events_OnGestureChanged;
			
			Logger.LogWarning("[PocketResizer] Plugin loaded successfully!");
			Logger.LogWarning("[PocketResizer] PocketResizer v1.0.0");
			Logger.LogWarning("[PocketResizer] Author: BarehSolok#2548");
			Logger.LogWarning("[PocketResizer] Enjoy the plugin! ;)");
		}
		protected override void Unload()
		{
			UnturnedPlayerEvents.OnPlayerUpdateGesture -= Events_OnGestureChanged;
			
			if (_conf.RevertOnUnload)
				RevertPocket();
			
			Logger.LogWarning("[PocketResizer] Plugin unloaded successfully!");
		}

		private static void Events_OnGestureChanged(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
		{
			if (gesture != UnturnedPlayerEvents.PlayerGesture.InventoryOpen)
				return;
			var bestPocket = GetBestSize(player);
			if (bestPocket == null) 
				return;
			player.Inventory.items[2].resize((byte)bestPocket.Width, (byte)bestPocket.Height);
#if DEBUG
			Logger.LogWarning($"[PocketResizer] Player: {player.CharacterName}");
			Logger.LogWarning($"[PocketResizer] Pocket size: {bestPocket.Width} × {bestPocket.Height}");
#endif
		}

		private static Pocket GetBestSize(UnturnedPlayer player)
		{
			var permissions = player.GetPermissions().Select(a => a.Name).Where(p => p.ToLower().StartsWith($"{_conf.PermissionPrefix}.") && !p.Equals($"{_conf.PermissionPrefix}.", StringComparison.InvariantCultureIgnoreCase));
			var enumerable = permissions as string[] ?? permissions.ToArray();
			if (enumerable.Length == 0)
				return null;
			var bestPocket = new Pocket (0, 0);
			foreach (var pocket in enumerable)
			{
				var w = byte.Parse(pocket.Split('.')[1]);
				var h = byte.Parse(pocket.Split('.')[2]);
				if (w * h > bestPocket.Width * bestPocket.Height)
					bestPocket = new Pocket(w, h);
			}
#if DEBUG
			Logger.LogWarning($"[PocketResizer] Player: {player.CharacterName}");
			Logger.LogWarning("[PocketResizer] Found Permissions: " + string.Join(", ",enumerable.ToArray()));
			Logger.LogWarning($"[PocketResizer] Pocket size taken: {bestPocket.Width} × {bestPocket.Height}");
#endif
			return bestPocket;
		}
		private static void RevertPocket()
		{
			foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
			{
				player.Inventory.items[2].resize(5, 3);
#if DEBUG
				Logger.LogWarning($"[PocketResizer] Player: {player.CharacterName}");
				Logger.LogWarning($"[PocketResizer] Pocket size: 5 × 3");
#endif
			}
		}
	}
}
