using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    // Assign in Inspector
    public Button selectedButton;
    public TextMeshProUGUI langText;
    //

    public void Start() {
        // Debug.Log("Settings Menu: Start");
        ActivateMenu();
    }

    public void OnEnable() {
        // Debug.Log("Settings Menu: Enable");
        ActivateMenu();
    }

    private void ActivateMenu() {
        selectedButton.Select();
        LocaleManager.instance.SetMenuLocalization(this, langText);
    }

    public void OnDisable() {
        LocaleManager.instance.ResetMenuLocalization();
    }

    public void OpenAudioSettings() {
        MenuManager.instance.NextMenu(MenuManager.Menu.Audio);
    }

    public void OpenAssistModeMenu() {
        MenuManager.instance.NextMenu(MenuManager.Menu.AssistMode);
    }

    public void ShowControls() {
        MenuManager.instance.NextMenu(MenuManager.Menu.Controls);
    }

    public void ChangeLanguage() {
        LocaleManager.instance.UpdateLanguage();
        LocaleManager.instance.ResetMenuLocalization();
        LocaleManager.instance.SetMenuLocalization(this, langText);
    }

    public void Back() {
        MenuManager.instance.PreviousMenu();
    }
}
