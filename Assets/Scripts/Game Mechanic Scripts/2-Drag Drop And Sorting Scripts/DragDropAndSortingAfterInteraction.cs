using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropAndSortingAfterInteraction : MonoBehaviour
{
    public bool isChosen;
    public bool isDone;
    public HandController hand;
    public DragDropAndSortingInGameManager gameManager;
    public AudioSource audioSource;

    [Header("Drag Drop And Sorting After Interaction")]
    public int numberTarget;
    public float moveSpeed;
    public Animator animator;
    public GameObject otterIdle;
    public GameObject otterInteracted;
    public Transform logTarget;
    
    public Vector3 logTargetOffset;
    public Vector3 positionTemp;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background Script").GetComponent<DragDropAndSortingInGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isReady)
        {
            if (isChosen && !isDone)
            {
                if (hand.isReady)
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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(positionTemp.x, positionTemp.y, 0), moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            hand = collision.GetComponent<HandController>();
            if (hand.isInteracting)
                isChosen = true;
        }

        if (collision.CompareTag("LogTarget"))
        {
            if (!audioSource.isPlaying)
            {
                gameManager.isReady = false;
                hand.isInteracting = true;

                if (collision.name == $"Target Group Number {numberTarget}")
                {
                    isDone = true;
                    logTarget = collision.transform;
                    audioSource.clip = gameManager.correctNumberSFX[numberTarget];
                    audioSource.Play();
                }
                else
                {
                    isChosen = false;
                    audioSource.clip = gameManager.wrongNumberSFX;
                    audioSource.Play();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            hand = collision.GetComponent<HandController>();
            if (hand.isInteracting)
                isChosen = false;
        }
    }
}
