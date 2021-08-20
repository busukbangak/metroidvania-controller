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
        public static String GRASS_1 = "res://Scenes/Maps/Grass/Grass_1.tscn";
    }

    public static class Screens
    {
        public static String PAUSE_MENU = "res://Scenes/Screens/PauseMenu.tscn";

        public static String DEBUG_OVERLAY = "res://Scenes/Screens/DebugOverlay.tscn";

        public static String MAIN_MENU = "res://Scenes/Screens/MainMenu.tscn";

        public static String LOADING = "res://Scenes/Screens/LoadingScreen.tscn";

        public static String QUIT = "res://Scenes/Screens/ExitScreen.tscn";
    }
}
