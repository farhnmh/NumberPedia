using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMoving : MonoBehaviour
{
    public bool isSplash;
    public BubbleSpawner bubbleSpawner;
    public AnimationClip bubbleSplash;
    public Vector2 minimumPos;
    public Vector2 maximumPos;
    public Vector3 targetPos;
    public float waitSplash;
    public float waitDestroy;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        bubbleSpawner = gameObject.transform.parent.GetComponent<BubbleSpawner>();

        waitSplash = Random.Range(5, 15);
        StartCoroutine(WaitForSplash());
        InitializeRandomData();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, targetPos) <= 0.01f)
            InitializeRandomData();

        if (isSplash)
        {
            for (int i = 0; i < bubbleSpawner.bubbleSpawned.Count; i++)
                if (gameObject == bubbleSpawner.bubbleSpawned[i])
                    bubbleSpawner.bubbleSpawned.RemoveAt(i);

            Destroy(gameObject);
        }
    }

    void InitializeRandomData()
    {
        float randomX = Random.Range(minimumPos.x, maximumPos.x);
        float randomY = Random.Range(minimumPos.y, maximumPos.y);
        targetPos = new Vector3(randomX, randomY, transform.position.z);
    }

    IEnumerator WaitForSplash()
    {
        yield return new WaitForSeconds(waitSplash);
        
        gameObject.GetComponent<Animator>().SetBool("isSplash", true);
        waitDestroy = bubbleSplash.length;

        yield return new WaitForSeconds(waitDestroy);

        isSplash = true;
    }
}
