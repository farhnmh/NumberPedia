using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportProgressManager : MonoBehaviour
{
    public UserManager userManager;
    public DatabaseManager dbManager;
    public bool isReport;

    [Header("Stage Level Setting")]
    public TextMeshProUGUI stageValueText;
    public TextMeshProUGUI levelValueText;
    public int stageValue;
    public int levelValue;

    [Header("History Detail")]
    public List<TextMeshProUGUI> datetimeText;
    public List<TextMeshProUGUI> stsText;

    // Start is called before the first frame update
    void Start()
    {
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
        dbManager = GameObject.Find("ImportantHandler").GetComponent<DatabaseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        stageValueText.text = stageValue.ToString();
        levelValueText.text = levelValue.ToString();

        if (isReport)
            UpdateHistory();
    }

    public void isCanPullHistory(bool isCan)
    {
        isReport = isCan;
    }

    public void ChangeStageValue(int factor)
    {
        if (factor == 1 && stageValue < 3)
            stageValue++;
        else if (factor == -1 && stageValue > 1)
            stageValue--;
    }

    public void ChangeLevelValue(int factor)
    {
        if (factor == 1 && levelValue < 3)
            levelValue++;
        else if (factor == -1 && levelValue > 1)
            levelValue--;
    }

    public void RefreshHistory()
    {
        dbManager.GetHistory();
    }

    public void UpdateHistory()
    {
        for (int a = 0; a <= 26; a++)
        {
            if (userManager.historyDetail[a].stage == stageValue &&
                userManager.historyDetail[a].level == levelValue)
            {
                for (int b = 1; b <= 3; b++)
                {
                    if (userManager.historyDetail[a].historyIndex == b)
                    {
                        datetimeText[b - 1].text = $"{userManager.historyDetail[a].datetime}";
                        stsText[b - 1].text = $"{userManager.historyDetail[a].answer}/" +
                                          $"{userManager.historyDetail[a].wrongAnswer}/" +
                                          $"{userManager.historyDetail[a].score}";
                    }
                }
            }
        }
    }
}
