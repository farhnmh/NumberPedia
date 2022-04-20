using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingAndDrawingManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource audioSource;
    public bool isReady;

    [Header("Counting Attribute")]
    public bool isCounting;
    public int numberTarget;
    public int numberIndex;
    public float delaySpeed;
    public Vector3 scaleFactor;
    public GameObject countingGroup;

    [Header("Drawing Attribute")]
    public bool isDrawing;
    public GameObject hint;
    public GameObject drawingGroup;
    public Animator drawingGroupAnimator;
    public List<GameObject> checkpointObject;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background Script").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAudioByNumber(int number)
    {
        audioSource.clip = gameManager.numberSFX[number];
        audioSource.Play();

        StartCoroutine(AudioFinishChecker());
    }

    public IEnumerator AudioFinishChecker()
    {
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        gameManager.levelIndex++;
    }
}
