using RFPocketResizer.EventListeners;
using RFPocketResizer.Utils;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;

namespace RFPocketResizer
{
    public class Plugin : RocketPlugin<Configuration>
    {
	    private static int Major = 1;
	    private static int Minor = 0;
	    private static int Patch = 0;
	    
		internal static Plugin Inst;
		internal static Configuration Conf;

		protected override void Load()
		{
			Inst = this;
			Conf = Configuration.Instance;

			if (Conf.Enabled)
			{
				PlayerAnimator.OnGestureChanged_Global += PlayerEvent.OnGestureChanged;
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
				
				PlayerAnimator.OnGestureChanged_Global -= PlayerEvent.OnGestureChanged;
			}

			Inst = null;
			Conf = null;
			
			Logger.LogWarning($"[{Name}] Plugin unloaded successfully!");
		}
	}
}
