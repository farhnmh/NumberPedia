using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCountingManager : MonoBehaviour
{
    public bool isPlay;
    public JustCountingInGameManager gameManager;
    public AudioSource audioSource;
    public bool isDone;

    [Header("Counting Attribute")]
    public int numberTarget;
    public int numberIndex;
    public float delaySpeed;
    public Vector3 scaleFactor;
    public GameObject countingGroup;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background Script").GetComponent<JustCountingInGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numberTarget == numberIndex && !isDone)
        {
            audioSource.clip = gameManager.numberSFX[numberTarget];
            audioSource.Play();

            StartCoroutine(AudioFinishChecker());
            isDone = true;
        }
    }

    public IEnumerator AudioFinishChecker()
    {
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        gameManager.levelIndex++;
    }
}
