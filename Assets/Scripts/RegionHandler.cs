using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Hermannia
{
	public class RegionHandler
	{
		private readonly Dictionary<Color32, Region> _regions;
		public Region GetRegionFromColor(Color32 color) => _regions[color];

		public RegionHandler(string json)
		{
			var deserializedRegions = GetDeserializedDictionary(json);
			_regions = RegionsFromDeserializedRegions(deserializedRegions);
		}

		public (Color color, Region region) GetRegionFromName(string name)
		{
			foreach (var pair in _regions)
				if (pair.Value.Name == name)
					return (pair.Key, pair.Value);
			return (Color.black, null);
		}

		#region RegionsSetup

		private static Dictionary<Color32, SerializableRegion> GetDeserializedDictionary(string json)
		{
			//Get deserialized json dictionary
			var stringRegions = JsonConvert.DeserializeObject<Dictionary<string, SerializableRegion>>(json);
			//get json dictionary with colors instead of string
			var serializedRegions = new Dictionary<Color32, SerializableRegion>();
			foreach (var pair in stringRegions!)
				serializedRegions[GetColorFromString(pair.Key)] = pair.Value;
			return serializedRegions;
		}

		private static Color GetColorFromString(string colorString)
		{
			var strings = colorString.Split(',');
			byte.TryParse(strings[0], out var r);
			byte.TryParse(strings[1], out var g);
			byte.TryParse(strings[2], out var b);
			return new Color32(r, g, b, 255);
		}

		private static Dictionary<Color32, Region> RegionsFromDeserializedRegions(Dictionary<Color32, SerializableRegion> serializedRegions)
		{
			var regionsDictionary = new Dictionary<Color32, Region>();
			var regions = serializedRegions.Select(pair => new Region(pair.Value.name)).ToArray();
			foreach (var pair in serializedRegions)
			{
				var neighbours = new List<Region>();
				foreach (var neighbour in pair.Value.neighbours)
					neighbours.AddRange(regions.Where(region => region.Name == neighbour));
				var foundRegion = regions.FirstOrDefault(r => r.Name == pair.Value.name);
				foundRegion!.Neighbours = neighbours;
				regionsDictionary[pair.Key] = foundRegion;
			}
			return regionsDictionary;
		}

		#endregion
	}
}