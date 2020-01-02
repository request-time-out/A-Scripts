// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PlantIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class PlantIcon : MonoBehaviour
  {
    [SerializeField]
    private Toggle _toggle;
    [SerializeField]
    private Image _icon;
    private GameObject _visibleObject;

    public PlantIcon()
    {
      base.\u002Ector();
    }

    public bool initialized { get; private set; }

    public bool visible
    {
      get
      {
        return this.visibleObject.get_activeSelf();
      }
      set
      {
        this.visibleObject.SetActive(value);
      }
    }

    public Toggle toggle
    {
      get
      {
        return this._toggle;
      }
    }

    public string itemName
    {
      get
      {
        return this._itemInfo.get_Value()?.Name ?? string.Empty;
      }
    }

    public Sprite itemIcon
    {
      get
      {
        return this._icon.get_sprite();
      }
    }

    public StuffItemInfo itemInfo
    {
      get
      {
        return this._itemInfo.get_Value();
      }
    }

    private ReactiveProperty<StuffItemInfo> _itemInfo { get; }

    public AIProject.SaveData.Environment.PlantInfo info
    {
      get
      {
        return this._info.get_Value();
      }
      set
      {
        this._info.set_Value(value);
      }
    }

    private ReactiveProperty<AIProject.SaveData.Environment.PlantInfo> _info { get; }

    private GameObject visibleObject
    {
      get
      {
        return ((object) this).GetCacheObject<GameObject>(ref this._visibleObject, (Func<GameObject>) (() => ((Component) ((Component) this).get_transform().GetChild(0)).get_gameObject()));
      }
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PlantIcon.\u003CStart\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
