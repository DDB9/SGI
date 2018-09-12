using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public AudioSource detectiveBriefing;
	public AudioSource menu;
	public AudioSource dean;
	public AudioSource teacher;
	public AudioSource clerk;
	public AudioSource janitor;
	public AudioSource accusation;

    private AudioSource[] allAudioSources;

    private void Awake()
    {
        Debug.Log("List of audio sources in scene: ");
        allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            Debug.Log(allAudioSources[i]);
        }
        
    }

    void Update(){
        /*
		if (detectiveIntro.isPlaying == false
            && Input.GetKeyDown(KeyCode.Alpha1)){       
			Menu(); 								
            //detectiveIntro.Play();
            Debug.Log("Detective intro is not playing");
        }
        */

        Menu();
	}

	void Menu(){
        if (menu.isPlaying == false)
        {
            //StartCoroutine(WaitForThreeSeconds());
                
        }

        if (Input.anyKeyDown)
        {
            StopAllAudio();
        }

		if (Input.GetKeyDown(KeyCode.Alpha1)){ // Speelt de Detective Intro openieuw af
			detectiveBriefing.Play();
            Debug.Log("Play detective intro");
		} 
		else if (Input.GetKeyDown(KeyCode.Alpha2)){ // Speelt de verklaring van de Dean af
            dean.Play();
            Debug.Log("Play Dean text");
        }
		else if (Input.GetKeyDown(KeyCode.Alpha3)){ // Speelt de verklaring van de Teacher af
            teacher.Play();
            Debug.Log("Play teacher text");
        }
		else if (Input.GetKeyDown(KeyCode.Alpha4)){ // Speelt de verklaring van de Clerk af
            clerk.Play();
            Debug.Log("Play clerk text");
        }
		else if (Input.GetKeyDown(KeyCode.Alpha5)){ // Speelt de verklaring van de Janitor af
            janitor.Play();
            Debug.Log("Play janitor text");
        }
		else if (Input.GetKeyDown(KeyCode.Alpha6)){ // Brengt de speler naar het menu waar hij iemand kan beschuldigen
            Accusation();
            Debug.Log("Accusation menu");
        }
		else if (Input.GetKeyDown(KeyCode.Space)){ // Speelt het menu opnieuw af
            Menu();
            Debug.Log("Go back to menu");
        }
	}
	
    private IEnumerator WaitForThreeSeconds()
    {
        //menu.Play();    // Speelt het menu af
        Debug.Log("Waiting for 3 seconds");
        yield return new WaitForSeconds(3f);
        Debug.Log("Waiting finished");
    }

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach(AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

	void Accusation(){ // Hier komt het menu waar de speler iemand kan beschuldigen.
		accusation.Play();
		if (Input.GetKey(KeyCode.Alpha1)){
            Menu();
		}
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Accuse Dean");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Accuse Teacher");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Accuse Clerk");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Accuse Janitor");
        }
    }
}
