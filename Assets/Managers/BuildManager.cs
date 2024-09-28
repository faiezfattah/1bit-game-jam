using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject buildPickerUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private TurretData singleTurret;

    private Vector3 selectedLocation;
    private GameObject currentUI;
    private bool isUIOpen = false;

    public event UnityAction<GameObject> ChoosenTurret;

    private Dictionary<Vector3Int, Turret> turretPlacement = new Dictionary<Vector3Int, Turret>();
    private void Start()
    {
        if (grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }
    private void Input()
    {
        Debug.Log("mouse down on: " + ReadSelectecGrid());
        if (turretPlacement.ContainsKey(ReadSelectecGrid()))
            OpenUpgradeMenu();
        else OpenTurretMenu(SelectedGridOnWorld());
    }
    public void OpenUpgradeMenu()
    {
        isUIOpen = true;
        Debug.Log("open upgrade menu"); // TODO: connect with the turret NOT DEPENDENT
    }
    private void OpenTurretMenu(Vector3 gridLocation)
    {
        if (!isUIOpen)
        {
            isUIOpen = true;
            currentUI = Instantiate(buildPickerUI, gridLocation, Quaternion.identity, UICanvas.transform);
            selectedLocation = gridLocation;
        }
        else CloseMenu();
    }
    private void PlaceTurret(GameObject turret)
    {
        Instantiate(turret, selectedLocation, Quaternion.identity);
    }
    private void CloseOnMove(Vector2 move)
    {
        CloseMenu();
    }
    private void CloseMenu()
    {
        if (isUIOpen)
        {
            isUIOpen = false;
            Destroy(currentUI);
            currentUI = null;
        }
    }
    private Vector3Int ReadSelectecGrid()
    {
        Vector2 location = Mouse.current.position.value;
        location = Camera.main.ScreenToWorldPoint(location);
        return grid.WorldToCell(location);
    }
    private Vector3 SelectedGridOnWorld()
    {
        return grid.GetCellCenterWorld(ReadSelectecGrid());
    }
    private void OnEnable()
    {
        inputReader.MouseClickEvent += Input;
        inputReader.MoveEvent += CloseOnMove;
    }
    private void OnDisable()
    {
        inputReader.MouseClickEvent -= Input;
        inputReader.MoveEvent -= CloseOnMove;
    }
}
