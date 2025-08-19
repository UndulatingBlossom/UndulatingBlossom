using UnityEngine;

public class SampleSceneInit: MonoBehaviour {

    // Assign in Inspector
    public GameObject audioManagerPrefab;
    public GameObject playerManagerPrefab;
    public GameObject localeManagerPrefab;
    public GameObject menuManagerPrefab;
    public Transform felixSpawnPoint;
    public Transform annaSpawnPoint;
    //

    private bool playersSpawned = false;

    public void Start() {
        // Debug.Log("Sample Scene Initializer: Start");

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
            MenuManager.instance.OpenMenu(MenuManager.Menu.CharacterSelection);
        }
    }

    public void Update() {
        if (!playersSpawned && PlayerManager.instance.IsReady()) {
            PlayerManager.instance.SetPosition(CharacterName.Felix, felixSpawnPoint.transform.position);
            PlayerManager.instance.SetPosition(CharacterName.Anna, annaSpawnPoint.transform.position);
            playersSpawned = true;
        }
    }
}
