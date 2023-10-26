using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject catnip;
    public GameObject human;
    private float xRange = 30.0f;
    private float yRange = 26.5f;
    private float radius = 2f;
    private float xRandRange;
    private float yRandRange;   
    private int catnipCount;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(human, GenerateSpawnPos(), human.transform.rotation);
        Instantiate(catnip, GenerateSpawnPos(), catnip.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        catnipCount = GameObject.FindGameObjectsWithTag("Catnip").Length;

        while (catnipCount == 0)
        {
            Vector3 pos = GenerateSpawnPos();
            if (DetectCollisions(pos) == 0) continue;
            Instantiate(catnip, pos, catnip.transform.rotation);
        }
    }

    public Vector3 GenerateSpawnPos()
    { 
        xRandRange = Random.Range(-xRange, xRange);
        yRandRange = Random.Range(-yRange, yRange);
        Vector2 randPos = new Vector2(xRandRange, yRandRange);
        return randPos;
    }

    private int DetectCollisions(Vector3 pos)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
        return hitColliders.Length;
    }
}
