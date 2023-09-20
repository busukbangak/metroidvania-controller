using Godot;
using System;
using System.Collections.Generic;

public class DebugStat
{
    public Godot.Object StatObject;
    public string Reference;
    public bool IsMethod;

    public DebugStat(Godot.Object statObject, string reference, bool isMethod)
    {
        StatObject = statObject;
        Reference = reference;
        IsMethod = isMethod;
    }
}

public class DebugManager : Node
{
    public static bool IsDebugging;

    public static Dictionary<string, DebugStat> Stats = new Dictionary<string, DebugStat>();

    public override void _Ready()
    {
        Add("FPS", this, nameof(GetFPS), true);
        Add("Static memory", this, nameof(GetStaticMemoryUsage), true);

        IsDebugging = UIManager.GetUI(nameof(Constants.Screens.DEBUG_OVERLAY)).GetNode<Control>("Container").Visible;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("debug"))
        {
            IsDebugging = UIManager.GetUI(nameof(Constants.Screens.DEBUG_OVERLAY)).GetNode<Control>("Container").Visible;
            UIManager.GetUI(nameof(Constants.Screens.DEBUG_OVERLAY)).GetNode<Control>("Container").Visible = !UIManager.GetUI(nameof(Constants.Screens.DEBUG_OVERLAY)).GetNode<Control>("Container").Visible;
        }

        // Update debugging values
        if (IsDebugging)
        {
            UpdateDebugValues();
        }
    }

    public static void UpdateDebugValues()
    {
        string labelText = "";
        foreach (KeyValuePair<string, DebugStat> stat in Stats)
        {
            object value;
            if (stat.Value.StatObject != null && WeakRef(stat.Value.StatObject)?.GetRef() != null)
            {
                if (stat.Value.IsMethod)
                {
                    value = stat.Value.StatObject.Call(stat.Value.Reference);
                }
                else
                {
                    value = stat.Value.StatObject.Get(stat.Value.Reference);
                }

                labelText += stat.Key + ": " + value;
                labelText += "\n";
            }

        }

        UIManager.GetUI(nameof(Constants.Screens.DEBUG_OVERLAY)).GetNode<Label>("Container/Label").Text = labelText;
    }

    public static void Add(string name, Godot.Object statObject = null, string reference = null, bool isMethod = false)
    {
        Stats.Add(name, new DebugStat(statObject, reference, isMethod));
    }

    public static void Remove()
    {

    }

    public string GetStaticMemoryUsage()
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = OS.GetStaticMemoryUsage();
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
        // show a single decimal place, and no space.
        return string.Format("{0:0.##} {1}", len, sizes[order]);
    }

    public float GetFPS()
    {
        return Engine.GetFramesPerSecond();
    }
}
