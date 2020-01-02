// Decompiled with JetBrains decompiler
// Type: MeshBrush.FavouriteTemplatesUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace MeshBrush
{
  public static class FavouriteTemplatesUtility
  {
    public static XDocument SaveFavouriteTemplates(
      List<string> favouriteTemplates,
      string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
        throw new ArgumentNullException(nameof (filePath), "MeshBrush: the specified file path is null or empty (and thus invalid). Couldn't save favourite templates list...");
      if (favouriteTemplates == null)
        throw new ArgumentNullException(nameof (favouriteTemplates), "MeshBrush: The passed list of favourite templates is null. Cancelling saving operation...");
      for (int index = favouriteTemplates.Count - 1; index >= 0; --index)
      {
        if (!File.Exists(favouriteTemplates[index]))
          favouriteTemplates.RemoveAt(index);
      }
      XDocument xdocument = new XDocument(new object[1]
      {
        (object) new XElement((XName) "favouriteMeshBrushTemplates", (object) favouriteTemplates.Select<string, XElement>((Func<string, XElement>) (template => new XElement((XName) nameof (template), (object) new XElement((XName) "path", (object) template)))))
      });
      xdocument.Save(filePath);
      return xdocument;
    }

    public static List<string> LoadFavouriteTemplates(string filePath)
    {
      if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        throw new ArgumentException("MeshBrush: the specified file path is invalid or doesn't exist! Can't load favourite templates list...", nameof (filePath));
      return new List<string>(XDocument.Load(filePath).Descendants((XName) "path").Select<XElement, string>((Func<XElement, string>) (path => path.Value)));
    }

    public static bool LoadFavouriteTemplates(string filePath, List<string> targetList)
    {
      if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        throw new ArgumentException("MeshBrush: the specified file path is invalid or doesn't exist! Can't load favourite templates list...", nameof (filePath));
      if (targetList == null)
        throw new ArgumentNullException(nameof (targetList), "MeshBrush: cannot write favourite templates to the specified target list because it is null.");
      try
      {
        targetList.Clear();
        foreach (XElement descendant in XDocument.Load(filePath).Descendants((XName) "path"))
          targetList.Add(descendant.Value);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("MeshBrush: loading favourite templates list failed. Error message: " + ex.Message));
        return false;
      }
      return true;
    }
  }
}
