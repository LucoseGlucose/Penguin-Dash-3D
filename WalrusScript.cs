using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WalrusScript : MonoBehaviour
{
    List<GameObject> platformsList;
    public GameObject[] platformsArray;
    public Animator animCtrl;
    public float tickT;
    bool canTick;
    public float sinkSpeed;
    int num;
    public int winNumber;
    bool canSink;
    bool canAttack;
    public float attackInterval;
    Vector3 attackPos;
    PlayerController player;
    public Transform homePos;
    float lerpT;
    public float lerpSpeed;
    public GameObject flag;
    bool shouldAttack;
    public float maxLerpDist;
    Vector3 lookAt;
    bool beatYet;
    float beatYPos;
    bool canSwitch;
    public Slider progressSlider;
    public float startDelay;
    bool canWork;
    GameObject platformObj;
    int currentLvl;
    bool startedYet;
    public Color sliderColor;

    void Start()
    {
        platformsList = new List<GameObject>();
        player = FindObjectOfType<PlayerController>();

        canTick = true;
        canAttack = true;
        canSwitch = true;
        transform.position = homePos.position;
        beatYPos = homePos.position.y;
        ResetList();
    }
    void Wait()
    {
        canWork = true;
        lerpT = 0f;
    }
    void Update()
    {
        currentLvl = player.currentLevel;
        if (!startedYet && currentLvl == 12)
        {
            Invoke(nameof(Wait), startDelay);
            startedYet = true;
        }

        if (player.inWater | player.hitEnemy)
        {
            transform.position = homePos.position;
        }
        if (canWork)
        {
            if (!beatYet)
            {
                progressSlider.value = platformsList.Count / (float)platformsArray.Length;
                progressSlider.minValue = winNumber / (float)platformsArray.Length;

                if (canTick)
                {
                    Invoke(nameof(DropPlatform), tickT);
                    canTick = false;
                }
                SinkPlatform();

                lookAt = new Vector3(player.transform.position.x, .55f, player.transform.position.z);
                if (canAttack)
                {
                    Invoke(nameof(Attack), attackInterval);
                    lerpT = .1f;
                    lerpSpeed *= -1f;
                    canAttack = false;
                }
                transform.position = Vector3.Lerp(homePos.position, attackPos, lerpT);
                lerpT += lerpSpeed * Time.deltaTime;
                if (lerpT >= maxLerpDist && canSwitch)
                {
                    lerpSpeed *= -1f;
                    canSwitch = false;
                }
                if (lerpT < .1f)
                {
                    animCtrl.SetBool("isRunning", false);
                    transform.LookAt(lookAt);
                    canSwitch = true;
                }
            }
            else
            {
                beatYPos -= sinkSpeed * Time.deltaTime;
                transform.position = new Vector3(homePos.position.x, beatYPos, homePos.position.z);
            }
        }
    }
    void DropPlatform()
    {
        if (platformsList.Count > winNumber)
        {
            num = Random.Range(0, platformsList.Count - 1);
            platformObj = platformsList[num];
            platformsList.RemoveAt(num);
        }
        else
        {
            flag.SetActive(true);
            beatYet = true;
            progressSlider.transform.Find("Background").GetComponent<Image>().color = Color.green;
        }

        canTick = true;
    }
    void SinkPlatform()
    {
        if (platformObj != null)
        {
            platformObj.transform.Translate(Vector3.forward * sinkSpeed * Time.deltaTime);
        }
    }
    public void ResetList()
    {
        platformsList.Clear();
        beatYet = false;
        transform.position = homePos.position;
        progressSlider.transform.Find("Background").GetComponent<Image>().color = sliderColor;

        foreach (var platform in platformsArray)
        {
            platformsList.Add(platform);
            platform.SetActive(true);
            platform.transform.position = new Vector3(platform.transform.position.x, -.5f, platform.transform.position.z);
        }
    }
    void Attack()
    {
        canAttack = true;
        attackPos = new Vector3(player.transform.position.x, .55f, player.transform.position.z);
        animCtrl.SetBool("isRunning", true);
    }
}
