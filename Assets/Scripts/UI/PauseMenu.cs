using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject[] menuPanel;
    private bool paused;
    

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        if(GameManager.instance.gameState == GameState.Fusing)
        {
            GameManager.instance.CancelFusion();
            return;
        }

        if (paused)
        {
            Time.timeScale = 1.0f;
            foreach (GameObject go in menuPanel)
            {
                go.SetActive(false);
            }
            if(SaveDataHolder.instance != null)
            SaveDataHolder.instance.SaveData();
        }
        else
        {
            Time.timeScale = 0;
            menuPanel[0].SetActive(true);
        }
        paused = !paused;
    }

    public void OpenMenu(int index)
    {
        foreach (GameObject go in menuPanel)
        {
            if(go.activeSelf)
            go.SetActive(false);
        }
        menuPanel[index].SetActive(true);
    }

    public void BackToMain()
    {
        if(SaveDataHolder.instance !=null)
        SaveDataHolder.instance.SaveData();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        if (SaveDataHolder.instance != null)
            SaveDataHolder.instance.SaveData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
