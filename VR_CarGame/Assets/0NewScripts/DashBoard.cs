using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DashBoard : MonoBehaviour
{
    //[SerializeField] private DashSettings dashSettings;
    [SerializeField] private TextMeshProUGUI[] txtMusic;

    [SerializeField] private AudioSource[] dashSongs;
    private int songCnt = 0;

    public void SongChanger()
    {

        if (songCnt < dashSongs.Length)
        {
            for(int i=0;i<txtMusic.Length;i++)
                txtMusic[i].text = dashSongs[songCnt].name;

            if (songCnt > 0)
                dashSongs[songCnt - 1].Stop();

            dashSongs[songCnt].Play();

            songCnt++;
        }
        else
        {
            for (int i = 0; i < txtMusic.Length; i++)
                txtMusic[i].text = "Play Music";

            dashSongs[songCnt - 1].Stop();

            songCnt = 0;
        }
    }
}
