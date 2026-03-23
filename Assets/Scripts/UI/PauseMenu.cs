using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject[] menuPanel;
    private bool paused;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause()
    {
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
