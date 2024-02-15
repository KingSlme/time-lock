using UnityEngine;
using UnityEngine.UI;

public class BackpackSocketHover : MonoBehaviour
{   
    private RawImage _socketIndicatorRawImage;

    private void Awake()
    {
        _socketIndicatorRawImage = GetComponentInChildren<RawImage>();
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
            
        holsterable.Holstered = false;
        _socketIndicatorRawImage.color = Color.white;
    }
}
