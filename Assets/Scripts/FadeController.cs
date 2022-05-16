using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public bool fadeIn;
    public bool fadeOut;

    [Header("General Attribute")]
    [SerializeField] GameObject image;
    public float increaseDecreaseIndex;
    public float delayOnAwake;
    public float delayOnProcess;
    public bool playOnStart;

    [Header("Fading For Move Scene")]
    public string sceneTarget;
    public bool fadingForNextScene;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
            StartCoroutine(DelayFading());
    }

    public void FadingForMoveScene(string scene)
    {
        image.SetActive(true);
        sceneTarget = scene;
        StartCoroutine(DelayFading());
    }

    IEnumerator DelayFading()
    {
        yield return new WaitForSeconds(delayOnAwake);
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        var tempAlpha = image.GetComponent<Image>().color;

        if (fadeOut)
            tempAlpha.a -= increaseDecreaseIndex;
        else if (fadeIn)
            tempAlpha.a += increaseDecreaseIndex;

        image.GetComponent<Image>().color = tempAlpha;

        if (fadeOut && tempAlpha.a <= 0)
        {
            if (fadingForNextScene)
                Common.CommonFunction.MoveToScene(sceneTarget);
            else
            {
                Destroy(image);
                Destroy(this);
            }
        }
        else if (fadeIn && tempAlpha.a >= 2)
        {
            if (fadingForNextScene)
                Common.CommonFunction.MoveToScene(sceneTarget);
            else
            {
                Destroy(image);
                Destroy(this);
            }
        }

        yield return new WaitForSeconds(delayOnProcess);
        StartCoroutine(Fading());
    }
}
