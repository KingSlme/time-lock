using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SprintingSystem : MonoBehaviour
{   
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _pantingSFX;
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _sprintSpeed = 1.5f;
    /// <summary>
    /// The player can only sprint a certain amount of distance before exhaustion
    /// This allows support for precise speed control with the thumbstick as opposed to a flat stamina system
    /// </summary>
    [SerializeField] private float _maxSprintDistance = 7.5f;
    [SerializeField] private float _recoveryTime = 5.0f;

    private InputData _inputData;
    private DynamicMoveProvider _dynamicMoveProvider;
    private float _sprintDistanceRemaining;
    private Vector2 _lastPosition;
    private Coroutine _beginRecoveryCoroutine;

    private void Awake()
    {
        _inputData = GetComponent<InputData>();
        _dynamicMoveProvider = GetComponent<DynamicMoveProvider>();
    }

    private void Start()
    {
        _sprintDistanceRemaining = _maxSprintDistance;
    }

    private void Update()
    {
        HandleSprintingSystem();
    }

    private void HandleSprintingSystem()
    {
        if (_sprintDistanceRemaining <= 0.0f)
        {   
            SetWalkSettings();
            if (_beginRecoveryCoroutine == null)
                _beginRecoveryCoroutine = StartCoroutine(BeginRecovery());
        }
        else if (CheckIsSprinting()) 
        {
            SetSprintSettings();
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);
            _sprintDistanceRemaining -= Vector2.Distance(_lastPosition, currentPosition);
            _lastPosition = currentPosition;
        }
        else
        {
            SetWalkSettings();
            float recoveryRate = _maxSprintDistance / _recoveryTime * Time.deltaTime;
            _sprintDistanceRemaining = Mathf.Clamp(_sprintDistanceRemaining + recoveryRate, 0.0f, _maxSprintDistance);
        }
    }

    private bool CheckIsSprinting()
    {
        if (_inputData.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primary2DAxisClicked))
            return primary2DAxisClicked;
        return false;
    }

    private void SetWalkSettings()
    {
        SetMovementSpeed(_walkSpeed);
    }

    private void SetSprintSettings()
    {
        SetMovementSpeed(_sprintSpeed);
    }

    private void SetMovementSpeed(float speed)
    {
        _dynamicMoveProvider.moveSpeed = speed;
    }

    private IEnumerator BeginRecovery()
    {   
        _audioSource.PlayOneShot(_pantingSFX);
        yield return new WaitForSeconds(_recoveryTime);
        _sprintDistanceRemaining = _maxSprintDistance;
        _beginRecoveryCoroutine = null;
    }
}
