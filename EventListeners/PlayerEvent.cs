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
            
            PocketUtil.Modify(arg1.player);
        }

        public static void OnConnected(UnturnedPlayer player)
        {
            PocketUtil.Modify(player.Player);
        }

        public static void OnRevived(PlayerLife obj)
        {
            PocketUtil.Modify(obj.player);
        }
    }
}