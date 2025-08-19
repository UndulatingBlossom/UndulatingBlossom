using UnityEngine;

public class MainMenuInit : MonoBehaviour {

    // Assign in Inspector
    public GameObject audioManagerPrefab;
    public GameObject playerManagerPrefab;
    public GameObject localeManagerPrefab;
    public GameObject menuManagerPrefab;
    //

    public void Start() {
        // Debug.Log("Main Menu Initializer: Start");

        // Ensure that all manager singletons are available
        if (LocaleManager.instance == null) {
            Instantiate(localeManagerPrefab);
        }
        if (AudioManager.instance == null) {
            Instantiate(audioManagerPrefab);
        }
        if (PlayerManager.instance == null) {
            Instantiate(playerManagerPrefab);
        }
        if (MenuManager.instance == null) {
            Instantiate(menuManagerPrefab);
        }

        MenuManager.instance.OpenMenu(MenuManager.Menu.Main);
    }
}
