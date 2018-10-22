using System.Collections;
using UnityEngine;

[System.Obsolete]
[CreateAssetMenu(menuName = "Conversation Sequence")]
public class ConversationSequence : ScriptableObject {

    public Object[] audioClips;

    public AudioClipTimeframed playingTimeframedClip { get; private set; }

    public IEnumerator Play(GameManager source)
    {
        if (source.isPlaying)
        {
            Debug.LogWarning("A Conversation Sequence is already playing", this);
            yield break;
        }
        //Start de sequence.
        source.isPlaying = true;
        for (int i = 0; i < audioClips.Length; i++)
        {
            //Speel alle audioClips af in de reeks
            AudioClip playable = GetValidatedClip(audioClips[i]);
            source.playingSequence = this;
            source.audioSource.clip = playable;
            source.audioSource.Play();
            //Wacht tot de audio source gestopt is met spelen en speel dan de volgende clip af
            while (source.audioSource.isPlaying)
            {
                yield return null;
            }
        }
        source.isPlaying = false;
    }

    //Functie om een juiste audioclip te krijgen van de Object[] audioClips.
    private AudioClip GetValidatedClip(Object target)
    {
        if (target.GetType() == typeof(AudioClip))
        {
            playingTimeframedClip = null;
            return target as AudioClip;
        }
        else if (target.GetType() == typeof(AudioClipTimeframed))
        {
            AudioClipTimeframed clip = target as AudioClipTimeframed;
            playingTimeframedClip = clip;
            return clip.clip;
        }
        else
        {
            throw new UnityException("No AudioClip or AudioClipTimeframed found on object " + target.name);
        }
    }
}
