using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEconomy", menuName = "Player/Economy")]
public class PlayerEconomy : ScriptableObject
{
    // TODO: need to notify at changes, use void event 
    [SerializeField] public int woodCount = 0;
    [SerializeField] public int ironCount = 0;

    public bool Pay(int woodPrice, int ironPrice)
    {
        if (woodPrice <= woodCount && ironPrice <= ironCount)
        {
            woodCount -= woodPrice;
            ironCount -= ironPrice;
            return true;
        }
        else return false;
    }
    public void AddWood(int value)
    {
        if(value > 0)
            woodCount += value;
        else Debug.Log("wood value invalid");
    }
    public void AddIron(int value)
    {
        if (value > 0)
            ironCount += value;
        else Debug.Log("iron value invalid");
    }
}
