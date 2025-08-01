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
        // すでに購読中なら一度解除
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