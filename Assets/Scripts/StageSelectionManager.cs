using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StageSelectionManager : MonoBehaviour
{
    public int stageIndex;
    public float moveSpeed;
    public List<GameObject> buttonChanger;
    public List<Transform> transformTarget;
    public List<GameObject> stageGroup;

    void Update()
    {
        if (stageIndex == 0)
        {
            buttonChanger[0].SetActive(false);
            buttonChanger[1].SetActive(true);
        }
        else if (stageIndex == stageGroup.Count - 1)
        {
            buttonChanger[0].SetActive(true);
            buttonChanger[1].SetActive(false);
        }
        else
        {
            buttonChanger[0].SetActive(true);
            buttonChanger[1].SetActive(true);
        }

        for (int i = 0; i < stageGroup.Count; i++)
        {
            if (i < stageIndex)
                MovingScene(stageGroup[i], transformTarget[0]);
            else if (i == stageIndex)
                MovingScene(stageGroup[i], transformTarget[1]);
            else if (i > stageIndex)
                MovingScene(stageGroup[i], transformTarget[2]);
        }
    }

    public void MovingScene(GameObject stage, Transform target)
    {
        stage.transform.position = Vector3.MoveTowards(stage.transform.position,
                                                       target.position,
                                                       moveSpeed * Time.deltaTime);
    }

    public void ChangeStageGroup(int temp)
    {
        if (temp == 0 && stageIndex != 0)
            stageIndex -= 1;
        else if (temp == 1 && stageIndex != stageGroup.Count)
            stageIndex += 1;
    }
}
