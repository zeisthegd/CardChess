using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class ScreenSettingsUI : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionsDropdown;

    void Start()
    {
        GetSupportedResolutions();
    }
    public void SetScreenResolution(int i)
    {
        string resolution = resolutionsDropdown.options[i].text;
        ChangeScreenResolution(resolution);
    }
    public void ChangeWindowMode()
    {
        if (Screen.fullScreen)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

    public void ChangeScreenResolution(string resolution)
    {
        Resolution[] supportedRes = Screen.resolutions;
        Resolution chosenRes = supportedRes[Array.IndexOf(supportedRes, supportedRes.ToList().Find(x => x.ToString() == resolution))];
        Debug.Log(chosenRes);
        Screen.SetResolution(chosenRes.width, chosenRes.height, Screen.fullScreenMode, chosenRes.refreshRate);
    }

    void GetSupportedResolutions()
    {
        Resolution[] supportedRes = Screen.resolutions;
        resolutionsDropdown.options = new List<TMP_Dropdown.OptionData>();
        for (int i = supportedRes.Length - 1; i >= 0; i--)
        {
            this.resolutionsDropdown.options.Add(new TMP_Dropdown.OptionData(supportedRes[i].ToString()));
        }
    }

    void OnEnable()
    {
        resolutionsDropdown.onValueChanged.AddListener(SetScreenResolution);
    }
}
