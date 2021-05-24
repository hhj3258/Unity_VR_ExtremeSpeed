using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelector : MonoBehaviour
{
    public GameObject head;
    public GameObject[] selectedCar;
    private void Start()
    {
        Instantiate(selectedCar[Lobby.cnt], transform.position, transform.rotation).transform.parent= head.transform;
    }

    private void Update()
    {
        
    }
}
