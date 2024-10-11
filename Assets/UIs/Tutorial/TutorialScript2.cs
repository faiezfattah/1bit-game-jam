using UnityEngine;
using UnityEngine.UIElements;

public class TutorialScript2 : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private VoidChannel togglePause;
    [SerializeField] private GameObject page1;

    private float coolDown;
    private bool isCoolingDown = false;
    [SerializeField] private float coolDownTime = 3f;
    private void Update() {
        if (isCoolingDown) coolDown += Time.unscaledDeltaTime;
    }
    private void DisableTutorial(Vector2 dir) {
        if (!page1.activeInHierarchy) {
            isCoolingDown = true;
            GetComponent<UIDocument>().enabled = true;
        }
        if (coolDown >= coolDownTime){
            gameObject.SetActive(false);
            togglePause.RaiseEvent();
        }
    }
    private void OnEnable() {
        inputReader.MoveEvent += DisableTutorial;
    }    private void OnDisable() {
        inputReader.MoveEvent -= DisableTutorial;
    }
}
