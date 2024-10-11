using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Tilemap tilemap;
    [Header("relays ----")]
    [SerializeField] private BuildDataChannel creationRelay;
    [SerializeField] private VoidChannel upgradeRelay;
    [SerializeField] private VoidChannel sellRelay;
    [SerializeField] private VoidChannel resetRelay;
    [SerializeField] private AudioChannel sfxRelay;
    [Header("player datas ----")]
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private PlayerBuild build;
    [Header("uis ----")]
    [SerializeField] private GameObject buildPickerUI;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private Pointer pointer;
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
    private void Start()
    {
        if (grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        pointer.SetOverride(false);
    }
    private void HandleMouseDown()
    {
        //Debug.Log("mouse down on: " + ReadSelectecGrid());
        bool hasTurret = build.CheckTurret(pointer.GetGridLocation());

        //if (!isUIOpen) selectedLocation = pointer.GetGridLocation();
        if (isUIOpen && !IsPointerOverUIElement()) {
            CloseMenu();
            return;
        }
        else selectedLocation = pointer.GetGridLocation();

        if (tilemap.GetTile(selectedLocation) != null) {
            PlaySFX(buildFailed);
            return;
        }

        if (hasTurret == false && !isUIOpen)
            OpenTurretMenu(pointer.GetWorldLocation());
        else OpenUpgradeMenu(pointer.GetWorldLocation());
    }
    public void OpenUpgradeMenu(Vector3 gridLocation)
    {
        if (!isUIOpen){
            pointer.SetOverride(true, selectedLocation);
            isUIOpen = true;

            BuildData data = build.GetBuild(selectedLocation);
            if (data == null) Debug.Log("upgrade data null");

            pointer.EnableCircle(data.range);
            int price = 0;
            if (data.nextData != null) price = data.nextData.coalPrice;
            upgradeUI.GetComponent<UpgradeMenu>().Setup(price, data.level);
            currentUI = Instantiate(upgradeUI, gridLocation, Quaternion.identity, UICanvas.transform);

            PlaySFX(openMenu);
        }
    }
    private void OpenTurretMenu(Vector3 gridLocation) {
        if (!isUIOpen) {
            isUIOpen = true;
            currentUI = Instantiate(buildPickerUI, gridLocation, Quaternion.identity, UICanvas.transform);
            Debug.Log("Opening: " + currentUI);

            PlaySFX(openMenu);

            pointer.SetOverride(true, selectedLocation);
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
            PlaySFX(buildSuccess);
        }
        if (!tryPayment)
            PaymentFailed();

        CloseMenu();
    }
    private void SellTurret()
    {
        BuildData data = build.buildPlacement[selectedLocation];
        if (data == null) Debug.Log("selling data null");

        economy.AddCoal(Mathf.FloorToInt(data.coalPrice / 2));
        economy.AddIron(Mathf.FloorToInt(data.ironPrice / 2));

        build.RemoveBuild(selectedLocation);
        Destroy(GameObjectPlacement[selectedLocation]);
        CloseMenu();
    }
    private void PaymentFailed()
    {
        PlaySFX(buildFailed);
    }
    private bool IsPointerOverUIElement() {
        return EventSystem.current.IsPointerOverGameObject();
    }
    private void CloseMenu() {
        if (!isUIOpen) return;

        if (isUIOpen) {
            isUIOpen = false;
            if (currentUI != null) {
                Destroy(currentUI);
            }
            currentUI = null;
            pointer.SetOverride(false);
            pointer.DisableCircle();
        }
    }
    private void HandleReset() {
        ClearExistingBuildings();

        foreach (Vector3Int Key in build.buildPlacement.Keys) {
            GameObject building = build.buildPlacement[Key].prefabs;
            building.GetComponent<Build>().data = build.buildPlacement[Key];
            Instantiate(building, grid.GetCellCenterWorld(Key), Quaternion.identity);
            GameObjectPlacement.Add(Key, building);
        }
        CloseMenu();
        GameObjectPlacement.Clear();
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

    private void PlaySFX(AudioClip clip) {
        sfxRelay.RaiseEvent(clip);
    }

    private void OnEnable()
    {
        inputReader.MouseClickEvent += HandleMouseDown;
        inputReader.MoveEvent += CloseOnMove;
        creationRelay.OnEventRaised += PlaceBuild;
        upgradeRelay.OnEvenRaised += RequestUpgrade;
        sellRelay.OnEvenRaised += SellTurret;
        resetRelay.OnEvenRaised += HandleReset;
    }
    private void OnDisable()
    {
        inputReader.MouseClickEvent -= HandleMouseDown;
        inputReader.MoveEvent -= CloseOnMove;
        creationRelay.OnEventRaised -= PlaceBuild;
        upgradeRelay.OnEvenRaised -= RequestUpgrade;
        sellRelay.OnEvenRaised -= SellTurret;
        resetRelay.OnEvenRaised -= HandleReset;
    }
}
