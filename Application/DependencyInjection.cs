namespace Application
{
    /// <summary>
    /// Configures dependency injection for the Application layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds services and configurations for the Application layer.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <returns>The configured service collection.</returns>
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            //سservices.AddAutoMapper(Assembly.GetExecutingAssembly());
            // Add FluentValidation if needed
            // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}