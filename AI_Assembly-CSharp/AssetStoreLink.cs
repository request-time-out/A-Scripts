// Decompiled with JetBrains decompiler
// Type: AssetStoreLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class AssetStoreLink : MonoBehaviour
{
  public string assetStoreID;

  public AssetStoreLink()
  {
    base.\u002Ector();
  }

  public void VisitStore()
  {
    Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/" + this.assetStoreID);
  }
}
