using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace group_14_assignment_5
{
    public class Submarine
    {
        private Model submarineModel;
        private Model propellerModel;
        private Model periscopeModel;

        Texture2D submarineTexture;
        Texture2D propellerTexture;
        Texture2D periscopeTexture;

        // Orbit parameters (THESE ARE EDITABLE IF YOU WANT TO MAKE ANY CHANGES)
        private float orbitRadius = 30f;
        private float orbitAngle = 0f;
        private float orbitSpeed = 0.5f;

        // Propeller animation
        private float propellerAngle = 0f;
        private float propellerSpeed = 12f;

        // Periscope scanning
        private float scanTime = 0f;
        private float scanSpeed = 2f;
        private float scanAmplitude = MathHelper.ToRadians(40);
        

        // Scale Transforms (scaling submarine will scale the others in relation)
        public Vector3 submarineScale = new Vector3(20f);
        public Vector3 propellerScale = new Vector3(0.4f);
        public Vector3 periscopeScale = new Vector3(0.3f);

        // Translation Offsets
        public Vector3 submarineTranslationOffset = Vector3.Zero;
        public Vector3 propellerTranslationOffset = new Vector3(0.80f, -0.14f, 0f);
        public Vector3 periscopeTranslationOffset = new Vector3(0f, 0.25f, 0f);

        // Rotation Offsets
        public Vector3 submarineRotationOffset = Vector3.Zero;
        public Vector3 propellerRotationOffset = new Vector3(0, MathHelper.Pi/3 - 0.07f, 0);
        public Vector3 periscopeRotationOffset = Vector3.Zero;

        // Propeller Spin Axis (Fixes mesh axis not being centered)
        public Vector3 propellerAxis = new Vector3(-0.67f,0,-1);

        public Submarine(ContentManager content)
        {
            // Load Meshes
            submarineModel = content.Load<Model>("meshes/mesh_submarine");
            propellerModel = content.Load<Model>("meshes/mesh_propeller");
            periscopeModel = content.Load<Model>("meshes/mesh_periscope");

            // Load Textures
            submarineTexture = content.Load<Texture2D>("textures/texture_submarine");
            propellerTexture = content.Load<Texture2D>("textures/texture_propeller");
            periscopeTexture = content.Load<Texture2D>("textures/texture_periscope");
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            orbitAngle += orbitSpeed * dt;
            propellerAngle += propellerSpeed * dt;
            scanTime += dt;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            Vector3 orbitPosition = new Vector3(
                orbitRadius * (float)System.Math.Cos(orbitAngle),
                0,
                orbitRadius * (float)System.Math.Sin(orbitAngle)
            );

            Matrix orbitTranslation = Matrix.CreateTranslation(orbitPosition);

            Matrix orbitRotation =
                Matrix.CreateRotationY(-orbitAngle + MathHelper.PiOver2);

            Matrix submarineOffsetRotation =
                Matrix.CreateFromYawPitchRoll(
                    submarineRotationOffset.Y,
                    submarineRotationOffset.X,
                    submarineRotationOffset.Z
                );

            Matrix submarineOffsetTranslation =
                Matrix.CreateTranslation(submarineTranslationOffset);

            Matrix submarineWorld =
                Matrix.CreateScale(submarineScale) *
                submarineOffsetRotation *
                orbitRotation *
                submarineOffsetTranslation *
                orbitTranslation;

            DrawModel(submarineModel, submarineTexture, submarineWorld, view, projection);

            DrawPropeller(submarineWorld, view, projection);
            DrawPeriscope(submarineWorld, view, projection);
        }

        private void DrawPropeller(Matrix parentWorld, Matrix view, Matrix projection)
        {
            // Normalize axis just in case
            Vector3 axis = Vector3.Normalize(propellerAxis);

            Matrix spin =
                Matrix.CreateFromAxisAngle(axis, propellerAngle);

            Matrix offsetRotation =
                Matrix.CreateFromYawPitchRoll(
                    propellerRotationOffset.Y,
                    propellerRotationOffset.X,
                    propellerRotationOffset.Z
                );

            Matrix offsetTranslation =
                Matrix.CreateTranslation(propellerTranslationOffset);

            Matrix world =
                Matrix.CreateScale(propellerScale) *
                spin *
                offsetRotation *
                offsetTranslation *
                parentWorld;

            DrawModel(propellerModel, propellerTexture, world, view, projection);
        }

        private void DrawPeriscope(Matrix parentWorld, Matrix view, Matrix projection)
        {
            float scanAngle =
                scanAmplitude * (float)System.Math.Sin(scanSpeed * scanTime);

            Matrix scanRotation =
                Matrix.CreateRotationY(scanAngle);

            Matrix offsetRotation =
                Matrix.CreateFromYawPitchRoll(
                    periscopeRotationOffset.Y,
                    periscopeRotationOffset.X,
                    periscopeRotationOffset.Z
                );

            Matrix offsetTranslation =
                Matrix.CreateTranslation(periscopeTranslationOffset);

            Matrix world =
                Matrix.CreateScale(periscopeScale) *
                scanRotation *
                offsetRotation *
                offsetTranslation *
                parentWorld;

            DrawModel(periscopeModel, periscopeTexture, world, view, projection);
        }

        private void DrawModel(Model model, Texture2D texture, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.TextureEnabled = true;
                    effect.Texture = texture;

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}