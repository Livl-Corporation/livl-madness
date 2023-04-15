using Unity.VisualScripting;
using UnityEngine;
 
public class AutomaticDoor : MonoBehaviour
{
    public GameObject movingLeftDoor;
    public GameObject movingRightDoor;
     
    private float maximumOpening = 0.2858f;
    private float maximumOpeningOnZAxis = 0.4357f;
    private float maximumClosing = 0.00005f;
     
    private float movementSpeed = 0.5f;
    [SerializeField] private bool isDoorMovingOnZAxis;
     
    bool playerIsHere;
     
    // Start is called before the first frame update
    void Start()
    {
        playerIsHere = false;
    }
 
    // Update is called once per frame
    void Update()
    {
        if(playerIsHere){
            if (isDoorMovingOnZAxis)
            {
                if (movingRightDoor.transform.position.z < maximumOpeningOnZAxis)
                {
                    movingRightDoor.transform.Translate(0f, 0f, movementSpeed * Time.deltaTime);
                    movingLeftDoor.transform.Translate(0f, 0f, -movementSpeed * Time.deltaTime);
                }
            }
            else
            {
                if(movingRightDoor.transform.position.z < maximumOpening)
                {

                    movingRightDoor.transform.Translate(-movementSpeed * Time.deltaTime, 0f, movementSpeed * Time.deltaTime);
                    movingLeftDoor.transform.Translate(movementSpeed * Time.deltaTime, 0f, -movementSpeed * Time.deltaTime);
                }
            }
        }else{
            if(movingRightDoor.transform.position.z > maximumClosing)
            {
                if (isDoorMovingOnZAxis)
                {
                    movingRightDoor.transform.Translate(0f, 0f, -movementSpeed * Time.deltaTime);
                    movingLeftDoor.transform.Translate(0f, 0f, +movementSpeed * Time.deltaTime);
                }
                else
                {
                    movingRightDoor.transform.Translate(+movementSpeed * Time.deltaTime, 0f, -movementSpeed * Time.deltaTime);
                    movingLeftDoor.transform.Translate(-movementSpeed * Time.deltaTime, 0f, +movementSpeed * Time.deltaTime);
                }
                
            }
        }
    }

    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            playerIsHere = true;
        }
    }
     
    private void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            playerIsHere = false;
        }
    }
}