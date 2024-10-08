using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Grid grid;
    [SerializeField] private SpriteRenderer pointer;
    [SerializeField] private LineRenderer circle;
    [SerializeField] private FloatEvent requestCircle;
    private bool isOverriden;
    private bool isDisabled = false;
    private void Update() {
        if (!isOverriden) transform.position = ReadMouseOnWorld();
    }
    private Vector3Int ReadMouseOnGrid() {
        Vector2 location = Mouse.current.position.value;
        location = Camera.main.ScreenToWorldPoint(location);
        return grid.WorldToCell(location);
    }
    private Vector3 ReadMouseOnWorld() {
        return grid.GetCellCenterWorld(ReadMouseOnGrid());
    }
    private void TogglePointer() {
        isDisabled = !isDisabled;
        pointer.enabled = isDisabled;
    }
    public void SetOverride(bool value) {
        isOverriden = value;
    }
    public void SetOverride(bool value, Vector3 pos) {
        isOverriden = value;
        transform.position = pos;
    }
    public void SetOverride(bool value, Vector3Int pos) {
        isOverriden = value;
        transform.position = grid.GetCellCenterWorld(pos);
    }
    public Vector3Int GetGridLocation() {
        return grid.WorldToCell(transform.position);
    }
    public Vector3 GetWorldLocation() {
        return transform.position;
    }
    public void DisableCircle() {
        circle.enabled = false;
    }
    public void EnableCircle(float radius = 0) {
        float angle = 0f;
        for (int i = 0; i < circle.positionCount; i++) {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            circle.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / circle.positionCount);
        }
        circle.useWorldSpace = false;
        circle.GetComponent<Transform>().position = GetWorldLocation();
        circle.enabled = true;
    }
    private void OnEnable() {
        requestCircle.OnEventRaised += EnableCircle;
        inputReader.EscapeEvent += TogglePointer;
    }
    private void OnDisable() {
        requestCircle.OnEventRaised -= EnableCircle;
        inputReader.EscapeEvent += TogglePointer;
    }
}
