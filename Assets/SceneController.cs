using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int introIndex;
    public int gameIndex;
    public int outroIndex;

    private static SceneController instance;

    public void Start()
    {
        if (!instance)
        {
            SceneController.DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void StartIntro()
    {
        SceneManager.LoadScene(introIndex);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameIndex);
    }

    public void StartEndScreen()
    {
        SceneManager.LoadScene(outroIndex);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartGame();
        }
    }

}
