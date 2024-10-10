using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PlayerGameTime", menuName = "Player/GameTime")]
public class PlayerGameTime : ScriptableObject
{
    public float gameTime = 0;
    public float dayTime = 0;
    public Rotate clockTurn;
    public float maxDayTimeInSeconds = 30f;
    public int dayCount = 0;

    public UnityAction onDayOver;
 
    public void UpdateGameTime()
    {
        gameTime += Time.deltaTime;
        dayTime += Time.deltaTime;
        clockTurn = new Rotate(dayTime/maxDayTimeInSeconds * 360);

        if (dayTime > maxDayTimeInSeconds)
        {
            TriggerDayOver();
            dayTime = 0;
            dayCount++;
        }
    }
    public void ResetGameTime()
    {
        gameTime = 0;
        dayTime = 0;
        dayCount = 0;
    }
    public void TriggerDayOver()
    {
        Debug.Log("dayover triggered, " + dayCount);
        onDayOver?.Invoke();
    }
}
