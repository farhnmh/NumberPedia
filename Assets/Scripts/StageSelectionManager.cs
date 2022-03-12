using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StageSelectionManager : MonoBehaviour
{
    public UserManager userManager;
    public DatabaseManager dbManager;

    public List<GameObject> stagePanel;
    public List<GameObject> stageButton;
    public List<GameObject> levelButton;

    [Header("Image Attribute")]
    public List<Sprite> stageButtonOn;
    public List<Sprite> levelButtonOn;
    public List<Sprite> stageButtonOff;
    public List<Sprite> levelButtonOff;

    int stage;
    int level;

    // Start is called before the first frame update
    void Start()
    {
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
        dbManager = GameObject.Find("ImportantHandler").GetComponent<DatabaseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        userManager.currentlyPlayingDetail.stage = stage;
        userManager.currentlyPlayingDetail.level = level;
    }

    public void UpdateCheckpoint()
    {
        for (int i = 1; i <= stageButton.Count; i++)
        {
            if (stageButton[i].name == $"Stage{i}Button")
            {
                stageButton[i].GetComponent<Button>().interactable = true;
                stageButton[i].GetComponent<Image>().sprite = stageButtonOn[i];
            }
            else
            {
                stageButton[i].GetComponent<Button>().interactable = false;
                stageButton[i].GetComponent<Image>().sprite = stageButtonOff[i];
            }

            
        }
    }

    public void ChooseStage(int stageIndex)
    {
        stage = stageIndex;
        for (int i = 0; i < 3; i++)
        {
            if (i == stageIndex - 1)
                stagePanel[i].SetActive(true);
            else
                stagePanel[i].SetActive(false);
        }
    }

    public void ChooseLevel(int levelIndex)
    {
        level = levelIndex;
        Common.CommonFunction.MoveToScene($"3-Windows-Level{level}");
    }
}
