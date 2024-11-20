using System;
using System.Collections.Generic;
using Core.MVC;
using Unity.Netcode;

namespace Core.OnlineRuntimeModule.ActivitiesModule.Voting
{
    public class VotingController: NetworkBehaviour
    {
        [ServerRpc]
        public void HostVotingServerRpc()
        {
            
        }

        [ClientRpc]
        public void StartVotingClientRpc()
        {
            
        }

        [ServerRpc]
        public void JoinVotingServerRpc(bool choose)
        {
            
        }

        [ServerRpc]
        public void SendResultServerRpc()
        {
            
        }
        
        [ClientRpc]
        public void SendResultClientRpc()
        {
            
        }

        private VotingView _votingView;
        public void OnInit(VotingView view)
        {
            
        }
    }
}