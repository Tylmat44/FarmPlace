using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolChest
{
    private static bool plowSelected = false;
    private static bool clearSelected = false;
    private static Resource seedSelected = null;
    private static Animal animalSelected = null;
    private static UseableBuilding useableBuildingSelected = null;

    public static bool PlowSelected { get => plowSelected; set => plowSelected = value; }
    public static bool ClearSelected { get => clearSelected; set => clearSelected = value; }
    public static Resource SeedSelected { get => seedSelected; set => seedSelected = value; }
    public static Animal AnimalSelected { get => animalSelected; set => animalSelected = value; }
    public static UseableBuilding UseableBuildingSelected { get => useableBuildingSelected; set => useableBuildingSelected = value; }

    public static void deselectAll()
    {
        PlowSelected = false;
        ClearSelected = false;
        SeedSelected = null;
        AnimalSelected = null;
        UseableBuildingSelected = null;
    }
}
