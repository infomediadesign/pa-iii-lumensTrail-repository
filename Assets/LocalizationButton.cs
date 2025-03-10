using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private int languageID;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null && LocalizationManager.instance != null)
        {
            button.onClick.AddListener(() => LocalizationManager.instance.ChangeLocale(languageID));
        }
    }
}
