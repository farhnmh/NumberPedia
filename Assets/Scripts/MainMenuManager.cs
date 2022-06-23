using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public UserManager userManager;
    public GameObject firstPanel;

    [Header("isTracking Attribute")]
    public Button btnTracking;
    public Sprite btnOn;
    public Sprite btnOff;

    [Header("Panel Movement Attribute")]
    public int panelIndex; 
    public float moveSpeedPanel; 
    public List<GameObject> panel;
    public List<Transform> targetPosition;

    [Header("Audio Attributes")]
    public AudioClip bgmScene;
    public AudioSource bgmHandler;
    public AudioSource sfxButtonClickedHandler;

    void Awake()
    {
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
        bgmHandler.clip = bgmScene;
        bgmHandler.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (userManager.isTracking)
            btnTracking.GetComponent<Image>().sprite = btnOn;
        else
            btnTracking.GetComponent<Image>().sprite = btnOff;

        if (userManager.isInitiating)
            firstPanel.SetActive(true);
        else
            firstPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < panel.Count; i++)
        {
            if (i == panelIndex)
                MovingPanel(panel[i], targetPosition[0]);
            else
                MovingPanel(panel[i], targetPosition[1]);
        }
    }

    public void ChooseTracking()
    {
        if (userManager.isTracking)
        {
            userManager.isTracking = false;
            btnTracking.GetComponent<Image>().sprite = btnOff;
        }
        else
        {
            userManager.isTracking = true;
            btnTracking.GetComponent<Image>().sprite = btnOn;
        }
    }

    public void SetupFirstPanel(bool index)
    {
        userManager.isInitiating = index;
    }

    public void SetupPanelIndex(int index)
    {
        panelIndex = index;
    }

    public void MovingPanel(GameObject panel, Transform targetPosition)
    {
        panel.transform.position = Vector3.MoveTowards(panel.transform.position, targetPosition.position, moveSpeedPanel * Time.deltaTime);
    }

    public void PlaySFXButtonClicked()
    {
        sfxButtonClickedHandler.Play();
    }
}
