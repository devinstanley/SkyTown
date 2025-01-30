using SharpDX.D3DCompiler;
using SharpDX;
using SkyTown.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Logic
{
    public static class ObjectParser
    {
        public static Tile ParseTile(string input)
        {
            var result = new Dictionary<string, List<int>>();

            var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                line.Trim().Trim('"');

                // Split the line into key and value
                var parts = line.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) continue;

                string key = parts[0].Trim().Trim('"'); // Remove quotes from the key
                string value = parts[1].Replace("(", "").Replace(")", "");

                int mos = 0;
                result[key] = value.Split(',')
                        .Select(m => { int.TryParse(m, out mos); return mos; })
                        .ToList();
            }


            return result;
        }


    }
}
