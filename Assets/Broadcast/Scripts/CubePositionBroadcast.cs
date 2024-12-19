using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using System;

public class CubePositionBroadcast : MonoBehaviour
{
    public List<Transform> cubePositions = new List<Transform>();
    public int transformIndex;

    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<PositionIndex>(OnPositionBroadcast);
        InstanceFinder.ServerManager.RegisterBroadcast<PositionIndex>(OnClientPositionBroadcast);
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<PositionIndex>(OnPositionBroadcast);
        InstanceFinder.ServerManager.UnregisterBroadcast<PositionIndex>(OnClientPositionBroadcast);
    }

    //중요한친구들
    //InstanceFinder
    //InstanceFinder.ClientManager.RegisterBroadcast
    //InstanceFinder.ClientManager.UnregisterBroadcast
    //private void OnPositionBroadcast(PositionIndex indexStruct, Channel channel)
    //IBroadcast
    //host개념

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
                int nextIndex = transformIndex + 1;
                if (nextIndex >= cubePositions.Count)
                    nextIndex = 0;
            if (InstanceFinder.ServerManager != null && InstanceFinder.ServerManager.Started)
            {
                InstanceFinder.ServerManager.Broadcast(new PositionIndex() { tIndex = nextIndex });
            }
            else if (InstanceFinder.ClientManager != null && InstanceFinder.ClientManager.Started)
            {
                InstanceFinder.ClientManager.Broadcast(new PositionIndex() { tIndex = nextIndex });
            }
        }

        transform.position = cubePositions[transformIndex].position;
    }

    private void OnPositionBroadcast(PositionIndex indexStruct, Channel channel)
    {
        transformIndex = indexStruct.tIndex;
    }

    private void OnClientPositionBroadcast(NetworkConnection connection, PositionIndex indexStruct, Channel channel)
    {
        InstanceFinder.ServerManager.Broadcast(indexStruct);
    }

    public struct PositionIndex : IBroadcast
    {
        public int tIndex;
    }
}
