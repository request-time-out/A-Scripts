// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ResManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{
  public class ResManager : MonoBehaviour
  {
    public Sprite[] spriteObjArray;
    private static ResManager instance;
    private string[] mWordList;
    private Dictionary<string, Sprite> spriteObjDict;

    public ResManager()
    {
      base.\u002Ector();
    }

    public static ResManager Get
    {
      get
      {
        if (Object.op_Equality((Object) ResManager.instance, (Object) null))
          ResManager.instance = (ResManager) Object.FindObjectOfType<ResManager>();
        return ResManager.instance;
      }
    }

    private void InitData()
    {
      this.spriteObjDict.Clear();
      foreach (Sprite spriteObj in this.spriteObjArray)
        this.spriteObjDict[((Object) spriteObj).get_name()] = spriteObj;
    }

    private void Awake()
    {
      ResManager.instance = (ResManager) null;
      this.InitData();
    }

    public Sprite GetSpriteByName(string spriteName)
    {
      Sprite sprite = (Sprite) null;
      return this.spriteObjDict.TryGetValue(spriteName, out sprite) ? sprite : (Sprite) null;
    }

    public string GetRandomSpriteName()
    {
      return ((Object) this.spriteObjArray[Random.Range(0, this.spriteObjArray.Length)]).get_name();
    }

    public int SpriteCount
    {
      get
      {
        return this.spriteObjArray.Length;
      }
    }

    public Sprite GetSpriteByIndex(int index)
    {
      return index < 0 || index >= this.spriteObjArray.Length ? (Sprite) null : this.spriteObjArray[index];
    }

    public string GetSpriteNameByIndex(int index)
    {
      return index < 0 || index >= this.spriteObjArray.Length ? string.Empty : ((Object) this.spriteObjArray[index]).get_name();
    }
  }
}
