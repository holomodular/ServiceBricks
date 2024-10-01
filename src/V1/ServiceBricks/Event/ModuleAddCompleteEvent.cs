using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBricks
{
    /// <summary>
    /// This event fires when the module is AddComplete
    /// </summary>
    /// <typeparam name="IModule"></typeparam>
    public abstract partial class ModuleAddCompleteEvent : DomainEvent<IModule>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleAddCompleteEvent() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public ModuleAddCompleteEvent(
            IModule module,
            IServiceCollection services,
            IConfiguration configuration) : base()
        {
            DomainObject = module;
            ServiceCollection = services;
            Configuration = configuration;
        }

        /// <summary>
        /// The service collection.
        /// </summary>
        public virtual IServiceCollection ServiceCollection { get; set; }

        /// <summary>
        /// The configuration
        /// </summary>
        public virtual IConfiguration Configuration { get; set; }
    }

    /// <summary>
    /// This event fires when the module is AddCompleteed
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public partial class ModuleAddCompleteEvent<TModule> : ModuleAddCompleteEvent
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleAddCompleteEvent() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public ModuleAddCompleteEvent(
            IModule module,
            IServiceCollection services,
            IConfiguration configuration) : base(module, services, configuration)
        {
        }
    }
}