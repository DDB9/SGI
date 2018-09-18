using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public AudioSource audioDetectiveIntro;
    public AudioSource audioDetectiveBriefing;
    public AudioSource audioDean;
    public AudioSource audioTeacher;
    public AudioSource audioClerk;
    public AudioSource audioJanitor;

    public AudioSource menuDetective;
    public AudioSource menuDean;
    public AudioSource menuTeacher;
    public AudioSource menuClerk;
    public AudioSource menuJanitor;
    public AudioSource menuAccusation;

    public AudioSource subMenuDean;
    public AudioSource subMenuTeacher;
    public AudioSource subMenuClerk;
    public AudioSource subMenuJanitor;
    public AudioSource subMenuReturn;

    public AudioSource win;
    public AudioSource lose;

    GameObject[] objects;
    Detective detective;
    Suspect dean;
    Suspect teacher;
    Suspect clerk;
    Suspect janitor;
    SubMenu accusation;

    int option = 0;
    bool next, makeAccusation, finished;

    // Use this for initialization
    void Start () {
        detective = new Detective(false, menuDetective, audioDetectiveIntro, audioDetectiveBriefing);
        dean = new Suspect(false, menuDean, audioDean);
        teacher = new Suspect(true, menuTeacher, audioTeacher);
        clerk = new Suspect(false, menuClerk, audioClerk);
        janitor = new Suspect(false, menuJanitor, audioJanitor);
        accusation = new SubMenu(menuAccusation, new AudioSource[] { subMenuDean, subMenuTeacher, subMenuClerk, subMenuJanitor, subMenuReturn });

        objects = new GameObject[] { detective, dean, teacher, clerk, janitor, accusation };
        GameLoop();
    }

    public void GameLoop() {
        next = detective.PlaySecondAudio();
        next = detective.MainAction();

        //while (!finished) {
        //    if (!makeAccusation) {
        //        //Menu
        //        while (!Input.GetMouseButtonDown(0)) {
        //            next = objects[option%6].PlayMenuAudio();
        //        }
        //        //Clip
        //        next = objects[option].MainAction();
        //        if (option%6 == 5) { makeAccusation = true; }

        //        option = 0;
        //    } else {
        //        //SubMenu
        //        while (!Input.GetMouseButtonDown(0)) {
        //            next = objects[5].PlaySubMenuAudio(option%5);
        //        }
        //        if(option%5 == 4) {
        //            makeAccusation = false;
        //        } else if (objects[option%5+1].guilty){
        //            win.Play();
        //            finished = true;
        //        } else {
        //            lose.Play();
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update() {
        InputHandling();
    }

    void InputHandling() {
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            option--;
        } else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            option++;
        }
    }


}
