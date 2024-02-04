using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketTagInteractor : XRSocketInteractor
{
    [SerializeField] private string _targetTag;

    protected override void Start()
    {
        if (string.IsNullOrEmpty(_targetTag))
            Debug.LogWarning($"{gameObject.name} has no target tag!");
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(_targetTag);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(_targetTag);
    }
}
