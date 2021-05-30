using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelector : MonoBehaviour
{
    public GameObject head;
    public GameObject[] selectedCar;
    GameObject createdCar;


    private void Start()
    {
        Instantiate(createdCar = selectedCar[Lobby.cnt], transform.position, transform.rotation).transform.parent = head.transform;
    }

}
