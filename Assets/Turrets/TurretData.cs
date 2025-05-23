using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "Game/Build Data")]
public class BuildData : ScriptableObject
{
    [SerializeField] public GameObject prefabs;
    [SerializeField] public string turretName;
    [SerializeField] public int level = 1;
    [SerializeField] public GameObject bullet;

    [SerializeField] public float range = 2f;
    [SerializeField] public float attackInterval = 1f;

    [SerializeField] public float bulletSpeed = 1f;
    [SerializeField] public float bulletLifeTime = 2f;
    [SerializeField] public float rotationSpeed = 30f;
    [SerializeField] public int damage = 1;

    [SerializeField] public int coalPrice = 10;
    [SerializeField] public int ironPrice = 1;

    [SerializeField] public BuildData nextData;
}
