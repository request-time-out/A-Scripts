// Decompiled with JetBrains decompiler
// Type: SaveFrameAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SaveFrameAssist : MonoBehaviour
{
  private FolderAssist dirFrameBack;
  private FolderAssist dirFrameFront;
  private string lastFrameBackName;
  private string lastFrameFrontName;
  [SerializeField]
  private GameObject objSaveFrameTop;
  [SerializeField]
  private GameObject objSaveBack;
  [SerializeField]
  private GameObject objSaveFront;
  [SerializeField]
  private Renderer rendBack;
  [SerializeField]
  private RawImage riFront;
  public Camera backFrameCam;
  public Camera frontFrameCam;
  public BoolReactiveProperty _forceBackFrameHide;
  public BoolReactiveProperty _backFrameDraw;
  public BoolReactiveProperty _frontFrameDraw;

  public SaveFrameAssist()
  {
    base.\u002Ector();
  }

  public bool forceBackFrameHide
  {
    get
    {
      return ((ReactiveProperty<bool>) this._forceBackFrameHide).get_Value();
    }
    set
    {
      ((ReactiveProperty<bool>) this._forceBackFrameHide).set_Value(value);
    }
  }

  public bool backFrameDraw
  {
    get
    {
      return ((ReactiveProperty<bool>) this._backFrameDraw).get_Value();
    }
    set
    {
      ((ReactiveProperty<bool>) this._backFrameDraw).set_Value(value);
    }
  }

  public bool frontFrameDraw
  {
    get
    {
      return ((ReactiveProperty<bool>) this._frontFrameDraw).get_Value();
    }
    set
    {
      ((ReactiveProperty<bool>) this._frontFrameDraw).set_Value(value);
    }
  }

  public void ForgetLastName()
  {
    this.lastFrameBackName = string.Empty;
    this.lastFrameFrontName = string.Empty;
  }

  public bool Initialize()
  {
    this.ChangeSaveFrameBack((byte) 1, true);
    this.ChangeSaveFrameFront((byte) 1, true);
    return true;
  }

  public bool ChangeSaveFrameBack(byte changeNo, bool listUpdate = true)
  {
    if (listUpdate)
    {
      string str = "cardframe/Back";
      string[] searchPattern = new string[1]{ "*.png" };
      this.dirFrameBack.lstFile.Clear();
      this.dirFrameBack.CreateFolderInfoEx(DefaultData.Path + str, searchPattern, true);
      this.dirFrameBack.CreateFolderInfoEx(UserData.Path + str, searchPattern, false);
      this.dirFrameBack.SortName(true);
    }
    int fileCount = this.dirFrameBack.GetFileCount();
    if (fileCount == 0)
      return false;
    int index = this.dirFrameBack.GetIndexFromFileName(this.lastFrameBackName);
    if (index == -1)
    {
      index = 0;
    }
    else
    {
      switch (changeNo)
      {
        case 0:
          index = (index + 1) % fileCount;
          break;
        case 1:
          index = (index + fileCount - 1) % fileCount;
          break;
      }
    }
    Texture texture1 = (Texture) PngAssist.LoadTexture(this.dirFrameBack.lstFile[index].FullPath);
    if (Object.op_Implicit((Object) this.rendBack) && Object.op_Implicit((Object) this.rendBack.get_material()))
    {
      Texture texture2 = this.rendBack.get_material().GetTexture(ChaShader.MainTex);
      if (Object.op_Implicit((Object) texture2))
        Object.Destroy((Object) texture2);
      this.rendBack.get_material().SetTexture(ChaShader.MainTex, texture1);
    }
    this.lastFrameBackName = this.dirFrameBack.lstFile[index].FileName;
    return true;
  }

  public bool ChangeSaveFrameFront(byte changeNo, bool listUpdate = true)
  {
    if (listUpdate)
    {
      string str = "cardframe/Front";
      string[] searchPattern = new string[1]{ "*.png" };
      this.dirFrameFront.lstFile.Clear();
      this.dirFrameFront.CreateFolderInfoEx(DefaultData.Path + str, searchPattern, true);
      this.dirFrameFront.CreateFolderInfoEx(UserData.Path + str, searchPattern, false);
      this.dirFrameFront.SortName(true);
    }
    int fileCount = this.dirFrameFront.GetFileCount();
    if (fileCount == 0)
      return false;
    int index = this.dirFrameFront.GetIndexFromFileName(this.lastFrameFrontName);
    if (index == -1)
    {
      index = 0;
    }
    else
    {
      switch (changeNo)
      {
        case 0:
          index = (index + 1) % fileCount;
          break;
        case 1:
          index = (index + fileCount - 1) % fileCount;
          break;
      }
    }
    Texture texture = (Texture) PngAssist.LoadTexture(this.dirFrameFront.lstFile[index].FullPath);
    if (Object.op_Implicit((Object) this.riFront.get_texture()))
      Object.Destroy((Object) this.riFront.get_texture());
    this.riFront.set_texture(texture);
    this.lastFrameFrontName = this.dirFrameFront.lstFile[index].FileName;
    return true;
  }

  public string GetNowPositionStringBack()
  {
    int fileCount = this.dirFrameBack.GetFileCount();
    return fileCount == 0 ? "ファイルがありません" : string.Format("{0:000} / {1:000}", (object) (this.dirFrameBack.GetIndexFromFileName(this.lastFrameBackName) + 1), (object) fileCount);
  }

  public string GetNowPositionStringFront()
  {
    int fileCount = this.dirFrameFront.GetFileCount();
    return fileCount == 0 ? "ファイルがありません" : string.Format("{0:000} / {1:000}", (object) (this.dirFrameFront.GetIndexFromFileName(this.lastFrameFrontName) + 1), (object) fileCount);
  }

  public bool SetActiveSaveFrameTop(bool active)
  {
    if (Object.op_Equality((Object) null, (Object) this.objSaveFrameTop))
      return false;
    this.objSaveFrameTop.SetActiveIfDifferent(active);
    return true;
  }

  public bool ChangeSaveFrameTexture(int bf, Texture tex)
  {
    if (Object.op_Equality((Object) null, (Object) this.objSaveFrameTop))
      return false;
    if (bf == 0)
    {
      if (Object.op_Implicit((Object) this.rendBack) && Object.op_Implicit((Object) this.rendBack.get_material()))
      {
        Texture texture = this.rendBack.get_material().GetTexture(ChaShader.MainTex);
        if (Object.op_Implicit((Object) texture))
          Object.Destroy((Object) texture);
        this.rendBack.get_material().SetTexture(ChaShader.MainTex, tex);
      }
    }
    else
    {
      if (Object.op_Implicit((Object) this.riFront.get_texture()))
        Object.Destroy((Object) this.riFront.get_texture());
      this.riFront.set_texture(tex);
    }
    return true;
  }

  private void Start()
  {
    ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._forceBackFrameHide, (Action<M0>) (hide =>
    {
      if (hide)
      {
        if (Object.op_Inequality((Object) null, (Object) this.objSaveBack))
          this.objSaveBack.SetActiveIfDifferent(false);
        ((Behaviour) this.backFrameCam).set_enabled(false);
      }
      else
      {
        if (Object.op_Inequality((Object) null, (Object) this.objSaveBack))
          this.objSaveBack.SetActiveIfDifferent(this.backFrameDraw);
        ((Behaviour) this.backFrameCam).set_enabled(this.backFrameDraw);
      }
    }));
    ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._backFrameDraw, (Action<M0>) (visible =>
    {
      bool active = !this.forceBackFrameHide && visible;
      if (Object.op_Inequality((Object) null, (Object) this.objSaveBack))
        this.objSaveBack.SetActiveIfDifferent(active);
      ((Behaviour) this.backFrameCam).set_enabled(active);
    }));
    ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._frontFrameDraw, (Action<M0>) (visible =>
    {
      if (Object.op_Inequality((Object) null, (Object) this.objSaveFront))
        this.objSaveFront.SetActiveIfDifferent(visible);
      ((Behaviour) this.frontFrameCam).set_enabled(visible);
    }));
  }
}
