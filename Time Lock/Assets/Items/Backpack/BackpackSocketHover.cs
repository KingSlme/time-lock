using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BackpackSocketHover : MonoBehaviour
{   
    private RawImage _socketIndicatorRawImage;
    private XRSocketInteractor _xrSocketInteractor;

    private void Awake()
    {
        _socketIndicatorRawImage = GetComponentInChildren<RawImage>();
        _xrSocketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Item"))
            return;
        IHolsterable holsterable = other.GetComponentInParent<IHolsterable>();
        if (holsterable == null)
            return;
        if (holsterable.Holstered)
            return;
        if (holsterable.CurrentSocket != null)
            return;

        holsterable.CurrentSocket = this;
        holsterable.Holstered = true;
        _socketIndicatorRawImage.color = Color.green;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Item"))
            return;
        IHolsterable holsterable = other.GetComponentInParent<IHolsterable>();
        if (holsterable == null)
            return;
        if (!holsterable.Holstered)
            return;
        if (holsterable.CurrentSocket != this)
            return;
        // If there is a holstered item, there cannot be a valid Exit event
        if (_xrSocketInteractor.hasSelection)
            return;

        holsterable.CurrentSocket = null;
        holsterable.Holstered = false;
        _socketIndicatorRawImage.color = Color.white;
    }
}
