using UnityEngine;

[CreateAssetMenu(menuName = "Case Data")]
public sealed class CaseData : ScriptableObject {

    [System.Serializable]
    public sealed class Suspect
    {
        [Tooltip("Utility zonder functie verder. Alleen maar overzichtelijker in Inspector")]
        public string suspectName;
        [Space]
        [Tooltip("AudioClip bij \"Speak to ...\" tijdens Question/Interrogation mode")]
        public AudioClip speakTo;
        [Tooltip("AudioClip die wordt gebruikt als de detective de suspect ondervraagt")]
        public AudioClip explanation;
        [Tooltip("AudioClip bij Accusation mode")]
        public AudioClip accusation;
        [Tooltip("Is de suspect de schuldige?")]
        public bool guilty;
        [Tooltip("Selecteer dit als de 'Suspect' geen suspect is maar een aanduiding voor een volgend menu.")]
        public bool nextMenu;   //Als dit true is, dan is de speakTo clip iets wat zegt dat je naar het volgende menu kunt, 
                                //explanation is het begin van de accusation ("So, who is the murderer?") en accusation dat je naar het vorige menu kunt.
    }

    [Header("Intro Setup")]
    public AudioClip[] introClips;      //Alle introclips op volgorde die worden afgespeeld aan het begin van een beschuldig-session

    [Header("Suspects en Accusation Setup")]
    public Suspect[] suspects;          //De suspects in de case

    public AudioClip accuseCorrect;     //Clip die je hoort als je de juiste suspect hebt geraden
    public AudioClip accuseWrong;       //Clip die je hoort als je een verkeerde suspect hebt gekozen
}
