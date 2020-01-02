// Decompiled with JetBrains decompiler
// Type: UI_OnMouseOverMessageEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OnMouseOverMessageEx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  [SerializeField]
  private Image imgMessage;
  [SerializeField]
  private Text textMessage;
  [SerializeField]
  private string[] strMessage;
  public int showMsgNo;

  public UI_OnMouseOverMessageEx()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) null, (Object) this.imgMessage))
      ((Behaviour) this.imgMessage).set_enabled(false);
    if (!Object.op_Inequality((Object) null, (Object) this.textMessage))
      return;
    ((Behaviour) this.textMessage).set_enabled(false);
  }

  public void OnPointerEnter(PointerEventData ped)
  {
    if (Object.op_Inequality((Object) null, (Object) this.imgMessage))
      ((Behaviour) this.imgMessage).set_enabled(true);
    if (!Object.op_Inequality((Object) null, (Object) this.textMessage))
      return;
    ((Behaviour) this.textMessage).set_enabled(true);
    if (this.strMessage == null || this.showMsgNo >= this.strMessage.Length || this.strMessage[this.showMsgNo] == null)
      return;
    this.textMessage.set_text(this.strMessage[this.showMsgNo]);
  }

  public void OnPointerExit(PointerEventData ped)
  {
    if (Object.op_Inequality((Object) null, (Object) this.imgMessage))
      ((Behaviour) this.imgMessage).set_enabled(false);
    if (!Object.op_Inequality((Object) null, (Object) this.textMessage))
      return;
    ((Behaviour) this.textMessage).set_enabled(false);
  }
}
