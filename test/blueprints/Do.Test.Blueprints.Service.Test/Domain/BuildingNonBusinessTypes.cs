using Do.Domain.Model;
using Do.Orm;

namespace Do.Test.Domain;

public class BuildingNonBusinessTypes : TestServiceSpec
{
    [Test]
    public void Non_business_types_are_added_to_type_collection([Values(typeof(string), typeof(int), typeof(Task), typeof(Func<Entity>), typeof(IQueryContext<Entity>))] Type type)
    {
        DomainModel.Types.Contains(type).ShouldBeTrue();
    }

    [Test]
    public void Non_business_types_with_no_generic_parameters_are_initialized_with_empty_collections([Values(typeof(string), typeof(int), typeof(Task))] Type type)
    {
        var model = DomainModel.Types[type];

        model.Properties.ShouldNotBeNull();
        model.Properties.Count().ShouldBe(0);

        model.GenericTypeArguments.ShouldNotBeNull();
        model.GenericTypeArguments.Count().ShouldBe(0);

        model.Methods.ShouldNotBeNull();
        model.Methods.Count().ShouldBe(0);

        model.Interfaces.ShouldNotBeNull();
        model.Interfaces.Count().ShouldBe(0);

        model.CustomAttributes.ShouldNotBeNull();
        model.CustomAttributes.Count().ShouldBe(0);

        model.Constructor.ShouldBeNull();
    }

    [Test]
    public void Non_business_types_with_generic_parameters_are_initialized_with_generic_type_definition()
    {
        var entityType = DomainModel.Types[typeof(Entity)];
        var genericType = DomainModel.Types[typeof(List<Entity>)];

        genericType.ShouldNotBeNull();
        genericType.GenericTypeDefinition.ShouldNotBeNull();
        genericType.GenericTypeDefinition.IsAssignableTo(typeof(List<>)).ShouldBeTrue();
    }

    [Test]
    public void Non_business_types_with_non_business_generic_parameters_are_initialized_with_generic_type_definition()
    {
        var stringType = DomainModel.Types[typeof(string)];
        var genericType = DomainModel.Types[typeof(List<string>)];

        genericType.ShouldNotBeNull();
        genericType.GenericTypeDefinition.ShouldNotBeNull();
        genericType.GenericTypeDefinition.IsAssignableTo(typeof(List<>)).ShouldBeTrue();
        genericType.GenericTypeArguments.First().IsAssignableTo<string>().ShouldBeTrue();
    }

    [Test]
    public void Non_business_types_with_generic_parameters_are_initialized_with_generic_arguments([Values(typeof(List<Entity>), typeof(Func<Entity>), typeof(IQueryContext<Entity>))] Type type)
    {
        var entityType = DomainModel.Types[typeof(Entity)];
        var genericType = DomainModel.Types[type];

        genericType.ShouldNotBeNull();
        genericType.GenericTypeArguments.Count().ShouldBe(1);
        genericType.GenericTypeArguments.First().ShouldBe(entityType);
    }

    [Test]
    public void Base_type_is_added_for_task()
    {
        var model = DomainModel.Types[typeof(Task<TransientWithTask>)];

        model.ShouldNotBeNull();
        model.BaseType.ShouldNotBeNull();
        model.BaseType.IsAssignableTo<Task>();
    }
}
