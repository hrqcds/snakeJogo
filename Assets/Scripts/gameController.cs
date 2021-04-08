using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    
    public enum Direction{

        ESQUERDA, DIREITA, CIMA, BAIXO

    };

    public Text txtScore;
    public Text txtHiScore;
    private int score;
    private int hiScore;

    public int colunas = 29;
    public int linhas = 15;

    [Header("Prefabs")]

    public Transform snakeFood;
    public GameObject PrefabSnakeBody;

    [Header("Configurações da cobra")]
    public float delayStep; //Tempo entre um passo e outro
    public float step; //Quantidade de movimento a cada passo
    public Transform snakeHead;

    public List<Transform> snakeBody;

    private Vector3 lastPosition;

    public Direction moveDirection; 

    
    public GameObject panelGameOver;
    public GameObject panelTitle;
    
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("moveSnake");
        setFood();
        hiScore = PlayerPrefs.GetInt("Hi-Score");
        txtHiScore.text = "Hi-Score" + hiScore.ToString();

    
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            moveDirection = Direction.CIMA;
            snakeHead.rotation = Quaternion.Euler(0,0,-90);
        }
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            moveDirection = Direction.BAIXO;
            snakeHead.rotation = Quaternion.Euler(0,0,90);
            
        }
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            moveDirection = Direction.ESQUERDA;
            snakeHead.rotation = Quaternion.Euler(0,0,0);

        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            moveDirection = Direction.DIREITA;
            snakeHead.rotation = Quaternion.Euler(0,0,180);

        } 

    }

    IEnumerator moveSnake(){

        yield return new WaitForSeconds(delayStep);

        Vector3 nexPos = Vector3.zero;

        switch(moveDirection){

            case Direction.CIMA:
                nexPos = Vector3.up; 
                break;
                case Direction.BAIXO:
                    nexPos = Vector3.down;
                    break;
                        case Direction.ESQUERDA:
                        nexPos = Vector3.left;
                        break;
                            case Direction.DIREITA:
                            nexPos = Vector3.right;
                            break;

        }

        nexPos *= step;
        lastPosition = snakeHead.position;
        snakeHead.position += nexPos;

        foreach(Transform t in snakeBody){
            Vector3 temp = t.position;
            t.position = lastPosition;
            lastPosition = temp;
            t.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        StartCoroutine("moveSnake");


    }

    public void eat(){

        Vector3 bodyPosition = snakeHead.position;
        if(snakeBody.Count > 0){

            bodyPosition = snakeBody[snakeBody.Count - 1].position;

        }

        GameObject temp = Instantiate(PrefabSnakeBody, bodyPosition, transform.localRotation );
        snakeBody.Add(temp.transform);
        score += 10;
        txtScore.text = "Pontos: " + score.ToString(); 
        setFood();        


    }

    public void setFood(){

        int x = Random.Range((colunas-1) / 2 * -1,(colunas-1)/2);
        int y = Random.Range((linhas-1) / 2 * -1, (linhas-1)/2);

        snakeFood.position = new Vector2(x * step, y * step);

    }

    public void gameOver(){

        panelGameOver.SetActive(true);
        Time.timeScale = 0;
        if(score > hiScore){
            PlayerPrefs.SetInt("Hi-Score", score);
        }

    }

    public void jogar(){

        snakeHead.position = Vector3.zero;
        moveDirection = Direction.ESQUERDA;
        snakeHead.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        foreach(Transform t in snakeBody){

            Destroy(t.gameObject);

        }
        snakeBody.Clear();
        setFood();
        score = 0;
        txtScore.text = "Pontos: " + score.ToString();
        panelGameOver.SetActive(false);
        Time.timeScale = 1;        

    }



    

}
