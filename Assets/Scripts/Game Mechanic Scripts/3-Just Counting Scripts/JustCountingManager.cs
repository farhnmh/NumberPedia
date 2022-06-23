using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCountingManager : MonoBehaviour
{
    public bool isZero;
    public bool isPlay;
    public bool isDone;
    public int numberIndex;
    public int numberTarget;
    public JustCountingInGameManager gameManager;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameObject.transform.parent.GetComponent<JustCountingInGameManager>();
        if (!isZero) numberTarget = gameObject.transform.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberTarget == numberIndex && !isZero && !isDone)
        {
            gameManager.GetComponent<AudioSource>().clip = gameManager.numberDetailSFX[numberTarget];
            gameManager.GetComponent<AudioSource>().Play();

            StartCoroutine(AudioFinishChecker());
            isDone = true;
        }
    }

    public IEnumerator AudioFinishChecker()
    {
        yield return new WaitUntil(() => gameManager.GetComponent<AudioSource>().isPlaying == false);
        gameManager.levelIndex++;
    }
}
