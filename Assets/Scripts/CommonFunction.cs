using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Common
{
    public class CommonFunction : MonoBehaviour
    {
        public static void MoveToScene(string sceneTarget)
        {
            SceneManager.LoadScene(sceneTarget);
        }

        public static void QuitApps()
        {
            Application.Quit();
        }
    }
}
