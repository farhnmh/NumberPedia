using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StageSelectionManager : MonoBehaviour
{
    public UserManager userManager;
    public DatabaseManager dbManager;

    public GameObject levelSelectionPanel;
    public GameObject levelSelection;
    public List<GameObject> stageButton;
    public List<GameObject> levelButton;

    [Header("Image Attribute")]
    public List<Sprite> levelSelectionSprite;
    public List<Sprite> stageButtonOn;
    public List<Sprite> levelButtonOn;
    public List<Sprite> stageButtonOff;
    public List<Sprite> levelButtonOff;

    [Header("Character Attribute")]
    public float charMoveSpeed;
    public GameObject character;
    public List<Transform> charTargetPosition;

    int checkpointStage;
    int checkpointLevel;

    int stage = 1;
    int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
        dbManager = GameObject.Find("ImportantHandler").GetComponent<DatabaseManager>();

        UpdateStageSelection();
    }

    // Update is called once per frame
    void Update()
    {
        userManager.currentlyPlayingDetail.stage = stage;
        userManager.currentlyPlayingDetail.level = level;
        checkpointStage = userManager.checkpointDetail.stage;
        checkpointLevel = userManager.checkpointDetail.level;

        if (stage != 0)
            character.transform.position = Vector3.MoveTowards(character.transform.position, charTargetPosition[stage - 1].position, charMoveSpeed * Time.deltaTime);
    }

    public void UpdateCheckpoint()
    {
        dbManager.GetCheckpoint();
        for (int i = 0; i <= checkpointStage - 1; i++)
        {
            stageButton[i].GetComponent<Button>().interactable = true;
            stageButton[i].GetComponent<Image>().sprite = stageButtonOn[i];
        }
    }

    public void UpdateStageSelection()
    {
        levelSelection.GetComponent<Image>().sprite = levelSelectionSprite[stage - 1];
        for (int i = 0; i < 3; i++)
        {
            levelButton[i].GetComponent<Button>().interactable = false;
            levelButton[i].GetComponent<Image>().sprite = levelButtonOff[i];
        }

        if (stage == checkpointStage)
        {
            for (int i = 0; i <= checkpointLevel - 1; i++)
            {
                levelButton[i].GetComponent<Button>().interactable = true;
                levelButton[i].GetComponent<Image>().sprite = levelButtonOn[i];
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                levelButton[i].GetComponent<Button>().interactable = true;
                levelButton[i].GetComponent<Image>().sprite = levelButtonOn[i];
            }
        }
    }

    public void ChooseStage(int stageIndex)
    {
        stage = stageIndex;
        UpdateStageSelection();
    }

    public void ChooseLevel(int levelIndex)
    {
        level = levelIndex;
        Common.CommonFunction.MoveToScene($"2-Windows-Level{level}");
    }
}
