﻿// Decompiled with JetBrains decompiler
// Type: UI_OnMouseOverMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OnMouseOverMessage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  public bool active;
  public Image imgComment;
  public Text txtComment;
  public string comment;

  public UI_OnMouseOverMessage()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) null, (Object) this.imgComment))
      ((Behaviour) this.imgComment).set_enabled(false);
    if (!Object.op_Inequality((Object) null, (Object) this.txtComment))
      return;
    ((Behaviour) this.txtComment).set_enabled(false);
  }

  public void OnPointerEnter(PointerEventData ped)
  {
    if (this.active)
    {
      if (Object.op_Inequality((Object) null, (Object) this.imgComment))
        ((Behaviour) this.imgComment).set_enabled(true);
      if (!Object.op_Inequality((Object) null, (Object) this.txtComment))
        return;
      ((Behaviour) this.txtComment).set_enabled(true);
      this.txtComment.set_text(this.comment);
    }
    else
    {
      if (Object.op_Inequality((Object) null, (Object) this.imgComment))
        ((Behaviour) this.imgComment).set_enabled(false);
      if (!Object.op_Inequality((Object) null, (Object) this.txtComment))
        return;
      ((Behaviour) this.txtComment).set_enabled(false);
    }
  }

  public void OnPointerExit(PointerEventData ped)
  {
    if (Object.op_Inequality((Object) null, (Object) this.imgComment))
      ((Behaviour) this.imgComment).set_enabled(false);
    if (!Object.op_Inequality((Object) null, (Object) this.txtComment))
      return;
    ((Behaviour) this.txtComment).set_enabled(false);
  }

  private void Update()
  {
  }
}
