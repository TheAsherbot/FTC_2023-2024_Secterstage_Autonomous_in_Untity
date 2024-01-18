using UnityEngine;

public class PushBot : _BaseRobot
{


    private StartingSpot startingSpot;
    [SerializeField] private int i;


    [SerializeField] private Vector3 velocity;
    private Rigidbody rigidbody;
    [SerializeField] private float movementSpeed = 2.5f;
    private float startY;


    private float rotation;
    [SerializeField] private float rotationSpeed = 175;



    private async void Awake()
    {
        OnPositionDecided += PushBot_OnPositionDecided;

        rigidbody = GetComponent<Rigidbody>();

        await System.Threading.Tasks.Task.Delay(300);

        startY = transform.position.y;
    }


    private void Update()
    {
        velocity = rigidbody.velocity;
    }
    private void FixedUpdate()
    {
        RunAutonomous();
    }

    public void Move(Vector3 movement)
    {
        rigidbody.AddRelativeForce(movement * movementSpeed, ForceMode.Impulse);
        if (rigidbody.velocity.magnitude > movementSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * movementSpeed;
        }
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }

    public void Rotate(float rotationForce)
    {
        rotation += rotationForce;
        Mathf.Clamp(rotation, -1, 1);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + (rotation * rotationSpeed * Time.deltaTime), 0);
        if (rotationForce == 0) return;

        if (rotation > 0)
        {
            rotation -= Time.deltaTime * 2;
            if (rotation < 0)
            {
                rotation = 0;
            }
        }
        else if (rotation < 0)
        {
            rotation += Time.deltaTime * 2;
            if (rotation > 0)
            {
                rotation = 0;
            }
        }
    }


    private void PushBot_OnPositionDecided(StartingSpot startingSpot)
    {
        this.startingSpot = startingSpot;
    }

    private void RunAutonomous()
    {
        i++;
        switch (startingSpot)
        {
            case StartingSpot.RedFront:
                RunRedFrontAutonomous();
                break;
            case StartingSpot.RedBack:
                RunRedBackAutonomous();
                break;
            case StartingSpot.BlueFront:
                RunBlueFrontAutonomous();
                break;
            case StartingSpot.BlueBack:
                RunBlueBackAutonomous();
                break;
        }
    }

    private void RunRedBackAutonomous()
    {
        if (i < 20)
        {

        }
        else if (i < 23)
        {
            Move(Vector3.left);
        }
        else if (i < 30)
        {

        }
        else if (i < 100)
        {
            Move(Vector3.forward);
        }
    }

    private void RunRedFrontAutonomous()
    {
        if (i < 20)
        {

        }
        else if (i < 50)
        {
            Move(Vector3.forward);
        }
    }

    private void RunBlueBackAutonomous()
    {
        if (i < 20)
        {
            
        }
        else if (i < 23)
        {
            Move(Vector3.right);
        }
        else if (i < 30)
        {

        }
        else if (i <  100)
        {
            Move(Vector3.forward);
        }
    }

    private void RunBlueFrontAutonomous()
    {
        if (i < 20)
        {

        }
        else if (i < 50)
        {
            Move(Vector3.forward);
        }
    }

}
