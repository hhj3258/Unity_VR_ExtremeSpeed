using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelector : MonoBehaviour
{
    public GameObject head;
    public GameObject[] selectedCar;



    private void Awake()
    {
        

        Instantiate(selectedCar[Lobby.cnt], transform.position, transform.rotation).transform.parent = head.transform;
        //head.transform.GetChild(0).Find(selectedCar[Lobby.cnt].name).Find("body").GetComponent<Renderer>().material.color = Lobby.selectedColor;
        var selectedCarName = head.transform.GetChild(0).Find(selectedCar[Lobby.cnt].name);

        switch (Lobby.cnt)
        {
            case 0:
                selectedCarName.Find("body").GetComponent<Renderer>().material.color = Lobby.selectedColor;
                break;
            case 1:
                selectedCarName.Find("Racing Car 1202").Find("Body").GetComponent<Renderer>().material.color =
                    Lobby.selectedColor;
                break;
            case 2:
                selectedCarName.Find("CARRERA").GetComponent<Renderer>().material.color =
                    Lobby.selectedColor;
                selectedCarName.Find("CARRERA").Find("left_door_gloss").GetComponent<Renderer>().material.color =
                    Lobby.selectedColor;
                selectedCarName.Find("CARRERA").Find("right_door_gloss").GetComponent<Renderer>().material.color =
                    Lobby.selectedColor;
                break;
            case 3:
                selectedCarName.GetComponent<Renderer>().material.color =
                    Lobby.selectedColor;
                break;

        }
    }

    private void Update()
    {
        
    }
}
