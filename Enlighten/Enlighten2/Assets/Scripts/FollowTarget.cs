using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    [SerializeField] Transform target;
    [SerializeField] [Range(0.01f, 1)] float dampTime = 0.01f;
    [SerializeField] float clampL;
    [SerializeField] float clampR;
    [SerializeField] float clampUp;
    [SerializeField] float clampDown;

    enum LevelCompleteState
    {
        Up = 0,
        Down = 1,
        Right = 2,
        Left = 3,
        None = 4
    }

    [SerializeField] LevelCompleteState levelExitState = LevelCompleteState.None;

    [HideInInspector] public int winState;

    private Vector3 velocity = Vector3.zero;
    private Camera _cam;

	// Use this for initialization
	void Start () {
        winState = (int)levelExitState;
        _cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Set Variables
        winState = (int)levelExitState;

        //Methods
    }

    void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (target)
        {
            Vector3 point = _cam.WorldToViewportPoint(target.position); //Get's Player's Position in World to View Port;
            Vector3 delta = target.position - _cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));  //Finds the difference between the middle of the screen/camera and our targets pos
            Vector3 destination = transform.position + delta; //Gets the correct position for the target to bein the center of the camera

            //print("Point: " + point + "Target Pos:" + target.position + "Delta: " + delta + "Destination" + destination);

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

            _cam.transform.position = new Vector3(Mathf.Clamp(transform.position.x, clampL, clampR), Mathf.Clamp(transform.position.y, clampDown,clampUp), transform.position.z);
        }
    }
}
