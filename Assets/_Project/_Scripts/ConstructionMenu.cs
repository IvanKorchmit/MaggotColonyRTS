using UnityEngine;

[CreateAssetMenu]
public class ConstructionMenu : ScriptableObject
{
    [System.Serializable]
    public class ConstructOption
    {
        public GameObject building;
        public string option;
    }
    [NonReorderable]
    public ConstructOption[] options;
}


