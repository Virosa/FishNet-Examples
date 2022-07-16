using FishNet.Object;
using UnityEngine;

public class RpcExample : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        Debug.Log($"RpcExample OnStartClient. Client sending Test RPC.");
        ClientTestRpc();
    }

    [ServerRpc]
    private void ClientTestRpc()
    {
        Debug.Log($"Server Received a Test RPC from {OwnerId}.");
        ServerTestRpc();
    }

    [ObserversRpc]
    private void ServerTestRpc()
    {
        Debug.Log($"Client Received a Test RPC from Server.");
    }
}
