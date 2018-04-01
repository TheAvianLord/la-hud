using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;

public class DictationScript : MonoBehaviour
{
    [SerializeField]
    private Text m_Recognitions;

    [SerializeField]
    private GameObject FourSquareHandler;

    private DictationRecognizer m_DictationRecognizer;
    private List<string> numbers;
    private List<string> locations;

    void Start()
    {
        numbers = new List<string>(new string[]{ "one", "2", "3", "4", "5" });
        locations = new List<string>(new string[] { "san francisco", "los angeles", "cerritos" });
        m_DictationRecognizer = new DictationRecognizer();
        m_DictationRecognizer.AutoSilenceTimeoutSeconds = 999;
        m_DictationRecognizer.InitialSilenceTimeoutSeconds = 999;
        getSpeech();
        m_DictationRecognizer.Start();
    }

    void Update()
    {
        var script = (KentFoursquareProvider)FourSquareHandler.GetComponent<MonoBehaviour>();
        if (!script.location_selected && locations.Contains(m_Recognitions.text))
        {
            script.NEAR_LOCATION = m_Recognitions.text;
            script.location_selected = true;
        }
        else if (script.location_selected && numbers.Contains(m_Recognitions.text))
        {
            script.VENUE_NUM_CHOICE = numbers.IndexOf(m_Recognitions.text) + 1;
            script.venue_selected = true;
        }
        else if (m_Recognitions.text == "reset")
            script.RESET_ALL = true;

    }

    private void getSpeech()
    {
        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            m_Recognitions.text = text;
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };
    }
}