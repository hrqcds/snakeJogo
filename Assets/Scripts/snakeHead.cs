using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeHead : MonoBehaviour
{

    public gameController gameController;

    private void OnTriggerEnter2D(Collider2D col) {
        
        switch(col.gameObject.tag){

            case "Food":
                gameController.eat();
                break;
                    case "Body":
                        gameController.gameOver();
                        break;
                    

        }

    }

}
