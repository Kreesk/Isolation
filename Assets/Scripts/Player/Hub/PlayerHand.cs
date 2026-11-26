
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

    public Transform ActiveBag => Bag.gameObject.activeSelf? Bag : FullBag;
    public Vector3 StartBagPosition => _startBagPosition;

    public event Action<int> OnFirstTimeCollected;
    bool _collectFirstTime;

    private Vector3 _startBagPosition;
    private Vector3 _startFullBagPosition;
    private Vector3 _startScale;

    private Tween _bagAnim;
    private Tween _garbageTween;

    private int _startMask;
    public PlayerHand(PlayerConfig config, PlayerSounds sounds)
    {
        Hand = config.Hand;
        Bag = config.Garbage;
        FullBag = config.FullBag;
        _sounds = sounds;

        _startBagPosition = Bag.localPosition;
        _startFullBagPosition = FullBag.localPosition;

        _startMask = Bag.gameObject.layer;
    }

    public void DropBag()
    {
        if (Occupied)
        {
            GetBag(false);
            _bagAnim.Stop();
            _garbageTween.Stop();
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
        FullBag.localPosition = _startFullBagPosition;
    }

    public void SetFullBag(bool f)
    {
        Bag.gameObject.SetActive(!f);
        FullBag.gameObject.SetActive(f);

        if(f)
            _sounds.PlayAction(PlayerActions.FullBag);
    }

    public void AnimateBag() =>  _bagAnim = Tween.ShakeLocalPosition(Bag, Vector3.down*0.05f, 1, frequency: 2f);
    public void AnimateThrow(Transform ps)
    {
        Vector3 startp = ActiveBag.position;
        Vector3 startScale = ActiveBag.localScale;

        ActiveBag.gameObject.layer = LayerMask.NameToLayer("Default");
        _garbageTween = Tween.Custom(0, 1f, duration: 0.12f ,t =>
        {
            Vector3 p = Vector3.Lerp(startp, ps.position + Vector3.up * 0.75f, t);
            Vector3 s = Vector3.Lerp(startScale, new Vector3(0.5f, 0.5f, 0.5f), t);
            ActiveBag.position = p;
            ActiveBag.localScale = s;
        }).OnComplete(() =>
        {
            ActiveBag.gameObject.layer = _startMask;
            ActiveBag.localScale = Vector3.one;
            DropBag();
        });
    } 

}
