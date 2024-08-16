using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public List<UnitData> unitData;

    #region Editor
#if UNITY_EDITOR
    [Header("Unit CSV Files")]
    public TextAsset unitDataSheet;

    public void LoadUnitDataFromFile()
    {
        string[,] grid = CSVReader.GetCSVGrid("/Assets/units" + ".csv");
        //string[,] grid = CSVReader.GetCSVGrid(unitDataSheet);
        UnitData unit = new UnitData();
        List<string> keys = new List<string>();

        //First create a list for holding our key values
        for (int y = 0; y < grid.GetUpperBound(1); ++y)
        {
            keys.Add(grid[0, y]);
        }

        //Loop through the columns, adding the value to the appropriate key
        for (int x = 1; x < grid.GetUpperBound(0); x++)
        {
            Dictionary<string, string> columnData = new Dictionary<string, string>();
            for (int k = 0; k < keys.Count; k++)
            {
                columnData.Add(keys[k], grid[x, k]);
                //Debug.Log("Key: " + keys[k] + ", Value: " + grid[x, k]);
            }

            //Loop through the dictionary using the key values
            foreach (KeyValuePair<string, string> item in columnData)
            {
                // Gets a unit data based off the ID and updates the data
                if (item.Key.Contains("ID"))
                    unit.ID = item.Value;
                if (item.Key.Contains("Name"))
                    unit.name = item.Value;
                if (item.Key.Contains("Description"))
                    unit.description = item.Value;
                if (item.Key.Contains("Health"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 100;
                    unit.health = temp;
                }
                if (item.Key.Contains("Damage"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 20;
                    unit.damage = temp;
                }
                if (item.Key.Contains("Speed"))
                {
                    int temp = int.TryParse(item.Value, out temp) ? temp : 10;
                    unit.speed = temp;
                }
            }
            UpdateUnit(unit);
        }
    }

    private void UpdateUnit(UnitData _unitData)
    {
        UnitData unit = unitData.Find(x => x.ID == _unitData.ID);
        unit.name = _unitData.name;
        unit.description = _unitData.description;
        unit.health = _unitData.health;
        unit.damage = _unitData.damage;
        unit.speed = _unitData.speed;

        //flag the object as "dirty" in the editor so it will be saved
        //EditorUtility.SetDirty(unit);

        // Prompt the editor database to save dirty assets, committing your changes to disk.
        AssetDatabase.SaveAssets();
    }

    public void CreateUnit()
    {
        UnitData ud = new UnitData();
        ud.name = "not loved";
        ud.speed = 100; ;
        unitData.Add(ud);
    }


    [CustomEditor(typeof(GameData))]
    public class GameDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameData gameData = (GameData)target;
            DrawDefaultInspector();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Load Unit Data from file?"))
            {
                if (EditorUtility.DisplayDialog("Load Unit Spreadsheet Data", "Are you sure you want to load data? This will overwrite any existing data", "Yes", "No"))
                {
                    gameData.LoadUnitDataFromFile();
                    EditorUtility.SetDirty(gameData);
                }
            }
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Create Unit Data from file?"))
            {
                gameData.CreateUnit();
                EditorUtility.SetDirty(gameData);
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
    #endregion
}

[System.Serializable]
public class UnitData
{
    public string ID;
    public string name;
    public string description;
    public int speed; 
    public int health; 
    public int damage;
}
