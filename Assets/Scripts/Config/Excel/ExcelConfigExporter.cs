using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cathei.BakingSheet;
using Cathei.BakingSheet.Unity;
using ExcelConfig;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ExcelConfigExporter", menuName = "Configs/ExcelConfigExporter")]
public class ExcelConfigExporter : ScriptableObject
{
    public const string PATH = "Assets/Config/System/ExcelConfigExporter.asset";

    [SerializeField] private AxieUpgradeConfig axieUpgradeConfig;

    [MenuItem("Tools/Export Config")]
    public static async void ExportConfig()
    {
        ExcelConfigExporter excelConfigExporter = AssetDatabase.LoadAssetAtPath<ExcelConfigExporter>(PATH);
        await excelConfigExporter.Export();
        Debug.Log("Exported!");
    }

    [Button]
    public async Task Export()
    {
        MainConfigSheetContainer sheetContainer = await BakeSheet();
        axieUpgradeConfig.Import(sheetContainer.AxieUpgrade);
        AssetDatabase.SaveAssets();
    }

    private async Task<MainConfigSheetContainer> BakeSheet()
    {
        UnityLogger logger = new();
        MainConfigSheetContainer sheetContainer = new(logger);

        string path = Path.Combine(Application.dataPath, "Excel");
        ExcelSheetConverter excelConverter = new(path);
        await sheetContainer.Bake(excelConverter);
        return sheetContainer;
    }
}
