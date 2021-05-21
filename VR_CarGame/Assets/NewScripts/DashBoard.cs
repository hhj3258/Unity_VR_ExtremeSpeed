using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBoard : MonoBehaviour
{
    public GameObject myCar;

    public float h2 = 0f;
    public float d2 = 0f;
    public float l = 0f;

    public AudioSource[] dashSongs;
    private int songCnt = 0;

    private void Update()
    {
        transform.position = myCar.transform.position + myCar.transform.TransformDirection(new Vector3(l, h2, -d2 + 0.1f));
        transform.rotation = myCar.transform.rotation;
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
