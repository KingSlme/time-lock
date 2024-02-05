using UnityEngine;

public class Footsteps : MonoBehaviour
{   
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _footstepSFX;
    [SerializeField] private float _minimumRequiredDistance = 1.0f;

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

    private void PlayFootstep()
    {   
        _audioSource.clip = GetRandomAudioClip(_footstepSFX);
        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    private AudioClip GetRandomAudioClip(AudioClip[] audioClips) => _footstepSFX[Random.Range(0, audioClips.Length)];
}
