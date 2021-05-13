using Rocket.API;

namespace PocketResizer
{
    public class Configuration : IRocketPluginConfiguration
    {
        public string PermissionPrefix;
        public bool RevertOnUnload;

        public void LoadDefaults ()
        {
            PermissionPrefix = "pocket";
            RevertOnUnload = true;
        }
    }
}