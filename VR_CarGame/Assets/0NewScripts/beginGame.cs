using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class beginGame : MonoBehaviour
{

    //트랙 시작 시
    [SerializeField] private TextMeshProUGUI txtCntDown;
    
    //완주 시
    [SerializeField] private TextMeshProUGUI[] txtGoal;
    [SerializeField] private GameObject[] btnLobby;

    //스타트라인
    private GameObject colStart;
    private triggerChk triggerChkStart;
    private GameObject colGoal;
    private triggerChk triggerChkGoal;
    [SerializeField] private AudioSource cntSound;
    
    private GameManager gManager;
    private CarControl carControl;

    //시간 초
    [SerializeField] private TextMeshProUGUI txtLapTime;
    private float second = 0;
    private float curTime = 0;
    private float minute = 0;
    private string resultTime;

    private Vector3 resetPos;
    private Quaternion resetRot;
    private Transform resetPoint;   //트랙 시작 포인트

    private List<ErrorReset> sectionList=new List<ErrorReset>();
    public List<ErrorReset> SectionList
    {
        get { return sectionList; }
        set { sectionList = value; }
    }

    [SerializeField] private Canvas outMenu;
    [SerializeField] private Canvas inMenu;

    int cnt = 3;    //시작 시 카운트 다운 변수

    void Awake()
    {
        minute = 0;
        curTime = 3f;
        second = 0;
        cnt = 3;

        gManager = GameManager.Instance;

        InvokeRepeating("CountDown", 1f, 1f);   //카운트 다운 인보크 1초에 1번 반복
        cntSound.Play();

        resultTime = string.Format("{0:00} : {1:00}", minute, second);

        colStart = GameObject.Find("ColStart");
        triggerChkStart = colStart.GetComponent<triggerChk>();

        colGoal = GameObject.Find("ColGoal");
        triggerChkGoal = colGoal.GetComponent<triggerChk>();
        colGoal.gameObject.SetActive(false);
        
        gManager.errorReset = FindObjectsOfType<ErrorReset>();
        //Debug.Log(gManager.errorReset.Length);

        
        resetPoint = transform.parent.parent.transform;
    }

    void Update()
    {
        if (RayClick.isReset || Input.GetKey(KeyCode.T))
        {
            if (sectionList.Count == 0)
            {
                resetPos = resetPoint.position;
                resetRot = resetPoint.rotation;
            }
            else
            {
                resetPos = SectionList[sectionList.Count - 1].MyPos;
                resetRot = SectionList[sectionList.Count - 1].MyRot;
            }

            gManager.MyCar.transform.position = resetPos;
            gManager.MyCar.transform.rotation = resetRot;
            gManager.MyCar.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        }

        //Debug.Log(colGoal);
        if (cnt <= 0 && !triggerChkGoal.IsEnter)
            LapTimer();
        else
            txtLapTime.text = resultTime;

        //카운트 다운이 끝나기 전까지 차량 속도 0
        if (gManager.MyCar)
        {
            if (cnt > 0)
            {
                //Debug.Log(gManager.MyCar.name);
                gManager.MyCar.GetComponent<Rigidbody>().velocity =
                    new Vector3(0, gManager.MyCar.GetComponent<Rigidbody>().velocity.y, 0);
            }
        }

        txtCntDown.text = cnt.ToString();

        if (cnt == 0)
        {
            txtCntDown.gameObject.SetActive(false);
        }

        //colGoal 오브젝트는 [gManager.ErrorResets.Length/2] 으로 인해 트랙 중반 쯤 깨어남
        if (triggerChkStart.IsEnter && triggerChkGoal.IsEnter)
        {
            EndGame();
        }

        if (gManager.ErrorResets[gManager.ErrorResets.Length / 2].IsEnter)      /////////////////수정필요
        {
            Debug.Log("colgoal활성화");
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

        //완주
        if (SectionList.Count == gManager.ErrorResets.Length)
        {
            for(int i = 0; i < txtGoal.Length; i++)
            {
                txtGoal[i].gameObject.SetActive(true);
                btnLobby[i].gameObject.SetActive(true);
            }

            carControl.LimitForwardSpeed = Mathf.Lerp(carControl.LimitForwardSpeed, 0f, 10f * Time.deltaTime);
            carControl.LimitBackwardSpeed = Mathf.Lerp(carControl.LimitBackwardSpeed, 0f, 10f * Time.deltaTime);

            outMenu.gameObject.SetActive(false);
            inMenu.gameObject.SetActive(false);
        }

    }


    void CountDown()
    {
        if (cnt == 1)
            CancelInvoke("CountDown");

        cnt--;
    }

    void LapTimer()
    {
        //Debug.Log("Time.timeSinceLevelLoad: " + Time.timeSinceLevelLoad);
        second = (int)(Time.timeSinceLevelLoad - curTime);
        
        if (second > 59)
        {
            curTime = Time.timeSinceLevelLoad;
            second = 0;
            minute++;

            if (minute > 59)
            {
                minute = 0;
            }
        }

        resultTime = string.Format("{0:00} : {1:00}", minute, second);
        txtLapTime.text = resultTime;

    }
}
