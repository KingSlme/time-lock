using UnityEngine;

public class LogTrigger : MonoBehaviour
{
    [SerializeField] private string _logMessage;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_hasTriggered)
            {
                _hasTriggered = true;
                LogManager.Instance.Log(_logMessage);
            }
        }
    }
}
