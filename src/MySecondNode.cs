using Godot;
using ImGuiNET;

public partial class MySecondNode : Node
{
    private Texture2D iconTexture;
    private SubViewport vp;
    private ColorRect vpSquare;
    private int iconSize = 64;
    private static bool fontLoaded = false;

    public override void _EnterTree()
    {
        if (!fontLoaded)
        {
            ImGuiGD.Init();

            // use Hack for the default glyphs, M+2 for Japanese
            ImGuiGD.AddFont(GD.Load<FontFile>("res://data/Hack-Regular.ttf"), 18.0f);
            ImGuiGD.AddFont(GD.Load<FontFile>("res://data/MPLUS2-Regular.ttf"), 22.0f, merge: true);

            ImGuiGD.AddFontDefault();
            ImGuiGD.RebuildFontAtlas();
            fontLoaded = true;
        }

        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;

        // let ImGui draw the mouse cursor
        io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
        io.MouseDrawCursor = true;
        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

    public override void _Ready()
    {
        ImGuiLayer.Instance?.Connect(_ImGuiLayout);
        iconTexture = GD.Load<Texture2D>("res://data/icon.svg");
        vp = GetNode<SubViewport>("%SubViewport");
        vpSquare = GetNode<ColorRect>("%VPSquare");
    }

    public override void _ExitTree()
    {
        // restore the hardware mouse cursor
        var io = ImGui.GetIO();
        io.MouseDrawCursor = false;
        //io.BackendFlags &= ~ImGuiBackendFlags.HasMouseCursors;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _Process(double delta)
    {
        vpSquare.Rotation += (float)delta;
    }

    private void _ImGuiLayout()
    {
        ImGui.Begin("Scene 2");
        ImGui.Text("hello Godot 4");
        ImGuiGodot.Image(iconTexture, new(iconSize, iconSize));
        ImGui.DragInt("size", ref iconSize, 1.0f, 32, 512);

        ImGui.Dummy(new(0, 20.0f));

        if (ImGui.Button("change scene"))
        {
            GetTree().ChangeSceneToFile("res://data/demo.tscn");
        }

        ImGui.End();

        ImGui.Begin("Unicode test");
        ImGui.Text("Hiragana: こんばんは");
        ImGui.Text("Katakana: ハロウィーン");
        ImGui.Text("   Kanji: 日本語");
        ImGui.End();

        ImGui.Begin("SubViewport test");
        ImGuiGodot.SubViewport(vp);
        ImGui.End();

        ImGui.ShowDemoWindow();
    }

    private void _on_show_hide()
    {
        if (ImGuiLayer.Instance.Visible)
        {
            ImGuiLayer.Instance.Visible = false;
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        else
        {
            ImGuiLayer.Instance.Visible = true;
            Input.MouseMode = Input.MouseModeEnum.Hidden;
        }
    }
}
