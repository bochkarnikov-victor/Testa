using R3;
using UnityEngine.UIElements;

namespace Utilities
{
    public static class UIToolkitRxExtensions
    {
        public static Observable<Unit> OnClickAsObservable(this Button button)
        {
            return Observable.FromEvent(h => button.clicked += h, h => button.clicked -= h);
        }
    }
}