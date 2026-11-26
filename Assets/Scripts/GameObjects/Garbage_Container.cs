using Assets.Scripts.Inputmanager;
using PrimeTween;
using UnityEngine;
using UnityEngine.Localization;

public class Garbage_Container : AudioAbs, IInteract
{
    [SerializeField] LocalizedString _string;
    [SerializeField] Outline _outline;

    public LocalizedString Prompt => _string;
    public InputPhase Phase => InputPhase.performed;

    public Outline Outline => _outline;

    Tween _container;
    public InteractResult Check(PlayerCarry carry, PlayerHand hand)
    {
        if(!hand.Occupied)return InteractResult.NoBag;
        if(hand.Occupied && carry.CurrentBagCollected <= 0)return InteractResult.EmptyBag;
        return InteractResult.Success;
    }

    public InteractResult Interact(PlayerCarry carry, PlayerHand hand)
    {
        InteractResult r = Check(carry, hand);
        Debug.Log($"{r} Garbage_Container");
        if(r != InteractResult.Success)return r;

        carry.FullDrop();
        PlayDefaultClips();
        AnimateContainer();
        hand.AnimateThrow(transform);
        return InteractResult.Success;
    }

    private void AnimateContainer()
    {
        Vector3 startpos = transform.position;
        _container = Tween.Position(transform, startpos + new Vector3(0, 0 ,-0.01f), 0.2f, Ease.InOutCubic, cycles: 2, CycleMode.Yoyo);
    }
    void OnDisable() => _container.Stop();
}
