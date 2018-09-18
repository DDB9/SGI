using System.Collections;
using UnityEngine;

public class GameObject : MonoBehaviour
{
    public bool guilty;
    protected bool isPlaying;
    protected AudioSource menuAudio;

    public bool PlayMenuAudio() {
        menuAudio.Play();

        //As long as the audio is playing or the user has not given any input
        while (!menuAudio.isPlaying) {
            if(Input.GetMouseButtonDown(0)) {
                return true;
            }
        }
        return false;
    }

    public virtual bool MainAction() { return true; }
    public virtual bool PlaySubMenuAudio(int option) { return true; }

}

public class Suspect : GameObject
{
    protected AudioSource mainAudio;

    public Suspect(bool Guilty, AudioSource MenuAudio, AudioSource MainAudio) {
        guilty = Guilty;
        menuAudio = MenuAudio;
        mainAudio = MainAudio;
    }

    public override bool MainAction() {
        mainAudio.Play();

        //As long as the audio is playing or the user has not given any input
        while (!mainAudio.isPlaying) {
            if (Input.GetMouseButtonDown(0)) {
                return true;
            }
        }
        return false;
    }
}

public class Detective : GameObject
{
    protected AudioSource mainAudio;
    protected AudioSource secondAudio;

    public Detective(bool Guilty, AudioSource MenuAudio, AudioSource MainAudio, AudioSource SecondAudio) {
        guilty = Guilty;
        menuAudio = MenuAudio;
        mainAudio = MainAudio;
        secondAudio = SecondAudio;
    }

    public override bool MainAction() {
        mainAudio.Play();

        //As long as the audio is playing or the user has not given any input
        while (!mainAudio.isPlaying) {
            if (Input.GetMouseButtonDown(0)) {
                return true;
            }
        }
        return false;
    }

    public bool PlaySecondAudio() {
        secondAudio.Play();

        //As long as the audio is playing or the user has not given any input
        while (!secondAudio.isPlaying) {
            if (Input.GetMouseButtonDown(0)) {
                return true;
            }
        }
        return false;
    }
}

public class SubMenu : GameObject
{
    AudioSource[] list;

    public SubMenu(AudioSource MenuAudio, AudioSource[] List) {
        menuAudio = MenuAudio;
        list = List;
    }

    public override bool PlaySubMenuAudio(int option) {
        list[option].Play();

        //As long as the audio is playing or the user has not given any input
        while (!list[option].isPlaying) {
            if (Input.GetMouseButtonDown(0)) {
                return true;
            }
        }
        return false;
    }
}



