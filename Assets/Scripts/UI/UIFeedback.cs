using UnityEngine;
using UnityEngine.EventSystems;

public class UIFeedback : MonoBehaviour
{
    [SerializeField] AudioClip clickSound;
    [SerializeField] float clickSoundVolume = 1f;

    public void PlayClickSound()
    {
        AudioManager.instance.PlaySoundFXClip(clickSound, transform, clickSoundVolume);
    }
}
