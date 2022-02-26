using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using UnityEngine;
using System.Threading;

public class HandPositionReceiver : MonoBehaviour
{
    public int port;
    public string dataString;
    public string[] rightHand;

    byte[] receivedData;

    IPEndPoint ipep;
    IPEndPoint sender;
    UdpClient newSock;
    Thread networkThread;

    void Start()
    {
        networkThread = new Thread(ReceiveData);
        networkThread.Start();

        receivedData = new byte[0];

        ipep = new IPEndPoint(IPAddress.Any, port);
        newSock = new UdpClient(ipep);
        sender = new IPEndPoint(IPAddress.Any, 0);
    }

    void Update()
    {
        ReceiveData();
    }

    public void ReceiveData()
    {
        if (newSock.Available > 0)
        {
            receivedData = newSock.Receive(ref sender);
            dataString = Encoding.ASCII.GetString(receivedData);
            rightHand = dataString.Split(',');
        }
        else
        {
            //print("No data to receive");
        }
    }
}
