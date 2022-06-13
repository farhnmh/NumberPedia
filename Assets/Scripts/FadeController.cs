using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [Header("Fading When Start Scene")]
    public bool playOnStart;
    [SerializeField] List<GameObject> imageFadeStart;

    [Header("Fading For Move Scene")]
    public bool playWhenMove;
    [SerializeField] GameObject imageFadeMove;
    public string sceneTarget;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
        {
            for (int i = 0; i < imageFadeStart.Count; i++)
            {
                imageFadeStart[i].SetActive(true);
                imageFadeStart[i].GetComponent<Animator>().SetTrigger("isFadingOut");
            }
        }
    }

    void Update()
    {
        if (imageFadeStart[0] != null && imageFadeStart[0].GetComponent<Image>().color.a <= 0)
            Destroy(imageFadeStart[0]);
        if (playWhenMove && imageFadeMove.GetComponent<Image>().color.a >= 1)
            Common.CommonFunction.MoveToScene(sceneTarget);
    }

    public void FadingForMoveScene(string scene)
    {
        if (playWhenMove)
        {
            sceneTarget = scene;

            imageFadeMove.SetActive(true);
            imageFadeMove.GetComponent<Animator>().SetTrigger("isFadingIn");
        }
    }
}
