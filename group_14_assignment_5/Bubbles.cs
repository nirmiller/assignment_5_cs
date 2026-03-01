using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace group_14_assignment_5;

public class Bubbles
{
    private Bubble[] bubbles;

    public Bubbles(int bubbleCount)
    {
        // will randomly generate bubbleCount number of bubbles
        bubbles = new Bubble[bubbleCount];
        
    }

    private void DrawMesh(Model m, Matrix view, Matrix projection, Matrix Orientation, Matrix Position)
    {
        Matrix[] transforms = new Matrix[m.Bones.Count];
        m.CopyAbsoluteBoneTransformsTo(transforms);
        foreach (ModelMesh mesh in m.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.DirectionalLight0.Enabled = true;
                
                effect.DirectionalLight0.DiffuseColor = lightPos;
                
                effect.DirectionalLight0.SpecularColor = new
                    Vector3(5, 0 ,0);
                effect.EmissiveColor =
                    colorVec;
                
                effect.View = view;
                effect.Projection = projection;
                effect.World = Orientation * transforms[mesh.ParentBone.Index] * Position;
                
            }
            mesh.Draw();
        }
    }
    
    
    private class Bubble
    {
        private Model bubbleModel;
        private Vector3 bubblePosition;
        private Vector3 bubbleRotation;
        private float bubbleSize;

        public Bubble(Model model, Vector3 bubblePosition, Vector3 bubbleRotation)
        {
            
        }

        public void Animate(GameTime gameTime)
        {
            
        }
        

    }


}