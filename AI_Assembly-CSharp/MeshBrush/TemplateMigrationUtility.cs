// Decompiled with JetBrains decompiler
// Type: MeshBrush.TemplateMigrationUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace MeshBrush
{
  public static class TemplateMigrationUtility
  {
    public static bool TryMigrate(string filePath)
    {
      if (!string.IsNullOrEmpty(filePath))
      {
        if (File.Exists(filePath))
        {
          try
          {
            XDocument xdocument = XDocument.Load(filePath);
            GameObject gameObject = new GameObject("MeshBrush Template Migration Utility");
            ((Object) gameObject).set_hideFlags((HideFlags) 61);
            MeshBrush.MeshBrush meshBrush = (MeshBrush.MeshBrush) gameObject.AddComponent<MeshBrush.MeshBrush>();
            foreach (XElement descendant in xdocument.Descendants())
            {
              string localName = descendant.Name.LocalName;
              if (localName != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (TemplateMigrationUtility.\u003C\u003Ef__switch\u0024map6 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  TemplateMigrationUtility.\u003C\u003Ef__switch\u0024map6 = new Dictionary<string, int>(55)
                  {
                    {
                      "meshBrushTemplate",
                      0
                    },
                    {
                      "active",
                      1
                    },
                    {
                      "isActive",
                      1
                    },
                    {
                      "groupName",
                      2
                    },
                    {
                      "classicUI",
                      3
                    },
                    {
                      "previewIconSize",
                      4
                    },
                    {
                      "lockSceneView",
                      5
                    },
                    {
                      "trisCounter",
                      6
                    },
                    {
                      "globalPaintingLayers",
                      7
                    },
                    {
                      "paintKey",
                      8
                    },
                    {
                      "deleteKey",
                      9
                    },
                    {
                      "combineAreaKey",
                      10
                    },
                    {
                      "increaseRadiusKey",
                      11
                    },
                    {
                      "decreaseRadiusKey",
                      12
                    },
                    {
                      "brushRadius",
                      13
                    },
                    {
                      "color",
                      14
                    },
                    {
                      "brushColor",
                      14
                    },
                    {
                      "useMeshDensity",
                      15
                    },
                    {
                      "minMeshDensity",
                      16
                    },
                    {
                      "maxMeshDensity",
                      17
                    },
                    {
                      "minNrOfMeshes",
                      18
                    },
                    {
                      "maxNrOfMeshes",
                      19
                    },
                    {
                      "delay",
                      20
                    },
                    {
                      "verticalOffset",
                      21
                    },
                    {
                      "alignWithStroke",
                      22
                    },
                    {
                      "slopeInfluence",
                      23
                    },
                    {
                      "useSlopeFilter",
                      24
                    },
                    {
                      "maxSlopeFilterAngle",
                      25
                    },
                    {
                      "inverseSlopeFilter",
                      26
                    },
                    {
                      "manualReferenceVectorSampling",
                      27
                    },
                    {
                      "showReferenceVectorInSceneGUI",
                      28
                    },
                    {
                      "slopeReferenceVector",
                      29
                    },
                    {
                      "slopeReferenceVector_HandleLocation",
                      30
                    },
                    {
                      "yAxisIsTangent",
                      31
                    },
                    {
                      "scattering",
                      32
                    },
                    {
                      "autoStatic",
                      33
                    },
                    {
                      "useOverlapFilter",
                      34
                    },
                    {
                      "randomAbsMinDist",
                      35
                    },
                    {
                      "uniformScale",
                      36
                    },
                    {
                      "constantUniformScale",
                      37
                    },
                    {
                      "foldoutState_SetOfMeshesToPaint",
                      38
                    },
                    {
                      "foldoutState_Templates",
                      39
                    },
                    {
                      "foldoutState_CustomizeKeyboardShortcuts",
                      40
                    },
                    {
                      "foldoutState_BrushSettings",
                      41
                    },
                    {
                      "foldoutState_Slopes",
                      42
                    },
                    {
                      "foldoutState_Randomizers",
                      43
                    },
                    {
                      "foldoutState_OverlapFilter",
                      44
                    },
                    {
                      "foldoutState_ApplyAdditiveScale",
                      45
                    },
                    {
                      "foldoutState_Optimize",
                      46
                    },
                    {
                      "randomUniformRange",
                      47
                    },
                    {
                      "randomNonUniformRange",
                      48
                    },
                    {
                      "constantAdditiveScale",
                      49
                    },
                    {
                      "constantScaleXYZ",
                      50
                    },
                    {
                      "randomRotation",
                      51
                    },
                    {
                      "autoSelectOnCombine",
                      52
                    }
                  };
                }
                int num1;
                // ISSUE: reference to a compiler-generated field
                if (TemplateMigrationUtility.\u003C\u003Ef__switch\u0024map6.TryGetValue(localName, out num1))
                {
                  switch (num1)
                  {
                    case 0:
                      XAttribute xattribute = descendant.Attribute((XName) "version");
                      if (xattribute != null && 1.89999997615814 <= (double) float.Parse(xattribute.Value))
                      {
                        Debug.LogWarning((object) "MeshBrush: The template you tried to migrate actually is already up to date with the current format. Cancelling process...");
                        return false;
                      }
                      continue;
                    case 1:
                      meshBrush.active = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 2:
                      meshBrush.groupName = descendant.Value;
                      continue;
                    case 3:
                      meshBrush.classicUI = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 4:
                      meshBrush.previewIconSize = float.Parse(descendant.Value);
                      continue;
                    case 5:
                      meshBrush.lockSceneView = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 6:
                      meshBrush.stats = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 7:
                      int index = 0;
                      using (IEnumerator<XElement> enumerator = descendant.Elements().GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          XElement current = enumerator.Current;
                          meshBrush.layerMask[index] = string.CompareOrdinal(current.Value, "false") != 0;
                          ++index;
                        }
                        continue;
                      }
                    case 8:
                      meshBrush.paintKey = (KeyCode) Enum.Parse(typeof (KeyCode), descendant.Value);
                      continue;
                    case 9:
                      meshBrush.deleteKey = (KeyCode) Enum.Parse(typeof (KeyCode), descendant.Value);
                      continue;
                    case 10:
                      meshBrush.combineKey = (KeyCode) Enum.Parse(typeof (KeyCode), descendant.Value);
                      continue;
                    case 11:
                      meshBrush.increaseRadiusKey = (KeyCode) Enum.Parse(typeof (KeyCode), descendant.Value);
                      continue;
                    case 12:
                      meshBrush.decreaseRadiusKey = (KeyCode) Enum.Parse(typeof (KeyCode), descendant.Value);
                      continue;
                    case 13:
                      meshBrush.radius = float.Parse(descendant.Value);
                      continue;
                    case 14:
                      meshBrush.color = new Color(float.Parse(descendant.Element((XName) "r").Value), float.Parse(descendant.Element((XName) "g").Value), float.Parse(descendant.Element((XName) "b").Value), float.Parse(descendant.Element((XName) "a").Value));
                      continue;
                    case 15:
                      meshBrush.useDensity = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 16:
                      meshBrush.densityRange.x = (__Null) (double) float.Parse(descendant.Value);
                      continue;
                    case 17:
                      meshBrush.densityRange.y = (__Null) (double) float.Parse(descendant.Value);
                      continue;
                    case 18:
                      meshBrush.quantityRange.x = (__Null) (double) float.Parse(descendant.Value);
                      continue;
                    case 19:
                      meshBrush.quantityRange.y = (__Null) (double) float.Parse(descendant.Value);
                      continue;
                    case 20:
                      meshBrush.delay = float.Parse(descendant.Value);
                      continue;
                    case 21:
                      float num2 = float.Parse(descendant.Value);
                      meshBrush.offsetRange = new Vector2(num2, num2);
                      continue;
                    case 22:
                      meshBrush.strokeAlignment = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 23:
                      float num3 = float.Parse(descendant.Value);
                      meshBrush.slopeInfluenceRange = new Vector2(num3, num3);
                      continue;
                    case 24:
                      meshBrush.useSlopeFilter = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 25:
                      float num4 = float.Parse(descendant.Value);
                      meshBrush.angleThresholdRange = new Vector2(num4, num4);
                      continue;
                    case 26:
                      meshBrush.inverseSlopeFilter = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 27:
                      meshBrush.manualReferenceVectorSampling = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 28:
                      meshBrush.showReferenceVectorInSceneView = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 29:
                      meshBrush.slopeReferenceVector = new Vector3(float.Parse(descendant.Element((XName) "x").Value), float.Parse(descendant.Element((XName) "y").Value), float.Parse(descendant.Element((XName) "z").Value));
                      continue;
                    case 30:
                      meshBrush.slopeReferenceVectorSampleLocation = new Vector3(float.Parse(descendant.Element((XName) "x").Value), float.Parse(descendant.Element((XName) "y").Value), float.Parse(descendant.Element((XName) "z").Value));
                      continue;
                    case 31:
                      meshBrush.yAxisTangent = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 32:
                      float num5 = float.Parse(descendant.Value);
                      meshBrush.scatteringRange = new Vector2(num5, num5);
                      continue;
                    case 33:
                      meshBrush.autoStatic = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 34:
                      meshBrush.useOverlapFilter = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 35:
                      meshBrush.minimumAbsoluteDistanceRange = new Vector2(float.Parse(descendant.Element((XName) "x").Value), float.Parse(descendant.Element((XName) "y").Value));
                      continue;
                    case 36:
                      meshBrush.uniformRandomScale = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 37:
                      meshBrush.uniformAdditiveScale = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 38:
                      meshBrush.meshesFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 39:
                      meshBrush.templatesFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 40:
                      meshBrush.keyBindingsFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 41:
                      meshBrush.brushFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 42:
                      meshBrush.slopesFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 43:
                      meshBrush.randomizersFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 44:
                      meshBrush.overlapFilterFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 45:
                      meshBrush.additiveScaleFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 46:
                      meshBrush.optimizationFoldout = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    case 47:
                      meshBrush.randomScaleRange = new Vector2(float.Parse(descendant.Element((XName) "x").Value), float.Parse(descendant.Element((XName) "y").Value));
                      continue;
                    case 48:
                      meshBrush.randomScaleRangeX = meshBrush.randomScaleRangeZ = new Vector2(float.Parse(descendant.Element((XName) "x").Value), float.Parse(descendant.Element((XName) "y").Value));
                      meshBrush.randomScaleRangeY = new Vector2(float.Parse(descendant.Element((XName) "z").Value), float.Parse(descendant.Element((XName) "w").Value));
                      continue;
                    case 49:
                      float num6 = float.Parse(descendant.Value);
                      meshBrush.additiveScaleRange = new Vector2(num6, num6);
                      continue;
                    case 50:
                      meshBrush.additiveScaleNonUniform = new Vector3(float.Parse(descendant.Element((XName) "x").Value), float.Parse(descendant.Element((XName) "y").Value), float.Parse(descendant.Element((XName) "z").Value));
                      continue;
                    case 51:
                      float num7 = float.Parse(descendant.Value);
                      meshBrush.randomRotationRange = new Vector2(num7, num7);
                      continue;
                    case 52:
                      meshBrush.autoSelectOnCombine = string.CompareOrdinal(descendant.Value, "true") == 0;
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
            meshBrush.SaveTemplate(filePath.Replace(".meshbrush", "__migrated.xml"));
          }
          catch (Exception ex)
          {
            Debug.LogError((object) ("MeshBrush: Failed to migrate template file \"" + filePath + "\". Perhaps the file is corrupted? " + ex.ToString()));
            return false;
          }
          return true;
        }
      }
      Debug.LogError((object) "MeshBrush: The specified template file path is invalid or doesn't exist! Cancelling migration process...");
      return false;
    }
  }
}
