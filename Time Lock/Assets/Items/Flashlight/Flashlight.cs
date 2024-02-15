using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flashlight : MonoBehaviour, IHolsterable
{
    public bool Holstered { get; set; } = false;
    public BackpackSocketHover CurrentSocket { get; set; } 

    [SerializeField] private Light _spotLight;
    [SerializeField] private Renderer _glassRenderer;
    [SerializeField] private Material _glassOnMaterial;
    [SerializeField] private Material _glassOffMaterial;
    [SerializeField] private AudioClip _flashlightOnSFX;
    [SerializeField] private AudioClip _flashlightOffSFX;

    private XRGrabInteractable _grabInteractable;
    private AudioSource _audioSource;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {   
        TurnOffFlashlightNoSFX();
        _grabInteractable.activated.AddListener(_ => TurnOnFlashlight());
        _grabInteractable.deactivated.AddListener(_ => TurnOffFlashlight());
        _grabInteractable.selectExited.AddListener(_ => TurnOffFlashlightNoSFX());
    }

    private void OnDestroy()
    {
        _grabInteractable.activated.RemoveListener(_ => TurnOnFlashlight());
        _grabInteractable.deactivated.RemoveListener(_ => TurnOffFlashlight());
        _grabInteractable.selectExited.RemoveListener(_ => TurnOffFlashlightNoSFX());
    }

    private void TurnOnFlashlight()
    {
        _spotLight.enabled = true;
        _glassRenderer.material = _glassOnMaterial;
        _audioSource.PlayOneShot(_flashlightOnSFX, 0.3f);
    }

    private void TurnOffFlashlight()
    {
        _spotLight.enabled = false;
        _glassRenderer.material = _glassOffMaterial;
        _audioSource.PlayOneShot(_flashlightOffSFX, 0.3f);
    }

    private void TurnOffFlashlightNoSFX()
    {
        _spotLight.enabled = false;
        _glassRenderer.material = _glassOffMaterial;
    }
}
