using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCountingAfterInteraction : MonoBehaviour
{
    public bool isZero;
    public bool isChosen;
    public bool isDone;
    public JustCountingInGameManager gameManager;
    public JustCountingManager levelManager;

    [Header("Counting Number After Interaction")]
    public int number;
    public Animator animator;
    public List<GameObject> objectInteract;
    public List<GameObject> numberTarget;

    // Update is called once per frame
    void Update()
    {
        if (levelManager.numberTarget == levelManager.numberIndex)
            animator.SetTrigger("isDone");
        if (isChosen)
        {
            if (!isZero) number = levelManager.numberIndex + 1;
            else { number = levelManager.numberIndex = 0; levelManager.isZero = false; }

            objectInteract[0].SetActive(false);
            objectInteract[1].SetActive(true);

            animator.SetTrigger("isReady");
            numberTarget[number].SetActive(true);

            if (!isDone)
            {
                StartCoroutine(LevelDone());
                isChosen = false;
                isDone = true;
            }
        }
    }

    IEnumerator LevelDone()
    {
        if (levelManager.numberIndex != levelManager.numberTarget - 1) 
        {
            levelManager.audioSource.clip = gameManager.justNumberSFX[number];
            levelManager.audioSource.Play();
        }

        yield return new WaitUntil(() => !levelManager.audioSource.isPlaying);

        levelManager.numberIndex++;
        levelManager.audioSource.clip = null;
        levelManager.isPlay = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand") && levelManager.isPlay && !isDone)
        {
            levelManager.isPlay = false;
            isChosen = true;
        }
    }
}
