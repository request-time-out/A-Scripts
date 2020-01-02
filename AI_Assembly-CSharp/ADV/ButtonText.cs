// Decompiled with JetBrains decompiler
// Type: ADV.ButtonText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ADV
{
  [RequireComponent(typeof (Button))]
  public class ButtonText : MonoBehaviour
  {
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI _tmpText;
    [SerializeField]
    private Text _unityText;

    public ButtonText()
    {
      base.\u002Ector();
    }

    private void SetColor(Color color)
    {
      if (Object.op_Inequality((Object) this._tmpText, (Object) null))
      {
        ((Graphic) this._tmpText).set_color(color);
      }
      else
      {
        if (!Object.op_Inequality((Object) this._unityText, (Object) null))
          return;
        ((Graphic) this._unityText).set_color(color);
      }
    }

    private void Start()
    {
      if (Object.op_Equality((Object) this._button, (Object) null))
      {
        this._button = (Button) ((Component) this).GetComponent<Button>();
        if (Object.op_Equality((Object) this._button, (Object) null))
        {
          Debug.LogError((object) "Button none", (Object) this);
          Object.Destroy((Object) this);
          return;
        }
      }
      if (Object.op_Equality((Object) this._tmpText, (Object) null) && Object.op_Equality((Object) this._unityText, (Object) null))
      {
        if (Object.op_Equality((Object) this._tmpText, (Object) null))
          this._tmpText = (TextMeshProUGUI) ((Component) this).GetComponentInChildren<TextMeshProUGUI>();
        if (Object.op_Equality((Object) this._unityText, (Object) null))
          this._unityText = (Text) ((Component) this).GetComponentInChildren<Text>();
        if (Object.op_Equality((Object) this._tmpText, (Object) null) && Object.op_Equality((Object) this._unityText, (Object) null))
        {
          Debug.LogError((object) "Component Text none", (Object) this);
          Object.Destroy((Object) this);
          return;
        }
      }
      ColorBlock colors = ((Selectable) this._button).get_colors();
      ReactiveProperty<bool> isOnUI = new ReactiveProperty<bool>(false);
      ReactiveProperty<bool> isPressUI = new ReactiveProperty<bool>(false);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) isOnUI, (Action<M0>) (isOn =>
      {
        if (isOn)
        {
          if (isPressUI.get_Value())
            this.SetColor(((ColorBlock) ref colors).get_pressedColor());
          else
            this.SetColor(((ColorBlock) ref colors).get_highlightedColor());
        }
        else if (isPressUI.get_Value())
          this.SetColor(((ColorBlock) ref colors).get_highlightedColor());
        else
          this.SetColor(((ColorBlock) ref colors).get_normalColor());
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) isPressUI, (Action<M0>) (isOn =>
      {
        if (isOn)
          this.SetColor(((ColorBlock) ref colors).get_pressedColor());
        else if (isOnUI.get_Value())
          this.SetColor(((ColorBlock) ref colors).get_highlightedColor());
        else
          this.SetColor(((ColorBlock) ref colors).get_normalColor());
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) this._button), (Action<M0>) (_ => isPressUI.set_Value(true)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerUpAsObservable((UIBehaviour) this._button), (Action<M0>) (_ => isPressUI.set_Value(false)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._button), (Action<M0>) (_ => isOnUI.set_Value(true)));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._button), (Action<M0>) (_ => isOnUI.set_Value(false)));
    }
  }
}
