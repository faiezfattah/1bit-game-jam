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
    [SerializeField] private PlayerBuild build;
    [SerializeField] private GameObject buildPickerUI;
    [SerializeField] private GameObject paymentFailedUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject pointer;


    private Vector3Int selectedLocation;
    private Turret currentTuret;
    private GameObject currentUI;
    private bool isUIOpen = false;
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
        bool hasTurret = build.CheckTurret(ReadSelectecGrid());
        Debug.Log(hasTurret);
        if (hasTurret == false)
            OpenTurretMenu(SelectedGridOnWorld());
        else OpenUpgradeMenu(SelectedGridOnWorld());
    }
    public void OpenUpgradeMenu(Vector3 gridLocation)
    {
        DisablePointer();
        isUIOpen = true;

        Turret turret = build.GetTurret(selectedLocation);
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
        int coal = currentTuret.turretData.nextTurretData.coalPrice;

        bool tryPayment = economy.Pay(coal, iron);

        if (tryPayment)
        {
            currentTuret.turretData = currentTuret.turretData.nextTurretData;
            CloseMenu();
        }
        if (!tryPayment) PaymentFailed();
    }

    private void PlaceTurret(GameObject turret)
    {
        TurretData data = turret.GetComponent<Turret>().turretData; // TODO: doesnt work for non-turret (shit)

        bool tryPayment = economy.Pay(data.coalPrice, data.ironPrice);

        if (tryPayment)
        {
            GameObject turretInstance = Instantiate(turret, grid.GetCellCenterWorld(selectedLocation), Quaternion.identity);
            build.AddTurret(selectedLocation, turretInstance.GetComponent<Turret>());
            Debug.Log("buld on: " + selectedLocation);
        }
        if (!tryPayment)
            PaymentFailed();

        CloseMenu();
    }
    private void SellTurret()
    {
        economy.AddCoal(Mathf.FloorToInt(currentTuret.turretData.coalPrice / 4));
        economy.AddIron(Mathf.FloorToInt(currentTuret.turretData.ironPrice / 4));

        Debug.Log(currentTuret.turretData.ironPrice);

        Destroy(currentTuret.gameObject);
        currentTuret = null;
        build.RemoveTurret(selectedLocation);
        CloseMenu();
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
