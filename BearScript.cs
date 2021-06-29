using UnityEngine;

public class BearScript : MonoBehaviour
{
    public LayerMask playerMask;
    public float speed;
    CharacterController cC;
    public float detectRange;
    PlayerController player;
    public Animator animCtrl;
    public float rotateT;
    bool canTurn;
    public float turnAngle;
    public Transform homePos;
    Vector3 playerDir;
    public float waterCheckDist;
    Ray waterRay;
    public LayerMask waterMask;
    public Transform waterCheckPos;
    RaycastHit waterInfo;
    Collider[] wallCheck;

    void Start()
    {
        cC = GetComponent<CharacterController>();
        speed += Random.Range(-.5f, .5f);
        player = FindObjectOfType<PlayerController>();
        canTurn = true;
        transform.position = homePos.position;
        transform.rotation = homePos.rotation;
    }
    void Update()
    {
        wallCheck = Physics.OverlapSphere(waterCheckPos.position, 1f, player.groundcheckmask);

        if (Physics.CheckSphere(transform.position, detectRange, playerMask) && !player.beatLevel)
        {
            playerDir = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(playerDir);
            if (wallCheck.Length == 0)
            {
                Invoke(nameof(MoveOffset), .2f);
            }
        }
        else
        {
            transform.position = homePos.position;
            animCtrl.SetBool("foundPlayer", false);
            if (canTurn)
            {
                Invoke(nameof(Rotate), rotateT);
                canTurn = false;
            }
        }
        if (player.hitEnemy | player.inWater | player.isSliding)
        {
            Invoke(nameof(GoBack), player.enemyHitT);
        }
        if (Physics.SphereCast(waterCheckPos.position, waterCheckDist, -homePos.transform.up, out waterInfo)) 
        {
            if (waterInfo.collider.CompareTag("Water"))
            {
                GoBack();
            }
        }
    }
    void MoveOffset()
    {
        animCtrl.SetBool("foundPlayer", true);
        cC.Move(transform.forward.normalized * speed * Time.deltaTime);
    }
    void Rotate()
    {
        transform.Rotate(Vector3.up * Random.Range(-turnAngle, turnAngle));
        canTurn = true;
    }
    void GoBack()
    {
        transform.position = homePos.position;
        transform.rotation = homePos.rotation;
    }
}
