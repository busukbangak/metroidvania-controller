using Godot;
using System;

public class Constants : Node
{

    public static class Units
    {

        // Units
        public static int UNIT_SIZE = 16;

    }

    public static class Maps
    {
        public static string GRASS_1 = "res://Scenes/Maps/Grass/Grass_1.tscn";
    }

    public static class Screens
    {
        public static string PAUSE_MENU = "res://Scenes/Screens/PauseMenu.tscn";

        public static string DEBUG_OVERLAY = "res://Scenes/Screens/DebugOverlay.tscn";

        public static string LOADING = "res://Scenes/Screens/LoadingScreen.tscn";
    }
}
