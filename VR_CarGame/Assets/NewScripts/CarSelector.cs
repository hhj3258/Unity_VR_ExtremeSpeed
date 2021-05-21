using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelector : MonoBehaviour
{
    private Lobby lobby;
    
    public GameObject[] selectedCar;
    private void Start()
    {
        Instantiate(selectedCar[Lobby.cnt], new Vector3(0,0,0),Quaternion.identity);
    }

    private void Update()
    {
        
    }
}
