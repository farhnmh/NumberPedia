using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public bool usingDatabase;
    public TextMeshProUGUI welcomeText;
    public UserManager userManager;
    public DatabaseManager dbManager;
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

    // Start is called before the first frame update
    void Start()
    {
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
        dbManager = GameObject.Find("ImportantHandler").GetComponent<DatabaseManager>();

        dbManager.GetCheckpoint();
        dbManager.GetHistory();

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
        welcomeText.text = $"Welcome Back, {userManager.fullName}!";

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
}
