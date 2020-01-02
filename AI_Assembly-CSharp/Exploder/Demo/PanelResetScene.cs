// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.PanelResetScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder.Demo
{
  public class PanelResetScene : UseObject
  {
    private List<GameObject> objectList;

    private void Start()
    {
      this.objectList = new List<GameObject>((IEnumerable<GameObject>) GameObject.FindGameObjectsWithTag("Exploder"));
    }

    public override void Use()
    {
      base.Use();
      ExploderUtils.ClearLog();
      using (List<GameObject>.Enumerator enumerator = this.objectList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          ExploderUtils.SetActiveRecursively(current, true);
          ExploderUtils.SetVisible(current, true);
        }
      }
    }

    private void Update()
    {
      if (!Input.GetKeyDown((KeyCode) 114))
        return;
      this.Use();
    }
  }
}
