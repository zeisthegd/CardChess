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
    [SerializeField] TMP_InputField nickNameTxt;
    [SerializeField] bool randomNickname;
    [SerializeField] bool autoConnect;

    void Start()
    {
        if (randomNickname)
            nickNameTxt.text = Randomizer.RandomString(4);
        if (randomNickname && autoConnect)
            TryConnect();
    }

    public void TryConnect()
    {
        NetworkManager.Instance.NetworkSettings.NickName = nickNameTxt.text;
        NetworkManager.Instance.Connect();
    }


}
