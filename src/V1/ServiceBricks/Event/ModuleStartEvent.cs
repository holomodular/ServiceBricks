using Microsoft.AspNetCore.Builder;

namespace ServiceBricks
{
    /// <summary>
    /// This event fires when the module is Started
    /// </summary>
    /// <typeparam name="IModule"></typeparam>
    public abstract partial class ModuleStartEvent : DomainEvent<IModule>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleStartEvent() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="applicationBuilder"></param>
        public ModuleStartEvent(
            IModule module,
            IApplicationBuilder applicationBuilder) : base()
        {
            DomainObject = module;
            ApplicationBuilder = applicationBuilder;
        }

        /// <summary>
        /// The application builder
        /// </summary>
        public virtual IApplicationBuilder ApplicationBuilder { get; set; }
    }

    /// <summary>
    /// This event fires when the module is Started
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public partial class ModuleStartEvent<TModule> : ModuleStartEvent
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ModuleStartEvent() : base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="applicationBuilder"></param>
        public ModuleStartEvent(
            IModule module,
            IApplicationBuilder applicationBuilder) : base(module, applicationBuilder)
        {
        }
    }
}