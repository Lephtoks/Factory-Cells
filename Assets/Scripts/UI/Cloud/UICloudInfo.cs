using Economics;
using Global;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cloud
{
    public class UICloudInfo : MonoBehaviour
    {
        [SerializeField] private Image[] icons;

        private int index = 0;
        public void ResetIcons() {
            foreach (var icon in icons) {
                icon.gameObject.SetActive(false);
            }
            index = 0;
        }

        public bool TryAddIcon(ItemStack stack) {
            if (stack.IsEmpty()) return false;
            
            if (index < icons.Length) {
                icons[index].gameObject.SetActive(true);
                icons[index].sprite = GlobalData.Instance.currencySettingsDatabase.Get(stack.CurrencyType).icon;
                index++;
                return true;
            }
            return false;
        }
    }
}
