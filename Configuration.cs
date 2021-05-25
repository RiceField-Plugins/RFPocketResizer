using Rocket.API;

namespace RFPocketResizer
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Enabled;
        public string PermissionPrefix;
        public bool RevertOnUnload;

        public void LoadDefaults ()
        {
            Enabled = true;
            PermissionPrefix = "pocket";
            RevertOnUnload = true;
        }
    }
}