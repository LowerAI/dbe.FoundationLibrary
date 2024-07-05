using System;

namespace dbe.FoundationLibrary.Core.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void RaiseEvent<TEventArgs>(this EventHandler handler, object sender, TEventArgs e) where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
