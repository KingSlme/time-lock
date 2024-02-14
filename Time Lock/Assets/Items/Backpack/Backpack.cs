using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Backpack : MonoBehaviour
{
    [SerializeField] private Transform _backPosition;

    public bool IsSocketHovered { get; set; } = false;

    private XRSocketInteractor[] _xrSocketInteractors;
    private XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _xrSocketInteractors = GetComponentsInChildren<XRSocketInteractor>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {   
        _grabInteractable.selectEntered.AddListener(_ => ActivateBackpack());
        _grabInteractable.selectExited.AddListener(_ => DeactivateBackpack());
    }

    private void OnDestroy()
    {
        _grabInteractable.selectEntered.RemoveListener(_ => ActivateBackpack());
        _grabInteractable.selectExited.RemoveListener(_ => DeactivateBackpack());
    }

    private void ActivateBackpack()
    {
        foreach (XRSocketInteractor xrSocketInteractor in _xrSocketInteractors)
        {   
            if (xrSocketInteractor.GetComponentInChildren<XRGrabInteractable>() != null)
                xrSocketInteractor.GetComponentInChildren<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Default", "Player");
        }
    }

    private void DeactivateBackpack()
    {
        StartCoroutine(ReturnToBackPosition());
        foreach (XRSocketInteractor xrSocketInteractor in _xrSocketInteractors)
        {
            if (xrSocketInteractor.GetComponentInChildren<XRGrabInteractable>() != null)
                xrSocketInteractor.GetComponentInChildren<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Default");
        }
    }

    private IEnumerator ReturnToBackPosition()
    {
        transform.SetPositionAndRotation(_backPosition.position, _backPosition.rotation);
        // Temporarily freeze rigidbody
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        yield return null;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    /// <summary>
    /// By parenting items to sockets, XRGrabInteractable(s) can have their interaction layers changed
    /// to prevent accidently grabbing items when the backpack is not selected
    /// </summary>
    public void ParentItemToSocket(SelectEnterEventArgs args)
    {   
        if (args.interactorObject.transform.parent.gameObject.activeSelf)
            args.interactableObject.transform.parent = args.interactorObject.transform;
    }

    public void UnparentItemFromSocket(SelectExitEventArgs args)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(UnparentCoroutine(args));
    }

    /// <summary>
    /// Coroutine to prevent trying to deactivate and change parent at same time when leaving playmode
    /// </summary>
    private IEnumerator UnparentCoroutine(SelectExitEventArgs args)
    {
        yield return null;
        args.interactableObject.transform.parent = null;
    }
}
