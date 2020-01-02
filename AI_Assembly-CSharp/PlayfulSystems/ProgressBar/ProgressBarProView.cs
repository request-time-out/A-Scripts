// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ProgressBar.ProgressBarProView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace PlayfulSystems.ProgressBar
{
  public abstract class ProgressBarProView : MonoBehaviour
  {
    protected ProgressBarProView()
    {
      base.\u002Ector();
    }

    public virtual void NewChangeStarted(float currentValue, float targetValue)
    {
    }

    public virtual void SetBarColor(Color color)
    {
    }

    public virtual bool CanUpdateView(float currentValue, float targetValue)
    {
      return ((Behaviour) this).get_isActiveAndEnabled();
    }

    public abstract void UpdateView(float currentValue, float targetValue);
  }
}
