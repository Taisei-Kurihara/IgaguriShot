using TMPro;
using R3;
using UnityEngine;
using System;

public interface TextUpdate
{
    TextMeshProUGUI Text { get; set; }
    void UpdateText(string st) { Text.text = st; }



    void UpdateSetting<T>(T st);

    IDisposable Subscription { get; set; }

    void ObserveReactiveProperty<T>(ReactiveProperty<T> rea)
    {
        // ���łɍw�ǒ��Ȃ��x����
        Unsubscribe();

        Subscription = rea.Subscribe(newValue => {
            UpdateSetting(newValue);
        });
    }

    void Unsubscribe()
    {
        Subscription?.Dispose();
        Subscription = null;
    }

}