using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string loadLevel;

    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene(loadLevel);
    }
}
