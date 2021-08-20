using Godot;
using System;

public class WorldManager : Node
{

    public static Node World;

    public static Node CurrentWorldSpace;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        var child = root.GetChild(root.GetChildCount() - 1);

        // Start default scene, if Game is started with main scene, else reparent other scene to world
        if (child.Name == "World")
        {
            World = child;
            ChangeWorldSpace(Constants.Screens.MAIN_MENU);
        }
        else
        {
            root.CallDeferred("remove_child", child);

            PackedScene packedWorldScene = (PackedScene)GD.Load("res://Scenes/World.tscn");

            World = (Node)packedWorldScene.Instance();

            root.CallDeferred("add_child", World);

            World.CallDeferred("add_child", child);
        }
    }

    public static void ChangeWorldSpace(String worldSpaceScenePath)
    {

        // Load a new scene.
        PackedScene nextWorldSpace = (PackedScene)GD.Load(worldSpaceScenePath);

        // It is now safe to remove the current scene
        CurrentWorldSpace?.CallDeferred("queue_free");

        // Instance the new scene.
        CurrentWorldSpace = nextWorldSpace.Instance();

        // Add it to the active scene, as child of world.
        World.CallDeferred("add_child", CurrentWorldSpace);
    }
}
