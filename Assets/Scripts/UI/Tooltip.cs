using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] Toggle toggle; // Reference to the toggle component, set in inspector
    void OnDisable()
    {
        // Wenn das Menü deaktiviert wird, wird der Tooltip automatisch deaktiviert.
        gameObject.SetActive(false);
        toggle.isOn = false; // Setze den Toggle zurück, wenn das Menü geschlossen wird
    }
}
