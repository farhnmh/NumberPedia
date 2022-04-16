using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public bool isInteracting;
    public bool isReady;
    public bool isTouched;
    public ControlManager controlManager;
    public Image loadingBar;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        controlManager = GameObject.Find("Background Script").GetComponent<ControlManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting)
        {
            if (isTouched)
            {
                loadingBar.fillAmount += controlManager.loadingSpeed * Time.deltaTime;
                if (loadingBar.fillAmount >= 1)
                    isReady = true;
            }
            else if (!isTouched)
            {
                loadingBar.fillAmount = 0;
                isReady = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
            isTouched = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
            isTouched = false;
    }
}
