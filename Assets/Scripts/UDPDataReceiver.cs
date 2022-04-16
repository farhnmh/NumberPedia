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
    public int portNum = 22222;
    public bool isTracking;
    public string packetReceived;
    public ControlManager controlManager;
    public TextMeshProUGUI detailText;

    static Thread receiverThread;
    static UdpClient udpClient;

    private void Start()
    {
        controlManager = GameObject.Find("Background Script").GetComponent<ControlManager>();
        udpClient = new UdpClient(portNum);

        if (isTracking)
        {
            receiverThread = new Thread(UDPReceiver);
            receiverThread.Start();
        }
    }

    void UDPReceiver()
    {
        detailText.text = "Receiver Thread Opened";
        while (true)
        {
            if (isTracking)
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
}
