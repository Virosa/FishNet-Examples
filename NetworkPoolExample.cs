using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;

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
            var resetData = new ResetData()
            {
                position = Vector3.zero, // Re/Spawn location
                rotation = Quaternion.identity // Re/Spawn rotation
            };
            GetThing().SendReactivate(resetData);
        }

        public class NetworkThing : NetworkBehaviour
        {
            [ObserversRpc]
            public void SendDeactivate()
            {
                gameObject.SetActive(false);
            }

            /// <summary>
            /// Must contain
            /// </summary>
            /// <param name="data"></param>
            [ObserversRpc]
            public void SendReactivate(ResetData data)
            {
                transform.position = data.position;
                transform.rotation = data.rotation;

                // Todo: Do your Cleanup(), as Awake/Start/OnStartClient/OnStartServer/etc will not be called.

                gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Whatever data needed to re/use this simple networked thing.
        /// </summary>
        public struct ResetData
        {
            public Vector3 position;
            public Quaternion rotation;
        }
    }
