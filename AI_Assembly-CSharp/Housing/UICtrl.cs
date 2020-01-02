// Decompiled with JetBrains decompiler
// Type: Housing.UICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Housing
{
  public class UICtrl : MonoBehaviour
  {
    [SerializeField]
    private SystemUICtrl systemUICtrl;
    [SerializeField]
    private ListUICtrl listUICtrl;
    [SerializeField]
    private AddUICtrl addUICtrl;
    [SerializeField]
    private InfoUICtrl infoUICtrl;
    [SerializeField]
    private SettingUICtrl settingUICtrl;
    [SerializeField]
    private SaveLoadUICtrl saveLoadUICtrl;
    [SerializeField]
    private ManipulateUICtrl manipulateUICtrl;
    private UIDerived[] deriveds;

    public UICtrl()
    {
      base.\u002Ector();
    }

    public SystemUICtrl SystemUICtrl
    {
      get
      {
        return this.systemUICtrl;
      }
    }

    public ListUICtrl ListUICtrl
    {
      get
      {
        return this.listUICtrl;
      }
    }

    public AddUICtrl AddUICtrl
    {
      get
      {
        return this.addUICtrl;
      }
    }

    public InfoUICtrl InfoUICtrl
    {
      get
      {
        return this.infoUICtrl;
      }
    }

    public SettingUICtrl SettingUICtrl
    {
      get
      {
        return this.settingUICtrl;
      }
    }

    public SaveLoadUICtrl SaveLoadUICtrl
    {
      get
      {
        return this.saveLoadUICtrl;
      }
    }

    public ManipulateUICtrl ManipulateUICtrl
    {
      get
      {
        return this.manipulateUICtrl;
      }
    }

    public bool IsInit { get; private set; }

    public CraftCamera CraftCamera { get; private set; }

    [DebuggerHidden]
    public IEnumerator Init(CraftCamera _craftCamera, bool _tutorial)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new UICtrl.\u003CInit\u003Ec__Iterator0()
      {
        _craftCamera = _craftCamera,
        _tutorial = _tutorial,
        \u0024this = this
      };
    }
  }
}
