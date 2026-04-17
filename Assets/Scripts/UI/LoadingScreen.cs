using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen; //Reference to the loading screen game object, set in inspector
    public Image backgroundImage; //Reference to the background image of the loading screen, set in inspector
    [SerializeField] Sprite defaultSprite; //Default sprite to use for the background image if no custom sprite is set, set in inspector
    public Image loadingImage;

    void Start()
    {
        loadingScreen.SetActive(false); //Ensure the loading screen is inactive at the start
    }

    public void LoadScene(int SceneIndex, Sprite background)
    {
        loadingScreen.SetActive(true); //Activate the loading screen
        loadingImage.fillAmount = 0; //Reset the loading image fill amount to 0
        if (background != null) //If a custom background sprite is provided, use it; otherwise, use the default sprite
        {
            backgroundImage.sprite = background;
        }
        else
        {
            backgroundImage.sprite = defaultSprite;
        }
        StartCoroutine(LoadSceneAsync(SceneIndex)); //Start the asynchronous loading of the scene
    }

    IEnumerator LoadSceneAsync(int SceneIndex)
    {
        float minimumLoadTime = 0.5f;
        float timer = 0;
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneIndex); //Start loading the scene asynchronously
        operation.allowSceneActivation = false; //Prevent the scene from activating immediately after loading

        while (!operation.isDone) //While the scene is still loading
        {
            timer += Time.unscaledDeltaTime; //Increment the timer by the time elapsed since the last frame
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f); //Calculate the loading progress (operation.progress goes from 0 to 0.9)

            float timeProgress = Mathf.Clamp01(timer / minimumLoadTime); //Calculate the time progress based on the timer and minimum load time

            float targetProgress = Mathf.Min(realProgress, timeProgress); //Determine the target progress to display, which is the lesser of real progress and time progress

            loadingImage.fillAmount = Mathf.Lerp(loadingImage.fillAmount, targetProgress, Time.unscaledDeltaTime * 8f);

            if(operation.progress >= 0.9f && timer >= minimumLoadTime)
            {
                operation.allowSceneActivation = true; //Allow the scene to activate once loading is complete and minimum load time has passed
            }

            yield return null; //Wait for the next frame before continuing the loop
        }
    }
}
