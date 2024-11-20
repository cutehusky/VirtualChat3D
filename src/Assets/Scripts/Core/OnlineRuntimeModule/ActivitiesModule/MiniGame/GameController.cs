using System.Collections.Generic;
using Unity.Netcode;

namespace Core.OnlineRuntimeModule.ActivitiesModule.MiniGame
{
    public class GameController: NetworkBehaviour
    {
        private GameView _gameView;

        public void OnInit(GameView view)
        {
            
        }
        public enum Card
        {
            A = 0,J,Q,Joker
        }

        public List<Card> currentPlayerCard;

        public NetworkVariable<bool> isStarted;

        public NetworkVariable<int> countOfPlayer;
        
        public NetworkVariable<int> currentPlayerIndex;

        public List<ulong> playerId;

        [ServerRpc]
        public void StartServerRpc()
        {
            
        }

        [ClientRpc]
        public void StartClientRpc(int[] cardList)
        {
            
        }

        [ClientRpc]
        public void StartCheckClientRpc()
        {
            
        }
        
        [ClientRpc]
        public void StartSendCardClientRpc()
        {
            
        }

        [ServerRpc]
        public void SendCardServerRpc(int card)
        {
            
        }
        
        [ServerRpc]
        public void JoinGameServerRpc()
        {
            
        }

        [ServerRpc]
        public void HostGameServerRpc()
        {
            
        }

        [ServerRpc]
        public void CheckCardServerRpc(bool isCheck)
        {
            
        }

        [ClientRpc]
        public void ShootClientRpc(bool isDead)
        {
            
        }

        [ClientRpc]
        public void WinClientRpc()
        {
            
        }
    }
}