using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject mainMenu;
    private void DisableTutorial(Vector2 dir) {
        if (!mainMenu.activeInHierarchy)
            gameObject.SetActive(false);
    }
    private void OnEnable() {
        inputReader.MoveEvent += DisableTutorial;
    }    private void OnDisable() {
        inputReader.MoveEvent -= DisableTutorial;
    }
}
