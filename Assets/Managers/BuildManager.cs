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
using UnityEditor;
using static UnityEngine.Rendering.HableCurve;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private InputReader inputReader;
    [Header("relays ----")]
    [SerializeField] private BuildDataChannel creationRelay;
    [SerializeField] private VoidChannel upgradeRelay;
    [SerializeField] private VoidChannel sellRelay;
    [SerializeField] private VoidChannel playRelay;
    [SerializeField] private VoidChannel rebuildRelay;
    [SerializeField] private AudioChannel sfxRelay;
    [Header("player datas ----")]
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private PlayerBuild build;
    [Header("uis ----")]
    [SerializeField] private GameObject buildPickerUI;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject paymentFailedUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject CircleArea;
    [Header("sounds---")]
    [SerializeField] private AudioClip buildSuccess;
    [SerializeField] private AudioClip buildFailed;
    [SerializeField] private AudioClip upgradeSuccess;
    [SerializeField] private AudioClip openMenu;

    private Dictionary<Vector3Int, GameObject> GameObjectPlacement = new Dictionary<Vector3Int, GameObject>();

    private LineRenderer circle;

    private Vector3Int selectedLocation;
    private GameObject currentUI;
    private bool isUIOpen = true;
    void Update() {
        if (isUIOpen) PlacePointer(grid.GetCellCenterWorld(selectedLocation));
        if (!isUIOpen) PlacePointer(SelectedGridOnWorld());
    }
    private void Start()
    {
        circle = CircleArea.GetComponent<LineRenderer>();
        if (grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        //DisablePointer();
    }
    private void HandleMouseDown()
    {
        //Debug.Log("mouse down on: " + ReadSelectecGrid());
        bool hasTurret = build.CheckTurret(ReadSelectecGrid());

        if (!isUIOpen) selectedLocation = grid.WorldToCell(ReadSelectecGrid());

        if (hasTurret == false)
            OpenTurretMenu(SelectedGridOnWorld());
        else OpenUpgradeMenu(SelectedGridOnWorld());
    }
    public void OpenUpgradeMenu(Vector3 gridLocation)
    {
        if (!isUIOpen){
            DisablePointer();
            isUIOpen = true;

            BuildData data = build.GetBuild(selectedLocation);
            if (data == null) Debug.Log("upgrade data null");

            EnableCircle(gridLocation, data.range);
            upgradeUI.GetComponent<UpgradeMenu>().Setup(data.level);
            currentUI = Instantiate(upgradeUI, gridLocation, Quaternion.identity, UICanvas.transform);

            PlaySFX(openMenu);
        }
    }
    private void OpenTurretMenu(Vector3 gridLocation)
    {
        if (!isUIOpen)
        {
            isUIOpen = true;
            currentUI = Instantiate(buildPickerUI, gridLocation, Quaternion.identity, UICanvas.transform);

            PlaySFX(openMenu);
        }
    }
    private void RequestUpgrade()
    {
        BuildData data = build.GetBuild(selectedLocation);
        GameObject currentBuild = GameObjectPlacement[selectedLocation];

        int iron = data.nextData.ironPrice;
        int coal = data.nextData.coalPrice;

        bool tryPayment = economy.Pay(coal, iron);
        if (tryPayment)
        {
            Destroy(currentBuild);
            GameObject replace = Instantiate(data.nextData.prefabs, grid.GetCellCenterWorld(selectedLocation), Quaternion.identity);
            GameObjectPlacement[selectedLocation] = replace;
            build.UpdateBuild(selectedLocation, replace);

            PlaySFX(upgradeSuccess);

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
            PlaySFX(buildSuccess);
        }
        if (!tryPayment)
            PaymentFailed();

        Debug.Log(buildingData.name);

        CloseMenu();
    }
    private void SellTurret()
    {
        BuildData data = build.buildPlacement[selectedLocation];
        if (data == null) Debug.Log("selling data nul");

        economy.AddCoal(Mathf.FloorToInt(data.coalPrice / 4));
        economy.AddIron(Mathf.FloorToInt(data.ironPrice / 4));

        build.RemoveBuild(selectedLocation);
        Destroy(GameObjectPlacement[selectedLocation]);
        CloseMenu();
    }
    private void PaymentFailed()
    {
        PlaySFX(buildFailed);
    }
    private void CloseMenu()
    {
        if (isUIOpen)
        {
            isUIOpen = false;
            if (currentUI != null) Destroy(currentUI);
            currentUI = null;
            EnablePointer();
            DisableCircle();
        }
    }
    private void HandlePlay() {
        isUIOpen = false;
        EnablePointer();
        GameObjectPlacement.Clear();
    }
    private void HandleLoad() {
        ClearExistingBuildings();
        Debug.Log("HandeLoad called");

        foreach (Vector3Int Key in build.buildPlacement.Keys) {

            Debug.Log("instantiatin builds: " + build.buildPlacement[Key]);

            GameObject building = build.buildPlacement[Key].prefabs;
            Debug.Log("got the game object");
            building.GetComponent<Build>().data = build.buildPlacement[Key];
            Debug.Log("reassign the data");
            Instantiate(building, grid.GetCellCenterWorld(Key), Quaternion.identity);
            Debug.Log("instantiated");
            GameObjectPlacement.Add(Key, building);
            Debug.Log("added to local dictionary");
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
    void EnableCircle(Vector3 worldSpace, float radius = 2) {
        float angle = 0f;
        for (int i = 0; i < circle.positionCount; i++) {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            circle.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / circle.positionCount);
        }
        circle.useWorldSpace = false;
        circle.GetComponent<Transform>().position = worldSpace;
        Debug.Log(worldSpace);
        Debug.Log(circle.transform.position);
        circle.enabled = true;
    }
    private void PlaySFX(AudioClip clip) {
        sfxRelay.RaiseEvent(clip);
    }
    void DisableCircle() {
        circle.enabled = false;
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
        inputReader.MouseClickEvent += HandleMouseDown;
        inputReader.MoveEvent += CloseOnMove;
        creationRelay.OnEventRaised += PlaceBuild;
        upgradeRelay.OnEvenRaised += RequestUpgrade;
        sellRelay.OnEvenRaised += SellTurret;
        playRelay.OnEvenRaised += HandlePlay;
        rebuildRelay.OnEvenRaised += HandleLoad;
    }
    private void OnDisable()
    {
        inputReader.MouseClickEvent -= HandleMouseDown;
        inputReader.MoveEvent -= CloseOnMove;
        creationRelay.OnEventRaised -= PlaceBuild;
        upgradeRelay.OnEvenRaised -= RequestUpgrade;
        sellRelay.OnEvenRaised -= SellTurret;
        playRelay.OnEvenRaised -= HandlePlay;
        rebuildRelay.OnEvenRaised -= HandleLoad;
    }
}
