using UnityEngine;

[CreateAssetMenu(fileName = "New Curse", menuName = "ScriptableObjects/Curse", order = 1)]
public class Curse : ScriptableObject
{
    public int curseIdentifier;
    public string curseDescription;
}
