using System;
using Unity.Netcode;
using UnityEngine;

namespace Core.OnlineRuntimeModule.ActivitiesModule.Whiteboard
{
    public class WhiteboardController: NetworkBehaviour
    {
        private LineRenderer _render;

        [ServerRpc(RequireOwnership = false)]
        public void DrawAtPositionServerRpc(Vector3 v)
        {
            
        }

        [ClientRpc]
        public void DrawAtPositionClientRpc(Vector3 v)
        {
            
        }
    }
}