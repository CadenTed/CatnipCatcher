using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public PlayerController playerControllerScript;

    public void StartNewCheese()
    {
        PlayerPrefs.SetString("sprite", "cheese");
        SceneManager.LoadScene("Main");

    }

    public void StartNewOak()
    {
        PlayerPrefs.SetString("sprite", "oak");
        SceneManager.LoadScene("Main");

    }



    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
