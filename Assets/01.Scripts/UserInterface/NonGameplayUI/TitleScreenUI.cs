using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

using TMPro;

using Penwyn.Game;
using Penwyn.Tools;

public class TitleScreenUI : MonoBehaviourPunCallbacks
{
    public TMP_InputField NickNameTxt;
    public StartMenu StartMenu;
    public bool RandomNickname;
    public bool AutoConnect;

    public override void OnEnable()
    {
        NetworkEventList.Instance.MasterConnected.OnEventRaised += OnMasterConnected;
    }

    void Start()
    {
        if (RandomNickname)
            NickNameTxt.text = Randomizer.RandomString(4);
        if (RandomNickname && AutoConnect)
            TryConnect();
    }

    public void TryConnect()
    {
        NetworkManager.Instance.NetworkSettings.NickName = NickNameTxt.text;
        NetworkManager.Instance.Connect();
    }

    private void OnMasterConnected()
    {
        gameObject.SetActive(false);
        StartMenu.gameObject.SetActive(true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        NetworkEventList.Instance.MasterConnected.OnEventRaised -= OnMasterConnected;
    }


}
