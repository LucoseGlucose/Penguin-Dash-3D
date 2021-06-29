using UnityEngine;

public class StarScript : MonoBehaviour
{
    public GameObject starParticles;
    public GameObject starMesh;
    public GameObject sphereMesh;
    public float rotSpeed;
    public float waitT;
    public AudioSource starSound;
    bool foundYet;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }
    public void Found()
    {
        if (!foundYet)
        {
            starSound.Play();
            starMesh.SetActive(false);
            sphereMesh.SetActive(false);
            starParticles.SetActive(true);
            Invoke(nameof(Wait), waitT);
            foundYet = true;
        }
    }
    void Wait()
    {
        this.gameObject.SetActive(false);
    }
}
