using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 1;
    public float rotationSpeed = 360f;
    CharacterController controller;
    bool jumping = false;
    public int jumpCount = 2;
    
    float upVelocity;
    float gravity = -10f;
    float jumpHeight = 1.5f;

    
    Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        Debug.LogWarning("<color=red>CharacterController 없음!</color>");
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = Grounded();
        if (grounded)
        {
            upVelocity = 0;
            jumpCount = 2;
        }

        Vector2 inputDir = ReadMoveInput();

        if (JumpPressed())
        {
            if (jumping == false && jumpCount>0)
            {
                upVelocity = Mathf.Sqrt(jumpHeight * -gravity);
                jumpCount -= 1;
            }
        }

        Debug.Log("방향 " + inputDir);

        Vector3 moveDir = mainCamera.transform.forward * inputDir.y
            + mainCamera.transform.right * inputDir.x;

        Vector3 velocity = moveDir + Vector3.up * upVelocity;

        MovePlayer(velocity);

        RotatePlayer(moveDir);
    }

    void RotatePlayer(Vector3 dir)
    {
        Vector3 forward = transform.forward;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                                        transform.rotation,
                                        rotation,
                            rotationSpeed * Time.deltaTime);
        }
    }

    void MovePlayer(Vector3 dir)
    {
        upVelocity += gravity * Time.deltaTime;
        controller.Move(speed * dir * Time.deltaTime);
    }

    Vector2 ReadMoveInput()
    {
        Vector2 input = Vector2.zero;
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed) { input.y = 1; }
            if (keyboard.sKey.isPressed) { input.y = -1; }
            if (keyboard.dKey.isPressed) { input.x = 1; }
            if (keyboard.aKey.isPressed) { input.x = -1; }
        }

        return input;
    }

    private bool JumpPressed()
    {
        Keyboard keyboard = Keyboard.current;
        //bool jump = false;
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
        {
            //jump = true;
            return true;
        }

        //return jump;
        return false;
    }

    public bool Grounded()
    {
        if (controller != null && controller.isGrounded)
        {
            return true;
        }

        return false;
    }
}
 