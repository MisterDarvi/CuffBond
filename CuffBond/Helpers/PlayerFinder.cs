using Exiled.API.Features;
using UnityEngine;

namespace CuffBond.Helpers
{
    public static class PlayerFinder
    {
        public static Player GetTarget(Player sender)
        {
            var maxDist = Plugin.Instance.Config.MaxBindDistance;
            var cam = sender.CameraTransform;
            var hits = Physics.RaycastAll(cam.position, cam.forward, maxDist);

            foreach (var hit in hits)
            {
                var hitbox = hit.collider.GetComponent<HitboxIdentity>();
                if (hitbox != null)
                {
                    var p = Validate(sender, hitbox.TargetHub, maxDist);
                    if (p != null) return p;
                    continue;
                }

                var hub = hit.collider.GetComponentInParent<ReferenceHub>();
                if (hub != null)
                {
                    var p = Validate(sender, hub, maxDist);
                    if (p != null) return p;
                }
            }

            return null;
        }

        private static Player Validate(Player sender, ReferenceHub hub, float maxDist)
        {
            if (hub == null || hub == sender.ReferenceHub || hub.isLocalPlayer)
                return null;

            if (hub.authManager.InstanceMode != CentralAuth.ClientInstanceMode.ReadyClient)
                return null;

            var p = Player.Get(hub);
            if (p == null || !p.IsAlive)
                return null;

            if (Vector3.Distance(sender.Position, p.Position) > maxDist)
                return null;

            return p;
        }
    }
}