using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // migth dich this manager approach
    [SerializeField] InputReader inputReader;
    private Grid grid;
    private void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }
    private void TestingInput()
    {
        Vector2 location = Mouse.current.position.value;
        location = Camera.main.ScreenToWorldPoint(location);
        Vector3Int gridLocation = grid.WorldToCell(location);
        Debug.Log("mouse down on: " + gridLocation);
    }
    private void OnEnable()
    {
        inputReader.MouseClickEvent += TestingInput;
    }
    private void OnDisable()
    {
        inputReader.MouseClickEvent -= TestingInput;
    }
}   
