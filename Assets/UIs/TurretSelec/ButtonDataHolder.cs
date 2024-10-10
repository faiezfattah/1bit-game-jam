using UnityEngine;
using UnityEngine.UI;

public class ButtonDataHolder : MonoBehaviour { 
    public GameObject buildPrefab;
    public GameObject hoverUI;
    public float range;
    public float damage;

    private BuildData data;

    private void Awake() {
        data = buildPrefab.GetComponent<Build>().data;
        range = data.range;
        damage = data.damage;

        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
}
