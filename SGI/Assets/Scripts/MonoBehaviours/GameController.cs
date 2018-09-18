using System.Collections;
using UnityEngine;

public sealed class GameController : MonoBehaviour {

    public enum GameState { Interrogating, Accusing }
    public GameState gameState;

    public CaseData activeCaseData;           //De active case data: hieruit haalt de GameController de data die per case nodig is
    public int activeSuspect;                 //De geselecteerde suspect: dus degene die geselecteerd is bij het ondervragen of beschuldigen in het menu. De int is de index in de Suspect array in de Case Data
    public string activeSuspectName;          //Debug. Alleen voor de inspector.

    public bool canPlayerInput { get; set; }  //True als de speler keuzes kan maken (ondervragen en beschuldigen en zo)

    private AudioSource audioSource;          //De audiosource die alles afspeelt

    private IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        //Intro
        for (int i = 0; i < activeCaseData.introClips.Length; i++)
        {
            PlayAtSource(activeCaseData.introClips[i]);
            //Check of de AudioSource nog aan het afspelen is. Zo niet, dan gaat de Enumerator verder.
            yield return new WaitUntil(() => !audioSource.isPlaying || Input.GetKeyUp(KeyCode.Space));
            audioSource.Stop();
        }
        PlayAtSource(activeCaseData.suspects[activeSuspect].speakTo);
        canPlayerInput = true;
    }

    private void Update()
    {
        activeSuspectName = activeCaseData.suspects[activeSuspect].suspectName;
        if (canPlayerInput)
        {
            if (gameState == GameState.Interrogating)
            {
                Debug.Log("Suspects ondervragen");
                //Tijdens het ondervragen van de suspects
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (activeSuspect <= 0)
                    {
                        activeSuspect = activeCaseData.suspects.Length - 1;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakTo);
                    }
                    else
                    {
                        activeSuspect--;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakTo);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (activeSuspect >= activeCaseData.suspects.Length - 1)
                    {
                        activeSuspect = 0;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakTo);
                    }
                    else
                    {
                        activeSuspect++;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakTo);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Ondervraag een suspect
                    StartCoroutine(Interrogate(activeSuspect));
                }
            }
            else if (gameState == GameState.Accusing)
            {
                Debug.Log("Suspects beschuldigen");
                //Tijdens het beschuldigen van de suspects
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (activeSuspect <= 0)
                    {
                        activeSuspect = activeCaseData.suspects.Length - 1;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].accusation);
                    }
                    else
                    {
                        activeSuspect--;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].accusation);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (activeSuspect >= activeCaseData.suspects.Length - 1)
                    {
                        activeSuspect = 0;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].accusation);
                    }
                    else
                    {
                        activeSuspect++;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].accusation);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Ondervraag een suspect
                    StartCoroutine(Accuse(activeSuspect));
                }
            }
        }
    }

    private IEnumerator Interrogate(int index)
    {
        if (activeCaseData.suspects[index].nextMenu)
        {
            Debug.Log("Naar Accuse menu");
            gameState = GameState.Accusing;
        }
        canPlayerInput = false;
        PlayAtSource(activeCaseData.suspects[index].explanation);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        audioSource.Stop();
        canPlayerInput = true;
    }

    private IEnumerator Accuse(int index)
    {
        canPlayerInput = false;
        if (activeCaseData.suspects[index].nextMenu)
        {
            Debug.Log("Naar Interrogate menu");
            gameState = GameState.Interrogating;
            canPlayerInput = true;
            activeSuspect = 0;
            PlayAtSource(activeCaseData.suspects[0].speakTo);
            yield break;
        }
        //Check of de accusation juist is
        if (activeCaseData.suspects[index].guilty)
        {
            //Juist
            PlayAtSource(activeCaseData.accuseCorrect);
            yield return new WaitUntil(() => !audioSource.isPlaying);
            audioSource.Stop();
            canPlayerInput = true;
            //Hier stoppen we de app voor nu.
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        else
        {
            //Onjuist
            PlayAtSource(activeCaseData.accuseWrong);
            yield return new WaitUntil(() => !audioSource.isPlaying);
            audioSource.Stop();
            canPlayerInput = true;
        }
    }

    private void PlayAtSource(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

/*
    public enum GameState { Intro, Questioning, Interrogate, Accusation }
    public GameState gameState;
    public CaseData caseData;   //Gebruikt manager om de suspects te processen.
    public KeyCode accuseKey;

    public bool waitForInput { get; set; }  //If true, wacht op player keyboard input om het proces te resumeren.
    public bool canAccuse { get; set; }     //Kan de player al accusen of moet ie nog wachten tot de accuseClip afgelopen is?
    public bool hasAccuseClipPlayed { get; set; }

    private AudioSource m_AudioSource;

    private IEnumerator Start()
    {
        gameState = GameState.Intro;
        waitForInput = false;
        m_AudioSource = GetComponent<AudioSource>();
        //Speel Intro clips af
        for (int i = 0; i < caseData.introClips.Length; i++)
        {
            m_AudioSource.clip = caseData.introClips[i];
            m_AudioSource.Play();
            yield return new WaitUntil(() => m_AudioSource.isPlaying == false);
        }
        StartCoroutine(MenuOptions());
    }

    private void Update()
    {
        if (waitForInput)
        {
            if (gameState == GameState.Questioning)
            {
                //Doe input shit hier. We kunnen er van uit gaan dat Alpha1 altijd wordt gebruikt voor 'Speak to me again' en Spacebar voor menu options repeat
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    //Revert to intro hier
                    StartCoroutine(Start());
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(MenuOptions());
                }
                else if (Input.GetKeyDown(accuseKey))
                {
                    gameState = GameState.Accusation;
                    return;
                }
                else
                {
                    for (int i = 0; i < caseData.suspects.Length; i++)
                    {
                        if (Input.GetKeyDown(caseData.suspects[i].hotkey))
                        {
                            //Ondervraag suspects hier.
                            StartCoroutine(Interrogate(caseData.suspects[i]));
                        }
                    }
                }
            }
            else if (gameState == GameState.Accusation)
            {
                if (canAccuse)
                {
                    //Hier gaan we uit van Alpha1 = terug naar Questioning en 'accuseKey' om de opties opnieuw te horen.
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        gameState = GameState.Questioning;
                        hasAccuseClipPlayed = false;
                        canAccuse = false;
                        return;
                    }
                    else if (Input.GetKeyDown(accuseKey))
                    {
                        PlayAccuseClip();
                    }
                    else
                    {
                        for (int i = 0; i < caseData.suspects.Length; i++)
                        {
                            if (Input.GetKeyDown(caseData.suspects[i].hotkey))
                            {
                                //Accuse suspect hier.
                                if (caseData.suspects[i].guilty)
                                {
                                    m_AudioSource.clip = caseData.accuseCorrect;
                                    m_AudioSource.Play();
                                    Debug.Log("Goed gedaan zeg.");
                                }
                                else
                                {
                                    m_AudioSource.clip = caseData.accuseWrong;
                                    m_AudioSource.Play();
                                    Debug.Log("Probeer opnieuw.");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!hasAccuseClipPlayed)
                    {
                        PlayAccuseClip();
                    }
                }
            }
        }
    }

    private IEnumerator MenuOptions()
    {
        m_AudioSource.clip = caseData.menuClip;
        m_AudioSource.Play();
        waitForInput = true;
        yield return new WaitUntil(() => m_AudioSource.isPlaying == false);
        gameState = GameState.Questioning;
    }

    private IEnumerator Interrogate(CaseData.Suspect suspect)
    {
        waitForInput = false;
        gameState = GameState.Interrogate;
        m_AudioSource.clip = suspect.explanation;
        m_AudioSource.Play();
        yield return new WaitUntil(() => m_AudioSource.isPlaying == false);
        gameState = GameState.Questioning;
        waitForInput = true;
    }

    private void PlayAccuseClip()
    {
        hasAccuseClipPlayed = true;
        m_AudioSource.clip = caseData.whoIsGuiltyClip;
        m_AudioSource.Play();
        waitForInput = true;
        canAccuse = true;
    }

    [ForNextVersion]
    private IEnumerator AccuseSuspect(CaseData.Suspect suspect)
    {
        yield break;
    }*/
}