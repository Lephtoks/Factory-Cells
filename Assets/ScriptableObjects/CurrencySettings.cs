using UnityEngine;
using Economics;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Economics/Currency Settings")]
	public class CurrencySettings : ScriptableObject {
		public Currency currency;
		public Texture2D icon;
		public string displayName;
		public Color color;
	}
}

