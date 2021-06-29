using UnityEngine;

public class SpringScript : MonoBehaviour
{
    Animator animCtrl;
    PlayerController player;
    public LayerMask playerMask;
    public float range = .75f;
    public float springHeight;
    bool canSpring;
    public float waitT;
    public AudioSource springSound;

    private void Start()
    {
        animCtrl = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        canSpring = true;
    }
    void Update()
    {
        if (Physics.CheckSphere(transform.position, range, playerMask) && canSpring)
        {
            player.HitSpring(springHeight);
            animCtrl.Play("Spring");
            springSound.Play();
            canSpring = false;
            Invoke(nameof(Wait), waitT);
        }
    }
    void Wait()
    {
        canSpring = true;
    }
}
