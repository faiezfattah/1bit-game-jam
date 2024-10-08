using UnityEngine;

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
    }
}
