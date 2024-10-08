using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private BuildDataChannel relay;
    [SerializeField] private FloatEvent pointerRelay;

    private GameObject currentHoverUI;
    private bool isUIOpen;
    private ButtonDataHolder btnData;
    public void SendBuild(GameObject build)
    {
        if (build == null) Debug.Log("sending null build");
        relay.RaiseEvent(build.GetComponent<Build>());
        CloseHoverUI();
    }
    public void OnPointerEnter(PointerEventData data) {
        GameObject btn = data.hovered[0];
        btn.TryGetComponent<ButtonDataHolder>(out btnData);
        OpenHoverUI(btnData.hoverUI);
    }
    public void OnPointerExit(PointerEventData data) {
        CloseHoverUI();
    }
    private void OpenHoverUI(GameObject hoverUI) {
        if (isUIOpen) { 
            currentHoverUI.SetActive(false);
            currentHoverUI = hoverUI;
            currentHoverUI.SetActive(true);
            pointerRelay.RaiseEvent(btnData.range);
        }
        if (!isUIOpen) {
            currentHoverUI = hoverUI;
            currentHoverUI.SetActive(true);
            pointerRelay.RaiseEvent(btnData.range);
        }
    }
    private void CloseHoverUI() {
        currentHoverUI?.SetActive(false);
        currentHoverUI = null;
        isUIOpen = false;
        pointerRelay.RaiseEvent(0);
    }
}
