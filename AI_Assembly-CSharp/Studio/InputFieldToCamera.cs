// Decompiled with JetBrains decompiler
// Type: Studio.InputFieldToCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class InputFieldToCamera : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IEventSystemHandler
  {
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private TMP_InputField inputFieldTMP;
    [SerializeField]
    private Canvas m_Canvas;

    public InputFieldToCamera()
    {
      base.\u002Ector();
    }

    private Canvas canvas
    {
      get
      {
        return this.m_Canvas ?? (this.m_Canvas = (Canvas) ((Component) this).GetComponentInParent<Canvas>());
      }
    }

    public void OnDeselect(BaseEventData eventData)
    {
      Singleton<Studio.Studio>.Instance.DeselectInputField(this.inputField, this.inputFieldTMP);
    }

    public void OnSelect(BaseEventData eventData)
    {
      Singleton<Studio.Studio>.Instance.SelectInputField(this.inputField, this.inputFieldTMP);
      SortCanvas.select = this.canvas;
    }

    public void OnSubmit(BaseEventData eventData)
    {
      Singleton<Studio.Studio>.Instance.SelectInputField(this.inputField, this.inputFieldTMP);
    }
  }
}
