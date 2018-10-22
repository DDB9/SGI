using System.Collections;
using UnityEngine;

[System.Obsolete("Script is obsolete. Use GameController instead.")]
public class GameManager : MonoBehaviour {

    public ConversationSequence initialSequence;    //Conversatie waarmee de game start
    public ConversationSequence playingSequence;    //Conversatie die aan het afspelen is

    [HideInInspector] public AudioSource audioSource;

    public bool isPlaying { get; set; }

    private IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        yield return StartCoroutine(initialSequence.Play(this));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && audioSource.isPlaying && playingSequence.playingTimeframedClip)
        {
            StopCoroutine(playingSequence.Play(this));
            Debug.Log("Interrupt at " + audioSource.timeSamples);
            //Kijk of de interruption effectief is
            int interruption = audioSource.timeSamples;
            if (interruption > playingSequence.playingTimeframedClip.frameStart 
                && interruption < playingSequence.playingTimeframedClip.frameEnd 
                || playingSequence.playingTimeframedClip.frameEnd <= -1)
            {
                //Effectief
                StartCoroutine(playingSequence.playingTimeframedClip.resultInFrame.Play(this));
            }
            else
            {
                StartCoroutine(playingSequence.playingTimeframedClip.resultOutOfFrameInitial.Play(this));
            }
        }
    }
}
