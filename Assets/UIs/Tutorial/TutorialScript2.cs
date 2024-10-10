using UnityEngine;

public class TutorialScript2 : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject page1;
    private void DisableTutorial(Vector2 dir) {
        if (!mainMenu.activeInHierarchy && !page1.activeInHierarchy) // TODO: fix this shit
            gameObject.SetActive(false);
    }
    private void OnEnable() {
        inputReader.MoveEvent += DisableTutorial;
    }    private void OnDisable() {
        inputReader.MoveEvent -= DisableTutorial;
    }
}
