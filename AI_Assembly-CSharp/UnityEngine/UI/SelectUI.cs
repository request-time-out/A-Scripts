// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.SelectUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  public class SelectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
  {
    [SerializeField]
    private bool isSelect;

    public SelectUI()
    {
      base.\u002Ector();
    }

    public bool IsSelect
    {
      get
      {
        return this.isSelect;
      }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      this.isSelect = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      this.isSelect = false;
    }

    protected virtual void OnDisable()
    {
      this.isSelect = false;
    }
  }
}
