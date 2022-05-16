using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NumberBlockHandler : MonoBehaviour
{
    public bool isInteracted;
    public RecognitionNumberManager gameManager;
    public GameObject interactableBlock;
    public GameObject nonInteractableBlock;
    public Animator animator;
    public AudioSource audioSource;

    [Header("Description Attribute")]
    public string numberIndex;
    public GameObject descriptionPanel;
    public AudioClip audioClip;
    public Sprite numberImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        interactableBlock.SetActive(isInteracted);
        nonInteractableBlock.SetActive(!isInteracted);
        
        if (!isInteracted)
            animator.enabled = false;

        if (!gameManager.isPlay)
        {
            if (!descriptionPanel.GetComponent<DescriptionHandler>().audioSource.isPlaying)
            {
                descriptionPanel.GetComponent<DescriptionHandler>().animator.SetTrigger("isScaleDown");
                gameManager.isPlay = true;
            }
        }
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

        handler.audioSource.clip = audioClip;
        handler.audioSource.Play(); 
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
