using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TypeWriterEffect : MonoBehaviour
{
    private TMP_Text _textBox;
    
    
    [Header("Test String")]
    [SerializeField] private string testText;
    
    //Basic Typewriter Functionality
    private int _currentVisibleCharactersIndex;
    private Coroutine _typeWriterCoroutine;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctationDelay;
    
    [Header("TypeWriter Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctationDelay = 0.5f;
    
    
    //Skiping functionality
    public bool CurrentlySkiping { get; private set; }
    private WaitForSeconds _skipDelay;

    [Header("Skip Settings")]
    [SerializeField] private bool quickSkip;
    [SerializeField] [Min(1)] private int skipSpeed = 5;
    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        
        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctationDelay = new WaitForSeconds(interpunctationDelay);
        
        _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeed));
    }


    private void Start()
    {
        EventSystem.SkipText.AddListener(Skip);
        SetText(testText);
    }

    void Skip()
    {
        StopCoroutine(_typeWriterCoroutine);
        _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
    }

    private IEnumerator skipSpeedReset()
    {
        yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount -1);
        CurrentlySkiping = false;
    }

    private void SetText(string text)
    {
        if (_typeWriterCoroutine != null)
        {
            StopCoroutine(_typeWriterCoroutine);
        }

        _textBox.text = text;
        _textBox.ForceMeshUpdate();

        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharactersIndex = 0;

        _typeWriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = _textBox.textInfo;

        while (_currentVisibleCharactersIndex < textInfo.characterCount)
        {
            if (_currentVisibleCharactersIndex >= textInfo.characterInfo.Length)
                break;  // Safety check

            char charakter = textInfo.characterInfo[_currentVisibleCharactersIndex].character;

            _textBox.maxVisibleCharacters++;
            
            //TODO: Add special effects
            /*
            if (charakter == '~')
            {
                VoblingEffect();
            }
            */
            if (!CurrentlySkiping &&
                (charakter == '.' || charakter == '!' || charakter == '?' || charakter == ',' || charakter == ':' ||
                 charakter == ';' || charakter == '-' || charakter == '–' || charakter == '—' || charakter == '…' ||
                 charakter == '„' || charakter == '”'))
            {
                yield return _interpunctationDelay;
            }
            else
            {
                yield return CurrentlySkiping ? _skipDelay : _simpleDelay;
            }

            _currentVisibleCharactersIndex++;

            if (_currentVisibleCharactersIndex == textInfo.characterCount - 1)
            {
                EventSystem.SkipedText.Invoke(true);
            }
        }
    }

}
