// Decompiled with JetBrains decompiler
// Type: OutputAnmInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OutputAnmInfo : MonoBehaviour
{
  public int msgCnt;
  public GameObject objAnm;
  public int start;
  public int end;
  public string outputFile;
  public bool outputDebugText;
  [Header("--------<ExtraOption>---------------------------------------------------------------")]
  public bool UseInfoFlag;
  public TextAsset taUseInfo;
  private List<string> lstUseName;
  private string[] arrUseName;
  private string msg;

  public OutputAnmInfo()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (string.Empty == this.outputFile)
      return;
    if (this.UseInfoFlag)
    {
      this.LoadUseNameList();
      this.arrUseName = this.lstUseName.ToArray();
    }
    AnimationKeyInfo animationKeyInfo = new AnimationKeyInfo();
    if (animationKeyInfo.CreateInfo(this.start, this.end, this.objAnm, this.arrUseName))
    {
      string path = Application.get_dataPath() + "/_CustomShapeOutput";
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string filepath = path + "/" + this.outputFile + ".bytes";
      animationKeyInfo.SaveInfo(filepath);
      if (!this.outputDebugText)
        return;
      string outputPath = filepath.Replace(".bytes", ".txt");
      animationKeyInfo.OutputText(outputPath);
    }
    else
      this.msg = this.outputFile + " の作成に失敗";
  }

  public void LoadUseNameList()
  {
    if (Object.op_Equality((Object) null, (Object) this.taUseInfo))
      return;
    string[,] data;
    YS_Assist.GetListString(this.taUseInfo.get_text(), out data);
    this.lstUseName.Clear();
    int length = data.GetLength(0);
    for (int index = 0; index < length; ++index)
      this.lstUseName.Add(data[index, 0].Replace("\r", string.Empty).Replace("\n", string.Empty));
  }

  private void OnGUI()
  {
    GUI.set_color(Color.get_white());
    GUILayout.BeginArea(new Rect(10f, (float) (10 + this.msgCnt * 25), 400f, 20f));
    GUILayout.Label(this.msg, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndArea();
  }

  private void Update()
  {
  }
}
