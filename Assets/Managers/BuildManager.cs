using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Transactions;
using NUnit.Framework.Constraints;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private InputReader inputReader;
    [Header("relays ----")]
    [SerializeField] private BuildDataChannel creationRelay;
    [SerializeField] private VoidChannel upgradeRelay;
    [SerializeField] private VoidChannel sellRelay;
    [SerializeField] private VoidChannel playRelay;
    [SerializeField] private VoidChannel loadRelay;
    [Header("player datas ----")]
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private PlayerBuild build;
    [Header("uis ----")]
    [SerializeField] private GameObject buildPickerUI;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject paymentFailedUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject pointer;

    private Dictionary<Vector3Int, GameObject> GameObjectPlacement = new Dictionary<Vector3Int, GameObject>();

    private Vector3Int selectedLocation;
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
        DisablePointer();
        isUIOpen = true;
    }
    private void Input()
    {
        //Debug.Log("mouse down on: " + ReadSelectecGrid());
        bool hasTurret = build.CheckTurret(ReadSelectecGrid());

        if (hasTurret == false)
            OpenTurretMenu(SelectedGridOnWorld());
        else OpenUpgradeMenu(SelectedGridOnWorld());
    }
    public void OpenUpgradeMenu(Vector3 gridLocation)
    {
        if (isUIOpen == false){
            DisablePointer();
            isUIOpen = true;

            BuildData data = build.GetBuild(selectedLocation);
            if (data == null) Debug.Log("upgrade data null");

            upgradeUI.GetComponent<UpgradeMenu>().Setup(data.level);

            selectedLocation = grid.WorldToCell(gridLocation);
            currentUI = Instantiate(upgradeUI, gridLocation, Quaternion.identity, UICanvas.transform);
        }

    }
    private void OpenTurretMenu(Vector3 gridLocation)
    {
        if (!isUIOpen)
        {
            DisablePointer();
            isUIOpen = true;

            selectedLocation = grid.WorldToCell(gridLocation);
            currentUI = Instantiate(buildPickerUI, gridLocation, Quaternion.identity, UICanvas.transform);

        }
    }
    private void RequestUpgrade()
    {
        BuildData data = build.buildPlacement[selectedLocation]; 

        //Debug.Log(data.coalPrice);
        //Debug.Log(data.ironPrice);

        int iron = data.nextData.ironPrice;
        int coal = data.nextData.coalPrice;

        bool tryPayment = economy.Pay(coal, iron);
        GameObject currentBuild = GameObjectPlacement[selectedLocation];
        if (tryPayment)
        {
            currentBuild.GetComponent<Build>().data = data.nextData;
            build.buildPlacement[selectedLocation] = data.nextData;
            CloseMenu();
        }
        if (!tryPayment) PaymentFailed();
    }

    private void PlaceBuild(Build building)
    {
        BuildData buildingData = building.data;

        bool tryPayment = economy.Pay(buildingData.coalPrice, buildingData.ironPrice);

        if (tryPayment)
        {
            GameObject buildInstance = Instantiate(buildingData.prefabs, grid.GetCellCenterWorld(selectedLocation), Quaternion.identity);

            build.AddBuild(selectedLocation, buildInstance);
            GameObjectPlacement[selectedLocation] = buildInstance;

            Debug.Log("buld on: " + selectedLocation);
        }
        if (!tryPayment)
            PaymentFailed();

        CloseMenu();
    }
    private void SellTurret()
    {
        BuildData data = build.buildPlacement[selectedLocation];
        if (data == null) Debug.Log("selling data nul");

        economy.AddCoal(Mathf.FloorToInt(data.coalPrice / 4));
        economy.AddIron(Mathf.FloorToInt(data.ironPrice / 4));

        build.RemoveTurret(selectedLocation);
        Destroy(GameObjectPlacement[selectedLocation]);
        CloseMenu();
    }
    private void PaymentFailed()
    {
        Debug.Log("payment failed");
    }
    private void CloseMenu()
    {
        if (isUIOpen)
        {
            isUIOpen = false;
            if (currentUI != null) Destroy(currentUI);
            currentUI = null;
            EnablePointer();
        }
    }
    private void HandlePlay() {
        isUIOpen = false;
        EnablePointer();
        GameObjectPlacement.Clear();
    }
    private void HandleLoad() {
        ClearExistingBuildings();
        foreach (Vector3Int Key in build.buildPlacement.Keys) {
            GameObject building = build.buildPlacement[Key].prefabs;
            build.GetComponent<Build>().data = build.buildPlacement[Key];
            Instantiate(building, grid.GetCellCenterWorld(Key), Quaternion.identity);
            GameObjectPlacement.Add(Key, building);
        }
    }
    private void ClearExistingBuildings() {
        Build[] existingBuildings = FindObjectsByType<Build>(FindObjectsSortMode.None);
        foreach (Build building in existingBuildings) {
            Destroy(building.gameObject);
        }
        GameObjectPlacement.Clear();
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
        creationRelay.OnEventRaised += PlaceBuild;
        upgradeRelay.OnEvenRaised += RequestUpgrade;
        sellRelay.OnEvenRaised += SellTurret;
        playRelay.OnEvenRaised += HandlePlay;
        loadRelay.OnEvenRaised += HandleLoad;
    }
    private void OnDisable()
    {
        inputReader.MouseClickEvent -= Input;
        inputReader.MoveEvent -= CloseOnMove;
        creationRelay.OnEventRaised -= PlaceBuild;
        upgradeRelay.OnEvenRaised -= RequestUpgrade;
        sellRelay.OnEvenRaised -= SellTurret;
        playRelay.OnEvenRaised -= HandlePlay;
        loadRelay.OnEvenRaised -= HandleLoad;
    }
}
