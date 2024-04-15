using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private AudioClip[] _keycardSuccessSFX;
    [SerializeField] private AudioClip[] _keycardFailureSFX;
    [SerializeField] private AudioClip _doorOpenSFX;
    [SerializeField] private AudioClip _doorCloseSFX;

    private float _toggleDoorCooldown = 1.0f;
    private Coroutine _toggleDoorCoroutine;
    private AudioSource _audioSource;
    private bool _firstOpen = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.TryGetComponent(out Keycard keycard))
            if ((int)keycard.KeycardLevel >= (int)_scannerLevel)
                _toggleDoorCoroutine ??= StartCoroutine(ToggleDoor());
            else 
                _audioSource.PlayOneShot(GetRandomAudioClip(_keycardFailureSFX));   
    }

    private IEnumerator ToggleDoor()
    {
        if (_doorAnimator.GetBool("open"))
            CloseDoor();
        else
            OpenDoor();
        yield return new WaitForSeconds(_toggleDoorCooldown);
        _toggleDoorCoroutine = null;
    }

    private void OpenDoor()
    {
        if (!_firstOpen)
        {
            HandleFirstOpen();
        }
        // Temporary Win State
        if (_scannerLevel == ScannerLevelEnum.Level4)
        {
            SceneManager.LoadScene("Win");
        }
        _doorAnimator.SetBool("open", true);
        _audioSource.PlayOneShot(GetRandomAudioClip(_keycardSuccessSFX));
        _audioSource.PlayOneShot(_doorOpenSFX);
    }

    private void CloseDoor()
    {
        _doorAnimator.SetBool("open", false);
        _audioSource.PlayOneShot(GetRandomAudioClip(_keycardSuccessSFX));
        _audioSource.PlayOneShot(_doorCloseSFX);
    }

    private AudioClip GetRandomAudioClip(AudioClip[] audioClips) => audioClips[Random.Range(0, audioClips.Length)];

    private void HandleFirstOpen()
    {
        _firstOpen = true;
        LogManager.Instance.Log($"Player opened level {_scannerLevel} door");
    } 
}
