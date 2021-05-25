using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DashBoard : MonoBehaviour
{
    [SerializeField] protected DashSettings dashSettings;

    public AudioSource[] dashSongs;
    private int songCnt = 0;

    private GameManager gManager;


    [Serializable]
    protected class DashSettings
    {
        public float dashHigh;
        public float dashDistance;
        public float dashHorizon;
    }

    private void Start()
    {
        gManager = GameManager.Instance;
    }

    private void Update()
    {
        //if(!gManager.MyCar) gManager.MyCar = GameObject.FindGameObjectWithTag("Car");

        transform.position = gManager.MyCar.transform.position +
            gManager.MyCar.transform.TransformDirection(new Vector3(dashSettings.dashHorizon, dashSettings.dashHigh, dashSettings.dashDistance));
        transform.rotation = gManager.MyCar.transform.rotation;
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
