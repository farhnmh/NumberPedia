using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Firebase;
using Firebase.Database;

public class DatabaseManager : MonoBehaviour
{
    DatabaseReference dbReference;
    UserManager userManager;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        userManager = GameObject.Find("ImportantHandler").GetComponent<UserManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region SIGN IN PROCESS
    public void SignInValidation(string name, string school)
    {
        SignValidation signManager = GameObject.Find("SignManager").GetComponent<SignValidation>();

        dbReference.Child(school).Child(name).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                print($"Retrieve Process is Successful || {snapshot}");

                if (snapshot.Value == null)
                {
                    signManager.notifTemp = "Account Doesn't Exists! Please Change The Account Detail!";
                    signManager.isLogin = false;
                }
                else
                {
                    signManager.notifTemp = "This Account Logged In Successfully!";

                    userManager.fullName = name;
                    userManager.schoolName = school;
                    userManager.age = int.Parse(snapshot.Child("Age").GetValue(true).ToString());
                    GetCheckpoint();
                    GetHistory();

                    signManager.isLogin = true;
                }
            }
            else
            {
                print("Retrieve Process is Failed");
            }
        });
    }
    #endregion

    #region SIGN UP PROCESS
    public void SignUpValidation(string name, string school, int age)
    {
        SignValidation signManager = GameObject.Find("SignManager").GetComponent<SignValidation>();

        dbReference.Child(school).Child(name).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                print($"Retrieve Process is Successful || {snapshot}");

                if (snapshot.Value == null)
                {
                    SignUp(name, school, age);
                    signManager.notifTemp = "New Account Created Successfully!";

                    userManager.fullName = name;
                    userManager.schoolName = school;
                    userManager.age = age;
                    GetCheckpoint();
                    GetHistory();

                    signManager.isLogin = true;
                }
                else
                {
                    signManager.notifTemp = "Account Already Exists! Please Change The Full Name!";
                    signManager.isLogin = false;
                }
            }
            else
            {
                print("Retrieve Process is Failed");
            }
        });
    }

    public void SignUp(string name, string school, int age)
    {
        //create biodata
        dbReference.Child(school).Child(name).Child("Age").SetValueAsync(age);
        dbReference.Child(school).Child(name).Child("Full Name").SetValueAsync(name);
        
        //create checkpoint
        dbReference.Child(school).Child(name).Child("Checkpoint").Child("Stage").SetValueAsync(1);
        dbReference.Child(school).Child(name).Child("Checkpoint").Child("Level").SetValueAsync(1);

        //create history
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                for (int k = 1; k <= 3; k++)
                {
                    dbReference.Child(school).Child(name).Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Answer").SetValueAsync("0");
                    dbReference.Child(school).Child(name).Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Datetime").SetValueAsync("No Game History Yet");
                    dbReference.Child(school).Child(name).Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Wrong Answer").SetValueAsync("0");
                    dbReference.Child(school).Child(name).Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Score").SetValueAsync("0");
                }
            }
        }
    }
    #endregion

    #region HISTORY AND CHECKPOINT PROCESS
    public void CreateCheckpoint()
    {

    }

    public void GetCheckpoint()
    {
        dbReference.Child(userManager.schoolName).Child(userManager.fullName).Child("Checkpoint").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                userManager.checkpointDetail.stage = int.Parse(snapshot.Child("Stage").GetValue(true).ToString());
                userManager.checkpointDetail.level = int.Parse(snapshot.Child("Level").GetValue(true).ToString());
            }
        });
    }
    
    public void CreateHistory()
    {

    }

    public void GetHistory()
    {
        dbReference.Child(userManager.schoolName).Child(userManager.fullName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        for (int k = 1; k <= 3; k++)
                        {
                            for (int a = 0; a <= 26; a++)
                            {
                                if (userManager.historyDetail[a].stage == i &&
                                    userManager.historyDetail[a].level == j &&
                                    userManager.historyDetail[a].historyIndex == k)
                                {
                                    userManager.historyDetail[a].answer = snapshot.Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Answer").GetValue(true).ToString();
                                    userManager.historyDetail[a].datetime = snapshot.Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Datetime").GetValue(true).ToString();
                                    userManager.historyDetail[a].score = snapshot.Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Score").GetValue(true).ToString();
                                    userManager.historyDetail[a].wrongAnswer = snapshot.Child($"Stage-{i}").Child($"Level-{j}").Child($"History-{k}").Child("Wrong Answer").GetValue(true).ToString();
                                }
                            }
                        }
                    }
                }
            }
        });
    }
    #endregion
}