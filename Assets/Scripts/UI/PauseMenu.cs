using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject[] menuPanel;
    private bool paused;
    [SerializeField] Toggle autoplay;
    [SerializeField] Toggle speedup;

    private void Start()
    {
        paused = false;
        autoplay.isOn = SaveDataHolder.instance.loadedState.autoplay;
        speedup.isOn = SaveDataHolder.instance.loadedState.speedup;
    }

    public void OnEsc(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        if(GameManager.instance.isFusing)
        {
            GameManager.instance.CancelFusion();
            return;
        }
        TogglePause();
    }

    public void TogglePause()
    {
        if (paused)
        {
            Time.timeScale = GameManager.instance.timeScale;
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
        GetComponent<LoadingScreen>().LoadScene(0, null);
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
