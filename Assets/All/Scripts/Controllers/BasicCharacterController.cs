using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    private CharacterController characterController;
    public float runSpeed = 2.5f;
    public float rotateSpeed = 3.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 6.0f;
    private Vector3 moveDirection = Vector3.zero;

    public GameObject plane;
    private readonly bool newDirection;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // We are grounded, so recalculate
        // move direction directly from axes

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (Player.isRunning)
        {
            moveDirection *= lookSpeed + (runSpeed / 2);
        }
        else
        {
            moveDirection *= lookSpeed;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            TurnCharacter(moveDirection);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            Vector3 foreward = new Vector3();


            if (Input.GetKey(KeyCode.W))
            {
                foreward = transform.forward;

            }
            else if (Input.GetKey(KeyCode.S))
            {
                foreward = -transform.forward;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            foreward.y -= gravity * Time.deltaTime;

            if (Player.isRunning)
            {
                characterController.Move(foreward * runSpeed * Time.deltaTime);
            }
            else
            {
                characterController.Move(foreward * Time.deltaTime);
            }
        }

        //if (this.transform.position.y < plane.transform.position.y - 10)
        //{
        //    Vector3 currentPosition = this.transform.position;
        //    currentPosition.y = plane.transform.position.y + 3;

        //    this.transform.position = currentPosition;
        //}


    }

    private void TurnCharacter(Vector3 moveDirection)
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * (rotateSpeed * Time.deltaTime), 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
    }

}
