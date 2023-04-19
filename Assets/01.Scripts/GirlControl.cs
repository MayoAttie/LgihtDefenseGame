using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlControl : MonoBehaviour
{
    Animator aniControl;
    private int animatonState = 0;
    public int electroPower = 1;
    private float moveSpeed = 0.5f;
    private float rotateSpeed = 120f;
    private float rotateSensative = 2f;
    private float roateLimit = 55f;
    private float currentRotate;
    private bool isAct = false;
    private bool isRun = false;
    private bool coroutineFlag = false;
    private bool isPlay = false;
    private bool isMenuOpen = false;

    Rigidbody myRigid;
    [SerializeField] Camera camera;
    [SerializeField] GameObject cannonObj;
    [SerializeField] Transform installPos;
    [SerializeField] GameObject Electro;
    GameManager gameMng;

    

    void Start()
    {
        aniControl = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        gameMng = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        isPlay = gameMng.isPlaying;

        if (!isPlay) return;

        Move();
        RotateX();
        RotateY();
        SetCannon();
        ElectroPos();

        //메뉴 오픈 확인
        if( ShopManager.instance != null)
        {
            isMenuOpen = ShopManager.instance.isMenuOpen;

        }
    }

    private void LateUpdate()
    {
        if (!isPlay) return;

        EventInput();
    }

    private void Move()
    {

        if(Input.GetKey(KeyCode.LeftShift)) //뛰기 속도
        {
            
            isRun = true;
            moveSpeed = 22f;
        }
        else
        {
            aniControl.SetBool("isRun", false);
            isRun = false;
            moveSpeed = 5f;
            //animatonState = 0;
            //aniControl.SetInteger("animeGirlControl", animatonState);
        }

        if(isRun && Input.GetKey(KeyCode.W))   // 뛰기
        {
            //걷기 이벤트 호출, 인트 스테이트 -1
            aniControl.SetBool("isRun", true);
            animatonState = -1;
            aniControl.SetInteger("animeGirlControl", animatonState);

            
            isAct = true;
            float inputVertical = Input.GetAxis("Vertical");
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 moveV = forward * inputVertical;
            Vector3 moveVelocity = moveV.normalized * moveSpeed;

            myRigid.MovePosition(transform.position + moveVelocity * Time.deltaTime);
            gameMng.isCursor = true;
        }
        else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)) && !isRun)   //걷기
        {
            isAct = true;
            animatonState = -1;
            aniControl.SetInteger("animeGirlControl", animatonState);

            float inputHorizontal = Input.GetAxis("Horizontal");
            float inputVertical = Input.GetAxis("Vertical");

            // 캐릭터의 로컬 좌표계에서의 forward 벡터를 월드 좌표계로 변환.
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            Vector3 moveH = right * inputHorizontal;
            Vector3 moveV = forward * inputVertical;
            Vector3 moveVelocity = (moveH + moveV).normalized * moveSpeed;

            myRigid.MovePosition(transform.position + moveVelocity * Time.deltaTime);
            gameMng.isCursor = true;

            //워킹 이벤트 처리
            aniControl.SetFloat("vertical", inputVertical);
            aniControl.SetFloat("horizontal", inputHorizontal);

        }
        else
        {
            isAct = false;
            aniControl.SetFloat("vertical", 0);
            aniControl.SetFloat("horizontal", 0);
            //animatonState = 0;
            //aniControl.SetInteger("animeGirlControl", animatonState);
        }

    }

    private void RotateX()
    {
        float inputMouseX = Input.GetAxis("Mouse X");
        float rotationY = inputMouseX * rotateSensative;

        // 캐릭터를 회전시킵니다.
        transform.Rotate(0f, rotationY, 0f);
    }

    private void RotateY()
    {
        
        float inputMouseY = Input.GetAxis("Mouse Y");
        float cameraRotation = inputMouseY * rotateSensative;
        currentRotate -= cameraRotation;
        currentRotate = Mathf.Clamp(currentRotate, -roateLimit, roateLimit);

        // 카메라를 회전시킵니다.
        Quaternion cameraRotationX = Quaternion.Euler(currentRotate, 0f, 0f);
        camera.transform.rotation = transform.rotation * cameraRotationX;
    }

    private void EventInput()
    {
        if(!isAct)
        {

            if (Input.GetKey(KeyCode.Q))    //댄스
            {
                animatonState = 3;
                aniControl.SetInteger("animeGirlControl", animatonState);
            }
            else    //아이들
            {
                animatonState = 0;
                aniControl.SetInteger("animeGirlControl", animatonState);
            }
        }


    }

    private void SetCannon()
    {

        if (Input.GetButtonDown("Fire1") && !isRun)
        {
            
            if(isMenuOpen)
            {
                return;
            }

            RaycastHit hit;

            // 설치 위치에서 아래로 광선을 쏴서 맞은 오브젝트가 Ground 태그인지 확인합니다.
            if (Physics.Raycast(installPos.position, Vector3.down, out hit, Mathf.Infinity) && hit.transform.CompareTag("Ground"))
            {
                // 설치 위치에 다른 오브젝트가 있는지 확인합니다.
                Collider[] colliders = Physics.OverlapSphere(installPos.position, 0.5f);



                if (colliders.Length == 1) // 설치 위치에 다른 오브젝트가 없는 경우
                {
                    // 캐논을 설치하는 코드를 작성합니다.
                    cannonObj.transform.position = installPos.position;
                    cannonObj.transform.rotation = installPos.rotation;
                }
            }


        }
    }

    private void ElectroPos()
    {
        if(Input.GetButtonDown("Fire2") && !isRun && animatonState != 3)
        {
            isAct = true;
            StartCoroutine(FireAct2());
        }
        else
            isAct= false;
    }

    IEnumerator HalfSeceondDelay()
    {
        if(!coroutineFlag)
        {
            coroutineFlag= true;
            yield return new WaitForSecondsRealtime(0.5f);
            coroutineFlag= false;

        }
    }

    IEnumerator FireAct2()
    {
        if(!coroutineFlag)
        {
            coroutineFlag= true;
            gameMng.isCursor = true;

            animatonState = 2;
            aniControl.SetInteger("animeGirlControl", animatonState);

            GameObject obj = Instantiate(Electro, installPos.position, installPos.rotation);
            Destroy(obj, 1.1f);
            yield return new WaitForSecondsRealtime(0.5f);

            ElectroCrushCheek electroAttack = installPos.gameObject.GetComponent<ElectroCrushCheek>();
            electroAttack.setCheek(true, electroPower);

            coroutineFlag = false;
        }
    }

}
