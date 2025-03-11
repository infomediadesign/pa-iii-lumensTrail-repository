using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{

    public static LocalizationManager instance { get; private set; }
    private bool active = false;
    [HideInInspector] public int currentLocaleID;

    private void Awake()
    {
        if (!Application.isPlaying) return;

        if (instance == null)
        {
            instance = this;
            ChangeLocale(0);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        gameObject.SetActive(true);
    }

    public void ChangeLocale(int _localeID)
    {
        if (active) return;
        StartCoroutine(SetLocale(_localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        FindObjectOfType<ALMenu>()?.SwitchButtonSprites(_localeID);
        this.currentLocaleID = _localeID;
        active = false;
    }
}
