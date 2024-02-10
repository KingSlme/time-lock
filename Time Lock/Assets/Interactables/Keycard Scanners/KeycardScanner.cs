using System.Collections;
using UnityEngine;

public class KeycardScanner : MonoBehaviour
{
    public enum ScannerLevelEnum
    {
        Level1,
        Level2,
        Level3,
        Level4
    }

    [SerializeField] private ScannerLevelEnum _scannerLevel;
    [SerializeField] private Animator _doorAnimator;

    private float _toggleDoorCooldown = 1.0f;
    private Coroutine _toggleDoorCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.TryGetComponent(out Keycard keycard))
            if ((int)keycard.KeycardLevel >= (int)_scannerLevel)
                if (_toggleDoorCoroutine == null)
                    _toggleDoorCoroutine =  StartCoroutine(ToggleDoor());
    }

    private IEnumerator ToggleDoor()
    {
        if (_doorAnimator.GetBool("open"))
        {
            _doorAnimator.SetBool("open", false);
        }
        else
        {
            _doorAnimator.SetBool("open", true);
        }
        yield return new WaitForSeconds(_toggleDoorCooldown);
        _toggleDoorCoroutine = null;
    }
}
