// Decompiled with JetBrains decompiler
// Type: UI_ToggleOnOff
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class UI_ToggleOnOff : MonoBehaviour
{
  public Image[] imgOn;
  public Image[] imgOff;
  public GameObject[] objOn;
  public GameObject[] objOff;
  public CanvasGroup[] cgOn;
  public CanvasGroup[] cgOff;

  public UI_ToggleOnOff()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Toggle component = (Toggle) ((Component) this).GetComponent<Toggle>();
    if (!Object.op_Implicit((Object) component))
      return;
    this.OnChange(component.get_isOn());
  }

  public void OnChange(bool check)
  {
    if (this.imgOn != null)
    {
      foreach (Image image in this.imgOn)
      {
        if (Object.op_Inequality((Object) null, (Object) image))
          ((Behaviour) image).set_enabled(check);
      }
    }
    if (this.imgOff != null)
    {
      foreach (Image image in this.imgOff)
      {
        if (Object.op_Inequality((Object) null, (Object) image))
          ((Behaviour) image).set_enabled(!check);
      }
    }
    if (this.objOn != null)
    {
      foreach (GameObject self in this.objOn)
      {
        if (Object.op_Inequality((Object) null, (Object) self))
          self.SetActiveIfDifferent(check);
      }
    }
    if (this.objOff != null)
    {
      foreach (GameObject self in this.objOff)
      {
        if (Object.op_Inequality((Object) null, (Object) self))
          self.SetActiveIfDifferent(!check);
      }
    }
    if (this.cgOn != null)
    {
      foreach (CanvasGroup canvasGroup in this.cgOn)
      {
        if (Object.op_Inequality((Object) null, (Object) canvasGroup))
          canvasGroup.Enable(check, false);
      }
    }
    if (this.cgOff == null)
      return;
    foreach (CanvasGroup canvasGroup in this.cgOff)
    {
      if (Object.op_Inequality((Object) null, (Object) canvasGroup))
        canvasGroup.Enable(!check, false);
    }
  }
}
