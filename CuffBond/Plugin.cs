using System;
using System.Collections.Generic;
using CuffBond.Core;
using CuffBond.Handlers;
using Exiled.API.Features;
using UserSettings.ServerSpecific;

namespace CuffBond
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }

        public override string Name => "CuffBond";
        public override string Author => "MrDarvi";
        public override string Prefix => "CuffBond";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 0, 0);

        internal const int HeaderId = 99900;
        internal const int ActionKey = 99901;

        private PlayerHandlers _playerHandlers;
        private ServerHandlers _serverHandlers;

        public override void OnEnabled()
        {
            Instance = this;

            _playerHandlers = new PlayerHandlers();
            _serverHandlers = new ServerHandlers();

            AppendSettings();

            ServerSpecificSettingsSync.ServerOnSettingValueReceived += _playerHandlers.OnSettingValueReceived;
            Exiled.Events.Handlers.Player.Left += _playerHandlers.OnPlayerLeft;
            Exiled.Events.Handlers.Player.RemovingHandcuffs += _playerHandlers.OnRemovingHandcuffs;
            Exiled.Events.Handlers.Server.RoundEnded += _serverHandlers.OnRoundEnded;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= _playerHandlers.OnSettingValueReceived;
            Exiled.Events.Handlers.Player.Left -= _playerHandlers.OnPlayerLeft;
            Exiled.Events.Handlers.Player.RemovingHandcuffs -= _playerHandlers.OnRemovingHandcuffs;
            Exiled.Events.Handlers.Server.RoundEnded -= _serverHandlers.OnRoundEnded;

            CuffManager.Clear();

            _playerHandlers = null;
            _serverHandlers = null;
            Instance = null;

            base.OnDisabled();
        }

        private void AppendSettings()
        {
            var existing = ServerSpecificSettingsSync.DefinedSettings
                ?? new ServerSpecificSettingBase[0];

            var settings = new List<ServerSpecificSettingBase>();

            foreach (var s in existing)
            {
                if (s.SettingId == HeaderId || s.SettingId == ActionKey)
                    continue;
                settings.Add(s);
            }

            settings.Add(new SSGroupHeader(HeaderId, Config.SssHeader));
            settings.Add(new SSKeybindSetting(ActionKey, Config.SssKeyLabel, hint: Config.SssKeyHint));

            ServerSpecificSettingsSync.DefinedSettings = settings.ToArray();
        }
    }
}