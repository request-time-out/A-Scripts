// Decompiled with JetBrains decompiler
// Type: Housing.InputFieldFocuse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Housing
{
  [DisallowMultipleComponent]
  [RequireComponent(typeof (InputField))]
  public class InputFieldFocuse : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IEventSystemHandler
  {
    [SerializeField]
    private InputField inputField;

    public InputFieldFocuse()
    {
      base.\u002Ector();
    }

    public void OnDeselect(BaseEventData eventData)
    {
      if (!Singleton<CraftScene>.IsInstance())
        return;
      Singleton<CraftScene>.Instance.DeselectInputField(this.inputField);
    }

    public void OnSelect(BaseEventData eventData)
    {
      if (!Singleton<CraftScene>.IsInstance())
        return;
      Singleton<CraftScene>.Instance.SelectInputField(this.inputField);
    }

    public void OnSubmit(BaseEventData eventData)
    {
      if (!Singleton<CraftScene>.IsInstance())
        return;
      Singleton<CraftScene>.Instance.SelectInputField(this.inputField);
    }
  }
}
