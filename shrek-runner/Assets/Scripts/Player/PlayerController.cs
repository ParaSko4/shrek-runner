using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float maxAngle = 360f;
    private const float errorAngle = 0.5f;
    private const float massToSwitchAnimation = 2f;

    enum PlayerAnimationPhase
    {
        PrepareToStay, Stay, PrepareToRun, Run, PrepareToWalk, Walk, PrepareToFall, Falling, PrepareToDie, Die
    }

    [SerializeField]
    private GameDataObject data;

    [SerializeField]
    private PoolManager pools;

    private PlayerAnimationPhase playerAnimationPhase = 0;

    private bool isRightSideDecorTouched = false;
    private bool isLeftSideDecorTouched = false;

    private float boardEnd = 0;
    private float finishPoint = 0;
    private float massCoef;

    private Rigidbody rb;
    private CapsuleCollider col;
    private Animator animator;

    private Vector3 forward;
    private Vector3 right;

    private Quaternion rotate;
    private RaycastHit decorHit;

    private float screen;
    private float rotationSpeed;
    private int touchCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();

        forward = transform.forward;
        right = transform.right;

        boardEnd = data.PlaneBorder.x / 2 - 1;
        finishPoint = data.PlaneBorder.z / 2 - 5;
        massCoef = rb.mass;
        screen = Screen.width;

        rotate = new Quaternion();
        decorHit = new RaycastHit();

        data.isGameLose = false;
        data.isGameWin = false;
        data.isGamePlayed = false;
    }

    private void FixedUpdate()
    {
        if (data.isGamePlayed)
        {
            if (finishPoint < transform.position.z)
            {
                data.GameWin();
            }

            rb.AddForce(forward * data.PlayerSpeed * rb.mass, ForceMode.Impulse);
            massCoef = rb.mass > 4 ? rb.mass / 2 : rb.mass;

            //if (Input.GetKey(KeyCode.RightArrow) && boardEnd > transform.position.x && !isRightSideDecorTouched)
            //{
            //    rb.AddForce(right * data.PlayerSpeed * data.PlayerSidesSpeed * massCoef, ForceMode.Impulse);
            //}
            //else if (Input.GetKey(KeyCode.LeftArrow) && -boardEnd < transform.position.x && !isLeftSideDecorTouched)
            //{
            //    rb.AddForce(-right * data.PlayerSpeed * data.PlayerSidesSpeed * massCoef, ForceMode.Impulse);
            //}
            touchCount = 0;
            rotationSpeed = rb.mass > 1 ? data.GiantFormRotationSpeed : data.NormalFormRotationSpeed;

            while (touchCount < Input.touchCount)
            {
                if (Input.GetTouch(touchCount).position.y > screen / 2)
                {
                    if (data.MaxSize.magnitude > transform.localScale.magnitude)
                    {
                        transform.localScale += data.PerSize;
                        rb.mass += data.PerMass;
                    }
                }
                if (Input.GetTouch(touchCount).position.y < screen / 2)
                {
                    if (data.MinSize.magnitude < transform.localScale.magnitude)
                    {
                        transform.localScale -= data.PerSize;
                        rb.mass -= data.PerMass;
                    }
                    else
                    {
                        rb.mass = 1f;
                    }
                }


                if (Input.GetTouch(touchCount).position.x > screen / 2 && boardEnd > transform.position.x && !isRightSideDecorTouched)
                {
                    rb.AddForce(right * data.PlayerSpeed * data.PlayerSidesSpeed * massCoef, ForceMode.Impulse);

                    rotate = transform.rotation * Quaternion.AngleAxis(rotationSpeed, Vector3.up);
                }
                else if (Input.GetTouch(touchCount).position.x < screen / 2 && -boardEnd < transform.position.x && !isLeftSideDecorTouched)
                {
                    rb.AddForce(-right * data.PlayerSpeed * data.PlayerSidesSpeed * massCoef, ForceMode.Impulse);

                    rotate = transform.rotation * Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
                }

                RotateTo();

                ++touchCount;
            }

            if (transform.eulerAngles.y != 0 && touchCount == 0)
            {
                if (transform.eulerAngles.y < data.AngleMovingRotation)
                {
                    rotate = transform.rotation * Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
                }
                else if (transform.eulerAngles.y > maxAngle - data.AngleMovingRotation)
                {
                    rotate = transform.rotation * Quaternion.AngleAxis(data.GiantFormRotationSpeed, Vector3.up);
                }

                if (rotate.eulerAngles.y > -errorAngle && rotate.eulerAngles.y < errorAngle)
                {
                    rotate.y = 0;
                }

                RotateTo();
            }
        }
    }

    void Update()
    {
        PlayerAnimationPhaseLogic();

        if (data.isGamePlayed)
        {
            
            //MovingRotatetion(rb.mass > 1 ? data.GiantFormRotationSpeed : data.NormalFormRotationSpeed);
        }
    }

    //private void MovingRotatetion(float rotationSpeed)
    //{
    //    if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        rotate = transform.rotation * Quaternion.AngleAxis(rotationSpeed, Vector3.up);
    //    }
    //    else if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        rotate = transform.rotation * Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
    //    }
    //    else if (transform.eulerAngles.y != 0)
    //    {
    //        if (transform.eulerAngles.y < data.AngleMovingRotation)
    //        {
    //            rotate = transform.rotation * Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
    //        }
    //        else if (transform.eulerAngles.y > maxAngle - data.AngleMovingRotation)
    //        {
    //            rotate = transform.rotation * Quaternion.AngleAxis(data.GiantFormRotationSpeed, Vector3.up);
    //        }

    //        if (rotate.eulerAngles.y > -errorAngle && rotate.eulerAngles.y < errorAngle)
    //        {
    //            rotate.y = 0;
    //        }
    //    }

    //    if (rotate.eulerAngles.y < data.AngleMovingRotation || rotate.eulerAngles.y > maxAngle - data.AngleMovingRotation)
    //    {
    //        transform.rotation = rotate;
    //    }
    //}

    private void RotateTo()
    {
        if (rotate.eulerAngles.y < data.AngleMovingRotation || rotate.eulerAngles.y > maxAngle - data.AngleMovingRotation)
        {
            transform.rotation = rotate;
        }
    }
    private void PlayerAnimationPhaseLogic()
    {
        if (data.isGameWin)
        {
            playerAnimationPhase = PlayerAnimationPhase.PrepareToStay;
        }

        switch (playerAnimationPhase)
        {
            case PlayerAnimationPhase.PrepareToStay:
                playerAnimationPhase = PlayerAnimationPhase.Stay;

                animator.SetBool("isIdle", true);
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", false);

                break;
            case PlayerAnimationPhase.Stay:
                if (data.isGamePlayed)
                {
                    playerAnimationPhase = PlayerAnimationPhase.PrepareToRun;
                }

                break;
            case PlayerAnimationPhase.PrepareToRun:
                playerAnimationPhase = PlayerAnimationPhase.Run;

                animator.SetBool("isIdle", false);
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", true);

                break;
            case PlayerAnimationPhase.Run:
                if (rb.mass > massToSwitchAnimation)
                {
                    playerAnimationPhase = PlayerAnimationPhase.PrepareToWalk;
                }

                break;
            case PlayerAnimationPhase.PrepareToWalk:
                playerAnimationPhase = PlayerAnimationPhase.Walk;

                animator.SetBool("isIdle", false);
                animator.SetBool("isWalk", true);
                animator.SetBool("isRun", false);

                break;
            case PlayerAnimationPhase.Walk:
                if (rb.mass < massToSwitchAnimation)
                {
                    playerAnimationPhase = PlayerAnimationPhase.PrepareToRun;
                }

                break;
            case PlayerAnimationPhase.PrepareToFall:
                playerAnimationPhase = PlayerAnimationPhase.Falling;
                animator.SetBool("isFalling", true);

                break;
            case PlayerAnimationPhase.PrepareToDie:
                playerAnimationPhase = PlayerAnimationPhase.Die;
                animator.SetBool("isDeath", true);

                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Obstacle_Too_Big":
                TouchObstacle("Obstacle_Too_Big");
                break;
            case "Decor":
                TouchObstacle("Decor");
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Decor":
                isLeftSideDecorTouched = false;
                isRightSideDecorTouched = false;

                break;
            case "Obstacle_Too_Big":
                isLeftSideDecorTouched = false;
                isRightSideDecorTouched = false;

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Obstacle":
                if (data.wallMass > rb.mass)
                {
                    Lose();
                    return;
                }

                other.gameObject.SetActive(false);

                var partObj = pools.destrWallPool.GetObject();
                var part = pools.destrWallPool.GetPS(partObj);

                partObj.transform.position = other.transform.position + new Vector3(0, 2, 0);
                part.Play();

                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle_Spike":
                Lose();
                break;
            case "Obstacle_Too_Big":
                Lose();
                break;
            case "Obstacle_Spike_Wall":
                Lose();
                break;
            case "Poisonous_Puddle":
                Lose();
                break;
            case "Obstacle_Hole":
                col.enabled = false;
                playerAnimationPhase = PlayerAnimationPhase.PrepareToFall;
                rb.AddForce(transform.up * data.PreFallingForce * massCoef, ForceMode.Impulse);
                data.GameOver();

                break;
        }
    }

    private void TouchObstacle(string obstacle)
    {
        if (Physics.Raycast(transform.position, Vector3.right, out decorHit, 1))
        {
            if (decorHit.transform.tag.Equals(obstacle))
            {
                isRightSideDecorTouched = true;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.left, out decorHit, 1))
            {
                if (decorHit.transform.tag.Equals(obstacle))
                {
                    isLeftSideDecorTouched = true;
                }
            }
        }
    }

    public void Lose()
    {
        data.GameOver();
        playerAnimationPhase = PlayerAnimationPhase.PrepareToDie;
    }

    public void Win()
    {
        data.GameWin();
    }
}