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
    public GameObject objectIdle;
    public GameObject animationTarget;

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
                    objectIdle.SetActive(false);
                    animationTarget.SetActive(true);
                    break;
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            var HandObject = collision.GetComponent<HandController>();
            isChosen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {

        }
    }
}
