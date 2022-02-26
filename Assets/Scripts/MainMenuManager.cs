using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public UserManager userManager;

    // Start is called before the first frame update
    void Start()
    {
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
    }

    // Update is called once per frame
    void Update()
    {
        welcomeText.text = $"Welcome Back, {userManager.fullName}!";
    }
}
