using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

public class YNotRobots6780 : _BaseRobot
{

    public enum ElevatorStage
    {
        Stage0,
        Stage1,
        Stage2,
        Stage3,
    }
    public enum ElevatorRotation
    {
        Down,
        Hover,
        Up,
    }


    // Elevator
    private const float ELEVATOR_SPEED = 0.64f; // meters Per Seconds
    private const float OUT_ELEVATOR_POSITION = 0.245f;
    private const int UP_ELEVATOR_ROTATION = -30;
    private const int HOVER_ELEVATOR_ROTATION = -5;
    private const int DOWN_ELEVATOR_ROTATION = 0;
    private const float ELEVATOR_ROTATION_TIME = 1;

    // Intake
    private const float INTAKE_SPEED = 360;
    private const float INTAKE_HINGE_TIME = 1f;
    private const float INTAKE_FORCE = 0.5f;

    // Bucket
    private const int UP_BUCKET_ROTATION = 0;
    private const int DOWN_BUCKET_ROTATION = 100;
    private const float BUCKET_ROTATION_TIME = 1f;




    private StartingSpot startingSpot;
    [SerializeField] private int tick;


    [SerializeField] private Vector3 velocity;
    private Rigidbody rigidbody;
    [SerializeField] private float movementSpeed = 2.5f;
    private float startY;


    [SerializeField] private float rotationSpeed = 175;


    [Header("###ROBOT PARTS####")]
    [Header("Elevators")]
    [SerializeField] private Transform elevatorStage1;
    [SerializeField] private Transform elevatorStage2;
    [SerializeField] private Transform elevatorStage3;
    [SerializeField] private Transform elevatorStage4;
    [SerializeField] private Transform elevatorHinge;
    private ElevatorStage currentElevatorStage;
    private ElevatorRotation currentElevatorRotation;
    private float elapsedElevatorRotationTime;
    private float elevatorRotation;
    private float startElevatorRotation;


    [Header("Intake")]
    [SerializeField] private IntakeTrigger intakeTrigger;
    [SerializeField] private Transform intakeHinge;
    [SerializeField] private Transform intake;
    private bool isIntakeOn;
    private bool isIntakeMovingDown;
    private float elapsedIntakeRotationTime;
    public float IntakeRotation
    {
        get;
        private set;
    }
    private float startIntakeRotation = -150;


    [Header("Bucket")]
    [SerializeField] private Transform bucketHinge;
    private bool isBucketUp = true;
    private float startBucketRotation;
    private float bucketRotation;
    private float elapsedBucketRotationTime;



    private async void Awake()
    {
        OnPositionDecided += PushBot_OnPositionDecided;


        rigidbody = GetComponent<Rigidbody>();


        intakeTrigger.OnTrigger += OnIntakeTrigger;
        


        await Task.Delay(250);

        startY = transform.position.y;
    }


    private void Start()
    {
        RunAutonomous();
    }

    private void Update()
    {
        velocity = rigidbody.velocity;

        if (Input.GetKeyDown(KeyCode.K)) isIntakeMovingDown = true;

        MoveElevator();
        RotateElevator();
        RotateIntake();
        MoveIntakeDown();
        RotateBucket();

        elapsedElevatorRotationTime += Time.deltaTime;
        elapsedIntakeRotationTime += Time.deltaTime;
        elapsedBucketRotationTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        tick++;
    }

    public void Move(Vector3 movement)
    {
        rigidbody.AddRelativeForce(movement * movementSpeed, ForceMode.Impulse);
        if (rigidbody.velocity.magnitude > movementSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * movementSpeed;
        }
        if (startY != 0)
        {
            transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        }
    }

