using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BubbleNumbersGameManager : MonoBehaviour
{
    [System.Serializable]
    public struct NumberDetail{
        public int number;
        public GameObject numberBubble;
        public AudioClip audioClip;
    };

    public bool isPlay;
    public AudioSource audioSource;
    public GameObject gameOverPanel;
    public GameObject retryButton;
    public GameObject homeButton;

    [Header("Randomize Attributes")]
    public int numberChallenge;
    public GameObject numberTarget;
    public TextMeshProUGUI challengeText;
    public List<GameObject> numberHints;
    public List<NumberDetail> bubbleNumbers;

    [Header("Bubble Number Attributes")]
    public Vector2 minimumPos;
    public Vector2 maximumPos;
    public AudioClip notifWrong;
    public List<AudioClip> notifCorrect;

    // Start is called before the first frame update
    void Start()
    {
        RandomChallenge();
    }

    // Update is called once per frame
    void Update()
    {
        isPlay = !audioSource.isPlaying;
        if (bubbleNumbers.Count == 0)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.GetComponent<Animator>().SetTrigger("isScaleUp");
            retryButton.GetComponent<Button>().interactable =
                homeButton.GetComponent<Button>().interactable =
                !gameOverPanel.GetComponent<AudioSource>().isPlaying;

            challengeText.text = "";
            for (int i = 0; i < numberHints.Count; i++) numberHints[i].SetActive(false);
        }
    }

    public void RandomChallenge()
    {
        int targetTemp = Random.Range(0, bubbleNumbers.Count);
        for (int i = 0; i < bubbleNumbers.Count; i++)
        {
            bubbleNumbers[i].numberBubble.GetComponent<BoxCollider2D>().enabled = false;
            bubbleNumbers[i].numberBubble.GetComponent<GameObjectMoveRandomly>().glowObject.SetActive(false);

            if (i == targetTemp)
            {
                numberTarget = bubbleNumbers[i].numberBubble;
                bubbleNumbers[i].numberBubble.GetComponent<BoxCollider2D>().enabled = true;
                audioSource.PlayOneShot(bubbleNumbers[i].audioClip);

                challengeText.text = $"Ayo Cari Angka {bubbleNumbers[i].number}!!";
                for (int j = 0; j < numberHints.Count; j++)
                    numberHints[j].GetComponent<SpriteRenderer>().sprite = bubbleNumbers[i].numberBubble.GetComponent<GameObjectMoveRandomly>().numberObject.GetComponent<SpriteRenderer>().sprite;

                numberChallenge = bubbleNumbers[i].number;
                bubbleNumbers[i].numberBubble.GetComponent<GameObjectMoveRandomly>().glowObject.SetActive(true);
            }
        }
    }
}
