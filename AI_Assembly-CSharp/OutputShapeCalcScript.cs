// Decompiled with JetBrains decompiler
// Type: OutputShapeCalcScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class OutputShapeCalcScript : MonoBehaviour
{
  public TextAsset text;
  private Dictionary<string, OutputShapeCalcScript.Info> dictBone;
  private List<string> lstSrc;

  public OutputShapeCalcScript()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.dictBone.Clear();
    if (!Object.op_Inequality((Object) null, (Object) this.text))
      return;
    string[,] data = (string[,]) null;
    YS_Assist.GetListString(this.text.get_text(), out data);
    OutputShapeCalcScript.Info info = (OutputShapeCalcScript.Info) null;
    int length1 = data.GetLength(0);
    int length2 = data.GetLength(1);
    if (length1 != 0 && length2 != 0)
    {
      for (int index = 0; index < length1; ++index)
      {
        if (!this.dictBone.TryGetValue(data[index, 0], out info))
        {
          info = new OutputShapeCalcScript.Info();
          this.dictBone[data[index, 0]] = info;
        }
        if ("v" == data[index, 2])
          info.lstPosX.Add(data[index, 1]);
        if ("v" == data[index, 3])
          info.lstPosY.Add(data[index, 1]);
        if ("v" == data[index, 4])
          info.lstPosZ.Add(data[index, 1]);
        if ("v" == data[index, 5])
          info.lstRotX.Add(data[index, 1]);
        if ("v" == data[index, 6])
          info.lstRotY.Add(data[index, 1]);
        if ("v" == data[index, 7])
          info.lstRotZ.Add(data[index, 1]);
        if ("v" == data[index, 8])
          info.lstSclX.Add(data[index, 1]);
        if ("v" == data[index, 9])
          info.lstSclY.Add(data[index, 1]);
        if ("v" == data[index, 10])
          info.lstSclZ.Add(data[index, 1]);
        if (!this.lstSrc.Contains(data[index, 1]))
          this.lstSrc.Add(data[index, 1]);
      }
    }
    this.OutputText(Application.get_dataPath() + "/shapecalc.txt");
  }

  public void OutputText(string outputPath)
  {
    StringBuilder stringBuilder = new StringBuilder(2048);
    stringBuilder.Length = 0;
    stringBuilder.Append("=== 計算式 ===================================================================\n");
    foreach (KeyValuePair<string, OutputShapeCalcScript.Info> keyValuePair in this.dictBone)
    {
      if (keyValuePair.Value.lstPosX.Count != 0)
      {
        stringBuilder.Append("dictDstBoneInfo[(int)DstBoneName.").Append(keyValuePair.Key).Append("].trfBone.SetLocalPositionX(");
        for (int index = 0; index < keyValuePair.Value.lstPosX.Count; ++index)
        {
          stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstPosX[index]).Append("].vctPos.x");
          if (index + 1 < keyValuePair.Value.lstPosX.Count)
            stringBuilder.Append(" + ");
          else
            stringBuilder.Append(");\n");
        }
      }
      if (keyValuePair.Value.lstPosY.Count != 0)
      {
        stringBuilder.Append("dictDstBoneInfo[(int)DstBoneName.").Append(keyValuePair.Key).Append("].trfBone.SetLocalPositionY(");
        for (int index = 0; index < keyValuePair.Value.lstPosY.Count; ++index)
        {
          stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstPosY[index]).Append("].vctPos.y");
          if (index + 1 < keyValuePair.Value.lstPosY.Count)
            stringBuilder.Append(" + ");
          else
            stringBuilder.Append(");\n");
        }
      }
      if (keyValuePair.Value.lstPosZ.Count != 0)
      {
        stringBuilder.Append("dictDstBoneInfo[(int)DstBoneName.").Append(keyValuePair.Key).Append("].trfBone.SetLocalPositionZ(");
        for (int index = 0; index < keyValuePair.Value.lstPosZ.Count; ++index)
        {
          stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstPosZ[index]).Append("].vctPos.z");
          if (index + 1 < keyValuePair.Value.lstPosZ.Count)
            stringBuilder.Append(" + ");
          else
            stringBuilder.Append(");\n");
        }
      }
      if (keyValuePair.Value.lstRotX.Count != 0 || keyValuePair.Value.lstRotY.Count != 0 || keyValuePair.Value.lstRotZ.Count != 0)
      {
        stringBuilder.Append("dictDstBoneInfo[(int)DstBoneName.").Append(keyValuePair.Key).Append("].trfBone.SetLocalRotation(\n");
        stringBuilder.Append("\t");
        if (keyValuePair.Value.lstRotX.Count != 0)
        {
          for (int index = 0; index < keyValuePair.Value.lstRotX.Count; ++index)
          {
            stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstRotX[index]).Append("].vctRot.x");
            if (index + 1 < keyValuePair.Value.lstRotX.Count)
              stringBuilder.Append(" + ");
            else
              stringBuilder.Append(",\n");
          }
        }
        else
          stringBuilder.Append("0.0f,\n");
        stringBuilder.Append("\t");
        if (keyValuePair.Value.lstRotY.Count != 0)
        {
          for (int index = 0; index < keyValuePair.Value.lstRotY.Count; ++index)
          {
            stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstRotY[index]).Append("].vctRot.y");
            if (index + 1 < keyValuePair.Value.lstRotY.Count)
              stringBuilder.Append(" + ");
            else
              stringBuilder.Append(",\n");
          }
        }
        else
          stringBuilder.Append("0.0f,\n");
        stringBuilder.Append("\t");
        if (keyValuePair.Value.lstRotZ.Count != 0)
        {
          for (int index = 0; index < keyValuePair.Value.lstRotZ.Count; ++index)
          {
            stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstRotZ[index]).Append("].vctRot.z");
            if (index + 1 < keyValuePair.Value.lstRotZ.Count)
              stringBuilder.Append(" + ");
            else
              stringBuilder.Append(");\n");
          }
        }
        else
          stringBuilder.Append("0.0f);\n");
      }
      if (keyValuePair.Value.lstSclX.Count != 0 || keyValuePair.Value.lstSclY.Count != 0 || keyValuePair.Value.lstSclZ.Count != 0)
      {
        stringBuilder.Append("dictDstBoneInfo[(int)DstBoneName.").Append(keyValuePair.Key).Append("].trfBone.SetLocalScale(\n");
        stringBuilder.Append("\t");
        if (keyValuePair.Value.lstSclX.Count != 0)
        {
          for (int index = 0; index < keyValuePair.Value.lstSclX.Count; ++index)
          {
            stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstSclX[index]).Append("].vctScl.x");
            if (index + 1 < keyValuePair.Value.lstSclX.Count)
              stringBuilder.Append(" + ");
            else
              stringBuilder.Append(",\n");
          }
        }
        else
          stringBuilder.Append("1.0f,\n");
        stringBuilder.Append("\t");
        if (keyValuePair.Value.lstSclY.Count != 0)
        {
          for (int index = 0; index < keyValuePair.Value.lstSclY.Count; ++index)
          {
            stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstSclY[index]).Append("].vctScl.y");
            if (index + 1 < keyValuePair.Value.lstSclY.Count)
              stringBuilder.Append(" + ");
            else
              stringBuilder.Append(",\n");
          }
        }
        else
          stringBuilder.Append("1.0f,\n");
        stringBuilder.Append("\t");
        if (keyValuePair.Value.lstSclZ.Count != 0)
        {
          for (int index = 0; index < keyValuePair.Value.lstSclZ.Count; ++index)
          {
            stringBuilder.Append("dictSrcBoneInfo[(int)SrcBoneName.").Append(keyValuePair.Value.lstSclZ[index]).Append("].vctScl.z");
            if (index + 1 < keyValuePair.Value.lstSclZ.Count)
              stringBuilder.Append(" + ");
            else
              stringBuilder.Append(");\n");
          }
        }
        else
          stringBuilder.Append("1.0f);\n");
      }
      stringBuilder.Append("\n");
    }
    for (int index = 0; index < 10; ++index)
      stringBuilder.Append("\n");
    stringBuilder.Append("=== 転送先 ===================================================================\n");
    stringBuilder.Append("public enum DstBoneName\n").Append("{\n");
    int num1 = 0;
    foreach (KeyValuePair<string, OutputShapeCalcScript.Info> keyValuePair in this.dictBone)
    {
      stringBuilder.Append("\t").Append(keyValuePair.Key).Append(",");
      ++num1;
      if (num1 == 4)
      {
        stringBuilder.Append("\n");
        num1 = 0;
      }
      else
        stringBuilder.Append("\t\t\t");
    }
    stringBuilder.Append("\n};");
    for (int index = 0; index < 10; ++index)
      stringBuilder.Append("\n");
    stringBuilder.Append("=== 転送元 ===================================================================\n");
    stringBuilder.Append("public enum SrcBoneName\n").Append("{\n");
    int num2 = 0;
    for (int index = 0; index < this.lstSrc.Count; ++index)
    {
      stringBuilder.Append("\t").Append(this.lstSrc[index]).Append(",");
      ++num2;
      if (num2 == 4)
      {
        stringBuilder.Append("\n");
        num2 = 0;
      }
      else
        stringBuilder.Append("\t\t\t");
    }
    stringBuilder.Append("\n};");
    using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream, Encoding.UTF8))
      {
        streamWriter.Write(stringBuilder.ToString());
        streamWriter.Write("\n");
      }
    }
  }

  private void OnGUI()
  {
    GUI.set_color(Color.get_white());
    GUILayout.BeginArea(new Rect(10f, 10f, 400f, 20f));
    GUILayout.Label("シェイプ計算スクリプト補助データ作成終了", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndArea();
  }

  private void Update()
  {
  }

  public class Info
  {
    public List<string> lstPosX = new List<string>();
    public List<string> lstPosY = new List<string>();
    public List<string> lstPosZ = new List<string>();
    public List<string> lstRotX = new List<string>();
    public List<string> lstRotY = new List<string>();
    public List<string> lstRotZ = new List<string>();
    public List<string> lstSclX = new List<string>();
    public List<string> lstSclY = new List<string>();
    public List<string> lstSclZ = new List<string>();
  }
}
