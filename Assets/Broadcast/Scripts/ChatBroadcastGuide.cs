using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBroadcastGuide : MonoBehaviour
{
    public Transform chatHolder;
    public GameObject msgElement;
    public TMP_InputField playerUsername, playerMessage;

    private void OnEnable()
    {
        InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageReceive);
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnClientMessageReceive);

    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.UnregisterBroadcast<Message>(OnMessageReceive);
        InstanceFinder.ServerManager.UnregisterBroadcast<Message>(OnClientMessageReceive);

    }

    private void OnMessageReceive(Message message, Channel channel)
    {
        var go = Instantiate(msgElement, chatHolder);
        go.GetComponent<TMP_Text>().text = $"{message.username} : {message.message}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Message message = new Message() { username = playerUsername.text, message = playerMessage.text };
            if (InstanceFinder.ServerManager != null && InstanceFinder.ServerManager.Started)
            {
                InstanceFinder.ServerManager.Broadcast(message);
            }
            else if(InstanceFinder.ClientManager != null && InstanceFinder.ClientManager.Started)
            {
                InstanceFinder.ClientManager.Broadcast(message);
            }
        }
    }

    private void OnClientMessageReceive(NetworkConnection connection, Message message, Channel channel)
    {
        InstanceFinder.ServerManager.Broadcast(message);
    }


    public struct Message : IBroadcast
    {
        public string username;
        public string message;
    }
}
