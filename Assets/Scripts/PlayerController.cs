using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private GameObject mainCamera;

    float speed = 8;
    float horizontalInput;
    float verticalInput;
    float xRange = 35;
    float yRange = 32;

    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;
    public SpriteRenderer healSprite;
    [SerializeField] private Sprite[] cheeseSprites;
    [SerializeField] private Sprite[] oakSprites;
    public string spriteChoice;

    public TextMeshProUGUI score;
    public TextMeshProUGUI finalScore;
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public Human human;

    public List<GameObject> lives;
    public List<Human> enemyList;

    public bool isGameOver;

    private int scoreNum;
    float countTime;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        healSprite = GameObject.Find("Heal Pool").GetComponent<SpriteRenderer>();
        scoreNum = 0;
        isGameOver = false;
        mainCamera = GameObject.Find("Main Camera");
        spriteChoice = PlayerPrefs.GetString("sprite");
        enemyList = new List<Human>();

        if (spriteChoice == "cheese")
        {
            playerSprite.sprite = cheeseSprites[0];
        }
        else if (spriteChoice == "oak")
        {
            playerSprite.sprite = oakSprites[0];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (spriteChoice == "cheese")
        {
            // Change direction of sprite based on input //

            // left
            if (horizontalInput < 0)
            {
                playerSprite.sprite = cheeseSprites[2];
            }

            // right

            if (horizontalInput > 0)
            {
                playerSprite.sprite = cheeseSprites[1];
            }

            // down
            if (verticalInput < 0)
            {
                playerSprite.sprite = cheeseSprites[0];
            }

            // up
            if (verticalInput > 0)
            {
                playerSprite.sprite = cheeseSprites[3];
            }

            healSprite.sprite = oakSprites[2];
        }
        else if (spriteChoice == "oak")
        {
            // Change direction of sprite based on input //

            // left
            if (horizontalInput < 0)
            {
                playerSprite.sprite = oakSprites[2];
            }

            // right

            if (horizontalInput > 0)
            {
                playerSprite.sprite = oakSprites[1];
            }

            // down
            if (verticalInput < 0)
            {
                playerSprite.sprite = oakSprites[0];
            }

            // up
            if (verticalInput > 0)
            {
                playerSprite.sprite = oakSprites[3];
            }

            healSprite.sprite = cheeseSprites[2];
        }


        if (!isGameOver)
        {
            // Player input
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector2.up * speed * Time.deltaTime * verticalInput, Space.World);
            transform.Translate(Vector2.right * speed * Time.deltaTime * horizontalInput, Space.World);

            checkBounds();
        }
        // toggle Win screen if 20 points gotten
        if (scoreNum == 20)
        {
            ToggleWin();
        }

        // toggle end game if all lives lost
        if (!lives[0].gameObject.activeInHierarchy)
        {
            // toggle end game screen
            toggleEndGame();
        }
    }

    // Check if player has run into catnip
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Catnip"))
        {
            // destroy catnip
            Destroy(collision.gameObject);

            // increase score
            scoreNum++;

            // update text
            score.text = "Catnips: " + scoreNum;

            // increase player speed
            speed = 20;
            StartCoroutine(CountDown());

            // If less than five enemies create new enemy
            if ( enemyList.Count < 2)
            { 
                Human go = Instantiate(human, GameObject.Find("SpawnManager").GetComponent<SpawnManager>().GenerateSpawnPos(), human.transform.rotation);
                enemyList.Add(go);
                Debug.Log(enemyList.Count);
            }
            // Otherwise increase enemy speed
            else if (enemyList[0].speed < 7)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].speed += 1;
                }
            }

        }

        
    }

    // Check if player runs into human enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            // Lose a life
            loseLife();
        }

        if (collision.gameObject.CompareTag("Heal"))
        {
            countTime = 3;
        }
    }

    void loseLife()
    {
        Debug.Log(lives.Count);

        // loop through heart list and check if last one is active
        for (int index = lives.Count - 1; index >= 0; index--)
        {
            if (lives[index].gameObject.activeInHierarchy)
            {
                Debug.Log(lives[index].name);
                // set heart inactive
                lives[index].gameObject.SetActive(false);
                break;
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Heal"))
        {
            AddLife();
        }
    }
    void AddLife()
    {

        if (countTime > 0)
        {
            countTime -= Time.deltaTime;
            Debug.Log(countTime);
        }
        else
        {
            // loop through lives list and check for soonest inactive life
            for (int index = 0; index < lives.Count; index++)
            {
                if (!lives[index].gameObject.activeInHierarchy)
                {
                    lives[index].gameObject.SetActive(true);
                    break;
                }
            }

        }
    }

    private void checkBounds()
    {
        // check if player is at left edge
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y);
        }
        // check if player is at right edge
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y);
        }
        // check if player is at the top
        if (transform.position.y < -yRange)
        {
            transform.position = new Vector3(transform.position.x, -yRange);
        }
        // check if player is at the bottom
        if (transform.position.y > yRange)
        {
            transform.position = new Vector3(transform.position.x, yRange);
        }
    }

    void toggleEndGame()
    {
        // toggle isGameOver
        isGameOver = true;

        // turn game over image on
        gameOverScreen.SetActive(true);

        Destroy(GameObject.Find("Catnip"));

        // check if score is undefined
        if (String.IsNullOrEmpty(scoreNum.ToString()))
        {
            // set score to 0
            scoreNum = 0;
        }

        // update final score text
        finalScore.text = "Final Score: " + scoreNum;

    }

    void ToggleWin()
    {
        // turn win image on
        winScreen.SetActive(true);

        // toggle isGameOver
        isGameOver = true;

        Destroy(GameObject.Find("Catnip"));

    }

    public void BackToMenu()
    {
        // go back to menu scene
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1.5f);
        speed = 8;

    }

}
