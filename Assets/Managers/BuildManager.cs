using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Transactions;
using NUnit.Framework.Constraints;
using System.Collections;
using System;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObjectChannel creationRelay;
    [SerializeField] private VoidChannel upgradeRelay;
    [SerializeField] private VoidChannel sellRelay;
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private GameObject buildPickerUI;
    [SerializeField] private GameObject paymentFailedUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject pointer;


    private Vector3Int selectedLocation;
    private Turret currentTuret;
    private GameObject currentUI;
    private bool isUIOpen = false;

    //TODO: move dict to player data
    private Dictionary<Vector3Int, Turret> turretPlacement = new Dictionary<Vector3Int, Turret>();
    void Update()
    {
        PlacePointer(SelectedGridOnWorld());
    }
    private void Start()
    {
        if (grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }
    private void Input()
    {
        //Debug.Log("mouse down on: " + ReadSelectecGrid());
        if (turretPlacement.ContainsKey(ReadSelectecGrid()))
            OpenUpgradeMenu(SelectedGridOnWorld());
        else OpenTurretMenu(SelectedGridOnWorld());
    }
    public void OpenUpgradeMenu(Vector3 gridLocation)
    {
        DisablePointer();
        isUIOpen = true;

        Turret turret = turretPlacement[selectedLocation];
        currentTuret = turret.GetComponent<Turret>();
        currentUI = Instantiate(turret.OpenUpgradeMenu(), gridLocation, Quaternion.identity, UICanvas.transform);

        selectedLocation = grid.WorldToCell(gridLocation);
    }
    private void OpenTurretMenu(Vector3 gridLocation)
    {
        if (!isUIOpen)
        {
            DisablePointer();
            isUIOpen = true;
            currentUI = Instantiate(buildPickerUI, gridLocation, Quaternion.identity, UICanvas.transform);

            selectedLocation = grid.WorldToCell(gridLocation);
        }
    }
    private void RequestUpgrade()
    {
        int iron = currentTuret.turretData.nextTurretData.ironPrice;
        int wood = currentTuret.turretData.nextTurretData.woodPrice;

        bool tryPayment = economy.Pay(wood, iron);

        if (tryPayment)
        {
            currentTuret.turretData = currentTuret.turretData.nextTurretData;
            CloseMenu();
        }
        if (!tryPayment) PaymentFailed();
    }

    private void PlaceTurret(GameObject turret)
    {
        TurretData data = turret.GetComponent<Turret>().turretData;

        bool tryPayment = economy.Pay(data.woodPrice, data.ironPrice);

        if (tryPayment)
        {
            GameObject turretInstance = Instantiate(turret, grid.GetCellCenterWorld(selectedLocation), Quaternion.identity);
            turretPlacement[selectedLocation] = turretInstance.GetComponent<Turret>();
        }
        if (!tryPayment)
            PaymentFailed();

        CloseMenu();
    }
    private void SellTurret()
    {
        economy.AddWood(Mathf.FloorToInt(currentTuret.turretData.woodPrice / 4));
        economy.AddIron(Mathf.FloorToInt(currentTuret.turretData.ironPrice / 4));

        currentTuret = null;
        turretPlacement.Remove(selectedLocation);
    }
    private void PaymentFailed()
    {
        StartCoroutine(PaymentFailedRoutine());
    }
    private IEnumerator PaymentFailedRoutine()
    {
        CloseMenu();
        yield return new WaitForSeconds(0.1f);
        currentUI = Instantiate(paymentFailedUI, UICanvas.transform);
        yield return new WaitForSeconds(1f);
        CloseMenu();

    }
    private void CloseMenu()
    {
        if (isUIOpen)
        {
            isUIOpen = false;
            Destroy(currentUI);
            currentUI = null;
            EnablePointer();
        }
    }
    private void CloseOnMove(Vector2 move)
    {
        CloseMenu();
    }
    private void PlacePointer(Vector3 gridLocation)
    {
        pointer.transform.position = gridLocation;
    }
    private void DisablePointer()
    {
        pointer.SetActive(false);
    }
    private void EnablePointer()
    {
        pointer.SetActive(true);
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
        creationRelay.OnEvenRaised += PlaceTurret;
        upgradeRelay.OnEvenRaised += RequestUpgrade;
        sellRelay.OnEvenRaised += SellTurret;
    }
    private void OnDisable()
    {
        inputReader.MouseClickEvent -= Input;
        inputReader.MoveEvent -= CloseOnMove;
        creationRelay.OnEvenRaised -= PlaceTurret;
        upgradeRelay.OnEvenRaised -= RequestUpgrade;
        sellRelay.OnEvenRaised -= SellTurret;
    }
}
