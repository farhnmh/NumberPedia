using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public int bubbleMax;
    public GameObject bubblePrefab;
    public List<GameObject> spawnerPos;
    public List<GameObject> bubbleSpawned;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bubbleSpawned.Count < bubbleMax)
        {
            int random = Random.Range(0, spawnerPos.Count);

            var bubbleTemp = Instantiate(bubblePrefab, spawnerPos[random].transform);
            bubbleTemp.transform.parent = gameObject.transform;  
            bubbleSpawned.Add(bubbleTemp);
        }
    }
}
