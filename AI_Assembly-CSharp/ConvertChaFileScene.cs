// Decompiled with JetBrains decompiler
// Type: ConvertChaFileScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ConvertChaFileScene : BaseLoader
{
  private FolderAssist fa = new FolderAssist();
  private string dirMoveCha = string.Empty;
  private Dictionary<string, ChaFile.ProductInfo> dictConvChaTrial = new Dictionary<string, ChaFile.ProductInfo>();
  public GameObject objHide;
  public GameObject objClick;
  [Header("キャラ用 -------------------------------------------")]
  public Image imgPBFront;
  public Text txtResult;
  private int vSyncCountBack;
  private List<string> lstEtcCha;
  public CoroutineAssist caConvert;

  private void Start()
  {
    this.vSyncCountBack = QualitySettings.get_vSyncCount();
    QualitySettings.set_vSyncCount(0);
    if (Object.op_Inequality((Object) null, (Object) this.objHide))
      this.objHide.SetActive(true);
    string folder = UserData.Path + "chara/female/";
    this.lstEtcCha = new List<string>();
    this.fa.CreateFolderInfo(folder, "*.png", true, true);
    this.dictConvChaTrial = new Dictionary<string, ChaFile.ProductInfo>();
    int fileCount = this.fa.GetFileCount();
    string empty1 = string.Empty;
    for (int index = 0; index < fileCount; ++index)
    {
      string fullPath = this.fa.lstFile[index].FullPath;
      ChaFile.ProductInfo info = (ChaFile.ProductInfo) null;
      if (ChaFile.GetProductInfo(fullPath, out info) && info.productNo == 100)
      {
        if (info.version < new Version(1, 0, 0))
          this.dictConvChaTrial[fullPath] = info;
      }
      else
        this.lstEtcCha.Add(fullPath);
    }
    if (this.lstEtcCha.Count != 0)
    {
      string path = UserData.Path + "chara/old/etc/";
      if (!System.IO.Directory.Exists(path))
        System.IO.Directory.CreateDirectory(path);
      string empty2 = string.Empty;
      for (int index = 0; index < this.lstEtcCha.Count; ++index)
      {
        int num = 0;
        string str;
        while (true)
        {
          str = path + Path.GetFileNameWithoutExtension(this.lstEtcCha[index]) + (num != 0 ? "(" + num.ToString() + ").png" : ".png");
          if (File.Exists(str))
            ++num;
          else
            break;
        }
        File.Move(this.lstEtcCha[index], str);
      }
    }
    if (this.dictConvChaTrial.Count == 0)
    {
      QualitySettings.set_vSyncCount(this.vSyncCountBack);
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "Logo",
        isFade = true,
        isAsync = true
      }, false);
    }
    else
    {
      if (this.dictConvChaTrial.Count != 0)
      {
        this.dirMoveCha = UserData.Path + "chara/old/female/";
        if (!System.IO.Directory.Exists(this.dirMoveCha))
          System.IO.Directory.CreateDirectory(this.dirMoveCha);
      }
      if (Object.op_Inequality((Object) null, (Object) this.objHide))
        this.objHide.SetActive(false);
      this.caConvert = new CoroutineAssist((MonoBehaviour) this, new Func<IEnumerator>(this.Convert));
      this.caConvert.Start(false, 20f);
    }
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.objClick) || !this.objClick.get_activeSelf() || !Input.get_anyKeyDown())
      return;
    QualitySettings.set_vSyncCount(this.vSyncCountBack);
    Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
    {
      levelName = "Logo",
      isFade = true,
      isAsync = true
    }, false);
  }

  [DebuggerHidden]
  private IEnumerator Convert()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ConvertChaFileScene.\u003CConvert\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void OnDestroy()
  {
    QualitySettings.set_vSyncCount(this.vSyncCountBack);
  }
}
