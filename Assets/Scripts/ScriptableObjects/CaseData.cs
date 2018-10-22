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
        public AudioClip speakToMenu;
        [Tooltip("I'd like to speak to... wat de detective zegt")]
        public AudioClip speakToDetective;
        [Tooltip("AudioClip die wordt gebruikt als de detective de suspect ondervraagt")]
        public AudioClip explanation;
        public Vector2 effectiveInterruption;   //Min sample en max sample waartussen de interruption effective is bij Maths Teacher
        [Tooltip("AudioClip bij Accusation mode")]
        public AudioClip accusation;
        [Tooltip("Is de suspect de schuldige?")]
        public bool guilty;
        [Tooltip("Selecteer dit als de 'Suspect' geen suspect is maar een aanduiding voor een volgend menu.")]
        public bool nextMenu;                   //Als dit true is, dan is de speakToMenu clip iets wat zegt dat je naar het volgende menu kunt, speakToDetective niks,
                                                //explanation is het begin van de accusation ("So, who is the murderer?") en accusation dat je naar het vorige menu kunt.

        public AudioClip interruptClipInitial;  //Wat de suspect voor het eerst zegt bij een ineffective interruption
        public AudioClip interruptClip;         //Wat de suspect later zegt bij een ineffective interruption
    }

    [Header("Intro Setup")]
    public AudioClip[] introClips;              //Alle introclips op volgorde die worden afgespeeld aan het begin van een beschuldig-session

    [Header("Suspects en Accusation Setup")]
    public Suspect[] suspects;                  //De suspects in de case

    public AudioClip[] accuseCorrect;           //Clips die je hoort als je de juiste suspect hebt geraden
    public AudioClip[] accuseWrong;             //Clips die je hoort als je een verkeerde suspect hebt gekozen
    public AudioClip[] interruptQuotes;         //Clips die je kunt horen als je een spreker onderbreekt
    public AudioClip[] excuseQuotes;            //Clips die je kunt horen als je een fout hebt gemaakt
}
