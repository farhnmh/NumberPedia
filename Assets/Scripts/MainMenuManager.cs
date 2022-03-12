using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public UserManager userManager;
    public DatabaseManager dbManager;

    // Start is called before the first frame update
    void Start()
    {
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
        dbManager = GameObject.Find("ImportantHandler").GetComponent<DatabaseManager>();

        dbManager.GetCheckpoint();
        dbManager.GetHistory();
    }

    // Update is called once per frame
    void Update()
    {
        welcomeText.text = $"Welcome Back, {userManager.fullName}!";
    }
}
