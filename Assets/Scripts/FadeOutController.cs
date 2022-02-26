using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeOutController : MonoBehaviour
{
    [SerializeField] List<GameObject> image;
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
        for (int i = 0; i <= image.Count - 1; i++) {
            var tempAlpha = image[i].GetComponent<Image>().color;
            tempAlpha.a -= decreaseIndex;
            image[i].GetComponent<Image>().color = tempAlpha;

            if (tempAlpha.a < 0)
            {
                Destroy(image[i]);
                Destroy(this);
            }
        }

        yield return new WaitForSeconds(delayOnProcess);
        StartCoroutine(FadingOut());
    }
}
