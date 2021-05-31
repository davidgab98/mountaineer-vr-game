using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleport : TeleportationProvider {

    public enum TeleportTypes { BLINK, SHIFT, SCREENFADE };
    public TeleportTypes teleportType;

    public float shiftTeleportSpeed = 30;

    [SerializeField]
    private float screenFadeTeleportDuration = 0.25f;
    [SerializeField]
    private ImageFader fader;
    float screenFadeTeleportTime;

    private CharacterController character;
    private Vector3 teleportDestination;
    private bool teleporting;

    protected override void Awake() {
        character = GetComponent<CharacterController>();
    }

    public override bool QueueTeleportRequest(TeleportRequest teleportRequest) {
        if(!teleporting) {
            teleportDestination = teleportRequest.destinationPosition;
            
            teleporting = true;
            return true;
        }
        return false;
    }

    protected override void Update() {
        if(teleporting) {
            if(teleportType == TeleportTypes.SHIFT) {
                ShiftTeleport();
            } else if(teleportType == TeleportTypes.SCREENFADE) {
                ScreenFadeTeleport();
            } else { 
                BlinkTeleport();
            }
        }
    }

    void BlinkTeleport() {
        character.transform.position = teleportDestination;
        teleporting = false;
    }

    void ScreenFadeTeleport() {
        fader.FadeOut(screenFadeTeleportDuration);

        screenFadeTeleportTime += Time.deltaTime;
        if(screenFadeTeleportTime >= screenFadeTeleportDuration) {

            character.transform.position = teleportDestination;
            fader.FadeIn(screenFadeTeleportDuration);

            teleporting = false;
            screenFadeTeleportTime = 0;
        }
    }

    void ShiftTeleport() {
        float step = shiftTeleportSpeed * Time.deltaTime;
        character.transform.position = Vector3.MoveTowards(character.transform.position, teleportDestination, step);

        if(character.transform.position.Equals(teleportDestination)) {
            teleporting = false;
        }
    }


}
