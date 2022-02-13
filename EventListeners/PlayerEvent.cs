using RFPocketResizer.Utils;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RFPocketResizer.EventListeners
{
    internal static class PlayerEvent
    {
        public static void OnGestureChanged(PlayerAnimator arg1, EPlayerGesture arg2)
        {
            if (arg2 != EPlayerGesture.INVENTORY_START)
                return;
            
            var bestPocket = PocketUtil.GetBestPocket(UnturnedPlayer.FromPlayer(arg1.player));
            if (bestPocket == null)
                return;

            var items = arg1.player.inventory.items[2];
            if (items.height != bestPocket.Height || items.width != bestPocket.Width)
                items.resize((byte)bestPocket.Width, (byte)bestPocket.Height);
#if DEBUG
            Logger.LogWarning($"[{Plugin.Inst.Name}] Player: {arg1.channel.owner.playerID.characterName}");
            Logger.LogWarning($"[{Plugin.Inst.Name}] Pocket size: {bestPocket.Width} × {bestPocket.Height}");
#endif
        }
    }
}