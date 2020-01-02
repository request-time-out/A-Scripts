// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomCanvasSortControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharaCustom
{
  public class CustomCanvasSortControl : MonoBehaviour
  {
    [SerializeField]
    private Canvas[] sortCanvas;

    public CustomCanvasSortControl()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (this.sortCanvas == null || this.sortCanvas.Length == 0)
        return;
      for (int index = 0; index < this.sortCanvas.Length; ++index)
        this.sortCanvas[index].set_sortingOrder(index + 1);
    }

    public void SortCanvas(Canvas cvs)
    {
      if (1 >= this.sortCanvas.Length || Object.op_Equality((Object) cvs, (Object) ((IEnumerable<Canvas>) this.sortCanvas).Last<Canvas>()))
        return;
      Canvas[] canvasArray = new Canvas[this.sortCanvas.Length];
      int num = 0;
      for (int index = 0; index < this.sortCanvas.Length; ++index)
      {
        if (!Object.op_Equality((Object) cvs, (Object) this.sortCanvas[index]))
          canvasArray[num++] = this.sortCanvas[index];
      }
      canvasArray[canvasArray.Length - 1] = cvs;
      this.sortCanvas = canvasArray;
      if (this.sortCanvas == null || this.sortCanvas.Length == 0)
        return;
      for (int index = 0; index < this.sortCanvas.Length; ++index)
        this.sortCanvas[index].set_sortingOrder(index + 1);
    }
  }
}
