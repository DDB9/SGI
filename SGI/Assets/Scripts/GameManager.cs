using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public AudioSource detectiveIntro;
	public AudioSource menu;
	public AudioSource dean;
	public AudioSource teacher;
	public AudioSource clerk;
	public AudioSource janitor;
	public AudioSource accusation;

	void Update(){ 									// Dit werkt niet, want nu blijft hij voor altijd Menu afspelen.
		if (detectiveIntro.isPlaying == false){ 	// Misschien dat het voor het debugging wel handig is om een ander
			Menu(); 								// geluidje voor menu of detectiveIntro te gebruiken.
		}
	}

	void Menu(){
		menu.Play();
		if (Input.GetKey(KeyCode.Alpha1)){
			detectiveIntro.Play();
		} 
		else if (Input.GetKey(KeyCode.Alpha2)){
			dean.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha3)){
			teacher.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha4)){
			clerk.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha5)){
			janitor.Play();
		}
		else if (Input.GetKey(KeyCode.Alpha6)){
			Accusation();
		}
		else if (Input.GetKey(KeyCode.Space)){
			Menu();
		}
	}
	
	void Accusation(){
		accusation.Play();
		if (Input.GetKey(KeyCode.Alpha1)){

		}
	}
}
