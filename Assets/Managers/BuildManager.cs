using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject pointer;
    private void Start()
    {
        if (grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }
    private void Update()
    {
        PlacePointer(ReadMouseLocationOnGrid());
    }
    private Vector3Int ReadMouseLocationOnGrid()
    {
        Vector2 location = Mouse.current.position.value;
        location = Camera.main.ScreenToWorldPoint(location);
        return grid.WorldToCell(location);
    }
    private void TestingInput()
    {
        Debug.Log("mouse down on: " + ReadMouseLocationOnGrid());
        PlaceTurret(ReadMouseLocationOnGrid());
    }
    private void PlaceTurret(Vector3Int gridLocation)
    {
        Instantiate(turret, grid.GetCellCenterWorld(gridLocation), Quaternion.identity);
    }
    private void PlacePointer(Vector3Int gridLocation)
    {
        pointer.transform.position = grid.GetCellCenterWorld(gridLocation);
    }
    private void OnEnable()
    {
        inputReader.MouseClickEvent += TestingInput;
    }
    private void OnDisable()
    {
        inputReader.MouseClickEvent -= TestingInput;
    }
}
