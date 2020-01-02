// Decompiled with JetBrains decompiler
// Type: Studio.SceneAssist.PointerDownAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Studio.SceneAssist
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
      using (List<UnityAction>.Enumerator enumerator = this.listAction.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Invoke();
      }
    }
  }
}
