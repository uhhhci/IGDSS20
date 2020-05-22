using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MouseSettings", order = 1)]
public class MouseSettings : ScriptableObject
{
    public float ScrollThreshold;
}