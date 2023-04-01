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
    [SerializeField, Range(0, 30f)] private float frequency = 13.5f;

    [SerializeField, Range(0,0.1f)] private float xAmplitude = 0.015f;
    [SerializeField, Range(0,0.1f)] private float yAmplitude = 0.015f;

    float frequencyStart;
    float xAmpStart;
    float yAmpStart;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform cameraParent = null;

    [SerializeField] private float returnSpeed;

    [SerializeField] float timeSinceStart;

    //speed at which the head bob starts to occur, if we want to increase the head bob amount when the player-
    //sprints/dashes, we could set another toggle amount value for that.
    float toggleSpeed = 3.0f;
    Vector3 startPos;

    private Rigidbody _rb;
    PlayerControlsManager playerControlsManager;

    private void Awake()
    {
        frequencyStart = frequency;
        xAmpStart = xAmplitude;
        yAmpStart = yAmplitude;

        _rb = GetComponent<Rigidbody>();
        playerControlsManager = GetComponent<PlayerControlsManager>();
        startPos = _camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled || playerControlsManager.aiming) return; //if headbob isn't enabled.

        CheckMotion();

        if (playerControlsManager.speed < toggleSpeed) ResetPos(returnSpeed * Time.deltaTime);

        _camera.LookAt(FocusTarget());
    }
    void CheckMotion() //this method checks to see if the player is moving and isnt on the ground
    {
        if (playerControlsManager.GetLerpTimeADS() < 1)
        {
            ResetPos(playerControlsManager.GetLerpTimeADS());
            return;
        }
        if (playerControlsManager.speed < toggleSpeed) return;
        if (!playerControlsManager.IsGrounded()) return;

        PlayMotion(FootStepMotion());
    }
    Vector3 FootStepMotion() //this method simulates the footsteps
    {
        timeSinceStart += Time.deltaTime;
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(timeSinceStart * frequency) * yAmplitude;
        pos.x += Mathf.Sin(timeSinceStart * frequency / 2) * xAmplitude;

        if (playerControlsManager.speed > 7f)
        {
            frequency = frequencyStart * 2;
            xAmplitude = xAmpStart * 1.5f;
            yAmplitude = yAmpStart * 1.5f;
        }
        else
        {
            frequency = frequencyStart;

            xAmplitude = xAmpStart;
            yAmplitude = yAmpStart;
        }

        return pos;
    }

    void ResetPos(float returnSpeed) //this method allows the camera to go back to it's local origin after movement
    {
        if (_camera.localPosition == startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, startPos, returnSpeed);
        timeSinceStart = 0;
    }

    Vector3 FocusTarget() //allows for the headbob to stay centered relative to where the player walks.
    {
        Vector3 pos = new Vector3(transform.position.x,transform.position.y + cameraParent.localPosition.y, transform.position.z);
        pos += cameraParent.forward * 15.0f;
        return pos;
    }
    void PlayMotion(Vector3 motion) //simple play method
    {
        _camera.localPosition = motion;
    }
}
