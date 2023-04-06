using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
public class ScreenSettingsUI : MonoBehaviour
{
    public TMP_Dropdown ResolutionsDropdown;
    public Toggle FullScreenTgl;

    private Resolution[] supportedRes;

    private void Awake()
    {
        supportedRes = Screen.resolutions;
    }

    private void Start()
    {
        GetSupportedResolutions();
    }

    public void SetScreenResolution(int i)
    {
        string resolution = ResolutionsDropdown.options[i].text;
        ChangeScreenResolution(resolution);
    }
    public void ChangeWindowMode()
    {
        if (Screen.fullScreen)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void SwitchFullScreenMode(bool fsValue)
    {
        if (fsValue == true)
        {
            Resolution maxRes = supportedRes[supportedRes.Length - 1];
            Screen.SetResolution(maxRes.width, maxRes.height, Screen.fullScreenMode, maxRes.refreshRate);
        }
        else
        {
            Screen.SetResolution(960, 540, Screen.fullScreenMode, 60);
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void ChangeScreenResolution(string resolution)
    {
        Resolution chosenRes = supportedRes[Array.IndexOf(supportedRes, supportedRes.ToList().Find(x => x.ToString() == resolution))];
        Debug.Log(chosenRes);
        Screen.SetResolution(chosenRes.width, chosenRes.height, Screen.fullScreenMode, chosenRes.refreshRate);
    }

    void GetSupportedResolutions()
    {
        if (ResolutionsDropdown == null)
            return;
        Resolution[] supportedRes = Screen.resolutions;
        ResolutionsDropdown.options = new List<TMP_Dropdown.OptionData>();
        for (int i = supportedRes.Length - 1; i >= 0; i--)
        {
            this.ResolutionsDropdown.options.Add(new TMP_Dropdown.OptionData(supportedRes[i].ToString()));
        }
    }

    void OnEnable()
    {
        ResolutionsDropdown?.onValueChanged.AddListener(SetScreenResolution);
        FullScreenTgl?.onValueChanged.AddListener(SwitchFullScreenMode);
    }

    private void OnDisable()
    {
        FullScreenTgl?.onValueChanged.RemoveAllListeners();
    }
}
