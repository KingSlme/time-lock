using UnityEngine;

public class Footsteps : MonoBehaviour
{   
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _footstepSFX;
    [SerializeField] private float _minimumRequiredDistance = 0.5f;

    private Vector2 _lastPosition; 

    private void Update()
    {
        CheckIsMoving();
    }

    private void CheckIsMoving()
    {   
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);
        if (Vector2.Distance(_lastPosition, currentPosition) >= _minimumRequiredDistance)
        {
            PlayFootstep();
            _lastPosition = currentPosition;
        }
    }

    private void PlayFootstep() => _audioSource.PlayOneShot(GetRandomAudioClip(_footstepSFX));

    private AudioClip GetRandomAudioClip(AudioClip[] audioClips) => audioClips[Random.Range(0, audioClips.Length)];
}
