
using System;
using PrimeTween;
using UnityEngine;

public class PlayerHand 
{
    private PlayerSounds _sounds;

    public readonly Transform Hand;
    public readonly Transform Bag;
    public readonly Transform FullBag;
    public bool Occupied => Bag.gameObject.activeSelf || FullBag.gameObject.activeSelf;
    public Vector3 StartBagPosition => _startBagPosition;

    public event Action<int> OnFirstTimeCollected;
    bool _collectFirstTime;

    private Vector3 _startBagPosition;
    private Tween _bagAnim;
    public PlayerHand(PlayerConfig config, PlayerSounds sounds)
    {
        Hand = config.Hand;
        Bag = config.Garbage;
        FullBag = config.FullBag;
        _sounds = sounds;

        _startBagPosition = Bag.localPosition;
    }

    public void DropBag()
    {
        if (Occupied)
        {
            GetBag(false);
            _bagAnim.Stop();
        }
    } 
    public bool PickUpBag()
    {
        if (Occupied)return false;
        if (!_collectFirstTime)
        {
            OnFirstTimeCollected?.Invoke(1);

            _collectFirstTime = true;
        }

        GetBag(true);
        return true;
    } 

    private void GetBag(bool t)
    {
        Bag.gameObject.SetActive(t);
        FullBag.gameObject.SetActive(false);
        Bag.localPosition = _startBagPosition;
    }

    public void SetFullBag(bool f)
    {
        Bag.gameObject.SetActive(!f);
        FullBag.gameObject.SetActive(f);

        if(f)
            _sounds.PlayAction(PlayerActions.FullBag);
    }

    public void AnimateBag() =>  _bagAnim = Tween.ShakeLocalPosition(Bag, Vector3.down*0.05f, 1, frequency: 2f);
}
