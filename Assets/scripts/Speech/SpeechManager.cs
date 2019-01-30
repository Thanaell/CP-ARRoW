using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;

/// <summary>
/// Classe permettant de faire des commandes via la voix en utilisant les outils de l'HoloToolkit
/// </summary>
public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        //Commande vocale "Lock" qui va attacher l'objet avec un Spatial Anchor
        keywords.Add("Lock", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                WorldAnchorManager.Instance.AttachAnchor(focusObject);
            }
        });

        //Commande vocale "Unlock" qui va détacher l'objet en lui enlevant son Spatial Anchor
        keywords.Add("Unlock", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                WorldAnchorManager.Instance.RemoveAnchor(focusObject);
            }
        });

        //Commande vocale "Move" qui va déclencher la méthode "OnMove" de l'objet regardé
        keywords.Add("Move", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                focusObject.SendMessage("OnMove", SendMessageOptions.DontRequireReceiver);
            }
        });

        //Commande vocale "Stop" qui va déclencher la méthode "OnStop" de l'objet regardé
        keywords.Add("Stop", () =>
        {
            var focusObject = GazeInteractionManager.Instance.FocusedObject;
            if (focusObject != null)
            {
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