using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    // might move it to camera
    [SerializeField] private PixelPerfectCamera mainCamera;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float speed = 5;
    private Vector2 dir;
    private void Start()
    {
        // prevent error 
        if (mainCamera != null)
            mainCamera = GetComponent<PixelPerfectCamera>();
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
    }
    private void ZoomCamera(float value)
    {
        int valueInt = Mathf.RoundToInt(value);
        mainCamera.assetsPPU -= valueInt;
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
