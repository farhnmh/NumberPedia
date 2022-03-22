using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public bool isInitiating;
    public bool isTracking;
    public string fullName;
    public string schoolName;
    public int age;

    [Serializable]
    public class playing
    {
        public int stage;
        public int level;
    }

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

    [SerializeField] public playing currentlyPlayingDetail;
    [SerializeField] public checkpoint checkpointDetail;
    [SerializeField] public history[] historyDetail;
}