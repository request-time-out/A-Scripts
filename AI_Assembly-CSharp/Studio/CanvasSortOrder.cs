// Decompiled with JetBrains decompiler
// Type: Studio.CanvasSortOrder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class CanvasSortOrder : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
  {
    [SerializeField]
    private Canvas m_Canvas;

    public CanvasSortOrder()
    {
      base.\u002Ector();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      SortCanvas.select = this.m_Canvas;
    }
  }
}
