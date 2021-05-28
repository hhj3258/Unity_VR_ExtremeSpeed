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


    //[SerializeField] private Vector3 myPos;

    //[Serializable]
    //private class DashSettings
    //{
    //    public float dashHigh;
    //    public float dashDistance;
    //    public float dashHorizon;

    //    public float rotX;
    //    public Vector3 dashScale;
    //}


    private void Start()
    {


    }

    private void Update()
    {
        //transform.position = gManager.MyCar.transform.position + gManager.MyCar.transform.TransformDirection(new Vector3(dashSettings.dashHorizon, dashSettings.dashHigh, dashSettings.dashDistance));

        //transform.localScale = dashSettings.dashScale;

        //transform.rotation = gManager.MyCar.transform.rotation;

    }


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
