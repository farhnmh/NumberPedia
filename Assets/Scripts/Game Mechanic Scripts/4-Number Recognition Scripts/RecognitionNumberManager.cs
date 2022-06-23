using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognitionNumberManager : MonoBehaviour
{
    public bool isDone;
    public StageManager stageManager;
    public GameObject descriptionPanel;
    public List<GameObject> numberBlocks;

    void Update()
    {
        if (isDone && descriptionPanel.transform.localScale.x <= 0)
        {
            gameObject.GetComponent<Animator>().SetTrigger("isMovingOut");
            StartCoroutine(SetPanelFalse());

            if (stageManager != null)
            {
                stageManager.isPlay = false;
                stageManager = null;
            }
        }
        else
        {
            for (int i = 0; i < numberBlocks.Count; i++)
            {
                if (descriptionPanel.transform.localScale.x >= 1) numberBlocks[i].GetComponent<Animator>().enabled = false;
                else numberBlocks[i].GetComponent<Animator>().enabled = numberBlocks[i].GetComponent<NumberBlockHandler>().isInteracted;
            }

            for (int i = 0; i < numberBlocks.Count; i++)
            {
                if (!numberBlocks[i].GetComponent<NumberBlockHandler>().isInteracted) isDone = true;
                else
                {
                    isDone = false;
                    break;
                }
            }
        }
    }

    IEnumerator SetPanelFalse()
    {
        isDone = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
