using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCountingInGameManager : MonoBehaviour
{
    public HandController hand;
    public List<AudioClip> numberSFX;

    [Header("Counting And Drawing In Game Manager")]
    public int levelTotal;
    public int levelIndex;
    public float moveSpeed;
    public List<Transform> transformTarget;
    public List<GameObject> levelGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < levelGroup.Count; i++)
        {
            if (i + 1 < levelIndex)
            {
                levelGroup[i].transform.position = Vector3.MoveTowards(levelGroup[i].transform.position,
                                                                   transformTarget[0].position,
                                                                   moveSpeed * Time.deltaTime);

                if (levelGroup[i].transform.position == transformTarget[0].position)
                    levelGroup[i].SetActive(false);
            }
            else if (i + 1 == levelIndex)
            {
                levelGroup[i].transform.position = Vector3.MoveTowards(levelGroup[i].transform.position,
                                                                   transformTarget[1].position,
                                                                   moveSpeed * Time.deltaTime);
            }
            else if (i + 1 > levelIndex)
            {
                levelGroup[i].transform.position = Vector3.MoveTowards(levelGroup[i].transform.position,
                                                                   transformTarget[2].position,
                                                                   moveSpeed * Time.deltaTime);
            }
        }
    }
}
