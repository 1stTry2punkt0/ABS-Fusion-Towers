using UnityEngine;

public interface IOnTopObj
{
    public Cost sellValue { get; set; }
    public MapTile mapTile { get; set; }

    public void OnSell();

    public void OnSelect();

    public void DeSelect();
}
