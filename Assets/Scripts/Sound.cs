using UnityEngine;

public class Sound : MonoBehaviour {
	
	public static Sound i;
	public AudioClip music;
	public AudioClip ping;
    public AudioClip lastPing;
	public AudioClip wave;


	void Awake(){
		i = this;
	}
	
	public static void MuteAll()
	{
		AudioSource[] sources = i.transform.GetComponents<AudioSource>();
		foreach(AudioSource aus in sources){
			if(aus.isPlaying){
				aus.Stop();
			}
		}
	}
	
	public static void Play (AudioClip clipToPlay)
	{
	    if (clipToPlay == null) return;

		AudioSource aus = FindOrAllocateNewSource();
		aus.clip = clipToPlay;
		aus.Play();
	}

    public static void Play( AudioClip clipToPlay , float vol)
    {
        if ( clipToPlay == null ) return;

        AudioSource aus = FindOrAllocateNewSource();
        aus.clip = clipToPlay;
        aus.volume = vol;
        aus.Play();
    }

	static AudioSource FindOrAllocateNewSource(){
		AudioSource[] sources = i.transform.GetComponents<AudioSource>();
		foreach(AudioSource aus in sources){
			if(!aus.isPlaying){
				return aus;
			}
		}

		return i.gameObject.AddComponent<AudioSource>();
	}
}
