﻿// Decompiled with JetBrains decompiler
// Type: SceneAssist.PointerDownAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SceneAssist
{
  public class PointerDownAction : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
  {
    public List<UnityAction> listAction;

    public PointerDownAction()
    {
      base.\u002Ector();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      for (int index = 0; index < this.listAction.Count; ++index)
        this.listAction[index].Invoke();
    }
  }
}
