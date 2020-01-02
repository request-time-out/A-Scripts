// Decompiled with JetBrains decompiler
// Type: Studio.StudioInputField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  [AddComponentMenu("Studio/GUI/Input Field", 1000)]
  public class StudioInputField : InputField
  {
    public StudioInputField()
    {
      base.\u002Ector();
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
      base.OnSelect(eventData);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
      base.OnDeselect(eventData);
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
      base.OnSubmit(eventData);
    }

    protected virtual void Start()
    {
      ((UIBehaviour) this).Start();
    }
  }
}
