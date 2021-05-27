using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글톤 사용을 위한 게임매니저 static 변수 선언
    static GameManager _instance;
    
    private GameObject myCar;
    private ErrorReset[] errorReset;


    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("싱글톤 오브젝트가 null임");
            }
            return _instance;
        }
    }

    //GameManager 싱글톤 적용
    private void Awake()
    {
        myCar = GameObject.FindWithTag("Car");

        if (_instance == null)
            _instance = this;

        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
            Destroy(gameObject);

        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
        
    }


    private void Start()
    {
        
        errorReset = GameObject.FindObjectsOfType<ErrorReset>();

    }

    public GameObject MyCar
    {
        get { return myCar; }
        set { myCar = value; }
    }

    public ErrorReset[] ErrorResets
    {
        get { return errorReset; }
    }


    private void Update()
    {
        if (!myCar)
        {
            myCar = GameObject.FindWithTag("Car");
        }

        //Debug.Log(myCar.name);
    }
}
