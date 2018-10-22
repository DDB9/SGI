using UnityEngine;

[System.Obsolete]
[CreateAssetMenu(menuName = "Framed Audio Clip")]
public class AudioClipTimeframed : ScriptableObject
{
    //Audio Clip maar dan Timeframed voor onderbrekingen.

    public AudioClip clip;
    public int frameStart;                                  //Sample point waarop de onderbreking effectief is (tot frameEnd)
    public int frameEnd = -1;                               //Sample point waarop de onderbreking niet meer effectief is. -1 is tot einde
    public ConversationSequence resultInFrame;              //Conversatie die speelt als de onderbreking effectief was
    public ConversationSequence resultOutOfFrameInitial;    //Conversatie die speelt als de onderbreking niet effectief was, voor de eerste keer
    public ConversationSequence resultOutOfFrame;           //Conversatie die speelt bij een ineffectieve onderbreking voor een meerdere keer
}