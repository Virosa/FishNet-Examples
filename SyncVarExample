using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Virosa {
    /// <summary>
    /// You may copy-paste this class ad verbatim. Add to a valid GameObject, then hit play.
    /// 
    /// Official Documentation for Sync Vars:
    /// https://fish-networking.gitbook.io/docs/manual/guides/synchronizing/syncvar
    /// </summary>
    public class SyncVarExample : NetworkBehaviour
    {
        [SyncVar(OnChange = nameof(OnIntegerChange))]
        public int integer;

        private void OnIntegerChange(int prev, int next, bool asServer)
        {
            Debug.Log($"Received Integer Change {prev} -> {next}, server? {asServer}");
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            integer = 5;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log($"OnStartClient SyncVarExample value: {integer}"); // Value = 5
            CmdChangeInteger(42);
        }

        /// <summary>
        /// Client command Server to set "integer" SyncVar to i.
        /// </summary>
        /// <param name="i"></param>
        [ServerRpc]
        private void CmdChangeInteger(int i)
        {
            integer = i;
        }
    }
}
