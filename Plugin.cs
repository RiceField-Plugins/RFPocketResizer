using RFPocketResizer.EventListeners;
using RFPocketResizer.Utils;
using Rocket.API.Extensions;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using SDG.Unturned;

namespace RFPocketResizer
{
    public class Plugin : RocketPlugin<Configuration>
    {
	    private static int Major = 1;
	    private static int Minor = 0;
	    private static int Patch = 1;
	    
		internal static Plugin Inst;
		internal static Configuration Conf;

		protected override void Load()
		{
			Inst = this;
			Conf = Configuration.Instance;

			if (Conf.Enabled)
			{
				U.Events.OnPlayerConnected += PlayerEvent.OnConnected;
				PlayerLife.OnRevived_Global += PlayerEvent.OnRevived;
				PlayerAnimator.OnGestureChanged_Global += PlayerEvent.OnGestureChanged;
				
				if (Level.isLoaded)
					foreach (var steamPlayer in Provider.clients)
						PocketUtil.Modify(steamPlayer.player);
			}
			else
				Logger.LogError($"[{Name}] Plugin: DISABLED");
			
			Logger.LogWarning($"[{Name}] Plugin loaded successfully!");
			Logger.LogWarning($"[{Name}] {Name} v{Major}.{Minor}.{Patch}");
			Logger.LogWarning($"[{Name}] Made with 'rice' by RiceField Plugins!");
		}
		protected override void Unload()
		{
			if (Conf.Enabled)
			{
				if (Conf.RevertOnUnload)
					PocketUtil.RevertPocket();
				
				U.Events.OnPlayerConnected -= PlayerEvent.OnConnected;
				PlayerLife.OnRevived_Global -= PlayerEvent.OnRevived;
				PlayerAnimator.OnGestureChanged_Global -= PlayerEvent.OnGestureChanged;
			}

			Inst = null;
			Conf = null;
			
			Logger.LogWarning($"[{Name}] Plugin unloaded successfully!");
		}
	}
}
