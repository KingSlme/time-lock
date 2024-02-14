using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BackpackSocketHover : MonoBehaviour
{   
    private Backpack _backpack;
    private RawImage _socketIndicatorRawImage;
    private XRSocketInteractor _xrSocketInteractor;

    private void Awake()
    {
        _backpack = GetComponentInParent<Backpack>();
        _socketIndicatorRawImage = GetComponentInChildren<RawImage>();
        _xrSocketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (!_xrSocketInteractor.hasSelection && !_backpack.IsSocketHovered)
            {   
                _backpack.IsSocketHovered = true;
                _socketIndicatorRawImage.color = Color.green;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            if (_backpack.IsSocketHovered)
            {
                _backpack.IsSocketHovered = false;
                _socketIndicatorRawImage.color = Color.white;
            }
        }
    }
}
