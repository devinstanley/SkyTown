using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SkyTown.LogicManagers
{
    public class InputManager
    {
        private Matrix MouseTranslation;
        private Vector2 viewportOffset;
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

        public void Update(Camera ViewCamera)
        {
            MouseTranslation = Matrix.Invert(ViewCamera.GetTransformation());
            viewportOffset = new(ViewCamera._viewport.X, ViewCamera._viewport.Y);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (IsLeftClicked())
            {
                Debug.WriteLine(GetMousePosition());
            }
        }

        public bool IsNewKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        //Returns current number key pressed, if none -1
        public int GetNumKeyDown()
        {
            foreach (Keys key in currentKeyboardState.GetPressedKeys())
            {
                if (key == Keys.D1 || key == Keys.NumPad1)
                {
                    return 1;
                }
                if (key == Keys.D2 || key == Keys.NumPad2)
                {
                    return 2;
                }
                if (key == Keys.D3 || key == Keys.NumPad3)
                {
                    return 3;
                }
                if (key == Keys.D4 || key == Keys.NumPad4)
                {
                    return 4;
                }
                if (key == Keys.D5 || key == Keys.NumPad5)
                {
                    return 5;
                }
                if (key == Keys.D6 || key == Keys.NumPad6)
                {
                    return 6;
                }
                if (key == Keys.D7 || key == Keys.NumPad7)
                {
                    return 7;
                }
                if (key == Keys.D8 || key == Keys.NumPad8)
                {
                    return 8;
                }
                if (key == Keys.D9 || key == Keys.NumPad9)
                {
                    return 9;
                }
            }
            return -1;
        }

        #region MouseHandling
        public Vector2 GetMousePosition()
        {
            Point mousePosition = currentMouseState.Position;
            return Vector2.Transform(new Vector2(mousePosition.X - viewportOffset.X, mousePosition.Y - viewportOffset.Y), MouseTranslation);
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
