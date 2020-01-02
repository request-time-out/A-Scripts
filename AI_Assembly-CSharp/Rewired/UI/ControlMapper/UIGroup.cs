// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class UIGroup : MonoBehaviour
  {
    [SerializeField]
    private Text _label;
    [SerializeField]
    private Transform _content;

    public UIGroup()
    {
      base.\u002Ector();
    }

    public string labelText
    {
      get
      {
        return Object.op_Inequality((Object) this._label, (Object) null) ? this._label.get_text() : string.Empty;
      }
      set
      {
        if (Object.op_Equality((Object) this._label, (Object) null))
          return;
        this._label.set_text(value);
      }
    }

    public Transform content
    {
      get
      {
        return this._content;
      }
    }

    public void SetLabelActive(bool state)
    {
      if (Object.op_Equality((Object) this._label, (Object) null))
        return;
      ((Component) this._label).get_gameObject().SetActive(state);
    }
  }
}
