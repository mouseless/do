namespace Do.Domain.Configuration;

public record TypeBuildLevelFilter(Func<TypeBuildContext, bool> Filter, BuildLevel BuildLevel);