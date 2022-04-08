using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPDataReceiver : MonoBehaviour
{
    public int portNum = 22222;
    public bool isTracking;
    public string packetReceived;
    public GameManager gameManager;

    static Thread receiverThread;
    static UdpClient udpClient;

    private void Start()
    {
        gameManager = GameObject.Find("Background Script").GetComponent<GameManager>();
        udpClient = new UdpClient(portNum);

        if (isTracking)
        {
            receiverThread = new Thread(UDPReceiver);
            receiverThread.Start();
        }
    }

    void UDPReceiver()
    {
        print("Receiver Thread Opened");
        while (true)
        {
            if (isTracking)
            {
                IPEndPoint remoteEP = null;
                byte[] data = udpClient.Receive(ref remoteEP);
                packetReceived = Encoding.ASCII.GetString(data);

                if (packetReceived != "")
                {
                    gameManager.dataSplitted = packetReceived.Split(',');
                    gameManager.totalHand = Convert.ToInt32(gameManager.dataSplitted[0]);
                }
                else
                    gameManager.totalHand = 0;
            }
        }
    }
}
