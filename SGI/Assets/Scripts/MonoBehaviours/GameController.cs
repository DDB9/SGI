using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour {

    public enum GameState { Intro, Questioning, Interrogate, Accusation }
    public GameState gameState = GameState.Intro;
    public KeyCode accuseKey;   //Keycode in Questioning state om naar Accusation state te gaan
    public CaseData caseData;   //Gebruikt manager om de suspects te processen.

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
                        canAccuse = false;
                        StartCoroutine(PlayAccuseClip());
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
                                    Debug.Log("Goed gedaan zeg.");
                                }
                                else
                                {
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
                        StartCoroutine(PlayAccuseClip());
                    }
                }
            }
        }
    }

    private IEnumerator MenuOptions()
    {
        m_AudioSource.clip = caseData.menuClip;
        m_AudioSource.Play();
        yield return new WaitUntil(() => m_AudioSource.isPlaying == false);
        waitForInput = true;
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

    private IEnumerator PlayAccuseClip()
    {
        hasAccuseClipPlayed = true;
        m_AudioSource.clip = caseData.whoIsGuiltyClip;
        m_AudioSource.Play();
        yield return new WaitUntil(() => m_AudioSource.isPlaying == false);
        waitForInput = true;
        canAccuse = true;
    }

    [ForNextVersion]
    private IEnumerator AccuseSuspect(CaseData.Suspect suspect)
    {
        yield break;
    }
}