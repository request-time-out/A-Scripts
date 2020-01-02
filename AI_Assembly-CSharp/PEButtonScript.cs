// Decompiled with JetBrains decompiler
// Type: PEButtonScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PEButtonScript : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
  private Button myButton;
  public ButtonTypes ButtonType;

  public PEButtonScript()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.myButton = (Button) ((Component) this).get_gameObject().GetComponent<Button>();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    UICanvasManager.GlobalAccess.MouseOverButton = true;
    UICanvasManager.GlobalAccess.UpdateToolTip(this.ButtonType);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    UICanvasManager.GlobalAccess.MouseOverButton = false;
    UICanvasManager.GlobalAccess.ClearToolTip();
  }

  public void OnButtonClicked()
  {
    UICanvasManager.GlobalAccess.UIButtonClick(this.ButtonType);
  }
}
