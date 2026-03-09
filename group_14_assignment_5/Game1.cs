using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment_5;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    // bubble
    private Bubbles _bubbles;
    private Bubbles _bubbles2;

    private Matrix view, projection;
    Model bubbleModel;

    // submarine 
    private Submarine submarine;

    private Matrix submarineView;
    private Matrix submarineProjection;

    // fish
    private Model _fishTail1;
    private Model _fishBody1;
    private Fish _fish1;
    private Fish _fish2;
    private Fish _fish3;
    private Fish _fish4;
    private Fish _fish5;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    

    protected override void LoadContent()
    {
        // bubble
        bubbleModel = Content.Load<Model>("models/sphere");

        _bubbles = new Bubbles(10, GraphicsDevice, bubbleModel, new Vector3(0, -90, -50));
        _bubbles2 = new Bubbles(15, GraphicsDevice, bubbleModel, new Vector3(30, -90, 0));
        //_bubbles = new Bubbles(10, GraphicsDevice, bubbleModel, new Vector3(0, -30, -50));
        //_bubbles2 = new Bubbles(15, GraphicsDevice, bubbleModel, new Vector3(30, -30, 0));
        
        // group view and projection matrix 
        view = Matrix.CreateLookAt(
            new Vector3(0, 0, 100),
            Vector3.Zero,
            Vector3.Up
        );

        projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45f),
            GraphicsDevice.Viewport.AspectRatio,
            0.1f,
            1000f
        );
        
        // submarine
        submarine = new Submarine(Content);

        submarineView = Matrix.CreateLookAt(
            new Vector3(12, 8, 12),
            Vector3.Zero,
            Vector3.Up
        );

        submarineProjection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.ToRadians(45f),
            GraphicsDevice.Viewport.AspectRatio,
            0.1f,
            100f
        );

        
        // fish
        //_fishTail1 = Content.Load<Model>("meshes/fishTail");
        _fishTail1 = Content.Load<Model>("meshes/fish_tail");

        
        //_fishBody1 = Content.Load<Model>("meshes/fishBody1");
        _fishBody1 = Content.Load<Model>("meshes/fish_body");


        _fish1 = new Fish(
            view: view,
            projection: projection,
            _fishTail1,
            5.0f,
            _fishBody1,
            5.0f,
            position: new Vector3(0.0f, 20f, 20f),
            90,
            -90,
            90,
            10f,
            30f,
            0.05f,
            0f,
            new(254 / 255f, 168 / 255f, 47 / 255f));
        
        _fish2 = new Fish(
            view: view,
            projection: projection,
            _fishTail1,
            5.0f,
            _fishBody1,
            5.0f,
            position: new Vector3(-100f, -50.0f, -70f),
            90,
            -90,
            90,
            12f,
            30f,
            0.05f,
            0f,
            new(225/255f, 145/255f, 107/255f));
        
        _fish3 = new Fish(
            view: view,
            projection: projection,
            _fishTail1,
            5.0f,
            _fishBody1,
            5.0f,
            position: new Vector3(-150f, -25f, -60f),
            90,
            -90,
            90,
            15f,
            30f,
            0.05f,
            0f,
            new(195/255f, 88/255f, 23/255f));
        
        
        _fish4 = new Fish(
            view: view,
            projection: projection,
            _fishTail1,
            5.0f,
            _fishBody1,
            5.0f,
            position: new Vector3(-110f, 0f, 30f),
            90,
            -90,
            90,
            20f,
            30f,
            0.05f,
            0f,
            new(195/255f, 88/255f, 23/255f));
        
        _fish5 = new Fish(
            view: view,
            projection: projection,
            _fishTail1,
            5.0f,
            _fishBody1,
            5.0f,
            position: new Vector3(-110f, -50.0f, 10f),
            90,
            -90,
            90,
            12f,
            40f,
            0.05f,
            0f,
            new(225/255f, 145/255f, 107/255f));

    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        _bubbles.Update(gameTime);
        _bubbles2.Update(gameTime);
        submarine.Update(gameTime);
        _fish1.Update(gameTime);
        _fish2.Update(gameTime);
        _fish3.Update(gameTime);
        _fish4.Update(gameTime);
        _fish5.Update(gameTime);


    

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);


        _bubbles.Draw(view, projection);
        _bubbles2.Draw(view, projection);
        submarine.Draw(view, projection);
        
        _fish1.DrawMesh();
        _fish2.DrawMesh();
        _fish3.DrawMesh();
        _fish4.DrawMesh();
        _fish5.DrawMesh();


    }
}