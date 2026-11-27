using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[Serializable]
public sealed class UIConfig
{
    [SerializeField] PlayerStateRefs _player;
    [SerializeField] MessageRefs _messages;
    [SerializeField] TimerRefs _timer;
    [SerializeField] PaperRefs _paper;
    [SerializeField] MenuRefs _menu;
    [SerializeField] ScoreRefs _scores;
    [SerializeField] MapRefs _mapRefs;
    [SerializeField] Tutorial _tutorial;
    [SerializeField] EndingRefs _endings;
    [SerializeField] VolumeEffects _volumeEffects;

    public PlayerStateRefs Player => _player;
    public MessageRefs Messages => _messages;
    public TimerRefs Timer => _timer;
    public PaperRefs Paper => _paper;
    public MenuRefs Menu => _menu;
    public ScoreRefs Scores => _scores;
    public MapRefs Map => _mapRefs;
    public Tutorial Tutorial => _tutorial;
    public EndingRefs Ending => _endings;
    public VolumeEffects VolumeEffects => _volumeEffects;
}

[Serializable]
public sealed class PlayerStateRefs
{
    [SerializeField] Image _stamina;

    public Image Stamina => _stamina;
}

[Serializable]
public sealed class MessageRefs
{
    [Serializable]
    public struct ResultMessage
    {
        public InteractResult result;
        public LocalizedString text;
    }

    [SerializeField] Image _hold;
    [SerializeField] TextMeshProUGUI _messageObject; 
    [SerializeField] ResultMessage[] _results;

    public Image Hold => _hold;
    public TextMeshProUGUI MessageObject => _messageObject;

    public LocalizedString Get(InteractResult result)
    {
        for (int i = 0; i < _results.Length; i++)
            if (_results[i].result == result)
                return _results[i].text;

        return null;
    }
}

[Serializable]
public sealed class TimerRefs 
{
    [SerializeField] TimerCounter _counter;
    [SerializeField] TextMeshProUGUI _text;

    public TimerCounter Counter => _counter;
    public TextMeshProUGUI Text => _text;
}

[Serializable]
public sealed class PaperRefs
{
    [SerializeField] PaperView[] _paperViews;

    public GameObject Current {get; private set;}

    public void SetCurrent(int i) => Current = _paperViews[i].Note;

    public PaperView GetPaper(int i)
    {
        if (_paperViews == null || i < 0 || i >= _paperViews.Length)return null;

        return _paperViews[i];
    }
}
[Serializable]
public sealed class MenuRefs
{
    [SerializeField] GameObject[] _menu;
    [SerializeField] GameObject[] _gameplay;
    [SerializeField] GameObject _pause;
    [SerializeField] GameObject _options;
    [SerializeField] GameObject _devs;
    [SerializeField] GameplaySettings _gameplaySettings;
    [SerializeField] RawImage[] _background;

    public GameObject[] MenuComponents => _menu;
    public GameObject[] GameplayComponents => _gameplay;
    public GameObject Pause => _pause;
    public GameObject Options => _options;
    public GameObject Devs => _devs;
    public GameplaySettings GameplaySettings => _gameplaySettings;
    public RawImage Background {get; private set;}

    public RawImage GetCurrentBackground(int i) => Background = _background[i] ;
}
[Serializable]
public sealed class ScoreRefs
{
    
    [Serializable]
    private struct FearRefs
    {
        [SerializeField] LocalizedString _total;
        [SerializeField] FearState _state;

        public LocalizedString Text => _total;
        public FearState State => _state;
    }
    [Serializable]
    private struct ReprimandRefs
    {
        [SerializeField] LocalizedString _text;
        [SerializeField] QuotaState _quotaState;

        public LocalizedString Text => _text;
        public QuotaState QuotaState => _quotaState; 
    }

    [SerializeField] FearRefs[] _fears;
    [SerializeField] ReprimandRefs[] _quota;
    [SerializeField] GameObject _scoreTotal;

