using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    private readonly SyncVar<int> _health = new SyncVar<int>();
    public int health;

    private void Awake()
    {
        _health.OnChange += _health_OnChange;
    }

    private void _health_OnChange(int prev, int next, bool asServer)
    {
        health = next;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!base.IsOwner)
            GetComponent<PlayerHealth>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdateHealth(this, -1);
        }
    }


    [ServerRpc]
    public void UpdateHealth(PlayerHealth script, int amountToChange)
    {
        script._health.Value += amountToChange;
    }
}
