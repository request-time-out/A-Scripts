// Decompiled with JetBrains decompiler
// Type: UI_DrawTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DrawTimer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  [SerializeField]
  private CanvasGroup cgSpace;
  private bool isOver;
  private float timeCnt;
  private float drawTime;
  private float fadeTime;

  public UI_DrawTimer()
  {
    base.\u002Ector();
  }

  public void Setup(float _drawTime, float _fadeTime)
  {
    this.drawTime = _drawTime;
    this.fadeTime = _fadeTime;
    this.timeCnt = this.drawTime;
    this.cgSpace.set_alpha(1f);
  }

  public void OnPointerEnter(PointerEventData ped)
  {
    this.isOver = true;
  }

  public void OnPointerExit(PointerEventData ped)
  {
    this.isOver = false;
  }

  private void Update()
  {
    if (this.isOver)
    {
      this.timeCnt = this.drawTime;
      this.cgSpace.set_alpha(1f);
    }
    this.timeCnt = Mathf.Max(0.0f, this.timeCnt - Time.get_deltaTime());
    if ((double) this.timeCnt >= (double) this.fadeTime)
      return;
    this.cgSpace.set_alpha(Mathf.InverseLerp(0.0f, this.fadeTime, this.timeCnt));
  }
}
