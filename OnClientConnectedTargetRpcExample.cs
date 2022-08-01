using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

public class OnClientConnectedTargetRpcExample : NetworkBehaviour
    {
        private static int playerNumber;

        // I added these, because inside a "Match" instance, you would probably do something like this.
        private NetworkConnection player1Connection;
        private NetworkConnection player2Connection;
        
        private void Awake()
        {
            InstanceFinder.ServerManager.OnRemoteConnectionState += OnClientConnectionState;
        }
        
        private void OnClientConnectionState(NetworkConnection connection, RemoteConnectionStateArgs arguments)
        {
            if (arguments.ConnectionState == RemoteConnectionState.Started) RegisterPlayer(connection);
        }

        private void RegisterPlayer(NetworkConnection connection)
        {
            var nextPlayerNumber = GetNextPlayerNumber();

            switch (nextPlayerNumber)
            {
                case 1:
                    player1Connection = connection;
                    SendPlayerNumber(connection, 1);
                    break;
                
                case 2:
                    player2Connection = connection;
                    SendPlayerNumber(connection, 2);
                    break;
                
                default:
                    SendNoGameForYou(connection);
                    break;
            }
        }

        private int GetNextPlayerNumber()
        {
            return ++playerNumber;
        }

        /// <summary>
        /// A Broadcast would work better, especially since this assume the client is an Observer of this NetworkObject.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="number"></param>
        [TargetRpc]
        private void SendPlayerNumber(NetworkConnection target, int number)
        {
            Debug.Log($"Server sent us our Player Number: {number}.");
        }

        [TargetRpc]
        private void SendNoGameForYou(NetworkConnection target)
        {
            Debug.Log($"From Server: Computer says noooo.");
        }
    }
