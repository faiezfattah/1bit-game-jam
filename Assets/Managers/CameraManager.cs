using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    // might move it to camera
    [SerializeField] private PixelPerfectCamera mainCamera;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject pointer1;
    [SerializeField] private GameObject pointer2;
    [SerializeField] private float speed = 5;
    private Vector2 dir;
    private Vector3 minBound;
    private Vector3 maxBound;
    private void Start()
    {
        // prevent error 
        if (mainCamera != null)
            mainCamera = GetComponent<PixelPerfectCamera>();
        maxBound = pointer1.transform.position;
        minBound = pointer2.transform.position;
    }
    private void Update()
    {
        if (dir != Vector2.zero)
            MoveCamera();
    }
    private void MoveCamera()
    {
        Vector2 movement = speed * Time.deltaTime * dir;
        mainCamera.transform.Translate(movement);

        float clampedX = Mathf.Clamp(transform.position.x, minBound.x, maxBound.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBound.y, maxBound.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
    private void ZoomCamera(float value)
    {
        int valueInt = Mathf.RoundToInt(value);
        mainCamera.assetsPPU -= valueInt;
        mainCamera.assetsPPU = Mathf.Clamp(mainCamera.assetsPPU, 10, 50);
    }
    private void CameraMoveDirection(Vector2 value)
    {
        dir = value;
    }
    private void OnEnable()
    {
        inputReader.MoveEvent += CameraMoveDirection;
        inputReader.MouseScrollEvent += ZoomCamera;
    }
    private void OnDisable()
    {
        inputReader.MoveEvent -= CameraMoveDirection;
        inputReader.MouseScrollEvent -= ZoomCamera;
    }
}
