using InfiniteCanvas.HealthDamageSystem.Damage;
using InfiniteCanvas.HealthDamageSystem.Modifications;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace InfiniteCanvas.HealthDamageSystem
{
    public class HealthDamageSystemLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe(options =>
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

            builder.RegisterMessageBroker<DamageRequest>(options);
            builder.RegisterMessageBroker<AddModificationToTarget>(options);
            builder.RegisterMessageBroker<RemoveModificationFromTarget>(options);

            builder.RegisterEntryPoint<HealthDB>().AsSelf();
            builder.RegisterEntryPoint<DamageTypeDB>().AsSelf();
            builder.RegisterEntryPoint<ModificationDB>().AsSelf();
            builder.RegisterEntryPoint<ModificationSystem>().AsSelf();
            builder.RegisterEntryPoint<DamageCalculationSystem>().AsSelf();
        }
    }
}