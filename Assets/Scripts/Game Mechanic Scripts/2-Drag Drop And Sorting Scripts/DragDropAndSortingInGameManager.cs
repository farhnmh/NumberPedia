using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropAndSortingInGameManager : MonoBehaviour
{
    public bool isReady;
    public HandController hand;
    public AudioClip wrongNumberSFX;
    public List<AudioClip> correctNumberSFX;
    public List<AudioClip> justNumberSFX;

    [Header("Drag Drop And Sorting In Game Manager")]
    public List<int> randomValue;
    public List<GameObject> otterGroup;
    public List<Transform> transformTarget;

    // Start is called before the first frame update
    void Start()
    {
        isReady = false;
        RandomSorting();
    }

    // Update is called once per frame
    void Update()
    {
        isPlayingChecker();
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

    void isPlayingChecker()
    {
        for (int i = 0; i < otterGroup.Count; i++)
        {
            var otter = otterGroup[i].GetComponent<DragDropAndSortingAfterInteraction>();
            if (otter.audioSource.isPlaying)
            {
                isReady = false;
                break;
            }
            else if (!otter.audioSource.isPlaying)
            {
                isReady = true;
            }
        }
    }

    void RandomSorting()
    {
        for (int i = 0; i < otterGroup.Count; i++)
        {
            int rand = Random.Range(0, randomValue.Count);

            otterGroup[i].transform.position = transformTarget[randomValue[rand] - 1].position;
            otterGroup[i].GetComponent<DragDropAndSortingAfterInteraction>().positionTemp = new Vector3(transformTarget[randomValue[rand] - 1].position.x,
                                                                                                        transformTarget[randomValue[rand] - 1].position.y,
                                                                                                        0);
            randomValue.RemoveAt(rand);
            isReady = true;
        }
    }
}
