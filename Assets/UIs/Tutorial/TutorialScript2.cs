using UnityEngine;

public class TutorialScript2 : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject page1;

    private float coolDown;
    private bool isCoolingDown = false;
    [SerializeField] private float coolDownTime = 3f;
    private void Update() {
        if (isCoolingDown) coolDown += Time.deltaTime;
    }
    private void DisableTutorial(Vector2 dir) {
        if (!page1.activeInHierarchy) {
            isCoolingDown = true;
        }
        if (!mainMenu.activeInHierarchy && coolDown >= coolDownTime)
            gameObject.SetActive(false);
    }
    private void OnEnable() {
        inputReader.MoveEvent += DisableTutorial;
    }    private void OnDisable() {
        inputReader.MoveEvent -= DisableTutorial;
    }
}
