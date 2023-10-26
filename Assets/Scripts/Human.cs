using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Human : MonoBehaviour
{
    public GameObject player;
    public float speed;
    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().isGameOver == false)
        {
            MoveCharacter(speed);
        }
    }

    public void MoveCharacter(float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
