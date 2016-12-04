using System;
using System.Collections.Generic;
using System.Text;
using CP;

namespace IMine
{
	class Program
	{
		static int[,] bworld = new int[50, 50];
		static RenderPane world;

		static uint px, py;

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
			CDraw window = new CDraw();

			px = 9;
			py = 0;

			window.Clear();

			for (int y = 0; y < 50; y++)
			for (int x = 0; x < 50; x++)
					bworld[x, y] = (y == 0 ? ((int)Blocks.Grass) : (y > 4 ? ((int)Blocks.Stone) : ((int)Blocks.Dirt)));

			world = new RenderPane(4 * 50, 4 * 50 + 16);

			#region Init Blocks
			#region Stone
			stone = new Bitmap(4, 4);
			stone.Fill(0x80);
			stone.Border(0x80, 'X');
			for (int x = 1; x < 3; x++)
				for (int y = 1; y < 3; y++)
					stone.SetText(x, y, ' ');
			#endregion

			#region Grass
			grass = new Bitmap(4, 4);
			grass.Fill(0xa0);
			grass.Border(0xa4, 'X');
			grass.SetColor(1, 1, 0x40);
			grass.SetColor(3, 1, 0x4a);
			for (int x = 0; x <= 3; x++)
				for (int y = 2; y <= 3; y++)
					grass.SetColor(x, y, 0x4a);
			#endregion

			#region Dirt
			dirt = new Bitmap(4, 4);
			dirt.Fill(0x64);
			for (int x = 1; x < 3; x++)
				for (int y = 1; y < 3; y++)
					stone.SetText(x, y, ' ');
			dirt.SetText(1, 1, '*');
			dirt.SetText(2, 2, '*');
			dirt.Border(0x64, 'X');
			#endregion

			miner = new Bitmap(4, 4);
			miner.Fill(0xe0);
			miner.DrawHorizontalText(0, 0, 0xe0, "****.__.    \\../");
			#endregion

			grass_mined = new Bitmap(4, 4);
			grass_mined.Fill(0x80);
			dirt_mined = grass_mined;
			stone_mined = grass_mined;

			Render();

			ConsoleKey k;

			while((k = Console.ReadKey(true).Key) != ConsoleKey.Escape)
			{
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
				if (px < 0 || px > uint.MaxValue - 10)
					px = 0;
				if (py < 0 || py > uint.MaxValue - 10)
					py = 0;
				if (px > 49)
					px = 49;
				if (py > 49)
					py = 49;
				if (bworld[px, py] != ((int)Blocks.Grass_Mined)) {
					bworld[px, py] = ((int)Blocks.Grass_Mined);
					for(int i = 0; i < 1000000; i++)
					{

					}
				}
				Render();
				var map = new Bitmap(Convert.ToInt32(px) + 1 + Convert.ToInt32(py), 1);
				map.DrawHorizontalText(0, 0, 0x0a, px.ToString() + ":" + py.ToString());
			}

			Console.ReadKey();
		}

		static void Render()
		{
			for(uint _x = 0; _x < 50; _x++)
				for (uint _y = 0; _y < 50; _y++)
				{
					uint x = _x * 4;
					uint y = _y * 4;

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
