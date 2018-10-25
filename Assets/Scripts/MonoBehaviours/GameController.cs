using System.Collections;
using UnityEngine;

public sealed class GameController : MonoBehaviour {

    public enum GameState { Interrogating, Accusing }
    public GameState gameState;

    public CaseData activeCaseData;           //De active case data: hieruit haalt de GameController de data die per case nodig is
    public int activeSuspect;                 //De geselecteerde suspect: dus degene die geselecteerd is bij het ondervragen of beschuldigen in het menu. De int is de index in de Suspect array in de Case Data
    public string activeSuspectName;          //Debug. Alleen voor de inspector.
    public bool[] interrupted = new bool[4];  //Bool array, houdt bij of suspect al is geinterrupt.
    public AudioClip[] winSequence;           //De sequence van audioclips die speelt als je wint
    public AudioClip interruptTutorial;       //Info clip
    public bool interruptInfoGiven;           //Is er al gezegd of de speler met spatiebalk kan interrupten
    public int strikes;                       //3 strikes en de speler is game-over.
    public AudioClip gameOverClip;            //Boze Detective Mallone
    public AudioSource bgSource;

    public bool canPlayerInput { get; set; }  //True als de speler keuzes kan maken (ondervragen en beschuldigen en zo)
    public bool isInterrogating { get; set; } //True als de interrogation functie aan het afspelen is

    private AudioSource audioSource;          //De audiosource die alles afspeelt
    private bool interrupt;                   //Trigger bij interruption

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
        PlayAtSource(activeCaseData.suspects[activeSuspect].speakToMenu);
        canPlayerInput = true;
    }

    private void Update()
    {
        activeSuspectName = activeCaseData.suspects[activeSuspect].suspectName;
        if (canPlayerInput)
        {
            if (gameState == GameState.Interrogating)
            {
                //Tijdens het ondervragen van de suspects
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (activeSuspect <= 0)
                    {
                        activeSuspect = activeCaseData.suspects.Length - 1;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakToMenu);
                    }
                    else
                    {
                        activeSuspect--;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakToMenu);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (activeSuspect >= activeCaseData.suspects.Length - 1)
                    {
                        activeSuspect = 0;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakToMenu);
                    }
                    else
                    {
                        activeSuspect++;
                        PlayAtSource(activeCaseData.suspects[activeSuspect].speakToMenu);
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
        if (isInterrogating && !activeCaseData.suspects[activeSuspect].nextMenu && Input.GetKeyDown(KeyCode.Space))
        {
            interrupt = true;
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
        PlayAtSource(activeCaseData.suspects[index].speakToDetective);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        if (!interruptInfoGiven)
        {
            interruptInfoGiven = true;
            PlayAtSource(interruptTutorial);
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
        isInterrogating = true;
        PlayAtSource(activeCaseData.suspects[index].explanation);
        yield return new WaitUntil(() => !audioSource.isPlaying || interrupt);
        if (interrupt)
        {
            StartCoroutine(Interrupt(index));
            yield break;
        }
        isInterrogating = false;
        audioSource.Stop();
        canPlayerInput = true;
    }

    private IEnumerator Interrupt(int index)
    {
        if (activeCaseData.suspects[index].guilty)
        {
            //Kijk of interruption effective of ineffective is adhv sample point van audio source
            if (audioSource.timeSamples >= activeCaseData.suspects[index].effectiveInterruption.x && audioSource.timeSamples <= activeCaseData.suspects[index].effectiveInterruption.y)
            {
                Debug.Log("Effective interruption!");
                PlayAtSource(activeCaseData.interruptQuotes);
                yield return new WaitUntil(() => !audioSource.isPlaying);
                PlayAtSource(activeCaseData.suspects[activeSuspect].interruptClipInitial);
                yield return new WaitUntil(() => !audioSource.isPlaying);
                for (int i = 0; i < winSequence.Length; i++)
                {
                    PlayAtSource(winSequence[i]);
                    yield return new WaitUntil(() => !audioSource.isPlaying);
                }
                //Hier stoppen we de app voor nu.
                yield return StartCoroutine(Quit());
            }
            else
            {
                Debug.Log("Stommerd.");
                PlayAtSource(activeCaseData.interruptQuotes);
                yield return new WaitUntil(() => !audioSource.isPlaying);
                if (!interrupted[activeSuspect])
                {
                    PlayAtSource(activeCaseData.suspects[activeSuspect].interruptClipInitial);
                    interrupted[activeSuspect] = true;
                }
                else
                {
                    PlayAtSource(activeCaseData.suspects[activeSuspect].interruptClip);
                }
                yield return new WaitUntil(() => !audioSource.isPlaying);
                PlayAtSource(activeCaseData.excuseQuotes);
                yield return new WaitUntil(() => !audioSource.isPlaying);
                yield return StartCoroutine(Strike());
                isInterrogating = false;
                interrupt = false;
                audioSource.Stop();
                canPlayerInput = true;
            }
        }
        else
        {
            //Ineffective sowieso
            Debug.Log("Stommerd.");
            PlayAtSource(activeCaseData.interruptQuotes);
            yield return new WaitUntil(() => !audioSource.isPlaying);
            if (!interrupted[activeSuspect])
            {
                PlayAtSource(activeCaseData.suspects[activeSuspect].interruptClipInitial);
                interrupted[activeSuspect] = true;
            }
            else
            {
                PlayAtSource(activeCaseData.suspects[activeSuspect].interruptClip);
            }
            yield return new WaitUntil(() => !audioSource.isPlaying);
            PlayAtSource(activeCaseData.excuseQuotes);
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return StartCoroutine(Strike());
            isInterrogating = false;
            interrupt = false;
            audioSource.Stop();
            canPlayerInput = true;
        }
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
            PlayAtSource(activeCaseData.suspects[0].speakToMenu);
            yield break;
        }
        //Check of de accusation juist is
        if (activeCaseData.suspects[index].guilty)
        {
            //Juist
            for (int i = 0; i < activeCaseData.accuseCorrect.Length; i++)
            {
                PlayAtSource(activeCaseData.accuseCorrect[i]);
                yield return new WaitUntil(() => !audioSource.isPlaying);
            }
            audioSource.Stop();
            canPlayerInput = true;
            //Hier stoppen we de app voor nu.
            yield return StartCoroutine(Quit());
        }
        else
        {
            //Onjuist
            for (int i = 0; i < activeCaseData.accuseWrong.Length; i++)
            {
                PlayAtSource(activeCaseData.accuseWrong[i]);
                yield return new WaitUntil(() => !audioSource.isPlaying);
            }
            yield return StartCoroutine(Strike());
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

    private void PlayAtSource(AudioClip[] clips)
    {
        audioSource.Stop();
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }

    private IEnumerator Strike()
    {
        strikes++;
        if (strikes >= 3)
        {
            PlayAtSource(gameOverClip);
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return StartCoroutine(Quit());
        }
    }

    private IEnumerator Quit()
    {
        while (Mathf.Approximately(bgSource.volume, 0))
        {
            bgSource.volume = Mathf.MoveTowards(bgSource.volume, 0, Time.deltaTime / 10);
            yield return null;
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}