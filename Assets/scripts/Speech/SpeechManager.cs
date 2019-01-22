using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        keywords.Add("Lock", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                //focusObject.SendMessage("OnKill", SendMessageOptions.DontRequireReceiver);
                WorldAnchorManager.Instance.AttachAnchor(focusObject);
            }
        });

        keywords.Add("Unlock", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                //focusObject.SendMessage("OnKill", SendMessageOptions.DontRequireReceiver);
                WorldAnchorManager.Instance.RemoveAnchor(focusObject);
            }
        });

        keywords.Add("Move", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnMove", SendMessageOptions.DontRequireReceiver);
            }
        });

        keywords.Add("Stop", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnStop", SendMessageOptions.DontRequireReceiver);
            }
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}