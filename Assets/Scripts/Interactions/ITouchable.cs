using UnityEngine;

namespace Interactions
{
    public interface ITouchable
    {
        float GetDepth();

        bool CapturesClick();
        
        bool IsSelected(Vector3 mousePos, Vector3 worldPos);
        
        void Select(Vector3 mousePos, Vector3 worldPos, int capturedButton, bool captured);
        virtual void OnHoverEnd(Vector3 mousePos, Vector3 worldPos, int capturedButton) {}
        virtual void OnHoverStart(Vector3 mousePos, Vector3 worldPos, int capturedButton) {}
    }
}