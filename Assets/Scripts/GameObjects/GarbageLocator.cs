
using System;
using UnityEngine;
public enum QuotaState{FullShiftCollected , Collected, NotCollected, WarningToCollect}
public class GarbageLocator : MonoBehaviour
{
    ItemsCollect[] _items;
    PapperShow[] _info;

    private int _quota;
    int _remainingItem;
    int _remainingPaper;

    bool _inited;
    public QuotaState State {get; private set;}

    public int TotalItem => _items.Length;
    public int TotalPaper => _info.Length;
    public event Action<int, int> Changed; 
    public event Action<int, int> InfoChanged;


    public event Action AllCollected;
    public event Action<QuotaState> QuotaCollected;

    void Awake() => Init();


    void Init()
    {
        if(!_inited)
            _inited = true;

        _items = GetComponentsInChildren<ItemsCollect>(true);
        _info = GetComponentsInChildren<PapperShow>(true);
        _remainingItem = _items.Length;
        _remainingPaper = _info.Length;

        _quota = _items.Length/2;

        foreach (var item in _items)
            item.Collected += OnCollected;

        foreach(var item in _info)
            item.OnCollected += OnCollectedPaper;
    }
    void OnDestroy()
    {
        if (_items == null) return;
        foreach (var item in _items)
            if (item != null) item.Collected -= OnCollected;

        if(_info == null)return;
        foreach(var item in _info)
            item.OnCollected -= OnCollectedPaper;
    }
    private void OnCollected(ItemsCollect _)
    {
        _remainingItem--;
        Changed?.Invoke(_remainingItem, TotalItem);     
        UpdateState();
        if(_remainingItem <= 0)
        {
            AllCollected?.Invoke();
        }
    }

    public void OnCollectedPaper(PapperShow c)
    {
        _remainingPaper--;
        InfoChanged?.Invoke(_remainingPaper, TotalPaper);
    }

    public void RestartLocator()
    {
        _items = GetComponentsInChildren<ItemsCollect>(true);
        _info = GetComponentsInChildren<PapperShow>(true);
        
        _remainingItem = _items.Length;
        _quota = _items.Length/2;
        
        foreach (var item in _items)
        {
            item.gameObject.SetActive(true);
            item.enabled = true;
        }     
        foreach (var item in _info)
        {
            item.gameObject.SetActive(true);
            item.enabled = true;
        }
    }
    private QuotaState Evaluate(int remain)
    {
        if(remain <= 0)return QuotaState.FullShiftCollected;
        if(remain <= _quota)return QuotaState.Collected;

        return QuotaState.NotCollected;
    }
    public void UpdateState()
    {
        QuotaState s = Evaluate(_remainingItem);
        
        State = s;

        QuotaCollected?.Invoke(State);
    }
}
