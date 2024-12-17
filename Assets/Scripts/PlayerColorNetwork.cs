using FishNet.Example.ColliderRollbacks;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorNetwork : NetworkBehaviour
{
    public GameObject body;
    public Color endColor;


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {

        }
        else
        {
            GetComponent<PlayerColorNetwork>().enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeColorServer(gameObject, endColor);
        }
    }

    [ServerRpc] // 서버에 rpc 보냄
    public void ChangeColorServer(GameObject player, Color color)
    {
        ChangeColor(player, color);
    }

    [ObserversRpc] // 옵저버들에게 rpc 보냄
    public void ChangeColor(GameObject player, Color color)
    {
        player.GetComponent<PlayerColorNetwork>().body.GetComponent<Renderer>().material.color = color;
    }
}
