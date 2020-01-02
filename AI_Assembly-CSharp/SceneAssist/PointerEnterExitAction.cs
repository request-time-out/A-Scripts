﻿// Decompiled with JetBrains decompiler
// Type: SceneAssist.PointerEnterExitAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SceneAssist
{
  public class PointerEnterExitAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
  {
    public List<UnityAction> listActionEnter;
    public List<UnityAction> listActionExit;

    public PointerEnterExitAction()
    {
      base.\u002Ector();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      for (int index = 0; index < this.listActionEnter.Count; ++index)
        this.listActionEnter[index].Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      for (int index = 0; index < this.listActionExit.Count; ++index)
        this.listActionExit[index].Invoke();
    }
  }
}
