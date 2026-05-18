using Exiled.API.Interfaces;

namespace CuffBond
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public float MaxBindDistance { get; set; } = 1.55f;

        // Messages
        public string MsgNoWeapon { get; set; } = "<color=red>You must hold a weapon in your hands!</color>";

        // User Interface
        public string SssHeader { get; set; } = "BIND AN ALLY";
        public string SssKeyLabel { get; set; } = "Bind / Unbind player";
        public string SssKeyHint { get; set; } = "Aim at a nearby player and press. If already bound — unbinds.";
    }
}