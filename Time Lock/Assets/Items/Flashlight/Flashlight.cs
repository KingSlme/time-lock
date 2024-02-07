using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light _spotLight;
    [SerializeField] private Renderer _glassRenderer;
    [SerializeField] private Material _glassOffMaterial;
    [SerializeField] private Material _glassOnMaterial;

    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        TurnOffFlashlight();
        _grabInteractable.activated.AddListener(_ => TurnOnFlashlight());
        _grabInteractable.deactivated.AddListener(_ => TurnOffFlashlight());
    }

    private void OnDestroy()
    {
        _grabInteractable.activated.RemoveListener(_ => TurnOnFlashlight());
        _grabInteractable.deactivated.RemoveListener(_ => TurnOffFlashlight());
    }

    private void TurnOnFlashlight()
    {
        _spotLight.enabled = true;
        _glassRenderer.material = _glassOnMaterial;
    }

    private void TurnOffFlashlight()
    {
        _spotLight.enabled = false;
        _glassRenderer.material = _glassOffMaterial;
    }
}
