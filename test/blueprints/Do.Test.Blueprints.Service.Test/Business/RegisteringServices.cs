using Do.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Do.Test.Business;
public class RegisteringServices : TestServiceSpec
{
    [Test]
    public void Types_containing_a_method_named_with_and_returns_self_are_registered_as_transient([Values(typeof(Entity), typeof(Operation), typeof(TransientWithTask))] Type type)
    {
        var actual1 = GiveMe.A(type);
        var actual2 = GiveMe.A(type);

        actual1.ShouldNotBeSameAs(actual2);
    }

    [Test]
    public void Transient_types_with_generic_type_arguments_are_registered([Values(typeof(OperationWithGenericParameter<Entity>))] Type type)
    {
        var actual1 = GiveMe.A(type);
        var actual2 = GiveMe.A(type);

        actual1.ShouldNotBeSameAs(actual2);
    }

    [Test]
    public void Transient_services_have_singleton_factories([Values(typeof(Func<Entity>), typeof(Func<Operation>), typeof(Func<OperationWithGenericParameter<Entity>>))] Type type)
    {
        var actual1 = GiveMe.The(type);
        var actual2 = GiveMe.The(type);

        actual1.ShouldBeSameAs(actual2);
    }

    [Test]
    public void Types_without__with__methods_are_registered_as_singleton([Values(typeof(Singleton), typeof(Entities), typeof(Class))] Type type)
    {
        var actual1 = GiveMe.The(type);
        var actual2 = GiveMe.The(type);

        actual1.ShouldBeSameAs(actual2);
    }

    [Test]
    public void Singleton_types_with_interfaces_are_registered_as_implementations([Values(typeof(IInterface), typeof(ISingleton))] Type type)
    {
        var actual1 = GiveMe.The(type);
        var actual2 = GiveMe.The(type);

        actual1.ShouldBeSameAs(actual2);
        actual1.GetType().ShouldBe(typeof(Singleton));
    }

    [Test]
    public void Exception_handlers_are_registered_as_singleton_and_forwarded_to_IExceptionHandler_interface()
    {
        var exceptionHandlers = GiveMe.The<IEnumerable<Do.ExceptionHandling.IExceptionHandler>>();
        var expected = GiveMe.The<SampleExceptionHandler>();

        var actual = exceptionHandlers.FirstOrDefault(h => h is SampleExceptionHandler);

        actual.ShouldBeSameAs(expected);
    }

    [Test]
    public void Transient_types_with_interfaces_are_registered_as_implementations()
    {
        var actual1 = GiveMe.The<IOperation>();
        var actual2 = GiveMe.The<IOperation>();

        actual1.ShouldNotBeSameAs(actual2);
        actual1.GetType().ShouldBe(typeof(Operation));
    }

    [Test]
    public void Types_that_implements_IScoped_are_registered_as_scoped()
    {
        var actual1 = GiveMe.The<Scoped>();
        var actual2 = GiveMe.The<Scoped>();
        var actual3 = GiveMe.The<IServiceProvider>().CreateScope().ServiceProvider.GetRequiredService<Scoped>();

        actual1.ShouldBeSameAs(actual2);
        actual1.ShouldNotBeSameAs(actual3);
    }

    [Test]
    public void Scoped_services_have_singleton_factories()
    {
        var actual1 = GiveMe.The<Func<Scoped>>();
        var actual2 = GiveMe.The<Func<Scoped>>();

        actual1.ShouldBeSameAs(actual2);
    }

    [Test]
    public void Static_types_are_not_registered()
    {
        var action = () => GiveMe.The(typeof(Static));

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(typeof(Static));
    }

    [Test]
    public void Value_types_are_not_registered([Values(typeof(Struct), typeof(Status))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }

    [Test]
    public void Records_are_not_registered()
    {
        var action = () => GiveMe.The<Record>();

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(typeof(Record));
    }

    [Test]
    public void Abstract_types_are_not_registered()
    {
        var action = () => GiveMe.The<SingletonBase>();

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(typeof(SingletonBase));
    }

    [Test]
    public void Exception_types_are_not_registered([Values(typeof(SampleException), typeof(Exception), typeof(TestServiceHandledException))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }

    [Test]
    public void System_types_are_not_registered([Values(typeof(int), typeof(string), typeof(Guid), typeof(List<>), typeof(Task<>))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }

    [Test]
    public void Attributes_are_not_registered()
    {
        var action = () => GiveMe.The<AuthorizationRequiredAttribute>();

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(typeof(AuthorizationRequiredAttribute));
    }

    [Test]
    public void Referenced_interfaces_are_not_registered([Values(typeof(IEquatable<Entity>), typeof(IScoped))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }

    [Test]
    public void Types_with_generic_type_parameters_are_not_registered([Values(typeof(OperationWithGenericParameter<>))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }

    [Test]
    public void Non_public_types_are_not_registered()
    {
        var nonPublicType = Activator.CreateInstance("Do.Test.Blueprints.Service", "Do.Test.Internal")?.GetType() ??
            throw new("`Do.Test.Internal` should have existed");
        var action = () => GiveMe.The(nonPublicType);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(nonPublicType);
    }

    [Test]
    public void Delegate_types_are_not_registered([Values(typeof(TaskDelegate))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }

    [Test]
    public void Types_having_no_with_method_but_public_properties_are_not_registered([Values(typeof(ClassDTO))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }

    [Test]
    public void Interfaces_are_only_registered_through_their_implemented_classes([Values(typeof(IInterfaceWithNoImplementation))] Type type)
    {
        var action = () => GiveMe.The(type);

        action.ShouldThrowExceptionWithServiceNotRegisteredMessage(type);
    }
}
