using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_14_assignment_5;

public class Bubbles
{
    private Bubble[] bubbles;
    private GraphicsDevice graphicsDevice;
    private Model bubbleModel;

    private Vector3 groupPosition;
    private float groupRiseSpeed = 10f;
    
    public Vector3 initialPosition;

    public Bubbles(int bubbleCount, GraphicsDevice graphicsDevice, Model bubbleModel, Vector3 _groupPosition)
    {
        this.graphicsDevice = graphicsDevice;
        this.bubbleModel = bubbleModel;

        groupPosition = _groupPosition;
        initialPosition = _groupPosition;
        bubbles = new Bubble[bubbleCount];
        var rng = new Random();

        for (int i = 0; i < bubbleCount; i++)
        {
            var pos = new Vector3(
                (float)(rng.NextDouble() * 30 - 10),
                (float)(rng.NextDouble() * 50 - 10),
                (float)(rng.NextDouble() * 5 - 10)
            );

            var sway = (float)(rng.NextDouble() * 10);

            float size = 0.6f + (float)rng.NextDouble() * 0.6f;

            bubbles[i] = new Bubble(bubbleModel, pos, size, sway);
        }
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        groupPosition.Y += groupRiseSpeed*(float)Math.Sin(dt);

        if (groupPosition.Y > 100)
        {
            groupPosition = initialPosition;
        }

        foreach (var b in bubbles)
            b.Animate(gameTime);
    }

    public void Draw(Matrix view, Matrix projection)
    {
        graphicsDevice.BlendState = BlendState.AlphaBlend;
        graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
        graphicsDevice.RasterizerState = RasterizerState.CullNone;

        Matrix parentWorld = Matrix.CreateTranslation(groupPosition);

        foreach (var b in bubbles)
        {
            Matrix world = b.ChildMatrix * parentWorld;
            DrawModel(bubbleModel, world, view, projection, 0.35f);
        }

        graphicsDevice.BlendState = BlendState.Opaque;
        graphicsDevice.DepthStencilState = DepthStencilState.Default;
        graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
    }

    private static void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, float alpha)
    {
        if (model == null) return;

        var transforms = new Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(transforms);

        foreach (var mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.LightingEnabled = false;
                effect.DiffuseColor = new Vector3(0.6f, 0.8f, 1f);
                effect.EmissiveColor = new Vector3(0.15f, 0.2f, 0.25f);
                effect.Alpha = alpha;

                effect.World = transforms[mesh.ParentBone.Index] * world;
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.Draw();
        }
    }

    private class Bubble
    {
        private Vector3 bubblePosition;
        private float bubbleSize;

        private Vector3 modelCenter;
        private float modelRadius;

        private float _swayAmount;

        public Matrix ChildMatrix { get; private set; } = Matrix.Identity;

        public Bubble(Model model, Vector3 position, float size, float swayAmount)
        {
            bubblePosition = position;
            bubbleSize = size;
            _swayAmount = swayAmount;

            BoundingSphere bounds = new BoundingSphere();
            foreach (var mesh in model.Meshes)
                bounds = BoundingSphere.CreateMerged(bounds, mesh.BoundingSphere);

            modelCenter = bounds.Center;
            modelRadius = bounds.Radius;
        }

        public void Animate(GameTime gameTime)
        {
            float t = (float)gameTime.TotalGameTime.TotalSeconds;

            Vector3 localPos = bubblePosition + new Vector3(_swayAmount*(float)Math.Sin(t), 0f, 0f);

            float scale = bubbleSize / modelRadius;

            scale = .3f+ .05f*(float)Math.Sin(2*t);

            ChildMatrix =
                Matrix.CreateTranslation(-modelCenter) *
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(localPos);
        }
    }
}