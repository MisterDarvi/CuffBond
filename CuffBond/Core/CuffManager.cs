using Exiled.API.Features;
using InventorySystem.Disarming;
using Mirror;
using System.Collections.Generic;

namespace CuffBond.Core
{
    public static class CuffManager
    {
        private static readonly HashSet<string> _bound = new HashSet<string>();

        public static bool IsBound(Player player) => _bound.Contains(player.UserId);

        public static void Bind(Player target)
        {
            if (_bound.Contains(target.UserId))
                return;

            DisarmedPlayers.Entries.Add(new DisarmedPlayers.DisarmedEntry(target.ReferenceHub.netId, 0u));
            Sync();

            _bound.Add(target.UserId);
        }

        public static void Unbind(Player target)
        {
            if (!_bound.Contains(target.UserId))
                return;

            DisarmedPlayers.Entries.RemoveAll(e => e.DisarmedPlayer == target.ReferenceHub.netId);
            Sync();

            _bound.Remove(target.UserId);
        }

        public static void RemoveOnLeave(Player player)
        {
            if (!_bound.Contains(player.UserId))
                return;

            DisarmedPlayers.Entries.RemoveAll(e => e.DisarmedPlayer == player.ReferenceHub.netId);
            Sync();

            _bound.Remove(player.UserId);
        }

        public static void Clear() => _bound.Clear();

        private static void Sync() =>
            NetworkServer.SendToAll(new DisarmedPlayersListMessage(DisarmedPlayers.Entries));
    }
}