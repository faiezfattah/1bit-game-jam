using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private BuildDataChannel relay;
    [SerializeField] private FloatEvent pointerRelay;

    private GameObject currentHoverUI;
    private ButtonDataHolder currentBtnData;

    public void SendBuild(GameObject build) {
        if (build == null) Debug.Log("sending null build");
        relay.RaiseEvent(build.GetComponent<Build>());
        CloseHoverUI();
    }

    public void OnPointerEnter(PointerEventData data) {
        GameObject btn = data.pointerEnter; 
        if (btn.TryGetComponent<ButtonDataHolder>(out var btnData)) {
            OpenHoverUI(btnData);
        }
    }

    public void OnPointerExit(PointerEventData data) {
        CloseHoverUI();
    }

    private void OpenHoverUI(ButtonDataHolder btnData) {
        if (btnData == currentBtnData) return;

        CloseHoverUI(); 

        currentBtnData = btnData;
        currentHoverUI = btnData.hoverUI;
        currentHoverUI.SetActive(true);
        pointerRelay.RaiseEvent(btnData.range);
    }

    private void CloseHoverUI() {
        if (currentHoverUI != null) {
            currentHoverUI.SetActive(false);
            currentHoverUI = null;
        }
        currentBtnData = null;
        pointerRelay.RaiseEvent(0);
    }
}