using UnityEngine;

public class StarManager : MonoBehaviour
{
    public StarScript[] stars;
    public float starRadius;
    GameObject starObject;
    public Transform pos;
    public LayerMask starMask;
    Collider[] starColliders;
    public int starCount;
    bool gavePoints;
    public Material goldPenguinMat;
    public SkinnedMeshRenderer meshR;
    Material[] meshMats;
    UIManager UIManager;
    [TextArea(2, 5)]
    public string firstStarText;
    [TextArea(2, 5)]
    public string lastStarText;

    void Start()
    {
        starCount = 0;
        UIManager = FindObjectOfType<UIManager>();

        for (int i = 0; i < stars.Length; i++)
        {
            if (PlayerPrefs.GetInt("Star" + i, 0) == 1)
            {
                stars[i].gameObject.SetActive(false);
                starCount++;
            }
        }
    }
    void Update()
    {
        starColliders = Physics.OverlapSphere(pos.position, starRadius, starMask);
        if (starColliders.Length > 0)
        {
            if (starColliders[0].CompareTag("Star"))
            {
                starObject = starColliders[0].gameObject;
                for (int i = 0; i < stars.Length; i++)
                {
                    if (stars[i].gameObject == starObject)
                    {
                        PlayerPrefs.SetInt("Star" + i, 1);
                        stars[i].Found();
                        if (!gavePoints)
                        {
                            starCount++;
                            gavePoints = true;
                            Invoke(nameof(Wait), stars[i].waitT);
                            if (starCount == 1)
                            {
                                UIManager.FirstStar(firstStarText);
                            }
                            if (starCount == 6)
                            {
                                UIManager.FirstStar(lastStarText);
                            }
                        }
                    }
                }
            }
        }
        if (starCount >= 6)
        {
            meshMats = meshR.materials;
            meshMats[0] = goldPenguinMat;
            meshMats[1] = goldPenguinMat;
            meshR.materials = meshMats;
        }
    }
    void Wait()
    {
        gavePoints = false;
    }
}
