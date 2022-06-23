using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class NumberBlockHandler : MonoBehaviour
{
    public bool isInteracted;
    public GameObject interactableBlock;
    public GameObject nonInteractableBlock;
    public Animator animator;
    public AudioSource audioSource;

    [Header("Description Attribute")]
    public string numberIndex;
    public VideoClip videoClip;
    public GameObject descriptionPanel;

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
        var handler = descriptionPanel.GetComponent<DescriptionHandler>();
        handler.animator.SetTrigger("isScaleUp");

        handler.title.text = $"Halo, Aku Angka {numberIndex}!";
        handler.description.text = $"Halo, Aku Angka {numberIndex}!";

        handler.videoPlayer.clip = videoClip;
        handler.videoPlayer.Play();

        StartCoroutine(WaitingForHide());
    }

    public IEnumerator WaitingForHide()
    {
        yield return new WaitForSeconds((float) videoClip.length);
        descriptionPanel.GetComponent<DescriptionHandler>().animator.SetTrigger("isScaleDown");
        isInteracted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hand"))
        {
            if (isInteracted && descriptionPanel.transform.localScale.x <= 0)
            {
                audioSource.Play();
                ShowDescriptionPanel();
            }
        }
    }
}
