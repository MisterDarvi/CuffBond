using System;
using System.Collections.Generic;
using CuffBond.Core;
using CuffBond.Helpers;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UserSettings.ServerSpecific;

namespace CuffBond.Handlers
{
    public class PlayerHandlers
    {
        private static readonly HashSet<Team> _scpTeams = new HashSet<Team> { Team.SCPs };

        public void OnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase setting)
        {
            try
            {
                if (!(setting is SSKeybindSetting keybind) || !keybind.SyncIsPressed)
                    return;

                if (hub.isLocalPlayer)
                    return;

                if (hub.authManager.InstanceMode != CentralAuth.ClientInstanceMode.ReadyClient)
                    return;

                var player = Player.Get(hub);
                if (player == null || !player.IsAlive)
                    return;

                if (keybind.SettingId == Plugin.ActionKey)
                    HandleAction(player);
            }
            catch (Exception ex)
            {
                Log.Error($"[CuffBond] {ex}");
            }
        }

        private void HandleAction(Player actor)
        {
            var target = PlayerFinder.GetTarget(actor);
            if (target == null || !target.IsAlive)
                return;

            if (CuffManager.IsBound(target))
            {
                CuffManager.Unbind(target);
                return;
            }

            if (!(actor.CurrentItem is Firearm))
            {
                actor.Broadcast(3, Plugin.Instance.Config.MsgNoWeapon, Broadcast.BroadcastFlags.Normal, true);
                return;
            }

            if (_scpTeams.Contains(target.Role.Team))
                return;

            if (target.CurrentItem != null)
                return;

            CuffManager.Bind(target);
        }

        public void OnRemovingHandcuffs(RemovingHandcuffsEventArgs ev)
        {
            if (CuffManager.IsBound(ev.Target))
                ev.IsAllowed = false;
        }

        public void OnPlayerLeft(LeftEventArgs ev)
        {
            if (ev.Player?.UserId == null)
                return;

            CuffManager.RemoveOnLeave(ev.Player);
        }
    }
}