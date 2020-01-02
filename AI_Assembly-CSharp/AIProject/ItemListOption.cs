// Decompiled with JetBrains decompiler
// Type: AIProject.ItemListOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject
{
  public class ItemListOption : MonoBehaviour
  {
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private Image _fillImage;
    [SerializeField]
    private StringReactiveProperty _text;
    [SerializeField]
    private TextMeshProUGUI _stackCountLabel;
    [SerializeField]
    private Button _button;

    public ItemListOption()
    {
      base.\u002Ector();
    }

    public Image IconImage
    {
      get
      {
        return this._iconImage;
      }
    }

    public StringReactiveProperty Text
    {
      get
      {
        return this._text;
      }
    }

    public bool EnabledButton
    {
      get
      {
        return ((Behaviour) this._button).get_enabled();
      }
      set
      {
        ((Behaviour) this._button).set_enabled(value);
      }
    }

    public Button.ButtonClickedEvent OnClicked
    {
      get
      {
        return this._button.get_onClick();
      }
    }

    public UnityEvent OnRemove { get; private set; }

    public bool IsInteractable
    {
      get
      {
        return ((Selectable) this._button).IsInteractable();
      }
    }

    public bool IsStackable { get; set; }

    public StuffItem Item { get; set; }

    private void Start()
    {
      ObservableExtensions.Subscribe<string>(Observable.Where<string>((IObservable<M0>) this._text, (Func<M0, bool>) (x => x != ((TMP_Text) this._stackCountLabel).get_text())), (Action<M0>) (x => ((TMP_Text) this._stackCountLabel).set_text(x)));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnUpdate()
    {
      if (this.Item == null || this.IsStackable)
        return;
      this._fillImage.set_fillAmount(1f);
    }
  }
}
