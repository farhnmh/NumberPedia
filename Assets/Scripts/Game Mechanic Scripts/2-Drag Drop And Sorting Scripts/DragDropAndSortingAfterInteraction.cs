using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropAndSortingAfterInteraction : MonoBehaviour
{
    public bool isChosen;
    public bool isDone;
    public HandController hand;
    public DragDropAndSortingInGameManager gameManager;

    [Header("Drag Drop And Sorting After Interaction")]
    public int numberTarget;
    public float moveSpeed;
    public Animator animator;
    public GameObject otterIdle;
    public GameObject otterInteracted;
    public Transform logTarget;
    public Transform positionTemp;
    public Vector3 logTargetOffset;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isReady)
        {
            if (isChosen && !isDone)
            {
                gameManager.ScaleDownOtters(numberTarget);

                otterIdle.SetActive(false);
                otterInteracted.SetActive(true);
                transform.position = new Vector3(hand.transform.position.x,
                                                 hand.transform.position.y,
                                                 0);
                hand.isInteracting = false;
            }
        }
        else
            gameManager.ScaleUpOtters(numberTarget);

        if (isDone)
        {
            animator.enabled = false;
            logTarget.tag = "Untagged";
            transform.tag = "Untagged";
            transform.transform.parent = logTarget;
            transform.position = Vector3.MoveTowards(transform.position, logTarget.position - logTargetOffset, moveSpeed * Time.deltaTime);
        }
        else if (!isChosen)
        {
            otterIdle.SetActive(true);
            otterInteracted.SetActive(false);
            transform.position = Vector3.MoveTowards(transform.position, positionTemp.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            hand = collision.GetComponent<HandController>();
            isChosen = true;
        }

        if (collision.CompareTag("LogTarget"))
        {
            if (!gameManager.audioSource.isPlaying)
            {
                gameManager.isReady = false;
                hand.isInteracting = true;

                if (collision.name == $"Target Group Number {numberTarget}")
                {
                    isDone = true;
                    logTarget = collision.transform;
                    gameManager.audioSource.clip = gameManager.correctNumberSFX[numberTarget];
                    gameManager.audioSource.Play();
                }
                else
                {
                    isChosen = false;
                    gameManager.audioSource.clip = gameManager.wrongNumberSFX;
                    gameManager.audioSource.Play();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            hand = null;
            isChosen = false;
        }
    }
}
