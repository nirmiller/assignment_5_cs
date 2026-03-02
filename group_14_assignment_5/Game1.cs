using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment_5;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private Bubbles _bubbles;

    private Matrix view, projection;
    Model bubbleModel;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
    
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        bubbleModel = Content.Load<Model>("models/sphere");

        _bubbles = new Bubbles(10, GraphicsDevice, bubbleModel, new Vector3(0, -30, -50));
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


    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        _bubbles.Update(gameTime);
    

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        

        _bubbles.Draw(view, projection);
        

        base.Draw(gameTime);
    }
    
}