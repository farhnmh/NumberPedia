using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public bool isUsed;
    public bool isTouched;
    public GameManager gameManager;
    public InGameObjectFunction objectTarget;
    public Image loadingBar;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background Script").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed)
            objectTarget.isChosen = true;

        if (isTouched)
        {
            loadingBar.fillAmount += gameManager.loadingSpeed * Time.deltaTime;
            if (loadingBar.fillAmount >= 1)
                isUsed = true;
        }
        else if (!isTouched)
            loadingBar.fillAmount -= gameManager.loadingSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            objectTarget = collision.GetComponent<InGameObjectFunction>();
            isTouched = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
            isTouched = false;
    }
}
