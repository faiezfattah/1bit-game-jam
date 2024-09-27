using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float speed = 5;
    private Vector2 dir;
    private void Start()
    {
        // prevent error 
        if (mainCamera != null)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (dir != Vector2.zero)
            MoveCamera();
    }
    private void MoveCamera()
    {
        Vector2 movement = dir * speed * Time.deltaTime;
        mainCamera.transform.Translate(movement);
    }
    private void ZoomCamera(float value)
    {
        mainCamera.orthographicSize += value;
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
