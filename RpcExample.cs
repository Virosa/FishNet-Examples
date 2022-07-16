using FishNet.Object;
using UnityEngine;

/// <summary>
/// You may copy-paste this class ad verbatim. Add to a valid GameObject, then hit play.
/// 
/// Official Docs for Remote Procedure Calls:
/// https://fish-networking.gitbook.io/docs/manual/guides/remote-procedure-calls
/// </summary>
public class RpcExample : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        Debug.Log($"RpcExample OnStartClient. Client sending Test RPC.");
        ClientTestRpc();
    }

    [ServerRpc(RequireOwnership = false)] // Note: This allow any client from calling this. You often do not wish to do this.
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
