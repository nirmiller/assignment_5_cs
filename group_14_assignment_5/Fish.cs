using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_14_assignment_5;

public class Fish
{
    // matrix
    private Matrix _worldMatrix;
    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    
    //
    private BasicEffect _basicEffect;

    // model
    private Model _fishTail;
    private float _fishTailScale;
    private Model _fishBody;
    private float _fishBodyScale; 

    //position of object 
    private Vector3 _position;
    
    // orientation of object 
    private float _yaw;
    private float _pitch;
    private float _roll;

    private float _time;

    private float _speed;
    private float _amplitude;
    private float _frequency;

    private float _tailAngle;
    private float _tailOffSet;
    
    private float _zOffset; // starting Z position
    
    private Vector3 _color = new Vector3(1f, 1f, 1f); // default white


    public Fish(Matrix view, 
        Matrix projection, 
        Model tail, 
        float fishTailScale, 
        Model body, 
        float bodyScale, 
        Vector3 position, 
        float yaw, 
        float pitch, 
        float roll,
        float speed,
        float amplitude,
        float frequency,
        float tailOffSet,
        Vector3 color)
    {
        _viewMatrix = view;
        _projectionMatrix = projection;
        
        _fishTail = tail;
        _fishTailScale = fishTailScale;
        _fishBody = body;
        _fishBodyScale = bodyScale;
        
        _position = position;
        
        _yaw = MathHelper.ToRadians(yaw);
        _pitch = MathHelper.ToRadians(pitch);
        _roll = MathHelper.ToRadians(roll);
        
        _speed = speed;
        _amplitude = amplitude;
        _frequency = frequency;
        
        _tailOffSet = tailOffSet;

        _color = color;
        
        _zOffset = position.Z;
    }

    public void Initialize(GraphicsDevice graphicsDevice)
    {
        _worldMatrix = Matrix.Identity;
    }
/*
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _time += dt;

        // Move forward along X
        _position.X += _speed * dt;
        
        // Loop back to start if passed max X
        float minX = -100f;
        float maxX = 100f;

        if (_position.X > maxX)
            _position.X = minX;

        // Sine path in Z
        //_position.Z = _amplitude * (float)Math.Sin(_position.X * _frequency);
        _position.Z = _zOffset + _amplitude * (float)Math.Sin(_position.X * _frequency);

        // Tail wiggle
        _tailAngle = 0.4f * (float)Math.Sin(_time * 6f);

        // compute rotation to face path
        float deltaX = 0.1f;
        float zAhead = _amplitude * (float)Math.Sin((_position.X + deltaX) * _frequency);
        Vector3 dir = new Vector3(deltaX, 0, zAhead - _position.Z);
        dir.Normalize();

        _yaw = (float)Math.Atan2(dir.Z, dir.X);
    }
*/
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _time += dt;

        // Move forward along X
        _position.X += _speed * dt;

        // Loop back to start if passed max X
        float minX = -100f;
        float maxX = 100f;
        if (_position.X > maxX)
            _position.X = minX;

        // Sine path in Z with offset
        _position.Z = _zOffset + _amplitude * (float)Math.Sin(_position.X * _frequency);

        // Tail wiggle
        _tailAngle = 0.4f * (float)Math.Sin(_time * 6f);

        // Compute rotation to face path
        float deltaX = 0.1f;
        float zAhead = _zOffset + _amplitude * (float)Math.Sin((_position.X + deltaX) * _frequency);
        Vector3 dir = new Vector3(deltaX, 0, zAhead - _position.Z);
        dir.Normalize();
        _yaw = (float)Math.Atan2(dir.Z, dir.X);

        // Pitch and roll could be added if needed
    }
    public void DrawMesh()
    {
        // body
        Matrix bodyAdjustment = Matrix.CreateRotationX(MathHelper.ToRadians(90));
        Matrix fishBodyWorld =
            Matrix.CreateScale(_fishBodyScale) *
            bodyAdjustment *
            Matrix.CreateFromYawPitchRoll(_yaw, _pitch, _roll) *
            Matrix.CreateTranslation(_position);

        DrawModel(_fishBody, fishBodyWorld);

 
        
        Matrix tailAdjustment = Matrix.CreateRotationX(MathHelper.ToRadians(90));
        Matrix fishTailWorld =
            Matrix.CreateScale(_fishTailScale) *
            tailAdjustment *
            Matrix.CreateRotationZ(-_tailAngle) *
            Matrix.CreateFromYawPitchRoll(_yaw, _pitch, _roll) * 
            Matrix.CreateTranslation(_position);

        DrawModel(_fishTail, fishTailWorld);

 
    }
    
    public void SetColor(Vector3 rgb)
    {
        _color = rgb;
    }

    private void DrawModel(Model model, Matrix world)
    {
        Matrix[] transforms = new Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(transforms);

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.View = _viewMatrix;
                effect.Projection = _projectionMatrix;
                effect.World = transforms[mesh.ParentBone.Index] * world;

                // Change color here
                effect.DiffuseColor = _color;
                effect.LightingEnabled = true; // enable lighting to see color
            }

            mesh.Draw();
        }
    }
}