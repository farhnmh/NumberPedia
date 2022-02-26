using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeOutController : MonoBehaviour
{
    [SerializeField] GameObject image;
    public float decreaseIndex;
    public float delayOnAwake;
    public float delayOnProcess;
    public bool isDestroy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayFading());
    }

    IEnumerator DelayFading()
    {
        yield return new WaitForSeconds(delayOnAwake);
        StartCoroutine(FadingOut());
    }

    IEnumerator FadingOut()
    {
        var tempAlpha = image.GetComponent<Image>().color;
        tempAlpha.a -= decreaseIndex;
        image.GetComponent<Image>().color = tempAlpha;

        yield return new WaitForSeconds(delayOnProcess);

        if (tempAlpha.a < 0)
        {
            Destroy(image);
            Destroy(this);
        }

        StartCoroutine(FadingOut());
    }
}
