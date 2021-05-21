using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DashBoard : MonoBehaviour
{

    public AudioSource[] dashSongs;
    private int songCnt = 0;

    [SerializeField]
    protected DashSettings dashSettings;

    [Serializable]
    protected class DashSettings
    {
        public float dashHigh;
        public float dashDistance;
        public float dashHorizon;
    }

    private GameObject focus;
    private void Start()
    {
        focus = GameObject.FindGameObjectWithTag("Car");
    }

    private void Update()
    {
        if(!focus) focus = GameObject.FindGameObjectWithTag("Car");

        transform.position = focus.transform.position + 
            focus.transform.TransformDirection(new Vector3(dashSettings.dashHorizon, dashSettings.dashHigh, dashSettings.dashDistance));
        transform.rotation = focus.transform.rotation;
    }

    public void SongChanger()
    {
        if (songCnt < dashSongs.Length)
        {
            transform.GetComponentInChildren<Text>().text = dashSongs[songCnt].name;

            if (songCnt > 0)
                dashSongs[songCnt - 1].Stop();

            dashSongs[songCnt].Play();


            songCnt++;
        }
        else
        {
            dashSongs[songCnt - 1].Stop();
            songCnt = 0;
        }
    }
}