    public void Rotate(float rotationForce)
    {
        Mathf.Clamp(rotationForce, -1, 1);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + (rotationForce * rotationSpeed * Time.deltaTime), 0);
    }

    public void SetElevatorStage(ElevatorStage elevatorStage)
    {
        currentElevatorStage = elevatorStage;
    }

    public void ToggleIntake()
    {
        isIntakeOn = !isIntakeOn;

        currentElevatorRotation = ElevatorRotation.Down;
        elapsedElevatorRotationTime = 0;
        startElevatorRotation = elevatorRotation;
    }

    public void WinchUp()
    {
        elapsedIntakeRotationTime = 0;

        currentElevatorRotation = ElevatorRotation.Up;
        isIntakeMovingDown = true;
        elapsedElevatorRotationTime = 0;
        startElevatorRotation = elevatorRotation;
    }

    public void WinchDown()
    {
        currentElevatorRotation = ElevatorRotation.Hover;
        elapsedElevatorRotationTime = 0;
        startElevatorRotation = elevatorRotation;
    }

    public void BucketUp()
    {
        startBucketRotation = bucketRotation;
        elapsedBucketRotationTime = 0;
        isBucketUp = true;
    }

    public void BucketDown()
    {
        startBucketRotation = bucketRotation;
        elapsedBucketRotationTime = 0;
        isBucketUp = false;
    }



    private void PushBot_OnPositionDecided(StartingSpot startingSpot)
    {
        this.startingSpot = startingSpot;
    }

    private void RunAutonomous()
    {
        switch (startingSpot)
        {
            case StartingSpot.RedFront:
                StartCoroutine(RunRedFrontAutonomous());
                break;
            case StartingSpot.RedBack:
                StartCoroutine(RunRedBackAutonomous());
                break;
            case StartingSpot.BlueFront:
                StartCoroutine(RunBlueFrontAutonomous());
                break;
            case StartingSpot.BlueBack:
                StartCoroutine(RunBlueBackAutonomous());
                break;
        }
    }

    private IEnumerator RunRedBackAutonomous()
    {
        yield return AutonomousAction(1, WinchDown);
        yield return AutonomousAction(20, null);
        yield return AutonomousAction(10, Move, Vector3.left);
        yield return AutonomousAction(10, null);
        yield return AutonomousAction(35, Move, Vector3.forward);
        yield return AutonomousAction(25, null);
        yield return AutonomousAction(15, Move, Vector3.left);
        yield return AutonomousAction(1, WinchUp);
        yield return AutonomousAction(74, null);
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage2);
        yield return AutonomousAction(1, BucketDown);
        yield return AutonomousAction(59, null);
        yield return AutonomousAction(1, BucketUp);
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage0);
        yield return AutonomousAction(1, WinchDown);
        yield return AutonomousAction(74, null);
        yield return AutonomousAction(47, Move, Vector3.right);
        yield return AutonomousAction(43, Move, Vector3.forward);
        yield return AutonomousAction(1, ToggleIntake);
        yield return AutonomousAction(2999, null);
    }

    private IEnumerator RunRedFrontAutonomous()
    {

        yield return AutonomousAction(1, WinchUp);
        yield return AutonomousAction(50, null);
        yield return AutonomousAction(1, WinchDown);
        yield return AutonomousAction(50, null);
        yield return AutonomousAction(1, ToggleIntake);
        yield return AutonomousAction(30, Move, Vector3.forward);
        yield return AutonomousAction(100000000, null);
        
        
/*
        yield return AutonomousAction(1, WinchUp);
        yield return AutonomousAction(20, null);
        yield return AutonomousAction(20, Move, Vector3.left);
        yield return AutonomousAction(10, null);
        yield return AutonomousAction(55, Move, Vector3.forward);
        yield return AutonomousAction(10, null);
        yield return AutonomousAction(50, Move, Vector3.left);
        yield return AutonomousAction(1, WinchUp);
        yield return AutonomousAction(74, null);
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage2);
        yield return AutonomousAction(60, null); // Bucket Down
        yield return AutonomousAction(20, null); // Bucket Up
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage0);
        yield return AutonomousAction(1, WinchDown);
        yield return AutonomousAction(74, null);
        yield return AutonomousAction(47, Move, Vector3.right);
        yield return AutonomousAction(43, Move, Vector3.forward);
*/
    }

    private IEnumerator RunBlueBackAutonomous()
    {
        yield return AutonomousAction(20, null);
        yield return AutonomousAction(20, Move, Vector3.right);
        yield return AutonomousAction(10, null);
        yield return AutonomousAction(87, Move, Vector3.forward);
        yield return AutonomousAction(10, null);
        yield return AutonomousAction(50, Move, Vector3.right);
        yield return AutonomousAction(75, WinchUp);
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage2);
        yield return AutonomousAction(60, null); // Bucket Down
        yield return AutonomousAction(20, null); // Bucket Up
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage0);
        yield return AutonomousAction(75, WinchDown);
        yield return AutonomousAction(47, Move, Vector3.right);
        yield return AutonomousAction(43, Move, Vector3.forward);
    }

    private IEnumerator RunBlueFrontAutonomous()
    {
        yield return AutonomousAction(20, null);
        yield return AutonomousAction(20, Move, Vector3.right);
        yield return AutonomousAction(10, null);
        yield return AutonomousAction(55, Move, Vector3.forward);
        yield return AutonomousAction(10, null);
        yield return AutonomousAction(50, Move, Vector3.right);
        yield return AutonomousAction(1, WinchUp);
        yield return AutonomousAction(74, null);
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage2);
        yield return AutonomousAction(60, null); // Bucket Down
        yield return AutonomousAction(20, null); // Bucket Up
        yield return AutonomousAction(125, SetElevatorStage, ElevatorStage.Stage0);
        yield return AutonomousAction(1, WinchDown);
        yield return AutonomousAction(74, null);
        yield return AutonomousAction(47, Move, Vector3.right);
        yield return AutonomousAction(43, Move, Vector3.forward);
    }


    private IEnumerator AutonomousAction(int ticks, Action action)
    {
        while (tick < ticks)
        {
            action?.Invoke();
            yield return new WaitForFixedUpdate();
        }
        tick = 0;
    }
    
    private IEnumerator AutonomousAction<T>(int ticks, Action<T> action, T value)
    {
        while (tick < ticks)
        {
            action?.Invoke(value);
            yield return new WaitForFixedUpdate();
        }
        tick = 0;
    }


    private void MoveElevator()
    {
        switch (currentElevatorStage)
        {
            case ElevatorStage.Stage0:
                if (elevatorStage4.localPosition.z != 0)
                {
                    MoveElevatorToPosition(elevatorStage4, 0);
                }
                else if (elevatorStage3.localPosition.z != 0)
                {
                    MoveElevatorToPosition(elevatorStage3, 0);
                }
                else if (elevatorStage2.localPosition.z != 0)
                {
                    MoveElevatorToPosition(elevatorStage2, 0);
                }
                else if (elevatorStage1.localPosition.z != 0)
                {
                    MoveElevatorToPosition(elevatorStage1, 0);
                }
                break;
            case ElevatorStage.Stage1:
                if (elevatorStage4.localPosition.z != 0)
                {
                    MoveElevatorToPosition(elevatorStage4, 0);
                }
                else if (elevatorStage3.localPosition.z != 0)
                {
                    MoveElevatorToPosition(elevatorStage3, 0);
                }
                else if (elevatorStage2.localPosition.z != OUT_ELEVATOR_POSITION / 2)
                {
                    MoveElevatorToPosition(elevatorStage2, OUT_ELEVATOR_POSITION / 2);
                }
                else if (elevatorStage1.localPosition.z != OUT_ELEVATOR_POSITION)
                {
                    MoveElevatorToPosition(elevatorStage1, OUT_ELEVATOR_POSITION);
                }
                break;
            case ElevatorStage.Stage2:
                if (elevatorStage4.localPosition.z != 0)
                {
                    MoveElevatorToPosition(elevatorStage4, 0);
                }
                else if (elevatorStage3.localPosition.z != OUT_ELEVATOR_POSITION / 4 * 3)
                {
                    MoveElevatorToPosition(elevatorStage3, OUT_ELEVATOR_POSITION / 4 * 3);
                }
                else if (elevatorStage2.localPosition.z != OUT_ELEVATOR_POSITION)
                {
                    MoveElevatorToPosition(elevatorStage2, OUT_ELEVATOR_POSITION);
                }
                else if (elevatorStage1.localPosition.z != OUT_ELEVATOR_POSITION)
                {
                    MoveElevatorToPosition(elevatorStage1, OUT_ELEVATOR_POSITION);
                }
                break;
            case ElevatorStage.Stage3:
                if (elevatorStage4.localPosition.z != OUT_ELEVATOR_POSITION)
                {
                    MoveElevatorToPosition(elevatorStage4, OUT_ELEVATOR_POSITION);
                }
                else if (elevatorStage3.localPosition.z != OUT_ELEVATOR_POSITION)
                {
                    MoveElevatorToPosition(elevatorStage3, OUT_ELEVATOR_POSITION);
                }
                else if (elevatorStage2.localPosition.z != OUT_ELEVATOR_POSITION)
                {
                    MoveElevatorToPosition(elevatorStage2, OUT_ELEVATOR_POSITION);
                }
                else if (elevatorStage1.localPosition.z != OUT_ELEVATOR_POSITION)
                {
                    MoveElevatorToPosition(elevatorStage1, OUT_ELEVATOR_POSITION);
                }
                break;
        }

        void MoveElevatorToPosition(Transform stage, float targetPosition)
        {
            targetPosition = Mathf.Clamp(targetPosition, 0.0f, ELEVATOR_SPEED);

            if (stage.localPosition.z == targetPosition)
            {
                
            }
            else if (stage.localPosition.z < 0)
            {
                stage.localPosition = new Vector3(stage.localPosition.x, stage.localPosition.y, 0);
            }
            else if (stage.localPosition.z > OUT_ELEVATOR_POSITION)
            {
                stage.localPosition = new Vector3(stage.localPosition.x, stage.localPosition.y, OUT_ELEVATOR_POSITION);
            }
            else if (stage.localPosition.z < targetPosition)
            {
                stage.localPosition = new Vector3(stage.localPosition.x, stage.localPosition.y, stage.localPosition.z + (ELEVATOR_SPEED * Time.deltaTime));
                if (stage.localPosition.z > targetPosition)
                {
                    stage.localPosition = new Vector3(stage.localPosition.x, stage.localPosition.y, targetPosition);
                }
            }
            else if (stage.localPosition.z > targetPosition)
            {
                stage.localPosition = new Vector3(stage.localPosition.x, stage.localPosition.y, stage.localPosition.z - (ELEVATOR_SPEED * Time.deltaTime));
                if (stage.localPosition.z < targetPosition)
                {
                    stage.localPosition = new Vector3(stage.localPosition.x, stage.localPosition.y, targetPosition);
                }
            }
        }
    }

    private void RotateElevator()
    {
        switch (currentElevatorRotation)
        {
            case ElevatorRotation.Down:
                RotateElevatorToRotation(DOWN_ELEVATOR_ROTATION);
                break;
            case ElevatorRotation.Hover:
                RotateElevatorToRotation(HOVER_ELEVATOR_ROTATION);
                break;
            case ElevatorRotation.Up:
                RotateElevatorToRotation(UP_ELEVATOR_ROTATION);
                break;
        }

        void RotateElevatorToRotation(float rotation)
        {
            elevatorRotation = Mathf.Lerp(startElevatorRotation, rotation, (float)(elapsedElevatorRotationTime * ELEVATOR_ROTATION_TIME) / ((float)Mathf.Abs(rotation - startElevatorRotation) / 30f));
            elevatorHinge.forward = Quaternion.AngleAxis(elevatorRotation, transform.right) * transform.forward;
        }
    }

    private void RotateIntake()
    {
        if (isIntakeOn)
        {
            //intake.localRotation = Quaternion.Euler(intake.localEulerAngles.x + (INTAKE_SPEED * Time.deltaTime), 0, 0);

            intake.Rotate(Vector3.right, INTAKE_SPEED * Time.deltaTime);
        }
    }

    private void MoveIntakeDown()
    {
        if (isIntakeMovingDown)
        {
            IntakeRotation = Mathf.Lerp(startIntakeRotation, 0, (float)(elapsedIntakeRotationTime / INTAKE_HINGE_TIME));
            intakeHinge.forward = Quaternion.AngleAxis(IntakeRotation, transform.right) * transform.forward;

            if (intakeHinge.localEulerAngles.x < 0)
            {
                intakeHinge.forward = transform.forward;
                isIntakeMovingDown = false;
            }
        }
    }

    private void RotateBucket()
    {
        if (isBucketUp)
        {
            RotateBucketToRotation(UP_BUCKET_ROTATION);
        }
        else
        {
            RotateBucketToRotation(DOWN_BUCKET_ROTATION);
        }

        void RotateBucketToRotation(float rotation)
        {
            bucketRotation = Mathf.Lerp(startBucketRotation, rotation, Mathf.Abs(rotation - startBucketRotation) / 30f * (float)(elapsedBucketRotationTime / BUCKET_ROTATION_TIME));
            bucketHinge.forward = Quaternion.AngleAxis(bucketRotation, elevatorHinge.right) * elevatorHinge.forward;
        }
    }


    private void OnIntakeTrigger(GameObject other)
    {
        if (other.CompareTag("Pixel"))
        {
            if (isIntakeOn)
            {
                other.GetComponentInParent<Rigidbody>().AddForce((new Vector3(0, 0.15f, 0) + -transform.forward).normalized * INTAKE_FORCE, ForceMode.Impulse);
            }
        }
    }


}
