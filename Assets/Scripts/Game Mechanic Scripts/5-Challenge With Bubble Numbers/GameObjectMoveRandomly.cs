using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMoveRandomly : MonoBehaviour
{
    public bool isChosen;
    public BubbleNumbersGameManager gameManager;
    public AnimationClip bubbleSplash;
    public GameObject numberObject;
    public GameObject glowObject;
    public Vector3 targetPos;
    public float moveSpeed;
    public float waitDestroy;

    void Start()
    {
        gameManager = transform.parent.GetComponent<BubbleNumbersGameManager>();
        InitializeRandomData();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, targetPos) <= 0.01f)
            InitializeRandomData();

        if (isChosen && gameManager.numberTarget == gameObject)
        {
            if (!gameObject.GetComponent<AudioSource>().isPlaying)
                gameObject.GetComponent<AudioSource>().Play();
            
            gameObject.GetComponent<Animator>().SetBool("isSplash", true);
            waitDestroy = bubbleSplash.length;

            StartCoroutine(WaitForDestroy());
        }
    }

    void InitializeRandomData()
    {
        float randomX = Random.Range(gameManager.minimumPos.x, gameManager.maximumPos.x);
        float randomY = Random.Range(gameManager.minimumPos.y, gameManager.maximumPos.y);
        targetPos = new Vector3(randomX, randomY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
            if (gameManager.isPlay)
                isChosen = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
            isChosen = false;
    }
    
    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(waitDestroy);

        for (int i = 0; i < gameManager.bubbleNumbers.Count; i++)
        {
            if (gameObject == gameManager.bubbleNumbers[i].numberBubble)
            {
                gameManager.bubbleNumbers.RemoveAt(i);
                gameManager.RandomChallenge();

                gameObject.SetActive(false);
            }
        }
    }
}
