using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public bool isPlay;
    public int levelIndex;
    public GameObject gameOverPanel;
    public GameObject retryButton;
    public GameObject homeButton;
    public List<GameObject> levelObjects;

    // Update is called once per frame
    void Update()
    {
        if (!isPlay)
        {
            levelObjects[levelIndex].SetActive(true);
            levelObjects[levelIndex].GetComponent<Animator>().SetTrigger("isMovingIn");
            levelIndex++;
            
            if (levelIndex == levelObjects.Count)
            {
                gameOverPanel.SetActive(true);
                gameOverPanel.GetComponent<Animator>().SetTrigger("isScaleUp");
            }

            isPlay = true;
        }
        
        retryButton.GetComponent<Button>().interactable =
                homeButton.GetComponent<Button>().interactable =
                !gameOverPanel.GetComponent<AudioSource>().isPlaying;
    }
}
