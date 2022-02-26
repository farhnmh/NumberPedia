using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public string fullName;
    public string schoolName;
    public int age;

    [Serializable]
    public class checkpoint
    {
        public int stage;
        public int level;
    }

    [Serializable]
    public class history
    {
        public int stage;
        public int level;
        public int historyIndex;
        public string answer;
        public string datetime;
        public string score;
        public string wrongAnswer;
    }

    [SerializeField] public checkpoint checkpointDetail;
    [SerializeField] public history[] historyDetail;
}