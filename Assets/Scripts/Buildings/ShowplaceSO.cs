using UnityEngine;

[CreateAssetMenu(fileName = "ShowplaceSO", menuName = "Scriptable Objects/ShowplaceSO")]
public class ShowplaceSO : ScriptableObject
{
    public Sprite icon;
    public TextSO showplaceName;
    public Cost baseCost;
    public TextSO lore;
    public TextSO effect;
}
