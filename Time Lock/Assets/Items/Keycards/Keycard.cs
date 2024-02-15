using UnityEngine;

public class Keycard : MonoBehaviour, IHolsterable
{
    public bool Holstered { get; set; } = false;
    public BackpackSocketHover CurrentSocket { get; set; } 

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
