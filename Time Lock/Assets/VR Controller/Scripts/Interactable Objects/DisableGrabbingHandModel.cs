using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableGrabbingHandModel : MonoBehaviour
{   
    /*
        Ensure tags are set for hand models and interactors
    */
    private GameObject _leftHandModel;
    private GameObject _rightHandModel;
    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {   
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _leftHandModel = GameObject.FindGameObjectWithTag("Left Hand Model");
        _rightHandModel = GameObject.FindGameObjectWithTag("Right Hand Model");
    }

    private void Start()
    {
        _grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        _grabInteractable.selectExited.AddListener(ShowGrabbingHand);
    }

    private void OnDestroy()
    {
        _grabInteractable.selectEntered.RemoveListener(HideGrabbingHand);
        _grabInteractable.selectExited.RemoveListener(ShowGrabbingHand);
    }

    private void HideGrabbingHand(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Left Hand Interactor"))
            _leftHandModel.SetActive(false);
        else if (args.interactorObject.transform.CompareTag("Right Hand Interactor"))
            _rightHandModel.SetActive(false);
    }

    private void ShowGrabbingHand(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("Left Hand Interactor"))
            _leftHandModel.SetActive(true);
        else if (args.interactorObject.transform.CompareTag("Right Hand Interactor"))
            _rightHandModel.SetActive(true);
    }
}
