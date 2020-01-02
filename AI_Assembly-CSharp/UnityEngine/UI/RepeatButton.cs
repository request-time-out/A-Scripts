// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.RepeatButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  public abstract class RepeatButton : SelectUI, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
  {
    private bool push;

    protected abstract void Process(bool push);

    public void OnPointerDown(PointerEventData eventData)
    {
      this.push = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      this.push = false;
    }

    protected virtual void Awake()
    {
      this.push = false;
    }

    private void Update()
    {
      this.Process(this.push);
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.push = false;
    }
  }
}
