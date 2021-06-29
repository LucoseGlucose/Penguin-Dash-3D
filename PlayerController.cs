using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    CharacterController cC;
    public float forward;
    public float sideways;
    public bool jump;
    public bool onground;
    public float speed = 8f;
    public float gravity = -9.81f;
    public Transform groundcheckpos;
    public float grounddistance = .8f;
    public LayerMask groundcheckmask;
    Vector3 downwardforce;
    public float jumpHeight;
    Vector3 jumpVelocity;
    public float smoothturnspeed;
    public GameObject penguinModel;
    public Animator animCtrl;
    public float sinkSpeed;
    public float runSpeed;
    public float runAnimSpeed;
    public float walkSpeed;
    public GameObject runParticles;
    public LayerMask waterMask;
    RaycastHit waterRayInfo;
    public Transform waterCheckPos;
    public bool inWater;
    public Camera mainCam;
    public float iceCheckDist;
    public float sinkWaitT;
    public Animator flagAnim;
    public float flagWindUpdateT = 2f;
    public float animSpeed;
    public bool beatLevel;
    public GameObject[] levels;
    public int currentLevel = 1;
    public float nextLvlWaitT;
    GameObject currentLvlObject;
    public float winRange;
    public LayerMask flagMask;
    bool nextLvlYet = false;
    public Vector3 moveDir;
    RaycastHit slideCheckRay;
    public float slideSpeed;
    public bool isSliding;
    public float coyoteTime;
    Vector3 respawnPosition;
    public float respawnTime;
    public Transform camPos;
    public float maxSlideSpeed;
    public GameObject water;
    public float enemyRange;
    public LayerMask enemyMask;
    public bool hitEnemy;
    public float enemyHitT;
    public float respawnOffset;
    public float slideDist;
    public bool resetPrefs;
    public AudioSource splashSound;
    public AudioSource hitSound;
    public AudioSource jumpSound;
    public AudioSource winSound;
    public AudioSource walkingSound;
    public AudioSource slidingSound;
    public AudioSource runningSound1;
    public AudioSource runningSound2;
    int runNum;
    Vector3 waterDist;
    UIManager uiManager;
    public Transform[] flags;
    public float crouchSpeed;
    public TextMeshProUGUI starText;
    public TextMeshProUGUI timerText;
    float lvlTimer;
    float timerMod;
    StarManager starManager;
    public float jumpTime;
    public WalrusScript walrus;
    float distToWalrus;
    public float walrusRange;
    public TextMeshProUGUI resumeText;
    public bool startedYet;
    public TextMeshProUGUI fpsCounter;
    public GameObject winScreen;

    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        cC = GetComponent<CharacterController>();
        starManager = GetComponent<StarManager>();
        speed = walkSpeed;
        Invoke(nameof(AnimateFlag), 0f);
        if (resetPrefs) PlayerPrefs.DeleteAll();
        currentLevel = PlayerPrefs.GetInt("BestLevel", 1);
        GetLevel();
        transform.position = respawnPosition;
        waterDist = new Vector3(0f, -1.75f, respawnPosition.z + 200f);
        water.transform.position = waterDist;
        runNum = 1;
        isSliding = false;
        slidingSound.enabled = false;
        uiManager.ShowTutorial(currentLevel - 1);
    }
    void Update()
    {
        GetInput();
        Gravity();
        Move();
        Jump();
        IsInWater();
        Slide();
        CheckForEnemies();
    }
    void Gravity()
    {
        if (Physics.CheckSphere(groundcheckpos.position, grounddistance, groundcheckmask) | isSliding)
        {
            onground = true;
        }
        else
        {
            Invoke(nameof(CoyoteTime), coyoteTime);
        }
        if (cC.enabled)
        {
            cC.Move(downwardforce * Time.deltaTime * Time.deltaTime);
        }
    }
    void CoyoteTime()
    {
        onground = false;
    }
    void GetInput()
    {
        if (!beatLevel)
        {
            if (!isSliding) forward = Input.GetAxis("Vertical");
            sideways = Input.GetAxis("Horizontal");
            jump = Input.GetKeyDown(KeyCode.Space);
        }
        if (!beatLevel) lvlTimer = Time.time - timerMod;
        lvlTimer -= lvlTimer % .01f;
        timerText.text = "Time:" + lvlTimer.ToString();
        starText.text = "Stars:" + starManager.starCount.ToString() + "/6";
        if (startedYet)
        {
            resumeText.text = "Resume";
        }
        float fps = 1f / Time.unscaledDeltaTime;
        fpsCounter.text = fps.ToString() + " FPS";
    }
    void Move()
    {
        moveDir = new Vector3(sideways, 0f, forward);
        if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
        {
            if (moveDir != Vector3.zero && onground && !isSliding && !beatLevel)
            {
                speed = runSpeed;
                animCtrl.SetFloat("runSpeed", runAnimSpeed);
                runParticles.SetActive(true);
                walkingSound.enabled = false;
                if (runNum > 0)
                {
                    runningSound1.enabled = true;
                    runningSound2.enabled = false;
                }
                if (runNum < 0)
                {
                    runningSound2.enabled = true;
                    runningSound1.enabled = false;
                }
            }
            else
            {
                animCtrl.SetFloat("runSpeed", 1f);
                runningSound1.enabled = false;
                runningSound2.enabled = false;
                runParticles.SetActive(false);
            }
        }
        else
        {
            animCtrl.SetFloat("runSpeed", 1f);
            runningSound1.enabled = false;
            runningSound2.enabled = false;
            runParticles.SetActive(false);
        }
        if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt) && !Input.GetKey(KeyCode.AltGr) && !isSliding)
        {
            speed = walkSpeed;
        }
        if (!onground && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt) && !Input.GetKey(KeyCode.AltGr))
        {
            speed = walkSpeed;
        }
        if (cC.enabled && !beatLevel)
        {
            cC.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        if (moveDir != Vector3.zero && inWater == false)
        {
            penguinModel.transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            animCtrl.SetBool("isWalking", true);
        }
        else animCtrl.SetBool("isWalking", false);

        if (onground && !beatLevel && moveDir != Vector3.zero && !isSliding && cC.velocity.magnitude > .25f)
        {
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                walkingSound.enabled = true;
            }
        }
        else walkingSound.enabled = false;

        if (Input.GetKey(KeyCode.LeftAlt) | Input.GetKey(KeyCode.RightAlt) | Input.GetKey(KeyCode.AltGr))
        {
            speed = crouchSpeed;
        }
    }
    void Jump()
    {
        if (!beatLevel)
        {
            if (jump)
            {
                if (onground)
                {
                    jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    animCtrl.SetBool("isJumping", true);
                    jumpSound.Play();
                    Invoke(nameof(StopJump), 1f);
                }
                else
                {
                    Invoke(nameof(JumpTime), jumpTime);
                }
            }
            jumpVelocity.y += gravity * Time.deltaTime;
            if (cC.enabled)
            {
                cC.Move(jumpVelocity * Time.deltaTime);
            }
        }
    }
    void JumpTime()
    {
        if (onground)
        {
            jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animCtrl.SetBool("isJumping", true);
            jumpSound.Play();
            Invoke(nameof(StopJump), 1f);
        }
    }
    void StopJump()
    {
        animCtrl.SetBool("isJumping", false);
    }
    void IsInWater()
    {
        if (Physics.SphereCast(waterCheckPos.position, iceCheckDist, Vector3.down, out waterRayInfo, .1f))
        {
            if (waterRayInfo.collider.CompareTag("Water"))
            {
                Invoke(nameof(WaitToSink), sinkWaitT);
            }
        }
        if (inWater)
        {
            cC.enabled = false;
            transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
            animCtrl.SetBool("Fell in Water", true);
            Invoke(nameof(Respawn), respawnTime);
        }
        if (Physics.CheckSphere(transform.position, winRange, flagMask) && onground)
        {
            BeatLevel();
        }
    }
    void WaitToSink()
    {
        if (Physics.SphereCast(waterCheckPos.position, iceCheckDist, Vector3.down, out waterRayInfo, .1f))
        {
            if (waterRayInfo.collider.CompareTag("Water"))
            {
                inWater = true;
                splashSound.enabled = true;
                splashSound.Play();
            }
        }
    }
    void Respawn()
    {
        if (inWater)
        {
            transform.position = respawnPosition;
            inWater = false;
            cC.enabled = true;
            penguinModel.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            animCtrl.SetBool("Fell in Water", false);
            animCtrl.SetBool("isSliding", false);
            isSliding = false;
            runNum *= -1;
            if (currentLevel == 12) walrus.ResetList();
        }
        if (hitEnemy)
        {
            transform.position = respawnPosition;
            hitEnemy = false;
            cC.enabled = true;
            penguinModel.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            animCtrl.SetBool("Hit Enemy", false);
            animCtrl.SetBool("isSliding", false);
            isSliding = false;
            runNum *= -1;
            if (currentLevel == 12) walrus.ResetList();
        }
    }
    void AnimateFlag()
    {
        animSpeed = Random.Range(.5f, 2f);
        flagAnim.SetFloat("Wind Speed", animSpeed);
        DoFlagAnim();
    }
    void DoFlagAnim()
    {
        Invoke(nameof(AnimateFlag), flagWindUpdateT);
    }
    public void BeatLevel()
    {
        beatLevel = true;
        uiManager.TutWait();
        if (beatLevel)
        {
            animCtrl.SetBool("BeatLevel", true);
            animCtrl.SetBool("isWalking", false);
            animCtrl.SetBool("isJumping", false);
            animCtrl.SetBool("isSliding", false);
            if (!nextLvlYet)
            {
                winSound.Play();
                respawnPosition = new Vector3(0f, 4f, transform.position.z);
                Invoke(nameof(ResetVars), nextLvlWaitT);
                nextLvlYet = true;
            }
        }
    }
    public void ResetVars()
    {
        if (currentLevel < 12)
        {
            currentLevel++;
            timerMod = Time.time;
            runNum *= -1;
            isSliding = false;
            beatLevel = false;
            nextLvlYet = false;
            cC.enabled = true;
            animCtrl.SetBool("BeatLevel", false);
            GetLevel();
            uiManager.ShowTutorial(currentLevel - 1);
        }
        else
        {
            winScreen.SetActive(true);
        }
    }
    public void GetLevel()
    {
        respawnPosition.z = flags[currentLevel - 1].position.z;
        waterDist = new Vector3(0f, -1.75f, respawnPosition.z + 200f);
        water.transform.position = waterDist;
        cC.enabled = false;
        transform.position = respawnPosition;
        cC.enabled = true;

        currentLvlObject = levels[currentLevel - 1];
        foreach(GameObject level in levels)
        {
            if (level != currentLvlObject)
            {
                level.SetActive(false);
            }
        }
        currentLvlObject.SetActive(true);

        if (currentLevel > PlayerPrefs.GetInt("BestLevel", 1))
        {
            PlayerPrefs.SetInt("BestLevel", currentLevel);
        }
        uiManager.UpdateButtons();
    }
    void Slide()
    {
        if (Physics.SphereCast(waterCheckPos.position, iceCheckDist, Vector3.down, out slideCheckRay, slideDist, groundcheckmask))
        {
            if (slideCheckRay.collider.CompareTag("Slide"))
            {
                if (speed <= maxSlideSpeed)
                {
                    speed += slideSpeed * Time.deltaTime;
                }
                isSliding = true;
                slidingSound.enabled = true;
                forward = 1f;
                animCtrl.SetBool("isSliding", true);
                animCtrl.SetBool("isWalking", false);
                animCtrl.SetBool("isJumping", false);
            }
        }
        else
        {
            isSliding = false;
            slidingSound.enabled = false;
            animCtrl.SetBool("isSliding", false);
            if (speed != walkSpeed)
            {
                speed -= slideSpeed/10f * Time.deltaTime;
            }
        }
    }
    void CheckForEnemies()
    {
        distToWalrus = Vector3.Distance(transform.position, walrus.transform.position);

        if (Physics.CheckSphere(transform.position, enemyRange, enemyMask) && !hitEnemy)
        {
            animCtrl.SetBool("Hit Enemy", true);
            animCtrl.SetBool("isWalking", false);
            animCtrl.SetBool("isJumping", false);
            hitSound.Play();
            cC.enabled = false;
            hitEnemy = true;
            Invoke(nameof(Respawn), enemyHitT);
        }
        if (distToWalrus < walrusRange && !hitEnemy)
        {
            animCtrl.SetBool("Hit Enemy", true);
            animCtrl.SetBool("isWalking", false);
            animCtrl.SetBool("isJumping", false);
            hitSound.Play();
            cC.enabled = false;
            hitEnemy = true;
            Invoke(nameof(Respawn), enemyHitT);
        }
        if (currentLevel == 12)
        {
            walrus.progressSlider.gameObject.SetActive(true);
        }
        else
        {
            walrus.progressSlider.gameObject.SetActive(false);
        }
    }
    public void HitSpring(float springHeight)
    {
        if (!beatLevel)
        {
            splashSound.enabled = false;
            jumpVelocity.y = Mathf.Sqrt(springHeight * -2f * gravity);
            animCtrl.SetBool("isJumping", true);
            Invoke(nameof(StopJump), 1f);
            jumpVelocity.y += gravity * Time.deltaTime;
            if (cC.enabled)
            {
                cC.Move(jumpVelocity * Time.deltaTime);
            }
        }
    }
}
