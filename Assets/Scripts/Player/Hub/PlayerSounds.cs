using System.Collections.Generic;
using UnityEngine;
public enum PlayerActions
{
    FullBag, TutorialNotification
}
[System.Serializable]
public struct PlayerActionSounds
{
    [SerializeField] PlayerActions actions;
    [SerializeField] AudioScriptableObj audio;

    public PlayerActions Actions => actions;
    public AudioScriptableObj Audio => audio;
}
public class PlayerSounds : AudioAbs
{
    [SerializeField] PlayerActionSounds[] actionSounds;

    private Dictionary<PlayerActions,  AudioScriptableObj> playerActionsDictionary;

    private float cooldown = 0;

    [SerializeField] float baseRate = 0.5f;
    [SerializeField] float magnitude = 0.17f;

    [SerializeField] float moveRate = 0.7f;
    [SerializeField] float runRate = 0.6f;

    private float GetStepRate(PlayerContext ctx)
    {
        if(ctx.IsSprinting) return runRate;
        return moveRate;
    }
    void Awake()
    {
        playerActionsDictionary = new Dictionary<PlayerActions, AudioScriptableObj>();
        foreach (var item in actionSounds)
        {
            if(!playerActionsDictionary.TryAdd(item.Actions, item.Audio))
            {
                Debug.LogWarning("THE SAME ACTIONS" + $"{item.Actions}", this);
            }
        }
    }

    public void Tick(PlayerContext ctx)
    {
        if(ctx.currentInput.sqrMagnitude > magnitude)
        {
            Rate(ctx);
        }
    }
    private void Rate(PlayerContext ctx)
    {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            PlayEnough(ctx.hit);
            float stepRate = baseRate;
            stepRate *= GetStepRate(ctx);
            cooldown = stepRate;
        }
    }
    private void PlayEnough(RaycastHit GroundHit)
    {
        if(_audio == null)return;
        if(GroundHit.collider == null)return;

        PlayFootStep(_audio, GroundHit.point);
    }
    private void PlayFootStep(AudioScriptableObj audio, Vector3 pos)
    {
        if(audio == null || audio.ArraClips.Length <= 0)return;
        AudioClip clip = GetRandomClip(audio.ArraClips);
        float pitch = RandomPitch(audio.RandomPitch, audio.MinPitch, audio.Pitch);

        PlayAtPosition(clip, pos, audio.Volume, pitch);
    }
    public void PlayAction(PlayerActions actions)
    {
        if(playerActionsDictionary.TryGetValue(actions, out AudioScriptableObj audio))
        {
            if(audio == null || audio.ArraClips.Length <= 0)return;

            PlayAtPosition(GetRandomClip(audio.ArraClips), transform.position, audio.Volume, RandomPitch(audio.RandomPitch, audio.MinPitch, audio.Pitch));
        }
    }
}
