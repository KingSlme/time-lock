using UnityEngine;

public class Keycard : MonoBehaviour
{
    public enum KeycardLevelEnum
    {
        Level1,
        Level2,
        Level3,
        Level4
    }

    [SerializeField] private KeycardLevelEnum _keycardLevel;

    public KeycardLevelEnum KeycardLevel => _keycardLevel;
}
