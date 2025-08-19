using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class LocaleManager : MonoBehaviour {

    public static LocaleManager instance;
    private enum Language{ English, German };
    private Language currLang = Language.English;
    private string langKey = "en";
    // private Language currLang = Language.German;
    // private string langKey = "de";
    private Dictionary<string, Dictionary<string, string>> localizedText = new();
    private Dictionary<TextMeshProUGUI, string> originalTexts = new();
    private TextMeshProUGUI[] tmpTexts;

    public void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    // <root>/Assets/StreamingAssets
    public void Start() {
        ReadLocalesFile(Path.Combine(Application.streamingAssetsPath, "locales.csv"));
    }

    // debugging helper
    private void PrintLocalesFile() {
        foreach (var languagePair in localizedText) {
            Debug.Log($"Language: {languagePair.Key}");
            foreach (var entry in languagePair.Value) {
                Debug.Log($"    Key: {entry.Key}, Value: {entry.Value}");
            }
        }
    }

    // pd.read_csv("Assets/locales.csv") but worse
    private void ReadLocalesFile(string filePath) {
        try {
            using StreamReader reader = new StreamReader(filePath);
            string line;
            string[] headers = null;
            int lineNumber = 0;

            if ((line = reader.ReadLine()) != null) {
                headers = line.Split(';');
                lineNumber++;
            }

            while ((line = reader.ReadLine()) != null) {
                lineNumber++;
                string[] values = line.Split(';');

                if (headers == null || values.Length != headers.Length) {
                    Debug.LogError($"CSV file is malformed at line {lineNumber}: {line}");
                    return;
                }

                // first column should be english
                int index_en = Array.IndexOf(headers, "en");
                string key = values[index_en];


                for (int i = 1; i < headers.Length; i++) {
                    string languageId = headers[i].Trim();
                    string text = values[i].Trim();

                    if (!localizedText.ContainsKey(languageId)) {
                        localizedText[languageId] = new Dictionary<string, string>();
                    }

                    localizedText[languageId][key] = text;
                }
            }
            // Debug.Log($"{lineNumber} lines parsed succesfully");

        } catch (Exception e) {
            Debug.LogError("Error reading CSV file: " + e.Message);
        }
    }

    public string Localize(string term, string langId = null) {
        langId ??= langKey; // langId or langKey

        // exceptions, good
        // if (langId == "en" || !Regex.IsMatch(term, @"\p{L}")) return term; // all unicode letters üé
        if (langId == "en" || !Regex.IsMatch(term, @"[a-zA-Z]")) return term; // ascii letters

        // exceptions, bad
        if (!localizedText.ContainsKey(langId) || !localizedText[langId].ContainsKey(term)) {
            // reduce number of useless logs
            if (localizedText[langId].ContainsValue(term)) return term;

            Debug.LogWarning($"Missing translation for term '{term}' in language '{langId}'.");
            return term; // Fallback to the input
        }

        return localizedText[langId][term];
    }

    // for now, rotate through options
    public void UpdateLanguage() {
        currLang = (currLang == Language.English) ? Language.German : Language.English;
        langKey = (currLang == Language.English) ? "en" : "de";
    }

    public void SetMenuLocalization(MonoBehaviour menu, TextMeshProUGUI langText = null) {
        // Debug.Log("Localizing " + menu.name.ToString());
        tmpTexts = menu.GetComponentsInChildren<TextMeshProUGUI>(true); // + inactive
        foreach (TextMeshProUGUI tmpText in tmpTexts) {
            // Store original text
            if (!originalTexts.ContainsKey(tmpText)) {
                originalTexts[tmpText] = tmpText.text;
            }
            tmpText.text = Localize(tmpText.text);
        }
        // fix the language button in settings
        if (menu is SettingsMenu) {
            langText.text = $"{Localize("Language")}: {langKey.ToUpper()}";
        }
    }

    public void ResetMenuLocalization() {
        foreach (TextMeshProUGUI tmpText in tmpTexts) {
            tmpText.text = originalTexts[tmpText];
        }
    }
}
