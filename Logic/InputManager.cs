using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SkyTown.LogicManagers
{
    public class InputManager
    {
        private Matrix MouseTranslation;
        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public MouseState currentMouseState;
        public MouseState previousMouseState;

        public InputManager()
        {
            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = Keyboard.GetState();

            currentMouseState = Mouse.GetState();
            previousMouseState = Mouse.GetState();

            
        }

        public void Update(Matrix transformationMatrix)
        {
            MouseTranslation = Matrix.Invert(transformationMatrix);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public bool IsNewKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        #region MouseHandling
        public Vector2 GetMousePosition()
        {
            Point mousePosition = currentMouseState.Position;
            return Vector2.Transform(new Vector2(mousePosition.X, mousePosition.Y), MouseTranslation);
        }
        public bool IsLeftClicked()
        {
            return currentMouseState.LeftButton.Equals(ButtonState.Pressed) && !previousMouseState.LeftButton.Equals(ButtonState.Pressed);
        }

        public bool IsLeftClickDown()
        {
            return currentMouseState.LeftButton.Equals(ButtonState.Pressed);
        }
        public bool IsLeftClickHolding()
        {
            return currentMouseState.LeftButton.Equals(ButtonState.Pressed) && previousMouseState.LeftButton.Equals(ButtonState.Pressed);
        }

        public bool IsRightClicked()
        {
            return currentMouseState.RightButton.Equals(ButtonState.Pressed) && !previousMouseState.RightButton.Equals(ButtonState.Pressed);
        }

        public bool IsRightClickDown()
        {
            return currentMouseState.RightButton.Equals(ButtonState.Pressed);
        }

        public bool IsRightClickHolding()
        {
            return currentMouseState.RightButton.Equals(ButtonState.Pressed) && previousMouseState.RightButton.Equals(ButtonState.Pressed);
        }
        #endregion
    }
}
