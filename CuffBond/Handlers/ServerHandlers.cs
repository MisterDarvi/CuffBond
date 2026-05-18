using CuffBond.Core;
using Exiled.Events.EventArgs.Server;

namespace CuffBond.Handlers
{
    public class ServerHandlers
    {
        public void OnRoundEnded(RoundEndedEventArgs ev) => CuffManager.Clear();
    }
}