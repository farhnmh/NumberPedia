using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingNumberAfterInteraction : MonoBehaviour
{
    public bool isChosen;
    public bool isDone;
    public CountingAndDrawingInGameManager gameManager;
    public CountingAndDrawingManager levelManager;
    public HandController hand;

    [Header("Counting Number After Interaction")]
    public int number;
    public float moveSpeed;
    public float fadeSpeed;
    public float delaySpeed;
    public Animator animator;
    public List<GameObject> objectInteract;
    public List<Transform> transformInteract;
    public List<GameObject> numberTarget;

    // Update is called once per frame
    void Update()
    {
        if (isChosen && !isDone)
        {
            if (hand.isReady)
            {
                number = levelManager.numberIndex + 1;
                objectInteract[0].SetActive(false);
                objectInteract[1].SetActive(true);

                levelManager.numberIndex++;

                isDone = true;
                isChosen = false;
            }
        }
        else if (isDone)
        {
            animator.SetTrigger("isReady");
            numberTarget[number].SetActive(true);
            StartCoroutine(FadeOut(numberTarget[number]));
            numberTarget[number].transform.position = Vector3.MoveTowards(numberTarget[number].transform.position,
                                                                     transformInteract[1].position,
                                                                     moveSpeed * Time.deltaTime);
        }

        if (levelManager.numberTarget == levelManager.numberIndex)
        {
            animator.SetTrigger("isDone");
            levelManager.drawingGroup.SetActive(true);
        }
    }

    IEnumerator FadeOut(GameObject number)
    {
        Color objectColor = number.GetComponent<Renderer>().material.color;
        float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        number.GetComponent<Renderer>().material.color = objectColor;
        
        yield return new WaitForSeconds(delaySpeed);
        StartCoroutine(FadeOut(number));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            hand = collision.GetComponent<HandController>();
            isChosen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
            isChosen = false;
    }
}
