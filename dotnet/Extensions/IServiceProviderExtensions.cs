namespace SPIN.Core.Extensions;

public static class IServiceProviderExtensions
{
    public static IEnumerable<T> GetServices<T>(this IServiceCollection serviceCollection)
    {
        bool noServicesRegistered = !serviceCollection.Any();
        if (noServicesRegistered)
        {
            return Array.Empty<T>();
        }

        using IEnumerator<ServiceDescriptor> serviceDescriptors = serviceCollection.GetEnumerator();
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        var services = new List<T>();
        do
        {
            ServiceDescriptor serviceDescriptor = serviceDescriptors.Current;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            bool serviceDescriptorIsNull = (serviceDescriptor is null);
            if (serviceDescriptorIsNull)
            {
                continue;
            }

            bool isNotServiceLooking = (serviceDescriptor!.ServiceType != typeof(T));
            if (isNotServiceLooking)
            {
                continue;
            }

            Func<IServiceProvider, object>? implementationFactory = serviceDescriptor.ImplementationFactory;
            bool implementationFactoryIsNull = (implementationFactory is null);

            if (implementationFactoryIsNull)
            {
                continue;
            }

#pragma warning disable CS8625
            var service = (T)implementationFactory!.Invoke(default);
#pragma warning restore CS8625
            bool serviceIsNull = (service is null);

            if (serviceIsNull)
            {
                continue;
            }

            services.Add(service);
        } while (serviceDescriptors.MoveNext());

        return services;
    }
}
