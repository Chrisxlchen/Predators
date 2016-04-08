using UnityEngine;
using System.Collections;

public class AskQuestions : MonoBehaviour {
    // Use this for initialization
    GameObject player;
    AudioSource audioS;
    bool initial_greeting;
    private int minFreq, maxFreq;
    AudioClip RecClip;
    private float timeElapse = 0;
    private int recTimeLen = 10;

    void Start () {
        timeElapse = 0;
        initial_greeting = false;
        player = GameObject.FindGameObjectWithTag("Player");
        audioS = GetComponent<AudioSource>();
        SetUPMicrophone();
        // StartMicListener();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        timeElapse += Time.deltaTime;
        print(timeElapse);
        // If the audio has stopped playing, this will restart the mic play the clip.
        if (!audioS.isPlaying)
        {
            StartRecord();
            // StartMicListener();
        }

        if ((transform.position - player.transform.position).sqrMagnitude <= 3 && initial_greeting == false)
        {
            AudioClip aclip;
            aclip = Resources.Load("Audio/howdoyoudo") as AudioClip;
            audioS.clip = aclip;

            audioS.Play();
            initial_greeting = true;
        }
        if (timeElapse >= recTimeLen)
        {
            StopRecord();
            PlayRecord();
        }
        
    }

    void SetUPMicrophone()
    {
        Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
        if (minFreq == 0 && maxFreq == 0)
        {
            maxFreq = 44100;
        }
        print("maxFreq: " + maxFreq + " minFreq: " + minFreq);
    }

    void StartRecord()
    {
        if (!Microphone.IsRecording(null))
        {
            RecClip = Microphone.Start(null, true, recTimeLen, maxFreq);
        }
    }
    void StopRecord()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            SavWav.Save("test", RecClip);
        }
    }

    void PlayRecord()
    {
        if (Microphone.IsRecording(null))
        {
            print("still recording " + Time.time);
            return;
        }
        else if (!audioS.isPlaying)
        {
            audioS.clip = RecClip;
            audioS.Play();
            print("Now the recode is finished.");
        }
    }

    /*
    /// Starts the Mic, and plays the audio back in (near) real-time.
    private void StartMicListener()
    {
        audioS.clip = Microphone.Start("Built-in Microphone", true, 999, maxFreq);
        // HACK - Forces the function to wait until the microphone has started, before moving onto the play function.
        while (!(Microphone.GetPosition("Built-in Microphone") > 0)) { }
        audioS.Play();
    }*/
}
