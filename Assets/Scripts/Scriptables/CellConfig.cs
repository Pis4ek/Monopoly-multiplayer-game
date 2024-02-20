using UnityEngine;

public class CellConfig : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
}