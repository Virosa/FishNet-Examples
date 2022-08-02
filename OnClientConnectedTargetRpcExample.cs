using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Virosa.Examples {
    public class OnClientConnectedTargetRpcExample : NetworkBehaviour
    {
        private static int playerNumber;

        [SerializeField] 
        private NetworkObject playerPrefab;

        private ExamplePlayer player1;
        private ExamplePlayer player2;
        
        // This is only to show you can a get a client's NetworkConnection from its Object.
        private NetworkConnection player1Connection => player1.Owner; // Will fail if player1 is null/unset.
        
        private void Awake()
        {
            InstanceFinder.SceneManager.OnClientLoadedStartScenes += OnClientReady;
        }

        private void OnClientReady(NetworkConnection connection, bool asServer)
        {
            if (!asServer) return;

            SpawnAndRegisterPlayer(connection);
        }
        
        private void SpawnAndRegisterPlayer(NetworkConnection connection)
        {
            var nextPlayerNumber = GetNextPlayerNumber();

            NetworkObject newPlayerNetworkObject = Instantiate(playerPrefab);
            ExamplePlayer newPlayer = newPlayerNetworkObject.GetComponent<ExamplePlayer>();

            // Set the player's number before Spawning.
            newPlayer.playerNumber = nextPlayerNumber;
            
            Spawn(newPlayerNetworkObject.gameObject);
            
            // In a typical "Match" class, you often want to keep a reference to the player's "main" object. You may want to use an array instead.
            switch (playerNumber)
            {
                case 1:
                    player1 = newPlayer;
                    break;
                
                case 2:
                    player2 = newPlayer;
                    break;
            }
        }

        private int GetNextPlayerNumber()
        {
            return ++playerNumber;
        }
    }

    public class ExamplePlayer : NetworkBehaviour
    {
        // If set on the Server before Spawning, is Guaranteed to be available by OnStartClient()
        [SyncVar] 
        public int playerNumber;

        public override void OnStartClient()
        {
            base.OnStartClient();

            switch (playerNumber)
            {
                case 1:
                case 2:
                    Debug.Log($"Yay we can play! We are Player {playerNumber}!");
                    break;
                
                default:
                    Debug.LogWarning($"We can't play, we are Player {playerNumber}. :(");
                    break;
            }
        }
    }
}
