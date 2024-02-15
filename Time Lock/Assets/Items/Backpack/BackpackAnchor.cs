using UnityEngine;

public class BackpackAnchor : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private void Update()
    {
        PositionBehind(0.25f, -0.25f);
    }

    private void PositionBehind(float backOffset, float yOffset)
    {
        Vector3 behindPosition = _mainCamera.transform.TransformPoint(Vector3.back * backOffset);
        transform.position = new Vector3(behindPosition.x, _mainCamera.transform.position.y + yOffset, behindPosition.z);
    }
}