    [SerializeField] TextMeshProUGUI _scoreTotalPaperCollected;
    [SerializeField] TextMeshProUGUI _scoreTotalCollected;
    [SerializeField] TextMeshProUGUI _scoreDay;
    [SerializeField] TextMeshProUGUI _fearStateText;
    [SerializeField] TextMeshProUGUI _quotaStateText;

    public GameObject ScoreTotal => _scoreTotal;
    public TextMeshProUGUI ScoreTotalCollected => _scoreTotalCollected;
    public TextMeshProUGUI ScoreTotalPaperCollected => _scoreTotalPaperCollected;
    public TextMeshProUGUI ScoreDay => _scoreDay;
    public TextMeshProUGUI FearStateText => _fearStateText;
    public TextMeshProUGUI QuotaStateText => _quotaStateText;

    public LocalizedString GetFearState(FearState state)
    {
        for (int i = 0; i < _fears.Length; i++)
            if(_fears[i].State == state)
                return _fears[i].Text;

        return null;
    }
    public LocalizedString GetQuotaState(QuotaState state)
    {
        for (int i = 0; i < _quota.Length; i++)
            if(_quota[i].QuotaState == state)
                return _quota[i].Text;

        return null;
    }
}
[Serializable]
public sealed class MapRefs
{
    [SerializeField] AudioSource _heartBeating;
    [SerializeField] TextMeshProUGUI _totalItems;
    [SerializeField] Transform _spawn;
    [SerializeField] GameObject _garbageTexture;

    [Header("StartGame")]
    [SerializeField] float _duration;
    [SerializeField] CanvasGroup _startCanvas;
    [SerializeField] EventSystem _system;

    public AudioSource HeartBeating => _heartBeating;
    public TextMeshProUGUI TotalItems => _totalItems;
    public Transform Spawn => _spawn;
    public GameObject GarbageTexture => _garbageTexture;
    public EventSystem System => _system;
    public CanvasGroup StartCanvas => _startCanvas;
    public float Duration => _duration;
}
[Serializable]
public sealed class Tutorial
{
    [SerializeField] bool _enabled;
    [SerializeField] GameObject _panel;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] LocalizedString[] _prompts;

    public bool Enabled => _enabled;
    public GameObject Panel => _panel;
    public TextMeshProUGUI Text => _text;
    public LocalizedString[] Prompts => _prompts;
}
[Serializable]
public sealed class EndingRefs
{
    [Header("Main")]
    [SerializeField] EndingView[] _endings;
    [SerializeField] GameObject _ending;
    [SerializeField] GameObject _endingStampPanel;
    public GameObject EndingStampPanel => _endingStampPanel;
    
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _currentText;
    [SerializeField] TextMeshProUGUI _currentStampText;
    public GameObject EndingPanel => _ending;

    public TextMeshProUGUI CurrentText => _currentText;
    public TextMeshProUGUI CurrentStampText => _currentStampText;

    public EndingView GetEnding(int i)
    {
        if(_endings == null || i < 0 || i >= _endings.Length)return null;
        return _endings[i];
    }
}
[Serializable]
public sealed class VolumeEffects
{
    [SerializeField] Volume _volume;
    public Volume Volume => _volume;


    public Vignette Vignette{get; private set;}
    public ColorAdjustments ColorAdjustments{get; private set;}

    public void Initialize()
    {
        if(_volume == null || _volume.profile == null)return;

        _volume.profile.TryGet(out Vignette vignette);
        _volume.profile.TryGet(out ColorAdjustments colorAdjustments);

        Vignette = vignette;
        ColorAdjustments = colorAdjustments;
    }

    public void Destroy()
    {
        if(Vignette != null)
            Vignette = null;

        if(ColorAdjustments != null)
            ColorAdjustments = null;
    }

    public void ActiveVignette(bool active) => Vignette.active = active;
    public void ActiveColor(bool active) => ColorAdjustments.active = active;
}