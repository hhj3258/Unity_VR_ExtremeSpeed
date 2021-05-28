using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelector : MonoBehaviour
{
    public GameObject head;
    public GameObject[] selectedCar;
    GameObject createdCar;

    [SerializeField] private Material carMaterial;

    private void Start()
    {

        Instantiate(createdCar = selectedCar[Lobby.cnt], transform.position, transform.rotation).transform.parent = head.transform;
        //head.transform.GetChild(0).Find(selectedCar[Lobby.cnt].name).Find("body").GetComponent<Renderer>().material.color = Lobby.selectedColor;

        //var selectedCarName = gManager.MyCar.transform;

    }

    private void Update()
    {
        
        //Debug.Log(.name);
        if (createdCar)
        {
            switch (Lobby.cnt)
            {
                case 0:
                    //var temp = createdCar.transform.GetChild(1).GetChild(2);
                    //Debug.Log(temp);
                    //temp.GetComponent<Renderer>().material.color = Lobby.selectedColor;
                    carMaterial.color = Lobby.selectedColor;
                    break;
                case 1:
                    createdCar.transform.Find("Racing Car 1202").Find("Body").GetComponent<Renderer>().material.color =
                        Lobby.selectedColor;
                    break;
                case 2:
                    createdCar.transform.Find("CARRERA").GetComponent<Renderer>().material.color =
                        Lobby.selectedColor;
                    createdCar.transform.Find("CARRERA").Find("left_door_gloss").GetComponent<Renderer>().material.color =
                        Lobby.selectedColor;
                    createdCar.transform.Find("CARRERA").Find("right_door_gloss").GetComponent<Renderer>().material.color =
                        Lobby.selectedColor;
                    break;
                case 3:
                    createdCar.transform.GetComponent<Renderer>().material.color =
                        Lobby.selectedColor;
                    break;
            }
        }
    }
}
