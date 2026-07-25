using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "ScriptableObjects/Relic", order = 1)]
public class Relic : ScriptableObject
{
    public Sprite relicSprite;
    public string relicName;
    public string relicDescription;
}
