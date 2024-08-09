using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip[] playerFootsteps;
    public AudioClip[] enemyFootsteps;
    public AudioClip[] hitSounds;
    public AudioClip[] dieSounds;
    public AudioClip[] attackSounds;
    public AudioClip fireBallSound;
    public AudioClip lazerSound;


    public void PlayPlayerFootsteps(AudioSource _source) => PlaySound(GetRandomSound(playerFootsteps), _source);
    public void PlayEnemyFootsteps(AudioSource _source) => PlaySound(GetRandomSound(enemyFootsteps), _source);
    public void PlayHitSound(AudioSource _source) => PlaySound(GetRandomSound(hitSounds), _source);
    public void PlayDieSound(AudioSource _source) => PlaySound(GetRandomSound(dieSounds), _source);
    public void PlayAttackSound(AudioSource _source) => PlaySound(GetRandomSound(attackSounds), _source);
    public void PlayFireballSound(AudioSource _source) => PlaySound(fireBallSound, _source);
    public void PlayLazerSound(AudioSource _source) => PlaySound(lazerSound, _source);

    private AudioClip GetRandomSound(AudioClip[] _clips)
    {
        return _clips[Random.Range(0, _clips.Length)];
    }

    private void PlaySound(AudioClip _clip, AudioSource _source, float volume = 1)
    {
        if (_clip == null || _source == null)
            return;

        _source.clip = _clip;
        _source.volume = volume;
        _source.pitch = Random.Range(0.8f, 1.2f);
        _source.Play();
    }
}
