using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPositionSync : MonoBehaviour
{
    public HandPositionReceiver hands;
    public bool isTracking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracking)
            HandTracking();
        else
            MouseTracking();
    }

    void HandTracking()
    {
        //float maxX = 0.0140625f;
        //float maxY = -0.01388888888f;

        //float posX = Convert.ToInt32(hands.rightHand[0]) * maxX;
        //float posY = Convert.ToInt32(hands.rightHand[1]) * maxY;
        //transform.position = new Vector2(posX, posY);
    }

    void MouseTracking()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        transform.position = mousePosition;
    }
}
