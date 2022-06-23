using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UDPDataReceiver : MonoBehaviour
{
    public bool isAlive;
    public int portNum;
    public ControlManager controlManager;
    public static UDPDataReceiver Instance;

    string packetReceived;
    string notification;
    static Thread receiverThread;
    static UdpClient udpClient;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        udpClient = new UdpClient(portNum);
        receiverThread = new Thread(UDPReceiver);
        receiverThread.Start();
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        udpClient.Close();
        receiverThread.Abort();
        isAlive = false;
    }
#endif

    void UDPReceiver()
    {
        notification = "Receiver Thread Opened";
        while (isAlive)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udpClient.Receive(ref remoteEP);
            packetReceived = Encoding.ASCII.GetString(data);

            if (controlManager != null)
            {
                if (packetReceived != "")
                {
                    controlManager.dataSplitted = packetReceived.Split(',');
                    controlManager.totalHand = Convert.ToInt32(controlManager.dataSplitted[0]);
                }
                else
                    controlManager.totalHand = 0;
            } 
        }
        udpClient.Close();
    }
}
