using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCountingInGameManager : MonoBehaviour
{
    public StageManager stageManager;
    public List<AudioClip> numberDetailSFX;
    public List<AudioClip> justNumberSFX;

    [Header("Counting And Drawing In Game Manager")]
    public int levelIndex;
    public int levelTotal;
    public float moveSpeed;
    public List<GameObject> levelGroup;

    // Start is called before the first frame update
    void Start()
    {
        levelTotal = levelGroup.Count;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levelGroup.Count; i++)
        {
            if (i + 1 < levelIndex)
            {
                levelGroup[i].GetComponent<Animator>().SetTrigger("isMovingOut");
                if (levelIndex > levelTotal && stageManager != null)
                {
                    gameObject.GetComponent<Animator>().SetTrigger("isMovingOut");
                    StartCoroutine(SetPanelFalse(gameObject));
                    stageManager.isPlay = false;
                    stageManager = null;
                }

                StartCoroutine(SetPanelFalse(levelGroup[i]));
            }
            else if (i + 1 == levelIndex)
            {
                levelGroup[i].SetActive(true);
                levelGroup[i].GetComponent<Animator>().SetTrigger("isMovingIn");
            }
        }
    }

    IEnumerator SetPanelFalse(GameObject panel)
    {
        yield return new WaitForSeconds(1f);
        panel.SetActive(false);
    }
}
