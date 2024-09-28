using UnityEngine;
using UnityEngine.InputSystem;

public class BuildPointer : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject pointer;
    [SerializeField] private InputReader inputReader;
    private void Start()
    {
        if (grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }
    void Update()
    {
        PlacePointer(ReadMouseLocationOnGrid());
    }
    private Vector3Int ReadMouseLocationOnGrid()
    {
        Vector2 location = Mouse.current.position.value;
        location = Camera.main.ScreenToWorldPoint(location);
        return grid.WorldToCell(location);
    }
    private void PlacePointer(Vector3Int gridLocation)
    {
        pointer.transform.position = grid.GetCellCenterWorld(gridLocation);
    }
}
