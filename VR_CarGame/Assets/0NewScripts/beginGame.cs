using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class beginGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCntDown;
    [SerializeField] private TextMeshProUGUI txtGoal;

    [SerializeField] private triggerChk colStart;
    [SerializeField] private triggerChk colGoal;
    [SerializeField] private AudioSource cntSound;
    
    private GameManager gManager;

    private CarControl carControl;

    int cnt = 3;    //시작 시 카운트 다운 변수
    int maxEnter = 0;   //트랙의 구간 통과 시 증가

    void Awake()
    {
        gManager = GameManager.Instance;
        
        InvokeRepeating("CountDown", 1f, 1f);   //카운트 다운 인보크 1초에 1번 반복
        cntSound.Play();
    }


    // Update is called once per frame
    void Update()
    {

        if (gManager.MyCar)
        {
            if (cnt > 0)
            {
                Debug.Log(gManager.MyCar.name);
                gManager.MyCar.GetComponent<Rigidbody>().velocity =
                    new Vector3(0, gManager.MyCar.GetComponent<Rigidbody>().velocity.y, 0);
            }
        }

        txtCntDown.text = cnt.ToString();

        //카운트 다운이 끝나기 전까지 차량 속도 0
        

        if (cnt == 0)
        {
            txtCntDown.gameObject.SetActive(false);
        }

        //colGoal 오브젝트는 [gManager.ErrorResets.Length/2] 으로 인해 트랙 중반 쯤 깨어남
        if (colStart.IsEnter && colGoal.IsEnter)
        {
            EndGame();
            
            //Debug.Log("maxEnter: " + maxEnter);
            //Debug.Log("gManager.ErrorResets.Length: " + gManager.ErrorResets.Length);
        }

        if (gManager.ErrorResets[gManager.ErrorResets.Length/2].IsEnter &&
            !gManager.ErrorResets[gManager.ErrorResets.Length / 2+1].IsEnter)
        {
            colGoal.gameObject.SetActive(true);
            //Debug.Log("colGoal 셋엑티브");
        }


    }

    //ErrorResets 스크립트를 가지고 있는 각 구간에 진입 시
    //IsEnter 변수=true
    //maxEnter이 gManager.ErrorResets.Length 과 같으면 정상적으로 트랙을 완주했음을 의미
    void EndGame()
    {
        carControl = gManager.MyCar.GetComponent<CarControl>();

        if (maxEnter < gManager.ErrorResets.Length)
        {
            for (int i = 0; i < gManager.ErrorResets.Length; i++)
            {
                if (gManager.ErrorResets[i].IsEnter == false)
                {
                    maxEnter = 0;
                    break;
                }

                maxEnter++;
            }
        }

        //완주
        if (maxEnter == gManager.ErrorResets.Length)
        {
            txtGoal.gameObject.SetActive(true);
            //gManager.MyCar.GetComponent<Rigidbody>().velocity =
            //    new Vector3(Mathf.Lerp(gManager.MyCar.GetComponent<Rigidbody>().velocity.x,0f, 0.1f * Time.deltaTime), 
            //    gManager.MyCar.GetComponent<Rigidbody>().velocity.y,
            //    Mathf.Lerp(gManager.MyCar.GetComponent<Rigidbody>().velocity.z, 0f, 0.1f * Time.deltaTime));

            carControl.LimitForwardSpeed = Mathf.Lerp(carControl.LimitForwardSpeed, 0f, Time.deltaTime);
            carControl.LimitBackwardSpeed = Mathf.Lerp(carControl.LimitBackwardSpeed, 0f, Time.deltaTime);

        }
    }


    void CountDown()
    {
        if (cnt == 1)
            CancelInvoke("CountDown");

        cnt--;
    }

    
}
