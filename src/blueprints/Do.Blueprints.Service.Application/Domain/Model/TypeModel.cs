using Do.Domain.Configuration;

namespace Do.Domain.Model;

public class TypeModel
    : IMemberModel, IModel, IEquatable<TypeModel>
{
    internal static string IdFrom(Type type) =>
        type.FullName ?? $"{type.Namespace}.{type.Name}<{string.Join(',', type.GenericTypeArguments.Select(IdFrom))}>";

    readonly Type _type;
    readonly string _id;
    readonly Lazy<string> _cSharpFriendlyFullName;
    BuildLevel _buildLevel;

    internal TypeModel(Type type) : this(type, IdFrom(type)) { }
    public TypeModel(Type type, string id)
    {
        _type = type;
        _id = id;
        _cSharpFriendlyFullName = new(BuildCSharpFriendlyFullName);

        Name = type.Name;
        FullName = type.FullName;
        Namespace = type.Namespace;
        IsPublic = type.IsPublic;
        IsAbstract = type.IsAbstract;
        IsValueType = type.IsValueType;
        IsSealed = type.IsSealed;
        IsInterface = type.IsInterface;
        IsGenericType = type.IsGenericType;
        IsGenericTypeDefinition = type.IsGenericTypeDefinition;
        IsGenericTypeParameter = type.IsGenericTypeParameter;
        IsGenericMethodParameter = type.IsGenericMethodParameter;
        ContainsGenericParameters = type.ContainsGenericParameters;

        _buildLevel = BuildLevel.Basics;
    }

    #region Basics

    public string Name { get; }
    public string? FullName { get; }
    public string? Namespace { get; }
    public bool IsPublic { get; }
    public bool IsAbstract { get; }
    public bool IsValueType { get; }
    public bool IsSealed { get; }
    public bool IsInterface { get; }
    public bool IsGenericType { get; }
    public bool IsGenericTypeDefinition { get; }
    public bool IsGenericTypeParameter { get; }
    public bool IsGenericMethodParameter { get; }
    public bool ContainsGenericParameters { get; }

    public string CSharpFriendlyFullName => _cSharpFriendlyFullName.Value;

    string BuildCSharpFriendlyFullName()
    {
        if (!IsGenericType) { return FullName ?? Name; }

        return $"{Namespace}.{Name[..Name.IndexOf("`")]}<{string.Join(", ", GenericTypeArguments.Select(t => t.CSharpFriendlyFullName))}>";
    }

    #endregion

    #region Generics

    TypeModel? _genericTypeDefinition;
    ModelCollection<TypeModel>? _genericTypeArguments;

    public TypeModel? GenericTypeDefinition => SafeGet(_genericTypeDefinition, BuildLevel.Generics);
    public ModelCollection<TypeModel> GenericTypeArguments => SafeGetRequired(_genericTypeArguments, BuildLevel.Generics);

    internal void SetGenerics(TypeModel? genericTypeDefinition, ModelCollection<TypeModel> genericTypeArguments)
    {
        _genericTypeDefinition = genericTypeDefinition;
        _genericTypeArguments = genericTypeArguments;

        _buildLevel = BuildLevel.Generics;
    }

    #endregion

    #region Inheritance

    TypeModel? _baseType;
    ModelCollection<TypeModel>? _interfaces;

    public TypeModel? BaseType => SafeGet(_baseType, BuildLevel.Inheritance);
    public ModelCollection<TypeModel> Interfaces => SafeGetRequired(_interfaces, BuildLevel.Inheritance);

    internal void SetInheritance(TypeModel? baseType, ModelCollection<TypeModel> interfaces)
    {
        _baseType = baseType;
        _interfaces = interfaces;

        _buildLevel = BuildLevel.Inheritance;
    }

    public bool IsAssignableTo<T>() =>
        IsAssignableTo(typeof(T));

    public bool IsAssignableTo(Type type) =>
        _type == type ||
        (
            IsBuilt(BuildLevel.Generics) && GenericTypeDefinition?.IsAssignableTo(type) == true
        ) ||
        (
            IsBuilt(BuildLevel.Inheritance) &&
            (
                BaseType?.IsAssignableTo(type) == true ||
                Interfaces?.Contains(IdFrom(type)) == true
            )
        );

    #endregion

    #region Metadata

    AttributeCollection? _customAttributes;

    public AttributeCollection CustomAttributes => SafeGetRequired(_customAttributes, BuildLevel.Metadata);

    internal void SetMetadata(AttributeCollection customAttributes)
    {
        _customAttributes = customAttributes;

        _buildLevel = BuildLevel.Metadata;
    }

    public bool Has<T>() where T : Attribute =>
        CustomAttributes.ContainsKey<T>();

    #endregion

    #region Members

    ConstructorGroupModel? _constructorGroup;
    List<ConstructorModel>? _constructors;
    ModelCollection<PropertyModel>? _properties;
    ModelCollection<MethodGroupModel>? _methodGroups;
    List<MethodModel>? _methods;

    internal ConstructorGroupModel? ConstructorGroup => SafeGet(_constructorGroup, BuildLevel.Members);
    public List<ConstructorModel> Constructors => SafeGetRequired(_constructors, BuildLevel.Members);
    public ModelCollection<PropertyModel> Properties => SafeGetRequired(_properties, BuildLevel.Members);
    internal ModelCollection<MethodGroupModel> MethodGroups => SafeGetRequired(_methodGroups, BuildLevel.Members);
    public List<MethodModel> Methods => SafeGetRequired(_methods, BuildLevel.Members);

    internal void SetMembers(ConstructorGroupModel? constructorGroup, ModelCollection<PropertyModel> properties, ModelCollection<MethodGroupModel> methodGroups)
    {
        _constructorGroup = constructorGroup;
        _constructors = constructorGroup?.Constructors.ToList() ?? [];
        _properties = properties;
        _methodGroups = methodGroups;
        _methods = methodGroups.SelectMany(m => m.Methods).ToList();

        _buildLevel = BuildLevel.Members;
    }

    public ConstructorModel GetConstructor() =>
        Constructors.Single();

    public MethodModel GetMethod(string name) =>
        MethodGroups[name].Methods.Single();

    public IEnumerable<MethodModel> GetMethods(string name) =>
        MethodGroups[name].Methods;

    #endregion

    public bool IsBuilt(BuildLevel buildLevel) =>
        _buildLevel.Covers(buildLevel);

    T SafeGetRequired<T>(T? result, BuildLevel buildLevel) where T : notnull =>
        SafeGet(result, buildLevel) ?? throw new($"'result' should not be null at {_buildLevel} level");

    T SafeGet<T>(T result, BuildLevel buildLevel)
    {
        if (IsBuilt(buildLevel)) { return result; }

        throw new NotSupportedException($"This operation requires {Name} to be built at {buildLevel} level, but it is built at {_buildLevel} level");
    }

    public void Apply(Action<Type> action) =>
        action(_type);

    public override bool Equals(object? obj) =>
        ((IEquatable<TypeModel>)this).Equals(obj as TypeModel);

    bool IEquatable<TypeModel>.Equals(TypeModel? other) =>
        other is not null && other._id == _id;

    public override int GetHashCode() =>
        _id.GetHashCode();

    string IModel.Id => _id;
}
