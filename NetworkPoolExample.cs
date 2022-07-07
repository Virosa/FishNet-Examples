using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;

/// <summary>
/// An example pattern for pooling Networked Objects without Despawning/Respawning, in FishNet.
/// </summary>
public class NetworkPoolExample : MonoBehaviour
{ 
    public List<NetworkThing> RecycledThings = new List<NetworkThing>();

    public void Recycle(NetworkThing obj)
    {
        obj.SendDeactivate();
        obj.gameObject.SetActive(false);
        RecycledThings.Add(obj);
    }

    public NetworkThing GetThing()
    {
        // Reuse if possible
        if (RecycledThings.Count > 0)
        {
            var thing = RecycledThings[0];
            RecycledThings.RemoveAt(0);
            return thing;
        }

        // Otherwise spawn a new thing
        var newThing = Instantiate(new NetworkThing());
        InstanceFinder.ServerManager.Spawn(newThing.gameObject);

        return newThing;
    }

    // Send All data needed to Re/active.
    // Note: This imply all data is contained here to active this Thing regardless of whether it is new or recycled.
    private void Reactivate()
    {
        var thing = GetThing();
        var resetData = new ThingResetData()
        {
            position = Vector3.zero, // Re/Spawn location
            rotation = Quaternion.identity // Re/Spawn rotation
        };
        GetThing().SendReactivate(resetData);
    }
}

public class NetworkThing : NetworkBehaviour
{
    [ObserversRpc]
    public void SendDeactivate()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Must contain all needed data needed to Re/Start the "Thing".
    /// </summary>
    /// <param name="thingResetData"></param>
    [ObserversRpc]
    public void SendReactivate(ThingResetData thingResetData)
    {
        // Note: Do your Cleanup() here, as Awake/Start/OnStartClient/OnStartServer/etc will not be called.
                
        // Apply all reset data.
        transform.position = thingResetData.position;
        transform.rotation = thingResetData.rotation;
                
        gameObject.SetActive(true);
    }
}

/// <summary>
/// Whatever data needed to re/use this simple networked thing.
/// </summary>
public struct ThingResetData
{
    public Vector3 position;
    public Quaternion rotation;
}
