#pragma strict

public var time: Time;
public var mainCamera : GameObject;

// This function will be called when scene loaded
function Start () {
    // Fixed update will be performed 20 time per second:
    // time.fixedDeltaTime = 0.05f;
    // Getting MainCamera gameobject:
    mainCamera =  GameObject.Find("Main Camera");
}

// This function will be called every time.fixedDeltaTime
// seconds:
function FixedUpdate () {
    // Camera rotation with step 2 * time.deltaTime:
    mainCamera.transform.Rotate(0, 2 * time.deltaTime, 0);
}