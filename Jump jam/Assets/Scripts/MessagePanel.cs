using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _text;
    [SerializeField] private Drive _car;
    [SerializeField] private Drive _bot;

    private bool _show = false;

    public string[] Messages = { "Faaaster!", "Boooooost!", "Chaaarge!", "Yahoooo!" };

    public IEnumerator ShowMessage(string value)
    {
        _text.text = value;
        _panel.transform.DOScaleY(1, 0.2f);
        yield return new WaitForSeconds(1f);
        _panel.transform.DOScaleY(0, 0.2f);
    }

    private void FixedUpdate()
    {
        if (_car.transform.position.z > _bot.transform.position.z && !_show && _car.Jumped)
        {
            _show = true;
            StartCoroutine(ShowMessage("Amazing Jump!!!"));
        }
    }
}
