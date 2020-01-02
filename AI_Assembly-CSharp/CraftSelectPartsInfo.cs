// Decompiled with JetBrains decompiler
// Type: CraftSelectPartsInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CraftSelectPartsInfo : MonoBehaviour
{
  [SerializeField]
  private Image Item;
  private Text ItemName;
  private StringReactiveProperty _szItemName;
  private string szPrevItemName;

  public CraftSelectPartsInfo()
  {
    base.\u002Ector();
  }

  public string szItemName
  {
    get
    {
      return ((ReactiveProperty<string>) this._szItemName).get_Value();
    }
    set
    {
      ((ReactiveProperty<string>) this._szItemName).set_Value(value);
    }
  }

  private void Start()
  {
    this.ItemName = this.Item != null ? (Text) ((Component) this.Item).GetComponentInChildren<Text>() : (Text) null;
    this.szPrevItemName = string.Empty;
    ObservableExtensions.Subscribe<string>(Observable.Where<string>((IObservable<M0>) this._szItemName, (Func<M0, bool>) (x => x != this.szPrevItemName)), (Action<M0>) (x => this.ChangItemIcon(x)));
  }

  private void ChangItemIcon(string name)
  {
    this.szPrevItemName = name;
    this.ItemName.set_text(name);
  }
}
