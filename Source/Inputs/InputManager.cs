using GameProject.Source.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.Inputs
{
	static class InputManager
	{
		static KeyboardState currentKeyState;
		static KeyboardState previousKeyState;
		static MouseStateExtended currentMouseState;
		static MouseStateExtended previousMouseState;
		public static bool mouseOverUI;

		static InputManager()
		{
			
		}

		public static MouseStateExtended GetMouseState()
		{
			previousMouseState = currentMouseState;
			currentMouseState = MouseExtended.GetState();
			return currentMouseState;
		}

		public static KeyboardState GetKeyState()
		{
			previousKeyState = currentKeyState;
			currentKeyState = Keyboard.GetState();
			return currentKeyState;
		}

		public static bool IsKeyPressed(Keys key)
		{
			return currentKeyState.IsKeyDown(key);
		}

		public static bool WasKeyPressed(Keys key)
		{
			return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
		}

		public static bool IsMousePressed(MouseButton key)
		{
			return currentMouseState.IsButtonDown(key);
		}

		public static bool WasMousePressed(MouseButton key)
		{
			return currentMouseState.IsButtonDown(key) && !previousMouseState.IsButtonDown(key);
		}


		public static void Update(GameTime gameTime)
		{
			GetKeyState();
			GetMouseState();
			if (WasKeyPressed(Keys.Tab))
				{
				ToggleDebugWindow();
				}

			if (currentMouseState.Y > Game1.screenHeight / 6 * 5)
			{
				mouseOverUI = true;
			} else 
			{
				mouseOverUI = false;
			}
		}


		static void ToggleDebugWindow()
		{
			if (Game1.DebugOn == false)
			{
				Game1.DebugOn = true;
				DebugInterface.debugViewOn = true;
			}
			else
			{
				Game1.DebugOn = false;
				DebugInterface.debugViewOn = false;
			}
		}
	}
	}
