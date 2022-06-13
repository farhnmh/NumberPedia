using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class NumberBlockHandler : MonoBehaviour
{
    public bool isInteracted;
    public RecognitionNumberManager gameManager;
    public GameObject interactableBlock;
    public GameObject nonInteractableBlock;
    public Animator animator;

    [Header("Description Attribute")]
    public string numberIndex;
    public VideoClip videoClip;
    public GameObject descriptionPanel;
    public AudioClip audioClip;
    public Sprite numberImage;

    // Update is called once per frame
    void Update()
    {
        interactableBlock.SetActive(isInteracted);
        nonInteractableBlock.SetActive(!isInteracted);
        
        if (!isInteracted)
            animator.enabled = false;
    }

    public void ShowDescriptionPanel()
    {
        gameManager.isPlay = false;

        var handler = descriptionPanel.GetComponent<DescriptionHandler>();
        handler.animator.SetTrigger("isScaleUp");

        handler.title.text = $"Halo, Aku Angka {numberIndex}!";
        handler.description.text = $"Halo, Aku Angka {numberIndex}!";
        
        handler.number.sprite = numberImage;
        handler.number.SetNativeSize();

        handler.videoPlayer.clip = videoClip;
        handler.videoPlayer.Play();

        StartCoroutine(WaitingForHide());
    }

    public IEnumerator WaitingForHide()
    {
        yield return new WaitForSeconds((float) videoClip.length);
        descriptionPanel.GetComponent<DescriptionHandler>().animator.SetTrigger("isScaleDown");
        gameManager.isPlay = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            if (isInteracted && gameManager.isPlay)
            {
                isInteracted = false;
                gameManager.isPlay = false;
                ShowDescriptionPanel();
            }
        }
    }
}
