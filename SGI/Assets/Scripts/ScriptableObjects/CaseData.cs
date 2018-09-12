using UnityEngine;

[CreateAssetMenu(menuName = "Case Data")]
public class CaseData : ScriptableObject {

    public AudioClip[] introClips;          //Alle intro clips die bij de case intro worden afgespeeld
    [Space]
    public AudioClip menuClip;              //Clip voor de menu opties
    public AudioClip whoIsGuiltyClip;       //Clip als je denkt te weten wie de schuldige is

    [System.Serializable]
	public class Suspect
    {
        public string suspectName;          //Utility zonder functie verder. Alleen maar overzichtelijker in Inspector.
        public AudioClip explanation;       //Audio Clip die wordt gebruikt als de detective de suspect ondervraagt
        [ForNextVersion][HideInInspector] public AudioClip accusationQuote;   //Audio Clip die wordt gebruikt als de detective de suspect beschuldigt
        public KeyCode hotkey;             //Tijdens Accusation mode klikt speler op deze Key om de suspect te beschuldigen
        public bool guilty;                 //Is de suspect de schuldige?
    }

    public Suspect[] suspects;
}
