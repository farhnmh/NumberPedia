using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectFunction : MonoBehaviour
{
    public enum DisplayFunction
    {
        PlayAnimationAfterInteraction
    }

    public bool isChosen;
    public DisplayFunction functionType;

    [Header("Play Animation After Interaction")]
    public int number;
    public float moveSpeed;
    public float fadeSpeed;
    public float delaySpeed;
    public List<GameObject> objectInteract;
    public List<Transform> transformInteract;
    public List<GameObject> numberTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isChosen)
            switch (functionType)
            {
                case DisplayFunction.PlayAnimationAfterInteraction:
                    objectInteract[0].SetActive(false);
                    objectInteract[1].SetActive(true);

                    for (int i = 0; i < numberTarget.Count; i++)
                    {
                        if (numberTarget[i].name == $"{number}")
                        {
                            numberTarget[i].SetActive(true);
                            StartCoroutine(FadeOut(numberTarget[i]));
                            numberTarget[i].transform.position = Vector3.MoveTowards(numberTarget[i].transform.position, 
                                                                                     transformInteract[1].position, 
                                                                                     moveSpeed * Time.deltaTime);
                        }
                    }
                    break;
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

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {

        }
    }
}
