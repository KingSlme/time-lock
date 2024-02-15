using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    [SerializeField] private Transform _backPosition;

    private XRSocketInteractor[] _xrSocketInteractors;
    private XRGrabInteractable _grabInteractable;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _xrSocketInteractors = GetComponentsInChildren<XRSocketInteractor>();
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {   
        DeactivateBackpack();
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
            // Ensures items can be grabbed when backpack is selected
            XRGrabInteractable xrGrabInteractable = xrSocketInteractor.GetComponentInChildren<XRGrabInteractable>();
            if (xrGrabInteractable != null)
                xrGrabInteractable.interactionLayers = InteractionLayerMask.GetMask("Default", "Player");
            // Ensures items can be holstered when backpack is selected
            if (!xrSocketInteractor.socketActive)
                xrSocketInteractor.socketActive = true;
        }
        ToggleBackpackVisibility(true);
    }

    private void DeactivateBackpack()
    {
        StartCoroutine(ReturnToBackPosition());
        foreach (XRSocketInteractor xrSocketInteractor in _xrSocketInteractors)
        {
            // Ensures items cannot be grabbed when backpack is not selected 
            XRGrabInteractable xrGrabInteractable = xrSocketInteractor.GetComponentInChildren<XRGrabInteractable>();
            if (xrGrabInteractable != null)
                xrGrabInteractable.interactionLayers = InteractionLayerMask.GetMask("Default");
            // Ensures item cannot be holstered when backpack is not selected
            if (!xrSocketInteractor.hasSelection)
                xrSocketInteractor.socketActive = false;
        }
        ToggleBackpackVisibility(false);
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

    /// <summary>
    /// Because camera movement can be indepdent through gazing, the backpack mesh, item meshes, and
    /// UI raw images should be hidden when it is not selected
    /// </summary>
    private void ToggleBackpackVisibility(bool visible) 
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
            renderer.enabled = visible;
        RawImage[] rawImages = GetComponentsInChildren<RawImage>();
        foreach (RawImage rawImage in rawImages)
            rawImage.enabled = visible;
    }
}
