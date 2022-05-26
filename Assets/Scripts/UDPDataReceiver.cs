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
    public int portNum;
    public string packetReceived;
    public ControlManager controlManager;
    public TextMeshProUGUI detailText;

    string notification;
    static Thread receiverThread;
    static UdpClient udpClient;

    void Awake()
    {
        controlManager = GameObject.Find("Background Script").GetComponent<ControlManager>();
        udpClient = new UdpClient(portNum);

        receiverThread = new Thread(UDPReceiver);
        receiverThread.Start();
    }

    void Start()
    {
        
    }

    void Update()
    {
        detailText.text = notification;
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        receiverThread.Abort();
        print("Aborted");
    }
#endif

    void UDPReceiver()
    {
        notification = "Receiver Thread Opened";
        while (true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udpClient.Receive(ref remoteEP);
            packetReceived = Encoding.ASCII.GetString(data);

            if (packetReceived != "")
            {
                controlManager.dataSplitted = packetReceived.Split(',');
                controlManager.totalHand = Convert.ToInt32(controlManager.dataSplitted[0]);
            }
            else
                controlManager.totalHand = 0;
        }
    }
}
