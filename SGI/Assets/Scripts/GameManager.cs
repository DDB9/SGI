using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public AudioSource detectiveIntro;
	public AudioSource detectiveBriefing;
	public AudioSource menu;
	public AudioSource dean;
	public AudioSource teacher;
	public AudioSource clerk;
	public AudioSource janitor;
	public AudioSource accusation;

	void Start(){
		detectiveIntro.Play();
	}

	void Update(){ 	
		if (detectiveIntro.isPlaying == false){
			detectiveBriefing.Play();
			Debug.Log("Briefing");
		}
	}

	void Menu(){
		menu.Play();	// Speelt het menu af
		if (Input.GetKey(KeyCode.Alpha1)){ // Speelt de Detective Intro openieuw af
			detectiveIntro.Play();	
		} 
		else if (Input.GetKey(KeyCode.Alpha2)){ // Speelt de verklaring van de Dean af
			dean.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha3)){ // Speelt de verklaring van de Teacher af
			teacher.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha4)){ // Speelt de verklaring van de Clerk af
			clerk.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha5)){ // Speelt de verklaring van de Janitor af
			janitor.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha6)){ // Brengt de speler naar het menu waar hij iemand kan beschuldigen
			Accusation();
		}
		else if (Input.GetKey(KeyCode.Space)){ // Speelt het menu opnieuw af
			Menu();
		}
	}
	
	void Accusation(){ // Hier komt het menu waar de speler iemand kan beschuldigen.
		accusation.Play();
		if (Input.GetKey(KeyCode.Alpha1)){

		}
	}
}
