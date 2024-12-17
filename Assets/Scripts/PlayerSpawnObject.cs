using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
public class PlayerSpawnObject : NetworkBehaviour
{
    public GameObject objToSpawn;
    [HideInInspector] public GameObject spawnedObject;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!this.IsOwner)
            GetComponent<PlayerSpawnObject>().enabled = false;
    }

    private void Update()
    {
        if (spawnedObject == null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnObject(objToSpawn, transform, this);
        }
        if (spawnedObject != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            Despawn(spawnedObject);
        }
    }

    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script)
    {
        GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
        ServerManager.Spawn(spawned);
        SetSpawnedObject(spawned, script);
    }

    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObject script)
    {
        script.spawnedObject = spawned;
    }

    [ServerRpc]
    public void DespawnObject(GameObject spawned, PlayerSpawnObject script)
    {
        ServerManager.Despawn(spawned);
        SetSpawnedObject(spawned, script);
    }
}
