# Unreleased

## Features

- Beta features are available in do-blueprints-service package;
  - `CodeGenerationLayer` is introduced, now it is possible to generate code
    during initialization of a service

## Improvements

- `DomainModel` generation is improved for generic type support
  - `TypeModel`'s for generic types with arguments are now marked as business
    type and initialized with all collection properties
  - `TypeModel`'s for non business generic types are now initialized with
    generic type arguments
  - `TypeModel`'s for generic `Task<T>` now have `Task` as base class
- `CanReturn()` helper is added for `MethodModel` which loops through all
  overloads and compares return types for given `TypeModel`
    - Generic `Task` return types are also supported
- `MvcNewtonsoftJsonOptions` is added to `RestApiLayer` as configuration target 

## Bugfixes

- `TypeModel`'s which are not business types were throwing null reference
  exception for collection properties, fixed
