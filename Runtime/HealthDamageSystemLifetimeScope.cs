using InfiniteCanvas.HealthDamageSystem.Damage;
using InfiniteCanvas.HealthDamageSystem.Messages;
using InfiniteCanvas.HealthDamageSystem.Resistances;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem
{
    public class HealthDamageSystemLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var messagePipeOptions = builder.RegisterMessagePipe(options =>
                                                                 {
                                                                     options.InstanceLifetime = InstanceLifetime.Scoped;
                                                                     options.DefaultAsyncPublishStrategy =
                                                                         AsyncPublishStrategy.Parallel;
                                                                     options.HandlingSubscribeDisposedPolicy =
                                                                         HandlingSubscribeDisposedPolicy.Throw;
                                                                     options.RequestHandlerLifetime =
                                                                         InstanceLifetime.Scoped;
                                                                 });
            builder.RegisterBuildCallback(resolver => GlobalMessagePipe.SetProvider(resolver.AsServiceProvider()));

            builder.RegisterMessageBroker<AddResistanceToTarget>(messagePipeOptions);

            builder.RegisterEntryPoint<ResistanceSystem>(Lifetime.Scoped);
            builder.RegisterEntryPoint<DamageCalculationSystem>(Lifetime.Scoped);
        }
    }
}