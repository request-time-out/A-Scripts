// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIImageHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (Image))]
  public class UIImageHelper : MonoBehaviour
  {
    [SerializeField]
    private UIImageHelper.State enabledState;
    [SerializeField]
    private UIImageHelper.State disabledState;
    private bool currentState;

    public UIImageHelper()
    {
      base.\u002Ector();
    }

    public void SetEnabledState(bool newState)
    {
      this.currentState = newState;
      UIImageHelper.State state = !newState ? this.disabledState : this.enabledState;
      if (state == null)
        return;
      Image component = (Image) ((Component) this).get_gameObject().GetComponent<Image>();
      if (Object.op_Equality((Object) component, (Object) null))
        Debug.LogError((object) "Image is missing!");
      else
        state.Set(component);
    }

    public void SetEnabledStateColor(Color color)
    {
      this.enabledState.color = color;
    }

    public void SetDisabledStateColor(Color color)
    {
      this.disabledState.color = color;
    }

    public void Refresh()
    {
      UIImageHelper.State state = !this.currentState ? this.disabledState : this.enabledState;
      Image component = (Image) ((Component) this).get_gameObject().GetComponent<Image>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      state.Set(component);
    }

    [Serializable]
    private class State
    {
      [SerializeField]
      public Color color;

      public void Set(Image image)
      {
        if (Object.op_Equality((Object) image, (Object) null))
          return;
        ((Graphic) image).set_color(this.color);
      }
    }
  }
}
