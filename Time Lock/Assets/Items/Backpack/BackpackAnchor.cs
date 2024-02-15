using UnityEngine;

public class BackpackAnchor : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private void Update()
    {
        AdjustYPosition(-0.25f);
    }

    private void AdjustYPosition(float yOffset)
    {
        transform.position = new Vector3(transform.position.x, _mainCamera.transform.position.y + yOffset, transform.position.z);
    }
}
