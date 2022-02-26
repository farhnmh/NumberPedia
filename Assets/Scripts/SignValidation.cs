using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SignValidation : MonoBehaviour
{
    public TextMeshProUGUI notification;
    public string notifTemp;
    public string mainMenuScene;
    public bool isLogin;

    DatabaseManager dbManager;

    [Header("Sign In Attribute")]
    [SerializeField] TMP_InputField nameInputIn; 
    [SerializeField] TMP_InputField schoolInputIn;

    [Header("Sign Up Attribute")]
    [SerializeField] TMP_InputField nameInputUp;
    [SerializeField] TMP_InputField schoolInputUp;
    [SerializeField] TMP_InputField ageInputUp;

    void Start()
    {
        dbManager = GameObject.Find("ImportantHandler").GetComponent<DatabaseManager>();
    }

    void Update()
    {
        notification.text = notifTemp;
        if (isLogin)
            Common.CommonFunction.MoveToScene(mainMenuScene);
    }

    public void SignInRequest()
    {
        if (nameInputIn.text != "" &&
            schoolInputIn.text != "")
        {
            notifTemp = "Please Wait...";
            dbManager.SignInValidation(nameInputIn.text.ToUpper(),
                                       schoolInputIn.text.ToUpper());
        }
        else
            notifTemp = "Please Fill In The Blank Field!";
    }

    public void SignUpRequest()
    {
        if (nameInputUp.text != "" &&
            schoolInputUp.text != "" &&
            ageInputUp.text != "") 
        {
            notifTemp = "Please Wait...";
            try
            {
                if (3 < int.Parse(ageInputUp.text) && int.Parse(ageInputUp.text) < 60)
                    dbManager.SignUpValidation(nameInputUp.text.ToUpper(),
                                               schoolInputUp.text.ToUpper(),
                                               int.Parse(ageInputUp.text));
                else
                    notifTemp = "Sign Up Failed! Please Input Age Between 3 And 60!";
            }
            catch
            {
                notifTemp = "Sign Up Failed! Please Input Age Correctly!";
            }
        }
        else
            notifTemp = "Please Fill In The Blank Field!";
    }
}
