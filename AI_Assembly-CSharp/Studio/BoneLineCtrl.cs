// Decompiled with JetBrains decompiler
// Type: Studio.BoneLineCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class BoneLineCtrl : MonoBehaviour
  {
    [SerializeField]
    private Material material;

    public BoneLineCtrl()
    {
      base.\u002Ector();
    }

    private void Draw(OCIChar _oCIChar)
    {
      if (!_oCIChar.charInfo.visibleAll || !_oCIChar.oiCharInfo.enableFK)
        return;
      List<OCIChar.BoneInfo> listBones = _oCIChar.listBones;
      if (_oCIChar.oiCharInfo.activeFK[0])
      {
        this.Draw(listBones, 100, 3, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 104, 3, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 108, 3, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 112, 3, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 116, 2, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 120, 6, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, (int) sbyte.MaxValue, 6, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 134, 10, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 145, 6, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 152, 6, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 159, 6, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 166, 2, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 169, 2, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 172, 4, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 177, 3, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 181, 3, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 185, 1, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 187, 1, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 189, 7, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 197, 7, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 205, 6, Studio.Studio.optionSystem.colorFKHair);
        this.Draw(listBones, 212, 6, Studio.Studio.optionSystem.colorFKHair);
      }
      if (_oCIChar.oiCharInfo.activeFK[1])
        this.Draw(listBones, 1, 1, Studio.Studio.optionSystem.colorFKNeck);
      if (_oCIChar.oiCharInfo.activeFK[2])
      {
        this.Draw(listBones, 53, 4, Studio.Studio.optionSystem.colorFKBreast);
        this.Draw(listBones, 59, 4, Studio.Studio.optionSystem.colorFKBreast);
      }
      if (_oCIChar.oiCharInfo.activeFK[3])
      {
        this.Draw(listBones, 3, 2, Studio.Studio.optionSystem.colorFKBody);
        this.DrawLine(listBones, 5, 6, Studio.Studio.optionSystem.colorFKBody);
        this.Draw(listBones, 6, 3, Studio.Studio.optionSystem.colorFKBody);
        this.DrawLine(listBones, 5, 10, Studio.Studio.optionSystem.colorFKBody);
        this.Draw(listBones, 10, 3, Studio.Studio.optionSystem.colorFKBody);
        this.DrawLine(listBones, 3, 14, Studio.Studio.optionSystem.colorFKBody);
        this.Draw(listBones, 14, 3, Studio.Studio.optionSystem.colorFKBody);
        this.DrawLine(listBones, 3, 18, Studio.Studio.optionSystem.colorFKBody);
        this.Draw(listBones, 18, 3, Studio.Studio.optionSystem.colorFKBody);
        this.DrawLine(listBones, 65, 66, Studio.Studio.optionSystem.colorFKBody);
      }
      if (_oCIChar.oiCharInfo.activeFK[4])
      {
        this.Draw(listBones, 22, 2, Studio.Studio.optionSystem.colorFKRightHand);
        this.Draw(listBones, 25, 2, Studio.Studio.optionSystem.colorFKRightHand);
        this.Draw(listBones, 28, 2, Studio.Studio.optionSystem.colorFKRightHand);
        this.Draw(listBones, 31, 2, Studio.Studio.optionSystem.colorFKRightHand);
        this.Draw(listBones, 34, 2, Studio.Studio.optionSystem.colorFKRightHand);
      }
      if (_oCIChar.oiCharInfo.activeFK[5])
      {
        this.Draw(listBones, 37, 2, Studio.Studio.optionSystem.colorFKLeftHand);
        this.Draw(listBones, 40, 2, Studio.Studio.optionSystem.colorFKLeftHand);
        this.Draw(listBones, 43, 2, Studio.Studio.optionSystem.colorFKLeftHand);
        this.Draw(listBones, 46, 2, Studio.Studio.optionSystem.colorFKLeftHand);
        this.Draw(listBones, 49, 2, Studio.Studio.optionSystem.colorFKLeftHand);
      }
      if (!_oCIChar.oiCharInfo.activeFK[6])
        return;
      this.Draw(listBones, 219, 5, Studio.Studio.optionSystem.colorFKSkirt);
      this.Draw(listBones, 225, 5, Studio.Studio.optionSystem.colorFKSkirt);
      this.Draw(listBones, 231, 5, Studio.Studio.optionSystem.colorFKSkirt);
      this.Draw(listBones, 237, 5, Studio.Studio.optionSystem.colorFKSkirt);
      this.Draw(listBones, 243, 5, Studio.Studio.optionSystem.colorFKSkirt);
      this.Draw(listBones, 249, 5, Studio.Studio.optionSystem.colorFKSkirt);
      this.Draw(listBones, (int) byte.MaxValue, 5, Studio.Studio.optionSystem.colorFKSkirt);
      this.Draw(listBones, 261, 5, Studio.Studio.optionSystem.colorFKSkirt);
    }

    private void Draw(List<OCIChar.BoneInfo> _bones, int _start, int _num, Color _color)
    {
      for (int index = 0; index < _num; ++index)
        this.DrawLine(_bones, _start + index, _start + index + 1, _color);
    }

    private void DrawLine(List<OCIChar.BoneInfo> _bones, int _start, int _end, Color _color)
    {
      OCIChar.BoneInfo boneInfo1 = _bones.Find((Predicate<OCIChar.BoneInfo>) (v => v.boneID == _start));
      if (boneInfo1 == null)
        return;
      OCIChar.BoneInfo boneInfo2 = _bones.Find((Predicate<OCIChar.BoneInfo>) (v => v.boneID == _end));
      if (boneInfo2 == null)
        return;
      this.DrawLine(boneInfo1.posision, boneInfo2.posision, _color);
    }

    private void DrawLine(Vector3 _s, Vector3 _e, Color _color)
    {
      GL.Color(_color);
      GL.Vertex(_s);
      GL.Vertex(_e);
    }

    private void OnPostRender()
    {
      if (Studio.Studio.optionSystem == null || !Studio.Studio.optionSystem.lineFK)
        return;
      IEnumerable<OCIChar> source = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 0)).Select<ObjectCtrlInfo, OCIChar>((Func<ObjectCtrlInfo, OCIChar>) (v => v as OCIChar));
      if (source == null || source.Count<OCIChar>() == 0)
        return;
      this.material.SetPass(0);
      GL.PushMatrix();
      GL.Begin(1);
      foreach (OCIChar _oCIChar in source)
        this.Draw(_oCIChar);
      GL.End();
      GL.PopMatrix();
    }
  }
}
