using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingNumberAfterInteraction : MonoBehaviour
{
    public bool isChosen;
    public bool isDone;
    public AudioSource audioSource;
    public CountingAndDrawingManager levelManager;
    public HandController hand;

    // Update is called once per frame
    void Update()
    {
        if (isChosen && !isDone)
        {
            if (hand.isReady)
            {
                levelManager.hint.SetActive(false);
                transform.position = hand.transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            hand = collision.GetComponent<HandController>();
            isChosen = true;
        }

        if (collision.CompareTag("Checkpoint"))
        {
            audioSource.Play();
            levelManager.checkpointObject[0].SetActive(false);
            levelManager.checkpointObject.RemoveAt(0);

            if (levelManager.checkpointObject.Count != 0)
                levelManager.checkpointObject[0].SetActive(true);
            else
            {
                levelManager.drawingGroupAnimator.SetTrigger("isDone");
                levelManager.PlayAudioByNumber(levelManager.numberTarget);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
            isChosen = false;
    }
}
