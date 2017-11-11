using Castle.Windsor;

namespace Server.Core.Container
{
    public static class ServiceLocator
    {
        private static IWindsorContainer container;

        public static void Set(IWindsorContainer newContainer)
        {
            container = newContainer;
        }

        public static bool IsContainerSet
        {
            get { return container != null; }
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static T[] ResolveAll<T>()
        {
            return container.ResolveAll<T>();
        }

        public static T Resolve<T>(string key)
        {
            return container.Resolve<T>(key);
        }

        public static T Resolve<T>(object argumentsAsAnonymousType)
        {
            return container.Resolve<T>(argumentsAsAnonymousType);
        }

        public static void Release()
        {
            if (container != null)
                container.Dispose();
        }
    }
}