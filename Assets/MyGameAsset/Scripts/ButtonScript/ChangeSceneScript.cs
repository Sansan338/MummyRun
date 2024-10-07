using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public void ChangeStartGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ChangeRuleScene()
    {
        SceneManager.LoadScene("OperationInstructionsScene");
    }

    public void ReturnTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void ExitGame()
    {
        if (UnityEditor.EditorApplication.isPlaying == true)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
