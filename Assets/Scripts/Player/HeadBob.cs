using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* This script is meant to emulate a head bob as the player walks/runs.
 * I grabbed this script off of youtube but modified it a bit so I'm not sure if anyone is morally willing
 * to create their own rendition of it, but if so, go ahead.
*/
public class HeadBob : MonoBehaviour
{
    //if head bob is enabled
    [SerializeField] private bool isEnabled = true;

    //amplitude and frequency values of head bob, these values can be tweaked to whatever works
    [SerializeField, Range(0,0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0,30f)] private float frequency = 20.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform cameraParent = null;

    //speed at which the head bob starts to occur, if we want to increase the head bob amount when the player-
    //sprints/dashes, we could set another toggle amount value for that.
    float toggleSpeed = 3.0f;
    Vector3 startPos;

    private Rigidbody _rb;
    PlayerMovement playerMove;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        playerMove = GetComponent<PlayerMovement>();
        startPos = _camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return; //if headbob isn't enabled.

        CheckMotion();
        ResetPos();
        _camera.LookAt(FocusTarget());
    }
    void CheckMotion() //this method checks to see if the player is moving and isnt on the ground
    {
        float speed = new Vector3(_rb.velocity.x, 0, _rb.velocity.z).magnitude;

        if (speed < toggleSpeed) return;
        if (!playerMove.IsGrounded()) return;

        PlayMotion(FootStepMotion());
    }
    Vector3 FootStepMotion() //this method simulates the footsteps
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    void ResetPos() //this method allows the camera to go back to it's local origin after movement
    {
        if (_camera.localPosition == startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, startPos, 1 * Time.deltaTime);
    }

    Vector3 FocusTarget() //allows for the headbob to stay centered relative to where the player walks.
    {
        Vector3 pos = new Vector3(transform.position.x,transform.position.y + cameraParent.localPosition.y, transform.position.z);
        pos += cameraParent.forward * 15.0f;
        return pos;
    }
    void PlayMotion(Vector3 motion) //simple play method
    {
        _camera.localPosition += motion;
    }
}
