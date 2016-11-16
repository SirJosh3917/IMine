using System;
using System.Collections.Generic;
using System.Text;
using CP;

namespace IMine
{
	class Program
	{
		//World width and height constants
		public const int WorldX = 50;
		public const int WorldY = 50;
		
		//Create a new world initalizer array
		static int[,] bworld = new int[WorldX, WorldY];
		
		//The world's render pane
		static RenderPane world;

		//Player's X and Y
		static uint px, py;

		//Drawomg Bitmaps
		static Bitmap
			miner,
			stone,
			stone_mined,
			grass,
			grass_mined,
			dirt,
			dirt_mined;

		static void Main(string[] args)
		{
			//Define Variables
			CDraw window = new CDraw();
			world = new RenderPane(4 * WorldX, 4 * WorldY + 16);
			stone = new Bitmap(4, 4);
			grass = new Bitmap(4, 4);
			dirt = new Bitmap(4, 4);
			miner = new Bitmap(4, 4);
			grass_mined = new Bitmap(4, 4);
			grass_mined.Fill(0x80);
			dirt_mined = grass_mined;
			stone_mined = grass_mined;

			px = 9;
			py = 0;
			
			//Clear the CDraw window
			window.Clear();
			
			//Generate the world
			for (int y = 0; y < WorldY; y++)
				for (int x = 0; x < WorldX; x++)
					bworld[x, y] = (y == 0 ? ((int)Blocks.Grass) : (y > 4 ? ((int)Blocks.Stone) : ((int)Blocks.Dirt)));

			#region Init Blocks
			#region Stone
			stone.Fill(0x80);
			stone.Border(0x80, 'X');
			for (int x = 1; x < 3; x++)
				for (int y = 1; y < 3; y++)
					stone.SetText(x, y, ' ');
			#endregion

			#region Grass
			grass.Fill(0xa0);
			grass.Border(0xa4, 'X');
			grass.SetColor(1, 1, 0x40);
			grass.SetColor(3, 1, 0x4a);
			for (int x = 0; x <= 3; x++)
				for (int y = 2; y <= 3; y++)
					grass.SetColor(x, y, 0x4a);
			#endregion

			#region Dirt
			dirt.Fill(0x64);
			for (int x = 1; x < 3; x++)
				for (int y = 1; y < 3; y++)
					stone.SetText(x, y, ' ');
			dirt.SetText(1, 1, '*');
			dirt.SetText(2, 2, '*');
			dirt.Border(0x64, 'X');
			#endregion

			miner.Fill(0xe0);
			miner.DrawHorizontalText(0, 0, 0xe0, "****.__.    \\../");
			#endregion
			
			//Render the world
			Render();
			
			ConsoleKey k;
			
			int _w1, _w2;
			
			//While any key but escape is pressed
			while((k = Console.ReadKey(true).Key) != ConsoleKey.Escape)
			{
				//Check if that key is WASD
				switch (k)
				{
					case ConsoleKey.W:
						world.RenderY -= 4;
						py--;
						break;
					case ConsoleKey.A:
						if(px < 40)
							world.RenderX -= 4;
						px--;
						break;
					case ConsoleKey.S:
						world.RenderY += 4;
						py++;
						break;
					case ConsoleKey.D:
						if(px > 8)
							world.RenderX += 4;
						px++;
						break;
				}
				
				//Block player from moving off of map
				if (pyx < 0 || px > (_w2 = uint.MaxValue - 10))
					px = 0;
				if (py < 0 || py > _w2)
					py = 0;
				if (px > (_w1 = WorldY - 1))
					px = _w1;
				if (py > _w1)
					py = _w1;
				
				//If the block isn't already a mined block
				if (bworld[px, py] != ((int)Blocks.Grass_Mined)) {
					bworld[px, py] = ((int)Blocks.Grass_Mined); //Mine it
				}
				Render();
				
				//X and Y
				var map = new Bitmap(px.Length + 1 + py.Length, 1);
				map.DrawHorizontalText(0, 0, 0x0a, px.ToString() + ":" + py.ToString());
			}
			
			//Wait for a key to be pressed and then exit
			Console.ReadKey();
		}

		static void Render()
		{
			//Draw all the correct bitmaps in their corresponding areas
			uint x, y;
			
			for(uint _x = 0; _x < 50; _x++)
				for (uint _y = 0; _y < 50; _y++)
				{
					x = _x * 4;
					y = _y * 4;

					world.DrawBitmap(x, y + 16, GetBlock(bworld[_x, _y]), false);
				}
			world.DrawBitmap(px * 4, py * 4 + 16, miner, false);
			world.Refresh();
		}

		static Bitmap GetBlock(int id)
		{
			switch(id)
			{
				case 0:
					return stone;
				case 1:
					return stone_mined;
				case 2:
					return grass;
				case 3:
					return grass_mined;
				case 4:
					return dirt;
				case 5:
					return dirt_mined;
			}
			return null;
		}
	}

	public enum Blocks
	{
		Stone = 0,
		Stone_Mined = 1,
		Grass = 2,
		Grass_Mined = 3,
		Dirt = 4,
		Dirt_Mined = 5
	}
}
