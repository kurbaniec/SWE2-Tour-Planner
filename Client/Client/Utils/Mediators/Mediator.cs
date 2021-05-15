using System;

namespace Client.Utils.Mediators
{
    /// <summary>
    /// Mediator Pattern.
    /// Provides an intuitive way for interactions between ViewModels.
    /// Learn more here https://marlongrech.wordpress.com/2008/03/20/more-than-just-mvc-for-wpf/.
    /// Based on https://www.codeproject.com/Articles/35277/MVVM-Mediator-Pattern by Sacha Barber.
    /// </summary>
    public class Mediator
    {
        private readonly MultiDictionary<ViewModelMessages, Action<object?>> internalList = new();

        /// <summary>
        /// Registers a Colleague to a specific message
        /// </summary>
        /// <param name="callback">The callback to use
        /// when the message it seen</param>
        /// <param name="message">The message to
        /// register to</param>
        public
            void Register(Action<object?> callback,
                ViewModelMessages message)
        {
            internalList.AddValue(message, callback);
        }


        /// <summary>
        /// Notify all colleagues that are registered to the
        /// specific message
        /// </summary>
        /// <param name="message">The message for the notify by</param>
        /// <param name="args">The arguments for the message</param>
        public void NotifyColleagues(ViewModelMessages message,
            object? args)
        {
            if (!internalList.ContainsKey(message)) return;
            //forward the message to all listeners
            foreach (Action<object?> callback in
                internalList[message])
                callback(args);
        }
    }
}