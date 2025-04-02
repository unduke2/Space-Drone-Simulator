using UnityEngine;

public static class DroneAudioManager
{
    public static void PlayFireSound(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;
        source.PlayOneShot(clip);
    }
}
