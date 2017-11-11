using Server.Core.Container;
using System;
using System.Collections.Generic;

namespace Server.Core.Domain
{
    /// <summary>
    /// http://www.udidahan.com/2009/06/14/domain-events-salvation/
    /// </summary>
    public static class DomainEvents
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static List<Delegate> actions;

         public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
                actions = new List<Delegate>();

            actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            actions = null;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if(ServiceLocator.IsContainerSet)
                foreach (var handler in ServiceLocator.ResolveAll<Handles<T>>())
                    handler.Handle(args);
     
            if (actions != null)
                foreach (var action in actions)
                    if (action is Action<T>)
                        ((Action<T>)action)(args);
        }
    } 
}
