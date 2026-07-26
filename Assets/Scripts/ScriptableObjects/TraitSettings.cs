using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Economics/Trait Settings")]
	public class TraitSettings : ScriptableObject {
		public Sprite icon;
		public string displayName;
	}
}

