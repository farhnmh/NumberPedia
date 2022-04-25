using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public int totalHand;
    public float maxX = 0.0140625f;
    public float maxY = -0.01388888888f;
    public float loadingSpeed;
    public bool isTracking;

    [Header("Position Attribute")]
    public GameObject cursor;
    public GameObject handGroup;
    public GameObject[] handObject;
    public Vector2[] positionHands;
    public string[] dataSplitted;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTracking)
        {
            cursor.SetActive(false);
            handGroup.SetActive(true);

            PositionSyncronizer();
            HandAttibuteSetter();
        }
        else
        {
            cursor.SetActive(true);
            handGroup.SetActive(false);

            MouseTracking();
        }
    }

    void MouseTracking()
    {
        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                       Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                                       Mathf.Clamp01(0));
        cursor.transform.position = mousePos;
    }

    void HandAttibuteSetter()
    {
        for (int i = 0; i < handObject.Length; i++)
        {
            if (i < totalHand)
            {
                handObject[i].SetActive(true);
                handObject[i].transform.position = positionHands[i];
            }
            else
            {
                handObject[i].SetActive(false);
            }
        }
    }

    void PositionSyncronizer()
    {
        if (totalHand == 4)
        {
            positionHands[0].x = Convert.ToInt32(dataSplitted[1]) * maxX;
            positionHands[0].y = Convert.ToInt32(dataSplitted[2]) * maxY;

            positionHands[1].x = Convert.ToInt32(dataSplitted[3]) * maxX;
            positionHands[1].y = Convert.ToInt32(dataSplitted[4]) * maxY;

            positionHands[2].x = Convert.ToInt32(dataSplitted[5]) * maxX;
            positionHands[2].y = Convert.ToInt32(dataSplitted[6]) * maxY;

            positionHands[3].x = Convert.ToInt32(dataSplitted[7]) * maxX;
            positionHands[3].y = Convert.ToInt32(dataSplitted[8]) * maxY;
        }
        else if (totalHand == 3)
        {
            positionHands[0].x = Convert.ToInt32(dataSplitted[1]) * maxX;
            positionHands[0].y = Convert.ToInt32(dataSplitted[2]) * maxY;

            positionHands[1].x = Convert.ToInt32(dataSplitted[3]) * maxX;
            positionHands[1].y = Convert.ToInt32(dataSplitted[4]) * maxY;

            positionHands[2].x = Convert.ToInt32(dataSplitted[5]) * maxX;
            positionHands[2].y = Convert.ToInt32(dataSplitted[6]) * maxY;
        }
        else if (totalHand == 2)
        {
            positionHands[0].x = Convert.ToInt32(dataSplitted[1]) * maxX;
            positionHands[0].y = Convert.ToInt32(dataSplitted[2]) * maxY;

            positionHands[1].x = Convert.ToInt32(dataSplitted[3]) * maxX;
            positionHands[1].y = Convert.ToInt32(dataSplitted[4]) * maxY;
        }
        else if (totalHand == 1)
        {
            positionHands[0].x = Convert.ToInt32(dataSplitted[1]) * maxX;
            positionHands[0].y = Convert.ToInt32(dataSplitted[2]) * maxY;
        }
    }
}
