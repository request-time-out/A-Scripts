// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_InfiniteOcean
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace LuxWater
{
  public class LuxWater_InfiniteOcean : MonoBehaviour
  {
    [Space(6f)]
    [LuxWater_HelpBtn("h.c1utuz9up55r")]
    public Camera MainCam;
    public float GridSize;
    private Transform trans;
    private Transform camTrans;

    public LuxWater_InfiniteOcean()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      this.trans = (Transform) ((Component) this).GetComponent<Transform>();
    }

    private void LateUpdate()
    {
      if (Object.op_Equality((Object) this.MainCam, (Object) null))
      {
        Camera main = Camera.get_main();
        if (Object.op_Equality((Object) main, (Object) null))
          return;
        this.MainCam = main;
      }
      if (Object.op_Equality((Object) this.camTrans, (Object) null))
        this.camTrans = ((Component) this.MainCam).get_transform();
      Vector3 position1 = this.camTrans.get_position();
      Vector3 position2 = this.trans.get_position();
      Vector3 lossyScale = this.trans.get_lossyScale();
      Vector2 vector2;
      ((Vector2) ref vector2).\u002Ector(this.GridSize * (float) lossyScale.x, this.GridSize * (float) lossyScale.z);
      float num1 = (float) Math.Round((double) (position1.x / vector2.x));
      float num2 = (float) vector2.x * num1;
      float num3 = (float) Math.Round((double) (position1.z / vector2.y));
      float num4 = (float) vector2.y * num3;
      position2.x = (__Null) ((double) num2 + position2.x % vector2.x);
      position2.z = (__Null) ((double) num4 + position2.z % vector2.y);
      this.trans.set_position(position2);
    }
  }
}
