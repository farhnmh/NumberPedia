using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropAndSortingInGameManager : MonoBehaviour
{
    public bool isDone;
    public bool isReady;
    public StageManager stageManager;
    public AudioSource audioSource;
    public AudioClip wrongNumberSFX;
    public List<AudioClip> correctNumberSFX;
    public List<AudioClip> justNumberSFX;

    [Header("Drag Drop And Sorting In Game Manager")]
    public List<int> randomValue;
    public List<GameObject> otterGroup;
    public List<Transform> transformTarget;

    private void Start()
    {
        RandomSorting();
    }

    // Update is called once per frame
    void Update()
    {
        PlayingChecker();
        if (isDone && !audioSource.isPlaying)
        {
            gameObject.GetComponent<Animator>().SetTrigger("isMovingOut");
            StartCoroutine(SetPanelFalse());

            if (stageManager != null)
            {
                stageManager.isPlay = false;
                stageManager = null;
            }
        }
    }

    IEnumerator SetPanelFalse()
    {
        isDone = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public void ScaleUpOtters(int numberTarget)
    {
        for (int i = 0; i < otterGroup.Count; i++)
        {
            var otter = otterGroup[i].GetComponent<DragDropAndSortingAfterInteraction>();
            if (otter.numberTarget != numberTarget)
                otter.animator.SetBool("isScaleDown", false);
        }
    }

    public void ScaleDownOtters(int numberTarget)
    {
        for (int i = 0; i < otterGroup.Count; i++)
        {
            var otter = otterGroup[i].GetComponent<DragDropAndSortingAfterInteraction>();
            if (otter.numberTarget != numberTarget)
                otter.animator.SetBool("isScaleDown", true);
        }
    }

    void PlayingChecker()
    {
        isReady = !audioSource.isPlaying;

        for (int i = 0; i < otterGroup.Count; i++)
        {
            var otter = otterGroup[i].GetComponent<DragDropAndSortingAfterInteraction>();
            if (otter.isDone) isDone = true;
            else
            {
                isDone = false;
                break;
            }
        }
    }

    void RandomSorting()
    {
        for (int i = 0; i < otterGroup.Count; i++)
        {
            int rand = Random.Range(0, randomValue.Count);
            otterGroup[i].GetComponent<DragDropAndSortingAfterInteraction>().positionTemp = transformTarget[randomValue[rand] - 1];

            randomValue.RemoveAt(rand);
            isReady = true;
        }
    }
}
